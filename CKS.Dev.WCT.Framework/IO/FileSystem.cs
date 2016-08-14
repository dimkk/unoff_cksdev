using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using CKS.Dev.WCT.Framework.Serialization;

namespace CKS.Dev.WCT.Framework.IO
{
    public class FileSystem
    {
        public static string Combine(string path, string name)
        {
            return path + name;
        }

        public static string Combine(DirectoryInfo dir, string name)
        {
            return dir.FullName + @"\" + name;
        }

        public static DirectoryInfo EnsureDirectory(string fullPath)
        {
            DirectoryInfo resultDir = null;
            if (!Directory.Exists(fullPath))
            {
                resultDir = Directory.CreateDirectory(fullPath);
            }
            else
            {
                resultDir = new DirectoryInfo(fullPath);
            }
            return resultDir;
        }

        public static DirectoryInfo EnsureDirectory(DirectoryInfo currentDir, string name)
        {
            DirectoryInfo resultDir = null;
            string fullPath = FileSystem.Combine(currentDir, name);
            if (!Directory.Exists(fullPath))
            {
                resultDir = currentDir.CreateSubdirectory(name);
            }
            else
            {
                resultDir = new DirectoryInfo(fullPath);
            }
            return resultDir;
        }

        public static DirectoryInfo EnsureDirectory(string currentDir, string name)
        {
            DirectoryInfo resultDir = null;
            string fullPath = Path.Combine(currentDir, name);
            if (!Directory.Exists(fullPath))
            {
                resultDir = Directory.CreateDirectory(fullPath);
            }
            else
            {
                resultDir = new DirectoryInfo(fullPath);
            }
            return resultDir;
        }

        public static void Create(DirectoryInfo dir, string filename, object obj)
        {
            Create(dir.FullName, filename, obj);
        }

        public static void Create(string directoryPath, string filename, object obj)
        {
            string fullname = Path.Combine(directoryPath, filename);
            string xml = Serializer.FormatXml(Serializer.ObjectToXML(obj));
            xml = xml.Insert(xml.IndexOf('>') + 1, String.Format("\r\n<!-- Created by LCM - {0} -->", DateTime.Now.ToString()));

            RemoveReadOnly(fullname);
            File.WriteAllText(fullname, xml, Encoding.UTF8);
        }

        public static T Load<T>(string path)
        {
            T result = default(T);
            if (File.Exists(path))
            {
                result = Serializer.XmlToObject<T>(File.ReadAllText(path));
            }
            return result;
        }

        public static T LoadOrCreate<T>(string path)
        {
            T result = default(T);
            if (File.Exists(path))
            {
                result = Serializer.XmlToObject<T>(File.ReadAllText(path));
            }
            if (result == null)
            {
                result = Activator.CreateInstance<T>();
            }
            return result;
        }

        public static void RemoveReadOnly(string fullname)
        {
            if(File.Exists(fullname))
            {
                FileAttributes attributes = File.GetAttributes(fullname);
                attributes = (attributes & FileAttributes.ReadOnly) == FileAttributes.ReadOnly ? attributes ^ FileAttributes.ReadOnly : attributes;
                File.SetAttributes(fullname, attributes);
            }
        }

        /// <summary>
        /// Ensures that the target directory exist before copy.
        /// </summary>
        /// <param name="source"></param>
        /// <param name="target"></param>
        public static void Copy(string source, string target)
        {
            Copy(source, target, false);
        }

        public static void Copy(string source, string target, bool overwrite)
        {
            string targetFolderPath = Path.GetDirectoryName(target);
            if (!Directory.Exists(targetFolderPath))
            {
                Directory.CreateDirectory(targetFolderPath);
            }

            File.Copy(source, target, overwrite);
        }

        public static string MakeSafeFilename(string filename, char replaceChar)
        {
            if (String.IsNullOrEmpty(filename))
            {
                return filename;
            }

            foreach (char c in System.IO.Path.GetInvalidFileNameChars())
            {
                filename = filename.Replace(c, replaceChar);
            }
            return filename;
        }


        //public static void Save(string directoryPath, SPFile file)
        //{
            
        //    FileSystem.EnsureDirectory(directoryPath);
        //    string fullname = Path.Combine(directoryPath, file.Name);
        //    RemoveReadOnly(fullname);
        //    using (FileStream fs = new FileStream(fullname, FileMode.Create))
        //    {
        //        using (BinaryWriter bw = new BinaryWriter(fs))
        //        {
        //            bw.Write(file.OpenBinary());
        //            bw.Close();
        //        }
        //    }
  
        //}
    }
}
