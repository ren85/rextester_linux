using System;

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
			cdata.ExecuteThis = idata.PathToSource;
			cdata.Executor = "/home/ren/.sdkman/candidates/kotlin/current/bin/kotlinc -script ";
			cdata.Success = true;
			return cdata;
		}
		#endregion
	}
}

