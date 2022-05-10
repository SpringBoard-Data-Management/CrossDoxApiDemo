using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrossDoxApiDemo;
public class CrossDoxUserPreferences
{
    public OutputFileType OutputFileType { get => OutputFileType.xlsx; } // must be set to (xlsx = 1)
    public bool MergeFiles { get; set; }
    public bool SeparateFiles { get; set; }
    public bool SingleFileCompression { get; set; }
}

public enum OutputFileType
{
    xlsx = 1
}