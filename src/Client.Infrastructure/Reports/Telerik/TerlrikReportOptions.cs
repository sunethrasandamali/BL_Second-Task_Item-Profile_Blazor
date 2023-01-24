using BlueLotus360.CleanArchitecture.Client.Infrastructure.Routes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlueLotus360.CleanArchitecture.Client.Infrastructure.Reports.Telerik
{
    public  class TerlrikReportOptions
    {
        public string ReportName { get; set; }

       

        public string ReportRestServicePath { get;protected set; }

        public IDictionary<string, object> ReportParameters { get; set; }

        public TerlrikReportOptions()
        {
            this.ReportRestServicePath = BaseEndpoint.ReportURL;
        }
    }
}
