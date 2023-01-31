using System;
using System.Net.Http.Headers;
using System.Drawing;
using System.IO;
using Excel;
using System.Data;
using Microsoft.Win32;

namespace PTTPL.OMS.Business.Documents
{
    public static class fileAppService {
        public static bool setThumbnail(string fullPath, string desternation) {
            try {

                var image = Image.FromFile(fullPath);
                var newImage = ScaleImage(image, 100, 100);
                newImage.Save(desternation);
                return true;
            } catch (Exception) {
                return false;
            }
        }

        public static Image ScaleImage(Image image, int maxWidth, int maxHeight) {
            var ratioX = (double)maxWidth / image.Width;
            var ratioY = (double)maxHeight / image.Height;
            var ratio = Math.Min(ratioX, ratioY);

            var newWidth = (int)(image.Width * ratio);
            var newHeight = (int)(image.Height * ratio);

            var newImage = new Bitmap(newWidth, newHeight);
            Graphics.FromImage(newImage).DrawImage(image, 0, 0, newWidth, newHeight);
            return newImage;
        }

        //public static fileViewModel getFile(string VirtualPath) {
        //    fileViewModel file = new fileViewModel();

        //    string path = HttpContext.Current.Server.MapPath(VirtualPath);
        //    if (File.Exists(path)) {
        //        file.name = Path.GetFileName(path);
        //        file.extension = Path.GetExtension(path);
        //        file.directory = Path.GetDirectoryName(path);
        //        file.path = path;
        //        file.virtualPath = VirtualPath;

        //        file.thumb = file.virtualPath + "/Thumb/" + file.name;
        //        if (!Exists(file.thumb))
        //            file.thumb = "";
        //    }

        //    return file;
        //}

        //public static bool Exists(string VirtualPath) {
        //    string path = HttpContext.Current.Server.MapPath(VirtualPath);
        //    if (File.Exists(path)) {
        //        return true;
        //    }
        //    return false;
        //}

        public static string getExtension(string ContentType) {
            string extension = "";
            string contentType = ContentType;

            switch (contentType.ToLower()) {
                case "image/gif":
                    extension = ".gif";
                    break;
                case "image/jpeg":
                    extension = ".jpeg";
                    break;
                case "image/pjpeg":
                    extension = ".jpeg";
                    break;
                case "image/bmp":
                    extension = ".bmp";
                    break;
                case "image/png":
                    extension = ".png";
                    break;
                case "image/svg+xml":
                    extension = ".svg";
                    break;
                case "image/tiff":
                    extension = ".fif";
                    break;
                case "application/msword":
                    extension = ".doc";
                    break;
                case "application/vnd.openxmlformats-officedocument.wordprocessingml.document":
                    extension = ".docx";
                    break;
                case "application/vnd.ms-excel":
                    extension = ".xls";
                    break;
                case "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet":
                    extension = ".xlsx";
                    break;
                case "application/pdf":
                    extension = ".pdf";
                    break;
                default:
                    RegistryKey key;
                    object value;
                    key = Registry.ClassesRoot.OpenSubKey(@"MIME\Database\Content Type\" + contentType, false);
                    value = key != null ? key.GetValue("Extension", null) : null;
                    extension = value != null ? value.ToString() : string.Empty;

                    break;
            }

            return extension;
        }

        public static bool getTypeImage(string ContentType) {
            bool extension = false;
            string contentType = ContentType;

            switch (contentType.ToLower()) {
                case "image/gif":
                case "image/jpeg":
                case "image/pjpeg":
                case "image/bmp":
                case "image/png":
                case "image/svg+xml":
                case "image/tiff":
                    extension = true;
                    break;
                default:
                    extension = false;
                    break;
            }

            return extension;
        }

        //public static bool deleteFile(string path) {
        //    try {
        //        path = HttpContext.Current.Server.MapPath("~" + path);
        //        if (System.IO.File.Exists(path)) {
        //            System.IO.File.Delete(path);
        //        }
        //    } catch (Exception) {

        //        throw;
        //    }


        //    return true;
        //}

        // Test Read Excel files
        public static DataSet readExcelFIle(string path)
        {
            bool checkFileExist = IsFileLocked(path);

            DataSet ds = new DataSet();
   

                var file = new FileInfo(path);

                using (var stream = new FileStream(path, FileMode.Open))
                {
                    IExcelDataReader reader = null;
                    if (file.Extension == ".xls")
                    {
                        reader = ExcelReaderFactory.CreateBinaryReader(stream);

                    }
                    else if (file.Extension == ".xlsx")
                    {
                        reader = ExcelReaderFactory.CreateOpenXmlReader(stream);
                    }

                    if (reader == null)
                        return ds;
                    ds = reader.AsDataSet();

                    return ds;
                }
            
        }


        // Check file Exist
        private static bool IsFileLocked(string file)
        {
            try
            {
                using (var inputStream = File.Open(file, FileMode.Open, FileAccess.Read, FileShare.None))
                {
                    return true;
                }
            }
            catch (IOException)
            {
                return false;
            }
        }

    }


    public class fileViewModel {
        public string name { get; set; }
        public string extension { get; set; }
        public string path { get; set; }
        public string virtualPath { get; set; }
        public string thumb { get; set; }
        public string directory { get; set; }
        public string orginal { get; set; }
        public string fileType { get; set; }
    }
    
}
