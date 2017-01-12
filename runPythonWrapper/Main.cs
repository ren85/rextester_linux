using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading;
using System.Runtime.InteropServices;
using System.Collections.Generic;
using ExecutionEngine;
using System.Linq;
using Services;
using runPythonWrapper;


namespace Test
{
	class MainClass
	{	
		public static void Main (string[] args)
		{
			var list = new List<ITest> ();
			list.Add (new AdaHello ());
			list.Add (new BashHello ());
			list.Add (new CClangHello ());
			list.Add (new CHello ());
			list.Add (new CPPClangHello ());
			list.Add (new CPPHello ());
			list.Add (new DHello ());
			list.Add (new ElixirHello ());
			list.Add (new ErlangHello ());
			list.Add (new FHello ());
			list.Add (new GoHello ());
			list.Add (new HaskellHello ());
			list.Add (new JavaHello ());
			list.Add (new JavascriptHello ());
			list.Add (new JavaTwoClasses ());
			list.Add (new LispHello ());
			list.Add (new LuaHello ());
			list.Add (new NasmHello ());
			list.Add (new NodeHello ());
			list.Add (new ObjectivecHello ());
			list.Add (new OcamlHello ());
			list.Add (new OctaveHello ());
			list.Add (new PascalHello ());
			list.Add (new PerlHello ());
			list.Add (new PhpHello ());
			list.Add (new PrologHello ());
			list.Add (new Python3Hello ());
			list.Add (new PythonHello ());
			list.Add (new RHello ());
			list.Add (new RubyHello ());
			list.Add (new ScalaHello ());
			list.Add (new SchemeHello ());
			list.Add (new SwiftHello ());
			list.Add (new TclHello ());

			foreach(var t in list)
			{
				try
				{
					Console.WriteLine(t.GetName());
					t.Do();
				}
				catch(Exception ex)
				{
					var a = ex;
					throw;
				}
			}
		}
	}
}

