using System;
using ServiceWarmup.n226589_s_dedikuoti_lt;

namespace ServiceWarmup
{
	public class JavaRun : IRun
	{
		public JavaRun ()
		{
		}

		#region IRun implementation
		public string Code
		{
			get
			{
			return @"
import java.util.*;
import java.lang.*;

class Rextester
{  
    public static void main(String args[])
    {
        System.out.println("""+Output+@""");
    }
}
";
			}
		}

		public Languages Language
		{
			get
			{
				return Languages.Java;
			}
		}

		public string Output
		{
			get
			{
				return "Hello, java world!";
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

