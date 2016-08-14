﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CKS.Dev11.VisualStudio.SharePoint.Commands.Info;
using Microsoft.SharePoint;

namespace CKS.Dev11.VisualStudio.SharePoint.Commands.Common.ExtensionMethods
{
    internal static class SPFileCollectionExtensions
    {
        internal static List<FileNodeInfo> ToFileNodeInfo(this SPFileCollection files)
        {
            List<FileNodeInfo> nodeInfos = new List<FileNodeInfo>();

            foreach (SPFile file in files)
            {
                FileNodeInfo nodeInfo = new FileNodeInfo
                {
                    Id = file.Item.ID,
                    Name = file.Name,
                    UniqueId = file.Item.UniqueId,
                    FileType = file.Item[SPBuiltInFieldId.File_x0020_Type] as string,
                    ServerRelativeUrl = file.ServerRelativeUrl,
                    Title = file.Item.Title,
                    IsCheckedOut = file.Level == SPFileLevel.Checkout
                };
                nodeInfos.Add(nodeInfo);
            }

            return nodeInfos;
        }
    }
}
