using CCPP.Core.Domain;
using System.Collections.Generic;

namespace CCPP.Core.FileParsers
{
    public class XmlFileParser : FileParser
    {
        public override string Extension => "xml";

        public override IEnumerable<PaymentTranstaction> ParseContent(string content)
        {
            throw new System.NotImplementedException();
        }
    }
}
