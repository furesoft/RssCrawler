using System;
using Akka.Actor;
using EpubSharp;
using RssCrawler.Messages;
using RssCrawler.Properties;

namespace RssCrawler.Actors
{
	public class EpubActor : ReceiveActor
	{
		private EpubWriter _writer;

		public EpubActor()
		{
			_writer = new EpubWriter();

			_writer.AddAuthor("Furesoft");

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
			_writer.AddFile(msg.Filename, msg.Content, msg.Type);
		}

		private void Handle(SaveEpubMessage msg)
		{
			_writer.SetCover(Resources.cover, ImageFormat.Jpeg);

			_writer.SetTitle($"epaper_{DateTime.Now.Date}");
			_writer.Write($"epaper_{DateTime.Now.Date.ToShortDateString()}.epub");
		}
	}
}