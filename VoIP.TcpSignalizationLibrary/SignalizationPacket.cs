using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace VoIP.TcpSignalizationLibrary
{
    [Serializable]
    public class SignalizationPacket
    {
        public SignalizationCommand Command { get; set; }

        public static SignalizationPacket FromXml(string xml)
        {
            using (TextReader reader = new StringReader(xml))
            {
                return (SignalizationPacket)serializer.Deserialize(reader);
            }
        }

        private static XmlSerializer serializer = new XmlSerializer(typeof(SignalizationPacket));     

        public string ToXml()
        {
            using (StringWriter textWriter = new StringWriter())
            {
                serializer.Serialize(textWriter, this);
                return textWriter.ToString();
            }
        }
    }
}
