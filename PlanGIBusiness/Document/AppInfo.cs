using Business.Library;
using Microsoft.Net.Http.Headers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace PTTPL.TMS.Business.Common {
    public static class AppsInfo {
        
        public static string server {
            get {
                var url = new AppSettingConfig().GetUrl("serivce_host").ToString();
                return url;
            }
        }

        public static string client {
            get {
                var url = new AppSettingConfig().GetUrl("client_host").ToString();
                return url;
            }
        }
        public static string domain {
            get {
                var url = new AppSettingConfig().GetUrl("serivce_domain").ToString();
                return url;
            }
        }

        public static string upload {
            get {
                var url = new AppSettingConfig().GetUrl("serivce_upload").ToString();
                return url;
            }
        }
        public static string document_upload {
            get {
                var url = new AppSettingConfig().GetUrl("serivce_document_upload").ToString();
                return url;
            }
        }
        public static string document_path {
            get {
                var url = new AppSettingConfig().GetUrl("serivce_document_path").ToString();
                return url;
            }
        }
        public static string local_path {
            get {
                try {
                    var url = new AppSettingConfig().GetUrl("service_local_path").ToString();
                    return url;
                } catch (Exception) {
                    return "";
                }
            }
        }

        public static string upload_host
        {
            get
            {
                try
                {
                    var url = new AppSettingConfig().GetUrl("upload_host").ToString();
                    return url;
                }
                catch (Exception)
                {
                    return "";
                }
            }
        }

        public static class MultipartRequestHelper
        {
            // Content-Type: multipart/form-data; boundary="----WebKitFormBoundarymx2fSWqWSd0OxQqq"
            // The spec at https://tools.ietf.org/html/rfc2046#section-5.1 states that 70 characters is a reasonable limit.
            public static string GetBoundary(Microsoft.Net.Http.Headers.MediaTypeHeaderValue contentType, int lengthLimit)
            {
                var boundary = HeaderUtilities.RemoveQuotes(contentType.Boundary).Value;

                if (string.IsNullOrWhiteSpace(boundary))
                {
                    throw new InvalidDataException("Missing content-type boundary.");
                }

                if (boundary.Length > lengthLimit)
                {
                    throw new InvalidDataException(
                        $"Multipart boundary length limit {lengthLimit} exceeded.");
                }

                return boundary;
            }

            public static bool IsMultipartContentType(string contentType)
            {
                return !string.IsNullOrEmpty(contentType)
                       && contentType.IndexOf("multipart/", StringComparison.OrdinalIgnoreCase) >= 0;
            }

            public static bool HasFormDataContentDisposition(Microsoft.Net.Http.Headers.ContentDispositionHeaderValue contentDisposition)
            {
                // Content-Disposition: form-data; name="key";
                return contentDisposition != null
                    && contentDisposition.DispositionType.Equals("form-data")
                    && string.IsNullOrEmpty(contentDisposition.FileName.Value)
                    && string.IsNullOrEmpty(contentDisposition.FileNameStar.Value);
            }

            public static bool HasFileContentDisposition(Microsoft.Net.Http.Headers.ContentDispositionHeaderValue contentDisposition)
            {
                // Content-Disposition: form-data; name="myfile1"; filename="Misc 002.jpg"
                return contentDisposition != null
                    && contentDisposition.DispositionType.Equals("form-data")
                    && (!string.IsNullOrEmpty(contentDisposition.FileName.Value)
                        || !string.IsNullOrEmpty(contentDisposition.FileNameStar.Value));
            }
        }
    }
}
