using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Nautilus.Properties;
using Nautilus.x360;

namespace Nautilus
{
    public partial class QuickPackEditor : Form
    {
        private RSAParams signature;
        private readonly MainForm xMainForm;
        private readonly NemoTools Tools;
        private readonly DTAParser Parser;
        private string pack;
        private string backup;
        public List<SongData> Songs;
        private readonly List<int> removed;
        private STFSPackage xPackage;
        private CreateSTFS repackaged = new CreateSTFS();
        private string DTAPath;
        private string incoming_pack;
        private readonly List<string> PackagesToUnpack;
        private string songToDePack;
        public string fileToExtract;
        private bool isPKG;
        private bool isDTA;
        
        public QuickPackEditor(MainForm xParent, Color ButtonBackColor, Color ButtonTextColor, string dta = "", string inpack = "")
        {
            InitializeComponent();

            xMainForm = xParent;
            Tools = new NemoTools();
            Parser = new DTAParser();
            Songs = new List<SongData>();
            removed = new List<int>();
            PackagesToUnpack = new List<string>();
            incoming_pack = inpack;

            ContentImage.AllowDrop = true;
            PackageImage.AllowDrop = true;

            var formButtons = new List<Button> { btnReset,btnRemove,btnRestore,btnClear,btnDePack,btnRePack, btnSelect, btnExtract };
            foreach (var button in formButtons)
            {
                button.BackColor = ButtonBackColor;
                button.ForeColor = ButtonTextColor;
                button.FlatAppearance.MouseOverBackColor = button.BackColor == Color.Transparent ? Color.FromArgb(127, Color.AliceBlue.R, Color.AliceBlue.G, Color.AliceBlue.B) : Tools.LightenColor(button.BackColor);
            }

            Log("Welcome to the Quick Pack Editor");
            Log("Drag a file to this form or use the 'Open file' button");
            Log("Ready to begin");
            DTAPath = dta;
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

        private void HandleDragDrop(object sender, DragEventArgs e)
        {
            if (picWorking.Visible) return;
            var files = (string[])e.Data.GetData(DataFormats.FileDrop);
            if (!menuStrip1.Enabled)
            {
                incoming_pack = files[0];
                btnReset.PerformClick();
            }
            PackagesToUnpack.Clear();
            ValidateFile(files[0]);
        }

        private void ValidateFile(string file)
        {
            isDTA = false;
            try
            {
                if (Path.GetExtension(file) == ".dta")
                {
                    isDTA = true;
                    DTAPath = file;
                    DoDTAOnly();
                }
                else if (VariousFunctions.ReadFileType(file) == XboxFileType.STFS)
                {                    
                    pack = file;
                    ReadContents();
                }
                else if (Path.GetExtension(file) == ".pkg")
                {
                    ExtractPKG(file);
                }
                else
                {
                    Log("That's not a valid file to drop here, only CON / LIVE / DTA files are allowed");
                }
            }
            catch (Exception ex)
            {
                Log("There was a problem accessing that file");
                Log("The error says: " + ex.Message);
                try
                {
                    xPackage.CloseIO(); //release in case in case
                }
                catch (Exception)
                {}
            }
        }

        public void ReadContents(string file = "")
        {
            if (file != "")
            {
                pack = file;
            }
            var tempfolder = Application.StartupPath + "\\quickpackeditor\\";
            if (!Directory.Exists(tempfolder))
            {
                Directory.CreateDirectory(tempfolder);
            }
            Tools.CurrentFolder = Path.GetDirectoryName(pack);
            Log("Reading CON file " + Path.GetFileName(pack));
            Songs.Clear();

            xPackage = new STFSPackage(pack);
            if (!xPackage.ParseSuccess)
            {
                xPackage.CloseIO();
                Log("Error opening " + Path.GetFileName(pack));
                Log("Ready");
                return;
            }

            if (picWorking.Visible) //background worker running, need to invoke
            {
                PackageImage.Invoke(new MethodInvoker(() => PackageImage.Image = xPackage.Header.PackageImage));
                ContentImage.Invoke(new MethodInvoker(() => ContentImage.Image = xPackage.Header.ContentImage));
                txtDescription.Invoke(new MethodInvoker(() => txtDescription.Text = Tools.FixBadChars(xPackage.Header.Description)));
                txtTitle.Invoke(new MethodInvoker(() => txtTitle.Text = Tools.FixBadChars(xPackage.Header.Title_Display)));
            }
            else
            {
                PackageImage.Image = xPackage.Header.PackageImage;
                ContentImage.Image = xPackage.Header.ContentImage;
                txtDescription.Text = Tools.FixBadChars(xPackage.Header.Description);
                txtTitle.Text = Tools.FixBadChars(xPackage.Header.Title_Display);
            }

            rockBandToolStripMenuItem.Checked = false;
            rockBand2ToolStripMenuItem.Checked = false;
            rockBand3ToolStripMenuItem.Checked = false;

            switch (xPackage.Header.TitleID)
            {
                case 0x45410829:
                    rockBandToolStripMenuItem.Checked = true;
                    break;
                case 0x45410869:
                    rockBand2ToolStripMenuItem.Checked = true;
                    break;
                case 0x45410914:
                    rockBand3ToolStripMenuItem.Checked = true;
                    break;
                default:
                    rockBand3ToolStripMenuItem.Checked = true;
                    break;
            }

            var xFile = xPackage.GetFile("/songs/songs.dta");
            if (xFile == null)
            {
                xPackage.CloseIO();
                Log("Could not find a songs.dta file inside that pack");
                Log("Ready");
                return;
            }

            Log("Found songs.dta file, extracting temporary DTA file");
            var xDTA = xFile.Extract();
            xPackage.CloseIO();
            if (xDTA == null || xDTA.Length == 0)
            {
                Log("Error extracting temporary DTA file");
                Log("Ready");
                return;
            }
            ReadDTA(xDTA);
            if (!picWorking.Visible)
            {
                btnRePack.Enabled = PackagesToUnpack.Count <= 1;
                btnDePack.Enabled = lstSongs.Items.Count > 1 && PackagesToUnpack.Count <= 1;
            }
            Log("Ready");
        }

        public void ReadDTA(byte[] xDTA, bool onlyRead = false)
        {
            Log("Reading DTA contents");
            if (Parser.ReadDTA(xDTA))
            {
                Songs = Parser.Songs;
            }
            if (onlyRead) return;
            if (picWorking.Visible) //background worker running, need to invoke
            {
                lstSongs.Invoke(new MethodInvoker(() => lstSongs.Items.Clear()));
            }
            else
            {
                lstSongs.Items.Clear();
            }
            PopulateEntries();
            Log("Found " + Songs.Count + " song " + (Songs.Count == 1 ? "entry" : "entries"));
            if (Songs.Count > 0)
            {
                if (picWorking.Visible) //background worker running, need to invoke
                {
                    btnDePack.Invoke(new MethodInvoker(() => btnDePack.Visible = true));
                    btnDePack.Invoke(new MethodInvoker(() => btnDePack.Enabled = true));
                }
                else
                {
                    btnDePack.Visible = true;
                    btnDePack.Enabled = true;
                }
                
            }
        }

        private void PopulateEntries(int sort_order = 1)
        {
            //0 by dta entry
            //1 by artist - song
            //2 by song - artist
            if (picWorking.Visible) //background worker running, need to invoke
            {
                lstSongs.Invoke(new MethodInvoker(() => lstSongs.Items.Clear()));
                btnSelect.Invoke(new MethodInvoker(() => btnSelect.Enabled = false));
                lstSongs.Invoke(new MethodInvoker(() => lstSongs.Sorted = sort_order != 0));
            }
            else
            {
                lstSongs.Items.Clear();
                btnSelect.Enabled = false;
                lstSongs.Sorted = sort_order != 0;
            }
            var entries = 0;
            foreach (var entry in Songs)
            {
                var id = entry.SongId == 0 || entry.SongId == 99999999 ? entry.ShortName : entry.SongId.ToString(CultureInfo.InvariantCulture);
                entries++;
                switch (sort_order)
                {
                    case 0:
                        if (picWorking.Visible) //background worker running, need to invoke
                        {
                            var entry1 = entry;
                            var entries1 = entries;
                            lstSongs.Invoke(new MethodInvoker(() => lstSongs.Items.Add(entry1.Artist + " - " + entry1.Name + " (" + id + ")" + " [#" + entries1 + "]")));
                        }
                        else
                        {
                            lstSongs.Items.Add(entry.Artist + " - " + entry.Name + " (" + id + ")" + " [#" + entries + "]");
                        }
                        break;
                    case 1:
                        if (picWorking.Visible) //background worker running, need to invoke
                        {
                            var entry1 = entry;
                            var entries1 = entries;
                            lstSongs.Invoke(new MethodInvoker(() =>lstSongs.Items.Add(entry1.Artist + " - " + entry1.Name + " (" + id + ")" + " [#" + entries1 +"]")));
                        }
                        else
                        {
                            lstSongs.Items.Add(entry.Artist + " - " + entry.Name + " (" + id + ")" + " [#" + entries + "]");
                        }
                        break;
                    case 2:
                        if (picWorking.Visible) //background worker running, need to invoke
                        {
                            var entry1 = entry;
                            var entries1 = entries;
                            lstSongs.Invoke(new MethodInvoker(() => lstSongs.Items.Add(entry1.Name + " - " + entry1.Artist + " (" + id + ")" + " [#" + entries1 + "]")));
                        }
                        else
                        {
                            lstSongs.Items.Add(entry.Name + " - " + entry.Artist + " (" + id + ")" + " [#" + entries + "]");
                        }
                        break;
                }
            }
            var count = 0;
            if (picWorking.Visible) //background worker running, need to invoke
            {
                lstSongs.Invoke(new MethodInvoker(() => count = lstSongs.Items.Count));
            }
            else
            {
                count = lstSongs.Items.Count;
            }
            if (count <= 0 || PackagesToUnpack.Count > 1) return;
            if (picWorking.Visible) //background worker running, need to invoke
            {
                btnSelect.Invoke(new MethodInvoker(() => btnSelect.Enabled = true));
            }
            else
            {
                btnSelect.Enabled = true;
            }
        }

        private void HandleDragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
                e.Effect = DragDropEffects.All;
        }
        
        private void helpToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            var message = Tools.ReadHelpFile("qpe");
            var help = new HelpForm(Text + " - Help", message);
            help.ShowDialog();
        }
        
        private void exportLogFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Tools.ExportLog(Text, lstLog.Items);
        }

        private void openFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var ofd = new OpenFileDialog
                {
                    InitialDirectory = Tools.CurrentFolder,
                    Title = "Select CON / LIVE / DTA / PKG file to open",
                    Multiselect = false
                };
            if (ofd.ShowDialog() != DialogResult.OK) return;
            if (string.IsNullOrWhiteSpace(ofd.FileName)) return;
            ValidateFile(ofd.FileName);
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            try
            {
                if (xPackage != null)
                {
                    xPackage.CloseIO(); //release in case in case
                }
            }
            catch (Exception)
            {}
            var newEditor = new QuickPackEditor(xMainForm, btnReset.BackColor,btnReset.ForeColor, "", incoming_pack);
            xMainForm.activeForm = newEditor;
            newEditor.Show();
            Dispose();
        }

        private void ErrorOut()
        {
            EnableDisable(true);
            if (backgroundWorker1.IsBusy)
            {
                backgroundWorker1.CancelAsync();
            }
            if (backgroundWorker2.IsBusy)
            {
                backgroundWorker2.CancelAsync();
            }
            if (backgroundWorker3.IsBusy)
            {
                backgroundWorker3.CancelAsync();
            }
            Log("There was an error with file " + Path.GetFileName(pack));
            Log("Ready");
            ChangeCursors(false);
        }

        private void EnableDisable(bool enabled, bool depack = false)
        {
            btnRestore.Enabled = removed.Count > 0; //only if there's something in the removed list
            btnRePack.Enabled = enabled;
            btnDePack.Enabled = enabled || depack;
            lstSongs.SelectedIndex = -1;
            lstSongs.Enabled = enabled;
            radioDTA.Enabled = enabled;
            radioPack.Enabled = enabled;
            txtTitle.Enabled = enabled;
            txtDescription.Enabled = enabled;
            chkBackup.Enabled = enabled;
            ContentImage.Enabled = enabled;
            PackageImage.Enabled = enabled;
            menuStrip1.Enabled = enabled;
            btnReset.Enabled = enabled;
            if (enabled) return;
            btnRestore.Enabled = false;
            btnRemove.Enabled = false;
            btnClear.Enabled = false;
            btnSelect.Enabled = false;
        }

        private void btnBegin_Click(object sender, EventArgs e)
        {
            if (isPKG)
            {
                MessageBox.Show("Not supported (yet) when working with PKG files", Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            backup = pack + "_backup";
            EnableDisable(false);
            ChangeCursors(true);
            
            var tempfolder = Application.StartupPath + "\\quickpackeditor\\";
            //if already exists, let's delete to make sure we don't have any duplicate files
            Tools.DeleteFolder(tempfolder, true);
            
            //now create an empty folder to use
            if (!Directory.Exists(tempfolder))
            {
                Directory.CreateDirectory(tempfolder);
            }

            var dta = tempfolder + "temp.dta";
            Tools.DeleteFile(dta);

            if (chkBackup.Checked)
            {
                Log("Creating backup file ... this may take a while");
                Tools.DeleteFile(backup);
                try
                {
                    if (DTAPath != "")
                    {
                        Tools.DeleteFile(DTAPath + ".bak");
                        File.Copy(DTAPath, DTAPath + ".bak");
                    }
                    else
                    {
                        File.Copy(pack, backup);
                    }
                    Log("Backup file created successfully");
                }
                catch (Exception)
                {
                    Log("Error creating backup file!");
                }
            }

            Log("Creating new songs.dta file");
            var sw = new StreamWriter(dta, false, Encoding.Default);
            foreach (var line in from object item in lstSongs.Items let index = (item.ToString().IndexOf("[#", StringComparison.Ordinal) + 2) 
                                 let index2 = item.ToString().IndexOf("]", index, StringComparison.Ordinal) select (Convert.ToInt16(item.ToString().Substring(index, index2 - index)) -1)
                                 into number from line in Songs[number].DTALines select line)
            {
                sw.WriteLine(line);
            }
            sw.Dispose();

            if (DTAPath != "")
            {
                Tools.DeleteFile(DTAPath);
                Tools.MoveFile(dta, DTAPath);

                if (DTAPath.EndsWith("\\Merged Songs\\songs.dta", StringComparison.Ordinal) && removed.Any()) //i.e. editing PS3 file
                {
                    sw = new StreamWriter(Path.GetDirectoryName(DTAPath) + "\\deleted.txt",false);
                    foreach (var t in removed)
                    {
                        sw.WriteLine(Songs[t].Artist + " - " + Songs[t].Name + " :: " + Songs[t].InternalName);
                    }
                    sw.Dispose();
                }
                Log("New songs.dta file created successfully");
                EndWorkers();
                return;
            }
            
            Log("New songs.dta file created successfully");
            if (radioDTA.Checked)
            {
                backgroundWorker2.RunWorkerAsync();
            }
            else if (radioPack.Checked)
            {
                backgroundWorker3.RunWorkerAsync();
            }
        }

        private bool UnlockSignSong(string file)
        {
            var success = Tools.UnlockCON(file);
            
            if (success && signAsCON.Checked)
            {
                success = Tools.SignCON(file);
            }
            return success;
        }

        private bool UnlockSignPack()
        {
            var success = true;

            Log("Trying to unlock CON file");
            if (Tools.UnlockCON(pack))
            {
                Log("Unlocked CON file successfully");
            }
            else
            {
                Log("Error unlocking CON file");
                success = false;
            }

            if (!success) return false;
            if (!signAsCON.Checked) return true; //no need to sign
            if (Tools.SignCON(pack))
            {
                Log("CON file signed successfully");
            }
            else
            {
                Log("Error signing CON file");
                success = false;
            }
            return success;
        }

        private bool FinishRepackage()
        {
            return FinishRepackage(txtTitle.Text, txtDescription.Text, false, "", "");
        }

        private bool FinishRepackage(string title, string description, bool depack, string thumbnail, string output_path)
        {
            repackaged.HeaderData.SaveConsoleID = 0;
            repackaged.HeaderData.ProfileID = 0;
            if (rockBandToolStripMenuItem.Checked)
            {
                repackaged.HeaderData.TitleID = 0x45410829;
                repackaged.HeaderData.Title_Package = "Rock Band";
            }
            else if (rockBand2ToolStripMenuItem.Checked)
            {
                repackaged.HeaderData.TitleID = 0x45410869;
                repackaged.HeaderData.Title_Package = "Rock Band 2";
            }
            else
            {
                repackaged.HeaderData.TitleID = 0x45410914;
                repackaged.HeaderData.Title_Package = "Rock Band 3";
            }
            repackaged.HeaderData.SetLanguage(Languages.English);
            repackaged.HeaderData.Publisher = "";
            if (signAsCON.Checked)
            {
                repackaged.HeaderData.ThisType = PackageType.SavedGame;
                repackaged.HeaderData.MakeAnonymous();
            }
            else
            {
                repackaged.HeaderData.ThisType = PackageType.MarketPlace;
            }
            repackaged.HeaderData.Description = description;
            repackaged.HeaderData.Title_Display = title;
            repackaged.HeaderData.ContentImageBinary = ContentImage.Image.ImageToBytes(ImageFormat.Png);

            if (depack && File.Exists(thumbnail) && useSongAlbumArt.Checked)
            {
                repackaged.HeaderData.PackageImageBinary = Tools.NemoLoadImage(thumbnail).ImageToBytes(ImageFormat.Png);
            }
            else
            {
                repackaged.HeaderData.PackageImageBinary = PackageImage.Image.ImageToBytes(ImageFormat.Png);
            }

            if (!depack)
            {
                Log("Saving rePACKaged file ... sit tight");
                Log("THIS STEP MAY TAKE A WHILE. DON'T CLOSE ME DOWN!");
            }

            var newPack = depack ? output_path : pack + "_new";
            try
            {
                signature = new RSAParams(Application.StartupPath + "\\bin\\KV.bin");
                var xy = new STFSPackage(repackaged, signature, newPack);
                xy.CloseIO();
                
                if (!depack)
                {
                    Log("Awesome ... looks like everything went smoothly");
                }
            }
            catch (Exception ex)
            {
                Log("There was an error: " + ex.Message);
                return false;
            }

            var success = true;
            if (depack) return UnlockSignSong(output_path);
            try
            {
                File.Delete(pack);
            }
            catch (Exception)
            {
                success = false;
            }

            if (success)
            {
                success = Tools.MoveFile(newPack, pack);
            }

            if (success) return UnlockSignPack();
            Log("Could not delete the original file and rename the new one");
            Log("The pack will be created with name '" + Path.GetFileName(pack) + "_new'");
            pack = pack + "_new";

            return UnlockSignPack();
        }

        private bool FinishPackage()
        {
            xPackage.Header.SaveConsoleID = 0;
            xPackage.Header.ProfileID = 0;
            if (rockBandToolStripMenuItem.Checked)
            {
                xPackage.Header.TitleID = 0x45410829;
                xPackage.Header.Title_Package = "Rock Band";
            }
            else if (rockBand2ToolStripMenuItem.Checked)
            {
                xPackage.Header.TitleID = 0x45410869;
                xPackage.Header.Title_Package = "Rock Band 2";
            }
            else
            {
                xPackage.Header.TitleID = 0x45410914;
                xPackage.Header.Title_Package = "Rock Band 3";
            }
            xPackage.Header.SetLanguage(Languages.English);
            xPackage.Header.Publisher = "";
            if (signAsCON.Checked)
            {
                repackaged.HeaderData.ThisType = PackageType.SavedGame;
                repackaged.HeaderData.MakeAnonymous();
            }
            else
            {
                repackaged.HeaderData.ThisType = PackageType.MarketPlace;
            }
            xPackage.Header.Description = txtDescription.Text;
            xPackage.Header.Title_Display = txtTitle.Text;
            xPackage.Header.ContentImageBinary = ContentImage.Image.ImageToBytes(ImageFormat.Png);
            xPackage.Header.PackageImageBinary = PackageImage.Image.ImageToBytes(ImageFormat.Png);

            try
            {
                Log("Saving file ... sit tight");
                Log("THIS STEP MAY TAKE A WHILE. DON'T CLOSE ME DOWN!");

                signature = new RSAParams(Application.StartupPath + "\\bin\\KV.bin");
                xPackage.RebuildPackage(signature);
                xPackage.FlushPackage(signature);
                xPackage.CloseIO();
            }
            catch (Exception ex)
            {
                Log("There was an error: " + ex.Message);
                xPackage.CloseIO();
                return false;
            }

            return UnlockSignPack();
        }

        private void ChangeCursors(bool building)
        {
            if (building)
            {
                Cursor = Cursors.WaitCursor;
                lstSongs.Cursor = Cursors.WaitCursor;
                lstLog.Cursor = Cursors.WaitCursor;
                txtTitle.Cursor = Cursors.WaitCursor;
                txtDescription.Cursor = Cursors.WaitCursor;
                PackageImage.Cursor = Cursors.WaitCursor;
                ContentImage.Cursor = Cursors.WaitCursor;
            }
            else
            {
                Cursor = Cursors.Default;
                lstSongs.Cursor = Cursors.Default;
                lstLog.Cursor = Cursors.Default;
                txtTitle.Cursor = Cursors.Default;
                txtDescription.Cursor = Cursors.Default;
                PackageImage.Cursor = Cursors.Hand;
                ContentImage.Cursor = Cursors.Hand;
            }

            picWorking.Visible = building;
            
        }

        private void btnRemove_Click(object sender, EventArgs e)
        {
            if(isPKG)
            {
                MessageBox.Show("Not supported (yet) when working with PKG files", Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            for (var i = lstSongs.SelectedIndices.Count - 1; i >= 0; i--)
            {
                var entry = lstSongs.Items[lstSongs.SelectedIndices[i]].ToString();
                var index = entry.IndexOf("[#", StringComparison.Ordinal) + 2;
                var index2 = entry.IndexOf("]", index);
                var number = Convert.ToInt16(entry.Substring(index, index2 - index));
                removed.Add(number - 1);
                lstSongs.Items.RemoveAt(lstSongs.SelectedIndices[i]);
            }

            if (lstSongs.Items.Count == 0)
            {
                lstSongs.Items.Add("Pack contents will shown here");
                btnRePack.Enabled = false;
                btnSelect.Enabled = false;
                btnClear.Enabled = false;
            }

            if (removed.Count <= 0) return;
            btnRestore.Enabled = true;
            Log("Removed " + removed.Count + " song " + (removed.Count > 1 ? "entries" : "entry"));
        }

        private void btnRestore_Click(object sender, EventArgs e)
        {
            if (isPKG)
            {
                MessageBox.Show("Not supported (yet) when working with PKG files", Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            var count = removed.Count;
            lstSongs.Items.Clear();
            removed.Clear();
            PopulateEntries();
            btnRestore.Enabled = false;
            btnRePack.Enabled = true;
            Log("Restored " + count + " song " + (count > 1 ? "entries" : "entry"));
        }

        private void lstSongs_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lstSongs.Cursor == Cursors.WaitCursor)
            {
                lstSongs.SelectedIndex = -1;
                return;
            }
            btnRemove.Enabled = lstSongs.SelectedIndex > -1;
            btnClear.Enabled = lstSongs.SelectedIndex > -1;
            btnExtract.Visible = lstSongs.SelectedItems.Count == 1 && lstSongs.SelectedItem.ToString() != "Pack contents will be shown here";
        }

        private void rockBandToolStripMenuItem_Click(object sender, EventArgs e)
        {
            rockBandToolStripMenuItem.Checked = true;
            rockBand2ToolStripMenuItem.Checked = false;
            rockBand3ToolStripMenuItem.Checked = false;

            ContentImage.Image = Resources.RB1;
            Log("Game ID and Content Image changed to Rock Band 1");
        }

        private void rockBand2ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            rockBandToolStripMenuItem.Checked = false;
            rockBand2ToolStripMenuItem.Checked = true;
            rockBand3ToolStripMenuItem.Checked = false;

            ContentImage.Image = Resources.RB2;
            Log("Game ID and Content Image changed to Rock Band 2");
        }

        private void rockBand3ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            rockBandToolStripMenuItem.Checked = false;
            rockBand2ToolStripMenuItem.Checked = false;
            rockBand3ToolStripMenuItem.Checked = true;

            ContentImage.Image = Resources.RB3;
            Log("Game ID and Content Image changed to Rock Band 3");
        }

        private void PackageImage_DragDrop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.Bitmap))
            {
                PackageImage.Image = (Bitmap)e.Data.GetData(DataFormats.Bitmap);
                Log("Content Image changed");
                return;
            }

            var files = (string[])e.Data.GetData(DataFormats.FileDrop);
            getImage(files[0], PackageImage);
        }

        private void ContentImage_DragDrop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.Bitmap))
            {
                ContentImage.Image = (Bitmap)e.Data.GetData(DataFormats.Bitmap);
                Log("Package Image changed");
                return;
            }

            var files = (string[])e.Data.GetData(DataFormats.FileDrop);
            getImage(files[0], ContentImage);
        }

        private void getImage(String file, PictureBox box)
        {
            var tempfolder = Application.StartupPath + "\\quickpackeditor\\";
            if (!Directory.Exists(tempfolder))
            {
                Directory.CreateDirectory(tempfolder);
            }

            var contentImage = "";

            try
            {
                //if not passed a string path for the image
                //show dialog box to find one
                if (string.IsNullOrWhiteSpace(file))
                {
                    var openFileDialog1 = new OpenFileDialog
                    {
                        Filter = "Image Files|*.bmp;*.jpg;*.jpeg;*.png",
                        Title = "Select an image",
                        InitialDirectory = Application.StartupPath + "\\res\\thumbs"
                    };
                    if (openFileDialog1.ShowDialog() == DialogResult.OK)
                    {
                        contentImage = openFileDialog1.FileName;
                    }
                }
                else
                {
                    //if file is not blank, then use that for the image

                    if ((file.Contains(".jpg") || file.Contains(".bmp") ||
                        file.Contains(".png") || file.Contains(".jpeg")) && !file.Contains(".png_xbox") && !file.Contains(".png_wii"))
                    {
                        contentImage = file;
                    }
                    else
                    {
                        Log("That's not a valid image file");
                        return;
                    }
                }

                if (string.IsNullOrWhiteSpace(contentImage)) return;

                var thumbnail = Tools.NemoLoadImage(contentImage);
                if (thumbnail.Width == 64 && thumbnail.Height == 64)
                {
                    box.Image = thumbnail;
                    Log((box == PackageImage ? "Package" : "Content") + " Image changed");
                    return;
                }

                var newimage = tempfolder + Path.GetFileNameWithoutExtension(contentImage) + ".png";
                Tools.ResizeImage(contentImage, 64, "png", newimage);

                if (File.Exists(newimage))
                {
                    box.Image = Tools.NemoLoadImage(newimage);
                    Log((box == PackageImage ? "Package" : "Content") + " Image changed");
                }
                else
                {
                    Log("Something went wrong, image not loaded");
                }
                Tools.DeleteFile(newimage);
            }
            catch
            {
                Log("Error loading image ... try again");
            }
        }
        
        private void btnClear_Click(object sender, EventArgs e)
        {
            lstSongs.ClearSelected();
        }

        private void btnSelect_Click(object sender, EventArgs e)
        {
            lstSongs.BeginUpdate();
            for (var i = 0; i < lstSongs.Items.Count; i++)
            {
                lstSongs.SetSelected(i, true);
            }
            lstSongs.EndUpdate();
        }

        private void chkBackup_CheckedChanged(object sender, EventArgs e)
        {
            Log("Create backup option " + (chkBackup.Checked ? "enabled" : "disabled"));
        }
        
        private void useSongAlbumArtToolStripMenuItem_Click(object sender, EventArgs e)
        {
            usePackThumbnail.Checked = !useSongAlbumArt.Checked;
        }

        private void usePackThumbnailToolStripMenuItem_Click(object sender, EventArgs e)
        {
            useSongAlbumArt.Checked = !usePackThumbnail.Checked;
        }

        private void btnDePack_Click(object sender, EventArgs e)
        {
            if (isPKG)
            {
                MessageBox.Show("Not supported (yet) when working with PKG files", Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            if (btnDePack.Text == "Cancel")
            {
                Log("User cancelled process...stopping as soon as possible");
                backgroundWorker1.CancelAsync();
                btnDePack.Enabled = false;
                return;
            }
            btnDePack.Text = "Cancel";
            toolTip1.SetToolTip(btnDePack, "Click to cancel dePACKing process");
            
            PackagesToUnpack.Clear();
            PackagesToUnpack.Add(pack);
            if (isDTA)
            {
                dePACKDTA();
            }
            else
            {
                dePACKFiles();
            }            
        }

        private void dePACKDTA()
        {
            Log("dePACKing " + Songs.Count + " component DTA files from pack DTA file");

            foreach (var song in Songs)
            {
                var path = Path.GetDirectoryName(DTAPath) + "\\" + song.Artist + " - " + song.Name + ".dta";
                var sw = new StreamWriter(path, false, Encoding.UTF8);
                foreach (var line in song.DTALines)
                {
                    if (line.Contains("#ifndef kControllerRealGuitar") || line.Contains("#endif")) continue;
                    if (line.Contains("latin1"))
                    {
                        sw.WriteLine(line.Replace("latin1", "utf8"));
                    }
                    else
                    {
                        sw.WriteLine(line);
                    }
                }
                sw.Dispose();
            }

            Log("Finished");
            Process.Start(Path.GetDirectoryName(DTAPath));
        }

        private void dePACKFiles()
        {
            Log("dePACKing your files ... sit tight");
            Log("THIS MIGHT TAKE A WHILE. DON'T CLOSE ME DOWN!");

            EnableDisable(false, true);
            ChangeCursors(true);
            var tempfolder = Application.StartupPath + "\\quickpackeditor\\";
            var outputfolder = Path.GetDirectoryName(PackagesToUnpack[0]) + "\\dePACKed files\\";

            //if already exists, let's delete to make sure we don't have any duplicate files
            Tools.DeleteFolder(tempfolder, true);

            //now create an empty folder to use
            if (!Directory.Exists(tempfolder))
            {
                Directory.CreateDirectory(tempfolder);
            }
            if (!Directory.Exists(outputfolder))
            {
                Directory.CreateDirectory(outputfolder);
            }
            backgroundWorker1.RunWorkerAsync();
        }

        private void backgroundWorker1_DoWork(object sender, System.ComponentModel.DoWorkEventArgs e)
        {
            var tempfolder = Application.StartupPath + "\\quickpackeditor\\";
            var outputfolder = Path.GetDirectoryName(PackagesToUnpack[0]) + "\\dePACKed files\\";
            var counter = 0;

            if (PackagesToUnpack.Count > 1)
            {
                Log("Starting Batch dePACKing ... this might take a while");
            }
            foreach (var file in PackagesToUnpack.TakeWhile(file => !backgroundWorker1.CancellationPending))
            {
                //read and grab contents of DTA file
                ReadContents(file);

                xPackage = new STFSPackage(file);
                if (!xPackage.ParseSuccess)
                {
                    xPackage.CloseIO();
                    Log("Error parsing pack '" + Path.GetFileName(file) + "' to extract the files");
                    continue;
                }

                Directory.CreateDirectory(tempfolder);

                Log("Extracting files from pack '" + Path.GetFileName(file) + "'");
                if (!xPackage.ExtractPayload(tempfolder, true, false))
                {
                    xPackage.CloseIO();
                    Log("Error extracting files from pack '" + Path.GetFileName(file) + "'");
                    continue;
                }
                var title = xPackage.Header.Title_Display;
                xPackage.CloseIO();
                Log("Extracted pack contents successfully");

                var songsfolder = tempfolder + "root\\songs\\";
                var tempdta = tempfolder + "\\temp.dta";
                Tools.DeleteFile(tempdta);

                try
                {
                    foreach (var song in Songs.TakeWhile(song => !backgroundWorker1.CancellationPending))
                    {
                        var songname = Tools.CleanString(song.Artist + " - " + song.Name, false);
                        var filename = songname;

                        if (useArtistSongShortName.Checked)
                        {
                            filename = Tools.CleanString(song.Artist, false) + Tools.CleanString(song.Name, false) + song.InternalName;                            
                        }
                        if (File.Exists(outputfolder + filename))
                        {
                            filename += "(1)";
                        }

                        Log("Creating CON file for '" + songname + "'");
                        repackaged = new CreateSTFS();
                        repackaged.AddFolder("songs");

                        var sw = new StreamWriter(tempdta, false, Encoding.UTF8);
                        foreach (var line in song.DTALines)
                        {
                            if (line.Contains("#ifndef kControllerRealGuitar") || line.Contains("#endif")) continue;
                            if (line.Contains("latin1"))
                            {
                                sw.WriteLine(line.Replace("latin1", "utf8"));
                            }
                            else
                            {
                                sw.WriteLine(line);
                            }
                        }
                        sw.Dispose();

                        repackaged.AddFile(tempdta, "songs/songs.dta");

                        var path = songsfolder + song.InternalName + "\\";
                        if (!Directory.Exists(path))
                        {
                            Log("Can't find the folder of files for " + song.Artist + " - " + song.Name);
                            Log("Can't find " + path);
                            Log("This is likely to a problem with the songs.dta file, skipping this song...");
                            continue;
                        }
                        repackaged.AddFolder("songs/" + song.InternalName);
                        //add mid, mogg and other files found at the songname folder level
                        var songContents = Directory.GetFiles(path);
                        if (songContents.Any())
                        {
                            foreach (var contents in songContents)
                            {
                                repackaged.AddFile(contents, "songs/" + song.InternalName + "/" + Path.GetFileName(contents));
                            }
                        }

                        if (Directory.Exists(songsfolder + song.InternalName + "\\gen\\"))
                        {
                            //add all items at the gen level (mostly png_xbox and milo_xbox files)
                            repackaged.AddFolder("songs/" + song.InternalName + "/gen");
                            var subContents = Directory.GetFiles(songsfolder + song.InternalName + "\\gen\\");
                            if (subContents.Any())
                            {
                                foreach (var contents in subContents)
                                {
                                    repackaged.AddFile(contents, "songs/" + song.InternalName + "/gen/" + Path.GetFileName(contents));
                                }
                            }
                        }
                        else
                        {
                            Log("No /gen folder found for " + song.Artist + " - " + song.Name);
                            Log("No album art will be added!");
                        }

                        var thumbnail = tempfolder + "temp.png";
                        Tools.DeleteFile(thumbnail);
                        var albumart = songsfolder + song.InternalName + "\\gen\\" + song.InternalName + "_keep.png_xbox";
                        if (useSongAlbumArt.Checked && File.Exists(albumart))
                        {
                            thumbnail = Tools.ConvertRBImage(albumart, thumbnail, "png") ? thumbnail : "";
                            thumbnail = Tools.ResizeImage(thumbnail, 64, "png") ? thumbnail : "";
                        }

                        if (FinishRepackage(song.Artist + " - " + song.Name, "dePACKed with Nautilus from pack '" + title + "'", true, thumbnail, outputfolder + filename))
                        {
                            Log("Created CON file successfully for '" + songname + "'");
                            counter++;
                        }
                        else
                        {
                            Log("Failed to create CON file for '" + songname + "'");
                        }
                    }
                }
                catch (Exception ex)
                {
                    Log("There was an error dePACKing the files");
                    Log("Error says: " + ex.Message); 
                }
            }

            Tools.DeleteFolder(tempfolder, true);

            if (counter > 0)
            {
                Log("Successfully dePACKaged " + counter + (counter > 1 ? " files" : " file"));
                if (openFolderAfterDePACK.Checked)
                {
                    Process.Start(outputfolder);
                }
            }
            else
            {
                Log("Failed to dePACKage any files");
            }

            if (PackagesToUnpack.Count > 1)
            {
                Log("Batch dePACKing completed");
            }
        }

        private void backgroundWorker1_RunWorkerCompleted(object sender, System.ComponentModel.RunWorkerCompletedEventArgs e)
        {
            EndWorkers();
            btnDePack.Text = "dePACK";
            toolTip1.SetToolTip(btnDePack, "Click to begin dePACKing process");
        }

        private void backgroundWorker2_RunWorkerCompleted(object sender, System.ComponentModel.RunWorkerCompletedEventArgs e)
        {
            EndWorkers();
        }

        private void EndWorkers()
        {
            ChangeCursors(false);
            EnableDisable(false);
            btnReset.Enabled = true;
            Log("Ready");
        }

        private void backgroundWorker2_DoWork(object sender, System.ComponentModel.DoWorkEventArgs e)
        {
            var tempfolder = Application.StartupPath + "\\quickpackeditor\\";
            var dta = tempfolder + "\\temp.dta";

            xPackage = new STFSPackage(pack);
            if (!xPackage.ParseSuccess)
            {
                xPackage.CloseIO();
                Log("Error parsing the pack file");
                ErrorOut();
                return;
            }

            var xent = xPackage.GetFile("/songs/songs.dta");
            if (xent == null)
            {
                xPackage.CloseIO();
                Log("Error accessing the DTA file in the pack");
                ErrorOut();
                return;
            }

            Log("Replacing songs.dta file with new copy");
            Log("THIS STEP MAY TAKE A WHILE. DON'T CLOSE ME DOWN!");

            if (!xent.Replace(dta))
            {
                xPackage.CloseIO();
                Log("Error replacing DTA file");

                if (File.Exists(backup))
                {
                    Tools.DeleteFile(pack);
                    Log(Tools.MoveFile(backup, pack) ? "Backup file restored successfully" : "Error restoring backup file");
                }
                ErrorOut();
                return;
            }
            if (FinishPackage())
            {
                Log("rePACKaged file successfully");
            }
            Tools.DeleteFolder(tempfolder, true);
        }

        private void backgroundWorker3_DoWork(object sender, System.ComponentModel.DoWorkEventArgs e)
        {
            var tempfolder = Application.StartupPath + "\\quickpackeditor\\";
            var dta = tempfolder + "\\temp.dta";

            xPackage = new STFSPackage(pack);
            if (!xPackage.ParseSuccess)
            {
                xPackage.CloseIO();
                Log("Error parsing pack '" + Path.GetFileName(pack) + "' to extract the files");
                ErrorOut();
                return;
            }

            if (!xPackage.ExtractPayload(tempfolder, true, false))
            {
                xPackage.CloseIO();
                Log("Error extracting files from pack '" + Path.GetFileName(pack) + "'");
                ErrorOut();
                return;
            }
            xPackage.CloseIO();

            var songsfolder = tempfolder + "root\\songs\\";
            var upgfolder = tempfolder + "root\\songs_upgrades\\";

            foreach (var i in removed.Where(i => Directory.Exists(songsfolder + Songs[i].InternalName)))
            {
                Tools.DeleteFolder(songsfolder + Songs[i].InternalName, true);
            }

            repackaged = new CreateSTFS();
            repackaged.AddFolder("songs");
            if (File.Exists(dta))
            {
                repackaged.AddFile(dta, "songs/songs.dta");
            }

            var subFolders = Directory.GetDirectories(songsfolder);

            foreach (var songName in subFolders.Select(folder => folder.Substring(songsfolder.Length, folder.Length - songsfolder.Length)).Select(songName => songName.Replace("\\", "")).Where(songName => Directory.Exists(songsfolder + songName)))
            {
                repackaged.AddFolder("songs/" + songName);

                //add mid, mogg and other files found at the songname folder level
                var songContents = Directory.GetFiles(songsfolder + songName + "\\");
                if (songContents.Any())
                {
                    foreach (var contents in songContents)
                    {
                        repackaged.AddFile(contents, "songs/" + songName + "/" + Path.GetFileName(contents));
                    }
                }

                //all all items at the gen level (mostly png_xbox and milo_xbox files)
                repackaged.AddFolder("songs/" + songName + "/gen");
                var subContents = Directory.GetFiles(songsfolder + songName + "\\gen\\");
                if (!subContents.Any()) continue;
                foreach (var contents in subContents)
                {
                    repackaged.AddFile(contents, "songs/" + songName + "/gen/" + Path.GetFileName(contents));
                }
            }

            if (Directory.Exists(upgfolder))
            {
                var checkUpg = Directory.GetFiles(upgfolder);

                if (!(checkUpg.Count() <= 1))
                {
                    repackaged.AddFolder("songs_upgrades");
                    if (File.Exists(upgfolder + "upgrades.dta"))
                    {
                        repackaged.AddFile(upgfolder + "upgrades.dta", "songs_upgrades/upgrades.dta");
                    }
                    var songsInput = Directory.GetFiles(upgfolder);
                    foreach (var file in from file in songsInput where (file.Substring(file.Length - 4, 4) == ".mid") let ending = file.Substring(file.Length - 7, 7) where ending != "(1).mid" && ending != "(2).mid" && ending != "(3).mid" && ending != "(4).mid" && ending != "(5).mid" select file)
                    {
                        repackaged.AddFile(file, "songs_upgrades/" + Path.GetFileName(file));
                    }
                }
            }
            if (FinishRepackage())
            {
                Log("rePACKaged file successfully");
            }
            Tools.DeleteFolder(tempfolder, true);
        }

        private void backgroundWorker3_RunWorkerCompleted(object sender, System.ComponentModel.RunWorkerCompletedEventArgs e)
        {
            EndWorkers();
        }

        private void PackageImage_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                getImage("", PackageImage);
            }
        }

        private void ContentImage_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                getImage("", ContentImage);
            }
        }

        private void QuickPack_Shown(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(incoming_pack) && File.Exists(incoming_pack))
            {
                ValidateFile(incoming_pack);
                incoming_pack = "";
                return;
            }
            if (string.IsNullOrWhiteSpace(DTAPath) || !File.Exists(DTAPath)) return;
            DoDTAOnly();
        }

        private void DoDTAOnly()
        {
            radioDTA.Checked = true;
            radioPack.Checked = false;
            radioPack.Enabled = false;
            ContentImage.Enabled = false;
            PackageImage.Enabled = false;
            btnDePack.Visible = false;
            txtDescription.Enabled = false;
            txtTitle.Enabled = false;
            btnRePack.Enabled = true;
            btnRePack.Text = "Save";
            optionsToolStripMenuItem.Enabled = false;
            openFileToolStripMenuItem.Enabled = false;
            ReadDTA(File.ReadAllBytes(DTAPath));
        }

        private void QuickPackEditor_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!picWorking.Visible)
            {
                Tools.DeleteFolder(Application.StartupPath + "\\pack\\", true);
                return;
            }
            MessageBox.Show("Please wait until the current process finishes", Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            e.Cancel = true;
        }

        private void sortByDTAEntry_Click(object sender, EventArgs e)
        {
            PopulateEntries(0);
        }

        private void sortByArtistSong_Click(object sender, EventArgs e)
        {
            PopulateEntries();
        }

        private void sortBySongArtist_Click(object sender, EventArgs e)
        {
            PopulateEntries(2);
        }

        private void contextMenuStrip2_Opening(object sender, System.ComponentModel.CancelEventArgs e)
        {
            sortByArtistSong.Enabled = lstSongs.Items.Count > 1;
            sortByDTAEntry.Enabled = sortByArtistSong.Enabled;
            sortBySongArtist.Enabled = sortByArtistSong.Enabled;
        }

        private void batchDePACKToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Batch dePACKing might take a while depending on how many packs you're dePACKing and how big those packs are\nAre you sure you want to do this now?",
                    Text, MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
            {
                return;
            }

            var ofd = new FolderPicker
            {
                Title = "Select folder with packs to dePACK",
            };
            if (ofd.ShowDialog(IntPtr.Zero) != true) return;

            Log("Selected folder '" + ofd.ResultPath + "'");

            var files = Directory.GetFiles(ofd.ResultPath);
            if (!files.Any())
            {
                Log("No files found in that folder");
                Log("Ready");
                return;
            }
            
            PackagesToUnpack.Clear();
            foreach (var file in files.Where(file => VariousFunctions.ReadFileType(file) == XboxFileType.STFS))
            {
                PackagesToUnpack.Add(file);
            }
            
            if (!PackagesToUnpack.Any())
            {
                Log("No CON files found in that folder");
                Log("Ready");
                return;
            }
            btnDePack.Text = "Cancel";
            toolTip1.SetToolTip(btnDePack, "Click to cancel dePACKing process");
            dePACKFiles();
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

        private void btnExtract_Click(object sender, EventArgs e)
        {
            if (isPKG)
            {
                MessageBox.Show("Not supported (yet) when working with PKG files", Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            Log("Extracting the selected file ... sit tight");
            Log("THIS MIGHT TAKE A WHILE. DON'T CLOSE ME DOWN!");

            var output = new SaveFileDialog() { Title = "Select where to save the extracted song" };
            output.ShowDialog();
            fileToExtract = output.FileName;

            if (string.IsNullOrEmpty(fileToExtract))
            {
                Log("Cancelled");
                return;
            }
            backgroundWorker4.RunWorkerAsync();
        }

        private void backgroundWorker4_DoWork(object sender, System.ComponentModel.DoWorkEventArgs e)
        {
            var tempfolder = Application.StartupPath + "\\quickpackeditor\\";
            Tools.DeleteFolder(tempfolder, true);
            if (!Directory.Exists(tempfolder))
            {
                Directory.CreateDirectory(tempfolder);
            }
            var tempDTA = tempfolder + "songs.dta";
            var xPackage = new STFSPackage(pack);
            if (!xPackage.ParseSuccess)
            {
                Log("Error opening pack to extract song");
                return;
            }
            var xfile = xPackage.GetFile("songs/songs.dta");
            if (xfile == null)
            {
                Log("Can't find the songs.dta file, can't extract this file");
                xPackage.CloseIO();
                return;
            };
            if (!xfile.ExtractToFile(tempDTA))
            {
                Log("Failed to extract songs.dta file, can't extract this song");
            }
            
            Log("Opened DTA file \"" + Path.GetFileName(tempDTA) + "\"");
            if (!Parser.ReadDTA(File.ReadAllBytes(tempDTA)))
            {
                Log("Failed to parse DTA file \"" + Path.GetFileName(tempDTA) + "\"");
            }
            Log("DTA file \"" + Path.GetFileName(tempDTA) + "\" contains " + Parser.Songs.Count + " component DTA file(s)");

            int dtaEntry;
            var selection = lstSongs.SelectedItems[0].ToString();
            var index1 = selection.IndexOf("[#");
            var index2 = selection.IndexOf("]", index1);
            dtaEntry = Convert.ToInt16(selection.Substring(index1 + 2, index2 - index1 - 2));

            File.Delete(tempDTA);
            var sw = new StreamWriter(tempDTA, false, System.Text.Encoding.UTF8);
            foreach (var line in Parser.Songs[dtaEntry - 1].DTALines)
            {
                if (line.Contains("latin1"))
                {
                    sw.WriteLine(line.Replace("latin1", "utf8"));
                }
                else
                {
                    sw.WriteLine(line);
                }
            }
            sw.Dispose();
            if (!Parser.ReadDTA(File.ReadAllBytes(tempDTA)))
            {
                Log("Failed to parse DTA file \"" + Path.GetFileName(tempDTA) + "\"");
            }
            var internalname = Parser.Songs[0].InternalName;
            if (Parser.Songs.Count == 1)
            {
                Log("Successfully separated DTA file for extraction");
            }
            else
            {
                Log("Something went wrong");
                xPackage.CloseIO();
                return;
            }

            var mogg = tempfolder + internalname + ".mogg";
            var xMogg = xPackage.GetFile("songs/" + internalname + "/" + internalname + ".mogg");
            if (xMogg == null)
            {
                Log("Failed to grab .mogg file, can't extract this file");
                xPackage.CloseIO();
                return;
            }
            else
            {
                Log("Grabbed .mogg file successfully, proceeding to next step");
                xMogg.ExtractToFile(mogg);
            }
            var midi = tempfolder + internalname + ".mid";
            var xMid = xPackage.GetFile("songs/" + internalname + "/" + internalname + ".mid");
            if (xMid == null)
            {
                Log("Failed to grab .mid file, can't extract this file");
                xPackage.CloseIO();
                return;
            }
            else
            {
                Log("Grabbed .mid file successfully, proceeding to next step");
                xMid.ExtractToFile(midi);
            }
            var art = tempfolder + internalname + "_keep.png_xbox";
            var xArt = xPackage.GetFile("songs/" + internalname + "/gen/" + internalname + "_keep.png_xbox");
            if (xArt == null)
            {
                Log("Failed to grab _keep.png_xbox file, continuing without it");
            }
            else
            {
                Log("Grabbed _keep.png_xbox file successfully, proceeding to next step");
                xArt.ExtractToFile(art);
            }
            var milo = tempfolder + internalname + ".milo_xbox";
            var xMilo = xPackage.GetFile("songs/" + internalname + "/gen/" + internalname + ".milo_xbox");
            if (xMilo == null)
            {
                Log("Failed to grab .milo_xbox file, continuing without it");
            }
            else
            {
                Log("Grabbed .milo_xbox file successfully, proceeding to next step");
                xMilo.ExtractToFile(milo);
            }
            xPackage.CloseIO();

            repackaged = new CreateSTFS();
            repackaged.AddFolder("songs");
            repackaged.AddFile(tempDTA, "songs/songs.dta");

            var song = Parser.Songs[0];
            repackaged.AddFolder("songs/" + song.InternalName);
            //add mid and mogg files
            repackaged.AddFile(midi, "songs/" + song.InternalName + "/" + Path.GetFileName(midi));
            repackaged.AddFile(mogg, "songs/" + song.InternalName + "/" + Path.GetFileName(mogg));
            repackaged.AddFolder("songs/" + song.InternalName + "/gen");
            //add png_xbox and milo_xbox files
            if (File.Exists(art))
            {
                repackaged.AddFile(art, "songs/" + song.InternalName + "/gen/" + Path.GetFileName(art));
            }
            if (File.Exists(milo))
            {
                repackaged.AddFile(milo, "songs/" + song.InternalName + "/gen/" + Path.GetFileName(milo));
            }

            var thumbnail = tempfolder + "thumb.png";
            Tools.DeleteFile(thumbnail);
            thumbnail = Tools.ConvertRBImage(art, thumbnail, "png") ? thumbnail : "";
            thumbnail = Tools.ResizeImage(thumbnail, 64, "png") ? thumbnail : "";
                
            if (FinishRepackage(song.Artist + " - " + song.Name, "Extracted with Nautilus", true, thumbnail, fileToExtract))
            {
                Log("Extracted file successfully for '" + Path.GetFileName(fileToExtract) + "'");
            }
            else
            {
                Log("Failed to extract file for '" + Path.GetFileName(fileToExtract) + "'");
            }
            
        }

        private void ExtractPKG(string file)
        {
            var folder = Application.StartupPath + "\\pack\\";
            if (!Directory.Exists(folder))
            {
                Directory.CreateDirectory(folder);
            }

            var outFolder = folder + Path.GetFileNameWithoutExtension(file).Replace(" ", "").Replace("-", "").Replace("_", "").Trim() + "_ex";
            Tools.DeleteFolder(outFolder, true);
            string klic;
            if (!Tools.ExtractPKG(file, outFolder, out klic))
            {
                Log("Failed to process that PKG file");
                return;
            }
            var DTA = Directory.GetFiles(outFolder, "songs.dta", SearchOption.AllDirectories);
            if (DTA.Count() == 0)
            {
                Log("No songs.dta file found, can't process");
                return;
            }
            ReadDTA(File.ReadAllBytes(DTA[0]));
            isPKG = true;
        }

        private void signAsCON_Click(object sender, EventArgs e)
        {
            signAsCON.Checked = true;
            signAsLIVE.Checked = false;
        }

        private void signAsLIVE_Click(object sender, EventArgs e)
        {
            signAsCON.Checked = false;
            signAsLIVE.Checked = true;
        }
    }
}