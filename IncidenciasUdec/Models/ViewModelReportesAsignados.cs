using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IncidenciasUdec.Models
{
    public class ViewModelReportesAsignados
    {
        public int IdReporte { get; set; }
        public string NombreReporte { get; set; }
        public string NombreColaboradorAsignado { get; set; }
        public string Estado { get; set; }

    }
}