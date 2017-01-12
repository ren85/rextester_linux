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
			cdata.ExecuteThis = "-f " + idata.PathToSource;
			cdata.Executor = "js24";
			cdata.Success = true;
			return cdata;
		}
		#endregion
	}
}

