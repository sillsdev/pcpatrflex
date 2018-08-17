// Copyright (c) 2018 SIL International
// This software is licensed under the LGPL, version 2.1 or later
// (http://www.gnu.org/licenses/lgpl-2.1.html)

using SIL.LCModel;
using SIL.LCModel.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIL.DisambiguateInFLExDB
{
	public class TextDisambiguation
	{
		public IText Text { get; set; }
		public string[] GuidBundles { get; set; }
		public String AndFile { get; set; }

		public TextDisambiguation(IText text, string[] guidBundles, string andFile)
		{
			Text = text;
			GuidBundles = guidBundles;
			AndFile = andFile;
		}

		public void Disambiguate(LcmCache cache)
		{
			var istText = Text.ContentsOA as IStText;
			var andGuids = AndFileLoader.GetGuidsFromAndFile(AndFile);
			int guidIndex = 0;
			for (int i = 0; i < istText.ParagraphsOS.Count; i++)
			{
				var para = istText.ParagraphsOS.ElementAtOrDefault(i) as IStTxtPara;
				//Console.WriteLine("text='" + para.Contents.Text + "'");
				//Console.WriteLine("i=" + i + "; guidIndex=" + guidIndex);
				var segment = para.SegmentsOS.FirstOrDefault();
				if (segment == null)
					continue;
				if (guidIndex < GuidBundles.Length)
				{
					if (Disambguated(cache, segment, GuidBundles.ElementAtOrDefault(guidIndex)))
					{
						//Console.WriteLine("did guid bundles for " + guidIndex);
						guidIndex++;
						continue;
					}
				}
				if (guidIndex < andGuids.Length)
				{
					Disambguated(cache, segment, andGuids.ElementAtOrDefault(guidIndex));
					//Console.WriteLine("did and guids for " + guidIndex);
					guidIndex++;
				}
			}
		}

		private bool Disambguated(LcmCache cache, ISegment segment, string chosen)
		{
			if (!String.IsNullOrEmpty(chosen))
			{
				var guids = GuidConverter.CreateListFromString(chosen);
				var segmentDisam = new SegmentDisambiguation(segment, guids);
				segmentDisam.Disambiguate(cache);
				return true;
			}
			return false;
		}
	}
}
