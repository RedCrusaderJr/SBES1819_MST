﻿using Common.Contracts;
using Common.Manager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Security.Principal;
using System.ServiceModel;
using System.ServiceModel.Security;
using System.Text; 
using System.Threading.Tasks;
using Common;
using System.Configuration;

namespace MST
{
    public class IPS_Client : ChannelFactory<IIPS_Service>, IIPS_Service, IDisposable
    {
        private IIPS_Service _proxy;

        /// <summary>
        /// Initialize the WCF communication channel to IPS host.
        /// </summary>
        /// <param name="binding"></param>
        /// <param name="address"></param>
        public IPS_Client(NetTcpBinding binding, EndpointAddress address)
            : base(binding, address)
        {
            string mstCertName = ConfigurationManager.AppSettings["mstCertName"];
            
            this.Credentials.ServiceCertificate.Authentication.CertificateValidationMode = X509CertificateValidationMode.ChainTrust;
            this.Credentials.ServiceCertificate.Authentication.RevocationMode = X509RevocationMode.NoCheck;

            this.Credentials.ClientCertificate.Certificate = CertManager.GetCertificateFromStorage(StoreName.My, StoreLocation.LocalMachine, mstCertName);

            _proxy = this.CreateChannel();
        }

        /// <summary>
        /// Encapsulates the call of MalwareDetection() method on IPS host.
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="processID"></param>
        /// <param name="processName"></param>
        /// <param name="timeOfDetection"></param>
        public void MalwareDetection(string userID, string processID, string processName, DateTime timeOfDetection)
        {
            try
            {
                _proxy.MalwareDetection(userID, processID, processName, timeOfDetection);
            }
            catch(Exception e)
            {
                Console.WriteLine($"Error on 'MalwareDetection()' with message: {e.Message}");
                Console.WriteLine($"[STACK_TRACE] {e.StackTrace}");
            }
        }

        public void Dispose()
        {
            if (_proxy != null)
            {
                _proxy = null;
            }

            this.Close();
        }
    }
}
