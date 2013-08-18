using System;
using ServiceWarmup.n226589_s_dedikuoti_lt;

namespace ServiceWarmup
{
	public class ScalaRun : IRun
	{
		public ScalaRun ()
		{
		}

		#region IRun implementation
		public string Code 
		{
			get 
			{
				return @"
object Rextester extends App {
    println("""+Output+@""")
}";
			}
		}
		
		public Languages Language 
		{
			get 
			{
				return Languages.Scala;
			}
		}
		
		public string Output 
		{
			get 
			{
				return "Hello, Scala world!";
			}
		}
		
		public string CompilerArgs
		{
			get
			{
				return "";
			}
		}
		#endregion
	}
}

