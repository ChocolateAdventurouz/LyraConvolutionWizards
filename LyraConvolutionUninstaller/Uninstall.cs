using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace LyraConvolutionUninstaller
{
    internal class Uninstall
    {
        string installationPath = System.IO.Path.GetDirectoryName(Application.ExecutablePath);

        private async Task removeUserData()
        {
            try
            {
                if (Directory.Exists(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData)) == true)
                {

                    Directory.Delete(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "\\LyraStarterGame", true);
                    return;
                }
                else
                {
                    return;
                }
            }

            catch (Exception ex)
            {
                return;
            }
        }
        private async Task removeKey()
        {
            RegistryKey key = null;
            using (RegistryKey parent = Registry.LocalMachine.OpenSubKey(
                         @"SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall", true))
            {
                Assembly asm = GetType().Assembly;
                if (parent == null)
                {
                    throw new Exception("Uninstall registry key not found.");
                }
                try
                {

                    string guidText = Assembly.GetExecutingAssembly().GetCustomAttribute<GuidAttribute>().Value.ToUpper();
                    parent.DeleteSubKey(guidText, false);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.ToString());
                }
                finally
                {
                    if (key != null)
                    {
                        key.Close();

                    }
                }
            }

            using (RegistryKey parent = Registry.LocalMachine.OpenSubKey(
             @"SOFTWARE\WOW6432Node\Microsoft\Windows\CurrentVersion\Uninstall", true))
            {
                if (parent == null)
                {
                    throw new Exception("Uninstall registry key not found.");
                }
                try
                {

                    parent.DeleteSubKey("LyraConvolution", false);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.ToString());
                }
                finally
                {
                    if (key != null)
                    {
                        key.Close();

                    }
                }
            }
            return;
        }

        private async Task RemoveShortcuts()
        {
            try
            {
                File.Delete(Environment.GetFolderPath(Environment.SpecialFolder.StartMenu) + Path.DirectorySeparatorChar + "Lyra Convolution" + ".lnk");
                File.Delete(Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + Path.DirectorySeparatorChar + "Lyra Convolution" + ".lnk");
            }
            catch
            {
                return;
            }
        }

        async Task DeleteDirectoryRecursively(string targetDirectory)
        {
            DirectoryInfo directoryInfo = new DirectoryInfo(installationPath);
            string[] filesToKeep = { "uninstall.exe" };

            // Get all files in the directory
            FileInfo[] allFiles = directoryInfo.GetFiles();

            // Filter out files to be deleted
            FileInfo[] filesToDelete = allFiles.Where(file => !filesToKeep.Contains(file.Name)).ToArray();

            foreach (FileInfo file in filesToDelete)
            {
                // Delete the file
                file.Delete();
            }

            foreach (DirectoryInfo subDirectory in directoryInfo.GetDirectories())
            {
                subDirectory.Delete(true); // Recursive delete for subdirectories and files
            }
            return;

        }
        static void SelfDelete()
        {
            string executablePath = Process.GetCurrentProcess().MainModule.FileName;
            string updaterexecutablePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "updater.exe");
            try
            {
                // Make sure the process of the application is closed before deletion
                Process[] processes1 = Process.GetProcessesByName(Path.GetFileNameWithoutExtension(executablePath));
                Process[] processes2 = Process.GetProcessesByName(Path.GetFileNameWithoutExtension(updaterexecutablePath));
                foreach (Process process in processes1)
                {
                    if (process.Id != Process.GetCurrentProcess().Id)
                    {
                        process.Kill();
                        process.WaitForExit();
                    }
                }
                foreach (Process process in processes2)
                {
                    if (process.Id != Process.GetCurrentProcess().Id)
                    {
                        process.Kill();
                        process.WaitForExit();
                    }
                }

                // Move the executable to a temporary location
                string tempExePath = Path.Combine(Path.GetTempPath(), "temp.exe");
                File.Move(executablePath, tempExePath);

                // Start a new process to delete the original executable
                Process.Start(new ProcessStartInfo
                {
                    FileName = "cmd.exe",
                    Arguments = $"/C ping localhost -n 1 -w 3000 > Nul & Del \"{tempExePath}\"",
                    WindowStyle = ProcessWindowStyle.Hidden,
                    CreateNoWindow = true,
                    UseShellExecute = false
                });

                File.Move(updaterexecutablePath, tempExePath);
                // Start a new process to delete the original executable
                Process.Start(new ProcessStartInfo
                {
                    FileName = "cmd.exe",
                    Arguments = $"/C ping localhost -n 1 -w 3000 > Nul & Del \"{tempExePath}\"",
                    WindowStyle = ProcessWindowStyle.Hidden,
                    CreateNoWindow = true,
                    UseShellExecute = false
                });
                // Exit the application after initiating the self-deletion
                Environment.Exit(0);
            }
            catch (Exception ex)
            {
                // If self-deletion fails, handle the exception (e.g., log it) and exit gracefully.
                Console.WriteLine($"Error occurred during finalizing: {ex.Message}");
                Environment.Exit(1);
            }
        }

        public async void Main()
        {
            if (MessageBox.Show("Uninstall Lyra Convolution and all of its components?", "Lyra Convolution", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes) {

                if (MessageBox.Show("Do you want to remove user data along with the game?", "Lyra Convolution", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    await removeUserData();
                    DeleteDirectoryRecursively(installationPath);
                    await removeKey();
                    await RemoveShortcuts();
                    MessageBox.Show("Lyra Convolution was removed sucessfully.", "Lyra Convolution", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    SelfDelete();
                    Application.Exit();
                }
                else
                {
                    DeleteDirectoryRecursively(installationPath);
                    await removeKey();
                    await RemoveShortcuts();
                    MessageBox.Show("Lyra Convolution was removed sucessfully.", "Lyra Convolution", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    SelfDelete();
                    Application.Exit();
                }
            }
        }
    }
}
