using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.FtpClient;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace Nautilus
{
    public partial class PS3Scanner : Form
    {
        private readonly string bigDTA;
        private readonly string PS3IP;
        private readonly NemoTools Tools;
        private readonly List<string> NTSCFolders;
        private readonly List<string> PALFolders; 
        private List<string> FoldersToSearch;
        private int DTAFound;
        private int DTARead;
        private DateTime endTime;
        private DateTime startTime;
        private readonly string DTAList;
        private bool CancelProcess;

        public PS3Scanner()
        {
            InitializeComponent();
            Tools = new NemoTools();
            bigDTA = Application.StartupPath + "\\bin\\ps3dta";
            PS3IP = Application.StartupPath + "\\bin\\ps3.ip";
            DTAList = Application.StartupPath + "\\bin\\PS3_DTA_LIST.csv";
            NTSCFolders = new List<string> {"BLUS30463", "BLUS30050", "BLUS30147"};
            PALFolders = new List<string> { "BLES00986", "BLES00385", "BLES00228" };
        }

        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            EnableDisable(true);
            endTime = DateTime.Now;
            var timeDiff = endTime - startTime;
            //check if user wants to cancel at this point
            if (CancelProcess)
            {
                Log("User cancelled scanning process");
                return;
            }
            if (chkDetailed.Checked)
            {
                Log("Process took " + timeDiff.Minutes + (timeDiff.Minutes == 1 ? " minute" : " minutes") + " and " + (timeDiff.Minutes == 0 && timeDiff.Seconds == 0 ? "1 second" : timeDiff.Seconds + " seconds"));
            }
            Log(File.Exists(bigDTA) ? "Close this form to import the big DTA file into Setlist Manager" : "Try changing the timeout and/or wait times and try again");
        }

        private void EnableDisable(bool enabled)
        {
            picWorking.Visible = !enabled;
            btnFind.Enabled = enabled;
            numPort.Enabled = enabled;
            ipAddress.Enabled = enabled;
            numTimeOut.Enabled = enabled;
            numTries.Enabled = enabled;
            numWait.Enabled = enabled;
            radioNTSC.Enabled = enabled;
            radioPAL.Enabled = enabled;
            chkListing.Enabled = enabled;
            chkDetailed.Enabled = enabled;
            
        }

        private void PS3Scanner_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!picWorking.Visible) return;
            if (MessageBox.Show("The scanning process hasn't finished, if you stop it now you will lose all the work that has been done and nothing " +
                                "will be saved\nYou can do the scan again later\n\nDo you want to cancel the scanning process?", Text, MessageBoxButtons.YesNo,
                                MessageBoxIcon.Question) == DialogResult.Yes)
            {
                Log("Cancelling scanning process, please wait");
                backgroundWorker1.CancelAsync();
                CancelProcess = true;
            }
            e.Cancel = true;
        }

        private void btnFind_Click(object sender, EventArgs e)
        {
            EnableDisable(false);
            DTAFound = 0;
            DTARead = 0;
            var sw = new StreamWriter(PS3IP, false);
            sw.WriteLine("PS3_ADDRESS=" + ipAddress.Text);
            sw.WriteLine("PS3_PORT=" + numPort.Value);
            sw.WriteLine("PS3_TIMEOUT=" + numTimeOut.Value);
            sw.WriteLine("PS3_TRIES=" + numTries.Value);
            sw.WriteLine("PS3_WAIT=" + numWait.Value);
            sw.Dispose();
            Tools.DeleteFile(bigDTA);
            Tools.DeleteFile(DTAList);
            if (chkListing.Checked)
            {
                sw = new StreamWriter(DTAList, false);
                sw.WriteLine("\"Short Name\",\"DTA Path\",\"Song ID\",\"Artist\",\"Song Name\"");
                sw.Dispose();
            }
            Log("Connecting to " + ipAddress.Text);
            FoldersToSearch = radioNTSC.Checked ? NTSCFolders : PALFolders;
            startTime = DateTime.Now;
            CancelProcess = false;
            backgroundWorker1.RunWorkerAsync();
        }

        private bool ConnectToFTP(IFtpClient client, bool message = false)
        {
            client.Host = ipAddress.IPAddress.ToString();
            client.Port = (int)numPort.Value;
            client.ConnectTimeout = (int) numTimeOut.Value;
            client.DataConnectionConnectTimeout = (int) numTimeOut.Value;
            client.DataConnectionReadTimeout = (int) numTimeOut.Value;
            client.ReadTimeout = (int) numTimeOut.Value;

            try
            {
                client.GetWorkingDirectory();
                return true;
            }
            catch (Exception ex)
            {
                if (!message) return false;
                MessageBox.Show("Unable to connect - please check your connection settings\n\nThe error says:\n\"" + ex.Message + "\"",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Log("Connection error: " + ex.Message);
                return false;
            }
        }
        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            var client = new FtpClient();
            //only for testing development of code with local ftp server
            //client.Credentials = new NetworkCredential();
            //client.Credentials.UserName = "ps3";

            var sb = new StringBuilder();
            if (!ConnectToFTP(client, true))
            {
                return;
            }
            
            //check if user wants to cancel at this point
            if (backgroundWorker1.CancellationPending || CancelProcess)
            {
                client.Disconnect();
                return;
            }

            var filepath = bigDTA;

            foreach (var folder in FoldersToSearch)
            {
                if (!client.IsConnected)
                {
                    Log("Disconnected from server, trying to reconnect");
                    if (!ConnectToFTP(client))
                    {
                        Log("Failed to reconnect, stopping");
                        return;
                    }
                }
                //check if user wants to cancel at this point
                if (backgroundWorker1.CancellationPending || CancelProcess)
                {
                    client.Disconnect();
                    return;
                }

                Log("Scanning folder " + folder + " for DTA files");
                IEnumerable<string> dtafiles;
                try
                {
                    dtafiles = getSongListings(client, "/dev_hdd0/game/" + folder + "/");
                }
                catch (Exception ex)
                {
                    MessageBox.Show("There was an error trying to retrieve the songs.dta files from folder '" + folder + "'\n\nThe error says:\n\"" + ex.Message + "\"", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    Log("Scanning error: " + ex.Message);
                    continue;
                }

                if (!dtafiles.Any())
                {
                    //check if user wants to cancel at this point
                    if (backgroundWorker1.CancellationPending || CancelProcess)
                    {
                        client.Disconnect();
                        return;
                    }
                    Log("No DTA files found in that folder ... skipping");
                    continue;
                }

                if (client.IsConnected)
                {
                    Log("Found " + dtafiles.Count() + " DTA " + (dtafiles.Count() == 1 ? "file" : "files") + " in that folder");
                    Log("Starting to read DTA files");
                    DTAFound += dtafiles.Count();
                }
                else
                {
                    dtafiles = new List<string>();
                }

                foreach (var file in dtafiles)
                {
                    //check if user wants to cancel at this point
                    if (backgroundWorker1.CancellationPending || CancelProcess)
                    {
                        client.Disconnect();
                        return;
                    }

                    var name = file.Replace("/dev_hdd0/game/" + folder + "/USRDIR/", "");
                    var failed = false;
                    if (chkDetailed.Checked)
                    {
                        Log("Trying to read DTA file " + name);
                    }
                    Stream ftpStream = null;
                    for (var attempts = 0; attempts < numTries.Value; attempts++)
                    {
                        try
                        {
                            ftpStream = client.OpenRead(file);
                            failed = false;
                            break;
                        }
                        catch
                        {
                            failed = true;

                            if (!client.IsConnected)
                            {
                                Log("Disconnected from server, trying to reconnect");
                                if (!ConnectToFTP(client))
                                {
                                    Log("Failed to reconnect, stopping");
                                    return;
                                }
                            }
                        }
                        Thread.Sleep((int)numWait.Value);
                    }

                    if (failed || ftpStream == null)
                    {
                        if (chkDetailed.Checked)
                        {
                            Log("Failed to read DTA file " + name);
                        }
                        continue;
                    }

                    if (chkDetailed.Checked)
                    {
                        Log("Read DTA file " + name + " successfully");
                    }
                    DTARead++;
                    using (var reader = new StreamReader(ftpStream, Encoding.UTF8))
                    {
                        var songinfo = reader.ReadToEnd();
                        sb.Append(songinfo);
                        sb.AppendLine();

                        if (!chkListing.Checked) continue;
                        var tempDTA = Application.StartupPath + "\\bin\\temp.dta";
                        try
                        {
                            if (chkDetailed.Checked)
                            {
                                Log("Processing that DTA file and adding songs to DTA listing CSV");
                            }
                            var sw = new StreamWriter(tempDTA, false);
                            sw.Write(songinfo);
                            sw.Dispose();

                            var Parser = new DTAParser();
                            if (Parser.ReadDTA(File.ReadAllBytes(tempDTA)) && Parser.Songs.Any())
                            {
                                //by request of grubextrapolate to create a database of song/DTA location
                                sw = new StreamWriter(DTAList, true);
                                {
                                    foreach (var song in Parser.Songs)
                                    {
                                        sw.WriteLine("\"" + song.ShortName + "\",\"" + file + "\",\"" + song.SongId + "\",\"" + song.Artist.Replace("\"", "\"\"") + "\",\"" + song.Name.Replace("\"", "\"\"") + "\"");
                                    }
                                }
                                sw.Dispose();
                                if (chkDetailed.Checked)
                                {
                                    Log("Added " + Parser.Songs.Count + (Parser.Songs.Count == 1 ? " song" : " songs") +
                                        " to the DTA listing CSV");
                                }
                            }
                            Tools.DeleteFile(tempDTA);
                        }
                        catch (Exception)
                        {
                            if (chkDetailed.Checked)
                            {
                                Log("There was an error processing that DTA file ... skipping");
                            }
                            Tools.DeleteFile(tempDTA);
                        }
                    }
                }
            }

            //check if user wants to cancel at this point
            if (backgroundWorker1.CancellationPending || CancelProcess)
            {
                client.Disconnect();
                return;
            }

            if (DTARead > 0)
            {
                if (chkDetailed.Checked)
                {
                    Log("Writing all DTA files to " + Application.StartupPath + "\\bin\\" + Path.GetFileName(bigDTA));
                }
                try
                {
                    using (var outfile = new StreamWriter(filepath, false, Encoding.UTF8))
                    {
                        outfile.Write(sb.ToString());
                    }
                    if (chkDetailed.Checked)
                    {
                        Log("Wrote DTA file successfully");
                    }
                }
                catch (Exception ex)
                {
                    Log("Error: " + ex.Message);
                }
            }
            else
            {
                if (chkDetailed.Checked)
                {
                    Log("No DTA files were read ... nothing to write");
                }
            }
            client.Disconnect();
            Log("Done!");
            Log("Found " + DTAFound + " DTA " + (DTAFound == 1 ? "file" : "files"));
            Log("Successfully read " + DTARead + " DTA " + (DTARead == 1 ? "file" : "files"));
        }
        
        private IEnumerable<string> getSongListings(IFtpClient client, string path)
        {
            //check if user wants to cancel at this point
            if (backgroundWorker1.CancellationPending || CancelProcess)
            {
                yield break;
            }

            var listings = new List<FtpListItem>();
            var failed = false;

            if (client.IsConnected)
            {
                for (var attempts = 0; attempts < (int)numTries.Value; attempts++)
                {
                    try
                    {
                        client.SetWorkingDirectory(path);
                        listings = client.GetListing(path).ToList();
                        failed = false;
                        break;
                    }
                    catch
                    {
                        failed = true;
                    }
                    Thread.Sleep((int)numWait.Value);
                }
            }

            if (!client.IsConnected)
            {
                if (!ConnectToFTP(client))
                {
                    yield break;
                }
            }
            if (failed)
            {
                Log("Failed to read directory " + Path.GetDirectoryName(path));
                yield break;
            }

            if (listings.Exists(x => x.Name == "songs.dta"))
            {
                if (!client.IsConnected)
                {
                    yield break;
                }
                var item = listings.Find(x => x.Name == "songs.dta");
                yield return item.FullName;
            }
            else if (listings.Exists(x => x.Type == FtpFileSystemObjectType.Directory))
            {
                var items = listings.FindAll(x => x.Type == FtpFileSystemObjectType.Directory);
                foreach (var listing in items.TakeWhile(listing => client.IsConnected).SelectMany(item => getSongListings(client, item.FullName)))
                {
                    yield return listing;
                }
            }
        }

        private void PS3Scanner_Shown(object sender, EventArgs e)
        {
            //load last used IP address
            if (File.Exists(PS3IP))
            {
                var sr = new StreamReader(PS3IP);
                try
                {
                    Log("Found configuration file ... loading");
                    var ip = Tools.GetConfigString(sr.ReadLine());
                    var port = Tools.GetConfigString(sr.ReadLine());
                    var timeout = Tools.GetConfigString(sr.ReadLine());
                    var tries = Tools.GetConfigString(sr.ReadLine());
                    var wait =  Tools.GetConfigString(sr.ReadLine());

                    sr.Dispose();

                    if (!string.IsNullOrWhiteSpace(ip))
                    {
                        ipAddress.Text = ip;
                        try
                        {
                            numPort.Value = Convert.ToDecimal(port);
                        }
                        catch (Exception)
                        {
                            numPort.Value = 21;
                        }
                        try
                        {
                            numTimeOut.Value = Convert.ToDecimal(timeout);
                        }
                        catch (Exception)
                        {
                            numTimeOut.Value = 2000;
                        }
                        try
                        {
                            numTries.Value = Convert.ToDecimal(tries);
                        }
                        catch (Exception)
                        {
                            numTries.Value = 5;
                        }
                        try
                        {
                            numWait.Value = Convert.ToDecimal(wait);
                        }
                        catch (Exception)
                        {
                            numWait.Value = 100;
                        }
                        Log("Ready");
                        return;
                    }
                }
                catch (Exception)
                {
                    sr.Dispose();
                }
            }

            //default value
            numPort.Value = 21;

            //nothing saved, try to get it from DNS
            var localIP = "";
            var host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (var ip in host.AddressList.Where(ip => ip.AddressFamily == AddressFamily.InterNetwork))
            {
                localIP = ip.ToString();
            }
            ipAddress.Text = !string.IsNullOrWhiteSpace(localIP) ? localIP : "192.168.0.1";
            Log("Ready");
        }

        private void Log(string message)
        {
            if (lstLog.InvokeRequired)
            {
                lstLog.Invoke(new MethodInvoker(() => lstLog.Items.Add(message)));
                lstLog.Invoke(new MethodInvoker(() => lstLog.SelectedIndex = lstLog.Items.Count - 1));
            }
            else
            {
                lstLog.Items.Add(message);
                lstLog.SelectedIndex = lstLog.Items.Count - 1;
            }
            
        }

        private void PS3Scanner_Resize(object sender, EventArgs e)
        {
            if (Width < 436)
            {
                Width = 436;
            }
            if (Height < 444)
            {
                Height = 444;
            }
        }

        private void exportLogFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Tools.ExportLog(Text, lstLog.Items);
        }
    }
}
