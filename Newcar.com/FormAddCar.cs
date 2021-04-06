using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Newcar.com
{
    public partial class AddCar : Form
    {
        SqlDataGet DataSource = new SqlDataGet();
        SqlConnection sqlconn = new SqlConnection(SqlUtils.conn_string);
        OpenFileDialog openFileDialog = new OpenFileDialog();

        bool insert_fail = false;

        int sellerID;

        string sql_query = "";
        public AddCar(Form1 MainForm)
        {
            InitializeComponent();
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            GetBrandData();
            GetCityData();
            GetEngineSize();
            GetReleseYear();
            gridControlPicture.DataSource = DataSource.GetImage(-1);
        }

        private void GetBrandData ()
        {
            sql_query = @"SELECT ID , Brand from BrandsData ";
            BoxEdit_Brand.Properties.DataSource = DataSource.Getwithquery(sql_query);
            BoxEdit_Brand.Properties.DisplayMember = "Brand";
            BoxEdit_Brand.Properties.ValueMember = "ID";
            
        }

        private void GetCityData ()
        {
            sql_query = @"SELECT ID , City from CityNameData;";
            BoxEdit_City.Properties.DataSource = DataSource.Getwithquery(sql_query);
            BoxEdit_City.Properties.DisplayMember = "City";
            BoxEdit_City.Properties.ValueMember = "ID";
        }

        private void BoxEdit_Brand_EditValueChanged(object sender, EventArgs e)
        {
            if (BoxEdit_Brand.EditValue != null)
            {
                string Brand_ID = BoxEdit_Brand.EditValue.ToString();
                BoxEdit_Model.Properties.DataSource = DataSource.GetModelBrands(Brand_ID);
                BoxEdit_Model.Properties.DisplayMember = "BrandModel";
                BoxEdit_Model.Properties.ValueMember = "ID";
            }
            
        }
        private void GetEngineSize()
        {
            sql_query = @"SELECT ID , EngineSize from EngineSizeData";
            BoxEdit_EngineSize.Properties.DataSource = DataSource.Getwithquery(sql_query);
            BoxEdit_EngineSize.Properties.DisplayMember = "EngineSize";
            BoxEdit_EngineSize.Properties.ValueMember = "EngineSize";
        }

        private void GetReleseYear ()
        {
            sql_query = @"SELECT ID , RYear from ReleseYearData";
            BoxEdit_Year.Properties.DataSource = DataSource.Getwithquery(sql_query);
            BoxEdit_Year.Properties.DisplayMember = "RYear";
            BoxEdit_Year.Properties.ValueMember = "RYear";
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            
            bool go = CheakupInsert();
            if (go == true)
            {
                sqlconn.Open();
                sellerID = InsertSql();
                InsertIamge(sellerID);

                if (insert_fail == true )
                {

                    DeleteFromSql();
                    MessageBox.Show("Error While inserting ");
                    sqlconn.Close();
                }
                sqlconn.Close();
                MessageBox.Show("Data added succsesfully");
                this.Close();


            }
            
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private bool CheakupInsert ()
        {
            bool allgood = true;
            if (textEditSellerName.Text == "")
            {
                textEditSellerName.ErrorText = " ";
                allgood = false;
            }
            if (textEditNumber.Text == "+994" || textEditNumber.Text == "")
            {
                textEditNumber.ErrorText = " ";
                allgood = false;
            }
            if (textEditEmail.Text == "")
            {
                textEditEmail.ErrorText = " ";
                allgood = false;
            }
            if (BoxEdit_City.Text == "")
            {
                BoxEdit_City.ErrorText = " ";
                allgood = false;
            }
            if (BoxEdit_Brand.Text == "" || BoxEdit_Brand.Text == "Select")
            {
                BoxEdit_Brand.ErrorText = " ";
                allgood = false;
            }
            if (BoxEdit_Model.Text == "" || BoxEdit_Model.Text == "Select")
            {
                BoxEdit_Model.ErrorText = " ";
                allgood = false;
            }
            if (BoxEdit_Body.Text == "")
            {
                BoxEdit_Body.ErrorText = " ";
                allgood = false;
            }
            if (textEditMileage.Text == "")
            {
                textEditMileage.ErrorText = " ";
                allgood = false;
            }
            if (BoxEdit_Color.Text == "")
            {
                BoxEdit_Color.ErrorText = " ";
                allgood = false;
            }
            if (textEditPrice.Text == "")
            {
                textEditPrice.ErrorText = " ";
                allgood = false;
            }
            if (BoxEdit_Fuel.Text == "")
            {
                BoxEdit_Fuel.ErrorText = " ";
                allgood = false;
            }
            if (BoxEdit_Drivetrain.Text == "")
            {
                BoxEdit_Drivetrain.ErrorText = " ";
                allgood = false;
            }
            if (BoxEdit_Gearbox.Text == "")
            {
                BoxEdit_Gearbox.ErrorText = " ";
                allgood = false;
            }
            if (BoxEdit_Year.Text == "")
            {
                BoxEdit_Year.ErrorText = " ";
                allgood = false;
            }
            if (BoxEdit_EngineSize.Text == "")
            {
                BoxEdit_EngineSize.ErrorText = " ";
                allgood = false;
            }
            if (textEdit_HorsePower.Text == "")
            {
                textEdit_HorsePower.ErrorText = " ";
                allgood = false;
            }
            return allgood;
        }

        private int InsertSql ()
        {

            try
            {
                sql_query = @"INSERT INTO [dbo].[SellerData]
                                ([Name]
                                ,[Number]
                                ,[Email]
                                ,[City]
                                ,[Seller_ID] 
                                )

                          VALUES
                                 (@Name , @Number , @Email , @City , IDENT_CURRENT('SellerData') )";
                SqlCommand sql_command = new SqlCommand(sql_query, sqlconn);
                sql_command.Parameters.Add("@Name", SqlDbType.NVarChar).Value = textEditSellerName.Text.Trim();
                sql_command.Parameters.Add("@Number", SqlDbType.NVarChar).Value = textEditNumber.Text.Trim();
                sql_command.Parameters.Add("@Email", SqlDbType.NVarChar).Value = textEditEmail.Text.Trim();
                sql_command.Parameters.Add("@City", SqlDbType.NVarChar).Value = BoxEdit_City.Text;
                sql_command.ExecuteNonQuery();
                sqlconn.Close();
                sqlconn.Open();
                sellerID = DataSource.GetSellerID();
                //
                //
                //
                sql_query = @"INSERT INTO [dbo].[CarData]
                          ([Brand]
                          ,[Model]
                          ,[Body]
                          ,[Mieage]
                          ,[Color]
                          ,[Price]
                          ,[Currency]
                          ,[FuelType]
                          ,[Drivetrain]
                          ,[Gearbox]
                          ,[ReleseYear]
                          ,[EngineSize]
                          ,[HorsePower]
                          ,[Onloan]
                          ,[Exchange]
                          ,[Description]
                          ,[Owner_ID])
                    VALUES
                         ( @Brand, @Model, @Body, @Mieage, @Color, @Price, @Currency, @FuelType
                         , @Drivetrain, @Gearbox, @ReleseYear, @EngineSize, @HorsePower, @Onloan
                         , @Exchange, @Description , @Owner_ID)";
                sql_command = new SqlCommand(sql_query, sqlconn);
                sql_command.Parameters.Add("@Brand", SqlDbType.NVarChar).Value = BoxEdit_Brand.Text;
                sql_command.Parameters.Add("@Model", SqlDbType.NVarChar).Value = BoxEdit_Model.Text;
                sql_command.Parameters.Add("@Body", SqlDbType.NVarChar).Value = BoxEdit_Body.Text;
                sql_command.Parameters.Add("@Mieage", SqlDbType.NVarChar).Value = textEditMileage.Text;
                sql_command.Parameters.Add("@Color", SqlDbType.NVarChar).Value = BoxEdit_Color.Text;
                sql_command.Parameters.Add("@Price", SqlDbType.Int).Value = textEditPrice.EditValue;
                sql_command.Parameters.Add("@Currency", SqlDbType.NVarChar).Value = BoxEdit_Currency.Text;
                sql_command.Parameters.Add("@FuelType", SqlDbType.NVarChar).Value = BoxEdit_Fuel.Text;
                sql_command.Parameters.Add("@Drivetrain", SqlDbType.NVarChar).Value = BoxEdit_Drivetrain.Text;
                sql_command.Parameters.Add("@Gearbox", SqlDbType.NVarChar).Value = BoxEdit_Gearbox.Text;
                sql_command.Parameters.Add("@ReleseYear", SqlDbType.Int).Value = BoxEdit_Year.EditValue;
                sql_command.Parameters.Add("@EngineSize", SqlDbType.Int).Value = BoxEdit_EngineSize.EditValue;
                sql_command.Parameters.Add("@HorsePower", SqlDbType.Int).Value = textEdit_HorsePower.EditValue;
                sql_command.Parameters.Add("@Onloan", SqlDbType.Bit).Value = checkEdit_OnLoan.EditValue;
                sql_command.Parameters.Add("@Exchange", SqlDbType.Bit).Value = checkEdit_Exchange.EditValue;
                sql_command.Parameters.Add("@Description", SqlDbType.NVarChar).Value = textEditDescription.Text;
                sql_command.Parameters.Add("@Owner_ID", SqlDbType.Int).Value = sellerID;
                sql_command.ExecuteNonQuery();
                //
                //
                //
                sql_query = @"INSERT INTO [dbo].[CarFeaturesData]
                         ([Car_ID]
                         ,[RainSensor]
                         ,[AirConditioners]
                         ,[BackviewCamera]
                         ,[SideCurtains]
                         ,[SeatHeater]
                         ,[Hatch]
                         ,[ABS]
                         ,[ParkingRadar]
                         ,[KsenonLights]
                         ,[LeatherSalon]
                         ,[LightWheels]
                         ,[InnerSecurity])
                         
                   VALUES
                         (@Car_ID , @RainSensor , @AirConditioners , @BackviewCamera , @SideCurtains , @SeatHeater , @Hatch , @ABS , 
                          @ParkingRadar, @KsenonLights , @LeatherSalon , @LightWheels , @InnerSecurity )";
                sql_command = new SqlCommand(sql_query, sqlconn);
                sql_command.Parameters.Add("@Car_ID", SqlDbType.Int).Value = sellerID;
                bool[] arr_Features = CheakFeatures();
                sql_command.Parameters.Add("@RainSensor", SqlDbType.Bit).Value = arr_Features[0];
                sql_command.Parameters.Add("@AirConditioners", SqlDbType.Bit).Value = arr_Features[1];
                sql_command.Parameters.Add("@BackviewCamera", SqlDbType.Bit).Value = arr_Features[2];
                sql_command.Parameters.Add("@SideCurtains", SqlDbType.Bit).Value = arr_Features[3];
                sql_command.Parameters.Add("@SeatHeater", SqlDbType.Bit).Value = arr_Features[4];
                sql_command.Parameters.Add("@Hatch", SqlDbType.Bit).Value = arr_Features[5];
                sql_command.Parameters.Add("@ABS", SqlDbType.Bit).Value = arr_Features[6];
                sql_command.Parameters.Add("@ParkingRadar", SqlDbType.Bit).Value = arr_Features[7];
                sql_command.Parameters.Add("@KsenonLights", SqlDbType.Bit).Value = arr_Features[8];
                sql_command.Parameters.Add("@LeatherSalon", SqlDbType.Bit).Value = arr_Features[9];
                sql_command.Parameters.Add("@LightWheels", SqlDbType.Bit).Value = arr_Features[10];
                sql_command.Parameters.Add("@InnerSecurity", SqlDbType.Bit).Value = arr_Features[11];
                sql_command.ExecuteNonQuery();
            }
            catch (Exception)
            {
                insert_fail = true;
            }
            ClearInput();
            return sellerID ;
        }

        private void ClearInput ()
        {
            textEditSellerName.Text = "";
            textEditNumber.Text = "";
            textEditEmail.Text = "";
            BoxEdit_City.EditValue = null;
            BoxEdit_Brand.EditValue = null;
            BoxEdit_Model.EditValue = null;
            BoxEdit_Body.Text = "";
            textEditMileage.Text = "";
            BoxEdit_Color.Text = "";
            textEditPrice.EditValue = null;
            BoxEdit_Currency.Text = "";
            BoxEdit_Fuel.Text = "";
            BoxEdit_Drivetrain.Text = "";
            BoxEdit_Gearbox.Text = "";
            BoxEdit_Year.EditValue = null;
            BoxEdit_EngineSize.EditValue = null;
            textEdit_HorsePower.EditValue = null;
            checkEdit_OnLoan.EditValue = null;
            checkEdit_Exchange.EditValue = null;
            textEditDescription.Text = "";

        }

        private bool[] CheakFeatures()
        {
            bool[] arr_Features = { false, false, false, false, false, false, false, false, false, false, false, false };
            if (textEdit_Features.Properties.GetItems().GetCheckedValues().Contains(item: "Rain sensor"))
                arr_Features[0] = true;

            if (textEdit_Features.Properties.GetItems().GetCheckedValues().Contains(item: "Air conditioners"))
                arr_Features[1] = true;

            if (textEdit_Features.Properties.GetItems().GetCheckedValues().Contains(item: "Back view camera"))
                arr_Features[2] = true;

            if (textEdit_Features.Properties.GetItems().GetCheckedValues().Contains(item: "Side curtains"))
                arr_Features[3] = true;

            if (textEdit_Features.Properties.GetItems().GetCheckedValues().Contains(item: "Seat heater"))
                arr_Features[4] = true;

            if (textEdit_Features.Properties.GetItems().GetCheckedValues().Contains(item: "Hatch"))
                arr_Features[5] = true;

            if (textEdit_Features.Properties.GetItems().GetCheckedValues().Contains(item: "ABS"))
                arr_Features[6] = true;

            if (textEdit_Features.Properties.GetItems().GetCheckedValues().Contains(item: "Parking radar"))
                arr_Features[7] = true;

            if (textEdit_Features.Properties.GetItems().GetCheckedValues().Contains(item: "Ksenon lights"))
                arr_Features[8] = true;

            if (textEdit_Features.Properties.GetItems().GetCheckedValues().Contains(item: "Leather salon"))
                arr_Features[9] = true;

            if (textEdit_Features.Properties.GetItems().GetCheckedValues().Contains(item: "Light wheels"))
                arr_Features[10] = true;

            if (textEdit_Features.Properties.GetItems().GetCheckedValues().Contains(item: "Inner security"))
                arr_Features[11] = true;

            return arr_Features;
        }

        private void DeleteFromSql ()
        {
           
           sql_query = $"delete from SellerData where ID = {sellerID}";
           SqlCommand sql_command4 = new SqlCommand(sql_query, sqlconn);
           sql_command4.ExecuteNonQuery();

                            
           sql_query = $"delete from CarData where ID = {sellerID} ";
           sql_command4 = new SqlCommand(sql_query, sqlconn);
           sql_command4.ExecuteNonQuery();

           sql_query = $"delete from CarFeaturesData where ID = {sellerID} ";
           sql_command4 = new SqlCommand(sql_query, sqlconn);
           sql_command4.ExecuteNonQuery();

            
        }

        private byte[] GetByteImage(string fileName)
        {
            byte[] imgByteArray = null;
            FileStream fileStream = new FileStream(fileName, FileMode.Open, FileAccess.Read);
            BinaryReader binaryReader = new BinaryReader(fileStream);
            imgByteArray = binaryReader.ReadBytes((int)fileStream.Length);
            binaryReader.Close();
            fileStream.Close();
            return imgByteArray;
        }

        private void btnAddPicture_Click(object sender, EventArgs e)
        {
            openFileDialog.Filter = "Image files | *.jpg;*.jpeg;*.png;";
            openFileDialog.Multiselect = true;
            DataTable dataTable = (DataTable)gridControlPicture.DataSource;
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                foreach (string fileName in openFileDialog.FileNames)
                {
                    dataTable.Rows.Add(0, GetByteImage(fileName));
                    GetByteImage(fileName);
                }
            }
        }



        private void InsertIamge(int sellerID)
        {
            DataTable dataTableImages = (DataTable)gridControlPicture.DataSource;
            for (int i = 0; i < dataTableImages.Rows.Count; i++)
            {
                DataRow dtRowImage = dataTableImages.Rows[i];
                sql_query = @"INSERT INTO [dbo].[CarImageData]
                             ([Car_image]
                             ,[Car_ID])
                      VALUES
                           (@Car_image , @Car_ID)";
                SqlCommand sql_command = new SqlCommand(sql_query, sqlconn);
                sql_command.Parameters.Add("@Car_image", SqlDbType.VarBinary).Value = dtRowImage["Car_image"];
                sql_command.Parameters.Add("@Car_ID", SqlDbType.Int).Value = sellerID;
                sql_command.ExecuteNonQuery();

            }

        }

        private void btnClearPicture_Click(object sender, EventArgs e)
        {

        }
    }
}
