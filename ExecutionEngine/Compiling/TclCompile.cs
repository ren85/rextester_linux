using System;

namespace ExecutionEngine
{
	public class TclCompile : ICompiler
	{
		public TclCompile ()
		{
		}

		#region ICompiler implementation
		public CompilerData Compile (InputData idata, CompilerData cdata)
		{
			cdata.ExecuteThis = idata.PathToSource;
			cdata.Executor = "tclsh";
			cdata.Success = true;
			return cdata;
		}
		#endregion
	}
}

