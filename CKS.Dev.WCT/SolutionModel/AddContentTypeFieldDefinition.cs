using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.SharePoint.Features;
using CKS.Dev.WCT.Extensions;

namespace CKS.Dev.WCT.SolutionModel
{
    public partial class AddContentTypeFieldDefinition
    {
        public void Setup(IAddContentTypeFieldUpgradeAction action)
        {
            action.ContentTypeId = this.ContentTypeId;
            if (!String.IsNullOrEmpty(this.FieldId))
            {
                action.FieldId = new Guid(this.FieldId);
            }
            if (this.PushDownSpecified)
            {
                action.IsPushedDown = TRUEFALSEExtensions.IsTrue(this.PushDown);
            }
        }
    }
}
