using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Win32;
using System.Windows.Forms;
using System.Diagnostics;
using System.IO;

namespace AccessInvestigation
{
   
        
    class Register
    {
        public void ShowErrorMessage()
        {
            Process p = new Process();
            // Redirect the output stream of the child process.
            p.StartInfo.UseShellExecute = false;
            p.StartInfo.RedirectStandardOutput = true;
            string nameFile = @".\file.bat";
            string chemin = Path.GetFullPath(nameFile);
            if (File.Exists(chemin))
            {
                MessageBox.Show("exist");
            }
            else MessageBox.Show("pas de fichier");
            p.StartInfo.FileName = @".\file.bat";
            p.Start();
            //Do not wait for the child process to exit before
            // reading to the end of its redirected stream.
             p.WaitForExit();
            // Read the output stream first and then wait.
            string output = p.StandardOutput.ReadToEnd();
            MessageBox.Show(output);
            p.WaitForExit();
            //MessageBox.Show(v);
        }

            
     }
        
}
