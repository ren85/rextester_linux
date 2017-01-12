using System;
using ExecutionEngine;

namespace runPythonWrapper
{
	public class DHello : ITest
	{
		public void Do()
		{
			var tp = new TestProgram()
			{
				Name = "D_Hello",
				Lang = Languages.D,
				Program = @"import std.stdio;
 
void main()
{
    writeln(""Hello, World!"");
}",
				Args = "source_file.d -ofa.out"
			};

			var res = Logic.TestProgram (tp);

			if (!string.IsNullOrEmpty (res.Warnings) || !string.IsNullOrEmpty (res.Errors)) 
			{
				throw new Exception ("warnings or errors not null");
			}

			if (string.IsNullOrEmpty (res.Output) || res.Output != "Hello, World!\n") 
			{
				throw new Exception ("output is wrong");
			}
		}

		public string GetName()
		{
			return "D Hello";
		}
	}
}

