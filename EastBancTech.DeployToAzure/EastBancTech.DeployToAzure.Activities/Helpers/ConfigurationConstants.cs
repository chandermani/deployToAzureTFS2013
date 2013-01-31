﻿// ----------------------------------------------------------------------------------
// Microsoft Developer & Platform Evangelism
// 
// Copyright (c) Microsoft Corporation. All rights reserved.
// 
// THIS CODE AND INFORMATION ARE PROVIDED "AS IS" WITHOUT WARRANTY OF ANY KIND, 
// EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE IMPLIED WARRANTIES 
// OF MERCHANTABILITY AND/OR FITNESS FOR A PARTICULAR PURPOSE.
// ----------------------------------------------------------------------------------
// The example companies, organizations, products, domain names,
// e-mail addresses, logos, people, places, and events depicted
// herein are fictitious.  No association with any real company,
// organization, product, domain name, email address, logo, person,
// places, or events is intended or should be inferred.
// ----------------------------------------------------------------------------------

using System;
using System.ServiceModel;
using System.ServiceModel.Channels;

namespace Microsoft.Samples.AzureManagementTools.PowerShell.Helpers
{
    public static class ConfigurationConstants
    {
        public const string ServiceEndpoint = "https://management.core.windows.net";
        public const string BlobEndpointTemplate = "http://{0}.blob.core.windows.net/";

        public static Binding WebHttpBinding()
        {
            var binding = new WebHttpBinding(WebHttpSecurityMode.Transport);
            binding.Security.Transport.ClientCredentialType = HttpClientCredentialType.Certificate;
            binding.MaxReceivedMessageSize = Int32.MaxValue;
            binding.MaxBufferSize = Int32.MaxValue;
            binding.ReaderQuotas.MaxStringContentLength = Int32.MaxValue;

            return binding;
        }
    }
}
