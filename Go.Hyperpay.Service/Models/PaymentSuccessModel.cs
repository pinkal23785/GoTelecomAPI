using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Go.Hyperpay.Service.Models
{
    public class Result
    {
        public string code { get; set; }
        public string description { get; set; }
    }

    public class ResultDetails
    {
        [JsonProperty("3DSecure.acsEci")]
        public string _3DSecureAcsEci { get; set; }
        public string ConnectorTxID1 { get; set; }

        [JsonProperty("authorizationResponse.stan")]
        public string AuthorizationResponseStan { get; set; }
        public string connectorId { get; set; }

        [JsonProperty("transaction.acquirer.settlementDate")]
        public string TransactionAcquirerSettlementDate { get; set; }

        [JsonProperty("transaction.receipt")]
        public string TransactionReceipt { get; set; }
        public string reconciliationId { get; set; }

        [JsonProperty("transaction.authorizationCode")]
        public string TransactionAuthorizationCode { get; set; }

        [JsonProperty("sourceOfFunds.provided.card.issuer")]
        public string SourceOfFundsProvidedCardIssuer { get; set; }

        [JsonProperty("response.acquirerMessage")]
        public string ResponseAcquirerMessage { get; set; }

        [JsonProperty("response.acquirerCode")]
        public string ResponseAcquirerCode { get; set; }
    }

    public class Card
    {
        public string bin { get; set; }
        public string binCountry { get; set; }
        public string last4Digits { get; set; }
        public string holder { get; set; }
        public string expiryMonth { get; set; }
        public string expiryYear { get; set; }
    }

    public class Customer
    {
        public string givenName { get; set; }
        public string surname { get; set; }
        public string email { get; set; }
        public string ip { get; set; }
        public string ipCountry { get; set; }
    }

    public class Billing
    {
        public string street1 { get; set; }
        public string city { get; set; }
        public string state { get; set; }
        public string postcode { get; set; }
        public string country { get; set; }
    }

    public class CustomParameters
    {
        public string SHOPPER_EndToEndIdentity { get; set; }
        public string CTPE_DESCRIPTOR_TEMPLATE { get; set; }
    }

    public class Risk
    {
        public string score { get; set; }
    }

    public class PaymentStatusResult
    {
        public string id { get; set; }
        public string paymentType { get; set; }
        public string paymentBrand { get; set; }
        public string amount { get; set; }
        public string currency { get; set; }
        public string descriptor { get; set; }
        public string merchantTransactionId { get; set; }
        public Result result { get; set; }
        public ResultDetails resultDetails { get; set; }
        public Card card { get; set; }
        public Customer customer { get; set; }
        public Billing billing { get; set; }
        public CustomParameters customParameters { get; set; }
        public Risk risk { get; set; }
        public string buildNumber { get; set; }
        public string timestamp { get; set; }
        public string ndc { get; set; }
    }


}
