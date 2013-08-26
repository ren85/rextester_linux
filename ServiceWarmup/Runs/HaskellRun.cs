using System;
using ServiceWarmup.n178_238_226_7;

namespace ServiceWarmup
{
	public class HaskellRun : IRun
	{
		public HaskellRun ()
		{
		}

		#region IRun implementation
		public string Code 
		{
			get 
			{
				return @"
main = putStrLn """+Output+@"""";
			}
		}

		public Languages Language 
		{
			get 
			{
				return Languages.Haskell;
			}
		}

		public string Output 
		{
			get 
			{
				return "Hello, Haskell world!";
			}
		}
		
		public string CompilerArgs
		{
			get
			{
				return "-o a.out source_file.hs";
			}
		}
		#endregion
	}
}