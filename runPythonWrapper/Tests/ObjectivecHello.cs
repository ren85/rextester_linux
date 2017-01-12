using System;
using ExecutionEngine;

namespace runPythonWrapper
{
	public class ObjectivecHello : ITest
	{
		public void Do()
		{
			var tp = new TestProgram()
			{
				Name = "Objectivec_Hello",
				Lang = Languages.ObjectiveC,
				Program = @"//gcc 4.8.4

#import <stdio.h>
 
int main(void)
{
    printf(""Hello, world!"");
    return 0;
}
",
				Args = "-MMD -MP -DGNUSTEP -DGNUSTEP_BASE_LIBRARY=1 -DGNU_GUI_LIBRARY=1 -DGNU_RUNTIME=1 -DGNUSTEP_BASE_LIBRARY=1 -fno-strict-aliasing -fexceptions -fobjc-exceptions -D_NATIVE_OBJC_EXCEPTIONS -pthread -fPIC -Wall -DGSWARN -DGSDIAGNOSE -Wno-import -g -O2 -fgnu-runtime -fconstant-string-class=NSConstantString -I. -I /usr/include/GNUstep -I/usr/include/GNUstep -o a.out source_file.m -lobjc -lgnustep-base"
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
			return "Objectivec Hello";
		}
	}
}

