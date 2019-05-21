using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ana2XmlPrep
{
	class Ana2XmlPrep
	{
		static void Main(string[] args)
		{
			if (args.Length != 4)
			{
				ShowUsage();
			}
			if (args[0] != "-i")
			{
				ShowUsage();
			}
			string InputFileName = args[1];
			if (!File.Exists(InputFileName))
			{
				Console.Error.WriteLine("Input file '" + InputFileName + "' not found.");
				Environment.Exit(1);
			}
			if (args[2] != "-o")
			{
				ShowUsage();
			}
			string OutputFileName = args[3];
			string Contents = File.ReadAllText(InputFileName, Encoding.UTF8);
			string Results = Contents.Replace("\r\n\r\n\\parse", "\r\n\\parse").Replace("\r\n\r\n\\failure", "\r\n\\failure");
			File.WriteAllText(OutputFileName, Results, Encoding.UTF8);
		}

		private static void ShowUsage()
		{
			Console.Error.WriteLine("Usage: Ana2XmlPrep -i input_file -o output_file");
			Environment.Exit(1);
		}
	}
}
