using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Newcar.com
{
    class SqlDataGet : SqlUtils
    {
        
        public DataTable GetModelBrands(string Brand_ID)
        {
            string query = $"SELECT ID , Brand_ID , BrandModel from BrandModel where Brand_ID = {Brand_ID}";
            SqlConnection Sqlconn = new SqlConnection(SqlUtils.conn_string);
            SqlDataAdapter dataAdapter = new SqlDataAdapter(query, Sqlconn);
            DataTable dtTable = new DataTable();
            dataAdapter.Fill(dtTable);
            return dtTable;
        }
        public DataTable GetImage (int sellerID)
        {
            string query = $@"SELECT ID , Car_image , Car_ID from CarImageData where Car_ID = {sellerID}";
            SqlConnection Sqlconn = new SqlConnection(SqlUtils.conn_string);
            SqlDataAdapter dataAdapter = new SqlDataAdapter(query, Sqlconn);
            DataTable dtTable = new DataTable();
            dataAdapter.Fill(dtTable);
            return dtTable;
        }

        public int  GetSellerID ()
        {
            int seller_ID = 0;
            string query = @"select * from SellerData where ID = IDENT_CURRENT('SellerData') ";
            SqlConnection Sqlconn = new SqlConnection(SqlUtils.conn_string);
            SqlDataAdapter dataAdapter = new SqlDataAdapter(query, Sqlconn);
            DataTable dtTable = new DataTable();
            dataAdapter.Fill(dtTable);
            seller_ID = (int)dtTable.Rows[0]["Seller_ID"];
            return seller_ID ;
        }

        public DataTable Getwithquery (string query)
        {
            SqlConnection Sqlconn = new SqlConnection(SqlUtils.conn_string);
            SqlDataAdapter dataAdapter = new SqlDataAdapter(query, Sqlconn);
            DataTable dtTable = new DataTable();
            dataAdapter.Fill(dtTable);
            return dtTable;
        }
    }

}
