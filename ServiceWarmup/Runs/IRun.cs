using System;
using ServiceWarmup.n178_238_226_7;

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

