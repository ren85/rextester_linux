using System;
using ExecutionEngine;

namespace runPythonWrapper
{
	public class CClangHello : ITest
	{
		public void Do()
		{
			var tp = new TestProgram()
			{
				Name = "CClang_Hello",
				Lang = Languages.CClang,
				Program = @"#include  <stdio.h>

int main(void)
{
    printf(""Hello, world!"");
    return 0;
}",
				Args = "-Wall -std=gnu99 -O2 -o a.out source_file.c"
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
			return "CClang Hello";
		}
	}
}

