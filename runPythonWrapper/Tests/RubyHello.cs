using System;
using ExecutionEngine;

namespace runPythonWrapper
{
	public class RubyHello : ITest
	{
		public void Do()
		{
			var tp = new TestProgram()
			{
				Name = "Ruby_Hello",
				Lang = Languages.Ruby,
				Program = @"puts ""Hello, world!"""
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
			return "Ruby Hello";
		}
	}
}

