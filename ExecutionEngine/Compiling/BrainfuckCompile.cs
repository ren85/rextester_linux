using System;

namespace ExecutionEngine
{
	public class BrainfuckCompile : ICompiler
	{
		public BrainfuckCompile ()
		{
		}

		#region ICompiler implementation
		public CompilerData Compile (InputData idata, CompilerData cdata)
		{
			cdata.ExecuteThis = idata.PathToSource;
			cdata.Executor = "bf";
			cdata.Success = true;
			return cdata;
		}
		#endregion
	}
}

