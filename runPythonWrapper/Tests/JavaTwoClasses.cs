using System;
using ExecutionEngine;

namespace runPythonWrapper
{
	public class JavaTwoClasses: ITest
	{
		public void Do()
		{
			var tp = new TestProgram()
			{
				Name = "Java_2classes",
				Lang = Languages.Java,
				Program = @"
class Rextester
{
   public static void main(String args[]) {
       System.out.println(new Foo());
       (new java.util.Date()).getDate();
   }

}

class Foo {

   public String toString() {
       String.format(""Foo!"");
   }

}"
			};

			var res = Logic.TestProgram (tp);

			if (!string.IsNullOrEmpty (res.Warnings)) 
			{
				throw new Exception ("warnings null");
			}

			if (string.IsNullOrEmpty (res.Errors) || !res.Errors.Contains("missing return statement")) 
			{
				throw new Exception ("error is wrong");
			}
		}

		public string GetName()
		{
			return "Java 2 classes";
		}
	}
}
