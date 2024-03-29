﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.Xml.Serialization;

// 
// This source code was auto-generated by wsdl, Version=4.8.3928.0.
// 


/// <remarks/>
// CODEGEN: The optional WSDL extension element 'operation' from namespace 'http://schemas.xmlsoap.org/wsdl/soap/' was not handled.
// CODEGEN: The optional WSDL extension element 'operation' from namespace 'http://schemas.xmlsoap.org/wsdl/soap/' was not handled.
[System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "4.8.3928.0")]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Web.Services.WebServiceBindingAttribute(Name="installmentPaymentOrderPortTypeBinding", Namespace="http://installmentPaymentOrder.eai.atheeb.net")]
public partial class installmentPaymentOrderService : System.Web.Services.Protocols.SoapHttpClientProtocol {
    
    private System.Threading.SendOrPostCallback installmentPaymentOrderOperationCompleted;
    
    /// <remarks/>
    public installmentPaymentOrderService() {
        this.Url = "http://172.16.68.23:18001/InstallmentPaymentOrder/installmentPaymentOrderPortType" +
            "";
    }
    
    /// <remarks/>
    public event installmentPaymentOrderCompletedEventHandler installmentPaymentOrderCompleted;
    
    /// <remarks/>
    [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://installmentPaymentOrder.eai.atheeb.net:installmentPaymentOrder", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Bare)]
    [return: System.Xml.Serialization.XmlElementAttribute("installmentPaymentOrderResp", Namespace="http://installmentPaymentOrder.eai.atheeb.com")]
    public installmentPaymentOrderResp installmentPaymentOrder([System.Xml.Serialization.XmlElementAttribute(Namespace="http://installmentPaymentOrder.eai.atheeb.com")] installmentPaymentOrderReq installmentPaymentOrderReq) {
        object[] results = this.Invoke("installmentPaymentOrder", new object[] {
                    installmentPaymentOrderReq});
        return ((installmentPaymentOrderResp)(results[0]));
    }
    
    /// <remarks/>
    public System.IAsyncResult BegininstallmentPaymentOrder(installmentPaymentOrderReq installmentPaymentOrderReq, System.AsyncCallback callback, object asyncState) {
        return this.BeginInvoke("installmentPaymentOrder", new object[] {
                    installmentPaymentOrderReq}, callback, asyncState);
    }
    
    /// <remarks/>
    public installmentPaymentOrderResp EndinstallmentPaymentOrder(System.IAsyncResult asyncResult) {
        object[] results = this.EndInvoke(asyncResult);
        return ((installmentPaymentOrderResp)(results[0]));
    }
    
    /// <remarks/>
    public void installmentPaymentOrderAsync(installmentPaymentOrderReq installmentPaymentOrderReq) {
        this.installmentPaymentOrderAsync(installmentPaymentOrderReq, null);
    }
    
    /// <remarks/>
    public void installmentPaymentOrderAsync(installmentPaymentOrderReq installmentPaymentOrderReq, object userState) {
        if ((this.installmentPaymentOrderOperationCompleted == null)) {
            this.installmentPaymentOrderOperationCompleted = new System.Threading.SendOrPostCallback(this.OninstallmentPaymentOrderOperationCompleted);
        }
        this.InvokeAsync("installmentPaymentOrder", new object[] {
                    installmentPaymentOrderReq}, this.installmentPaymentOrderOperationCompleted, userState);
    }
    
    private void OninstallmentPaymentOrderOperationCompleted(object arg) {
        if ((this.installmentPaymentOrderCompleted != null)) {
            System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
            this.installmentPaymentOrderCompleted(this, new installmentPaymentOrderCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
        }
    }
    
    /// <remarks/>
    public new void CancelAsync(object userState) {
        base.CancelAsync(userState);
    }
}

/// <remarks/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "4.8.3928.0")]
[System.SerializableAttribute()]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(Namespace="http://installmentPaymentOrder.eai.atheeb.com")]
public partial class installmentPaymentOrderReq {
    
    private string accountNumberField;
    
    private string contactNumberField;
    
    private string installmantPeriodField;
    
    private decimal totalDueAmountField;
    
    private bool totalDueAmountFieldSpecified;
    
    private System.DateTime billGenTimeStampField;
    
    private bool billGenTimeStampFieldSpecified;
    
    private System.DateTime expDateField;
    
    private bool expDateFieldSpecified;
    
    private string notificationLanguageField;
    
    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
    public string AccountNumber {
        get {
            return this.accountNumberField;
        }
        set {
            this.accountNumberField = value;
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
    public string ContactNumber {
        get {
            return this.contactNumberField;
        }
        set {
            this.contactNumberField = value;
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, DataType="integer")]
    public string InstallmantPeriod {
        get {
            return this.installmantPeriodField;
        }
        set {
            this.installmantPeriodField = value;
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
    public decimal TotalDueAmount {
        get {
            return this.totalDueAmountField;
        }
        set {
            this.totalDueAmountField = value;
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlIgnoreAttribute()]
    public bool TotalDueAmountSpecified {
        get {
            return this.totalDueAmountFieldSpecified;
        }
        set {
            this.totalDueAmountFieldSpecified = value;
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
    public System.DateTime BillGenTimeStamp {
        get {
            return this.billGenTimeStampField;
        }
        set {
            this.billGenTimeStampField = value;
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlIgnoreAttribute()]
    public bool BillGenTimeStampSpecified {
        get {
            return this.billGenTimeStampFieldSpecified;
        }
        set {
            this.billGenTimeStampFieldSpecified = value;
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
    public System.DateTime ExpDate {
        get {
            return this.expDateField;
        }
        set {
            this.expDateField = value;
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlIgnoreAttribute()]
    public bool ExpDateSpecified {
        get {
            return this.expDateFieldSpecified;
        }
        set {
            this.expDateFieldSpecified = value;
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
    public string NotificationLanguage {
        get {
            return this.notificationLanguageField;
        }
        set {
            this.notificationLanguageField = value;
        }
    }
}

/// <remarks/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "4.8.3928.0")]
[System.SerializableAttribute()]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(Namespace="http://installmentPaymentOrder.eai.atheeb.com")]
public partial class installmentPaymentOrderResp {
    
    private string statusCodeField;
    
    private string descriptionField;
    
    private string statusField;
    
    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
    public string StatusCode {
        get {
            return this.statusCodeField;
        }
        set {
            this.statusCodeField = value;
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
    public string Description {
        get {
            return this.descriptionField;
        }
        set {
            this.descriptionField = value;
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
    public string Status {
        get {
            return this.statusField;
        }
        set {
            this.statusField = value;
        }
    }
}

/// <remarks/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "4.8.3928.0")]
public delegate void installmentPaymentOrderCompletedEventHandler(object sender, installmentPaymentOrderCompletedEventArgs e);

/// <remarks/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "4.8.3928.0")]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
public partial class installmentPaymentOrderCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
    
    private object[] results;
    
    internal installmentPaymentOrderCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
            base(exception, cancelled, userState) {
        this.results = results;
    }
    
    /// <remarks/>
    public installmentPaymentOrderResp Result {
        get {
            this.RaiseExceptionIfNecessary();
            return ((installmentPaymentOrderResp)(this.results[0]));
        }
    }
}