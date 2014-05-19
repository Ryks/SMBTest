using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Diagnostics;

namespace SMBTest
{
	class Program
	{
		static void Main(string[] args)
		{
			// Specify the directory you want to manipulate.
			string path = @".\MyDir";
			
			// for debug
			foreach (string s in args)
			{
				Console.Write(s + " ");
			}
			Console.WriteLine("args[0] : " + args[0]);
			
			// set target directory
			if(args.Length > 0)
			{
				if(args[0] == "/?")
				{
					Console.WriteLine("Using: <this.command> + (space) + (parameter)");
					Console.WriteLine("[Example]");
					Console.WriteLine("<this.command> \"\\\\NFS(Server Name)\\target directory\" ");
					Console.WriteLine("<this.command> \"\\local directory\" ");
					Console.WriteLine("----------");
					return;
				}	
				path = @"" + args[0] + @"\MyDir";
			}
			
			Console.WriteLine("Target Dir : " + path);
			try
			{
				Console.WriteLine("Connecting to Target Dir : " + path);
				// Determine whether the directory exists.
				DirectoryInfo parent = Directory.GetParent(path);
				string str = parent.FullName;
				if (Directory.Exists(str) == false)
				{
					Console.WriteLine("Parent path does not exsist.");
					
					// Write Faild to EventLog
					string cpt = ".";						// computer name
					string log = "Application";				// event
					string src = "PowerShell Script";		// source 
					string msg = "Error in Parent path does not exsist. : " + str;	// message

					if (!EventLog.SourceExists(src, cpt))
					{
						EventSourceCreationData data = new EventSourceCreationData(src, log);
						EventLog.CreateEventSource(data);
					}
					EventLog evlog = new EventLog(log, cpt, src);
					evlog.WriteEntry(msg, EventLogEntryType.Error);
					return;
				}
				else
				{
					Console.WriteLine("Find Target Dir : " + path);
				}
				Console.WriteLine("Continue to Target Dir : " + path);
				if (Directory.Exists(path))
				{
					Console.WriteLine("That path exists already.");
					return;
				}
				
				// Try to create the directory.
				DirectoryInfo di = Directory.CreateDirectory(path);
				Console.WriteLine("The directory was created successfully at {0}.", Directory.GetCreationTime(path));
				
				// Delete the directory.
				di.Delete();
				Console.WriteLine("The directory was deleted successfully.");
				
				// Write success to EventLog
				string scpt = ".";						// computer name
				string slog = "Application";			// event
				string ssrc = "PowerShell Script";		// source 
				if (!EventLog.SourceExists(ssrc, scpt))
				{
					EventSourceCreationData data = new EventSourceCreationData(ssrc, slog);
					EventLog.CreateEventSource(data);
				}
				EventLog sevlog = new EventLog("Application", scpt, "PowerShell Script");
				sevlog.WriteEntry("Success! :" + path, EventLogEntryType.Information);
				
			}
			catch (Exception e)
			{
				Console.WriteLine("The process failed: {0}", e.ToString());
			}
			finally { }
		}

		void EventWrite()
		{
			
		}
	}
}
