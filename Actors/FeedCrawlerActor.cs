using System;
using Akka.Actor;
using RssCrawler.Messages;

namespace RssCrawler.Actors
{
	public class FeedCrawlerActor : ReceiveActor
	{
		public FeedCrawlerActor()
		{
			Receive<CrawlFeedMessage>(_ =>
			{
				Console.WriteLine("recieved : " + _.URI);
				return true;
			});
		}
	}
}