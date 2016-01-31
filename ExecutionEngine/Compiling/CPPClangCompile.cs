using System;
using System.IO;

namespace ExecutionEngine
{
	public class CPPClangCompile : ICompiler
	{
		public CPPClangCompile ()
		{
		}

		#region ICompiler implementation
		public CompilerData Compile (InputData idata, CompilerData cdata)
		{
			string compiler = "clang++-3.6";
			if(string.IsNullOrEmpty(idata.Compiler_args) || !idata.Compiler_args.Contains("-o a.out"))
			{
				cdata.Error = "Compiler args must contain '-o a.out'";
				cdata.Success = false;
				return cdata;
			}
			if(string.IsNullOrEmpty(idata.Compiler_args) || !idata.Compiler_args.Contains("source_file.cpp"))
			{
				cdata.Error = "Compiler args must contain 'source_file.cpp'";
				cdata.Success = false;
				return cdata;
			}

			idata.Compiler_args = idata.Compiler_args.Replace("source_file.cpp", idata.PathToSource);
			idata.Compiler_args = idata.Compiler_args.Replace("-o a.out", "-o "+ idata.BaseDir + "a.out ");

			string args = idata.Compiler_args;
			long compileTime;
			var res = Engine.CallCompiler(compiler, args, out compileTime);
			cdata.CompileTimeMs = compileTime;
			if(!File.Exists(idata.BaseDir+"a.out"))
			{
				if(res.Count > 1)
				{
					cdata.Error = Utils.ConcatenateString(res[0], res[1]);
					/*if(cdata.Error != null)
					{
						string[] ew = cdata.Error.Split(new string[]{"\n"}, StringSplitOptions.RemoveEmptyEntries);
						string error = "";
						string warning = "";
						foreach(var a in ew)
							if(a.Contains("error: "))
								error+=a+"\n";
							else if(a.Contains("warning: "))
								warning+=a+"\n";
						cdata.Error = error;
						cdata.Warning = warning;
					}*/
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

