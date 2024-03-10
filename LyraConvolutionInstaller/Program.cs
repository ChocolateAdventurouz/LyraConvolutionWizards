﻿/*
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
                                                                  

Program.cs: Written by Karapatakis Aggelos (ChocolateAdventurouz)
Based on Race Fantasy Wizards *Program.cs* - https://github.com/ChocolateAdventurouz/RaceFantasyWizards/blob/master/RaceFantasyInstaller/Program.cs
Modified & Improved for Lyra Convolution Wizards
Written at: 24/08/2023
Included at: 27/2/2024
 */

using LyraConvolutionWizards.Helpers;
using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Windows.Forms;

namespace LyraConvolutionWizards
{
    internal static class Program
    {
        static void RunApplication()
        {
            Application.Run(new MainWindow());
        }

        static void ActLikeSfx()
        {
            string tempPath = Path.Combine(Path.GetTempPath(), "LyraInstall");

            try
            {
                // Create the temporary directory if it doesn't exist
                Directory.CreateDirectory(tempPath);

                // Write files into the temporary directory
                File.WriteAllBytes(Path.Combine(tempPath, "System.Buffers.dll"), Properties.WizardResources.System_Buffers);
                File.WriteAllBytes(Path.Combine(tempPath, "System.Memory.dll"), Properties.WizardResources.System_Memory);
                File.WriteAllBytes(Path.Combine(tempPath, "System.Numerics.Vectors.dll"), Properties.WizardResources.System_Numerics_Vectors);
                File.WriteAllBytes(Path.Combine(tempPath, "System.Runtime.CompilerServices.Unsafe.dll"), Properties.WizardResources.System_Runtime_CompilerServices_Unsafe);
                File.WriteAllBytes(Path.Combine(tempPath, "System.Text.Encodings.Web.dll"), Properties.WizardResources.System_Text_Encodings_Web);
                File.WriteAllBytes(Path.Combine(tempPath, "System.Threading.Tasks.Extensions.dll"), Properties.WizardResources.System_Threading_Tasks_Extensions);
                File.WriteAllBytes(Path.Combine(tempPath, "Microsoft.Deployment.Compression.dll"), Properties.WizardResources.Microsoft_Deployment_Compression);
                File.WriteAllBytes(Path.Combine(tempPath, "Microsoft.Bcl.AsyncInterfaces.dll"), Properties.WizardResources.Microsoft_Bcl_AsyncInterfaces);
                File.WriteAllBytes(Path.Combine(tempPath, "Microsoft.Deployment.Compression.Cab.dll"), Properties.WizardResources.Microsoft_Deployment_Compression_Cab);
                File.WriteAllBytes(Path.Combine(tempPath, "System.ValueTuple.dll"), Properties.WizardResources.System_ValueTuple);
                File.Copy(Assembly.GetEntryAssembly().Location, tempPath + "\\" + Path.GetFileName(Assembly.GetEntryAssembly().Location), true);
                // Create a marker file to indicate completion
                File.Create(Path.Combine(tempPath, ".sfxcomplete"));

                
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error while extracting DLLs: {ex.Message}");
            }
        }

        static void StartExecution()
        {
            Application.SetCompatibleTextRenderingDefault(false);
            Application.EnableVisualStyles();

            SystemHelper host = new SystemHelper();
            if (host.GetArchitecture() == "64-bit")
            {
                RunApplication();
            }
            else
            {
                MessageBox.Show("Lyra is not supported on this system.", "Lyra - Error");
            }
        }

        [STAThread]
        static void Main()
        {
            string tempPath = Path.Combine(Path.GetTempPath(), "LyraInstall");

            try
            {
                string markerFilePath = Path.Combine(tempPath, ".sfxcomplete");

                // Check if the marker file exists
                if (File.Exists(markerFilePath))
                {
                    StartExecution();
                }
                else
                {
                    ActLikeSfx();
                    Process.Start(Path.Combine(tempPath, Path.GetFileName(Assembly.GetEntryAssembly().Location)));
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in main method: {ex.Message}");
            }
        }
    }
}
