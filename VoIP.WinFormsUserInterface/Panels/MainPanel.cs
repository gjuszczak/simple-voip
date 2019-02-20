using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using VoIP.WinFormsUserInterface.Properties;

namespace VoIP.WinFormsUserInterface.Panels
{
    public partial class MainPanel : PanelBase
    {
        public MainPanel()
        {
            InitializeComponent();

            this.Dock = DockStyle.Fill;
        }

        public event Action<string, int> BeginCall;

        private void btnCall_Click(object sender, EventArgs e)
        {
            try
            {
                Uri u = new Uri(string.Format("voip://{0}", tbConnection.Text));

                if (BeginCall != null)
                    BeginCall(u.Host, u.IsDefaultPort ? Settings.Default.SignalizationPort : u.Port);
            }
            catch
            {
                MessageBox.Show("Wprowadzono nieprawidłowe dane połaczenia", "Błąd wprowadzonych danych", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }
    }
}
