﻿using System;
using ExecutionEngine;

namespace runPythonWrapper
{
	public class FHello : ITest
	{
		public void Do()
		{
			var tp = new TestProgram()
			{
				Name = "F_Hello",
				Lang = Languages.FSharp,
				Program = @"open System
printfn ""Hello, world!"""
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
			return "F sharp Hello";
		}
	}
}

