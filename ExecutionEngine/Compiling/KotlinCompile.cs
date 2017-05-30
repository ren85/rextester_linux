using System;
using System.IO;

namespace ExecutionEngine
{
	public class KotlinCompile: ICompiler
	{
		public KotlinCompile ()
		{
		}

		#region ICompiler implementation
		public CompilerData Compile (InputData idata, CompilerData cdata)
		{
			//cdata.ExecuteThis = idata.PathToSource;
			//cdata.Executor = "/home/ren/.sdkman/candidates/kotlin/current/bin/kotlinc -script ";
			//cdata.Success = true;
			//return cdata;


			Directory.SetCurrentDirectory (idata.BaseDir);
			string compiler = "/home/ren/.sdkman/candidates/kotlin/current/bin/kotlinc";
			string args = idata.PathToSource + " -include-runtime -d " + idata.BaseDir + idata.Rand + ".jar";
			long compileTime;
			var res = Engine.CallCompiler(compiler, args, out compileTime);
			cdata.CompileTimeMs = compileTime;
			if(!File.Exists(idata.BaseDir + idata.Rand + ".jar"))
			{
				if(res.Count > 1)
				{
					cdata.Error = Utils.ConcatenateString(res[0], res[1]);
				}	
				cdata.Success = false;
				return cdata;		
			}
			if (res.Count > 1 && (!string.IsNullOrEmpty (res [0]) || !string.IsNullOrEmpty (res [1]))) {
				cdata.Warning = Utils.ConcatenateString (res [0], res [1]);
			}
			cdata.ExecuteThis = " -jar " + idata.BaseDir + idata.Rand + ".jar";		
			cdata.Executor = "java";					
			cdata.Success = true;
			return cdata;
		}
		#endregion
	}
}

