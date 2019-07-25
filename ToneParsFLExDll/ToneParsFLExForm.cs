// Copyright (c) 2019 SIL International
// This software is licensed under the LGPL, version 2.1 or later
// (http://www.gnu.org/licenses/lgpl-2.1.html)

using Microsoft.Win32;
using SIL.DisambiguateInFLExDB;
using SIL.FieldWorks.LexText.Controls;
using SIL.FieldWorks.WordWorks.Parser;
using SIL.LCModel;
using SIL.LCModel.Core.Text;
using SIL.LCModel.Core.WritingSystems;
using SIL.LCModel.DomainServices;
using SIL.PrepFLExDB;
using SIL.WritingSystems;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using XCore;

namespace SIL.ToneParsFLEx
{
	public partial class ToneParsFLExForm : Form
	{
		public LcmCache Cache { get; set; }

		protected IList<IText> Texts { get; set; }
		protected IList<SegmentToShow> SegmentsInListBox { get; set; }
		protected FLExDBExtractor Extractor { get; set; }
		protected Font AnalysisFont { get; set; }
		protected Font VernacularFont { get; set; }

		private String ToneRuleFile { get; set; }
		private String IntxCtlFile { get; set; }

		public static string m_strRegKey = "Software\\SIL\\ToneParsFLEx";
		const string m_strLastDatabase = "LastDatabase";
		const string m_strLastToneRuleFile = "LastToneRuleFile";
		const string m_strLastIntxCtlFile = "LastIntxCtlFile";
		const string m_strLastText = "LastText";
		const string m_strLastSegment = "LastSegment";
		const string m_strLastTraceOptions = "LastTraceOptions";
		const string m_strLocationX = "LocationX";
		const string m_strLocationY = "LocationY";
		const string m_strSizeHeight = "SizeHeight";
		const string m_strSizeWidth = "SizeWidth";
		const string m_strWindowState = "WindowState";

		const string m_strAll = "All";
		const string m_strLeftmost = "Leftmost";
		const string m_strOff = "Off";
		const string m_strRightmost = "Rightmos";

		public Rectangle RectNormal { get; set; }

		public string LastDatabase { get; set; }
		public string LastToneRuleFile { get; set; }
		public string LastIntxCtlFile { get; set; }
		public string LastText { get; set; }
		public string LastSegment { get; set; }
		public string LastTraceOption { get; set; }
		public string RetrievedLastText { get; set; }
		public string RetrievedLastSegment { get; set; }

		private RegistryKey regkey;
		private ParserConnection m_parserConnection;

		private ContextMenuStrip helpContextMenu;
		const string UserDocumentation = "User Documentation";
		const string ToneParsDocumentation = "TonePars Documentation";
		const string About = "About";

		public ToneParsFLExForm()
		{
			InitializeComponent();
			btnParse.Enabled = false;

			splitContainer1.Dock = DockStyle.Bottom;
			splitContainer1.SplitterWidth = 3;

			lbSegments.DisplayMember = "Baseline";
			lbSegments.ValueMember = "Segment";

			BuildHelpContextMenu();

			try
			{
				regkey = Registry.CurrentUser.OpenSubKey(m_strRegKey);
				if (regkey != null)
				{
					Cursor.Current = Cursors.WaitCursor;
					Application.DoEvents();
					retrieveRegistryInfo();
					regkey.Close();
					DesktopBounds = RectNormal;
					WindowState = WindowState;
					StartPosition = FormStartPosition.Manual;
					if (!String.IsNullOrEmpty(LastToneRuleFile))
						tbGrammarFile.Text = LastToneRuleFile;
					if (!String.IsNullOrEmpty(LastIntxCtlFile))
						tbIntxCtlFile.Text = LastIntxCtlFile;
					//if (!String.IsNullOrEmpty(LastTraceOption))
					//{
					//	switch (LastTraceOption)
					//	{
					//		case m_strLeftmost:
					//			rbLeftmost.Checked = true;
					//			break;
					//		case m_strRightmost:
					//			rbRightmost.Checked = true;
					//			break;
					//		case m_strAll:
					//			rbAll.Checked = true;
					//			break;
					//		default:
					//			rbOff.Checked = true;
					//			break;
					//	}
					//}
					if (Cache != null && Cache.LangProject.Texts.Count > 0)
						btnDisambiguate.Enabled = true;
					else
						btnDisambiguate.Enabled = false;
					Cursor.Current = Cursors.Default;
				}
				AdjustSplitterLocation();
			}
			catch (Exception e)
			{
				Console.WriteLine(e.Message);
				Console.WriteLine(e.InnerException);
				Console.WriteLine(e.StackTrace);
				MessageBox.Show(e.Message + e.InnerException + e.StackTrace);
			}
		}

		private void AdjustSplitterLocation()
		{
			int x = splitContainer1.Location.X;
			// Make the Y location be where the bottom of the disambiguate
			//  button is plus a gap of 15 pixels
			int y = this.ClientSize.Height - (btnDisambiguate.Bottom + 15);
			splitContainer1.Size = new Size(x, y);
		}

		private void BuildHelpContextMenu()
		{
			helpContextMenu = new ContextMenuStrip();
			ToolStripMenuItem userDoc = new ToolStripMenuItem(UserDocumentation);
			userDoc.Click += new EventHandler(UserDoc_Click);
			userDoc.Name = UserDocumentation;
			ToolStripMenuItem pcpatrDoc = new ToolStripMenuItem(ToneParsDocumentation);
			pcpatrDoc.Click += new EventHandler(PCPATRDoc_Click);
			pcpatrDoc.Name = ToneParsDocumentation;
			ToolStripMenuItem about = new ToolStripMenuItem(About);
			about.Click += new EventHandler(About_Click);
			about.Name = About;
			helpContextMenu.Items.Add(userDoc);
			helpContextMenu.Items.Add(pcpatrDoc);
			helpContextMenu.Items.Add("-");
			helpContextMenu.Items.Add(about);
		}

		void retrieveRegistryInfo()
		{
			// Window location
			int iX = (int)regkey.GetValue(m_strLocationX, 100);
			int iY = (int)regkey.GetValue(m_strLocationY, 100);
			int iWidth = (int)regkey.GetValue(m_strSizeWidth, 809); // 1228);
			int iHeight = (int)regkey.GetValue(m_strSizeHeight, 670); // 947);
			RectNormal = new Rectangle(iX, iY, iWidth, iHeight);
			// Set form properties
			WindowState = (FormWindowState)regkey.GetValue(m_strWindowState, 0);

			LastDatabase = (string)regkey.GetValue(m_strLastDatabase);
			ToneRuleFile = LastToneRuleFile = (string)regkey.GetValue(m_strLastToneRuleFile);
			IntxCtlFile = LastIntxCtlFile = (string)regkey.GetValue(m_strLastIntxCtlFile);
			RetrievedLastText = LastText = (string)regkey.GetValue(m_strLastText);
			RetrievedLastSegment = LastSegment = (string)regkey.GetValue(m_strLastSegment);
			LastTraceOption = (string)regkey.GetValue(m_strLastTraceOptions);
		}
		public void saveRegistryInfo()
		{
			regkey = Registry.CurrentUser.OpenSubKey(m_strRegKey, true);
			if (regkey == null)
			{
				regkey = Registry.CurrentUser.CreateSubKey(m_strRegKey);
			}

			if (LastDatabase != null)
				regkey.SetValue(m_strLastDatabase, LastDatabase);
			if (LastToneRuleFile != null)
				regkey.SetValue(m_strLastToneRuleFile, LastToneRuleFile);
			if (LastIntxCtlFile != null)
				regkey.SetValue(m_strLastIntxCtlFile, LastIntxCtlFile);
			if (LastText != null)
				regkey.SetValue(m_strLastText, LastText);
			if (LastSegment != null)
				regkey.SetValue(m_strLastSegment, LastSegment);
			if (LastTraceOption != null)
				regkey.SetValue(m_strLastTraceOptions, LastTraceOption);
			// Window position and location
			regkey.SetValue(m_strWindowState, (int)WindowState);
			regkey.SetValue(m_strLocationX, RectNormal.X);
			regkey.SetValue(m_strLocationY, RectNormal.Y);
			regkey.SetValue(m_strSizeWidth, RectNormal.Width);
			regkey.SetValue(m_strSizeHeight, RectNormal.Height);

			regkey.Close();
		}

		private static Font CreateFont(CoreWritingSystemDefinition wsDef)
		{
			float fontSize = (wsDef.DefaultFontSize == 0) ? 10 : wsDef.DefaultFontSize;
			var fStyle = FontStyle.Regular;
			if (wsDef.DefaultFontFeatures.Contains("Bold"))
			{
				fStyle |= FontStyle.Bold;
			}
			if (wsDef.DefaultFontFeatures.Contains("Italic"))
			{
				fStyle |= FontStyle.Italic;
			}
			return new Font(wsDef.DefaultFontName, fontSize, fStyle);
		}

		private void EnsureDatabaseHasBeenPrepped()
		{
			var preparer = new Preparer(Cache, false);
			preparer.AddToneParsList();
			preparer.AddToneParsFormCustomField();
			preparer.AddToneParsSenseCustomField();
		}

		public void PrepareForm()
		{
			if (Cache != null)
			{
				EnsureDatabaseHasBeenPrepped();
				Extractor = new FLExDBExtractor(Cache);
				AnalysisFont = CreateFont(Cache.LanguageProject.DefaultAnalysisWritingSystem);
				lbTexts.Font = AnalysisFont;
				VernacularFont = CreateFont(Cache.LanguageProject.DefaultVernacularWritingSystem);
				lbSegments.Font = VernacularFont;
				lbSegments.RightToLeft = Cache.LanguageProject.DefaultVernacularWritingSystem.RightToLeftScript ? RightToLeft.Yes : RightToLeft.No;
				FillTextsListBox();
			}
		}

		public void FillTextsListBox()
		{
			Texts = Cache.LanguageProject.Texts.Where(t => t.ContentsOA != null).Cast<IText>().
				OrderBy(t => t.ShortName).ToList();
			lbTexts.DataSource = Texts;
			if (Texts.Count > 0)
				btnDisambiguate.Enabled = true;
			// select last used text and segment, if any
			if (!String.IsNullOrEmpty(LastText))
			{
				var selectedText = Texts.Where(t => t.Guid.ToString() == RetrievedLastText).FirstOrDefault();
				if (selectedText != null)
				{
					lbTexts.SelectedIndex = Texts.IndexOf(selectedText);
				}
				var selectedSegment = SegmentsInListBox.Where(s => s.Segment.Guid.ToString() == RetrievedLastSegment).FirstOrDefault();
				if (selectedSegment != null)
				{
					lbSegments.SelectedIndex = SegmentsInListBox.IndexOf(selectedSegment);
				}
			}
		}

		private void OnFormClosing(object sender, EventArgs e)
		{
			saveRegistryInfo();
			if (m_parserConnection != null)
			{
				m_parserConnection.Dispose();
			}
			m_parserConnection = null;
		}

		private void Texts_SelectedIndexChanged(object sender, EventArgs e)
		{
			SegmentsInListBox = new List<SegmentToShow>();
			IText text = lbTexts.SelectedItem as IText;
			LastText = text.Guid.ToString();
			var contents = text.ContentsOA;
			IList<IStPara> paragraphs = contents.ParagraphsOS;
			foreach (IStPara para in paragraphs)
			{
				var paraUse = para as IStTxtPara;
				if (paraUse != null)
				{
					foreach (var segment in paraUse.SegmentsOS)
					{
						SegmentsInListBox.Add(new SegmentToShow(segment, segment.BaselineText.Text));
					}
				}
			}
			lbSegments.DataSource = SegmentsInListBox;
		}

		private void Parse_Click(object sender, EventArgs e)
		{
			Cursor.Current = Cursors.WaitCursor;
			Application.DoEvents();
			var selectedSegmentToShow = (SegmentToShow)lbSegments.SelectedItem;
			var inputFile = Path.Combine(Path.GetTempPath(), "ToneParsInvoker.txt");
			File.WriteAllText(inputFile, selectedSegmentToShow.Baseline);
			var invoker = new ToneParsInvoker(tbGrammarFile.Text, tbIntxCtlFile.Text, inputFile, "", Cache);
			if (ConnectToParser(invoker.Queue))
			{
				m_parserConnection.ReloadGrammarAndLexicon();
				WaitForLoadToFinish();
			}
			invoker.DecompSeparationChar = GetDecompSeparationCharacter();
			invoker.Invoke();
			invoker.SaveResultsInDatabase();
			Cursor.Current = Cursors.Default;
		}

		private void WaitForLoadToFinish()
		{
			while (m_parserConnection.GetQueueSize(ParserPriority.ReloadGrammarAndLexicon) > 0)
			{
				Thread.Sleep(250);
			}
		}

		public bool ConnectToParser(IdleQueue idleQueue)
		{
			//CheckDisposed();

			if (m_parserConnection == null)
			{
				// Don't bother if the lexicon is empty.  See FWNX-1019.
				if (Cache.ServiceLocator.GetInstance<ILexEntryRepository>().Count == 0)
				{
					//MessageBox.Show("ConnectToParser returns false");
					return false;
				}
				m_parserConnection = new ParserConnection(Cache, idleQueue);
			}
			//StartProgressUpdateTimer();
			//MessageBox.Show("ConnectToParser returns true");
			return true;
		}

		private char GetDecompSeparationCharacter()
		{
			String textInptControlFileContents = File.ReadAllText(tbIntxCtlFile.Text);
			string decompSeparationCharacter = ToneParsInvoker.GetFieldFromAntRecord(textInptControlFileContents, "\\dsc ").Trim();
			return decompSeparationCharacter[0];
		}

		private void DisambiguateSegment(SegmentToShow selectedSegmentToShow, string result)
		{
			if (!String.IsNullOrEmpty(result))
			{
				var guids = DisambiguateInFLExDB.GuidConverter.CreateListFromString(result);
				var disambiguator = new SegmentDisambiguation(selectedSegmentToShow.Segment, guids);
				disambiguator.Disambiguate(Cache);
			}
		}

		private string GetAnaForm(SegmentToShow selectedSegmentToShow)
		{
			var segment = selectedSegmentToShow.Segment;
			var ana = Extractor.ExtractTextSegmentAsANA(segment);
			return ana;
		}

		private string GetAnaForm(IText selectedTextToShow)
		{
			var sb = new StringBuilder();
			var contents = selectedTextToShow.ContentsOA;
			IList<IStPara> paragraphs = contents.ParagraphsOS;
			foreach (IStPara para in paragraphs)
			{
				var paraUse = para as IStTxtPara;
				if (paraUse != null)
				{
					foreach (var segment in paraUse.SegmentsOS)
					{
						var ana = Extractor.ExtractTextSegmentAsANA(segment);
						sb.Append(ana.Substring(0, ana.Length-1)); // skip final extra nl
						// Now add period so PcPatr will treat it as an end of a sentence
						sb.Append("\\n .\n\n");
					}
				}
			}
			return sb.ToString();
		}

		private void Segments_SelectedIndexChanged(object sender, EventArgs e)
		{
			var selectedSegmentToShow = (SegmentToShow)lbSegments.SelectedItem;
			LastSegment = selectedSegmentToShow.Segment.Guid.ToString();
			var ana = GetAnaForm(selectedSegmentToShow);
			if (ana.Contains("\\a \n"))
			{
				btnParse.Enabled = false;
			}
			else
			{
				btnParse.Enabled = true;
			}
			btnParse.Enabled = true;
		}

		private void Browse_Click(object sender, EventArgs e)
		{
			OpenFileDialog dlg = new OpenFileDialog();
			dlg.Filter = "Tone Parse Rule File (*TP.ctl)|*.ctl|" +
			"All Files (*.*)|*.*";
			if (dlg.ShowDialog() == DialogResult.OK)
			{
				ToneRuleFile = dlg.FileName;
				LastToneRuleFile = ToneRuleFile;
				tbGrammarFile.Text = ToneRuleFile;
			}
		}

		private void btnBrowseIntxCtl_Click(object sender, EventArgs e)
		{
			OpenFileDialog dlg = new OpenFileDialog();
			dlg.Filter = "AMPLE Input Text Control File (*intx.ctl)|*.ctl|" +
			"All Files (*.*)|*.*";
			if (dlg.ShowDialog() == DialogResult.OK)
			{
				IntxCtlFile = dlg.FileName;
				LastIntxCtlFile = IntxCtlFile;
				tbIntxCtlFile.Text = IntxCtlFile;
			}
		}

		protected override void OnMove(EventArgs ea)
		{
			base.OnMove(ea);

			if (WindowState == FormWindowState.Normal)
				RectNormal = DesktopBounds;
		}
		protected override void OnResize(EventArgs ea)
		{
			base.OnResize(ea);

			if (WindowState == FormWindowState.Normal)
				RectNormal = DesktopBounds;
			AdjustSplitterLocation();
		}

		private void splitContainer1_SplitterMoved(object sender, SplitterEventArgs e)
		{
			lblSegments.Left = e.SplitX + 10;
			btnParse.Left = lblSegments.Right + 10;
		}

		private void Disambiguate_Click(object sender, EventArgs e)
		{
			Cursor.Current = Cursors.WaitCursor;
			Application.DoEvents();
			var selectedTextToShow = lbTexts.SelectedItem as IText;
			//string ana = GetAnaForm(selectedTextToShow);
			//string andResult;
			//PcPatrBrowserApp browser;
			//var grammarOK = ProcessANAFileAndShowResults(ana, out andResult, out browser);
			//if (grammarOK)
			//{
			//	var textdisambiguator = new TextDisambiguation(selectedTextToShow, browser.PropertiesChosen, andResult);
			//	textdisambiguator.Disambiguate(Cache);
			//}
			Cursor.Current = Cursors.Default;
		}

		//private bool ProcessANAFileAndShowResults(string ana, out string andResult, out PcPatrBrowserApp browser)
		//{
		//	String anaFile = Path.Combine(Path.GetTempPath(), "Invoker.ana");
		//	File.WriteAllText(anaFile, ana);
		//	var invoker = new PCPatrInvoker(ToneRuleFile, anaFile, LastTraceOption);
		//	invoker.Invoke();
		//	andResult = invoker.AndFile;
		//	if (File.Exists(andResult))
		//	{
		//		browser = ShowPcPatrBrowser(andResult);
		//		return true;
		//	}
		//	else
		//	{
		//		browser = null;
		//		MessageBox.Show("The PC-PATR grammar file had an error in it and failed to load.\nWe will show the error log after you click on OK.\nPlease fix all errors in the grammar file and then try again.",
		//			"Grammar Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
		//		Process.Start(andResult.Replace(".and", ".log"));
		//		return false;
		//	}
		//}

		private void btnHelp_Click(object sender, EventArgs e)
		{
			Button btnSender = (Button)sender;
			Point ptLowerLeft = new Point(0, btnSender.Height);
			ptLowerLeft = btnSender.PointToScreen(ptLowerLeft);
			helpContextMenu.Show(ptLowerLeft);
		}

		void UserDoc_Click(object sender, EventArgs e)
		{
			ToolStripItem menuItem = (ToolStripItem)sender;
			if (menuItem.Name == UserDocumentation)
			{
				String basedir = GetAppBaseDir();
				Process.Start(Path.Combine(basedir, "doc", "UserDocumentation.pdf"));
			}
		}

		private static string GetAppBaseDir()
		{
			string basedir;
			string rootdir;
			int indexOfBinInPath;
			DetermineIndexOfBinInExecutablesPath(out rootdir, out indexOfBinInPath);
			if (indexOfBinInPath >= 0)
				basedir = rootdir.Substring(0, indexOfBinInPath);
			else
				basedir = rootdir;
			return basedir;
		}

		private static void DetermineIndexOfBinInExecutablesPath(out string rootdir, out int indexOfBinInPath)
		{
			Uri uriBase = new Uri(Assembly.GetExecutingAssembly().CodeBase);
			rootdir = Path.GetDirectoryName(Uri.UnescapeDataString(uriBase.AbsolutePath));
			indexOfBinInPath = rootdir.LastIndexOf("bin");
		}

		void PCPATRDoc_Click(object sender, EventArgs e)
		{
			ToolStripItem menuItem = (ToolStripItem)sender;
			if (menuItem.Name == ToneParsDocumentation)
			{
				String basedir = GetAppBaseDir();
				Process.Start(Path.Combine(basedir, "doc", "pcpatr.html"));
			}
		}

		void About_Click(object sender, EventArgs e)
		{
			ToolStripItem menuItem = (ToolStripItem)sender;
			if (menuItem.Name == About)
			{
				var dialog = new AboutBox();
				// for some reason the following is needed to keep the dialog within the form
				Point pt = dialog.PointToClient(System.Windows.Forms.Cursor.Position);
				dialog.Location = new Point(this.Location.X + 20, this.Location.Y + 20);
				dialog.Show();
			}
		}

		//private void rbOff_CheckedChanged(object sender, EventArgs e)
		//{
		//	LastTraceOption = m_strOff;
		//}

		//private void rbLeftmost_CheckedChanged(object sender, EventArgs e)
		//{
		//	LastTraceOption = m_strLeftmost;
		//}

		//private void rbRightmost_CheckedChanged(object sender, EventArgs e)
		//{
		//	LastTraceOption = m_strRightmost;
		//}

		//private void rbAll_CheckedChanged(object sender, EventArgs e)
		//{
		//	LastTraceOption = m_strAll;
		//}

	}

	public class SegmentToShow
	{
		public ISegment Segment { get; set; }
		public String Baseline { get; set; }

		public SegmentToShow(ISegment segment, string baseline)
		{
			Segment = segment;
			Baseline = baseline;
		}
	}
}
