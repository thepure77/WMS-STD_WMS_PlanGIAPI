using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace PTTPL.TMS.Business.ViewModels
{
    [Serializable]
    [DataContract]
    public class resultViewModel
    {
        [DataMember]
        public bool result { get; set; }
        [DataMember]
        public string msg { get; set; }
        [DataMember]
        public string value { get; set; }
        [DataMember]
        public string url { get; set; }
        [DataMember]
        public int id { get; set; }
        [DataMember]
        public string uploadDate { get; set; }
    }

    [Serializable]
    [DataContract]
    public class resultSOCINoViewModel
    {
        [DataMember]
        public string orderNo { get; set; }
    }

    [Serializable]
    [DataContract]
    public class resultColoadNoViewModel
    {
        [DataMember]
        public string orderNo { get; set; }
    }

    [Serializable]
    [DataContract]
    public class resultRfidViewModel
    {
        [DataMember]
        public string result { get; set; }
        [DataMember]
        public string msg { get; set; }
    }

    public class Engineer
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string FilePath { get; set; }
    }
    public class EngineerVM
    {
        public string Name { get; set; }
        public IFormFile File { get; set; }
    }
}
