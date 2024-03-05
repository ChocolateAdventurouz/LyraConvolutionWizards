using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LyraConvolutionWizards.Helpers
{
    public class SystemHelper
    {
        ///<summary>
        /// Get host's architecture.
        /// </summary>
        public string GetArchitecture()
        {

            string x86 = "32-bit";
            string x64 = "64-bit";

            if (System.Runtime.InteropServices.RuntimeInformation.OSArchitecture == System.Runtime.InteropServices.Architecture.X64) { return x64; } // Was breaking on Windows 11, method rewritten
            else { return x86; }
        }


        /// <summary>
        /// Get host's running version of Windows and compares if it is Windows 7 or later.
        /// </summary>
        /// <returns>
        /// true if it is running windows 7 or later
        /// false if it runs an older version of windows such as 2000, XP32/64 Vista32/64 etc
        /// </returns>
        public bool IsWindows7OrLater() // Seems to break on Windows 11
        {

            if (Environment.OSVersion.Version.Major >= 7) { return true; }
            else { return false; }
        }
    }
}
