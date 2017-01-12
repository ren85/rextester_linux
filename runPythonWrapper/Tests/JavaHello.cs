using System;
using ExecutionEngine;

namespace runPythonWrapper
{
	public class JavaHello : ITest
	{
		public void Do()
		{
			var tp = new TestProgram()
			{
				Name = "Java_Hello",
				Lang = Languages.Java,
				Program = @"//Title of this code
//'main' method must be in a class 'Rextester'.

import java.util.*;
import java.lang.*;
import java.nio.charset.*;

class Rextester
{  
    public static void main(String args[])
    {
        System.out.println(""ėšęįųšįęė"");
    }
    
}"
			};

			var res = Logic.TestProgram (tp);

			if (!string.IsNullOrEmpty (res.Warnings) || !string.IsNullOrEmpty (res.Errors)) 
			{
				throw new Exception ("warnings or errors not null");
			}

			if (string.IsNullOrEmpty (res.Output) || res.Output != "ėšęįųšįęė\n") 
			{
				throw new Exception ("output is wrong");
			}
		}

		public string GetName()
		{
			return "Java Hello";
		}
	}
}

