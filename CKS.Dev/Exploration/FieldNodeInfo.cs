using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.SharePoint.Explorer.Extensions;

namespace CKS.Dev.VisualStudio.SharePoint.Exploration
{
    /// <summary>
    /// Field Node Info container.
    /// </summary>
    [Serializable]
    public class FieldNodeInfo : IFieldNodeInfo
    {
        /// <summary>
        /// Gets the name of the content type that this field is associated with, if any.
        /// </summary>
        /// <value></value>
        /// <returns>The name of the content type that this field is associated with</returns>
        public string ContentTypeName { get; set; }

        /// <summary>
        /// Gets the ID of the field.
        /// </summary>
        /// <value></value>
        /// <returns>The ID of the field.</returns>
        public Guid Id { get; set; }

        /// <summary>
        /// Gets a value that indicates whether the field is hidden.
        /// </summary>
        /// <value></value>
        /// <returns>true if the field is hidden; otherwise, false.</returns>
        public bool IsHidden { get; set; }

        /// <summary>
        /// Gets the ID of the list that this field is associated with, if any.
        /// </summary>
        /// <value></value>
        /// <returns>The ID of the list that this field is associated with.</returns>
        public Guid ListId { get; set; }

        /// <summary>
        /// Gets the name of the field.
        /// </summary>
        /// <value></value>
        /// <returns>The name of the field.</returns>
        public string Title { get; set; }
    }
}
