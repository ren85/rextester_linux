using System;
using System.IO;
using System.Text.RegularExpressions;

namespace ExecutionEngine
{
	public class GoCompile : ICompiler
	{
		public GoCompile ()
		{
		}
		
		#region ICompiler implementation
		public CompilerData Compile (InputData idata, CompilerData cdata)
		{
			string compiler = "go";


			if(string.IsNullOrEmpty(idata.Compiler_args) || !idata.Compiler_args.Contains("-o a.out"))
			{
				cdata.Error = "Compiler args must contain '-o a.out'";
				cdata.Success = false;
				return cdata;
			}
			if(string.IsNullOrEmpty(idata.Compiler_args) || !idata.Compiler_args.Contains("source_file.go"))
			{
				cdata.Error = "Compiler args must contain 'source_file.go'";
				cdata.Success = false;
				return cdata;
			}
			
			idata.Compiler_args = idata.Compiler_args.Replace("source_file.go", idata.PathToSource);
			idata.Compiler_args = idata.Compiler_args.Replace("-o a.out", "-o "+ idata.BaseDir + "a.out ");
			
			string args = "build " + idata.Compiler_args;
			//string args = "build -o " + idata.BaseDir + "a.out " + idata.PathToSource;

			long compileTime;
			var res = Engine.CallCompiler(compiler, args, out compileTime);
			cdata.CompileTimeMs = compileTime;

			Regex r = new Regex(@"service/usercode/\d+/\d+");

			if(!File.Exists(idata.BaseDir+"a.out"))
			{
				if(res.Count > 1)
				{
					cdata.Error = Utils.ConcatenateString(res[0], res[1]);
					cdata.Error = cdata.Error.Replace("# command-line-arguments\n", "");
					cdata.Error = r.Replace(cdata.Error, "source_file");

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
			{
				cdata.Warning = Utils.ConcatenateString(res[0], res[1]);
				cdata.Warning = r.Replace(cdata.Warning, "source_file");
			}
			cdata.ExecuteThis = idata.BaseDir+"a.out";
			cdata.Executor = "";
			cdata.Success = true;
			return cdata;
		}
		#endregion
	}
}

