using System;
using System.Linq;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.Xml.Linq;
using System.IO;
using System.Reflection;
using CKS.Dev.VisualStudio.SharePoint.Deployment.DeploymentSteps;
using CKS.Dev.VisualStudio.SharePoint.Exploration;

namespace CKS.Dev.VisualStudio.SharePoint.Environment
{
    [ComVisible(true)]
    [Guid(GuidList.guidCKSDEV_ComponentPickerPageSharePoint)]
    [ClassInterface(ClassInterfaceType.AutoDual)]
    partial class SharePointReferenceView
        : ComponentPickerPage
    {
        const string Namespace = "urn:CKS.Dev.Schema.References";

        public string ReferencePath { get; set; }

        public SharePointReferenceView()
        {
            InitializeComponent();
            DoubleBuffered = true;
        }

        protected override void InitializeItems()
        {
            _referencesList.Items.Clear();
            _referencesList.Groups.Clear();
            string sharePointInstallFolder = DTEManager.ProjectService.SharePointInstallPath;
            XDocument document = XDocument.Load(ReferencePath);
            XNamespace ns = Namespace;
            foreach (XElement groupElement in document.Root.Elements(ns + "referenceGroup"))
            {
                string groupName = groupElement.Attribute("name").Value;
                ListViewGroup group = _referencesList.Groups.Add(groupName, groupName);
                foreach (XElement referenceElement in groupElement.Elements(ns + "reference"))
                {
                    if (referenceElement.Attributes("path").Count() > 0)
                    {
                        string relativePath = referenceElement.Attribute("path").Value;
                        string fullPath = Path.Combine(sharePointInstallFolder, relativePath);
                        AddAssemblyPath(group, fullPath);
                    }
                    else if (referenceElement.Attributes("assembly").Count() > 0)
                    {
                        string assemblyName = referenceElement.Attribute("assembly").Value;
                        string path = AssemblyCache.QueryAssemblyInfo(assemblyName);
                        AddAssemblyPath(group, path);
                    }
                }
            }
        }

        private void AddAssemblyPath(ListViewGroup group, string fullPath)
        {
            if (File.Exists(fullPath))
            {
                AssemblyName assemblyName = null;
                try
                {
                    assemblyName = AssemblyName.GetAssemblyName(fullPath);
                }
                catch (BadImageFormatException) { }
                catch (ArgumentException) { }
                catch (FileLoadException) { }
                if (assemblyName != null)
                {
                    ListViewItem item = new ListViewItem(assemblyName.Name);
                    item.SubItems.Add(assemblyName.Version.ToString());
                    item.SubItems.Add(fullPath);
                    item.Group = group;
                    _referencesList.Items.Add(item);
                }
            }
        }

        protected override ComponentPickerItem[] GetSelection()
        {
            List<ComponentPickerItem> items = new List<ComponentPickerItem>();
            foreach (ListViewItem item in _referencesList.SelectedItems)
            {
                items.Add(new ComponentPickerItem()
                {
                    Title = item.Text,
                    Path = item.SubItems[2].Text
                });
            }
            return items.ToArray();
        }

        protected override void ClearSelection()
        {
            List<ListViewItem> items =  _referencesList.SelectedItems.OfType<ListViewItem>().ToList();
            foreach (ListViewItem item in items)
            {
                item.Selected = false;
            }
        }

        protected override void SetSelectionMode(bool multiSelect)
        {
            _referencesList.MultiSelect = multiSelect;
        }

        void ReferencesList_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            ListViewItem item = _referencesList.GetItemAt(e.Location.X, e.Y);
            if (item != null)
            {
                NotifyItemDoubleClicked();
            }
        }

        void ReferencesList_SelectedIndexChanged(object sender, EventArgs e)
        {
            NotifySelectionChanged();
        }
    }
}