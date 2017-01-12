using System;
using ExecutionEngine;

namespace runPythonWrapper
{
	public class SchemeHello : ITest
	{
		public void Do()
		{
			var tp = new TestProgram()
			{
				Name = "Scheme_Hello",
				Lang = Languages.Scheme,
				Program = @"(display ""Hello, World!"")"
			};

			var res = Logic.TestProgram (tp);

			if (!string.IsNullOrEmpty (res.Warnings) || !string.IsNullOrEmpty (res.Errors)) 
			{
				throw new Exception ("warnings or errors not null");
			}

			if (string.IsNullOrEmpty (res.Output) || res.Output != "Hello, World!") 
			{
				throw new Exception ("output is wrong");
			}
		}

		public string GetName()
		{
			return "Scheme Hello";
		}
	}
}

