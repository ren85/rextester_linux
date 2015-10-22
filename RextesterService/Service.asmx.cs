using System;
using System.Web.Services;
using System.Collections.Generic;
using ExecutionEngine;
using System.Text.RegularExpressions;

using System.IO.Compression;
using System.Text;
using System.IO;
using System.Web;
using System.Web.Script.Serialization;
using Services;
using System.Net;
using System.Web.Script.Services;

namespace RextesterService
{
	[WebService (Namespace = "http://rextester.com/")]
	public class Service : WebService
	{
		[WebMethod]
		public Result DoWork(string Program, string Input, Languages Language, string user, string pass, string compiler_args = "", bool bytes = false, bool programCompressed = false, bool inputCompressed = false)
		{
			if(user != GlobalUtils.TopSecret.ServiceUser || pass != GlobalUtils.TopSecret.ServicePass)
			{
				return new Result()
				{
					Errors = null,
					Warnings = null,
					Output = null,
					Stats = null,
					Exit_Status = null,
					Exit_Code = null,
					System_Error = "Not authorized."			
				};
			}
			
			if(programCompressed)
				Program = Decompress(Program);
			if(inputCompressed)
				Input = Decompress(Input);
				
			Engine engine = new Engine();
			InputData idata = new InputData()
			{
				Program = Program,
				Input = Input,
				Lang = Language,
				Compiler_args = compiler_args
			};

			var odata = engine.DoWork(idata);		
			
			Regex r = new Regex(Engine.RootPath + @"\d+/source");
			if(Language == Languages.Javascript && !string.IsNullOrEmpty(odata.Output))
				odata.Output = r.Replace(odata.Output, "source_file");
				
			var res = new Result()
				{
					Errors = !string.IsNullOrEmpty(odata.Errors) ? r.Replace(odata.Errors, "source_file") : odata.Errors, 
					Warnings = !string.IsNullOrEmpty(odata.Warnings) ? r.Replace(odata.Warnings, "source_file") : odata.Warnings,
					Output = odata.Output,
					Stats = odata.Stats,
					Exit_Status = odata.Exit_Status,
					Exit_Code = odata.ExitCode,
					System_Error = odata.System_Error,
					Files = odata.Files
				};
			if(!string.IsNullOrEmpty(odata.Output) && odata.Output.Length > 1000)
			{
				res.Output = Compress(odata.Output);
				res.IsOutputCompressed = true;
			}
			if(bytes)
			{
				if(!string.IsNullOrEmpty(res.Errors))
				{
					res.Errors_Bytes =  System.Text.Encoding.Unicode.GetBytes(res.Errors);
					res.Errors = null;
				}
				if(!string.IsNullOrEmpty(res.Warnings))
				{
					res.Warnings_Bytes =  System.Text.Encoding.Unicode.GetBytes(res.Warnings);
					res.Warnings = null;
				}
				if(!string.IsNullOrEmpty(res.Output))
				{
					res.Output_Bytes =  System.Text.Encoding.Unicode.GetBytes(res.Output);
					res.Output = null;
				}
			}
			return res;			
		}
		
		[WebMethod]
		public int Sum(int a, int b)
		{
			return a+b;
		}
		
		[WebMethod]
		public DiffResult Diff(string left, string right, string user, string pass)
		{
			if(user != GlobalUtils.TopSecret.ServiceUser || pass != GlobalUtils.TopSecret.ServicePass)
				return null;
				
			var res = ExecutionEngine.Diff.GetDiff(Decompress(left), Decompress(right));
			res.Result = Compress(res.Result);
			return res;
		}

		[WebMethod]
		public void GetPythonDotCompletions(string source, int line, int column)
		{
			source = HttpUtility.HtmlDecode(source);
			JavaScriptSerializer json = new JavaScriptSerializer();
			HttpContext.Current.Response.AppendHeader("Access-Control-Allow-Origin", "*");
			HttpContext.Current.Response.Write(json.Serialize(CodeCompletion.GetPythonDotCompletion(source, line, column)));
		}
		[WebMethod]
		public void GetPythonParenCompletions(string source, int line, int column)
		{
			source = HttpUtility.HtmlDecode(source);
			JavaScriptSerializer json = new JavaScriptSerializer();
			HttpContext.Current.Response.AppendHeader("Access-Control-Allow-Origin", "*");
			HttpContext.Current.Response.Write(json.Serialize(CodeCompletion.GetPythonParenCompletion(source, line, column)));
		}

		[WebMethod]
		public string GetCPPCompletions(string source, int line, int column)
		{
			source = Decompress(source);
			//HttpContext.Current.Response.AppendHeader("Access-Control-Allow-Origin", "*");
			var res = CodeCompletion.GetCPP_Completions (source, line, column);
			JavaScriptSerializer json = new JavaScriptSerializer();
			var s = json.Serialize(res);		
			return Compress(s);
		}

		string Compress(string text)
        {
        	 if(string.IsNullOrEmpty(text))
        	 	return "";
        	 	
             byte[] buffer = Encoding.UTF8.GetBytes(text);
             MemoryStream ms = new MemoryStream();
             using (GZipStream zip =new GZipStream(ms, CompressionMode.Compress, true))
             {
                  zip.Write(buffer, 0, buffer.Length);
             }
            
             ms.Position = 0;
             //MemoryStream outStream = new MemoryStream();
            
             byte[] compressed = new byte[ms.Length];
             ms.Read(compressed, 0, compressed.Length);
            
             byte[] gzBuffer = new byte[compressed.Length + 4];
             System.Buffer.BlockCopy(compressed, 0, gzBuffer, 4, compressed.Length);
             System.Buffer.BlockCopy(BitConverter.GetBytes(buffer.Length), 0, gzBuffer, 0, 4);
             return Convert.ToBase64String (gzBuffer);
        }
        
        string Decompress(string compressedText)
        {
	        if(string.IsNullOrEmpty(compressedText))
    	 		return "";
    	 		
            byte[] gzBuffer = Convert.FromBase64String(compressedText);
            using (MemoryStream ms =new MemoryStream())
            {
                int msgLength = BitConverter.ToInt32(gzBuffer, 0);
                ms.Write(gzBuffer, 4, gzBuffer.Length - 4);
        
                byte[] buffer =new byte[msgLength];
        
                ms.Position = 0;
                using (GZipStream zip =new GZipStream(ms, CompressionMode.Decompress))
                {
                   zip.Read(buffer, 0, buffer.Length);
                }
        
                return Encoding.UTF8.GetString(buffer);
            }
        }
	}
	
	
	public class Result
	{
		public string Errors
		{
			get;
			set;
		}
		
		public byte[] Errors_Bytes
		{
			get;
			set;
		}
		
		public string Warnings
		{
			get;
			set;		
		}
		
		public byte[] Warnings_Bytes
		{
			get;
			set;
		}
		
		public string Output
		{
			get;
			set;		
		}
		
		public bool IsOutputCompressed
		{
			get;
			set;
		}
		
		public byte[] Output_Bytes
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
		public int? Exit_Code
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
}

