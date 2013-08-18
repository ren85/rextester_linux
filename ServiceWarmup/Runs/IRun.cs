using System;
using ServiceWarmup.n226589_s_dedikuoti_lt;

namespace ServiceWarmup
{
	public interface IRun
	{
		string Code
		{
			get;
		}
		Languages Language
		{
			get;
		}
		string Output
		{
			get;
		}
		string CompilerArgs
		{
			get;
		}
	}
}

