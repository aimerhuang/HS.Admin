using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Extra.UpGrade.Api
{
    public class FileItem
    {
        private string fileName;
        private string mimeType;
        private byte[] content;
        private FileInfo fileInfo;
        public FileItem(FileInfo fileInfo)
        {
            if (fileInfo == null || !fileInfo.Exists)
            {
                throw new ArgumentException("fileInfo is null or not exists!");
            }
            this.fileInfo = fileInfo;
        }
        public FileItem(string filePath)
            : this(new FileInfo(filePath))
        {
        }
        public FileItem(string fileName, byte[] content)
        {
            if (string.IsNullOrEmpty(fileName))
            {
                throw new ArgumentNullException("fileName");
            }
            if (content == null || content.Length == 0)
            {
                throw new ArgumentNullException("content");
            }
            this.fileName = fileName;
            this.content = content;
        }
        public FileItem(string fileName, byte[] content, string mimeType)
            : this(fileName, content)
        {
            if (string.IsNullOrEmpty(mimeType))
            {
                throw new ArgumentNullException("mimeType");
            }
            this.mimeType = mimeType;
        }
        public string GetFileName()
        {
            if (this.fileName == null && this.fileInfo != null && this.fileInfo.Exists)
            {
                this.fileName = this.fileInfo.FullName;
            }
            return this.fileName;
        }
        public string GetMimeType()
        {
            if (this.mimeType == null)
            {
                this.mimeType = FileItem.GetMimeType(this.GetContent());
            }
            return this.mimeType;
        }
        public byte[] GetContent()
        {
            if (this.content == null && this.fileInfo != null && this.fileInfo.Exists)
            {
                using (Stream stream = this.fileInfo.OpenRead())
                {
                    this.content = new byte[stream.Length];
                    stream.Read(this.content, 0, this.content.Length);
                }
            }
            return this.content;
        }
        public static string GetFileSuffix(byte[] fileData)
        {
            string result;
            if (fileData == null || fileData.Length < 10)
            {
                result = null;
            }
            else
            {
                if (fileData[0] == 71 && fileData[1] == 73 && fileData[2] == 70)
                {
                    result = "GIF";
                }
                else
                {
                    if (fileData[1] == 80 && fileData[2] == 78 && fileData[3] == 71)
                    {
                        result = "PNG";
                    }
                    else
                    {
                        if (fileData[6] == 74 && fileData[7] == 70 && fileData[8] == 73 && fileData[9] == 70)
                        {
                            result = "JPG";
                        }
                        else
                        {
                            if (fileData[0] == 66 && fileData[1] == 77)
                            {
                                result = "BMP";
                            }
                            else
                            {
                                result = null;
                            }
                        }
                    }
                }
            }
            return result;
        }
        public static string GetMimeType(byte[] fileData)
        {
            string fileSuffix = FileItem.GetFileSuffix(fileData);
            string text = fileSuffix;
            string result;
            if (text != null)
            {
                if (text == "JPG")
                {
                    result = "image/jpeg";
                    return result;
                }
                if (text == "GIF")
                {
                    result = "image/gif";
                    return result;
                }
                if (text == "PNG")
                {
                    result = "image/png";
                    return result;
                }
                if (text == "BMP")
                {
                    result = "image/bmp";
                    return result;
                }
            }
            result = "application/octet-stream";
            return result;
        }
        public static string GetMimeType(string fileName)
        {
            fileName = fileName.ToLower();
            string result;
            if (fileName.EndsWith(".bmp", StringComparison.CurrentCulture))
            {
                result = "image/bmp";
            }
            else
            {
                if (fileName.EndsWith(".gif", StringComparison.CurrentCulture))
                {
                    result = "image/gif";
                }
                else
                {
                    if (fileName.EndsWith(".jpg", StringComparison.CurrentCulture) || fileName.EndsWith(".jpeg", StringComparison.CurrentCulture))
                    {
                        result = "image/jpeg";
                    }
                    else
                    {
                        if (fileName.EndsWith(".png", StringComparison.CurrentCulture))
                        {
                            result = "image/png";
                        }
                        else
                        {
                            result = "application/octet-stream";
                        }
                    }
                }
            }
            return result;
        }
    }
}
