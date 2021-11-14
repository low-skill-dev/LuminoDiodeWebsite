using LuminoDiodeRandomDataGenerators;

namespace Website.Models.DocumentModel
{
	public class WebText
	{
		public string Text;
		public string? Link;
		public bool? IsBold;
		public bool? IsItalic;
#if DEBUG
		public static WebText GenerateRandom()
		{
			return new WebText() { Text = RandomDataGenerator.String(), Link = RandomDataGenerator.String() };
		}
#endif
	}
}
