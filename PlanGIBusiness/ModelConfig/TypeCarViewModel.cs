using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MasterDataBusiness.ViewModels
{


    public  class TypeCarViewModel
    {

        public Guid? typeCar_Index { get; set; }
        public string typeCar_Id { get; set; }
        public string typeCar_Name { get; set; }
    }
}
