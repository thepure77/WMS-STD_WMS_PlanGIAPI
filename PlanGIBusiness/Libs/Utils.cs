using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace PlanGIBusiness.Libs
{
    public class Utils
    {
        public string saveReport(byte[] file, string name, string rootPath)
        {
            var saveLocation = PhysicalPath(name, rootPath);
            FileStream fs = new FileStream(saveLocation, FileMode.Create);
            BinaryWriter bw = new BinaryWriter(fs);
            try
            {
                try
                {
                    bw.Write(file);
                }
                finally
                {
                    fs.Close();
                    bw.Close();
                }
            }
            catch (Exception ex)
            {
            }
            return VirtualPath(name);
            //throw new NotImplementedException();
        }

        private string ReportPath
        {
            get
            {
                //var url = System.Configuration.ConfigurationManager.AppSettings["SERVICE-REPORT"].ToString();
                var url = "\\ReportGenerator\\";
                return url;
            }
        }

        public string VirtualPath(string name)
        {
            var filename = name;
            var vPath = ReportPath;
            vPath = vPath.Replace("~", "");
            return vPath + filename;
        }

        public string PhysicalPath(string name, string rootPath)
        {

            //rootPath = rootPath.Replace("\\TOP_ReportAPI", "\\TOP_WMS");
            //rootPath = rootPath.Replace("\\ReportAPI", "\\TOP_WMS");
            var filename = name;
            var vPath = ReportPath;
            //string path = @"C:\tmp\ReportGenerator\";

            //var path = HttpContext.Current.Server.MapPath(vPath);
            var path = rootPath + vPath;
            if (!System.IO.Directory.Exists(path))
            {
                System.IO.Directory.CreateDirectory(path);
            }

            var saveLocation = System.IO.Path.Combine(path, filename);
            return saveLocation;
        }
    }
}
