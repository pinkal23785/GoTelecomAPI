using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace Hyperpay.Aywa.Web.Webservice.Models
{
    [DataContract]
    public class getAywaCardPaymentRequest
    {
        [DataMember]
        public string billNumber { get; set; }
        [DataMember]
        public string billingAccount { get; set; }
        [DataMember]
        public emaildetails emaildetails { get; set; }
        [DataMember]
        public mobiledetails mobiledetails { get; set; }

        [DataMember]
        public string orderID { get; set; }
        [DataMember]
        public string paymentMethod { get; set; }
        [DataMember]
        public string paymentRef { get; set; }
        [DataMember]
        public string totalPaidAmount { get; set; }
    }
    #region Email
    [DataContract]
    public class emaildetails
    {
        [DataMember]
        public attachment attachment { get; set; }
        [DataMember]
        public bcc bcc { get; set; }
        [DataMember]
        public cc cc { get; set; }
        [DataMember]
        public from from { get; set; }
        [DataMember]
        public message message { get; set; }
        [DataMember]
        public replyTo replyTo { get; set; }
        [DataMember]
        public string subject { get; set; }
        [DataMember]
        public to to { get; set; }

    }
    [DataContract]
    public class attachment
    {
        public string attachmentContent { get; set; }
        public string attachmentContentType { get; set; }
        public string attachmentName { get; set; }

    }
    [DataContract]
    public class bcc
    {
        [DataMember]
        public string mailId { get; set; }
        [DataMember]
        public string name { get; set; }
    }
    [DataContract]
    public class cc
    {
        [DataMember]
        public string mailId { get; set; }
        [DataMember]
        public string name { get; set; }
    }
    [DataContract]
    public class from
    {
        [DataMember]
        public string mailId { get; set; }
        [DataMember]
        public string name { get; set; }
    }
    [DataContract]
    public class message
    {
        [DataMember]
        public string messageHTML { get; set; }
        [DataMember]
        public string messageText { get; set; }
    }
    [DataContract]
    public class replyTo
    {
        [DataMember]
        public string mailId { get; set; }
        [DataMember]
        public string name { get; set; }
    }
    [DataContract]
    public class to
    {
        [DataMember]
        public string mailId { get; set; }
        [DataMember]
        public string name { get; set; }
    }
    #endregion

    #region Mobile
    [DataContract]
    public class mobiledetails
    {
        [DataMember]
        public headerReq headerReq { get; set; }
        [DataMember]
        public string sender { get; set; }
        [DataMember]
        public string receiver { get; set; }
        [DataMember]
        public string language { get; set; }
        [DataMember]
        public string message { get; set; }
    }
    [DataContract]
    public class headerReq
    {
        [DataMember]
        public string associatedChannelId { get; set; }
        [DataMember]
        public string associatedChannelType { get; set; }
        [DataMember]
        public string callerMessageID { get; set; }
        [DataMember]
        public string callerServiceID { get; set; }
        [DataMember]
        public string callerSystemID { get; set; }
        [DataMember]
        public string conversationID { get; set; }
        [DataMember]
        public string requestTimeStamp { get; set; }
        [DataMember]
        public string serviceID { get; set; }
        [DataMember]
        public string userID { get; set; }
        [DataMember]
        public string versionNo { get; set; }
    }
    #endregion
}
