using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;

namespace Hyt.Util.Net
{
    /// <summary>
    /// FTP工具类
    /// </summary>
    /// <remarks>2013-7-3 杨浩 添加</remarks>
    public class FtpUtil
    {
        private readonly string _ftpUri;            //ftp服务器地址
        private readonly string _ftpName;           //ftp账户
        private readonly string _ftpPwd;            //ftp密码
        private FtpWebRequest _ftpRequest;          //请求
        private FtpWebResponse _ftpResponse;        //响应

        /// <summary>
        /// FTP工具
        /// </summary>
        /// <param name="uri">FTP地址</param>
        /// <param name="name">用户名</param>
        /// <param name="password">密码</param>
        /// <remarks>2013-7-3 杨浩 添加</remarks>
        public FtpUtil(string uri, string name, string password)
        {
            this._ftpUri = uri;
            this._ftpName = name;
            this._ftpPwd = password;
        }

        /// <summary>
        /// 连接类
        /// </summary>
        /// <param name="uri">ftp地址</param>
        /// <returns></returns>
        /// <remarks>2013-7-3 杨浩 添加</remarks>
        private void Conn(string uri)
        {
            _ftpRequest = (FtpWebRequest)WebRequest.Create(uri);
            //登录ftp服务器，ftpName:账户名，ftpPwd:密码
            _ftpRequest.Credentials = new NetworkCredential(_ftpName, _ftpPwd);
            _ftpRequest.UseBinary = true;  //该值指定文件传输的数据类型。
        }

        /// <summary>
        /// 删除文件
        /// </summary>
        /// <param name="fileName">文件名</param>
        /// <returns></returns>
        /// <remarks>2013-7-3 杨浩 添加</remarks>
        public void DeleteFileName(string fileName)
        {
            string uri = _ftpUri + fileName;
            Conn(uri);
            try
            {
                _ftpRequest.Method = WebRequestMethods.Ftp.DeleteFile;
                _ftpResponse = (FtpWebResponse)_ftpRequest.GetResponse();
                _ftpResponse.Close();
            }
            catch (Exception exp)
            {
                Log.LogManager.Instance.WriteLog(exp.Message);
                throw exp;
            }
        }

        /// <summary>
        /// 上传文件，使用FTPWebRequest、FTPWebResponse实例
        /// </summary>
        /// <param name="uri">ftp地址</param>
        /// <param name="fileName">文件名</param>
        /// <param name="fileData">文件内容</param>
        /// <param name="msg">传出参数，返回传输结果</param>
        /// <returns></returns>
        /// <remarks>2013-7-3 杨浩 添加</remarks>
        public void UploadFile(string uri, string fileName, byte[] fileData, out string msg)
        {
            string URI = uri.EndsWith("/") ? uri : uri + "/";
            URI += fileName;
            //连接ftp服务器
            Conn(URI);
            _ftpRequest.Method = WebRequestMethods.Ftp.UploadFile;
            _ftpRequest.ContentLength = fileData.Length; //上传文件时通知服务器文件的大小
            try
            {
                //将文件流中的数据（byte[] fileData）写入请求流
                using (Stream ftpstream = _ftpRequest.GetRequestStream())
                {
                    ftpstream.Write(fileData, 0, fileData.Length);
                }

                _ftpResponse = (FtpWebResponse)_ftpRequest.GetResponse(); //响应
                msg = _ftpResponse.StatusDescription; //响应状态
                _ftpResponse.Close();
            }
            catch (Exception exp)
            {
                msg = "Failed to upload:" + exp.Message;
                Log.LogManager.Instance.WriteLog(exp.Message);
                throw exp;
            }
        }

        /// <summary>
        /// 获取文件列表  
        /// </summary>
        /// <returns></returns>
        public string[] GetFileList(string uriPath)
        {
            string[] downloadFiles;
            StringBuilder result = new StringBuilder();

            try
            {

                Conn(uriPath);

                // 指定数据传输类型  
                _ftpRequest.UseBinary = true;


                // 指定执行什么命令  
                _ftpRequest.Method = WebRequestMethods.Ftp.ListDirectory;
                WebResponse response = _ftpRequest.GetResponse();


                //获取文件流
                StreamReader reader = new StreamReader(response.GetResponseStream());
                string line = reader.ReadLine();


                //如果有文件就将文件名添加到文件列表
                while (line != null)
                {
                    result.Append(line);
                    result.Append("\n");
                    line = reader.ReadLine();
                }


                result.Remove(result.ToString().LastIndexOf('\n'), 1);


                //关闭流
                reader.Close();
                response.Close();
                return result.ToString().Split('\n');
            }
            catch (Exception ex)
            {
                downloadFiles = null;
                return downloadFiles;
            }
        }

        /// <summary>
        /// 上传文件，使用WebClient类
        /// </summary>
        /// <param name="uri">ftp地址</param>
        /// <param name="fileName">文件名</param>
        /// <param name="fileData">文件内容</param>
        /// <param name="msg">传出参数，输出传输结果</param>
        /// <returns></returns>
        /// <remarks>2013-7-3 杨浩 添加</remarks>
        public void UploadFileByWebClient(string uri, string fileName, byte[] fileData, out string msg)
        {
            string URI = uri.EndsWith("/") ? uri : uri + "/";
            URI += fileName;

            try
            {
                WebClient client = new WebClient();
                //登录FTP服务
                client.Credentials = new NetworkCredential(_ftpName, _ftpPwd);
                client.UploadData(URI, "STOR", fileData); //指定为ftp上传方式
                msg = "上传成功!";
            }
            catch (Exception exp)
            {
                msg = "Failed to upload:" + exp.Message;
                Log.LogManager.Instance.WriteLog(exp.Message);
                throw exp;
            }
        }

        /// <summary>
        /// 下载文件，使用FTPWebRequest、FTPWebResponse实例
        /// </summary>
        /// <param name="uri">ftp地址</param>
        /// <param name="destinationDir">目标文件存放地址</param>
        /// <param name="msg">传出参数，返回传输结果</param>
        /// <returns></returns>
        /// <remarks>2013-7-3 杨浩 添加</remarks>
        public void DownloadFile(string uri, string destinationDir, out string msg)
        {
            string fileName = Path.GetFileName(uri);
            string destinationPath = Path.Combine(destinationDir, fileName);

            try
            {
                //连接ftp服务器
                Conn(uri);

                using (FileStream outputStream = new FileStream(destinationPath, FileMode.OpenOrCreate))
                {
                    using (_ftpResponse = (FtpWebResponse)_ftpRequest.GetResponse())
                    {
                        //将响应流中的数据写入到文件流
                        using (Stream ftpStream = _ftpResponse.GetResponseStream())
                        {
                            int bufferSize = 2048;
                            int readCount;
                            byte[] buffer = new byte[bufferSize];
                            readCount = ftpStream.Read(buffer, 0, bufferSize);
                            while (readCount > 0)
                            {
                                outputStream.Write(buffer, 0, readCount);
                                readCount = ftpStream.Read(buffer, 0, bufferSize);
                            }
                        }
                        msg = _ftpResponse.StatusDescription;
                    }
                }
            }
            catch (Exception exp)
            {
                msg = "Failed to download:" + exp.Message;
                Log.LogManager.Instance.WriteLog(exp.Message);
                throw exp;
            }
        }

        /// <summary>
        /// 文件下载，使用WebClient类
        /// </summary>
        /// <param name="uri">ftp服务地址</param>
        /// <param name="destinationDir">存放目录</param>
        /// <returns></returns>
        /// <remarks>2013-7-3 杨浩 添加</remarks>
        public void DownloadFileByWebClient(string uri, string destinationDir)
        {

            string fileName = Path.GetFileName(uri);
            string destinationPath = Path.Combine(destinationDir, fileName);

            try
            {
                WebClient client = new WebClient();
                client.Credentials = new NetworkCredential(this._ftpName, this._ftpPwd);

                byte[] responseData = client.DownloadData(uri);

                using (FileStream fileStream = new FileStream(destinationPath, FileMode.OpenOrCreate))
                {
                    fileStream.Write(responseData, 0, responseData.Length);
                }

            }
            catch (Exception exp)
            {
                Log.LogManager.Instance.WriteLog(exp.Message);
                throw exp;
            }
        }

        /// <summary>
        /// 获取文件流 
        /// </summary>
        /// <param name="uri">FTP文件地址</param>
        /// <param name="msg">消息</param>
        /// <returns>文件流</returns>
        /// <remarks>2013-7-3 杨浩 添加</remarks>
        public Stream FileStream(string uri, ref string msg)
        {
            //连接ftp服务器
            Conn(uri);
            Stream fileStream = new MemoryStream();
            try
            {
                using (_ftpResponse = (FtpWebResponse)_ftpRequest.GetResponse())
                {
                    //将响应流中的数据写入到文件流
                    using (Stream ftpStream = _ftpResponse.GetResponseStream())
                    {
                        // 设置当前流的位置为流的开始
                        //ftpStream.Seek(0, SeekOrigin.Begin);

                        int bufferSize = 2048;
                        int readCount;
                        byte[] buffer = new byte[bufferSize];
                        readCount = ftpStream.Read(buffer, 0, bufferSize);

                        while (readCount > 0)
                        {
                            fileStream.Write(buffer, 0, readCount);
                            readCount = ftpStream.Read(buffer, 0, bufferSize);
                        }
                    }
                }
                msg = "get file success!";
            }
            catch (Exception ex)
            {            
                msg =_ftpResponse==null?"404":_ftpResponse.StatusDescription;
            }
            return fileStream;

        }

        /// <summary>
        /// 遍历文件
        /// </summary>
        /// <returns>文件列表数组</returns>
        /// <remarks>2013-7-3 杨浩 添加</remarks>
        public ArrayList GetListDirectoryDetails()
        {
            ArrayList fileInfo = new ArrayList();
            try
            {
                Conn(_ftpUri);

                //获取 FTP 服务器上的文件的详细列表的 FTP LIST 协议方法
                _ftpRequest.Method = WebRequestMethods.Ftp.ListDirectory;
                try
                {
                    _ftpResponse = (FtpWebResponse)_ftpRequest.GetResponse(); //响应
                }
                catch (System.Net.WebException e)
                {
                    Log.LogManager.Instance.WriteLog(e.Message);
                    throw e;
                }
                catch (System.InvalidOperationException e)
                {
                    Log.LogManager.Instance.WriteLog(e.Message);
                    throw e;
                }

                using (Stream responseStream = _ftpResponse.GetResponseStream())
                {
                    using (StreamReader reader = new StreamReader(responseStream))
                    {
                        string line = reader.ReadLine();
                        while (line != null)
                        {
                            fileInfo.Add(line);
                            line = reader.ReadLine();
                        }
                    }
                }
            }
            catch (Exception exp)
            {
                Log.LogManager.Instance.WriteLog(exp.Message);
                throw exp;
            }

            return fileInfo;

        }

        /// <summary>
        /// 上传文件
        /// </summary>
        /// <param name="mstr">文件流</param>
        /// <param name="ftpUri">上传到的完整路径</param>
        /// <returns>上传结果</returns>
        /// <remarks>2013-7-25 黄波  修改</remarks>
        public bool Upload(MemoryStream mstr, string ftpUri)
        {
            try
            {
                CheckMakeDir(ftpUri.Substring(0, ftpUri.LastIndexOf('/')));
                FtpWebRequest ftp = (FtpWebRequest)WebRequest.Create(ftpUri);
                ftp.Credentials = new NetworkCredential(_ftpName, _ftpPwd);
                ftp.Method = WebRequestMethods.Ftp.UploadFile;
                ftp.UseBinary = true;
                ftp.UsePassive = true;
                ftp.KeepAlive = true;
                Log.LogManager.Instance.WriteLog("1");
                using (Stream stream = ftp.GetRequestStream())
                {
                    Log.LogManager.Instance.WriteLog("2");
                    byte[] bytes = mstr.ToArray();
                    stream.Write(bytes, 0, bytes.Length);
                    stream.Close();
                    mstr.Close();
                }
                //ftp.GetResponse().Close();
                //ftp.GetRequestStream().Close();
                return true;
            }
            catch (Exception ex)
            {
                Log.LogManager.Instance.WriteLog(ex.Message + "黄波");
                return false;
            }
        }

        /// <summary>
        /// 上传文件
        /// </summary>
        /// <param name="mstr">文件字节数组</param>
        /// <param name="ftpUri">FTP存放位置</param>
        /// <returns>上传结果</returns>
        /// <remarks>2013-7-3 杨浩 添加</remarks>
        public bool Upload(byte[] mstr, string ftpUri)
        {
            try
            {
                FtpWebRequest ftp = (FtpWebRequest)WebRequest.Create(ftpUri);
                ftp.Credentials = new NetworkCredential(_ftpName, _ftpPwd);
                ftp.Method = WebRequestMethods.Ftp.UploadFile;
                ftp.UseBinary = true;
                ftp.UsePassive = true;
                using (Stream stream = ftp.GetRequestStream())
                {
                    stream.Write(mstr, 0, mstr.Length);
                    stream.Close();
                }
                return true;
            }
            catch (Exception ex)
            {
                Log.LogManager.Instance.WriteLog(ex.Message);
                return false;
            }
        }

        /// <summary>
        /// 重命名文件
        /// </summary>
        /// <param name="fileName">文件名</param>
        /// <param name="reNameFileName">新文件名</param>
        /// <returns></returns>
        /// <remarks>2013-7-3 杨浩 添加</remarks>
        public bool RenameFileName(string fileName, string reNameFileName)
        {
            bool result = true;
            FtpWebRequest ftp = (FtpWebRequest)WebRequest.Create(reNameFileName);
            ftp.Credentials = new NetworkCredential(_ftpName, _ftpPwd);
            try
            {
                ftp.Method = WebRequestMethods.Ftp.Rename;
                ftp.RenameTo = fileName;
                ftp.UseBinary = true;
                ftp.UsePassive = true;
                _ftpResponse = (FtpWebResponse)ftp.GetResponse();
                Stream ftpStream = _ftpResponse.GetResponseStream();
                ftpStream.Close();
                _ftpResponse.Close();

            }
            catch (Exception exp)
            {
                result = false;
                Log.LogManager.Instance.WriteLog(exp.Message);
            }

            return result;
        }

        /// <summary>
        /// 删除文件
        /// </summary>
        /// <param name="fileName">文件名</param>
        /// <returns>删除结果</returns>
        /// <remarks>2013-7-3 杨浩 添加</remarks>
        public bool DeleteFile(string fileName)
        {
            bool result = true;
            FtpWebRequest ftp = (FtpWebRequest)WebRequest.Create(fileName);
            ftp.Credentials = new NetworkCredential(_ftpName, _ftpPwd);
            try
            {
                ftp.Method = WebRequestMethods.Ftp.DeleteFile;
                ftp.UseBinary = true;
                ftp.UsePassive = true;
                _ftpResponse = (FtpWebResponse)ftp.GetResponse();
                Stream ftpStream = _ftpResponse.GetResponseStream();
                ftpStream.Close();
                _ftpResponse.Close();

            }
            catch (Exception exp)
            {
                result = false;
                Log.LogManager.Instance.WriteLog(exp.Message);
            }

            return result;
        }

        /// <summary>
        /// 检查Ftp文件夹是否存在，不存在则创建
        /// </summary>
        /// <param name="ftpDirectoryPath">Ftp目录地址(eg:ftp://222.222.222.222:21/2013/08/05/ )</param>
        /// <returns></returns>
        /// <remarks>2013-7-3 杨浩 添加</remarks>
        public void CheckMakeDir(string ftpDirectoryPath)
        {
            try
            {
                if (!FtpDirectoryExists(ftpDirectoryPath))
                {
                    Conn(ftpDirectoryPath); //连接
                    _ftpRequest.Method = WebRequestMethods.Ftp.MakeDirectory;
                    FtpWebResponse response = (FtpWebResponse)_ftpRequest.GetResponse();
                    response.Close();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 检查FTP文件夹是否存在
        /// </summary>
        /// <param name="directoryPath">FTP文件路径</param>
        /// <returns>true:存在 false:不存在</returns>
        /// <remarks>2013-7-3 杨浩 添加</remarks>
        public bool FtpDirectoryExists(string directoryPath)
        {
            bool isExists = true;
            try
            {
                Conn(directoryPath);
                _ftpRequest.Method = WebRequestMethods.Ftp.ListDirectory;
                //_ftpRequest.KeepAlive = false;
                FtpWebResponse response = (FtpWebResponse)_ftpRequest.GetResponse();
            }
            catch (WebException)
            {
                isExists = false;
            }
            return isExists;
        }

        /// <summary>
        /// 创建FTP目录
        /// </summary>
        /// <param name="dirName">目录名称</param>
        /// <returns></returns>
        /// <remarks>2013-7-3 杨浩 添加</remarks>
        public void MakeDir(string dirName)
        {
            try
            {

                Conn(dirName);//连接
                _ftpRequest.Method = WebRequestMethods.Ftp.MakeDirectory;
                FtpWebResponse response = (FtpWebResponse)_ftpRequest.GetResponse();
                response.Close();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
