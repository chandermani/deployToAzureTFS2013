// -----------------------------------------------------------------------------------------
// Copyright (c) 2011 Eastbanc Technologies LLC.
//
// Permission is hereby granted, free of charge, to any person obtaining  a copy of this 
// software and associated documentation files (the "Software"),  to deal in the Software 
// without restriction, including without limitation the rights to use, copy, modify, merge,
// publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons 
// to whom the Software is furnished to do so, subject to the following conditions:
// The above copyright notice and this permission notice shall be included in all copies or 
// substantial portions of the Software.
// -----------------------------------------------------------------------------------------
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, 
// INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR 
// PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE 
// FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR 
// OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER 
// DEALINGS IN THE SOFTWARE. 
// -----------------------------------------------------------------------------------------

using System;
using System.Security.Cryptography.X509Certificates;
using System.Activities;
using Microsoft.TeamFoundation.Build.Client;

namespace EastBancTech.DeployToAzure.Activities.Common
{
    /// <summary>
    /// Load Azure API certificate
    /// </summary>
    [BuildActivity(HostEnvironmentOption.Agent)]
    public sealed class LoadCertificate : CodeActivity<X509Certificate2>
    {
        /// <summary>
        /// Certificate Store Location.
        /// For example: LocalMachine or CurrentUser
        /// </summary>
        public InArgument<StoreLocation> StoreLocation { get; set; }

        /// <summary>
        /// Certificate Store Name.
        /// For example: Root
        /// </summary>
        public InArgument<StoreName> StoreName { get; set; }

        /// <summary>
        /// Certificate Thimbprint
        /// </summary>
        public InArgument<string> Thumbprint { get; set; }

        protected override X509Certificate2 Execute(CodeActivityContext context)
        {
            System.Diagnostics.Debug.WriteLine("Store Location:{0}", context.GetValue(StoreLocation).ToString());
            //System.Diagnostics.Debug.WriteLine("Store Name:{0}", context.GetValue(StoreName).ToString());
            //System.Diagnostics.Debug.WriteLine("Store Thumbprint:{0}", context.GetValue(Thumbprint).ToString());

            var storeLocation = context.GetValue(StoreLocation);           
            var storeName = context.GetValue(StoreName);

            var thumbprint = context.GetValue(Thumbprint);

            var store = new X509Store(storeName, storeLocation);
            try
            {
                // create and open store for read-only access
                store.Open(OpenFlags.ReadOnly);

                // search store
                X509Certificate2Collection col = store.Certificates.Find(
                  X509FindType.FindByThumbprint , thumbprint , true);

                // return first certificate found
                if (col.Count> 0)
                    return col[0];
                else
                {
                    throw new Exception("Certificate hasn't been found.");
                }
            }
            // close the store
            finally { store.Close(); }

        }
    }
}
