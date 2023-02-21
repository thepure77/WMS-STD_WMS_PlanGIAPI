using System;
using System.Collections.Generic;
using System.Text;

namespace PlanGIBusiness.Demo
{
    public class DemoCallbackViewModel
    {
        public string referenceNo { get; set; }
        public string status { get; set; }
        public string statusAfter { get; set; }
        public string statusBefore { get; set; }
        public string statusDesc { get; set; }
        public string statusDateTime { get; set; }
    }

    public class DemoCallbackResponseViewModel
    {
        public string status { get; set; }
        public string message { get; set; }
        public DemoCallbackResponseItemViewModel data { get; set; }
    }

    public class DemoCallbackResponseItemViewModel
    {
        public string logId { get; set; }
    }
}
