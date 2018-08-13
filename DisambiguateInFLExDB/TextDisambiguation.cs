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
	public class TextDisambiguation : Disambiguation
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
			var pcpatrAgent = GetPCPATRSyntacticParsingAgent(cache);
			NonUndoableUnitOfWorkHelper.Do(cache.ActionHandlerAccessor, () =>
			{
				// run through each segment in the text
				// if there is a guid bundle for the segment in the text, use it
				// otherwise find the \a, etc. items in the And File
				// if there are no % in all the \p fields, use that set of guids
			});
		}


	}
}
