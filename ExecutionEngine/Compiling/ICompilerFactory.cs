using System;
using ExecutionEngine;

namespace ExecutionEngine
{
	public class ICompilerFactory
	{
		public static ICompiler GetICompiler(Languages lang)
		{
			switch(lang)
			{
				case Languages.Java:
					return new JavaCompile();
				case Languages.Python:
					return new PythonCompile();
				case Languages.C:
					return new CCompile();
				case Languages.CPP:
					return new CPPCompile();
				case Languages.Php:
					return new PhpCompile();
				case Languages.Pascal:
					return new PascalCompile();
				case Languages.ObjectiveC:
					return new ObjectiveCCompile();
				case Languages.Haskell:
					return new HaskellCompile();
				case Languages.Ruby:
					return new RubyCompile();
				case Languages.Perl:
					return new PerlCompile();
				case Languages.Lua:
					return new LuaCompile();
				case Languages.Nasm:
					return new NasmCompile();
				case Languages.Javascript:
					return new JavascriptCompile();
				case Languages.Lisp:
					return new LispCompile();
				case Languages.Prolog:
					return new PrologCompile();
				case Languages.Go:
					return new GoCompile();
				case Languages.Scala:
					return new ScalaCompile();
				case Languages.Scheme:
					return new SchemeCompile();
				case Languages.Nodejs:
					return new NodeCompile();
				case Languages.Python3:
					return new Python3Compile();
				case Languages.Octave:
					return new OctaveCompile();
				case Languages.CClang:
					return new CClangCompile();
				case Languages.CppClang:
					return new CPPClangCompile();
				default:
					return null;
			}
		}
	}
}

