using System;
using ExecutionEngine;

namespace runPythonWrapper
{
	public class HaskellHello : ITest
	{
		public void Do()
		{
			var tp = new TestProgram()
			{
				Name = "Haskell_Hello",
				Lang = Languages.Haskell,
				Program = @"main = print $ ""Hello, world!""",
				Args = "-o a.out source_file.hs"
			};

			var res = Logic.TestProgram (tp);

			if (!string.IsNullOrEmpty (res.Warnings) || !string.IsNullOrEmpty (res.Errors)) 
			{
				throw new Exception ("warnings or errors not null");
			}

			if (string.IsNullOrEmpty (res.Output) || res.Output != "\"Hello, world!\"\n") 
			{
				throw new Exception ("output is wrong");
			}
		}

		public string GetName()
		{
			return "Haskell Hello";
		}
	}
}

