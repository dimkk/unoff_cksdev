using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.SharePoint;

namespace CKS.Dev.VisualStudio.SharePoint.Commands
{
    class FieldInfo
    {
        public string Name { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Template { get; set; }
        public TagInfo Tag { get; set; }
        public SPField Field { get; set; }

        public FieldInfo(string name, string title, string description, string template, TagInfo tag, SPField field)
        {
            Name = name;
            Title = title;
            Description = description;
            Template = template;
            Tag = tag;
            Field = field;
        }
    }
}
