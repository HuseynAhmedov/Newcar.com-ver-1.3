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
    public partial class Form1 : Form
    {
        SqlUtils sqlUtils = new SqlUtils();
        SqlDataGet DataSource = new SqlDataGet();

        string sql_query = "";
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

            SearchEngineSql();
            GetBrandData();
        }

        private void btnSell_Click(object sender, EventArgs e)
        {
            AddCar addCar = new AddCar(this);
            addCar.ShowDialog();
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            SearchEngineSql();
        }

        private void GetBrandData()
        {
            sql_query = @"SELECT ID, Brand from BrandsData ";
            BoxEdit_Brand.Properties.DataSource = DataSource.Getwithquery(sql_query);
            BoxEdit_Brand.Properties.DisplayMember = "Brand";
            BoxEdit_Brand.Properties.ValueMember = "ID";

        }
        private void btnSearch_Click(object sender, EventArgs e)
        {
            SearchEngineSql();
        }
        public void SearchEngineSql ()
        {
            int on_loan = 0;
            int Exchange = 0;
            if ((BoxEdit_Brand.Text.Trim() == "All brands" || BoxEdit_Brand.Text == "") && (BoxEdit_Model.Text.Trim() == "All models" || BoxEdit_Model.Text == "") && textEdit_price_min.EditValue == null && textEdit_Price_max.EditValue == null && textEdit_Year_min.EditValue == null && textEdit_Year_max.EditValue == null && on_loan == 0 && Exchange == 0 && BoxEdit_Currency.Text == "All currencies")
            {
                
                sql_query = $"SELECT ID , (TRIM(Brand)+ ' ' + TRIM(Model) ) as FullName , Color ,  (CAST(Price as varchar(20)) + ' ' +  Currency ) as FullPrice  , CAST(ReleseYear as varchar(20)) as NewReleseYear , (SELECT TOP(1) Car_image from CarImageData IMG where Owner_ID = IMG.Car_ID ) as Car_image from CarData";
            }
            else
            {
                sql_query = $"SELECT ID , (TRIM(Brand)+ ' ' + TRIM(Model) ) as FullName , Color ,  (CAST(Price as varchar(20)) + ' ' +  Currency ) as FullPrice  , CAST(ReleseYear as varchar(20)) as NewReleseYear , (SELECT TOP(1) Car_image from CarImageData IMG where Owner_ID = IMG.Car_ID ) as Car_image from CarData where  ";
                if (BoxEdit_Currency.Text == "All currencies")
                {
                    sql_query = sql_query + $"(Currency = 'AZN' or Currency = 'RUB' or Currency = 'USD')";
                }
                else
                {
                    sql_query = sql_query + $" Currency = '{BoxEdit_Currency.Text}'";
                }
                if (BoxEdit_Brand.EditValue != null && BoxEdit_Brand.Text.Trim() != "All brands")
                {
                    sql_query = sql_query + $" AND Brand = '{BoxEdit_Brand.Text.Trim()}' ";
                }
                if (BoxEdit_Model.EditValue != null && BoxEdit_Model.Text != "" && BoxEdit_Model.Text.Trim() != "All models")
                {
                    sql_query = sql_query + $" And Model = '{BoxEdit_Model.Text.Trim()}' ";
                }
                if (textEdit_price_min.EditValue != null && textEdit_price_min.Text != "")
                {
                    sql_query = sql_query + $" AND Price > {textEdit_price_min.EditValue} ";
                }
                if (textEdit_Price_max.EditValue != null && textEdit_Price_max.Text != "")
                {
                    sql_query = sql_query + $" AND Price < {textEdit_Price_max.EditValue} ";
                }
                if (textEdit_Year_min.EditValue != null && textEdit_Year_min.Text != "")
                {
                    sql_query = sql_query + $" AND ReleseYear > {textEdit_Year_min.EditValue} ";
                }
                if (textEdit_Year_max.EditValue != null && textEdit_Year_max.Text != "" )
                {
                    sql_query = sql_query + $" AND ReleseYear < {textEdit_Year_max.EditValue}";
                }
                if (BoxEdit_2Cheack.Properties.GetItems().GetCheckedValues().Contains(item: "All cars") == false)
                {
                    if (BoxEdit_2Cheack.Properties.GetItems().GetCheckedValues().Contains(item: "On loan"))
                    {
                        on_loan = 1;
                        sql_query = sql_query + $" and Onloan = { on_loan } ";
                    }
                    if (BoxEdit_2Cheack.Properties.GetItems().GetCheckedValues().Contains(item: "Avalible for exchange"))
                    {
                        Exchange = 1;
                        sql_query = sql_query + $" and Exchange = { Exchange } ";
                    }
                }
            }

            grdControlHome.DataSource = DataSource.Getwithquery(sql_query);
           
        }

        private void BoxEdit_Brand_EditValueChanged(object sender, EventArgs e)
        {
            string Brand_ID = BoxEdit_Brand.EditValue.ToString();
            BoxEdit_Model.Properties.DataSource = DataSource.GetModelBrands(Brand_ID);
            BoxEdit_Model.Properties.DisplayMember = "BrandModel";
            BoxEdit_Model.Properties.ValueMember = "ID";
        }
    }
}
