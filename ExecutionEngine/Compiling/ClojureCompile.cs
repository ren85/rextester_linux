using System;
using System.IO;

namespace ExecutionEngine
{
	public class ClojureCompile: ICompiler
	{
		public ClojureCompile ()
		{
		}

		#region ICompiler implementation
		public CompilerData Compile (InputData idata, CompilerData cdata)
		{
			string compiler = "clojurec";
			string args = " -d " + idata.BaseDir + " -cp " + idata.BaseDir + " " + idata.PathToSource;
			long compileTime;
			var res = Engine.CallCompiler(compiler, args, out compileTime);
			cdata.CompileTimeMs = compileTime;
			if(!File.Exists(idata.BaseDir+"Rextester.class") || !string.IsNullOrEmpty(res[1]))
			{
				if(res.Count > 1)
				{
					if(string.IsNullOrEmpty(res[0]) && string.IsNullOrEmpty(res[1]))
						cdata.Error = "Method 'main' must be in a class 'Rextester'.";
					else
						cdata.Error = Utils.ConcatenateString(res[0], res[1]);
				}
				cdata.Success = false;
				return cdata;		
			}
			if(res.Count > 1 && (!string.IsNullOrEmpty(res[0]) || !string.IsNullOrEmpty(res[1])))
				cdata.Warning = Utils.ConcatenateString(res[0], res[1]);
			cdata.ExecuteThis = "-Xmx256m -Dfile.encoding=UTF-8 -classpath " +idata.BaseDir+" Rextester";		
			cdata.Executor = "java";					
			cdata.Success = true;
			return cdata;
		}
		#endregion
	}
}

