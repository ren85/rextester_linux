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
			Directory.SetCurrentDirectory (idata.BaseDir);
			File.Move (idata.PathToSource, idata.PathToSource.Replace ("source.m", "source_rextester.m"));
			cdata.ExecuteThis = " -q -f --no-window-system --no-history " + idata.PathToSource.Replace ("source.m", "source_rextester.m");
			cdata.Executor = "octave";
			cdata.Success = true;
			string source = File.ReadAllText(idata.PathToSource.Replace ("source.m", "source_rextester.m"));
			File.WriteAllText(idata.PathToSource.Replace ("source.m", "source_rextester.m"), "cd " + idata.BaseDir + ";" + Environment.NewLine + source);
			return cdata;
		}
	}
}

