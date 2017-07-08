using System;
using System.IO;

namespace ExecutionEngine
{
	public class FortranCompile : ICompiler
	{
		public FortranCompile ()
		{
		}

		#region ICompiler implementation
		public CompilerData Compile (InputData idata, CompilerData cdata)
		{
			string compiler = "gfortran";
			string args = "-o "+idata.BaseDir+"a.out " + " -ffree-form " + idata.PathToSource;
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
			{
				cdata.Warning = Utils.ConcatenateString(res[0], res[1]);
			}
			cdata.ExecuteThis = idata.BaseDir+"a.out";
			cdata.Executor = "";
			cdata.Success = true;
			return cdata;
		}
		#endregion
	}
}

