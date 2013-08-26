using System;
using System.IO;

namespace ExecutionEngine
{
	public class NasmCompile : ICompiler
	{
		public NasmCompile ()
		{
		}

		#region ICompiler implementation
		public CompilerData Compile (InputData idata, CompilerData cdata)
		{			
			string compiler = "nasm";
			string args = "-f elf64 -o "+idata.BaseDir+"1.o " + idata.PathToSource;
			long compileTime;
			var res = Engine.CallCompiler(compiler, args, out compileTime);
			cdata.CompileTimeMs = compileTime;
			if(!File.Exists(idata.BaseDir+"1.o"))
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
			
			//now linker
			compiler = "ld";
			args = "-o "+idata.BaseDir+"a.out "+idata.BaseDir+"1.o";
			res = Engine.CallCompiler(compiler, args, out compileTime);
			cdata.CompileTimeMs += compileTime;
			if(!File.Exists(idata.BaseDir+"a.out"))
			{
				if(res.Count > 1)
				{
					cdata.Error = "Linker:\n"+Utils.ConcatenateString(res[0], res[1]);
				}	
				cdata.Success = false;
				return cdata;
			}
			if(res.Count > 1 && (!string.IsNullOrEmpty(res[0]) || !string.IsNullOrEmpty(res[1])))
			{
				if(!string.IsNullOrEmpty(cdata.Warning))
					cdata.Warning += "\n";
				else
					cdata.Warning = "";
				cdata.Warning += "Linker:\n";
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

