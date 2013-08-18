using System;

namespace ExecutionEngine
{
	public class PythonCompile : ICompiler
	{
		public PythonCompile ()
		{
		}

		#region ICompiler implementation
		public CompilerData Compile (InputData idata, CompilerData cdata)
		{
			cdata.ExecuteThis = idata.PathToSource;
			cdata.Executor = "python";
			cdata.Success = true;
			return cdata;
		}
		#endregion
	}
}

