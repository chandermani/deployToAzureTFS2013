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

using System.ServiceModel;
using System.Activities;
using EastBancTech.DeployToAzure.Activities.Helpers;
using EastBancTech.DeployToAzure.Activities.Model;
using Microsoft.Samples.WindowsAzure.ServiceManagement;
using Microsoft.TeamFoundation.Build.Client;

namespace EastBancTech.DeployToAzure.Activities.HostedServices
{
    /// <summary>
    /// Deletes the specified deployment. Note that the deployment should be in suspended state.
    /// </summary>
    [BuildActivity(HostEnvironmentOption.Agent)]
    public sealed class RemoveDeployment : OperationActivity 
    {
        public InArgument<string> HostedServiceName { get; set; }
        public InArgument<DeploymentSlot> Slot { get; set; }

        protected override string ExecuteOperation(CodeActivityContext context)
        {
            var hostedServiceName = context.GetValue<string>(HostedServiceName);
            var slot = context.GetValue(Slot).Description();

            using (new OperationContextScope((IContextChannel)this.channel))
            {
                try
                {
                    this.RetryCall(s => this.channel.DeleteDeploymentBySlot(s, hostedServiceName, slot));
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
