using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace Smith
{
    public unsafe class GOB : IDisposable, IEnumerable<string>
    {
        private class FileEntry
        {
            public readonly int offset;
            public readonly int length;

            public FileEntry(int _offset, int _length)
            {
                offset = _offset;
                length = _length;
            }
        }

        public override string ToString()
        {
            return Path.GetFileName(path);
        }

        private Stream Stream;

        public readonly string path;

        private Dictionary<string, FileEntry> FileEntries = new Dictionary<string, FileEntry>(StringComparer.InvariantCultureIgnoreCase);

        public GOB(string gobfile, Stream s)
        {
            if (s.Length == 0)
                throw new Exception("crappy gob");

            path = gobfile;
            Stream = s;

            BinaryReader br = new BinaryReader(Stream);

            br.ReadBytes(3);    // verification GOB
            br.ReadByte();      // version?
            br.ReadInt32();     // offset to first file length
            br.ReadInt32();     // offset to number of files
            int numFiles = br.ReadInt32();

            for (int x = 0; x < numFiles; x++)
            {
                int offset = br.ReadInt32();
                int length = br.ReadInt32();
                byte[] sz = br.ReadBytes(128);
                string filename;
                fixed (byte* psz = sz)
                    filename = new string((sbyte*)psz);

                if (string.IsNullOrEmpty(filename))
                {
                    //Log.Warning("GOB " + Path.GetFileName(path) + " has empty file name");
                    continue;
                }


                /*if(Tokenizer.IsGarbage(filename))
                {
                    Log.Error("CORRUPT GOB " + Path.GetFileName(path) + " has invalid file string (" + filename[0] + "), stopping load of gob");
                    return;
                }*/

                if (FileEntries.ContainsKey(filename))
                {
                    //Log.Warning("GOB specifies duplicate file entry " + filename + "; ignoring");
                }
                else
                    FileEntries.Add(filename, new FileEntry(offset, length));
            }
        }

        public IEnumerator<string> GetEnumerator()
        {
            return FileEntries.Keys.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        public int Count
        {
            get
            {
                return FileEntries.Count;
            }
        }

        public MemoryStream this[string filename]
        {
            get
            {
                if (!FileEntries.ContainsKey(filename))
                    return null;

                FileEntry fe = FileEntries[filename];
                Stream.Position = fe.offset;
                byte[] buf = new byte[fe.length];
                Stream.Read(buf, 0, fe.length);
                return new MemoryStream(buf);
            }
        }

        public string[] GetFilesWithExtension(string ext)
        {
            if (string.IsNullOrEmpty(ext))
                return new string[0];

            if (!ext.StartsWith("."))
                ext = "." + ext;

            List<string> files = new List<string>();
            foreach (string file in this)
                if (ext.Equals(System.IO.Path.GetExtension(file)))
                    files.Add(file);

            return files.ToArray();
        }

        ~GOB()
        {
            Dispose(false);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private bool disposed = false;
        protected virtual void Dispose(bool disposing)
        {
            if (disposed)
                return;

            if (disposing)
            {
                if (Stream != null)
                {
                    Stream.Dispose();
                    Stream = null;
                }
            }

            disposed = true;
        }
    }
}
