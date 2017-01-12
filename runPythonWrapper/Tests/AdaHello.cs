using System;
using ExecutionEngine;

namespace runPythonWrapper
{
	public class AdaHello : ITest
	{
		public void Do()
		{
			var tp = new TestProgram()
			{
				Name = "Ada_Hello",
				Lang = Languages.Ada,
				Program = @"with Ada.Text_IO; use Ada.Text_IO;
    procedure Hello is
    begin
       Put_Line (""Hello, world!"");
    end Hello;"
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
			return "Ada Hello";
		}
	}
}

