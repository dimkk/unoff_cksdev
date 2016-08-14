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
    internal static class Constants
    {
        public const string SourcePackageFolderPath = "pkg";
        public const string SourceSolutionManifestFile = "manifest.xml";
        public const string SourceSolutionMappingFile = "solution.xml";
        public const string SourceSolutionTemplatesPath = "Templates";
        public const string SourceSolutionRootFilesPath = "RootFiles";
        public const string SourceSolutionSchemaFile = "schema.xml";

        public const string PackageTemplateFileRelativeLocation = @"Package\Package.Template.xml";
        public const string TargetElementManifestName = "Elements.xml";
        public const string TargetAssemblyFullnameToken = "$SharePoint.Project.AssemblyFullName$";

        public const string SPDocumentNamespaceUri = "http://schemas.microsoft.com/sharepoint/";
        public const string VSeWSSSPProjectFlavorGuidCS = "{593B0543-81F6-4436-BA1E-4747859CAAE2}";
        public const string VSeWSSSPProjectFlavorGuidVB = "{EC05E597-79D4-47f3-ADA0-324C4F7C7484}";
        public const string VS2010SPProjectFlavorGuid = "{BB1F664B-9266-4fd6-B973-E1E44974B511}";

        public const string SPSiteSchemaFileName = "onet.xml";
        public const string SPTypeNameGenericElement = "Microsoft.VisualStudio.SharePoint.GenericElement";
        public const string SPTypeNameContentType = "Microsoft.VisualStudio.SharePoint.ContentType";
        public const string SPTypeNameEventHandler = "Microsoft.VisualStudio.SharePoint.EventHandler";
        public const string SPTypeNameListDefinition = "Microsoft.VisualStudio.SharePoint.ListDefinition";
        public const string SPTypeNameListInstance = "Microsoft.VisualStudio.SharePoint.ListInstance";
        public const string SPTypeNameModule = "Microsoft.VisualStudio.SharePoint.Module";
        public const string SPTypeNameWebPart = "Microsoft.VisualStudio.SharePoint.WebPart";
        public const string SPTypeNameVisualWebPart = "Microsoft.VisualStudio.SharePoint.VisualWebPart";
        public const string SPTypeNameSiteDefinition = "Microsoft.VisualStudio.SharePoint.SiteDefinition";
        public const string SPTypeNameWorkflow = "Microsoft.VisualStudio.SharePoint.Workflow";
        public const string SPTypeNameWorkflowAssociation = "Microsoft.VisualStudio.SharePoint.WorkflowAssociation";
        public const string SPTypeNameBusinessDataConnectivity = "Microsoft.VisualStudio.SharePoint.BusinessDataConnectivity";

        
 



        public const string xmlNodeTitleWhiteSpace = "#whitespace";
        public const string xmlNodeTitleComment = "#comment";

        public static string[] SiteSchemaListInstanceExcludeAttributes = new string[] { "VersioningEnabled", "EnableMinorVersions", "EnableContentTypes", "ForceCheckout" };
        public static readonly string[] MappedManifestElements = new string[] { "FeatureManifests", "TemplateFiles", "RootFiles", "Assemblies", "SiteDefinitionManifests" };

        public static string SOLUTIONID_FILENAME = "solutionid.txt";
    }
}