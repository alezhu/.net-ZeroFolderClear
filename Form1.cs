using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using System.IO;

namespace ZeroFolderClear
{
	/// <summary>
	/// Summary description for Form1.
	/// </summary>
	public class Form1 : System.Windows.Forms.Form
	{
		private bool doStop = false;
		private System.Windows.Forms.CheckedListBox checkedListBox1;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Button button1;
		private System.Windows.Forms.Button button2;
		private System.Windows.Forms.ProgressBar progressBar1;
		private System.Windows.Forms.Label labDirectory;
		private System.Windows.Forms.ListBox listBox1;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public Form1()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			//
			// TODO: Add any constructor code after InitializeComponent call
			//
		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if (components != null) 
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.checkedListBox1 = new System.Windows.Forms.CheckedListBox();
			this.label1 = new System.Windows.Forms.Label();
			this.button1 = new System.Windows.Forms.Button();
			this.button2 = new System.Windows.Forms.Button();
			this.progressBar1 = new System.Windows.Forms.ProgressBar();
			this.labDirectory = new System.Windows.Forms.Label();
			this.listBox1 = new System.Windows.Forms.ListBox();
			this.SuspendLayout();
			// 
			// checkedListBox1
			// 
			this.checkedListBox1.CheckOnClick = true;
			this.checkedListBox1.Location = new System.Drawing.Point(8, 32);
			this.checkedListBox1.Name = "checkedListBox1";
			this.checkedListBox1.Size = new System.Drawing.Size(168, 94);
			this.checkedListBox1.Sorted = true;
			this.checkedListBox1.TabIndex = 0;
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(8, 8);
			this.label1.Name = "label1";
			this.label1.TabIndex = 6;
			this.label1.Text = "Drives:";
			// 
			// button1
			// 
			this.button1.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.button1.Location = new System.Drawing.Point(184, 96);
			this.button1.Name = "button1";
			this.button1.TabIndex = 2;
			this.button1.Text = "Start";
			this.button1.Click += new System.EventHandler(this.button1_Click);
			// 
			// button2
			// 
			this.button2.Enabled = false;
			this.button2.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.button2.Location = new System.Drawing.Point(272, 96);
			this.button2.Name = "button2";
			this.button2.TabIndex = 3;
			this.button2.Text = "Stop";
			this.button2.Click += new System.EventHandler(this.button2_Click);
			// 
			// progressBar1
			// 
			this.progressBar1.Location = new System.Drawing.Point(184, 32);
			this.progressBar1.Name = "progressBar1";
			this.progressBar1.Size = new System.Drawing.Size(320, 23);
			this.progressBar1.Step = 1;
			this.progressBar1.TabIndex = 4;
			// 
			// labDirectory
			// 
			this.labDirectory.Location = new System.Drawing.Point(184, 64);
			this.labDirectory.Name = "labDirectory";
			this.labDirectory.Size = new System.Drawing.Size(320, 23);
			this.labDirectory.TabIndex = 5;
			// 
			// listBox1
			// 
			this.listBox1.Location = new System.Drawing.Point(8, 136);
			this.listBox1.Name = "listBox1";
			this.listBox1.Size = new System.Drawing.Size(496, 160);
			this.listBox1.TabIndex = 7;
			// 
			// Form1
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(512, 307);
			this.Controls.Add(this.listBox1);
			this.Controls.Add(this.labDirectory);
			this.Controls.Add(this.progressBar1);
			this.Controls.Add(this.button2);
			this.Controls.Add(this.button1);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.checkedListBox1);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.MaximizeBox = false;
			this.Name = "Form1";
			this.Text = "Zero Folder Clear";
			this.Load += new System.EventHandler(this.Form1_Load);
			this.ResumeLayout(false);

		}
		#endregion

		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main() 
		{
			Application.EnableVisualStyles(); 
			Application.Run(new Form1());
		}

		private void Form1_Load(object sender, System.EventArgs e) {
			this.Text  += " v" + System.Diagnostics.FileVersionInfo.GetVersionInfo(System.Reflection.Assembly.GetExecutingAssembly().Location).FileVersion; 
			string[] Drives = Directory.GetLogicalDrives();
			foreach(string s in Drives) {
				if (azLib.DriveInfo.DriveType(s) == azLib.DriveType.DRIVE_FIXED ) {
					checkedListBox1.Items.Add(s); 
				}
			}
		}
		private void button1_Click(object sender, System.EventArgs e) {
			button1.Enabled = false;
			button2.Enabled = true;
			doStop = false;
			listBox1.Items.Clear();
			foreach(string s in checkedListBox1.CheckedItems) {
				DirectoryInfo di = new DirectoryInfo(s);
				if (!ProcessDirectory(di))
					break;
			}
			button1.Enabled = true;
			button2.Enabled = false;
			labDirectory.Text = "Завершено" ;
		}

		private bool ProcessDirectory(DirectoryInfo Dir) {
			string fn =Dir.FullName;
			this.labDirectory.Text = fn; 
			progressBar1.PerformStep(); 
			if ((progressBar1.Value == progressBar1.Maximum)||(progressBar1.Value == 0))
				progressBar1.Step  = -progressBar1.Step; 
			Application.DoEvents();	
			if (doStop) 
				return false;
			try {
					foreach(DirectoryInfo di in Dir.GetDirectories()) {
						if (!ProcessDirectory(di))
							return false;
				}
			}
			catch {};
			try {
				Dir.Delete(); 
				listBox1.Items.Add(Dir.FullName);
			}
			catch {};
			return true;
		}

		private void button2_Click(object sender, System.EventArgs e) {
			doStop = true;
		}
	}
}
