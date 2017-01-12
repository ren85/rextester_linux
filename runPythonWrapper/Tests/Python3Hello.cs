using System;
using ExecutionEngine;

namespace runPythonWrapper
{
	public class Python3Hello : ITest
	{
		public void Do()
		{
			var tp = new TestProgram()
			{
				Name = "Python3_Hello",
				Lang = Languages.Python3,
				Program = @"print(""Hello, world!"");"
			};

			var res = Logic.TestProgram (tp);

			if (!string.IsNullOrEmpty (res.Warnings) || !string.IsNullOrEmpty (res.Errors)) 
			{
				throw new Exception ("warnings or errors not null");
			}

			if (string.IsNullOrEmpty (res.Output) || res.Output != "Hello, world!\n") 
			{
				throw new Exception ("output is wrong");
			}
		}

		public string GetName()
		{
			return "Python3 Hello";
		}
	}
}

