using System;
using System.IO;
using System.Collections.Generic;

namespace ExecutionEngine
{
	public class RCompile : ICompiler
	{
		public RCompile ()
		{
		}

		public CompilerData Compile (InputData idata, CompilerData cdata)
		{
			Directory.SetCurrentDirectory (idata.BaseDir);
			List<string> dropLines = new List<string>();
			dropLines.Add("sh: /bin/rm: Permission denied");
			dropLines.Add("sh: 1: rm: Permission denied");

			cdata.ExecuteThis = " --slave --vanilla -f " + idata.PathToSource;
			cdata.Executor = "R";
			cdata.Success = true;
			string source = File.ReadAllText(idata.PathToSource);
			File.WriteAllText(idata.PathToSource, "setwd('" + idata.BaseDir + "')" + Environment.NewLine + source);
			return cdata;
		}
	}
}

