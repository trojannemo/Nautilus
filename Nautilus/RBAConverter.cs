using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using Nautilus.Properties;
using Nautilus.x360;

namespace Nautilus
{
    public partial class RBAConverter : Form
    {
        private string[] RBAFiles;
        private string[] RBSFiles;
        private readonly NemoTools Tools;
        private readonly DTAParser Parser;
        private string basesongname;
        private string tempfolder;
        private string SongArtist;
        private string SongTitle;
        private int converted;
        private string thumbnail;
        private readonly string config;
        private bool doLIVE;
        private readonly List<string> FilesToConvert;
        private readonly string StartupFolder;

        public RBAConverter(Color ButtonBackColor, Color ButtonTextColor, string inputfolder = "")
        {
            InitializeComponent();
            Tools = new NemoTools();
            Parser = new DTAParser();
            FilesToConvert = new List<string>();
            var formButtons = new List<Button> { btnRefresh, btnFolder, btnBegin };
            foreach (var button in formButtons)
            {
                button.BackColor = ButtonBackColor;
                button.ForeColor = ButtonTextColor;
                button.FlatAppearance.MouseOverBackColor = button.BackColor == Color.Transparent ? Color.FromArgb(127, Color.AliceBlue.R, Color.AliceBlue.G, Color.AliceBlue.B) : Tools.LightenColor(button.BackColor);
            }
            config = Application.StartupPath + "\\bin\\config\\rbaconverter.config";
            StartupFolder = inputfolder;
        }

        private void LoadConfig()
        {
            if (!File.Exists(config)) return;
            var sr = new StreamReader(config);
            try
            {
                sr.ReadLine(); //skip initial line
                var folder = Tools.GetConfigString(sr.ReadLine());
                if (!string.IsNullOrWhiteSpace(folder) && Directory.Exists(folder))
                {
                    Log("Loading last used folder...");
                    txtFolder.Text = folder;
                }
                artistSongTool.Checked = sr.ReadLine().Contains("True");
                songTool.Checked = sr.ReadLine().Contains("True");
                songByArtistTool.Checked = sr.ReadLine().Contains("True");
                albumArtTool.Checked = sr.ReadLine().Contains("True");
                rB3IconTool.Checked = sr.ReadLine().Contains("True");
                sr.Dispose();
            }
            catch (Exception)
            {
                sr.Dispose();
                Tools.DeleteFile(config);
            }
        }

        private void SaveConfig()
        {
            var sw = new StreamWriter(config, false);
            sw.WriteLine("//Do not modify this file manually");
            sw.WriteLine("LastFolder=" + txtFolder.Text);
            sw.WriteLine("ArtistSong=" + artistSongTool.Checked);
            sw.WriteLine("Song=" + songTool.Checked);
            sw.WriteLine("SongByArtist=" + songByArtistTool.Checked);
            sw.WriteLine("UseAlbumArt=" + albumArtTool.Checked);
            sw.WriteLine("UseRB3Icon=" + rB3IconTool.Checked);
            sw.Dispose();
        }

        private void btnFolder_Click(object sender, EventArgs e)
        {
            //if user selects new folder, assign that value
            //if user cancels or selects same folder, this forces the text_changed event to run again
            var tFolder = txtFolder.Text;
            var ofd = new FolderPicker
            {
                InputPath = tFolder,
                Title = "Select folder where your source files are",
            };
            if (ofd.ShowDialog(IntPtr.Zero) == true)
            {
                txtFolder.Text = ofd.ResultPath;
                Tools.CurrentFolder = txtFolder.Text;
            }
            else
            {
                txtFolder.Text = tFolder;
            }
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
        
        public bool ConvertRBAtoCON(string rba)
        {
            if (backgroundWorker1.CancellationPending) return false;
            if (!File.Exists(rba))
            {
                Log("Can't find file " + rba);
                Log("Skipping it...");
                return false;
            }
            var success = ExtractRBA(rba);
            if (success)
            {
                success = makeCON(rba);
            }
            Tools.DeleteFolder(tempfolder, true);
            return success;
        }
        
        private bool ExtractRBA(string rba)
        {
            if (backgroundWorker1.CancellationPending) return false;
            thumbnail = "";
            basesongname = Path.GetFileNameWithoutExtension(rba).Replace(" ", "").Trim();
            if (basesongname.Length > 26)
            {
                basesongname = basesongname.Substring(0, 26);
            }
            try
            {
                using (var bReadRba = new BinaryReader(File.Open(rba, FileMode.Open)))
                {
                    var signature = bReadRba.ReadChars(4);
                    if ((signature[0] != 'R') ||
                        (signature[1] != 'B') ||
                        (signature[2] != 'S') ||
                        (signature[3] != 'F'))
                    {
                        Log("Unknown file format, can't convert file " + Path.GetFileName(rba));
                        return false;
                    }
                    var rba_header_values = new int[(int)RBA_HEADER_INDEX.HEADER_INDEX_COUNT];
                    for (var i = 0; i < (int)RBA_HEADER_INDEX.HEADER_INDEX_COUNT; i++)
                    {
                        var v = bReadRba.ReadInt32();
                        rba_header_values[i] = v;
                    }
                    if (rba_header_values[(int)RBA_HEADER_INDEX.OFFSET_SONGS_DTA] != 0)
                    {
                        var offset = rba_header_values[(int)RBA_HEADER_INDEX.OFFSET_SONGS_DTA];
                        var len = rba_header_values[(int)RBA_HEADER_INDEX.LENGTH_SONGS_DTA];
                        bReadRba.BaseStream.Seek(offset, SeekOrigin.Begin);
                        var data = bReadRba.ReadBytes(len);
                        var fname = tempfolder + "songs.dta.raw";
                        using (var bWrite = new BinaryWriter(File.Open(fname, FileMode.Create)))
                        {
                            bWrite.Write(data);
                            bWrite.Dispose();
                        }
                        RBAPatchSongsDta(Path.GetExtension(rba).ToLowerInvariant() == ".rbs");
                    }
                    if (rba_header_values[(int)RBA_HEADER_INDEX.OFFSET_MID] != 0)
                    {
                        var offset = rba_header_values[(int)RBA_HEADER_INDEX.OFFSET_MID];
                        var len = rba_header_values[(int)RBA_HEADER_INDEX.LENGTH_MID];
                        bReadRba.BaseStream.Seek(offset, SeekOrigin.Begin);
                        var data = bReadRba.ReadBytes(len);
                        var fname = tempfolder + basesongname + ".mid";
                        using (var bWrite = new BinaryWriter(File.Open(fname, FileMode.Create)))
                        {
                            bWrite.Write(data);
                            bWrite.Dispose();
                        }
                    }
                    if (rba_header_values[(int)RBA_HEADER_INDEX.OFFSET_MOGG] != 0)
                    {
                        var offset = rba_header_values[(int)RBA_HEADER_INDEX.OFFSET_MOGG];
                        var len = rba_header_values[(int)RBA_HEADER_INDEX.LENGTH_MOGG];
                        bReadRba.BaseStream.Seek(offset, SeekOrigin.Begin);
                        var data = bReadRba.ReadBytes(len);
                        var fname = tempfolder + basesongname + ".mogg";
                        using (var bWrite = new BinaryWriter(File.Open(fname, FileMode.Create)))
                        {
                            bWrite.Write(data);
                            bWrite.Dispose();
                        }
                    }
                    if (rba_header_values[(int)RBA_HEADER_INDEX.OFFSET_MILO_XBOX] != 0)
                    {
                        var offset = rba_header_values[(int)RBA_HEADER_INDEX.OFFSET_MILO_XBOX];
                        var len = rba_header_values[(int)RBA_HEADER_INDEX.LENGTH_MILO_XBOX];
                        bReadRba.BaseStream.Seek(offset, SeekOrigin.Begin);
                        var data = bReadRba.ReadBytes(len);
                        var fname = tempfolder + basesongname + ".milo_xbox";
                        using (var bWrite = new BinaryWriter(File.Open(fname, FileMode.Create)))
                        {
                            bWrite.Write(data);
                            bWrite.Dispose();
                        }
                    }
                    if (rba_header_values[(int)RBA_HEADER_INDEX.OFFSET_BMP] != 0)
                    {
                        var offset = rba_header_values[(int)RBA_HEADER_INDEX.OFFSET_BMP];
                        var len = rba_header_values[(int)RBA_HEADER_INDEX.LENGTH_BMP];
                        bReadRba.BaseStream.Seek(offset, SeekOrigin.Begin);
                        var data = bReadRba.ReadBytes(len);
                        var fname = tempfolder + basesongname + (Path.GetExtension(rba).ToLowerInvariant() == ".rbs" ? "_keep.png_xbox" : ".bmp");
                        using (var bWrite = new BinaryWriter(File.Open(fname, FileMode.Create)))
                        {
                            bWrite.Write(data);
                            bWrite.Dispose();
                        }
                        if (Path.GetExtension(rba).ToLowerInvariant() == ".rba")
                        {
                            var xboxfile = fname.Replace(".bmp", "_keep.png_xbox");
                            Tools.DeleteFile(xboxfile);

                            Tools.TextureSize = 256;
                            Tools.ConvertImagetoRB(fname, xboxfile);

                            thumbnail = fname.Replace(".bmp", ".png");
                            Tools.DeleteFile(thumbnail);
                            Tools.ResizeImage(fname, 64, "png", thumbnail);
                            if (!File.Exists(thumbnail))
                            {
                                thumbnail = "";
                            }
                        }
                    }
                    if (rba_header_values[(int)RBA_HEADER_INDEX.OFFSET_WEIGHTS] != 0)
                    {
                        var offset = rba_header_values[(int)RBA_HEADER_INDEX.OFFSET_WEIGHTS];
                        var len = rba_header_values[(int)RBA_HEADER_INDEX.LENGTH_WEIGHTS];
                        bReadRba.BaseStream.Seek(offset, SeekOrigin.Begin);
                        var data = bReadRba.ReadBytes(len);
                        var fname = tempfolder + basesongname + ".bin";
                        using (var bWrite = new BinaryWriter(File.Open(fname, FileMode.Create)))
                        {
                            bWrite.Write(data);
                            bWrite.Close();
                        }
                    }
                    bReadRba.Dispose();
                    return true;
                }
            }
            catch (Exception e)
            {
                Log("Error extracting RBA file " + Path.GetFileName(rba));
                Log(e.ToString());
                return false;
            }
        }

        private void RBAPatchSongsDta(bool isRBS)
        {
            var fsongsraw = tempfolder + "songs.dta.raw";
            var fsongsdta = tempfolder + "songs.dta";
            try
            {
                // Create an instance of StreamReader to read from a file.
                using (var srSongsRaw = new StreamReader(fsongsraw, Encoding.Default))
                {
                    var swSongsDta = new StreamWriter(fsongsdta, false, Encoding.Default);
                    using (swSongsDta)
                    {
                        // copy the first line
                        var line = srSongsRaw.ReadLine();
                        swSongsDta.WriteLine(line);
                        // patch 'song' in the second line to be '<basename>'
                        line = srSongsRaw.ReadLine();
                        if (line != null && line.Contains("'song'"))
                        {
                            line = "   '" + basesongname + "'";
                        }
                        swSongsDta.WriteLine(line);
                        // Read and display lines from the file until the end of
                        // the file is reached.
                        while ((line = srSongsRaw.ReadLine()) != null)
                        {
                            if (line.Contains("'name'"))
                            {
                                swSongsDta.WriteLine(line);
                                line = srSongsRaw.ReadLine();
                                if (line != null && !line.Contains("songs/"))
                                {
                                    SongTitle = Parser.GetSongName(line);
                                }
                                // -          "songs/song/song" => "songs/<basename>/<basename"
                                if (line != null && line.Contains("songs/"))
                                {
                                    if (isRBS)
                                    {
                                        basesongname = Parser.GetInternalName(line);
                                    }
                                    else
                                    {
                                        line = "         \"songs/" + basesongname + "/" + basesongname + "\"";
                                    }
                                }
                            }
                            else if (line.Contains("songs/") && line.Contains(".mid"))
                            {
                                line = "         \"songs/" + basesongname + "/" + basesongname + ".mid";
                            }
                            else if (line.Contains("'artist'"))
                            {
                                swSongsDta.WriteLine(line);
                                line = srSongsRaw.ReadLine();
                                SongArtist = Parser.GetArtistName(line);
                            }
                            // -    ('song_id' 0) => ('song_id' <basename>)
                            else if (line.Contains("('song_id' 0)"))
                            {
                                line = "   ('song_id' " + basesongname + ")";
                            }
                            else if (line.Contains("('real_bass' 0)") || line.Contains("('real_guitar' 0)"))
                            {
                                // Swallow this line - don't put it in the final songs.dta
                                // because it prevents you from making a separate upgrades.dta
                                //only if there is no pro bass in the song!
                                line = "";
                            }
                            if (line != "")
                            {
                                swSongsDta.WriteLine(line);
                            }
                            if (srSongsRaw.EndOfStream)
                            {
                                break;
                            }
                        }
                    }
                    swSongsDta.Dispose();
                    srSongsRaw.Dispose();
                }
            }
            catch (Exception ex)
            {
                Log("There was an error:");
                Log(ex.Message);
            }
        }

        private bool makeCON(string rba)
        {
            if (backgroundWorker1.CancellationPending) return false;
            var con = txtFolder.Text + "\\" + Path.GetFileNameWithoutExtension(rba).Replace("_rb3con", "") + "_rb3con";
            Tools.DeleteFile(con);
            var xsession = new CreateSTFS { HeaderData = { TitleID = 0x45410914 } };
            xsession.HeaderData.Title_Package = "Rock Band 3";
            xsession.HeaderData.SetLanguage(Languages.English);
            xsession.HeaderData.Publisher = "";
            xsession.STFSType = STFSType.Type0;
            xsession.HeaderData.ThisType = PackageType.SavedGame;
            xsession.HeaderData.PackageImageBinary = rB3IconTool.Checked || string.IsNullOrWhiteSpace(thumbnail) || !File.Exists(thumbnail) ? 
               Resources.RB3.ImageToBytes(ImageFormat.Png) : Tools.NemoLoadImage(thumbnail).ImageToBytes(ImageFormat.Png);
            xsession.HeaderData.ContentImageBinary = Resources.RB3.ImageToBytes(ImageFormat.Png);
            xsession.HeaderData.MakeAnonymous();
            var bOk = PackageCheckFiles(xsession);
            if (bOk)
            {
                bOk = PackageCreate(con, xsession);
            }
            if (bOk)
            {
                bOk = Tools.UnlockCON(con);
            }
            if (!bOk) return false;
            Tools.SignCON(con);
            return true;
        }

        private bool PackageCreate(string con, CreateSTFS xsession)
        {
            var signature = new RSAParams(Application.StartupPath + "\\bin\\KV.bin");
            try
            {
                xsession.HeaderData.Title_Display = artistSongTool.Checked ? (SongArtist + " - " + SongTitle) : ("\"" + SongTitle + "\"" + (songByArtistTool.Checked ? " by " + SongArtist : ""));
                xsession.HeaderData.Description = "Converted to CON using Nautilus";
                var xy = new STFSPackage(xsession, signature, con);
                xy.CloseIO();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        private void txtFolder_TextChanged(object sender, EventArgs e)
        {
            if (picWorking.Visible) return;
            if (string.IsNullOrWhiteSpace(txtFolder.Text))
            {
                btnRefresh.Visible = false;
            }
            btnRefresh.Visible = true;
            if (txtFolder.Text != "")
            {
                Log("Reading input directory ... hang on");
                try
                {
                    RBAFiles = Directory.GetFiles(txtFolder.Text, "*.rba");
                    RBSFiles = Directory.GetFiles(txtFolder.Text, "*.rbs");

                    if (!RBAFiles.Any() && !RBSFiles.Any())
                    {
                        Log("Did not find any RBA files ... try a different directory");
                        Log("You can also drag and drop RBA files here");
                        Log("Ready");
                        btnBegin.Visible = false;
                    }
                    else
                    {
                        Log("Found " + (RBAFiles.Count() + RBSFiles.Count()) + " RBA " + (RBAFiles.Count() + RBSFiles.Count() == 1 ? "file" : "files"));
                        Log("Ready to begin");
                        btnBegin.Visible = true;
                    }

                    btnRefresh.Visible = true;
                    tempfolder = txtFolder.Text + "\\temp\\";
                }
                catch (Exception ex)
                {
                    Log("There was an error: " + ex.Message);
                }
            }
            else
            {
                btnRefresh.Visible = false;
                btnBegin.Visible = false;
            }
        }

        private bool PackageCheckFiles(CreateSTFS xsession)
        {
            var fnamedta = tempfolder + "songs.dta";
            if (File.Exists(fnamedta))
            {
                xsession.AddFolder("songs");
                xsession.AddFolder("songs/" + basesongname);
                xsession.AddFolder("songs/" + basesongname + "/gen");
                if (!xsession.AddFile(fnamedta, "songs/songs.dta"))
                {
                    Log("ERROR: Could not add " + fnamedta + " to CON");
                    return  false;
                }
            }
            else
            {
                Log("FAIL: " + fnamedta + " is not present");
                return  false;
            }
            var files = Directory.GetFiles(tempfolder, "*.mid");
            if (files.Count() != 0)
            {
                if (!xsession.AddFile(files[0], "songs/" + basesongname + "/" + basesongname + ".mid"))
                {
                    Log("ERROR: Could not add MIDI file to CON");
                    return false;
                }
            }
            files = Directory.GetFiles(tempfolder, "*.mogg");
            if (files.Count() != 0)
            {
                if (!xsession.AddFile(files[0], "songs/" + basesongname + "/" + basesongname + ".mogg"))
                {
                    Log("ERROR: Could not add mogg file to CON");
                    return false;
                }
            }
            files = Directory.GetFiles(tempfolder, "*.milo_xbox");
            if (files.Count() != 0)
            {
                if (!xsession.AddFile(files[0], "songs/" + basesongname + "/gen/" + basesongname + ".milo_xbox"))
                {
                    Log("ERROR: Could not add milo file to CON");
                    return false;
                }
            }
            files = Directory.GetFiles(tempfolder, "*.png_xbox");
            if (files.Count() != 0)
            {
                if (!xsession.AddFile(files[0], "songs/" + basesongname + "/gen/" + basesongname + "_keep.png_xbox"))
                {
                    Log("ERROR: Could not add album art to CON");
                    return false;
                }
            }
            files = Directory.GetFiles(tempfolder, "*.bin");
            if (!files.Any()) return true;
            if (xsession.AddFile(files[0], "songs/" + basesongname + "/gen/" + basesongname + "_weights.bin"))
                return true;
            Log("ERROR: Could not add weights file to CON");
            return false;
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            var tFolder = txtFolder.Text;
            txtFolder.Text = "";
            txtFolder.Text = tFolder;
        }
        
        private void HandleDragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
                e.Effect = DragDropEffects.All;
        }

        private void HandleDragDrop(object sender, DragEventArgs e)
        {
            if (picWorking.Visible) return;
            var files = (string[])e.Data.GetData(DataFormats.FileDrop);
            if (VariousFunctions.ReadFileType(files[0]) == XboxFileType.STFS)
            {
                var xFile = new STFSPackage(files[0]);
                if (xFile.ParseSuccess)
                {
                    doLIVE = xFile.Header.ThisType != PackageType.MarketPlace;
                    xFile.CloseIO();
                    SelectFilesToConvert(Path.GetDirectoryName(files[0]));
                    return;
                }
            }
            txtFolder.Text = Path.GetDirectoryName(files[0]);
            Tools.CurrentFolder = txtFolder.Text;
        }
        
        private void exportLogFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Tools.ExportLog(Text, lstLog.Items);
        }

        private void RBAConverter_Shown(object sender, EventArgs e)
        {
            Log("Welcome to " + Text);
            Log("Drag and drop the RBA, CON or LIVE files here");
            Log("Click 'Change Input Folder' to select folder with RBA files");
            Log("Click 'LIVE <-> CON' menu to batch convert LIVE and CON files");
            Log("Ready");
            LoadConfig();
            if (string.IsNullOrWhiteSpace(StartupFolder)) return;
            txtFolder.Text = StartupFolder;
        }

        enum RBA_HEADER_INDEX
        {
            HEADER_VALUE_UNKNOWN,
            OFFSET_SONGS_DTA,
            OFFSET_MID,
            OFFSET_MOGG,
            OFFSET_MILO_XBOX,
            OFFSET_BMP,
            OFFSET_WEIGHTS,
            OFFSET_BACKEND,
            LENGTH_SONGS_DTA,
            LENGTH_MID,
            LENGTH_MOGG,
            LENGTH_MILO_XBOX,
            LENGTH_BMP,
            LENGTH_WEIGHTS,
            LENGTH_BACKEND,
            HEADER_INDEX_COUNT
        };

        private void EnableDisable(bool enabled)
        {
            picWorking.Visible = !enabled;
            lstLog.Cursor = enabled ? Cursors.Default : Cursors.WaitCursor;
            Cursor = lstLog.Cursor;
            btnFolder.Enabled = enabled;
            btnRefresh.Enabled = enabled;
            txtFolder.Enabled = enabled;
        }

        private void btnBegin_Click(object sender, EventArgs e)
        {
            if (btnBegin.Text == "Cancel")
            {
                Log("User cancelled process...stopping as soon as possible");
                if (backgroundWorker1.IsBusy)
                {
                    backgroundWorker1.CancelAsync();
                }
                else
                {
                    backgroundWorker2.CancelAsync();
                }
                btnBegin.Enabled = false;
                return;
            }
            btnBegin.Text = "Cancel";
            toolTip1.SetToolTip(btnBegin, "Click to cancel conversion process");
            EnableDisable(false);
            backgroundWorker1.RunWorkerAsync();
        }

        private void backgroundWorker1_DoWork(object sender, System.ComponentModel.DoWorkEventArgs e)
        {
            foreach (var rba in RBAFiles.Where(File.Exists))
            {
                if (backgroundWorker1.CancellationPending) return;
                Log("Converting file '" + Path.GetFileName(rba) + "' to CON");
                basesongname = "";
                SongArtist = "";
                SongTitle = "";
                Tools.DeleteFolder(tempfolder, true);
                Directory.CreateDirectory(tempfolder);
                if (ConvertRBAtoCON(rba))
                {
                    Log("Converted file '" + Path.GetFileName(rba) + "' to CON successfully");
                    converted++;
                }
                else
                {
                    Log("Failed to convert '" + Path.GetFileName(rba) + "' to CON");
                }
            }
            foreach (var rbs in RBSFiles.Where(File.Exists))
            {
                if (backgroundWorker1.CancellationPending) return;
                Log("Converting file '" + Path.GetFileName(rbs) + "' to CON");
                basesongname = "";
                SongArtist = "";
                SongTitle = "";
                Tools.DeleteFolder(tempfolder, true);
                Directory.CreateDirectory(tempfolder);
                if (ConvertRBAtoCON(rbs))
                {
                    Log("Converted file '" + Path.GetFileName(rbs) + "' to CON successfully");
                    converted++;
                }
                else
                {
                    Log("Failed to convert '" + Path.GetFileName(rbs) + "' to CON");
                }
            }
        }

        private void backgroundWorker1_RunWorkerCompleted(object sender, System.ComponentModel.RunWorkerCompletedEventArgs e)
        {
            EnableDisable(true);
            btnBegin.Text = "&Begin";
            toolTip1.SetToolTip(btnBegin, "Click to begin");
            btnBegin.Enabled = true;
            Tools.DeleteFolder(tempfolder, true);
            if (converted == 0)
            {
                Log("No files were converted");
            }
            else
            {
                Log("Converted " + converted + " RBA " + (converted == 1 ? "file" : "files") + " successfully");
            }
            Log("Ready");
            Tools.DeleteFolder(tempfolder, true);
        }

        private void RBAConverter_FormClosing(object sender, FormClosingEventArgs e)
        {
            Tools.DeleteFolder(tempfolder, true);
            if (!picWorking.Visible)
            {
                SaveConfig();
                return;
            }
            MessageBox.Show("Please wait until the current process finishes", Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            e.Cancel = true;
        }

        private void helpToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var message = Tools.ReadHelpFile("rba");
            var help = new HelpForm(Text + " - Help", message);
            help.ShowDialog();
        }

        private void albumArtTool_Click(object sender, EventArgs e)
        {
            albumArtTool.Checked = true;
            rB3IconTool.Checked = false;
        }

        private void rB3IconTool_Click(object sender, EventArgs e)
        {
            albumArtTool.Checked = false;
            rB3IconTool.Checked = true;
        }

        private void artistSongTool_Click(object sender, EventArgs e)
        {
            artistSongTool.Checked = true;
            songTool.Checked = false;
            songByArtistTool.Checked = false;
        }

        private void songTool_Click(object sender, EventArgs e)
        {
            artistSongTool.Checked = false;
            songTool.Checked = true;
            songByArtistTool.Checked = false;
        }

        private void songByArtistTool_Click(object sender, EventArgs e)
        {
            artistSongTool.Checked = false;
            songTool.Checked = false;
            songByArtistTool.Checked = true;
        }

        private void LIVEtoCON_Click(object sender, EventArgs e)
        {
            doLIVE = false;
            SelectFilesToConvert();
        }

        private void SelectFilesToConvert(string folder = "")
        {
            FilesToConvert.Clear();
            var path = folder;
            if (string.IsNullOrWhiteSpace(folder))
            {                
                var ofd = new FolderPicker
                {
                    InputPath = Tools.CurrentFolder,
                    Title = "Select folder containing files",
                };
                if (ofd.ShowDialog(IntPtr.Zero) != true) return;
                path = ofd.ResultPath;
            }
            Tools.CurrentFolder = path;
            var files = Directory.GetFiles(path);
            if (!files.Any())
            {
                Log("No files found in that folder, try another");
                return;
            }
            foreach (var file in files.Where(file => VariousFunctions.ReadFileType(file) == XboxFileType.STFS))
            {
                FilesToConvert.Add(file);
            }
            if (!FilesToConvert.Any())
            {
                Log("No STFS files found in that folder, try another");
                return;
            }
            Log("Received " + (doLIVE ? "CON" : "LIVE") + " file, searching for STFS files to convert to " + (doLIVE ? "LIVE" : "CON"));
            Log("Found " + FilesToConvert.Count + " STFS " + (FilesToConvert.Count == 1 ? "file" : "files, converting to " + (doLIVE ? "LIVE" : "CON...")));
            EnableDisable(false);
            btnBegin.Enabled = true;
            btnBegin.Visible = true;
            btnBegin.Text = "Cancel";
            toolTip1.SetToolTip(btnBegin, "Click to cancel conversion process");
            backgroundWorker2.RunWorkerAsync();
        }

        private void CONtoLIVE_Click(object sender, EventArgs e)
        {
            doLIVE = true;
            SelectFilesToConvert();
        }

        private void backgroundWorker2_DoWork(object sender, System.ComponentModel.DoWorkEventArgs e)
        {
            var counter = 0;
            foreach (var file in FilesToConvert.TakeWhile(file => !backgroundWorker2.CancellationPending))
            {
                Log("Converting STFS file '" + Path.GetFileName(file) + "'");
                try
                {
                    var xFile = new STFSPackage(file);
                    if (!xFile.ParseSuccess)
                    {
                        Log("Couldn't parse that file, skipping...");
                        continue;
                    }
                    xFile.Header.MakeAnonymous();
                    xFile.Header.ThisType = doLIVE ? PackageType.MarketPlace : PackageType.SavedGame;
                    var signature = doLIVE ? new RSAParams(StrongSigned.LIVE) : new RSAParams(Application.StartupPath + "\\bin\\KV.bin");
                    xFile.RebuildPackage(signature);
                    xFile.FlushPackage(signature);
                    xFile.CloseIO();
                    Tools.UnlockCON(file);
                    if (!doLIVE)
                    {
                        Tools.SignCON(file);
                    }
                    Log("Successfully converted '" + Path.GetFileName(file) + "' to " + (doLIVE ? "LIVE" : "CON..."));
                    counter++;
                    xFile.CloseIO();
                }
                catch (Exception ex)
                {
                    Log("There was an error converting that file");
                    Log("The error says: " + ex.Message);
                }
            }
            Log("Finished converting " + (counter == 1 ? "file" : "files"));
            Log("Ready");
        }

        private void backgroundWorker2_RunWorkerCompleted(object sender, System.ComponentModel.RunWorkerCompletedEventArgs e)
        {
            EnableDisable(true);
            btnBegin.Text = "&Begin";
            toolTip1.SetToolTip(btnBegin, "Click to begin");
            btnBegin.Enabled = false;
            btnBegin.Visible = false;
        }

        private void picPin_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Left) return;
            switch (picPin.Tag.ToString())
            {
                case "pinned":
                    picPin.Image = Resources.unpinned;
                    picPin.Tag = "unpinned";
                    break;
                case "unpinned":
                    picPin.Image = Resources.pinned;
                    picPin.Tag = "pinned";
                    break;
            }
            TopMost = picPin.Tag.ToString() == "pinned";
        }
    }
}
