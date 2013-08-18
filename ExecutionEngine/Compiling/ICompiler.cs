using System;

namespace ExecutionEngine
{
	public interface ICompiler
	{
		CompilerData Compile(InputData idata, CompilerData cdata);			
	}
}

