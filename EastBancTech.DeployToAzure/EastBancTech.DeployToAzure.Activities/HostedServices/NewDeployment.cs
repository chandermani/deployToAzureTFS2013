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
using System.ServiceModel;
using System.Activities;
using EastBancTech.DeployToAzure.Activities.Helpers;
using EastBancTech.DeployToAzure.Activities.Model;
using Microsoft.Samples.AzureManagementTools.PowerShell.Helpers;
using Microsoft.Samples.WindowsAzure.ServiceManagement;
using Microsoft.TeamFoundation.Build.Client;

namespace EastBancTech.DeployToAzure.Activities.HostedServices
{
    /// <summary>
    /// Create a new deployment. Note that there shouldn't be a deployment 
    /// of the same name or in the same slot when executing this command.
    /// </summary>
    [BuildActivity(HostEnvironmentOption.Agent)]
    public sealed class NewDeployment : OperationActivity 
    {
        
        public InArgument<string> HostedServiceName { get; set; }
        public InArgument<DeploymentSlot> Slot { get; set; }
        public InArgument<string> StorageServiceName { get; set; }
        public InArgument<string> DeploymentName { get; set; }
        public InArgument<string> Label { get; set; }
        public InArgument<string> Package { get; set; }
        public InArgument<string> Configuration { get; set; }

        protected override string ExecuteOperation(CodeActivityContext context)
        {
            var hostedServiceName = context.GetValue<string>(HostedServiceName);
            var slot = context.GetValue(Slot).Description();            
            var storageServiceName = context.GetValue<string>(StorageServiceName);
            var deploymentName = context.GetValue<string>(DeploymentName);
            var label = context.GetValue<string>(Label);
            var package = context.GetValue<string>(Package);
            var configuration = context.GetValue<string>(Configuration);

            if (string.IsNullOrEmpty(deploymentName))
            {
                deploymentName = Guid.NewGuid().ToString();
            }
            
            Uri packageUrl;
            if (package.StartsWith(Uri.UriSchemeHttp, StringComparison.OrdinalIgnoreCase) ||
                package.StartsWith(Uri.UriSchemeHttps, StringComparison.OrdinalIgnoreCase))
            {
                packageUrl = new Uri(package);
            }
            else
            {
                //upload package to blob
                var storageName = string.IsNullOrEmpty(storageServiceName) ? hostedServiceName : storageServiceName;
                packageUrl = this.RetryCall(s =>
                    AzureBlob.UploadPackageToBlob(
                    channel,
                    storageName,
                    s,
                    package));
            }

            //create new deployment package
            var deploymentInput = new CreateDeploymentInput
            {
                PackageUrl = packageUrl,
                Configuration = Utility.GetConfiguration(configuration),
                Label = ServiceManagementHelper.EncodeToBase64String(label),
                Name = deploymentName
            };

            using (new OperationContextScope((IContextChannel)channel))
            {
                try
                {
                    this.RetryCall(s => this.channel.CreateOrUpdateDeployment(
                        s,
                        hostedServiceName,
                        slot,
                        deploymentInput));
                }
                catch (CommunicationException ex)
                {
                    throw new CommunicationExceptionEx(ex);
                }

                return RetrieveOperationId();
            }
        }
    }
}
