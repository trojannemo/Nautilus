using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using PDFLibNet;
using PdfFileWriter;

namespace Nautilus
{
    public partial class SetlistExporter : Form
    {
        public string setlistpath;
        public string setlistname;
        public string console;
        private readonly NemoTools Tools;
        private readonly DTAParser Parser;
        public List<SongData> Songs;
        private string export_path;
        private string export_dir;
        private PdfFont NormalFont;
        private PdfFont BoldFont;
        private PdfFont ItalicFont;
        public Color AltColor;
        public bool SortByArtist;
        private int TotalPageNumbers;
        private bool isLoading;
        private PDFStyle ActiveIndex;
        private bool ExportSuccess;
        private readonly string tempPDF;
        private const string AppName = "Setlist Exporter";
        public bool isRB4;
        public bool isBlitz;

        public SetlistExporter()
        {
            InitializeComponent();
            Tools = new NemoTools();
            Parser = new DTAParser();
            Songs = new List<SongData>();
            TotalPageNumbers = 0;
            isLoading = true;
            tempPDF = Application.StartupPath + "\\bin\\temp.pdf";
            ControlBox = false;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            Dispose();
        }
        
        private void btnExport_Click(object sender, EventArgs e)
        {
            if (picWorking.Visible)
            {
                MessageBox.Show("Please wait until the current process finishes", AppName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            var desktop = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            if (Songs.Count == 0)
            {
                MessageBox.Show("Something went wrong, I'm seeing 0 songs in your setlist\nClose me down and try again", AppName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            export_path = Path.GetFileNameWithoutExtension(setlistpath) + "_export.";
            if (string.IsNullOrWhiteSpace(export_dir))
            {
                export_dir = desktop;
            }
            var sfd = new SaveFileDialog
                {
                    Title = "Setlist Exporter",
                    InitialDirectory = export_dir,
                    OverwritePrompt = true,
                    AddExtension = true
                };
            var extension = "";
            switch (tabControl1.SelectedIndex)
            {
                case 0:
                    sfd.Filter = "Adobe Acrobat Files (*.pdf)|*pdf";
                    extension = "pdf";
                    break;
                case 1:
                    sfd.Filter = "Microsoft Excel Files (*.xlsx)|*xlsx";
                    extension = "xlsx";
                    break;
                case 2:
                    sfd.Filter = "CSV Files (*.csv)|*csv";
                    extension = "csv";
                    break;
                case 3:
                    sfd.Filter = "JSON files (*.json|*json";
                    extension = "json";
                    break;

            }
            sfd.FileName = export_path + extension;
            if (sfd.ShowDialog() != DialogResult.OK || string.IsNullOrWhiteSpace(sfd.FileName)) return;
            export_path = Path.GetDirectoryName(sfd.FileName) + "\\" + Path.GetFileNameWithoutExtension(sfd.FileName) + "." + extension;
            export_dir = Path.GetDirectoryName(sfd.FileName);
            PrepareToWork();
            fileExporter.RunWorkerAsync();
        }
        
        private string FormatDiff(int diff, bool isCSV = false, bool isJSON = false)
        {
            var SongData = new SongData();
            if (isCSV)
            {
                return radioTierNamesCSV.Checked ? SongData.GetDifficulty(diff) : (diff).ToString(CultureInfo.InvariantCulture);
            }
            if (isJSON)
            {
                return radioTierNamesJSON.Checked ? SongData.GetDifficulty(diff) : (diff).ToString(CultureInfo.InvariantCulture);
            }
            return radioTierNamesExcel.Checked ? SongData.GetDifficulty(diff) : (diff).ToString(CultureInfo.InvariantCulture);
        }

        private bool ExportPDF(bool CreateTempFile = false)
        {
            if (isLoading) return false;
            var outPDF = CreateTempFile || string.IsNullOrWhiteSpace(export_path) ? tempPDF : export_path;
            Tools.DeleteFile(outPDF);
            var Document = new PdfDocument(PaperType.Letter, false, UnitOfMeasure.Inch, outPDF);
            DefineFontResources(Document);
            switch (ActiveIndex)
            {
                case PDFStyle.Basic1:
                    doPDFBasic1(Document);
                    break;
                case PDFStyle.Basic2:
                case PDFStyle.DeadlyRobots: //"DeadlyRobots" custom format
                    doPDFBasic2(Document);
                    break;
                case PDFStyle.Detailed1:
                    doPDFDetailed1(Document);
                    break;
                case PDFStyle.Detailed2:
                    doPDFDetailed2(Document);
                    break;
                case PDFStyle.Detailed3:
                    doPDFDetailed3(Document);
                    break;
                case PDFStyle.espher:
                    doPDFEspher(Document);
                    break;
                case PDFStyle.Macst3r:
                    doPDFMacst3r(Document);
                    break;
            }
            
            try
            {
                Document.CreateFile();
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error exporting PDF file: \n" + ex.Message, AppName, MessageBoxButtons.OK,MessageBoxIcon.Error);
                return false;
            }
        }

        private void DefineFontResources(PdfDocument Document)
        {
            // Define font resources
            // Arguments: PdfDocument class, font family name, font style, embed flag
            // Font style (must be: Regular, Bold, Italic or Bold | Italic) All other styles are invalid.
            // Embed font. If true, the font file will be embedded in the PDF file.
            var index = 0;
            cboFonts.Invoke(new MethodInvoker(() => index = cboFonts.SelectedIndex));
            string font;
            switch (index)
            {
                case 1:
                    font = "Calibri";
                    break;
                case 2:
                    font = "Tahoma";
                    break;
                case 3:
                    font = "Times New Roman";
                    break;
                default:
                    font = "Arial";
                    break;
            }
            try
            {
                NormalFont = new PdfFont(Document, font, FontStyle.Regular);
                BoldFont = new PdfFont(Document, font, FontStyle.Bold);
                ItalicFont = new PdfFont(Document, font, FontStyle.Italic);
            }
            catch (Exception ex)
            {
                MessageBox.Show("There was an error loading font " + font + "\nThe error says:\n" + ex.Message +
                    "\nPlease try a different font", AppName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                cboFonts.Focus();
            }
        }

        private void doPDFBasic1(PdfDocument Document)
        {
            const double top_border = 0.5;
            const double left_border = 0.6;
            const double song_margin = 0.1;
            const double page_height = 11.0;
            const double page_width = 8.5;
            const double line_height = 0.16;
            const double line_width = 0.01;
            const double fontsize = 12.00;
            const double page_half = (page_width - (left_border * 2)) / 2;
            const double pane2 = left_border + page_half;
            var page_number = 1;
            string[] artist = {""};
            var pane = left_border;
            var current_height = page_height - top_border;
            var alternate = false;
            //add new page
            var Page = new PdfPage(Document);
            var Contents = new PdfContents(Page);
            current_height = current_height - line_height;
            foreach (var song in Songs.Where(song => song.Artist.ToLowerInvariant() != artist[0]))
            {
                alternate = !alternate;
                if (current_height <= top_border || current_height - line_height <= top_border)
                {
                    current_height = page_height - top_border - line_height;
                    if (pane == left_border)
                    {
                        pane = pane2;
                    }
                    else if (pane == pane2)
                    {
                        //new page
                        addPDFFormatBasic1(Contents, page_number);
                        Page = new PdfPage(Document);
                        Contents = new PdfContents(Page);
                        pane = left_border;
                        page_number++;
                    }
                }
                try
                {
                    if (chkAlternate.Checked && alternate)
                    {
                        Contents.SetColorNonStroking(AltColor);
                        var hilight = current_height - (line_width* 4);
                        Contents.DrawRectangle(pane, hilight, page_half, line_height, PaintOp.Fill);
                        Contents.SetColorNonStroking(Color.Black);
                    }
                    Contents.DrawText(BoldFont, fontsize, pane + song_margin, current_height, FixString(song.Artist, 44, true));
                    current_height = current_height - line_height;
                    artist[0] = song.Artist.ToLowerInvariant();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("There was an error:\n" + ex.Message, AppName, MessageBoxButtons.OK,MessageBoxIcon.Error);
                }
                if (SortByArtist)
                {
                    var songs = (from entry in Songs where entry.Artist.ToLowerInvariant() == artist[0] select entry.Name.ToString(CultureInfo.InvariantCulture)).ToList();
                    songs.Sort();
                    foreach (var line in songs)
                    {
                        if (current_height <= top_border)
                        {
                            current_height = page_height - top_border - line_height;
                            if (pane == left_border)
                            {
                                pane = pane2;
                            }
                            else if (pane == pane2)
                            {
                                //new page
                                addPDFFormatBasic1(Contents, page_number);
                                Page = new PdfPage(Document);
                                Contents = new PdfContents(Page);
                                pane = left_border;
                                page_number++;
                            }
                        }
                        try
                        {
                            if (chkAlternate.Checked && alternate)
                            {
                                Contents.SetColorNonStroking(AltColor);
                                var hilight = current_height - (line_width*4);
                                Contents.DrawRectangle(pane, hilight, page_half, line_height, PaintOp.Fill);
                                Contents.SetColorNonStroking(Color.Black);
                            }
                            Contents.DrawText(NormalFont, fontsize, pane + (song_margin * 2), current_height, FixString(line, 44));
                            current_height = current_height - line_height;
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("There was an error:\n" + ex.Message + "\n\nLine: '" + line + "'", AppName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
                else
                {
                    try
                    {
                        if (chkAlternate.Checked && alternate)
                        {
                            Contents.SetColorNonStroking(AltColor);
                            var hilight = current_height - (line_width * 4);
                            Contents.DrawRectangle(pane, hilight, page_half, line_height, PaintOp.Fill);
                            Contents.SetColorNonStroking(Color.Black);
                        }
                        Contents.DrawText(NormalFont, fontsize, pane + (song_margin * 2), current_height, FixString(song.Name, 44));
                        current_height = current_height - line_height;
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("There was an error:\n" + ex.Message, AppName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                if (!chkAlternate.Checked)
                {
                    current_height = current_height - line_height;
                }
            }
            addPDFFormatBasic1(Contents, page_number);
        }
        
        private void addPDFFormatBasic1(PdfContents Contents, int page_number)
        {
            const double top_border = 0.5;
            const double left_border = 0.6;
            const double page_height = 11.0;
            const double page_width = 8.5;
            const double line_width = 0.01;
            const double page_half = (page_width - (left_border * 2)) / 2;
            const double pane2 = left_border + page_half;
            Contents.SetLineWidth(line_width);
            Contents.DrawLine(pane2 - line_width, top_border, pane2 - line_width, page_height - top_border, line_width);
            AddHeaderFooter(Contents, page_number);
        }

        private void AddHeaderFooter(PdfContents Contents, int page_number)
        {
            const double top_border = 0.5;
            const double left_border = 0.6;
            const double page_height = 11.0;
            const double page_width = 8.5;
            const double line_height = 0.16;
            const double line_width = 0.01;
            const double footersize = 8.00;
            const double name_height = page_height - top_border + (line_height / 2);
            Contents.SetLineWidth(line_width);
            Contents.SetColorStroking(Color.Black);
            Contents.SetColorNonStroking(Color.Black);
            if (ActiveIndex != PDFStyle.espher && ActiveIndex != PDFStyle.Macst3r)
            {
                var header = chkHeaders.Checked ? setlistname.Trim() : "";
                header = header + (!string.IsNullOrWhiteSpace(header) && chkCountHeader.Checked ? " - " : "") + (chkCountHeader.Checked ? string.Format("{0:n0}", Songs.Count) + " Songs" : "");
                if (!string.IsNullOrWhiteSpace(header))
                {
                    Contents.DrawText(NormalFont, footersize, 4.25, name_height, TextJustify.Center, header);
                }
            }
            if (chkCountFooter.Checked)
            {
                Contents.DrawText(NormalFont, footersize, left_border, top_border - line_height, TextJustify.Left, string.Format("{0:n0}", Songs.Count) + " Songs");
            }
            if (chkPages.Checked)
            {
                Contents.DrawText(NormalFont, footersize, 4.25, top_border - line_height, TextJustify.Center, "Page " + page_number + " of " + TotalPageNumbers);
            }            
            if (chkBorder.Checked)
            {
                Contents.DrawRectangle(left_border - (line_width / 2), top_border - line_width, page_width - (left_border * 2), page_height - (top_border * 2) + line_width, PaintOp.CloseStroke);
            }
        }

        private void doPDFBasic2(PdfDocument Document)
        {
            const double top_border = 0.5;
            const double left_border = 0.6;
            const double song_margin = 0.1;
            const double page_height = 11.0;
            const double page_width = 8.5;
            const double line_height = 0.16;
            const double line_width = 0.01;
            const double fontsize = 10.00;
            const double page_third = (page_width - (left_border * 2)) / 3;
            const double pane2 = left_border + page_third;
            const double pane3 = left_border + (page_third * 2);
            var page_number = 1;
            string[] artist = {""};
            var pane = left_border;
            var current_height = page_height - top_border;
            var alternate = false;
            //add new page
            var Page = new PdfPage(Document);
            var Contents = new PdfContents(Page);
            current_height = current_height - line_height;
            foreach (var song in Songs.Where(song => song.Artist.ToLowerInvariant() != artist[0]))
            {
                alternate = !alternate;
                if (current_height <= top_border || current_height - line_height <= top_border)
                {
                    current_height = page_height - top_border - line_height;
                    if (pane == left_border)
                    {
                        pane = pane2;
                    }
                    else if (pane == pane2)
                    {
                        pane = pane3;
                    }
                    else if (pane == pane3)
                    {
                        //new page
                        addPDFFormatBasic2(Contents, page_number);
                        Page = new PdfPage(Document);
                        Contents = new PdfContents(Page);
                        pane = left_border;
                        page_number++;
                    }
                }
                try
                {
                    if (chkAlternate.Checked && alternate)
                    {
                        Contents.SetColorNonStroking(AltColor);
                        var hilight = current_height - (line_width*4);
                        Contents.DrawRectangle(pane, hilight, page_third - (line_width*2), line_height, PaintOp.Fill);
                        Contents.SetColorNonStroking(Color.Black);
                    }
                    Contents.DrawText(BoldFont, fontsize, pane + song_margin, current_height, FixString(song.Artist, 36, true));
                    current_height = current_height - line_height;
                    artist[0] = song.Artist.ToLowerInvariant();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("There was an error:\n" + ex.Message, AppName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                if (SortByArtist)
                {
                    var songs = (from entry in Songs where entry.Artist.ToLowerInvariant() == artist[0] select entry.Name).ToList();
                    songs.Sort();
                    foreach (var line in songs)
                    {
                        if (current_height <= top_border)
                        {
                            current_height = page_height - top_border - line_height;
                            if (pane == left_border)
                            {
                                pane = pane2;
                            }
                            else if (pane == pane2)
                            {
                                pane = pane3;
                            }
                            else if (pane == pane3)
                            {
                                //new page
                                addPDFFormatBasic2(Contents, page_number);
                                Page = new PdfPage(Document);
                                Contents = new PdfContents(Page);
                                pane = left_border;
                                page_number++;
                            }
                        }
                        try
                        {
                            if (chkAlternate.Checked && alternate)
                            {
                                Contents.SetColorNonStroking(AltColor);
                                var hilight = current_height - (line_width*4);
                                Contents.DrawRectangle(pane, hilight, page_third - (line_width*2), line_height, PaintOp.Fill);
                                Contents.SetColorNonStroking(Color.Black);
                            }
                            Contents.DrawText(NormalFont, fontsize, pane + (song_margin * 2), current_height, FixString(line, 35));
                            current_height = current_height - line_height;
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("There was an error:\n" + ex.Message, AppName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
                else
                {
                    try
                    {
                        if (chkAlternate.Checked && alternate)
                        {
                            Contents.SetColorNonStroking(AltColor);
                            var hilight = current_height - (line_width * 4);
                            Contents.DrawRectangle(pane, hilight, page_third - (line_width * 2), line_height, PaintOp.Fill);
                            Contents.SetColorNonStroking(Color.Black);
                        }
                        Contents.DrawText(NormalFont, fontsize, pane + (song_margin * 2), current_height, FixString(song.Name, 35));
                        current_height = current_height - line_height;
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("There was an error:\n" + ex.Message, AppName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                if (!chkAlternate.Checked)
                {
                    current_height = current_height - line_height;
                }
            }
            addPDFFormatBasic2(Contents, page_number);
        }

        private void addPDFFormatBasic2(PdfContents Contents, int page_number)
        {
            const double top_border = 0.5;
            const double left_border = 0.6;
            const double page_height = 11.0;
            const double page_width = 8.5;
            const double line_width = 0.01;
            const double page_third = (page_width - (left_border * 2)) / 3;
            const double pane2 = left_border + page_third;
            const double pane3 = left_border + (page_third * 2);
            Contents.SetLineWidth(line_width);
            Contents.DrawLine(pane2 - line_width, top_border, pane2 - line_width, page_height - top_border, line_width);
            Contents.DrawLine(pane3 - line_width, top_border, pane3 - line_width, page_height - top_border, line_width);
            AddHeaderFooter(Contents, page_number);
        }
        
        private void doPDFDetailed1(PdfDocument Document)
        {
            const double top_border = 0.5;
            const double left_border = 0.6;
            const double song_margin = 0.1;
            const double page_height = 11.0;
            const double line_height = 0.16;
            const double page_width = 8.5;
            const double line_width = 0.01;
            const double fontsize = 10.00;
            var page_number = 1;
            var artist = "";
            var alternate = false;
            //add new page
            var Page = new PdfPage(Document);
            var Contents = new PdfContents(Page);
            var current_height = page_height - top_border - (line_height);
            for (var i = 0; i < Songs.Count; i++)
            {
                var songs = new List<SongData>();
                if (SortByArtist)
                {
                    songs = (from entry in Songs where String.Equals(entry.Artist, Songs[i].Artist, StringComparison.InvariantCultureIgnoreCase) select entry).ToList();
                    if (songs.Count > 1)
                    {
                        songs.Sort((a, b) => String.Compare(a.Name.Replace("The ", "").ToLowerInvariant(), b.Name.Replace("The ", "").ToLowerInvariant(), StringComparison.Ordinal));
                    }
                }
                else
                {
                    songs.Add(Songs[i]);
                }
                var first = true;
                foreach (var song in songs)
                {
                    if (Songs[i].Artist.ToLowerInvariant() != artist)
                    {
                        alternate = !alternate;
                    }
                    current_height = current_height - line_height;
                    if (current_height <= top_border)
                    {
                        addPDFFormatDetailed1(Contents, page_number);
                        Page = new PdfPage(Document);
                        Contents = new PdfContents(Page);
                        current_height = page_height - top_border - (line_height * 2);
                        page_number++;
                    }
                    if (chkAlternate.Checked && alternate)
                    {
                        Contents.SetColorNonStroking(AltColor);
                        var hilight = current_height - (line_width * 4);
                        Contents.DrawRectangle(left_border, hilight, page_width - (left_border * 2), line_height, PaintOp.Fill);
                        Contents.SetColorNonStroking(Color.Black);
                    }
                    try
                    {
                        if (first || !SortByArtist || !chkArtistOnce.Checked)
                        {
                            Contents.DrawText(NormalFont, fontsize, left_border + song_margin, current_height, TextJustify.Left, FixString(song.Artist, isRB4 ? 32 : 26));
                            first = false;
                        }
                        var spacer = isRB4 ? 0.55 : 0;
                        Contents.DrawText(NormalFont, fontsize, left_border + song_margin + 1.8 + (isRB4 ? 0.275 : 0.0), current_height, TextJustify.Left, FixString(song.Name, isRB4 ? 30 : 26));
                        var harm = (song.VocalsDiff == 0 || song.VocalParts < 2 ? "None" : song.VocalParts + "-Part");
                        Contents.DrawText(NormalFont, fontsize, left_border + 3.85 + spacer, current_height, TextJustify.Center, harm);
                        Contents.DrawText(NormalFont, fontsize, left_border + 4.4 + spacer, current_height, TextJustify.Center, song.GetGender(true));
                        if (!isRB4)
                        {
                            Contents.DrawText(NormalFont, fontsize, left_border + 4.95, current_height, TextJustify.Center, song.KeysDiff > 0 ? "Yes" : "No");
                        }
                        Contents.DrawText(NormalFont, fontsize, left_border + 5.5, current_height, TextJustify.Center, Parser.GetSongDuration(song.Length.ToString(CultureInfo.InvariantCulture)));
                        Contents.DrawText(NormalFont, fontsize, left_border + 6.05, current_height, TextJustify.Center, song.GetRating());
                        Contents.DrawText(NormalFont, fontsize, left_border + 6.8, current_height, TextJustify.Center, FixString(song.Genre, 14));
                        artist = song.Artist.ToLowerInvariant();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("There was an error:\n" + ex.Message, AppName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                i += songs.Count - 1;
            }
            addPDFFormatDetailed1(Contents, page_number);
        }

        private void addPDFFormatDetailed1(PdfContents Contents, int page_number)
        {
            const double top_border = 0.5;
            const double left_border = 0.6;
            const double song_margin = 0.1;
            const double page_height = 11.0;
            const double page_width = 8.5;
            const double line_height = 0.16;
            const double line_width = 0.01;
            const double fontsize = 10.00;
            const double header_height = page_height - top_border - line_height + (line_width * 4);
            Contents.SetLineWidth(line_width);
            //draw initial stuff
            var spacer = isRB4 ? 0.55 : 0;
            Contents.SetColorStroking(Color.Black);
            Contents.SetColorNonStroking(Color.LightGray);
            Contents.DrawRectangle(left_border - (line_width / 2), page_height - top_border - line_height, page_width - (left_border * 2), line_height, PaintOp.CloseFillStroke);
            Contents.SetColorNonStroking(Color.Black);
            Contents.DrawText(BoldFont, fontsize, left_border + song_margin, header_height, TextJustify.Left, "Artist");
            Contents.DrawLine(left_border + 1.8 + (isRB4 ? 0.275 : 0.0), page_height - top_border, left_border + 1.8 + (isRB4 ? 0.275 : 0.0), top_border);
            Contents.DrawText(BoldFont, fontsize, left_border + song_margin + 1.8 + (isRB4 ? 0.275 : 0.0), header_height, TextJustify.Left, "Song");
            Contents.DrawLine(left_border + 3.6 + spacer, page_height - top_border, left_border + 3.6 + spacer, top_border);
            Contents.DrawText(BoldFont, fontsize, left_border + 3.85 + spacer, header_height, TextJustify.Center, "Harm.");
            Contents.DrawLine(left_border + 4.1 + spacer, page_height - top_border, left_border + 4.1 + spacer, top_border);
            Contents.DrawText(BoldFont, fontsize, left_border + 4.4 + spacer, header_height, TextJustify.Center, "Singer");
            if (!isRB4)
            {
                Contents.DrawLine(left_border + 4.7, page_height - top_border, left_border + 4.7, top_border);
                Contents.DrawText(BoldFont, fontsize, left_border + 4.95, header_height, TextJustify.Center, "Keys?");
            }
            Contents.DrawLine(left_border + 5.2, page_height - top_border, left_border + 5.2, top_border);
            Contents.DrawText(BoldFont, fontsize, left_border + 5.5, header_height, TextJustify.Center, "Length");
            Contents.DrawLine(left_border + 5.8, page_height - top_border, left_border + 5.8, top_border);
            Contents.DrawText(BoldFont, fontsize, left_border + 6.05, header_height, TextJustify.Center, "Rating");
            Contents.DrawLine(left_border + 6.3, page_height - top_border, left_border + 6.3, top_border);
            Contents.DrawText(BoldFont, fontsize, left_border + 6.8, header_height, TextJustify.Center, "Genre");
            AddHeaderFooter(Contents, page_number);
        }

        private void doPDFDetailed2(PdfDocument Document)
        {
            const double top_border = 0.5;
            const double left_border = 0.6;
            const double song_margin = 0.1;
            const double page_height = 11.0;
            const double line_height = 0.16;
            const double page_width = 8.5;
            const double line_width = 0.01; 
            const double fontsize = 10.00;
            var page_number = 1;
            var artist = "";
            var alternate = false;
            //add new page
            var Page = new PdfPage(Document);
            var Contents = new PdfContents(Page);
            var current_height = page_height - top_border - (line_height);
            for (var i = 0; i < Songs.Count; i++)
            {
                var songs = new List<SongData>();
                if (SortByArtist)
                {
                    songs = (from entry in Songs where String.Equals(entry.Artist, Songs[i].Artist, StringComparison.InvariantCultureIgnoreCase) select entry).ToList();
                    if (songs.Count > 1)
                    {
                        songs.Sort((a, b) => String.Compare(a.Name.Replace("The ", "").ToLowerInvariant(), b.Name.Replace("The ", "").ToLowerInvariant(), StringComparison.Ordinal));
                    }
                }
                else
                {
                    songs.Add(Songs[i]);
                }
                var first = true;
                foreach (var song in songs)
                {
                    if (song.Artist.ToLowerInvariant() != artist)
                    {
                        alternate = !alternate;
                    }
                    current_height = current_height - line_height;
                    if (current_height <= top_border)
                    {
                        addPDFFormatDetailed2(Contents, page_number);
                        Page = new PdfPage(Document);
                        Contents = new PdfContents(Page);
                        current_height = page_height - top_border - (line_height * 2);
                        page_number++;
                    }
                    if (chkAlternate.Checked && alternate)
                    {
                        Contents.SetColorNonStroking(AltColor);
                        var hilight = current_height - (line_width * 4);
                        Contents.DrawRectangle(left_border, hilight, page_width - (left_border * 2), line_height, PaintOp.Fill);
                        Contents.SetColorNonStroking(Color.Black);
                    }
                    try
                    {
                        if (first || !SortByArtist || !chkArtistOnce.Checked)
                        {
                            Contents.DrawText(NormalFont, fontsize, left_border + song_margin, current_height,TextJustify.Left, FixString(song.Artist, isRB4 ? 32 : 26));
                            first = false;
                        }
                        var spacer = isRB4 ? 0.45 : 0;
                        Contents.DrawText(NormalFont, fontsize, left_border + song_margin + 1.8 + (isRB4 ? 0.275 : 0.0), current_height, TextJustify.Left, FixString(song.Name, isRB4 ? 30 : 26));
                        Contents.DrawText(NormalFont, fontsize, left_border + 3.95 + spacer, current_height, TextJustify.Center, song.GuitarDiff == 0 ? "-" : (song.GuitarDiff).ToString(CultureInfo.InvariantCulture));
                        Contents.DrawText(NormalFont, fontsize, left_border + 4.4 + spacer, current_height, TextJustify.Center, song.BassDiff == 0 ? "-" : (song.BassDiff).ToString(CultureInfo.InvariantCulture));
                        Contents.DrawText(NormalFont, fontsize, left_border + 4.85 + spacer, current_height, TextJustify.Center, song.DrumsDiff == 0 ? "-" : (song.DrumsDiff).ToString(CultureInfo.InvariantCulture));
                        if (!isRB4)
                        {
                            Contents.DrawText(NormalFont, fontsize, left_border + 5.3, current_height, TextJustify.Center, song.KeysDiff == 0 ? "-" : (song.KeysDiff).ToString(CultureInfo.InvariantCulture));
                        }
                        Contents.DrawText(NormalFont, fontsize, left_border + 5.78, current_height, TextJustify.Center, song.VocalsDiff == 0 ? "-" : (song.VocalsDiff).ToString(CultureInfo.InvariantCulture));
                        var harm = (song.VocalsDiff == 0 || song.VocalParts < 2 ? "None" : song.VocalParts + "-Part");
                        Contents.DrawText(NormalFont, fontsize, left_border + 6.4, current_height, TextJustify.Center, harm);
                        Contents.DrawText(NormalFont, fontsize, left_border + 7.05, current_height, TextJustify.Center, Parser.GetSongDuration(song.Length.ToString(CultureInfo.InvariantCulture)));
                        artist = song.Artist.ToLowerInvariant();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("There was an error:\n" + ex.Message, AppName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                i += songs.Count - 1;
            }
            addPDFFormatDetailed2(Contents, page_number);
        }

        private void addPDFFormatDetailed2(PdfContents Contents, int page_number)
        {
            const double top_border = 0.5;
            const double left_border = 0.6;
            const double song_margin = 0.1;
            const double page_height = 11.0;
            const double page_width = 8.5;
            const double line_height = 0.16;
            const double line_width = 0.01;
            const double fontsize = 10.00;
            const double header_height = page_height - top_border - line_height + (line_width * 4);
            Contents.SetLineWidth(line_width);
            //draw initial stuff
            Contents.SetColorStroking(Color.Black);
            Contents.SetColorNonStroking(Color.LightGray);
            Contents.DrawRectangle(left_border - (line_width / 2), page_height - top_border - line_height, page_width - (left_border * 2), line_height, PaintOp.CloseFillStroke);
            Contents.SetColorNonStroking(Color.Black);
            var spacer = isRB4 ? 0.45 : 0;
            Contents.DrawText(BoldFont, fontsize, left_border + song_margin, header_height, TextJustify.Left, "Artist");
            Contents.DrawLine(left_border + 1.8 + (isRB4 ? 0.275 : 0.0), page_height - top_border, left_border + 1.8 + (isRB4 ? 0.275 : 0.0), top_border);
            Contents.DrawText(BoldFont, fontsize, left_border + song_margin + 1.8 + (isRB4 ? 0.275 : 0.0), header_height, TextJustify.Left, "Song");
            Contents.DrawLine(left_border + 3.7 + spacer, page_height - top_border, left_border + 3.7 + spacer, top_border);
            Contents.DrawText(BoldFont, fontsize, left_border + 3.95 + spacer, header_height, TextJustify.Center, "Guitar");
            Contents.DrawLine(left_border + 4.2 + spacer, page_height - top_border, left_border + 4.2 + spacer, top_border);
            Contents.DrawText(BoldFont, fontsize, left_border + 4.4 + spacer, header_height, TextJustify.Center, "Bass");
            Contents.DrawLine(left_border + 4.6 + spacer, page_height - top_border, left_border + 4.6 + spacer, top_border);
            Contents.DrawText(BoldFont, fontsize, left_border + 4.85 + spacer, header_height, TextJustify.Center, "Drums");
            if (!isRB4)
            {
                Contents.DrawLine(left_border + 5.1, page_height - top_border, left_border + 5.1, top_border);
                Contents.DrawText(BoldFont, fontsize, left_border + 5.3, header_height, TextJustify.Center, "Keys");
            }
            Contents.DrawLine(left_border + 5.53, page_height - top_border, left_border + 5.53, top_border);
            Contents.DrawText(BoldFont, fontsize, left_border + 5.78, header_height, TextJustify.Center, "Vocals");
            Contents.DrawLine(left_border + 6.05, page_height - top_border, left_border + 6.05, top_border);
            Contents.DrawText(BoldFont, fontsize, left_border + 6.4, header_height, TextJustify.Center, "Harmonies");
            Contents.DrawLine(left_border + 6.8, page_height - top_border, left_border + 6.8, top_border);
            Contents.DrawText(BoldFont, fontsize, left_border + 7.05, header_height, TextJustify.Center, "Length");
            AddHeaderFooter(Contents, page_number);
        }

        private void doPDFDetailed3(PdfDocument Document)
        {
            const double top_border = 0.5;
            const double left_border = 0.6;
            const double song_margin = 0.1;
            const double page_height = 11.0;
            const double page_width = 8.5;
            const double line_height = 0.16;
            const double line_width = 0.01;
            const double fontsize = 11.00;
            var page_number = 1;
            var artist = "";
            var alternate = false;
            var icon_path = Application.StartupPath + "\\res\\icons\\" + (chkAlternate.Checked ? "cube" : "circle") + "\\";
            const double instruments_left = left_border + 4.16 + line_width + song_margin;
            //add new page
            var Page = new PdfPage(Document);
            var Contents = new PdfContents(Page);
            var current_height = page_height - top_border - (line_height);
            //JPGs because transparent PNGs were messing up the PDF graphics???
            var nodrums = new PdfImage(Document, icon_path + "nodrums.jpg");
            var prodrums = new PdfImage(Document, icon_path + "prodrums.jpg");
            var bass = new PdfImage(Document, icon_path + "bass.jpg");
            var nobass = new PdfImage(Document, icon_path + "nobass.jpg");
            var probass = new PdfImage(Document, icon_path + "probass.jpg");
            var guitar = new PdfImage(Document, icon_path + "guitar.jpg");
            var noguitar = new PdfImage(Document, icon_path + "noguitar.jpg");
            var proguitar = new PdfImage(Document, icon_path + "proguitar.jpg");
            var keys = new PdfImage(Document, icon_path + "keys.jpg");
            var nokeys = new PdfImage(Document, icon_path + "nokeys.jpg");
            var prokeys = new PdfImage(Document, icon_path + "prokeys.jpg");
            var novocals = new PdfImage(Document, icon_path + "novocals.jpg");
            var harm1 = new PdfImage(Document, icon_path + "vocals.jpg");
            var harm2 = new PdfImage(Document, icon_path + "harm2.jpg");
            var harm3 = new PdfImage(Document, icon_path + "harm3.jpg");
            for (var i = 0; i < Songs.Count; i++)
            {
                var songs = new List<SongData>();
                if (SortByArtist)
                {
                    songs = (from entry in Songs where String.Equals(entry.Artist, Songs[i].Artist, StringComparison.InvariantCultureIgnoreCase) select entry).ToList();
                    if (songs.Count > 1)
                    {
                        songs.Sort((a, b) => String.Compare(a.Name.Replace("The ", "").ToLowerInvariant(), b.Name.Replace("The ", "").ToLowerInvariant(), StringComparison.Ordinal));
                    }
                }
                else
                {
                    songs.Add(Songs[i]);
                }
                var first = true;
                foreach (var song in songs)
                {
                    if (song.Artist.ToLowerInvariant() != artist)
                    {
                        alternate = !alternate;
                    }
                    current_height = current_height - line_height;
                    if (current_height <= top_border)
                    {
                        addPDFFormatDetailed3(Contents, page_number);
                        Page = new PdfPage(Document);
                        Contents = new PdfContents(Page);
                        current_height = page_height - top_border - (line_height * 2);
                        page_number++;
                    }
                    if (chkAlternate.Checked && alternate)
                    {
                        Contents.SetColorNonStroking(AltColor);
                        var hilight = current_height - (line_width * 4);
                        Contents.DrawRectangle(left_border, hilight, page_width - (left_border * 2), line_height, PaintOp.Fill);
                        Contents.SetColorNonStroking(Color.Black);
                    }
                    try
                    {
                        var vocals = novocals;
                        if (song.VocalsDiff > 0)
                        {
                            switch (song.VocalParts)
                            {
                                case 1:
                                    vocals = harm1;
                                    break;
                                case 2:
                                    vocals = harm2;
                                    break;
                                case 3:
                                    vocals = harm3;
                                    break;
                            }
                        }
                        if (first || !SortByArtist || !chkArtistOnce.Checked)
                        {
                            Contents.DrawText(NormalFont, fontsize, left_border + song_margin, current_height, TextJustify.Left, FixString(song.Artist, isRB4 ? 32 : 27));
                            first = false;
                        }
                        var spacer = isRB4 ? 0.17 : 0;
                        Contents.DrawText(NormalFont, fontsize, left_border + song_margin + 2.08 + (spacer/2), current_height, TextJustify.Left, FixString(song.Name, isRB4 ? 29 : 27));
                        Contents.DrawImage((song.GuitarDiff > 0 ? (!isRB4 && !isBlitz && song.ProGuitarDiff > 0 ? proguitar : guitar) : noguitar), instruments_left + spacer, current_height - (line_width * 4), 0.15, 0.15);
                        Contents.DrawImage((song.BassDiff > 0 ? (!isRB4 && !isBlitz && song.ProBassDiff > 0 ? probass : bass) : nobass), instruments_left + spacer + 0.17, current_height - (line_width * 4), 0.15, 0.15);
                        Contents.DrawImage((song.DrumsDiff > 0 ? prodrums : nodrums), instruments_left + spacer + (0.17 * 2), current_height - (line_width * 4), 0.15, 0.15);
                        if (!isRB4)
                        {
                            Contents.DrawImage((song.KeysDiff > 0 ? (!isBlitz && song.ProKeysDiff > 0 ? prokeys : keys) : nokeys), instruments_left + (0.17 * 3), current_height - (line_width * 4), 0.15, 0.15);
                        }
                        Contents.DrawImage(vocals, instruments_left + spacer + (0.17 * (isRB4 ? 3 : 4)), current_height - (line_width * 4), 0.15, 0.15);
                        Contents.DrawText(NormalFont, fontsize, left_border + 5.75, current_height, TextJustify.Center, FixString(song.Genre, 15));
                        Contents.DrawText(NormalFont, fontsize - 0.5, left_border + 6.55, current_height, TextJustify.Center, song.GetGender(true));
                        Contents.DrawText(NormalFont, fontsize, left_border + 7.05, current_height, TextJustify.Center, Parser.GetSongDuration(song.Length.ToString(CultureInfo.InvariantCulture)));
                        artist = song.Artist.ToLowerInvariant();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("There was an error:\n" + ex.Message, AppName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                i += songs.Count - 1;
            }
            addPDFFormatDetailed3(Contents, page_number);
        }

        private void addPDFFormatDetailed3(PdfContents Contents, int page_number)
        {
            const double top_border = 0.5;
            const double left_border = 0.6;
            const double song_margin = 0.1;
            const double page_height = 11.0;
            const double page_width = 8.5;
            const double line_height = 0.16;
            const double line_width = 0.01;
            const double fontsize = 10.00;
            const double header_height = page_height - top_border - line_height + (line_width * 4);
            Contents.SetLineWidth(line_width);
            //draw initial stuff
            Contents.SetColorStroking(Color.Black);
            Contents.SetColorNonStroking(Color.LightGray);
            Contents.DrawRectangle(left_border - (line_width / 2), page_height - top_border - line_height, page_width - (left_border * 2), line_height, PaintOp.CloseFillStroke);
            Contents.SetColorNonStroking(Color.Black);
            var spacer = isRB4 ? 0.17 : 0;
            Contents.DrawText(BoldFont, fontsize, left_border + song_margin, header_height, TextJustify.Left, "Artist");
            Contents.DrawLine(left_border + 2.08 + (spacer / 2), page_height - top_border, left_border + 2.08 + (spacer / 2), top_border);
            Contents.DrawText(BoldFont, fontsize, left_border + song_margin + 2.08 + (spacer / 2), header_height, TextJustify.Left, "Song");
            Contents.DrawLine(left_border + 4.16 + spacer, page_height - top_border, left_border + 4.16 + spacer, top_border);
            Contents.DrawText(BoldFont, fontsize, left_border + 4.68 + (spacer/2), header_height, TextJustify.Center, "Instruments");
            Contents.DrawLine(left_border + 5.2, page_height - top_border, left_border + 5.2, top_border);
            Contents.DrawText(BoldFont, fontsize, left_border + 5.75, header_height, TextJustify.Center, "Genre");
            Contents.DrawLine(left_border + 6.3, page_height - top_border, left_border + 6.3, top_border);
            Contents.DrawText(BoldFont, fontsize, left_border + 6.55, header_height, TextJustify.Center, "Singer");
            Contents.DrawLine(left_border + 6.8, page_height - top_border, left_border + 6.8, top_border);
            Contents.DrawText(BoldFont, fontsize, left_border + 7.05, header_height, TextJustify.Center, "Length");
            AddHeaderFooter(Contents, page_number);
        }

        private void doPDFEspher(PdfDocument Document)
        {
            const double top_border = 0.17;
            const double left_border = 0.59;
            const double right_border = 0.43;
            const double song_margin = 0.17;
            const double page_height = 11.0;
            const double line_height = 0.17;
            const double page_width = 8.5;
            const double line_width = 0.01;
            const double fontsize = 8.00;
            var page_number = 1;
            //add new page
            var Page = new PdfPage(Document);
            var Contents = new PdfContents(Page);
            var current_height = page_height - top_border - (line_height * 4);
            for (var i = 0; i < Songs.Count; i++)
            {
                var songs = (from entry in Songs where String.Equals(entry.Artist, Songs[i].Artist, StringComparison.InvariantCultureIgnoreCase) select entry).ToList();
                if (songs.Count > 1)
                {
                    songs.Sort((a, b) => String.Compare(a.Name.Replace("The ", "").ToLowerInvariant(), b.Name.Replace("The ", "").ToLowerInvariant(), StringComparison.Ordinal));
                }
                var first = true;
                foreach (var song in songs)
                {
                    var limit = chkCountFooter.Checked || chkPages.Checked ? top_border + line_height : top_border; 
                    current_height -= line_height;
                    if (current_height <= limit)
                    {
                        addPDFFormatEspher(Document, Contents, page_number);
                        Page = new PdfPage(Document);
                        Contents = new PdfContents(Page);
                        current_height = page_height - top_border - (line_height * 3);
                        page_number++;
                        if (!first)
                        {
                            if (chkAlternate.Checked)
                            {
                                Contents.SetColorNonStroking(AltColor);
                                var hilight = current_height - (line_width * 4);
                                Contents.DrawRectangle(left_border, hilight, page_width - left_border - right_border, line_height, PaintOp.Fill);
                                Contents.SetColorNonStroking(Color.White);
                                Contents.DrawRectangle(left_border + 6.75, hilight, 0.05, line_height, PaintOp.Fill);
                                Contents.DrawRectangle(left_border + 7.15, hilight, 0.05, line_height, PaintOp.Fill);
                                Contents.SetColorNonStroking(Color.Black);
                            }
                            Contents.DrawText(BoldFont, fontsize, left_border, current_height, TextJustify.Left, FixString(song.Artist + " (continued)", 200, true));
                            current_height -= line_height;
                        }
                    }
                    try
                    {
                        if (first)
                        {
                            if (current_height <= limit + line_height)
                            {
                                addPDFFormatEspher(Document, Contents, page_number);
                                Page = new PdfPage(Document);
                                Contents = new PdfContents(Page);
                                current_height = page_height - top_border - (line_height*3);
                                page_number++;
                            }
                            if (chkAlternate.Checked)
                            {
                                Contents.SetColorNonStroking(AltColor);
                                var hilight = current_height - (line_width*4);
                                Contents.DrawRectangle(left_border, hilight, page_width - left_border - right_border, line_height, PaintOp.Fill);
                                Contents.SetColorNonStroking(Color.White);
                                Contents.DrawRectangle(left_border + 6.75, hilight, 0.05, line_height, PaintOp.Fill);
                                Contents.DrawRectangle(left_border + 7.15, hilight, 0.05, line_height, PaintOp.Fill);
                                Contents.SetColorNonStroking(Color.Black);
                            }
                            Contents.DrawText(BoldFont, fontsize, left_border, current_height, TextJustify.Left, FixString(song.Artist, 200, true));
                            first = false;
                            current_height -= line_height;
                        }
                        Contents.DrawText(BoldFont, fontsize, left_border + song_margin, current_height, TextJustify.Left, FixString(song.Name, 48, true));
                        if (string.IsNullOrWhiteSpace(song.Album))
                        {
                            Contents.DrawText(ItalicFont, fontsize, left_border + song_margin + 2.6, current_height, TextJustify.Left, "No Album");
                        }
                        else
                        {
                            Contents.DrawText(NormalFont, fontsize, left_border + song_margin + 2.6, current_height, TextJustify.Left, FixString(song.Album, isRB4 ? 35 : 31));
                        }
                        var spacer = isRB4 ? 0.3 : 0;
                        Contents.DrawText(NormalFont, fontsize, left_border + 4.4 + spacer, current_height, TextJustify.Center, song.YearReleased.ToString(CultureInfo.InvariantCulture));
                        Contents.DrawText(NormalFont, fontsize, left_border + 4.9 + spacer, current_height, TextJustify.Center, Parser.GetSongDuration(song.Length.ToString(CultureInfo.InvariantCulture)));
                        Contents.DrawText(NormalFont, fontsize, left_border + 5.4 + spacer, current_height, TextJustify.Center, song.GuitarDiff == 0 ? "-" : (song.GuitarDiff).ToString(CultureInfo.InvariantCulture));
                        Contents.DrawText(NormalFont, fontsize, left_border + 5.7 + spacer, current_height, TextJustify.Center, song.BassDiff == 0 ? "-" : (song.BassDiff).ToString(CultureInfo.InvariantCulture));
                        Contents.DrawText(NormalFont, fontsize, left_border + 6.0 + spacer, current_height, TextJustify.Center, song.DrumsDiff == 0 ? "-" : (song.DrumsDiff).ToString(CultureInfo.InvariantCulture));
                        if (!isRB4)
                        {
                            Contents.DrawText(NormalFont, fontsize, left_border + 6.3, current_height, TextJustify.Center, song.KeysDiff == 0 ? "-" : (song.KeysDiff).ToString(CultureInfo.InvariantCulture));
                        }
                        Contents.DrawText(NormalFont, fontsize, left_border + 6.6, current_height, TextJustify.Center, song.VocalsDiff == 0 ? "-" : (song.VocalsDiff).ToString(CultureInfo.InvariantCulture));
                        var harm = "";
                        switch (song.VocalParts)
                        {
                            case 0:
                                harm = "N/A";
                                break;
                            case 2:
                                harm = "2";
                                break;
                            case 3:
                                harm = "3";
                                break;
                        }
                        Contents.DrawText(NormalFont, fontsize, left_border + (ActiveIndex == PDFStyle.Macst3r ? 7.05 : 6.99), current_height, TextJustify.Center, harm);
                        Contents.DrawRectangle(left_border + 7.3, current_height, 0.1, 0.1, PaintOp.CloseStroke);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("There was an error:\n" + ex.Message, AppName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                i += songs.Count - 1;
            }
            addPDFFormatEspher(Document, Contents, page_number);
        }

        private void addPDFFormatEspher(PdfDocument Document, PdfContents Contents, int page_number)
        {
            const double top_border = 0.17;
            const double left_border = 0.59;
            const double right_border = 0.43;
            const double song_margin = 0.17;
            const double page_height = 11.0;
            const double line_height = 0.17;
            const double page_width = 8.5;
            const double line_width = 0.01;
            const double fontsize = 8.00;
            var header_height = page_height - top_border - line_height;
            Contents.SetColorStroking(Color.Black);
            if (page_number == 1 && (chkHeaders.Checked || chkCountHeader.Checked))
            {
                if (chkHeaders.Checked)
                {
                    Contents.DrawText(BoldFont, 24.00, left_border, header_height - line_height + (line_height / 2), TextJustify.Left, setlistname.Length > 22? setlistname.Substring(0,22) : setlistname);
                }
                header_height -= line_height;
                if (chkCountHeader.Checked)
                {
                    Contents.DrawText(NormalFont, fontsize, left_border + 4.4, header_height + (line_height/2),TextJustify.Left, "Total Number of Songs:");
                    Contents.DrawText(NormalFont, fontsize, left_border + 6.0, header_height + (line_height/2),TextJustify.Center, string.Format("{0:n0}", Songs.Count));
                }
                header_height -= line_height;
            }
            //draw initial stuff
            Contents.SetColorStroking(Color.Black);
            Contents.SetColorNonStroking(Tools.DarkenColor(AltColor));
            Contents.DrawRectangle(left_border, header_height - (line_width * 4), page_width - left_border - right_border, line_height, PaintOp.Fill);
            Contents.SetColorNonStroking(Color.White);
            Contents.DrawRectangle(left_border + 6.75, header_height - (line_width * 4), 0.05, line_height, PaintOp.Fill);
            Contents.DrawRectangle(left_border + 7.15, header_height - (line_width * 4), 0.05, line_height, PaintOp.Fill);
            Contents.SetColorNonStroking(Color.Black);
            var spacer = isRB4 ? 0.3 : 0;
            Contents.DrawText(BoldFont, fontsize, left_border, header_height, TextJustify.Left, "Artist");
            Contents.DrawText(BoldFont, fontsize, left_border + 5.4 + spacer, header_height, TextJustify.Left, "Difficulties");
            header_height -= line_height;
            Contents.SetColorNonStroking(Tools.DarkenColor(AltColor));
            Contents.DrawRectangle(left_border, header_height - (line_width * 4), page_width - left_border - right_border, line_height, PaintOp.Fill);
            Contents.SetColorNonStroking(Color.White);
            Contents.DrawRectangle(left_border + 6.75, header_height - (line_width * 4), 0.05, line_height, PaintOp.Fill);
            Contents.DrawRectangle(left_border + 7.15, header_height - (line_width * 4), 0.05, line_height, PaintOp.Fill);
            Contents.SetColorNonStroking(Color.Black);
            Contents.DrawText(BoldFont, fontsize, left_border + song_margin, header_height, TextJustify.Left, "Song");
            Contents.DrawText(BoldFont, fontsize, left_border + song_margin + 2.6, header_height, TextJustify.Left, "Album");
            Contents.DrawText(BoldFont, fontsize, left_border + 4.4 + spacer, header_height, TextJustify.Center, "Year");
            Contents.DrawText(BoldFont, fontsize, left_border + 4.9 + spacer, header_height, TextJustify.Center, "Length");
            Contents.DrawText(BoldFont, fontsize, left_border + 5.4 + spacer, header_height, TextJustify.Center, "G");
            Contents.DrawText(BoldFont, fontsize, left_border + 5.7 + spacer, header_height, TextJustify.Center, "B");
            Contents.DrawText(BoldFont, fontsize, left_border + 6.0 + spacer, header_height, TextJustify.Center, "D");
            if (!isRB4)
            {
                Contents.DrawText(BoldFont, fontsize, left_border + 6.3, header_height, TextJustify.Center, "K");
            }
            Contents.DrawText(BoldFont, fontsize, left_border + 6.6, header_height, TextJustify.Center, "V");
            Contents.DrawText(BoldFont, fontsize, left_border + (ActiveIndex == PDFStyle.Macst3r ? 7.05 : 6.99), header_height, TextJustify.Center, "H");
            try
            {
                var funnyFont = new PdfFont(Document, "Webdings", FontStyle.Regular);
                Contents.DrawText(funnyFont, fontsize, left_border + 7.35, header_height, TextJustify.Center, " Y ");
            }
            catch (Exception) //Webdings not installed
            {
                Contents.DrawText(BoldFont, fontsize, left_border + 7.35, header_height, TextJustify.Center, "Fav");
            }
            AddHeaderFooter(Contents, page_number);
        }

        private void doPDFMacst3r(PdfDocument Document)
        {
            const double top_border = 0.17;
            const double left_border = 0.59;
            const double right_border = 0.43;
            const double page_height = 11.0;
            const double line_height = 0.17;
            const double page_width = 8.5;
            const double line_width = 0.01;
            const double fontsize = 8.00;
            const double artist_left = left_border + 0.37;
            const double check_left = left_border + 0.13;
            const double song_left = left_border + 0.56;
            const double album_left = left_border + 2.88;
            const double year_left = left_border + 4.57;
            const double length_left = left_border + 5.06;
            var page_number = 1;
            //add new page
            var Page = new PdfPage(Document);
            var Contents = new PdfContents(Page);
            var current_height = page_height - top_border - (line_height * 4);
            for (var i = 0; i < Songs.Count; i++)
            {
                var songs = (from entry in Songs where String.Equals(entry.Artist, Songs[i].Artist, StringComparison.InvariantCultureIgnoreCase) select entry).ToList();
                if (songs.Count > 1)
                {
                    songs.Sort((a, b) => String.Compare(a.Name.Replace("The ", "").ToLowerInvariant(), b.Name.Replace("The ", "").ToLowerInvariant(), StringComparison.Ordinal));
                }
                var first = true;
                foreach (var song in songs)
                {
                    var limit = chkCountFooter.Checked || chkPages.Checked ? top_border + line_height : top_border;
                    current_height -= line_height;
                    if (current_height <= limit)
                    {
                        addPDFFormatMacst3r(Document, Contents, page_number);
                        Page = new PdfPage(Document);
                        Contents = new PdfContents(Page);
                        current_height = page_height - top_border - (line_height * 3);
                        page_number++;
                        if (!first)
                        {
                            if (chkAlternate.Checked)
                            {
                                Contents.SetColorNonStroking(AltColor);
                                var hilight = current_height - (line_width * 4);
                                Contents.DrawRectangle(left_border, hilight, page_width - left_border - right_border, line_height, PaintOp.Fill);
                                Contents.SetColorNonStroking(Color.Black);
                            }
                            Contents.DrawText(BoldFont, fontsize, artist_left, current_height, TextJustify.Left, FixString(song.Artist + " (continued)", 200, true));
                            current_height -= line_height;
                        }
                    }
                    try
                    {
                        if (first)
                        {
                            if (current_height <= limit + line_height)
                            {
                                addPDFFormatMacst3r(Document, Contents, page_number);
                                Page = new PdfPage(Document);
                                Contents = new PdfContents(Page);
                                current_height = page_height - top_border - (line_height * 3);
                                page_number++;
                            }
                            if (chkAlternate.Checked)
                            {
                                Contents.SetColorNonStroking(AltColor);
                                var hilight = current_height - (line_width * 4);
                                Contents.DrawRectangle(left_border, hilight, page_width - left_border - right_border, line_height, PaintOp.Fill);
                                Contents.SetColorNonStroking(Color.Black);
                            }
                            Contents.DrawText(BoldFont, fontsize, artist_left, current_height, TextJustify.Left, FixString(song.Artist, 200, true));
                            first = false;
                            current_height -= line_height;
                        }
                        Contents.DrawRectangle(check_left, current_height, 0.1, 0.1, PaintOp.CloseStroke);
                        Contents.DrawText(BoldFont, fontsize, song_left, current_height, TextJustify.Left, FixString(song.Name, 44, true));
                        if (string.IsNullOrWhiteSpace(song.Album))
                        {
                            Contents.DrawText(ItalicFont, fontsize, album_left, current_height, TextJustify.Left, "No Album");
                        }
                        else
                        {
                            Contents.DrawText(NormalFont, fontsize, album_left, current_height, TextJustify.Left, FixString(song.Album, isRB4 ? 35 : 31));
                        }
                        var spacer = isRB4 ? line_height * 1.7 : 0.0;
                        Contents.DrawText(NormalFont, fontsize, year_left + spacer, current_height, TextJustify.Center, song.YearReleased.ToString(CultureInfo.InvariantCulture));
                        Contents.DrawText(NormalFont, fontsize, length_left + spacer, current_height, TextJustify.Center, Parser.GetSongDuration(song.Length.ToString(CultureInfo.InvariantCulture)));
                        
                        spacer = line_height * 1.7;
                        var multiplier = isRB4 ? 1 : 0;
                        const double inst_start = 5.71;
                        Contents.DrawText(NormalFont, fontsize, left_border + inst_start + (spacer * multiplier), current_height, TextJustify.Center, song.VocalsDiff == 0 ? "-" : (song.VocalsDiff).ToString(CultureInfo.InvariantCulture));
                        var harm = "-";
                        switch (song.VocalParts)
                        {
                            case 2:
                                harm = "2";
                                break;
                            case 3:
                                harm = "3";
                                break;
                        }
                        multiplier++;
                        Contents.DrawText(NormalFont, fontsize, left_border + inst_start + (spacer * multiplier), current_height, TextJustify.Center, harm);
                        multiplier++;
                        Contents.DrawText(NormalFont, fontsize, left_border + inst_start + (spacer * multiplier), current_height, TextJustify.Center, song.GuitarDiff == 0 ? "-" : (song.GuitarDiff).ToString(CultureInfo.InvariantCulture));
                        multiplier++;
                        Contents.DrawText(NormalFont, fontsize, left_border + inst_start + (spacer * multiplier), current_height, TextJustify.Center, song.BassDiff == 0 ? "-" : (song.BassDiff).ToString(CultureInfo.InvariantCulture));
                        multiplier++;
                        Contents.DrawText(NormalFont, fontsize, left_border + inst_start + (spacer * multiplier), current_height, TextJustify.Center, song.DrumsDiff == 0 ? "-" : (song.DrumsDiff).ToString(CultureInfo.InvariantCulture));
                        if (!isRB4)
                        {
                            multiplier++;
                            Contents.DrawText(NormalFont, fontsize, left_border + inst_start + (spacer*multiplier), current_height, TextJustify.Center, song.KeysDiff == 0 ? "-" : (song.KeysDiff).ToString(CultureInfo.InvariantCulture));
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("There was an error:\n" + ex.Message, AppName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                i += songs.Count - 1;
            }
            addPDFFormatMacst3r(Document, Contents, page_number);
        }

        private void addPDFFormatMacst3r(PdfDocument Document, PdfContents Contents, int page_number)
        {
            const double top_border = 0.17;
            const double left_border = 0.59;
            const double right_border = 0.43;
            const double page_height = 11.0;
            const double line_height = 0.17;
            const double page_width = 8.5;
            const double line_width = 0.01;
            const double fontsize = 8.00;
            const double artist_left = left_border + 0.37;
            const double check_left = left_border + 0.18;
            const double song_left = left_border + 0.56;
            const double album_left = left_border + 2.88;
            const double year_left = left_border + 4.57;
            const double length_left = left_border + 5.06;
            var header_height = page_height - top_border - line_height;

            var icon_path = Application.StartupPath + "\\res\\icons\\cube\\";
            var drums = new PdfImage(Document, icon_path + "drums.jpg");
            var bass = new PdfImage(Document, icon_path + "bass.jpg");
            var guitar = new PdfImage(Document, icon_path + "guitar.jpg");
            var keys = new PdfImage(Document, icon_path + "keys.jpg");
            var vocals = new PdfImage(Document, icon_path + "vocals.jpg");
            var harm3 = new PdfImage(Document, icon_path + "harm3.jpg");
            
            Contents.SetColorStroking(Color.Black);
            if (page_number == 1 && (chkHeaders.Checked || chkCountHeader.Checked))
            {
                if (chkHeaders.Checked)
                {
                    Contents.DrawText(NormalFont, fontsize, left_border, header_height, TextJustify.Left, setlistname);
                }
                header_height -= line_height;
                if (chkCountHeader.Checked)
                {
                    Contents.DrawText(NormalFont, fontsize, left_border + 0.30, header_height + (line_height/3), TextJustify.Left, "(" + string.Format("{0:n0}", Songs.Count) + " Songs)");
                }
                header_height -= line_height;
            }
            //draw initial stuff
            Contents.SetColorStroking(Color.Black);
            Contents.SetColorNonStroking(Tools.DarkenColor(AltColor));
            Contents.DrawRectangle(left_border, header_height - (line_width * 4), page_width - left_border - right_border, line_height, PaintOp.Fill);
            Contents.SetColorNonStroking(Color.Black);
            Contents.DrawText(BoldFont, fontsize, artist_left, header_height, TextJustify.Left, "Artist");
            Contents.DrawText(BoldFont, fontsize, left_border + 6.29, header_height, TextJustify.Left, "Difficulties");
            header_height -= line_height;
            Contents.SetColorNonStroking(Tools.DarkenColor(AltColor));
            Contents.DrawRectangle(left_border, header_height - (line_width * 4), page_width - left_border - right_border, line_height, PaintOp.Fill);
            Contents.SetColorNonStroking(Color.Black);
            try
            {
                var funnyFont = new PdfFont(Document, "Webdings", FontStyle.Regular);
                Contents.DrawText(funnyFont, fontsize, check_left, header_height, TextJustify.Center, " Y ");
            }
            catch (Exception) //Webdings not installed
            {
                Contents.DrawText(BoldFont, fontsize, check_left, header_height, TextJustify.Center, "Fav");
            }

            var spacer = isRB4 ? line_height*1.7 : 0.0;
            Contents.DrawText(BoldFont, fontsize, song_left, header_height, TextJustify.Left, "Song");
            Contents.DrawText(BoldFont, fontsize, album_left, header_height, TextJustify.Left, "Album");
            Contents.DrawText(BoldFont, fontsize, year_left + spacer, header_height, TextJustify.Center, "Year");
            Contents.DrawText(BoldFont, fontsize, length_left + spacer, header_height, TextJustify.Center, "Length");
            spacer = line_height * 1.7;
            var multiplier = isRB4 ? 1 : 0;
            Contents.DrawImage(vocals, left_border + 5.63 + (spacer * multiplier), header_height - 0.04, line_height, line_height);
            multiplier++;
            Contents.DrawImage(harm3, left_border + 5.63 + (spacer * multiplier), header_height - 0.04, line_height, line_height);
            multiplier++;
            Contents.DrawImage(guitar, left_border + 5.63 + (spacer * multiplier), header_height - 0.04, line_height, line_height);
            multiplier++;
            Contents.DrawImage(bass, left_border + 5.63 + (spacer * multiplier), header_height - 0.04, line_height, line_height);
            multiplier++;
            Contents.DrawImage(drums, left_border + 5.63 + (spacer * multiplier), header_height - 0.04, line_height, line_height);
            if (!isRB4)
            {
                multiplier++;
                Contents.DrawImage(keys, left_border + 5.63 + (spacer*multiplier), header_height - 0.04, line_height, line_height);
            }
            AddHeaderFooter(Contents, page_number);
        }

        private string FixString(string line, int max, bool isBold = false)
        {
            var cut = line;
            if (isBold)
            {
                max = max - 3;
            }
            //sizing was done with Times, different fonts need adjusting
            var index = 0;
            cboFonts.Invoke(new MethodInvoker(() => index = cboFonts.SelectedIndex));
            switch (index)
            {
                case 0:
                case 2:
                    max = max - 3;
                    break;
                case 1:
                    max = max + 1;
                    break;
            }
            if (cut.Length > max)
            {
                cut = cut.Substring(0, max - 2) + "...";
            }
            //the following is not supported by this PDF class
            cut = cut.Replace("�", " ");
            return cut;
        }
        
        private bool ExportJSON()
        {
            Tools.DeleteFile(export_path);
            var sw = new StreamWriter(export_path, false, System.Text.Encoding.UTF8);
            var del = ",";
            var i = 0;
            try
            {
                sw.WriteLine("{");
                sw.WriteLine("\"data\":");
                sw.WriteLine("[");
                foreach (var song in Songs)
                {
                    //minimal
                    sw.WriteLine("[");
                    sw.WriteLine("\"" + song.Artist.Replace("\"", "\\\"") + "\"" + del);
                    sw.WriteLine("\"" + song.Name.Replace("\"", "\\\"") + "\"" + del);
                    sw.WriteLine("\"" + song.VocalParts + "\"" + del);
                    sw.WriteLine("\"" + Parser.GetSongDuration(song.Length.ToString(CultureInfo.InvariantCulture)) + "\"" + (radioDefaultJSON.Checked ? del : ""));
                    
                    //default
                    if (radioDefaultJSON.Checked)
                    {
                        sw.WriteLine("\"" + FormatDiff(song.DrumsDiff, false, true) + "\"" + del);
                        sw.WriteLine("\"" + FormatDiff(song.BassDiff, false, true) + "\"" + del);
                        sw.WriteLine("\"" + FormatDiff(song.GuitarDiff, false, true) + "\"" + del);
                        sw.WriteLine("\"" + (isRB4 ? FormatDiff(0, false, true) : FormatDiff(song.KeysDiff, false, true)) + "\"" + del);
                        sw.WriteLine("\"" + FormatDiff(song.VocalsDiff, false, true) + "\"" + del);
                        sw.WriteLine("\"" + song.GetRating() + "\"" + del);
                        sw.WriteLine("\"" + song.Genre + "\"" + del);
                        sw.WriteLine("\"" + song.Album.Replace("\"", "\\\"") + "\"" + "");
                    }             
                    i++;
                    if (i == Songs.Count)
                    {
                        sw.WriteLine("]"); //last entry doesn't take a comma
                    }
                    else
                    {
                        sw.WriteLine("]" + del);                         
                    }                    
                }
                sw.WriteLine("]");
                sw.WriteLine("}");
                sw.Dispose();
                return true;
            }
            catch (Exception ex)
            {
                sw.Dispose();
                MessageBox.Show("Error exporting JSON file: \n" + ex.Message, AppName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }    
        }

        private bool ExportCSV()
        {
            Tools.DeleteFile(export_path);
            var sw = new StreamWriter(export_path, false, System.Text.Encoding.UTF8);
            var del = radioTab.Checked ? "\t" : ",";
            try
            {
                if (chkHeadersCSV.Checked)
                {
                    //if writing full difficult names, let's expand the header, otherwise just put inst. name
                    var diff = radioTierNamesCSV.Checked ? " Diff." : "";
                    var line = "\"Artist\"" + del + "\"Song Title\"" + del + "\"Vocal Parts\"" + del + "\"Duration\"";
                    if (radioDefaultCSV.Checked || radioEverythingCSV.Checked)
                    {
                        line = line + del + "\"Drums\"" + diff + del + "\"Bass\"" + diff + del + "\"Guitar\"" + diff + del + (!isRB4 ? "\"Keys\"" + diff + del : "") + "\"Vocals\"" + diff +
                               del + "\"Genre\"" + del + "\"Rating\"" + del + "\"Album Name\"";
                    }
                    if (radioEverythingCSV.Checked)
                    {
                        line = line + del + "\"Track #\"" + del + "\"Master?\"" + del + "\"Year Released\"" + del + "\"Year Recorded\"" + del + 
                            (!isRB4 && !isBlitz ? "\"Pro Bass\"" + diff + del + "\"Pro Bass Tuning\"" + del + "\"Pro Guitar\"" + diff + del + 
                            "\"Pro Guitar Tuning\"" + del + "\"Pro Keys\"" + diff + del : "") + 
                            "\"Band\"" + diff + del + "\"Singer\"" + del + "\"Source\"" + 
                            (!isRB4 ? del + "\"Preview Start\"" + del + "\"Preview End\"" + del + "\"Game Version\"" + del + "\"Tonic Note\"" + del + 
                            "\"Tonality\"" + del + "\"Scroll Speed\"" + del + "\"Percussion Bank\"" + del + "\"Drum Bank\"" + del + "\"Song ID\"" + del + 
                            "\"Wipe Proof\"" + del + "\"Short Name\"" + del + "\"File Path\"" : "");
                    }
                    sw.WriteLine(line);
                }
                foreach (var song in Songs)
                {
                    var line = "\"" + song.Artist.Replace("\"", "\"\"") + "\"" + del + "\"" + song.Name.Replace("\"", "\"\"") + "\"" + del + "\"" + song.VocalParts + "\"" + 
                        del + "\"" + Parser.GetSongDuration(song.Length.ToString(CultureInfo.InvariantCulture)) + "\"";
                    if (radioDefaultCSV.Checked || radioEverythingCSV.Checked)
                    {
                        line = line + del + "\"" + FormatDiff(song.DrumsDiff, true) + "\"" + del + "\"" + FormatDiff(song.BassDiff, true) +
                            "\"" + del + "\"" + FormatDiff(song.GuitarDiff, true) + "\"" + (!isRB4 ? del + "\"" + FormatDiff(song.KeysDiff, true) + "\"" : "") + del +
                               "\"" + FormatDiff(song.VocalsDiff, true) + "\"" + del + "\"" + song.Genre + "\"" + del + "\"" + song.GetRating() + "\"" + del +
                               "\"" + song.Album.Replace("\"", "\"\"") + "\"";
                    }
                    if (radioEverythingCSV.Checked)
                    {
                        var isCustom = song.SongId.ToString(CultureInfo.InvariantCulture).StartsWith("11844", StringComparison.Ordinal) ||
                        song.SongId.ToString(CultureInfo.InvariantCulture).StartsWith("10746", StringComparison.Ordinal);
                        line = line + del + "\"" + song.TrackNumber + "\"" + del + "\"" + song.IsMaster() + "\"" + del + "\"" + song.YearReleased + "\"" + del +
                               "\"" + song.YearRecorded + "\"" + 
                               (!isRB4 && !isBlitz ? del + "\"" + FormatDiff(song.ProBassDiff, true) + "\"" + del + "\"" + song.ProBassTuning + "\"" + del +
                               "\"" + FormatDiff(song.ProGuitarDiff, true) + "\"" + del + "\"" + song.ProGuitarTuning + "\"" + del + "\"" +
                               FormatDiff(song.ProKeysDiff, true) + "\"" : "") + del + "\"" + FormatDiff(song.BandDiff, true) + "\"" + del + "\"" + song.GetGender(true) + 
                               "\"" + del + "\"" + song.GetSource() + 
                               (!isRB4 ? "\"" + del + "\"" + Parser.GetSongDuration(song.PreviewStart.ToString(CultureInfo.InvariantCulture)) + "\"" + del +
                               "\"" + Parser.GetSongDuration(song.PreviewEnd.ToString(CultureInfo.InvariantCulture)) + "\"" + del + "\"" + song.GameVersion + "\"" + del +
                               "\"" + song.TonicNote + "\"" + del + "\"" + song.Tonality + "\"" + del + "\"" + song.ScrollSpeed + "\"" + del + "\"" + song.PercussionBank +
                               "\"" + del + "\"" + song.DrumBank + "\"" + del + "\"" + song.SongId + "\"" + del + "\"" + (isCustom ? "No" : "Yes") + "\"" + del + "\"" + song.ShortName 
                               + "\"" + del + "\"" + song.FilePath + "\"" : "");
                    }
                    sw.WriteLine(line);
                }
                sw.Dispose();
                return true;
            }
            catch (Exception ex)
            {
                sw.Dispose();
                MessageBox.Show("Error exporting CSV file: \n" + ex.Message, AppName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }    
        }

        private bool ExportExcel(bool CreateTempFile = false)
        {
            var excel = new CreateExcelFile();
            var ds = new DataSet();
            var dt1 = new DataTable(setlistname);
            excel.TruncateLongStrings = chkTruncate.Checked;

            var stringType = Type.GetType("System.String");
            var decimalType = Type.GetType("System.Decimal");
            if (stringType == null || decimalType == null) return false;

            //minimal
            dt1.Columns.Add("Artist", stringType);
            dt1.Columns.Add("Song Title", stringType);
            dt1.Columns.Add("V. Parts", decimalType);
            dt1.Columns.Add("Duration", stringType);

            //required after my modications to code, but now allows us to specify width of columns
            //this is Excel size, NOT pixels - it's about 7 * value = actual pixels = 30 below = 210 pixels
            var colWidths = new List<double> { 30, 30, 8, 9, };
            
            //if writing full difficult names, let's expand the header, otherwise just put isnt. name
            var diff_name= radioTierNamesExcel.Checked ? " Diff." : "";

            //need the right type so excel won't complain that "number is stored as string"
            var diff_type = radioTierNamesExcel.Checked ? stringType : decimalType;

            //need to change sizing of the excel columns based on info
            var diff_width = radioTierNamesExcel.Checked ? 12 : 7;

            //default
            if (radioDefaultExcel.Checked || radioEverythingExcel.Checked)
            {
                dt1.Columns.Add("Drums" + diff_name, diff_type);
                dt1.Columns.Add("Bass" + diff_name, diff_type);
                dt1.Columns.Add("Guitar" + diff_name, diff_type);
                if (!isRB4)
                {
                    dt1.Columns.Add("Keys" + diff_name, diff_type);
                }
                dt1.Columns.Add("Vocals" + diff_name, diff_type);
                dt1.Columns.Add("Genre", stringType);
                dt1.Columns.Add("Rating", stringType);
                dt1.Columns.Add("Album Name", stringType);

                colWidths.AddRange(isRB4
                    ? new List<double> {diff_width, diff_width, diff_width, diff_width, 16, 7, 30}
                    : new List<double> {diff_width, diff_width, diff_width, diff_width, diff_width, 16, 7, 30});
            }

            //everything
            if (radioEverythingExcel.Checked)
            {
                dt1.Columns.Add("Track #", decimalType);
                dt1.Columns.Add("Master?", stringType);
                dt1.Columns.Add("Year Rel.", decimalType);
                dt1.Columns.Add("Year Rec.", decimalType);
                if (!isRB4 && !isBlitz)
                {
                    dt1.Columns.Add("P. Bass" + diff_name, diff_type);
                    dt1.Columns.Add("P. Bass Tuning", stringType);
                    dt1.Columns.Add("P. Guitar" + diff_name, diff_type);
                    dt1.Columns.Add("P. Guitar Tuning", stringType);
                    dt1.Columns.Add("P. Keys" + diff_name, diff_type);
                }
                dt1.Columns.Add("Band" + diff_name, diff_type);
                dt1.Columns.Add("Singer", stringType);
                dt1.Columns.Add("Source", stringType);
                if (!isRB4)
                {
                    dt1.Columns.Add("Prev. Start", stringType);
                    dt1.Columns.Add("Prev. End", stringType);
                    dt1.Columns.Add("Game V.", decimalType);
                    dt1.Columns.Add("Tonic", decimalType);
                    dt1.Columns.Add("Tonality", decimalType);
                    dt1.Columns.Add("Scroll Sp.", decimalType);
                    dt1.Columns.Add("Perc. Bank", stringType);
                    dt1.Columns.Add("Drum Bank", stringType);
                    dt1.Columns.Add("Song ID", decimalType);
                    dt1.Columns.Add("Wipe Proof", stringType);
                    dt1.Columns.Add("Short Name", stringType);
                    dt1.Columns.Add("File Path", stringType);
                }

                if (isRB4)
                {
                    colWidths.AddRange(new List<double> { 7, 8, 9, 9, diff_width, 8, 9});
                }
                else if (isBlitz)
                {
                    colWidths.AddRange(new List<double> { 7, 8, 9, 9, diff_width, 8, 9, 10, 9, 8, 6, 8, 9, 22, 17, 12, 10, 25, 65 });
                }
                else
                {
                    colWidths.AddRange(new List<double> { 7, 8, 9, 9, diff_width, 18, diff_width + 1, 18, diff_width, diff_width, 8, 9, 10, 9, 8, 6, 8, 9, 22, 17, 12, 10, 25, 65 });
                }
            }

            foreach (var song in Songs)
            {
                if (radioMinimalExcel.Checked)
                {
                    dt1.Rows.Add(new object[]
                        {
                            song.Artist,song.Name,song.VocalParts.ToString(CultureInfo.InvariantCulture),Parser.GetSongDuration(song.Length.ToString(CultureInfo.InvariantCulture))
                        });
                }
                else if (radioDefaultExcel.Checked)
                {
                    if (isRB4)
                    {
                        dt1.Rows.Add(new object[]
                        {
                            song.Artist, song.Name, song.VocalParts.ToString(CultureInfo.InvariantCulture),
                            Parser.GetSongDuration(song.Length.ToString(CultureInfo.InvariantCulture)),
                            FormatDiff(song.DrumsDiff), FormatDiff(song.BassDiff),
                            FormatDiff(song.GuitarDiff), FormatDiff(song.VocalsDiff), song.Genre, song.GetRating(), song.Album
                        });
                    }
                    else
                    {
                        dt1.Rows.Add(new object[]
                        {
                            song.Artist, song.Name, song.VocalParts.ToString(CultureInfo.InvariantCulture),
                            Parser.GetSongDuration(song.Length.ToString(CultureInfo.InvariantCulture)),
                            FormatDiff(song.DrumsDiff), FormatDiff(song.BassDiff),
                            FormatDiff(song.GuitarDiff), FormatDiff(song.KeysDiff),
                            FormatDiff(song.VocalsDiff), song.Genre, song.GetRating(), song.Album
                        });
                    }
                }
                else if (radioEverythingExcel.Checked)
                {
                    var isCustom = song.SongId.ToString(CultureInfo.InvariantCulture).StartsWith("11844", StringComparison.Ordinal) ||
                        song.SongId.ToString(CultureInfo.InvariantCulture).StartsWith("10746", StringComparison.Ordinal);
                    if (isRB4)
                    {
                        dt1.Rows.Add(new object[]
                        {
                            song.Artist, song.Name, song.VocalParts.ToString(CultureInfo.InvariantCulture),
                            Parser.GetSongDuration(song.Length.ToString(CultureInfo.InvariantCulture)),
                            FormatDiff(song.DrumsDiff), FormatDiff(song.BassDiff),
                            FormatDiff(song.GuitarDiff), FormatDiff(song.VocalsDiff), song.Genre, song.GetRating(), song.Album,
                            song.TrackNumber.ToString(CultureInfo.InvariantCulture), song.IsMaster(),
                            song.YearReleased.ToString(CultureInfo.InvariantCulture),
                            song.YearRecorded.ToString(CultureInfo.InvariantCulture),
                            FormatDiff(song.BandDiff), song.GetGender(true), song.GetSource()
                        });
                    }
                    else if (isBlitz)
                    {
                        dt1.Rows.Add(new object[]
                        {
                            song.Artist, song.Name, song.VocalParts.ToString(CultureInfo.InvariantCulture),
                            Parser.GetSongDuration(song.Length.ToString(CultureInfo.InvariantCulture)),
                            FormatDiff(song.DrumsDiff), FormatDiff(song.BassDiff),
                            FormatDiff(song.GuitarDiff), FormatDiff(song.KeysDiff),
                            FormatDiff(song.VocalsDiff), song.Genre, song.GetRating(), song.Album,
                            song.TrackNumber.ToString(CultureInfo.InvariantCulture), song.IsMaster(),
                            song.YearReleased.ToString(CultureInfo.InvariantCulture),
                            song.YearRecorded.ToString(CultureInfo.InvariantCulture),
                            FormatDiff(song.BandDiff), song.GetGender(true), song.GetSource(),
                            Parser.GetSongDuration(song.PreviewStart.ToString(CultureInfo.InvariantCulture)), 
                            Parser.GetSongDuration(song.PreviewEnd.ToString(CultureInfo.InvariantCulture)),
                            song.GameVersion.ToString(CultureInfo.InvariantCulture), song.TonicNote.ToString(CultureInfo.InvariantCulture),
                            song.Tonality.ToString(CultureInfo.InvariantCulture), song.ScrollSpeed.ToString(CultureInfo.InvariantCulture),
                            song.PercussionBank, song.DrumBank, song.SongId, isCustom ? "No" : "Yes", song.ShortName, song.FilePath
                        });
                    }
                    else
                    {
                        dt1.Rows.Add(new object[]
                        {
                            song.Artist, song.Name, song.VocalParts.ToString(CultureInfo.InvariantCulture),
                            Parser.GetSongDuration(song.Length.ToString(CultureInfo.InvariantCulture)),
                            FormatDiff(song.DrumsDiff), FormatDiff(song.BassDiff),
                            FormatDiff(song.GuitarDiff), FormatDiff(song.KeysDiff),
                            FormatDiff(song.VocalsDiff), song.Genre, song.GetRating(), song.Album,
                            song.TrackNumber.ToString(CultureInfo.InvariantCulture), song.IsMaster(),
                            song.YearReleased.ToString(CultureInfo.InvariantCulture),
                            song.YearRecorded.ToString(CultureInfo.InvariantCulture),
                            FormatDiff(song.ProBassDiff), song.ProBassDiff > 0 ? song.ProBassTuning : "", FormatDiff(song.ProGuitarDiff), song.ProGuitarDiff > 0 ? song.ProGuitarTuning : "", FormatDiff(song.ProKeysDiff),
                            FormatDiff(song.BandDiff), song.GetGender(true), song.GetSource(),
                            Parser.GetSongDuration(song.PreviewStart.ToString(CultureInfo.InvariantCulture)), 
                            Parser.GetSongDuration(song.PreviewEnd.ToString(CultureInfo.InvariantCulture)),
                            song.GameVersion.ToString(CultureInfo.InvariantCulture), song.TonicNote.ToString(CultureInfo.InvariantCulture),
                            song.Tonality.ToString(CultureInfo.InvariantCulture), song.ScrollSpeed.ToString(CultureInfo.InvariantCulture),
                            song.PercussionBank, song.DrumBank, song.SongId, isCustom ? "No" : "Yes", song.ShortName, song.FilePath
                        });
                    }
                }
            }
            ds.Tables.Add(dt1);
            excel.doHeader = chkHeadersExcel.Checked; //whether to add a separate header formatting for the first line
            excel.doStyle = radioStylized.Checked; //whether to stylize rest of contents
            excel.colWidths = colWidths; //list of custom column widths, otherwise will be 20
            excel.AltColor = radioStylized.Checked ? AltColor : Color.Transparent; //for use when stylizing the contents
            cboFontsExcel.Invoke(new MethodInvoker(() => excel.FontIndex = (uint) cboFontsExcel.SelectedIndex));
            if (!CreateTempFile) return excel.CreateExcelDocument(ds, export_path);
            var xcel = Application.StartupPath + "\\bin\\temp.xlsx";
            Tools.DeleteFile(xcel);
            export_path = xcel;
            return excel.CreateExcelDocument(ds, export_path);
        }
        
        private void SetlistExport_Shown(object sender, EventArgs e)
        {
            cboPDF.SelectedIndex = 0; //Basic 1
            cboFonts.SelectedIndex = 3; //Times New Roman
            cboColor.SelectedIndex = 0;
            cboFontsExcel.SelectedIndex = 3;
            cboColorsExcel.SelectedIndex = 0;
            cboSorting.SelectedIndex = 0;
            cboSortingExcel.SelectedIndex = 0;
            cboSortingCSV.SelectedIndex = 0;
            cboSortingJSON.SelectedIndex = 0;
            lblSongCount.Text = "Songs exported: " + string.Format("{0:n0}", Songs.Count);
            isLoading = false;
            PrepareToWork();
            pdfPreviewer.RunWorkerAsync();
        }

        private void PresetChanged(object sender, EventArgs e)
        {
            switch (((RadioButton)(sender)).Name)
            {
                case "radioDefaultExcel":
                    lblPresetExcel.Text = "Ideal for detailed booklets, more info = more pages.";
                    break;
                case "radioDefaultCSV":
                    lblPresetCSV.Text = "Ideal for detailed booklets, more info = more pages.";
                    break;
                case "radioMinimalExcel":
                    lblPresetExcel.Text = "Only basic song information, less info = less pages.";
                    break;
                case "radioMinimalCSV":
                    lblPresetCSV.Text = "Only basic song information, less info = less pages.";
                    break;
                case "radioEverythingExcel":
                    lblPresetExcel.Text = "All the song information available, ideal for data dumps.";
                    break;
                case "radioEverythingCSV":
                    lblPresetCSV.Text = "All the song information available, ideal for data dumps.";
                    break;
            }
        }

        private void chkAlternate_CheckedChanged(object sender, EventArgs e)
        {
            cboColor.Enabled = chkAlternate.Checked;
            chkAlternate.BackColor = cboColor.Enabled ? AltColor : Color.Transparent;
            if (isLoading) return;
            PrepareToWork();
            pdfPreviewer.RunWorkerAsync();
        }

        private void cboColor_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (cboColor.SelectedIndex)
            {
                case 0:
                    AltColor = Color.FromArgb(191, 225, 255);  //blue
                    break;
                case 1:
                    AltColor = Color.FromArgb(191, 255, 208);  //green
                    break;
                case 2:
                    AltColor = Color.FromArgb(235, 235, 235);  //grey
                    break;
                case 3:
                    AltColor = Color.FromArgb(255, 191, 203);  //red
                    break;
                case 4:
                    AltColor = Color.FromArgb(253, 255, 191); //yellow
                    break;
                case 5:
                    if (ActiveIndex == PDFStyle.espher && isLoading)
                    {
                        AltColor = Color.FromArgb(192, 192, 192);
                    }
                    else
                    {
                        AltColor = ColorPicker(AltColor);
                    }
                    break;
                default:
                    AltColor = Color.White;
                    break;
            }
            chkAlternate.BackColor = cboColor.Enabled ? AltColor : Color.Transparent;
            if (isLoading) return;
            PrepareToWork();
            pdfPreviewer.RunWorkerAsync();
        }

        private Color ColorPicker(Color initialcolor)
        {
            colorDialog1.Color = initialcolor;
            colorDialog1.SolidColorOnly = true;
            colorDialog1.ShowDialog();
            return colorDialog1.Color;
        }
        
        private void cboPDF_SelectedIndexChanged(object sender, EventArgs e)
        {
            grpSorting.Enabled = true;
            grpMisc.Enabled = true;
            //chkAlternate.Enabled = true;
            ActiveIndex = (PDFStyle) cboPDF.SelectedIndex;
            if (ActiveIndex == PDFStyle.Detailed3)
            {
                isLoading = true; //prevent it from recalculating pages
                //chkAlternate.Enabled = false;
                chkAlternate.Checked = false;
                var icons = new List<string>
                    {
                        "bass",
                        "nobass",
                        "probass",
                        "drums",
                        "nodrums",
                        "prodrums",
                        "keys",
                        "nokeys",
                        "prokeys",
                        "guitar",
                        "noguitar",
                        "proguitar",
                        "vocals",
                        "novocals",
                        "harm2",
                        "harm3"
                    };
                foreach (var icon in icons.Where(icon => !File.Exists(Application.StartupPath + "\\res\\icons\\" + (chkAlternate.Checked ? "cube" : "circle") + "\\" + icon + ".jpg")))
                {
                    MessageBox.Show("Instrument icon '" + icon + ".jpg' is missing\nStyle 'Detailed 3' cannot be used\nRe-download this program and don't delete any files",
                        AppName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    btnExport.Enabled = false;
                    break;
                }
                isLoading = false;
            }
            else if (ActiveIndex == PDFStyle.DeadlyRobots)
            {
                isLoading = true; //prevent it from recalculating pages
                cboFonts.SelectedIndex = 2;
                chkAlternate.Checked = true;
                cboColor.SelectedIndex = 2;
                chkHeaders.Checked = false;
                chkCountHeader.Checked = false;
                chkCountFooter.Checked = true;
                chkPages.Checked = true;                
                chkBorder.Checked = false;
                chkArtistOnce.Checked = false;
                isLoading = false;
            }
            else if (ActiveIndex == PDFStyle.espher)
            {
                isLoading = true; //prevent it from recalculating pages
                cboFonts.SelectedIndex = 0;
                grpSorting.Enabled = false;
                grpMisc.Enabled = false;
                chkCountFooter.Checked = false;
                chkPages.Checked = true;
                chkAlternate.Checked = true;
                chkHeaders.Checked = true;
                chkCountHeader.Checked = true;
                AltColor = Color.FromArgb(192, 192, 192);
                chkAlternate.BackColor = AltColor;
                cboColor.SelectedIndex = 5;
                cboSorting.SelectedIndex = 0;
                chkBorder.Checked = false;
                chkArtistOnce.Checked = false;
                SortByArtist = true;
                UpdateSorting();
                isLoading = false;
            }
            else if (ActiveIndex == PDFStyle.Macst3r)
            {
                isLoading = true; //prevent it from recalculating pages
                cboFonts.SelectedIndex = 0;
                grpSorting.Enabled = false;
                grpMisc.Enabled = false;
                chkCountFooter.Checked = false;
                chkPages.Checked = true;
                chkAlternate.Checked = true;
                chkHeaders.Checked = true;
                chkCountHeader.Checked = true; 
                cboColor.SelectedIndex = 0;
                cboSorting.SelectedIndex = 0;
                chkBorder.Checked = false;
                chkArtistOnce.Checked = false;
                SortByArtist = true;
                UpdateSorting();
                var icons = new List<string>
                    {
                        "bass",
                        "drums",
                        "keys",
                        "guitar",
                        "vocals",
                        "harm3"
                    };
                foreach (var icon in icons.Where(icon => !File.Exists(Application.StartupPath + "\\res\\icons\\cube\\" + icon + ".jpg")))
                {
                    MessageBox.Show("Instrument icon '" + icon + ".jpg' is missing\nStyle 'MACST3R' cannot be used\nRe-download this program and don't delete any files",
                        AppName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    btnExport.Enabled = false;
                    break;
                }
                isLoading = false;
            }
            else
            {
                btnExport.Enabled = true;
            }
            chkArtistOnce.Enabled = cboPDF.SelectedIndex > 1 && cboPDF.SelectedIndex < 5 && SortByArtist;
            if (isLoading) return;
            PrepareToWork();
            pdfPreviewer.RunWorkerAsync();
        }

        private void DisplayThumbnail()
        {
            var pdf = Application.StartupPath + "\\bin\\temp.pdf";
            var jpg = Application.StartupPath + "\\bin\\pdf.jpg";
            Tools.DeleteFile(jpg);
            if (!File.Exists(pdf))
            {
                MessageBox.Show("Error displaying PDF thumbnail", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            try
            {
                var doc = new PDFWrapper();
                doc.LoadPDF(pdf);
                TotalPageNumbers = doc.PageCount;
                doc.CurrentPage = 1;
                doc.ExportJpg(jpg, 1, 1, 150, 99, -1);
                doc.Dispose();
                Tools.DeleteFile(pdf);
            }
            catch (Exception)
            {
                MessageBox.Show("Error displaying PDF thumbnail", "Error", MessageBoxButtons.OK,
                                MessageBoxIcon.Exclamation);
                return;
            }
            if (!File.Exists(jpg))
            {
                MessageBox.Show("Error displaying thumbnail", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            picThumbnail.Image = Tools.NemoLoadImage(jpg);
            Tools.DeleteFile(jpg);
        }
        
        private void cboFonts_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (isLoading) return;
            PrepareToWork();
            pdfPreviewer.RunWorkerAsync();
        }

        private void backgroundWorker1_DoWork(object sender, System.ComponentModel.DoWorkEventArgs e)
        {
            ExportPDF(true);
        }

        private void PrepareToWork(bool enabled = false)
        {
            if (pdfPreviewer.IsBusy) return;
            tabControl1.Enabled = enabled;
            picWorking.Visible = !enabled;
            btnExport.Enabled = enabled;
            btnClose.Enabled = enabled;
        }

        private void backgroundWorker1_RunWorkerCompleted(object sender, System.ComponentModel.RunWorkerCompletedEventArgs e)
        {
            DisplayThumbnail();
            lblPages.Text = string.Format("{0:n0}", TotalPageNumbers);
            PrepareToWork(true);
            cboPDF.Focus();
        }

        private void chkBorder_CheckedChanged(object sender, EventArgs e)
        {
            if (isLoading) return;
            PrepareToWork();
            pdfPreviewer.RunWorkerAsync();
        }

        private void radioPlain_CheckedChanged(object sender, EventArgs e)
        {
            radioStylized.BackColor = radioStylized.Checked ? AltColor : Color.Transparent;
            cboColor.Enabled = radioStylized.Checked;
        }

        private void cboColorsExcel_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (cboColorsExcel.SelectedIndex)
            {
                case 0:
                    AltColor = Color.FromArgb(191, 225, 255);  //blue
                    break;
                case 1:
                    AltColor = Color.FromArgb(191, 255, 208);  //green
                    break;
                case 2:
                    AltColor = Color.FromArgb(235, 235, 235);  //grey
                    break;
                case 3:
                    AltColor = Color.FromArgb(255, 191, 203);  //red
                    break;
                case 4:
                    AltColor = Color.FromArgb(253, 255, 191); //yellow
                    break;
                case 5:
                    AltColor = ColorPicker(AltColor);
                    break;
                default:
                    AltColor = Color.White;
                    break;
            }
            radioStylized.BackColor = AltColor;
        }

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (tabControl1.SelectedIndex == 0)
            {
                Width = 679;
                PrepareToWork();
                pdfPreviewer.RunWorkerAsync();
            }
            else
            {
                Width = 435;
            }
            lblPages.Visible = tabControl1.SelectedIndex == 0;
            lblTotalPages.Visible = tabControl1.SelectedIndex == 0;
            picThumbnail.Visible = tabControl1.SelectedIndex == 0;
        }

        private void cboSorting_SelectedIndexChanged(object sender, EventArgs e)
        {
            chkArtistOnce.Enabled = cboSorting.SelectedIndex == 0;
            SortByArtist = cboSorting.SelectedIndex == 0;
            if (isLoading) return;
            isLoading = true;
            cboSortingCSV.SelectedIndex = cboSorting.SelectedIndex;
            cboSortingExcel.SelectedIndex = cboSorting.SelectedIndex;
            cboSortingJSON.SelectedIndex = cboSorting.SelectedIndex;
            isLoading = false;
            UpdateSorting(true);
        }

        private static string PrepForSorting(string raw)
        {
            return raw.Replace("The ", "").Replace("\"", "").ToLowerInvariant();
        }

        private void UpdateSorting(bool isPDF = false)
        {
            if (SortByArtist)
            {
                Songs.Sort((a, b) => String.Compare(PrepForSorting(a.Artist), PrepForSorting(b.Artist), StringComparison.Ordinal));
            }
            else
            {
                Songs.Sort((a, b) => String.Compare(PrepForSorting(a.Name), PrepForSorting(b.Name), StringComparison.Ordinal));
            }
            if (!isPDF) return;
            PrepareToWork();
            pdfPreviewer.RunWorkerAsync();
        }

        private void cboSortingExcel_SelectedIndexChanged(object sender, EventArgs e)
        {
            SortByArtist = cboSortingExcel.SelectedIndex == 0;
            if (isLoading) return;
            isLoading = true;
            cboSorting.SelectedIndex = cboSortingExcel.SelectedIndex;
            cboSortingCSV.SelectedIndex = cboSortingExcel.SelectedIndex;
            cboSortingJSON.SelectedIndex = cboSortingExcel.SelectedIndex;
            isLoading = false;
            UpdateSorting();
        }

        private void cboSortingCSV_SelectedIndexChanged(object sender, EventArgs e)
        {
            SortByArtist = cboSortingCSV.SelectedIndex == 0;
            if (isLoading) return;
            isLoading = true;
            cboSorting.SelectedIndex = cboSortingCSV.SelectedIndex;
            cboSortingExcel.SelectedIndex = cboSortingCSV.SelectedIndex;
            cboSortingJSON.SelectedIndex = cboSortingCSV.SelectedIndex;
            isLoading = false;
            UpdateSorting();
        }

        private void fileExporter_DoWork(object sender, System.ComponentModel.DoWorkEventArgs e)
        {
            var index = 0;
            tabControl1.Invoke(new MethodInvoker(() => index = tabControl1.SelectedIndex));
            switch (index)
            {
                case 0:
                    ExportSuccess = ExportPDF();
                    break;
                case 1:
                    ExportSuccess = ExportExcel();
                    break;
                case 2:
                    ExportSuccess = ExportCSV();
                    break;
                case 3:
                    ExportSuccess = ExportJSON();
                    break;
            }
        }

        private void fileExporter_RunWorkerCompleted(object sender, System.ComponentModel.RunWorkerCompletedEventArgs e)
        {
            PrepareToWork(true);
            if (ExportSuccess && chkLaunch.Checked && !string.IsNullOrWhiteSpace(export_path))
            {
                Process.Start(export_path);
            }
        }

        private void SetlistExporter_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!picWorking.Visible)
            {
                Tools.DeleteFile(tempPDF);
                return;
            }
            MessageBox.Show("Please wait until the current process finishes", AppName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            e.Cancel = true;
        }

        public void SetGame(Image icon)
        {
            picGame.Image = icon;
        }

        private void cboSortingJSON_SelectedIndexChanged(object sender, EventArgs e)
        {
            SortByArtist = cboSortingJSON.SelectedIndex == 0;
            if (isLoading) return;
            isLoading = true;
            cboSorting.SelectedIndex = cboSortingJSON.SelectedIndex;
            cboSortingExcel.SelectedIndex = cboSortingJSON.SelectedIndex;
            isLoading = false;
            UpdateSorting();
        }
    }

    public enum PDFStyle
    {
        Basic1 = 0,
        Basic2 = 1,
        Detailed1 = 2,
        Detailed2 = 3,
        Detailed3 = 4,
        DeadlyRobots = 5,
        espher = 6,
        Macst3r = 7,
    }
}
