using System;
using System.IO;

namespace ExecutionEngine
{
	public class DCompile: ICompiler
	{
		public DCompile ()
		{
		}

		#region ICompiler implementation
		public CompilerData Compile (InputData idata, CompilerData cdata)
		{
			string compiler = "dmd";
			if(string.IsNullOrEmpty(idata.Compiler_args) || !idata.Compiler_args.Contains("-ofa.out"))
			{
				cdata.Error = "Compiler args must contain '-ofa.out'";
				cdata.Success = false;
				return cdata;
			}
			if(string.IsNullOrEmpty(idata.Compiler_args) || !idata.Compiler_args.Contains("source_file.d"))
			{
				cdata.Error = "Compiler args must contain 'source_file.d'";
				cdata.Success = false;
				return cdata;
			}

			idata.Compiler_args = idata.Compiler_args.Replace("source_file.d", idata.PathToSource);
			idata.Compiler_args = idata.Compiler_args.Replace("-ofa.out", "-of"+ idata.BaseDir + "a.out ");

			string args = idata.Compiler_args;
			long compileTime;
			var res = Engine.CallCompiler(compiler, args, out compileTime);
			cdata.CompileTimeMs = compileTime;
			if(!File.Exists(idata.BaseDir+"a.out"))
			{
				if(res.Count > 1)
				{
					cdata.Error = Utils.ConcatenateString(res[0], res[1]);
				}
				cdata.Success = false;
				return cdata;
			}
			if(res.Count > 1 && (!string.IsNullOrEmpty(res[0]) || !string.IsNullOrEmpty(res[1])))
				cdata.Warning = Utils.ConcatenateString(res[0], res[1]);
			cdata.ExecuteThis = idata.BaseDir+"a.out";
			cdata.Executor = "";
			cdata.Success = true;
			return cdata;
		}
		#endregion
	}
}

