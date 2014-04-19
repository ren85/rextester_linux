using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading;
using System.Collections.Generic;
using System.IO.Compression;
using System.Linq;


namespace ExecutionEngine
{
	public class InputData
	{
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
		
		public string BaseDir
		{
			get;
			set;
		}
		
		public string PathToSource
		{
			get;
			set;
		}

		public string Rand
		{
			get;
			set;
		}
		public string Compiler_args
		{
			get;
			set;
		}
	}
	
	public class OutputData
	{
		public string Output
		{
			get;
			set;			
		}
		public string Errors
		{
			get;
			set;			
		}
		public string Warnings
		{
			get;
			set;
		}
		public string Stats
		{
			get;
			set;
		}	
		public string Exit_Status
		{
			get;
			set;
		}
		public int ExitCode
		{
			get;
			set;
		}
		public string System_Error
		{
			get;
			set;
		}
		public List<byte[]> Files
		{
			get;
			set;
		}
	}
	public enum Languages
	{
		Java,
		Python,
		C,
		CPP,
		Php,
		Pascal,
		ObjectiveC,
		Haskell,
		Ruby,
		Perl,
		Lua,
		Nasm,
		Javascript,
		Lisp,
		Prolog,
		Go,
		Scala,
		Scheme,
		Nodejs,
		Python3,
		Octave,
		CClang,
		CppClang,
		D
	}	
	
	public class CompilerData
	{
		public bool Success
		{
			get;
			set;
		}
		public string Warning
		{
			get;
			set;			
		}
		public string Error
		{
			get;
			set;
		}
		public string Executor
		{
			get;
			set;			
		}			
		public string ExecuteThis
		{
			get;
			set;
		}
		public string CleanThis
		{
			get;
			set;			
		}
		public long CompileTimeMs
		{
			get;
			set;
		}
	}
	
	public class Engine
	{
		public Engine ()
		{}
		
		public static string RootPath
		{
			get
			{
				//return @"/home/ren/Desktop/rextester_linux/RextesterService/usercode/";				
				return @"/var/www/service/usercode/"; 
			}
		}
		
		static string ParentRootPath
		{
			get
			{
				//return @"/home/ren/Desktop/rextester_linux/RextesterService/";
				return @"/var/www/service/";
			}
		}
		
		double CpuTimeInSec
		{
			get;
			set;
		}
		int MemoryPickInKilobytes
		{
			get;
			set;
		}
		public OutputData DoWork(InputData idata)
		{
			CompilerData cdata = null;
			try
			{				
				OutputData odata = new OutputData();	
				cdata = CreateExecutable(idata);				
				if(!cdata.Success)
				{
					odata.Errors = cdata.Error;
					odata.Warnings = cdata.Warning;
					odata.Stats = string.Format("Compilation time: {0} sec", Math.Round((double)cdata.CompileTimeMs/(double)1000, 2));
					return odata;
				}
				if(!string.IsNullOrEmpty(cdata.Warning))
				{
					odata.Warnings = cdata.Warning;
				}
				
				Stopwatch watch = new Stopwatch();
				watch.Start();
				using(Process process = new Process())
				{
					process.StartInfo.FileName = ParentRootPath+"parent.py";
					process.StartInfo.Arguments = cdata.Executor+(string.IsNullOrEmpty(cdata.Executor) ? "" : " ")+cdata.ExecuteThis;
					process.StartInfo.UseShellExecute = false;
					process.StartInfo.CreateNoWindow = true;
					process.StartInfo.RedirectStandardError = true;
					process.StartInfo.RedirectStandardOutput = true;
					process.StartInfo.RedirectStandardInput = true;
					
					process.Start();
					
                    if (!string.IsNullOrEmpty(idata.Input))
                    {
                        InputWriter input = new InputWriter(process.StandardInput, idata.Input);
                        Thread inputWriter = new Thread(new ThreadStart(input.Writeinput));
                        inputWriter.Start();
                    }
					
					OutputReader output = new OutputReader(process.StandardOutput);
	                Thread outputReader = new Thread(new ThreadStart(output.ReadOutput));
	                outputReader.Start();
	                OutputReader error = new OutputReader(process.StandardError);
	                Thread errorReader = new Thread(new ThreadStart(error.ReadOutput));
	                errorReader.Start();
					
					process.WaitForExit();
					
					errorReader.Join(5000);
	                outputReader.Join(5000);
					
					if(!string.IsNullOrEmpty(error.Output))
					{
						int index = error.Output.LastIndexOf('\n');
						int exitcode;
						if(index != -1 && index+1 < error.Output.Length)						
						{
							string[] info = error.Output.Substring(index+1).Split(" ".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
							if(info.Length > 0 && Int32.TryParse(info[0], out exitcode))
							{
								odata.ExitCode = exitcode;
								switch(exitcode)
								{
									case -8:
										odata.Exit_Status = "Floating point exception (SIGFPE)";
										break;
									case -9:
										odata.Exit_Status = "Kill signal (SIGKILL)";
										break;
									case -11:
										odata.Exit_Status = "Invalid memory reference (SIGSEGV)";
										break;
									case -6:
										odata.Exit_Status = "Abort signal from abort(3) (SIGABRT)";
										break;
									case -4:
										odata.Exit_Status = "Illegal instruction (SIGILL)";
										break;
									case -13:
										odata.Exit_Status = "Broken pipe: write to pipe with no readers (SIGPIPE)";
										break;
									case -14:
										odata.Exit_Status = "Timer signal from alarm(2) (SIGALRM)";
										break;	
									case -15:
										odata.Exit_Status = "Termination signal (SIGTERM)";
										break;
									case -19:
										odata.Exit_Status = "Stop process (SIGSTOP)";
										break;	
									case -17:
										odata.Exit_Status = "Child stopped or terminated (SIGCHLD)";
										break;
									default:
										odata.Exit_Status = string.Format("Exit code: {0} (see 'man 7 signal' for explanation)", exitcode);
										break;
								}
								
								error.Output = error.Output.Substring(0, index);
								if(info.Length > 1)
								{
									double cpuTime;
									Double.TryParse(info[1], out cpuTime);
									CpuTimeInSec = cpuTime;
								}								
								if(info.Length > 2)
								{
									int memory;
									Int32.TryParse(info[2], out memory);
									MemoryPickInKilobytes = memory;
								}
							}
							
						}
					}					
					odata.Errors = error.Output;
					odata.Output = output.Output;	
					if(idata.Lang == Languages.Octave)
					{
						string bad_err = "error: No such file or directory"+Environment.NewLine+"error: ignoring octave_execution_exception while preparing to exit";
						if(odata.Errors != null && odata.Errors.Contains(bad_err))
						{
							odata.Errors = odata.Errors.Replace(bad_err, "");
							if(string.IsNullOrEmpty(odata.Errors.Trim()))
							{
								odata.Errors = null;
							}
						}
						List<FileData> files = new List<FileData>();
						foreach (string file_name in Directory.GetFiles(cdata.CleanThis, "*.png"))
						{ 
							var file = new FileData();
							file.Data = File.ReadAllBytes(file_name);
							file.CreationDate = File.GetCreationTime(file_name);
							files.Add(file);
						}
						odata.Files = files.OrderBy(f => f.CreationDate).Select(f => f.Data).ToList();
					}
				}
				watch.Stop();

				if(Utils.IsCompiled(idata.Lang))
				{
					odata.Stats = string.Format("Compilation time: {0} sec, absolute running time: {1} sec, cpu time: {2} sec, memory peak: {3} Mb", Math.Round((double)cdata.CompileTimeMs / (double)1000, 2), Math.Round((double)watch.ElapsedMilliseconds / (double)1000, 2), Math.Round(CpuTimeInSec, 2), MemoryPickInKilobytes/1024);
				}
				else
				{
					odata.Stats = string.Format("Absolute running time: {0} sec, cpu time: {1} sec, memory peak: {2} Mb", Math.Round((double)watch.ElapsedMilliseconds / (double)1000, 2), Math.Round(CpuTimeInSec, 2), MemoryPickInKilobytes/1024);
				}
				return odata;
			}
			catch(Exception ex)
			{
				return new OutputData()
					{
						System_Error = ex.Message
					};
			}
			finally
			{
				if(cdata != null)
					Cleanup(cdata.CleanThis);
			}
		}

		class FileData
		{
			public byte[] Data {get; set;}
			public DateTime CreationDate {get; set;}
		}
	
		private void Cleanup(string dir)
		{
			
			try
			{
				//cleanup
				Directory.Delete(dir, true);
			}
			catch(Exception){}
		}
		CompilerData CreateExecutable(InputData input)
		{
			string ext = "";
			string rand = Utils.RandomString();
			string dir = rand + "/";
			switch(input.Lang)
			{
				case Languages.Java:
					ext = ".java";
					break;
				case Languages.Python:
					ext = ".py";
					break;
				case Languages.C:
					ext = ".c";
					break;	
				case Languages.CPP:
					ext = ".cpp";
					break;
				case Languages.Php:
					ext = ".php";
					break;
				case Languages.Pascal:
					ext = ".pas";
					break;
				case Languages.ObjectiveC:
					ext = ".m";
					break;
				case Languages.Haskell:
					ext = ".hs";
					break;
				case Languages.Ruby:
					ext = ".rb";
					break;
				case Languages.Perl:
					ext = ".pl";
					break;
				case Languages.Lua:
					ext = ".lua";
					break;
				case Languages.Nasm:
					ext = ".asm";
					break;
				case Languages.Javascript:
					ext = ".js";
					break;
				case Languages.Lisp:
					ext = ".lsp";
					break;
				case Languages.Prolog:
					ext = ".prolog";
					break;
				case Languages.Go:
					ext = ".go";
					break;
				case Languages.Scala:
					ext = ".scala";
					break;
				case Languages.Scheme:
					ext = ".scm";
					break;
				case Languages.Nodejs:
					ext = ".js";
					break;
				case Languages.Python3:
					ext = ".py";
					break;
				case Languages.Octave:
					ext = ".m";
					break;
				case Languages.CClang:
					ext = ".c";
					break;
				case Languages.CppClang:
					ext = ".cpp";
					break;
				case Languages.D:
					ext = ".d";
					break;
				default:
					ext = ".unknown";
					break;
			}
			string PathToSource = RootPath+dir+/*rand*/"source"+ext;
			input.PathToSource = PathToSource;
			input.BaseDir = RootPath+dir;
			input.Rand = rand;
			Directory.CreateDirectory(RootPath+dir);
			using(TextWriter sw = new StreamWriter(PathToSource))
			{
				sw.Write(input.Program);				
			}
			CompilerData cdata = new CompilerData();
			cdata.CleanThis = RootPath+dir;
			
			var comp = ICompilerFactory.GetICompiler(input.Lang);
			if(comp != null)
				return comp.Compile(input, cdata);
			
			cdata.Success = false;
			return cdata;			
		}
		
		public static List<string> CallCompiler(string compiler, string args, out long CompileTimeMs)
		{
			Stopwatch watch = new Stopwatch();			
			using(Process process = new Process())
			{
				process.StartInfo.FileName = ParentRootPath+"compile_parent.py";
				process.StartInfo.Arguments = compiler+" "+args;
				process.StartInfo.UseShellExecute = false;
				process.StartInfo.CreateNoWindow = true;
				process.StartInfo.RedirectStandardError = true;
				process.StartInfo.RedirectStandardOutput = true;

				watch.Start();
				process.Start();
				//process.PriorityClass = ProcessPriorityClass.AboveNormal;
				
				OutputReader output = new OutputReader(process.StandardOutput, 100);
                Thread outputReader = new Thread(new ThreadStart(output.ReadOutput));
                outputReader.Start();
                OutputReader error = new OutputReader(process.StandardError, 100);
                Thread errorReader = new Thread(new ThreadStart(error.ReadOutput));
                errorReader.Start();
				
				process.WaitForExit();
				watch.Stop();

				CompileTimeMs = watch.ElapsedMilliseconds;
				errorReader.Join(5000);
                outputReader.Join(5000);
				
				List<string> compOutput = new List<string>();
				compOutput.Add(output.Output);
				compOutput.Add(error.Output);
				return compOutput;			
			}
		}		
	}
    class InputWriter
    {
        StreamWriter writer;
        string input;

        public InputWriter(StreamWriter writer, string input)
        {
            this.writer = writer;
            this.input = input;
        }

        public void Writeinput()
        {
			var encoding = new System.Text.UTF8Encoding(false);
            using (StreamWriter utf8Writer = new StreamWriter(writer.BaseStream, encoding))
            {
                utf8Writer.Write(input);
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
                                sb.Append("\n\n...");
                                addMore = false;
                            }
                        }
                    }
                    else
                        break;
                }
                Output = sb.ToString();
            }
            catch (Exception e)
            {
                Output = string.Format("Error while reading output: {0}", e.Message);
            }
        }
	}
}





