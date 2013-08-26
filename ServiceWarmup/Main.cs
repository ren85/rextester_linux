using System;
using System.Threading;
using ServiceWarmup;
using System.Diagnostics;
using System.IO;

namespace ServiceWarmup
{
	class MainClass
	{
		static string Path
		{
			get
			{
				return "/tmp/excercise/";
			}
		}
		static int SleepIntervalInMilliseconds
		{
			get
			{
				return 20000;
			}
		}
		static int Tries
		{
			get
			{
				return 3000;
			}
		}
		static int Count
		{
			get;
			set;
		}
		static int WholeCount
		{
			get;
			set;
		}
		static int Fails
		{
			get;
			set;
		}
		static double TotalMilliseconds
		{
			get;
			set;
		}
		public static void Main (string[] args)
		{
			Stopwatch watch = new Stopwatch();
			while(true)
			{
				Thread.Sleep(SleepIntervalInMilliseconds);
				var run = RunFactory.Next();				
				using(var service = new n178_238_226_7.Service())
				{					 
					try
					{
						watch.Reset();
						watch.Start();
						var res = service.DoWork(run.Code, null, run.Language, GlobalUtils.TopSecret.ServiceUser, GlobalUtils.TopSecret.ServicePass, run.CompilerArgs, false, false, false);
						watch.Stop();
						TotalMilliseconds += watch.ElapsedMilliseconds;
						Count++;
						if(res.Output == null || res.Output.TrimEnd() != run.Output)
						{
							Fails++;
						}
					}
					catch(Exception)
					{
						Fails++;
					}
				}
				WholeCount++;
				if(WholeCount == Tries)
				{					
					int avgInMilliseconds = (Count != 0 ? (int)Math.Round(TotalMilliseconds / (double)Count, 0) : -1);
					string info = string.Format("{0}-{1}-{2}from{3}", DateTime.Now.ToString("yyyy_MM_dd__HH_mm_ss"), avgInMilliseconds, Fails, Tries);
					try
					{
						File.Create(Path+info);	
					}
					catch(Exception)
					{}
					WholeCount = 0;
					Count = 0;
					Fails = 0;
					TotalMilliseconds = 0;
				}
			}
		}
	}
}
