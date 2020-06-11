using ONVIF_Manager.Models;
using System;
using System.Collections.Generic;
using System.Net;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Text;

namespace ONVIF_Manager.BusinessLayer
{
    public class ServicesHelper
    {
        public static string ReplaceHost(string original, string newHostName)
        {
            var builder = new UriBuilder(original);
            builder.Host = newHostName;
            return builder.Uri.ToString();
        }

        public static ClientBase<TChannel> CreateServiceClient<TChannel>(string serviceAddress, string username, string password, Func<CustomBinding, EndpointAddress, ClientBase<TChannel>> creator) 
            where TChannel : class
        {
            var binding = new CustomBinding
            {
                Elements =  {
                    new TextMessageEncodingBindingElement
                    {
                        MessageVersion = MessageVersion.CreateVersion(EnvelopeVersion.Soap12, AddressingVersion.None),
                        WriteEncoding = new  System.Text.UTF8Encoding(false)
                    },
                    new HttpTransportBindingElement //or HttpsTransportBindingElement for https:
                    {
                        AuthenticationScheme = AuthenticationSchemes.Digest,
                        MaxBufferSize = int.MaxValue,
                        MaxReceivedMessageSize = int.MaxValue
                    }}
            };

            EndpointAddress address =
                   new EndpointAddress(serviceAddress);


            ClientBase<TChannel> serviceClient = creator(binding, address);

            System.Net.ServicePoint servicePoint =
                System.Net.ServicePointManager.FindServicePoint(serviceClient.Endpoint.Address.Uri);

            servicePoint.Expect100Continue = false;

            serviceClient.ClientCredentials.Windows.ClientCredential = new NetworkCredential(username, password, "ONVIF_Manager");
            serviceClient.ClientCredentials.UserName.UserName = username;
            serviceClient.ClientCredentials.UserName.Password = password;
            serviceClient.ClientCredentials.Windows.AllowedImpersonationLevel = System.Security.Principal.TokenImpersonationLevel.Impersonation;

            return serviceClient;
        }
    }
}
