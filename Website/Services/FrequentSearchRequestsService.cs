using FuzzySharp;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Website.Services
{
	public class FrequenciedService
	{
		public int Frequency { get; private set; }
		public DocumentSearchService DocumentSearchServiceScope { get; set; }
		public FrequenciedService(int Frequency, DocumentSearchService DocumentSearchServiceScope)
		{
			this.Frequency = Frequency;
			this.DocumentSearchServiceScope = DocumentSearchServiceScope;
		}
		public void IncFreq(int count = 1) => this.Frequency = this.Frequency + count;
		public void DecFreq(int count = 1) => this.Frequency = this.Frequency - count;
	}

	public class FrequentSearchRequestsService : BackgroundService
	{
		/* Длинные названия для полей необходимы чтобы человек мог понять смысл
		 * поля без просмотра исходного кода класса, однако в скрытых методах
		 * можно прибегнуть к сокращениям.
		 */

		// Хранит последние, еще не обработанные запросы
		private readonly List<DocumentSearchService> RecentRequests_DocumentSearchServiceScopes;

		// to be private in release
		// Хранит наиболее частые запросы
		public List<FrequenciedService> FrequentRequests;

		// Интервал обработки последних запросов, даже если не набрано нужное количество
		public int Interval_msec = 1000 * 60 * 5; // 5 min interval

		// Количественный интервал обработки последних запросов
		public int Interval_numOfRecentRequests = 50; // every 50 requests interval

		// Интервал проверки необходимости провести обработку
		public int Interval_updateNeededCheck_msec = 30 * 1000; // 5 sec interval

		// Количество хранимых частых запросов
		public int NumOfFrequentRequestsStored = 50;

		// Очки сходства по методу Fuzz.TokenSortRation для засчитывания строк как одинаковых по токену
		public int TokenSortRationNeededToCountAsSimilar = 90;

		// Время жизни сохранненого запроса. Если частый запрос существует более 5 минут, его следует обновить.
		public int ResponseLifetime_msec = 1000 * 60 * 5 / 5 / 60; // 5 min interval

		// Время последнего обновления
		private System.DateTime LastUpdateTime;

		private readonly IServiceScopeFactory ScopeFactory;
		public FrequentSearchRequestsService(IServiceScopeFactory ScopeFactory)
		{
			this.ScopeFactory = ScopeFactory;
			this.RecentRequests_DocumentSearchServiceScopes = new List<DocumentSearchService>();
			this.FrequentRequests = new List<FrequenciedService>();
		}

		/// <summary>
		/// Tries to find frequent request which is similar to passed.
		/// If found, returns it, otherwise returns null.
		/// </summary>
		public DocumentSearchService? GetSimilarRequestOrNull(string UserRequest)
		{
			// Пытается найти подходящий ответ на переданный запрос среди частых запросов
			try
			{
				var tryFind = this.FrequentRequests
						.First(x => Fuzz.TokenSortRatio(UserRequest, x.DocumentSearchServiceScope.Request)
						> this.TokenSortRationNeededToCountAsSimilar);
				tryFind.IncFreq();

				// Если ответ устарел - обновить сейчас
				if ((tryFind.DocumentSearchServiceScope.ProceedDateTime - DateTime.Now)
					.Seconds > this.ResponseLifetime_msec)
				{
					tryFind.DocumentSearchServiceScope = this.ScopeFactory.CreateScope().ServiceProvider.
						GetRequiredService<DocumentSearchService>();
					tryFind.DocumentSearchServiceScope.ProceedRequest(UserRequest);
				}

				return tryFind.DocumentSearchServiceScope;
			}
			// Ничего не найдено (следует обработать запрос через БД)
			catch (System.InvalidOperationException)
			{
				return null;
			}
		}

		public List<DocumentSearchService> GetFrequentSearchesScopes() => this.FrequentRequests.Select(x => x.DocumentSearchServiceScope).ToList();

		public void AddDocumentSearchServiceScope(DocumentSearchService SearchScope)
			=> this.RecentRequests_DocumentSearchServiceScopes.Add(SearchScope);

		/* Наиболее схожая строка среди запросов переданных сервисов
		 * Это фактически статический метод, ибо если получать SearchService через
		 * this, то вызов на мой взгляд выглядит непонятно/неоднозначно с чем он работает.
		 */
		private int IndexOfMostCommonByTokenSortRatio(IList<FrequenciedService> SearchServices, string SearchRequest, out int SimilarityScore)
		{
			SimilarityScore = 0;
			int IndexOfMostCommon = 0;

			for (int i = 0; i < SearchServices.Count; i++)
			{
				var score = Fuzz.TokenSortRatio(SearchServices[i].DocumentSearchServiceScope.Request, SearchRequest);
				if (score > SimilarityScore)
				{
					SimilarityScore = score;
					IndexOfMostCommon = i;
				}
			}

			return IndexOfMostCommon;
		}
		public void Update()
		{
			// Установка последнего времени обновления
			this.LastUpdateTime = System.DateTime.UtcNow;

			/* Теоретически, блокировка доступа не нужна, ибо во всех методах кроме этого используется
			 * перебор через First, который использует перебор через цикл foreach, а не for, за исключением метода IndexOfMostCommonByTokenSortRatio, но он вызывается единственный раз
			 * собственно в этом методе.
			 */
			// lock (this.RecentRequests_DocumentSearchServiceScopes)
			//	lock (this.FrequentRequests)
			{
				// Присвоение каждому недавнему запросы частоты 0
				var recent = this.RecentRequests_DocumentSearchServiceScopes
					.Select(SearchServiceScope => new FrequenciedService(0, SearchServiceScope)).ToList();

				// Вычисление действительной частоты для каждого недавнего запроса и удаление походих
				for (int i = 0; i < recent.Count - 1; i++)
				{
					for (int r = i + 1; r < recent.Count; r++)
					{
						/* Если найдены две похожие строки, то одной добавить частоты, а вторую удалить.
						 * В идеале данный алгоритм должен быть улучшен, и сначала из двух похожих
						 * строк выводить наиболее популярную, далее оставлять её, а не просто первую
						 * из двух, однако сайт не про поиск, поэтому это может быть реализовано только в будущем.
						 */
						var DebugVar1 = Fuzz.TokenSortRatio(recent[i].DocumentSearchServiceScope.Request, recent[r].DocumentSearchServiceScope.Request);
						if (Fuzz.TokenSortRatio(recent[i].DocumentSearchServiceScope.Request, recent[r].DocumentSearchServiceScope.Request)
							> this.TokenSortRationNeededToCountAsSimilar)
						{
							recent[i].IncFreq(recent[r].Frequency + 1);
							recent.RemoveAt(r--);
						}
					}
				}


				// Учет обработанных недавних запросов в общей статистике частых запросов
				for (int i = 0; i < recent.Count; i++)
				{
					/* Если похожая строка существует, то инкрементировать её
					 */
					int score;
					int index = this.IndexOfMostCommonByTokenSortRatio(this.FrequentRequests, recent[i].DocumentSearchServiceScope.Request, out score);
					if (score > this.TokenSortRationNeededToCountAsSimilar)
					{
						this.FrequentRequests[index].IncFreq(recent[i].Frequency + 1);
						recent.RemoveAt(i--);
						continue;
					}
				}
				// Если строка не совпадает ни с одним из уже существующих частых - добавить её
				this.FrequentRequests.AddRange(recent);

				// Сортировка запросов по убыванию популярности
				this.FrequentRequests = this.FrequentRequests.OrderByDescending(x => x.Frequency).Take(this.NumOfFrequentRequestsStored).ToList();

				// Снижение частоты каждого запроса с целью вытеснения устаревших частых запросов
				for (int i = 0; i < this.FrequentRequests.Count; i++)
				{
					this.FrequentRequests[i].DecFreq();
				}

				// Опустошение коллекции недавних запросов
				this.RecentRequests_DocumentSearchServiceScopes.Clear();
			}
		}

		/// <summary>
		/// Checking if proceeding recent requests is needed
		/// </summary>
		protected async override Task ExecuteAsync(CancellationToken ct)
		{
			// Пока сервису не приказано остановиться
			while (!ct.IsCancellationRequested)
			{
				// Обновить если накоплено достаточное число недавних запросов
				if (this.RecentRequests_DocumentSearchServiceScopes.Count() > this.Interval_numOfRecentRequests)
					this.Update();
				// Обновить если прошел интервал обновления и получен хотябы один новый запрос для обработки
				if (((DateTime.UtcNow - this.LastUpdateTime).TotalSeconds > (this.Interval_msec / 1000))
					&& this.RecentRequests_DocumentSearchServiceScopes.Count > 0)
					this.Update();

				// Интервал проверки необходимости обновления
				await Task.Delay(this.Interval_updateNeededCheck_msec);
			}
		}
	}
}
