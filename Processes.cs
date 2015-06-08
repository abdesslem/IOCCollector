using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace AccessInvestigation
{
    public class Processes
    {
        public void RunProcess()
        {
     Process[] processlist = Process.GetProcesses();
     System.IO.StreamWriter file = new System.IO.StreamWriter("files\\Process.txt", true);
    foreach(Process theprocess in processlist){
    //Console.WriteLine("Process: {0} ID: {1}", theprocess.ProcessName, theprocess.Id);
   // System.IO.StreamWriter file = new System.IO.StreamWriter("Process.txt", true);
    file.WriteLine("Process: {0} ID: {1}", theprocess.ProcessName, theprocess.Id);
  //  file.Close();
        }
    file.Close();
        }

    }
}
