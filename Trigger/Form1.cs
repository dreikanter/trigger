using System;
using System.Diagnostics;
using System.Windows.Forms;
using Microsoft.Win32;
using log4net;

namespace Trigger
{
    public partial class MainForm : Form
    {
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public MainForm()
        {
            log.Info("Service started");
            InitializeComponent();
            SystemEvents.SessionSwitch += OnSessionSwitch;
        }

        private void OnMenuItemExitClick(object sender, EventArgs e)
        {
            Close();
            log.Info("Service terminated");
        }

        private static void OnSessionSwitch(object sender, SessionSwitchEventArgs e)
        {
            switch(e.Reason)
            {
                case SessionSwitchReason.SessionUnlock:
                    OnSessionUnlock();
                    break;

                default:
                    break;
            }
        }

        private static void OnSessionUnlock()
        {
            var cmd = @"D:\hp\bin\sendver.bat";
            log.Debug(String.Format("Session unlocked. Executing '{0}'...", cmd));
            var p = new Process
                {
                    StartInfo =
                        {
                            UseShellExecute = false,
                            RedirectStandardOutput = true,
                            FileName = cmd
                        }
                };
            p.Start();
            var output = p.StandardOutput.ReadToEnd();
            p.WaitForExit();
            log.Debug(output);
        }
    }
}
