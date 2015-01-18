using System;
using System.IO;
using System.Diagnostics;
using System.Xml;
using Data;

namespace AppWatcher
{
	public class ProcessManager
	{
		private ProcessStartInfo appStartInfo;
		private Process appProcess;
		private string logDir;
		private string logFilename;

		public ProcessManager (string appToLaunch)
		{
			// find executable from plist
			string plistPath = Path.Combine (appToLaunch, "Contents/Info.plist");
			string exeToLaunch = findExeToLaunch (plistPath);

			// decide logfilename
			DateTime time = DateTime.Now;
			logFilename = time.ToString("yyyy-MM-dd-HH-mm-ss");

			DirectoryInfo exeDir = Directory.GetParent (System.Reflection.Assembly.GetExecutingAssembly ().Location);
			logDir = Path.Combine(exeDir.Parent.Parent.Parent.FullName, "log");

			// start process
			appStartInfo = new ProcessStartInfo ();
			appStartInfo.FileName = exeToLaunch;
			appStartInfo.UseShellExecute = false;
			appProcess = Process.Start (appStartInfo);
			appProcess.EnableRaisingEvents = true;
			appProcess.Exited += new EventHandler (onProcessExited);
		}

		public void quitProcess()
		{
			appProcess.CloseMainWindow ();
			appProcess.Close ();
		}

		private void onProcessExited(object sender, System.EventArgs e)
		{
			appProcess = Process.Start (appStartInfo);
			appProcess.EnableRaisingEvents = true;
			appProcess.Exited += new EventHandler (onProcessExited);

			// save log
			if (!Directory.Exists (logDir))
				Directory.CreateDirectory (logDir);

			StreamWriter sw = new StreamWriter (Path.Combine (logDir, logFilename), true, System.Text.Encoding.Unicode);
			sw.WriteLine (appProcess.ProcessName + " quite on " + DateTime.Now);
			sw.Close ();
		}

		private string findExeToLaunch(string plistPath)
		{
			PList plist = new PList (plistPath);
			return Path.Combine (Directory.GetParent (plistPath).FullName, "MacOS/" + plist ["CFBundleExecutable"]);
		}
	}
}

