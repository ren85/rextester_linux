using System;

namespace ExecutionEngine
{
	public class Python3Compile : ICompiler
	{
		public Python3Compile ()
		{
		}
		
		#region ICompiler implementation
		public CompilerData Compile (InputData idata, CompilerData cdata)
		{
			cdata.ExecuteThis = idata.PathToSource;
			cdata.Executor = "python3";
			cdata.Success = true;
			return cdata;
		}
		#endregion
	}
}

