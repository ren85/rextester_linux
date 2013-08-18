using System;
using System.Collections.Generic;

namespace ServiceWarmup
{
	public class RunFactory
	{
		RunFactory ()
		{
		}
		
		static List<IRun> list = new List<IRun>()
		{
			new JavaRun(),
			new CRun(),
			new CppRun(),
			new HaskellRun(),
			new ScalaRun(),
			new GoRun()
		};
		
		static int Count
		{
			get;
			set;
		}
		
		static public IRun Next()
		{
			Count = (Count + 1) % list.Count;
			return list[Count];
		}
	}
}

