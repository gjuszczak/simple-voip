using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using VoIP.TcpSignalizationLibrary;

namespace VoIP.WinFormsUserInterface.Helpers
{
    public static class ExtensionMethods
    {
        public static bool AfterPacketIncomeInvoke(this Control invokedControl, Action<TcpSignalizationClient, SignalizationPacket> callback, TcpSignalizationClient client, SignalizationPacket packet)
        {
            return invokedControl.InvokeIfRequired(callback, client, packet);
        }

        private static bool InvokeIfRequired(this Control invokedControl, Delegate callback, params object[] parameters)
        {
            try
            {
                if (invokedControl.InvokeRequired)
                {
                    invokedControl.BeginInvoke(callback, parameters);
                    return true;
                }
                return false;
            }
            catch { return true; }
        }
    }
}
