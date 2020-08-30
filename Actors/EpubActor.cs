using System;
using Akka.Actor;
using EpubSharp;
using RssCrawler.Messages;
using RssCrawler.Properties;

namespace RssCrawler.Actors
{
	public class EpubActor : ReceiveActor
	{
		public EpubActor()
		{
			Singleton<EpubWriter>.Instance.AddAuthor("Furesoft");

			Receive<AddFileMessage>(_ =>
			{
				Handle(_);
			});

			Receive<SaveEpubMessage>(_ =>
			{
				Handle(_);
			});
		}

		private void Handle(AddFileMessage msg)
		{
			Console.WriteLine("Adding File: " + msg.Filename);

			Singleton<EpubWriter>.Instance.AddFile(msg.Filename, msg.Content, msg.Type);
		}

		private void Handle(SaveEpubMessage msg)
		{
			Singleton<EpubWriter>.Instance.AddChapter("test", "hello world");
			Singleton<EpubWriter>.Instance.SetCover(Resources.cover, ImageFormat.Jpeg);

			Singleton<EpubWriter>.Instance.SetTitle($"epaper_{DateTime.Now.Date}");
			Singleton<EpubWriter>.Instance.Write($"epaper_{DateTime.Now.Date.ToShortDateString()}.epub");
		}
	}
}