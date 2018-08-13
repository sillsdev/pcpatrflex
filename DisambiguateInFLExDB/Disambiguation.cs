// Copyright (c) 2018 SIL International
// This software is licensed under the LGPL, version 2.1 or later
// (http://www.gnu.org/licenses/lgpl-2.1.html)

using SIL.LcmLoaderUI;
using SIL.LCModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIL.DisambiguateInFLExDB
{
	public class Disambiguation
	{

		public ICmAgent GetPCPATRSyntacticParsingAgent(LcmCache cache)
		{
			var agents = cache.LangProject.AnalyzingAgentsOC;
			var pcpatrAgents = agents.Where(a => a.Name.BestAnalysisAlternative.Text == Constants.PcPatrSyntacticParser);
			if (pcpatrAgents.Count() > 0)
			{
				return pcpatrAgents.First();
			}
			else
			{
				return null;
			}
		}


	}
}
