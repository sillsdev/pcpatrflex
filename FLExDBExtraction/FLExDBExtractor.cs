// Copyright (c) 2018 SIL International
// This software is licensed under the LGPL, version 2.1 or later
// (http://www.gnu.org/licenses/lgpl-2.1.html)

using SIL.LCModel;
using SIL.PrepFLExDB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIL.FLExDBExtraction
{
    public class FLExDBExtractor
	{
		public LcmCache Cache { get; set; }

		ICmPossibilityList PcpatrList { get; set; }

		FieldDescription CustomField { get; set; }

		public FLExDBExtractor(LcmCache cache)
		{
			Cache = cache;

			var possListRepository = Cache.ServiceLocator.GetInstance<ICmPossibilityListRepository>();
			PcpatrList = possListRepository.AllInstances().FirstOrDefault(list => list.Name.BestAnalysisAlternative.Text == Constants.PcPatrFeatureDescriptorList);

			var customFields = GetListOfCustomFields();
			CustomField = customFields.Find(fd => fd.Name == Constants.PcPatrFeatureDescriptorCustomField);

		}

		public string ExtractPcPatrLexicon()
		{
			var sb = new StringBuilder();
			var lexEntries = Cache.LanguageProject.LexDbOA.Entries;
			foreach (ILexEntry entry in lexEntries.OrderBy(e => e.ShortName))
			{
				formatEntry(entry, sb);
			}
			return sb.ToString();
		}

		protected void formatEntry(ILexEntry entry, StringBuilder sb)
		{
			sb.Append("\\w ");
			sb.Append(entry.LexemeFormOA.Form.BestVernacularAlternative.Text);
			sb.Append("\n\\c ");
			var sense = entry.SensesOS.FirstOrDefault<ILexSense>();
			var msa = (IMoStemMsa)sense.MorphoSyntaxAnalysisRA;
			var pos = msa.PartOfSpeechRA;
			sb.Append(pos.Name.BestAnalysisAlternative.Text);
			sb.Append("\n\\g ");
			sb.Append(sense.Gloss.BestAnalysisAlternative.Text);
			sb.Append("\n\\f");
			if (CustomField != null)
			{
				IList<string> fds = new List<string>() { };
				var size = Cache.MainCacheAccessor.get_VecSize(sense.Hvo, CustomField.Id);
				for (int i = 0; i <size; i++)
				{
					var hvo = Cache.MainCacheAccessor.get_VecItem(sense.Hvo, CustomField.Id, i);
					if (PcpatrList != null)
					{
						var item = PcpatrList.PossibilitiesOS.Where(ps => ps.Hvo == hvo);
						var fd = item.ElementAt(0).Name.BestAnalysisAlternative.Text;
						fds.Add(fd);
					}
				}
				fds = fds.OrderBy(fd => fd).ToList();
				foreach (string fd in fds)
				{
					sb.Append(" " + fd);
				}
			}
			sb.Append("\n\n");
		}

		public List<FieldDescription> GetListOfCustomFields()
		{
			return (from fd in FieldDescription.FieldDescriptors(Cache)
					where fd.IsCustomField //&& GetItem(m_locationComboBox, fd.Class) != null
					select fd).ToList();
		}

	}
}
