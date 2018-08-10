namespace SIL.PcPatrFLEx
{
	partial class PcPatrFLExForm
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PcPatrFLExForm));
			this.btnParse = new System.Windows.Forms.Button();
			this.btnProject = new System.Windows.Forms.Button();
			this.lblDatabaseToUse = new System.Windows.Forms.Label();
			this.splitContainer1 = new System.Windows.Forms.SplitContainer();
			this.lbTexts = new System.Windows.Forms.ListBox();
			this.lbSegments = new System.Windows.Forms.ListBox();
			this.lblTexts = new System.Windows.Forms.Label();
			this.lblSegments = new System.Windows.Forms.Label();
			this.label1 = new System.Windows.Forms.Label();
			this.tbGrammarFile = new System.Windows.Forms.TextBox();
			this.btnBrowse = new System.Windows.Forms.Button();
			this.lblProjectName = new System.Windows.Forms.Label();
			this.btnDisambiguate = new System.Windows.Forms.Button();
			((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
			this.splitContainer1.Panel1.SuspendLayout();
			this.splitContainer1.Panel2.SuspendLayout();
			this.splitContainer1.SuspendLayout();
			this.SuspendLayout();
			// 
			// btnParse
			// 
			this.btnParse.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.btnParse.Location = new System.Drawing.Point(519, 201);
			this.btnParse.Name = "btnParse";
			this.btnParse.Size = new System.Drawing.Size(267, 38);
			this.btnParse.TabIndex = 0;
			this.btnParse.Text = "&Parse && show this segment";
			this.btnParse.UseVisualStyleBackColor = true;
			this.btnParse.Click += new System.EventHandler(this.Parse_Click);
			// 
			// btnProject
			// 
			this.btnProject.Location = new System.Drawing.Point(13, 12);
			this.btnProject.Name = "btnProject";
			this.btnProject.Size = new System.Drawing.Size(191, 38);
			this.btnProject.TabIndex = 2;
			this.btnProject.Text = "&Choose FLEx Project";
			this.btnProject.UseVisualStyleBackColor = true;
			this.btnProject.Click += new System.EventHandler(this.ChooseProject_Click);
			// 
			// lblDatabaseToUse
			// 
			this.lblDatabaseToUse.AutoSize = true;
			this.lblDatabaseToUse.Location = new System.Drawing.Point(282, 17);
			this.lblDatabaseToUse.Name = "lblDatabaseToUse";
			this.lblDatabaseToUse.Size = new System.Drawing.Size(158, 20);
			this.lblDatabaseToUse.TabIndex = 3;
			this.lblDatabaseToUse.Text = "Chosen FLEx Project";
			// 
			// splitContainer1
			// 
			this.splitContainer1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.splitContainer1.Location = new System.Drawing.Point(3, 248);
			this.splitContainer1.Name = "splitContainer1";
			// 
			// splitContainer1.Panel1
			// 
			this.splitContainer1.Panel1.Controls.Add(this.lbTexts);
			// 
			// splitContainer1.Panel2
			// 
			this.splitContainer1.Panel2.Controls.Add(this.lbSegments);
			this.splitContainer1.Size = new System.Drawing.Size(796, 687);
			this.splitContainer1.SplitterDistance = 356;
			this.splitContainer1.TabIndex = 4;
			this.splitContainer1.SplitterMoved += new System.Windows.Forms.SplitterEventHandler(this.splitContainer1_SplitterMoved);
			// 
			// lbTexts
			// 
			this.lbTexts.Dock = System.Windows.Forms.DockStyle.Fill;
			this.lbTexts.FormattingEnabled = true;
			this.lbTexts.ItemHeight = 20;
			this.lbTexts.Location = new System.Drawing.Point(0, 0);
			this.lbTexts.Name = "lbTexts";
			this.lbTexts.Size = new System.Drawing.Size(356, 687);
			this.lbTexts.TabIndex = 1;
			this.lbTexts.SelectedIndexChanged += new System.EventHandler(this.Texts_SelectedIndexChanged);
			// 
			// lbSegments
			// 
			this.lbSegments.Dock = System.Windows.Forms.DockStyle.Fill;
			this.lbSegments.FormattingEnabled = true;
			this.lbSegments.ItemHeight = 20;
			this.lbSegments.Location = new System.Drawing.Point(0, 0);
			this.lbSegments.Name = "lbSegments";
			this.lbSegments.Size = new System.Drawing.Size(436, 687);
			this.lbSegments.TabIndex = 2;
			this.lbSegments.SelectedIndexChanged += new System.EventHandler(this.Segments_SelectedIndexChanged);
			// 
			// lblTexts
			// 
			this.lblTexts.AutoSize = true;
			this.lblTexts.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lblTexts.Location = new System.Drawing.Point(9, 211);
			this.lblTexts.Name = "lblTexts";
			this.lblTexts.Size = new System.Drawing.Size(61, 25);
			this.lblTexts.TabIndex = 3;
			this.lblTexts.Text = "Texts";
			// 
			// lblSegments
			// 
			this.lblSegments.AutoSize = true;
			this.lblSegments.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lblSegments.Location = new System.Drawing.Point(367, 211);
			this.lblSegments.Name = "lblSegments";
			this.lblSegments.Size = new System.Drawing.Size(101, 25);
			this.lblSegments.TabIndex = 4;
			this.lblSegments.Text = "Segments";
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(13, 124);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(180, 20);
			this.label1.TabIndex = 5;
			this.label1.Text = "PC-PATR Grammar file: ";
			// 
			// tbGrammarFile
			// 
			this.tbGrammarFile.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.tbGrammarFile.Location = new System.Drawing.Point(216, 120);
			this.tbGrammarFile.Name = "tbGrammarFile";
			this.tbGrammarFile.Size = new System.Drawing.Size(885, 26);
			this.tbGrammarFile.TabIndex = 6;
			// 
			// btnBrowse
			// 
			this.btnBrowse.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.btnBrowse.Location = new System.Drawing.Point(1128, 120);
			this.btnBrowse.Name = "btnBrowse";
			this.btnBrowse.Size = new System.Drawing.Size(94, 38);
			this.btnBrowse.TabIndex = 7;
			this.btnBrowse.Text = "&Browse";
			this.btnBrowse.UseVisualStyleBackColor = true;
			this.btnBrowse.Click += new System.EventHandler(this.Browse_Click);
			// 
			// lblProjectName
			// 
			this.lblProjectName.AutoSize = true;
			this.lblProjectName.Location = new System.Drawing.Point(13, 73);
			this.lblProjectName.Name = "lblProjectName";
			this.lblProjectName.Size = new System.Drawing.Size(158, 20);
			this.lblProjectName.TabIndex = 8;
			this.lblProjectName.Text = "Chosen FLEx Project";
			// 
			// btnDisambiguate
			// 
			this.btnDisambiguate.Location = new System.Drawing.Point(77, 201);
			this.btnDisambiguate.Name = "btnDisambiguate";
			this.btnDisambiguate.Size = new System.Drawing.Size(244, 38);
			this.btnDisambiguate.TabIndex = 9;
			this.btnDisambiguate.Text = "&Disambiguate && show this text";
			this.btnDisambiguate.UseVisualStyleBackColor = true;
			this.btnDisambiguate.Click += new System.EventHandler(this.Disambiguate_Click);
			// 
			// PcPatrFLExForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(1228, 947);
			this.Controls.Add(this.btnDisambiguate);
			this.Controls.Add(this.lblProjectName);
			this.Controls.Add(this.btnBrowse);
			this.Controls.Add(this.tbGrammarFile);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.lblSegments);
			this.Controls.Add(this.lblTexts);
			this.Controls.Add(this.splitContainer1);
			this.Controls.Add(this.lblDatabaseToUse);
			this.Controls.Add(this.btnProject);
			this.Controls.Add(this.btnParse);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Name = "PcPatrFLExForm";
			this.Text = "Use PC-PATR with FLEx";
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.OnFormClosing);
			this.splitContainer1.Panel1.ResumeLayout(false);
			this.splitContainer1.Panel2.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
			this.splitContainer1.ResumeLayout(false);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Button btnParse;
		private System.Windows.Forms.Button btnProject;
		private System.Windows.Forms.Label lblDatabaseToUse;
		private System.Windows.Forms.SplitContainer splitContainer1;
		private System.Windows.Forms.Label lblTexts;
		private System.Windows.Forms.ListBox lbTexts;
		private System.Windows.Forms.Label lblSegments;
		private System.Windows.Forms.ListBox lbSegments;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.TextBox tbGrammarFile;
		private System.Windows.Forms.Button btnBrowse;
		private System.Windows.Forms.Label lblProjectName;
		private System.Windows.Forms.Button btnDisambiguate;
	}
}

