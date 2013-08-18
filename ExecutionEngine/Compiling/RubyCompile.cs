using System;

namespace ExecutionEngine
{
	public class RubyCompile : ICompiler
	{
		public RubyCompile ()
		{
		}

		#region ICompiler implementation
		public CompilerData Compile (InputData idata, CompilerData cdata)
		{
			cdata.ExecuteThis = idata.PathToSource;
			cdata.Executor = "ruby -w -W1 ";
			cdata.Success = true;
			return cdata;
		}
		#endregion
	}
}

