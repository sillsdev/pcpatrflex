// Copyright (c) 2018-2019 SIL International
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
using SIL.LCModel;
using System.Text.RegularExpressions;
using System.Threading;
using SIL.LCModel.DomainServices;
using SIL.LcmLoaderUI;

namespace SIL.DisambiguateInFLExDB
{
	public class ToneParsInvoker
	{
		public LcmCache Cache { get; set; }

		public String AnaFile { get; set; }
		public String AntFile { get; set; }
		public String DatabaseName { get; set; }
		public String InputFile { get; set; }
		public String IntxCtlFile { get; set; }
		public String ParserFilerXMLString { get; set; }
		public String ToneParsBatchFile { get; set; }
		public String ToneParsCmdFile { get; set; }
		public String ToneParsLogFile { get; set; }
		public String ToneParsRuleFile { get; set; }
		public String TraceOptions { get; set; }
		public String XAmpleBatchFile { get; set; }
		public String XAmpleCmdFile { get; set; }
		public String XAmpleLogFile { get; set; }
		public Char DecompSeparationChar { get; set; }

		protected String[] AntRecords { get; set; }
		protected const String kAdCtl = "adctl.txt";
		protected const String kTPAdCtl = "TPadctl.txt";

		public ToneParsInvoker(string toneParsRuleFile, string intxCtlFile, string inputFile, string traceOptions, LcmCache cache)
		{
			ToneParsRuleFile = toneParsRuleFile;
			IntxCtlFile = intxCtlFile;
			InputFile = inputFile;
			TraceOptions = traceOptions;
			Cache = cache;
			DatabaseName = ConvertNameToUseAnsiCharacters(cache.ProjectId.Name);
			InitFileNames();
			DecompSeparationChar = '-';
		}

		private void InitFileNames()
		{
			AnaFile = Path.Combine(Path.GetTempPath(), "ToneParsInvoker.ana");
			AntFile = Path.Combine(Path.GetTempPath(), "ToneParsInvoker.ant");
			XAmpleBatchFile = Path.Combine(Path.GetTempPath(), "XAmpleFLEx.bat");
			XAmpleCmdFile = Path.Combine(Path.GetTempPath(), "XAmpleCmd.cmd");
			XAmpleLogFile = Path.Combine(Path.GetTempPath(), "XAmpleInvoker.log");
			ToneParsBatchFile = Path.Combine(Path.GetTempPath(), "ToneParsFLEx.bat");
			ToneParsCmdFile = Path.Combine(Path.GetTempPath(), "ToneParsCmd.cmd");
			ToneParsLogFile = Path.Combine(Path.GetTempPath(), "ToneParsInvoker.log");
		}

		// following borrowed from SIL.FieldWorks.WordWorks.Parser (ParserCore.dll)
		/// <summary>
		/// Convert any characters in the name which are higher than 0x00FF to hex.
		/// Neither XAmple nor PC-PATR can read a file name containing letters above 0x00FF.
		/// </summary>
		/// <param name="originalName">The original name to be converted</param>
		/// <returns>Converted name</returns>
		internal static string ConvertNameToUseAnsiCharacters(string originalName)
		{
			var sb = new StringBuilder();
			char[] letters = originalName.ToCharArray();
			foreach (var letter in letters)
			{
				int value = Convert.ToInt32(letter);
				if (value > 255)
				{
					string hex = value.ToString("X4");
					sb.Append(hex);
				}
				else
				{
					sb.Append(letter);
				}
			}
			return sb.ToString();
		}

		[DllImport("kernel32.dll", SetLastError = true)]
		private static extern int GetShortPathName(String pathName, StringBuilder shortName, int cbShortName);

		private void CreateBatchFile()
		{
			StringBuilder sbBatchFile = new StringBuilder();
			sbBatchFile.Append("@echo off");
			sbBatchFile.Append(Environment.NewLine);
			sbBatchFile.Append("cd \"");
			sbBatchFile.Append(Path.GetTempPath());
			// XAmple
			sbBatchFile.Append("\"");
			sbBatchFile.Append(Environment.NewLine);
			sbBatchFile.Append("\"");
			sbBatchFile.Append(GetXAmpleExePath());
			//xample32 -b -u -f Abaza.cmd -i Abaza.txt -o Abaza.ana -e Abazagram.txt -m >Abaza.log
			sbBatchFile.Append("\\xample64\" -b -u -f \"");
			sbBatchFile.Append(XAmpleCmdFile);
			sbBatchFile.Append("\" -i \"");
			sbBatchFile.Append(InputFile);
			sbBatchFile.Append("\" -o \"");
			sbBatchFile.Append(AnaFile);
			sbBatchFile.Append("\" -e \"");
			sbBatchFile.Append(DatabaseName);
			sbBatchFile.Append("gram.txt");
			sbBatchFile.Append("\" >\"");
			sbBatchFile.Append(XAmpleLogFile);
			sbBatchFile.Append("\"");
			sbBatchFile.Append(Environment.NewLine);
			sbBatchFile.Append(Environment.NewLine);
			//Console.WriteLine("==========");
			//Console.WriteLine("XAmpleBatch File");
			//Console.WriteLine("==========");
			//Console.Write(sbBatchFile.ToString());
			File.WriteAllText(XAmpleBatchFile, sbBatchFile.ToString());
			// TonePars
			sbBatchFile.Clear();
			sbBatchFile.Append("@echo off");
			sbBatchFile.Append(Environment.NewLine);
			sbBatchFile.Append("cd \"");
			sbBatchFile.Append(Path.GetTempPath());
			sbBatchFile.Append("\"");
			sbBatchFile.Append(Environment.NewLine);
			sbBatchFile.Append("\"");
			sbBatchFile.Append(GetXAmpleExePath());
			sbBatchFile.Append("\\tonepars64\" -b -u -v -f \"");
			sbBatchFile.Append(ToneParsCmdFile);
			sbBatchFile.Append("\" -i \"");
			sbBatchFile.Append(AnaFile);
			sbBatchFile.Append("\" -o \"");
			sbBatchFile.Append(AntFile);
			sbBatchFile.Append("\" >\"");
			sbBatchFile.Append(ToneParsLogFile);
			sbBatchFile.Append("\"");
			sbBatchFile.Append(Environment.NewLine);

			//Console.WriteLine("==========");
			//Console.WriteLine("ToneParsBatch File");
			//Console.WriteLine("==========");
			//Console.Write(sbBatchFile.ToString());
			File.WriteAllText(ToneParsBatchFile, sbBatchFile.ToString());
		}

		private void CreateXAmpleCmdFile()
		{
			StringBuilder sbCmdFile = new StringBuilder();
			sbCmdFile.Append(DatabaseName);
			sbCmdFile.Append(kAdCtl);
			sbCmdFile.Append(Environment.NewLine);
			StringBuilder sbCdTabFileShortPath = new StringBuilder(255);
			int i = GetShortPathName(Path.GetTempPath() + "XAmplecd.tab", sbCdTabFileShortPath, sbCdTabFileShortPath.Capacity);
			sbCmdFile.Append(sbCdTabFileShortPath.ToString());
			sbCmdFile.Append(Environment.NewLine);
			sbCmdFile.Append(Environment.NewLine);
			sbCmdFile.Append(DatabaseName);
			sbCmdFile.Append("lex.txt");
			sbCmdFile.Append(Environment.NewLine);
			sbCmdFile.Append(Environment.NewLine);
			StringBuilder sbIntxCtlFileShortPath = new StringBuilder(255);
			i = GetShortPathName(IntxCtlFile, sbIntxCtlFileShortPath, sbIntxCtlFileShortPath.Capacity);
			sbCmdFile.Append(sbIntxCtlFileShortPath.ToString());
			sbCmdFile.Append(Environment.NewLine);
			sbCmdFile.Append("y");
			sbCmdFile.Append(Environment.NewLine);
			sbCmdFile.Append("y");
			sbCmdFile.Append(Environment.NewLine);
			sbCmdFile.Append(Environment.NewLine);
			sbCmdFile.Append(Environment.NewLine);
			sbCmdFile.Append(Environment.NewLine);
			//Console.WriteLine("==========");
			//Console.WriteLine("XAmple cmd");
			//Console.WriteLine("==========");
			//Console.Write(sbCmdFile.ToString());
			File.WriteAllText(XAmpleCmdFile, sbCmdFile.ToString());
		}

		private void CreateToneParsCmdFile()
		{
			StringBuilder sbCmdFile = new StringBuilder();
			sbCmdFile.Append(DatabaseName);
			sbCmdFile.Append(kTPAdCtl);
			sbCmdFile.Append(Environment.NewLine);
			StringBuilder sbToneRuleFileShortPath = new StringBuilder(255);
			int i = GetShortPathName(ToneParsRuleFile + ".hvo", sbToneRuleFileShortPath, sbToneRuleFileShortPath.Capacity);
			sbCmdFile.Append(sbToneRuleFileShortPath.ToString());
			sbCmdFile.Append(Environment.NewLine);
			StringBuilder sbCdTabFileShortPath = new StringBuilder(255);
			i = GetShortPathName(Path.GetTempPath() + "ToneParscd.tab", sbCdTabFileShortPath, sbCdTabFileShortPath.Capacity);
			sbCmdFile.Append(sbCdTabFileShortPath.ToString());
			sbCmdFile.Append(Environment.NewLine);
			sbCmdFile.Append(Environment.NewLine);
			sbCmdFile.Append(DatabaseName);
			sbCmdFile.Append("lex.txt");
			sbCmdFile.Append(Environment.NewLine);
			sbCmdFile.Append(Environment.NewLine);
			StringBuilder sbIntxCtlFileShortPath = new StringBuilder(255);
			i = GetShortPathName(IntxCtlFile, sbIntxCtlFileShortPath, sbIntxCtlFileShortPath.Capacity);
			sbCmdFile.Append(sbIntxCtlFileShortPath.ToString());
			sbCmdFile.Append(Environment.NewLine);
			sbCmdFile.Append(Environment.NewLine);
			//Console.WriteLine("============");
			//Console.WriteLine("TonePars Cmd");
			//Console.WriteLine("============");
			//Console.Write(sbCmdFile.ToString());
			File.WriteAllText(ToneParsCmdFile, sbCmdFile.ToString());
		}

		private String GetXAmpleExePath()
		{
			Uri uriBase = new Uri(Assembly.GetExecutingAssembly().CodeBase);
			var rootdir = Path.GetDirectoryName(Uri.UnescapeDataString(uriBase.AbsolutePath));
			return rootdir;
		}
		public void Invoke()
		{
			// now tonepars handles it:RemoveAllomorphHvoFromLexiconFile();
			AppendToneParsPropertiesToAdCtlFile();
			ConvertMorphnameIsToUseHvosInToneRuleFile();
			CreateBatchFile();
			CreateXAmpleCmdFile();
			CreateToneParsCmdFile();

			var processInfo = new ProcessStartInfo("cmd.exe", "/c\"" + XAmpleBatchFile + "\"");
			InvokeBatchFile(processInfo);

			processInfo = new ProcessStartInfo("cmd.exe", "/c\"" + ToneParsBatchFile + "\"");
			InvokeBatchFile(processInfo);
			CreateAntRecords();
		}

		private void CreateAntRecords()
		{
			AntRecords = new string[] { "" };
			String antFileContents = "";
			using (var streamReader = new StreamReader(AntFile, Encoding.UTF8))
			{
				antFileContents = streamReader.ReadToEnd().Replace("\r", "");
			}
			if (String.IsNullOrEmpty(antFileContents))
			{
				return;
			}
			AntRecords = antFileContents.Split(new string[] { "\\a " }, StringSplitOptions.None);
		}

		private static void InvokeBatchFile(ProcessStartInfo processInfo)
		{
			processInfo.CreateNoWindow = true;
			processInfo.UseShellExecute = false;
			processInfo.RedirectStandardError = true;
			processInfo.RedirectStandardOutput = true;

			var process = Process.Start(processInfo);
			process.Start();
			string stdOutput = process.StandardOutput.ReadToEnd();
			string stdError = process.StandardError.ReadToEnd();
			process.WaitForExit();
			process.StandardOutput.Close();
			process.StandardError.Close();
			process.Close();
			// Give it time to completely finish or the output file won't be available
			Thread.Sleep(1000);
		}

		public Boolean ConvertAntToParserFilerXML(int word)
		{
			ParserFilerXMLString = "";
			if (word > 0 && word < AntRecords.Length)
			{
				String record = AntRecords[word];
				if (String.IsNullOrEmpty(record))
					return false;
				string wordform = GetFieldFromAntRecord(record, "\\w ");
				var sb = new StringBuilder();
				CreateWordFormElementBegin(wordform, sb);
				Boolean parseFailed = record.Contains("%0%");
				if (parseFailed)
				{
					sb.Append("<WfiAnalysis/>\n");
				}
				else
				{
					int analysisEnd = record.IndexOf("\n");
					String analysis = record.Substring(0, analysisEnd);
					string decomp = GetFieldFromAntRecord(record, "\\d ");
					string underlying = GetFieldFromAntRecord(record, "\\u ");
					if (record.StartsWith("%"))
					{ // multiple analyses
						string[] analyses = analysis.Split('%');
						string[] decomps = decomp.Split('%');
						string[] underlyings = underlying.Split('%');
						for (int i = 2; i < analyses.Length - 1; i++)
						{
							CreateWfiAnalysisElement(sb, analyses[i], decomps[i], underlyings[i]);
						}
					}
					else
					{ // only one analysis
						CreateWfiAnalysisElement(sb, analysis, decomp, underlying);
					}
				}
				sb.Append("</Wordform>\n");
				ParserFilerXMLString = sb.ToString();
				return true;
			}
			return false;
		}

		private void CreateWfiAnalysisElement(StringBuilder sb, string analysis, string decomp, string underlying)
		{
			string[] msaHvos = analysis.Split(' ');
			string[] alloForms = decomp.Split(DecompSeparationChar);
			string[] alloHvos = underlying.Split(DecompSeparationChar);
			sb.Append("<WfiAnalysis>\n");
			sb.Append("<Morphs>\n");
			int i = 0;
			foreach (string msa in msaHvos)
			{
				if (msa == "<" || msa == "W" || msa == ">")
					continue;
				sb.Append("<Morph>\n");
				sb.Append("<MoForm DbRef=\"");
				sb.Append(alloHvos[i]);
				sb.Append("\" Label=\"");
				sb.Append(alloForms[i]);
				sb.Append("\" wordType=\"\"/>\n"); // we hope wordType is not used
				sb.Append("<MSI DbRef=\"");
				sb.Append(msa);
				sb.Append("\"/>");
				sb.Append("</Morph>\n");
				i++;
			}
			sb.Append("</Morphs>\n");
			sb.Append("</WfiAnalysis>\n");
		}

		private void CreateWordFormElementBegin(string wordform, StringBuilder sb)
		{
			sb.Append("<Wordform DbRef=\"");
			//  what do about capitalization???
			//if (wordform == "mbumbukiam")
			//{
			//	wordform = "Mbumbukiam";
			//}
			// find hvo of wordform and append it
			var thiswf = Cache.ServiceLocator.GetInstance<IWfiWordformRepository>().AllInstances().Where(wf => wf.Form.VernacularDefaultWritingSystem.Text == wordform).FirstOrDefault();
			if (thiswf != null)
			{
				sb.Append(thiswf.Hvo);
			}
			sb.Append("\" Form=\"");
			sb.Append(wordform);
			sb.Append("\">\n");
		}

		private static string GetFieldFromAntRecord(string record, string fieldMarker)
		{
			int fieldBegin = record.IndexOf(fieldMarker) + fieldMarker.Length;
			int fieldEnd = record.Substring(fieldBegin).IndexOf("\n");
			String field = record.Substring(fieldBegin, fieldEnd);
			return field;
		}

		private void ConvertMorphnameIsToUseHvosInToneRuleFile()
		{
			// TonePars rule file can have 'morphname is' statements, but the morphname there is the gloss, not the hvo of the MSA.
			// We need to convert all of these from gloss to MSA hvo.

			String toneParsRuleFileContents = File.ReadAllText(ToneParsRuleFile);
			
			//var input = "test1test2test3";
			//var replacements = new Dictionary<string, string> { { "1", "*" }, { "2", "_" }, { "3", "&" } };
			//var output = replacements.Aggregate(input, (current, replacement) => current.Replace(replacement.Key, replacement.Value));
			
			// Find all instances of 'morphname is', replace morphname/gloss with the hvo of the MSA
			var matches = Regex.Matches(toneParsRuleFileContents, " morphname is ([^ \r\n]+)", RegexOptions.Multiline);
			var replacements = new Dictionary<string, string> {};
			var lexEntries = Cache.LanguageProject.LexDbOA.Entries;
			//Console.WriteLine("lex entries count=" + lexEntries.Count());
			var senses = lexEntries.SelectMany(lex => lex.SensesOS);
			//Console.WriteLine("senses count=" + senses.Count());
			//foreach (ILexSense sense in senses)
			//{
			//	Console.WriteLine("sense gloss='" + sense.Gloss.AnalysisDefaultWritingSystem.Text + "'");
			//}

			foreach (Match match in matches)
			{
				var item = match.Value;
				int i = item.LastIndexOf(" ") + 1;
				//var gloss = item.Substring(i + 1);
				//Console.WriteLine("item.Length=" + item.Length + "; i=" + i + "; j=" + j);
				var glossWithFinalParen = item.Substring(i);
				int j = glossWithFinalParen.LastIndexOf(")");
				var gloss = (j == -1) ? glossWithFinalParen.Trim() : glossWithFinalParen.Substring(0, j).Trim();
				//Console.WriteLine("item=" + item + "\tgloss=" + gloss + "\tgloss)=" + glossWithFinalParen);
				//Console.WriteLine("item='" + item + "'\tgloss='" + gloss + "'\tgloss length=" + gloss.Length);
				if (!replacements.ContainsKey(item))
				{
					var sense = senses.FirstOrDefault(s => s.Gloss.AnalysisDefaultWritingSystem.Text.Equals(gloss));
					//var sense = senses.Select(s => gloss == s.Gloss.AnalysisDefaultWritingSystem.Text);
					if (sense == null)
					{
						//Console.WriteLine("sense not found");
						continue;
					}
					var hvo = sense.MorphoSyntaxAnalysisRA.Hvo;
					//var msa = sense.MorphoSyntaxAnalysisRA;
					//var obj = Cache.ServiceLocator.GetObject(hvo);
					//var guid = obj.Guid;
					//Console.WriteLine("gloss=" + gloss + "; sense hvo=" + sense.Hvo + "; msa hvo=" + msa.Hvo + "; guid=" + guid.ToString());
					//if (msa is IMoInflAffMsa)
					//{
					//	var miam = msa as IMoInflAffMsa;
					//	Console.WriteLine("\t\tinfl affix msa hvo=" + miam.Hvo);
					//}
					replacements.Add(item, item.Replace(gloss, Convert.ToString(hvo)));
				}
			}
			
			var toneParsRuleWithHvos = replacements.Aggregate(toneParsRuleFileContents, (current, replacement) => current.Replace(replacement.Key, replacement.Value));
			String toneParsRuleFile = ToneParsRuleFile + ".hvo";
			File.WriteAllText(toneParsRuleFile, toneParsRuleWithHvos);
		}

		private void RemoveAllomorphHvoFromLexiconFile()
		{
			// Remove hvo ID from lexicon file; TonePars does not handle it
			String xAmpleLexiconFile = Path.GetTempPath() + DatabaseName + "lex.txt";
			String xAmpleLexicon = File.ReadAllText(xAmpleLexiconFile);
			//Console.WriteLine("# of matches=" + matches.Count);
			//Console.WriteLine("XAmple ==========");
			//Console.Write(xAmpleLexicon);
			//Console.WriteLine("==========");
			String toneParsLexicon = Regex.Replace(xAmpleLexicon, @"^\\a ([^ ]+) \{[1-9][0-9]*\}", @"\a $1", RegexOptions.Multiline);
			//Console.WriteLine("TonePars ==========");
			//Console.Write(toneParsLexicon);
			//Console.WriteLine("==========");
			String toneParsLexiconFile = Path.GetTempPath() + DatabaseName + "TPlex.txt";
			File.WriteAllText(toneParsLexiconFile, toneParsLexicon);
		}

		private void AppendToneParsPropertiesToAdCtlFile()
		{
			// Append all TonePars properties in FLEx DB as allomorph properties to the AD Ctl file
			String xAmpleAdCtlFile = Path.GetTempPath() + DatabaseName + kAdCtl;
			String xAmpleAdCtl = File.ReadAllText(xAmpleAdCtlFile);
			var props = GetAllToneParsPropsFromPossibilityList();
			String toneParsAdCtlFile = Path.GetTempPath() + DatabaseName + kTPAdCtl;
			File.WriteAllText(toneParsAdCtlFile, xAmpleAdCtl + props);
		}

		private string GetAllToneParsPropsFromPossibilityList()
		{
			var possListRepository = Cache.ServiceLocator.GetInstance<ICmPossibilityListRepository>();
			var toneParsList = possListRepository.AllInstances().FirstOrDefault(list => list.Name.BestAnalysisAlternative.Text == Constants.ToneParsPropertiesList);
			var sb = new StringBuilder();
			foreach (var prop in toneParsList.PossibilitiesOS)
			{
				sb.Append("\\ap ");
				sb.Append(prop.Name.AnalysisDefaultWritingSystem.Text);
				sb.Append("\n");
			}
			return sb.ToString();
		}

		//private void CreateTakeFile()
		//{
		//	String takeFile = Path.Combine(Path.GetTempPath(), takeFileName);
		//	StringBuilder sbTakeFileShortPath = new StringBuilder(255);
		//	int i = GetShortPathName(takeFile, sbTakeFileShortPath, sbTakeFileShortPath.Capacity);
		//	var sbTake = new StringBuilder();
		//	sbTake.Append("set comment |\n");
		//	sbTake.Append("log ");
		//	sbTake.Append(logFileName);
		//	sbTake.Append("\n");
		//	sbTake.Append("load grammar ");
		//	StringBuilder sbGrammarFileShortPath = new StringBuilder(255);
		//	i = GetShortPathName(ToneParsRuleFile, sbGrammarFileShortPath, sbGrammarFileShortPath.Capacity);
		//	sbTake.Append(sbGrammarFileShortPath.ToString() + "\n");
		//	sbTake.Append("set timing on\n");
		//	sbTake.Append("set gloss on\n");
		//	sbTake.Append("set features all\n");
		//	HandleRootGloss(sbTake);
		//	sbTake.Append("set tree xml\n");
		//	sbTake.Append("set ambiguities 100\n");
		//	sbTake.Append("set write-ample-parses on\n");
		//	sbTake.Append("file disambiguate ");
		//	StringBuilder sbAnaFileShortPath = new StringBuilder(255);
		//	i = GetShortPathName(AnaFile, sbAnaFileShortPath, sbAnaFileShortPath.Capacity);
		//	String anashort = sbAnaFileShortPath.ToString();
		//	sbTake.Append(anashort);
		//	//Console.WriteLine("anashort='" + anashort + "'");
		//	sbTake.Append(" ");
		//	//String andshort = "";
		//	String result = anashort.Substring(0, anashort.Length - 1) + "d";
		//	sbTake.Append(result + "\n");
		//	sbTake.Append("exit\n");
		//	//Console.Write(sbTake.ToString());
		//	File.WriteAllText(takeFile, sbTake.ToString());
		//	InputFile = result;
		//}

		private void HandleRootGloss(StringBuilder sbTake)
		{
			if (String.IsNullOrEmpty(TraceOptions))
			{
				return;
			}
			sbTake.Append("set rootgloss ");
			String result;
			result = GetRootGlossStateValue();
			sbTake.Append(result + "\n");
		}

		public string GetRootGlossStateValue()
		{
			string result;
			String sBeginning = TraceOptions.Substring(0, 1).ToLower();
			switch (sBeginning)
			{
				case "l":
					result = "leftheaded";
					break;
				case "r":
					result = "rightheaded";
					break;
				case "a":
					result = "all";
					break;
				default:
					result = "off";
					break;
			}

			return result;
		}
	}
}
