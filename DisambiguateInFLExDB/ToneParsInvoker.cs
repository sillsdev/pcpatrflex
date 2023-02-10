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
//using SIL.LcmLoaderUI;
using XCore;
using SIL.FieldWorks.WordWorks.Parser;
using System.Xml.Linq;
using SIL.LCModel.Infrastructure;
using SIL.LCModel.Core.Text;
using System.Windows.Forms;

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
        public Boolean InvocationSucceeded { get; set; }
        public String ParserFilerXMLString { get; set; }
        public String ToneParsBatchFile { get; set; }
        public String ToneParsCmdFile { get; set; }
        public String ToneParsLogFile { get; set; }
        public String ToneParsRuleFile { get; set; }
        public String XAmpleBatchFile { get; set; }
        public String XAmpleCmdFile { get; set; }
        public String XAmpleLogFile { get; set; }
        public Char DecompSeparationChar { get; set; }
        public IdleQueue Queue { get; set; }

        protected String[] AntRecords { get; set; }
        protected const String kAdCtl = "adctl.txt";
        protected const String kTPAdCtl = "TPadctl.txt";
        protected const String kLexicon = "lex.txt";
        protected const String kTPLexicon = "TPlex.txt";

        public ToneParsInvoker(string toneParsRuleFile, string intxCtlFile, string inputFile, char decomp, LcmCache cache)
        {
            ToneParsRuleFile = toneParsRuleFile;
            IntxCtlFile = intxCtlFile;
            InputFile = inputFile;
            DecompSeparationChar = decomp;
            Cache = cache;
            DatabaseName = ConvertNameToUseAnsiCharacters(cache.ProjectId.Name);
            InitFileNames();
            Queue = new IdleQueue { IsPaused = true };
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
            sbBatchFile.Append("\\tonepars64\" -b -u ");
            sbBatchFile.Append(ToneParsInvokerOptions.Instance.GetOptionsString());
            sbBatchFile.Append(" -f \"");
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
            sbCmdFile.Append("XAmplecd.tab");
            sbCmdFile.Append(Environment.NewLine);
            sbCmdFile.Append(Environment.NewLine);
            sbCmdFile.Append(DatabaseName);
            sbCmdFile.Append(kTPLexicon);
            sbCmdFile.Append(Environment.NewLine);
            sbCmdFile.Append(Environment.NewLine);
            StringBuilder sbIntxCtlFileShortPath = new StringBuilder(255);
            int i = GetShortPathName(IntxCtlFile, sbIntxCtlFileShortPath, sbIntxCtlFileShortPath.Capacity);
            sbCmdFile.Append(sbIntxCtlFileShortPath.ToString());
            sbCmdFile.Append(Environment.NewLine);
            sbCmdFile.Append("y");
            sbCmdFile.Append(Environment.NewLine);
            sbCmdFile.Append("y");
            sbCmdFile.Append(Environment.NewLine);
            sbCmdFile.Append(Environment.NewLine);
            sbCmdFile.Append(Environment.NewLine);
            sbCmdFile.Append(Environment.NewLine);
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
            sbCmdFile.Append("ToneParscd.tab");
            sbCmdFile.Append(Environment.NewLine);
            sbCmdFile.Append(Environment.NewLine);
            sbCmdFile.Append(DatabaseName);
            sbCmdFile.Append(kTPLexicon);
            sbCmdFile.Append(Environment.NewLine);
            sbCmdFile.Append(Environment.NewLine);
            StringBuilder sbIntxCtlFileShortPath = new StringBuilder(255);
            i = GetShortPathName(IntxCtlFile, sbIntxCtlFileShortPath, sbIntxCtlFileShortPath.Capacity);
            sbCmdFile.Append(sbIntxCtlFileShortPath.ToString());
            sbCmdFile.Append(Environment.NewLine);
            sbCmdFile.Append(Environment.NewLine);
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
            AppendToneParsPropertiesToAdCtlFile();
            AddToneParsPropertiesToLexiconFile();
            ConvertMorphnameIsToUseHvosInToneRuleFile();
            CreateBatchFile();
            CreateXAmpleCmdFile();
            CreateToneParsCmdFile();
            CopyCodeTableFilesToTemp();

            File.Delete(AntFile);
            File.Delete(ToneParsLogFile);


            var processInfo = new ProcessStartInfo("cmd.exe", "/c\"" + XAmpleBatchFile + "\"");
            InvokeBatchFile(processInfo);
            if (!InvocationSucceeded)
            {
                MessageBox.Show("There was a problem with XAmple parsing the segment or text.  Make sure every word can be parsed by XAmple.");
                return;
            }

            processInfo = new ProcessStartInfo("cmd.exe", "/c\"" + ToneParsBatchFile + "\"");
            InvokeBatchFile(processInfo);
            if (!InvocationSucceeded)
            {
                MessageBox.Show("There was a problem with TonePars parsing the segment.  See the %TEMP%\\ToneParsInvoker.log file.");
                return;
            }
            CreateAntRecords();
        }

        private void CopyCodeTableFilesToTemp()
        {
            const string kTPcdtab = "ToneParscd.tab";
            const string kXAcdtab = "XAmplecd.tab";
            Uri uriBase = new Uri(Assembly.GetExecutingAssembly().CodeBase);
            var rootdir = Path.GetDirectoryName(Uri.UnescapeDataString(uriBase.AbsolutePath));
            File.Copy(Path.Combine(rootdir, kTPcdtab), Path.Combine(Path.GetTempPath(), kTPcdtab), true);
            File.Copy(Path.Combine(rootdir, kXAcdtab), Path.Combine(Path.GetTempPath(), kXAcdtab), true);
        }

        private void CreateAntRecords()
        {
            AntRecords = new string[] { "" };
            String antFileContents = "";
            //private const int NumberOfRetries = 3;
            //        private const int DelayOnRetry = 1000;

            //for (int i=1; i <= NumberOfRetries; ++i)
            //            {
            //    try {
            antFileContents = File.ReadAllText(AntFile, Encoding.UTF8).Replace("\r", "");
            //    break; // When done we can break loop
            //}
            //catch (IOException e) when(i <= NumberOfRetries)
            //    {
            //        // You may check error code to filter some exceptions, not every error
            //        // can be recovered.
            //        Thread.Sleep(DelayOnRetry);
            //    }
            //}
            StringBuilder sb = new StringBuilder();
            using (var stream = File.Open(AntFile, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                byte[] b = new byte[1024];
                UTF8Encoding temp = new UTF8Encoding(true);

                while (stream.Read(b, 0, b.Length) > 0)
                {
                    sb.Append(temp.GetString(b));
                }
            }
            antFileContents = sb.ToString().Replace("\r", "");
            //using (var streamReader = new StreamReader(AntFile, Encoding.UTF8))
            //{
            //    antFileContents = streamReader.ReadToEnd().Replace("\r", "");
            //    streamReader.Close();
            //}
            if (String.IsNullOrEmpty(antFileContents))
            {
                return;
            }
            AntRecords = antFileContents.Split(new string[] { "\\a " }, StringSplitOptions.None);
        }

        private void InvokeBatchFile(ProcessStartInfo processInfo)
		{
			processInfo.CreateNoWindow = true;
			processInfo.UseShellExecute = false;
			processInfo.RedirectStandardError = true;
			processInfo.RedirectStandardOutput = true;

            using (var process = Process.Start(processInfo))
            {
                process.Start();
                string stdOutput = process.StandardOutput.ReadToEnd();
                string stdError = process.StandardError.ReadToEnd();
                process.WaitForExit();
                if (process.ExitCode == 0)
                {
                    InvocationSucceeded = true;
                }
                else
                {
                    InvocationSucceeded = false;
                }
                process.StandardOutput.Close();
                process.StandardError.Close();
                process.Close();
            }
            // Give it time to completely finish or the output file won't be available
            Thread.Sleep(1000);
		}

		public Boolean ConvertAntToParserFilerXML(int word)
		{
			ParserFilerXMLString = "";
			if (word > 0 && AntRecords != null && word < AntRecords.Length)
			{
				String record = AntRecords[word];
				if (String.IsNullOrEmpty(record))
					return false;
				string wordform = GetFieldFromAntRecord(record, "\\w ");
				if (String.IsNullOrEmpty(wordform))
					return false;
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
			sb.Append("<Wordform DbRef=\"\" Form=\"");
			//  what do about capitalization???
			sb.Append(wordform);
			sb.Append("\">\n");
		}

		public static string GetFieldFromAntRecord(string record, string fieldMarker)
		{
			int fieldBegin = record.IndexOf(fieldMarker) + fieldMarker.Length;
			int fieldEnd = record.Substring(fieldBegin).IndexOf("\n");
			String field = record.Substring(fieldBegin, fieldEnd);
			return field;
		}

		// TonePars rule file can have 'morphname is' statements, but the morphname there is the gloss, not the hvo of the MSA.
		// We need to convert all of these from gloss to MSA hvo.
		private void ConvertMorphnameIsToUseHvosInToneRuleFile()
		{

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
                    ILexSense sense = null;
                    foreach (ILexSense s in senses)
                    {
                        var g = s.Gloss;
                        if (g == null || g.AnalysisDefaultWritingSystem == null || g.AnalysisDefaultWritingSystem.Text == null)
                        {
                            continue;
                        }
                        if (g.AnalysisDefaultWritingSystem.Text.Equals(gloss))
                        {
                            sense = s;
                            break;
                        }
                    }
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
			var toneParsList = possListRepository.AllInstances().FirstOrDefault(list => list.Name.BestAnalysisAlternative.Text == ToneParsConstants.ToneParsPropertiesList);
			var sb = new StringBuilder();
			foreach (var prop in toneParsList.PossibilitiesOS)
			{
				sb.Append("\\ap ");
				sb.Append(prop.Name.AnalysisDefaultWritingSystem.Text);
				sb.Append("\n");
			}
			return sb.ToString();
		}

		private void AddToneParsPropertiesToLexiconFile()
		{
			String xAmpleLexiconFile = Path.GetTempPath() + DatabaseName + kLexicon;
			String xAmpleLexicon = File.ReadAllText(xAmpleLexiconFile);
			var allomorphHvoPropertyMapper = new Dictionary<string, string> { };
			var morphemePropertyMapper = new Dictionary<string, string> { };
			var possListRepository = Cache.ServiceLocator.GetInstance<ICmPossibilityListRepository>();
			var toneParsList = possListRepository.AllInstances().FirstOrDefault(list => list.Name.BestAnalysisAlternative.Text == ToneParsConstants.ToneParsPropertiesList);
			BuildAllomorphPropertyMapper(allomorphHvoPropertyMapper, toneParsList);
			BuildMorphemePropertyMapper(morphemePropertyMapper, toneParsList);
			// Add allomorph properties
			var lexWithAlloProps = allomorphHvoPropertyMapper.Aggregate(xAmpleLexicon, (current, replacement) => current.Replace(replacement.Key, replacement.Value));
			// Add morpheme properties
			var lexWithAlloAndMorphProps = morphemePropertyMapper.Aggregate(lexWithAlloProps, (current, replacement) => current.Replace(replacement.Key, replacement.Value));

			String toneParsLexiconFile = Path.GetTempPath() + DatabaseName + kTPLexicon;
			File.WriteAllText(toneParsLexiconFile, lexWithAlloAndMorphProps);
		}

		private static void BuildAllomorphPropertyMapper(Dictionary<string, string> allomorphHvoPropertyMapper, ICmPossibilityList toneParsList)
		{
			foreach (var prop in toneParsList.PossibilitiesOS)
			{
				var refObjs = prop.ReferringObjects.Select(o => o).Where(o => !(o is ILexSense));
				foreach (ICmObject obj in refObjs)
				{
					var sHvo = obj.Hvo.ToString();
					if (!allomorphHvoPropertyMapper.ContainsKey(sHvo))
					{
						var hvoMatch = " {" + sHvo + "}";
						var replaceWith = hvoMatch + " " + prop.Name.AnalysisDefaultWritingSystem.Text;
						allomorphHvoPropertyMapper.Add(hvoMatch, replaceWith);
					}
				}
			}
		}

		private static void BuildMorphemePropertyMapper(Dictionary<string, string> morphemePropertyMapper, ICmPossibilityList toneParsList)
		{
			foreach (var prop in toneParsList.PossibilitiesOS)
			{
				var refObjs = prop.ReferringObjects.Select(o => o).Where(o => o is ILexSense);
				foreach (ICmObject obj in refObjs)
				{
					var sense = obj as ILexSense;
					var sHvo = sense.MorphoSyntaxAnalysisRA.Hvo.ToString();
					if (!morphemePropertyMapper.ContainsKey(sHvo))
					{
						var hvoMatch = "\\lx " + sHvo + "\r\n";
						var replaceWith = hvoMatch + "\\mp " + prop.Name.AnalysisDefaultWritingSystem.Text + "\r\n";
						morphemePropertyMapper.Add(hvoMatch, replaceWith);
					}
				}
			}
		}

		public void SaveResultsInDatabase()
		{
			var m_parseFiler = new ParseFiler(Cache, task => { }, Queue, Cache.LanguageProject.DefaultParserAgent);
			int i = 1;
			while (ConvertAntToParserFilerXML(i))
			{
				// call parser filer on
				//Console.WriteLine(ParserFilerXMLString);
				int wordformBegin = ParserFilerXMLString.IndexOf("Form=\"") + 6;
				int wordformEnd = ParserFilerXMLString.Substring(wordformBegin).IndexOf("\"");
				var wordform = ParserFilerXMLString.Substring(wordformBegin, wordformEnd);
				IWfiWordform thiswf = GetWordformFromString(wordform);
				if (thiswf != null)
				{
					var parseResult = ParseWord(ParserFilerXMLString);
					m_parseFiler.ProcessParse(thiswf, ParserPriority.Low, parseResult);
				}
				i++;
			}
			ExecuteIdleQueue(Queue);
		}

		// Used in Unit Testing
		public IWfiWordform GetWordformFromString(string wordform)
		{
			if (String.IsNullOrEmpty(wordform))
				return null;
			ILcmServiceLocator servLoc = Cache.ServiceLocator;
			IWfiWordform wf = servLoc.GetInstance<IWfiWordformRepository>().GetMatchingWordform(Cache.DefaultVernWs, wordform);
			if (wf == null)
			{
				NonUndoableUnitOfWorkHelper.Do(Cache.ActionHandlerAccessor, () =>
				{
					wf = servLoc.GetInstance<IWfiWordformFactory>().Create(TsStringUtils.MakeString(wordform, Cache.DefaultVernWs));
				});
			}
			return wf;
		}

		protected void ExecuteIdleQueue(IdleQueue idleQueue)
		{
			foreach (var task in idleQueue)
				task.Delegate(task.Parameter);
			idleQueue.Clear();
		}

		//-----------------------
		// ParseWord(), TryCreateParseMorph(), and ProcessMsaHvo() are from XAmpleParser.cs
		// Ideally, we'd expose them from XAmpleParser.cs
		public ParseResult ParseWord(string results)
		{
			//TODO: fix! CheckDisposed();

			results = results.Replace("DB_REF_HERE", "'0'");
			results = results.Replace("<...>", "[...]");
			var wordformElem = XElement.Parse(results.ToString());
			string errorMessage = null;
			var exceptionElem = wordformElem.Element("Exception");
			if (exceptionElem != null)
			{
				var totalAnalysesValue = (string)exceptionElem.Attribute("totalAnalyses");
				switch ((string)exceptionElem.Attribute("code"))
				{
					case "ReachedMaxAnalyses":
						errorMessage = String.Format("Maximum permitted analyses ({0}) reached." /*ParserCoreStrings.ksReachedMaxAnalysesAllowed*/,
							totalAnalysesValue);
						break;
					case "ReachedMaxBufferSize":
						errorMessage = String.Format("Maximum internal buffer size ({0}) reached." /*ParserCoreStrings.ksReachedMaxInternalBufferSize*/,
							totalAnalysesValue);
						break;
				}
			}
			else
			{
				errorMessage = (string)wordformElem.Element("Error");
			}

			ParseResult result;
			using (new WorkerThreadReadHelper(Cache.ServiceLocator.GetInstance<IWorkerThreadReadHandler>()))
			{
				var analyses = new List<ParseAnalysis>();
				foreach (XElement analysisElem in wordformElem.Descendants("WfiAnalysis"))
				{
					var morphs = new List<ParseMorph>();
					bool skip = false;
					foreach (XElement morphElem in analysisElem.Descendants("Morph"))
					{
						ParseMorph morph;
						if (!TryCreateParseMorph(Cache, morphElem, out morph))
						{
							skip = true;
							break;
						}
						if (morph != null)
							morphs.Add(morph);
					}

					if (!skip && morphs.Count > 0)
						analyses.Add(new ParseAnalysis(morphs));
				}
				result = new ParseResult(analyses, errorMessage);
			}

			return result;
		}

		private static bool TryCreateParseMorph(LcmCache cache, XElement morphElem, out ParseMorph morph)
		{
			XElement formElement = morphElem.Element("MoForm");
			Debug.Assert(formElement != null);
			var formHvo = (string)formElement.Attribute("DbRef");

			XElement msiElement = morphElem.Element("MSI");
			Debug.Assert(msiElement != null);
			var msaHvo = (string)msiElement.Attribute("DbRef");

			// Normally, the hvo for MoForm is a MoForm and the hvo for MSI is an MSA
			// There are four exceptions, though, when an irregularly inflected form is involved:
			// 1. <MoForm DbRef="x"... and x is an hvo for a LexEntryInflType.
			//       This is one of the null allomorphs we create when building the
			//       input for the parser in order to still get the Word Grammar to have something in any
			//       required slots in affix templates.  The parser filer can ignore these.
			// 2. <MSI DbRef="y"... and y is an hvo for a LexEntryInflType.
			//       This is one of the null allomorphs we create when building the
			//       input for the parser in order to still get the Word Grammar to have something in any
			//       required slots in affix templates.  The parser filer can ignore these.
			// 3. <MSI DbRef="y"... and y is an hvo for a LexEntry.
			//       The LexEntry is a variant form for the first set of LexEntryRefs.
			// 4. <MSI DbRef="y"... and y is an hvo for a LexEntry followed by a period and an index digit.
			//       The LexEntry is a variant form and the (non-zero) index indicates
			//       which set of LexEntryRefs it is for.
			ICmObject objForm;
			if (!cache.ServiceLocator.GetInstance<ICmObjectRepository>().TryGetObject(int.Parse(formHvo), out objForm))
			{
				morph = null;
				return false;
			}
			var form = objForm as IMoForm;
			if (form == null)
			{
				morph = null;
				return true;
			}

			// Irregulary inflected forms can have a combination MSA hvo: the LexEntry hvo, a period, and an index to the LexEntryRef
			Tuple<int, int> msaTuple = ProcessMsaHvo(msaHvo);
			ICmObject objMsa;
			if (!cache.ServiceLocator.GetInstance<ICmObjectRepository>().TryGetObject(msaTuple.Item1, out objMsa))
			{
				morph = null;
				return false;
			}
			var msa = objMsa as IMoMorphSynAnalysis;
			if (msa != null)
			{
				morph = new ParseMorph(form, msa);
				return true;
			}

			var msaAsLexEntry = objMsa as ILexEntry;
			if (msaAsLexEntry != null)
			{
				// is an irregularly inflected form
				// get the MoStemMsa of its variant
				if (msaAsLexEntry.EntryRefsOS.Count > 0)
				{
					ILexEntryRef lexEntryRef = msaAsLexEntry.EntryRefsOS[msaTuple.Item2];
					ILexSense sense = MorphServices.GetMainOrFirstSenseOfVariant(lexEntryRef);
					var inflType = lexEntryRef.VariantEntryTypesRS[0] as ILexEntryInflType;
					morph = new ParseMorph(form, sense.MorphoSyntaxAnalysisRA, inflType);
					return true;
				}
			}

			// if it is anything else, we ignore it
			morph = null;
			return true;
		}

		private static Tuple<int, int> ProcessMsaHvo(string msaHvo)
		{
			string[] msaHvoParts = msaHvo.Split('.');
			return Tuple.Create(int.Parse(msaHvoParts[0]), msaHvoParts.Length == 2 ? int.Parse(msaHvoParts[1]) : 0);
		}
		// -----------------------
	}
    public static class ToneParsConstants
    {
        public const string ToneParsPropertiesSenseCustomField = "ToneParsSense";
        public const string ToneParsPropertiesFormCustomField = "ToneParsForm";
        public const string ToneParsPropertiesList = "TonePars Properties";
    }
}
