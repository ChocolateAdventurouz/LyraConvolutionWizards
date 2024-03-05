/*
  _    ___  _ ____  ____    ____  ____  _      _     ____  _     _     _____  _  ____  _     
/ \   \  \///  __\/  _ \  /   _\/  _ \/ \  /|/ \ |\/  _ \/ \   / \ /\/__ __\/ \/  _ \/ \  /|
| |    \  / |  \/|| / \|  |  /  | / \|| |\ ||| | //| / \|| |   | | ||  / \  | || / \|| |\ ||
| |_/\ / /  |    /| |-||  |  \__| \_/|| | \||| \// | \_/|| |_/\| \_/|  | |  | || \_/|| | \||
\____//_/   \_/\_\\_/ \|  \____/\____/\_/  \|\__/  \____/\____/\____/  \_/  \_/\____/\_/  \|
                                                                                            
             _      _  ____  ____  ____  ____  ____                                         
            / \  /|/ \/_   \/  _ \/  __\/  _ \/ ___\                                        
            | |  ||| | /   /| / \||  \/|| | \||    \                                        
        __  | |/\||| |/   /_| |-|||    /| |_/|\___ |  __                                    
        \/  \_/  \|\_/\____/\_/ \|\_/\_\\____/\____/  \/                                    
                                                                  

MainWindow.cs: Written by Karapatakis Aggelos (ChocolateAdventurouz)
Some parts are based on Race Fantasy Wizards *installDestForm.cs* - https://github.com/ChocolateAdventurouz/RaceFantasyWizards/blob/master/RaceFantasyInstaller/installDestForm.cs
Modified & Improved for Lyra Convolution Wizards
Written at: 27/2/2024
 */
using LyraConvolutionWizards.Helpers;
using System;
using LyraConvolutionWizards.Installation;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace LyraConvolutionWizards
{
    public partial class MainWindow : Form
    {
        public bool isInstalled;
        public string path;
        public void RegisterFileConfig()
        {
            SharedValues.Instance.InstallationDir = path.ToString();
        }
        public string installationDir = "C:\\Program Files\\George Sepetadelis\\Lyra Convolution";
        public MainWindow()
        {
            SharedValues.Instance.InstallationDir = installationDir;
            InitializeComponent();
        }

        private void MainWindow_Load(object sender, EventArgs e)
        {
            path = textBox1.Text;
            RegisterFileConfig();
            return;
            // Threads initialization
        }

        private void button3_Click(object sender, EventArgs e)
        {
            using (FolderBrowserDialog folderDialog = new FolderBrowserDialog()) // well, it broke again...
            {
                // Set the initial directory (optional)
                folderDialog.SelectedPath = @"C:\";

                // Show the folder dialog and get the result
                DialogResult result = folderDialog.ShowDialog();

                if (result == DialogResult.OK && !string.IsNullOrWhiteSpace(folderDialog.SelectedPath))
                {
                    textBox1.Text = folderDialog.SelectedPath;
                    path = folderDialog.SelectedPath.ToString();
                    RegisterFileConfig();
                    return;
                }
                return;
            }
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start("start https://chocolateadventurouz.rf.gd");
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            return;
        }

        private async void button2_Click(object sender, EventArgs e)
        {
            Installation.Install install = new Installation.Install();
            progressBar1.Value = 5;
            label3.Text = "Installing, please wait...";
            Task installTask = new Task( () => { install.MainInstallProcess(); } );
            installTask.Start();
            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (SharedValues.Instance.InstallFinished)
            {
                Application.Exit();
            }
            else
            {
                this.Close();
            }
        }
        private int ExitHandler()
        {
            if (MessageBox.Show("Are you sure you want to cancel the installation process?", "Lyra Convolution - Warning", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                string tempPath = Path.Combine(Path.GetTempPath() + "LyraInstall");
                if (File.Exists(Path.Combine(Path.GetTempPath(), "LyraInstall") + ".sfxcomplete"))
                { File.Delete(Path.Combine(Path.GetTempPath(), "LyraInstall") + ".sfxcomplete");

                    File.Delete(Path.Combine(tempPath, "\\Lyra.cab"));
                    File.Delete(Path.Combine(tempPath, "pakchunk0-Windows.gs01"));
                    File.Delete(Path.Combine(tempPath, "pakchunk0-Windows.gs02"));
                    File.Delete(Path.Combine(tempPath, "pakchunk0-Windows.gs03"));
                }
                Application.Exit();
                return 0;
            }
            else
            {
                return 1;
            }
        }
        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            switch (e.CloseReason)
            {
                case CloseReason.UserClosing:
                    e.Cancel = true;
                    if (ExitHandler() == 0)
                    {
                        e.Cancel = true;
                        e.Cancel = false;
                        if (File.Exists(Path.Combine(Path.GetTempPath(), "LyraInstall") + ".sfxcomplete"))
                        { File.Delete(Path.Combine(Path.GetTempPath(), "LyraInstall") + ".sfxcomplete"); }
                        Application.Exit();
                        return;
                    }
                    else
                    {
                        e.Cancel = true;
                        break;
                    }
            }
        }
    }
}
