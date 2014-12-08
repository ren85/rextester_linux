using System;
using System.IO;

namespace ExecutionEngine
{
	public class RCompile : ICompiler
	{
		public RCompile ()
		{
		}

		public CompilerData Compile (InputData idata, CompilerData cdata)
		{
			cdata.ExecuteThis = " --slave -f " + idata.PathToSource;
			cdata.Executor = "R";
			cdata.Success = true;
			string source = File.ReadAllText(idata.PathToSource);
			File.WriteAllText(idata.PathToSource, "setwd('" + idata.BaseDir + "')" + Environment.NewLine + source);
			return cdata;
		}
	}
}

