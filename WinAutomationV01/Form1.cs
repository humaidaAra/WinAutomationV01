using System;
using System.Diagnostics;
using System.Media;
using System.Windows.Forms;
using System.Text.RegularExpressions;

namespace WinAutomationV01
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            textBox1.Focus();
        }

        private void button1_Click(object sender, EventArgs e)
        {
             if(textBox1.Text.Equals(""))
            {
                SystemSounds.Beep.Play();
                textBox1.Focus();
                return;
            }
            try
            {
                String link = textBox1.Text;
                String patt = @"^((?:https?:)?\/\/)?((?:www|m)\.)?((?:youtube\.com|youtu.be))(\/(?:[\w\-]+\?v=|embed\/|v\/)?)([\w\-]+)(\S+)?$";
                MatchCollection mat = Regex.Matches(link.Trim(), patt);
                foreach (Match find in mat)
                {
                    Process p = new Process();
                    string des = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
                    // Redirect the output stream of the child process. 
                    p.StartInfo.UseShellExecute = false;
                    p.StartInfo.CreateNoWindow = true;
                    p.StartInfo.FileName = "cmd.exe";
                    p.StartInfo.Arguments = "/C yt-dlp -f bestvideo*+bestaudio/best -o \""+des+"\"\\vid \"" + link.Trim() + "\"";
                    p.StartInfo.RedirectStandardOutput = true;
                    p.StartInfo.RedirectStandardInput = true;
                    p.Start();

                    // Do not wait for the child process to exit before
                    // reading to the end of it s redirected stream.

                    String result = p.StandardOutput.ReadToEnd();

                    p.WaitForExit();
                    Process p2 = new Process();
                    // Redirect the output stream of the child process. 
                    p2.StartInfo.UseShellExecute = false;
                    p2.StartInfo.CreateNoWindow = true;
                    p2.StartInfo.FileName = "cmd.exe";
                    p2.StartInfo.Arguments = "/C " + des + "\\vid.webm";
                    p2.StartInfo.RedirectStandardOutput = true;
                    p2.StartInfo.RedirectStandardInput = true;
                    p2.Start();
                    p2.WaitForExit();
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
                this.Close();
            }
        }
    }
}
