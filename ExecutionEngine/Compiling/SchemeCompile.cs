using System;
using System.IO;

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
			Directory.SetCurrentDirectory (idata.BaseDir);
			cdata.ExecuteThis = "--no-auto-compile -s "+idata.PathToSource; //--no-auto-compile
			cdata.Executor = "guile";
			cdata.Success = true;
			return cdata;
		}
		#endregion
	}
}

