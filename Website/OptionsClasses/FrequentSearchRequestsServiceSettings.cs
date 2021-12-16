namespace Website.OptionsClasses
{
	public class FrequentSearchRequestsServiceOptions
	{
		public const string Position = "FrequentSearchRequestsServiceSettings";

		public int Interval_msec { get; set; }
		public int Interval_numOfRecentRequests { get; set; }
		public int Interval_updateNeededCheck_msec { get; set; }
		public int NumOfFrequentRequestsStored { get; set; }
		public int TokenSortRationNeededToCountAsSimilar { get; set; }
		public int ResponseLifetime_msec { get; set; }
	}
}
