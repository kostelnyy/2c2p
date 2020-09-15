﻿using System.Linq;

namespace CCPP.Core.FileParsers
{
    public abstract class FileParser
    {
        public abstract string Extension { get; }
        public bool CanParseFile(string fileName)
        {
            var fileExtenstion = fileName.Split('.').Last();
            return Extension.Equals(fileExtenstion);
        }
    }
}