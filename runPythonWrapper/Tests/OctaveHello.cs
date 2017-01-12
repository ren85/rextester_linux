using System;
using ExecutionEngine;

namespace runPythonWrapper
{
	public class OctaveHello : ITest
	{
		public void Do()
		{
			var tp = new TestProgram()
			{
				Name = "Octave_Hello",
				Lang = Languages.Octave,
				Program = @"x=""Hello, world!"";
x"
			};

			var res = Logic.TestProgram (tp);

			if (!string.IsNullOrEmpty (res.Warnings) || !string.IsNullOrEmpty (res.Errors)) 
			{
				throw new Exception ("warnings or errors not null");
			}

			if (string.IsNullOrEmpty (res.Output) || res.Output != "x = Hello, world!\n") 
			{
				throw new Exception ("output is wrong");
			}
		}

		public string GetName()
		{
			return "Octave Hello";
		}
	}
}

