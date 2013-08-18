using System;

namespace ExecutionEngine
{
	public class LispCompile : ICompiler
	{
		public LispCompile ()
		{
		}
		
		#region ICompiler implementation
		public CompilerData Compile (InputData idata, CompilerData cdata)
		{
			cdata.ExecuteThis = idata.PathToSource;
			cdata.Executor = "clisp";
			cdata.Success = true;
			return cdata;
		}
		#endregion
	}
}

