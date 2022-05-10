using System.Collections.Generic;

namespace CrossDoxApiDemo
{
    public class ParseInfoResponse
    {
        public List<ParseInfoExternal> parseInfos;
    }

    public class ParseInfoExternal
    {
        public string Status;
        public string Type;
        public string Retailer;
        public List<string> FileNames;

        //Success Fields
        public string JSON;
        public bool IsMerged;
        public List<int> RecordCounts;
        public int SourceStartPage;
        public int SourceEndPage;

        //Failure Fields
        public string InnerMessage;
    }

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