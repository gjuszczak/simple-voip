using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VoIP.TcpSignalizationLibrary
{
    public enum SignalizationCommand
    {
        Call,
        Ringing,
        Answer,
        Busy,
        End,
        Error
    }
}
