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
using System.IO;
using System.Linq;
using System.Activities;
using Microsoft.TeamFoundation.Build.Client;

namespace EastBancTech.DeployToAzure.Activities.Build
{
    /// <summary>
    /// Searches for deployment package and configuration files in specified folder
    /// </summary>
    [BuildActivity(HostEnvironmentOption.Agent)]
    public sealed class FindPackageAndConfigurationFiles : CodeActivity
    {
        /// <summary>
        /// Path to folder where package and configuration files are located
        /// </summary>
        public InArgument<string> Path { get; set; }

        /// <summary>
        /// Path to package file
        /// </summary>
        public OutArgument<string> Package { get; set; }
        /// <summary>
        /// Path to configuration file
        /// </summary>
        public OutArgument<string> Configuration { get; set; }

        
        protected override void Execute(CodeActivityContext context)
        {            
            string path = context.GetValue(this.Path);
                        
            var buildDir= new DirectoryInfo(path);

            var package = buildDir.GetFiles().FirstOrDefault<FileInfo>(m => m.Extension == ".cspkg");
            var configuration = buildDir.GetFiles().FirstOrDefault<FileInfo>(m => m.Extension == ".cscfg");

            if (package == null)
                throw new Exception(String.Format("Cloud Service package file hasn't been found in the folder: {0}", path));

            if (configuration == null)
                throw new Exception(String.Format("Cloud Service configuration file hasn't been found in the folder: {0}", path));
            
            context.SetValue(Package,package.FullName);
            context.SetValue(Configuration,configuration.FullName);
        }
    }
}
