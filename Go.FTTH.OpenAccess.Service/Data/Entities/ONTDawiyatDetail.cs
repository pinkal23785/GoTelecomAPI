using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Go.FTTH.OpenAccess.Service.Data.Entities
{
    public class ONTDawiyatDetail
    {
        public int ID { get; set; }
        public int ONT_ID { get; set; }
        public string LATENCY { get; set; }
        public string THROUGHPUT { get; set; }
        public string Rx { get; set; }
        public string Tx { get; set; }
        public string DownloadSpeed { get; set; }
        public string UploadSpeed { get; set; }
        public DateTime MODIFY_DT { get; set; }
        public string ONTStatus { get; set; }
    }
}
