using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing.Imaging;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Nautilus.Properties;
using Nautilus.x360;
using Microsoft.VisualBasic.FileIO;
using System.Drawing;
using SearchOption = System.IO.SearchOption;
using NautilusFREE;

namespace Nautilus
{
    public partial class VideoPreparer : Form
    {
        private static string inputDir;
        private static List<string> inputFiles;
        private static string tempFolder;
        private static string sOpenPackage;
        private static string xOut;
        private readonly CreateSTFS packfiles = new CreateSTFS();
        private DateTime endTime;
        private DateTime startTime;
        private string songsFolder;
        private readonly DateTime nextFriday;
        private readonly NemoTools Tools;
        private readonly DTAParser Parser;
        private readonly string PackDate;
        private readonly nTools nautilus3;
        private bool doRecursiveSearching;
        private readonly string DescText = "Created with Nautilus for video recording purposes ONLY.";

        public VideoPreparer(Color ButtonBackColor, Color ButtonTextColor)
        {
            InitializeComponent();

            inputFiles = new List<string>();
            Tools = new NemoTools();
            Parser = new DTAParser();
            inputDir = Application.StartupPath + "\\videoprep_input\\";
            nautilus3 = new nTools();

            groupBox1.AllowDrop = true;

            tempFolder = Application.StartupPath + "\\videoprep_temp\\";
            Tools.DeleteFolder(tempFolder, true);
            
            toolTip1.SetToolTip(btnBegin, "Click to begin process");
            toolTip1.SetToolTip(btnFolder, "Click to select the input folder");
            toolTip1.SetToolTip(btnRefresh, "Click to refresh if the contents of the folder have changed");
            toolTip1.SetToolTip(txtFolder, "This is the working directory");
            toolTip1.SetToolTip(txtTitle, "Enter a title for your pack (visible in the Xbox dashboard)");
            toolTip1.SetToolTip(lstLog, "This is the application log. Right click to export");
            
            nextFriday = DateTime.Today;
            while (nextFriday.DayOfWeek != DayOfWeek.Friday)
            {
                nextFriday = nextFriday.AddDays(1D);
            }

            var month = "0" + nextFriday.Month;
            month = month.Substring(month.Length - 2, 2);
            var day = "0" + nextFriday.Day;
            day = day.Substring(day.Length - 2, 2);
            PackDate = month + "/" + day + "/" + nextFriday.Year.ToString(CultureInfo.InvariantCulture).Substring(2, 2);
            
            var formButtons = new List<Button> { btnFolder, btnRefresh, btnReset, btnView, btnBegin };
            foreach (var button in formButtons)
            {
                button.BackColor = ButtonBackColor;
                button.ForeColor = ButtonTextColor;
                button.FlatAppearance.MouseOverBackColor = button.BackColor == Color.Transparent ? Color.FromArgb(127, Color.AliceBlue.R, Color.AliceBlue.G, Color.AliceBlue.B) : Tools.LightenColor(button.BackColor);
            }

            if (!Directory.Exists(tempFolder))
            {
                Directory.CreateDirectory(tempFolder);
            }

            doRecursiveSearching = useRecursiveSearching.Checked;
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
        
        private void txtFolder_TextChanged(object sender, EventArgs e)
        {
            if (picWorking.Visible) return;
            inputFiles.Clear();
            if (string.IsNullOrWhiteSpace(txtFolder.Text))
            {
                btnRefresh.Visible = false;
            }
            btnRefresh.Visible = true;

            if (!(Directory.Exists(inputDir)))
            {
                Directory.CreateDirectory(inputDir);
            }
            if (txtFolder.Text != "")
            {
                Log("");
                Log("Reading input directory ... hang on");
                long byteCount = 0;

                try
                {
                    var inFiles = Directory.GetFiles(txtFolder.Text, ".", doRecursiveSearching ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly);
                    foreach (var file in inFiles)
                    {
                        try
                        {
                            if (VariousFunctions.ReadFileType(file) != XboxFileType.STFS) continue;
                            inputFiles.Add(file);

                            var fileSize = new FileInfo(file);
                            byteCount = byteCount + fileSize.Length;
                        }
                        catch (Exception ex)
                        {
                            Log("There was a problem accessing that file");
                            Log("The error says: " + ex.Message);
                        }
                    }
                    if (!inputFiles.Any())
                    {
                        Log("Did not find any CON files ... try a different directory");
                        Log("You can also drag and drop CON files here");
                        Log("Ready");
                        btnBegin.Visible = false;
                        btnRefresh.Visible = true;
                    }
                    else
                    {
                        Log("Found " + inputFiles.Count + " CON " + (inputFiles.Count > 1 ? "files" : "file"));

                        if (byteCount > 4294967296) //greater than 4.00GB
                        {
                            var sizeInGB = (Decimal) byteCount/1073741824;

                            MessageBox.Show("There is a 4.00GB combined input file size limit\nYour files add up to " +
                                sizeInGB + "GB!\nTry deleting some files from the input folder first",
                                Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                            Log("Combined input file size is over 4GB limit ... delete some files and try again");
                            return;
                        }
                        Log("Ready to begin");
                        btnBegin.Visible = true;
                        btnRefresh.Visible = true;
                    }
                }
                catch (Exception ex)
                {
                    Log("There was an error: " + ex.Message);
                }
            }
            else
            {
                btnBegin.Visible = false;
                btnView.Visible = false;
                btnRefresh.Visible = false;
            }
        }

        private bool extractRBFiles()
        {
            var counter = 0;
            var success = 0;

            foreach (var file in inputFiles)
            {
                if (backgroundWorker1.CancellationPending) return false;
                try
                {
                    if (VariousFunctions.ReadFileType(file) != XboxFileType.STFS) continue;
                    try
                    {
                        counter++;
                        Parser.ExtractDTA(file);
                        Parser.ReadDTA(Parser.DTA);
                        if (Parser.Songs.Count > 1)
                        {
                            Log("File " + Path.GetFileName(file) + " is a pack, try dePACKing first, skipping...");
                            continue;
                        }

                        var xPackage = new STFSPackage(file);
                        if (!xPackage.ParseSuccess)
                        {
                            Log("Failed to extract '" + Path.GetFileName(file) + "'");
                            Log("Skipping this file");
                        }

                        //if working inner temp folder exists, delete to start clean
                        var temptempFile = tempFolder + "temp\\";
                        Tools.DeleteFolder(temptempFile, true);

                        if (backgroundWorker1.CancellationPending)
                        {
                            xPackage.CloseIO();
                            return false;
                        }

                        //extract songs folder, subfolders and all files into a combined directory
                        xPackage.ExtractPayload(temptempFile, true, false);
                        xPackage.CloseIO();
                        temptempFile = temptempFile + "root\\";
                        if (Directory.Exists(temptempFile + "songs\\"))
                        {
                            var subFolders = Directory.GetDirectories(temptempFile + "songs\\");
                            var tempFileName = subFolders[0].Substring((temptempFile + "songs\\").Length,
                                                                       subFolders[0].Length -
                                                                       (temptempFile + "songs\\").Length);

                            if (subFolders.Count() != 0)
                                //upgrades won't have subdirectories, skip this step in that case
                            {
                                if (File.Exists(temptempFile + "songs\\songs.dta"))
                                {
                                    //move songs.dta to the song's folder for sorting later
                                    //allows to skip duplicates
                                    Tools.MoveFile(temptempFile + "songs\\songs.dta",
                                              temptempFile + "songs\\" + tempFileName + "\\songs.dta");
                                }
                                foreach (var foldertoMove in subFolders)
                                {
                                    tempFileName = foldertoMove.Substring((temptempFile + "songs\\").Length,
                                                                          foldertoMove.Length -
                                                                          (temptempFile + "songs\\").Length);

                                    var folderpath = tempFolder + "songs\\";

                                    //let's make sure songs folder is there, if not, create it
                                    if (!(Directory.Exists(folderpath)))
                                    {
                                        Directory.CreateDirectory(tempFolder + "songs\\");
                                    }

                                    //if this song already exists in the working directory, delete it
                                    //copy this one instead
                                    Tools.DeleteFolder(folderpath + tempFileName + "\\", true);
                                    
                                    if (Path.GetPathRoot(temptempFile) == Path.GetPathRoot(songsFolder))
                                    {
                                        Directory.Move(temptempFile + "songs\\" + tempFileName + "\\",
                                                       folderpath + tempFileName + "\\");
                                    }
                                    else
                                    {
                                        FileSystem.CopyDirectory(temptempFile + "songs\\" + tempFileName + "\\",
                                                                 folderpath + tempFileName + "\\");
                                    }
                                }
                            }
                            
                            Log("Extracting file " + counter + " of " + inputFiles.Count);
                            success++;
                        }

                        //move other root files but no spa.bin files to root directory
                        var rootFiles = Directory.GetFiles(temptempFile);
                        if (rootFiles.Count() != 0)
                        {
                            foreach (var rootFile in rootFiles.Where(rootFile => rootFile.Substring(rootFile.Length - 7, 7) != "spa.bin"))
                            {
                                Tools.MoveFile(rootFile, tempFolder + Path.GetFileName(rootFile));
                            }
                        }

                        //delete folder to get rid of useless files
                        Tools.DeleteFolder(temptempFile, true);
                    }
                    catch (Exception ex)
                    {
                        Log("There was an error: " + ex.Message);
                        return false;
                    }
                }
                catch (Exception ex)
                {
                    Log("There was a problem accessing that file");
                    Log("The error says: " + ex.Message);
                }
            }
            Log("Successfully extracted " + success + " of " + counter + " files");
            
            return true;
        }

        private bool addFiles()
        {
            if (backgroundWorker1.CancellationPending) return false;

            var name = "song";
            if (chkFTV.Checked)
            {
                name = "ftv";
            }

            var filesAdded = 0;
            if (songsFolder.Substring(songsFolder.Length - 1, 1) != "\\")
            {
                songsFolder = songsFolder + "\\";
            }

            if (Directory.Exists(songsFolder))
            {
                var songsInput = Directory.GetFiles(songsFolder);//, ".", doRecursiveSearching ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly);
                var enoughFolders = Directory.GetDirectories(songsFolder);
                if (enoughFolders.Any()) //only add if there's at least one folder in there
                {
                    packfiles.AddFolder("songs");

                    foreach (var sFile in songsInput.Where(sFile => sFile.Substring(sFile.Length - 4, 4) != ".dta"))
                    {
                        packfiles.AddFile(sFile, "songs/" + Path.GetFileName(sFile));
                    }

                    //create song folders and add contents to each
                    var subFolders = Directory.GetDirectories(songsFolder);
                    var sFolderLength = songsFolder;

                    //if songs.dta doesn't exist, create one. if exists, leave it, will be appended
                    if (!(File.Exists(songsFolder + "songs.dta")))
                    {
                        var tempDta = new StreamWriter(songsFolder + "songs.dta", false, Encoding.Default);
                        tempDta.Dispose();
                    }

                    foreach (var songName in from folder in subFolders select folder.Substring(sFolderLength.Length, folder.Length - sFolderLength.Length) into songName select songName.Replace("\\", "") into songName where Directory.Exists(songsFolder + songName) select songName)//let ending = songName.Substring(songName.Length - 3, 3) where ending != "(1)" && ending != "(2)" && ending != "(3)" && ending != "(4)" && ending != "(5)" select songName)
                    {
                        if (backgroundWorker1.CancellationPending) return false;

                        //check if the file is a copy (i.e. same song) and discard if so
                        var ending = "";
                        if (songName.Length > 3)
                        {
                            ending = songName.Substring(songName.Length - 3, 3);
                        }
                        if (ending == "(1)" || ending == "(2)" || ending == "(3)" || ending == "(4)" || ending == "(5)") continue;

                        filesAdded++;
                        Log("Adding files for song " + filesAdded);
                        //add all items at the songfolder level (mostly .mid and .mogg files)
                        packfiles.AddFolder("songs/" + songName);
                        var didVolume = false;

                        //if songs.dta is there, add to main songs.dta and then delete
                        if (File.Exists(songsFolder + songName + "\\songs.dta"))
                        {
                            var dtaIn = new StreamReader(songsFolder + songName + "\\songs.dta", Encoding.Default);
                            var dtaOut = new StreamWriter(songsFolder + "songs.dta", true, Encoding.Default);
                                    
                            while (dtaIn.Peek() >= 0)
                            {
                                var line = dtaIn.ReadLine();

                                if (string.IsNullOrWhiteSpace(line)) continue;
                                line = Tools.FixBadChars(line);

                                if (line.Contains("mute_volume") && chkVolume.Checked)
                                {
                                    if (!didVolume)
                                    {
                                        dtaOut.WriteLine("      (mute_volume 0.0)");
                                        dtaOut.WriteLine("      (mute_volume_vocals 0.0)");
                                        didVolume = true;                                        
                                    }
                                    line = dtaIn.ReadLine();
                                    continue;                                 
                                }                               

                                if (chkID.Checked)
                                {
                                    //replace short id with custom generated id
                                    if (line == "(")
                                    {
                                        dtaOut.WriteLine(line);
                                        dtaIn.ReadLine();

                                        var month = "0" + nextFriday.Month;
                                        month = month.Substring(month.Length - 2, 2);
                                        var day = "0" + nextFriday.Day;
                                        day = day.Substring(day.Length - 2, 2);

                                        line = "      '" + month + day +
                                               nextFriday.Year.ToString(CultureInfo.InvariantCulture)
                                                         .Substring(2, 2) + name + filesAdded + "'";
                                    }
                                    else if (!string.IsNullOrWhiteSpace(line) && (line.Substring(0, 1) == "(" && !line.Contains(")")))
                                    {
                                        line = "(" + nextFriday.Month + nextFriday.Day +
                                               nextFriday.Year.ToString(CultureInfo.InvariantCulture)
                                                         .Substring(2, 2) + name + filesAdded;
                                    }
                                }

                                if (chkRename.Checked)
                                {
                                    var replace = defaultTool.Checked ? "RECORDING" : "PREVIEW";
                                    //replace artist names to group all together
                                    if (line.ToLowerInvariant().Contains("(artist"))
                                    {
                                        line = "   (artist \"VIDEO " + replace + "\")";
                                    }
                                    if (line.ToLowerInvariant().Contains("'artist'"))
                                    {
                                        dtaOut.WriteLine(line);
                                        dtaIn.ReadLine();
                                        line = "      \"VIDEO " + replace + "\"";
                                    }
                                }

                                if (chkID.Checked)
                                {
                                    //replace song id with custom generated id
                                    if (line.Contains("song_id"))
                                    {
                                        line = "   ('song_id' " + nextFriday.Month + nextFriday.Day +
                                               nextFriday.Year.ToString(CultureInfo.InvariantCulture)
                                                         .Substring(2, 2) + name + filesAdded + ")";
                                    }
                                }

                                //add mute level setting so all instruments are heard even when missed
                                if ((line.Contains("'drum_freestyle'") || line.Contains("(drum_freestyle")) && chkVolume.Checked && !didVolume)
                                {
                                    if (line.Contains("'drum_freestyle'"))
                                    {
                                        dtaOut.WriteLine(line);
                                        line = dtaIn.ReadLine();
                                        dtaOut.WriteLine(line);
                                        line = dtaIn.ReadLine();
                                        dtaOut.WriteLine(line);
                                        line = dtaIn.ReadLine();
                                        dtaOut.WriteLine(line);
                                        line = dtaIn.ReadLine();
                                        dtaOut.WriteLine(line);
                                        line = dtaIn.ReadLine();
                                        dtaOut.WriteLine(line);                                        
                                    }
                                    else if (line.Contains("(drum_freestyle") && !didVolume)
                                    {
                                        dtaOut.WriteLine(line);
                                        line = dtaIn.ReadLine();
                                        dtaOut.WriteLine(line);
                                        line = dtaIn.ReadLine();
                                        dtaOut.WriteLine(line);
                                    }
                                    dtaOut.WriteLine("      (mute_volume 0.0)");
                                    dtaOut.WriteLine("      (mute_volume_vocals 0.0)");
                                    didVolume = true;
                                    continue;
                                }

                                if (line != null && !line.Trim().StartsWith(";;", StringComparison.Ordinal))
                                {
                                    dtaOut.WriteLine(line);
                                }
                            }
                            dtaIn.Dispose();
                            dtaOut.WriteLine(""); //separate entries
                            dtaOut.Dispose();

                            //delete original dta files
                            Tools.DeleteFile(songsFolder + songName + "\\songs.dta");
                        }

                        //add mid, mogg and other files found at the songname folder level
                        var songContents = Directory.GetFiles(songsFolder + songName);
                        if (songContents.Count() != 0)
                        {
                            foreach (var contents in songContents)
                            {
                                if (backgroundWorker1.CancellationPending) return false;
                                packfiles.AddFile(contents, "songs/" + songName + "/" + Path.GetFileName(contents));
                            }
                        }

                        //all all items at the gen level (mostly png_xbox and milo_xbox files)
                        packfiles.AddFolder("songs/" + songName + "/gen");
                        var subContents = Directory.GetFiles(songsFolder + songName + "\\gen");
                        if (!subContents.Any()) continue;
                        foreach (var contents in subContents)
                        {
                            if (backgroundWorker1.CancellationPending) return false;
                            packfiles.AddFile(contents, "songs/" + songName + "/gen/" + Path.GetFileName(contents));
                        }
                    }
                    //C16 modification. if multiple songs.dta files are found, combine them
                    var c16Dta = Directory.GetFiles(songsFolder, "*.dta");
                    if (c16Dta.Count() > 1)
                    {
                        //prompt user that multiple dtas were found, and to choose whether to combine them (C16) or not
                        var dialogResult =
                            MessageBox.Show("Found multiple songs.dta files in the 'songs' folder\nDo you want me to merge (YES) or discard (NO) the duplicates?\nUnless you're C16 or you know what you're doing, I suggest you discard the duplicates (NO)",
                                "C16 Modification Activate!", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
                        switch (dialogResult)
                        {
                            case DialogResult.No:
                                foreach (var c16 in c16Dta.Where(c16 => c16.Substring(c16.Length - 9, 9) != "songs.dta"))
                                {
                                    Tools.DeleteFile(c16);
                                }
                                break;
                            case DialogResult.Yes:
                                {
                                    //create temporary dta file
                                    File.Create(songsFolder + "temp_songs.dta").Dispose();
                                   
                                    //process the duplicate dtas
                                    foreach (var c16 in c16Dta)
                                    {
                                        //read first dta file
                                        var dtaIn = new StreamReader(c16, Encoding.Default);
                                        var dtaPack = dtaIn.ReadToEnd();
                                        dtaIn.Dispose();

                                        //write to temp dta
                                        var dtaOut = new StreamWriter(songsFolder + "temp_songs.dta", true, Encoding.Default);
                                        dtaOut.Write(dtaPack);
                                        dtaOut.Dispose();

                                        //delete that dta file
                                        Tools.DeleteFile(c16);
                                    }

                                    //now rename that to songs.dta and just allow existing code to add it to the package
                                    Tools.MoveFile(songsFolder + "temp_songs.dta", songsFolder + "songs.dta");
                                }
                                break;
                        }
                    }

                    if (File.Exists(songsFolder + "songs.dta"))
                    {
                        packfiles.AddFile(songsFolder + "songs.dta", "songs/songs.dta");
                    }
                }
            }
            
            Log("Successfully added files for " + filesAdded + (filesAdded > 1 ? " songs" : " song"));
            if (filesAdded < inputFiles.Count())
            {
                Log("It looks like there were some duplicate songs");
                Log("I skipped: " + (inputFiles.Count() - filesAdded) + (inputFiles.Count() - filesAdded > 1? " songs" : " song"));
            }

            Tools.DeleteFolder(tempFolder + "temp\\", true);
            return true;
        }

        private bool buildPackage()
        {
            bool success;

            try
            {
                var signature = new RSAParams(Application.StartupPath + "\\bin\\KV.bin");
                var bigpack = new STFSPackage(packfiles, signature, xOut);
                var count1 = bigpack.xFileDirectory.Count;
                bigpack.CloseIO();

                var pack = new STFSPackage(xOut);
                var count2 = pack.xFileDirectory.Count;
                pack.CloseIO();

                if (count1 == count2)
                {
                    Log("Awesome ... looks like everything went smoothly");
                    success = true;
                }
                else
                {
                    MessageBox.Show("Sorry, it looks like there was a problem creating your pack\nThere should be a total of " +
                        count1 + " files but the pack is only reporting " + count2 + " files\n\nThis is caused by a nasty bug in the X360 library " +
                                    "we thought we had fixed!\n\nClick 'View Package' to open pack in CON Explorer and start checking each song's folder, " +
                                    "whenever you find the first song that has only a .mid file in its folder, that's likely the file causing the problem\nRemove that " +
                                    "file and try building the pack again\n\nDO NOT PUT THIS PACK IN YOUR GAME", Text + " - Big Bad Bug", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    sOpenPackage = xOut;
                    Log("Pack creation failed ... remove problem file and try again");
                    success = false;
                }
            }
            catch
            {
                Log("There was an error building CON file");
                success = false;
            }
            return success;
        }

        private void EnableDisable(bool enabled)
        {
            //load files in directory
            btnFolder.Enabled = enabled;
            btnRefresh.Enabled = enabled;
            txtTitle.Enabled = enabled;
            chkFTV.Enabled = enabled;
            chkID.Enabled = enabled;
            chkRename.Enabled = enabled;
            chkVolume.Enabled = enabled;
            menuStrip1.Enabled = enabled;
        }
        
        private void btnRefresh_Click(object sender, EventArgs e)
        {
            var tFolder = txtFolder.Text;
            txtFolder.Text = "";
            txtFolder.Text = tFolder;
        }
        
        private void HandleDragDrop(object sender, DragEventArgs e)
        {
            if (picWorking.Visible) return;

            var files = (string[])e.Data.GetData(DataFormats.FileDrop);
            txtFolder.Text = Path.GetDirectoryName(files[0]);
            Tools.CurrentFolder = txtFolder.Text;
        }
        
        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            sOpenPackage = "";

            try
            {
                var files = Directory.GetFiles(txtFolder.Text, ".", doRecursiveSearching ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly);
                if (files.Count() != 0)
                {
                    if (!extractRBFiles())
                    {
                        Log("There was an error extracting the files ... stopping here");
                        return;
                    }
                }
                else
                {
                    Log("No new files found and no existing directory found ... there's nothing to do");
                    backgroundWorker1.CancelAsync();
                    return;
                }
            }
            catch
            {
                Log("Error retrieving files to extract");
            }

            var success = false;
            if (backgroundWorker1.CancellationPending)
            {
                goto Finish;
            }

            //check if the CON/LIVE files were actually RB songs or not
            //if not, end
            var isRbSong = Directory.Exists(tempFolder + "songs\\");
            if (!isRbSong)
            {
                Log("I can't find a 'songs' folder in the extracted files");
                Log("Are you sure these are Rock Band songs?");
                Log("Check the files and try again");
                return;
            }

            Log("Beginning to add the files");

            packfiles.HeaderData.TitleID = 0x45410914;
            packfiles.HeaderData.Title_Package = "Rock Band 3";
            packfiles.HeaderData.SetLanguage(Languages.English);
            packfiles.HeaderData.Publisher = "";
            packfiles.STFSType = STFSType.Type0;
            packfiles.HeaderData.ThisType = PackageType.SavedGame;
            packfiles.HeaderData.Title_Display = txtTitle.Text;
            packfiles.HeaderData.Description = txtDesc.Text;
            packfiles.HeaderData.ContentImageBinary = Resources.RB3.ImageToBytes(ImageFormat.Png);
            packfiles.HeaderData.PackageImageBinary = Resources.RB3.ImageToBytes(ImageFormat.Png);
            packfiles.HeaderData.MakeAnonymous();

            songsFolder = tempFolder + "songs\\";
            success = addFiles();
            if (success)
            {
                Log("All set, going to try to build the pack for you");
                Log("This may take a while, depending on how many songs");
                Log("Unless you get an error or I crash, assume that I'm working!");

                success = buildPackage();
            }

            if (success)
            {
                Log("Trying to unlock CON file");
                if (Tools.UnlockCON(xOut))
                {
                    Log("Unlocked CON file successfully"); 
                }
                else
                {
                    Log("Error unlocking CON file");
                    success = false;
                }
            }

            if (success)
            {
                Log("Trying to sign CON file");
                if (Tools.SignCON(xOut))
                {
                    Log("CON file signed successfully");
                }
                else
                {
                    Log("Error signing CON file");
                    success = false;
                }
            }

            Finish:
            Log("Deleting temporary folder");
            if (Directory.Exists(tempFolder))
            {
                try
                {
                    Tools.SendtoTrash(tempFolder, true); //send files to recycle bin
                    Directory.CreateDirectory(tempFolder); //restore empty extracted folder (requested by C16)
                }
                catch
                {
                    Log("Error deleting temporary folder");
                }
            }
            
            if (success)
            {
                Log("Done!");
                endTime = DateTime.Now;
                var timeDiff = endTime - startTime;
                Log("Process took " + timeDiff.Minutes + (timeDiff.Minutes == 1 ? " minute" : " minutes") + " and " + (timeDiff.Minutes == 0 && timeDiff.Seconds == 0 ? "1 second" : timeDiff.Seconds + " seconds"));
                Log("You can click View Package to close this form");
                Log("and open the new pack in CON Explorer");
                Log("Click reset to start again and create a new pack,");
                Log("or just close me down and enjoy your new pack!");

                sOpenPackage = xOut;
            }
            else
            {
                Log("Something went wrong and your pack was NOT created, sorry!");
            }
        }

        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            picWorking.Visible = false;
            btnBegin.Enabled = true;
            toolTip1.SetToolTip(btnBegin, "Click to create pack");
            btnBegin.Text = "&Begin";
            btnReset.Visible = true;
            if (string.IsNullOrWhiteSpace(sOpenPackage)) return;
            btnBegin.Visible = false;
            btnView.Visible = true;
        }

        private void helpToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            var message = Tools.ReadHelpFile("vp");
            var help = new HelpForm(Text + " - Help", message);
            help.ShowDialog();
        }
        
        private void videoPrep_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (picWorking.Visible)
            {
                MessageBox.Show("Please wait until the current process finishes", Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                e.Cancel = true;
                return;
            }
            Tools.DeleteFolder(Application.StartupPath + "\\videoprep_input\\", true);
            Tools.DeleteFolder(Application.StartupPath + "\\videoprep_temp\\", true);
        }

        private void HandleDragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
                e.Effect = DragDropEffects.All;
        }

        private void txtTitle_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control & e.KeyCode == Keys.A)
                txtTitle.SelectAll();
        }

        private void txtTitle_DoubleClick(object sender, EventArgs e)
        {
            txtTitle.SelectAll();
        }

        private void chkFTV_CheckedChanged(object sender, EventArgs e)
        {
            txtTitle.Text = chkFTV.Checked ? txtTitle.Text.Replace("Video", "FtV") : txtTitle.Text.Replace("FtV", "Video");
        }

        private void exportLogFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Tools.ExportLog(Text, lstLog.Items);
        }

        private void videoPrep_Shown(object sender, EventArgs e)
        {
            Log("Welcome to " + Text);
            Log("Drag and drop the CON / LIVE file(s) to be packaged here");
            Log("Or click 'Change Input Folder' to select the files");
            Log("Ready to begin");
            

            txtFolder.Text = inputDir;
        }

        private void defaultTool_Click(object sender, EventArgs e)
        {
            chkRename.Text = "Change artist to 'VIDEO RECORDING' to group songs together";
            txtTitle.Text = "Video Recording Pack";
            chkFTV.Enabled = false;
            defaultTool.Checked = true;
            weeklyPreviewTool.Checked = false;
            txtDesc.Text = DescText;
        }

        private void WeeklyPreviewTool_Click(object sender, EventArgs e)
        {
            chkRename.Text = "Change artist to 'VIDEO PREVIEW' to group songs together";
            txtTitle.Text = "Video Preview Pack - Week of " + PackDate;
            chkFTV.Enabled = true;
            defaultTool.Checked = false;
            weeklyPreviewTool.Checked = true;
            txtDesc.Text = DescText;
        }

        private void btnBegin_Click(object sender, EventArgs e)
        {
            if (btnBegin.Text == "Cancel")
            {
                Log("User cancelled process...stopping as soon as possible");
                backgroundWorker1.CancelAsync();
                btnBegin.Enabled = false;
                return;
            }
            btnBegin.Text = "Cancel";
            toolTip1.SetToolTip(btnBegin, "Click to cancel pack creation");

            startTime = DateTime.Now;
            try
            {
                if (!(Directory.Exists(tempFolder)))
                {
                    Directory.CreateDirectory(tempFolder);
                }
                if (!(Directory.Exists(tempFolder + "songs\\")))
                {
                    Directory.CreateDirectory(tempFolder + "songs\\");
                }

                EnableDisable(false);
                Log("Where should I save the pack to?");

                //set default file name and folder, prompt user for file name
                var fileOutput = new SaveFileDialog();
                if (txtTitle.Text != "")
                {
                    var name = txtTitle.Text.Replace(" ", "");
                    name = name.Replace(":", "");
                    name = name.Replace(",", "");
                    name = name.Replace(">", "");
                    name = name.Replace("<", "");
                    name = name.Replace("?", "");
                    name = name.Replace("!", "");
                    name = name.Replace("/", "");
                    fileOutput.FileName = name;
                }
                else
                {
                    fileOutput.FileName = "VideoPack_";
                }

                fileOutput.InitialDirectory = txtFolder.Text;
                if (fileOutput.ShowDialog() == DialogResult.OK)
                {
                    xOut = fileOutput.FileName;
                    //start animation and send to background worker
                    backgroundWorker1.RunWorkerAsync();
                    picWorking.Visible = true;
                    Tools.CurrentFolder = Path.GetDirectoryName(xOut);
                }
                else
                {
                    EnableDisable(true);
                    Log("Process cancelled");
                }
            }
            catch (Exception ex)
            {
                Log("There was an error: " + ex.Message);
                EnableDisable(true);
            }
        }

        private void btnView_Click(object sender, EventArgs e)
        {
            var xExplorer = new CONExplorer(Color.FromArgb(34, 169, 31), Color.White);
            xExplorer.LoadCON(sOpenPackage);
            xExplorer.Show();
            Close();
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            lstLog.Items.Clear();
            txtTitle.Text = null;
            inputDir = Application.StartupPath + "\\videoprep_input\\";
            txtFolder.Text = "";
            txtFolder.Text = inputDir;
            tempFolder = Application.StartupPath + "\\videoprep_temp\\";
            inputFiles.Clear();
            btnBegin.Visible = true;
            btnView.Visible = false;
            btnReset.Visible = false;
            btnFolder.Enabled = true;
            btnRefresh.Enabled = true;
            txtTitle.Enabled = true;
            chkID.Checked = true;
            chkRename.Checked = true;
            chkVolume.Checked = true;
            chkFTV.Checked = false;

            Tools.DeleteFolder(inputDir, true);
            Tools.DeleteFolder(tempFolder, true);
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

        private void useRecursiveSearchingToolStripMenuItem_Click(object sender, EventArgs e)
        {
            doRecursiveSearching = useRecursiveSearching.Checked;
            btnRefresh_Click(sender, e);
        }
    }
}