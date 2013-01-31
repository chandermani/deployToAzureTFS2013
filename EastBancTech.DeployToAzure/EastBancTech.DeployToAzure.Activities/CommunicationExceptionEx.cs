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
using System.ServiceModel;
using Microsoft.Samples.WindowsAzure.ServiceManagement;

namespace EastBancTech.DeployToAzure.Activities
{
    class CommunicationExceptionEx:ApplicationException 
    {
        public CommunicationExceptionEx(CommunicationException inner):base(GetDetailedMessage(inner),inner)
        {
        }

        private static string GetDetailedMessage(CommunicationException inner)
        {
            ServiceManagementError error = null;
            ServiceManagementHelper.TryGetExceptionDetails(inner, out error);

            if (error == null)
            {
                return inner.Message;
            }
            else
            {
                string errorDetails = string.Format(
                    CultureInfo.InvariantCulture,
                    "HTTP Status Code: {0} - HTTP Error Message: {1}",
                    error.Code,
                    error.Message);
                return errorDetails;
            }
        }
    }
}
