// Copyright (c) 2018 SIL International
// This software is licensed under the LGPL, version 2.1 or later
// (http://www.gnu.org/licenses/lgpl-2.1.html)

using System;
using System.Runtime.InteropServices;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;

namespace SIL.DisambiguateInFLExDB
{
	public class PCPatrInvoker
	{
		const string takeFileName = "PcPatrFLEx.tak";
		const string logFileName = "Invoker.log";
		public String GrammarFile { get; set; }
		public String AnaFile { get; set; }
		public String AndFile { get; set; }
		public String LogFile { get; set; }
		public String BatchFile { get; set; }

		public PCPatrInvoker(string grammarFile, string anaFile)
		{
			GrammarFile = grammarFile;
			AnaFile = anaFile;
			LogFile = Path.Combine(Path.GetTempPath(), logFileName);
		}

		[DllImport("kernel32.dll", SetLastError = true)]
		private static extern int GetShortPathName(String pathName, StringBuilder shortName, int cbShortName);

		private void CreateBatchFile()
		{
			BatchFile = Path.Combine(Path.GetTempPath(), "PcPatrFLEx.bat");
			StringBuilder sbBatchFile = new StringBuilder();
			sbBatchFile.Append("@echo off\n");
			sbBatchFile.Append("cd \"");
			sbBatchFile.Append(Path.GetTempPath());
			sbBatchFile.Append("\"\n");
			sbBatchFile.Append("pcpatr32 -t ");
			sbBatchFile.Append(takeFileName);
			sbBatchFile.Append("\n");
			//Console.Write(sbBatchFile.ToString());
			File.WriteAllText(BatchFile ,sbBatchFile.ToString());
		}
		public void Invoke()
		{
			CreateBatchFile();
			CreateTakeFile();

			File.Delete(AndFile);
			File.Delete(LogFile);

			var processInfo = new ProcessStartInfo("cmd.exe", "/c\"" + BatchFile + "\"");
			processInfo.CreateNoWindow = true;
			processInfo.UseShellExecute = false;
			processInfo.RedirectStandardError = true;
			processInfo.RedirectStandardOutput = true;

			var process = Process.Start(processInfo);
			process.Start();
			process.WaitForExit();
			string output = process.StandardOutput.ReadToEnd();
			string error = process.StandardError.ReadToEnd();
			//Console.Write(output);
			//Console.Write(error);
		}

		private void CreateTakeFile()
		{
			String takeFile = Path.Combine(Path.GetTempPath(), takeFileName);
			StringBuilder sbTakeFileShortPath = new StringBuilder(255);
			int i = GetShortPathName(takeFile, sbTakeFileShortPath, sbTakeFileShortPath.Capacity);
			var sbTake = new StringBuilder();
			sbTake.Append("set comment |\n");
			sbTake.Append("log ");
			sbTake.Append(logFileName);
			sbTake.Append("\n");
			sbTake.Append("load grammar ");
			StringBuilder sbGrammarFileShortPath = new StringBuilder(255);
			i = GetShortPathName(GrammarFile, sbGrammarFileShortPath, sbGrammarFileShortPath.Capacity);
			sbTake.Append(sbGrammarFileShortPath.ToString() + "\n");
			sbTake.Append("set timing on\n");
			sbTake.Append("set gloss on\n");
			sbTake.Append("set features all\n");
			sbTake.Append("set tree xml\n");
			sbTake.Append("set ambiguities 100\n");
			sbTake.Append("set write-ample-parses on\n");
			sbTake.Append("file disambiguate ");
			StringBuilder sbAnaFileShortPath = new StringBuilder(255);
			i = GetShortPathName(AnaFile, sbAnaFileShortPath, sbAnaFileShortPath.Capacity);
			String anashort = sbAnaFileShortPath.ToString();
			sbTake.Append(anashort);
			//Console.WriteLine("anashort='" + anashort + "'");
			sbTake.Append(" ");
			String andshort = ""; // sbTakeFileShortPath.ToString();
			String result = anashort.Substring(0, anashort.Length - 1) + "d";
			sbTake.Append(result + "\n");
			sbTake.Append("exit\n");
			//Console.Write(sbTake.ToString());
			File.WriteAllText(takeFile, sbTake.ToString());
			AndFile = result;
		}
	}
}
