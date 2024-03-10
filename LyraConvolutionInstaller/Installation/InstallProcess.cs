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
                                                                  

InstallProcess.cs: Written by Karapatakis Aggelos (ChocolateAdventurouz)
Based on Race Fantasy Wizards *Installation.cs* - https://github.com/ChocolateAdventurouz/RaceFantasyWizards/blob/master/RaceFantasyInstaller/Installation.cs
Modified & Improved for Lyra Convolution Wizards
Written at: 05/08/2023
Included at: 27/2/2024
 */
using LyraConvolutionWizards.Helpers;
using Microsoft.Win32;
using System;
using Microsoft.Deployment.Compression.Cab;
using System.Collections.Generic;
using System.Diagnostics;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using IWshRuntimeLibrary;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
namespace LyraConvolutionWizards.Installation
{
    partial class StepMethods
    {
        string tempPath = Path.Combine(Path.GetTempPath() + "LyraInstall");
        string installationPath;

        public async Task ReadUpdaterInfo()
        {
            try
            {
                Console.WriteLine("[{0}] - Reading installation directory", DateTime.Now);
                installationPath = SharedValues.Instance.InstallationDir;
                await Task.Delay(333);
            }
            catch (FileNotFoundException)
            {
                Console.WriteLine("[{0}] - Exception Raised: Couldn't find Installation Directory", DateTime.Now);
                MessageBox.Show("A fatal error occurred while trying to access the temporary location. It is recommended to stop any cleaning applications and re-run the setup file again.", "Lyra Convolution - Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        static void AppendFileBytesToCombinedStream(string filePath, FileStream combinedFileStream)
        {
            using (FileStream partFileStream = new FileStream(filePath, FileMode.Open))
            {
                byte[] buffer = new byte[4096]; // Adjust the buffer size as needed
                int bytesRead;
                while ((bytesRead = partFileStream.Read(buffer, 0, buffer.Length)) > 0)
                {
                    combinedFileStream.Write(buffer, 0, bytesRead);
                }
            }
        }
        private void CreateShortcutDesktop()
        {
            string link = Environment.GetFolderPath(Environment.SpecialFolder.Desktop)
                + Path.DirectorySeparatorChar + "Lyra Convolution" + ".lnk";
            var shell = new WshShell();
            var shortcut = shell.CreateShortcut(link) as IWshShortcut;
            shortcut.TargetPath = @installationPath + "\\LyraGame.exe";
            shortcut.IconLocation = @installationPath + "\\LyraGame.exe";
            shortcut.WorkingDirectory = @installationPath;
            Thread.Sleep(333);
            shortcut.Save();
        }
        private void CreateShortcutStartMenu()
        {
            string link = Environment.GetFolderPath(Environment.SpecialFolder.StartMenu)
                + Path.DirectorySeparatorChar + "Lyra Convolution" + ".lnk";
            var shell = new WshShell();
            var shortcut = shell.CreateShortcut(link) as IWshShortcut;
            shortcut.TargetPath = @installationPath + "\\LyraGame.exe";
            shortcut.IconLocation = @installationPath + "\\LyraGame.exe";
            shortcut.WorkingDirectory = @installationPath;
            Thread.Sleep(333);
            shortcut.Save();
        }
        public void CreateShortcuts()
        {
            CreateShortcutDesktop();
            CreateShortcutStartMenu();
            return;
        }
        public async void DeployGamePackage() {

            try
            {

                //File.WriteAllBytes(Path.Combine(tempPath, "\\Lyra.cab"), Properties.WizardResources.Lyra);
                System.IO.File.WriteAllBytes(Path.Combine(tempPath, "pakchunk0-Windows.gs02"), Properties.WizardResources.pakchunk0_Windows);
                System.IO.File.WriteAllBytes(Path.Combine(tempPath, "pakchunk0-Windows.gs03"), Properties.WizardResources.pakchunk0_Windows1);
                System.IO.File.WriteAllBytes(Path.Combine(tempPath, "pakchunk0-Windows.gs01"), Properties.WizardResources.pakchunk0_Windows2);

                using (FileStream combinedFileStream = new FileStream(Path.Combine(tempPath, "pakchunk0-Windows.pak"), FileMode.Create))
                {
                    AppendFileBytesToCombinedStream(Path.Combine(tempPath, "pakchunk0-Windows.gs01"), combinedFileStream);
                    AppendFileBytesToCombinedStream(Path.Combine(tempPath, "pakchunk0-Windows.gs02"), combinedFileStream);
                    AppendFileBytesToCombinedStream(Path.Combine(tempPath, "pakchunk0-Windows.gs03"), combinedFileStream);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error writing file: {ex.Message}");
            }

            return;
        }

        public async Task ExtractGameFiles()
        {
            
            Directory.CreateDirectory(installationPath);
            System.IO.File.WriteAllBytes(Path.Combine(tempPath, "Lyra.cab"), Properties.WizardResources.Lyra);
            System.IO.File.WriteAllBytes(Path.Combine(installationPath, "uninstall.exe"), Properties.WizardResources.uninstall);
            await Task.Run(() => DeployGamePackage());
            string gameCabFile = Path.Combine(tempPath, "Lyra.cab");
            CabInfo gameCab = new CabInfo(gameCabFile);
            gameCab.Unpack(installationPath);
            try
            {
                System.IO.File.Copy(Path.Combine(tempPath, "pakchunk0-Windows.pak"), installationPath + "\\LyraStarterGame\\Content\\Paks\\pakchunk0-Windows.pak");
            }
            catch (IOException ex)
            {
                System.IO.Directory.Delete(installationPath, true);
                System.IO.Directory.CreateDirectory(installationPath);
                ExtractGameFiles();
            }
            return;
        }

        public async Task WriteRegValues()
        {

            using (RegistryKey parent64 = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\WOW6432Node\Microsoft\Windows\CurrentVersion\Uninstall", true))
            {
                if (parent64 == null)
                {
                    Console.WriteLine("[ERROR] Uninstall registry key not found.");
                    MessageBox.Show("Uninstall registry key not found. There might be a corruption to your system or you might trying installing Lyra in a 32-bit system ", "Lyra - Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

                try
                {
                    using (RegistryKey key = parent64.OpenSubKey("LyraConvolution", true) ?? parent64.CreateSubKey("LyraConvolution"))
                    {
                        if (key == null)
                        {
                            Console.WriteLine("[{0}] - UninstallInfo couldn't be deployed to registry. Passing...", DateTime.Now);
                        }
                        key.SetValue("DisplayName", "Lyra Convolution");

                        Assembly asm = Assembly.GetExecutingAssembly();
                        FileVersionInfo fileVersion = FileVersionInfo.GetVersionInfo(asm.Location);
                        key.SetValue("ApplicationVersion", fileVersion.ProductVersion.ToString());
                        key.SetValue("Publisher", "George Sepetadelis");
                        key.SetValue("DisplayIcon", Path.Combine(installationPath, "LyraGame.exe"));
                        key.SetValue("DisplayVersion", key.GetValue("ApplicationVersion"));
                        key.SetValue("URLInfoAbout", "https://sepetadelhs.rf.gd");
                        key.SetValue("Contact", "giorgossepetadelis11@gmail.com");
                        key.SetValue("InstallDate", DateTime.Now.ToString("yyyyMMdd"));
                        key.SetValue("UninstallString", Path.Combine(installationPath, "uninstall.exe"));
                        key.SetValue("Location", installationPath);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("[{0}] - Couldn't write uninstall information. Passing...", DateTime.Now);
                    MessageBox.Show($"An error occurred while writing the uninstall information. Lyra is fully installed but it is recommended to re-run the installation wizard again. \nIf you intend to contact support about this error, provide this log below: \n{ex.ToString()}", "Lyra - Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
    }
    public class Install : MainWindow
    {
        
        public async Task MainInstallProcess()
        {
            try
            {
                StepMethods stepMethods = new StepMethods();
                stepMethods.ReadUpdaterInfo();

                Task extractGameTask = Task.Run(async () => await stepMethods.ExtractGameFiles());
                Task writeRegValuesTask = Task.Run(async () => await stepMethods.WriteRegValues());

                await Task.WhenAll(extractGameTask, writeRegValuesTask);

                stepMethods.CreateShortcuts();
            }
            finally
            {
                if (MessageBox.Show("Installation completed.", "Lyra Convolution", MessageBoxButtons.OK, MessageBoxIcon.Information) == DialogResult.OK)
                {
                    Application.Exit();
                }
            }


        }

    }
}