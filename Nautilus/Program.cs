using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using Nautilus.x360;
using Un4seen.Bass;

namespace Nautilus
{
    static class Program
    {
        private const string APP_NAME = "Nautilus";
        private const string bKey = "2X14232420202322";
        private const string user = "nemo";
        private const string domain = "keepitfishy";
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            BassNet.Registration(user + "@" + domain + ".com", bKey);
            bool result;
            var mutex = new System.Threading.Mutex(true, "UniqueAppId", out result);
            if (!result)
            {
                MessageBox.Show("There's already another instance of " + APP_NAME + " running!\n\nYou should only have one instance running at any given time to avoid file access conflicts and possible crashes.\n\nProceed with caution.", APP_NAME, MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }

            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            var binFolder = Application.StartupPath + "\\bin\\";
            if (!Directory.Exists(binFolder))
            {
                Directory.CreateDirectory(binFolder);
            }

            if (!File.Exists(Application.StartupPath + "\\bin\\nautilusFREE.dll"))
            {
                MessageBox.Show("Required file nautilusFREE.dll is missing", "Critical Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Process.GetCurrentProcess().Kill();
            }
            if (!File.Exists(Application.StartupPath + "\\bin\\KV.bin"))
            {
                MessageBox.Show("Required file 'KV.bin' was not found in the \\bin subdirectory\n" + APP_NAME + " can't work without it ...", "Critical Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Process.GetCurrentProcess().Kill();
            }
            var kv = new RSAParams(Application.StartupPath + "\\bin\\KV.bin");
            if (!kv.Valid)
            {
                MessageBox.Show("Required file 'KV.bin' was found in the \\bin subdirectory but it is not valid\n" + APP_NAME + " can't work without it ...", "Critical Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Process.GetCurrentProcess().Kill();
            }

            var con = "";
            var argument = args.Aggregate("", (current, arg) => current + " " + arg).ToLowerInvariant().Trim();
            if (string.IsNullOrWhiteSpace(argument))
            {
                Application.Run(new MainForm());
            }
            else if (argument.Contains("-help"))
            {
                var help = Application.StartupPath + "\\bin\\help\\help";
                var message = File.Exists(help) ? File.ReadAllText(help) : "Help file missing!";
                Application.Run(new HelpForm("Help", message, false, true));
            }
            else if (argument.Contains("-creator"))
            {
                Application.Run(new CONCreator(Color.FromArgb(34, 169, 31), Color.Black));
            }
            else if (argument.Contains("-pack"))
            {
                Application.Run(new PackCreator(null, Color.FromArgb(34, 169, 31), Color.Black));
            }
            else if (argument.Contains("-quickpack"))
            {
                Application.Run(new QuickPackEditor(null, Color.FromArgb(34, 169, 31), Color.Black));
            }
            else if (argument.Contains("-quickdta"))
            {
                Application.Run(new QuickDTAEditor());
            }
            else if (argument.Contains("-studio"))
            {
                Application.Run(new MiniStudio());
            }
            else if (argument.Contains("-audioconverter"))
            {
                Application.Run(new AudioConverter());
            }
            else if (argument.Contains("-karaoke"))
            {
                Application.Run(new CDGConverter());
            }
            else if (argument.Contains("-extractor"))
            {
                Application.Run(new BatchExtractor(Color.FromArgb(197, 34, 35), Color.Black));
            }
            else if (argument.Contains("-renamer"))
            {
                Application.Run(new BatchRenamer(Color.FromArgb(197, 34, 35), Color.Black));
            }
            else if (argument.Contains("-indexer"))
            {
                Application.Run(new FileIndexer(Color.FromArgb(197, 34, 35), Color.Black));
            }
            else if (argument.Contains("-dta"))
            {
                Application.Run(new BatchProcessor(Color.FromArgb(197, 34, 35), Color.Black));
            }
            else if (argument.Contains("-manager"))
            {
                Application.Run(new SetlistManager(Color.FromArgb(197, 34, 35), Color.Black));
            }
            else if (argument.EndsWith(".setlist", StringComparison.Ordinal))
            {
                var file = argument.Trim();
                string setlist;

                if (File.Exists(file))
                {
                    setlist = file;
                }
                else if (File.Exists(Application.StartupPath + "\\setlist\\" + file))
                {
                    setlist = Application.StartupPath + "\\setlist\\" + file;
                }
                else
                {
                    setlist = "";
                }
                Application.Run(new SetlistManager(Color.FromArgb(197, 34, 35), Color.Black, setlist));
            }
            else if (argument.Contains("-event"))
            {
                Application.Run(new EventManager());
            }
            else if (argument.Contains("-visualizer"))
            {
                try
                {
                    var file = argument.Replace("-visualizer -", "").Replace("-visualizer", "").Trim();
                    if (file != "")
                    {
                        if (File.Exists(file))
                        {
                            con = file;
                        }
                    }
                }
                catch
                {
                    con = "";
                }
                Application.Run(new Visualizer(Color.FromArgb(230, 215, 0), Color.Black, con));
            }
            else if (argument.Contains("-save"))
            {
                Application.Run(new SaveFileImageEditor(Color.FromArgb(230, 215, 0), Color.Black));
            }
            else if (argument.Contains("-analyzer"))
            {
                Application.Run(new SongAnalyzer(""));
            }
            else if (argument.Contains("-audioa"))
            {
                Application.Run(new AudioAnalyzer(""));
            }
            else if (argument.Contains("-normalizer"))
            {
                Application.Run(new VolumeNormalizer(Color.FromArgb(230, 215, 0), Color.Black));
            }
            else if (argument.Contains("-milo"))
            {
                var file = Application.StartupPath + "\\bin\\MiloMod.exe";
                if (!File.Exists(file))
                {
                    MessageBox.Show(Path.GetFileName(file) + " is missing, can't launch...", "Missing File", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }
                Process.Start(file);
            }
            else if (argument.Contains("-viewer"))
            {
                var file = Application.StartupPath + "\\bin\\RB3SaveScoresViewer.exe";
                if (!File.Exists(file))
                {
                    MessageBox.Show(Path.GetFileName(file) + " is missing, can't launch...", "Missing File", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }
                Process.Start(file);
            }
            else if (argument.Contains("-character"))
            {
                var file = Application.StartupPath + "\\bin\\CharEditor.exe";
                if (!File.Exists(file))
                {
                    MessageBox.Show(Path.GetFileName(file) + " is missing, can't launch...", "Missing File", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }
                Process.Start(file);                
            }
            else if (argument.EndsWith(".mid", StringComparison.Ordinal))
            {
                var midi = "";
                var doclean = argument.Contains("-cleaner");
                if (doclean)
                {
                    argument = argument.Replace("-cleaner", "").Trim();
                }
                if (File.Exists(argument))
                {
                    midi = argument;
                }
                else if (File.Exists(Application.StartupPath + "\\" + argument))
                {
                    midi = Application.StartupPath + "\\" + argument;
                }
                if (string.IsNullOrWhiteSpace(midi))
                {
                    Application.Run(new MainForm());
                }
                else
                {
                    if (doclean)
                    {
                        Application.Run(new MIDICleaner(midi, Color.FromArgb(230, 215, 0), Color.Black));
                    }
                    else
                    {
                        Application.Run(new MIDISelector(midi));
                    }
                }
            }
            else if (argument.Contains("-cleaner"))
            {
                Application.Run(new MIDICleaner("", Color.FromArgb(230, 215, 0), Color.Black));
            }
            else if (argument.Contains("-bundler"))
            {
                Application.Run(new ProUpgradeBundler(Color.FromArgb(230, 215, 0), Color.Black));
            }
            else if (argument.Contains("-art"))
            {
                Application.Run(new AdvancedArtConverter("", Color.FromArgb(37, 89, 201), Color.Black));
            }
            else if (argument.Contains("-wii"))
            {
                try
                {
                    var file = argument.Replace("-wii -", "").Trim();
                    con = "";
                    if (file != "")
                    {
                        if (Directory.Exists(file))
                        {
                            con = file;
                        }
                    }
                }
                catch
                {
                    con = "";
                }
                Application.Run(new WiiConverter(Color.FromArgb(37, 89, 201), Color.Black, con));
            }
            else if (argument.Contains("-ps3"))
            {
                Application.Run(new PS3Converter(null, Color.FromArgb(37, 89, 201), Color.Black));
            }
            else if (argument.Contains("-phaseshift") || argument.Contains("-yarg") || argument.Contains("-clonehero"))
            {
                Application.Run(new PhaseShiftConverter(Color.FromArgb(37, 89, 201), Color.Black));
            }
            else if (argument.Contains("-rba"))
            {
                Application.Run(new RBAEditor(Color.FromArgb(37, 89, 201), Color.Black));
            }
            else if (argument.Contains("-usb"))
            {
                Application.Run(new RBtoUSB());
            }
            else if (argument.Contains("-video"))
            {
                Application.Run(new VideoPreparer(Color.FromArgb(240, 104, 4), Color.Black));
            }
            else if (argument.Contains("-stems"))
            {
                Application.Run(new StemsIsolator(Color.FromArgb(240, 104, 4), Color.Black));
            }
            else if (argument.Contains("-cryptor"))
            {
                Application.Run(new Cryptor(Color.FromArgb(240, 104, 4), Color.Black));
            }
            else if (argument.Contains("-mogg"))
            {
                Application.Run(new MoggMaker(Color.FromArgb(240, 104, 4), Color.Black));
            }
            else if (argument.EndsWith(".mogg", StringComparison.Ordinal))
            {
                Application.Run(new Worker(argument));
            }
            else
            {
                try
                {
                    if (VariousFunctions.ReadFileType(argument) == XboxFileType.STFS)
                    {
                        var xExplorer = new CONExplorer(Color.FromArgb(34, 169, 31), Color.Black, true);
                        xExplorer.LoadCON(argument);
                        Application.Run(xExplorer);
                    }
                }
                catch (Exception)
                {
                    Application.Run(new MainForm());
                }
            }
            GC.KeepAlive(mutex);
        }

        static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            try
            {
                var ex = (Exception)e.ExceptionObject;
                var vers = Assembly.GetExecutingAssembly().GetName().Version;
                var version = " v" + String.Format("{0}.{1}.{2}", vers.Major, vers.Minor, vers.Build);
               
                Clipboard.SetText(APP_NAME + " crashed on me! Please see the error log below:" + Environment.NewLine + Environment.NewLine + "[code]" +
                        Environment.NewLine + APP_NAME + " version" + version + Environment.NewLine + "Error Message:" + Environment.NewLine + 
                        ex.Message + Environment.NewLine + Environment.NewLine + "Stack Trace:" + Environment.NewLine + ex.StackTrace + Environment.NewLine + "[/code]");

                MessageBox.Show("Derp, " + APP_NAME + " has crashed! Sorry.\nI copied some helpful information to your clipboard " +
                    "that can help fix this in the future.", "Fatal Error", MessageBoxButtons.OK, MessageBoxIcon.Stop);    
            }
            finally
            {
                Application.Exit();
            }
        }
    }
}
