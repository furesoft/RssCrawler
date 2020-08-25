using System;
using System.Collections.Generic;
using Akka.Actor;
using EpubSharp;
using EpubSharp.Format;
using RssCrawler.Messages;
using RssCrawler.Properties;
using Scriban;

namespace RssCrawler.Actors
{
	public class TableOfContentsActor : ReceiveActor
	{
		private List<object> _feedinfo = new List<object>();
		private Dictionary<string, Guid> _ids = new Dictionary<string, Guid>();

		public TableOfContentsActor()
		{
			Receive<TableOfContentsItemMessage>(_ =>
			{
				_feedinfo.Add(new { title = _.Title, _.ID });
				_ids.Add(_.Title, _.ID);

				Console.WriteLine("Item added to TOC");

				return true;
			});

			Receive<RenderTableOfContentsMessage>(_ =>
			{
				var noc = Template.Parse(Resources.TOC).Render(new { info = _feedinfo });
				Console.WriteLine("TOC rendered");

				// send rendered toc to EpubActor with AddFileMessage
				var epubRef = Context.ActorOf<EpubActor>();
				epubRef.Tell(new AddFileMessage { Filename = "toc.ncx", Content = noc, Type = EpubContentType.Xml });
			});
		}
	}
}