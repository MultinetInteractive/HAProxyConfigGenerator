using System;
using System.IO;
using System.Linq;

namespace HAProxyConfigGenerator
{
    class Program
    {
        static int Main(string[] args)
        {
			if (args.Length < 1)
			{
				Console.Error.WriteLine("Please provide path to HAProxy-JSON.");
				return -1;
			}

			var path = args.First();

			if (!File.Exists(path))
			{
				Console.Error.WriteLine("The file provided does not exist.");
				return -1;
			}

			var cfg = HAProxyConfig.Parse(path);
			Console.WriteLine(cfg.ToHAProxyConfigString());			
			return 0;
        }
    }
}