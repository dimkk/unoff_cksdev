using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CKS.Dev.VisualStudio.SharePoint.Environment
{
    static class VSPackageGuids
    {
        public const string UIContext_SharePointProject = "7E396C85-D374-4531-95B9-43E5E1A1CF3C";
        public const string PackageText = "23c6cc48-1264-4469-94c9-73a6b68584b1";
        public const string CommandSetText = "13b396e9-5f1e-447c-836c-c13393219d3b";
        public const string SPMetalGeneratorText = "6D15FC13-E27F-4BB1-88E3-B0B091675A0A";
        public const string SandboxedVisualWebPartGeneratorText = "99DA1F10-2DA3-4A62-9C9F-05BD348F3D57";
        public const string SharePointReferencesPageText = "BB8F11D5-5618-4764-86C9-6A3D06AA7E3D";
        public static readonly Guid Package = new Guid(PackageText);
        public static readonly Guid CommandSet = new Guid(CommandSetText);
        public static readonly Guid SPMetalGenerator = new Guid(SPMetalGeneratorText);
        public static readonly Guid SandboxedVisualWebPartGenerator = new Guid(SandboxedVisualWebPartGeneratorText);
        public static readonly Guid SharePointReferencesPage = new Guid(SharePointReferencesPageText);
        public const int QuickDeployMenu = 0x101010;
        public const int ProjectNodeGroup = 0x101;
        public const int QuickDeployGroup = 0x201;
        public const int QuickDeployButton = 0x2010;
    }
}
