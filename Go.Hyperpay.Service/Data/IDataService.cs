using Go.Hyperpay.Service.Data.Entities;
using Go.Hyperpay.Service.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Go.Hyperpay.Service.Data
{
    public interface IDataService
    {
        public Task InsertPaymentTransactionStatus(CheckoutSuccessResult transactionStatus,string sessionId, 
            CheckoutModel parameter,string MerchantTransactionId,string EntityID);
        public Task UpdatePaymentStatus(PaymentStatusResult transactionStatus);
    }
}
