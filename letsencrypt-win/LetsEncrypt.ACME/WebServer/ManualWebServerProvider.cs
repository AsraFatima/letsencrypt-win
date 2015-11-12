﻿using LetsEncrypt.ACME.Util;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LetsEncrypt.ACME.WebServer
{
    public class ManualWebServerProvider : BaseManualProvider, IWebServerProvider
    {
        public string FilePath
        { get; set; }

        public void UploadFile(Uri fileUrl, Stream s)
        {
            var path = FilePath;

            if (string.IsNullOrEmpty(path))
                path = Path.GetTempFileName();
            else
            {
                int index = 0;
                while (File.Exists(path))
                    path = string.Format("{0}.{1}", FilePath, ++index);
            }

            var dir = Path.GetDirectoryName(path);
            if (!Directory.Exists(dir))
                throw new DirectoryNotFoundException("Missing folder in requested file path");
            
            using (var fs = new FileStream(path, FileMode.CreateNew))
            {
                s.CopyTo(fs);
            }

            _writer.WriteLine("Manually Upload Web Server File:");
            _writer.WriteLine("  *           URL:  [{0}]", fileUrl);
            _writer.WriteLine("  *  File Content:  [{0}]", path);
        }
    }
}
