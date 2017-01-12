using System;
using ExecutionEngine;
using System.Collections.Generic;
using System.Text;

namespace ExecutionEngine
{
	public class Utils
	{
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
		
		public static string ConcatenateString(string a, string b)
		{
			if(string.IsNullOrEmpty(a))
				return b;
			if(string.IsNullOrEmpty(b))
				return a;
			
			return a+"\n\n"+b;
		}
		
		public static bool IsCompiled(Languages lang)
		{
			if(lang == Languages.C || lang == Languages.CPP || lang == Languages.CClang || lang == Languages.CppClang || lang == Languages.Java || 
			   lang == Languages.Pascal || lang == Languages.Haskell || lang == Languages.ObjectiveC || lang == Languages.Nasm || 
			   lang == Languages.Go || lang == Languages.Scala || lang == Languages.D || lang == Languages.Swift || lang == Languages.FSharp ||
			   lang == Languages.Rust || lang == Languages.Ada || lang == Languages.Erlang ||
			   lang == Languages.Ocaml || lang == Languages.Clojure)
				return true;
			else
				return false;
			
		}
		
		public static string RemoveSomeLines(string input, List<string> dropLines)
		{			
			string[] drops = dropLines.ToArray();			
			StringBuilder res = new StringBuilder();
			string[] lines = input.Split("\n".ToCharArray(), StringSplitOptions.None);
			bool dropIt = false;
			foreach(string line in lines)
			{
				dropIt = false;
				for(int i=0; i<drops.Length; i++)
					if(drops[i] != null)
					{
						if(line.Contains(drops[i]))
						{
							drops[i] = null;
							dropIt = true;
							break;
						}							
					}

				if(!dropIt)
					res.AppendLine(line);	
			}
			
			string r = res.ToString();
			if(r != null)
				r = r.Trim("\n".ToCharArray());
			return string.IsNullOrEmpty(r) ? null : r;
		}
	}
}

