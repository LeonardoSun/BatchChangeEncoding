using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace BatchChangeEncoding
{
    public static class Ext
    {
        public static bool MatchHeader(this string str, string match)
        {
            if (str.Length < match.Length)
                return false;
            for (int i = 0; i < match.Length; i++)
            {
                if (str[i] != match[i])
                    return false;
            }
            return true;
        }
    }

    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            comboBox1.SelectedIndex = 1;
        }

        public void Change(string path, string ext = "*.cs")
        {
            foreach (var f in new DirectoryInfo(path).GetFiles(ext, SearchOption.AllDirectories))
            {
                string s = File.ReadAllText(f.FullName/*, Encoding.GetEncoding("gb2312")*/);
                string header = "////////////\r\n//    created by 孙庆号\r\n////////////\r\n\r\n";
                if (!s.MatchHeader(header))
                    s = header + s;
                var encoding = new System.Text.UTF8Encoding(true);
                File.WriteAllText(f.FullName, s, /*Encoding.UTF8*/encoding);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            folderBrowserDialog1.SelectedPath = textBox2.Text;
            if (folderBrowserDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                textBox2.Text = folderBrowserDialog1.SelectedPath;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            var path = textBox2.Text;
            if (string.IsNullOrWhiteSpace(path))
            {
                MessageBox.Show("Directory is empty.");
                return;
            }
            var dir = new DirectoryInfo(path);
            if (!dir.Exists)
            {
                MessageBox.Show("Directory doesn't exists.");
                return;
            }
            string ext;
            switch (comboBox1.SelectedIndex)
            {
                case 0:
                    ext = "*.cs";
                    Change(path, ext);
                    break;
                default:
                case 1:
                    ext = "*.cpp";
                    Change(path, ext);
                    ext = "*.h";
                    Change(path, ext);
                    break;
            }
            MessageBox.Show("Done.");
        }
    }
}
