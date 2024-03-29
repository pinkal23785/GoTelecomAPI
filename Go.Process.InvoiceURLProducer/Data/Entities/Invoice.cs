using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Go.Process.InvoiceURLProducer.Data.Entities
{
    public class Invoice
    {
        public string INVOICE_ID { get; set; }
        public string ACCOUNT_ID { get; set; }
        public DateTime? STATEMENT_DATE { get; set; }
        public double? BILL_AMOUNT { get; set; }
        public double? MIN_AMOUNT_DUE { get; set; }
        public DateTime? DUE_DATE { get; set; }
        public DateTime? CREATED_DATE { get; set; }
        public string CREATED_BY { get; set; }
        [Column(TypeName = "BLOB")]
        public byte[] INVOICE_DATA_PDF { get; set; }
        public string FINAL_INVOICE_XML { get; set; }
        public string BILL_NO { get; set; }
        public string BILL_STATUS { get; set; }
        public byte[] INVOICE_DATA_PDF_AR { get; set; }
        public string FINAL_INVOICE_XML_AR { get; set; }
    }
}
