using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPFclient.Models
{
    public class TotalInformation
    {
        public TotalInformation(string modelName, string filePath, string drawningCode)
        {
            ModelName = modelName;
            FilePath = filePath;
            DrawingCode = drawningCode;
        }
        public string ModelName { get; set; }
        public string FilePath { get; set; }
        public string DrawingCode { get; set; }
    }
}
