using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Diagnostics;

namespace AccessInvestigation
{
    public class Execute
    {
        public Execute(string command)
        {
            // create the ProcessStartInfo using "cmd" as the program to be run,
            // and "/c " as the parameters.
            // Incidentally, /c tells cmd that we want it to execute the command that follows,
            // and then exit.
            // string command = "REG QUERY HKCU\\Software\\Microsoft\\Windows\\CurrentVersion\\Run";
           
            System.Diagnostics.ProcessStartInfo procStartInfo =new System.Diagnostics.ProcessStartInfo("cmd", "/c " + command);
            // The following commands are needed to redirect the standard output.
            // This means that it will be redirected to the Process.StandardOutput StreamReader.
            procStartInfo.RedirectStandardOutput = true;
            procStartInfo.UseShellExecute = false;
            // Do not create the black window.
            procStartInfo.CreateNoWindow = true;
            // Now we create a process, assign its ProcessStartInfo and start it
            System.Diagnostics.Process proc = new System.Diagnostics.Process();
            proc.StartInfo = procStartInfo;
            proc.Start();
            // Get the output into a string
            string result = proc.StandardOutput.ReadToEnd();
            // Write the string to a file.
            try
            {
                if (command == "netstat -anb") 
                   {
                    System.IO.StreamWriter file = new System.IO.StreamWriter("files\\Port.txt", true);
                    file.WriteLine(result);
                    file.Close();
                   // AES.EncryptFile("files\\Port.txt","files\\encPort.txt",Connection.key,Connection.IV);
                   }
                if (command == "forfiles /P C:\\Users\\slouma\\Desktop\\ /D +14/2/2014 /M *.exe")
                {
                    System.IO.StreamWriter file = new System.IO.StreamWriter("files\\ModifiedFiles.txt", true);
                    file.WriteLine(result);
                    file.Close();
                }
               /*   if (command == "netstat -anb")
                {
                    System.IO.StreamWriter file = new System.IO.StreamWriter("files\\Port.txt", true);
                    file.WriteLine(result);
                    file.Close();
                }*/
                else
                { 
                    System.IO.StreamWriter file = new System.IO.StreamWriter("files\\Register.txt", true);
                    file.WriteLine(result);
                    file.Close();
                
                }
                
            }
            catch (IOException e)
            {
                Console.WriteLine(
                    "{0}: The write operation could not " +
                    "be performed because the specified " +
                    "part of the file is locked.",
                    e.GetType().Name);
            }
        }
    }
}
