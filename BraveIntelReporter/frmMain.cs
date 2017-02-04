using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using System.Windows.Automation;
using System.Windows.Forms;
using System.Xml;
using Newtonsoft.Json.Linq;


namespace BraveIntelReporter
{
    public partial class 
        frmMain : Form
    {
        long reported = 0;
        long failed = 0;

        private DateTime LastIntelReported = DateTime.MinValue;

        private STATE state = STATE.INIT;
        private System.Timers.Timer timerEveProcessCheck = new System.Timers.Timer();
        private System.Timers.Timer timerFileDiscover = new System.Timers.Timer();
        private System.Timers.Timer timerFileReader = new System.Timers.Timer();
        private System.Timers.Timer timerConfigCheck = new System.Timers.Timer();
        private bool eveRunningLast = false;
        private Dictionary<String, FileInfo> roomToFile = new Dictionary<String, FileInfo>();
        private Dictionary<String, String> roomToLastLine = new Dictionary<String, String>();
        private Dictionary<FileInfo, long> fileToOffset = new Dictionary<FileInfo, long>();
        //private Dictionary<String, int> sytstemNames = new Dictionary<string, int>();
        //string jumpSystems = File.ReadAllText("smallsystems.json");

        [DllImport("User32.dll")]
        protected static extern int SetClipboardViewer(int hWndNewViewer);

        [DllImport("User32.dll", CharSet = CharSet.Auto)]
        public static extern bool ChangeClipboardChain(IntPtr hWndRemove, IntPtr hWndNewNext);

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern int SendMessage(IntPtr hwnd, int wMsg, IntPtr wParam, IntPtr lParam);

        IntPtr nextClipboardViewer;



        private static Object readerLock = new Object(); // Ensures that only one thread can read files at a time.

        #region SetEveToBackground
        /// <summary>
        /// For use with the Set EVE to Background option
        /// </summary>

        /// <summary>
        /// Changes the size, position, and Z order of a child, pop-up or top-level window.
        /// </summary>
        /// <param name="hWnd">A handle to the window.</param>
        /// <param name="hWndInsertAfter">A handle to the window to precede the positioned window in the Z order. (HWND value)</param>
        /// <param name="X">The new position of the left side of the window, in client coordinates.</param>
        /// <param name="Y">The new position of the top of the window, in client coordinates.</param>
        /// <param name="W">The new width of the window, in pixels.</param>
        /// <param name="H">The new height of the window, in pixels.</param>
        /// <param name="uFlags">The window sizing and positioning flags. (SWP value)</param>
        /// <returns>Nonzero if function succeeds, zero if function fails.</returns>
        /// 



        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int X, int Y, int cx, int cy, SetWindowPosFlags uFlags);
        private static IntPtr myhandle;
        static readonly IntPtr HWND_BOTTOM = new IntPtr(1);
        private static string processname = "exefile";
        private Process SelectedKeepInBackgroundProcess = null;
        private List<Process> IgnoredKeepInBackgroundProcesses = new List<Process>();

        private void OnFocusChangedHandler(object src, AutomationFocusChangedEventArgs args)
        {
            if (!mnuSetEveToBackground.Checked) return;
            // Two buckets, SelectedKeepInBackgroundProcess and IgnoredProcess.  When a new process is started, we should be able to guess which bucket it goes into.
            var processes = Process.GetProcesses().Where(p => (p.ProcessName.ToLower() == processname.ToLower() && !p.HasExited)).ToList();
            if (SelectedKeepInBackgroundProcess != null && SelectedKeepInBackgroundProcess.HasExited) SelectedKeepInBackgroundProcess = null;
            // Check that all ignored processes still exist
            for (int i = 0; i < IgnoredKeepInBackgroundProcesses.Count; i++ )
            {
                if (IgnoredKeepInBackgroundProcesses[i].HasExited)
                {
                    IgnoredKeepInBackgroundProcesses.Remove(IgnoredKeepInBackgroundProcesses[i]);
                    i--;
                }
            }
            int knownProcesses = (SelectedKeepInBackgroundProcess == null) ? 0 : 1;
            knownProcesses += IgnoredKeepInBackgroundProcesses.Count;

            if (knownProcesses < processes.Count)
            {
                if (SelectedKeepInBackgroundProcess != null)
                { 
                    // We have the process we want in the background, so ignore all the others.
                    foreach (Process p in processes)
                    {
                        if (SelectedKeepInBackgroundProcess.Id != p.Id && !IgnoredKeepInBackgroundProcesses.Contains(p))
                            IgnoredKeepInBackgroundProcesses.Add(p);
                    }
                }
                else if (knownProcesses == processes.Count - 1)
                {
                    // If we know all but one process, and we don't have a background process, we've found the one to select.
                    foreach (Process p in processes)
                    {
                        if (!IgnoredKeepInBackgroundProcesses.Contains(p))
                        {
                            SelectedKeepInBackgroundProcess = p;
                            break;
                        }
                    }
                }
                else // There's more than one unknown process and we don't know which to select.
                {
                    IgnoredKeepInBackgroundProcesses.Clear();
                    appendText("Unknown which Eve process should be set to background.  Disabling SetToBackground feature.");
                    GetKeepInBackgroundProcess();
                    return;
                }
            }
            if (SelectedKeepInBackgroundProcess != null)
                SetWindowPos(SelectedKeepInBackgroundProcess.MainWindowHandle, HWND_BOTTOM, 0, 0, 0, 0, SetWindowPosFlags.DoNotReposition | SetWindowPosFlags.IgnoreMove | SetWindowPosFlags.DoNotActivate | SetWindowPosFlags.IgnoreResize);
        }

        private void checkClipboard()
        {
 

        

            nextClipboardViewer = (IntPtr)SetClipboardViewer((int)this.Handle);

           

            return;
        }


        private Process GetKeepInBackgroundProcess()
        {
            var processes = Process.GetProcesses().Where(p => p.ProcessName.ToLower() == processname.ToLower()).ToList();

            if (processes.Count == 1 && !IgnoredKeepInBackgroundProcesses.Contains(processes[0])) SelectedKeepInBackgroundProcess = processes[0];
            int knownprocesscount = 0;
            if (SelectedKeepInBackgroundProcess != null) knownprocesscount++;

            if (SelectedKeepInBackgroundProcess != null) return SelectedKeepInBackgroundProcess;
            else if (processes.Count > 1)
            {
                frmSelectProcess selectForm = new frmSelectProcess(processname);
                selectForm.ShowDialog();
                SelectedKeepInBackgroundProcess = selectForm.SelectedProcess;
                IgnoredKeepInBackgroundProcesses = processes;
                IgnoredKeepInBackgroundProcesses.Remove(SelectedKeepInBackgroundProcess);
            }
            return SelectedKeepInBackgroundProcess;
        }

        [Flags()]
        enum SetWindowPosFlags : uint
        {
            /// <summary>If the calling thread and the thread that owns the window are attached to different input queues, 
            /// the system posts the request to the thread that owns the window. This prevents the calling thread from 
            /// blocking its execution while other threads process the request.</summary>
            /// <remarks>SWP_ASYNCWINDOWPOS</remarks>
            AsynchronousWindowPosition = 0x4000,
            /// <summary>Prevents generation of the WM_SYNCPAINT message.</summary>
            /// <remarks>SWP_DEFERERASE</remarks>
            DeferErase = 0x2000,
            /// <summary>Draws a frame (defined in the window's class description) around the window.</summary>
            /// <remarks>SWP_DRAWFRAME</remarks>
            DrawFrame = 0x0020,
            /// <summary>Applies new frame styles set using the SetWindowLong function. Sends a WM_NCCALCSIZE message to 
            /// the window, even if the window's size is not being changed. If this flag is not specified, WM_NCCALCSIZE 
            /// is sent only when the window's size is being changed.</summary>
            /// <remarks>SWP_FRAMECHANGED</remarks>
            FrameChanged = 0x0020,
            /// <summary>Hides the window.</summary>
            /// <remarks>SWP_HIDEWINDOW</remarks>
            HideWindow = 0x0080,
            /// <summary>Does not activate the window. If this flag is not set, the window is activated and moved to the 
            /// top of either the topmost or non-topmost group (depending on the setting of the hWndInsertAfter 
            /// parameter).</summary>
            /// <remarks>SWP_NOACTIVATE</remarks>
            DoNotActivate = 0x0010,
            /// <summary>Discards the entire contents of the client area. If this flag is not specified, the valid 
            /// contents of the client area are saved and copied back into the client area after the window is sized or 
            /// repositioned.</summary>
            /// <remarks>SWP_NOCOPYBITS</remarks>
            DoNotCopyBits = 0x0100,
            /// <summary>Retains the current position (ignores X and Y parameters).</summary>
            /// <remarks>SWP_NOMOVE</remarks>
            IgnoreMove = 0x0002,
            /// <summary>Does not change the owner window's position in the Z order.</summary>
            /// <remarks>SWP_NOOWNERZORDER</remarks>
            DoNotChangeOwnerZOrder = 0x0200,
            /// <summary>Does not redraw changes. If this flag is set, no repainting of any kind occurs. This applies to 
            /// the client area, the nonclient area (including the title bar and scroll bars), and any part of the parent 
            /// window uncovered as a result of the window being moved. When this flag is set, the application must 
            /// explicitly invalidate or redraw any parts of the window and parent window that need redrawing.</summary>
            /// <remarks>SWP_NOREDRAW</remarks>
            DoNotRedraw = 0x0008,
            /// <summary>Same as the SWP_NOOWNERZORDER flag.</summary>
            /// <remarks>SWP_NOREPOSITION</remarks>
            DoNotReposition = 0x0200,
            /// <summary>Prevents the window from receiving the WM_WINDOWPOSCHANGING message.</summary>
            /// <remarks>SWP_NOSENDCHANGING</remarks>
            DoNotSendChangingEvent = 0x0400,
            /// <summary>Retains the current size (ignores the cx and cy parameters).</summary>
            /// <remarks>SWP_NOSIZE</remarks>
            IgnoreResize = 0x0001,
            /// <summary>Retains the current Z order (ignores the hWndInsertAfter parameter).</summary>
            /// <remarks>SWP_NOZORDER</remarks>
            IgnoreZOrder = 0x0004,
            /// <summary>Displays the window.</summary>
            /// <remarks>SWP_SHOWWINDOW</remarks>
            ShowWindow = 0x0040,
        }
        #endregion

        enum STATE
        {
            INIT, START, RUNNING, DOWNTIME, STOP
        };

        private Boolean isEveRunning()
        {
            return (Process.GetProcesses().Where(p => p.ProcessName.ToLower() == "exefile").ToList().Count() != 0);
        }

        private void updateLatestIntelFiles()
        {
            if (Configuration.EnableReporting == false)
            {
                appendVerbose("Reporting disabled in settings.");
                return;
            }
            if (LastIntelReported > (DateTime.Now.AddMilliseconds(-1 * timerFileDiscover.Interval))) return; //If intel has been reported recently, we don't need to recheck.
            if (DateTime.UtcNow.TimeOfDay > new TimeSpan(10, 59, 00) && DateTime.UtcNow.TimeOfDay < new TimeSpan(11, 05, 00))
            {
                setState(STATE.DOWNTIME);
                appendText("Downtime Detected.  Waiting for new chat logs to be created.");
            }
            if (state == STATE.RUNNING)
            {
                appendVerbose("Sending heartbeat.");
               // ReportIntel(string.Empty, "Running");
            }
            appendVerbose("Updating chatlog file list.");
            string oldfiles = string.Empty;
            foreach (FileInfo fi in roomToFile.Values)
                oldfiles += fi.Name + ", ";

            string report = string.Empty;
            foreach (String roomName in Configuration.RoomsToMonitor)
            {
                Debug.WriteLine("KIU Checking for : " + roomName);

                FileInfo[] files = new DirectoryInfo(Configuration.LogDirectory)
                        .GetFiles(roomName + "_*.txt", SearchOption.TopDirectoryOnly);
                FileInfo fi = files.OrderByDescending(f => f.LastWriteTime).FirstOrDefault();

                if (fi == null)
                {
                    continue;
                }

                Debug.WriteLine("KIU Latest: " + fi);

                // Check if eve has opened this file -> Eve is running and user has joined channel
                Boolean inUse = false;
                try
                {
                    FileStream fs = fi.Open(FileMode.Open, FileAccess.Read, FileShare.None);
                    fs.Close();
                }
                catch
                {
                    inUse = true;
                }

                if (!inUse)
                {
                    Debug.WriteLine("KIU Skipping: " + fi);
                    continue;
                }

                Debug.WriteLine("KIU Using: " + fi);
                roomToFile[roomName] = fi;
                report += fi.Name + "\r\n";
            }

            // Clear offset list of old files if necessary.
            List<FileInfo> deletethese = new List<FileInfo>();
            foreach (FileInfo fi in fileToOffset.Keys)
                if (!roomToFile.ContainsValue(fi)) deletethese.Add(fi);
            foreach (FileInfo fi in deletethese)
                fileToOffset.Remove(fi);

            // If a new file is created, recheck to make sure we have the most up to date log files.
            FileSystemWatcher watcher = new FileSystemWatcher(Configuration.LogDirectory);
            watcher.NotifyFilter = NotifyFilters.CreationTime;
            watcher.Created += new FileSystemEventHandler(FileCreated);
            watcher.EnableRaisingEvents = true;

            string newfiles = string.Empty;
            foreach (FileInfo fi in roomToFile.Values)
                newfiles += fi.Name + ", ";
            if (!newfiles.Equals(oldfiles))
            {
                if (newfiles.Length > 2) newfiles = newfiles.Substring(0, newfiles.Length - 2); // trim the last comma and space
                if (oldfiles.Length > 2) oldfiles = oldfiles.Substring(0, oldfiles.Length - 2); // trim the last comma and space
                if (state == STATE.RUNNING || state == STATE.DOWNTIME) appendText(string.Format("Intel Files Changed. Old Files: {0}, New Files: {1}", oldfiles, newfiles));
                if (state == STATE.DOWNTIME) setState(STATE.START);
            }
            lblMonitoringFiles.Invoke(new MethodInvoker(() => lblMonitoringFiles.Text = report));
        }

        private void FileCreated(object sender, FileSystemEventArgs e)
        {
            appendVerbose("New File Detected: " + e.Name);
            updateLatestIntelFiles();
        }

        private void setState(STATE nState)
        {
            if (state == nState)
            {
                return;
            }

            state = nState;
            Debug.WriteLine("KIU STATE: " + nState);

            if (STATE.START == nState)
            {
                execEveTimer(null, null);
                execFileDiscoverTimer(null, null);
                execFileReaderTimer(null, null);

                timerFileDiscover.Start();
                timerFileReader.Interval = Configuration.MonitorFrequency;
                timerFileReader.Start();
                //ReportIntel(string.Empty, "start");
                appendVerbose("EVE State Change.  Current State: " + Enum.GetName(typeof(STATE), state));
                setState(STATE.RUNNING);
            }
            if (STATE.STOP == nState)
            {
                timerFileDiscover.Stop();
                timerFileReader.Stop();
                //ReportIntel(string.Empty, "stop");
                appendVerbose("EVE State Change.  Current State: " + Enum.GetName(typeof(STATE), state));
            }
            if (STATE.DOWNTIME == nState)
            {
                timerFileReader.Stop();
               // ReportIntel(string.Empty, "stop");
                appendVerbose("EVE State Change.  Current State: " + Enum.GetName(typeof(STATE), state));
            }
        }

        private void init()
        {
            // checkClipboard();
           
            timerEveProcessCheck.Elapsed += new ElapsedEventHandler(execEveTimer);
            timerEveProcessCheck.Interval = 1000 * 60 * 1;
            timerEveProcessCheck.Start();

            timerFileDiscover.Elapsed += new ElapsedEventHandler(execFileDiscoverTimer);
            timerFileDiscover.Interval = 1000 * 60 * 2;

            timerFileReader.Elapsed += new ElapsedEventHandler(execFileReaderTimer);

            timerConfigCheck.Elapsed += new ElapsedEventHandler(execConfigCheckTimer);
            timerConfigCheck.Interval = Configuration.ConfigCheckFrequency * 1000 * 60;

            myhandle = this.Handle; // for SetEveToBackground
            Automation.AddAutomationFocusChangedEventHandler(OnFocusChangedHandler);

            mnuSetEveToBackground.Checked = Configuration.SetEveToBackground;
            mnuOutputVerbose.Checked = Configuration.Verbose;
            mnuOutputMinimal.Checked = !Configuration.Verbose;

            if (Configuration.SetEveToBackground) GetKeepInBackgroundProcess();

            if (Configuration.CheckClipboard)
            {
                
                Thread oThread = new Thread(new ThreadStart(checkClipboard));
                oThread.Start();
                
            }
        }

        private void execEveTimer(object sender, EventArgs e)
        {
            Boolean eveRunning = isEveRunning();
            if (eveRunning == eveRunningLast)
            {
                return;
            }
            eveRunningLast = eveRunning;
            if (eveRunning)
            {
                setState(STATE.START);
            }
            else
            {
                setState(STATE.STOP);
            }
        }

        private void execFileDiscoverTimer(object sender, EventArgs e)
        {
            updateLatestIntelFiles();
        }

        private void execConfigCheckTimer(object sender, EventArgs e)
        {
            string report = string.Empty;
            Configuration.GetConfig(out report);
            appendVerbose(report);
        }

        private void execFileReaderTimer(object sender, EventArgs e)
        {
            if (!Monitor.TryEnter(readerLock))
            {
                Debug.WriteLine("File Reader Thread: Locked");
                return; // Ensures that only one thread can read files at a time.
            }
            if (!Configuration.EnableReporting)
            {
                Debug.WriteLine("Reporting disabled in settings.");
                return;
            }
            FileStream logFileStream;
            StreamReader logFileReader;
            // dictionary of system names so i can link intel to actual systems
             Dictionary<string, string> sytstemNames = File.ReadLines(@"jumpsystems.csv").Select(lines => lines.Split(',')).ToDictionary(gr => gr[0],gr => gr[1]);


            String line;
            String bigLine;


            try
            {
                foreach (String roomName in Configuration.RoomsToMonitor)
                {
                    FileInfo logfile = null;
                    roomToFile.TryGetValue(roomName, out logfile);

                    if (logfile == null)
                    {
                        Debug.WriteLine("KIU Skipping room: " + roomName);
                        continue;
                    }

                    long offset = 0;
                    fileToOffset.TryGetValue(logfile, out offset);

                    logfile.Refresh();
                    Debug.WriteLine("Offset: " + offset.ToString());
                    Debug.WriteLine("File Length: " + logfile.Length.ToString());
                    if (offset != 0 && logfile.Length == offset) continue; // No new data in file
                    logFileStream = new FileStream(logfile.FullName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                    logFileReader = new StreamReader(logFileStream);
                    logFileReader.BaseStream.Seek(offset, SeekOrigin.Begin);
                    bigLine = "";

                    while (!logFileReader.EndOfStream)
                    {
                        line = logFileReader.ReadLine();

                        if (line.Trim().Length == 0)
                        {
                            continue;
                        }

                        //line = line.Remove(0, 1);
                        if (line.Length < 23) continue;
                        DateTime utcTimeOfLine = DateTime.MinValue;
                        if (!DateTime.TryParse(line.Substring(2, 19), out utcTimeOfLine)) continue;
                        Double minutesFromNow = Math.Abs(DateTime.UtcNow.Subtract(utcTimeOfLine).TotalMinutes);
                        //I think this is saying... if the intel isnt fresh, dont bother
                        if (minutesFromNow > 2) continue;

                        //if it is less than 2 mins.. do bother

                        //lets look to see if a system was referenced....
                        appendText(line);
                        string examineLine = "" + line;
                        examineLine = examineLine.Split('>')[1];
                        foreach (KeyValuePair<string, string> key in sytstemNames)
                        {
                            string formerString = " " + key.Key ;
                           
                            if (examineLine.Contains(formerString))
                            {
                                
                                string replacestring = "<url=showinfo:5//" + key.Value + ">" + key.Key + "</url>";
                                line =line.Replace(formerString, replacestring);

                            }

                        }
                        //sends it
                        //ReportIntel(line);
                        //make a big line

                        bigLine = bigLine + "\\n"+ line;
                        LastIntelReported = DateTime.Now;
                      
                    }

                    ReportIntel(bigLine);
                    for (int i = 1; i < 300; i++) //wait 30 seconds. 
                    {
                        Application.DoEvents();
                        System.Threading.Thread.Sleep(100);
                    }
                    offset = logfile.Length;
                    if (fileToOffset.ContainsKey(logfile)) fileToOffset[logfile] = offset;
                    else fileToOffset.Add(logfile, offset);

                    // Clean up
                    logFileReader.Close();
                    logFileStream.Close();
                } // foreach
            } // try
            catch (Exception ex)
            {
                appendText(string.Format("Intel Server Error: {0}\r\n", ex.Message));
            }
            finally { Monitor.Exit(readerLock); }
        }

        private void appendText(String line)
        {
            Debug.WriteLine("KIU append: " + line);
            this.txtIntel.Invoke(new MethodInvoker(() => this.txtIntel.AppendText(line + "\r\n")));
        }

        private void appendVerbose(string line)
        {
            Debug.WriteLine("KIU verbose: " + line);
            if (Configuration.Verbose)
                this.txtIntel.Invoke(new MethodInvoker(() => this.txtIntel.AppendText(line + "\r\n")));
        }
        public frmMain()
        {
            InitializeComponent();
        }

        private void frmMain_Load(object sender, EventArgs e)
        {

            string report = string.Empty;
            bool haveglobalsettings = false;
            while (!haveglobalsettings)
            {
                haveglobalsettings = Configuration.GetConfig(out report);
                if (!haveglobalsettings)
                {
                    appendText(report);
                    appendText("Waiting 30 seconds and retrying.");
                    for (int i = 1; i < 300; i++) // wait 30 seconds 
                    {
                        Application.DoEvents();
                        System.Threading.Thread.Sleep(100);
                    }
                }
            }

            if (Configuration.FirstRun)
            {
                new frmSettings().ShowDialog();
            }
            init();

            setState(STATE.START);

        }
        private bool InvalidAuthToken = false;
        private void ReportIntel(string lastline, string status = "")
        {
            Encoding myEncoding = System.Text.UTF8Encoding.UTF8;
            WebClient client = new WebClient();
            try
            {
                if (lastline.Contains("EVE System > Channel MOTD:")) return;
                lastline = lastline.Replace('"', '\'');
                string postMessage = new ReportLine(lastline, status).ToJson();
                byte[] KiuResponse = client.UploadData(Configuration.ReportServer, "POST", myEncoding.GetBytes(postMessage));

                if (myEncoding.GetString(KiuResponse) == "OK\n") reported++;
            }
            catch (Exception ex)
            {
                failed++;
                if (ex.Message == "The remote server returned an error: (401) Unauthorized." && !InvalidAuthToken)
                {
                    appendText("Authorization Token Invalid.  Try refreshing your auth token in settings.\r\n");
                    if (Configuration.MapURL != string.Empty)
                    {
                        ProcessStartInfo sInfo = new ProcessStartInfo(Configuration.MapURL + "/?nav=uploader");
                        Process.Start(sInfo);
                    }
                    MessageBox.Show("Authorization Token Invalid.  Try refreshing your auth token in settings.\r\n");
                        new frmSettings().ShowDialog();
                    InvalidAuthToken = true; // We really don't want to pop up windows and webpages more than once.
                }
                else if (ex.Message == "The remote server returned an error: (426) 426.")
                    appendText("Client version not supported.  Please close and restart application to update. (May require two restarts.)\r\n");
                else
                    appendText(string.Format("Intel Server Error: {0}\r\n", ex.Message));
                Debug.Write(string.Format("Exception: {0}", ex.Message));
            }
            lblReported.Invoke(new MethodInvoker(() => lblReported.Text = reported.ToString()));
            lblFailed.Invoke(new MethodInvoker(() => lblFailed.Text = failed.ToString()));
        }

        private void frmMain_Resize(object sender, EventArgs e)
        {
            if (this.WindowState == FormWindowState.Minimized)
            {
                this.Hide();
                notifyIcon1.Visible = true;
                notifyIcon1.BalloonTipText = "Minimized to system tray.";
                notifyIcon1.ShowBalloonTip(500);
                this.ShowInTaskbar = false;
            }
            else if (this.WindowState == FormWindowState.Normal)
            {
                notifyIcon1.Visible = false;
                this.ShowInTaskbar = true;
            }
        }

        private void notifyIcon1_DoubleClick(object sender, EventArgs e)
        {
            this.Show();
            this.WindowState = FormWindowState.Normal;
            this.ShowInTaskbar = true;
            notifyIcon1.Visible = false;
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new frmAbout().ShowDialog();
        }

        private void mnuViewMap_Click(object sender, EventArgs e)
        {
            if (Configuration.MapURL != string.Empty)
            {
                //mnuViewMap_Click
                ProcessStartInfo sInfo = new ProcessStartInfo(Configuration.MapURL);
                Process.Start(sInfo);
            }
        }

        private void frmMain_FormClosed(object sender, FormClosedEventArgs e)
        {
            ReportIntel(string.Empty, "stop");
        }

        private void optionsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new frmSettings().ShowDialog();
        }

        private void mnuOutputMinimal_Click(object sender, EventArgs e)
        {
            Configuration.Verbose = false;
            Configuration.Save();
            mnuOutputMinimal.Checked = true;
            mnuOutputVerbose.Checked = false;
        }

        private void mnuOutputVerbose_Click(object sender, EventArgs e)
        {
            Configuration.Verbose = true;
            Configuration.Save();
            mnuOutputMinimal.Checked = false;
            mnuOutputVerbose.Checked = true;
        }

        private void settingsToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void mnuSetEveToBackground_Click(object sender, EventArgs e)
        {
            mnuSetEveToBackground.Checked = !mnuSetEveToBackground.Checked;
            Configuration.SetEveToBackground = mnuSetEveToBackground.Checked;
            Configuration.Save();
            if (Configuration.SetEveToBackground)
            {
                SelectedKeepInBackgroundProcess = null;
                GetKeepInBackgroundProcess();
            }
            else
            {
                SelectedKeepInBackgroundProcess = null;
                IgnoredKeepInBackgroundProcesses.Clear();
            }
        }

        private void mnuViewPlanningMap_Click(object sender, EventArgs e)
        {
            if (Configuration.PlanningMapURL != string.Empty)
            {
                //mnuViewMap_Click
                ProcessStartInfo sInfo = new ProcessStartInfo(Configuration.PlanningMapURL);
                Process.Start(sInfo);
            }
        }

        private void mnuViewJumpMap_Click(object sender, EventArgs e)
        {
            if (Configuration.JumpMapURL != string.Empty)
            {
                //mnuViewMap_Click
                ProcessStartInfo sInfo = new ProcessStartInfo(Configuration.JumpMapURL);
                Process.Start(sInfo);
            }
        }

        private void mnuViewSearch_Click(object sender, EventArgs e)
        {
            if (Configuration.searchURL != string.Empty)
            {
                //mnuViewMap_Click
                ProcessStartInfo sInfo = new ProcessStartInfo(Configuration.searchURL);
                Process.Start(sInfo);
            }
        }

        private void mnuViewVeterans_Click(object sender, EventArgs e)
        {
            if (Configuration.veteranURL != string.Empty)
            {
                //mnuViewMap_Click
                ProcessStartInfo sInfo = new ProcessStartInfo(Configuration.veteranURL);
                Process.Start(sInfo);
            }
        }

        private void mnuViewLeaderBoard_Click(object sender, EventArgs e)
        {
            if (Configuration.leaderboardURL != string.Empty)
            {
                //mnuViewMap_Click
                ProcessStartInfo sInfo = new ProcessStartInfo(Configuration.leaderboardURL);
                Process.Start(sInfo);
            }
        }

        private void mnuViewSolo_Click(object sender, EventArgs e)
        {
            if (Configuration.onev1URL != string.Empty)
            {
                //mnuViewMap_Click
                ProcessStartInfo sInfo = new ProcessStartInfo(Configuration.onev1URL);
                Process.Start(sInfo);
            }
        }

        private void mnuViewFitting_Click(object sender, EventArgs e)
        {
            if (Configuration.shipFitURL != string.Empty)
            {
                //mnuViewMap_Click
                ProcessStartInfo sInfo = new ProcessStartInfo(Configuration.shipFitURL);
                Process.Start(sInfo);
            }
        }

        private void mnuDscan_Click(object sender, EventArgs e)
        {
            if (Configuration.dscanURL != string.Empty)
            {
                //mnuViewMap_Click
                ProcessStartInfo sInfo = new ProcessStartInfo(Configuration.dscanURL);
                Process.Start(sInfo);
            }
        }

        private void notifyIcon1_MouseDoubleClick(object sender, MouseEventArgs e)
        {

        }


        protected override void WndProc(ref System.Windows.Forms.Message m)
        {
  
   
            const int WM_DRAWCLIPBOARD = 0x308;
            const int WM_CHANGECBCHAIN = 0x030D;
            
            switch (m.Msg)
            {
                case WM_DRAWCLIPBOARD:
                   
                    DisplayClipboardData();
                   SendMessage(nextClipboardViewer, m.Msg, m.WParam, m.LParam);
                    break;

                case WM_CHANGECBCHAIN:
                    
                   if (m.WParam == nextClipboardViewer)
                       nextClipboardViewer = m.LParam;
                    else
                       SendMessage(nextClipboardViewer, m.Msg, m.WParam, m.LParam);
                    break;

                default:
                    base.WndProc(ref m);
                    break;
            }
        }

        void DisplayClipboardData()
        {
            try
            {
                IDataObject iData = new DataObject();
                iData = Clipboard.GetDataObject();

                string clipboardTextInput;
                string dscanJsonStart = "{\"rawdata\":\"";
                string dscanJsonEnd = "\"}";
                string overmindScanLink = "https://collab.eveovermind.com/dscan.php?replay=";
                string dscanJsonOut;
                Boolean isDscanLine = false;
                Boolean isDscanTabs = true;
                WebClient dScanClient = new WebClient();
                Encoding myDscanEncoding = System.Text.UTF8Encoding.UTF8;
                if (iData.GetDataPresent(DataFormats.Text)) { 
                     clipboardTextInput  = (string)iData.GetData(DataFormats.Text);
                    //check if it is a dscan.
                    //dscan has three columns, tab separated
                    //split clipbaord content by lines
                    string[] result = clipboardTextInput.Split(new string[] { "\n", "\r\n" }, StringSplitOptions.RemoveEmptyEntries);

                    //then for each line we want to check it has the three columns of a d-scan
                    foreach (string line in result)
                    {
                        isDscanLine = true;
                       //split by the \t delimiter    
                        string[] splitContent = line.Split('\t');
                        if (3 != splitContent.Length)
                        {
                            isDscanTabs = false;
                            break;
                        }
                    }
                    if ((isDscanLine)&&(isDscanTabs))
                    {
                        //has three columns, could well be a dscan.. lets send
                        clipboardTextInput = WebUtility.UrlEncode(clipboardTextInput);
                        dscanJsonOut = dscanJsonStart + clipboardTextInput + dscanJsonEnd;
                        appendText("D-Scan detected on clipboard, processing at server" );
                        byte[] dscanResponseBytes = dScanClient.UploadData(Configuration.dscanAPIURL, "POST", myDscanEncoding.GetBytes(dscanJsonOut));
                        string dscanResponseJson = dScanClient.Encoding.GetString(dscanResponseBytes);
                        JObject dscanResponse = JObject.Parse(dscanResponseJson);
                        string scanStatus = (string)dscanResponse["Status"];
                        string scanReplay = (string)dscanResponse["replay"];

                        if("Success" == scanStatus)
                        {
                            appendText("Dscan Successful, Replay code: " + scanReplay);
                            //output to Clipboard should be
                            overmindScanLink = overmindScanLink+scanReplay;
                            Clipboard.SetText(overmindScanLink);
                        }
                    }
                }

                else
                    //not the correct type, just return
                    return;
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());
            }
        }
    }
}
