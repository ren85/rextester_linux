using System;
using ExecutionEngine;

namespace runPythonWrapper
{
	public class RHello : ITest
	{
		public void Do()
		{
			var tp = new TestProgram()
			{
				Name = "R_Hello",
				Lang = Languages.R,
				Program = @"print(""Hello, world!"")"
			};

			var res = Logic.TestProgram (tp);

			if (!string.IsNullOrEmpty (res.Warnings) || !string.IsNullOrEmpty (res.Errors)) 
			{
				throw new Exception ("warnings or errors not null");
			}

			if (string.IsNullOrEmpty (res.Output) || res.Output != "[1] \"Hello, world!\"\n") 
			{
				throw new Exception ("output is wrong");
			}
		}

		public string GetName()
		{
			return "R Hello";
		}
	}
}

