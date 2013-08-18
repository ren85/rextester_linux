using System;

namespace ExecutionEngine
{
	public class JavascriptCompile : ICompiler
	{
		public JavascriptCompile ()
		{
		}
		
		#region ICompiler implementation
		public CompilerData Compile (InputData idata, CompilerData cdata)
		{
			cdata.ExecuteThis = idata.PathToSource;
			cdata.Executor = "/home/ren/v8/d8";
			cdata.Success = true;
			return cdata;
		}
		#endregion
	}
}

