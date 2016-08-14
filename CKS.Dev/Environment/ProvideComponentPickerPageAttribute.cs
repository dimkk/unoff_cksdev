using System;
using System.Globalization;
using Microsoft.VisualStudio.Shell;

namespace CKS.Dev.VisualStudio.SharePoint.Environment
{
    /// <summary>
    /// TODO: Wouter can you complete the comments for this class
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = true)]
    public class ProvideComponentPickerPageAttribute : RegistrationAttribute
    {
        Guid _pageGuid;
        Guid _packageGuid;
        string _componentName;
        int _sortOrder = 0x35;

        /// <summary>
        /// Initializes a new instance of the <see cref="ProvideComponentPickerPageAttribute"/> class.
        /// </summary>
        /// <param name="pageType">Type of the page.</param>
        /// <param name="packageType">Type of the package.</param>
        /// <param name="componentName">Name of the component.</param>
        public ProvideComponentPickerPageAttribute(
               object pageType,
               object packageType,
               string componentName)
        {
            _pageGuid = GetObjectGuid(pageType);
            _packageGuid = GetObjectGuid(packageType);
            _componentName = componentName;
        }

        /// <summary>
        /// Gets the object GUID.
        /// </summary>
        /// <param name="objType">Type of the obj.</param>
        /// <returns></returns>
        private Guid GetObjectGuid(object objType)
        {
            Guid type;
            if (objType is string)
            {
                type = new Guid((string)objType);
            }
            else if (objType is Type)
            {
                type = ((Type)objType).GUID;
            }
            else if (objType is Guid)
            {
                type = (Guid)objType;
            }
            else
            {
                throw new ArgumentException("Bad parameter", "objType");
            }
            return type;
        }

        /// <summary>
        /// Gets the page reg key string.
        /// </summary>
        /// <value>The page reg key string.</value>
        private string PageRegKeyString
        {
            get
            {
                return string.Format(CultureInfo.InvariantCulture, "ComponentPickerPages\\{0}", _componentName);
            }
        }

        /// <summary>
        /// Register.
        /// </summary>
        /// <param name="context">The registration context.</param>
        public override void Register(RegistrationContext context)
        {
            using (Key pageKey = context.CreateKey(PageRegKeyString))
            {
                pageKey.SetValue(string.Empty, string.Empty);
                pageKey.SetValue("Package", _packageGuid.ToString("B"));
                pageKey.SetValue("Page", _pageGuid.ToString("B"));
                pageKey.SetValue("Sort", _sortOrder);
                pageKey.SetValue("ComponentType", ".NET Assembly");
                pageKey.SetValue("AddToMru", 1);
            }
        }

        /// <summary>
        /// Unregister.
        /// </summary>
        /// <param name="context">The registration context.</param>
        public override void Unregister(RegistrationContext context)
        {
            context.RemoveKey(PageRegKeyString);
        }
    }
}
