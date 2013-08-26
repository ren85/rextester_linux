using System;
using ServiceWarmup.n178_238_226_7;

namespace ServiceWarmup
{
	public class CppRun : IRun
	{
		public CppRun ()
		{
		}

		#region IRun implementation
		public string Code 
		{
			get 
			{
				return @"
#include <iostream>
using namespace std;

int main()
{
    cout << """+Output+@""";
}";
			}
		}

		public Languages Language 
		{
			get 
			{
				return Languages.CPP;
			}
		}

		public string Output 
		{
			get 
			{
				return "Hello, C++ world!";
			}
		}

		public string CompilerArgs
		{
			get
			{
				return "-Wall -std=c++11 -O2 -o a.out source_file.cpp";
			}
		}
		#endregion
	}
}