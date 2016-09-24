﻿using System;
using System.IO;

namespace ExecutionEngine
{
	public class SwiftCompile: ICompiler
	{
		public SwiftCompile () 
		{
		}

		#region ICompiler implementation
		public CompilerData Compile (InputData idata, CompilerData cdata)
		{
			string compiler = "swiftc";
			if(string.IsNullOrEmpty(idata.Compiler_args) || !idata.Compiler_args.Contains("-o a.out"))
			{
				cdata.Error = "Compiler args must contain '-o a.out'";
				cdata.Success = false;
				return cdata;
			}
			if(string.IsNullOrEmpty(idata.Compiler_args) || !idata.Compiler_args.Contains("source_file.swift"))
			{
				cdata.Error = "Compiler args must contain 'source_file.swift'";
				cdata.Success = false;
				return cdata;
			}

			idata.Compiler_args = idata.Compiler_args.Replace("source_file.swift", idata.PathToSource);
			idata.Compiler_args = idata.Compiler_args.Replace("-o a.out", "-o "+ idata.BaseDir + "a.out ");

			string args = idata.Compiler_args;

			long compileTime;
			var res = Engine.CallCompiler(compiler, args, out compileTime);
			cdata.CompileTimeMs = compileTime;
			if(!File.Exists(idata.BaseDir+"a.out"))
			{
				if(res.Count > 1)
				{
					cdata.Error = Utils.ConcatenateString(res[0], res[1]);
				}	
				cdata.Success = false;
				return cdata;
			}
			if(res.Count > 1 && (!string.IsNullOrEmpty(res[0]) || !string.IsNullOrEmpty(res[1])))
				cdata.Warning = Utils.ConcatenateString(res[0], res[1]);
			cdata.ExecuteThis = idata.BaseDir+"a.out";
			cdata.Executor = "";
			cdata.Success = true;
			return cdata;
		}
		#endregion
	}
}

