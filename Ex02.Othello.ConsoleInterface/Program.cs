using System;
using System.Collections.Generic;
using System.Text;

namespace Ex02.Othello.ConsoleInterface
{
	class Program
	{
		static void Main(string[] args)
		{
			ConsoleFrontend frontend = new ConsoleFrontend();
			frontend.Run();
		}
	}
}
