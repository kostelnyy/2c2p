using CCPP.Core.Domain;
using System.Collections.Generic;
using System.IO;

namespace CCPP.Core.FileParsers
{
    public abstract class FileParser
    {
        public abstract string Extension { get; }
        public bool CanParseFile(string fileName)
        {
            var fileExtenstion = Path.GetExtension(fileName);
            return Extension.Equals(fileExtenstion);
        }

        public abstract IEnumerable<PaymentTransaction> ParseContent(Stream content);
    }
}
