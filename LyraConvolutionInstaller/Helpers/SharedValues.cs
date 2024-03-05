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
                                                                  

SharedValues.cs: Written by Karapatakis Aggelos (ChocolateAdventurouz)
Based based on Race Fantasy Wizards *SharedVariables.cs* - https://github.com/ChocolateAdventurouz/RaceFantasyWizards/blob/master/RaceFantasyInstaller/SharedVariables.cs
Modified & Improved for Lyra Convolution Wizards
Written at: 27/8/2023
Included at: 27/2/2024
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LyraConvolutionWizards.Helpers
{
    internal class SharedValues
    {
        /// <summary>
        /// Used to detect if the checkbox is checked so afther the installer instance is killed, the launcher will be executed
        /// </summary>
        //public bool LaunchRaceFantasy { get; set; }

        /// <summary>
        /// Used to hold the installation path that the user selected
        /// </summary>
        public string InstallationDir { get; set; }

        public bool InstallFinished { get; set; }
        private static SharedValues instance;
        public static SharedValues Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new SharedValues();
                }
                return instance;
            }
        }

    }
}
