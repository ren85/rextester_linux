using System;
using System.IO;

namespace ExecutionEngine
{
	public class BashCompile : ICompiler
	{
		public BashCompile ()
		{
		}


		#region ICompiler implementation
		public CompilerData Compile (InputData idata, CompilerData cdata)
		{
			Directory.SetCurrentDirectory (idata.BaseDir);
			cdata.ExecuteThis = idata.PathToSource;
			cdata.Executor = "bash";
			cdata.Success = true;
			return cdata;
		}
		#endregion
	}
}

