using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MasterDataBusiness.ViewModels
{


    public  class TransportViewModel
    {

        public Guid? transport_Index { get; set; }
        public string transport_Id { get; set; }
        public string transport_Name { get; set; }
    }
}
