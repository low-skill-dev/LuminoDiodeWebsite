using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Website.Repository;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using System;
using FuzzySharp;

namespace Website.Services
{
#if DEBUG
	public
#endif
	class FrequenciedService
	{
		public int Frequency { get; private set; }
		public DocumentSearchService DocumentSearchServiceScope { get; init; }
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
		 * можно прибегнуть к сокращениям с комментарием.
		 */
		private List<DocumentSearchService> RecentRequests_DocumentSearchServiceScopes;
#if DEBUG
		public
#endif
#if RELEASE
		private
#endif
		List<FrequenciedService> FrequentRequests;
		public int Interval_msec = 1000 * 60 * 5 / 5 / 60; // 5 min interval
		public int Interval_numOfRecentRequests = 50; // every 50 requests interval
		public int Interval_updateNeededCheck_msec = 30 * 1000; // 5 sec interval
		public int NumOfFrequentRequestsStored = 50;
		public int TokenSetRationNeededToCountAsSimilar = 90;
		private System.DateTime LastUpdateTime;

		public FrequentSearchRequestsService()
		{
			this.RecentRequests_DocumentSearchServiceScopes = new List<DocumentSearchService>();
			this.FrequentRequests = new List<FrequenciedService>();
		}


		public List<DocumentSearchService> GetFrequentSearchesScopes() => this.FrequentRequests.Select(x => x.DocumentSearchServiceScope).ToList();

		public void AddDocumentSearchServiceScope(DocumentSearchService SearchScope)
			=> this.RecentRequests_DocumentSearchServiceScopes.Add(SearchScope);


		private int IndexOfMostCommonByTokenSetRatio(IList<FrequenciedService> SearchServices, string SearchRequest, out int CommonScore)
		{
			CommonScore = 0;
			int IndexOfMostCommon = 0;

			for (int i = 0; i < SearchServices.Count; i++)
			{
				var score = Fuzz.TokenSetRatio(SearchServices[i].DocumentSearchServiceScope.Request, SearchRequest);
				if (score > CommonScore)
				{
					CommonScore = score;
					IndexOfMostCommon = i;
				}
			}

			return IndexOfMostCommon;
		}
		public void Update()
		{
			this.LastUpdateTime = System.DateTime.UtcNow;

			lock (this.RecentRequests_DocumentSearchServiceScopes)
				lock (this.FrequentRequests)
				{
					int Frequency = 0;
					var RecentRequests_FreqAndService = this.RecentRequests_DocumentSearchServiceScopes
						.Select(SearchServiceScope => new FrequenciedService(Frequency, SearchServiceScope)).ToList();

					var recent = RecentRequests_FreqAndService; //short name
					for (int i = 0; i < recent.Count - 1; i++)
					{
						for (int r = i + 1; r < recent.Count; r++)
						{
							/* Если найдены две похожие строки, то одной добавить частоты, а вторую удалить.
							 * В идеале данный алгоритм должен быть улучшен, и сначала из двух похожих
							 * строк выводить наиболее популярную, далее оставлять её, а не просто первую
							 * из двух, однако сайт не про поиск, поэтому это может быть реализовано только в будущем.
							 */
							var DebugVar1 = Fuzz.TokenSetRatio(recent[i].DocumentSearchServiceScope.Request, recent[r].DocumentSearchServiceScope.Request);
							if (Fuzz.TokenSetRatio(recent[i].DocumentSearchServiceScope.Request, recent[r].DocumentSearchServiceScope.Request)
								> this.TokenSetRationNeededToCountAsSimilar)
							{
								recent[i].IncFreq(recent[r].Frequency + 1);
								recent.RemoveAt(r--);
							}
						}
					}
					for (int i = 0; i < recent.Count; i++)
					{
						/* Если похожая строка существует, то инкрементировать её
						 */
						int score;
						int index = IndexOfMostCommonByTokenSetRatio(this.FrequentRequests, recent[i].DocumentSearchServiceScope.Request, out score);
						if (score > this.TokenSetRationNeededToCountAsSimilar)
						{
							this.FrequentRequests[index].IncFreq(recent[i].Frequency + 1);
							recent.RemoveAt(i--);
							continue;
						}
					}
					this.FrequentRequests.AddRange(recent);

					this.FrequentRequests = this.FrequentRequests.OrderByDescending(x => x.Frequency).Take(NumOfFrequentRequestsStored).ToList();
					for (int i = 0; i < this.FrequentRequests.Count; i++)
					{
						this.FrequentRequests[i].DecFreq();
					}


					this.RecentRequests_DocumentSearchServiceScopes = new List<DocumentSearchService>();
				}
		}

		/// <summary>
		/// Checking if proceeding recent requests is needed
		/// </summary>
		protected async override Task ExecuteAsync(CancellationToken ct)
		{
			while (!ct.IsCancellationRequested)
			{
				if (this.RecentRequests_DocumentSearchServiceScopes.Count() > this.Interval_numOfRecentRequests)
					this.Update();
				if (((DateTime.UtcNow - this.LastUpdateTime).TotalSeconds > (this.Interval_msec / 1000))
					&& this.RecentRequests_DocumentSearchServiceScopes.Count > 2)
					this.Update();

				await Task.Delay(this.Interval_updateNeededCheck_msec);
			}
		}
	}
}
