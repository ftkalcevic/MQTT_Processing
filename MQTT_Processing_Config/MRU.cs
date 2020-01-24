using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MQTT_Processing_Config
{
    class MRUFile
    {
        public string filename { get; set; }
        public string filepath { get; set; }
        public MRUFile(string filename, string filepath)
        {
            this.filename = filename;
            this.filepath = filepath;
        }
    }

    class MRU
    {
        List<MRUFile> files;
        int maxFiles;
        public List<MRUFile> mruFiles { get => files; }

        public MRU(int maxFiles = 10)
        {
            this.maxFiles = maxFiles;
            files = new List<MRUFile>();
        }

        public void LoadMRU(System.Collections.Specialized.StringCollection MRU)
        {
            files = new List<MRUFile>();
            if (MRU != null)
            {
                foreach (string filepath in MRU)
                {
                    if (File.Exists(filepath))
                    {
                        files.Add(new MRUFile(Path.GetFileName(filepath), filepath));
                    }
                }
            }
        }

        public System.Collections.Specialized.StringCollection SaveMRU()
        {
            var MRU = new System.Collections.Specialized.StringCollection();

            MRU.Clear();
            foreach (MRUFile file in files)
            {
                MRU.Add(file.filepath);
            }
            return MRU;
        }

        public void AddFile(string filepath)
        {
            // does the file already exist?
            for (int index = 0; index < files.Count; index++)
                if ( files[index].filepath.ToLower() == filepath.ToLower() )
                {
                    //  Match.  Just move the file to the front of the list, if it isn't already there.
                    if (index > 0)
                    {
                        MRUFile entry = files[index];
                        files.RemoveAt(index);
                        files.Insert(0, entry);
                    }
                    return;
                }
            string filename = Path.GetFileName(filepath);
            MRUFile newEntry = new MRUFile(filename, filepath);
            files.Insert(0, newEntry);
        }
    }
}
