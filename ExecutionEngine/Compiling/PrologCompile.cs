using System;

namespace ExecutionEngine
{
	public class PrologCompile : ICompiler
	{
		public PrologCompile ()
		{
		}
		
		#region ICompiler implementation
		public CompilerData Compile (InputData idata, CompilerData cdata)
		{
			cdata.ExecuteThis = idata.PathToSource;
			cdata.Executor = "prolog -q -s ";
			cdata.Success = true;
			return cdata;
		}
		#endregion
	}
}


