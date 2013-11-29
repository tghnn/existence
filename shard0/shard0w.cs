using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Text;
using System.Linq;
using System.IO;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;

namespace shard0w
{
    public partial class shard0w : Form
    {
        string fname = "";
        public shard0w(string _f)
        {
            InitializeComponent();
            if (_f != "") fload(_f);
        }

        override protected void OnResize(EventArgs e)
        {            
            Document.Height = this.Height - 330;
            Invalidate();
        }

        private void Document_LinkClicked(object sender, LinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start(e.LinkText);
        }

        private void shard0w_Load(object sender, EventArgs e)
        {
            FontSize();
        }

        private void Timer_Tick_1(object sender, EventArgs e)
        {
            lineCount.Text = "Line: " + Document.GetLineFromCharIndex(Document.SelectionStart).ToString();
            status_ZoomFactor.Text = Document.ZoomFactor.ToString();
        }

        private void file_New_Click(object sender, EventArgs e)
        {
            New();
        }

        private void file_Open_Click(object sender, EventArgs e)
        {
            Open();
        }

        private void file_Save_Click(object sender, EventArgs e)
        {
            Save();
        }

        private void file_Calculate_Click(object sender, EventArgs e)
        {
            Calculate();
        }
        private void tool_Calculate_Click(object sender, EventArgs e)
        {
            Calculate();
        }

        private void file_Exit_Click(object sender, EventArgs e)
        {
            Exit();
        }


        private void edit_Undo_Click(object sender, EventArgs e)
        {
            Undo();
        }

        private void edit_Redo_Click(object sender, EventArgs e)
        {
            Redo();
        }

        private void edit_Cut_Click(object sender, EventArgs e)
        {
            Cut();
        }

        private void edit_Copy_Click(object sender, EventArgs e)
        {
            Copy();
        }

        private void edit_Paste_Click(object sender, EventArgs e)
        {
            Paste();
        }

        private void edit_SelectAll_Click(object sender, EventArgs e)
        {
            SelectAll();
        }

        private void clearAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ClearAll();
        }

        private void tb_New_Click(object sender, EventArgs e)
        {
            New();
        }

        private void tb_Open_Click(object sender, EventArgs e)
        {
            Open();
        }

        private void tb_Save_Click(object sender, EventArgs e)
        {
            Save();
        }

        private void tb_Cut_Click(object sender, EventArgs e)
        {
            Cut();
        }

        private void tb_Copy_Click(object sender, EventArgs e)
        {
            Copy();
        }

        private void tb_Paste_Click(object sender, EventArgs e)
        {
            Paste();
        }

        private void tb_ZoomIn_Click(object sender, EventArgs e)
        {
            if (Document.ZoomFactor == 63)
                return;
            else
                Document.ZoomFactor = Document.ZoomFactor + 1;
        }

        private void tb_ZoomOut_Click(object sender, EventArgs e)
        {
            if (Document.ZoomFactor == 1)
                return;
            else
                Document.ZoomFactor = Document.ZoomFactor - 1;
        }

        private void rc_Undo_Click(object sender, EventArgs e)
        {
            Undo();
        }

        private void rc_Redo_Click(object sender, EventArgs e)
        {
            Redo();
        }

        private void rc_Cut_Click(object sender, EventArgs e)
        {
            Cut();
        }

        private void rc_Copy_Click(object sender, EventArgs e)
        {
            Copy();
        }

        private void rc_Paste_Click(object sender, EventArgs e)
        {
            Paste();
        }


        void New()
        {
            fname = ""; Text = "shard0w";
            Document.Clear();
        }


        void Open()
        {
            if (openWork.ShowDialog() == DialogResult.OK) fload(openWork.FileName);
        }


        void Save()
        {
            if (fname == "") {if (saveWork.ShowDialog() == DialogResult.OK) setfname(saveWork.FileName); else return;}
                try
                {
                    Document.SaveFile(fname, RichTextBoxStreamType.PlainText);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
        }
        void Exit()
        {
            Application.Exit();
        }

        void Undo()
        {
            Document.Undo();
        }

        void Redo()
        {
            Document.Redo();
        }

        void Cut()
        {
            Document.Cut();
        }

        void Copy()
        {
            if (Document.Focused) Document.Copy(); else Result.Copy();
        }

        void Paste()
        {
            Document.Paste();
        }

        void SelectAll()
        {
            Document.SelectAll();
        }

        void ClearAll()
        {
            Document.Clear();
        }

        void FontSize()
        {
            for (int fntSize = 10; fntSize <= 75; fntSize++)
            {
                tb_FontSize.Items.Add(fntSize.ToString());
            }
        }
        void fload(string _f) {
            if (File.Exists(_f)) {
                setfname(_f);
                Document.LoadFile(fname, RichTextBoxStreamType.PlainText);
            }
        }
        void setfname(string _f) {
                fname = _f; Text = "shard0w # " + Path.GetFileName(fname);
        }

        void Calculate ()
        {
            Document.Height = this.Height - 220;
            Save();
            if (fname == "") return;
            ProcessStartInfo start = new ProcessStartInfo();
            start.Arguments = fname;
            start.FileName = "shard0.exe";
            start.WindowStyle = ProcessWindowStyle.Normal;
            using (Process proc = Process.Start(start))
            {
                proc.WaitForExit();
            }
            string r0; r0 = Path.GetFileNameWithoutExtension(fname) + "0.txt";
            if (File.Exists(r0)) {
                Result.LoadFile(r0, RichTextBoxStreamType.PlainText);
                Result.SelectionStart = Result.Text.Length;
                Result.ScrollToCaret();
            } else {
                Result.Clear();
                Result.AppendText("And Then There Were None");
            }
        }
    
    }
    static class Program
    {
        [STAThread]
        static int Main(string[] args)
        {
            if (!File.Exists("shard0.exe")) return -1;
            string s0 = "";
            if (args.Length > 0) s0 = args[0];
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new shard0w(s0));
            return 0;
        }
    }

}   
