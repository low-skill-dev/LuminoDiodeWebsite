using System.Collections.Generic;
using System.Collections;
using System.Linq;

namespace Website.Models.ViewModels
{
	public class Alert
	{
		public enum ALERT_TYPE
		{
			Primary,
			Secondary,
			Success,
			Danger,
			Warning,
			Info,
			Light,
			Dark
		}
		public enum ALERT_COLOR
		{
			Blue = ALERT_TYPE.Primary,
			Gray = ALERT_TYPE.Secondary,
			Green = ALERT_TYPE.Success,
			Red = ALERT_TYPE.Danger,
			Yellow = ALERT_TYPE.Warning,
			Aquamarine = ALERT_TYPE.Info,
			LightGray = ALERT_TYPE.Light,
			DarkGray = ALERT_TYPE.Dark
		}

		public string GetHtmlClass()
		{
			if (this.AlertType == ALERT_TYPE.Primary)
				return "alert alert-primary";
			if (this.AlertType == ALERT_TYPE.Secondary)
				return "alert alert-secondary";
			if (this.AlertType == ALERT_TYPE.Success)
				return "alert alert-success";
			if (this.AlertType == ALERT_TYPE.Danger)
				return "alert alert-danger";
			if (this.AlertType == ALERT_TYPE.Warning)
				return "alert alert-warning";
			if (this.AlertType == ALERT_TYPE.Info)
				return "alert alert-info";
			if (this.AlertType == ALERT_TYPE.Light)
				return "alert alert-light";
			if (this.AlertType == ALERT_TYPE.Dark)
				return "alert alert-dark";

			return "alert alert-primary";
		}

		public ALERT_TYPE AlertType { get;}
		public string Message { get;}

		public Alert(string Message, ALERT_TYPE AlertType)
		{
			this.Message = Message.Replace('\n', ' ');
			this.AlertType = AlertType;
		}
		public Alert(string Message, ALERT_COLOR AlertColor)
		{
			this.Message = Message.Replace('\n', ' ');
			this.AlertType = (ALERT_TYPE)AlertColor;
		}

		public string ToHtmlString()
		{
			return "<div class='p-1'>"
			+ $"<div class='{this.GetHtmlClass()}' role='alert'>"
			+ this.Message
			+ @"</div></div>";
		}
		public string ToHtmlStringMany(IEnumerable<Alert> alerts)
		{
			return string.Join('\n', alerts.Select(x => x.ToHtmlString()));
		}
	}
}
