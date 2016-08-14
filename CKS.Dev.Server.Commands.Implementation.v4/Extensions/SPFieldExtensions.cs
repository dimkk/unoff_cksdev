﻿using System;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Publishing.WebControls;
using Microsoft.SharePoint.WebControls;

namespace CKS.Dev.VisualStudio.SharePoint.Commands.Extensions
{
    /// <summary>
    /// Extension methods for the SPField type.
    /// </summary>
    public static class SPFieldExtensions
    {
        // from Microsoft.SharePoint.Publishing.WebServices.SharepointPublishingToolboxService
        internal static Type GetFieldRenderingControlType(this SPField field)
        {
            // Exception for the PublishingStartDate and the PublishingEndDate fields which require Context to retrieve the FieldRenderingControl
            if (field.InternalName.Equals("PublishingStartDate") || field.InternalName.Equals("PublishingExpirationDate"))
            {
                return typeof(PublishingScheduleFieldControl);
            }

            if (field.FieldRenderingControl == null)
            {
                return typeof(FormField);
            }

            BaseFieldControl fieldRenderingControl = field.FieldRenderingControl;
            if (!fieldRenderingControl.GetType().Equals(typeof(RichTextField)))
            {
                return fieldRenderingControl.GetType();
            }

            return typeof(NoteField);
        }
    }
}
