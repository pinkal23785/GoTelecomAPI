﻿//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Go.SMSA.Services
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Core.Objects;
    using System.Data.Entity.Infrastructure;
    using System.Linq;
    
    public partial class CADBEntities : DbContext
    {
        public CADBEntities()
            : base("name=CADBEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
    
        public virtual int EQUIPMENT_SIM_DETAILS(string p_SERIAL_ID, string v_TYPE, ObjectParameter v_IMSI, ObjectParameter v_MSISDN, ObjectParameter v_ICCID, 
            ObjectParameter v_MACID, ObjectParameter v_CPE_MODEL, ObjectParameter v_STATUS)
        {
            var p_SERIAL_IDParameter = p_SERIAL_ID != null ?
                new ObjectParameter("P_SERIAL_ID", p_SERIAL_ID) :
                new ObjectParameter("P_SERIAL_ID", typeof(string));
    
            var v_TYPEParameter = v_TYPE != null ?
                new ObjectParameter("V_TYPE", v_TYPE) :
                new ObjectParameter("V_TYPE", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("EQUIPMENT_SIM_DETAILS", p_SERIAL_IDParameter, v_TYPEParameter, v_IMSI,
                v_MSISDN, v_ICCID, v_MACID, v_CPE_MODEL, v_STATUS);
        }
    }
}
