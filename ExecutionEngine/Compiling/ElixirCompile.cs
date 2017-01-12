using System;
using System.IO;

namespace ExecutionEngine
{
	public class ElixirCompile: ICompiler
	{
		public ElixirCompile () 
		{
		}

		#region ICompiler implementation
		public CompilerData Compile (InputData idata, CompilerData cdata)
		{	
			Directory.SetCurrentDirectory (idata.BaseDir);
			cdata.ExecuteThis = idata.PathToSource;
			cdata.Executor = "elixir";
			cdata.Success = true;
			return cdata;
		}
		#endregion
	}
}

