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
using System.Globalization;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.ServiceModel.Security;
using System.Activities;
using Microsoft.Samples.AzureManagementTools.PowerShell.Helpers;
using Microsoft.Samples.WindowsAzure.ServiceManagement;

namespace EastBancTech.DeployToAzure.Activities
{
    /// <summary>
    /// Base activity class
    /// </summary>
    public abstract class BaseActivity : CodeActivity
    {
        /// <summary>
        /// Communication channel
        /// </summary>
        public InOutArgument<IServiceManagement> Channel { get; set; }

        /// <summary>
        /// Azure API certificate 
        /// </summary>
        public InArgument<X509Certificate2> Certificate { get; set; }

        /// <summary>
        /// Azure Subscription Id
        /// </summary>
        public InArgument<string> SubscriptionId { get; set; }


        protected override void Execute(CodeActivityContext context)
        {
           certificate = context.GetValue(this.Certificate);
           subscriptionId = context.GetValue(this.SubscriptionId);           
           if (channel == null)
                channel = CreateChannel();
          ExecuteActivity(context);
          context.SetValue(Channel,channel);          
        }

        protected abstract void ExecuteActivity(CodeActivityContext context);

        protected IServiceManagement channel;
        protected X509Certificate2 certificate;
        protected string subscriptionId;

        private IServiceManagement CreateChannel()
        {
            return ServiceManagementHelper.CreateServiceManagementChannel(ConfigurationConstants.WebHttpBinding(), new Uri(ConfigurationConstants.ServiceEndpoint), certificate);

        }

        public void RetryCall(Action<string> call)
        {
            this.RetryCall(this.subscriptionId, call);
        }

        public void RetryCall(string subscriptionId, Action<string> call)
        {
            try
            {
                try
                {
                    call(subscriptionId);
                }
                catch (MessageSecurityException ex)
                {
                    var webException = ex.InnerException as WebException;

                    if (webException == null)
                    {
                        throw;
                    }

                    var webResponse = webException.Response as HttpWebResponse;

                    if (webResponse != null && webResponse.StatusCode == HttpStatusCode.Forbidden)
                    {
                        this.channel = this.CreateChannel();
                        if (subscriptionId.Equals(subscriptionId.ToUpper((CultureInfo.InvariantCulture))))
                        {
                            call(subscriptionId.ToLower(CultureInfo.InvariantCulture));
                        }
                        else
                        {
                            call(subscriptionId.ToUpper(CultureInfo.InvariantCulture));
                        }
                    }
                    else
                    {
                        throw;
                    }
                }
            }
            catch (MessageSecurityException ex)
            {
                var webException = ex.InnerException as WebException;

                if (webException == null)
                {
                    throw;
                }

                var webResponse = webException.Response as HttpWebResponse;

                if (webResponse != null && webResponse.StatusCode == HttpStatusCode.Forbidden)
                {
                    this.channel = this.CreateChannel();
                    if (subscriptionId.Equals(subscriptionId.ToUpper((CultureInfo.InvariantCulture))))
                    {
                        call(subscriptionId.ToLower(CultureInfo.InvariantCulture));
                    }
                    else
                    {
                        call(subscriptionId.ToUpper(CultureInfo.InvariantCulture));
                    }
                }
                else
                {
                    throw;
                }
            }
        }

        public TResult RetryCall<TResult>(Func<string, TResult> call)
        {
            return this.RetryCall(this.subscriptionId, call);
        }

        public TResult RetryCall<TResult>(string subscriptionId, Func<string, TResult> call)
        {
            try
            {
                try
                {
                    return call(subscriptionId);
                }
                catch (MessageSecurityException ex)
                {
                    var webException = ex.InnerException as WebException;

                    if (webException == null)
                    {
                        throw;
                    }

                    var webResponse = webException.Response as HttpWebResponse;

                    if (webResponse != null && webResponse.StatusCode == HttpStatusCode.Forbidden)
                    {
                        if (subscriptionId.Equals(subscriptionId.ToUpper((CultureInfo.InvariantCulture))))
                        {
                            return call(subscriptionId.ToLower(CultureInfo.InvariantCulture));
                        }
                        else
                        {
                            return call(subscriptionId.ToUpper(CultureInfo.InvariantCulture));
                        }
                    }
                    else
                    {
                        throw;
                    }
                }
            }
            catch (MessageSecurityException ex)
            {
                var webException = ex.InnerException as WebException;

                if (webException == null)
                {
                    throw;
                }

                var webResponse = webException.Response as HttpWebResponse;

                if (webResponse != null && webResponse.StatusCode == HttpStatusCode.Forbidden)
                {
                    if (subscriptionId.Equals(subscriptionId.ToUpper((CultureInfo.InvariantCulture))))
                    {
                        return call(subscriptionId.ToLower(CultureInfo.InvariantCulture));
                    }
                    else
                    {
                        return call(subscriptionId.ToUpper(CultureInfo.InvariantCulture));
                    }
                }
                else
                {
                    throw;
                }
            }
        }


    }
}
