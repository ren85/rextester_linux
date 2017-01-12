using System;
using ExecutionEngine;

namespace runPythonWrapper
{
	public class ScalaHello : ITest
	{
		public void Do()
		{
			var tp = new TestProgram()
			{
				Name = "Scala_Hello",
				Lang = Languages.Scala,
				Program = @"object Rextester extends App {
    println(""Hello, World!"")
 }"
			};

			var res = Logic.TestProgram (tp);

			if (!string.IsNullOrEmpty (res.Warnings) || !string.IsNullOrEmpty (res.Errors)) 
			{
				throw new Exception ("warnings or errors not null");
			}

			if (string.IsNullOrEmpty (res.Output) || res.Output != "Hello, World!\n") 
			{
				throw new Exception ("output is wrong");
			}
		}

		public string GetName()
		{
			return "Scala Hello";
		}
	}
}

