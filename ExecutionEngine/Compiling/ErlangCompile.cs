using System;
using System.IO;

namespace ExecutionEngine
{
	public class ErlangCompile: ICompiler
	{
		public ErlangCompile () 
		{
		}

		#region ICompiler implementation
		public CompilerData Compile (InputData idata, CompilerData cdata)
		{
			Environment.SetEnvironmentVariable ("HOME", idata.BaseDir);
			Directory.SetCurrentDirectory (idata.BaseDir);	
			string compiler = "erl";
			string args = "-compile " + idata.PathToSource;
			long compileTime;

			var res = Engine.CallCompiler(compiler, args, out compileTime);
			cdata.CompileTimeMs = compileTime;
			if(!File.Exists(idata.BaseDir+"source.beam"))
			{
				if(res.Count > 1)
				{
					cdata.Error = Utils.ConcatenateString(res[0], res[1]);
				}	
				cdata.Success = false;
				return cdata;	
			}
			if(res.Count > 1 && (!string.IsNullOrEmpty(res[0]) || !string.IsNullOrEmpty(res[1])))
			{
				cdata.Warning = Utils.ConcatenateString(res[0], res[1]);
			}
			cdata.ExecuteThis = "-noshell -s source entry_point -s init stop";
			cdata.Executor = "erl";
			cdata.Success = true;
			return cdata;
		}
		#endregion
	}
}

