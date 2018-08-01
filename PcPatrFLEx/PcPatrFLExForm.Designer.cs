﻿namespace SIL.PcPatrFLEx
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
			((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
			this.splitContainer1.Panel1.SuspendLayout();
			this.splitContainer1.Panel2.SuspendLayout();
			this.splitContainer1.SuspendLayout();
			this.SuspendLayout();
			// 
			// btnParse
			// 
			this.btnParse.Location = new System.Drawing.Point(697, 12);
			this.btnParse.Name = "btnParse";
			this.btnParse.Size = new System.Drawing.Size(94, 38);
			this.btnParse.TabIndex = 0;
			this.btnParse.Text = "Parse";
			this.btnParse.UseVisualStyleBackColor = true;
			this.btnParse.Click += new System.EventHandler(this.btnParse_Click);
			// 
			// btnProject
			// 
			this.btnProject.Location = new System.Drawing.Point(13, 12);
			this.btnProject.Name = "btnProject";
			this.btnProject.Size = new System.Drawing.Size(191, 38);
			this.btnProject.TabIndex = 2;
			this.btnProject.Text = "Choose FLEx Project";
			this.btnProject.UseVisualStyleBackColor = true;
			this.btnProject.Click += new System.EventHandler(this.btnProject_Click);
			// 
			// lblDatabaseToUse
			// 
			this.lblDatabaseToUse.AutoSize = true;
			this.lblDatabaseToUse.Location = new System.Drawing.Point(282, 30);
			this.lblDatabaseToUse.Name = "lblDatabaseToUse";
			this.lblDatabaseToUse.Size = new System.Drawing.Size(158, 20);
			this.lblDatabaseToUse.TabIndex = 3;
			this.lblDatabaseToUse.Text = "Chosen FLEx Project";
			// 
			// splitContainer1
			// 
			this.splitContainer1.Location = new System.Drawing.Point(4, 100);
			this.splitContainer1.Name = "splitContainer1";
			// 
			// splitContainer1.Panel1
			// 
			this.splitContainer1.Panel1.Controls.Add(this.lbTexts);
			// 
			// splitContainer1.Panel2
			// 
			this.splitContainer1.Panel2.Controls.Add(this.lbSegments);
			this.splitContainer1.Size = new System.Drawing.Size(796, 466);
			this.splitContainer1.SplitterDistance = 253;
			this.splitContainer1.TabIndex = 4;
			// 
			// lbTexts
			// 
			this.lbTexts.Dock = System.Windows.Forms.DockStyle.Fill;
			this.lbTexts.FormattingEnabled = true;
			this.lbTexts.ItemHeight = 20;
			this.lbTexts.Location = new System.Drawing.Point(0, 0);
			this.lbTexts.Name = "lbTexts";
			this.lbTexts.Size = new System.Drawing.Size(253, 466);
			this.lbTexts.TabIndex = 1;
			this.lbTexts.SelectedIndexChanged += new System.EventHandler(this.lbTexts_SelectedIndexChanged_1);
			// 
			// lbSegments
			// 
			this.lbSegments.Dock = System.Windows.Forms.DockStyle.Fill;
			this.lbSegments.FormattingEnabled = true;
			this.lbSegments.ItemHeight = 20;
			this.lbSegments.Location = new System.Drawing.Point(0, 0);
			this.lbSegments.Name = "lbSegments";
			this.lbSegments.Size = new System.Drawing.Size(539, 466);
			this.lbSegments.TabIndex = 2;
			this.lbSegments.SelectedIndexChanged += new System.EventHandler(this.lbSegments_SelectedIndexChanged);
			// 
			// lblTexts
			// 
			this.lblTexts.AutoSize = true;
			this.lblTexts.Location = new System.Drawing.Point(12, 77);
			this.lblTexts.Name = "lblTexts";
			this.lblTexts.Size = new System.Drawing.Size(47, 20);
			this.lblTexts.TabIndex = 3;
			this.lblTexts.Text = "Texts";
			// 
			// lblSegments
			// 
			this.lblSegments.AutoSize = true;
			this.lblSegments.Location = new System.Drawing.Point(282, 77);
			this.lblSegments.Name = "lblSegments";
			this.lblSegments.Size = new System.Drawing.Size(82, 20);
			this.lblSegments.TabIndex = 4;
			this.lblSegments.Text = "Segments";
			// 
			// PcPatrFLExForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(800, 729);
			this.Controls.Add(this.lblSegments);
			this.Controls.Add(this.lblTexts);
			this.Controls.Add(this.splitContainer1);
			this.Controls.Add(this.lblDatabaseToUse);
			this.Controls.Add(this.btnProject);
			this.Controls.Add(this.btnParse);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Name = "PcPatrFLExForm";
			this.Text = "Use PC-PATR with FLEx";
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
	}
}

