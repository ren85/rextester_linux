using System;
using ExecutionEngine;

namespace runPythonWrapper
{
	public class GoHello : ITest
	{
		public void Do()
		{
			var tp = new TestProgram()
			{
				Name = "Go_Hello",
				Lang = Languages.Go,
				Program = @"package main  
import ""fmt"" 

func main() { 
    fmt.Printf(""Hello, world!"") 
}",
				Args = "-o a.out source_file.go"
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
			return "Go Hello";
		}
	}
}

