using System;
using ServiceWarmup.n178_238_226_7;

namespace ServiceWarmup
{
	public class CRun : IRun
	{
		public CRun ()
		{
		}

		#region IRun implementation
		public string Code 
		{
			get 
			{
				return @"
#include  <stdio.h>

int main(void)
{
    printf("""+Output+@""");
    return 0;
}";
			}
		}

		public Languages Language 
		{
			get 
			{
				return Languages.C;
			}
		}

		public string Output 
		{
			get 
			{
				return "Hello, C world!";
			}
		}

		public string CompilerArgs
		{
			get
			{
				return "-Wall -std=gnu99 -O2 -o a.out source_file.c";
			}
		}
		#endregion
	}
}

