using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.IO;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Linq;
using CKS.Dev.VisualStudio.SharePoint.Commands;
using CKS.Dev.VisualStudio.SharePoint.Commands.Info;
using CKS.Dev.VisualStudio.SharePoint.Exploration;
using Microsoft.VisualStudio.SharePoint.Explorer;
using Microsoft.VisualStudio.SharePoint.Explorer.Extensions;

namespace CKS.Dev.VisualStudio.SharePoint.Exploration
{
    /// <summary>
    /// Feature element node extension.
    /// </summary>
    [Export(typeof(IExplorerNodeTypeProvider))]
    [ExplorerNodeType(ExplorerNodeIds.FeatureElementNode)]
    public class FeatureElementNodeTypeProvider : IExplorerNodeTypeProvider
    {
        /// <summary>
        /// Creates the feature element nodes.
        /// </summary>
        /// <param name="parentNode">The parent node.</param>
        internal static void CreateFeatureElementNodes(IExplorerNode parentNode)
        {
            IFeatureNodeInfo info = parentNode.ParentNode.Annotations.GetValue<IFeatureNodeInfo>();
            FeatureInfo featureDetails = new FeatureInfo()
            {
                FeatureID = info.Id
            };
            FeatureElementInfo[] elements =
                parentNode.Context.SharePointConnection.ExecuteCommand<FeatureInfo, FeatureElementInfo[]>(FeatureSharePointCommandIds.GetFeatureElements, featureDetails);
            foreach (FeatureElementInfo element in elements)
            {
                IExplorerNode elementNode = CreateNode(parentNode, element);
                elementNode.DoubleClick += ElementNode_DoubleClick;
            }
        }

        /// <summary>
        /// Handles the DoubleClick event of the ElementNode control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="Microsoft.VisualStudio.SharePoint.Explorer.ExplorerNodeEventArgs"/> instance containing the event data.</param>
        static void ElementNode_DoubleClick(object sender, ExplorerNodeEventArgs e)
        {
            FeatureElementInfo elementInfo = e.Node.Annotations.GetValue<FeatureElementInfo>();

            XDocument document = XDocument.Parse(e.Node.Context.SharePointConnection.ExecuteCommand<FeatureElementInfo, string>(FeatureSharePointCommandIds.GetElementDefinition, elementInfo));

            XmlWriterSettings settings = new XmlWriterSettings();
            settings.OmitXmlDeclaration = true;
            settings.NewLineOnAttributes = true;
            settings.Indent = true;
            StringBuilder text = new StringBuilder();
            using (StringWriter stringWriter = new StringWriter(text))
            using (XmlWriter xmlWriter = XmlWriter.Create(stringWriter, settings))
            {
                document.WriteTo(xmlWriter);
            }
            e.Node.Context.ShowMessageBox(text.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Information);
            //IVsUIShellOpenDocument documentManager = (IVsUIShellOpenDocument)Package.GetGlobalService(typeof(SVsUIShellOpenDocument));
            //documentManager.
            //IOleServiceProvider provider;
            //IVsUIHierarchy hierarchy;
            //uint itemID;
            //IVsWindowFrame frame;

            ////Guid editorType = new Guid("{FA3CD31E-987B-443A-9B81-186104E8DAC1}");
            //Guid logicalView = new Guid("{7651a700-06e5-11d1-8ebd-00a0c90f26ea}");
            ////string path = Path.GetTempFileName() + ".xml";
            ////File.WriteAllText(path, "");
            //string p;
            //documentManager.MapLogicalView(ref editorType, ref logicalView, out p);
            //ErrorHandler.ThrowOnFailure(documentManager.OpenDocumentViaProjectWithSpecific(
            //    "txt",
            //    (uint)__VSSPECIFICEDITORFLAGS.VSSPECIFICEDITOR_UseEditor,
            //    ref editorType,
            //    p,
            //    ref logicalView,
            //    out provider,
            //    out hierarchy,
            //    out itemID,
            //    out frame));
            //frame.Show();
            //if (frame == null)
            //{
            //    ErrorHandler.ThrowOnFailure(documentManager.OpenSpecificEditor(0, null, ref editorType, physicalView, ref logicalView, "Owner",
            //        hierarchy, itemID, IntPtr.Zero, provider, out frame));
            //}
            //IOleServiceProvider vsProvider = (IOleServiceProvider)Package.GetGlobalService(typeof(IOleServiceProvider));
            //ILocalRegistry registry = (ILocalRegistry)Package.GetGlobalService(typeof(SLocalRegistry));
            //IVsUIShell shell = (IVsUIShell)Package.GetGlobalService(typeof(SVsUIShell));
            //
            //IVsRunningDocumentTable rdt = (IVsRunningDocumentTable)Package.GetGlobalService(typeof(SVsRunningDocumentTable));


            //Guid textLinesClassID = typeof(IVsTextLines).GUID;
            //IntPtr textLinesPointer = IntPtr.Zero;
            //ErrorHandler.ThrowOnFailure(registry.CreateInstance(
            //    typeof(VsTextBufferClass).GUID, 
            //    null, 
            //    ref textLinesClassID, 
            //    (uint)CLSCTX.CLSCTX_INPROC_SERVER, 
            //    out textLinesPointer));
            //IVsTextLines textBuffer = (IVsTextLines)Marshal.GetObjectForIUnknown(textLinesPointer);

            //IObjectWithSite sitedBuffer = (IObjectWithSite)textBuffer;
            //sitedBuffer.SetSite(vsProvider);

            //
            //XmlWriterSettings settings = new XmlWriterSettings();
            //settings.OmitXmlDeclaration = true;
            //settings.NewLineOnAttributes = true;
            //settings.Indent = true;
            //StringBuilder text = new StringBuilder();
            //using(StringWriter stringWriter= new StringWriter(text))
            //using (XmlWriter xmlWriter = XmlWriter.Create(stringWriter, settings))
            //{
            //    document.WriteTo(xmlWriter);
            //}
            //ErrorHandler.ThrowOnFailure(textBuffer.InitializeContent(text.ToString(), text.Length));

            //IVsPersistDocData2 docData2 = (IVsPersistDocData2)textBuffer;
            //ErrorHandler.ThrowOnFailure(docData2.SetDocDataReadOnly(1));
            //IntPtr windowPointer = IntPtr.Zero;
            //Guid windowClassID = typeof(IVsCodeWindow).GUID;
            //ErrorHandler.ThrowOnFailure(
            //    registry.CreateInstance(typeof(VsCodeWindowClass).GUID, null, 
            //    ref windowClassID, (uint)CLSCTX.CLSCTX_INPROC_SERVER, out windowPointer));
            //IVsCodeWindow window = (IVsCodeWindow)Marshal.GetObjectForIUnknown(windowPointer);
            //ErrorHandler.ThrowOnFailure(window.SetBuffer(textBuffer));



            //Guid empty = Guid.Empty;
            //Guid persistenceSlot = Guid.NewGuid();
            //int[] position = new int[1];
            //IVsWindowFrame frame;
            //ErrorHandler.ThrowOnFailure(
            //    shell.CreateToolWindow(
            //    0,
            //    0,
            //    window,
            //    ref empty,
            //    ref persistenceSlot,
            //    ref empty,
            //    null,
            //    "Caption", position, out frame));
            //frame.SetProperty((int)__VSFPROPID.VSFPROPID_FrameMode, VSFRAMEMODE.VSFM_Dock);
            //Guid guidCmdUI_TextEditor = new Guid("{8B382828-6202-11d1-8870-0000F87579D2}");
            //frame.SetGuidProperty((int)__VSFPROPID.VSFPROPID_InheritKeyBindings, ref guidCmdUI_TextEditor);
            //frame.Show();
        }

        /// <summary>
        /// Creates the node.
        /// </summary>
        /// <param name="parentNode">The parent node.</param>
        /// <param name="element">The element.</param>
        /// <returns></returns>
        internal static IExplorerNode CreateNode(IExplorerNode parentNode, FeatureElementInfo element)
        {
            Dictionary<object, object> annotations = new Dictionary<object, object>();
            annotations.Add(typeof(FeatureElementInfo), element);
            return parentNode.ChildNodes.Add(ExplorerNodeIds.FeatureElementNode,
                String.Format("{0} ({1})", element.Name, element.ElementType),
                annotations);
        }

        /// <summary>
        /// Initializes the new node type.
        /// </summary>
        /// <param name="typeDefinition">The definition of the new node type.</param>
        public void InitializeType(IExplorerNodeTypeDefinition typeDefinition)
        {
            typeDefinition.IsAlwaysLeaf = true;
        }
    }
}
