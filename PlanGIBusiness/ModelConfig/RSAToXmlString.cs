using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace APIJWTAndRSA.Class
{
    public class RSAToXmlString
    {
        public string ToXmlString(RSACryptoServiceProvider rsa, bool bSta)
        {
            try
            {
                RSAParameters parameters = rsa.ExportParameters(bSta);
                if (bSta)
                {
                    return string.Format("<RSAKeyValue><Modulus>{0}</Modulus><Exponent>{1}</Exponent><P>{2}</P><Q>{3}</Q><DP>{4}</DP><DQ>{5}</DQ><InverseQ>{6}</InverseQ><D>{7}</D></RSAKeyValue>",
                           Convert.ToBase64String(parameters.Modulus),
                           Convert.ToBase64String(parameters.Exponent),
                           Convert.ToBase64String(parameters.P),
                           Convert.ToBase64String(parameters.Q),
                           Convert.ToBase64String(parameters.DP),
                           Convert.ToBase64String(parameters.DQ),
                           Convert.ToBase64String(parameters.InverseQ),
                           Convert.ToBase64String(parameters.D));
                }
                else
                {
                    return string.Format("<RSAKeyValue><Modulus>{0}</Modulus><Exponent>{1}</Exponent></RSAKeyValue>",
                            Convert.ToBase64String(parameters.Modulus),
                            Convert.ToBase64String(parameters.Exponent));
                }
            }
            catch (Exception oEx)
            {
                return "";
            }
        }


        public string GetKeyFromEncryptionString(string rawkey, ref int keySize)
        {
            keySize = 0;
            string xmlKey = "";

            if (rawkey != null && rawkey.Length > 0)
            {
                byte[] keyBytes = Convert.FromBase64String(rawkey);
                var stringKey = Encoding.UTF8.GetString(keyBytes);

                if (stringKey.Contains("!"))
                {
                    var splittedValues = stringKey.Split(new char[] { '!' }, 2);

                    try
                    {
                        keySize = int.Parse(splittedValues[0]);
                        xmlKey = splittedValues[1];
                    }
                    catch (Exception e) { }
                }
                else {
                    xmlKey = stringKey;
                }
            }
            return xmlKey;
        }


        public static void FromXmlString(ref RSACryptoServiceProvider rsa, string xmlString)
        {
            RSAParameters parameters = new RSAParameters();

            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(xmlString);

            if (xmlDoc.DocumentElement.Name.Equals("RSAKeyValue"))
            {
                foreach (XmlNode node in xmlDoc.DocumentElement.ChildNodes)
                {
                    switch (node.Name)
                    {
                        case "Modulus": parameters.Modulus = Convert.FromBase64String(node.InnerText); break;
                        case "Exponent": parameters.Exponent = Convert.FromBase64String(node.InnerText); break;
                        case "P": parameters.P = Convert.FromBase64String(node.InnerText); break;
                        case "Q": parameters.Q = Convert.FromBase64String(node.InnerText); break;
                        case "DP": parameters.DP = Convert.FromBase64String(node.InnerText); break;
                        case "DQ": parameters.DQ = Convert.FromBase64String(node.InnerText); break;
                        case "InverseQ": parameters.InverseQ = Convert.FromBase64String(node.InnerText); break;
                        case "D": parameters.D = Convert.FromBase64String(node.InnerText); break;
                    }
                }
            }
            else
            {
                throw new Exception("Invalid XML RSA key.");
            }

            rsa.ImportParameters(parameters);
        }

        public static string getPublicTagXml() {

            StringBuilder Key = new StringBuilder();

            Key.AppendLine("<RSAKeyValue>");
            Key.AppendLine("<Modulus>ss/ZltmaO1kdUXLz4ut106So1vfdyAgxS58LtDZf6C9tBos1zguMjBVgpttWrQPFDR9nDx9b1g9luYOgjHskwHREudXggFPmGJa5j6JyF3E8UT5/ISJk5X8Q9TerUUY1qvOM8LEOFUZAyhvFCqxdQmi5VSPmKSx");
            Key.AppendLine("S5Hg/v6T/zZhUNbnkMAhJzu1mevuUmoRkX+24zx7WMecURj5te6F4RDa8upVsHkfM78wed8DdjTxRo9t0A6AeMMzzVkyHwLa+qbLzNsYGZCD07nLM9CS30ZDEDxxZwedQKrI/eDPaLnyDRc5gsAzD6lDAxf+RyxpehTFJcTiKJYGluD3istrePQ==.</Modulus>");
            Key.AppendLine("<Exponent>AQAB</Exponent>");
            Key.AppendLine("</RSAKeyValue>");



            return Key.ToString();
        }
    }
}
