using System;
using System.Collections.Generic;
using Akka.Actor;
using EpubSharp;
using EpubSharp.Format;
using RssCrawler.Core;
using RssCrawler.Messages;
using RssCrawler.Properties;
using Scriban;

namespace RssCrawler.Actors
{
	public class TableOfContentsActor : ReceiveActor
	{
		public TableOfContentsActor()
		{
			Receive<TableOfContentsItemMessage>(_ =>
			{
				ServiceProvider.Get<List<object>>("feedinfo").Add(new { title = _.Title, _.ID });
				ServiceProvider.Get<Dictionary<string, Guid>>("ids").Add(_.Title, _.ID);

				return true;
			});

			Receive<RenderTableOfContentsMessage>(_ =>
			{
				var noc = Template.Parse(Resources.TOC).Render(new { info = ServiceProvider.Get<List<object>>("feedinfo") });

				// send rendered toc to EpubActor with AddFileMessage
				var epubRef = Context.ActorOf<EpubActor>();
				epubRef.Tell(new AddFileMessage { Filename = "toc.ncx", Content = noc, Type = EpubContentType.Xml });

				Context.Sender.Tell(new RenderTOCSuccededMessage());
			});
		}
	}
}