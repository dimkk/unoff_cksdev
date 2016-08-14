// Guids.cs
// MUST match guids.h
using System;

namespace CKS.Dev.VisualStudio.SharePoint.Environment
{
    static class GuidList
    {
        public const string guidCKSDEV_ComponentPickerPageSharePoint = "BB8F11D5-5618-4764-86C9-6A3D06AA7E3D";

        public const string guidCKSDEV_Extensions_PackagePkgString = "b78f40c4-85f8-44d1-a71c-49c16d5b8f17";
        public const string guidCKSDEV_Extensions_PackageCmdSetString = "d0ec82a9-563b-46a9-8ac7-4a561f69b0cb";

        public static readonly Guid guidCKSDEV_Extensions_PackageCmdSet = new Guid(guidCKSDEV_Extensions_PackageCmdSetString);

        public const string guidCKSDEVPkgString = "F120F40F-F543-4d15-8BBB-4F4B174C6A23";
        public const string guidCKSDEVCmdSetString = "F8FC4244-3BA1-4bf5-A65A-23B2F3D3CA9F";
        public const string guidToolWindowPersistanceString = "dc1e7d75-ea65-44bf-83cc-1afd0d5fb261";
        public const string guidCKSDEVCSProjectFactoryString = "593B0543-81F6-4436-BA1E-4747859CAAE2";
        public const string guidCKSDEVVBProjectFactoryString = "EC05E597-79D4-47f3-ADA0-324C4F7C7484";
        public const string guidUIContextNoSolutionString = "adfc4e64-0397-11d1-9f4e-00a0c911004f";
        public const string guidUIContextSolutionExistsString = "f1536ef8-92ec-443c-9ed7-fdadf150da82";

        public static readonly Guid guidCKSDEVCmdSet = new Guid(guidCKSDEVCmdSetString);
    };
}