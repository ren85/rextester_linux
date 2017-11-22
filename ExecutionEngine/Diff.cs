using System;
using System.IO;
using System.Diagnostics;
using System.Threading;

namespace ExecutionEngine
{
	public class Diff
	{
		public Diff ()
		{
		}
		
		static string ParentRootPath
		{
			get
			{
				//return @"/home/ren/Desktop/rextester/RextesterService/";
				return @"/var/www/service/";
			}
		}
		public static DiffResult GetDiff(string left, string right)
		{
			string diff_dir = "";
			try
			{
				int r = Utils.GetUniqueRandom().Next(1, Int32.MaxValue);
				diff_dir = ParentRootPath+"diff/"+r+"/";	
				Directory.CreateDirectory(diff_dir);
				using(TextWriter tw = new StreamWriter(diff_dir+"left"))
				{
					tw.Write(left);					
				}
				using(TextWriter tw = new StreamWriter(diff_dir+"right"))
				{
					tw.Write(right);					
				}
				using(Process process = new Process())
				{
					process.StartInfo.FileName = ParentRootPath+"codediff.py";
					process.StartInfo.Arguments = diff_dir+"left "+diff_dir+"right "+"-o "+diff_dir+"result "+"-w 80";
					process.StartInfo.UseShellExecute = false;
					process.StartInfo.CreateNoWindow = true;
					process.StartInfo.RedirectStandardError = true;
					process.StartInfo.RedirectStandardOutput = true;
					
					process.Start();

	                OutputReader error = new OutputReader(process.StandardError);
	                Thread errorReader = new Thread(new ThreadStart(error.ReadOutput));
	                errorReader.Start();
					
					process.WaitForExit(20000);
					if(!process.HasExited)
						process.Kill();
					
					errorReader.Join(5000);
					
					string result = "";
					if(string.IsNullOrEmpty(error.Output))
						using(TextReader tr = new StreamReader(diff_dir+"result"))
						{
							result = tr.ReadToEnd();
						}					

					
					if(!string.IsNullOrEmpty(error.Output))
					{
						return new DiffResult() { IsError = true, Result = error.Output };
					}

					return new DiffResult() { Result = result };
				}
			}
			catch(Exception e)
			{
				return new DiffResult() { IsError = true, Result = e.Message };				
			}
			finally
			{
				try
				{
					//cleanup
					Directory.Delete(diff_dir, true);
				}
				catch(Exception){}			
			}
		}
	}
	
	public class DiffResult
	{
		public bool IsError
		{
			get;
			set;
		}
		
		public string Result
		{
			get;
			set;
		}
	}
}

