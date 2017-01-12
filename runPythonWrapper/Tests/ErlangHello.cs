using System;
using ExecutionEngine;

namespace runPythonWrapper
{
	public class ErlangHello : ITest
	{
		public void Do()
		{
			var tp = new TestProgram()
			{
				Name = "Erlang_Hello",
				Lang = Languages.Erlang,
				Program = @"
-module(source).
-export([entry_point/0]).

entry_point() ->
	io:fwrite(""Hello, world!"")."
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
			return "Erlang Hello";
		}
	}
}

