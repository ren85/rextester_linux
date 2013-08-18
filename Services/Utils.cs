using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using System.Diagnostics;
using System.Linq;

namespace Services
{
	public class Utils
	{
		public static string ParentRootPath
		{
			get
			{
				return @"/var/www/service/";
			}
		}

        private static readonly Random globalRandom = new Random();
        private static readonly object globalLock = new object(); 
		
		public static Random GetUniqueRandom()
        {
            lock (globalLock)
            {
                return new Random(globalRandom.Next());
            }
        }
		
		public static string RandomString()
		{
			Random rg = Utils.GetUniqueRandom();
			return rg.Next(1, Int32.MaxValue).ToString();
		}

		public static List<string> CallProgram(string program_full_path, string args)
		{
			try
			{
				using(Process process = new Process())
				{
					process.StartInfo.FileName = program_full_path;
					process.StartInfo.Arguments = args;
					process.StartInfo.UseShellExecute = false;
					process.StartInfo.CreateNoWindow = true;
					process.StartInfo.RedirectStandardError = true;
					process.StartInfo.RedirectStandardOutput = true;				
					process.Start();

					OutputReader output = new OutputReader(process.StandardOutput, 100);
	                Thread outputReader = new Thread(new ThreadStart(output.ReadOutput));
	                outputReader.Start();
	               
					process.WaitForExit();
					outputReader.Join(1000);

					var res =  output.Output.Split("\n\r".ToCharArray(), StringSplitOptions.RemoveEmptyEntries).ToList<string>();		
					return res;
				}
			}
			catch(Exception)
			{
				return new List<string>() {};
			}
		}	
	}

	class OutputReader
    {
        StreamReader reader;
        public string Output
        {
            get;
            set;
        }
        StringBuilder sb = new StringBuilder();
        public StringBuilder Builder
        {
            get
            {
                return sb;
            }
        }
        public OutputReader(StreamReader reader, int interval = 10)
        {
            this.reader = reader;
			this.CheckInterval = interval;
        }

		int CheckInterval
		{
			get;
			set;
		}
		
        public void ReadOutput()
        {
            try
            {                
                int bufferSize = 40000;
                byte[] buffer = new byte[bufferSize];
                int outputLimit = 200000;
                int count;
                bool addMore = true;
                while (true)
                {
                    Thread.Sleep(CheckInterval);
                    count = reader.BaseStream.Read(buffer, 0, bufferSize);
                    if (count != 0)
                    {
                        if (addMore)
                        {
                            sb.Append(Encoding.UTF8.GetString(buffer, 0, count));
                            if (sb.Length > outputLimit)
                            {
                                addMore = false;
                            }
                        }
                    }
                    else
                        break;
                }
                Output = sb.ToString();
            }
            catch (Exception)
            {}
        }
	}
}

