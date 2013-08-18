using System;

namespace ExecutionEngine
{
	public class PhpCompile : ICompiler
	{
		public PhpCompile ()
		{
		}

		#region ICompiler implementation
		public CompilerData Compile (InputData idata, CompilerData cdata)
		{
			cdata.ExecuteThis = idata.PathToSource;
			cdata.Executor = "php";
			cdata.Success = true;
			return cdata;
		}
		#endregion
	}
}

