using System;
using System.Collections.Generic;
using System.Text;

namespace RssCrawler.Core
{
	public static class ServiceProvider
	{
		private static Dictionary<string, object> data = new Dictionary<string, object>();

		public static T Get<T>(string name)
			where T : class, new()
		{
			if (data.ContainsKey(name))
			{
				return (T)data[name];
			}
			else
			{
				data.Add(name, new T());
				return Get<T>(name);
			}
		}
	}
}