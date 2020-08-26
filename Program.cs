using Akka.Actor;
using Akka.Configuration;
using Akka.Routing;
using CodeHollow.FeedReader;
using EpubSharp;
using HtmlAgilityPack;
using RssCrawler.Actors;
using RssCrawler.Messages;
using RssCrawler.Properties;
using Scriban;
using Scriban.Runtime;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;

namespace RssCrawler
{
	public static class Program
	{
		public static void Main()
		{
			var writer = new EpubWriter();

			writer.AddAuthor("Furesoft");
			var uris = new[] {
				"https://rss.golem.de/rss.php?tp=dev&feed=RSS2.0",
				"https://www.heise.de/rss/heiseplus-atom.xml",
				"https://www.spiegel.de/schlagzeilen/tops/index.rss",
				"https://www.spiegel.de/schlagzeilen/index.rss",
				"https://www.welt.de/feeds/topnews.rss",
			};

			var actors = ActorSystem.Create("feedCrawlerSystem");

			actors.ActorOf<FeedSupervisor>().Tell(new StartFeedCrawlingMessage { URIs = uris });

			while (Console.ReadLine() != "c")
			{
				actors.Terminate();
			}

			actors.WhenTerminated.Wait();

			/*

			foreach (var uri in uris)
			{
				var feed = FeedReader.ReadAsync(uri).Result;

				if (feed.ImageUrl != null)
				{
					var webClient = new WebClient();
					var logo = webClient.DownloadData(feed.ImageUrl);
					writer.AddFile(Path.GetFileName(feed.ImageUrl), logo, EpubSharp.Format.EpubContentType.ImageJpeg);
				}

				foreach (var item in feed.Items)
				{
					if (item.Content != null)
					{
						var d = new HtmlDocument();
						d.LoadHtml(item.Content);

						var nodes = d.DocumentNode.SelectNodes("img");
						if (nodes != null)
						{
							foreach (var n in nodes)
							{
								var url = n.Attributes["src"].Value;

								//download image
								var webClient = new WebClient();
								var logo = webClient.DownloadData(url);
								writer.AddFile(Path.GetFileName(url), logo, EpubSharp.Format.EpubContentType.ImageJpeg);
								//set new uri
								n.Attributes["src"].Value = Path.GetFileName(url);
							}

							var ms = new MemoryStream();
							d.Save(ms);

							item.Content = Encoding.UTF8.GetString(ms.ToArray());
						}
					}
				}

				var template = Template.Parse(Resources.Template);
				var scriptObject1 = new ScriptObject
				{
					{ "logo", Path.GetFileName(feed.ImageUrl) },
					{ "feed", feed }
				};

				var context = new TemplateContext();
				context.PushGlobal(scriptObject1);

				var result = template.Render(context);

				writer.AddChapter(feed.Title, result, ids[feed.Title]);
			}

			//when to call SaveEpubMessage?

			*/
		}
	}
}