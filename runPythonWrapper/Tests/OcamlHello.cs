using System;
using ExecutionEngine;

namespace runPythonWrapper
{
	public class OcamlHello : ITest
	{
		public void Do()
		{
			var tp = new TestProgram()
			{
				Name = "Ocaml_Hello",
				Lang = Languages.Ocaml,
				Program = @"print_string ""Hello world!\n"";;"
			};

			var res = Logic.TestProgram (tp);

			if (!string.IsNullOrEmpty (res.Warnings) || !string.IsNullOrEmpty (res.Errors)) 
			{
				throw new Exception ("warnings or errors not null");
			}

			if (string.IsNullOrEmpty (res.Output) || res.Output != "Hello world!\n") 
			{
				throw new Exception ("output is wrong");
			}
		}

		public string GetName()
		{
			return "Ocaml Hello";
		}
	}
}

