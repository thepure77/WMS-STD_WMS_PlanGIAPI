using System;
using System.Collections.Generic;
using System.Text;

namespace PlanGIBusiness
{
    public class Pagination
    {
        public int CurrentPage { get; set; }

        public int PerPage { get; set; }

        public int TotalRow { get; set; }

        public string Key { get; set; }

        public bool AdvanceSearch { get; set; }
        public int NumPerPage { get; set; }

    }
}
