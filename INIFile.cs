using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;

namespace Smith
{
    public class INIFile : IDisposable
    {
        private string iniFile = string.Empty;

        public INIFile(string _iniFile)
        {
            iniFile = _iniFile;
        }

        ~INIFile()
        {
            Dispose(false);
        }

        public void Dispose()
        {
            Dispose(true);

            GC.SuppressFinalize(this);
        }

        private bool DidWrite = false;

        //private volatile bool disposed = false;
        protected virtual void Dispose(bool disposing)
        {
            if(DidWrite)
                WritePrivateProfileString(null, null, null, null);
        }

        [DllImport("kernel32.dll", EntryPoint = "GetPrivateProfileStringW", SetLastError = true, CharSet = CharSet.Unicode, ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
        private static extern UInt32 GetPrivateProfileString(string lpAppName, string lpKeyName, string lpDefault, string lpReturnString, int nSize, string lpFilename);

        [DllImport("kernel32.dll", EntryPoint = "WritePrivateProfileStringW", SetLastError = true, CharSet = CharSet.Unicode, ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
        private static extern UInt32 WritePrivateProfileString(string lpAppName, string lpKeyName, string lpString, string lpFilename);

        public string GetKey(string catagory, string key, string defaultValue)
        {
            string returnString = new string(' ', 1024);
            GetPrivateProfileString(catagory, key, defaultValue, returnString, 1024, iniFile);

            return returnString.Split('\0')[0];
        }

        public void WriteKey(string catagory, string key, string value)
        {
            DidWrite = true;
            WritePrivateProfileString(catagory, key, value, iniFile);
        }
    }
}
