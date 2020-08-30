using System.Collections.Generic;

namespace RssCrawler.Messages
{
	public class ResultMessage
	{
		public List<AddFileMessage> Files { get; set; } = new List<AddFileMessage>();

		public ResultMessage WithFile(AddFileMessage msg)
		{
			Files.Add(msg);
			return this;
		}
	}
}