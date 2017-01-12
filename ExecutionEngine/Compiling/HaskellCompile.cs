using System;
using System.Collections.Generic;
using System.IO;

namespace ExecutionEngine
{
	public class HaskellCompile : ICompiler
	{
		public HaskellCompile ()
		{
		}

		#region ICompiler implementation
		public CompilerData Compile (InputData idata, CompilerData cdata)
		{
			Directory.SetCurrentDirectory (idata.BaseDir);
			List<string> dropLines = new List<string>();
			dropLines.Add("Compiling");
			dropLines.Add("Linking");
			
			string compiler = "ghc";

			if(string.IsNullOrEmpty(idata.Compiler_args) || !idata.Compiler_args.Contains("-o a.out"))
			{
				cdata.Error = "Compiler args must contain '-o a.out'";
				cdata.Success = false;
				return cdata;
			}
			if(string.IsNullOrEmpty(idata.Compiler_args) || !idata.Compiler_args.Contains("source_file.hs"))
			{
				cdata.Error = "Compiler args must contain 'source_file.hs'";
				cdata.Success = false;
				return cdata;
			}
			
			idata.Compiler_args = idata.Compiler_args.Replace("source_file.hs", idata.PathToSource);
			idata.Compiler_args = idata.Compiler_args.Replace("-o a.out", "-o "+ idata.BaseDir + "a.out ");
			
			string args = idata.Compiler_args;
			//string args = /*"-Wall*/" -o " + idata.BaseDir + "a.out " + idata.PathToSource;

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

