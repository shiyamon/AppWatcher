
using System;
using System.Collections.Generic;
using System.Linq;
using MonoMac.Foundation;
using MonoMac.AppKit;
using System.IO;

namespace AppWatcher
{
	public partial class MainWindowController : MonoMac.AppKit.NSWindowController
	{
		private const string cfgFilename = "awConfig.cfg";
		private ProcessManager procMng;

		#region Constructors

		// Called when created from unmanaged code
		public MainWindowController (IntPtr handle) : base (handle)
		{
			Initialize ();
		}
		
		// Called when created directly from a XIB file
		[Export ("initWithCoder:")]
		public MainWindowController (NSCoder coder) : base (coder)
		{
			Initialize ();
		}
		
		// Call to load from the XIB/NIB file
		public MainWindowController () : base ("MainWindow")
		{
			Initialize ();
		}
		
		// Shared initialization code
		void Initialize ()
		{
			// my code starts here..............................................
			string appToLaunch = LoadConfig ();
			procMng = new ProcessManager (appToLaunch);
		}

		#endregion

		//strongly typed window accessor
		public new MainWindow Window {
			get {
				return (MainWindow)base.Window;
			}
		}

		public override void AwakeFromNib ()
		{
			base.AwakeFromNib ();

			quitBtn.Activated += (object sender, EventArgs e) => {
				procMng.quitProcess();
				Window.Close();
			};
		}

		private string LoadConfig()
		{
			string exepath = System.Reflection.Assembly.GetExecutingAssembly().Location;
			DirectoryInfo exeDir = Directory.GetParent(exepath);
			string cfgFilepath = Path.Combine (exeDir.Parent.Parent.Parent.FullName, cfgFilename);

			string cfgTxt = File.ReadAllText (cfgFilepath);
			string appToLaunch = cfgTxt.Split ('=') [1];

			return appToLaunch;
		}
	}
}

