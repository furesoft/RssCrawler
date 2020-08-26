using System;
using System.Collections.Generic;
using Akka.Actor;
using CodeHollow.FeedReader;
using RssCrawler.Messages;

namespace RssCrawler.Actors
{
	public class FeedCrawlerActor : ReceiveActor
	{
		public FeedCrawlerActor()
		{
			Receive<CrawlFeedMessage>(_ =>
		   {
			   var feed = FeedReader.ReadAsync(_.URI).Result;

			   var actorRef = Context.ActorOf<TableOfContentsActor>();
			   actorRef.Tell(new TableOfContentsItemMessage { ID = _.ID, Title = feed.Title });

			   var epubRef = Context.ActorOf<EpubActor>();

			   //send DownloadImagesMessage to WebAssetLoaderActor
			   //send AddChapterMessage to EpubActor
			   Context.Sender.Tell(new CrawlSuccededMessage());

			   Console.WriteLine("Feed aquiered");
			   return true;
		   });
		}
	}
}