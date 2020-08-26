using System;
using Akka.Actor;
using Akka.Routing;
using RssCrawler.Messages;

namespace RssCrawler.Actors
{
	public class FeedSupervisor : ReceiveActor
	{
		private int _uriCount = 0;
		private int _doneCount = 0;

		public FeedSupervisor()
		{
			Receive<StartFeedCrawlingMessage>(_ =>
			{
				var crawlActor = Context.ActorOf(Props.Create<FeedCrawlerActor>().WithRouter(new RoundRobinPool(5)));
				_uriCount = _.URIs.Length;

				foreach (var uri in _.URIs)
				{
					crawlActor.Tell(new CrawlFeedMessage { URI = uri, ID = Guid.NewGuid() });
				}
			});
			Receive<CrawlSuccededMessage>(_ =>
			{
				_doneCount++;

				if (_doneCount >= _uriCount)
				{
					Console.WriteLine("all crawls done");
					var tocActorRef = Context.ActorOf<TableOfContentsActor>();
					tocActorRef.Tell(new RenderTableOfContentsMessage());
					
				}
			});
			Receive<RenderTOCSuccededMessage>(_ =>
			{
				Context.ActorOf<EpubActor>().Tell(new SaveEpubMessage());
			});
		}
	}
}