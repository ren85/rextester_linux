using System;
using System.Collections.Generic;
using System.IO;

namespace Services
{
	public class CodeCompletion
	{
		static List<string> GetPythonCompletion(string source, int line, int column, string file_name)
		{
			string path = Utils.ParentRootPath + "Python/" + Utils.RandomString()+".py";
			File.WriteAllText(path, source);
			var completions = Utils.CallProgram("python", string.Format("{0} {1} {2} {3}", Utils.ParentRootPath + "Python/" + file_name, path, line, column));
			File.Delete(path);
			return completions;
		}

		public static List<string> GetPythonDotCompletion(string source, int line, int column)
		{
			return CodeCompletion.GetPythonCompletion(source, line, column, "py_dot.py");
		}

		public static List<string> GetPythonParenCompletion(string source, int line, int column)
		{
			return CodeCompletion.GetPythonCompletion(source, line, column, "py_paren.py");
		}
	}
}

