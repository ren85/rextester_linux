using System;
using ExecutionEngine;

namespace runPythonWrapper
{
	public class CPPClangHello : ITest
	{
		public void Do()
		{
			var tp = new TestProgram()
			{
				Name = "CPPClang_Hello",
				Lang = Languages.CppClang,
				Program = @"#include <iostream>

int main()
{
    std::cout << ""Hello, world!"";
}",
				Args = "-Wall -std=c++14 -stdlib=libc++ -O2 -o a.out source_file.cpp"
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
			return "CPPClang Hello";
		}
	}
}

