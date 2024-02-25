using System;
using System.Collections.Specialized;
using System.IO;
using System.Windows.Forms;
using System.Drawing;
using Nautilus.Properties;
using System.Security.Cryptography;
using System.Diagnostics;

namespace Nautilus
{
    public partial class RBAEditor : Form
    {
        private readonly NemoTools Tools;
        private string basesongname;
        private string tempFolder;
        private string originalRBA;
        private readonly string inputFile;
        string dtaHash = "";
        string dtaLength = "";
        string midiHash = "";
        string midiLength = "";
        string moggHash = "";
        string moggLength = "";
        string miloHash = "";
        string miloLength = "";
        string artHash = "";
        string artLength = "";
        string weightsHash = "";
        string weightsLength = "";
        string backendHash = "";
        string backendLength = "";
        private bool isRBS;

        public RBAEditor(Color ButtonBackColor, Color ButtonTextColor, string arg = "")
        {
            InitializeComponent();
            Tools = new NemoTools();
            tempFolder = Application.StartupPath + "\\rbaTemp\\";
            inputFile = arg;
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
         
        private bool ExtractRBA()
        {
            Tools.DeleteFolder(tempFolder, true);
            if (!Directory.Exists(tempFolder))
            {
                Directory.CreateDirectory(tempFolder);
            }
            basesongname = Path.GetFileNameWithoutExtension(originalRBA).Replace(" ", "").Trim();
            if (basesongname.Length > 26)
            {
                basesongname = basesongname.Substring(0, 26);
            }
            try
            {
                Log("Reading file " + originalRBA);
                using (var bReadRba = new BinaryReader(File.Open(originalRBA, FileMode.Open)))
                {
                    var signature = bReadRba.ReadChars(4);
                    if ((signature[0] != 'R') ||
                        (signature[1] != 'B') ||
                        (signature[2] != 'S') ||
                        (signature[3] != 'F'))
                    {
                        Log("Unknown file format, can't read file " + Path.GetFileName(originalRBA));
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
                        var fname = tempFolder + (isRBS ? "songs.dta" : "songs.dta.raw");
                        using (var bWrite = new BinaryWriter(File.Open(fname, FileMode.Create)))
                        {
                            bWrite.Write(data);
                            bWrite.Dispose();
                        }
                        if (File.Exists(fname))
                        {
                            Log("Extracted DTA file successfully");
                            btnDTA.BackColor = Color.AliceBlue;
                        }
                        else
                        {
                            Log("Failed to extract DTA file");
                            btnDTA.BackColor = Color.Gray;
                        }                        
                    }
                    else
                    {
                        Log("This file does not contain a DTA file!");
                        btnDTA.BackColor = Color.Gray;
                    }
                    btnDTA.Enabled = btnDTA.BackColor == Color.AliceBlue;
                    if (rba_header_values[(int)RBA_HEADER_INDEX.OFFSET_MID] != 0)
                    {
                        var offset = rba_header_values[(int)RBA_HEADER_INDEX.OFFSET_MID];
                        var len = rba_header_values[(int)RBA_HEADER_INDEX.LENGTH_MID];
                        bReadRba.BaseStream.Seek(offset, SeekOrigin.Begin);
                        var data = bReadRba.ReadBytes(len);
                        var fname = tempFolder + basesongname + ".mid";
                        using (var bWrite = new BinaryWriter(File.Open(fname, FileMode.Create)))
                        {
                            bWrite.Write(data);
                            bWrite.Dispose();
                        }
                        if (File.Exists(fname))
                        {
                            Log("Extracted " + basesongname + ".mid successfully");
                            btnMIDI.BackColor = Color.AliceBlue;
                        }
                        else
                        {
                            Log("Failed to extract " + basesongname + ".mid");
                            btnMIDI.BackColor = Color.Gray;
                        }
                    }
                    else
                    {
                        Log("This file does not contain a MIDI file!");
                        btnMIDI.BackColor = Color.Gray;
                    }
                    btnMIDI.Enabled = btnMIDI.BackColor == Color.AliceBlue;
                    if (rba_header_values[(int)RBA_HEADER_INDEX.OFFSET_MOGG] != 0)
                    {
                        var offset = rba_header_values[(int)RBA_HEADER_INDEX.OFFSET_MOGG];
                        var len = rba_header_values[(int)RBA_HEADER_INDEX.LENGTH_MOGG];
                        bReadRba.BaseStream.Seek(offset, SeekOrigin.Begin);
                        var data = bReadRba.ReadBytes(len);
                        var fname = tempFolder + basesongname + ".mogg";
                        using (var bWrite = new BinaryWriter(File.Open(fname, FileMode.Create)))
                        {
                            bWrite.Write(data);
                            bWrite.Dispose();
                        }
                        if (File.Exists(fname))
                        {
                            Log("Extracted " + basesongname + ".mogg successfully");
                            btnMogg.BackColor = Color.AliceBlue;
                        }
                        else
                        {
                            Log("Failed to extract " + basesongname + ".mogg");
                            btnMogg.BackColor = Color.Gray;
                        }
                    }
                    else
                    {
                        Log("This file does not contain a MOGG file!");
                        btnMogg.BackColor = Color.Gray;
                    }
                    btnMogg.Enabled = btnMogg.BackColor == Color.AliceBlue;
                    if (rba_header_values[(int)RBA_HEADER_INDEX.OFFSET_MILO_XBOX] != 0)
                    {
                        var offset = rba_header_values[(int)RBA_HEADER_INDEX.OFFSET_MILO_XBOX];
                        var len = rba_header_values[(int)RBA_HEADER_INDEX.LENGTH_MILO_XBOX];
                        bReadRba.BaseStream.Seek(offset, SeekOrigin.Begin);
                        var data = bReadRba.ReadBytes(len);
                        var fname = tempFolder + basesongname + ".milo_xbox";
                        using (var bWrite = new BinaryWriter(File.Open(fname, FileMode.Create)))
                        {
                            bWrite.Write(data);
                            bWrite.Dispose();
                        }
                        if (File.Exists(fname))
                        {
                            Log("Extracted " + basesongname + ".milo_xbox successfully");
                            btnMilo.BackColor = Color.AliceBlue;
                        }
                        else
                        {
                            Log("Failed to extract " + basesongname + ".milo_xbox");
                            btnMilo.BackColor = Color.Gray;
                        }
                    }
                    else
                    {
                        Log("This file does not contain a MILO_XBOX file!");
                        btnMilo.BackColor = Color.Gray;
                    }
                    btnMilo.Enabled = btnMilo.BackColor == Color.AliceBlue;
                    if (rba_header_values[(int)RBA_HEADER_INDEX.OFFSET_BMP] != 0)
                    {
                        var offset = rba_header_values[(int)RBA_HEADER_INDEX.OFFSET_BMP];
                        var len = rba_header_values[(int)RBA_HEADER_INDEX.LENGTH_BMP];
                        bReadRba.BaseStream.Seek(offset, SeekOrigin.Begin);
                        var data = bReadRba.ReadBytes(len);
                        var fname = tempFolder + basesongname + (isRBS ? ".png_xbox" : ".bmp");
                        using (var bWrite = new BinaryWriter(File.Open(fname, FileMode.Create)))
                        {
                            bWrite.Write(data);
                            bWrite.Dispose();
                        }
                        if (File.Exists(fname))
                        {
                            Log("Extracted " + basesongname + (isRBS ? ".png_xbox" : ".bmp") + " successfully");
                            btnArt.BackColor = Color.AliceBlue;
                        }
                        else
                        {
                            Log("Failed to extract " + basesongname + (isRBS ? ".png_xbox" : ".bmp"));
                            btnArt.BackColor = Color.Gray;
                        }
                    }
                    else
                    {
                        Log("This file does not contain an ALBUM ART file!");
                        btnArt.BackColor = Color.Gray;
                    }
                    btnArt.Enabled = btnArt.BackColor == Color.AliceBlue;
                    if (rba_header_values[(int)RBA_HEADER_INDEX.OFFSET_WEIGHTS] != 0)
                    {
                        var offset = rba_header_values[(int)RBA_HEADER_INDEX.OFFSET_WEIGHTS];
                        var len = rba_header_values[(int)RBA_HEADER_INDEX.LENGTH_WEIGHTS];
                        bReadRba.BaseStream.Seek(offset, SeekOrigin.Begin);
                        var data = bReadRba.ReadBytes(len);
                        var fname = tempFolder + basesongname + "_weights.bin";
                        using (var bWrite = new BinaryWriter(File.Open(fname, FileMode.Create)))
                        {
                            bWrite.Write(data);
                            bWrite.Close();
                        }
                        if (File.Exists(fname))
                        {
                            Log("Extracted " + basesongname + "_weights.bin successfully");
                            btnWeights.BackColor = Color.AliceBlue;
                        }
                        else
                        {
                            Log("Failed to extract " + basesongname + "_weights.bin");
                            btnWeights.BackColor = Color.Gray;
                        }
                    }
                    else
                    {
                        Log("This file does not contain a WEIGHTS.BIN file!");
                        btnWeights.BackColor = Color.Gray;
                    }
                    btnWeights.Enabled = btnWeights.BackColor == Color.AliceBlue;
                    if (rba_header_values[(int)RBA_HEADER_INDEX.OFFSET_BACKEND] != 0)
                    {
                        var offset = rba_header_values[(int)RBA_HEADER_INDEX.OFFSET_BACKEND];
                        var len = rba_header_values[(int)RBA_HEADER_INDEX.LENGTH_BACKEND];
                        bReadRba.BaseStream.Seek(offset, SeekOrigin.Begin);
                        var data = bReadRba.ReadBytes(len);
                        var fname = tempFolder + "backend.raw";
                        using (var bWrite = new BinaryWriter(File.Open(fname, FileMode.Create)))
                        {
                            bWrite.Write(data);
                            bWrite.Close();
                        }
                        if (File.Exists(fname))
                        {
                            Log("Extracted backend.raw successfully");
                            btnBackend.BackColor = Color.AliceBlue;
                        }
                        else
                        {
                            Log("Failed to extract backend.raw");
                            btnBackend.BackColor = Color.Gray;
                        }
                    }
                    else
                    {
                        Log("This file does not contain a BACKEND file!");
                        btnBackend.BackColor = Color.Gray;
                    }
                    btnBackend.Enabled = btnBackend.BackColor == Color.AliceBlue;
                    bReadRba.Dispose();
                    Log("Extracted file contents successfully");
                    return true;
                }
            }
            catch (Exception e)
            {
                Log("Error extracting file " + Path.GetFileName(originalRBA));
                Log(e.ToString());
                return false;
            }
        }
                
        private void HandleDragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
                e.Effect = DragDropEffects.All;
        }

        private void HandleDragDrop(object sender, DragEventArgs e)
        {
            var files = (string[])e.Data.GetData(DataFormats.FileDrop);
            ProcessInputfile(files[0]);
        }

        private void ProcessInputfile(string file)
        {
            var ext = Path.GetExtension(file.ToLowerInvariant());
            if (ext == ".rba" || ext == ".rbs")
            {
                isRBS = ext == ".rbs";
                originalRBA = file;
                ExtractRBA();
            }
            else
            {
                Log("Invalid file");
            }
        }
        
        private void exportLogFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Tools.ExportLog(Text, lstLog.Items);
        }

        private void RBAEditor_Shown(object sender, EventArgs e)
        {
            Log("Welcome to " + Text);
            Log("Drag and drop the RBA/RBS file here");
            Log("Ready");

            if (File.Exists(inputFile))
            {
                ProcessInputfile(inputFile);
            }
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
           
        private void RBAEditor_FormClosing(object sender, FormClosingEventArgs e)
        {            
            if (!Text.Contains("*"))
            {
                Tools.DeleteFolder(tempFolder, true);
                return;
            }
            var result = MessageBox.Show("You have unsaved changes, do you really want to exit without saving?", "Warning", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Exclamation);
            if (result == DialogResult.No || result == DialogResult.Cancel)
            {
                e.Cancel = true;
                return;
            }
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

        private void btnDTA_Click(object sender, EventArgs e)
        {
            var files = new StringCollection();
            files.Add(tempFolder + (isRBS ? "songs.dta" : "songs.dta.raw"));
            CopyFile(files);
        }

        private void CopyFile(StringCollection files)
        {
            if (File.Exists(files[0]))
            {
                var moveEffect = new byte[] { 1, 0, 0, 0 };
                var dropEffect = new MemoryStream();
                dropEffect.Write(moveEffect, 0, moveEffect.Length);

                var data = new DataObject();
                data.SetFileDropList(files);
                data.SetData("Preferred DropEffect", dropEffect);

                Clipboard.Clear();
                Clipboard.SetDataObject(data, true);
                               
                Log("File is in your clipboard, paste it anywhere");
            }
            else
            {
                Log("Process failed");
            }
        }

        private void btnArt_Click(object sender, EventArgs e)
        {
            var files = new StringCollection();
            files.Add(tempFolder + basesongname + (isRBS ? ".png_xbox" : ".bmp"));
            CopyFile(files);
        }

        private void btnMIDI_Click(object sender, EventArgs e)
        {
            var files = new StringCollection();
            files.Add(tempFolder + basesongname + ".mid");
            CopyFile(files);
        }

        private void btnMogg_Click(object sender, EventArgs e)
        {
            var files = new StringCollection();
            files.Add(tempFolder + basesongname + ".mogg");
            CopyFile(files);
        }

        private void btnMilo_Click(object sender, EventArgs e)
        {
            var files = new StringCollection();
            files.Add(tempFolder + basesongname + ".milo_xbox");
            CopyFile(files);
        }

        private void btnWeights_Click(object sender, EventArgs e)
        {
            var files = new StringCollection();
            files.Add(tempFolder + basesongname + "_weights.bin");
            CopyFile(files);
        }

        private void btnBackend_Click(object sender, EventArgs e)
        {
            var files = new StringCollection();
            files.Add(tempFolder + "backend.raw");
            CopyFile(files);
        }
            
        private void ReplaceFile(string existingFile, string newFile, Button btn)
        {
            try
            {
                Tools.DeleteFile(existingFile);
                File.Copy(newFile, existingFile);
            }
            catch (Exception e)
            {
                Log("Error replacing file with " + newFile);
                Log("Error says " + e.Message);
            }
            if (File.Exists(existingFile))
            {
                Log("Successfully replaced file with " + newFile);
                btn.BackColor = Color.LightYellow;
                Text = "*** RBA Editor ***";
            }
        }

        private void btnDTA_DragDrop(object sender, DragEventArgs e)
        {
            var files = (string[])e.Data.GetData(DataFormats.FileDrop);
            switch (Path.GetFileName(files[0]).ToLowerInvariant())
            {
                case "songs.dta.raw":
                    if (!isRBS)
                    {
                        ReplaceFile(tempFolder + "songs.dta.raw", files[0], btnDTA);
                    }
                    else
                    {
                        MessageBox.Show("Only songs.dta files can be dropped here", Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    }
                    break;
                case "songs.dta":
                    if (isRBS)
                    {
                        ReplaceFile(tempFolder + "songs.dta", files[0], btnDTA);
                    }
                    else
                    {
                        MessageBox.Show("Only songs.dta.raw files can be dropped here", Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    }
                    break;                
                default:
                    MessageBox.Show("Only songs.dta.raw and songs.dta files can be dropped here", Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    break;
            }
        }

        private void btnArt_DragDrop(object sender, DragEventArgs e)
        {
            var files = (string[])e.Data.GetData(DataFormats.FileDrop);
            switch (Path.GetExtension(Path.GetFileName(files[0]).ToLowerInvariant()))
            {
                case ".png_xbox":
                    if (isRBS)
                    {
                        ReplaceFile(tempFolder + basesongname + ".png_xbox", files[0], btnArt);     
                    }
                    else
                    {
                        MessageBox.Show("Only .bmp files can be dropped here", Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    }
                    break;
                case ".bmp":
                    if (!isRBS)
                    {
                        ReplaceFile(tempFolder + basesongname + ".bmp", files[0], btnArt);
                    }
                    else
                    {
                        MessageBox.Show("Only .png_xbox files can be dropped here", Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    }
                    break;
                default:
                    MessageBox.Show("Only .bmp and .png_xbox files can be dropped here", Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    break;
            }
        }

        private void btnMIDI_DragDrop(object sender, DragEventArgs e)
        {
            var files = (string[])e.Data.GetData(DataFormats.FileDrop);
            switch (Path.GetExtension(Path.GetFileName(files[0]).ToLowerInvariant()))
            {
                case ".mid":
                    ReplaceFile(tempFolder + basesongname + ".mid", files[0], btnMIDI);
                    break;
                default:
                    MessageBox.Show("Only .mid files can be dropped here", Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    break;
            }
        }

        private void btnMogg_DragDrop(object sender, DragEventArgs e)
        {
            var files = (string[])e.Data.GetData(DataFormats.FileDrop);
            switch (Path.GetExtension(Path.GetFileName(files[0]).ToLowerInvariant()))
            {
                case ".mogg":
                //case ".ogg":
                    ReplaceFile(tempFolder + basesongname + ".mogg", files[0], btnMogg);
                    break;
                default:
                    MessageBox.Show("Only .mogg files can be dropped here", Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    break;
            }
        }

        private void btnMilo_DragDrop(object sender, DragEventArgs e)
        {
            var files = (string[])e.Data.GetData(DataFormats.FileDrop);
            switch (Path.GetExtension(Path.GetFileName(files[0]).ToLowerInvariant()))
            {
                case ".milo_xbox":
                    ReplaceFile(tempFolder + basesongname + ".milo_xbox", files[0], btnMilo);
                    break;
                default:
                    MessageBox.Show("Only .milo_xbox files can be dropped here", Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    break;
            }
        }

        private void btnWeights_DragDrop(object sender, DragEventArgs e)
        {
            var files = (string[])e.Data.GetData(DataFormats.FileDrop);
            switch (Path.GetExtension(Path.GetFileName(files[0]).ToLowerInvariant()))
            {
                case ".bin":
                    ReplaceFile(tempFolder + basesongname + "_weights.bin", files[0], btnWeights);
                    break;
                default:
                    MessageBox.Show("Only _weights.bin files can be dropped here", Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    break;
            }
        }

        private void btnBackend_DragDrop(object sender, DragEventArgs e)
        {
            var files = (string[])e.Data.GetData(DataFormats.FileDrop);
            switch (Path.GetFileName(files[0]).ToLowerInvariant())
            {
                case "backend.raw":
                    ReplaceFile(tempFolder + "backend.raw", files[0], btnBackend);
                    break;
                default:
                    MessageBox.Show("Only backend.raw files can be dropped here", Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    break;
            }
        }

        private void RBAEditor_TextChanged(object sender, EventArgs e)
        {
            btnSave.Visible = Text.Contains("*");
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            var newRBA = originalRBA.Replace(".rba", "_edited.rba").Replace(".rbs", "_edited.rbs");
            Tools.DeleteFile(newRBA);

            //make sure necessary component files are present and calculate hash and length
            //code taken from Pikmin's RBA Builder 2.0
            var dta = tempFolder + (isRBS ? "songs.dta" : "songs.dta.raw");
            if (!File.Exists(dta))
            {
                Log("Missing DTA file, can't save changes to file");
                return;
            }
            else
            {
                dtaHash = BitConverter.ToString(getHash(dta)).Replace("-", string.Empty);
                dtaLength = getLength(dta).ToString();
                Log("DTA file hash is " + dtaHash);
                Log("DTA file length is " + dtaLength);
            }
            var art = tempFolder + basesongname + (isRBS ? ".png_xbox" : ".bmp");
            if (!File.Exists(art))
            {
                Log("Missing ALBUM ART file, can't save changes to file");
                return;
            }
            else
            {
                artHash = BitConverter.ToString(getHash(art)).Replace("-", string.Empty);
                artLength = getLength(art).ToString();
                Log("ALBUM ART file hash is " + artHash);
                Log("ALBUM ART file length is " + artLength);
            }
            var midi = tempFolder + basesongname + ".mid";
            if (!File.Exists(midi))
            {
                Log("Missing MIDI file, can't save changes to file");
                return;
            }
            else
            {
                midiHash = BitConverter.ToString(getHash(midi)).Replace("-", string.Empty);
                midiLength = getLength(midi).ToString();
                Log("MIDI file hash is " + midiHash);
                Log("MIDI file length is " + midiLength);
            }
            var mogg = tempFolder + basesongname + ".mogg";
            if (!File.Exists(mogg))
            {
                Log("Missing MOGG file, can't save changes to file");
                return;
            }
            else
            {
                moggHash = BitConverter.ToString(getHash(mogg)).Replace("-", string.Empty);
                moggLength = getLength(mogg).ToString();
                Log("MOGG file hash is " + moggHash);
                Log("MOGG file length is " + moggLength);
            }
            var milo = tempFolder + basesongname + ".milo_xbox";
            if (!File.Exists(milo))
            {
                Log("Missing MILO_XBOX file, can't save changes to file");
                return;
            }
            else
            {
                miloHash = BitConverter.ToString(getHash(milo)).Replace("-", string.Empty);
                miloLength = getLength(milo).ToString();
                Log("MILO_XBOX file hash is " + miloHash);
                Log("MILO_XBOX file length is " + miloLength);
            }
            var weights = tempFolder + basesongname + "_weights.bin";
            if (!File.Exists(weights))
            {
                Log("Missing WEIGHTS.BIN file, won't add it to the file");
            }
            else
            {
                weightsHash = BitConverter.ToString(getHash(weights)).Replace("-", string.Empty);
                weightsLength = getLength(weights).ToString();
                Log("WEIGHTS.BIN file hash is " + weightsHash);
                Log("WEIGHTS.BIN file length is " + weightsLength);
            }
            var backend = tempFolder + "backend.raw";
            if (!File.Exists(backend))
            {
                Log("Missing BACKEND file, won't add it to the file");                
            }
            else
            {
                backendHash = BitConverter.ToString(getHash(backend)).Replace("-", string.Empty);
                backendLength = getLength(backend).ToString();
                Log("BACKEND file hash is " + backendHash);
                Log("BACKEND file length is " + backendLength);
            }

            byte[] buffer1 = new byte[4]
      {
        (byte) 82,
        (byte) 66,
        (byte) 83,
        (byte) 70
      };
            byte[] buffer2 = new byte[38]
      {
        (byte) 2,
        (byte) 0,
        (byte) 34,
        (byte) 0,
        (byte) 2,
        (byte) 49,
        (byte) 49,
        (byte) 48,
        (byte) 52,
        (byte) 49,
        (byte) 49,
        (byte) 95,
        (byte) 65,
        (byte) 0,
        (byte) 48,
        (byte) 52,
        (byte) 49,
        (byte) 49,
        (byte) 95,
        (byte) 65,
        (byte) 0,
        (byte) 2,
        (byte) 49,
        (byte) 49,
        (byte) 48,
        (byte) 52,
        (byte) 49,
        (byte) 49,
        (byte) 95,
        (byte) 65,
        (byte) 0,
        (byte) 112,
        (byte) 105,
        (byte) 108,
        (byte) 101,
        (byte) 114,
        (byte) 58,
        (byte) 0
      };
            int num1 = Convert.ToInt32(dtaLength) + 262;
            int num2 = Convert.ToInt32(midiLength) + num1;
            int num3 = Convert.ToInt32(moggLength) + num2;
            int num4 = Convert.ToInt32(miloLength) + num3;
            int num5 = File.Exists(weights) ? (Convert.ToInt32(weightsLength) + num4) : 0;
            int num6 = Convert.ToInt32(artLength) + (File.Exists(weights) ? num5 : num4);            
            using (BinaryWriter binaryWriter = new BinaryWriter((Stream)File.OpenWrite("header.temp")))
            {
                binaryWriter.Write(buffer1);
                binaryWriter.Write(4);
                binaryWriter.Write(262);
                binaryWriter.Write(num1);
                binaryWriter.Write(num2);
                binaryWriter.Write(num3);
                binaryWriter.Write(num4);
                binaryWriter.Write(num5);
                if (!isRBS)
                {
                    binaryWriter.Write(num6);
                }
                binaryWriter.BaseStream.Position = 36L;
                binaryWriter.Write(Convert.ToInt32(dtaLength));
                binaryWriter.Write(Convert.ToInt32(midiLength));
                binaryWriter.Write(Convert.ToInt32(moggLength));
                binaryWriter.Write(Convert.ToInt32(miloLength));
                binaryWriter.Write(Convert.ToInt32(artLength));
                if (File.Exists(weights) && !isRBS)
                {
                    binaryWriter.Write(Convert.ToInt32(weightsLength));
                }
                else
                {
                    binaryWriter.Write(0);
                }
                if (File.Exists(backend) && !isRBS)
                {
                    binaryWriter.Write(Convert.ToInt32(backendLength));
                }
                else
                {
                    binaryWriter.Write(0);
                }
                binaryWriter.BaseStream.Position = 64L;
                binaryWriter.Write(getHash(dta));
                binaryWriter.Write(getHash(midi));
                binaryWriter.Write(getHash(mogg));
                binaryWriter.Write(getHash(milo));
                binaryWriter.Write(getHash(art));
                if (File.Exists(weights) && !isRBS)
                {
                    binaryWriter.Write(getHash(weights));
                }
                else
                {
                    binaryWriter.Write(0);
                }
                if (File.Exists(backend) && !isRBS)
                {
                    binaryWriter.Write(getHash(backend));
                }
                else
                {
                    binaryWriter.Write(0);
                }
                binaryWriter.BaseStream.Position = 224L;
                binaryWriter.Write(buffer2);
                binaryWriter.Close();
            }
            byte[] hash = getHash("header.temp");
            if (File.Exists(newRBA))
            {
                File.Delete(newRBA);
            }
            File.Copy("header.temp", newRBA);
            File.Delete("header.temp");
            using (BinaryWriter binaryWriter = new BinaryWriter((Stream)File.OpenWrite(newRBA)))
            {
                binaryWriter.BaseStream.Position = 204L;
                binaryWriter.Write(hash);
                binaryWriter.BaseStream.Position = 262L;
                binaryWriter.Write(File.ReadAllBytes(dta));
                binaryWriter.Write(File.ReadAllBytes(midi));
                binaryWriter.Write(File.ReadAllBytes(mogg));
                binaryWriter.Write(File.ReadAllBytes(milo));
                binaryWriter.Write(File.ReadAllBytes(art));
                if (File.Exists(weights) && !isRBS)
                {
                    binaryWriter.Write(File.ReadAllBytes(weights));
                }                
                if (File.Exists(backend) && !isRBS)
                {
                    binaryWriter.Write(File.ReadAllBytes(backend));
                }
                binaryWriter.Close();
            }
            Log("Changes saved to " + Path.GetFileName(newRBA) + " successfully");

            Text = "RBA Editor";

            originalRBA = newRBA;
            ExtractRBA();
        }      

        private static byte[] getHash(string input)
        {
            //code taken from Pikmin's RBA Builder 2.0
            using (FileStream inputStream = File.OpenRead(input))
            {
                return new SHA1Managed().ComputeHash((Stream)inputStream);
            }
        }

        private static long getLength(string input)
        {
            //code taken from Pikmin's RBA Builder 2.0
            using (FileStream fileStream = File.OpenRead(input))
            {
                return fileStream.Length;
            }
        }

        private void sendToAudioAnalyzerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var analyzer = new AudioAnalyzer(tempFolder + basesongname + ".mogg");
            analyzer.Show();
        }

        private void sendToSongAnalyzerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var newAnalyzer = new SongAnalyzer(tempFolder + basesongname + ".mogg");           
            newAnalyzer.Show();
        }

        private void sendToSongAnalyzerToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            var newAnalyzer = new SongAnalyzer(tempFolder + basesongname + ".mid");
            newAnalyzer.Show();
        }

        private void sendToVisualizerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var newVisualizer = new Visualizer(Color.AliceBlue, Color.Black, tempFolder + (isRBS ? "songs.dta" : "songs.dta.raw"));
            newVisualizer.Show();
        }

        private void sendToVisualizerToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            var newVisualizer = new Visualizer(Color.AliceBlue, Color.Black, tempFolder + basesongname + (isRBS ? ".png_xbox" : ".bmp"));
            newVisualizer.Show();
        }

        private void openWithNotepadToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Process.Start("notepad.exe", tempFolder + (isRBS ? "songs.dta" : "songs.dta.raw"));
        }

        private void openInNotepadToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Process.Start("notepad.exe", tempFolder + "backend.raw");
        }
    }
}
