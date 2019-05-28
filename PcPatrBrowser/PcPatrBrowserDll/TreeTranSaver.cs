// Copyright (c) 2019 SIL International
// This software is licensed under the LGPL, version 2.1 or later
// (http://www.gnu.org/licenses/lgpl-2.1.html)

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace SIL.PcPatrBrowser
{
	/// <summary>
	/// Save selected parses to an ANA file for input to TreeTran.
	/// </summary>
	public class TreeTranSaver
	{
		private PcPatrDocument m_doc;
		int iRecord = 0;
		int iMParse = 0;
		int iMorph = 0;

		public TreeTranSaver(PcPatrDocument doc)
		{
			m_doc = doc;
		}

		public string CreateANAFromParsesChosen(int[] parsesChosen)
		{
			var sb = new StringBuilder();
			var sbInputWords = new StringBuilder();
			sb.Append("<?xml version=\"1.0\" encoding=\"UTF-8\"?>\n");
			sb.Append("<XAmpleANA>\n");
			var sentence = m_doc.FirstSentence;
			//Console.Out.WriteLine("in CreateANAFromParsesChosen");
			for (int iSent=0; iSent < m_doc.NumberOfSentences; iSent++)
			{
				sbInputWords.Clear();
				//Console.Out.WriteLine("iSent=" + iSent);
				//Console.Out.WriteLine("sentence=" + sentence.Node.OuterXml);
				if (parsesChosen[iSent] == 0 || sentence.Parses.Length == 0)
				{
					// output all parses
					//sentence.
				}
				else
				{
					// output selected parse
					var parse = sentence.GoToParse(parsesChosen[iSent]);
					var leaves = parse.Node.SelectNodes(".//Leaf");
					XmlNode wordParse = null;
					XmlNode origWord = null;
					// a d cat p fd w n
					for (int iLeaf = 0; iLeaf < leaves.Count; iLeaf++)
					{
						iRecord++;
						iMParse = 1;
						var leaf = leaves.Item(iLeaf);
						wordParse = FindWordParseToUse(sentence, iLeaf, leaf);
						//Console.Out.WriteLine("wordparse=" + wordParse.OuterXml);
						sb.Append("<anaRec id=\"anaRec");
						sb.Append(iRecord);
						sb.Append("\">\n");
						sb.Append("<w id=\"w");
						sb.Append(iRecord);
						sb.Append("\">");
						origWord = wordParse.SelectSingleNode("../OrigWord");
						sb.Append(origWord.InnerText);
						sb.Append("</w>\n");
						var recordParseID = iRecord + "." + iMParse;
						sb.Append("<mparse id=\"mparse");
						sb.Append(recordParseID);
						sb.Append("\">\n");
						sb.Append("<a id=\"a");
						sb.Append(recordParseID);
						sb.Append("\">\n");
						OutputMorphItems(sb, wordParse, recordParseID);
						sb.Append("</a>\n");
						sb.Append("<cat id=\"cat");
						sb.Append(recordParseID);
						sb.Append("\">");
						var wordCat = wordParse.SelectSingleNode("./WordCat");
						sb.Append(wordCat.InnerText);
						sb.Append("</cat>\n");
						sb.Append("</mparse>\n");
						var nonAlpha = wordParse.SelectSingleNode("../NonAlpha");
						if (nonAlpha != null)
						{
							sb.Append("<n id=\"n");
							sb.Append(iRecord);
							sb.Append("\">");
							var na = nonAlpha.InnerText;
							if (na.Length > 0)
							{
								sb.Append(nonAlpha.InnerText);
							}
							else
							{
								sb.Append(" ");
							}
							sb.Append("</n>\n");
						}
						sb.Append("</anaRec>\n");
						sbInputWords.Append("<Word id=\"word");
						sbInputWords.Append(iRecord);
						sbInputWords.Append("\">\n");
						if (origWord != null)
						{
							sbInputWords.Append(InsertNewLines(origWord.OuterXml));
							sbInputWords.Append("\n");
						}
						//sbInputWords.Append("\n");
						if (wordParse != null)
						{
							sbInputWords.Append(InsertNewLines(wordParse.OuterXml));
							sbInputWords.Append("\n");
						}
						//if (wordCat != null)
						//{
						//	sbInputWords.Append(InsertNewLines(wordCat.OuterXml));
						//	sbInputWords.Append("\n");
						//}
						if (nonAlpha != null)
						{
							sbInputWords.Append(InsertNewLines(nonAlpha.OuterXml));
							sbInputWords.Append("\n");
						}
						sbInputWords.Append("</Word>\n");
					}
					sb.Append("<Analysis count=\"1\" >\n");
					sb.Append("<Input>\n");
					//sb.Append("<Word id =\"word1\">\n");
					//if (origWord != null)
					//{
					//	sb.Append(InsertNewLines(origWord.OuterXml));
					//}
					//// output the selected word with the selected wordparse
					//if (wordParse != null)
					//{
					//	sb.Append(InsertNewLines(wordParse.OuterXml));
					//	sb.Append("\n");
					//}
					//var nonAlpha = wordParse.SelectSingleNode("../NonAlpha");
					//if (nonAlpha != null)
					//{
					//	sb.Append(InsertNewLines(nonAlpha.OuterXml));
					//	sb.Append("\n");
					//}
					//sb.Append("</Word>\n");
					sb.Append(sbInputWords.ToString());
					sb.Append("</Input>\n");
					if (parse != null && parse.Node != null)
					{
						sb.Append(InsertNewLines(parse.Node.OuterXml));
						sb.Append("\n");
					}
					sb.Append("</Analysis>\n");
				}
				sentence = m_doc.NextSentence;
			}
			sb.Append("</XAmpleANA>\n");
			return sb.ToString();
		}

		private void OutputMorphItems(StringBuilder sb, XmlNode wordParse, string recordParseID)
		{
			var morphs = wordParse.SelectSingleNode("./Morphs");
			string outMtype = "";
			iMorph = 1;
			foreach (XmlNode morphHead in morphs.ChildNodes)
			{
				var mtype = morphHead.Name;
				switch (mtype)
				{
					case "Prefix":
						outMtype = "pfx";
						break;
					case "Root":
						outMtype = "root";
						break;
					case "Suffix":
						outMtype = "sfx";
						break;
				}
				string morphID = OutputMorphTypeElementBegin(sb, recordParseID, outMtype);
				OutputMorphItem(sb, morphID, morphHead.SelectSingleNode("./Morph"), "m");
				OutputMorphItem(sb, morphID, morphHead.SelectSingleNode("./Decomp"), "d");
				OutputMorphItem(sb, morphID, morphHead.SelectSingleNode("./MorphCat"), "mcat");
				OutputMorphItem(sb, morphID, morphHead.SelectSingleNode("./Prop"), "p");
				OutputMorphItem(sb, morphID, morphHead.SelectSingleNode("./FeatDesc"), "fd");
				OutputMorphTypeElementEnd(sb, outMtype);
				iMorph++;
			}
		}

		private string OutputMorphTypeElementBegin(StringBuilder sb, string recordParseID, string outMtype)
		{
			sb.Append("<");
			sb.Append(outMtype);
			sb.Append(" id=\"");
			sb.Append(outMtype);
			var morphID = recordParseID + "." + iMorph;
			sb.Append(morphID);
			sb.Append("\">\n");
			return morphID;
		}

		private static void OutputMorphItem(StringBuilder sb, string morphID, XmlNode morphItem, string morphNodeName)
		{
			sb.Append("<");
			sb.Append(morphNodeName);
			sb.Append(" id=\"");
			sb.Append(morphNodeName);
			sb.Append(morphID);
			sb.Append("\">");
			sb.Append(morphItem.InnerText);
			sb.Append("</");
			sb.Append(morphNodeName);
			sb.Append(">\n");
		}

		private static void OutputMorphTypeElementEnd(StringBuilder sb, string outMtype)
		{
			sb.Append("</");
			sb.Append(outMtype);
			sb.Append(">\n");
		}

		XmlNode FindWordParseToUse(PcPatrSentence sentence, int iLeaf, XmlNode leaf)
		{
			// find the /Analysis/Input/Word/WordParse that corresponds to this Leaf node
			// we use the guids in the properties to find the WordParse that has the same
			// sequence of guids in it.  Hopefully, this is never non-unique.
			var props = leaf.SelectSingleNode("./Fs/F[@name='properties']/Str").InnerText;
			var propList = props.Split('=');
			var sbXPath = new StringBuilder();
			sbXPath.Append("/Analysis/Input/Word[" + (iLeaf + 1) + "]/WordParse[");
			for (int i = 1; i <= propList.Length; i++)
			{
				if (i > 1)
				{
					sbXPath.Append(" and ");
				}
				sbXPath.Append("descendant::Prop[");
				sbXPath.Append(i);
				sbXPath.Append("][.='");
				sbXPath.Append(propList[i - 1]);
				sbXPath.Append("']");
			}
			sbXPath.Append("]");
			var wordParse = sentence.Node.SelectSingleNode(sbXPath.ToString());
			return wordParse;
		}

		string ConstructAFieldFromLeafNode(XmlNode leaf)
		{
			var sb = new StringBuilder();
			return sb.ToString();
		}

		string InsertNewLines(string xml)
		{
			return xml.Replace("><", ">\n<");
		}
	}
}
