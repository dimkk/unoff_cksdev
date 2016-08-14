using EnvDTE;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.OLE.Interop;
using Microsoft.VisualStudio.SharePoint;
using Microsoft.VisualStudio.SharePoint.ProjectExtensions.Wizards.WssSchemaClasses;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using Microsoft.VisualStudio.TemplateWizard;
using Microsoft.VisualStudio.VSHelp;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Principal;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;
using System.Globalization;
using System.Text;
using Eval = Microsoft.Build.Evaluation;

namespace CKS.Dev.VisualStudio.SharePoint.Content.Wizards
{
    /// <summary>
    /// Helpers for use with Item and Project wizards.
    /// </summary>
    public static class WizardHelpers
    {
        /// <returns>The default site Uri</returns>
        public static Uri GetDefaultProjectUrl()
        {
            ProjectManager projectManager = ProjectManager.Create(
                DTEManager.ActiveProject);
            return projectManager.Project.SiteUrl;
        }

        /// <summary>
        /// Check that the Uri is not null
        /// </summary>
        /// <param name="projectUrl">The project Uri</param>
        public static void CheckMissingSiteUrl(Uri projectUrl)
        {
            if (projectUrl == null)
            {
                //RtlAwareMessageBox.ShowError(null, WizardResources.SiteUrlMissing, WizardResources.ValidationErrorCaption);
                throw new WizardCancelledException("WizardResources.SiteUrlMissing");
            }
        }

        /// <summary>
        /// Makes the name compliant.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns></returns>
        public static string MakeNameCompliant(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                return string.Empty;
            }
            if (char.IsDigit(name.ToCharArray()[0]))
            {
                name = "_" + name;
            }
            StringBuilder builder = new StringBuilder(name.Length);
            string str = name;
            for (int i = 0; i < str.Length; i++)
            {
                char currentChar = str.ToCharArray()[i];
                if (IsValidCharForName(currentChar))
                {
                    builder.Append(currentChar);
                }
                else
                {
                    builder.Append('_');
                }
            }
            name = builder.ToString();
            return name;
        }

        /// <summary>
        /// Determines whether [is valid char for name] [the specified current char].
        /// </summary>
        /// <param name="currentChar">The current char.</param>
        /// <returns>
        /// 	<c>true</c> if [is valid char for name] [the specified current char]; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsValidCharForName(char currentChar)
        {
            UnicodeCategory unicodeCategory = char.GetUnicodeCategory(currentChar);
            if (((((unicodeCategory != UnicodeCategory.UppercaseLetter) && (unicodeCategory != UnicodeCategory.LowercaseLetter))
                && ((unicodeCategory != UnicodeCategory.OtherLetter) && (unicodeCategory != UnicodeCategory.ConnectorPunctuation)))
                && (((unicodeCategory != UnicodeCategory.ModifierLetter) && (unicodeCategory != UnicodeCategory.NonSpacingMark)) && ((unicodeCategory != UnicodeCategory.SpacingCombiningMark) && (unicodeCategory != UnicodeCategory.TitlecaseLetter)))) && (((unicodeCategory != UnicodeCategory.Format) && (unicodeCategory != UnicodeCategory.LetterNumber)) && ((unicodeCategory != UnicodeCategory.DecimalDigitNumber) && (currentChar != '.'))))
            {
                return (currentChar == '_');
            }
            return true;

        }
    }
}
