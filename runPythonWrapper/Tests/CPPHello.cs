using System;
using ExecutionEngine;

namespace runPythonWrapper
{
	public class CPPHello : ITest
	{
		public void Do()
		{
			var tp = new TestProgram()
			{
				Name = "CPP_Hello",
				Lang = Languages.CPP,
				Program = @"#include <iostream>

int main()
{
    std::cout << ""Hello, world!"";
}",
				Args = "-Wall -std=c++14 -O2 -o a.out source_file.cpp"
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
			return "CPP Hello";
		}
	}
}

