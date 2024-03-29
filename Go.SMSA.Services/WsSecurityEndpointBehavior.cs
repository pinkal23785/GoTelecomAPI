using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;
using System.ServiceModel.Dispatcher;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace GO.SMSA.Service
{
    public class WsSecurityEndpointBehavior : IEndpointBehavior
    {
        private readonly string password;
        private readonly string username;
        private readonly string Id;
        public WsSecurityEndpointBehavior(string username, string password,string Id)
        {
            this.username = username;
            this.password = password;
            this.Id = Id;
        }
        public void AddBindingParameters(ServiceEndpoint endpoint, BindingParameterCollection bindingParameters)
        {

        }

        public void ApplyClientBehavior(ServiceEndpoint endpoint, ClientRuntime clientRuntime)
        {
            clientRuntime.ClientMessageInspectors.Add(new WsSecurityMessageInspector(username, password,Id));
        }

        public void ApplyDispatchBehavior(ServiceEndpoint endpoint, EndpointDispatcher endpointDispatcher)
        {

        }

        public void Validate(ServiceEndpoint endpoint)
        {

        }
    }
    public class WsSecurityMessageInspector : IClientMessageInspector
    {
        private readonly string password;
        private readonly string username;
        private readonly string Id;

        public WsSecurityMessageInspector(string username, string password, string Id)
        {
            this.username = username;
            this.password = password;
            this.Id = Id;
        }

        object IClientMessageInspector.BeforeSendRequest(ref Message request, IClientChannel channel)
        {
            var header = new Security
            {
                UsernameToken =
            {
                Password = new Password
                {
                    Value = password,
                    Type =
                        "http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-username-token-profile-1.0#PasswordText"
                },
                Username = username,
                Id=Id
            }
            };
            request.Headers.Add(header);
            return null;
        }

        void IClientMessageInspector.AfterReceiveReply(ref Message reply, object correlationState)
        {
        }
    }

    public class Password
    {
        [XmlAttribute] public string Type { get; set; }

        [XmlText] public string Value { get; set; }
    }

    [XmlRoot(Namespace = "http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-secext-1.0.xsd")]
    public class UsernameToken
    {
        [XmlAttribute] public string Id { get; set; }
        [XmlElement] public string Username { get; set; }

        [XmlElement] public Password Password { get; set; }
    }

    public class Security : MessageHeader
    {
        public Security()
        {
            UsernameToken = new UsernameToken();
        }

        public UsernameToken UsernameToken { get; set; }

        public override string Name => GetType().Name;

        public override string Namespace =>
            "http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-secext-1.0.xsd";

        public override bool MustUnderstand => true;

        protected override void OnWriteHeaderContents(XmlDictionaryWriter writer, MessageVersion messageVersion)
        {
            var serializer = new XmlSerializer(typeof(UsernameToken));
            serializer.Serialize(writer, UsernameToken);
        }
    }

}
