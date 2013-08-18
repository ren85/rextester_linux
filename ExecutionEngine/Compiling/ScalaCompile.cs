using System;
using System.IO;
using Mono.Unix.Native;

namespace ExecutionEngine
{
	public class ScalaCompile : ICompiler
	{
		public ScalaCompile ()
		{
		}
		
		#region ICompiler implementation
		public CompilerData Compile (InputData idata, CompilerData cdata)
		{
			string compiler = "fsc";
			string args = "-deprecation -unchecked -encoding UTF-8 -d " + idata.BaseDir +" "+ idata.PathToSource;
			long compileTime;
			//Syscall.chmod(idata.BaseDir, FilePermissions.ACCESSPERMS);
			//Syscall.chmod(idata.PathToSource, FilePermissions.ACCESSPERMS);
			var res = Engine.CallCompiler(compiler, args, out compileTime);
			cdata.CompileTimeMs = compileTime;
			if(!File.Exists(idata.BaseDir+"Rextester.class") || !string.IsNullOrEmpty(res[1]))
			{
				if(res.Count > 1)
				{
					if(string.IsNullOrEmpty(res[0]) && string.IsNullOrEmpty(res[1]))
						cdata.Error = "Entry class 'Rextester' missing (it's either you haven't declared 'object Rextester' or you have declared 'package some_package').";
					else
						cdata.Error = Utils.ConcatenateString(res[0], res[1]);
				}
				cdata.Success = false;
				return cdata;		
			}
			if(res.Count > 1 && (!string.IsNullOrEmpty(res[0]) || !string.IsNullOrEmpty(res[1])))
				cdata.Warning = Utils.ConcatenateString(res[0], res[1]);
			cdata.ExecuteThis = "-Dfile.encoding=UTF-8 -classpath " +idata.BaseDir+" Rextester";		
			cdata.Executor = "scala";					
			cdata.Success = true;
			return cdata;
		}
		#endregion
	}
}

