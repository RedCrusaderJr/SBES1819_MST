﻿using Common.Contracts;
using Manager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Security.Principal;
using System.ServiceModel;
using System.ServiceModel.Security;
using System.Text;
using System.Threading.Tasks;

namespace IPS
{
    public class MST_Client : ChannelFactory<IMST_Service>, IMST_Service, IDisposable
    {
        private IMST_Service _proxy;

        public MST_Client(NetTcpBinding binding, EndpointAddress address) 
            : base(binding, address)
        {
            //string subjectName = Formatter.ParseName(WindowsIdentity.GetCurrent().Name);
            string subjectName = "MSTCert";

            this.Credentials.ServiceCertificate.Authentication.CertificateValidationMode = X509CertificateValidationMode.ChainTrust;
            //this.Credentials.ServiceCertificate.Authentication.CustomCertificateValidator = new ClientCertValidator();
            this.Credentials.ServiceCertificate.Authentication.RevocationMode = X509RevocationMode.NoCheck;

            this.Credentials.ClientCertificate.Certificate = CertManager.GetCertificateFromStorage(StoreName.My, StoreLocation.LocalMachine, subjectName);

            _proxy = this.CreateChannel();
        }

        public void ProcessShutdown(string userID, string processID)
        {
            try
            {
                _proxy.ProcessShutdown(userID, processID);
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error on 'ProcessShutdown()' with message: {e.Message}");
                Console.WriteLine($"[STACK_TRACE] {e.StackTrace}");
            }
        }

        public void Dispose()
        {
            if(_proxy != null)
            {
                _proxy = null;
            }

            this.Close();
        }
    }
}
