using System;
using System.IO;

namespace ExecutionEngine
{
	public class ObjectiveCCompile : ICompiler
	{
		public ObjectiveCCompile ()
		{
		}

		#region ICompiler implementation
		public CompilerData Compile (InputData idata, CompilerData cdata)
		{
			string compiler = "gcc";

			if(string.IsNullOrEmpty(idata.Compiler_args) || !idata.Compiler_args.Contains("-o a.out"))
			{
				cdata.Error = "Compiler args must contain '-o a.out'";
				cdata.Success = false;
				return cdata;
			}
			if(string.IsNullOrEmpty(idata.Compiler_args) || !idata.Compiler_args.Contains("source_file.m"))
			{
				cdata.Error = "Compiler args must contain 'source_file.m'";
				cdata.Success = false;
				return cdata;
			}
			
			idata.Compiler_args = idata.Compiler_args.Replace("source_file.m", idata.PathToSource);
			idata.Compiler_args = idata.Compiler_args.Replace("-o a.out", "-o "+ idata.BaseDir + "a.out ");
			
			string args = idata.Compiler_args;
			//string args = " -MMD -MP -DGNUSTEP -DGNUSTEP_BASE_LIBRARY=1 -DGNU_GUI_LIBRARY=1 -DGNU_RUNTIME=1 -DGNUSTEP_BASE_LIBRARY=1 -fno-strict-aliasing -fexceptions -fobjc-exceptions -D_NATIVE_OBJC_EXCEPTIONS -pthread -fPIC -Wall -DGSWARN -DGSDIAGNOSE -Wno-import -g -O2 -fgnu-runtime -fconstant-string-class=NSConstantString -I. -I/usr/local/include/GNUstep -I/usr/include/GNUstep" + " -o "+ idata.BaseDir + "a.out " + idata.PathToSource + "  -lobjc -lgnustep-base";
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

