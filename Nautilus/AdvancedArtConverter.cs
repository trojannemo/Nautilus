using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using System.Drawing;
using Nautilus.Properties;
using Nautilus.Texture;

namespace Nautilus
{
    public partial class AdvancedArtConverter : Form
    {
        private List<string> imagesXbox;
        private List<string> imagesBMP;
        private List<string> imagesJPG;
        private List<string> imagesPNG;
        private List<string> imagesWii;
        private List<string> imagesTiff;
        private List<string> imagesDDS;
        private List<string> imagesGIF;
        private List<string> imagesPS3;
        private List<string> imagesPS4;
        private List<string> imagesTGA;
        private List<string> imagesTPL;
        private List<string> miloXbox;
        private List<string> miloPS3;
        private List<string> miloWii;
        private static int imageCount;
        private static int imgCounter;
        private readonly string wimgt;
        private readonly NemoTools Tools;
        private readonly string config;
        
        public AdvancedArtConverter(string Folder, Color ButtonBackColor, Color ButtonTextColor)
        {
            InitializeComponent();
            
            Tools = new NemoTools {TextureSize = 512};

            var formButtons = new List<Button> { btnRefresh, btnFolder, btnToPS3,btnFromPS3,btnToWii,btnFromWii,btnToXbox, btnFromXbox, btnFromPS4, btnToPS4 };
            foreach (var button in formButtons)
            {
                button.BackColor = ButtonBackColor;
                button.ForeColor = ButtonTextColor;
                button.FlatAppearance.MouseOverBackColor = button.BackColor == Color.Transparent ? Color.FromArgb(127, Color.AliceBlue.R, Color.AliceBlue.G, Color.AliceBlue.B) : Tools.LightenColor(button.BackColor);
            }

            Log("Welcome to Advanced Art Converter");
            Log("Drag and drop the file(s) to be converted here");
            Log("Or click 'Change Input Folder' to select the files");
            Log("Ready to begin");

            //folder sent via command line is more important
            var pngDir = Folder != "" ? Folder : "";

            //load last used directory if saved and still exists
            config = Application.StartupPath + "\\bin\\config\\art.config";
            if (string.IsNullOrWhiteSpace(pngDir))
            {
                if (File.Exists(config))
                {
                    var sr = new StreamReader(config);
                    var line = sr.ReadLine();
                    sr.Dispose();
                    if (string.IsNullOrWhiteSpace(line))
                    {
                        Tools.DeleteFile(config);
                    }
                    else if (line != "" && Directory.Exists(line))
                    {
                        Log("Loaded last directory used");
                        pngDir = line;
                    }
                    else
                    {
                        Tools.DeleteFile(config);
                    }
                }
            }
            txtFolder.Text = pngDir;
            imgCounter = 0;
            
            wimgt = Application.StartupPath + "\\bin\\wimgt.exe";
            if (File.Exists(wimgt)) return;
            MessageBox.Show("Can't find wimgt.exe ... I won't be able to convert Wii files without it",
                            "Missing Executable", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            tabWii.Enabled = false;
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
            if (string.IsNullOrWhiteSpace(txtFolder.Text))
            {
                btnRefresh.Visible = false;
                return;
            }
            btnRefresh.Visible = true;

            try
            {
                //check if any png_xbox files exist in the directory
                imagesXbox = Directory.GetFiles(txtFolder.Text, "*.png_xbox").ToList();
                imagesPS3 = Directory.GetFiles(txtFolder.Text, "*.png_ps3").ToList();
                imagesPS4 = Directory.GetFiles(txtFolder.Text, "*.png_ps4").ToList();
                imagesWii = Directory.GetFiles(txtFolder.Text, "*.png_wii").ToList();
                imagesPNG = Directory.GetFiles(txtFolder.Text, "*.png").ToList();
                imagesBMP = Directory.GetFiles(txtFolder.Text, "*.bmp").ToList();
                imagesJPG = Directory.GetFiles(txtFolder.Text, "*.jpg").ToList();
                imagesGIF = Directory.GetFiles(txtFolder.Text, "*.gif").ToList();
                imagesTiff = Directory.GetFiles(txtFolder.Text, "*.tif").ToList();
                imagesDDS = Directory.GetFiles(txtFolder.Text, "*.dds").ToList();
                imagesTGA = Directory.GetFiles(txtFolder.Text, "*.tga").ToList();
                imagesTPL = Directory.GetFiles(txtFolder.Text, "*.tpl").ToList();
                miloXbox = Directory.GetFiles(txtFolder.Text, "*.milo_xbox").ToList();
                miloPS3 = Directory.GetFiles(txtFolder.Text, "*.milo_ps3").ToList();
                miloWii = Directory.GetFiles(txtFolder.Text, "*.milo_wii").ToList();

                //remove png_xbox and png_wii images from the list of png images
                var to_remove = new List<int>();
                for (var i = 0; i < imagesPNG.Count; i++)
                {
                    if (Path.GetExtension(imagesPNG[i]) != ".png")
                    {
                        to_remove.Add(i);
                    }
                }
                to_remove.Sort();
                to_remove.Reverse();
                for (var i = to_remove.Count; i > 0; i--)
                {
                    imagesPNG.RemoveAt(i - 1);
                }

                imageCount = imagesBMP.Count() + imagesJPG.Count() + imagesPNG.Count() + imagesGIF.Count() + imagesTiff.Count() + imagesTGA.Count();

                if (!imagesXbox.Any() && !imagesDDS.Any() && imageCount == 0 && !imagesWii.Any() && !imagesTPL.Any() && !imagesPS3.Any() && !imagesPS4.Any() && !miloXbox.Any() && !miloPS3.Any() && !miloWii.Any())
                {
                    Log("No supported image files found");
                    Log("Choose a different folder");
                    Log("You can also drag-drop images here");

                    if (!miloXbox.Any() && !miloPS3.Any() && !miloWii.Any())
                    {
                        btnFromWii.Enabled = false;
                        btnToWii.Enabled = false;
                        btnFromXbox.Enabled = false;
                        btnToXbox.Enabled = false;
                        btnToPS3.Enabled = false;
                        btnFromPS3.Enabled = false;
                        btnFromPS4.Enabled = false;
                        btnToPS4.Enabled = false;
                    }
                }
                else
                {
                    btnFromWii.Enabled = true;
                    btnToWii.Enabled = true;
                    btnFromXbox.Enabled = true;
                    btnToXbox.Enabled = true;
                    btnFromPS3.Enabled = true;
                    btnToPS3.Enabled = true;
                    btnFromPS4.Enabled = true;
                    btnToPS4.Enabled = true;

                    if (imagesXbox.Any())
                    {
                        Log("Found " + imagesXbox.Count() + " .png_xbox " + (imagesXbox.Count() > 1? "files" : "file"));
                    }
                    if (imagesPS3.Any())
                    {
                        Log("Found " + imagesPS3.Count() + " .png_ps3 " + (imagesPS3.Count() > 1 ? "files" : "file"));
                    }
                    if (imagesPS4.Any())
                    {
                        Log("Found " + imagesPS4.Count() + " .png_ps4 " + (imagesPS4.Count() > 1 ? "files" : "file"));
                    }
                    if (imagesWii.Any())
                    {
                        Log("Found " + imagesWii.Count() + " .png_wii " + (imagesWii.Count() > 1 ? "files" : "file"));
                    }
                    if (imagesDDS.Any())
                    {
                        Log("Found " + imagesDDS.Count() + " .dds " + (imagesDDS.Count() > 1 ? "files" : "file"));
                    }
                    if (imagesTPL.Any())
                    {
                        Log("Found " + imagesTPL.Count() + " .tpl " + (imagesTPL.Count() > 1 ? "files" : "file"));
                    }
                    if (imagesPNG.Any())
                    {
                        Log("Found " + imagesPNG.Count() + " .png " + (imagesPNG.Count() > 1 ? "files" : "file"));
                    }
                    if (imagesBMP.Any())
                    {
                        Log("Found " + imagesBMP.Count() + " .bmp " + (imagesBMP.Count() > 1 ? "files" : "file"));
                    }
                    if (imagesTiff.Any())
                    {
                        Log("Found " + imagesTiff.Count() + " .tif " + (imagesTiff.Count() > 1 ? "files" : "file"));
                    }
                    if (imagesTGA.Any())
                    {
                        Log("Found " + imagesTGA.Count() + " .tga " + (imagesTGA.Count() > 1 ? "files" : "file"));
                    }
                    if (imagesJPG.Any())
                    {
                        Log("Found " + imagesJPG.Count() + " .jpg " + (imagesJPG.Count() > 1 ? "files" : "file"));
                    }
                    if (imagesGIF.Any())
                    {
                        Log("Found " + imagesGIF.Count() + " .gif " + (imagesGIF.Count() > 1 ? "files" : "file"));
                    }
                    if (miloXbox.Any())
                    {
                        Log("Found " + miloXbox.Count() + " .milo_xbox " + (miloXbox.Count() == 1 ? "file" : "files"));
                    }
                    if (miloPS3.Any())
                    {
                        Log("Found " + miloPS3.Count() + " .milo_ps3 " + (miloPS3.Count() == 1 ? "file" : "files"));
                    }
                    if (miloWii.Any())
                    {
                        Log("Found " + miloWii.Count() + " .milo_wii " + (miloWii.Count() == 1 ? "file" : "files"));
                    }
                    Log("Ready to begin");
                }

                if (imageCount == 0)
                {
                    if (!imagesTPL.Any())
                    {
                        btnToWii.Enabled = false;
                    }
                    if (!imagesDDS.Any() && !imagesPS3.Any())
                    {
                        btnToXbox.Enabled = false;
                    }
                    if (!imagesDDS.Any() && !imagesXbox.Any())
                    {
                        btnToPS3.Enabled = false;
                    }
                    if (!imagesXbox.Any() && !imagesDDS.Any() && (imagesWii.Any() || imagesTPL.Any()))
                    {
                        tabControl1.SelectTab(1); //switch to Wii tab
                    }
                    else if (!imagesXbox.Any() && !imagesDDS.Any() && !imagesWii.Any() && !imagesTPL.Any() && imagesPS3.Any())
                    {
                        tabControl1.SelectTab(2); //switch to PS3 tab
                    }
                    if (imagesPS4.Any())
                    {
                        tabControl1.SelectTab(3); //switch to PS4 tab
                    }
                }
                if (!imagesXbox.Any() && !imagesDDS.Any())
                {
                    btnFromXbox.Enabled = false;
                }
                if (!imagesWii.Any() && !imagesTPL.Any())
                {
                    btnFromWii.Enabled = false;
                }
                if (!imagesPS3.Any() && !imagesDDS.Any())
                {
                    btnFromPS3.Enabled = false;
                }
                if (!imagesPS4.Any())
                {
                    btnFromPS4.Enabled = false;
                }

                if (miloXbox.Any())
                {
                    btnFromXbox.Enabled = true;
                }
                if (miloPS3.Any())
                {
                    btnFromPS3.Enabled = true;
                }
                if (miloWii.Any())
                {
                    btnFromWii.Enabled = true;
                }
            }
            catch (Exception ex)
            {
                Log("There was an error:");
                Log(ex.Message);
            }
        }

        private void btnFolder_Click(object sender, EventArgs e)
        {
            //if user selects new folder, assign that value
            //if user cancels or selects same folder, this forces the text_changed event to run again
            var tempFolder = txtFolder.Text;
            var ofd = new FolderPicker
            {
                InputPath = txtFolder.Text,
                Title = "Select folder where your source images are",
            };            
            txtFolder.Text = "";            
            if (ofd.ShowDialog(IntPtr.Zero) == true)
            {
                txtFolder.Text = ofd.ResultPath;
                Tools.CurrentFolder = txtFolder.Text;
            }
            else
            {
                txtFolder.Text = tempFolder;
            }
        }

        private void FromWii(string image)
        {
            if (!File.Exists(image)) return;

            imgCounter++;
            Log("Converting file " + imgCounter + " of " + (imagesWii.Count() + imagesTPL.Count()));
            Log(" - " + Path.GetFileName(image));
            var format = Path.GetExtension(image).Replace(".", "").Trim().ToLowerInvariant();

            var success = false;
            // Save the image
            if (chkWiiBMP.Checked)
            {
                success = Tools.ConvertWiiImage(image, image, "bmp", false);
            }
            if (chkWiiJPG.Checked)
            {
                success = Tools.ConvertWiiImage(image, image, "jpg", false);
            }
            if (chkWiiPNG.Checked)
            {
                success = Tools.ConvertWiiImage(image, image, "png", false);
            }

            if (success)
            {
                //if clean up box checked, delete originals
                if (chkCleanUpWii.Checked)
                {
                    Tools.SendtoTrash(image);
                }
                Log("Converted from " + format + " format successfully");
            }
            else
            {
                Log("Error converting from " + format + " format");
            }
        }
        
        private void FromPS3(string image)
        {
            if (!File.Exists(image)) return;

            imgCounter++;
            Log("Converting file " + imgCounter + " of " + imagesPS3.Count());
            Log(" - " + Path.GetFileName(image));
            
            var success = false;
            // Save the image
            if (chkPS3BMP.Checked)
            {
                success = Tools.ConvertRBImage(image, image, "bmp");
            }
            if (chkPS3PNG.Checked)
            {
                success = Tools.ConvertRBImage(image, image, "png");
            }
            if (chkPS3JPG.Checked)
            {
                success = Tools.ConvertRBImage(image, image, "jpg");
            }

            if (success)
            {
                //if clean up box checked, delete originals
                if (chkCleanUpPS3.Checked)
                {
                    Tools.SendtoTrash(image);
                }
                Log("Converted from png_ps3 format successfully");
            }
            else
            {
                Log("Error converting from png_ps3 format");
            }
        }

        private void FromXbox(string image)
        {
            if (!File.Exists(image)) return;

            imgCounter++;
            Log("Converting file " + imgCounter + " of " + (imagesXbox.Count() + imagesDDS.Count()));
            Log(" - " + Path.GetFileName(image));
            var format = Path.GetExtension(image).Replace(".", "").Trim().ToLowerInvariant();

            var success = false;
            // Save the image
            if (chkXboxBMP.Checked)
            {
                success = Tools.ConvertRBImage(image, image, "bmp");
            }
            if (chkXboxPNG.Checked)
            {
                success = Tools.ConvertRBImage(image, image, "png");
            }
            if (chkXboxJPG.Checked)
            {
                success = Tools.ConvertRBImage(image, image, "jpg");
            }
            
            if (success)
            {
                //if clean up box checked, delete originals
                if (chkCleanUpXbox.Checked)
                {
                    Tools.SendtoTrash(image);
                }
                Log("Converted from " + format + " format successfully");
            }
            else
            {
                Log("Error converting from " + format + " format");
            }
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            var tempFolder = txtFolder.Text;
            txtFolder.Text = "";
            txtFolder.Text = tempFolder;
        }
        
        private void HandleDragDrop(object sender, DragEventArgs e)
        {
            var files = (string[]) e.Data.GetData(DataFormats.FileDrop);
            
            txtFolder.Text = ""; //force "changed text"
            txtFolder.Text = Path.GetDirectoryName(files[0]);
            Tools.CurrentFolder = txtFolder.Text;
        }
        
        private void HandleDragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
                e.Effect = DragDropEffects.All;
        }
        
        private void helpToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            var message = Tools.ReadHelpFile("aac");
            var help = new HelpForm(Text + " - Help", message);
            help.ShowDialog();
        }

        private void AlbumConvert_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (picWorking.Visible)
            {
                MessageBox.Show("Please wait until the current process finishes", Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                e.Cancel = true;
                return;
            }

            Tools.TextureSize = 512; //reset it just in case

            //save current directory for next time
            var sw = new StreamWriter(config, false);
            sw.WriteLine(txtFolder.Text);
            sw.Dispose();
        }

        private void exportLogFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Tools.ExportLog(Text, lstLog.Items);
        }
        
        private void btnToWii_Click(object sender, EventArgs e)
        {
            picWorking.Visible = true;
            try
            {
                imgCounter = 0;
                foreach (var image in imagesBMP)
                {
                    ToWii(image);
                }
                foreach (var image in imagesJPG)
                {
                    ToWii(image);
                }
                foreach (var image in imagesGIF)
                {
                    ToWii(image);
                }
                foreach (var image in imagesTiff)
                {
                    ToWii(image);
                }
                foreach (var image in imagesTGA)
                {
                    ToWii(image);
                }
                foreach (var image in imagesTPL)
                {
                    ToWii(image);
                }
                foreach (var image in imagesPNG.Where(image => !image.EndsWith(".png_wii", StringComparison.Ordinal)))
                {
                    ToWii(image);
                }
                Log("Done");
                picWorking.Visible = false;
                btnRefresh.PerformClick();
            }
            catch (Exception ex)
            {
                Log("There was an error:");
                Log(ex.Message);
                picWorking.Visible = false;
            }
        }

        private void btnToXbox_Click(object sender, EventArgs e)
        {
            picWorking.Visible = true;
            try
            {
                imgCounter = 0;
                foreach (var image in imagesBMP)
                {
                    ToXbox(image);
                }
                foreach (var image in imagesJPG)
                {
                    ToXbox(image);
                }
                foreach (var image in imagesGIF)
                {
                    ToXbox(image);
                }
                foreach (var image in imagesTiff)
                {
                    ToXbox(image);
                }
                foreach (var image in imagesTGA)
                {
                    ToXbox(image);
                }
                foreach (var image in imagesDDS)
                {
                    ToXbox(image);
                }
                foreach (var image in imagesPNG.Where(image => !image.EndsWith(".png_wii", StringComparison.Ordinal) && !image.EndsWith(".png_xbox", StringComparison.Ordinal) && !image.EndsWith(".png_ps3", StringComparison.Ordinal)))
                {
                    ToXbox(image);
                }
                foreach (var image in imagesPS3)
                {
                    ToXbox(image, true);
                }
                Log("Done");
                picWorking.Visible = false;
                btnRefresh.PerformClick();
            }
            catch (Exception ex)
            {
                Log("There was an error:");
                Log(ex.Message);
                picWorking.Visible = false;
            }
        }

        private void btnToPS3_Click(object sender, EventArgs e)
        {
            picWorking.Visible = true;
            try
            {
                imgCounter = 0;
                foreach (var image in imagesBMP)
                {
                    ToPS3(image);
                }
                foreach (var image in imagesJPG)
                {
                    ToPS3(image);
                }
                foreach (var image in imagesGIF)
                {
                    ToPS3(image);
                }
                foreach (var image in imagesTiff)
                {
                    ToPS3(image);
                }
                foreach (var image in imagesTGA)
                {
                    ToPS3(image);
                }
                foreach (var image in imagesDDS)
                {
                    ToPS3(image);
                }
                foreach (var image in imagesPNG.Where(image => !image.EndsWith(".png_wii", StringComparison.Ordinal) && !image.EndsWith(".png_xbox", StringComparison.Ordinal) && !image.EndsWith(".png_ps3", StringComparison.Ordinal)))
                {
                    ToPS3(image);
                }
                foreach (var image in imagesXbox)
                {
                    ToPS3(image, true);
                }
                Log("Done");
                picWorking.Visible = false;
                btnRefresh.PerformClick();
            }
            catch (Exception ex)
            {
                Log("There was an error:");
                Log(ex.Message);
                picWorking.Visible = false;
            }
        }

        private void btnFromPS3_Click(object sender, EventArgs e)
        {
            if ((!(chkPS3BMP.Checked)) && (!(chkPS3PNG.Checked)) && (!(chkPS3JPG.Checked)))
            {
                MessageBox.Show("Please select at least one output format", Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            picWorking.Visible = true;
            imgCounter = 0;
            foreach (var image in imagesPS3)
            {
                FromPS3(image);
            }
            foreach (var image in imagesDDS)
            {
                FromPS3(image);
            }
            foreach (var milo in miloPS3)
            {
                if (!File.Exists(milo)) continue;

                var format = "png";
                if (chkPS3BMP.Checked)
                {
                    format = "bmp";
                }
                else if (chkPS3JPG.Checked)
                {
                    format = "jpg";
                }

                Log("Searching .milo_ps3 file for textures");
                Log(" - " + Path.GetFileName(milo));
                try
                {
                    int count;
                    PikminMiloTools.ExtractTextures(milo, keepDDSFiles.Checked, false, format, out count);
                    
                    Log(count > 0 ? "Extracted " + count + " " + (count == 1 ? "texture" : "textures") + " successfully" : "No textures found in that file");
                }
                catch (Exception)
                {
                    Log("Error extracting textures from .milo_ps3 file");
                }
            }

            Log("Done");
            picWorking.Visible = false;
            btnRefresh.PerformClick();
        }

        private void btnFromXbox_Click(object sender, EventArgs e)
        {
            if ((!(chkXboxBMP.Checked)) && (!(chkXboxPNG.Checked)) && (!(chkXboxJPG.Checked)))
            {
                MessageBox.Show("Please select at least one output format", Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            picWorking.Visible = true;
            imgCounter = 0;
            foreach (var image in imagesXbox)
            {
                FromXbox(image);
            }
            foreach (var image in imagesDDS)
            {
                FromXbox(image);
            }
            foreach (var milo in miloXbox)
            {
                if (!File.Exists(milo)) continue;
                
                var format = "png";
                if (chkXboxBMP.Checked)
                {
                    format = "bmp";
                }
                else if (chkXboxJPG.Checked)
                {
                    format = "jpg";
                }
                Log("Searching .milo_xbox file for textures");
                Log(" - " + Path.GetFileName(milo));
                try
                {
                    int count;
                    PikminMiloTools.ExtractTextures(milo, keepDDSFiles.Checked, false, format, out count);
                    
                    Log(count > 0 ? "Extracted " + count + " " + (count == 1 ? "texture" : "textures") + " successfully" : "No textures found in that file");
                }
                catch (Exception)
                {
                    Log("Error extracting textures from .milo_xbox file");
                }
            }

            Log("Done");
            picWorking.Visible = false;
            btnRefresh.PerformClick();
        }

        private void btnFromWii_Click(object sender, EventArgs e)
        {
            if ((!(chkWiiBMP.Checked)) && (!(chkWiiPNG.Checked)) && (!(chkWiiJPG.Checked)))
            {
                MessageBox.Show("Please select at least one output format", Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            picWorking.Visible = true;
            imgCounter = 0;
            foreach (var image in imagesWii)
            {
                FromWii(image);
            }
            foreach (var image in imagesTPL)
            {
                FromWii(image);
            }
            foreach (var milo in miloWii)
            {
                if (!File.Exists(milo)) continue;

                var format = "png";
                if (chkWiiBMP.Checked)
                {
                    format = "bmp";
                }
                else if (chkWiiJPG.Checked)
                {
                    format = "jpg";
                }

                Log("Searching .milo_wii file for textures");
                Log(" - " + Path.GetFileName(milo));
                try
                {
                    int count;
                    PikminMiloTools.ExtractTextures(milo, false, true, format, out count);

                    Log(count > 0 ? "Extracted " + count + " " + (count == 1 ? "texture" : "textures") + " successfully" : "No textures found in that file");
                }
                catch (Exception)
                {
                    Log("Error extracting textures from .milo_ps3 file");
                }
            }

            Log("Done");
            picWorking.Visible = false;
            btnRefresh.PerformClick();
        }

        private void btnFromWii_EnabledChanged(object sender, EventArgs e)
        {
            chkWiiBMP.Enabled = btnFromWii.Enabled;
            chkWiiPNG.Enabled = btnFromWii.Enabled;
            chkWiiJPG.Enabled = btnFromWii.Enabled;
        }

        private void btnFromXbox_EnabledChanged(object sender, EventArgs e)
        {
            chkXboxBMP.Enabled = btnFromXbox.Enabled;
            chkXboxPNG.Enabled = btnFromXbox.Enabled;
            chkXboxJPG.Enabled = btnFromXbox.Enabled;
        }

        private void btnFromPS3_EnabledChanged(object sender, EventArgs e)
        {
            chkPS3BMP.Enabled = btnFromPS3.Enabled;
            chkPS3JPG.Enabled = btnFromPS3.Enabled;
            chkPS3PNG.Enabled = btnFromPS3.Enabled;
        }
        
        private void ToXbox(string image, bool flip_bytes = false)
        {
            if (!File.Exists(image)) return;

            imgCounter++;
            Log("Converting file " + imgCounter + " of " + (imageCount + imagesDDS.Count() + imagesPS3.Count()));
            Log(" - " + Path.GetFileName(image));

            var success = flip_bytes ? Tools.ConvertPS3toXbox(image, image, chkCleanUpXbox.Checked) : Tools.ConvertImagetoRB(image, image, chkCleanUpXbox.Checked);
            Log(success? "Converted to png_xbox format successfully" : "Error converting to png_xbox format");
        }

        private void ToWii(string image)
        {
            if (!File.Exists(image)) return;

            imgCounter++;
            Log("Converting file " + imgCounter + " of " + (imageCount + imagesXbox.Count() + imagesDDS.Count() + imagesPS3.Count() + imagesTPL.Count()));
            Log(" - " + Path.GetFileName(image));

            Log(Tools.ConvertImagetoWii(wimgt, image) ? "Converted to png_wii format successfully" : "Error converting to png_wii format");
        }

        private void ToPS3(string image, bool flip_bytes = false)
        {
            if (!File.Exists(image)) return;

            imgCounter++;
            Log("Converting file " + imgCounter + " of " + (imageCount + imagesDDS.Count() + imagesXbox.Count()));
            Log(" - " + Path.GetFileName(image));

            var success = flip_bytes ? Tools.ConvertXboxtoPS3(image, image, chkCleanUpPS3.Checked) : Tools.ConvertImagetoRB(image, image, chkCleanUpPS3.Checked,true);
            Log(success ? "Converted to png_ps3 format successfully" : "Error converting to png_ps3 format");
        }

        private void x256ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Tools.TextureSize = 256;
            x256.Checked = true;
            x512.Checked = false;
            x1024.Checked = false;
            x2048.Checked = false;
        }

        private void x512defaultToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Tools.TextureSize = 512;
            x256.Checked = false;
            x512.Checked = true;
            x1024.Checked = false;
            x2048.Checked = false;
        }

        private void x1024ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Tools.TextureSize = 1024;
            x256.Checked = false;
            x512.Checked = false;
            x1024.Checked = true;
            x2048.Checked = false;
        }

        private void keepDDSFilesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Tools.KeepDDS = keepDDSFiles.Checked;
        }

        private void x2048muricaToolStrip_Click(object sender, EventArgs e)
        {
            Tools.TextureSize = 2048;
            x256.Checked = false;
            x512.Checked = false;
            x1024.Checked = false;
            x2048.Checked = true;
        }
        
        private void copyFolderPathToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Clipboard.SetText(txtFolder.Text);
            Log("Folder path copied to clipboard");
        }

        private void pasteFolderPathToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (Directory.Exists(Clipboard.GetText()) || Path.GetDirectoryName(Clipboard.GetText()) != "")
                {
                    Log("Pasted new directory from clipboard");
                    txtFolder.Text = Clipboard.GetText();
                    btnRefresh.PerformClick();
                }
                else
                {
                    Log("Invalid folder path");
                }
            }
            catch (Exception)
            {
                Log("Invalid folder path");
            }
        }

        private void contextMenuStrip2_Opening(object sender, System.ComponentModel.CancelEventArgs e)
        {
            copyFolderPathToolStripMenuItem.Visible = txtFolder.Text != "";
            pasteFolderPathToolStripMenuItem.Visible = Clipboard.GetText() != "";
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

        private void textureDXT1_Click(object sender, EventArgs e)
        {
            textureDXT5.Checked = false;
            textureDXT1.Checked = true;
            Tools.useDXT5 = false;
        }

        private void textureDXT5_Click(object sender, EventArgs e)
        {
            textureDXT1.Checked = false;
            textureDXT5.Checked = true;
            Tools.useDXT5 = true;
        }

        private void btnToPS4_Click(object sender, EventArgs e)
        {
            picWorking.Visible = true;
            try
            {
                imgCounter = 0;
                foreach (var image in imagesBMP)
                {
                    ToPS4(image);
                }
                foreach (var image in imagesJPG)
                {
                    ToPS4(image);
                }               
                foreach (var image in imagesPNG.Where(image => !image.EndsWith(".png_wii", StringComparison.Ordinal) && !image.EndsWith(".png_xbox", StringComparison.Ordinal) && !image.EndsWith(".png_ps3", StringComparison.Ordinal) && !image.EndsWith(".png_ps4", StringComparison.Ordinal)))
                {
                    ToPS4(image);
                }                
                Log("Done");
                picWorking.Visible = false;
                btnRefresh.PerformClick();
            }
            catch (Exception ex)
            {
                Log("There was an error:");
                Log(ex.Message);
                picWorking.Visible = false;
            }
        }

        private void ToPS4(string image)
        {
            if (!File.Exists(image)) return;

            imgCounter++;
            Log("Converting file " + imgCounter);
            Log(" - " + Path.GetFileName(image));

            var newFile = Path.GetDirectoryName(image) + "\\" + Path.GetFileNameWithoutExtension(image) + ".png_ps4";
            using (var fileStream = new FileStream(newFile, FileMode.CreateNew, FileAccess.Write))
            {
                TextureWriter.WriteStream(TextureConverter.ToTexture(Tools.NemoLoadImage(image)), fileStream);
            }
            var success = File.Exists(newFile);
            Log(success ? "Converted to .png_ps4 format successfully" : "Error converting to .png_ps4 format");
        }

        private void btnFromPS4_Click(object sender, EventArgs e)
        {
            if ((!(chkPS4BMP.Checked)) && (!(chkPS4PNG.Checked)) && (!(chkPS4JPG.Checked)))
            {
                MessageBox.Show("Please select at least one output format", Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            picWorking.Visible = true;
            imgCounter = 0;
            foreach (var image in imagesPS4)
            {
                FromPS4(image);
            }        
            Log("Done");
            picWorking.Visible = false;
            btnRefresh.PerformClick();
        }

        private void FromPS4(string image)
        {
            if (!File.Exists(image)) return;

            imgCounter++;
            Log("Converting file " + imgCounter + " of " + imagesPS4.Count());
            Log(" - " + Path.GetFileName(image));

            var success = false;
            Bitmap bitmap;
            using (var fileStream = new FileStream(image, FileMode.Open, FileAccess.Read))
            {
                var converter = new TextureConverter();
                bitmap = converter.ToBitmap(TextureReader.ReadStream(fileStream), 0);
            }                  

            if (chkPS4BMP.Checked)
            {               
                var newFile = Path.GetDirectoryName(image) + "\\" + Path.GetFileNameWithoutExtension(image) + ".bmp";
                bitmap.Save(newFile, System.Drawing.Imaging.ImageFormat.Bmp); 
                success = File.Exists(newFile);
            }
            if (chkPS4PNG.Checked)
            {
                var newFile = Path.GetDirectoryName(image) + "\\" + Path.GetFileNameWithoutExtension(image) + ".png";
                bitmap.Save(newFile, System.Drawing.Imaging.ImageFormat.Png);
                success = File.Exists(newFile);
            }
            if (chkPS4JPG.Checked)
            {
                var newFile = Path.GetDirectoryName(image) + "\\" + Path.GetFileNameWithoutExtension(image) + ".jpg";
                bitmap.Save(newFile, System.Drawing.Imaging.ImageFormat.Jpeg);
                success = File.Exists(newFile);
            }

            if (success)
            {
                //if clean up box checked, delete originals
                if (chkCleanUpPS4.Checked)
                {
                    Tools.SendtoTrash(image);
                }
                Log("Converted from .png_ps4 format successfully");
            }
            else
            {
                Log("Error converting from .png_ps4 format");
            }
        }

        private void btnFromPS4_EnabledChanged(object sender, EventArgs e)
        {
            chkPS4BMP.Enabled = btnFromPS4.Enabled;
            chkPS4JPG.Enabled = btnFromPS4.Enabled;
            chkPS4PNG.Enabled = btnFromPS4.Enabled;
        }
    }
}