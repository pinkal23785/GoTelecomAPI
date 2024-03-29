using Hyperpay.Aywa.Web.Data.Entities;
using Hyperpay.Aywa.Web.Models;
using Hyperpay.Aywa.Web.SADAD;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Hyperpay.Aywa.Web.Data
{
    public interface IDataService
    {
        public Task InsertPaymentTransactionStatus(CheckoutSuccessResult transactionStatus, string sessionId,
            CheckoutModel parameter, string MerchantTransactionId, string EntityID);
        public Task UpdatePaymentStatus(PaymentStatusResult transactionStatus);

        public Task AddOTP(CustomerOTP otp);

        Task<PaymentTransactionStatus> GetPaymentCheckout(string checkoutId);

        Task<PrisonsAywaCardTran> ProcessAywaCard(string checkoutId, int CardTypeID);

        Task<InvoiceModel> GetInvoiceDetails(string paymentInfo);

        Task ReserveAywaCardTrans(SADADServiceResponse sadad, CheckoutModel parameter);

        Task<List<PrisonsAywaaCardType>> GetAywaaCardType();

        Task<PrisonsAywaaCardType> GetAywaaCardTypeById(int ID);

        Task<int> UploadAwayaCard(int CardType, List<AwayaCardModel> awayaCards);

        Task<PrisonsAywaCardStock> GetPrisonsAywaCardStock(int CardStockID);

        Task<string> GetInvoice(int CardTransID);

        Task<List<TransactionHistory>> SearchTransactionByMobileNumber(string MobileNumber, string TransDate);


    }
    public class DataService : IDataService
    {
        private CADBContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private ILoggerService _loggerService;
        private static SemaphoreSlim readLock = new SemaphoreSlim(1);
        public DataService(CADBContext context, IHttpContextAccessor httpContextAccessor, ILoggerService loggerService)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
            _loggerService = loggerService;

        }
        public async Task InsertPaymentTransactionStatus(CheckoutSuccessResult transactionStatus,
            string sessionId, CheckoutModel parameter, string MerchantTransactionId, string EntityID
            )
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
                    CARDTYPE = parameter.CardType,
                    AMOUNT = parameter.Amount,
                    CURRENCY = "SAR",
                    STREET = parameter.StreetAddress,
                    CITY = parameter.City,
                    STATE = parameter.State,
                    COUNTRY = "SA",
                    POSTALCODE = parameter.PinCode,
                    GIVENNAME = parameter.FirstName,
                    SURNAME = parameter.LastName,
                    CHECKOUTDATE = DateTime.Now.ToString(),
                    EMAIL_ID = parameter.Email,
                    MOBILE_NUMBER = parameter.MobileNumber,
                    USER_AGENT = ""
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
                try
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

                    //_context.PaymentTransactionStatuses.Update(paymentStatus);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException ex)
                {
                    foreach (var entry in ex.Entries)
                    {
                        if (entry.Entity is PaymentWidgetModel)
                        {
                            var proposedValues = entry.CurrentValues;
                            var databaseValues = entry.GetDatabaseValues();

                            foreach (var property in proposedValues.Properties)
                            {
                                var proposedValue = proposedValues[property];
                                var databaseValue = databaseValues[property];

                                // TODO: decide which value should be written to database
                                // proposedValues[property] = <value to be saved>;
                            }

                            // Refresh original values to bypass next concurrency check
                            entry.OriginalValues.SetValues(databaseValues);
                        }
                        else
                        {
                            throw new NotSupportedException(
                                "Don't know how to handle concurrency conflicts for "
                                + entry.Metadata.Name);
                        }
                    }
                }
            }
        }

        public async Task AddOTP(CustomerOTP otp)
        {
            await _context.AddAsync(otp);
            await _context.SaveChangesAsync();
        }

        public async Task<PaymentTransactionStatus> GetPaymentCheckout(string checkoutId)
        {
            return await _context.PaymentTransactionStatuses.Where(x => x.CHECKOUT == checkoutId).FirstOrDefaultAsync();
        }

        public async Task<PrisonsAywaCardTran> ProcessAywaCard(string checkoutId, int CardTypeID)
        {
            DateTime OracleDate;
            DbConnection connection = _context.Database.GetDbConnection();
            if (connection.State == ConnectionState.Closed)
            {
                await connection.OpenAsync();
            }

            var command = connection.CreateCommand();
            command.CommandText = "select sysdate from dual";
            OracleDate = (DateTime)await command.ExecuteScalarAsync();

            using (var transaction = _context.Database.BeginTransaction())
            {
               
                try
                {
                    
                    var paymentTrans = _context.PaymentTransactionStatuses.Where(x => x.CHECKOUT == checkoutId).FirstOrDefault();
                    await readLock.WaitAsync();
                    var selectedCard = await _context.PrisonsAywaCardStocks.Where(x => x.STATUS == "Active"
                    && x.CARD_TYPE_ID == CardTypeID && x.EXPIRY > OracleDate).OrderBy(x => x.EXPIRY).ThenBy(x => x.ID).FirstOrDefaultAsync();

                    await _loggerService.AddLog("Selected Card:" + selectedCard.CARDNUMBER + ", Oracle Current Date:" + OracleDate.ToString());
                    //  var selectedCard = selectedCardList.Where(x => DateTime.ParseExact(x.EXPIRY, "dd/MM/yyyy", CultureInfo.InvariantCulture) > OracleDate).FirstOrDefault();

                    if (selectedCard != null)
                    {
                        selectedCard.STATUS = "Consumed";
                        selectedCard.LAST_MODIFY_DATE = DateTime.Now;
                        selectedCard.LAST_MODIFY_BY = "Public User";
                        await _context.SaveChangesAsync();


                        var objAywaTrans = new PrisonsAywaCardTran()
                        {
                            CARD_STOCK_ID = selectedCard.ID,
                            MOBILE = paymentTrans.MOBILE_NUMBER,
                            EMAIL = paymentTrans.EMAIL_ID,
                            // TRANS_TIME = DateTime.Now.ToString(),
                            TRANS_TYPE = "Consumption",
                            CREATED_BY = "Public User",
                            TOTAL_AMOUNT_VAT = paymentTrans.AMOUNT,
                            PAYMENT_REF = paymentTrans.CONNECTORTXID1,
                            PAYMENT_METHOD = paymentTrans.CARDTYPE.Trim().ToLower() == "mada" ? "Span" : "Credit_Card",
                            CONTACT_PREF = "Mobile",
                            CARD_TYPE_ID = selectedCard.CARD_TYPE_ID,
                            QUANTITY = 1
                            //TRNAS_TIMESTAMP = DateTime.Now


                        };
                        await _context.AddAsync(objAywaTrans);
                        await _context.SaveChangesAsync();
                        transaction.Commit();
                        return objAywaTrans;
                    }
                }
                finally
                {
                    readLock.Release();
                }
            }
            return null;
        }

        public async Task<InvoiceModel> GetInvoiceDetails(string paymentInfo)
        {
            var result = await (from c in _context.PrisonsAywaCardStocks
                                join t in _context.PrisonsAywaCardTrans
                                on c.ID equals t.CARD_STOCK_ID
                                join ct in _context.PrisonsAywaaCardTypes
                                on c.CARD_TYPE_ID equals ct.ID
                                where t.ID == Convert.ToInt32(paymentInfo)
                                select new InvoiceModel
                                {
                                    AywaCardNumber = c.CARDNUMBER,
                                    AywaCardName = ct.NAME,
                                    InvoiceNo = t.INVOICE_ID,
                                    OrderNo = t.INVOICE_ID,
                                    PaymentMethod = t.PAYMENT_METHOD,
                                    Card_Amount = ct.CREDIT,
                                    TotalAmount = t.TOTAL_AMOUNT_VAT,
                                    InvoiceDate = t.TRANS_TIME,
                                    HyperPayTransactionID = t.PAYMENT_REF
                                }).FirstOrDefaultAsync();

            var ObjHyperpay = await _context.PaymentTransactionStatuses.Where(x => x.CONNECTORTXID1 == result.HyperPayTransactionID).FirstOrDefaultAsync();
            if (ObjHyperpay != null)
            {
                result.PaymentMethod = ObjHyperpay.PAYMENT_BRAND;
            }
            return result;
        }

        public async Task ReserveAywaCardTrans(SADADServiceResponse sadad, CheckoutModel parameter)
        {
            var objAywaTrans = new PrisonsAywaCardTran()
            {
                MOBILE = parameter.MobileNumber,
                EMAIL = parameter.Email,
                // TRANS_TIME = DateTime.Now.ToString(),
                TRANS_TYPE = "Order",
                PAYMENT_REF = "cash",
                CREATED_BY = "Public User",
                PAYMENT_METHOD = "SADAD",
                TOTAL_AMOUNT_VAT = parameter.Amount,
                PUR_ORD_RESP_STATUS = "Pending",
                PUR_ORD_SUBSCRIBER_ID = sadad.subscriberId,
                PUR_ORD_ACCOUNT_ID = sadad.accountId,
                CONTACT_PREF = "Mobile",
                LANG_PREF = parameter.Lang == "en" ? "English" : "Arabic",
                CARD_TYPE_ID = Convert.ToInt32(parameter.AywaaCardID),
                MERCHANT_ID = "1",

            };
            await _context.AddAsync(objAywaTrans);
            await _context.SaveChangesAsync();
        }


        public async Task<List<PrisonsAywaaCardType>> GetAywaaCardType()
        {
            return await _context.PrisonsAywaaCardTypes.OrderByDescending(x => x.CREDIT).ToListAsync();

        }

        public async Task<int> UploadAwayaCard(int CardType, List<AwayaCardModel> awayaCards)
        {
            int RecordProcessed = 0;
            foreach (var card in awayaCards)
            {
                using (var transaction = _context.Database.BeginTransaction())
                {
                    try
                    {

                        var objCardType = await _context.PrisonsAywaaCardTypes.Where(x => x.ID == CardType).FirstOrDefaultAsync();
                        if (objCardType != null)
                        {
                            var objCard = await _context.PrisonsAywaCardStocks.Where(x => x.CARDNUMBER == card.CardNumber
                             && x.BATCHNO == card.BatchNo && (x.STATUS == "Active" || x.STATUS == "Inactive")).FirstOrDefaultAsync();

                            if (objCard == null)
                            {
                                var maxCard = await _context.PrisonsAywaCardStocks.MaxAsync(x => x.ID);
                                var NewCard = new PrisonsAywaCardStock()
                                {
                                    ID = maxCard + 1,
                                    CARD_TYPE_ID = CardType,
                                    BATCHNO = card.BatchNo,
                                    CONTROLNO = card.ControlNo,
                                    CARDNUMBER = card.CardNumber,
                                    STATUS = card.Status,
                                    EXPIRY = DateTime.ParseExact(card.ExpiryDate, "dd/MM/yyyy", CultureInfo.InvariantCulture),
                                    BALANCE = objCardType.CREDIT,
                                    INSERT_DATE = DateTime.Now,
                                    INSERT_BY = "al.alnajjar@c.go.com.sa",
                                };
                                await _context.AddAsync(NewCard);
                                await _context.SaveChangesAsync();

                                var maxCardTrans = await _context.PrisonsAywaCardTrans.MaxAsync(x => x.ID);
                                var NewCardTrans = new PrisonsAywaCardTran()
                                {
                                    ID = maxCardTrans + 1,
                                    CARD_STOCK_ID = NewCard.ID,
                                    TRANS_TIME = DateTime.Now.ToString("dd-MMM-yy"),
                                    TRANS_TYPE = "Add",
                                    CREATED_BY = "al.alnajjar@c.go.com.sa",

                                };
                                await _context.AddAsync(NewCardTrans);
                                await _context.SaveChangesAsync();
                                await transaction.CommitAsync();
                                RecordProcessed++;
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        await transaction.RollbackAsync();
                    }
                }
            }
            return RecordProcessed;
        }


        public async Task<PrisonsAywaCardStock> GetPrisonsAywaCardStock(int CardStockID)
        {
            var result = await _context.PrisonsAywaCardStocks.Where(x => x.ID == CardStockID).FirstOrDefaultAsync();
            return result;
        }
        public async Task<PrisonsAywaaCardType> GetAywaaCardTypeById(int ID)
        {
            var result = await _context.PrisonsAywaaCardTypes.Where(x => x.ID == ID).FirstOrDefaultAsync();
            return result;
        }
        public async Task<string> GetInvoice(int CardTransID)
        {
            var result = await _context.PrisonsAywaCardTrans.Where(x => x.ID == CardTransID).Select(x => x.INVOICE_ID).FirstOrDefaultAsync();
            return result;
        }

        public async Task<List<TransactionHistory>> SearchTransactionByMobileNumber(string MobileNumber, string TransDate)
        {
            string SQL = $@"select x.ID, Trans_type,TO_CHAR(TRNAS_TIMESTAMP ,'YY-MON-DD') Trans_time,Email,Mobile,Payment_Ref,Payment_Method,Invoice_ID,CardNumber,Last_Modify_Date,
PUR_ORD_RESP_STATUS,PUR_ORD_SUBSCRIBER_ID,x.pur_ord_account_id,pur_ord_order_id From
PRISONS_AYWA_CARD_TRANS x, PRISONS_AYWA_CARD_STOCK y where x.card_stock_id=y.id ";

            if (!string.IsNullOrEmpty(MobileNumber))
            {
                SQL = SQL + $@"and mobile in ('{MobileNumber.Trim()}') ";
            }
            if (!string.IsNullOrEmpty(TransDate))
            {
                SQL = SQL + $@" and lower(TO_CHAR(TRNAS_TIMESTAMP ,'YY-MON-DD'))=lower('{TransDate.Trim()}')";
            }
            var result = await _context.TransactionHistories.FromSqlRaw(SQL).ToListAsync();
            return result;
        }


    }

}
