using System;
using System.IO;

namespace ExecutionEngine
{
	public class PrologCompile : ICompiler
	{
		public PrologCompile ()
		{
		}
		
		#region ICompiler implementation
		public CompilerData Compile (InputData idata, CompilerData cdata)
		{
			Directory.SetCurrentDirectory (idata.BaseDir);
			cdata.ExecuteThis = idata.PathToSource;
			cdata.Executor = "prolog -q -s ";
			cdata.Success = true;
			return cdata;
		}
		#endregion
	}
}


