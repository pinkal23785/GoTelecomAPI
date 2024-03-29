using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Go.FtthCollection.Service.Models
{
    // using System.Xml.Serialization;
    // XmlSerializer serializer = new XmlSerializer(typeof(Envelope));
    // using (StringReader reader = new StringReader(xml))
    // {
    //    var test = (Envelope)serializer.Deserialize(reader);
    // }

    [XmlRoot(ElementName = "installmentPaymentOrderResp")]
    public class InstallmentPaymentOrderResp
    {

        [XmlElement(ElementName = "StatusCode")]
        public int StatusCode { get; set; }

        [XmlElement(ElementName = "Description")]
        public string Description { get; set; }

        [XmlElement(ElementName = "Status")]
        public string Status { get; set; }

        [XmlAttribute(AttributeName = "tns")]
        public string Tns { get; set; }

        [XmlText]
        public string Text { get; set; }
    }

    [XmlRoot(ElementName = "Body")]
    public class Body
    {

        [XmlElement(ElementName = "installmentPaymentOrderResp")]
        public InstallmentPaymentOrderResp InstallmentPaymentOrderResp { get; set; }
    }

    [XmlRoot(ElementName = "Envelope")]
    public class Envelope
    {

        [XmlElement(ElementName = "Body")]
        public Body Body { get; set; }

        [XmlAttribute(AttributeName = "env")]
        public string Env { get; set; }

        [XmlAttribute(AttributeName = "enc")]
        public string Enc { get; set; }

        [XmlAttribute(AttributeName = "ns0")]
        public string Ns0 { get; set; }

        [XmlAttribute(AttributeName = "xsd")]
        public string Xsd { get; set; }

        [XmlAttribute(AttributeName = "xsi")]
        public string Xsi { get; set; }

        [XmlText]
        public string Text { get; set; }
    }


}
