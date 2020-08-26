using EpubSharp.Format;

namespace RssCrawler.Messages
{

	public class StartFeedCrawlingMessage
	{
		public string[] URIs { get; set; }
	}
}