using Go.Hyperpay.Service.Data.Entities;
using Go.Hyperpay.Service.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Go.Hyperpay.Service.Data
{
    public class DataService : IDataService
    {
        private CADBContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public DataService(CADBContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }
        public async Task InsertPaymentTransactionStatus(CheckoutSuccessResult transactionStatus,
            string sessionId, CheckoutModel parameter, string MerchantTransactionId, string EntityID)
        {

            var IsCheckoutExist = (from pt in _context.PaymentTransactionStatuses
                                   where pt.CHECKOUT == transactionStatus.id
                                   select pt).Any();
            if (!IsCheckoutExist)
            {
                int newTransactionMax = 0;
                if (_context.PaymentTransactionStatuses.Count() > 0)
                {
                    newTransactionMax = _context.PaymentTransactionStatuses.Max(x => x.ID);
                }
                var newTransaction = new PaymentTransactionStatus()
                {
                    ID = newTransactionMax + 1,
                    CHECKOUT = transactionStatus.id,
                    NDC = transactionStatus.ndc,
                    CLIENT_SESSION_ID = sessionId,
                    //MOBILENUMBER = parameter.mobileNumber,
                    MERCHANTTRANSACTIONID = MerchantTransactionId,
                    ENTITYID = EntityID,
                    CARDTYPE = parameter.cardType,
                    AMOUNT = parameter.amount,
                    CURRENCY = parameter.currency,
                    STREET = parameter.billingStreet1,
                    CITY = parameter.billingCity,
                    STATE = parameter.billingState,
                    COUNTRY = parameter.billingCountry,
                    POSTALCODE = parameter.billingPostcode,
                    GIVENNAME = parameter.customerGivenName,
                    SURNAME = parameter.surname,
                    CHECKOUTDATE = DateTime.Now.ToString(),
                    EMAIL_ID=parameter.customerEmail,
                    MOBILE_NUMBER=parameter.mobileNumber,
                    USER_AGENT=parameter.userAgent
                };
                await _context.PaymentTransactionStatuses.AddAsync(newTransaction);
                await _context.SaveChangesAsync();
            }
        }

        public async Task UpdatePaymentStatus(PaymentStatusResult result)
        {
            var paymentStatus = (from pt in _context.PaymentTransactionStatuses
                                 where pt.CHECKOUT == result.ndc
                                 select pt).FirstOrDefault();
            if (paymentStatus != null)
            {

                paymentStatus.CLIENT_IP = "";// _httpContextAccessor.HttpContext.Request.HttpContext.Connection.RemoteIpAddress.ToString();
                paymentStatus.CLIENT_PORT = "";// _httpContextAccessor.HttpContext.Request.Host.Port.Value.ToString();
                paymentStatus.CLIENT_CONTEXT = "";// _httpContextAccessor.HttpContext.Request.HttpContext.Request.Path;
                paymentStatus.HYPERPAY_TRANS_ID = result.resultDetails != null ? result.resultDetails.TransactionReceipt : "";
                paymentStatus.PAYMENT_TYPE = result.paymentType;
                paymentStatus.PAYMENT_BRAND = result.paymentBrand;
                paymentStatus.PAYMENT_AMOUNT = result.amount;
                paymentStatus.PAYMENT_CURRENCY = result.currency;
                paymentStatus.PAYMENT_DESCRIPTOR = result.descriptor;
                paymentStatus.MERCHANT_TRANSACTION_ID = result.merchantTransactionId;
                paymentStatus.RESULT_CODE = result.result.code;
                paymentStatus.RESULT_DESCRIPTION = result.result.description;
                paymentStatus.CONNECTORTXID1 = result.resultDetails != null ? result.resultDetails.ConnectorTxID1 : "";
                paymentStatus.CARD_BIN = result.card != null ? result.card.bin : "";
                paymentStatus.CARD_BIN_COUNTRY = result.card != null ? result.card.binCountry : "";
                paymentStatus.CARD_LAST_4DIGITS = result.card != null ? result.card.last4Digits : "";
                paymentStatus.CARD_HOLDER = result.card != null ? result.card.holder : "";
                paymentStatus.CARD_EXPIRY_MONTH = result.card != null ? result.card.expiryMonth : "";
                paymentStatus.CARD_EXPIRY_YEAR = result.card != null ? result.card.expiryYear : "";
                paymentStatus.CUST_GIVEN_NAME = result.customer != null ? result.customer.givenName : "";
                paymentStatus.CUST_SURNAME = result.customer != null ? result.customer.surname : "";
                paymentStatus.CUST_EMAIL = result.customer != null ? result.customer.email : "";
                paymentStatus.CUST_IP = result.customer != null ? result.customer.ip : "";
                paymentStatus.CUST_COUNTRY = result.customer != null ? result.customer.ipCountry : "";
                paymentStatus.BILLING_STREET1 = result.billing != null ? result.billing.street1 : "";
                paymentStatus.BILLING_CITY = result.billing != null ? result.billing.city : "";
                paymentStatus.BILLING_STATE = result.billing != null ? result.billing.state : "";
                paymentStatus.BILLING_POST_CODE = result.billing != null ? result.billing.postcode : "";
                paymentStatus.BILLING_COUNTRY = result.billing != null ? result.billing.country : "";
                paymentStatus.SHOPPER_ENDTOENDIDENTITY = result.customParameters != null ? result.customParameters.SHOPPER_EndToEndIdentity : "";
                paymentStatus.CTPE_DESCRIPTOR_TEMPLATE = result.customParameters != null ? result.customParameters.CTPE_DESCRIPTOR_TEMPLATE : "";
                paymentStatus.SCORE = result.risk != null ? result.risk.score : "";
                paymentStatus.BUILD_NUMBER = result.buildNumber;
                paymentStatus.TRANS_TIMESTAMP = result.timestamp;
                paymentStatus.NDC = result.ndc;
                paymentStatus.ERROR_PARAM_NAME = "";
                paymentStatus.ERROR_PARAM_VAL = "";
                paymentStatus.ERROR_PARAM_MSG = "";
                paymentStatus.CLIENT_TRANSACTION_ID = "";
                paymentStatus.TRANSACTION_TIMESTAMP = "";

                _context.PaymentTransactionStatuses.Update(paymentStatus);
                await _context.SaveChangesAsync();
            }
        }

    }
}
