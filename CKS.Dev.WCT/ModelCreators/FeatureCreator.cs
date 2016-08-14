using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

using Microsoft.VisualStudio.SharePoint;

using CKS.Dev.WCT.Framework.Extensions;
using CKS.Dev.WCT.SolutionModel;
using CKS.Dev.WCT.Extensions;
using CKS.Dev.WCT.Framework.IO;

namespace CKS.Dev.WCT.ModelCreators
{
    public class FeatureCreator
    {
        #region Properties 

        public WCTContext WCTContext { get; set; }

        #endregion


        public FeatureCreator(WCTContext context)
        {
            this.WCTContext = context;
        }



        //[System.Xml.Serialization.XmlElementAttribute("ContentType", typeof(ContentTypeDefinition))]
        //[System.Xml.Serialization.XmlElementAttribute("ContentTypeBinding", typeof(ContentTypeBindingDefinition))]
        //[System.Xml.Serialization.XmlElementAttribute("Control", typeof(DelegateControlDefinition))]
        //[System.Xml.Serialization.XmlElementAttribute("CustomAction", typeof(CustomActionDefinition))]
        //[System.Xml.Serialization.XmlElementAttribute("CustomActionGroup", typeof(CustomActionGroupDefinition))]
        //[System.Xml.Serialization.XmlElementAttribute("DocumentConverter", typeof(DocumentConverterDefinition))]
        //[System.Xml.Serialization.XmlElementAttribute("FeatureSiteTemplateAssociation", typeof(FeatureSiteTemplateAssociationDefinition))]
        //[System.Xml.Serialization.XmlElementAttribute("Field", typeof(SharedFieldDefinition))]
        //[System.Xml.Serialization.XmlElementAttribute("GroupMigrator", typeof(GroupMigratorDefinition))]
        //[System.Xml.Serialization.XmlElementAttribute("HideCustomAction", typeof(HideCustomActionDefinition))]
        //[System.Xml.Serialization.XmlElementAttribute("ListInstance", typeof(ListInstanceDefinition))]
        //[System.Xml.Serialization.XmlElementAttribute("ListTemplate", typeof(ListTemplateDefinition))]
        //[System.Xml.Serialization.XmlElementAttribute("Module", typeof(ModuleDefinition))]
        //[System.Xml.Serialization.XmlElementAttribute("PropertyBag", typeof(PropertyBagDefinition))]
        //[System.Xml.Serialization.XmlElementAttribute("Receivers", typeof(ReceiverDefinitionCollection))]
        //[System.Xml.Serialization.XmlElementAttribute("UserMigrator", typeof(UserMigratorDefinition))]
        //[System.Xml.Serialization.XmlElementAttribute("WebPartAdderExtension", typeof(WebPartAdderExtensionDefinition))]
        //[System.Xml.Serialization.XmlElementAttribute("WebTemplate", typeof(WebTemplateDefinition))]
        //[System.Xml.Serialization.XmlElementAttribute("Workflow", typeof(WorkflowDefinition))]
        //[System.Xml.Serialization.XmlElementAttribute("WorkflowActions", typeof(WorkflowActionsDefinition))]
        //[System.Xml.Serialization.XmlElementAttribute("WorkflowAssociation", typeof(WorkflowAssociationDefinition))]
        public void AddVSSharePointItems(FeatureDefinition feature, FileInfo info)
        {
            ElementDefinitionCollection elementDefintion = FileSystem.Load<ElementDefinitionCollection>(info.FullName);
            elementDefintion.SourceFileInfo = info;

            foreach (object item in elementDefintion.Items)
            {
                VSSharePointItem vsItem = Create(feature, elementDefintion, item);
                if (vsItem != null)
                {
                    feature.SharePointItems.Add(vsItem);
                }
            }

            foreach (var entry in elementDefintion.GenericItems)
            {
                string typeName = entry.Key;
                if (entry.Value.Count > 0)
                {
                    VSGenericItem vsItem = new VSGenericItem(feature);
                    vsItem.Items = entry.Value;

                    Type type = vsItem.Items[0].GetType();

                    vsItem.Name = feature.Name + " " + type.Name;
                    
                    feature.SharePointItems.Add(vsItem);
                }
            }

            ProjectFile projFile = this.WCTContext.SourceProject.Files.GetValue(info.FullName);
            if (projFile != null)
            {
                projFile.Used = true;
                projFile.Referenced = true;
            }

        }


        private VSSharePointItem Create(FeatureDefinition feature, ElementDefinitionCollection elementDefintion, object item)
        {
            VSSharePointItem result = null;

            if (result == null && item is ModuleDefinition)
            {
                result = ProcessModuleDefinition(feature, (ModuleDefinition)item);
            }

            if (result == null && item is ContentTypeDefinition)
            {
                result = ProcessContentTypeDefinition(feature, (ContentTypeDefinition)item);
            }

            if (result == null && item is ReceiverDefinitionCollection)
            {
                result = ProcessReceiversDefinition(feature, (ReceiverDefinitionCollection)item);
            }

            if (result == null && item is ListTemplateDefinition)
            {
                result = ProcessListDefinition(feature, elementDefintion, (ListTemplateDefinition)item);
            }

            if (result == null && item is ListInstanceDefinition)
            {
                result = ProcessListInstance(feature, (ListInstanceDefinition)item);
            }

            if (result == null && item is WorkflowDefinition)
            {
                result = ProcessWorkflowDefinition(feature, (WorkflowDefinition)item);
            }

            if (result == null)
            {
                result = ProcessGenericDefinition(feature, elementDefintion, item);
            }

            return result;
        }

        private VSSharePointItem ProcessModuleDefinition(FeatureDefinition feature, ModuleDefinition module)
        {
            VSSharePointItem result = null;
            if (module.List == 113 || "_catalogs/wp".EqualsIgnoreCase(module.Url))
            {
                result = AddElementWebPart(feature, module);
            }
            else
            {
                result = AddElementModule(feature, module);
            }
            return result;
        }

        private VSSharePointItem AddElementWebPart(FeatureDefinition feature, ModuleDefinition module)
        {
            VSWebPartItem vsItem = new VSWebPartItem(feature);
            vsItem.Module = module;

            foreach (FileDefinition fileDef in module.File.AsSafeEnumable())
            {
                string filename = (String.IsNullOrWhiteSpace(fileDef.Path)) ? fileDef.Url : fileDef.Path;
                fileDef.LocalPath = Path.Combine(module.Path.EnsureString(), filename);

                fileDef.Filename = Path.Combine(feature.SourceFolder, fileDef.LocalPath);
                fileDef.ParseWebPart(this.WCTContext.SourceProject);

                vsItem.AddProjectFile(this.WCTContext, fileDef.Filename, fileDef.LocalPath);

                // Add the class file to this Item
                if (fileDef.Classes != null)
                {
                    foreach (ClassInformation classInfo in fileDef.Classes)
                    {
                        if (!classInfo.File.Used)
                        {
                            vsItem.AddProjectFile(this.WCTContext, classInfo.File.Info.FullName, DeploymentType.NoDeployment);
                            vsItem.Classes.Add(classInfo);
                        }
                    }
                }
            }

            return vsItem;
        }


        private VSSharePointItem AddElementModule(FeatureDefinition feature, ModuleDefinition module)
        {
            VSModuleItem vsItem = new VSModuleItem(feature);
            vsItem.Module = module;

            foreach (FileDefinition fileDef in module.File.AsSafeEnumable())
            {
                string filename = (String.IsNullOrWhiteSpace(fileDef.Path)) ? fileDef.Url : fileDef.Path;
                fileDef.LocalPath = Path.Combine(module.Path.EnsureString(), filename);

                fileDef.Filename = Path.Combine(feature.SourceFolder, fileDef.LocalPath);

                vsItem.AddProjectFile(this.WCTContext, fileDef.Filename, fileDef.LocalPath);
            }
            return vsItem;
        }



        private VSSharePointItem ProcessContentTypeDefinition(FeatureDefinition feature, ContentTypeDefinition ct)
        {
            VSContentTypeItem vsItem = new VSContentTypeItem(feature);
            
            vsItem.ContentType = ct;
            
            return vsItem;
        }

        private VSSharePointItem ProcessReceiversDefinition(FeatureDefinition feature, ReceiverDefinitionCollection receiver)
        {
            VSEventHandlerItem vsItem = new VSEventHandlerItem(feature, receiver);
            
            foreach (ReceiverDefinition def in vsItem.Receivers.Receiver)
            {

                ClassInformationCollection classes = this.WCTContext.SourceProject.Classes.GetValue(def.Class);
                foreach (ClassInformation classInfo in classes)
                {
                    if (classInfo != null && !classInfo.File.Used)
                    {
                        vsItem.AddProjectFile(this.WCTContext, classInfo.File.Info.FullName, DeploymentType.NoDeployment);
                    }
                }
            }

            return vsItem;
        }

        private VSSharePointItem ProcessListDefinition(FeatureDefinition feature, ElementDefinitionCollection elementDefintion, ListTemplateDefinition listTemplateDef)
        {
            VSListDefinitionItem vsItem = new VSListDefinitionItem(feature, listTemplateDef);

            string path = Path.Combine(feature.SourceFolder, listTemplateDef.Name);
            if (Directory.Exists(path))
            {
                DirectoryInfo dir = new DirectoryInfo(path);
                foreach (var file in dir.GetFiles(this.WCTContext, "*", SearchOption.AllDirectories))
                {
                    //string localName = file.GetLocalName(feature.SourceFolder);
                    //string filePath = Path.Combine(Path.GetDirectoryName(elementDefintion.SourceFileInfo.DirectoryName), file.Name);

                    vsItem.AddProjectFile(this.WCTContext, file.FullName);
                }
            }
            
            return vsItem;
        }

        private VSSharePointItem ProcessListInstance(FeatureDefinition feature, ListInstanceDefinition listInstance)
        {
            VSListInstanceItem vsItem = new VSListInstanceItem(feature, listInstance);

            return vsItem;
        }

        private VSSharePointItem ProcessWorkflowDefinition(FeatureDefinition feature, WorkflowDefinition workflow)
        {
            VSWorkflowItem vsItem = new VSWorkflowItem(feature, workflow);

            vsItem.AddCodeFile(this.WCTContext, vsItem.Workflow.CodeBesideClass);
            vsItem.AddCodeFile(this.WCTContext, vsItem.Workflow.EngineClass);

            return vsItem;
        }

        private VSSharePointItem ProcessGenericDefinition(FeatureDefinition feature, ElementDefinitionCollection elementDefintion, object item)
        {

            string name = item.GetType().FullName;

            List<object> list = null;
            if (elementDefintion.GenericItems.ContainsKey(name))
            {
                list = elementDefintion.GenericItems[name];
            }
            else
            {
                list = new List<object>();
                elementDefintion.GenericItems.Add(name, list);
            }

            list.Add(item);

            return null;
        }

    }

}
