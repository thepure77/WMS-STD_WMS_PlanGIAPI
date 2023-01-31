using System;
using System.Collections.Generic;
using System.Text;

namespace PlanGIBusiness
{
    class logtxt
    {
        public void loggingErr(String Folder, String msg)
        {
            var PathLog = System.IO.Directory.GetCurrentDirectory();
            if (Folder != "")
            {
                PathLog += "\\log_Err" + Folder + "\\";
            }
            else
            {
                PathLog += "\\NoFolder\\";
            }

            try
            {


                //var path = Directory.GetCurrentDirectory();
                //path += "\\" + floder.ToString() + "\\";

                if (!System.IO.Directory.Exists(PathLog))
                {
                    System.IO.Directory.CreateDirectory(PathLog);

                }
                String FileName = "log_Err" + DateTime.Now.ToString("yyyy-MM-dd-HHmmss") + ".txt";

                String logmsg = DateTime.Now.ToString("yyyy-MM-dd-HHmmss ") + " -- " + msg + Environment.NewLine;


                System.IO.File.AppendAllText(PathLog + FileName, logmsg);

            }
            catch (Exception ex)
            {
                //  lblerr.Text = ex.Message;
            }
            finally
            {

            }
        }
        public void logging(String Folder, String msg)
        {
            var PathLog = System.IO.Directory.GetCurrentDirectory();
            if (Folder != "")
            {
                PathLog += "\\log_" + Folder + "\\";
            }
            else
            {
                PathLog += "\\NoFolder\\";
            }

            try
            {


                //var path = Directory.GetCurrentDirectory();
                //path += "\\" + floder.ToString() + "\\";

                if (!System.IO.Directory.Exists(PathLog))
                {
                    System.IO.Directory.CreateDirectory(PathLog);

                }
                String FileName = "log_" + DateTime.Now.ToString("yyyy-MM-dd") + ".txt";

                String logmsg = DateTime.Now.ToString("yyyy-MM-dd-HHmmss ") + " -- " + msg + Environment.NewLine;


                System.IO.File.AppendAllText(PathLog + FileName, logmsg);

            }
            catch (Exception ex)
            {
                //  lblerr.Text = ex.Message;
            }
            finally
            {

            }
        }

    }
}
