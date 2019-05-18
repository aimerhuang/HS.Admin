using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;
using System.IO;
using System.Net;

namespace Extra.UpGrade.SDK.YouZan
{
    public class KDTUtil
    {    
        public const String APP_ID_KEY = "app_id";
        public const String METHOD_KEY = "method";
        public const String TIMESTAMP_KEY = "timestamp";
        public const String FORMAT_KEY = "format";
        public const String VERSION_KEY = "v";
        public const String SIGN_KEY = "sign";
        public const String SIGN_METHOD_KEY = "sign_method";

        public const int ALLOWED_DEVIATE_SECONDS = 600;



        public const int ERR_SYSTEM = -1;
        public const int ERR_INVALID_APP_ID = 40001;
        public const int ERR_INVALID_APP = 40002;
        public const int ERR_INVALID_TIMESTAMP = 40003;
        public const int ERR_EMPTY_SIGNATURE = 40004;
        public const int ERR_INVALID_SIGNATURE = 40005;
        public const int ERR_INVALID_METHOD_NAME = 40006;
        public const int ERR_INVALID_METHOD = 40007;
        public const int ERR_INVALID_TEAM = 40008;
        public const int ERR_PARAMETER = 41000;
        public const int ERR_LOGIC = 50000;

        public static String sign(String appSecret, Dictionary<String,String> parames, String method)
        {
            String signResult = String.Empty;
            List<String> keyList = parames.Keys.ToList();
            keyList.Sort();
            String signContent = appSecret;

            foreach (var key in keyList)
            {
               signContent += (key+parames[key]);
            }
            signContent += appSecret;
            signResult = hash(method, signContent);
            return signResult;
        }

        static String hash(String method, String signContent)
        {
            String hashResult = String.Empty;
            switch (method)
            {
                case "md5":
                default :
                    hashResult = System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(signContent, "MD5").ToLower(); 
                    break;
            }
            return hashResult;
        }

        private static readonly Encoding encoding = Encoding.UTF8;
        public static HttpWebRequest MultipartFormDataPost(string postUrl, string userAgent, Dictionary<string, object> postParameters, String fileKey)
        {
            string formDataBoundary = String.Format("----------{0:N}", Guid.NewGuid());
            string contentType = "multipart/form-data; boundary=" + formDataBoundary;

            byte[] formData = GetMultipartFormData(postParameters, formDataBoundary, fileKey);
            return PostForm(postUrl, userAgent, contentType, formData);
        }

        private static HttpWebRequest PostForm(string postUrl, string userAgent, string contentType, byte[] formData)
        {
            HttpWebRequest request = WebRequest.Create(postUrl) as HttpWebRequest;

            if (request == null)
            {
                throw new NullReferenceException("request is not a http request");
            }

            request.Method = "POST";
            request.ContentType = contentType;
            request.UserAgent = userAgent;
            request.CookieContainer = new CookieContainer();
            request.ContentLength = formData.Length;
            using (Stream requestStream = request.GetRequestStream())
            {
                requestStream.Write(formData, 0, formData.Length);
                requestStream.Close();
            }
            return request;
        }

        private static byte[] GetMultipartFormData(Dictionary<string, object> postParameters, string boundary, String fileKey)
        {
            Stream formDataStream = new System.IO.MemoryStream();
            bool needsCLRF = false;

            foreach (var param in postParameters)
            {
                if (needsCLRF)
                    formDataStream.Write(encoding.GetBytes("\r\n"), 0, encoding.GetByteCount("\r\n"));

                needsCLRF = true;

                if (param.Value is FileParameter)
                {
                    FileParameter fileToUpload = (FileParameter)param.Value;

                    // Add just the first part of this param, since we will write the file data directly to the Stream
                    string header = string.Format("--{0}\r\nContent-Disposition: form-data; name=\"{1}\"; filename=\"{2}\"\r\nContent-Type: {3}\r\n\r\n",
                        boundary,
                        fileKey,
                        fileToUpload.FileName ?? param.Key,
                        fileToUpload.ContentType ?? "image/unknown");

                    formDataStream.Write(encoding.GetBytes(header), 0, encoding.GetByteCount(header));

                    formDataStream.Write(fileToUpload.File, 0, fileToUpload.File.Length);
                }
                else
                {
                    string postData = string.Format("--{0}\r\nContent-Disposition: form-data; name=\"{1}\"\r\n\r\n{2}",
                        boundary,
                        param.Key,
                        param.Value);
                    formDataStream.Write(encoding.GetBytes(postData), 0, encoding.GetByteCount(postData));
                }
            }

            string footer = "\r\n--" + boundary + "--\r\n";
            formDataStream.Write(encoding.GetBytes(footer), 0, encoding.GetByteCount(footer));

            formDataStream.Position = 0;
            byte[] formData = new byte[formDataStream.Length];
            formDataStream.Read(formData, 0, formData.Length);
            formDataStream.Close();

            return formData;
        }

        public class FileParameter
        {
            public byte[] File { get; set; }
            public string FileName { get; set; }
            public string ContentType { get; set; }
            public FileParameter(byte[] file) : this(file, null) { }
            public FileParameter(byte[] file, string filename) : this(file, filename, null) { }
            public FileParameter(byte[] file, string filename, string contenttype)
            {
                File = file;
                FileName = filename;
                ContentType = contenttype;
            }
        }
    }
}
