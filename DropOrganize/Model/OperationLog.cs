using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DropOrganize.Model
{
    public class OperationLog
    {
        public CodeEnum Code { get; set; }
        public string Msg { get; set; }
        public string FileName { get; set; }
        public string FromPath { get; set; }
        public string ToPath { get; set; }
        public DateTime Timestamp { get; set; }

        public enum CodeEnum
        {
            Success = 0,
            Error = 1
        }
    }
}
