using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.Windows.Forms;

namespace Newcar.com
{
    public class SqlUtils
    {                                          
        public static string conn_string { get; set; }
        private static SqlUtils sqlUtils;

        public SqlUtils()
        {
            conn_string = ConfigurationManager.ConnectionStrings["MainConString"].ConnectionString;
        }
        public static SqlUtils GetInstance()
        {
            if (sqlUtils==null)
            {
                sqlUtils = new SqlUtils();

            }
            return sqlUtils;
        }

    }

}
