using System.Windows.Forms;

namespace Nautilus
{
    public partial class PasswordUnlocker : Form
    {
        public string _shortName;
        private string _origName;

        public PasswordUnlocker(string name = "", string internalName = "")
        {
            InitializeComponent();            
            txtPass.Text = name;
            _shortName = internalName;
            _origName = name;
        }

        public string EnteredText
        {
            get
            {
                return (txtPass.Text);
            }
        }

        private void btnGo_Click(object sender, System.EventArgs e)
        {
            Close();
        }
        
        public void Renamer()
        {
            txtPass.PasswordChar = '\0';
            topLabel.Text = "Enter new name below\nthen click OK";
            toolTip1.SetToolTip(btnOK, "Click to change name");
            toolTip1.SetToolTip(txtPass, "Enter new name here");
        }

        public void LockManager()
        {
            topLabel.Text = "Enter password below\nthen click OK";
            toolTip1.SetToolTip(btnOK, "Click to save password");
            toolTip1.SetToolTip(txtPass, "Enter password here");
        }

        public void IDChanger()
        {
            txtPass.PasswordChar = '\0';
            topLabel.Text = "Enter new song ID below\nthen click OK";
            toolTip1.SetToolTip(btnOK, "Click to change song ID");
            toolTip1.SetToolTip(txtPass, "Enter new song ID here");
            btnGenerate.Visible = true;
            btnCancel.Visible = true;
        }

        private void btnCancel_Click(object sender, System.EventArgs e)
        {
            txtPass.Text = _origName;
            Close();
        }

        private void btnGenerate_Click(object sender, System.EventArgs e)
        {
            string id = "";
            if (!string.IsNullOrEmpty(_shortName))
            {
                id = _shortName;
            }
            else if (!string.IsNullOrEmpty(txtPass.Text))
            {
                id = txtPass.Text;
            }
            else
            {
                return;
            }
            var corrector = new SongIDCorrector();
            txtPass.Text = corrector.ShortnameToSongID(id).ToString();
        }
    }
}
