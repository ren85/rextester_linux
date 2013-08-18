using System;

namespace ExecutionEngine
{
	public class PerlCompile : ICompiler
	{
		public PerlCompile ()
		{
		}

		#region ICompiler implementation
		public CompilerData Compile (InputData idata, CompilerData cdata)
		{
			cdata.ExecuteThis = idata.PathToSource;
			cdata.Executor = "perl -w ";
			cdata.Success = true;
			return cdata;
		}
		#endregion
	}
}

