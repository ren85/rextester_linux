using System;
using ExecutionEngine;
using System.Diagnostics;

namespace runPythonWrapper
{
	public class Logic
	{
		public static OutputData TestProgram(TestProgram tp)
		{			
			bool directly = false;
			if (directly) 
			{
				return TestEngineDirectly (tp.Program, tp.Input, tp.Lang, tp.Args);
			}
			else
			{
				return TestEngineThroughService (tp.Program, tp.Input, tp.Lang, tp.Args);
			}
		}
		static OutputData TestEngineDirectly(string Program, string Input, Languages Lang, string Args)
		{
			Engine engine = new Engine();
			InputData idata = new InputData()
			{
				Program = Program,
				Input = Input,
				Lang = Lang,
				Compiler_args = Args
			};
			return engine.DoWork(idata);		
		}

		static OutputData TestEngineThroughService(string Program, string Input, Languages Lang, string Args)
		{
			OutputData odata;
			bool bytes = true;

			using(var s = new runPythonWrapper.api.rextester.com.Service())
			{

				Stopwatch watch = new Stopwatch();
				watch.Start();
				runPythonWrapper.api.rextester.com.Result res = s.DoWork(Program, Input, (runPythonWrapper.api.rextester.com.Languages)Lang, GlobalUtils.TopSecret.ServiceUser, GlobalUtils.TopSecret.ServicePass, Args, bytes, false, false);	

				watch.Stop();
				if(res != null)
				{
					if(string.IsNullOrEmpty(res.Stats))
						res.Stats = "";
					else
						res.Stats += ", ";
					res.Stats += string.Format("absolute service time: {0} sec", Math.Round((double)watch.ElapsedMilliseconds/(double)1000, 2));
				}

				odata = new OutputData()
				{
					Errors = res.Errors,
					Warnings = res.Warnings,
					Stats = res.Stats,
					Output = res.Output,
					Exit_Status = res.Exit_Status,
					System_Error = res.System_Error
				};
				if(bytes)
				{
					if(res.Errors_Bytes != null)
						odata.Errors = System.Text.Encoding.Unicode.GetString(res.Errors_Bytes);
					if(res.Warnings_Bytes != null)
						odata.Warnings = System.Text.Encoding.Unicode.GetString(res.Warnings_Bytes);
					if(res.Output_Bytes != null)
						odata.Output = System.Text.Encoding.Unicode.GetString(res.Output_Bytes);
				}
			}
			return odata;
		}

		static void ShowData(OutputData odata)
		{
			if(!string.IsNullOrEmpty(odata.System_Error))
			{
				Console.WriteLine("System error:");
				Console.WriteLine(odata.System_Error);				
			}
			else
			{
				Console.WriteLine("Errors:");
				Console.WriteLine(odata.Errors);
				Console.WriteLine("Warnings:");
				Console.WriteLine(odata.Warnings);
				Console.WriteLine("Output:");	
				Console.WriteLine(odata.Output);		
				Console.WriteLine("Exit status:");	
				Console.WriteLine(odata.Exit_Status);	
				Console.WriteLine("Stats:");	
				Console.WriteLine(odata.Stats);	
			}
			Console.ReadLine();
		}
	}

	public class TestProgram
	{
		public string Name
		{
			get;
			set;
		}
		public string Program
		{
			get;
			set;
		}
		public string Input
		{
			get;
			set;				
		}
		public Languages Lang
		{
			get;
			set;
		}
		public string ShouldContain
		{
			get;
			set;
		}
		public string Args
		{
			get;
			set;
		}
	}

}

