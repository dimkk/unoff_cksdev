//--------------------------------------------------------------------------------
// This file is a "Sample" from the SharePoint Foundation 2010
// Samples
//
// Copyright (c) Microsoft Corporation. All rights reserved.
//
// This source code is intended only as a supplement to Microsoft
// Development Tools and/or on-line documentation.  See these other
// materials for detailed information regarding Microsoft code samples.
// 
// THIS CODE AND INFORMATION ARE PROVIDED AS IS WITHOUT WARRANTY OF ANY
// KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE
// IMPLIED WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A
// PARTICULAR PURPOSE.
//--------------------------------------------------------------------------------

namespace CKS.Dev.WCT.Common
{
    using System;
    using Microsoft.VisualStudio.SharePoint;

    public static class Logger
    {
        public static EnvDTE.DTE DesignTimeEnvironment { get; set; }

        private static System.IServiceProvider _serviceProvider;
        private static System.IServiceProvider ServiceProvider
        {
            get
            {
                if (Logger._serviceProvider == null)
                {
                    Microsoft.VisualStudio.OLE.Interop.IServiceProvider sp = null;
                    sp = Logger.DesignTimeEnvironment as Microsoft.VisualStudio.OLE.Interop.IServiceProvider;
                    if (sp != null)
                    {
                        Logger._serviceProvider = new Microsoft.VisualStudio.Shell.ServiceProvider(sp);
                    }
                }
                return Logger._serviceProvider;

            }
        }

        private static ISharePointProjectService _projectService;
        private static ISharePointProjectService ProjectService
        {
            get
            {
                if (_projectService == null)
                {
                    _projectService = Logger.ServiceProvider.GetService(typeof(ISharePointProjectService)) as ISharePointProjectService;
                }
                return _projectService;
            }
        }

        public static void LogError(string message)
        {
            Logger.ProjectService.Logger.WriteLine(message, LogCategory.Error);
        }

        public static void LogError(Exception ex)
        {
            Logger.LogError(ex.ToString());
        }

        public static void LogWarning(string message)
        {
            Logger.ProjectService.Logger.WriteLine(message, LogCategory.Warning);
        }

        public static void LogInformation(string message)
        {
            Logger.ProjectService.Logger.WriteLine(message, LogCategory.Message);
        }

        public static void LogStatus(string message)
        {
            Logger.ProjectService.Logger.WriteLine(message, LogCategory.Status);
        }

        public static void LogVerbose(string message)
        {
            Logger.ProjectService.Logger.WriteLine(message, LogCategory.Verbose);
        }
    }
}