using System;
using System.IO;
using System.Collections.Generic;

namespace ExecutionEngine
{
	public class FSharpCompile: ICompiler
	{
		public FSharpCompile ()
		{
		}

		#region ICompiler implementation
		public CompilerData Compile (InputData idata, CompilerData cdata)
		{
			Directory.SetCurrentDirectory (idata.BaseDir);
			List<string> dropLines = new List<string>();
			dropLines.Add("F# Compiler for F# 4.1");
			dropLines.Add("Freely distributed under the Apache 2.0 Open Source License");
			dropLines.Add ("F# Compiler for F# 4.0");


			string compiler = "fsharpc";
			string args = "-o "+idata.BaseDir+"a.exe "+ idata.PathToSource;
			long compileTime;

			var res = Engine.CallCompiler(compiler, args, out compileTime);
			cdata.CompileTimeMs = compileTime;
			if(!File.Exists(idata.BaseDir+"a.exe"))
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
			cdata.ExecuteThis = idata.BaseDir+"a.exe";
			cdata.Executor = "mono";
			cdata.Success = true;
			return cdata;
		}
		#endregion
	}
}

