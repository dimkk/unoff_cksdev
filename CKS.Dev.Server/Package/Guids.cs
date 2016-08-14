// Guids.cs
// MUST match guids.h
using System;

namespace CKS.Dev.VisualStudio.SharePoint.Environment
{
    static class GuidList
    {
        public const string guidCKS_Dev_ServerPkgString = "a5074c8b-7745-436b-a64e-a6f9ae26ea9d";
        public const string guidCKS_Dev_ServerCmdSetString = "53ba6480-c933-421f-90e1-1e472cf470b7";

        public static readonly Guid guidCKS_Dev_ServerACmdSet = new Guid(guidCKS_Dev_ServerCmdSetString);
    };
}