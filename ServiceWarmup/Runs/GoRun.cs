using System;
using ServiceWarmup.n226589_s_dedikuoti_lt;

namespace ServiceWarmup
{
	public class GoRun : IRun
	{
		public GoRun ()
		{
		}

		#region IRun implementation
		public string Code 
		{
			get 
			{
				return @"
package main  
import ""fmt"" 

func main() { 
	fmt.Printf("""+Output+@""") 
}";
			}
		}
		
		public Languages Language 
		{
			get 
			{
				return Languages.Go;
			}
		}
		
		public string Output 
		{
			get 
			{
				return "Hello, Go world!";
			}
		}
		
		public string CompilerArgs
		{
			get
			{
				return "-o a.out source_file.go";
			}
		}
#endregion
	}
}

