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
			cdata.Executor = "/home/ren/v8/out/ia32.release/d8";
			cdata.Success = true;
			return cdata;
		}
		#endregion
	}
}

