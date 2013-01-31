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

using System.ServiceModel.Web;
using System.Activities;
using Microsoft.Samples.WindowsAzure.ServiceManagement;

namespace EastBancTech.DeployToAzure.Activities
{
    /// <summary>
    /// Base class for activities executing operation
    /// </summary>
    public abstract class OperationActivity : BaseActivity
    {
        /// <summary>
        /// Started operation id
        /// </summary>
        public OutArgument<string> OperationId { get; set; }
       
        protected override void ExecuteActivity(CodeActivityContext context)
        {
            string operationId = ExecuteOperation(context);
            context.SetValue(OperationId,operationId);
        }

        protected static string RetrieveOperationId()
        {
            var operationId = string.Empty;

            if (WebOperationContext.Current.IncomingResponse != null)
            {
                operationId = WebOperationContext.Current.IncomingResponse.Headers[Constants.OperationTrackingIdHeader];
            }

            return operationId;
        }
        protected abstract string ExecuteOperation(CodeActivityContext context);
    }
}
