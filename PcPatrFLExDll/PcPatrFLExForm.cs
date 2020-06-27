using Microsoft.Win32;
using SIL.DisambiguateInFLExDB;
using SIL.LCModel;
using SIL.LCModel.Core.Text;
using SIL.LCModel.Core.WritingSystems;
using SIL.LCModel.DomainServices;
using SIL.PcPatrBrowser;
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
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SIL.PcPatrFLEx
{
	public partial class PcPatrFLExForm : Form
	{
		public LcmCache Cache { get; set; }

		private IList<IText> Texts { get; set; }
		private IList<SegmentToShow> SegmentsInListBox { get; set; }
		private FLExDBExtractor Extractor { get; set; }
		private String GrammarFile { get; set; }
		private Font AnalysisFont { get; set; }
		private Font VernacularFont { get; set; }

		public static string m_strRegKey = "Software\\SIL\\PcPatrFLEx";
		const string m_strLastDatabase = "LastDatabase";
		const string m_strLastGrammarFile = "LastGrammarFile";
		const string m_strLastText = "LastText";
		const string m_strLastSegment = "LastSegment";
		const string m_strLastRootGlossSelection = "LastRootGlossSelection";
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
		public string LastGrammarFile { get; set; }
		public string LastText { get; set; }
		public string LastSegment { get; set; }
		public string LastRootGlossSelection { get; set; }
		public string RetrievedLastText { get; set; }
		public string RetrievedLastSegment { get; set; }

		private RegistryKey regkey;

		private ContextMenuStrip helpContextMenu;
		const string UserDocumentation = "User Documentation";
		const string PCPATRReferenceManual = "PC-PATR Reference Manual";
		const string About = "About";

		public PcPatrFLExForm()
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
					RetrieveRegistryInfo();
					regkey.Close();
					DesktopBounds = RectNormal;
					WindowState = WindowState;
					StartPosition = FormStartPosition.Manual;
					if (!String.IsNullOrEmpty(LastGrammarFile))
						tbGrammarFile.Text = LastGrammarFile;
					if (!String.IsNullOrEmpty(LastRootGlossSelection))
					{
						switch (LastRootGlossSelection)
						{
							case m_strLeftmost:
								rbLeftmost.Checked = true;
								break;
							case m_strRightmost:
								rbRightmost.Checked = true;
								break;
							case m_strAll:
								rbAll.Checked = true;
								break;
							default:
								rbOff.Checked = true;
								break;
						}
					}
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
			ToolStripMenuItem pcpatrDoc = new ToolStripMenuItem(PCPATRReferenceManual);
			pcpatrDoc.Click += new EventHandler(PCPATRDoc_Click);
			pcpatrDoc.Name = PCPATRReferenceManual;
			ToolStripMenuItem about = new ToolStripMenuItem(About);
			about.Click += new EventHandler(About_Click);
			about.Name = About;
			helpContextMenu.Items.Add(userDoc);
			helpContextMenu.Items.Add(pcpatrDoc);
			helpContextMenu.Items.Add("-");
			helpContextMenu.Items.Add(about);
		}

		void RetrieveRegistryInfo()
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
			GrammarFile = LastGrammarFile = (string)regkey.GetValue(m_strLastGrammarFile);
			RetrievedLastText = LastText = (string)regkey.GetValue(m_strLastText);
			RetrievedLastSegment = LastSegment = (string)regkey.GetValue(m_strLastSegment);
			LastRootGlossSelection = (string)regkey.GetValue(m_strLastRootGlossSelection);
		}
		public void SaveRegistryInfo()
		{
			regkey = Registry.CurrentUser.OpenSubKey(m_strRegKey, true);
			if (regkey == null)
			{
				regkey = Registry.CurrentUser.CreateSubKey(m_strRegKey);
			}

			if (LastDatabase != null)
				regkey.SetValue(m_strLastDatabase, LastDatabase);
			if (LastGrammarFile != null)
				regkey.SetValue(m_strLastGrammarFile, LastGrammarFile);
			if (LastText != null)
				regkey.SetValue(m_strLastText, LastText);
			if (LastSegment != null)
				regkey.SetValue(m_strLastSegment, LastSegment);
			if (LastRootGlossSelection != null)
				regkey.SetValue(m_strLastRootGlossSelection, LastRootGlossSelection);
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
			preparer.AddPCPATRList();
			preparer.AddPCPATRSenseCustomField();
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
			Console.WriteLine("form closing");
			SaveRegistryInfo();
		}

		private void Texts_SelectedIndexChanged(object sender, EventArgs e)
		{
			SegmentsInListBox = new List<SegmentToShow>();
			IText text = lbTexts.SelectedItem as IText;
			LastText = text.Guid.ToString();
			var contents = text.ContentsOA;
			IList<IStPara> paragraphs = contents.ParagraphsOS;
            if (paragraphs != null)
            {
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
            }
			lbSegments.DataSource = SegmentsInListBox;
		}

		private void Parse_Click(object sender, EventArgs e)
		{
			Cursor.Current = Cursors.WaitCursor;
			Application.DoEvents();
			var selectedSegmentToShow = (SegmentToShow)lbSegments.SelectedItem;
			string ana = GetAnaForm(selectedSegmentToShow);
			string andResult;
			PcPatrBrowserApp browser;
			var grammarOK = ProcessANAFileAndShowResults(ana, out andResult, out browser);
			if (grammarOK && browser.PropertiesChosen.Count() != 0)
			{
				var result = browser.PropertiesChosen;
				DisambiguateSegment(selectedSegmentToShow, result[0]);
			}
			Cursor.Current = Cursors.Default;
		}

		private PcPatrBrowserApp ShowPcPatrBrowser(string andResult)
		{
			var browser = new PcPatrBrowserApp(GetAppBaseDir());
			browser.AdjustUIForPcPatrFLEx();
			browser.LanguageInfo.GlossFontFace = AnalysisFont.FontFamily.Name;
			browser.LanguageInfo.GlossFontSize = AnalysisFont.Size;
			browser.LanguageInfo.GlossFontStyle = AnalysisFont.Style;
			browser.LanguageInfo.LexFontFace = VernacularFont.FontFamily.Name;
			browser.LanguageInfo.LexFontSize = VernacularFont.Size;
			browser.LanguageInfo.LexFontStyle = VernacularFont.Style;
			browser.LanguageInfo.UseRTL = Cache.LanguageProject.DefaultVernacularWritingSystem.RightToLeftScript;
			browser.LoadAnaFile(andResult);
			browser.ShowDialog();
			return browser;
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
                        int iEnd = Math.Max(ana.Length - 1, 0);
						sb.Append(ana.Substring(0, iEnd)); // skip final extra nl, if any
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
		}

		private void lbSegments_DoubleClick(object sender, EventArgs e)
		{
			if (lbSegments.SelectedItem != null)
			{
				Segments_SelectedIndexChanged(sender, e);
				if (btnParse.Enabled)
					Parse_Click(sender, e);
			}
		}
		private void Browse_Click(object sender, EventArgs e)
		{
			OpenFileDialog dlg = new OpenFileDialog();
			dlg.Filter = "PC-PATR Grammar File (*.grm)|*.grm|" +
			"All Files (*.*)|*.*";
			if (dlg.ShowDialog() == DialogResult.OK)
			{
				GrammarFile = dlg.FileName;
				LastGrammarFile = GrammarFile;
				tbGrammarFile.Text = GrammarFile;
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
			string ana = GetAnaForm(selectedTextToShow);
			string andResult;
			PcPatrBrowserApp browser;
			var grammarOK = ProcessANAFileAndShowResults(ana, out andResult, out browser);
			if (grammarOK)
			{
				var textdisambiguator = new TextDisambiguation(selectedTextToShow, browser.PropertiesChosen, andResult);
				textdisambiguator.Disambiguate(Cache);
			}
			Cursor.Current = Cursors.Default;
		}

		private bool ProcessANAFileAndShowResults(string ana, out string andResult, out PcPatrBrowserApp browser)
		{
			String anaFile = Path.Combine(Path.GetTempPath(), "Invoker.ana");
			File.WriteAllText(anaFile, ana);
			var invoker = new PCPatrInvoker(GrammarFile, anaFile, LastRootGlossSelection);
			invoker.Invoke();
			andResult = invoker.AndFile;
			if (File.Exists(andResult) && invoker.InvocationSucceeded)
			{
				browser = ShowPcPatrBrowser(andResult);
				return true;
			}
			else
			{
				browser = null;
				string message = "";
				if (File.Exists(andResult))
				{
					message = "The PC-PATR processing failed!\nPerhaps there are incompatible feature values in one of the forms.\nWe will show the error log after you click on OK.\nYou may also want to try and run the PcPatrFLEx.bat file in the %TEMP% diretory.\nThis may or may not show which features are incompatible or some other PC-PATR error or warning message.";
				}
				else
				{
					message = "The PC-PATR grammar file had an error in it and failed to load.\nWe will show the error log after you click on OK.\nPlease fix all errors in the grammar file and then try again.";
				}
				MessageBox.Show(message + "",
				"Grammar Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
				Process.Start(andResult.Replace(".and", ".log"));
				return false;
			}
		}

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
			if (menuItem.Name == PCPATRReferenceManual)
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
				Console.WriteLine("dialog result=" + dialog.Location.X + "," + dialog.Location.Y);
				dialog.Show();
			}
		}

		private void rbOff_CheckedChanged(object sender, EventArgs e)
		{
			LastRootGlossSelection = m_strOff;
		}

		private void rbLeftmost_CheckedChanged(object sender, EventArgs e)
		{
			LastRootGlossSelection = m_strLeftmost;
		}

		private void rbRightmost_CheckedChanged(object sender, EventArgs e)
		{
			LastRootGlossSelection = m_strRightmost;
		}

		private void rbAll_CheckedChanged(object sender, EventArgs e)
		{
			LastRootGlossSelection = m_strAll;
		}
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
