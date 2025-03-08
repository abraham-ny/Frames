using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Frames.Model
{
    public class Project
    {
        public String Name { get; set; }
        public String Status { get; set; }
        public double Progress { get; set; }
        public String Tag { get; set; }
        public string Path { get; set; }
        public string ThumbnailPath { get; set; }
        public TimeSpan TotalDuration { get; set; }
        public int FileCount { get; set; }
        public long TotalSize { get; set; }
        public List<string> SelectedFiles { get; set; } // List of relative file paths
    }
}
