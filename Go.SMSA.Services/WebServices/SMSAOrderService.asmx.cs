using Go.SMSA.Services.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Services;

namespace Go.SMSA.Services.WebServices
{
    /// <summary>
    /// Summary description for SMSAOrderService
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.None)]
    [System.ComponentModel.ToolboxItem(false)]
    
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class SMSAOrderService : System.Web.Services.WebService
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        [WebMethod]
        public int CreateOrder(RemedyOrderRequest requestModel)
        {
            log.Info("Remedy service called");
            File.AppendAllText("D:\\Applications\\SMSA\\SMSALog.txt", JsonConvert.SerializeObject(requestModel) + Environment.NewLine);
            log.Info(JsonConvert.SerializeObject(requestModel));



            SMSAService sMSAService = new SMSAService();
            SMSAOrderRequestModel order = new SMSAOrderRequestModel();
            try
            {
                order.customerIdentifier = new CustomerIdentifier()
                {
                    id = requestModel.CustomerID
                };
                order.facilityIdentifier = new FacilityIdentifier() { id = requestModel.FacilityID };
                order.referenceNum = requestModel.OrderReferenceNumber;
                order.billingCode = requestModel.BillingCode;
                order.routingInfo = new RoutingInfo()
                {
                    carrier = requestModel.Carrier,
                    mode = requestModel.Mode
                };
                order.shipTo = new ShipTo()
                {
                    address1 = requestModel.Address1,
                    address2 = requestModel.Address2,
                    city = requestModel.City,
                    companyName = requestModel.CompanyName,
                    country = "SA",
                    name = requestModel.Name,
                    state = requestModel.State,
                    zip = requestModel.Zip

                };
                order.orderItems = new List<OrderItem>();

                if (!string.IsNullOrEmpty(requestModel.LTESKU) && requestModel.LTEQty != 0)
                {
                    var LTEnewitemIdentifier = new ItemIdentifier()
                    {
                        sku = requestModel.LTESKU
                    };
                    var LTEOrderItem = new OrderItem()
                    {
                        itemIdentifier = LTEnewitemIdentifier,
                        qty = (int)(requestModel.LTEQty)
                    };
                    order.orderItems.Add(LTEOrderItem);
                }
                if (!string.IsNullOrEmpty(requestModel.SIMSKU) && requestModel.SIMQty != 0)
                {
                    var SIMnewitemIdentifier = new ItemIdentifier()
                    {
                        sku = requestModel.SIMSKU
                    };
                    var SIMOrderItem = new OrderItem()
                    {
                        itemIdentifier = SIMnewitemIdentifier,
                        qty = (int)(requestModel.SIMQty)
                    };
                    order.orderItems.Add(SIMOrderItem);
                }
                
                return sMSAService.CreateOrder(order).Result;
                //return 1;
            }
            catch(Exception ex)
            {
                log.Error(ex.Message + " " + ex.InnerException);
                throw ex;
            }
        }
    }
}
