using System;

namespace HAProxyConfigGenerator
{
    class Program
    {
        static int Main(string[] args)
        {
			if (args.Length < 1)
			{
				Console.WriteLine("Please provide path to HAProxy-JSON.");
				return -1;
			}

            Console.WriteLine("TODO: Write the actual code");
			return 0;
        }
    }
}