using System.Collections.Generic;

namespace CrossDoxApiDemo
{
    public class CrossDoxParsedData
    {
        public List<ParseResponseMetadata> ParseResponseMetadata { get; set; }
        public int ZipCount { get; set; }
        public string ZipName { get; set; }
        public string ZipBase64 { get; set; }
    }

    public class ParseResponseMetadata
    {
        public string Status { get; set; }
        public string Type { get; set; }
        public string Retailer { get; set; }
        public string FileName { get; set; }
    }
}