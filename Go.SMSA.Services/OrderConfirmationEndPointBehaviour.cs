using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;
using System.ServiceModel.Dispatcher;
using System.Web;
using System.Xml;
using System.Xml.Serialization;

namespace Go.SMSA.Services.OrderConfirmationBehavior
{
    public class OrderConfirmationEndPointBehaviour : IEndpointBehavior
    {
        private readonly string password;
        private readonly string username;
        private readonly string authentication;
        private readonly string locale;
        private readonly string timeZone;

        public OrderConfirmationEndPointBehaviour(string username, string password, string authentication, string locale, string timezone)
        {
            this.username = username;
            this.password = password;
            this.authentication = authentication;
            this.locale = locale;
            this.timeZone = timezone;

        }

        public void AddBindingParameters(ServiceEndpoint endpoint, BindingParameterCollection bindingParameters)
        {

        }

        public void ApplyClientBehavior(ServiceEndpoint endpoint, ClientRuntime clientRuntime)
        {
            clientRuntime.ClientMessageInspectors.Add(new OCSecurityMessageInspector(username, password, authentication, locale, timeZone));
        }

        public void ApplyDispatchBehavior(ServiceEndpoint endpoint, EndpointDispatcher endpointDispatcher)
        {

        }

        public void Validate(ServiceEndpoint endpoint)
        {

        }

    }
    public class OCSecurityMessageInspector : IClientMessageInspector
    {
        private readonly string password;
        private readonly string username;
        private readonly string authentication;
        private readonly string locale;
        private readonly string timeZone;

        public OCSecurityMessageInspector(string username, string password, string authentication, string locale, string timezone)
        {
            this.username = username;
            this.password = password;
            this.authentication = authentication;
            this.locale = locale;
            this.timeZone = timezone;
        }

        object IClientMessageInspector.BeforeSendRequest(ref Message request, IClientChannel channel)
        {
            //var header = new AuthenticationInfo
            //{
            //    authenticationInfo =
            //    {
            //        UserName = username,
            //        Password = password,
            //        Authentication=authentication,
            //        Locale=locale,
            //        TimeZone=timeZone
            //    }
            //};

            var header = new CustomHeader();
            request.Headers.Add(header);
            return null;
        }

        void IClientMessageInspector.AfterReceiveReply(ref Message reply, object correlationState)
        {
        }
    }
    
   

    public class CustomHeader : MessageHeader
    {
        public override string Name => GetType().Name;
        public override string Namespace =>"";
        public override bool MustUnderstand => true;
        protected override void OnWriteHeaderContents(XmlDictionaryWriter writer, MessageVersion messageVersion)
        {
            //var serializer = new XmlSerializer(typeof(Auth));
            //serializer.Serialize(writer, authenticationInfo);

            writer.WriteStartElement("urn:AuthenticationInfo");
            writer.WriteElementString("urn:userName", "webuser");
            writer.WriteElementString("urn:password", "Atheeb@123");
            writer.WriteElementString("urn:authentication", "");
            writer.WriteElementString("urn:locale", "");
            writer.WriteElementString("urn:timeZonee", "");
            writer.WriteEndElement();
        }
    }
}
