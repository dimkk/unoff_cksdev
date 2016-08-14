using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CKS.Dev.WCT.SolutionModel
{
    public class ClassMapDictionary : Dictionary<string, ClassInformationCollection>
    {

        public void Add(ClassInformation classInfo)
        {
            if (classInfo != null)
            {
                Add(classInfo.FullClassNameWithoutAssembly, classInfo);
            }
        }

        public void Add(string key, ClassInformation classInfo)
        {
            if (!String.IsNullOrEmpty(classInfo.FullClassNameWithoutAssembly))
            {
                ClassInformationCollection collection = null;
                if (this.ContainsKey(key))
                {
                    collection = this[key];
                }
                else
                {
                    collection = new ClassInformationCollection();
                    this.Add(key, collection);
                }

                collection.Add(classInfo);
            }
        }


    }
}
