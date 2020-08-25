using EpubSharp.Format;

namespace RssCrawler.Messages
{
	public class AddFileMessage
	{
		public string Filename { get; internal set; }
		public string Content { get; internal set; }
		public EpubContentType Type { get; internal set; }
	}
}