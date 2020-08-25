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
	public class EpubActor : ReceiveActor
	{
		private EpubWriter _writer;

		public EpubActor()
		{
			_writer = new EpubWriter();

			_writer.AddAuthor("Furesoft");

			Receive<AddFileMessage>(_ =>
			{
				_writer.AddFile(_.Filename, _.Content, _.Type);
			});

			Receive<SaveEpubMessage>(_ =>
			{
				_writer.SetCover(Resources.cover, ImageFormat.Jpeg);

				_writer.SetTitle($"epaper_{DateTime.Now.Date}");
				_writer.Write($"epaper_{DateTime.Now.Date.ToShortDateString()}.epub");
			});
		}
	}
}