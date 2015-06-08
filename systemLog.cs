using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Security;

namespace AccessInvestigation
{
    class systemLog
    {
        System.IO.StreamWriter file;
        public void AppLog(String log)
        {
            String machine = "."; // local machine
            //log peut étre system oubien application
                    EventLog aLog = new EventLog(log, machine);
                    EventLogEntry entry;
                    EventLogEntryCollection entries = aLog.Entries;
                    
                    //Stack<EventLogEntry> stack = new Stack<EventLogEntry>();
                    //entries.Count
                    if (entries.Count != 0 & entries.Count > 1000)
                    {
                        for (int i = 0; i < 1000; i++)
                        {
                            entry = entries[i];
                            // stack.Push(entry); 
                            if (log == "application")
                            {
                              file = new System.IO.StreamWriter("files\\ApplicationLog.txt", true);
                            }
                            else if (log == "system")
                            {
                               file = new System.IO.StreamWriter("files\\SystemLog.txt", true);
                            }  
                            file.WriteLine("[Index]\t" + entry.Index +
                                      "\n[EventID]\t" + entry.InstanceId +
                                      "\n[TimeWritten]\t" + entry.TimeWritten +
                                      "\n[MachineName]\t" + entry.MachineName +
                                      "\n[Source]\t" + entry.Source +
                                      "\n[UserName]\t" + entry.UserName +
                                      "\n[Message]\t" + entry.Message +
                                      "\n---------------------------------------------------\n");
                            file.Close();
                        }
                    }
                    else if (entries.Count != 0 & entries.Count < 1000)
                    {


                        for (int i = 0; i < entries.Count; i++)
                        {
                            entry = entries[i];
                            // stack.Push(entry);
                            if (log == "application")
                            {
                                file = new System.IO.StreamWriter("files\\ApplicationLog.txt", true);
                            }
                            else if (log == "system")
                            {
                                file = new System.IO.StreamWriter("files\\SystemLog.txt", true);
                            }  
                            file.WriteLine("[Index]\t" + entry.Index +
                                      "\n[EventID]\t" + entry.InstanceId +
                                      "\n[TimeWritten]\t" + entry.TimeWritten +
                                      "\n[MachineName]\t" + entry.MachineName +
                                      "\n[Source]\t" + entry.Source +
                                      "\n[UserName]\t" + entry.UserName +
                                      "\n[Message]\t" + entry.Message +
                                      "\n---------------------------------------------------\n");
                            file.Close();
                        }
                    }

                    // entry = stack.Pop();// only display the last record
                }
                
            }

        }