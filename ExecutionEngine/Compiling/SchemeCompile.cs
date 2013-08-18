using System;

namespace ExecutionEngine
{
	public class SchemeCompile : ICompiler
	{
		public SchemeCompile ()
		{
		}
		
		#region ICompiler implementation
		public CompilerData Compile (InputData idata, CompilerData cdata)
		{
			cdata.ExecuteThis = "-s "+idata.PathToSource; //--no-auto-compile
			cdata.Executor = "guile";
			cdata.Success = true;
			return cdata;
		}
		#endregion
	}
}

