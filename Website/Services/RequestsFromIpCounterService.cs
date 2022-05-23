using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Hosting;
using System.Collections.Generic;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Website.Services.SettingsProviders;

namespace Website.Services
{
	/// <summary>
	/// Сервис должен считать количество запросов за последнее время с каждого IP адреса.
	/// Если число запросов превышает лимит - контроллер будет возвращать http 429 - Too many requests.
	/// Идея состоит в хранение словаря IP-Int, где IP - адрес, с которого выполнялись запросы,
	/// int - количество запросов за последние 5 минут. Каждые же 60 секунд сервис вычитает количетсов допустимых запросов.
	/// Если число запросов превышено - выдается бан.
	/// 
	/// Обновление: бан не выдается, как только число запросов вернется в рамки за период времени можноо будет отправить запрос снова
	/// </summary>
	public class RequestsFromIpCounterService : BackgroundService
	{
		private Dictionary<long, int> RequestsByIpLastTime = new();
		private readonly RequestsFromIpCounterServiceSettingsProvider SettingsProvider;

		private int MaxRequestsPerPeriod
			=> this.SettingsProvider.AllowedNumOfRequestsPerPeriod;
		private int Period_secs
			=> this.SettingsProvider.ControlledPeriod_secs;
		private int UnbanInterval_secs
			=> this.SettingsProvider.UnbanInterval_secs;

		public RequestsFromIpCounterService(RequestsFromIpCounterServiceSettingsProvider SettingsProvider)
		{
			this.SettingsProvider = SettingsProvider;
		}

		protected override async Task ExecuteAsync(CancellationToken stoppingToken)
		{
			while (true)
			{
				await Task.Delay(UnbanInterval_secs * 1000);
				/* Переменная содержит то, насколько будет умньшено число запросов на каждой итерации сервиса. 
				 * Вычисляется как (допустимое число запросов за период) * (период обновления/длинна периода).
				 * Таким образом, максимальное число запросов за период будет сниматься за один, собственно, период.
				 */
				int DecreaseInDelay = (int)System.Math.Ceiling((float)this.MaxRequestsPerPeriod * ((float)this.UnbanInterval_secs / (float)this.Period_secs));
				foreach (var k in this.RequestsByIpLastTime.Keys)
				{
					this.RequestsByIpLastTime[k] = this.RequestsByIpLastTime[k] - DecreaseInDelay;
					if (this.RequestsByIpLastTime[k] < 0) this.RequestsByIpLastTime[k] = 0;
				}
			}
		}
		public void CountRequest(IPAddress RequesterIp)
		{
			if (!this.RequestsByIpLastTime.ContainsKey(RequesterIp.Address))
				this.RequestsByIpLastTime.Add(RequesterIp.Address, 1);
			else
				this.RequestsByIpLastTime[RequesterIp.Address]++;
		}
		public void CountRequest(ActionExecutingContext context)
		{
			var ip = context.HttpContext.Connection.RemoteIpAddress;
			if (ip is not null)
			{
				this.CountRequest(ip);
			}
		}
		public bool IPAddressIsBanned(IPAddress RequesterIp)
		{
			if (this.RequestsByIpLastTime.ContainsKey(RequesterIp.Address))
				if (this.RequestsByIpLastTime[RequesterIp.Address] > this.MaxRequestsPerPeriod) return true;

			return false;
		}
		public bool IPAddressIsBanned(ActionExecutingContext context)
		{
			if (context.HttpContext.Connection.RemoteIpAddress is not null)
				return this.IPAddressIsBanned(context.HttpContext.Connection.RemoteIpAddress);

			return false;
		}
	}
}
