using System;
using System.IO;
using System.Collections.Generic;
using System.Text;

namespace ExecutionEngine
{
	public class PascalCompile : ICompiler
	{
		public PascalCompile ()
		{
		}

		#region ICompiler implementation
		public CompilerData Compile (InputData idata, CompilerData cdata)
		{
			List<string> dropLines = new List<string>();
			dropLines.Add("Free Pascal Compiler version");
			dropLines.Add("Copyright (c)");
			dropLines.Add("Target OS:");
			dropLines.Add("Compiling ");
			dropLines.Add("Linking ");
			dropLines.Add("lines compiled");
			dropLines.Add("contains output sections; did you forget -T?");
			dropLines.Add("Fatal: Compilation aborted");
			dropLines.Add("returned an error exitcode (normal if you did not specify a source file to be compiled)");
			
			
			string compiler = "fpc";
			string args = "-o"+idata.BaseDir+"a.out " + " -Fi"+idata.BaseDir + " " + idata.PathToSource;
			long compileTime;
			var res = Engine.CallCompiler(compiler, args, out compileTime);
			cdata.CompileTimeMs = compileTime;
			if(!File.Exists(idata.BaseDir+"a.out"))
			{
				if(res.Count > 1)
				{
					cdata.Error = Utils.RemoveSomeLines(Utils.ConcatenateString(res[0], res[1]), dropLines);
				}	
				cdata.Success = false;
				return cdata;
			}
			if(res.Count > 1 && (!string.IsNullOrEmpty(res[0]) || !string.IsNullOrEmpty(res[1])))
			{
				cdata.Warning = Utils.RemoveSomeLines(Utils.ConcatenateString(res[0], res[1]), dropLines);
			}
			cdata.ExecuteThis = idata.BaseDir+"a.out";
			cdata.Executor = "";
			cdata.Success = true;
			return cdata;
		}
		#endregion
	}
}

