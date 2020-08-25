using System;

namespace RssCrawler.Messages
{
	public class CrawlFeedMessage
	{
		public CrawlFeedMessage()
		{
		}

		public string URI { get; set; }
		public Guid ID { get; internal set; }
	}
}