using CCPP.Core.Domain;
using System.Collections.Generic;
using System.IO;

namespace CCPP.Core.FileParsers.Xml
{
    public class XmlFileParser : FileParser
    {
        public override string Extension => "xml";

        public override IEnumerable<PaymentTranstaction> ParseContent(Stream content)
        {
            throw new System.NotImplementedException();
        }
    }
}
