using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace DMarket.BaseViewModels.IO
{
    public class IODataBase
    {
        public IODataBase(string folderName)
        {
            if (string.IsNullOrEmpty(folderName))
                throw new Exception("Folder Name is empty or null");
            FolderName = folderName;
            PhysicalPath = Path.Combine(System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location), "IODataBase", FolderName);
        }

        string _FolderName;

        public string FolderName
        {
            get { return _FolderName; }
            private set { _FolderName = value; }
        }

        private string _PhysicalPath;

        public string PhysicalPath
        {
            get { return _PhysicalPath; }
            private set { _PhysicalPath = value; }
        }

        public void Save(string pId, string id, List<string> fileIds, List<byte[]> listofBytes)
        {
            for (int i = 0; i < fileIds.Count; i++)
            {
                Save(pId, id, fileIds[i], listofBytes[i]);
            }
        }

        public void Save(string pId, string id, string fileId, byte[] bytes)
        {
            string folder = Path.Combine(PhysicalPath, pId, id);
            if (!Directory.Exists(folder))
                Directory.CreateDirectory(folder);
            string fileName = Path.Combine(folder, fileId + ".jpg");
            if (!File.Exists(fileName))
                File.WriteAllBytes(fileName, bytes);
        }

        public byte[] ReadFile(string pId, string id, string fileId)
        {
            string folder = Path.Combine(PhysicalPath, pId, id);
            string fileName = Path.Combine(folder, fileId + ".jpg");
            return File.ReadAllBytes(fileName);
        }

        public List<byte[]> ReadFile(string pId, string id)
        {
            string folder = Path.Combine(PhysicalPath, pId, id);
            List<byte[]> fileData = new List<byte[]>();
            foreach (var fileName in Directory.GetFiles(folder, "*.*", SearchOption.TopDirectoryOnly))
            {
                fileData.Add(File.ReadAllBytes(fileName));
            }
            return fileData;
        }

        public void RemoveFile(string pId, string id, string fileId)
        {
            string folder = Path.Combine(PhysicalPath, pId, id);
            string fileName = Path.Combine(folder, fileId + ".jpg");
            if (File.Exists(fileName))
                File.Delete(fileName);
        }

        public void RemoveFiles(string pId, string id)
        {
            string folder = Path.Combine(PhysicalPath, pId, id);
            if (Directory.Exists(folder))
                Directory.Delete(folder, true);
        }
    }
}
