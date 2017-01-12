using System;
using ExecutionEngine;

namespace runPythonWrapper
{
	public class PerlHello : ITest
	{
		public void Do()
		{
			var tp = new TestProgram()
			{
				Name = "Perl_Hello",
				Lang = Languages.Perl,
				Program = @"print ""Hello World"";"
			};

			var res = Logic.TestProgram (tp);

			if (!string.IsNullOrEmpty (res.Warnings) || !string.IsNullOrEmpty (res.Errors)) 
			{
				throw new Exception ("warnings or errors not null");
			}

			if (string.IsNullOrEmpty (res.Output) || res.Output != "Hello World") 
			{
				throw new Exception ("output is wrong");
			}
		}

		public string GetName()
		{
			return "Perl Hello";
		}
	}
}

