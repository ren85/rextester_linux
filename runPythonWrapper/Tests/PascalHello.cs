using System;
using ExecutionEngine;

namespace runPythonWrapper
{
	public class PascalHello : ITest
	{
		public void Do()
		{
			var tp = new TestProgram()
			{
				Name = "Pascal_Hello",
				Lang = Languages.Pascal,
				Program = @"//fpc 2.6.2

program HelloWorld;

begin
    writeln('Hello, world!');
end.
"
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
			return "Pascal Hello";
		}
	}
}

