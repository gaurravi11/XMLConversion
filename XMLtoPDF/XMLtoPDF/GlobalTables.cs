using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace XMLtoPDF
{
    class GlobalTables
    {

        public static DataTable Admin { get; set; }
        public static DataTable County { get; set; }
        public static DataTable Dictionary { get; set; }
        public static DataTable Locality { get; set; }
        public static DataTable RightType { get; set; }
        public static DataTable Validari_Fluxuri { get; set; }
        public static DataTable Validari { get; set; }
        public static DataTable Differente_Suprafete { get; set; }
    }
}
