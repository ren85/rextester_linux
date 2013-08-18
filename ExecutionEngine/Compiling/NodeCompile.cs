using System;

namespace ExecutionEngine
{
	public class NodeCompile : ICompiler
	{
		public NodeCompile ()
		{
		}
		
		#region ICompiler implementation
		public CompilerData Compile (InputData idata, CompilerData cdata)
		{
			cdata.ExecuteThis = idata.PathToSource;
			cdata.Executor = "nodejs";
			cdata.Success = true;
			return cdata;
		}
		#endregion
	}
}

