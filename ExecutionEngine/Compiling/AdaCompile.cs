using System;
using System.IO;
using System.Collections.Generic;

namespace ExecutionEngine
{
	public class AdaCompile: ICompiler
	{
		public AdaCompile ()
		{
		}

		#region ICompiler implementation
		public CompilerData Compile (InputData idata, CompilerData cdata)
		{		
			List<string> dropLines = new List<string>();
			dropLines.Add("gcc-4.9 -c -I");
			dropLines.Add ("file name does not match unit name, should be");


			Directory.SetCurrentDirectory (idata.BaseDir);	
			string compiler = "gnat compile";
			string args = idata.PathToSource;
			long compileTime;
			var res = Engine.CallCompiler(compiler, args, out compileTime);
			cdata.CompileTimeMs = compileTime;
			if(!File.Exists(idata.BaseDir + "source.o"))
			{
				if(res.Count > 1)
				{
					cdata.Error = Utils.RemoveSomeLines(Utils.ConcatenateString(res[0], res[1]), dropLines);
				}	
				cdata.Success = false;
				return cdata;
			}
			if(res.Count > 1 && (!string.IsNullOrEmpty(res[0]) || !string.IsNullOrEmpty(res[1])))
			{
				cdata.Warning = Utils.RemoveSomeLines(Utils.ConcatenateString(res[0], res[1]),dropLines);
			}

			//now binder
			compiler = "gnatbind";
			args = idata.BaseDir + "source";
			res = Engine.CallCompiler(compiler, args, out compileTime);
			cdata.CompileTimeMs += compileTime;
			if(!File.Exists(idata.BaseDir+"b~source.adb"))
			{
				if(res.Count > 1)
				{
					cdata.Error = "Binder:\n"+Utils.RemoveSomeLines(Utils.ConcatenateString(res[0], res[1]), dropLines);
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
				cdata.Warning += "Binder:\n";
				cdata.Warning = Utils.RemoveSomeLines(Utils.ConcatenateString(res[0], res[1]), dropLines);
			}

			//now linker
			compiler = "gnatlink";
			args = idata.BaseDir + "source";
			res = Engine.CallCompiler(compiler, args, out compileTime);
			cdata.CompileTimeMs += compileTime;
			if(!File.Exists(idata.BaseDir+"source"))
			{
				if(res.Count > 1)
				{
					cdata.Error = "Linker:\n"+Utils.RemoveSomeLines(Utils.ConcatenateString(res[0], res[1]), dropLines);
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
				cdata.Warning = Utils.RemoveSomeLines(Utils.ConcatenateString(res[0], res[1]), dropLines);
			}

			cdata.ExecuteThis = idata.BaseDir+"source";
			cdata.Executor = "";
			cdata.Success = true;
			return cdata;
		}
		#endregion
	}
}

