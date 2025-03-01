using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Nautilus.Properties;
using Nautilus.x360;
using Microsoft.VisualBasic.FileIO;
using SearchOption = System.IO.SearchOption;
using NautilusFREE;

namespace Nautilus
{
    public partial class PackCreator : Form
    {
        private static string inputDir;
        private static List<string> inputFiles;
        private static int intFiles;
        private static string tempFolder;
        private static string tempThumbs;
        private static string sOpenPackage;
        private static string xOut;
        private Boolean continueSession;
        private readonly CreateSTFS packfiles = new CreateSTFS();
        private DateTime startTime;
        private DateTime endTime;
        private string contentImage;
        private readonly MainForm xMainForm;
        private readonly NemoTools Tools;
        private int CurrentThumbs;
        private static string[] Thumbnails;
        private Encoding fileEncoding;
        private string fileEncodingString;
        private static Color mMenuBackground;
        private nTools nautilus3;
        private bool doRecursiveSearching;
        private const int minWidth = 430;
        private readonly Color buttonBackColor;
        private readonly Color buttonTextColor;

        public PackCreator(MainForm xParent, Color ButtonBackColor, Color ButtonTextColor)
        {
            xMainForm = xParent;

            InitializeComponent();
            Width = minWidth;
            mMenuBackground = menuStrip1.BackColor;
            menuStrip1.Renderer = new DarkRenderer();
            nautilus3 = new nTools();

            picContent.AllowDrop = true;
            picPackage.AllowDrop = true;
            thumb1.AllowDrop = true;
            thumb2.AllowDrop = true;
            thumb3.AllowDrop = true;
            thumb4.AllowDrop = true;
            thumb5.AllowDrop = true;
            thumb6.AllowDrop = true;
            thumb7.AllowDrop = true;
            thumb8.AllowDrop = true;
            thumb9.AllowDrop = true;
            thumb10.AllowDrop = true;

            inputFiles = new List<string>();
            tempFolder = Application.StartupPath + "\\extracted\\";
            tempThumbs = tempFolder + "thumbs\\";
            Tools = new NemoTools();
            inputDir = Application.StartupPath + "\\input\\";
            CurrentThumbs = 10;

            fileEncoding = new UTF8Encoding(false);
            fileEncodingString = "utf8";

            intFiles = 0;
            toolTip1.SetToolTip(btnBegin, "Click to begin process");
            toolTip1.SetToolTip(btnFolder, "Click to select the input folder");
            toolTip1.SetToolTip(btnRefresh, "Click to refresh if the contents of the folder have changed");
            toolTip1.SetToolTip(txtFolder, "This is the working directory");
            toolTip1.SetToolTip(picContent, "Click here to select the Content Image (visible in here)");
            toolTip1.SetToolTip(picPackage, "Click here to select the Package Image (visible in the Xbox dashboard)");
            toolTip1.SetToolTip(txtTitle, "Enter a title for your pack (visible in the Xbox dashboard)");
            toolTip1.SetToolTip(txtDesc, "Enter a description for your pack (visible in here)");
            toolTip1.SetToolTip(label3, "Choose the format for your pack");
            toolTip1.SetToolTip(radioCON, "Click here for use with retail consoles");
            toolTip1.SetToolTip(radioLIVE, "Click here for use with modded consoles");
            toolTip1.SetToolTip(chkKeepFiles, "Click here to save the extracted files for later use");
            toolTip1.SetToolTip(lstLog, "This is the application log. Right click to export");
            
            if (!Directory.Exists(tempFolder))
            {
                Directory.CreateDirectory(tempFolder);
                //Directory.CreateDirectory(tempFolder + "songs\\");
                //Directory.CreateDirectory(tempFolder + "songs_upgrades");
                Directory.CreateDirectory(tempThumbs);
            }

            buttonBackColor = ButtonBackColor;
            buttonTextColor = ButtonTextColor;
            DoButtonColors();

            checkTempFiles();
            doRecursiveSearching = useRecursiveSearching.Checked;
        }

        private void DoButtonColors()
        {
            var formButtons = new List<Button> { btnFolder, btnReset, btnRefresh, btnShowHide, btnViewPackage, btnPrev, btnNext, btnBegin };
            foreach (var button in formButtons)
            {
                button.BackColor = buttonBackColor;
                button.ForeColor = buttonTextColor;
                button.FlatAppearance.MouseOverBackColor = button.BackColor == Color.Transparent ? Color.FromArgb(127, Color.AliceBlue.R, Color.AliceBlue.G, Color.AliceBlue.B) : Tools.LightenColor(button.BackColor);
            }
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
        
        private void ClearThumbnails()
        {
            var boxes = new List<PictureBox> { thumb1, thumb2, thumb3, thumb4, thumb5, thumb6, thumb7, thumb8, thumb9, thumb10 }; 
            
            foreach (var box in boxes)
            {
                /*try
                {
                    box.Image.Dispose();
                }
                catch (Exception)
                {}*/
                try
                {
                    box.Image = null;
                }
                catch (Exception)
                {}
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
            btnShowHide.Visible = false;

            if (!(Directory.Exists(inputDir)))
            {
                Directory.CreateDirectory(inputDir);
            }

            if (txtFolder.Text != "")
            {
                Log("");
                Log("Reading input directory ... hang on");
                Tools.DeleteFolder(tempThumbs, true);
                Directory.CreateDirectory(tempThumbs);

                ClearThumbnails();

                intFiles = 0;
                long byteCount = 0;
                string[] oldSongs = {};
                string[] oldUpgrades = {};

                try
                {
                    if (continueSession)
                    {
                        if (Directory.Exists(tempFolder + "songs\\"))
                        {
                            oldSongs = Directory.GetDirectories(tempFolder + "songs\\");

                            intFiles = intFiles + oldSongs.Count();

                            if (oldSongs.Any())
                            {
                                var oldFiles = Directory.GetFiles(tempFolder, ".", SearchOption.AllDirectories);

                                byteCount = oldFiles.Select(file => new FileInfo(file)).Aggregate(byteCount, (current, fileSize) => current + fileSize.Length);
                                Log("Found " + oldSongs.Count() + " previously-extracted CON " + (oldSongs.Count() > 1 ? "files" : "file"));
                            }
                        }

                        if (Directory.Exists(tempFolder + "songs_upgrades\\"))
                        {
                            oldUpgrades = Directory.GetFiles(tempFolder + "songs_upgrades\\","*.mid",SearchOption.AllDirectories);

                            intFiles = intFiles + oldUpgrades.Count();

                            if (oldUpgrades.Any())
                            {
                                Log("Found " + oldUpgrades.Count() + " previously-extracted pro upgrade " + (oldUpgrades.Count() > 1 ? "files" : "file"));
                            }
                        }
                    }
                    //load files in directory
                    var inFiles = Directory.GetFiles(txtFolder.Text, ".", doRecursiveSearching ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly).ToList();
                    foreach (var file in inFiles)
                    {
                        try
                        {
                            if (VariousFunctions.ReadFileType(file) != XboxFileType.STFS) continue;
                            intFiles++;
                            inputFiles.Add(file);
                            
                            var xPackage = new STFSPackage(file);
                            if (xPackage.ParseSuccess)
                            {
                                var thumbnail = tempThumbs + Path.GetFileName(file).Replace(" ", "") + ".png";
                                var img = xPackage.Header.PackageImage;
                                Tools.DeleteFile(thumbnail);
                                img.Save(thumbnail, ImageFormat.Png);
                                img.Dispose();
                            }
                            xPackage.CloseIO();

                            var fileSize = new FileInfo(file);
                            byteCount = byteCount + fileSize.Length;
                        }
                        catch (Exception ex)
                        {
                            Log("There was a problem accessing that file");
                            Log("The error says: " + ex.Message);
                        }
                    }
                    if (intFiles == 0)
                    {
                        if (continueSession)
                        {
                            Log("No new files found ... working with existing file structure only");
                            btnBegin.Visible = true;
                        }
                        else
                        {
                            Log("Did not find any CON files ... try a different directory");
                            Log("You can also drag and drop CON files here");
                            Log("Ready");
                            btnBegin.Visible = false;
                        }
                        btnRefresh.Visible = true;
                    }
                    else
                    {
                        var newfiles = intFiles - oldSongs.Count() - oldUpgrades.Count();
                        if (newfiles > 0)
                        {
                            Log("Found " + newfiles + " CON " + (newfiles > 1 ? "files" : "file"));

                            Thumbnails = Directory.GetFiles(tempThumbs, "*.png");
                            if (Thumbnails.Any())
                            {
                                if (Thumbnails.Count() > 10)
                                {
                                    btnNext.Enabled = true;
                                }
                                LoadThumbnails(10);
                                btnShowHide.Visible = true;
                            }
                        }

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
                btnViewPackage.Visible = false;
                btnRefresh.Visible = false;
            }
        }

        private bool extractRBFiles()
        {
            var counter = 0;
            var success = 0;
            foreach (var file in inputFiles.Where(File.Exists))
            {
                if (backgroundWorker1.CancellationPending) return false;
                try
                {
                    if (VariousFunctions.ReadFileType(file) != XboxFileType.STFS) continue;
                    try
                    {
                        counter++;
                        var xPackage = new STFSPackage(file);
                        if (!xPackage.ParseSuccess)
                        {
                            Log("Failed to extract '" + Path.GetFileName(file) + "'");
                            Log("Skipping this file");
                            continue;
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
                        if (!xPackage.ExtractPayload(temptempFile, true, false))
                        {
                            Log("Error extracting xPackage payload, skipping this file");
                            xPackage.CloseIO();
                            continue;
                        }
                        xPackage.CloseIO();
                        temptempFile = temptempFile + "root\\";
                        if (Directory.Exists(temptempFile + "songs\\"))
                        {
                            var subFolders = Directory.GetDirectories(temptempFile + "songs\\");
                            var tempFileName = subFolders[0].Substring((temptempFile + "songs\\").Length,subFolders[0].Length - (temptempFile + "songs\\").Length);
                            if (subFolders.Count() != 0)//upgrades won't have subdirectories, skip this step in that case
                            {
                                if (File.Exists(temptempFile + "songs\\songs.dta"))
                                {
                                    //move songs.dta to the song's folder for sorting later
                                    //allows to skip duplicates
                                    if (!Tools.MoveFile(temptempFile + "songs\\songs.dta", temptempFile + "songs\\" + tempFileName + "\\songs.dta"))
                                    {
                                        Log("Error moving songs.dta file to the extracted folder");
                                    }
                                }
                                foreach (var foldertoMove in subFolders)
                                {
                                    tempFileName = foldertoMove.Substring((temptempFile + "songs\\").Length,foldertoMove.Length - (temptempFile + "songs\\").Length);
                                    var folderpath = tempFolder + "songs\\";
                                    //let's make sure songs folder is there, if not, create it
                                    if (!(Directory.Exists(folderpath)))
                                    {
                                        Directory.CreateDirectory(folderpath);
                                    }
                                    //if this song already exists in the working directory, delete it
                                    //copy this one instead
                                    Tools.SendtoTrash(folderpath + tempFileName + "\\",true);
                                    try
                                    {
                                        if (Path.GetPathRoot(temptempFile) == Path.GetPathRoot(tempFolder))
                                        {
                                            Directory.Move(temptempFile + "songs\\" + tempFileName + "\\",folderpath + tempFileName + "\\");
                                        }
                                        else
                                        {
                                            FileSystem.CopyDirectory(temptempFile + "songs\\" + tempFileName + "\\",folderpath + tempFileName + "\\");
                                            Tools.DeleteFolder(temptempFile + "songs\\" + tempFileName + "\\", true);
                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                        try
                                        {
                                            FileSystem.CopyDirectory(temptempFile + "songs\\" + tempFileName + "\\",folderpath + tempFileName + "\\");
                                            Tools.DeleteFolder(temptempFile + "songs\\" + tempFileName + "\\", true);
                                        }
                                        catch (Exception)
                                        {
                                            Log("Error moving extracted contents for song file " + Path.GetDirectoryName(foldertoMove));
                                            Log("The error says: " + ex.Message);
                                            Log("This song won't end up in the pack, sorry");
                                        }
                                    }
                                }
                            }
                            Log("Extracting file " + counter + " of " + inputFiles.Count + " ('" + Path.GetFileName(file) + "')");
                            success++;
                        }
                        if (Directory.Exists(temptempFile + "songs_upgrades\\") && File.Exists(temptempFile + "songs_upgrades\\upgrades.dta"))
                        {
                            //how many midis in this pro upgrade?
                            var midis = Directory.GetFiles(temptempFile + "songs_upgrades\\", "*.mid");
                            //if more than one, this is a pack, load whole pack dta as is
                            if (midis.Count() > 1)
                            {
                                var dtaIn = new StreamReader(temptempFile + "songs_upgrades\\upgrades.dta", fileEncoding);
                                var dtaPack = dtaIn.ReadToEnd();
                                dtaIn.Dispose();
                                var upOut = new StreamWriter(tempFolder + "songs_upgrades\\upgrades.dta", true, fileEncoding);
                                upOut.Write(dtaPack);
                                upOut.WriteLine(""); //separate entries
                                upOut.Dispose();
                                //delete original dta file
                                Tools.DeleteFile(temptempFile + "songs_upgrades\\upgrades.dta");
                                //move all the midis
                                foreach (var midi in midis.Where(midi => !File.Exists(tempFolder + "songs_upgrades\\" + Path.GetFileName(midi))).Where(midi => !Tools.MoveFile(midi, tempFolder + "songs_upgrades\\" + Path.GetFileName(midi))))
                                {
                                    Log("Error moving MIDI file " + Path.GetFileName(midi));
                                }
                            }
                            //if only one midi, then do the workaround to avoid duplicates
                            else if (midis.Count() == 1)
                            {
                                //check if midi already exists
                                if (!File.Exists(tempFolder + "songs_upgrades\\" + Path.GetFileName(midis[0])))
                                {
                                    //if file doesn't exist, then move this midi there
                                    if (!Tools.MoveFile(midis[0], tempFolder + "songs_upgrades\\" + Path.GetFileName(midis[0])))
                                    {
                                        Log("Error moving MIDI file " + Path.GetFileName(midis[0]));
                                    }
                                    //this limits the addition of upgrades.dta to when we're also adding a new midi = not a duplicate
                                    var dtaIn = new StreamReader(temptempFile + "songs_upgrades\\upgrades.dta", fileEncoding);
                                    var dtaPack = dtaIn.ReadToEnd();
                                    dtaIn.Dispose();
                                    var upOut = new StreamWriter(tempFolder + "songs_upgrades\\upgrades.dta", true, fileEncoding);
                                    upOut.Write(dtaPack);
                                    upOut.Dispose();
                                    //delete original dta files
                                    Tools.DeleteFile(temptempFile + "songs_upgrades\\upgrades.dta");
                                }
                            }
                            Log("Extracting file " + counter + " of " + inputFiles.Count);
                            success++;
                        }
                        //move other root files but no spa.bin files to root directory
                        var rootFiles = Directory.GetFiles(temptempFile);
                        if (rootFiles.Count() != 0)
                        {
                            foreach (var rootFile in rootFiles.Where(rootFile => !rootFile.EndsWith("spa.bin", StringComparison.Ordinal)).Where(rootFile => !Tools.MoveFile(rootFile, tempFolder + Path.GetFileName(rootFile))))
                            {
                                Log("Error moving root file " + Path.GetFileName(rootFile));
                            }
                        }
                        //delete folder to get rid of useless files
                        Tools.DeleteFolder(temptempFile, true);
                    }
                    catch (Exception ex)
                    {
                        Log("There was an error processing file " + Path.GetFileName(file));
                        Log("The error says: " + ex.Message);
                    }
                }
                catch (Exception ex)
                {
                    Log("There was a problem extracting file " + Path.GetFileName(file));
                    Log("The error says: " + ex.Message);
                }
            }
            if (counter > 0)
            {
                Log("Successfully extracted " + success + " of " + counter + (counter == 1 ? " file" : " files"));
                
                var extracted = Directory.GetDirectories(tempFolder + "songs\\").Count();
                if (extracted > counter)
                {
                    Log("The " + counter + " input CON " + (counter == 1 ? "file" : "files") + " contained a total of " + extracted + " songs inside");
                }
            }
            else
            {
                Log("No files were extracted");
            }
            return success > 0;
        }

        private bool addFiles()
        {
            if (backgroundWorker1.CancellationPending) return false;

            var filesAdded = 0;
            var totalInput = 0;

            var songsFolder = tempFolder + "songs\\";
            var upgFolder = tempFolder + "songs_upgrades\\";
            var dtafile = songsFolder + "songs.dta";

            if (!continueSession)
            {
                Tools.DeleteFile(dtafile);
            }

            if (Directory.Exists(songsFolder))
            {
                totalInput = Directory.GetDirectories(songsFolder).Count();
            }
            if (Directory.Exists(upgFolder))
            {
                totalInput += Directory.GetDirectories(upgFolder).Count();
            }

            if (hidePackFromRB3.Checked)
            {
                using (var dtaOut = new StreamWriter(dtafile, false, fileEncoding))
                {
                    dtaOut.WriteLine("#ifndef kControllerRealGuitar");
                }
            }            

            if (Directory.Exists(songsFolder))
            {
                var songsInput = Directory.GetFiles(songsFolder);
                var enoughFolders = Directory.GetDirectories(songsFolder);
                if (enoughFolders.Any()) //only add if there's at least one folder in there
                {
                    packfiles.AddFolder("songs"); 
                    foreach (var sFile in songsInput.Where(sFile => Path.GetExtension(sFile) != ".dta"))
                    {
                        packfiles.AddFile(sFile, "songs/" + Path.GetFileName(sFile));
                    }

                    //create song folders and add contents to each
                    var subFolders = Directory.GetDirectories(songsFolder);
                    var sFolderLength = songsFolder;
                    foreach (var folder in subFolders)
                    {
                        if (backgroundWorker1.CancellationPending) return false;
                       
                        var songName = folder.Substring(sFolderLength.Length, folder.Length - sFolderLength.Length);
                        songName = songName.Replace("\\", "");

                        if (!Directory.Exists(songsFolder + songName)) continue;
                        //check if the file is a copy (i.e. same song) and discard if so
                        var ending = "";
                        if (songName.Length > 3)
                        {
                            ending = songName.Substring(songName.Length - 3, 3);
                        }
                        if (ending == "(1)" || ending == "(2)" || ending == "(3)" || ending == "(4)" || ending == "(5)") continue;
                        
                        filesAdded++;
                        Log("Adding files for song " + filesAdded + " ('" + songName + "')");

                        //add all items at the songfolder level (mostly .mid and .mogg files)
                        packfiles.AddFolder("songs/" + songName);
                        
                        //if songs.dta is there, add to main songs.dta and then delete
                        if (File.Exists(songsFolder + songName + "\\songs.dta"))
                        {
                            var dtaIn = new StreamReader(songsFolder + songName + "\\songs.dta", fileEncoding);
                           var dtaOut = new StreamWriter(dtafile, true, fileEncoding);
                                                                
                            while (dtaIn.Peek() >= 0)
                            {
                                var line = dtaIn.ReadLine();

                                if (string.IsNullOrWhiteSpace(line)) continue;
                                if (line.Trim().StartsWith(";;", StringComparison.Ordinal)) continue;
                                line = Tools.FixBadChars(line);
                                if (line.Contains("(encoding") || line.Contains("'encoding"))
                                {
                                    line = "   ('encoding' '" + fileEncodingString + "')";
                                }
                                dtaOut.WriteLine(line);
                            }
                            dtaIn.Dispose();
                            dtaOut.WriteLine(""); //separate entries
                            dtaOut.Dispose();

                            //delete original dta files
                            Tools.DeleteFile(songsFolder + songName + "\\songs.dta");
                        }

                        //add mid, mogg and other files found at the songname folder level
                        var songContents = Directory.GetFiles(songsFolder + songName);
                        if (songContents.Any())
                        {
                            foreach (var content in songContents)
                            {
                                if (backgroundWorker1.CancellationPending) return false;
                                if (Path.GetExtension(content) == ".mogg")
                                {
                                    nautilus3.WriteOutData(nautilus3.DeObfM(File.ReadAllBytes(content)), content);
                                }
                                packfiles.AddFile(content, "songs/" + songName + "/" + Path.GetFileName(content));
                            }
                        }

                        //add all items at the gen level (mostly png_xbox and milo_xbox files)
                        packfiles.AddFolder("songs/" + songName + "/gen");
                        
                        var subContents = Directory.GetFiles(songsFolder + songName + "\\gen");
                        if (!subContents.Any()) continue;
                        foreach (var content in subContents)
                        {
                            if (backgroundWorker1.CancellationPending) return false;

                            packfiles.AddFile(content, "songs/" + songName + "/gen/" + Path.GetFileName(content));
                        }
                    }

                    //C16 modification. if multiple songs.dta files are found, combine them
                    var c16Dta = Directory.GetFiles(songsFolder, "*.dta");
                    if (c16Dta.Count() > 1)
                    {
                        //prompt user that multiple dtas were found, and to choose whether to combine them (C16) or not
                        var dialogResult = MessageBox.Show("Found multiple songs.dta files in the 'songs' folder\nDo you want me to merge (YES) or discard (NO) the duplicates?\nUnless you're C16 or you know what you're doing, I suggest you discard the duplicates (NO)",
                                "C16 Modification Activate!", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
                        switch (dialogResult)
                        {
                            case DialogResult.No:
                                foreach (var c16 in c16Dta.Where(c16 => Path.GetFileNameWithoutExtension(c16) != "songs"))
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
                                        var dtaIn = new StreamReader(c16, fileEncoding);
                                        var dtaPack = dtaIn.ReadToEnd();
                                        dtaIn.Dispose();

                                        //write to temp dta
                                        var dtaOut = new StreamWriter(songsFolder + "temp_songs.dta", true, fileEncoding);
                                        dtaOut.Write(dtaPack);
                                        dtaOut.WriteLine(""); //separate entries
                                        dtaOut.Dispose();

                                        //delete that dta file
                                        Tools.DeleteFile(c16);
                                    }

                                    //now rename that to songs.dta and just allow existing code to add it to the package
                                    if (!Tools.MoveFile(songsFolder + "temp_songs.dta", songsFolder + "songs.dta"))
                                    {
                                        Log("Error renaming " + songsFolder + "temp_songs.dta");
                                    }
                                }
                                break;
                        }
                    }
                                        
                    if (hidePackFromRB3.Checked)
                    {
                        using (var dtaOut = new StreamWriter(dtafile, true, fileEncoding))
                        {
                            dtaOut.Write("#endif");
                        }
                    }                    

                    if (File.Exists(dtafile))
                    {
                        packfiles.AddFile(dtafile, "songs/songs.dta");
                    }
                    else if (File.Exists(songsFolder + "temp_songs.dta"))
                    {
                        packfiles.AddFile(songsFolder + "temp_songs.dta", "songs/songs.dta");
                    }
                }
            }
            
            if (Directory.Exists(upgFolder))
            {
                var checkUpg = Directory.GetFiles(upgFolder);
                if (checkUpg.Count() > 1)
                {
                    //C16 modification. if multiple songs.dta files are found, combine them
                    var c16Dta = Directory.GetFiles(upgFolder, "*.dta");
                    if (c16Dta.Count() > 1)
                    {
                        //prompt user that multiple dtas were found, and to choose whether to combine them (C16) or not
                        var dialogResult = MessageBox.Show("Found multiple upgrades.dta files in the 'songs_upgrades' folder\nDo you want me to merge (YES) or discard (NO) the duplicates?\nUnless you're C16 or you know what you're doing, I suggest you discard the duplicates (NO)",
                                "C16 Modification Activate!", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
                        switch (dialogResult)
                        {
                            case DialogResult.No:
                                foreach (var c16 in c16Dta.Where(c16 => Path.GetFileNameWithoutExtension(c16) != "upgrades"))
                                {
                                    Tools.DeleteFile(c16);
                                }
                                break;
                            case DialogResult.Yes:
                                {
                                    //process the duplicate dtas
                                    foreach (var c16 in c16Dta)
                                    {
                                        //read first dta file
                                        var dtaIn = new StreamReader(c16, fileEncoding);
                                        var dtaPack = dtaIn.ReadToEnd();
                                        dtaIn.Dispose();

                                        //write to temp dta
                                        var dtaOut = new StreamWriter(upgFolder + "temp_upgrades.dta", true, fileEncoding);
                                        dtaOut.Write(dtaPack);
                                        dtaOut.WriteLine(""); //separate entries
                                        dtaOut.Dispose();

                                        //delete that dta file
                                        Tools.DeleteFile(c16);
                                    }

                                    //now rename that to upgrades.dta and just allow existing code to add it to the package
                                   if (!Tools.MoveFile(upgFolder + "temp_upgrades.dta", upgFolder + "upgrades.dta"))
                                   {
                                       Log("Error renaming " + upgFolder + "temp_upgrades.dta");
                                   }
                                }
                                break;
                        }
                    }
                    
                    packfiles.AddFolder("songs_upgrades");
                    if (File.Exists(upgFolder + "upgrades.dta"))
                    {
                        packfiles.AddFile(upgFolder + "upgrades.dta", "songs_upgrades/upgrades.dta");
                    }
                    else if (File.Exists(upgFolder + "temp_upgrades.dta"))
                    {
                        packfiles.AddFile(upgFolder + "temp_upgrades.dta", "songs_upgrades/upgrades.dta");
                    }

                    var songsInput = Directory.GetFiles(upgFolder);
                    foreach (var file in from file in songsInput where (Path.GetExtension(file) == ".mid") select file) //let ending = file.Substring(file.Length - 7, 7) where ending != "(1).mid" && ending != "(2).mid" && ending != "(3).mid" && ending != "(4).mid" && ending != "(5).mid" select file)
                    {
                        filesAdded++;
                        Log("Adding files for song " + filesAdded + " ('" + Path.GetFileName(file) + "')");
                        packfiles.AddFile(file, "songs_upgrades/" + Path.GetFileName(file));
                    }
                }
            }
            Log("Successfully added files for " + filesAdded + (filesAdded > 1 ? " songs" : " song"));
            if (filesAdded >= totalInput) return true;
            Log("It looks like there were some duplicate songs");
            Log("I skipped: " + (totalInput - filesAdded) + (totalInput - filesAdded > 1 ? " songs" : " song"));
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
                        count1 + " files but the pack is only reporting " + count2 +" files\n\nThis is caused by a nasty bug in the X360 library " +
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
            btnFolder.Enabled = enabled;
            btnRefresh.Enabled = enabled;
            picPackage.Enabled = enabled;
            picContent.Enabled = enabled;
            txtDesc.Enabled = enabled;
            txtTitle.Enabled = enabled;
            radioLIVE.Enabled = enabled;
            radioCON.Enabled = enabled;
            chkKeepFiles.Enabled = enabled;
            menuStrip1.Enabled = enabled;
            btnShowHide.Visible = enabled;
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
                /*if (!(Directory.Exists(tempFolder + "songs_upgrades\\")))
                {
                    Directory.CreateDirectory(tempFolder + "songs_upgrades\\");
                }
                if (!(Directory.Exists(tempFolder + "songs\\")))
                {
                    Directory.CreateDirectory(tempFolder + "songs\\");
                }*/
                
                EnableDisable(false);

                Log("Where should I save the pack to?");
                //set default file name and folder, prompt user for file name
                var fileOutput = new SaveFileDialog();
                if (txtTitle.Text != "")
                {
                    fileOutput.FileName = Tools.CleanString(txtTitle.Text, true);
                    fileOutput.FileName = fileOutput.FileName.Replace(" ", "");
                }
                else
                {
                    fileOutput.FileName = "CustomPack_";
                }
                
                fileOutput.InitialDirectory = txtFolder.Text;
                if (fileOutput.ShowDialog() == DialogResult.OK)
                {
                    xOut = fileOutput.FileName;
                    Tools.CurrentFolder = Path.GetDirectoryName(xOut);
                    //start animation and send to background worker
                    picWorking.Visible = true;
                    backgroundWorker1.RunWorkerAsync();
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

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            var tFolder = txtFolder.Text;
            txtFolder.Text = "";
            txtFolder.Text = tFolder;
        }

        private void getImage(String file, PictureBox box)
        {
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
                    Log(box.Name.Replace("pic", "") + " Image changed successfully");
                    return;
                }
                
                var newimage = Application.StartupPath + "\\" + Path.GetFileNameWithoutExtension(contentImage) + ".png";
                Tools.ResizeImage(contentImage, 64, "png", newimage);
                    
                if (File.Exists(newimage))
                {
                    box.Image = Tools.NemoLoadImage(newimage);
                    Log(box.Name.Replace("pic", "") + " Image changed successfully");
                }
                else
                {
                    Log("Something went wrong, image not loaded");
                }
            }
            catch
            {
                Log("Error loading image ... try again");
            }
        }
        
        private void HandleDragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.Bitmap))
            {
                e.Effect = DragDropEffects.Move;
                return;
            }
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                e.Effect = DragDropEffects.All;
            }
        }

        private void HandleDragDrop(object sender, DragEventArgs e)
        {
            if (picWorking.Visible) return;
            try
            {
                var files = (string[])e.Data.GetData(DataFormats.FileDrop);
                if (files == null) return;
                txtFolder.Text = Path.GetDirectoryName(files[0]);
                Tools.CurrentFolder = Path.GetDirectoryName(files[0]);
            }
            catch (Exception ex)
            {
                Log("There was an error:");
                Log(ex.Message);
            }
        }

        private void btnViewPackage_Click(object sender, EventArgs e)
        {
            var xExplorer = new CONExplorer(Color.FromArgb(34, 169, 31), Color.White);
            xExplorer.LoadCON(sOpenPackage);
            xExplorer.Show();
            Dispose();
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
                        backgroundWorker1.CancelAsync();
                        btnBegin.Visible = true;
                        return;
                    }
                }
                else
                {
                    if (!continueSession)
                    {
                        Log("No new files found and no existing directory found ... there's nothing to do");
                        backgroundWorker1.CancelAsync();
                        btnBegin.Visible = true;
                        return;
                    }
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
            var isRbSong = Directory.Exists(tempFolder + "songs\\") || Directory.Exists(tempFolder + "songs_upgrades\\");
            if (!continueSession && !isRbSong)
            {
                Log("I can't find a 'songs' or a 'songs_upgrades' folder in the extracted files");
                Log("Are you sure these are Rock Band songs?");
                Log("Check the files and try again");
                backgroundWorker1.CancelAsync();
                btnBegin.Visible = true;
                return;
            }

            if (chkKeepFiles.Checked)
            {
                var moggs = Directory.GetFiles(tempFolder, "*.mogg", SearchOption.AllDirectories);
                foreach (var mogg in moggs)
                {
                    //nautilus3.WriteOutData(nautilus3.ObfM(File.ReadAllBytes(mogg)), mogg);
                }
            }

            Log("Beginning to add files to the pack");
            
            if (rockBandToolStripMenuItem.Checked)
            {
                packfiles.HeaderData.TitleID = 0x45410829;
                packfiles.HeaderData.Title_Package = "Rock Band";
            }
            else if (rockBand2ToolStripMenuItem.Checked)
            {
                packfiles.HeaderData.TitleID = 0x45410869;
                packfiles.HeaderData.Title_Package = "Rock Band 2";
            }
            else
            {
                packfiles.HeaderData.TitleID = 0x45410914;
                packfiles.HeaderData.Title_Package = "Rock Band 3";
            }
            packfiles.HeaderData.SetLanguage(Languages.English);
            packfiles.HeaderData.Publisher = "";
            packfiles.STFSType = STFSType.Type0;
            if (radioCON.Checked)
            {
                packfiles.HeaderData.ThisType = PackageType.SavedGame;
                packfiles.HeaderData.MakeAnonymous();
            }
            else
            {
                packfiles.HeaderData.ThisType = PackageType.MarketPlace;
            }
            packfiles.HeaderData.Title_Display = txtTitle.Text;
            packfiles.HeaderData.Description = txtDesc.Text;
            packfiles.HeaderData.ContentImageBinary = picContent.Image.ImageToBytes(ImageFormat.Png);
            packfiles.HeaderData.PackageImageBinary = picPackage.Image.ImageToBytes(ImageFormat.Png);

            success = addFiles();
            if (success)
            {
                Log("All set, going to try to build the pack for you");
                Log("This may take a while, depending on how many songs");
                Log("Unless you get an error or I crash, assume that I'm working!");
                
                Invoke(new MethodInvoker(() => Width = minWidth));
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
                //convert to CON if button checked, if not, leave as LIVE
                if (radioCON.Checked)
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
            }

            Finish:
            if (chkKeepFiles.Checked)
            {
                Log("Keeping extracted files for next time");
            }
            else
            {
                Log("Deleting temporary folder");
                if (Directory.Exists(tempFolder))
                {
                    try
                    {
                        Tools.SendtoTrash(tempFolder,true); //send files to recycle bin
                        Directory.CreateDirectory(tempFolder); //restore empty extracted folder (requested by C16)
                    }
                    catch
                    {
                        Log("Error deleting temporary folder");
                    }
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
            btnReset.Visible = true;
            btnBegin.Enabled = true;
            toolTip1.SetToolTip(btnBegin, "Click to create pack");
            btnBegin.Text = "&Begin";

            if (string.IsNullOrWhiteSpace(sOpenPackage)) return;
            btnBegin.Visible = false;
            btnViewPackage.Visible = true;
        }              

        private void helpToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            var message = Tools.ReadHelpFile("pc");
            var help = new HelpForm(Text + " - Help", message);
            help.ShowDialog();
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            ClearThumbnails();
            
            if (!chkKeepFiles.Checked)
            {
                Tools.DeleteFolder(tempFolder, true);
            }
            
            var newPackager = new PackCreator(xMainForm, buttonBackColor, buttonTextColor);
            xMainForm.activeForm = newPackager;
            newPackager.lstLog.Items.AddRange(lstLog.Items);
            newPackager.Log("Finished resetting ... ready");
            newPackager.Show();
            Dispose();
        }

        private void checkTempFiles()
        {
            //if temporary directory exists, previous session, ask what to do
            if ((Directory.Exists(tempFolder + "songs\\") && Directory.GetFiles(tempFolder + "songs\\").Any()) || (Directory.Exists(tempFolder + "songs_upgrades\\") && Directory.GetFiles(tempFolder + "songs_upgrades\\").Any()))
            {
                var dialogResult =
                    MessageBox.Show("Found previously extracted files in the temporary folder\nTo continue using these files, click YES\nTo delete these files and start from scratch, click NO",
                        "Old files found", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
                if (dialogResult != DialogResult.Yes)
                {
                    try
                    {
                        Tools.SendtoTrash(tempFolder,true); //send files to recycle bin
                        Directory.CreateDirectory(tempFolder); //restore empty extracted folders (requested by C16)
                        //Directory.CreateDirectory(tempFolder + "songs\\");
                        //Directory.CreateDirectory(tempFolder + "songs_upgrades");
                    }
                    catch
                    {
                        Log("Error deleting old files ... try manually");
                    }
                }
                else
                {
                    continueSession = true;
                    chkKeepFiles.Checked = true;
                    Log("Continuing previous session");
                    txtFolder.Text = ""; //trigger counting the files
                    txtFolder.Text = inputDir;
                    btnBegin.Visible = true;
                }
            }
            Show();
            Focus();    
        }

        private void sendImage(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Left || picWorking.Visible) return;

            try
            {
                var pictureFrom = ((PictureBox)(sender));
                pictureFrom.Image = ((PictureBox)(sender)).Image;

                if (pictureFrom.Image == null)
                {
                    return;
                }

                DoDragDrop(pictureFrom.Image, DragDropEffects.Move);
            }
            catch (Exception)
            {}
        }

        private void useExistingFolderStructureToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var ofd = new FolderPicker
            {
                Title = "***ADVANCED USE ONLY***\nSelect the 'ROOT' folder\n(i.e. contains 'songs' and 'songs_upgrades' folders)",
            };
            if (ofd.ShowDialog(IntPtr.Zero) != true) return;
            var oldFiles = ofd.ResultPath;

            if (oldFiles != "")
            {
                //this will make the log say it's adding songs to existing files, otherwise it's "" and just says adding files
                continueSession = true;
                chkKeepFiles.Checked = true;
                tempFolder = oldFiles + "\\";
                Log("Using existing folder structure");
                Log("THIS FEATURE IS ONLY FOR ADVANCED USERS!");
                Log("You can press 'Begin' now or add more files to the 'input' folder");
                btnBegin.Visible = true;
            }
            Tools.CurrentFolder = oldFiles;
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

        private void txtDesc_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control & e.KeyCode == Keys.A)
                txtDesc.SelectAll();
        }

        private void txtDesc_DoubleClick(object sender, EventArgs e)
        {
            txtDesc.SelectAll();
        }

        private void exportLogFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Tools.ExportLog(Text, lstLog.Items);
        }

        private void rockBandToolStripMenuItem_Click(object sender, EventArgs e)
        {
            rockBandToolStripMenuItem.Checked = true;
            rockBand2ToolStripMenuItem.Checked = false;
            rockBand3ToolStripMenuItem.Checked = false;

            picPackage.Image = Resources.RB1;
            picContent.Image = Resources.RB1;
        }

        private void rockBand2ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            rockBandToolStripMenuItem.Checked = false;
            rockBand2ToolStripMenuItem.Checked = true;
            rockBand3ToolStripMenuItem.Checked = false;

            picPackage.Image = Resources.RB2;
            picContent.Image = Resources.RB2;
        }

        private void rockBand3ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            rockBandToolStripMenuItem.Checked = false;
            rockBand2ToolStripMenuItem.Checked = false;
            rockBand3ToolStripMenuItem.Checked = true;

            picPackage.Image = Resources.RB3;
            picContent.Image = Resources.RB3;
        }

        private void SongPackager_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (picWorking.Visible)
            {
                MessageBox.Show("Please wait until the current process finishes", Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                e.Cancel = true;
                return;
            }
            //Tools.DeleteFolder(Application.StartupPath + "\\input\\");
            Tools.DeleteFolder(tempThumbs, true);
            Tools.DeleteFolder(tempFolder, !chkKeepFiles.Checked);
        }
        
        private void pictureBox2_DragDrop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.Bitmap))
            {
                picPackage.Image = (Bitmap)e.Data.GetData(DataFormats.Bitmap);
                Log("Package Image changed");
                return;
            }
            
            var files = (string[])e.Data.GetData(DataFormats.FileDrop);
            getImage(files[0], picPackage);
        }

        private void ImageDragDrop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.Bitmap))
            {
                ((PictureBox)(sender)).Image = (Bitmap)e.Data.GetData(DataFormats.Bitmap);
            }
        }

        private void pictureBox1_DragDrop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.Bitmap))
            {
                picContent.Image = (Bitmap)e.Data.GetData(DataFormats.Bitmap);
                Log("Content Image changed");
                return;
            }

            var files = (string[])e.Data.GetData(DataFormats.FileDrop);
            getImage(files[0], picContent);
        }

        private void btnShowHide_Click(object sender, EventArgs e)
        {
            if (btnShowHide.Text == "-->")
            {
                Width = 606;
                btnShowHide.Text = "<--";
            }
            else
            {
                Width = minWidth;
                btnShowHide.Text = "-->";
            }
        }

        private void LoadThumbnails(int thumbs)
        {
            var items = Thumbnails.Count();
            var counter = 9;
            
            try
            {
                if (!string.IsNullOrWhiteSpace(Thumbnails[thumbs - 10]))
                {
                    thumb1.Image = Tools.NemoLoadImage(Thumbnails[thumbs - 10]);
                }
                if ((thumbs - counter) >= items)
                {
                    return;
                }
                counter--;

                if (!string.IsNullOrWhiteSpace(Thumbnails[thumbs - 9]))
                {
                    thumb2.Image = Tools.NemoLoadImage(Thumbnails[thumbs - 9]);
                }
                if ((thumbs - counter) >= items)
                {
                    return;
                }
                counter--;

                if (!string.IsNullOrWhiteSpace(Thumbnails[thumbs - 8]))
                {
                    thumb3.Image = Tools.NemoLoadImage(Thumbnails[thumbs - 8]);
                }
                if ((thumbs - counter) >= items)
                {
                    return;
                }
                counter--;

                if (!string.IsNullOrWhiteSpace(Thumbnails[thumbs - 7]))
                {
                    thumb4.Image = Tools.NemoLoadImage(Thumbnails[thumbs - 7]);
                }
                if ((thumbs - counter) >= items)
                {
                    return;
                }
                counter--;

                if (!string.IsNullOrWhiteSpace(Thumbnails[thumbs - 6]))
                {
                    thumb5.Image = Tools.NemoLoadImage(Thumbnails[thumbs - 6]);
                }
                if ((thumbs - counter) >= items)
                {
                    return;
                }
                counter--;

                if (!string.IsNullOrWhiteSpace(Thumbnails[thumbs - 5]))
                {
                    thumb6.Image = Tools.NemoLoadImage(Thumbnails[thumbs - 5]);
                }
                if ((thumbs - counter) >= items)
                {
                    return;
                }
                counter--;

                if (!string.IsNullOrWhiteSpace(Thumbnails[thumbs - 4]))
                {
                    thumb7.Image = Tools.NemoLoadImage(Thumbnails[thumbs - 4]);
                }
                if ((thumbs - counter) >= items)
                {
                    return;
                }
                counter--;

                if (!string.IsNullOrWhiteSpace(Thumbnails[thumbs - 3]))
                {
                    thumb8.Image = Tools.NemoLoadImage(Thumbnails[thumbs - 3]);
                }
                if ((thumbs - counter) >= items)
                {
                    return;
                }
                counter--;

                if (!string.IsNullOrWhiteSpace(Thumbnails[thumbs - 2]))
                {
                    thumb9.Image = Tools.NemoLoadImage(Thumbnails[thumbs - 2]);
                }
                if ((thumbs - counter) >= items)
                {
                    return;
                }
                if (!string.IsNullOrWhiteSpace(Thumbnails[thumbs - 1]))
                {
                    thumb10.Image = Tools.NemoLoadImage(Thumbnails[thumbs - 1]);
                }
            }
            catch (Exception ex)
            {
                Log("Error displaying the thumbnails");
                Log("Error says: " + ex.Message);
            }
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            switch (CurrentThumbs)
            {
                case 10:
                    CurrentThumbs = 20;
                    break;
                case 20:
                    CurrentThumbs = 30;
                    break;
                case 30:
                    CurrentThumbs = 40;
                    break;
                case 40:
                    CurrentThumbs = 50;
                    break;
                case 50:
                    CurrentThumbs = 60;
                    break;
                case 60:
                    CurrentThumbs = 70;
                    break;
                case 70:
                    CurrentThumbs = 80;
                    break;
                case 80:
                    CurrentThumbs = 90;
                    break;
                case 90:
                    CurrentThumbs = 100;
                    break;
            }

            ClearThumbnails();
            LoadThumbnails(CurrentThumbs);

            if (CurrentThumbs > Thumbnails.Count())
            {
                btnNext.Enabled = false;
            }
            if (CurrentThumbs != 10)
            {
                btnPrev.Enabled = true;
            }
        }

        private void btnPrev_Click(object sender, EventArgs e)
        {
            switch (CurrentThumbs)
            {
                case 100:
                    CurrentThumbs = 90;
                    break;
                case 90:
                    CurrentThumbs = 80;
                    break;
                case 80:
                    CurrentThumbs = 70;
                    break;
                case 70:
                    CurrentThumbs = 60;
                    break;
                case 60:
                    CurrentThumbs = 50;
                    break;
                case 50:
                    CurrentThumbs = 40;
                    break;
                case 40:
                    CurrentThumbs = 30;
                    break;
                case 30:
                    CurrentThumbs = 20;
                    break;
                case 20:
                    CurrentThumbs = 10;
                    break;
            }

            ClearThumbnails();
            LoadThumbnails(CurrentThumbs);

            if (CurrentThumbs == 10)
            {
                btnPrev.Enabled = false;
            }
            if (CurrentThumbs < Thumbnails.Count())
            {
                btnNext.Enabled = true;
            }
        }

        private void SongPackager_Shown(object sender, EventArgs e)
        {
            Log("Welcome to " + Text);
            Log("Drag and drop the CON / LIVE file(s) to be packaged here");
            Log("Or click 'Change Input Folder' to select the files");
            Log("Ready to begin");
            
            txtFolder.Text = inputDir;
        }

        private void picPackage_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                getImage("", picPackage);
            }
        }

        private void picContent_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                getImage("", picContent);
            }
        }

        private void aNSIToolStripMenuItem_Click(object sender, EventArgs e)
        {
            aNSIToolStripMenuItem.Checked = true;
            uTF8ToolStripMenuItem.Checked = false;

            fileEncoding = Encoding.GetEncoding(28591);
            fileEncodingString = "latin1";
        }

        private void uTF8ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            aNSIToolStripMenuItem.Checked = false;
            uTF8ToolStripMenuItem.Checked = true;

            fileEncoding = new UTF8Encoding(false);
            fileEncodingString = "utf8";
        }

        private sealed class DarkRenderer : ToolStripProfessionalRenderer
        {
            public DarkRenderer() : base(new DarkColors()) { }
        }

        private sealed class DarkColors : ProfessionalColorTable
        {
            public override Color ImageMarginGradientBegin
            {
                get { return mMenuBackground; }
            }
            public override Color ImageMarginGradientEnd
            {
                get { return mMenuBackground; }
            }
            public override Color ImageMarginGradientMiddle
            {
                get { return mMenuBackground; }
            }
            public override Color ToolStripDropDownBackground
            {
                get { return mMenuBackground; }
            }
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

        private void useRecursiveSearching_Click(object sender, EventArgs e)
        {
            doRecursiveSearching = useRecursiveSearching.Checked;
            btnRefresh_Click(sender, e);
        }

        private void btnRB1_Click(object sender, EventArgs e)
        {
            rockBandToolStripMenuItem_Click(sender, e);
        }

        private void btnRB2_Click(object sender, EventArgs e)
        {
            rockBand2ToolStripMenuItem_Click(sender, e);
        }

        private void btnRB3_Click(object sender, EventArgs e)
        {
            rockBand3ToolStripMenuItem_Click(sender, e);
        }
    }
}