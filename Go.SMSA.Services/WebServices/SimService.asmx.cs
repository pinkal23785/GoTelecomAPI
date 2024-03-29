using Go.SMSA.Services.ResponseModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;

namespace Go.SMSA.Services.WebServices
{
    /// <summary>
    /// Summary description for SimService
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class SimService : System.Web.Services.WebService
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        [WebMethod]
        public SIMDetailsResponseModel GetSIMDetails(string SerialNo)
        {
            try
            {
                SMSAService sMSAService = new SMSAService();
                var result = sMSAService.GetSIMDetails(SerialNo).Result;
                return result;
            }
            catch(Exception ex)
            {
                log.Error(ex.Message + " " + ex.InnerException);
                throw ex;
            }
        }
    }
}
