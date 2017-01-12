using System;
using ExecutionEngine;

namespace runPythonWrapper
{
	public class PrologHello : ITest
	{
		public void Do()
		{
			var tp = new TestProgram()
			{
				Name = "Prolog_Hello",
				Lang = Languages.Prolog,
				Program = @"program :- write('Hello, world!').
:- program.",
				Input="halt()."
			};

			var res = Logic.TestProgram (tp);

			if (!string.IsNullOrEmpty (res.Warnings) || !string.IsNullOrEmpty (res.Errors)) 
			{
				throw new Exception ("warnings or errors not null");
			}

			if (string.IsNullOrEmpty (res.Output) || res.Output != "Hello, world!") 
			{
				throw new Exception ("output is wrong");
			}
		}

		public string GetName()
		{
			return "Prolog Hello";
		}
	}
}

