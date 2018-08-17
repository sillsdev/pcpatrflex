// Copyright (c) 2018 SIL International
// This software is licensed under the LGPL, version 2.1 or later
// (http://www.gnu.org/licenses/lgpl-2.1.html)

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIL.DisambiguateInFLExDB
{
	public static class AndFileLoader
	{
		static string[] splitOn = new string[] { "\\parse", "\\endparse" };
		static string[] pfield = new string[] { "\\p " };

		public static string[] GetGuidsFromAndFile(String andfile)
		{
			var guids = new List<String>();
			var sr = new StreamReader(andfile, Encoding.UTF8);
			var contents = sr.ReadToEnd();
			sr.Close();
			//var splitOn = new string[] { "\\parse", "\\endparse" };
			var sections = contents.Split(splitOn, StringSplitOptions.RemoveEmptyEntries);
			foreach (string section in sections)
			{
				//Console.WriteLine("section='" + section + "'");
				if (section.Contains("\\p"))
				{
					ProcessAna(guids, section);
				}
			}

			//int iEndParse = 0;
			//int iParse = contents.Substring(iEndParse).IndexOf("\\parse");
			//Console.WriteLine("1st: parse=" + iParse + "; endparse=" + iEndParse);
			//while (iParse > -1)
			//{
			//	ProcessAna(guids, contents, iEndParse, iParse);
			//	iEndParse = contents.Substring(iParse).IndexOf("\\endparse");
			//	iParse = contents.Substring(iEndParse).IndexOf("\\parse");
			//}

			return guids.ToArray();
		}


		private static void ProcessAna(List<string> guids, string ana)
		{
			if (ana.Contains("%"))
			{
				// still ambiguous; skip it
				guids.Add("");
			}
			else
			{
				var ps = ana.Split(pfield, StringSplitOptions.RemoveEmptyEntries);
				var sb = new StringBuilder();
				//sb.Clear();
				foreach (string p in ps)
				{
					//Console.WriteLine("p='" + p + "'");
					if (p[0] != '\r' && p[1] != '\n')
					{
						int i = p.IndexOf("\n");
						String start = p.Substring(0, i + 1);
						var clean = start.Replace("\r", "").Replace("\n", "");
						sb.Append(clean);
						sb.Append("\n");
					}
				}
				guids.Add(sb.ToString());
				//Console.WriteLine("added '" + sb.ToString() + "'");
				//int iPField = ana.IndexOf("\\p ");
				//while (iPField > -1)
				//{
				//	iPField += 3;
				//	int iNL = ana.Substring(iPField).IndexOf("\n") + iPField;
				//	Console.WriteLine("p=" + iPField + "; nl=" + iNL);
				//	guids.Add(ana.Substring(iPField, iNL - iPField));
				//	iPField = ana.Substring(iNL).IndexOf("\\p ");
				//}
			}

		}


		private static void ProcessAna(List<string> guids, string contents, int iEndParse, int iParse)
		{
			Console.WriteLine("parse=" + iParse + "; endparse=" + iEndParse);
			var ana = contents.Substring(iEndParse, iParse - iEndParse);
			Console.WriteLine("ana='" + ana + "'");
			if (ana.Contains("%"))
			{
				// still ambiguous; skip it
				guids.Add("");
			}
			else
			{
				int iPField = ana.IndexOf("\\p ");
				while (iPField > -1)
				{
					iPField += 3;
					int iNL = ana.Substring(iPField).IndexOf("\n");
					guids.Add(ana.Substring(iPField, iNL - iPField));
					iPField = ana.Substring(iNL).IndexOf("\\p ");
				}
			}

		}
	}
}
