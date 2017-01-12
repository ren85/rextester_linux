using System;
using ExecutionEngine;

namespace runPythonWrapper
{
	public class PhpHello : ITest
	{
		public void Do()
		{
			var tp = new TestProgram()
			{
				Name = "Php_Hello",
				Lang = Languages.Php,
				Program = @"<?php //php 5.5.9

    echo ""Hello, world!""
    
?>"
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
			return "Php Hello";
		}
	}
}

