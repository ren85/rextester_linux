using System;
using System.IO;

namespace ExecutionEngine
{
	public class OctaveCompile : ICompiler
	{
		public OctaveCompile ()
		{
		}

		public CompilerData Compile (InputData idata, CompilerData cdata)
		{
			cdata.ExecuteThis = " -f -q --no-window-system " + idata.PathToSource;
			cdata.Executor = "octave";
			cdata.Success = true;
			string source = File.ReadAllText(idata.PathToSource);
			File.WriteAllText(idata.PathToSource, "cd " + idata.BaseDir + ";" + Environment.NewLine + source);
			return cdata;
		}
	}
}

