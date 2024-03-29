
using Go.LTEWallet.Services.Data.Entities;
using Go.LTEWallet.Services.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Go.LTEWallet.Services.Data
{
    public class DataService : IDataService
    {
        private CADBContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private PortalDBContext _portalDBContext;
        public DataService(CADBContext context, IHttpContextAccessor httpContextAccessor, PortalDBContext portalDBContext)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
            _portalDBContext = portalDBContext;
        }
        public async Task<List<CommissionPlan>> GetCommissionPlan()
        {
            string query = "select plan_id PLANID, plan_name PLANNAME, price PLANPRICE From plan_commission_config where is_Active =1";
            //" over(partition by DEAL.PLAN_ID order by PA.attrib_value desc) RN FROM PROD_CATALOG_PRODUCT_TBL PRO, prod_catalog_deal_tbl DEAL," +
            //" PROD_CATALOG_ATTRIBUTES_TBL PA,plan_commission_config pc WHERE pro.deal_id = DEAL.DEAL_ID AND PA.PROD_ID = pro.prod_id AND  " +
            //"pc.plan_id=deal.plan_id and is_active = 1 and PA.ATTRIB_NAME like '/event/billing/product/fee/cycle/cycle_forward_%');";
            var commissionPlans = await _context.CommissionPlans.FromSqlRaw(query).ToListAsync();
            return commissionPlans;
        }
        public async Task<CustomerWalletBalance> GetCommissionWalletBalance(string MobileNumber)
        {
            var result = await (from user in _context.CommissionMerchantUsers
                                join balance in _context.CommissionWalletBalances
                                on user.MERCHANTID.Trim() equals balance.MERCHANTID.Trim()
                                where user.MOBILENUMBER == MobileNumber
                                select new CustomerWalletBalance
                                {
                                    ID = balance.ID,
                                    INSERT_BY = balance.INSERT_BY,
                                    INSERT_DATE = balance.INSERT_DATE != null ? null : balance.INSERT_DATE,
                                    LAST_TRANS_ID = balance.LAST_TRANS_ID,
                                    USER_FULL_NAME_AR = balance.USER_FULL_NAME_AR,
                                    USER_FULL_NAME_EN = balance.USER_FULL_NAME_EN,
                                    WALLET_BALANCE = balance.WALLET_BALANCE,
                                    MOBILENUMBER = user.MOBILENUMBER,
                                    MerchantID=user.MERCHANTID
                                }).OrderByDescending(x => x.WALLET_BALANCE).FirstOrDefaultAsync();

            return result;
        }
        public async Task<bool> ValidateWalletBalance(string MobileNumber, string PlanId)
        {
            try
            {
                var walletBalance = await GetCommissionWalletBalance(MobileNumber);
                if (walletBalance == null)
                    throw new RowNotInTableException("User not found");
                // var result = _context.raw
                string query = "select plan_id PLANID, plan_name PLANNAME, price PLANPRICE From plan_commission_config where is_Active =1 and plan_id =" + PlanId;

                var planDetail = await _context.CommissionPlans.FromSqlRaw(query).ToListAsync();

                if (planDetail == null)
                    throw new RowNotInTableException("Plan not found");
                if (walletBalance.WALLET_BALANCE != null && planDetail[0].PlanPrice != null)
                {
                    int NumberOfCards = (int)(walletBalance.WALLET_BALANCE / planDetail[0].PlanPrice);
                    if (NumberOfCards >= 1)
                        return true;
                }

                return false;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public async Task<dynamic> GetPlanVoucher(string MobileNumber, string PlanId)
        {
            StringBuilder strQuery = new StringBuilder();
            if (await ValidateWalletBalance(MobileNumber, PlanId))
            {

                //strQuery.Append("select Pkg_Password_Utility_TPSC.decryptVoucherData(tuv_voucher.PIN_NUMBER) as SERIAL_NUMBER from TUV_VOUCHER_DETAILS_TBL tuv_voucher, TUV_VOUCHER_ORDER_DETAILS_TBL tuv_order");
                //strQuery.Append(" where tuv_order.voucher_plan ='" + PlanId + "'");
                //strQuery.Append(" and tuv_voucher.voucher_status in (35,23)");
                //strQuery.Append(" and tuv_order.Voucher_Expiry_Date > sysdate and tuv_voucher.tuv_order_id = tuv_order.tuv_order_id and rownum < 2");

                //var planVoucher = await _context.PlanVouchers.FromSqlRaw(strQuery.ToString()).FirstOrDefaultAsync();

                OracleParameter p1 = new OracleParameter("piv_plan_id", PlanId);
                OracleParameter p2 = new OracleParameter("pov_voucher", OracleDbType.Varchar2, 50, null, ParameterDirection.Output);
                OracleParameter p3 = new OracleParameter("pov_status", OracleDbType.Varchar2, 50, null, ParameterDirection.Output);
                string query = "BEGIN PKG_LTE.prc_lte_merchant_vou_select(:piv_plan_id, :pov_voucher, :pov_status);END;";
                var result = _context.Database.ExecuteSqlRaw(query, p1, p2, p3);
                return p2.Value;
                //return planVoucher;
            }
            else
            {
                throw new InsufficientExecutionStackException("Insufficient wallet balance");
            }
        }

        public async Task<VoucherDetail> GetVoucherDetails(string SerialNumber)
        {
            StringBuilder strQuery = new StringBuilder();

            strQuery.Append("select tuv_order.TUV_ORDER_ID, tuv_voucher.SERIAL_NUMBER from tuv_voucher_details_tbl tuv_voucher, tuv_voucher_order_details_tbl tuv_order");
            strQuery.Append(" where tuv_order.voucher_plan in (SELECT PLAN_ID FROM PLAN_COMMISSION_CONFIG) ");
            strQuery.Append(" and Pkg_Password_Utility_TPSC.decryptVoucherData(tuv_voucher.PIN_NUMBER) ='" + SerialNumber + "'");
            strQuery.Append(" and tuv_voucher.tuv_order_id=tuv_order.tuv_order_id");

            var voucherDetail = await _context.VoucherDetails.FromSqlRaw(strQuery.ToString()).FirstOrDefaultAsync();
            return voucherDetail;

        }
        public async Task UpdateVoucherStatus(string SerialNumber)
        {
            StringBuilder strQuery = new StringBuilder();
            strQuery.Append("update tuv_voucher_details_tbl set VOUCHER_STATUS=11 , REDEMPTION_DATE=sysdate where (PIN_NUMBER)=Pkg_Password_Utility_TPSC.encryptVoucherData(" + SerialNumber + ");");
            await _context.Database.ExecuteSqlRawAsync(strQuery.ToString());
        }
        public async Task<string> RedeemVoucher(string MobileNumber, string PlanId, string SerialNumber)
        {
            try
            {
                if (await ValidateWalletBalance(MobileNumber, PlanId))
                {
                    OracleParameter p1 = new OracleParameter("piv_mobile_id", MobileNumber);
                    OracleParameter p2 = new OracleParameter("piv_plan_id", PlanId);
                    OracleParameter p3 = new OracleParameter("piv_vourcher_id", SerialNumber);
                    OracleParameter p4 = new OracleParameter("pov_status", OracleDbType.Varchar2, 50, null, ParameterDirection.Output);
                    string query = "BEGIN PKG_LTE.prc_lte_merchant_redeem_update(:piv_mobile_id, :piv_plan_id, :piv_vourcher_id, :pov_status);END;";
                    var result = _context.Database.ExecuteSqlRaw(query, p1, p2, p3, p4);
                    return p4.Value.ToString();
                }
                else
                {
                    return "Failed";
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<string> RedeemVoucherByMerchantID(string MerchantID, string PlanId, string SerialNumber)
        {
            try
            {
               // if (await ValidateWalletBalance(MobileNumber, PlanId))
               // {
                    OracleParameter p1 = new OracleParameter("piv_merchant_id", MerchantID);
                    OracleParameter p2 = new OracleParameter("piv_plan_id", PlanId);
                    OracleParameter p3 = new OracleParameter("piv_vourcher_id", SerialNumber);
                    OracleParameter p4 = new OracleParameter("pov_status", OracleDbType.Varchar2, 50, null, ParameterDirection.Output);
                    string query = "BEGIN PKG_LTE.prc_lte_merc_redeem_update_new(:piv_merchant_id, :piv_plan_id, :piv_vourcher_id, :pov_status);END;";
                    var result = _context.Database.ExecuteSqlRaw(query, p1, p2, p3, p4);
                    return p4.Value.ToString();
                //}
                //else
                //{
                //    return "Failed";
                //}
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<List<RenewalOrderLogs>> GetRenewalOrderLogs(string MobileNumber)
        {
            string query = @"select Pkg_Password_Utility_TPSC.decryptVoucherData(card_number)CARDPIN,trans_time CARDDURATION,
                            redemption_amount USEDPOINTS,plan_name PLANNAME from COMMISSION_WALLET_REDEM_TRANS x, 
                            PLAN_COMMISSION_CONFIG y, commission_merchant_users z where x.renewal_plan_id =y.plan_id and
                            x.merchantid=z.merchantid and
                            z.mobilenumber='" + MobileNumber + "'";
            var accountDetail = await _context.RenewalOrders.FromSqlRaw(query).ToListAsync();
            return accountDetail;
        }

        public async Task AddOTP(CustomerOTP oTP)
        {
            await _context.CustomerOTPs.AddAsync(oTP);
            await _context.SaveChangesAsync();
        }

        public async Task<string> UpdateMechantInfo(string Name, string Mobile, string City)
        {
            var objMerchant = await _context.CommissionMerchantUsers.Where(x => x.MOBILENUMBER == Mobile).FirstOrDefaultAsync();
            if (objMerchant != null)
            {
                var objMerchantBalance = await _context.CommissionWalletBalances.Where(x => x.MERCHANTID == objMerchant.MERCHANTID).FirstOrDefaultAsync();
                if (objMerchantBalance != null)
                {
                    objMerchantBalance.USER_FULL_NAME_EN = Name;
                    objMerchant.CITY = City;
                }
                objMerchant.CITY = City;
                objMerchant.FULLNAME = Name;
                await _context.SaveChangesAsync();

                var portalMerchant = await _portalDBContext.HotLTEMerchants.Where(x => x.MobileNumber == Mobile).FirstOrDefaultAsync();
                if (portalMerchant != null)
                {
                    portalMerchant.FullName = Name;
                    portalMerchant.City = City;
                    await _portalDBContext.SaveChangesAsync();
                }
                return "success";
            }
            return "failed";
        }
        public async Task<string> UpdateMechantWithMobile(string Name, string OldMobile, string NewMobile, string City, string OTP)
        {
            var objMerchant = await _context.CommissionMerchantUsers.Where(x => x.MOBILENUMBER == OldMobile).FirstOrDefaultAsync();
            if (objMerchant == null)
                return "User Not Found";
            var objValidOTP = await _context.CustomerOTPs.Where(x => x.MOBILENUMBER == NewMobile && x.OTP == OTP && x.OTP_EXPIRE_DATE >= DateTime.Now).FirstOrDefaultAsync();

            if (objValidOTP == null)
                return "Invalid OTP";
            if (objMerchant != null && objValidOTP != null)
            {
                var objMerchantBalance = await _context.CommissionWalletBalances.Where(x => x.MERCHANTID == objMerchant.MERCHANTID).FirstOrDefaultAsync();
                if (objMerchantBalance != null)
                {
                    objMerchantBalance.USER_ID = NewMobile;
                    objMerchantBalance.USER_FULL_NAME_EN = Name;
                    objMerchant.CITY = City;
                }
                objMerchant.CITY = City;
                objMerchant.FULLNAME = Name;
                objMerchant.MOBILENUMBER = NewMobile;
                objValidOTP.IS_VERIFIED = "1";
                await _context.SaveChangesAsync();

                var portalMerchant = await _portalDBContext.HotLTEMerchants.Where(x => x.MobileNumber == OldMobile).FirstOrDefaultAsync();
                if (portalMerchant != null)
                {
                    portalMerchant.FullName = Name;
                    portalMerchant.City = City;
                    portalMerchant.MobileNumber = NewMobile;
                    await _portalDBContext.SaveChangesAsync();
                }
                return "success";
            }
            return "failed";
        }

        public async Task<Order> GetOrder(string MobileNumber)
        {
            string queryOrderSummay = @$"SELECT y.PLAN_NAME PlanName,COUNT (*) NumberOfOrders,
            SUM (x.REDEMPTION_AMOUNT) Total_Credit_Amount FROM COMMISSION_WALLET_REDEM_TRANS x,
            PLAN_DETAILS_TBL y, commission_merchant_users z WHERE x.renewal_plan_id = y.plan_id
            AND x.merchantid = z.merchantid AND z.mobilenumber='{MobileNumber}' and
            trans_type = 'Credit' GROUP BY y.plan_name, z.mobilenumber;";

            string queryOrderDetail = $@"SELECT RENEWAL_ORDER_ID OrderId,trans_time OrderDate,plan_name PlanName,REDEMPTION_AMOUNT Credit_Amount,
            WALLET_BALANCE_BEFORE Balance_Before,WALLET_BALANCE_AFTER Balance_After,  newSIMDet.sim_serial SIM, newDeviceDet.device_serial DEVICE_SN
            FROM COMMISSION_WALLET_REDEM_TRANS x,
            PLAN_DETAILS_TBL y,
            commission_merchant_users z,
            (select order_id, ATTRIB_VALUE device_serial from order_attr_tbl where ATTRIB_NAME 
             in('NewDeviceSerialNumber','LTESerialNo')) newDeviceDet,
            (select order_id, ATTRIB_VALUE sim_serial from order_attr_tbl where ATTRIB_NAME in('NewSIMICCID','ICCID')) newSIMDet
            WHERE x.renewal_plan_id = y.plan_id
            AND x.merchantid = z.merchantid
            AND x.RENEWAL_ORDER_ID=newDeviceDet.order_id
            AND x.RENEWAL_ORDER_ID=newSIMDet.order_id
            AND z.mobilenumber='{MobileNumber}'
            and
            trans_type = 'Credit';";

            var result = await _context.OrderSummaries.FromSqlRaw(queryOrderSummay).ToListAsync();
            var result2 = await _context.OrderDetails.FromSqlRaw(queryOrderDetail).ToListAsync();

            Order newOrder = new Order();
            newOrder.OrderSummaries = result;
            newOrder.OrderDetails = result2;

            return newOrder;
        }
    }
}
