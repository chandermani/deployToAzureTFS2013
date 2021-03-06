﻿// -----------------------------------------------------------------------------------------
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
using Microsoft.Samples.WindowsAzure.ServiceManagement;
using Microsoft.TeamFoundation.Build.Client;

namespace EastBancTech.DeployToAzure.Activities.Common
{
    /// <summary>
    /// Retrive status for specified operation.
    /// </summary>
    [BuildActivity(HostEnvironmentOption.Agent)]
    public sealed class GetOperationStatus :BaseActivity
    {
        
        public InArgument<string> OperationId { get; set; }
        public OutArgument<Operation> Operation { get; set; }
        

        protected override void ExecuteActivity(CodeActivityContext context)
        {
            string operationId = context.GetValue(this.OperationId);
            Operation operation = null;

            try
            {
                operation = this.RetryCall(s => this.channel.GetOperationStatus(s, operationId));
            }
            catch (CommunicationException ex)
            {
                throw new CommunicationExceptionEx(ex);
            }

            context.SetValue(Operation, operation);
        }
    }
}
