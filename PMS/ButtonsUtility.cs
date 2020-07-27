using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Windows.Forms;

namespace PMS
{
    static class ButtonsUtility
    {
        public static String cs = ConfigurationManager.ConnectionStrings["PMSDB"].ConnectionString;

        public static SqlConnection con = new SqlConnection(cs);

        public static string UserName;

        public static string UserType;

        public static int UserId;

        public static int NoOfDaysUserId;

        public static DashBoardAdmin DashBoard;
        public static bool connectionIndicator;
        public static void ExitProgram() //will be used for Exit Button to Close the Application
        {
            DialogResult dialog = MessageBox.Show("Are you sure you want to Exit the application?", "Exit", MessageBoxButtons.YesNo,MessageBoxIcon.Warning);
            if (dialog == DialogResult.Yes)
            {
                System.Windows.Forms.Application.Exit();
            }

        }

        public static void NavigateToLogin() //will Navigate to login screen by logging out the user.
        {
            DialogResult dialog = MessageBox.Show("Are you sure you want to Logout?", "Logout", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (dialog == DialogResult.Yes)
            {
                LoginForm Login_Form = new LoginForm();
                Login_Form.Show();
            }
        }
        public static void Login(string User_Name, string Password, LoginForm login)
        {

            try
            {
                LoginForm Login_Form = new LoginForm();
                con.Open();
                SqlCommand cmd = new SqlCommand("Usp_LoginUser", con);
                cmd.CommandType = CommandType.StoredProcedure;
                DataTable dt = new DataTable();
                connectionIndicator = true;

                cmd.Parameters.AddWithValue("@UserName", SqlDbType.VarChar).Value = User_Name;
                cmd.Parameters.AddWithValue("@Password", SqlDbType.VarChar).Value = Password;
                dt.Load(cmd.ExecuteReader());

                UserType = dt.Rows[0][0].ToString();


                if (UserType != "0")
                {
                    UserName = dt.Rows[0][1].ToString();
                    UserId = Convert.ToInt32(dt.Rows[0][2]);
                   

                    DashBoard = new DashBoardAdmin(UserType, UserName,0);  //ItemId parameter will be zero because it is logn

                    DashBoard.Show();

                    //Login_Form.SuccessfulLogin();
                    //  login.SuccessfulLogin();

                }
                else
                {
                    login.LoginFailed();



                }

            }
            catch (Exception exp)
            {
                throw exp;
            }
            finally
            {
                con.Close();
                connectionIndicator = false;

            }

        }
        public static void Menu(int ItemId)
        {

            DashBoard = new DashBoardAdmin(UserType, UserName,ItemId);
            DashBoard.Show();

        }

        public static int EditItemValues(String NameOfItem, String ItemDescription, String ItemCode,  String UOM, string ItemPreviousCode, decimal cost, decimal ReworkCost, decimal InhouseRejCost)
        {
            try
            {

                con.Open();
                SqlCommand cmd = new SqlCommand("Usp_UpdateItemValue", con);
                cmd.CommandType = CommandType.StoredProcedure;



                cmd.Parameters.AddWithValue("@NameOfItem", SqlDbType.VarChar).Value = NameOfItem;
                cmd.Parameters.AddWithValue("@ItemDescription", SqlDbType.VarChar).Value = ItemDescription;
                cmd.Parameters.AddWithValue("@CodeOfItem", SqlDbType.VarChar).Value = ItemCode;
                cmd.Parameters.AddWithValue("@ItemUnitOfMeasure", SqlDbType.VarChar).Value = UOM;
                cmd.Parameters.AddWithValue("@PreviousCode", SqlDbType.VarChar).Value = ItemPreviousCode;
                cmd.Parameters.AddWithValue("@Cost", SqlDbType.Decimal).Value = cost;
                cmd.Parameters.AddWithValue("@ReworkCost", SqlDbType.Decimal).Value = ReworkCost;
                cmd.Parameters.AddWithValue("@InhouseRejCost", SqlDbType.Decimal).Value = InhouseRejCost;

                int row = cmd.ExecuteNonQuery();
                return row;


            }
            catch (Exception exp)
            {
                throw exp;
            }
            finally
            {
                con.Close();

            }
        }
        public static int InsertNewItem(string NameOfItem, string ItemDescription, string ItemCode, string UOM, decimal cost,decimal ReworkCost,decimal InhouseRejCost)
        {
            try
            {

                con.Open();
                SqlCommand cmd = new SqlCommand("Usp_InsertNewItem", con);
                cmd.CommandType = CommandType.StoredProcedure;



                cmd.Parameters.AddWithValue("@NameOfItem", SqlDbType.VarChar).Value = NameOfItem;
                cmd.Parameters.AddWithValue("@ItemDescription", SqlDbType.VarChar).Value = ItemDescription;
                cmd.Parameters.AddWithValue("@CodeOfItem", SqlDbType.VarChar).Value = ItemCode;              
                cmd.Parameters.AddWithValue("@ItemUnitOfMeasure", SqlDbType.VarChar).Value = UOM;
                cmd.Parameters.AddWithValue("@Cost", SqlDbType.Decimal).Value = cost;
                cmd.Parameters.AddWithValue("@ReworkCost", SqlDbType.Decimal).Value = ReworkCost;
                cmd.Parameters.AddWithValue("@InhouseRejCost", SqlDbType.Decimal).Value = InhouseRejCost;
               
               

                int row = cmd.ExecuteNonQuery();

                return row;
            }
            catch (Exception exp)
            {
                throw exp;
            }
            finally
            {
                con.Close();

            }
        }
        public static DataTable fetchItems()
        {
            try
            {


                if (connectionIndicator == false)
                {
                    con.Open();
                    connectionIndicator = true;
                }
                SqlCommand cmd = new SqlCommand("Usp_ShowAllItems", con);
                cmd.CommandType = CommandType.StoredProcedure;

                DataTable dt = new DataTable();

                dt.Load(cmd.ExecuteReader());

                return dt;


            }
            catch (Exception exp)
            {
                throw exp;
            }
            finally
            {
                if (connectionIndicator)
                {
                    con.Close();
                    connectionIndicator = false;
                }
            }
        }
        public static string ItemNameUtility(int ItemId)
        {
            try
            {


                if (connectionIndicator == false)
                {
                    con.Open();
                    connectionIndicator = true;
                }
                SqlCommand cmd = new SqlCommand("Usp_ItemName", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ItemId", SqlDbType.Int).Value = ItemId;
                DataTable dt = new DataTable();

                dt.Load(cmd.ExecuteReader());

                string ItemName = dt.Rows[0][0].ToString();

                return ItemName;


            }
            catch (Exception exp)
            {
                throw exp;
            }
            finally
            {
                if (connectionIndicator)
                {
                    con.Close();
                    connectionIndicator = false;
                }
            }
        }

        public static DataTable FetchItemDetails(int ItemId)
        {

            try
            {


                if (connectionIndicator == false)
                {
                    con.Open();
                    connectionIndicator = true;
                }
                SqlCommand cmd = new SqlCommand("Usp_FetchItems", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ItemId", SqlDbType.Int).Value = ItemId;
                DataTable dt = new DataTable();

                dt.Load(cmd.ExecuteReader());



                return dt;


            }
            catch (Exception exp)
            {
                throw exp;
            }
            finally
            {
                if (connectionIndicator)
                {
                    con.Close();
                    connectionIndicator = false;
                }
            }

        }

        public static DataTable FetchUserDetails(int UsersId)
        {

            try
            {


                if (connectionIndicator == false)
                {
                    con.Open();
                    connectionIndicator = true;
                }
                SqlCommand cmd = new SqlCommand("Usp_GetUsersDetails", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@UserId", SqlDbType.Int).Value = UsersId;
                DataTable dt = new DataTable();

                dt.Load(cmd.ExecuteReader());



                return dt;


            }
            catch (Exception exp)
            {
                throw exp;
            }
            finally
            {
                if (connectionIndicator)
                {
                    con.Close();
                    connectionIndicator = false;
                }
            }

        }
        public static int DeleteItem(int itemId)
        {
            try
            {


                if (connectionIndicator == false)
                {
                    con.Open();
                    connectionIndicator = true;
                }
                SqlCommand cmd = new SqlCommand("Usp_DeleteItem", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ItemId", SqlDbType.Int).Value = itemId;


                int row = cmd.ExecuteNonQuery();
                return row;
             
            }
            catch (Exception exp)
            {
                throw exp;
            }
            finally
            {
                if (connectionIndicator)
                {
                    con.Close();
                    connectionIndicator = false;
                }
            }

        }


        public static int DeleteMachine(int MachineId)
        {
            try
            {


                if (connectionIndicator == false)
                {
                    con.Open();
                    connectionIndicator = true;
                }
                SqlCommand cmd = new SqlCommand("Usp_DeleteMachine", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@MachineId", SqlDbType.Int).Value = MachineId;


                int row = cmd.ExecuteNonQuery();
                return row;

            }
            catch (Exception exp)
            {
                throw exp;
            }
            finally
            {
                if (connectionIndicator)
                {
                    con.Close();
                    connectionIndicator = false;
                }
            }

        }

        public static int DeleteWorkShop(int WorkShopId)
        {
            try
            {


                if (connectionIndicator == false)
                {
                    con.Open();
                    connectionIndicator = true;
                }
                SqlCommand cmd = new SqlCommand("Usp_DeleteWorkShop", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@WorkShopId", SqlDbType.Int).Value = WorkShopId;


                int row = cmd.ExecuteNonQuery();
                return row;

            }
            catch (Exception exp)
            {
                throw exp;
            }
            finally
            {
                if (connectionIndicator)
                {
                    con.Close();
                    connectionIndicator = false;
                }
            }

        }

        public static int DeleteProcess(int ProcessId)
        {
            try
            {


                if (connectionIndicator == false)
                {
                    con.Open();
                    connectionIndicator = true;
                }
                SqlCommand cmd = new SqlCommand("Usp_DeleteProcess", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ProcessId", SqlDbType.Int).Value = ProcessId;


                int row = cmd.ExecuteNonQuery();
                return row;

            }
            catch (Exception exp)
            {
                throw exp;
            }
            finally
            {
                if (connectionIndicator)
                {
                    con.Close();
                    connectionIndicator = false;
                }
            }

        }
        static public string DateTimeNow()
        {
            DateTime time = DateTime.Now;             // Use current time.
            string format = "MMM ddd d HH:mm yyyy";   // Use this format.
            return time.ToString(format);                           // Return DateTimeNow.
        }

        static public int SaveDailyEntry(int ItemId, int TotalNoofUnits,decimal TotalCost, string Datetime) //save daily entry of each item
        {
            try
            {


                if (connectionIndicator == false)
                {
                    con.Open();
                    connectionIndicator = true;
                }

                SqlCommand cmd = new SqlCommand("Usp_SaveDailyEntry", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ItemId", SqlDbType.Int).Value = ItemId;
                cmd.Parameters.AddWithValue("@UserId", SqlDbType.Int).Value = UserId;
                cmd.Parameters.AddWithValue("@TotalNoofUnits", SqlDbType.Int).Value = TotalNoofUnits;
               
                cmd.Parameters.AddWithValue("@TotalCost", SqlDbType.Decimal).Value = TotalCost;
                cmd.Parameters.AddWithValue("@DateTime", SqlDbType.VarChar).Value = Datetime;
                //cmd.Parameters.AddWithValue("@ItemName", SqlDbType.VarChar).Value = itemName;


                int row = cmd.ExecuteNonQuery();
                return row;


            }
            catch (Exception exp)
            {
                throw exp;
            }
            finally
            {
                if (connectionIndicator)
                {
                    con.Close();
                    connectionIndicator = false;
                }
            }

        }


        static public DataTable GetMaterialInfoDetailsbyName(string name)
        {
            try
            {
                if(connectionIndicator==false)
                {
                    con.Open();
                    connectionIndicator = true;
                }
                SqlCommand cmd = new SqlCommand("usp_MaterialInfoDetailByName", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@MaterialName", SqlDbType.VarChar).Value = name;

                DataTable dt = new DataTable();

                dt.Load(cmd.ExecuteReader());

                return dt;

            }
            catch (Exception exp)
            {
                throw exp;
            }
            finally
            {
                if (connectionIndicator)
                {
                    con.Close();
                    connectionIndicator = false;
                }
            }

            
        }
        static public int SaveDailyEntryMaterial(int ItemId, int MaterialId,decimal ActualMaterial,decimal ActualCost ,decimal PlannedMaterial,decimal PlannedCost, string Datetime) //save daily entry of each item
        {
            try
            {


                if (connectionIndicator == false)
                {
                    con.Open();
                    connectionIndicator = true;
                }

                SqlCommand cmd = new SqlCommand("Usp_SaveDailyEntryMaterial", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ItemId", SqlDbType.Int).Value = ItemId;
                cmd.Parameters.AddWithValue("@UserId", SqlDbType.Int).Value = UserId;
                 cmd.Parameters.AddWithValue("@DateTime", SqlDbType.VarChar).Value = Datetime;
                 cmd.Parameters.AddWithValue("@MaterialId", SqlDbType.Int).Value = MaterialId;
                cmd.Parameters.AddWithValue("@ActualMaterial", SqlDbType.Decimal).Value = ActualMaterial;
                 cmd.Parameters.AddWithValue("@ActualCost", SqlDbType.Decimal).Value = ActualCost;
                 cmd.Parameters.AddWithValue("@PlannedMaterial", SqlDbType.Decimal).Value = PlannedMaterial;
                 cmd.Parameters.AddWithValue("@PlannedCost", SqlDbType.Decimal).Value = PlannedCost;


                int row = cmd.ExecuteNonQuery();
                return row;


            }
            catch (Exception exp)
            {
                throw exp;
            }
            finally
            {
                if (connectionIndicator)
                {
                    con.Close();
                    connectionIndicator = false;
                }
            }

        }
        static public int SaveDieAndMold(int ItemId, string  Name, string Code, int PlannedShots) //save Die and Mold
        {
            try
            {


                if (connectionIndicator == false)
                {
                    con.Open();
                    connectionIndicator = true;
                }

                SqlCommand cmd = new SqlCommand("Usp_SaveDieAndMold", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ItemId", SqlDbType.Int).Value = ItemId;
                cmd.Parameters.AddWithValue("@Name", SqlDbType.VarChar).Value = Name;
                cmd.Parameters.AddWithValue("@Code", SqlDbType.VarChar).Value = Code;
                cmd.Parameters.AddWithValue("@PlannedShots", SqlDbType.Int).Value = PlannedShots;
                cmd.Parameters.AddWithValue("@UserId", SqlDbType.Int).Value = UserId;
                
                

                int row = cmd.ExecuteNonQuery();
                return row;


            }
            catch (Exception exp)
            {
                throw exp;
            }
            finally
            {
                if (connectionIndicator)
                {
                    con.Close();
                    connectionIndicator = false;
                }
            }

        }

        static public int SaveDailyEntryDelCompliance(int ItemId, int Planned, int Actual, decimal PercntDelCom, string Datetime) //save daily entry of each item for Del Compliance
        {
            try
            {


                if (connectionIndicator == false)
                {
                    con.Open();
                    connectionIndicator = true;
                }

                SqlCommand cmd = new SqlCommand("Usp_SaveDailyEntryDelComp", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ItemId", SqlDbType.Int).Value = ItemId;
                cmd.Parameters.AddWithValue("@UserId", SqlDbType.Int).Value = UserId;
                cmd.Parameters.AddWithValue("@Planned", SqlDbType.Int).Value = Planned;
                cmd.Parameters.AddWithValue("@Actual", SqlDbType.Int).Value = Actual;
                cmd.Parameters.AddWithValue("@PercntDelCom", SqlDbType.Int).Value = PercntDelCom;
                cmd.Parameters.AddWithValue("@DateTime", SqlDbType.VarChar).Value = Datetime;



                int row = cmd.ExecuteNonQuery();
                return row;


            }
            catch (Exception exp)
            {
                throw exp;
            }
            finally
            {
                if (connectionIndicator)
                {
                    con.Close();
                    connectionIndicator = false;
                }
            }

        }

        static public int SaveDailyEntryProductionAchievement(int ItemId, int Planned, int Actual, decimal PercntDelCom, string Datetime) //save daily entry of each item for Production Achievement
        {
            try
            {


                if (connectionIndicator == false)
                {
                    con.Open();
                    connectionIndicator = true;
                }

                SqlCommand cmd = new SqlCommand("Usp_SaveDailyEntryProductionAchievement", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ItemId", SqlDbType.Int).Value = ItemId;
                cmd.Parameters.AddWithValue("@UserId", SqlDbType.Int).Value = UserId;
                cmd.Parameters.AddWithValue("@Planned", SqlDbType.Int).Value = Planned;
                cmd.Parameters.AddWithValue("@Actual", SqlDbType.Int).Value = Actual;
                cmd.Parameters.AddWithValue("@PercntDelCom", SqlDbType.Int).Value = PercntDelCom;
                cmd.Parameters.AddWithValue("@DateTime", SqlDbType.VarChar).Value = Datetime;



                int row = cmd.ExecuteNonQuery();
                return row;


            }
            catch (Exception exp)
            {
                throw exp;
            }
            finally
            {
                if (connectionIndicator)
                {
                    con.Close();
                    connectionIndicator = false;
                }
            }

        }

        static public int SaveDailyEntryCustomerClaims(int ItemId, int Planned, int Actual, string Datetime) //save daily entry of each item for Customer Claims
        {
            try
            {


                if (connectionIndicator == false)
                {
                    con.Open();
                    connectionIndicator = true;
                }

                SqlCommand cmd = new SqlCommand("Usp_SaveDailyEntryCustomerClaims", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ItemId", SqlDbType.Int).Value = ItemId;
                cmd.Parameters.AddWithValue("@UserId", SqlDbType.Int).Value = UserId;
                cmd.Parameters.AddWithValue("@Planned", SqlDbType.Int).Value = Planned;
                cmd.Parameters.AddWithValue("@Actual", SqlDbType.Int).Value = Actual;
                cmd.Parameters.AddWithValue("@DateTime", SqlDbType.VarChar).Value = Datetime;



                int row = cmd.ExecuteNonQuery();
                return row;


            }
            catch (Exception exp)
            {
                throw exp;
            }
            finally
            {
                if (connectionIndicator)
                {
                    con.Close();
                    connectionIndicator = false;
                }
            }

        }


        static public int EditDailyEntryDelCompliance(int ItemId, int Planned, int Actual, decimal PercntDelCom, string Datetime) //save daily entry of each item for Del Compliance
        {
            try
            {


                if (connectionIndicator == false)
                {
                    con.Open();
                    connectionIndicator = true;
                }

                SqlCommand cmd = new SqlCommand("Usp_EditDailyEntryDelComp", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ItemId", SqlDbType.Int).Value = ItemId;
                cmd.Parameters.AddWithValue("@UserId", SqlDbType.Int).Value = UserId;
                cmd.Parameters.AddWithValue("@Planned", SqlDbType.Int).Value = Planned;
                cmd.Parameters.AddWithValue("@Actual", SqlDbType.Int).Value = Actual;
                cmd.Parameters.AddWithValue("@PercntDelCom", SqlDbType.Int).Value = PercntDelCom;
                cmd.Parameters.AddWithValue("@DateTime", SqlDbType.VarChar).Value = Datetime;



                int row = cmd.ExecuteNonQuery();
                return row;


            }
            catch (Exception exp)
            {
                throw exp;
            }
            finally
            {
                if (connectionIndicator)
                {
                    con.Close();
                    connectionIndicator = false;
                }
            }

        }

        static public int EditDailyEntryProductionAchievement(int ItemId, int Planned, int Actual, decimal PercntDelCom, string Datetime) //save daily entry of each item for Production Achievement
        {
            try
            {


                if (connectionIndicator == false)
                {
                    con.Open();
                    connectionIndicator = true;
                }

                SqlCommand cmd = new SqlCommand("Usp_EditDailyEntryProductionAchievement", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ItemId", SqlDbType.Int).Value = ItemId;
                cmd.Parameters.AddWithValue("@UserId", SqlDbType.Int).Value = UserId;
                cmd.Parameters.AddWithValue("@Planned", SqlDbType.Int).Value = Planned;
                cmd.Parameters.AddWithValue("@Actual", SqlDbType.Int).Value = Actual;
                cmd.Parameters.AddWithValue("@PercntDelCom", SqlDbType.Int).Value = PercntDelCom;
                cmd.Parameters.AddWithValue("@DateTime", SqlDbType.VarChar).Value = Datetime;



                int row = cmd.ExecuteNonQuery();
                return row;


            }
            catch (Exception exp)
            {
                throw exp;
            }
            finally
            {
                if (connectionIndicator)
                {
                    con.Close();
                    connectionIndicator = false;
                }
            }

        }

        static public int EditDailyEntryCustomerClaims(int ItemId, int Planned, int Actual, string Datetime) //Edit daily entry of each item for CustomerClaims
        {
            try
            {


                if (connectionIndicator == false)
                {
                    con.Open();
                    connectionIndicator = true;
                }

                SqlCommand cmd = new SqlCommand("Usp_EditDailyEntryCustomerClaims", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ItemId", SqlDbType.Int).Value = ItemId;
                cmd.Parameters.AddWithValue("@UserId", SqlDbType.Int).Value = UserId;
                cmd.Parameters.AddWithValue("@Planned", SqlDbType.Int).Value = Planned;
                cmd.Parameters.AddWithValue("@Actual", SqlDbType.Int).Value = Actual;
                cmd.Parameters.AddWithValue("@DateTime", SqlDbType.VarChar).Value = Datetime;



                int row = cmd.ExecuteNonQuery();
                return row;


            }
            catch (Exception exp)
            {
                throw exp;
            }
            finally
            {
                if (connectionIndicator)
                {
                    con.Close();
                    connectionIndicator = false;
                }
            }

        }

        static public int EditDailyEntry(int ItemId, int TotalNoofUnits,  decimal TotalCost, string DateTime) //save daily entry of each item
        {
            try
            {


                if (connectionIndicator == false)
                {
                    con.Open();
                    connectionIndicator = true;
                }
                SqlCommand cmd = new SqlCommand("Usp_EditDailyEntry", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ItemId", SqlDbType.Int).Value = ItemId;
                cmd.Parameters.AddWithValue("@UserId", SqlDbType.Int).Value = UserId;
                cmd.Parameters.AddWithValue("@TotalNoofUnits", SqlDbType.Int).Value = TotalNoofUnits;
                cmd.Parameters.AddWithValue("@TotalCost", SqlDbType.Decimal).Value = TotalCost;
                cmd.Parameters.AddWithValue("@DateTime", SqlDbType.VarChar).Value = DateTime;



                int row = cmd.ExecuteNonQuery();
                return row;

            }
            catch (Exception exp)
            {
                throw exp;
            }
            finally
            {
                if (connectionIndicator)
                {
                    con.Close();
                    connectionIndicator = false;
                }
            }
        }

        static public int EditDailyEntryMaterial(int ItemId, int MaterialId, decimal ActualMaterial, decimal ActualCost, decimal PlannedMaterial, decimal PlannedCost, string Datetime) //save daily entry of each item
        {
            try
            {


                if (connectionIndicator == false)
                {
                    con.Open();
                    connectionIndicator = true;
                }
                SqlCommand cmd = new SqlCommand("Usp_EditDailyEntryMaterial", con);
         
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ItemId", SqlDbType.Int).Value = ItemId;
                cmd.Parameters.AddWithValue("@UserId", SqlDbType.Int).Value = UserId;
                cmd.Parameters.AddWithValue("@DateTime", SqlDbType.VarChar).Value = Datetime;
                cmd.Parameters.AddWithValue("@MaterialId", SqlDbType.Int).Value = MaterialId;
                cmd.Parameters.AddWithValue("@ActualMaterial", SqlDbType.Decimal).Value = ActualMaterial;
                cmd.Parameters.AddWithValue("@ActualCost", SqlDbType.Decimal).Value = ActualCost;
                cmd.Parameters.AddWithValue("@PlannedMaterial", SqlDbType.Decimal).Value = PlannedMaterial;
                cmd.Parameters.AddWithValue("@PlannedCost", SqlDbType.Decimal).Value = PlannedCost;



                int row = cmd.ExecuteNonQuery();
                return row;

            }
            catch (Exception exp)
            {
                throw exp;
            }
            finally
            {
                if (connectionIndicator)
                {
                    con.Close();
                    connectionIndicator = false;
                }
            }
        }
        static public int EditMoldsAndDie(int ItemId, string Name, string Code, int PlannedShots, string PreviousCode)
        {
            try
            {


                if (connectionIndicator == false)
                {
                    con.Open();
                    connectionIndicator = true;
                }
                SqlCommand cmd = new SqlCommand("Usp_EditDieAndMolds", con);
               
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ItemId", SqlDbType.Int).Value = ItemId;
                cmd.Parameters.AddWithValue("@Name", SqlDbType.VarChar).Value = Name;
                cmd.Parameters.AddWithValue("@Code", SqlDbType.VarChar).Value = Code;
                cmd.Parameters.AddWithValue("@PlannedShots", SqlDbType.Int).Value = PlannedShots;
                cmd.Parameters.AddWithValue("@PreviousCode", SqlDbType.Int).Value = PreviousCode;
                cmd.Parameters.AddWithValue("@UserId", SqlDbType.Int).Value = UserId;



                int row = cmd.ExecuteNonQuery();
                return row;

            }
            catch (Exception exp)
            {
                throw exp;
            }
            finally
            {
                if (connectionIndicator)
                {
                    con.Close();
                    connectionIndicator = false;
                }
            }
        }
        static public DataTable TotalUnitsMadeDetails(int ItemId)
        {
            try
            {


                if (connectionIndicator == false)
                {
                    con.Open();
                    connectionIndicator = true;
                }
                SqlCommand cmd = new SqlCommand("Usp_GetDailyEntry", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ItemId", SqlDbType.Int).Value = ItemId;
                DataTable dt = new DataTable();

                dt.Load(cmd.ExecuteReader());



                return dt;


            }
            catch (Exception exp)
            {
                throw exp;
            }
            finally
            {
                if (connectionIndicator)
                {
                    con.Close();
                    connectionIndicator = false;
                }
            }
        }

        static public decimal CalculateTotalCostofMaterial(decimal costofEachUnit, decimal Multiple, decimal totaMaterial)
        {
            decimal costofEachUnitOfMultiple = costofEachUnit / Multiple;
            costofEachUnitOfMultiple = costofEachUnitOfMultiple * totaMaterial;
            return costofEachUnitOfMultiple;
        }
        static public DataTable TotalUnitsMadeDetailsByName(string ItemName, string DateTime, string KPI)
        {
            try
            {


                if (connectionIndicator == false)
                {
                    con.Open();
                    connectionIndicator = true;
                }
                SqlCommand cmd;
                DataTable dt = new DataTable();
                if (KPI == "DailyEntry")
                {
                    cmd = new SqlCommand("Usp_GetDailyEntrybyName", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@ItemName", SqlDbType.VarChar).Value = ItemName;
                    cmd.Parameters.AddWithValue("@Date", SqlDbType.VarChar).Value = DateTime;

                    dt.Load(cmd.ExecuteReader());
                }
                else if (KPI == "ReworkRatio")
                {
                    cmd = new SqlCommand("Usp_GetDailyEntrybyNameReworkRatio", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@ItemName", SqlDbType.VarChar).Value = ItemName;
                    cmd.Parameters.AddWithValue("@Date", SqlDbType.VarChar).Value = DateTime;

                    dt.Load(cmd.ExecuteReader());
                }
                else if (KPI == "InhouseRatio")
                {
                    cmd = new SqlCommand("Usp_GetDailyEntrybyNameInhouseRejection", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@ItemName", SqlDbType.VarChar).Value = ItemName;
                    cmd.Parameters.AddWithValue("@Date", SqlDbType.VarChar).Value = DateTime;

                    dt.Load(cmd.ExecuteReader());
                }
                else if (KPI == "CustomerRejection")
                {
                    cmd = new SqlCommand("Usp_GetDailyEntrybyNameCustomerRejection", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@ItemName", SqlDbType.VarChar).Value = ItemName;
                    cmd.Parameters.AddWithValue("@Date", SqlDbType.VarChar).Value = DateTime;

                    dt.Load(cmd.ExecuteReader());
                }


                return dt;






            }
            catch (Exception exp)
            {
                throw exp;
            }
            finally
            {
                if (connectionIndicator)
                {
                    con.Close();
                    connectionIndicator = false;
                }


            }


        }
        static public DataTable TotalUnitsMadeDetailsByNameforDailyComp(string ItemName, string DateTime)
        {
            try
            {


                if (connectionIndicator == false)
                {
                    con.Open();
                    connectionIndicator = true;
                }
                SqlCommand cmd;
                DataTable dt = new DataTable();

                cmd = new SqlCommand("Usp_GetDailyEntrybyNameDelComp", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ItemName", SqlDbType.VarChar).Value = ItemName;
                cmd.Parameters.AddWithValue("@Date", SqlDbType.VarChar).Value = DateTime;

                dt.Load(cmd.ExecuteReader());



                return dt;






            }
            catch (Exception exp)
            {
                throw exp;
            }
            finally
            {
                if (connectionIndicator)
                {
                    con.Close();
                    connectionIndicator = false;
                }


            }


        }

        static public DataTable TotalUnitsMadeDetailsByNameforProductionAchievement(string ItemName, string DateTime)
        {
            try
            {


                if (connectionIndicator == false)
                {
                    con.Open();
                    connectionIndicator = true;
                }
                SqlCommand cmd;
                DataTable dt = new DataTable();

                cmd = new SqlCommand("Usp_GetDailyEntrybyNameAchievementRate", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ItemName", SqlDbType.VarChar).Value = ItemName;
                cmd.Parameters.AddWithValue("@Date", SqlDbType.VarChar).Value = DateTime;

                dt.Load(cmd.ExecuteReader());



                return dt;






            }
            catch (Exception exp)
            {
                throw exp;
            }
            finally
            {
                if (connectionIndicator)
                {
                    con.Close();
                    connectionIndicator = false;
                }


            }


        }


        static public DataTable TotalUnitsMadeDetailsByNameforCustomerClaims(string ItemName, string DateTime)
        {
            try
            {


                if (connectionIndicator == false)
                {
                    con.Open();
                    connectionIndicator = true;
                }
                SqlCommand cmd;
                DataTable dt = new DataTable();

                cmd = new SqlCommand("Usp_GetDailyEntrybyNameCustomerClaims", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ItemName", SqlDbType.VarChar).Value = ItemName;
                cmd.Parameters.AddWithValue("@Date", SqlDbType.VarChar).Value = DateTime;

                dt.Load(cmd.ExecuteReader());



                return dt;






            }
            catch (Exception exp)
            {
                throw exp;
            }
            finally
            {
                if (connectionIndicator)
                {
                    con.Close();
                    connectionIndicator = false;
                }


            }


        }
        static public int SaveDailyEntryofKPI(int ItemId, int TotalNoofUnits, decimal TotalCost, string dateTime, string KPI) //save daily entry of rework ratio each item
        {
            int row = 0;

            try
            {


                if (connectionIndicator == false)
                {
                    con.Open();
                    connectionIndicator = true;
                }
                SqlCommand cmd;
                if (KPI == "ReworkRatio")
                {
                    cmd = new SqlCommand("Usp_SaveDailyEntryofReworkRatio", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@ItemId", SqlDbType.Int).Value = ItemId;
                    cmd.Parameters.AddWithValue("@UserId", SqlDbType.Int).Value = UserId;
                    cmd.Parameters.AddWithValue("@TotalNoofUnitsReworked", SqlDbType.Int).Value = TotalNoofUnits;
                    cmd.Parameters.AddWithValue("@TotalCostRework", SqlDbType.Decimal).Value = TotalCost;
                    cmd.Parameters.AddWithValue("@Date", SqlDbType.VarChar).Value = dateTime;



                    row = cmd.ExecuteNonQuery();
                }
                else if (KPI == "InhouseRatio")
                {
                    cmd = new SqlCommand("Usp_SaveDailyEntryofInhouseRejection", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@ItemId", SqlDbType.Int).Value = ItemId;
                    cmd.Parameters.AddWithValue("@UserId", SqlDbType.Int).Value = UserId;
                    cmd.Parameters.AddWithValue("@TotalNoofUnitsRejected", SqlDbType.Int).Value = TotalNoofUnits;
                    cmd.Parameters.AddWithValue("@TotalCostofRejection", SqlDbType.Decimal).Value = TotalCost;
                    cmd.Parameters.AddWithValue("@Date", SqlDbType.VarChar).Value = dateTime;



                    row = cmd.ExecuteNonQuery();
                }
                else if (KPI == "CustomerRejection")
                {
                    cmd = new SqlCommand("Usp_SaveDailyEntryofCustomerRejection", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@ItemId", SqlDbType.Int).Value = ItemId;
                    cmd.Parameters.AddWithValue("@UserId", SqlDbType.Int).Value = UserId;
                    cmd.Parameters.AddWithValue("@TotalNoofUnitsRejected", SqlDbType.Int).Value = TotalNoofUnits;
                    cmd.Parameters.AddWithValue("@TotalCostofRejection", SqlDbType.Decimal).Value = TotalCost;
                    cmd.Parameters.AddWithValue("@Date", SqlDbType.VarChar).Value = dateTime;



                    row = cmd.ExecuteNonQuery();
                }
            }
            catch (Exception exp)
            {
                throw exp;
            }
            finally
            {
                if (connectionIndicator)
                {
                    con.Close();
                    connectionIndicator = false;
                }
            }
            return row;
        }
        static public int EditDailyEntryofReworkRatio(int ItemId, int TotalNoofUnits, decimal TotalCost, string dateTime, string KPI) //save daily entry of rework ratio each item
        {
            int rows = 0;
            try
            {


                if (connectionIndicator == false)
                {
                    con.Open();
                    connectionIndicator = true;
                }
                SqlCommand cmd;

                if (KPI == "ReworkRatio")
                {

                    cmd = new SqlCommand("Usp_EditDailyEntryofReworkRatio", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@ItemId", SqlDbType.Int).Value = ItemId;
                    cmd.Parameters.AddWithValue("@UserId", SqlDbType.Int).Value = UserId;
                    cmd.Parameters.AddWithValue("@TotalNoofUnitsReworked", SqlDbType.Int).Value = TotalNoofUnits;
                    cmd.Parameters.AddWithValue("@TotalCostRework", SqlDbType.Decimal).Value = TotalCost;
                    cmd.Parameters.AddWithValue("@Date", SqlDbType.VarChar).Value = dateTime;



                    rows = cmd.ExecuteNonQuery();
                }
                else if (KPI == "InhouseRatio")
                {
                    cmd = new SqlCommand("Usp_EditDailyEntryofInhouseRejection", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@ItemId", SqlDbType.Int).Value = ItemId;
                    cmd.Parameters.AddWithValue("@UserId", SqlDbType.Int).Value = UserId;
                    cmd.Parameters.AddWithValue("@TotalNoofUnits", SqlDbType.Int).Value = TotalNoofUnits;
                    cmd.Parameters.AddWithValue("@TotalCost", SqlDbType.Decimal).Value = TotalCost;
                    cmd.Parameters.AddWithValue("@Date", SqlDbType.VarChar).Value = dateTime;



                    rows = cmd.ExecuteNonQuery();
                }
                else if (KPI == "CustomerRejection")
                {
                    cmd = new SqlCommand("Usp_EditDailyEntryofCustomerRejection", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@ItemId", SqlDbType.Int).Value = ItemId;
                    cmd.Parameters.AddWithValue("@UserId", SqlDbType.Int).Value = UserId;
                    cmd.Parameters.AddWithValue("@TotalNoofUnits", SqlDbType.Int).Value = TotalNoofUnits;
                    cmd.Parameters.AddWithValue("@TotalCost", SqlDbType.Decimal).Value = TotalCost;
                    cmd.Parameters.AddWithValue("@Date", SqlDbType.VarChar).Value = dateTime;



                    rows = cmd.ExecuteNonQuery();
                }

            }
            catch (Exception exp)
            {
                throw exp;
            }
            finally
            {
                if (connectionIndicator)
                {
                    con.Close();
                    connectionIndicator = false;
                }
            }
            return rows;
        }
        public static DataTable InitializeItemDropDown()
        {



            try
            {


                if (connectionIndicator == false)
                {
                    con.Open();
                    connectionIndicator = true;
                }
                SqlCommand cmd = new SqlCommand("Usp_ShowAllItemsinDropDown", con);
                cmd.CommandType = CommandType.StoredProcedure;

                DataTable dt = new DataTable();

                dt.Load(cmd.ExecuteReader());

                return dt;


            }
            catch (Exception exp)
            {
                throw exp;
            }
            finally
            {
                if (connectionIndicator)
                {
                    con.Close();
                    connectionIndicator = false;
                }
            }

        }

        public static DataTable InitializeItemByMachineDropDown(int MachineId)
        {



            try
            {


                if (connectionIndicator == false)
                {
                    con.Open();
                    connectionIndicator = true;
                }
                SqlCommand cmd = new SqlCommand("Usp_ShowAllItemsforSelectedMachineinDropDown", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@MachineId", SqlDbType.Int).Value = MachineId;

                DataTable dt = new DataTable();

                dt.Load(cmd.ExecuteReader());

                return dt;


            }
            catch (Exception exp)
            {
                throw exp;
            }
            finally
            {
                if (connectionIndicator)
                {
                    con.Close();
                    connectionIndicator = false;
                }
            }

        }

        public static DataTable InitializeItemDropDownDieAndMolds(int ItemId)
        {



            try
            {


                if (connectionIndicator == false)
                {
                    con.Open();
                    connectionIndicator = true;
                }
                SqlCommand cmd = new SqlCommand("Usp_ShowAllDiesinDropDown", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ItemId", SqlDbType.Int).Value = ItemId;

                DataTable dt = new DataTable();

                dt.Load(cmd.ExecuteReader());

                return dt;


            }
            catch (Exception exp)
            {
                throw exp;
            }
            finally
            {
                if (connectionIndicator)
                {
                    con.Close();
                    connectionIndicator = false;
                }
            }

        }
        public static DataTable InitializeUser()
        {



            try
            {


                if (connectionIndicator == false)
                {
                    con.Open();
                    connectionIndicator = true;
                }
                SqlCommand cmd = new SqlCommand("Usp_ShowAllIUsersinDropDown", con);
                cmd.CommandType = CommandType.StoredProcedure;

                DataTable dt = new DataTable();

                dt.Load(cmd.ExecuteReader());

                return dt;


            }
            catch (Exception exp)
            {
                throw exp;
            }
            finally
            {
                if (connectionIndicator)
                {
                    con.Close();
                    connectionIndicator = false;
                }
            }

        }

        public static DataTable InitializeUserTypesDropDown()
        {



            try
            {


                if (connectionIndicator == false)
                {
                    con.Open();
                    connectionIndicator = true;
                }
                SqlCommand cmd = new SqlCommand("Usp_ShowAllIUsersTypesinDropDown", con);
                cmd.CommandType = CommandType.StoredProcedure;

                DataTable dt = new DataTable();

                dt.Load(cmd.ExecuteReader());

                return dt;


            }
            catch (Exception exp)
            {
                throw exp;
            }
            finally
            {
                if (connectionIndicator)
                {
                    con.Close();
                    connectionIndicator = false;
                }
            }

        }
        //public static DataTable InitializeItemDropDown()
        //{



        //    try
        //    {


        //        if (connectionIndicator == false)
        //        {
        //            con.Open();
        //            connectionIndicator = true;
        //        }
        //        SqlCommand cmd = new SqlCommand("Usp_ShowAllItemsinDropDown", con);
        //        cmd.CommandType = CommandType.StoredProcedure;

        //        DataTable dt = new DataTable();

        //        dt.Load(cmd.ExecuteReader());

        //        return dt;


        //    }
        //    catch (Exception exp)
        //    {
        //        throw exp;
        //    }
        //    finally
        //    {
        //        if (connectionIndicator)
        //        {
        //            con.Close();
        //            connectionIndicator = false;
        //        }
        //    }

        //}
        public static int SaveFactoryOrShop(string Name, int FacOrShop)
        {
            int rows = 0;
            try
            {
                if (connectionIndicator == false)
                {
                    con.Open();
                    connectionIndicator = true;
                }
                SqlCommand cmd;

                if (FacOrShop == 0)//0 for factory
                {


                    cmd = new SqlCommand("Usp_SaveFactory", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@FactoryName", SqlDbType.VarChar).Value = Name;
                    cmd.Parameters.AddWithValue("@UserId", SqlDbType.Int).Value = UserId;

                    rows = cmd.ExecuteNonQuery();
                }
                else if (FacOrShop == 1)//1 for shop
                {

                    cmd = new SqlCommand("Usp_SaveShop", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@WsName", SqlDbType.VarChar).Value = Name;
                    cmd.Parameters.AddWithValue("@UserId", SqlDbType.Int).Value = UserId;

                    rows = cmd.ExecuteNonQuery();

                }

            }
            catch (Exception exp)
            {
                throw exp;

            }
            finally
            {
                if (connectionIndicator)
                {
                    con.Close();
                    connectionIndicator = false;
                }

            }
            return rows;
        }

        public static int SaveFactory(string Name, int ShiftTime, int BreakTime)
        {
            int rows = 0;
            try
            {
                if (connectionIndicator == false)
                {
                    con.Open();
                    connectionIndicator = true;
                }
                SqlCommand cmd;

               


                    cmd = new SqlCommand("Usp_SaveFactory", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@FactoryName", SqlDbType.VarChar).Value = Name;
                    cmd.Parameters.AddWithValue("@ShiftTime", SqlDbType.Int).Value = ShiftTime;
                    cmd.Parameters.AddWithValue("@BreakTime", SqlDbType.Int).Value = BreakTime;
                    cmd.Parameters.AddWithValue("@UserId", SqlDbType.Int).Value = UserId;

                    rows = cmd.ExecuteNonQuery();
               

            }
            catch (Exception exp)
            {
                throw exp;

            }
            finally
            {
                if (connectionIndicator)
                {
                    con.Close();
                    connectionIndicator = false;
                }

            }
            return rows;
        }
        public static int SaveMachine(string Name, int WorkShopId, decimal ShifTime, string Code, int NoOfMonths)
        {
            int rows = 0;
            try
            {
                if (connectionIndicator == false)
                {
                    con.Open();
                    connectionIndicator = true;
                }


                SqlCommand cmd = new SqlCommand("Usp_SaveMachine", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@MachineName", SqlDbType.VarChar).Value = Name;
                cmd.Parameters.AddWithValue("@UserId", SqlDbType.Int).Value = UserId;
                cmd.Parameters.AddWithValue("@WorkShopId", SqlDbType.Int).Value = WorkShopId;
                cmd.Parameters.AddWithValue("@ShifTime", SqlDbType.Decimal).Value = ShifTime;
                cmd.Parameters.AddWithValue("@Code", SqlDbType.VarChar).Value = Code;
                cmd.Parameters.AddWithValue("@NoOfMonths", SqlDbType.Int).Value = NoOfMonths;

                rows = cmd.ExecuteNonQuery();
                if (rows > 0)
                {
                    UpdateShopInfoDueToChangeinMachine(WorkShopId);
                }

            }
            catch (Exception exp)
            {
                throw exp;

            }
            finally
            {
                if (connectionIndicator)
                {
                    con.Close();
                    connectionIndicator = false;
                }

            }
            return rows;
        }

        public static int EditFactoryOrShop(string Name, int FacOrShop)
        {
            int rows = 0;
            try
            {


                if (connectionIndicator == false)
                {
                    con.Open();
                    connectionIndicator = true;
                }
                SqlCommand cmd;
                if (FacOrShop == 0)//=0 for factory
                {
                    cmd = new SqlCommand("Usp_EditFactory", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@FactoryName", SqlDbType.VarChar).Value = Name;
                    cmd.Parameters.AddWithValue("@UserId", SqlDbType.Int).Value = UserId;

                    rows = cmd.ExecuteNonQuery();
                }
                else if (FacOrShop > 0)//>0 for Shop
                {
                    cmd = new SqlCommand("Usp_EditShop", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@ShopName", SqlDbType.VarChar).Value = Name;
                    cmd.Parameters.AddWithValue("@UserId", SqlDbType.Int).Value = UserId;
                    cmd.Parameters.AddWithValue("@WorkShopId", SqlDbType.Int).Value = FacOrShop;

                    rows = cmd.ExecuteNonQuery();

                }
            }
            catch (Exception exp)
            {
                throw exp;

            }
            finally
            {
                if (connectionIndicator)
                {
                    con.Close();
                    connectionIndicator = false;
                }

            }
            return rows;
        }


        public static int EditFactory(string Name, int ShifTme, int BreakTime)
        {
            int rows = 0;
            try
            {


                if (connectionIndicator == false)
                {
                    con.Open();
                    connectionIndicator = true;
                }
                SqlCommand cmd;
              
                    cmd = new SqlCommand("Usp_EditFactory", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@FactoryName", SqlDbType.VarChar).Value = Name;
                    cmd.Parameters.AddWithValue("@ShifTime", SqlDbType.Int).Value = ShifTme;
                    cmd.Parameters.AddWithValue("@BreakTime", SqlDbType.Int).Value = BreakTime;
                    cmd.Parameters.AddWithValue("@UserId", SqlDbType.Int).Value = UserId;

                    rows = cmd.ExecuteNonQuery();
               
            }
            catch (Exception exp)
            {
                throw exp;

            }
            finally
            {
                if (connectionIndicator)
                {
                    con.Close();
                    connectionIndicator = false;
                }

            }
            return rows;
        }
        public static void UpdateShopInfoDueToChangeinMachine(int WorkShopId)
        {

            int rows = 0;
            try
            {


                if (connectionIndicator == false)
                {
                    con.Open();
                    connectionIndicator = true;
                }
                SqlCommand cmd = new SqlCommand("Usp_UpdateWSDueUpdateInMachine", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@WorkShopId", SqlDbType.Int).Value = WorkShopId;
                cmd.Parameters.AddWithValue("@UserId", SqlDbType.Int).Value = UserId;


                rows = cmd.ExecuteNonQuery();


            }
            catch (Exception exp)
            {
                throw exp;

            }
            finally
            {
                if (connectionIndicator)
                {
                    con.Close();
                    connectionIndicator = false;
                }

            }

        }
        public static int EditMachine(string Name, int MachineId, decimal ShiftTime, int WorkShopId, string Code, int NoOfMonths)
        {
            int rows = 0;
            try
            {


                if (connectionIndicator == false)
                {
                    con.Open();
                    connectionIndicator = true;
                }
                SqlCommand cmd = new SqlCommand("Usp_EditMachine", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@MachineName", SqlDbType.VarChar).Value = Name;
                cmd.Parameters.AddWithValue("@UserId", SqlDbType.Int).Value = UserId;
                cmd.Parameters.AddWithValue("@MachineId", SqlDbType.Int).Value = MachineId;
                cmd.Parameters.AddWithValue("@ShiftTime", SqlDbType.Decimal).Value = ShiftTime;
                cmd.Parameters.AddWithValue("@WorkShopId", SqlDbType.Int).Value = WorkShopId;
                cmd.Parameters.AddWithValue("@Code", SqlDbType.VarChar).Value = Code;
                cmd.Parameters.AddWithValue("@NoOfMonths", SqlDbType.Int).Value = NoOfMonths;

                rows = cmd.ExecuteNonQuery();
                if (rows > 0)
                {
                    UpdateShopInfoDueToChangeinMachine(WorkShopId);
                }

            }
            catch (Exception exp)
            {
                throw exp;

            }
            finally
            {
                if (connectionIndicator)
                {
                    con.Close();
                    connectionIndicator = false;
                }

            }
            return rows;
        }


        public static DataTable ViewFactory()
        {
            string factoryName = "";
            try
            {


                if (connectionIndicator == false)
                {
                    con.Open();
                    connectionIndicator = true;
                }
                SqlCommand cmd = new SqlCommand("Usp_GetFactory", con);
                cmd.CommandType = CommandType.StoredProcedure;

                DataTable dt = new DataTable();

                dt.Load(cmd.ExecuteReader());

                return dt;



            }
            catch (Exception exp)
            {
                throw exp;
            }
            finally
            {
                if (connectionIndicator)
                {
                    con.Close();
                    connectionIndicator = false;
                }
            }
            
        }

        public static DataTable ViewShops()
        {

            try
            {


                if (connectionIndicator == false)
                {
                    con.Open();
                    connectionIndicator = true;
                }
                SqlCommand cmd = new SqlCommand("Usp_GetWorkShop", con);
                cmd.CommandType = CommandType.StoredProcedure;

                DataTable dt = new DataTable();

                dt.Load(cmd.ExecuteReader());

                return dt;



            }
            catch (Exception exp)
            {
                throw exp;
            }
            finally
            {
                if (connectionIndicator)
                {
                    con.Close();
                    connectionIndicator = false;
                }
            }

        }
        public static DataTable ViewMachines(int WorkShopId)
        {

            try
            {


                if (connectionIndicator == false)
                {
                    con.Open();
                    connectionIndicator = true;
                }
                SqlCommand cmd = new SqlCommand("Usp_GetMachines", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@WorkShopId", SqlDbType.Int).Value = WorkShopId;

                DataTable dt = new DataTable();

                dt.Load(cmd.ExecuteReader());

                return dt;



            }
            catch (Exception exp)
            {
                throw exp;
            }
            finally
            {
                if (connectionIndicator)
                {
                    con.Close();
                    connectionIndicator = false;
                }
            }

        }

        public static DataTable GetAllMachines()
        {

            try
            {


                if (connectionIndicator == false)
                {
                    con.Open();
                    connectionIndicator = true;
                }
                SqlCommand cmd = new SqlCommand("Usp_GetAllMachines", con);
                cmd.CommandType = CommandType.StoredProcedure;
             //   cmd.Parameters.AddWithValue("@WorkShopId", SqlDbType.Int).Value = WorkShopId;

                DataTable dt = new DataTable();

                dt.Load(cmd.ExecuteReader());

                return dt;



            }
            catch (Exception exp)
            {
                throw exp;
            }
            finally
            {
                if (connectionIndicator)
                {
                    con.Close();
                    connectionIndicator = false;
                }
            }

        }


        public static DataTable GetAllMachinesAssociatedToItem()
        {

            try
            {


                if (connectionIndicator == false)
                {
                    con.Open();
                    connectionIndicator = true;
                }
                SqlCommand cmd = new SqlCommand("Usp_GetAllMachines", con);
                cmd.CommandType = CommandType.StoredProcedure;
                //   cmd.Parameters.AddWithValue("@WorkShopId", SqlDbType.Int).Value = WorkShopId;

                DataTable dt = new DataTable();

                dt.Load(cmd.ExecuteReader());

                return dt;



            }
            catch (Exception exp)
            {
                throw exp;
            }
            finally
            {
                if (connectionIndicator)
                {
                    con.Close();
                    connectionIndicator = false;
                }
            }

        }

        public static DataTable ViewMachinesDetailsTPMMachine(int WorkShopId)
        {

            try
            {


                if (connectionIndicator == false)
                {
                    con.Open();
                    connectionIndicator = true;
                }
                SqlCommand cmd = new SqlCommand("Usp_Usp_GetMachinesDetailsTPMMachine", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@WorkShopId", SqlDbType.Int).Value = WorkShopId;

                DataTable dt = new DataTable();

                dt.Load(cmd.ExecuteReader());

                return dt;



            }
            catch (Exception exp)
            {
                throw exp;
            }
            finally
            {
                if (connectionIndicator)
                {
                    con.Close();
                    connectionIndicator = false;
                }
            }

        }
        public static int GetWorkShopId(string WorkShopName)
        {
            try
            {


                if (connectionIndicator == false)
                {
                    con.Open();
                    connectionIndicator = true;
                }
                SqlCommand cmd = new SqlCommand("Usp_GetWorkShopId", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@ShopName", SqlDbType.VarChar).Value = WorkShopName;
                DataTable dt = new DataTable();

                dt.Load(cmd.ExecuteReader());

                return Convert.ToInt32(dt.Rows[0][0]);



            }
            catch (Exception exp)
            {
                throw exp;
            }
            finally
            {
                if (connectionIndicator)
                {
                    con.Close();
                    connectionIndicator = false;
                }
            }
        }
        public static DataTable GetMachineId(string MachineCode,bool isName=false)
        {
            try
            {
                DataTable dt = new DataTable();

                if (connectionIndicator == false)
                {
                    con.Open();
                    connectionIndicator = true;
                }
                if (!isName)
                {
                    SqlCommand cmd = new SqlCommand("Usp_GetMachineId", con);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@Code", SqlDbType.VarChar).Value = MachineCode;
                    dt.Load(cmd.ExecuteReader());
                }
                else
                {
                    SqlCommand cmd = new SqlCommand("Usp_GetMachineIdByName", con);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@Name", SqlDbType.VarChar).Value = MachineCode;
                    dt.Load(cmd.ExecuteReader());
                }
               

              

                return dt;



            }
            catch (Exception exp)
            {
                throw exp;
            }
            finally
            {
                if (connectionIndicator)
                {
                    con.Close();
                    connectionIndicator = false;
                }
            }
        }
        public static int GetDownTime(string dateTime, int MachineId, int  WorkShopId, int ItemId, int ProcessId)
        {
            try
            {


                if (connectionIndicator == false)
                {
                    con.Open();
                    connectionIndicator = true;
                }
                SqlCommand cmd = new SqlCommand("Usp_GetMachineDownTime", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@MachineId", SqlDbType.Int).Value = MachineId;
                cmd.Parameters.AddWithValue("@WorkShopId", SqlDbType.Int).Value = WorkShopId;
                cmd.Parameters.AddWithValue("@ItemId", SqlDbType.Int).Value = ItemId;
                cmd.Parameters.AddWithValue("@ProcessId", SqlDbType.Int).Value = ProcessId;
                cmd.Parameters.AddWithValue("@dateTime", SqlDbType.VarChar).Value = dateTime;
                DataTable dt = new DataTable();

                dt.Load(cmd.ExecuteReader());

                return Convert.ToInt32(dt.Rows[0][0]);



            }
            catch (Exception exp)
            {
                throw exp;
            }
            finally
            {
                if (connectionIndicator)
                {
                    con.Close();
                    connectionIndicator = false;
                }
            }
        }
        public static int SaveMachineDailyDownTime(int MachineId, int WorkShopId, decimal DownTimeDaily, decimal Ratio, string dateTime,int ItemId, int ProcessId)
        {
            int rows = 0;
            try
            {


                if (connectionIndicator == false)
                {
                    con.Open();
                    connectionIndicator = true;
                }
                SqlCommand cmd = new SqlCommand("Usp_SaveDailyMachineEntry", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@MachineId", SqlDbType.Int).Value = MachineId;
                cmd.Parameters.AddWithValue("@WorkShopId", SqlDbType.Int).Value = WorkShopId;
                cmd.Parameters.AddWithValue("@UserId", SqlDbType.Int).Value = UserId;

                cmd.Parameters.AddWithValue("@DownTime", SqlDbType.Decimal).Value = DownTimeDaily;
                cmd.Parameters.AddWithValue("@Ratio", SqlDbType.Decimal).Value = Ratio;
                cmd.Parameters.AddWithValue("@Date", SqlDbType.VarChar).Value = dateTime;

                cmd.Parameters.AddWithValue("@ItemID", SqlDbType.Decimal).Value = ItemId;
                cmd.Parameters.AddWithValue("@ProcessId", SqlDbType.VarChar).Value = ProcessId;


                rows = cmd.ExecuteNonQuery();
                if (rows > 0)
                {
                    UpdateShopInfoDailyEntryofWorkShop(WorkShopId, dateTime);
                }

            }
            catch (Exception exp)
            {
                throw exp;

            }
            finally
            {
                if (connectionIndicator)
                {
                    con.Close();
                    connectionIndicator = false;
                }

            }
            return rows;
        }
        public static int EditMachineDailyEntry(int MachineId, int WorkShopId, decimal DownTimeDaily, decimal Ratio, string dateTime,int ItemId, int ProcessId)
        {
            int rows = 0;
            try
            {


                if (connectionIndicator == false)
                {
                    con.Open();
                    connectionIndicator = true;
                }
                SqlCommand cmd = new SqlCommand("Usp_EditMachineDailyEntry", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@MachineId", SqlDbType.Int).Value = MachineId;
                cmd.Parameters.AddWithValue("@ShopId", SqlDbType.Int).Value = WorkShopId;
                cmd.Parameters.AddWithValue("@UserId", SqlDbType.Int).Value = UserId;
                cmd.Parameters.AddWithValue("@DownTime", SqlDbType.Decimal).Value = DownTimeDaily;
                cmd.Parameters.AddWithValue("@Ratio", SqlDbType.Decimal).Value = Ratio;
                cmd.Parameters.AddWithValue("@Date", SqlDbType.VarChar).Value = dateTime;
                cmd.Parameters.AddWithValue("@ItemId", SqlDbType.Int).Value = ItemId;
                cmd.Parameters.AddWithValue("@ProcessId", SqlDbType.Int).Value = ProcessId;


                rows = cmd.ExecuteNonQuery();
                if (rows > 0)
                {
                    UpdateShopInfoDailyEntryofWorkShop(WorkShopId, dateTime);
                }

            }
            catch (Exception exp)
            {
                throw exp;

            }
            finally
            {
                if (connectionIndicator)
                {
                    con.Close();
                    connectionIndicator = false;
                }

            }
            return rows;
        }
        public static void UpdateShopInfoDailyEntryofWorkShop(int WorkShopId, string dateTime)
        {
            int rows = 0;
            try
            {


                if (connectionIndicator == false)
                {
                    con.Open();
                    connectionIndicator = true;
                }
                SqlCommand cmd = new SqlCommand("Usp_SaveWSDueSaveInMachine", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@WorkShopId", SqlDbType.Int).Value = WorkShopId;
                cmd.Parameters.AddWithValue("@Date", SqlDbType.VarChar).Value = dateTime;
                cmd.Parameters.AddWithValue("@UserId", SqlDbType.Int).Value = UserId;


                rows = cmd.ExecuteNonQuery();


            }
            catch (Exception exp)
            {
                throw exp;

            }
            finally
            {
                if (connectionIndicator)
                {
                    con.Close();
                    connectionIndicator = false;
                }

            }
        }

        public static DataTable Getcost(int item_id)
        {
            try
            {


                if (connectionIndicator == false)
                {
                    con.Open();
                    connectionIndicator = true;
                }
                SqlCommand cmd = new SqlCommand("usp_GetItemCost", con);
                cmd.CommandType = CommandType.StoredProcedure;


                cmd.Parameters.AddWithValue("@Item_Id", SqlDbType.VarChar).Value = item_id;
                DataTable dt = new DataTable();

                dt.Load(cmd.ExecuteReader());

                return dt;



            }
            catch (Exception exp)
            {
                throw exp;
            }
            finally
            {
                if (connectionIndicator)
                {
                    con.Close();
                    connectionIndicator = false;
                }
            }
        }
        public static int GetITemId(string itemName)
        {
            try
            {


                if (connectionIndicator == false)
                {
                    con.Open();
                    connectionIndicator = true;
                }
                SqlCommand cmd = new SqlCommand("usp_GetItemId", con);
                cmd.CommandType = CommandType.StoredProcedure;


                cmd.Parameters.AddWithValue("@ItemName", SqlDbType.VarChar).Value = itemName;
                DataTable dt = new DataTable();

                dt.Load(cmd.ExecuteReader());

                return Convert.ToInt32(dt.Rows[0][0]);



            }
            catch (Exception exp)
            {
                throw exp;
            }
            finally
            {
                if (connectionIndicator)
                {
                    con.Close();
                    connectionIndicator = false;
                }
            }

        }


        public static int GetUserId(string UserName)
        {
            try
            {


                if (connectionIndicator == false)
                {
                    con.Open();
                    connectionIndicator = true;
                }
                SqlCommand cmd = new SqlCommand("usp_GetUserId", con);
                cmd.CommandType = CommandType.StoredProcedure;


                cmd.Parameters.AddWithValue("@UserName", SqlDbType.VarChar).Value = UserName;
                DataTable dt = new DataTable();

                dt.Load(cmd.ExecuteReader());

                return Convert.ToInt32(dt.Rows[0][0]);



            }
            catch (Exception exp)
            {
                throw exp;
            }
            finally
            {
                if (connectionIndicator)
                {
                    con.Close();
                    connectionIndicator = false;
                }
            }

        }


        public static int GetUserTypesId(string UserName)
        {
            try
            {


                if (connectionIndicator == false)
                {
                    con.Open();
                    connectionIndicator = true;
                }
                SqlCommand cmd = new SqlCommand("usp_GetUserTypesId", con);
                cmd.CommandType = CommandType.StoredProcedure;


                cmd.Parameters.AddWithValue("@UserName", SqlDbType.VarChar).Value = UserName;
                DataTable dt = new DataTable();

                dt.Load(cmd.ExecuteReader());

                return Convert.ToInt32(dt.Rows[0][0]);



            }
            catch (Exception exp)
            {
                throw exp;
            }
            finally
            {
                if (connectionIndicator)
                {
                    con.Close();
                    connectionIndicator = false;
                }
            }

        }
        public static int SaveProcess(int itemId, string ProcessName, int NoofOperators, int CycleTime, decimal ratingFactor, decimal Capacity,string MachineCode)
        {
            try
            {


                if (connectionIndicator == false)
                {
                    con.Open();
                    connectionIndicator = true;
                }
                SqlCommand cmd = new SqlCommand("usp_SaveProcess", con);
                cmd.CommandType = CommandType.StoredProcedure;


                cmd.Parameters.AddWithValue("@itemId", SqlDbType.Int).Value = itemId;
                cmd.Parameters.AddWithValue("@ProcessName", SqlDbType.VarChar).Value = ProcessName;
                cmd.Parameters.AddWithValue("@NoofOperators", SqlDbType.Int).Value = NoofOperators;
                cmd.Parameters.AddWithValue("@CycleTime", SqlDbType.Int).Value = CycleTime;
                cmd.Parameters.AddWithValue("@ratingFactor", SqlDbType.Decimal).Value = ratingFactor;
                cmd.Parameters.AddWithValue("@Capacity", SqlDbType.Decimal).Value = Capacity;
                cmd.Parameters.AddWithValue("@UserId", SqlDbType.Int).Value = UserId;
                cmd.Parameters.AddWithValue("@MachineCode", SqlDbType.VarChar).Value = MachineCode;



                DataTable dt = new DataTable();

                int rows = cmd.ExecuteNonQuery(); ;

                return rows;



            }
            catch (Exception exp)
            {
                throw exp;
            }
            finally
            {
                if (connectionIndicator)
                {
                    con.Close();
                    connectionIndicator = false;
                }
            }

        }


        public static int SaveMaterial(int itemId, string Name, string Code, string Unit, decimal Multiple, decimal Cost)
        {
            try
            {


                if (connectionIndicator == false)
                {
                    con.Open();
                    connectionIndicator = true;
                }
                SqlCommand cmd = new SqlCommand("usp_SaveMaterial", con);
                cmd.CommandType = CommandType.StoredProcedure;


                cmd.Parameters.AddWithValue("@itemId", SqlDbType.Int).Value = itemId;
                cmd.Parameters.AddWithValue("@Name", SqlDbType.VarChar).Value = Name;
                cmd.Parameters.AddWithValue("@Code", SqlDbType.VarChar).Value = Code;
                cmd.Parameters.AddWithValue("@Unit", SqlDbType.VarChar).Value = Unit;
                cmd.Parameters.AddWithValue("@Multiple", SqlDbType.Decimal).Value = Multiple;
                cmd.Parameters.AddWithValue("@Cost", SqlDbType.Decimal).Value = Cost;
                cmd.Parameters.AddWithValue("@UserId", SqlDbType.Int).Value = UserId;


                DataTable dt = new DataTable();

                int rows = cmd.ExecuteNonQuery(); ;

                return rows;



            }
            catch (Exception exp)
            {
                throw exp;
            }
            finally
            {
                if (connectionIndicator)
                {
                    con.Close();
                    connectionIndicator = false;
                }
            }

        }

        public static DataTable InitializeItemDropDownProcesses(int itemId)
        {

            try
            {


                if (connectionIndicator == false)
                {
                    con.Open();
                    connectionIndicator = true;
                }
                SqlCommand cmd = new SqlCommand("Usp_ShowAllItemsProcessinDropDown", con);
                cmd.CommandType = CommandType.StoredProcedure;


                cmd.Parameters.AddWithValue("@ItemId", SqlDbType.Int).Value = itemId;
                DataTable dt = new DataTable();

                dt.Load(cmd.ExecuteReader());

                return dt;



            }
            catch (Exception exp)
            {
                throw exp;
            }
            finally
            {
                if (connectionIndicator)
                {
                    con.Close();
                    connectionIndicator = false;
                }
            }
        }

        public static DataTable InitializeItemMachineDropDownProcesses(int itemId, string MachineCode)
        {

            try
            {


                if (connectionIndicator == false)
                {
                    con.Open();
                    connectionIndicator = true;
                }
                SqlCommand cmd = new SqlCommand("Usp_ShowAllItemsProcessesinDropDown", con);
                cmd.CommandType = CommandType.StoredProcedure;


                cmd.Parameters.AddWithValue("@ItemId", SqlDbType.Int).Value = itemId;
                cmd.Parameters.AddWithValue("@MachineCode", SqlDbType.VarChar).Value = MachineCode;
                DataTable dt = new DataTable();

                dt.Load(cmd.ExecuteReader());

                return dt;



            }
            catch (Exception exp)
            {
                throw exp;
            }
            finally
            {
                if (connectionIndicator)
                {
                    con.Close();
                    connectionIndicator = false;
                }
            }
        }

        public static DataTable InitializeItemDropDownMaterials(int itemId)
        {

            try
            {


                if (connectionIndicator == false)
                {
                    con.Open();
                    connectionIndicator = true;
                }
                SqlCommand cmd = new SqlCommand("Usp_ShowAllItemsMaterialsinDropDown", con);
                cmd.CommandType = CommandType.StoredProcedure;


                cmd.Parameters.AddWithValue("@ItemId", SqlDbType.Int).Value = itemId;
                DataTable dt = new DataTable();

                dt.Load(cmd.ExecuteReader());

                return dt;



            }
            catch (Exception exp)
            {
                throw exp;
            }
            finally
            {
                if (connectionIndicator)
                {
                    con.Close();
                    connectionIndicator = false;
                }
            }
        }

        public static int EditProcess(int itemId, string ProcessName, int NoofOperators, decimal CycleTime, decimal RatingFactor, decimal Capacity, int ProcessId,string Machine_Code)
        {
            try
            {


                if (connectionIndicator == false)
                {
                    con.Open();
                    connectionIndicator = true;
                }
                SqlCommand cmd = new SqlCommand("usp_EditProcess", con);
                cmd.CommandType = CommandType.StoredProcedure;


                cmd.Parameters.AddWithValue("@itemId", SqlDbType.Int).Value = itemId;
                cmd.Parameters.AddWithValue("@ProcessName", SqlDbType.VarChar).Value = ProcessName;
                cmd.Parameters.AddWithValue("@NoofOperators", SqlDbType.Int).Value = NoofOperators;
                cmd.Parameters.AddWithValue("@CycleTime", SqlDbType.Int).Value = CycleTime;
                cmd.Parameters.AddWithValue("@ratingFactor", SqlDbType.Decimal).Value = RatingFactor;
                cmd.Parameters.AddWithValue("@Capacity", SqlDbType.Decimal).Value = Capacity;
                cmd.Parameters.AddWithValue("@UserId", SqlDbType.Int).Value = UserId;
                cmd.Parameters.AddWithValue("@ProcessId", SqlDbType.Int).Value = ProcessId;
                cmd.Parameters.AddWithValue("@MachineCode", SqlDbType.VarChar).Value = Machine_Code;


                DataTable dt = new DataTable();

                int rows = cmd.ExecuteNonQuery();


                return rows;



            }
            catch (Exception exp)
            {
                throw exp;
            }
            finally
            {
                if (connectionIndicator)
                {
                    con.Close();
                    connectionIndicator = false;
                }
            }
        }


        public static int EditMaterial(int itemId, string Name, string Code, string Unit, decimal Multiple, decimal Cost, int MaterialId)
        {
            try
            {


                if (connectionIndicator == false)
                {
                    con.Open();
                    connectionIndicator = true;
                }
                SqlCommand cmd = new SqlCommand("usp_EditMaterial", con);
                cmd.CommandType = CommandType.StoredProcedure;


                cmd.Parameters.AddWithValue("@itemId", SqlDbType.Int).Value = itemId;
                cmd.Parameters.AddWithValue("@Name", SqlDbType.VarChar).Value = Name;
                cmd.Parameters.AddWithValue("@Code", SqlDbType.VarChar).Value = Code;
                cmd.Parameters.AddWithValue("@Unit", SqlDbType.VarChar).Value = Unit;
                cmd.Parameters.AddWithValue("@Multiple", SqlDbType.Decimal).Value = Multiple;
                cmd.Parameters.AddWithValue("@Cost", SqlDbType.Decimal).Value = Cost;
                cmd.Parameters.AddWithValue("@UserId", SqlDbType.Int).Value = UserId;
                cmd.Parameters.AddWithValue("@MaterialId", SqlDbType.Int).Value = MaterialId;


                DataTable dt = new DataTable();

                int rows = cmd.ExecuteNonQuery();


                return rows;



            }
            catch (Exception exp)
            {
                throw exp;
            }
            finally
            {
                if (connectionIndicator)
                {
                    con.Close();
                    connectionIndicator = false;
                }
            }
        }

        public static DataTable GetProcessDetails(int ProcessId)
        {
            try
            {


                if (connectionIndicator == false)
                {
                    con.Open();
                    connectionIndicator = true;
                }
                SqlCommand cmd = new SqlCommand("usp_GetProcess", con);
                cmd.CommandType = CommandType.StoredProcedure;



                cmd.Parameters.AddWithValue("@ProcessId", SqlDbType.Int).Value = ProcessId;





                DataTable dt = new DataTable();

                dt.Load(cmd.ExecuteReader());

                return dt;



            }
            catch (Exception exp)
            {
                throw exp;
            }
            finally
            {
                if (connectionIndicator)
                {
                    con.Close();
                    connectionIndicator = false;
                }
            }
        }
        public static int GetProcessId(string ProcessName, int ItemId)
        {
            try
            {


                if (connectionIndicator == false)
                {
                    con.Open();
                    connectionIndicator = true;
                }
                SqlCommand cmd = new SqlCommand("usp_GetProcessId", con);
                cmd.CommandType = CommandType.StoredProcedure;



                cmd.Parameters.AddWithValue("@ItemId", SqlDbType.VarChar).Value = ItemId;

                cmd.Parameters.AddWithValue("@ProcessName", SqlDbType.VarChar).Value = ProcessName;



                DataTable dt = new DataTable();

                dt.Load(cmd.ExecuteReader());

                return Convert.ToInt32(dt.Rows[0][0]);



            }
            catch (Exception exp)
            {
                throw exp;
            }
            finally
            {
                if (connectionIndicator)
                {
                    con.Close();
                    connectionIndicator = false;
                }
            }

        }

        public static DataTable GetMaterials(string Code, int ItemId )
        {
            try
            {


                if (connectionIndicator == false)
                {
                    con.Open();
                    connectionIndicator = true;
                }
                SqlCommand cmd = new SqlCommand("usp_GetMaterialsDetails", con);
                cmd.CommandType = CommandType.StoredProcedure;



                cmd.Parameters.AddWithValue("@ItemId", SqlDbType.Int).Value = ItemId;

                cmd.Parameters.AddWithValue("@Code", SqlDbType.VarChar).Value = Code;

                


                DataTable dt = new DataTable();

                dt.Load(cmd.ExecuteReader());

                return dt;



            }
            catch (Exception exp)
            {
                throw exp;
            }
            finally
            {
                if (connectionIndicator)
                {
                    con.Close();
                    connectionIndicator = false;
                }
            }

        }

        public static DataTable GetMaterialsDaily(string Code, int ItemId, string Date)
        {
            try
            {


                if (connectionIndicator == false)
                {
                    con.Open();
                    connectionIndicator = true;
                }
                SqlCommand cmd = new SqlCommand("Usp_GetDailyEntryActualValueMaterials", con);
                cmd.CommandType = CommandType.StoredProcedure;



                cmd.Parameters.AddWithValue("@ItemId", SqlDbType.Int).Value = ItemId;

                cmd.Parameters.AddWithValue("@Code", SqlDbType.VarChar).Value = Code;

                cmd.Parameters.AddWithValue("@Date", SqlDbType.VarChar).Value = Date;




                DataTable dt = new DataTable();

                dt.Load(cmd.ExecuteReader());

                return dt;



            }
            catch (Exception exp)
            {
                throw exp;
            }
            finally
            {
                if (connectionIndicator)
                {
                    con.Close();
                    connectionIndicator = false;
                }
            }

        }
        public static DataTable GetProcessIdAndCapacity(string ProcessName, int ItemId)
        {
            try
            {


                if (connectionIndicator == false)
                {
                    con.Open();
                    connectionIndicator = true;
                }
                SqlCommand cmd = new SqlCommand("usp_GetProcessIdAndCapacity", con);
                cmd.CommandType = CommandType.StoredProcedure;



                cmd.Parameters.AddWithValue("@ItemId", SqlDbType.Int).Value = ItemId;

                cmd.Parameters.AddWithValue("@ProcessName", SqlDbType.VarChar).Value = ProcessName;



                DataTable dt = new DataTable();

                dt.Load(cmd.ExecuteReader());

                return dt;



            }
            catch (Exception exp)
            {
                throw exp;
            }
            finally
            {
                if (connectionIndicator)
                {
                    con.Close();
                    connectionIndicator = false;
                }
            }

        }

        public static DataTable GetProcessDetailsOnChange(int ProcessId, int ItemId, string Date)
        {
            try
            {


                if (connectionIndicator == false)
                {
                    con.Open();
                    connectionIndicator = true;
                }
                SqlCommand cmd = new SqlCommand("usp_GetProcessDetailsOnChange", con);
                cmd.CommandType = CommandType.StoredProcedure;


                cmd.Parameters.AddWithValue("@ProcessId", SqlDbType.Int).Value = ProcessId;
                cmd.Parameters.AddWithValue("@ItemId", SqlDbType.Int).Value = ItemId;
                cmd.Parameters.AddWithValue("@Date", SqlDbType.Int).Value = Date;





                DataTable dt = new DataTable();

                dt.Load(cmd.ExecuteReader());

                return dt;



            }
            catch (Exception exp)
            {
                throw exp;
            }
            finally
            {
                if (connectionIndicator)
                {
                    con.Close();
                    connectionIndicator = false;
                }
            }

        }
        public static DataTable GetProcesses(int ItemId)
        {
            try
            {


                if (connectionIndicator == false)
                {
                    con.Open();
                    connectionIndicator = true;
                }
                SqlCommand cmd = new SqlCommand("usp_GetProcessofItem", con);
                cmd.CommandType = CommandType.StoredProcedure;



                cmd.Parameters.AddWithValue("@ItemId", SqlDbType.Int).Value = ItemId;





                DataTable dt = new DataTable();

                dt.Load(cmd.ExecuteReader());

                return dt;



            }
            catch (Exception exp)
            {
                throw exp;
            }
            finally
            {
                if (connectionIndicator)
                {
                    con.Close();
                    connectionIndicator = false;
                }
            }
        }


        public static DataTable GetProcessesForMachine(int ItemId,int MachineId)
        {
            try
            {


                if (connectionIndicator == false)
                {
                    con.Open();
                    connectionIndicator = true;
                }
                SqlCommand cmd = new SqlCommand("usp_GetProcessofItemMachine", con);
                cmd.CommandType = CommandType.StoredProcedure;



                cmd.Parameters.AddWithValue("@ItemId", SqlDbType.Int).Value = ItemId;
                cmd.Parameters.AddWithValue("@MachineId", SqlDbType.Int).Value = MachineId;





                DataTable dt = new DataTable();

                dt.Load(cmd.ExecuteReader());

                return dt;



            }
            catch (Exception exp)
            {
                throw exp;
            }
            finally
            {
                if (connectionIndicator)
                {
                    con.Close();
                    connectionIndicator = false;
                }
            }
        }


        public static int SaveDailyEntryProcess(int itemId, int ProcessId, int Volume, int NoOfOperator, decimal Hours, string datetime, decimal AcutalManHours, decimal plannedManHour, decimal Efficiency)
        {
            try
            {


                if (connectionIndicator == false)
                {
                    con.Open();
                    connectionIndicator = true;
                }
                SqlCommand cmd = new SqlCommand("usp_SaveProcessDailyEntry", con);
                cmd.CommandType = CommandType.StoredProcedure;


                cmd.Parameters.AddWithValue("@itemId", SqlDbType.Int).Value = itemId;
                cmd.Parameters.AddWithValue("@ProcessId", SqlDbType.Int).Value = ProcessId;
                cmd.Parameters.AddWithValue("@Volume", SqlDbType.Int).Value = Volume;
                cmd.Parameters.AddWithValue("@NoOfOperator", SqlDbType.Int).Value = NoOfOperator;
                cmd.Parameters.AddWithValue("@Hours", SqlDbType.Decimal).Value = Hours;
                cmd.Parameters.AddWithValue("@Date", SqlDbType.VarChar).Value = datetime;
                cmd.Parameters.AddWithValue("@ActualManHours", SqlDbType.Decimal).Value = AcutalManHours;
                cmd.Parameters.AddWithValue("@plannedManHour", SqlDbType.Decimal).Value = plannedManHour;
                cmd.Parameters.AddWithValue("@Efficiency", SqlDbType.Decimal).Value = Efficiency;
                cmd.Parameters.AddWithValue("@UserId", SqlDbType.Int).Value = UserId;




                DataTable dt = new DataTable();

                int rows = cmd.ExecuteNonQuery();


                return rows;



            }
            catch (Exception exp)
            {
                throw exp;
            }
            finally
            {
                if (connectionIndicator)
                {
                    con.Close();
                    connectionIndicator = false;
                }
            }
        }

        public static int EditDailyEntryProductionEfficiency(int itemId, int ProcessId, int Volume, int NoOfOperator, decimal Hours, string datetime, decimal AcutalManHours, decimal plannedManHour, decimal Efficiency)
        {
            try
            {


                if (connectionIndicator == false)
                {
                    con.Open();
                    connectionIndicator = true;
                }
                SqlCommand cmd = new SqlCommand("usp_EditProcessDailyEntry", con);
                cmd.CommandType = CommandType.StoredProcedure;


                cmd.Parameters.AddWithValue("@itemId", SqlDbType.Int).Value = itemId;
                cmd.Parameters.AddWithValue("@ProcessId", SqlDbType.Int).Value = ProcessId;
                cmd.Parameters.AddWithValue("@Volume", SqlDbType.Int).Value = Volume;
                cmd.Parameters.AddWithValue("@NoOfOperator", SqlDbType.Int).Value = NoOfOperator;
                cmd.Parameters.AddWithValue("@Hours", SqlDbType.Decimal).Value = Hours;
                cmd.Parameters.AddWithValue("@Date", SqlDbType.VarChar).Value = datetime;
                cmd.Parameters.AddWithValue("@ActualManHours", SqlDbType.Decimal).Value = AcutalManHours;
                cmd.Parameters.AddWithValue("@plannedManHour", SqlDbType.Decimal).Value = plannedManHour;
                cmd.Parameters.AddWithValue("@Efficiency", SqlDbType.Decimal).Value = Efficiency;
                cmd.Parameters.AddWithValue("@UserId", SqlDbType.Int).Value = UserId;




                DataTable dt = new DataTable();

                int rows = cmd.ExecuteNonQuery();


                return rows;



            }
            catch (Exception exp)
            {
                throw exp;
            }
            finally
            {
                if (connectionIndicator)
                {
                    con.Close();
                    connectionIndicator = false;
                }
            }
        }

        public static DataTable GetMonths()
        {
            try
            {


                if (connectionIndicator == false)
                {
                    con.Open();
                    connectionIndicator = true;
                }
                SqlCommand cmd = new SqlCommand("usp_GetMonths", con);
                cmd.CommandType = CommandType.StoredProcedure;

                DataTable dt = new DataTable();

                dt.Load(cmd.ExecuteReader());

                return dt;



            }
            catch (Exception exp)
            {
                throw exp;
            }
            finally
            {
                if (connectionIndicator)
                {
                    con.Close();
                    connectionIndicator = false;
                }
            }
        }

        public static int GetMonthId(string MonthName)
        {
            try
            {


                if (connectionIndicator == false)
                {
                    con.Open();
                    connectionIndicator = true;
                }
                SqlCommand cmd = new SqlCommand("usp_GetMonthId", con);
                cmd.CommandType = CommandType.StoredProcedure;



                cmd.Parameters.AddWithValue("@MonthName", SqlDbType.VarChar).Value = MonthName;

                DataTable dt = new DataTable();

                dt.Load(cmd.ExecuteReader());

                return Convert.ToInt32(dt.Rows[0][0]);



            }
            catch (Exception exp)
            {
                throw exp;
            }
            finally
            {
                if (connectionIndicator)
                {
                    con.Close();
                    connectionIndicator = false;
                }
            }
        }

        public static int InsertTargetReworkRatio(int Item_Id, int Month_Id, decimal Target, int year)
        {

            try
            {


                if (connectionIndicator == false)
                {
                    con.Open();
                    connectionIndicator = true;
                }
                SqlCommand cmd = new SqlCommand("Usp_SaveTargetReworkRatio", con);
                cmd.CommandType = CommandType.StoredProcedure;


                cmd.Parameters.AddWithValue("@ItemId", SqlDbType.Int).Value = Item_Id;
                cmd.Parameters.AddWithValue("@MonthId", SqlDbType.Int).Value = Month_Id;
                cmd.Parameters.AddWithValue("@Target", SqlDbType.Decimal).Value = Target;
                cmd.Parameters.AddWithValue("@UserId", SqlDbType.Int).Value = UserId;
                cmd.Parameters.AddWithValue("@Year", SqlDbType.Int).Value = year;


                DataTable dt = new DataTable();

                int rows = cmd.ExecuteNonQuery();

                return rows;



            }
            catch (Exception exp)
            {
                throw exp;
            }
            finally
            {
                if (connectionIndicator)
                {
                    con.Close();
                    connectionIndicator = false;
                }
            }
        }

        public static int InsertTargetMaterialYieldVariance(int Item_Id, int Month_Id, decimal Target, int year)
        {

            try
            {


                if (connectionIndicator == false)
                {
                    con.Open();
                    connectionIndicator = true;
                }
                SqlCommand cmd = new SqlCommand("Usp_SaveTargetMaterialYieldVariance", con);
                cmd.CommandType = CommandType.StoredProcedure;


                cmd.Parameters.AddWithValue("@ItemId", SqlDbType.Int).Value = Item_Id;
                cmd.Parameters.AddWithValue("@MonthId", SqlDbType.Int).Value = Month_Id;
                cmd.Parameters.AddWithValue("@Target", SqlDbType.Decimal).Value = Target;
                cmd.Parameters.AddWithValue("@UserId", SqlDbType.Int).Value = UserId;
                cmd.Parameters.AddWithValue("@Year", SqlDbType.Int).Value = year;

                DataTable dt = new DataTable();

                int rows = cmd.ExecuteNonQuery();

                return rows;



            }
            catch (Exception exp)
            {
                throw exp;
            }
            finally
            {
                if (connectionIndicator)
                {
                    con.Close();
                    connectionIndicator = false;
                }
            }
        }


        public static int InsertTargetEquipmentFailureWorkShop(int WorkShopId, int Month_Id, decimal Target, int NoOfDays, int year)
        {

            try
            {


                if (connectionIndicator == false)
                {
                    con.Open();
                    connectionIndicator = true;
                }
                SqlCommand cmd = new SqlCommand("Usp_SaveTargetEquipmentFailureRatebyWorkShopMonthly", con);
                cmd.CommandType = CommandType.StoredProcedure;


                cmd.Parameters.AddWithValue("@WorkShopId", SqlDbType.Int).Value = WorkShopId;
                cmd.Parameters.AddWithValue("@MonthId", SqlDbType.Int).Value = Month_Id;
                cmd.Parameters.AddWithValue("@Target", SqlDbType.Decimal).Value = Target;
                cmd.Parameters.AddWithValue("@UserId", SqlDbType.Int).Value = UserId;
                cmd.Parameters.AddWithValue("@NoOfDays", SqlDbType.Int).Value = NoOfDays;
                cmd.Parameters.AddWithValue("@year", SqlDbType.Int).Value = year;

                DataTable dt = new DataTable();

                int rows = cmd.ExecuteNonQuery();

                return rows;



            }
            catch (Exception exp)
            {
                throw exp;
            }
            finally
            {
                if (connectionIndicator)
                {
                    con.Close();
                    connectionIndicator = false;
                }
            }
        }


        public static int InsertTargetOEE(int ItemId, int Month_Id, decimal Target, int NoOfDays, int year)
        {

            try
            {


                if (connectionIndicator == false)
                {
                    con.Open();
                    connectionIndicator = true;
                }
                SqlCommand cmd = new SqlCommand("Usp_SaveOEEMonthly", con);
                cmd.CommandType = CommandType.StoredProcedure;


                cmd.Parameters.AddWithValue("@ItemId", SqlDbType.Int).Value = ItemId;
                cmd.Parameters.AddWithValue("@MonthId", SqlDbType.Int).Value = Month_Id;
                cmd.Parameters.AddWithValue("@Target", SqlDbType.Decimal).Value = Target;
                cmd.Parameters.AddWithValue("@NoOfDays", SqlDbType.Int).Value = NoOfDays;
                cmd.Parameters.AddWithValue("@UserId", SqlDbType.Int).Value = UserId;
                cmd.Parameters.AddWithValue("@Year", SqlDbType.Int).Value = year;
               

                DataTable dt = new DataTable();

                int rows = cmd.ExecuteNonQuery();

                return rows;



            }
            catch (Exception exp)
            {
                throw exp;
            }
            finally
            {
                if (connectionIndicator)
                {
                    con.Close();
                    connectionIndicator = false;
                }
            }
        }
        public static int InsertTargetProductionAchievement(int Item_Id, int Month_Id, decimal Target, int year)
        {

            try
            {


                if (connectionIndicator == false)
                {
                    con.Open();
                    connectionIndicator = true;
                }
                SqlCommand cmd = new SqlCommand("Usp_SaveTargetProductionAchievementRate", con);
                cmd.CommandType = CommandType.StoredProcedure;


                cmd.Parameters.AddWithValue("@ItemId", SqlDbType.Int).Value = Item_Id;
                cmd.Parameters.AddWithValue("@MonthId", SqlDbType.Int).Value = Month_Id;
                cmd.Parameters.AddWithValue("@Target", SqlDbType.Decimal).Value = Target;
                cmd.Parameters.AddWithValue("@UserId", SqlDbType.Int).Value = UserId;
                cmd.Parameters.AddWithValue("@Year", SqlDbType.Int).Value = year;
                
                DataTable dt = new DataTable();

                int rows = cmd.ExecuteNonQuery();

                return rows;



            }
            catch (Exception exp)
            {
                throw exp;
            }
            finally
            {
                if (connectionIndicator)
                {
                    con.Close();
                    connectionIndicator = false;
                }
            }
        }

        public static int InsertTargetCustomerClaims(int Item_Id, int Month_Id, decimal Target, int year)
        {

            try
            {


                if (connectionIndicator == false)
                {
                    con.Open();
                    connectionIndicator = true;
                }
                SqlCommand cmd = new SqlCommand("Usp_SaveTargetCustomerClaims", con);
                cmd.CommandType = CommandType.StoredProcedure;


                cmd.Parameters.AddWithValue("@ItemId", SqlDbType.Int).Value = Item_Id;
                cmd.Parameters.AddWithValue("@MonthId", SqlDbType.Int).Value = Month_Id;
                cmd.Parameters.AddWithValue("@Target", SqlDbType.Decimal).Value = Target;
                cmd.Parameters.AddWithValue("@UserId", SqlDbType.Int).Value = UserId;
                cmd.Parameters.AddWithValue("@Year", SqlDbType.Int).Value = year;

                DataTable dt = new DataTable();

                int rows = cmd.ExecuteNonQuery();

                return rows;



            }
            catch (Exception exp)
            {
                throw exp;
            }
            finally
            {
                if (connectionIndicator)
                {
                    con.Close();
                    connectionIndicator = false;
                }
            }
        }
        public static int InsertTargetDelivaryComplianceRatio(int Item_Id, int Month_Id, decimal Target, int year)
        {

            try
            {


                if (connectionIndicator == false)
                {
                    con.Open();
                    connectionIndicator = true;
                }
                SqlCommand cmd = new SqlCommand("Usp_SaveTargetDelivaryComplianceRatio", con);
                cmd.CommandType = CommandType.StoredProcedure;


                cmd.Parameters.AddWithValue("@ItemId", SqlDbType.Int).Value = Item_Id;
                cmd.Parameters.AddWithValue("@MonthId", SqlDbType.Int).Value = Month_Id;
                cmd.Parameters.AddWithValue("@Target", SqlDbType.Decimal).Value = Target;
                cmd.Parameters.AddWithValue("@UserId", SqlDbType.Int).Value = UserId;
                cmd.Parameters.AddWithValue("@Year", SqlDbType.Int).Value = year;

                DataTable dt = new DataTable();

                int rows = cmd.ExecuteNonQuery();

                return rows;



            }
            catch (Exception exp)
            {
                throw exp;
            }
            finally
            {
                if (connectionIndicator)
                {
                    con.Close();
                    connectionIndicator = false;
                }
            }
        }

        public static int InsertTargetReworkCost(int Item_Id, int Month_Id, decimal Target, int year)
        {

            try
            {


                if (connectionIndicator == false)
                {
                    con.Open();
                    connectionIndicator = true;
                }
                SqlCommand cmd = new SqlCommand("Usp_SaveTargetReworkCost", con);
                cmd.CommandType = CommandType.StoredProcedure;


                cmd.Parameters.AddWithValue("@ItemId", SqlDbType.Int).Value = Item_Id;
                cmd.Parameters.AddWithValue("@MonthId", SqlDbType.Int).Value = Month_Id;
                cmd.Parameters.AddWithValue("@Target", SqlDbType.Decimal).Value = Target;
                cmd.Parameters.AddWithValue("@UserId", SqlDbType.Int).Value = UserId;
                cmd.Parameters.AddWithValue("@Year", SqlDbType.Int).Value = year;

                DataTable dt = new DataTable();

                int rows = cmd.ExecuteNonQuery();

                return rows;



            }
            catch (Exception exp)
            {
                throw exp;
            }
            finally
            {
                if (connectionIndicator)
                {
                    con.Close();
                    connectionIndicator = false;
                }
            }
        }

        public static int InsertTargetInhouseRejcectionkCost(int Item_Id, int Month_Id, decimal Target, int  year)
        {

            try
            {


                if (connectionIndicator == false)
                {
                    con.Open();
                    connectionIndicator = true;
                }
                SqlCommand cmd = new SqlCommand("Usp_SaveTargetInhouseRejCost", con);
                cmd.CommandType = CommandType.StoredProcedure;


                cmd.Parameters.AddWithValue("@ItemId", SqlDbType.Int).Value = Item_Id;
                cmd.Parameters.AddWithValue("@MonthId", SqlDbType.Int).Value = Month_Id;
                cmd.Parameters.AddWithValue("@Target", SqlDbType.Decimal).Value = Target;
                cmd.Parameters.AddWithValue("@UserId", SqlDbType.Int).Value = UserId;
                cmd.Parameters.AddWithValue("@Year", SqlDbType.Int).Value = year;

                DataTable dt = new DataTable();

                int rows = cmd.ExecuteNonQuery();

                return rows;



            }
            catch (Exception exp)
            {
                throw exp;
            }
            finally
            {
                if (connectionIndicator)
                {
                    con.Close();
                    connectionIndicator = false;
                }
            }
        }

        public static int InsertTargetCustRejcectionkCost(int item_Id, int month_Id, decimal target, int  year)
        {

            try
            {


                if (connectionIndicator == false)
                {
                    con.Open();
                    connectionIndicator = true;
                }
                SqlCommand cmd = new SqlCommand("Usp_SaveTargetCustomerRejCost", con);
                cmd.CommandType = CommandType.StoredProcedure;


                cmd.Parameters.AddWithValue("@ItemId", SqlDbType.Int).Value = item_Id;
                cmd.Parameters.AddWithValue("@MonthId", SqlDbType.Int).Value = month_Id;
                cmd.Parameters.AddWithValue("@Target", SqlDbType.Decimal).Value = target;
                cmd.Parameters.AddWithValue("@UserId", SqlDbType.Int).Value = UserId;
                cmd.Parameters.AddWithValue("@Year", SqlDbType.Int).Value = year;

                DataTable dt = new DataTable();

                int rows = cmd.ExecuteNonQuery();

                return rows;



            }
            catch (Exception exp)
            {
                throw exp;
            }
            finally
            {
                if (connectionIndicator)
                {
                    con.Close();
                    connectionIndicator = false;
                }
            }
        }
        public static int InsertTargetCustomerRejectionRatio(int Item_Id, int Month_Id, decimal Target,int year)
        {

            try
            {


                if (connectionIndicator == false)
                {
                    con.Open();
                    connectionIndicator = true;
                }
                SqlCommand cmd = new SqlCommand("Usp_SaveTargetCustomerRejectionRatio", con);
                cmd.CommandType = CommandType.StoredProcedure;


                cmd.Parameters.AddWithValue("@ItemId", SqlDbType.Int).Value = Item_Id;
                cmd.Parameters.AddWithValue("@MonthId", SqlDbType.Int).Value = Month_Id;
                cmd.Parameters.AddWithValue("@Target", SqlDbType.Decimal).Value = Target;
                cmd.Parameters.AddWithValue("@Year", SqlDbType.Decimal).Value = year;
                cmd.Parameters.AddWithValue("@UserId", SqlDbType.Int).Value = UserId;

                DataTable dt = new DataTable();

                int rows = cmd.ExecuteNonQuery();

                return rows;



            }
            catch (Exception exp)
            {
                throw exp;
            }
            finally
            {
                if (connectionIndicator)
                {
                    con.Close();
                    connectionIndicator = false;
                }
            }
        }


        public static int InsertTargetAttendanceRatio(int Month_Id, decimal Target, int noOfDays, int years)
        {

            try
            {


                if (connectionIndicator == false)
                {
                    con.Open();
                    connectionIndicator = true;
                }
                SqlCommand cmd = new SqlCommand("Usp_SaveTargetAttendanceRatio", con);
                cmd.CommandType = CommandType.StoredProcedure;


               
                cmd.Parameters.AddWithValue("@MonthId", SqlDbType.Int).Value = Month_Id;
                cmd.Parameters.AddWithValue("@Target", SqlDbType.Decimal).Value = Target;
                cmd.Parameters.AddWithValue("@UserId", SqlDbType.Int).Value = UserId;
                cmd.Parameters.AddWithValue("@NoOfDays", SqlDbType.Int).Value = noOfDays;
                cmd.Parameters.AddWithValue("@Year", SqlDbType.Int).Value = years;


                DataTable dt = new DataTable();

                int rows = cmd.ExecuteNonQuery();

                return rows;



            }
            catch (Exception exp)
            {
                throw exp;
            }
            finally
            {
                if (connectionIndicator)
                {
                    con.Close();
                    connectionIndicator = false;
                }
            }
        }

        public static int InsertTargetProductionEfficiency(int Item_Id, int Month_Id, decimal Target, int year)
        {

            try
            {


                if (connectionIndicator == false)
                {
                    con.Open();
                    connectionIndicator = true;
                }
                SqlCommand cmd = new SqlCommand("Usp_SaveTargetProductionEfficiency", con);
                cmd.CommandType = CommandType.StoredProcedure;


                cmd.Parameters.AddWithValue("@ItemId", SqlDbType.Int).Value = Item_Id;
                cmd.Parameters.AddWithValue("@MonthId", SqlDbType.Int).Value = Month_Id;
                cmd.Parameters.AddWithValue("@Target", SqlDbType.Decimal).Value = Target;
                cmd.Parameters.AddWithValue("@UserId", SqlDbType.Int).Value = UserId;
                cmd.Parameters.AddWithValue("@Year", SqlDbType.Int).Value = year;


                DataTable dt = new DataTable();

                int rows = cmd.ExecuteNonQuery();

                return rows;



            }
            catch (Exception exp)
            {
                throw exp;
            }
            finally
            {
                if (connectionIndicator)
                {
                    con.Close();
                    connectionIndicator = false;
                }
            }
        }
        public static int InsertTargetInhouseRejectionRatio(int Item_Id, int Month_Id, decimal Target, int year)
        {

            try
            {


                if (connectionIndicator == false)
                {
                    con.Open();
                    connectionIndicator = true;
                }
                SqlCommand cmd = new SqlCommand("Usp_SaveTargetInhouseRejectionRatio", con);
                cmd.CommandType = CommandType.StoredProcedure;


                cmd.Parameters.AddWithValue("@ItemId", SqlDbType.Int).Value = Item_Id;
                cmd.Parameters.AddWithValue("@MonthId", SqlDbType.Int).Value = Month_Id;
                cmd.Parameters.AddWithValue("@Target", SqlDbType.Decimal).Value = Target;
                cmd.Parameters.AddWithValue("@UserId", SqlDbType.Int).Value = UserId;
                cmd.Parameters.AddWithValue("@Year", SqlDbType.Int).Value = year;

                DataTable dt = new DataTable();

                int rows = cmd.ExecuteNonQuery();

                return rows;



            }
            catch (Exception exp)
            {
                throw exp;
            }
            finally
            {
                if (connectionIndicator)
                {
                    con.Close();
                    connectionIndicator = false;
                }
            }
        }

        public static DataTable GetCalculationsReworkRatioMonthly(int Item_Id, int year)
        {

            try
            {


                if (connectionIndicator == false)
                {
                    con.Open();
                    connectionIndicator = true;
                }
                SqlCommand cmd = new SqlCommand("usp_GetMonthsReworkRatio", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ItemId", SqlDbType.Int).Value = Item_Id;
                cmd.Parameters.AddWithValue("@Year", SqlDbType.Int).Value = year;
              

                DataTable dt = new DataTable();

                dt.Load(cmd.ExecuteReader());

                return dt;



            }
            catch (Exception exp)
            {
                throw exp;
            }
            finally
            {
                if (connectionIndicator)
                {
                    con.Close();
                    connectionIndicator = false;
                }
            }
        }


        public static DataTable GetCalculationsReworkRatioMonthDate(int Item_Id,int MonthId, int year)
        {

            try
            {


                if (connectionIndicator == false)
                {
                    con.Open();
                    connectionIndicator = true;
                }
                SqlCommand cmd = new SqlCommand("usp_GetMonthsReworkRatioMonthlyDaybyDay", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ItemId", SqlDbType.Int).Value = Item_Id;
                cmd.Parameters.AddWithValue("@MonthId", SqlDbType.Int).Value = MonthId;
                cmd.Parameters.AddWithValue("@Year", SqlDbType.Int).Value = year;


                DataTable dt = new DataTable();

                dt.Load(cmd.ExecuteReader());

                return dt;



            }
            catch (Exception exp)
            {
                throw exp;
            }
            finally
            {
                if (connectionIndicator)
                {
                    con.Close();
                    connectionIndicator = false;
                }
            }
        }


        public static DataTable GetCalculationsReworkCostMonthDate(int Item_Id, int MonthId, int year)
        {

            try
            {


                if (connectionIndicator == false)
                {
                    con.Open();
                    connectionIndicator = true;
                }
                SqlCommand cmd = new SqlCommand("usp_GetMonthsReworkCostMonthlyDaybyDay", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ItemId", SqlDbType.Int).Value = Item_Id;
                cmd.Parameters.AddWithValue("@MonthId", SqlDbType.Int).Value = MonthId;
                cmd.Parameters.AddWithValue("@Year", SqlDbType.Int).Value = year;


                DataTable dt = new DataTable();

                dt.Load(cmd.ExecuteReader());

                return dt;



            }
            catch (Exception exp)
            {
                throw exp;
            }
            finally
            {
                if (connectionIndicator)
                {
                    con.Close();
                    connectionIndicator = false;
                }
            }
        }

        public static DataTable GetCalculationsCustRejRatioMonthDate(int Item_Id, int MonthId, int year)
        {

            try
            {


                if (connectionIndicator == false)
                {
                    con.Open();
                    connectionIndicator = true;
                }
                SqlCommand cmd = new SqlCommand("usp_GetMonthsCustRejRatioMonthlyDaybyDay", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ItemId", SqlDbType.Int).Value = Item_Id;
                cmd.Parameters.AddWithValue("@MonthId", SqlDbType.Int).Value = MonthId;
                cmd.Parameters.AddWithValue("@Year", SqlDbType.Int).Value = year;


                DataTable dt = new DataTable();

                dt.Load(cmd.ExecuteReader());

                return dt;



            }
            catch (Exception exp)
            {
                throw exp;
            }
            finally
            {
                if (connectionIndicator)
                {
                    con.Close();
                    connectionIndicator = false;
                }
            }
        }

        public static DataTable GetCalculationsCustRejCostMonthDate(int Item_Id, int MonthId, int year)
        {

            try
            {


                if (connectionIndicator == false)
                {
                    con.Open();
                    connectionIndicator = true;
                }
                SqlCommand cmd = new SqlCommand("usp_GetMonthsCustRejCostMonthlyDaybyDay", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ItemId", SqlDbType.Int).Value = Item_Id;
                cmd.Parameters.AddWithValue("@MonthId", SqlDbType.Int).Value = MonthId;
                cmd.Parameters.AddWithValue("@Year", SqlDbType.Int).Value = year;


                DataTable dt = new DataTable();

                dt.Load(cmd.ExecuteReader());

                return dt;



            }
            catch (Exception exp)
            {
                throw exp;
            }
            finally
            {
                if (connectionIndicator)
                {
                    con.Close();
                    connectionIndicator = false;
                }
            }
        }

        public static DataTable GetCalculationsInhouseRejRatioMonthDate(int Item_Id, int MonthId, int year)
        {

            try
            {


                if (connectionIndicator == false)
                {
                    con.Open();
                    connectionIndicator = true;
                }
                SqlCommand cmd = new SqlCommand("usp_GetMonthsInhouseRejRatioMonthlyDaybyDay", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ItemId", SqlDbType.Int).Value = Item_Id;
                cmd.Parameters.AddWithValue("@MonthId", SqlDbType.Int).Value = MonthId;
                cmd.Parameters.AddWithValue("@Year", SqlDbType.Int).Value = year;


                DataTable dt = new DataTable();

                dt.Load(cmd.ExecuteReader());

                return dt;



            }
            catch (Exception exp)
            {
                throw exp;
            }
            finally
            {
                if (connectionIndicator)
                {
                    con.Close();
                    connectionIndicator = false;
                }
            }
        }

        public static DataTable GetCalculationsInhouseRejCostMonthDate(int Item_Id, int MonthId, int year)
        {

            try
            {


                if (connectionIndicator == false)
                {
                    con.Open();
                    connectionIndicator = true;
                }
                SqlCommand cmd = new SqlCommand("usp_GetMonthsInhouseRejCostMonthlyDaybyDay", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ItemId", SqlDbType.Int).Value = Item_Id;
                cmd.Parameters.AddWithValue("@MonthId", SqlDbType.Int).Value = MonthId;
                cmd.Parameters.AddWithValue("@Year", SqlDbType.Int).Value = year;


                DataTable dt = new DataTable();

                dt.Load(cmd.ExecuteReader());

                return dt;



            }
            catch (Exception exp)
            {
                throw exp;
            }
            finally
            {
                if (connectionIndicator)
                {
                    con.Close();
                    connectionIndicator = false;
                }
            }
        }


        public static DataTable GetCalculationsCustomerClaimsMonthDate(int Item_Id, int MonthId, int year)
        {

            try
            {


                if (connectionIndicator == false)
                {
                    con.Open();
                    connectionIndicator = true;
                }
                SqlCommand cmd = new SqlCommand("usp_GetMonthsCustomerClaimsMonthlyDaybyDay", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ItemId", SqlDbType.Int).Value = Item_Id;
                cmd.Parameters.AddWithValue("@MonthId", SqlDbType.Int).Value = MonthId;
                cmd.Parameters.AddWithValue("@Year", SqlDbType.Int).Value = year;


                DataTable dt = new DataTable();

                dt.Load(cmd.ExecuteReader());

                return dt;



            }
            catch (Exception exp)
            {
                throw exp;
            }
            finally
            {
                if (connectionIndicator)
                {
                    con.Close();
                    connectionIndicator = false;
                }
            }
        }


        public static DataTable GetCalculationsDelClaimsMonthDate(int Item_Id, int MonthId, int year)
        {

            try
            {


                if (connectionIndicator == false)
                {
                    con.Open();
                    connectionIndicator = true;
                }
                SqlCommand cmd = new SqlCommand("usp_GetMonthsDelCompMonthlyDaybyDay", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ItemId", SqlDbType.Int).Value = Item_Id;
                cmd.Parameters.AddWithValue("@MonthId", SqlDbType.Int).Value = MonthId;
                cmd.Parameters.AddWithValue("@Year", SqlDbType.Int).Value = year;


                DataTable dt = new DataTable();

                dt.Load(cmd.ExecuteReader());

                return dt;



            }
            catch (Exception exp)
            {
                throw exp;
            }
            finally
            {
                if (connectionIndicator)
                {
                    con.Close();
                    connectionIndicator = false;
                }
            }
        }

        public static DataTable GetCalculationsProdAchivMonthDate(int Item_Id, int MonthId, int year)
        {

            try
            {


                if (connectionIndicator == false)
                {
                    con.Open();
                    connectionIndicator = true;
                }
                SqlCommand cmd = new SqlCommand("usp_GetMonthsProdAchivMonthlyDaybyDay", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ItemId", SqlDbType.Int).Value = Item_Id;
                cmd.Parameters.AddWithValue("@MonthId", SqlDbType.Int).Value = MonthId;
                cmd.Parameters.AddWithValue("@Year", SqlDbType.Int).Value = year;


                DataTable dt = new DataTable();

                dt.Load(cmd.ExecuteReader());

                return dt;



            }
            catch (Exception exp)
            {
                throw exp;
            }
            finally
            {
                if (connectionIndicator)
                {
                    con.Close();
                    connectionIndicator = false;
                }
            }
        }

        public static DataTable GetCalculationsProdEffiMonthDate(int Item_Id, int MonthId, int year)
        {

            try
            {


                if (connectionIndicator == false)
                {
                    con.Open();
                    connectionIndicator = true;
                }
                SqlCommand cmd = new SqlCommand("usp_GetMonthsProdEfficMonthlyDaybyDay", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ItemId", SqlDbType.Int).Value = Item_Id;
                cmd.Parameters.AddWithValue("@MonthId", SqlDbType.Int).Value = MonthId;
                cmd.Parameters.AddWithValue("@Year", SqlDbType.Int).Value = year;


                DataTable dt = new DataTable();

                dt.Load(cmd.ExecuteReader());

                return dt;



            }
            catch (Exception exp)
            {
                throw exp;
            }
            finally
            {
                if (connectionIndicator)
                {
                    con.Close();
                    connectionIndicator = false;
                }
            }
        }


        public static DataTable GetCalculationsEquipFailRateMonthDate(int Item_Id, int MonthId, int year)
        {

            try
            {


                if (connectionIndicator == false)
                {
                    con.Open();
                    connectionIndicator = true;
                }
                SqlCommand cmd = new SqlCommand("usp_GetMonthsEqipFailMonthlyDaybyDay", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ItemId", SqlDbType.Int).Value = Item_Id;
                cmd.Parameters.AddWithValue("@MonthId", SqlDbType.Int).Value = MonthId;
                cmd.Parameters.AddWithValue("@Year", SqlDbType.Int).Value = year;



                DataTable dt = new DataTable();

                dt.Load(cmd.ExecuteReader());

                return dt;



            }
            catch (Exception exp)
            {
                throw exp;
            }
            finally
            {
                if (connectionIndicator)
                {
                    con.Close();
                    connectionIndicator = false;
                }
            }
        }

        public static DataTable GetCalculationsOEEDateDashBoard(int Item_Id, int MonthId, int year)
        {

            try
            {


                if (connectionIndicator == false)
                {
                    con.Open();
                    connectionIndicator = true;
                }
                SqlCommand cmd = new SqlCommand("usp_GetMonthsOEEDaybyDayDashBoard", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ItemId", SqlDbType.Int).Value = Item_Id;
                cmd.Parameters.AddWithValue("@MonthId", SqlDbType.Int).Value = MonthId;
                cmd.Parameters.AddWithValue("@Year", SqlDbType.Int).Value = year;


                DataTable dt = new DataTable();

                dt.Load(cmd.ExecuteReader());

                return dt;



            }
            catch (Exception exp)
            {
                throw exp;
            }
            finally
            {
                if (connectionIndicator)
                {
                    con.Close();
                    connectionIndicator = false;
                }
            }
        }


        public static DataTable GetCalculationsAttendanceDateDashBoard( int MonthId, int year)
        {

            try
            {


                if (connectionIndicator == false)
                {
                    con.Open();
                    connectionIndicator = true;
                }
                SqlCommand cmd = new SqlCommand("usp_GetMonthsAttendanceValueDaybyDayDashBoard", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@MonthId", SqlDbType.Int).Value = MonthId;
                cmd.Parameters.AddWithValue("@Year", SqlDbType.Int).Value = year;


                DataTable dt = new DataTable();

                dt.Load(cmd.ExecuteReader());

                return dt;



            }
            catch (Exception exp)
            {
                throw exp;
            }
            finally
            {
                if (connectionIndicator)
                {
                    con.Close();
                    connectionIndicator = false;
                }
            }
        }

        public static DataTable GetCalculationsMatYieldDateDashBoard(int ItemId, int MonthId, int year)
        {

            try
            {


                if (connectionIndicator == false)
                {
                    con.Open();
                    connectionIndicator = true;
                }
                SqlCommand cmd = new SqlCommand("usp_GetMatYieldVarCostMonthlyDaybyDay", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ItemId", SqlDbType.Int).Value = ItemId;
                cmd.Parameters.AddWithValue("@MonthId", SqlDbType.Int).Value = MonthId;
                cmd.Parameters.AddWithValue("@Year", SqlDbType.Int).Value = year;


                DataTable dt = new DataTable();

                dt.Load(cmd.ExecuteReader());

                return dt;



            }
            catch (Exception exp)
            {
                throw exp;
            }
            finally
            {
                if (connectionIndicator)
                {
                    con.Close();
                    connectionIndicator = false;
                }
            }
        }

        public static DataTable GetCalculationsAttendanceRatioDateDashBoard(int Item_Id, int MonthId)
        {

            try
            {


                if (connectionIndicator == false)
                {
                    con.Open();
                    connectionIndicator = true;
                }
                SqlCommand cmd = new SqlCommand("usp_GetMonthsAttendanceRatioDaybyDayDashBoard", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ItemId", SqlDbType.Int).Value = Item_Id;
                cmd.Parameters.AddWithValue("@MonthId", SqlDbType.Int).Value = MonthId;


                DataTable dt = new DataTable();

                dt.Load(cmd.ExecuteReader());

                return dt;



            }
            catch (Exception exp)
            {
                throw exp;
            }
            finally
            {
                if (connectionIndicator)
                {
                    con.Close();
                    connectionIndicator = false;
                }
            }
        }
        public static DataTable GetCalculationsOEEMonthDate(int Item_Id, int MonthId)
        {

            try
            {


                if (connectionIndicator == false)
                {
                    con.Open();
                    connectionIndicator = true;
                }
                SqlCommand cmd = new SqlCommand("usp_GetMonthsOEEMonthlyDaybyDay", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ItemId", SqlDbType.Int).Value = Item_Id;
                cmd.Parameters.AddWithValue("@MonthId", SqlDbType.Int).Value = MonthId;


                DataTable dt = new DataTable();

                dt.Load(cmd.ExecuteReader());

                return dt;



            }
            catch (Exception exp)
            {
                throw exp;
            }
            finally
            {
                if (connectionIndicator)
                {
                    con.Close();
                    connectionIndicator = false;
                }
            }
        }
        public static DataTable GetCalculationsReworkRatioMonthlyDashBoard(int Item_Id,int MonthId)
        {

            try
            {


                if (connectionIndicator == false)
                {
                    con.Open();
                    connectionIndicator = true;
                }
                SqlCommand cmd = new SqlCommand("usp_GetMonthsReworkRatioAdmin", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ItemId", SqlDbType.Int).Value = Item_Id;
                cmd.Parameters.AddWithValue("@MonthId", SqlDbType.Int).Value = MonthId;


                DataTable dt = new DataTable();

                dt.Load(cmd.ExecuteReader());

                return dt;



            }
            catch (Exception exp)
            {
                throw exp;
            }
            finally
            {
                if (connectionIndicator)
                {
                    con.Close();
                    connectionIndicator = false;
                }
            }
        }


        public static DataTable GetCalculationsMaterialYieldVarianceCostMonthly(int Item_Id, int year)
        {

            try
            {


                if (connectionIndicator == false)
                {
                    con.Open();
                    connectionIndicator = true;
                }
                SqlCommand cmd = new SqlCommand("usp_GetMonthsMaterialYieldVarianceCost", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ItemId", SqlDbType.Int).Value = Item_Id;
                     cmd.Parameters.AddWithValue("@Year", SqlDbType.Int).Value = year;


                DataTable dt = new DataTable();

                dt.Load(cmd.ExecuteReader());

                return dt;



            }
            catch (Exception exp)
            {
                throw exp;
            }
            finally
            {
                if (connectionIndicator)
                {
                    con.Close();
                    connectionIndicator = false;
                }
            }
        }
        public static DataTable GetCalculationsOEEMonthly(int Item_Id, int year)
        {

            try
            {


                if (connectionIndicator == false)
                {
                    con.Open();
                    connectionIndicator = true;
                }
                SqlCommand cmd = new SqlCommand("usp_GetMonthsOEE", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ItemId", SqlDbType.Int).Value = Item_Id;
                cmd.Parameters.AddWithValue("@Year", SqlDbType.Int).Value = year;


                DataTable dt = new DataTable();

                dt.Load(cmd.ExecuteReader());

                return dt;



            }
            catch (Exception exp)
            {
                throw exp;
            }
            finally
            {
                if (connectionIndicator)
                {
                    con.Close();
                    connectionIndicator = false;
                }
            }
        }
        public static DataTable GetCalculationsEquipmentFailureRateMonthlyWorkshop(int WorkshopId, int year)
        {

            try
            {


                if (connectionIndicator == false)
                {
                    con.Open();
                    connectionIndicator = true;
                }
                SqlCommand cmd = new SqlCommand("usp_GetMonthsEqupFailureRateShop", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ShopId", SqlDbType.Int).Value = WorkshopId;
                cmd.Parameters.AddWithValue("@Year", SqlDbType.Int).Value = year;


                DataTable dt = new DataTable();

                dt.Load(cmd.ExecuteReader());

                return dt;



            }
            catch (Exception exp)
            {
                throw exp;
            }
            finally
            {
                if (connectionIndicator)
                {
                    con.Close();
                    connectionIndicator = false;
                }
            }
        }
        public static DataTable GetCalculationsProductionAchivRateMonthly(int Item_Id, int year)
        {

            try
            {


                if (connectionIndicator == false)
                {
                    con.Open();
                    connectionIndicator = true;
                }
                SqlCommand cmd = new SqlCommand("usp_GetMonthsProductionAchievementRate", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ItemId", SqlDbType.Int).Value = Item_Id;
                cmd.Parameters.AddWithValue("@Year", SqlDbType.Int).Value = year;


                DataTable dt = new DataTable();

                dt.Load(cmd.ExecuteReader());

                return dt;



            }
            catch (Exception exp)
            {
                throw exp;
            }
            finally
            {
                if (connectionIndicator)
                {
                    con.Close();
                    connectionIndicator = false;
                }
            }
        }
        public static DataTable GetCalculationsCustomerClaimsMonthly(int Item_Id, int year)
        {

            try
            {


                if (connectionIndicator == false)
                {
                    con.Open();
                    connectionIndicator = true;
                }
                SqlCommand cmd = new SqlCommand("usp_GetMonthsCustomerClaims", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ItemId", SqlDbType.Int).Value = Item_Id;
                cmd.Parameters.AddWithValue("@Year", SqlDbType.Int).Value = year;



                DataTable dt = new DataTable();

                dt.Load(cmd.ExecuteReader());

                return dt;



            }
            catch (Exception exp)
            {
                throw exp;
            }
            finally
            {
                if (connectionIndicator)
                {
                    con.Close();
                    connectionIndicator = false;
                }
            }
        }

        public static DataTable GetCalculationsReworkCostMonthly(int Item_Id)
        {

            try
            {


                if (connectionIndicator == false)
                {
                    con.Open();
                    connectionIndicator = true;
                }
                SqlCommand cmd = new SqlCommand("usp_GetMonthsReworkCost", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ItemId", SqlDbType.Int).Value = Item_Id;


                DataTable dt = new DataTable();

                dt.Load(cmd.ExecuteReader());

                return dt;



            }
            catch (Exception exp)
            {
                throw exp;
            }
            finally
            {
                if (connectionIndicator)
                {
                    con.Close();
                    connectionIndicator = false;
                }
            }
        }

        public static DataTable GetCalculationsDelivaryComplianceRatioMonthly(int Item_Id, int year)
        {

            try
            {


                if (connectionIndicator == false)
                {
                    con.Open();
                    connectionIndicator = true;
                }
                SqlCommand cmd = new SqlCommand("usp_GetMonthsDelivaryComplianceRatio", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ItemId", SqlDbType.Int).Value = Item_Id;
                cmd.Parameters.AddWithValue("@Year", SqlDbType.Int).Value = year;


                DataTable dt = new DataTable();

                dt.Load(cmd.ExecuteReader());

                return dt;



            }
            catch (Exception exp)
            {
                throw exp;
            }
            finally
            {
                if (connectionIndicator)
                {
                    con.Close();
                    connectionIndicator = false;
                }
            }
        }

        public static DataTable GetCalculationsInhouseRejectionRatioMonthly(int Item_Id, int year)
        {

            try
            {


                if (connectionIndicator == false)
                {
                    con.Open();
                    connectionIndicator = true;
                }
                SqlCommand cmd = new SqlCommand("usp_GetMonthsInhouseRejectionRatio", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ItemId", SqlDbType.Int).Value = Item_Id;
                cmd.Parameters.AddWithValue("@Year", SqlDbType.Int).Value = year;


                DataTable dt = new DataTable();

                dt.Load(cmd.ExecuteReader());

                return dt;



            }
            catch (Exception exp)
            {
                throw exp;
            }
            finally
            {
                if (connectionIndicator)
                {
                    con.Close();
                    connectionIndicator = false;
                }
            }
        }

        public static DataTable GetCalculationsCustomerRejectionRatioMonthly(int Item_Id, int year)
        {

            try
            {


                if (connectionIndicator == false)
                {
                    con.Open();
                    connectionIndicator = true;
                }
                SqlCommand cmd = new SqlCommand("usp_GetMonthsCustomerRejectionRatio", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ItemId", SqlDbType.Int).Value = Item_Id;
                cmd.Parameters.AddWithValue("@Year", SqlDbType.Int).Value = year;

                DataTable dt = new DataTable();

                dt.Load(cmd.ExecuteReader());

                return dt;



            }
            catch (Exception exp)
            {
                throw exp;
            }
            finally
            {
                if (connectionIndicator)
                {
                    con.Close();
                    connectionIndicator = false;
                }
            }
        }

        public static DataTable GetCalculationsAttendanceRatioMonthly(int year)
        {

            try
            {


                if (connectionIndicator == false)
                {
                    con.Open();
                    connectionIndicator = true;
                }
                SqlCommand cmd = new SqlCommand("usp_GetMonthsAttendanceRatio", con);
                cmd.CommandType = CommandType.StoredProcedure;
               cmd.Parameters.AddWithValue("@Year", SqlDbType.Int).Value = year;
                //  cmd.Parameters.AddWithValue("@UserId", SqlDbType.Int).Value = UserId;

                DataTable dt = new DataTable();

                dt.Load(cmd.ExecuteReader());

                return dt;



            }
            catch (Exception exp)
            {
                throw exp;
            }
            finally
            {
                if (connectionIndicator)
                {
                    con.Close();
                    connectionIndicator = false;
                }
            }
        }

        public static DataTable GetCalculationsProductionEfficiencyMonthly(int Item_Id, int year)
        {

            try
            {


                if (connectionIndicator == false)
                {
                    con.Open();
                    connectionIndicator = true;
                }
                SqlCommand cmd = new SqlCommand("usp_GetMonthsProductionEfficiency", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ItemId", SqlDbType.Int).Value = Item_Id;
                cmd.Parameters.AddWithValue("@Year", SqlDbType.Int).Value = year;

                DataTable dt = new DataTable();

                dt.Load(cmd.ExecuteReader());

                return dt;



            }
            catch (Exception exp)
            {
                throw exp;
            }
            finally
            {
                if (connectionIndicator)
                {
                    con.Close();
                    connectionIndicator = false;
                }
            }
        }
        public static DataTable GetCalculationsCustomerRejectionCostMonthly(int Item_Id)
        {

            try
            {


                if (connectionIndicator == false)
                {
                    con.Open();
                    connectionIndicator = true;
                }
                SqlCommand cmd = new SqlCommand("usp_GetMonthsCustomerRejectionRatio", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ItemId", SqlDbType.Int).Value = Item_Id;
                //  cmd.Parameters.AddWithValue("@UserId", SqlDbType.Int).Value = UserId;

                DataTable dt = new DataTable();

                dt.Load(cmd.ExecuteReader());

                return dt;



            }
            catch (Exception exp)
            {
                throw exp;
            }
            finally
            {
                if (connectionIndicator)
                {
                    con.Close();
                    connectionIndicator = false;
                }
            }
        }
        public static DataTable GetCalculationsInhouseRejectionCostMonthly(int Item_Id, int year)
        {

            try
            {


                if (connectionIndicator == false)
                {
                    con.Open();
                    connectionIndicator = true;
                }
                SqlCommand cmd = new SqlCommand("usp_GetMonthsInhouseRejectionCost", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ItemId", SqlDbType.Int).Value = Item_Id;
                cmd.Parameters.AddWithValue("@Year", SqlDbType.Int).Value = year;

                DataTable dt = new DataTable();

                dt.Load(cmd.ExecuteReader());

                return dt;



            }
            catch (Exception exp)
            {
                throw exp;
            }
            finally
            {
                if (connectionIndicator)
                {
                    con.Close();
                    connectionIndicator = false;
                }
            }
        }

        public static DataTable GetCalculationsCustRejectionCostMonthly(int Item_Id , int year)
        {

            try
            {


                if (connectionIndicator == false)
                {
                    con.Open();
                    connectionIndicator = true;
                }
                SqlCommand cmd = new SqlCommand("usp_GetMonthsCustomerRejectionCost", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ItemId", SqlDbType.Int).Value = Item_Id;
                cmd.Parameters.AddWithValue("@Year", SqlDbType.Int).Value = year;
                
                DataTable dt = new DataTable();

                dt.Load(cmd.ExecuteReader());

                return dt;



            }
            catch (Exception exp)
            {
                throw exp;
            }
            finally
            {
                if (connectionIndicator)
                {
                    con.Close();
                    connectionIndicator = false;
                }
            }
        }
        public static decimal GetMonthTargetReworkRatio(int Month_Id,int ItemId, int Year)
        {
            try
            {


                if (connectionIndicator == false)
                {
                    con.Open();
                    connectionIndicator = true;
                }
                SqlCommand cmd = new SqlCommand("usp_GetMonthsTargetReworkRatio", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@MonthId", SqlDbType.Int).Value = Month_Id;
                cmd.Parameters.AddWithValue("@ItemId", SqlDbType.Int).Value = ItemId;
                cmd.Parameters.AddWithValue("@Year", SqlDbType.Int).Value = Year;

                DataTable dt = new DataTable();

                dt.Load(cmd.ExecuteReader());
                if (Convert.ToDecimal(dt.Rows[0][0]) >= 0)
                {
                    return Convert.ToDecimal(dt.Rows[0][0]);
                }
                else
                    return -1;


            }
            catch (Exception exp)
            {
                throw exp;
            }
            finally
            {
                if (connectionIndicator)
                {
                    con.Close();
                    connectionIndicator = false;
                }
            }
        }

        public static decimal GetMonthTargetMaterialYieldVarianceCost(int Month_Id, int ItemId, int year)
        {
            try
            {


                if (connectionIndicator == false)
                {
                    con.Open();
                    connectionIndicator = true;
                }
                SqlCommand cmd = new SqlCommand("usp_GetMonthsTargetMaterialYieldVarianceCost", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@MonthId", SqlDbType.Int).Value = Month_Id;
                cmd.Parameters.AddWithValue("@ItemId", SqlDbType.Int).Value = ItemId;
                cmd.Parameters.AddWithValue("@Year", SqlDbType.Int).Value = year;

                DataTable dt = new DataTable();

                dt.Load(cmd.ExecuteReader());
                if (Convert.ToDecimal(dt.Rows[0][0]) >= 0)
                {
                    return Convert.ToDecimal(dt.Rows[0][0]);
                }
                else
                    return -1;


            }
            catch (Exception exp)
            {
                throw exp;
            }
            finally
            {
                if (connectionIndicator)
                {
                    con.Close();
                    connectionIndicator = false;
                }
            }
        }


        public static DataTable GetIdAndMonthOverAllKPIResult(int Month_Id, int ItemId)
        {
            try
            {


                if (connectionIndicator == false)
                {
                    con.Open();
                    connectionIndicator = true;
                }
                SqlCommand cmd = new SqlCommand("Usp_GetIdMonthAllKpisResult", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@MonthId", SqlDbType.Int).Value = Month_Id;
                cmd.Parameters.AddWithValue("@ItemId", SqlDbType.Int).Value = ItemId;

                DataTable dt = new DataTable();

                dt.Load(cmd.ExecuteReader());
                  return dt;
                
               


            }
            catch (Exception exp)
            {
                throw exp;
            }
            finally
            {
                if (connectionIndicator)
                {
                    con.Close();
                    connectionIndicator = false;
                }
            }
        }

        public static DataTable GetIdAndMonthOverAllKPIProdAchivAndOEEResult(int Month_Id, int ItemId)
        {
            try
            {


                if (connectionIndicator == false)
                {
                    con.Open();
                    connectionIndicator = true;
                }
                SqlCommand cmd = new SqlCommand("Usp_GetIdMonthAllKpisProAchivAndOEEResult", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@MonthId", SqlDbType.Int).Value = Month_Id;
                cmd.Parameters.AddWithValue("@ItemId", SqlDbType.Int).Value = ItemId;

                DataTable dt = new DataTable();

                dt.Load(cmd.ExecuteReader());
                return dt;




            }
            catch (Exception exp)
            {
                throw exp;
            }
            finally
            {
                if (connectionIndicator)
                {
                    con.Close();
                    connectionIndicator = false;
                }
            }
        }
        public static DataTable GetMonthAttendacneRatioOverAllKPIResult(int Month_Id)
        {
            try
            {


                if (connectionIndicator == false)
                {
                    con.Open();
                    connectionIndicator = true;
                }
                SqlCommand cmd = new SqlCommand("usp_GetMonthsAttendanceRatioDashBord", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@MonthId", SqlDbType.Int).Value = Month_Id;
                

                DataTable dt = new DataTable();

                dt.Load(cmd.ExecuteReader());
                return dt;




            }
            catch (Exception exp)
            {
                throw exp;
            }
            finally
            {
                if (connectionIndicator)
                {
                    con.Close();
                    connectionIndicator = false;
                }
            }
        }


        public static DataTable GetMontEquipFailRateAllKPIResult(int Month_Id)
        {
            try
            {


                if (connectionIndicator == false)
                {
                    con.Open();
                    connectionIndicator = true;
                }
                SqlCommand cmd = new SqlCommand("Usp_GetEquipFailRateDashBoardMonthly", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@MonthId", SqlDbType.Int).Value = Month_Id;


                DataTable dt = new DataTable();

                dt.Load(cmd.ExecuteReader());
                return dt;




            }
            catch (Exception exp)
            {
                throw exp;
            }
            finally
            {
                if (connectionIndicator)
                {
                    con.Close();
                    connectionIndicator = false;
                }
            }
        }
        public static DataTable GetMonthTargetEquipFailureRateShopMonthly(int Month_Id, int WorkShopId, int year)
        {
            try
            {


                if (connectionIndicator == false)
                {
                    con.Open();
                    connectionIndicator = true;
                }
                SqlCommand cmd = new SqlCommand("usp_GetMonthsTargetEquipFailRateShop", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@MonthId", SqlDbType.Int).Value = Month_Id;
                cmd.Parameters.AddWithValue("@ShopId", SqlDbType.Int).Value = WorkShopId;
                cmd.Parameters.AddWithValue("@Year", SqlDbType.Int).Value = year;

                DataTable dt = new DataTable();

                dt.Load(cmd.ExecuteReader());
                
                    return dt;
                
               


            }
            catch (Exception exp)
            {
                throw exp;
            }
            finally
            {
                if (connectionIndicator)
                {
                    con.Close();
                    connectionIndicator = false;
                }
            }
        }

        public static DataTable GetMonthTargetOEEMonthly(int Month_Id, int ItemId, int year)
        {
            try
            {


                if (connectionIndicator == false)
                {
                    con.Open();
                    connectionIndicator = true;
                }
                SqlCommand cmd = new SqlCommand("usp_GetMonthsTargeOEE", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@MonthId", SqlDbType.Int).Value = Month_Id;
                cmd.Parameters.AddWithValue("@ItemId", SqlDbType.Int).Value = ItemId;
                cmd.Parameters.AddWithValue("@Year", SqlDbType.Int).Value = year;

                DataTable dt = new DataTable();

                dt.Load(cmd.ExecuteReader());

                return dt;




            }
            catch (Exception exp)
            {
                throw exp;
            }
            finally
            {
                if (connectionIndicator)
                {
                    con.Close();
                    connectionIndicator = false;
                }
            }
        }

        public static decimal GetMonthTargetCostofInhouseRejection(int Month_Id, int ItemId ,int year)
        {
            try
            {


                if (connectionIndicator == false)
                {
                    con.Open();
                    connectionIndicator = true;
                }
                SqlCommand cmd = new SqlCommand("usp_GetMonthsTargetInhouseRejectionCost", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@MonthId", SqlDbType.Int).Value = Month_Id;
                cmd.Parameters.AddWithValue("@ItemId", SqlDbType.Int).Value = ItemId;
                cmd.Parameters.AddWithValue("@Year", SqlDbType.Int).Value = year;

                DataTable dt = new DataTable();

                dt.Load(cmd.ExecuteReader());
                if (Convert.ToDecimal(dt.Rows[0][0]) >= 0)
                {
                    return Convert.ToDecimal(dt.Rows[0][0]);
                }
                else
                    return -1;


            }
            catch (Exception exp)
            {
                throw exp;
            }
            finally
            {
                if (connectionIndicator)
                {
                    con.Close();
                    connectionIndicator = false;
                }
            }
        }
        public static decimal GetMonthTargetCostofCustRejection(int Month_Id, int ItemId, int year)
        {
            try
            {


                if (connectionIndicator == false)
                {
                    con.Open();
                    connectionIndicator = true;
                }
                SqlCommand cmd = new SqlCommand("usp_GetMonthsTargetCustomerRejectionCost", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@MonthId", SqlDbType.Int).Value = Month_Id;
                cmd.Parameters.AddWithValue("@ItemId", SqlDbType.Int).Value = ItemId;
                cmd.Parameters.AddWithValue("@Year", SqlDbType.Int).Value = year;
                DataTable dt = new DataTable();

                dt.Load(cmd.ExecuteReader());
                if (Convert.ToDecimal(dt.Rows[0][0]) >= 0)
                {
                    return Convert.ToDecimal(dt.Rows[0][0]);
                }
                else
                    return -1;


            }
            catch (Exception exp)
            {
                throw exp;
            }
            finally
            {
                if (connectionIndicator)
                {
                    con.Close();
                    connectionIndicator = false;
                }
            }
        }
        public static decimal GetMonthTargetProductionAchiv(int Month_Id, int ItemId, int year)
        {
            try
            {


                if (connectionIndicator == false)
                {
                    con.Open();
                    connectionIndicator = true;
                }
                SqlCommand cmd = new SqlCommand("usp_GetMonthsTargetProductionAchievementRate", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@MonthId", SqlDbType.Int).Value = Month_Id;
                cmd.Parameters.AddWithValue("@ItemId", SqlDbType.Int).Value = ItemId;
                cmd.Parameters.AddWithValue("@Year", SqlDbType.Int).Value = year;

                DataTable dt = new DataTable();

                dt.Load(cmd.ExecuteReader());
                if (Convert.ToDecimal(dt.Rows[0][0]) >= 0)
                {
                    return Convert.ToDecimal(dt.Rows[0][0]);
                }
                else
                    return -1;


            }
            catch (Exception exp)
            {
                throw exp;
            }
            finally
            {
                if (connectionIndicator)
                {
                    con.Close();
                    connectionIndicator = false;
                }
            }
        }


        public static decimal GetMonthTargetCustomerClaims(int Month_Id, int ItemId, int year)
        {
            try
            {


                if (connectionIndicator == false)
                {
                    con.Open();
                    connectionIndicator = true;
                }
                SqlCommand cmd = new SqlCommand("usp_GetMonthsTargetCustomerClaims", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@MonthId", SqlDbType.Int).Value = Month_Id;
                cmd.Parameters.AddWithValue("@ItemId", SqlDbType.Int).Value = ItemId;
                cmd.Parameters.AddWithValue("@Year", SqlDbType.Int).Value = year;

                DataTable dt = new DataTable();

                dt.Load(cmd.ExecuteReader());
                if (Convert.ToDecimal(dt.Rows[0][0]) >= 0)
                {
                    return Convert.ToDecimal(dt.Rows[0][0]);
                }
                else
                    return -1;


            }
            catch (Exception exp)
            {
                throw exp;
            }
            finally
            {
                if (connectionIndicator)
                {
                    con.Close();
                    connectionIndicator = false;
                }
            }
        }
        public static decimal GetMonthTargetDelivaryComplianceRatio(int Month_Id, int ItemId, int year)
        {
            try
            {


                if (connectionIndicator == false)
                {
                    con.Open();
                    connectionIndicator = true;
                }
                SqlCommand cmd = new SqlCommand("usp_GetMonthsTargetDelivaryComplianceRatio", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@MonthId", SqlDbType.Int).Value = Month_Id;
                cmd.Parameters.AddWithValue("@ItemId", SqlDbType.Int).Value = ItemId;
                cmd.Parameters.AddWithValue("@Year", SqlDbType.Int).Value = year;

                DataTable dt = new DataTable();

                dt.Load(cmd.ExecuteReader());
                if (Convert.ToDecimal(dt.Rows[0][0]) >= 0)
                {
                    return Convert.ToDecimal(dt.Rows[0][0]);
                }
                else
                    return -1;


            }
            catch (Exception exp)
            {
                throw exp;
            }
            finally
            {
                if (connectionIndicator)
                {
                    con.Close();
                    connectionIndicator = false;
                }
            }
        }

      

        public static decimal GetMonthTargetReworkCost(int Month_Id, int ItemId, int year)
        {
            try
            {


                if (connectionIndicator == false)
                {
                    con.Open();
                    connectionIndicator = true;
                }
                SqlCommand cmd = new SqlCommand("usp_GetMonthsTargetReworkCost", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@MonthId", SqlDbType.Int).Value = Month_Id;
                cmd.Parameters.AddWithValue("@ItemId", SqlDbType.Int).Value = ItemId;
                cmd.Parameters.AddWithValue("@Year", SqlDbType.Int).Value = year;

                DataTable dt = new DataTable();

                dt.Load(cmd.ExecuteReader());
                if (Convert.ToDecimal(dt.Rows[0][0]) >= 0)
                {
                    return Convert.ToDecimal(dt.Rows[0][0]);
                }
                else
                    return -1;


            }
            catch (Exception exp)
            {
                throw exp;
            }
            finally
            {
                if (connectionIndicator)
                {
                    con.Close();
                    connectionIndicator = false;
                }
            }
        }

        public static decimal GetMonthTargetCustomerRejectionRatio(int Month_Id, int ItemId, int year)
        {
            try
            {


                if (connectionIndicator == false)
                {
                    con.Open();
                    connectionIndicator = true;
                }
                SqlCommand cmd = new SqlCommand("usp_GetMonthsTargetCustomerRejectionRatio", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@MonthId", SqlDbType.Int).Value = Month_Id;
                cmd.Parameters.AddWithValue("@ItemId", SqlDbType.Int).Value = ItemId;
                cmd.Parameters.AddWithValue("@Year", SqlDbType.Int).Value = year;

                DataTable dt = new DataTable();

                dt.Load(cmd.ExecuteReader());
                if (Convert.ToDecimal(dt.Rows[0][0]) >= 0)
                {
                    return Convert.ToDecimal(dt.Rows[0][0]);
                }
                else
                    return -1;


            }
            catch (Exception exp)
            {
                throw exp;
            }
            finally
            {
                if (connectionIndicator)
                {
                    con.Close();
                    connectionIndicator = false;
                }
            }
        }


        public static DataTable GetMonthTargetAttendanceRatio(int Month_Id,int year)
        {
            try
            {


                if (connectionIndicator == false)
                {
                    con.Open();
                    connectionIndicator = true;
                }
                SqlCommand cmd = new SqlCommand("usp_GetMonthsTargetAttendanceRatio", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@MonthId", SqlDbType.Int).Value = Month_Id;
                cmd.Parameters.AddWithValue("@Year", SqlDbType.Int).Value = year;

              

                DataTable dt = new DataTable();

                dt.Load(cmd.ExecuteReader());
                
                    return dt;


            }
            catch (Exception exp)
            {
                throw exp;
            }
            finally
            {
                if (connectionIndicator)
                {
                    con.Close();
                    connectionIndicator = false;
                }
            }
        }



        public static decimal GetMonthTargetProductionEfficiency(int Month_Id, int ItemId, int year)
        {
            try
            {


                if (connectionIndicator == false)
                {
                    con.Open();
                    connectionIndicator = true;
                }
                SqlCommand cmd = new SqlCommand("usp_GetMonthsTargetProductionEfficiency", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@MonthId", SqlDbType.Int).Value = Month_Id;
                cmd.Parameters.AddWithValue("@ItemId", SqlDbType.Int).Value = ItemId;
                cmd.Parameters.AddWithValue("@Year", SqlDbType.Int).Value = year;

                DataTable dt = new DataTable();

                dt.Load(cmd.ExecuteReader());
                if (Convert.ToDecimal(dt.Rows[0][0]) >= 0)
                {
                    return Convert.ToDecimal(dt.Rows[0][0]);
                }
                else
                    return -1;


            }
            catch (Exception exp)
            {
                throw exp;
            }
            finally
            {
                if (connectionIndicator)
                {
                    con.Close();
                    connectionIndicator = false;
                }
            }
        }
        public static decimal GetMonthTargetInhouseRejectionRatio(int Month_Id, int ItemId, int year)
        {
            try
            {


                if (connectionIndicator == false)
                {
                    con.Open();
                    connectionIndicator = true;
                }
                SqlCommand cmd = new SqlCommand("usp_GetMonthsTargetInhouseRejectionRatio", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@MonthId", SqlDbType.Int).Value = Month_Id;
                cmd.Parameters.AddWithValue("@ItemId", SqlDbType.Int).Value = ItemId;
                cmd.Parameters.AddWithValue("@Year", SqlDbType.Int).Value = year;

                DataTable dt = new DataTable();

                dt.Load(cmd.ExecuteReader());
                if (Convert.ToDecimal(dt.Rows[0][0]) >= 0)
                {
                    return Convert.ToDecimal(dt.Rows[0][0]);
                }
                else
                    return -1;


            }
            catch (Exception exp)
            {
                throw exp;
            }
            finally
            {
                if (connectionIndicator)
                {
                    con.Close();
                    connectionIndicator = false;
                }
            }
        }
        public static int EditTargetReworkRatio(int Item_Id, int Month_Id, decimal Target,int year)
        {
            try
            {


                if (connectionIndicator == false)
                {
                    con.Open();
                    connectionIndicator = true;
                }
                SqlCommand cmd = new SqlCommand("Usp_EditTargetReworkRatio", con);
                cmd.CommandType = CommandType.StoredProcedure;


                cmd.Parameters.AddWithValue("@ItemId", SqlDbType.Int).Value = Item_Id;
                cmd.Parameters.AddWithValue("@MonthId", SqlDbType.Int).Value = Month_Id;
                cmd.Parameters.AddWithValue("@Target", SqlDbType.Decimal).Value = Target;
                cmd.Parameters.AddWithValue("@UserId", SqlDbType.Int).Value = UserId;
                cmd.Parameters.AddWithValue("@Year", SqlDbType.Int).Value = year;

                DataTable dt = new DataTable();

                int rows = cmd.ExecuteNonQuery();

                return rows;



            }
            catch (Exception exp)
            {
                throw exp;
            }
            finally
            {
                if (connectionIndicator)
                {
                    con.Close();
                    connectionIndicator = false;
                }
            }
        }

        public static int EditTargetMaterialYieldVarianceCost(int Item_Id, int Month_Id, decimal Target, int year)
        {
            try
            {


                if (connectionIndicator == false)
                {
                    con.Open();
                    connectionIndicator = true;
                }
                SqlCommand cmd = new SqlCommand("Usp_EditTargetMaterialYieldVarianceCost", con);
                cmd.CommandType = CommandType.StoredProcedure;


                cmd.Parameters.AddWithValue("@ItemId", SqlDbType.Int).Value = Item_Id;
                cmd.Parameters.AddWithValue("@MonthId", SqlDbType.Int).Value = Month_Id;
                cmd.Parameters.AddWithValue("@Target", SqlDbType.Decimal).Value = Target;
                cmd.Parameters.AddWithValue("@UserId", SqlDbType.Int).Value = UserId;
                cmd.Parameters.AddWithValue("@Year", SqlDbType.Int).Value = year;

                DataTable dt = new DataTable();

                int rows = cmd.ExecuteNonQuery();

                return rows;



            }
            catch (Exception exp)
            {
                throw exp;
            }
            finally
            {
                if (connectionIndicator)
                {
                    con.Close();
                    connectionIndicator = false;
                }
            }
        }
        public static int EditTargetEquipFailureRateMonethWorkShop(int WorkShop, int Month_Id, decimal Target, int NoOfDays, int year)
        {
            try
            {


                if (connectionIndicator == false)
                {
                    con.Open();
                    connectionIndicator = true;
                }
                SqlCommand cmd = new SqlCommand("Usp_EditTargetEquipmentFailureRatebyWorkShopMonthly", con);
                cmd.CommandType = CommandType.StoredProcedure;


                cmd.Parameters.AddWithValue("@WorkShopId", SqlDbType.Int).Value = WorkShop;
                cmd.Parameters.AddWithValue("@MonthId", SqlDbType.Int).Value = Month_Id;
                cmd.Parameters.AddWithValue("@Target", SqlDbType.Decimal).Value = Target;
                cmd.Parameters.AddWithValue("@UserId", SqlDbType.Int).Value = UserId;
                cmd.Parameters.AddWithValue("@NoOfDays", SqlDbType.Int).Value = NoOfDays;
                cmd.Parameters.AddWithValue("@Year", SqlDbType.Int).Value = year;

                DataTable dt = new DataTable();

                int rows = cmd.ExecuteNonQuery();

                return rows;



            }
            catch (Exception exp)
            {
                throw exp;
            }
            finally
            {
                if (connectionIndicator)
                {
                    con.Close();
                    connectionIndicator = false;
                }
            }
        }
        public static int EditTargetProductionAchievement(int Item_Id, int Month_Id, decimal Target, int year)
        {
            try
            {


                if (connectionIndicator == false)
                {
                    con.Open();
                    connectionIndicator = true;
                }
                SqlCommand cmd = new SqlCommand("Usp_EditTargetProductionAchievementRate", con);
                cmd.CommandType = CommandType.StoredProcedure;


                cmd.Parameters.AddWithValue("@ItemId", SqlDbType.Int).Value = Item_Id;
                cmd.Parameters.AddWithValue("@MonthId", SqlDbType.Int).Value = Month_Id;
                cmd.Parameters.AddWithValue("@Target", SqlDbType.Decimal).Value = Target;
                cmd.Parameters.AddWithValue("@UserId", SqlDbType.Int).Value = UserId;
                cmd.Parameters.AddWithValue("@Year", SqlDbType.Int).Value = year;

                DataTable dt = new DataTable();

                int rows = cmd.ExecuteNonQuery();

                return rows;



            }
            catch (Exception exp)
            {
                throw exp;
            }
            finally
            {
                if (connectionIndicator)
                {
                    con.Close();
                    connectionIndicator = false;
                }
            }
        }

        public static int EditTargetOEE(int Item_Id, int Month_Id, decimal Target, int NoOfDays, int year)
        {
            try
            {


                if (connectionIndicator == false)
                {
                    con.Open();
                    connectionIndicator = true;
                }
                SqlCommand cmd = new SqlCommand("Usp_EditOEEMonthly", con);
                cmd.CommandType = CommandType.StoredProcedure;


                cmd.Parameters.AddWithValue("@ItemId", SqlDbType.Int).Value = Item_Id;
                cmd.Parameters.AddWithValue("@MonthId", SqlDbType.Int).Value = Month_Id;
                cmd.Parameters.AddWithValue("@Target", SqlDbType.Decimal).Value = Target;
                cmd.Parameters.AddWithValue("@NoOfDays", SqlDbType.Int).Value = NoOfDays;
                cmd.Parameters.AddWithValue("@UserId", SqlDbType.Int).Value = UserId;
                cmd.Parameters.AddWithValue("@Year", SqlDbType.Int).Value = year;

                DataTable dt = new DataTable();

                int rows = cmd.ExecuteNonQuery();

                return rows;



            }
            catch (Exception exp)
            {
                throw exp;
            }
            finally
            {
                if (connectionIndicator)
                {
                    con.Close();
                    connectionIndicator = false;
                }
            }
        }
        public static int EditTargetCustomerClaims(int Item_Id, int Month_Id, decimal Target, int year)
        {
            try
            {


                if (connectionIndicator == false)
                {
                    con.Open();
                    connectionIndicator = true;
                }
                SqlCommand cmd = new SqlCommand("Usp_EditTargetCustomerClaims", con);
                cmd.CommandType = CommandType.StoredProcedure;


                cmd.Parameters.AddWithValue("@ItemId", SqlDbType.Int).Value = Item_Id;
                cmd.Parameters.AddWithValue("@MonthId", SqlDbType.Int).Value = Month_Id;
                cmd.Parameters.AddWithValue("@Target", SqlDbType.Decimal).Value = Target;
                cmd.Parameters.AddWithValue("@UserId", SqlDbType.Int).Value = UserId;
                cmd.Parameters.AddWithValue("@Year", SqlDbType.Int).Value = year;

                DataTable dt = new DataTable();

                int rows = cmd.ExecuteNonQuery();

                return rows;



            }
            catch (Exception exp)
            {
                throw exp;
            }
            finally
            {
                if (connectionIndicator)
                {
                    con.Close();
                    connectionIndicator = false;
                }
            }
        }

        public static int EditTargetDelivaryComplianceRatioMonthly(int Item_Id, int Month_Id, decimal Target,int year)
        {
            try
            {


                if (connectionIndicator == false)
                {
                    con.Open();
                    connectionIndicator = true;
                }
                SqlCommand cmd = new SqlCommand("Usp_EditTargetDelivaryComplianceRatio", con);
                cmd.CommandType = CommandType.StoredProcedure;


                cmd.Parameters.AddWithValue("@ItemId", SqlDbType.Int).Value = Item_Id;
                cmd.Parameters.AddWithValue("@MonthId", SqlDbType.Int).Value = Month_Id;
                cmd.Parameters.AddWithValue("@Target", SqlDbType.Decimal).Value = Target;
                cmd.Parameters.AddWithValue("@UserId", SqlDbType.Int).Value = UserId;
                cmd.Parameters.AddWithValue("@Year", SqlDbType.Int).Value = year;

                DataTable dt = new DataTable();

                int rows = cmd.ExecuteNonQuery();

                return rows;



            }
            catch (Exception exp)
            {
                throw exp;
            }
            finally
            {
                if (connectionIndicator)
                {
                    con.Close();
                    connectionIndicator = false;
                }
            }
        }

        public static int EditTargetReworkCost(int Item_Id, int Month_Id, decimal Target, int year)
        {
            try
            {


                if (connectionIndicator == false)
                {
                    con.Open();
                    connectionIndicator = true;
                }
                SqlCommand cmd = new SqlCommand("Usp_EditTargetReworkCost", con);
                cmd.CommandType = CommandType.StoredProcedure;


                cmd.Parameters.AddWithValue("@ItemId", SqlDbType.Int).Value = Item_Id;
                cmd.Parameters.AddWithValue("@MonthId", SqlDbType.Int).Value = Month_Id;
                cmd.Parameters.AddWithValue("@Target", SqlDbType.Decimal).Value = Target;
                cmd.Parameters.AddWithValue("@UserId", SqlDbType.Int).Value = UserId;
                cmd.Parameters.AddWithValue("@Year", SqlDbType.Int).Value = year;


                DataTable dt = new DataTable();

                int rows = cmd.ExecuteNonQuery();

                return rows;



            }
            catch (Exception exp)
            {
                throw exp;
            }
            finally
            {
                if (connectionIndicator)
                {
                    con.Close();
                    connectionIndicator = false;
                }
            }
        }

        public static int EditTargetInhouseRejCost(int Item_Id, int Month_Id, decimal Target, int year)
        {
            try
            {


                if (connectionIndicator == false)
                {
                    con.Open();
                    connectionIndicator = true;
                }
                SqlCommand cmd = new SqlCommand("Usp_EditTargetInhouseRejectionCost", con);
                cmd.CommandType = CommandType.StoredProcedure;


                cmd.Parameters.AddWithValue("@ItemId", SqlDbType.Int).Value = Item_Id;
                cmd.Parameters.AddWithValue("@MonthId", SqlDbType.Int).Value = Month_Id;
                cmd.Parameters.AddWithValue("@Target", SqlDbType.Decimal).Value = Target;
                cmd.Parameters.AddWithValue("@UserId", SqlDbType.Int).Value = UserId;
                cmd.Parameters.AddWithValue("@Year", SqlDbType.Int).Value = year;
     

                DataTable dt = new DataTable();

                int rows = cmd.ExecuteNonQuery();

                return rows;



            }
            catch (Exception exp)
            {
                throw exp;
            }
            finally
            {
                if (connectionIndicator)
                {
                    con.Close();
                    connectionIndicator = false;
                }
            }
        }


        public static int EditTargetCustRejCost(int itemId, int monthId, decimal target, int year)
        {
            try
            {


                if (connectionIndicator == false)
                {
                    con.Open();
                    connectionIndicator = true;
                }
                SqlCommand cmd = new SqlCommand("Usp_EditTargetCustomerRejectionCost", con);
                cmd.CommandType = CommandType.StoredProcedure;


                cmd.Parameters.AddWithValue("@ItemId", SqlDbType.Int).Value = itemId;
                cmd.Parameters.AddWithValue("@MonthId", SqlDbType.Int).Value = monthId;
                cmd.Parameters.AddWithValue("@Target", SqlDbType.Decimal).Value = target;
                cmd.Parameters.AddWithValue("@UserId", SqlDbType.Int).Value = UserId;
                cmd.Parameters.AddWithValue("@Year", SqlDbType.Int).Value = year;

                DataTable dt = new DataTable();

                int rows = cmd.ExecuteNonQuery();

                return rows;



            }
            catch (Exception exp)
            {
                throw exp;
            }
            finally
            {
                if (connectionIndicator)
                {
                    con.Close();
                    connectionIndicator = false;
                }
            }
        }
        public static int EditTargetCustomerRejectionRatio(int Item_Id, int Month_Id, decimal Target , int year)
        {
            try
            {


                if (connectionIndicator == false)
                {
                    con.Open();
                    connectionIndicator = true;
                }
                SqlCommand cmd = new SqlCommand("Usp_EditTargetCustomerRejectionRatio", con);
                cmd.CommandType = CommandType.StoredProcedure;


                cmd.Parameters.AddWithValue("@ItemId", SqlDbType.Int).Value = Item_Id;
                cmd.Parameters.AddWithValue("@MonthId", SqlDbType.Int).Value = Month_Id;
                cmd.Parameters.AddWithValue("@Target", SqlDbType.Decimal).Value = Target;
                cmd.Parameters.AddWithValue("@UserId", SqlDbType.Int).Value = UserId;
                cmd.Parameters.AddWithValue("@Year", SqlDbType.Int).Value = year;

                DataTable dt = new DataTable();

                int rows = cmd.ExecuteNonQuery();

                return rows;



            }
            catch (Exception exp)
            {
                throw exp;
            }
            finally
            {
                if (connectionIndicator)
                {
                    con.Close();
                    connectionIndicator = false;
                }
            }
        }

        public static int EditTargetAttendanceRatio(int noOfDays, int Month_Id, decimal Target, int year)
        {
            try
            {


                if (connectionIndicator == false)
                {
                    con.Open();
                    connectionIndicator = true;
                }
                SqlCommand cmd = new SqlCommand("Usp_EditTargetAttendanceRatio", con);
                cmd.CommandType = CommandType.StoredProcedure;


                cmd.Parameters.AddWithValue("@NoOfDays", SqlDbType.Int).Value = noOfDays;
                cmd.Parameters.AddWithValue("@MonthId", SqlDbType.Int).Value = Month_Id;
                cmd.Parameters.AddWithValue("@Target", SqlDbType.Decimal).Value = Target;
                cmd.Parameters.AddWithValue("@UserId", SqlDbType.Int).Value = UserId;
                cmd.Parameters.AddWithValue("@Year", SqlDbType.Int).Value = year;

                DataTable dt = new DataTable();

                int rows = cmd.ExecuteNonQuery();

                return rows;



            }
            catch (Exception exp)
            {
                throw exp;
            }
            finally
            {
                if (connectionIndicator)
                {
                    con.Close();
                    connectionIndicator = false;
                }
            }
        }


        public static int EditTargetMonthlyProductionEfficiency(int Item_Id, int Month_Id, decimal Target, int year)
        {
            try
            {


                if (connectionIndicator == false)
                {
                    con.Open();
                    connectionIndicator = true;
                }
                SqlCommand cmd = new SqlCommand("Usp_EditTargetMonthlyProductionAchievement", con);
                cmd.CommandType = CommandType.StoredProcedure;


                cmd.Parameters.AddWithValue("@ItemId", SqlDbType.Int).Value = Item_Id;
                cmd.Parameters.AddWithValue("@MonthId", SqlDbType.Int).Value = Month_Id;
                cmd.Parameters.AddWithValue("@Target", SqlDbType.Decimal).Value = Target;
                cmd.Parameters.AddWithValue("@UserId", SqlDbType.Int).Value = UserId;
                cmd.Parameters.AddWithValue("@Year", SqlDbType.Int).Value = year;

                DataTable dt = new DataTable();

                int rows = cmd.ExecuteNonQuery();

                return rows;



            }
            catch (Exception exp)
            {
                throw exp;
            }
            finally
            {
                if (connectionIndicator)
                {
                    con.Close();
                    connectionIndicator = false;
                }
            }
        }
        public static int EditTargetInhouseRejectionRatio(int Item_Id, int Month_Id, decimal Target, int year)
        {
            try
            {


                if (connectionIndicator == false)
                {
                    con.Open();
                    connectionIndicator = true;
                }
                SqlCommand cmd = new SqlCommand("Usp_EditTargetInhouseRejectionRatio", con);
                cmd.CommandType = CommandType.StoredProcedure;


                cmd.Parameters.AddWithValue("@ItemId", SqlDbType.Int).Value = Item_Id;
                cmd.Parameters.AddWithValue("@MonthId", SqlDbType.Int).Value = Month_Id;
                cmd.Parameters.AddWithValue("@Target", SqlDbType.Decimal).Value = Target;
                cmd.Parameters.AddWithValue("@UserId", SqlDbType.Int).Value = UserId;
                cmd.Parameters.AddWithValue("@Year", SqlDbType.Int).Value = year;

                DataTable dt = new DataTable();

                int rows = cmd.ExecuteNonQuery();

                return rows;



            }
            catch (Exception exp)
            {
                throw exp;
            }
            finally
            {
                if (connectionIndicator)
                {
                    con.Close();
                    connectionIndicator = false;
                }
            }
        }

        public static void SaveMonthsforObservationsActivitiesReworkRatio(int Item_Id)
        {
            try
            {


                if (connectionIndicator == false)
                {
                    con.Open();
                    connectionIndicator = true;
                }
                SqlCommand cmd = new SqlCommand("Usp_ObservationActivitiesMonthsReworkRatio", con);
                cmd.CommandType = CommandType.StoredProcedure;


                cmd.Parameters.AddWithValue("@ItemId", SqlDbType.Int).Value = Item_Id;
              

                DataTable dt = new DataTable();

                int row= cmd.ExecuteNonQuery();

                



            }
            catch (Exception exp)
            {
                throw exp;
            }
            finally
            {
                if (connectionIndicator)
                {
                    con.Close();
                    connectionIndicator = false;
                }
            }
        }

        public static void SaveMonthsforObservationsActivitiesMaterialYieldVarianceCost(int Item_Id)
        {
            try
            {


                if (connectionIndicator == false)
                {
                    con.Open();
                    connectionIndicator = true;
                }
                SqlCommand cmd = new SqlCommand("Usp_ObservationActivitiesMonthsMaterialYieldVarianceCost", con);
                cmd.CommandType = CommandType.StoredProcedure;


                cmd.Parameters.AddWithValue("@ItemId", SqlDbType.Int).Value = Item_Id;


                DataTable dt = new DataTable();

                int row = cmd.ExecuteNonQuery();





            }
            catch (Exception exp)
            {
                throw exp;
            }
            finally
            {
                if (connectionIndicator)
                {
                    con.Close();
                    connectionIndicator = false;
                }
            }
        }
        public static void SaveMonthsforObservationsActivitiesOEE(int Item_Id)
        {
            try
            {


                if (connectionIndicator == false)
                {
                    con.Open();
                    connectionIndicator = true;
                }
                SqlCommand cmd = new SqlCommand("Usp_ObservationActivitiesMonthsOEE", con);
                cmd.CommandType = CommandType.StoredProcedure;


                cmd.Parameters.AddWithValue("@ItemId", SqlDbType.Int).Value = Item_Id;


                DataTable dt = new DataTable();

                int row = cmd.ExecuteNonQuery();





            }
            catch (Exception exp)
            {
                throw exp;
            }
            finally
            {
                if (connectionIndicator)
                {
                    con.Close();
                    connectionIndicator = false;
                }
            }
        }
        public static void SaveMonthsforObservationsActivitiesEquipFailRate(int WorkShopId)
        {
            try
            {


                if (connectionIndicator == false)
                {
                    con.Open();
                    connectionIndicator = true;
                }
                SqlCommand cmd = new SqlCommand("Usp_ObservationActivitiesMonthsEquipFailRate", con);
                cmd.CommandType = CommandType.StoredProcedure;


                cmd.Parameters.AddWithValue("@WorkShopId", SqlDbType.Int).Value = WorkShopId;


                DataTable dt = new DataTable();

                int row = cmd.ExecuteNonQuery();





            }
            catch (Exception exp)
            {
                throw exp;
            }
            finally
            {
                if (connectionIndicator)
                {
                    con.Close();
                    connectionIndicator = false;
                }
            }
        }
        public static void SaveMonthsforObservationsActivitiesCustomerRejectionRatio(int Item_Id)
        {
            try
            {


                if (connectionIndicator == false)
                {
                    con.Open();
                    connectionIndicator = true;
                }
                SqlCommand cmd = new SqlCommand("Usp_ObservationActivitiesMonthsCustomerRejectionRatio", con);
                cmd.CommandType = CommandType.StoredProcedure;


                cmd.Parameters.AddWithValue("@ItemId", SqlDbType.Int).Value = Item_Id;


                DataTable dt = new DataTable();

                int row = cmd.ExecuteNonQuery();





            }
            catch (Exception exp)
            {
                throw exp;
            }
            finally
            {
                if (connectionIndicator)
                {
                    con.Close();
                    connectionIndicator = false;
                }
            }
        }

        public static void SaveMonthsforObservationsActivitiesAttendanceRatio()
        {
            try
            {


                if (connectionIndicator == false)
                {
                    con.Open();
                    connectionIndicator = true;
                }
                SqlCommand cmd = new SqlCommand("Usp_ObservationActivitiesMonthsAttendanceRatio", con);
                cmd.CommandType = CommandType.StoredProcedure;

//                cmd.Parameters.AddWithValue("@ItemId", SqlDbType.Int).Value = Item_Id;


                DataTable dt = new DataTable();

                int row = cmd.ExecuteNonQuery();





            }
            catch (Exception exp)
            {
                throw exp;
            }
            finally
            {
                if (connectionIndicator)
                {
                    con.Close();
                    connectionIndicator = false;
                }
            }
        }
        public static void SaveMonthsforObservationsActivitiesProductionEfficiency(int Item_Id)
        {
            try
            {


                if (connectionIndicator == false)
                {
                    con.Open();
                    connectionIndicator = true;
                }
                SqlCommand cmd = new SqlCommand("Usp_ObservationActivitiesMonthsProductionEfficiency", con);
                cmd.CommandType = CommandType.StoredProcedure;


                cmd.Parameters.AddWithValue("@ItemId", SqlDbType.Int).Value = Item_Id;


                DataTable dt = new DataTable();

                int row = cmd.ExecuteNonQuery();





            }
            catch (Exception exp)
            {
                throw exp;
            }
            finally
            {
                if (connectionIndicator)
                {
                    con.Close();
                    connectionIndicator = false;
                }
            }
        }
        public static void SaveMonthsforObservationsActivitiesProdAchivRate(int Item_Id)
        {
            try
            {


                if (connectionIndicator == false)
                {
                    con.Open();
                    connectionIndicator = true;
                }
                SqlCommand cmd = new SqlCommand("Usp_ObservationActivitiesMonthsProdAchivRate", con);
                cmd.CommandType = CommandType.StoredProcedure;


                cmd.Parameters.AddWithValue("@ItemId", SqlDbType.Int).Value = Item_Id;


                DataTable dt = new DataTable();

                int row = cmd.ExecuteNonQuery();





            }
            catch (Exception exp)
            {
                throw exp;
            }
            finally
            {
                if (connectionIndicator)
                {
                    con.Close();
                    connectionIndicator = false;
                }
            }
        }
        public static void SaveMonthsforObservationsActivitiesCustomerClaims(int Item_Id)
        {
            try
            {


                if (connectionIndicator == false)
                {
                    con.Open();
                    connectionIndicator = true;
                }
                SqlCommand cmd = new SqlCommand("Usp_ObservationActivitiesMonthsCustomerClaims", con);
                cmd.CommandType = CommandType.StoredProcedure;


                cmd.Parameters.AddWithValue("@ItemId", SqlDbType.Int).Value = Item_Id;


                DataTable dt = new DataTable();

                int row = cmd.ExecuteNonQuery();





            }
            catch (Exception exp)
            {
                throw exp;
            }
            finally
            {
                if (connectionIndicator)
                {
                    con.Close();
                    connectionIndicator = false;
                }
            }
        }
        public static void SaveMonthsforObservationsActivitiesDelivaryCompliance(int Item_Id)
        {
            try
            {


                if (connectionIndicator == false)
                {
                    con.Open();
                    connectionIndicator = true;
                }
                SqlCommand cmd = new SqlCommand("Usp_ObservationActivitiesMonthsDelivaryCompliance", con);
                cmd.CommandType = CommandType.StoredProcedure;


                cmd.Parameters.AddWithValue("@ItemId", SqlDbType.Int).Value = Item_Id;


                DataTable dt = new DataTable();

                int row = cmd.ExecuteNonQuery();





            }
            catch (Exception exp)
            {
                throw exp;
            }
            finally
            {
                if (connectionIndicator)
                {
                    con.Close();
                    connectionIndicator = false;
                }
            }
        }

        public static void SaveMonthsforObservationsActivitiesCostofRework(int Item_Id)
        {
            try
            {


                if (connectionIndicator == false)
                {
                    con.Open();
                    connectionIndicator = true;
                }
                SqlCommand cmd = new SqlCommand("Usp_ObservationActivitiesMonthsCostOfRework", con);
                cmd.CommandType = CommandType.StoredProcedure;


                cmd.Parameters.AddWithValue("@ItemId", SqlDbType.Int).Value = Item_Id;


                DataTable dt = new DataTable();

                int row = cmd.ExecuteNonQuery();





            }
            catch (Exception exp)
            {
                throw exp;
            }
            finally
            {
                if (connectionIndicator)
                {
                    con.Close();
                    connectionIndicator = false;
                }
            }
        }

        public static void SaveMonthsforObservationsActivitiesInhouseRejectionRatio(int Item_Id)
        {
            try
            {


                if (connectionIndicator == false)
                {
                    con.Open();
                    connectionIndicator = true;
                }
                SqlCommand cmd = new SqlCommand("Usp_ObservationActivitiesMonthsInhouseRejectionRatio", con);
                cmd.CommandType = CommandType.StoredProcedure;


                cmd.Parameters.AddWithValue("@ItemId", SqlDbType.Int).Value = Item_Id;


                DataTable dt = new DataTable();

                int row = cmd.ExecuteNonQuery();





            }
            catch (Exception exp)
            {
                throw exp;
            }
            finally
            {
                if (connectionIndicator)
                {
                    con.Close();
                    connectionIndicator = false;
                }
            }
        }
        public static void SaveMonthsforObservationsActivitiesInhouseRejectionCost(int Item_Id)
        {
            try
            {


                if (connectionIndicator == false)
                {
                    con.Open();
                    connectionIndicator = true;
                }
                SqlCommand cmd = new SqlCommand("Usp_ObservationActivitiesMonthsInhouseRejectionCost", con);
                cmd.CommandType = CommandType.StoredProcedure;


                cmd.Parameters.AddWithValue("@ItemId", SqlDbType.Int).Value = Item_Id;


                DataTable dt = new DataTable();

                int row = cmd.ExecuteNonQuery();





            }
            catch (Exception exp)
            {
                throw exp;
            }
            finally
            {
                if (connectionIndicator)
                {
                    con.Close();
                    connectionIndicator = false;
                }
            }
        }

        public static void SaveMonthsforObservationsActivitiesCustomerRejectionCost(int Item_Id)
        {
            try
            {


                if (connectionIndicator == false)
                {
                    con.Open();
                    connectionIndicator = true;
                }
                SqlCommand cmd = new SqlCommand("Usp_ObservationActivitiesMonthsCustomerRejectionCost", con);
                cmd.CommandType = CommandType.StoredProcedure;


                cmd.Parameters.AddWithValue("@ItemId", SqlDbType.Int).Value = Item_Id;


                DataTable dt = new DataTable();

                int row = cmd.ExecuteNonQuery();





            }
            catch (Exception exp)
            {
                throw exp;
            }
            finally
            {
                if (connectionIndicator)
                {
                    con.Close();
                    connectionIndicator = false;
                }
            }
        }
        public static DataSet GetActivitesObservationReworkRatio(int ItemId)
        {
            try
            {


                if (connectionIndicator == false)
                {
                    con.Open();
                    connectionIndicator = true;
                }
               
                DataSet ds = new DataSet("TimeRanges");

                SqlCommand sqlComm = new SqlCommand("usp_GetActivitesObservationReworkRatio", con);
                sqlComm.Parameters.AddWithValue("@ItemId", SqlDbType.Int).Value = ItemId;
                sqlComm.CommandType = CommandType.StoredProcedure;

                SqlDataAdapter da = new SqlDataAdapter();
                da.SelectCommand = sqlComm;

                da.Fill(ds);

                return ds;


            }
            catch (Exception exp)
            {
                throw exp;
            }
            finally
            {
                if (connectionIndicator)
                {
                    con.Close();
                    connectionIndicator = false;
                }
            }
            
        }


        

        public static DataSet GetActivitesObservationMaterialYieldVarianceCost(int ItemId)
        {
            try
            {


                if (connectionIndicator == false)
                {
                    con.Open();
                    connectionIndicator = true;
                }

                DataSet ds = new DataSet("TimeRanges");

                SqlCommand sqlComm = new SqlCommand("usp_GetActivitesObservationMaterialYieldVarianceCost", con);
                sqlComm.Parameters.AddWithValue("@ItemId", SqlDbType.Int).Value = ItemId;
                sqlComm.CommandType = CommandType.StoredProcedure;

                SqlDataAdapter da = new SqlDataAdapter();
                da.SelectCommand = sqlComm;

                da.Fill(ds);

                return ds;


            }
            catch (Exception exp)
            {
                throw exp;
            }
            finally
            {
                if (connectionIndicator)
                {
                    con.Close();
                    connectionIndicator = false;
                }
            }

        }
        public static DataSet GetActivitesObservationOEE(int ItemId)
        {
            try
            {


                if (connectionIndicator == false)
                {
                    con.Open();
                    connectionIndicator = true;
                }

                DataSet ds = new DataSet("TimeRanges");

                SqlCommand sqlComm = new SqlCommand("usp_GetActivitesObservationOEE", con);
                sqlComm.Parameters.AddWithValue("@ItemId", SqlDbType.Int).Value = ItemId;
                sqlComm.CommandType = CommandType.StoredProcedure;

                SqlDataAdapter da = new SqlDataAdapter();
                da.SelectCommand = sqlComm;

                da.Fill(ds);

                return ds;


            }
            catch (Exception exp)
            {
                throw exp;
            }
            finally
            {
                if (connectionIndicator)
                {
                    con.Close();
                    connectionIndicator = false;
                }
            }

        }

        public static DataSet GetActivitesObservationEquipFailRateMonthlyWorkShop(int WorkShop_Id)
        {
            try
            {


                if (connectionIndicator == false)
                {
                    con.Open();
                    connectionIndicator = true;
                }

                DataSet ds = new DataSet("TimeRanges");

                SqlCommand sqlComm = new SqlCommand("usp_GetActivitesObservationEquipFailRateWorkShop", con);
                sqlComm.Parameters.AddWithValue("@WorkShop", SqlDbType.Int).Value = WorkShop_Id;
                sqlComm.CommandType = CommandType.StoredProcedure;

                SqlDataAdapter da = new SqlDataAdapter();
                da.SelectCommand = sqlComm;

                da.Fill(ds);

                return ds;


            }
            catch (Exception exp)
            {
                throw exp;
            }
            finally
            {
                if (connectionIndicator)
                {
                    con.Close();
                    connectionIndicator = false;
                }
            }

        }
        public static DataSet GetActivitesObservationCustomerRejectionRatio(int ItemId)
        {
            try
            {


                if (connectionIndicator == false)
                {
                    con.Open();
                    connectionIndicator = true;
                }

                DataSet ds = new DataSet("TimeRanges");

                SqlCommand sqlComm = new SqlCommand("usp_GetActivitesObservationCustomerRejectionRatio", con);
                sqlComm.Parameters.AddWithValue("@ItemId", SqlDbType.Int).Value = ItemId;
                sqlComm.CommandType = CommandType.StoredProcedure;

                SqlDataAdapter da = new SqlDataAdapter();
                da.SelectCommand = sqlComm;

                da.Fill(ds);

                return ds;


            }
            catch (Exception exp)
            {
                throw exp;
            }
            finally
            {
                if (connectionIndicator)
                {
                    con.Close();
                    connectionIndicator = false;
                }
            }

        }

        public static DataSet GetActivitesObservationAttendanceRatio()
        {
            try
            {


                if (connectionIndicator == false)
                {
                    con.Open();
                    connectionIndicator = true;
                }

                DataSet ds = new DataSet("TimeRanges");

                SqlCommand sqlComm = new SqlCommand("usp_GetActivitesObservationAttendanceRatio", con);
               // sqlComm.Parameters.AddWithValue("@ItemId", SqlDbType.Int).Value = ItemId;
                sqlComm.CommandType = CommandType.StoredProcedure;

                SqlDataAdapter da = new SqlDataAdapter();
                da.SelectCommand = sqlComm;

                da.Fill(ds);

                return ds;


            }
            catch (Exception exp)
            {
                throw exp;
            }
            finally
            {
                if (connectionIndicator)
                {
                    con.Close();
                    connectionIndicator = false;
                }
            }

        }
        public static DataSet GetActivitesObservationProductionEfficiency(int ItemId)
        {
            try
            {


                if (connectionIndicator == false)
                {
                    con.Open();
                    connectionIndicator = true;
                }

                DataSet ds = new DataSet("TimeRanges");

                SqlCommand sqlComm = new SqlCommand("usp_GetActivitesObservationProductionEfficiency", con);
                sqlComm.Parameters.AddWithValue("@ItemId", SqlDbType.Int).Value = ItemId;
                sqlComm.CommandType = CommandType.StoredProcedure;

                SqlDataAdapter da = new SqlDataAdapter();
                da.SelectCommand = sqlComm;

                da.Fill(ds);

                return ds;


            }
            catch (Exception exp)
            {
                throw exp;
            }
            finally
            {
                if (connectionIndicator)
                {
                    con.Close();
                    connectionIndicator = false;
                }
            }

        }
        public static DataSet GetActivitesObservationProdAchivRate(int ItemId)
        {
            try
            {


                if (connectionIndicator == false)
                {
                    con.Open();
                    connectionIndicator = true;
                }

                DataSet ds = new DataSet("TimeRanges");

                SqlCommand sqlComm = new SqlCommand("usp_GetActivitesObservationProdAchivRate", con);
                sqlComm.Parameters.AddWithValue("@ItemId", SqlDbType.Int).Value = ItemId;
                sqlComm.CommandType = CommandType.StoredProcedure;

                SqlDataAdapter da = new SqlDataAdapter();
                da.SelectCommand = sqlComm;

                da.Fill(ds);

                return ds;


            }
            catch (Exception exp)
            {
                throw exp;
            }
            finally
            {
                if (connectionIndicator)
                {
                    con.Close();
                    connectionIndicator = false;
                }
            }

        }
        public static DataSet GetActivitesObservationCustomerClaims(int ItemId)
        {
            try
            {


                if (connectionIndicator == false)
                {
                    con.Open();
                    connectionIndicator = true;
                }

                DataSet ds = new DataSet("TimeRanges");

                SqlCommand sqlComm = new SqlCommand("usp_GetActivitesObservationCustomerClaims", con);
                sqlComm.Parameters.AddWithValue("@ItemId", SqlDbType.Int).Value = ItemId;
                sqlComm.CommandType = CommandType.StoredProcedure;

                SqlDataAdapter da = new SqlDataAdapter();
                da.SelectCommand = sqlComm;

                da.Fill(ds);

                return ds;


            }
            catch (Exception exp)
            {
                throw exp;
            }
            finally
            {
                if (connectionIndicator)
                {
                    con.Close();
                    connectionIndicator = false;
                }
            }

        }

        public static DataSet GetActivitesObservationDelivaryCompliance(int ItemId)
        {
            try
            {


                if (connectionIndicator == false)
                {
                    con.Open();
                    connectionIndicator = true;
                }

                DataSet ds = new DataSet("TimeRanges");

                SqlCommand sqlComm = new SqlCommand("usp_GetActivitesObservationDelivaryCompliance", con);
                sqlComm.Parameters.AddWithValue("@ItemId", SqlDbType.Int).Value = ItemId;
                sqlComm.CommandType = CommandType.StoredProcedure;

                SqlDataAdapter da = new SqlDataAdapter();
                da.SelectCommand = sqlComm;

                da.Fill(ds);

                return ds;


            }
            catch (Exception exp)
            {
                throw exp;
            }
            finally
            {
                if (connectionIndicator)
                {
                    con.Close();
                    connectionIndicator = false;
                }
            }

        }


        public static DataSet GetActivitesObservationCostOfRework(int ItemId)
        {
            try
            {


                if (connectionIndicator == false)
                {
                    con.Open();
                    connectionIndicator = true;
                }

                DataSet ds = new DataSet("TimeRanges");

                SqlCommand sqlComm = new SqlCommand("usp_GetActivitesObservationReworkCost", con);
                sqlComm.Parameters.AddWithValue("@ItemId", SqlDbType.Int).Value = ItemId;
                sqlComm.CommandType = CommandType.StoredProcedure;

                SqlDataAdapter da = new SqlDataAdapter();
                da.SelectCommand = sqlComm;

                da.Fill(ds);

                return ds;


            }
            catch (Exception exp)
            {
                throw exp;
            }
            finally
            {
                if (connectionIndicator)
                {
                    con.Close();
                    connectionIndicator = false;
                }
            }

        }


        public static DataSet GetActivitesObservationInhouseRejectionRatio(int ItemId)
        {
            try
            {


                if (connectionIndicator == false)
                {
                    con.Open();
                    connectionIndicator = true;
                }

                DataSet ds = new DataSet("TimeRanges");

                SqlCommand sqlComm = new SqlCommand("usp_GetActivitesObservationInhouseRejectionRatio", con);
                sqlComm.Parameters.AddWithValue("@ItemId", SqlDbType.Int).Value = ItemId;
                sqlComm.CommandType = CommandType.StoredProcedure;

                SqlDataAdapter da = new SqlDataAdapter();
                da.SelectCommand = sqlComm;

                da.Fill(ds);

                return ds;


            }
            catch (Exception exp)
            {
                throw exp;
            }
            finally
            {
                if (connectionIndicator)
                {
                    con.Close();
                    connectionIndicator = false;
                }
            }

        }

        public static DataSet GetActivitesObservationInhouseRejCost(int ItemId)
        {
            try
            {


                if (connectionIndicator == false)
                {
                    con.Open();
                    connectionIndicator = true;
                }

                DataSet ds = new DataSet("TimeRanges");

                SqlCommand sqlComm = new SqlCommand("usp_GetActivitesObservationInhouseRejectionCost", con);
                sqlComm.Parameters.AddWithValue("@ItemId", SqlDbType.Int).Value = ItemId;
                sqlComm.CommandType = CommandType.StoredProcedure;

                SqlDataAdapter da = new SqlDataAdapter();
                da.SelectCommand = sqlComm;

                da.Fill(ds);

                return ds;


            }
            catch (Exception exp)
            {
                throw exp;
            }
            finally
            {
                if (connectionIndicator)
                {
                    con.Close();
                    connectionIndicator = false;
                }
            }

        }
        public static DataSet GetActivitesObservationCustomerRejectionCost(int ItemId)
        {
            try
            {


                if (connectionIndicator == false)
                {
                    con.Open();
                    connectionIndicator = true;
                }

                DataSet ds = new DataSet("TimeRanges");

                SqlCommand sqlComm = new SqlCommand("usp_GetActivitesObservationCustomerRejectionCost", con);
                sqlComm.Parameters.AddWithValue("@ItemId", SqlDbType.Int).Value = ItemId;
                sqlComm.CommandType = CommandType.StoredProcedure;

                SqlDataAdapter da = new SqlDataAdapter();
                da.SelectCommand = sqlComm;

                da.Fill(ds);

                return ds;


            }
            catch (Exception exp)
            {
                throw exp;
            }
            finally
            {
                if (connectionIndicator)
                {
                    con.Close();
                    connectionIndicator = false;
                }
            }

        }
        public static void EditReworkRatioObservation(int Item_Id, string observations, int Month, int Obs_or_activity)
        {
            try
            {

            if (connectionIndicator == false)
                {
                    con.Open();
                    connectionIndicator = true;
                }
            SqlCommand cmd = new SqlCommand("Usp_EditObservationActivitiesReworkRatio", con);
                cmd.CommandType = CommandType.StoredProcedure;


              cmd.Parameters.AddWithValue("@ItemId", SqlDbType.Int).Value = Item_Id;
              cmd.Parameters.AddWithValue("@Obs", SqlDbType.VarChar).Value = observations;
              cmd.Parameters.AddWithValue("@MonthsID", SqlDbType.Int).Value = Month;
              cmd.Parameters.AddWithValue("@ObsOrActivity", SqlDbType.Int).Value = Obs_or_activity;
              

                DataTable dt = new DataTable();

                int row= cmd.ExecuteNonQuery();

                



            }
            catch (Exception exp)
            {
                throw exp;
            }
            finally
            {
                if (connectionIndicator)
                {
                    con.Close();
                    connectionIndicator = false;
                }
            }
        }


        public static void EditMaterialYieldVarianceCostObservation(int Item_Id, string observations, int Month, int Obs_or_activity)
        {
            try
            {

                if (connectionIndicator == false)
                {
                    con.Open();
                    connectionIndicator = true;
                }
                SqlCommand cmd = new SqlCommand("Usp_EditObservationActivitiesMaterialYieldVarianceMonthlyCost", con);
                cmd.CommandType = CommandType.StoredProcedure;


                cmd.Parameters.AddWithValue("@ItemId", SqlDbType.Int).Value = Item_Id;
                cmd.Parameters.AddWithValue("@Obs", SqlDbType.VarChar).Value = observations;
                cmd.Parameters.AddWithValue("@MonthsID", SqlDbType.Int).Value = Month;
                cmd.Parameters.AddWithValue("@ObsOrActivity", SqlDbType.Int).Value = Obs_or_activity;


                DataTable dt = new DataTable();

                int row = cmd.ExecuteNonQuery();





            }
            catch (Exception exp)
            {
                throw exp;
            }
            finally
            {
                if (connectionIndicator)
                {
                    con.Close();
                    connectionIndicator = false;
                }
            }
        }
        public static void EditOEEObservation(int Item_Id, string observations, int Month, int Obs_or_activity)
        {
            try
            {

                if (connectionIndicator == false)
                {
                    con.Open();
                    connectionIndicator = true;
                }
                SqlCommand cmd = new SqlCommand("Usp_EditObservationActivitiesOEE", con);
                cmd.CommandType = CommandType.StoredProcedure;


                cmd.Parameters.AddWithValue("@ItemId", SqlDbType.Int).Value = Item_Id;
                cmd.Parameters.AddWithValue("@Obs", SqlDbType.VarChar).Value = observations;
                cmd.Parameters.AddWithValue("@MonthsID", SqlDbType.Int).Value = Month;
                cmd.Parameters.AddWithValue("@ObsOrActivity", SqlDbType.Int).Value = Obs_or_activity;


                DataTable dt = new DataTable();

                int row = cmd.ExecuteNonQuery();





            }
            catch (Exception exp)
            {
                throw exp;
            }
            finally
            {
                if (connectionIndicator)
                {
                    con.Close();
                    connectionIndicator = false;
                }
            }
        }

        public static void EditEquipFailureRateShopMonthlyObservation(int WorkShop, string observations, int Month, int Obs_or_activity)
        {
            try
            {

                if (connectionIndicator == false)
                {
                    con.Open();
                    connectionIndicator = true;
                }
                SqlCommand cmd = new SqlCommand("Usp_EditObservationActivitiesEquipFailureRateMonthlyShop", con);
                cmd.CommandType = CommandType.StoredProcedure;


                cmd.Parameters.AddWithValue("@WorkShopId", SqlDbType.Int).Value = WorkShop;
                cmd.Parameters.AddWithValue("@Obs", SqlDbType.VarChar).Value = observations;
                cmd.Parameters.AddWithValue("@MonthsID", SqlDbType.Int).Value = Month;
                cmd.Parameters.AddWithValue("@ObsOrActivity", SqlDbType.Int).Value = Obs_or_activity;


                DataTable dt = new DataTable();

                int row = cmd.ExecuteNonQuery();





            }
            catch (Exception exp)
            {
                throw exp;
            }
            finally
            {
                if (connectionIndicator)
                {
                    con.Close();
                    connectionIndicator = false;
                }
            }
        }
        public static void EditCustomerRejectionRatioObservation(int Item_Id, string observations, int Month, int Obs_or_activity)
        {
            try
            {

                if (connectionIndicator == false)
                {
                    con.Open();
                    connectionIndicator = true;
                }
                SqlCommand cmd = new SqlCommand("Usp_EditObservationActivitiesCustomerRejectionRatio", con);
                cmd.CommandType = CommandType.StoredProcedure;


                cmd.Parameters.AddWithValue("@ItemId", SqlDbType.Int).Value = Item_Id;
                cmd.Parameters.AddWithValue("@Obs", SqlDbType.VarChar).Value = observations;
                cmd.Parameters.AddWithValue("@MonthsID", SqlDbType.Int).Value = Month;
                cmd.Parameters.AddWithValue("@ObsOrActivity", SqlDbType.Int).Value = Obs_or_activity;


                DataTable dt = new DataTable();

                int row = cmd.ExecuteNonQuery();





            }
            catch (Exception exp)
            {
                throw exp;
            }
            finally
            {
                if (connectionIndicator)
                {
                    con.Close();
                    connectionIndicator = false;
                }
            }
        }


        public static void EditCustomerAttendanceRatioObservation(string observations, int Month, int Obs_or_activity)
        {
            try
            {

                if (connectionIndicator == false)
                {
                    con.Open();
                    connectionIndicator = true;
                }
                SqlCommand cmd = new SqlCommand("Usp_EditObservationActivitiesAttendanceRatio", con);
                cmd.CommandType = CommandType.StoredProcedure;


                //cmd.Parameters.AddWithValue("@ItemId", SqlDbType.Int).Value = Item_Id;
                cmd.Parameters.AddWithValue("@Obs", SqlDbType.VarChar).Value = observations;
                cmd.Parameters.AddWithValue("@MonthsID", SqlDbType.Int).Value = Month;
                cmd.Parameters.AddWithValue("@ObsOrActivity", SqlDbType.Int).Value = Obs_or_activity;


                DataTable dt = new DataTable();

                int row = cmd.ExecuteNonQuery();





            }
            catch (Exception exp)
            {
                throw exp;
            }
            finally
            {
                if (connectionIndicator)
                {
                    con.Close();
                    connectionIndicator = false;
                }
            }
        }
        public static void EditProductionEfficiencyObservation(int Item_Id, string observations, int Month, int Obs_or_activity)
        {
            try
            {

                if (connectionIndicator == false)
                {
                    con.Open();
                    connectionIndicator = true;
                }
                SqlCommand cmd = new SqlCommand("Usp_EditObservationActivitiesProductionEfficiency", con);
                cmd.CommandType = CommandType.StoredProcedure;


                cmd.Parameters.AddWithValue("@ItemId", SqlDbType.Int).Value = Item_Id;
                cmd.Parameters.AddWithValue("@Obs", SqlDbType.VarChar).Value = observations;
                cmd.Parameters.AddWithValue("@MonthsID", SqlDbType.Int).Value = Month;
                cmd.Parameters.AddWithValue("@ObsOrActivity", SqlDbType.Int).Value = Obs_or_activity;


                DataTable dt = new DataTable();

                int row = cmd.ExecuteNonQuery();





            }
            catch (Exception exp)
            {
                throw exp;
            }
            finally
            {
                if (connectionIndicator)
                {
                    con.Close();
                    connectionIndicator = false;
                }
            }
        }

        public static void EditProdAchivRateObservation(int Item_Id, string observations, int Month, int Obs_or_activity)
        {
            try
            {

                if (connectionIndicator == false)
                {
                    con.Open();
                    connectionIndicator = true;
                }
                SqlCommand cmd = new SqlCommand("Usp_EditObservationActivitiesProdAchivRate", con);
                cmd.CommandType = CommandType.StoredProcedure;


                cmd.Parameters.AddWithValue("@ItemId", SqlDbType.Int).Value = Item_Id;
                cmd.Parameters.AddWithValue("@Obs", SqlDbType.VarChar).Value = observations;
                cmd.Parameters.AddWithValue("@MonthsID", SqlDbType.Int).Value = Month;
                cmd.Parameters.AddWithValue("@ObsOrActivity", SqlDbType.Int).Value = Obs_or_activity;


                DataTable dt = new DataTable();

                int row = cmd.ExecuteNonQuery();





            }
            catch (Exception exp)
            {
                throw exp;
            }
            finally
            {
                if (connectionIndicator)
                {
                    con.Close();
                    connectionIndicator = false;
                }
            }
        }
        public static void EditCustomerClaimsObservation(int Item_Id, string observations, int Month, int Obs_or_activity)
        {
            try
            {

                if (connectionIndicator == false)
                {
                    con.Open();
                    connectionIndicator = true;
                }
                SqlCommand cmd = new SqlCommand("Usp_EditObservationActivitiesCustomerClaims", con);
                cmd.CommandType = CommandType.StoredProcedure;


                cmd.Parameters.AddWithValue("@ItemId", SqlDbType.Int).Value = Item_Id;
                cmd.Parameters.AddWithValue("@Obs", SqlDbType.VarChar).Value = observations;
                cmd.Parameters.AddWithValue("@MonthsID", SqlDbType.Int).Value = Month;
                cmd.Parameters.AddWithValue("@ObsOrActivity", SqlDbType.Int).Value = Obs_or_activity;


                DataTable dt = new DataTable();

                int row = cmd.ExecuteNonQuery();





            }
            catch (Exception exp)
            {
                throw exp;
            }
            finally
            {
                if (connectionIndicator)
                {
                    con.Close();
                    connectionIndicator = false;
                }
            }
        }

        public static void EditDelivaryComplianceObservation(int Item_Id, string observations, int Month, int Obs_or_activity)
        {
            try
            {

                if (connectionIndicator == false)
                {
                    con.Open();
                    connectionIndicator = true;
                }
                SqlCommand cmd = new SqlCommand("Usp_EditObservationActivitiesDelivaryCompliance", con);
                cmd.CommandType = CommandType.StoredProcedure;


                cmd.Parameters.AddWithValue("@ItemId", SqlDbType.Int).Value = Item_Id;
                cmd.Parameters.AddWithValue("@Obs", SqlDbType.VarChar).Value = observations;
                cmd.Parameters.AddWithValue("@MonthsID", SqlDbType.Int).Value = Month;
                cmd.Parameters.AddWithValue("@ObsOrActivity", SqlDbType.Int).Value = Obs_or_activity;


                DataTable dt = new DataTable();

                int row = cmd.ExecuteNonQuery();





            }
            catch (Exception exp)
            {
                throw exp;
            }
            finally
            {
                if (connectionIndicator)
                {
                    con.Close();
                    connectionIndicator = false;
                }
            }
        }
        public static void EditCustomerCostofRework(int Item_Id, string observations, int Month, int Obs_or_activity)
        {
            try
            {

                if (connectionIndicator == false)
                {
                    con.Open();
                    connectionIndicator = true;
                }
                SqlCommand cmd = new SqlCommand("usp_GetActivitesObservationReworkCost", con);
                cmd.CommandType = CommandType.StoredProcedure;


                cmd.Parameters.AddWithValue("@ItemId", SqlDbType.Int).Value = Item_Id;
                cmd.Parameters.AddWithValue("@Obs", SqlDbType.VarChar).Value = observations;
                cmd.Parameters.AddWithValue("@MonthsID", SqlDbType.Int).Value = Month;
                cmd.Parameters.AddWithValue("@ObsOrActivity", SqlDbType.Int).Value = Obs_or_activity;


                DataTable dt = new DataTable();

                int row = cmd.ExecuteNonQuery();





            }
            catch (Exception exp)
            {
                throw exp;
            }
            finally
            {
                if (connectionIndicator)
                {
                    con.Close();
                    connectionIndicator = false;
                }
            }
        }
        public static void EditInhouseRejectionRatioObservation(int Item_Id, string observations, int Month, int Obs_or_activity)
        {
            try
            {

                if (connectionIndicator == false)
                {
                    con.Open();
                    connectionIndicator = true;
                }
                SqlCommand cmd = new SqlCommand("Usp_EditObservationActivitiesInhouseRejectionRatio", con);
                cmd.CommandType = CommandType.StoredProcedure;


                cmd.Parameters.AddWithValue("@ItemId", SqlDbType.Int).Value = Item_Id;
                cmd.Parameters.AddWithValue("@Obs", SqlDbType.VarChar).Value = observations;
                cmd.Parameters.AddWithValue("@MonthsID", SqlDbType.Int).Value = Month;
                cmd.Parameters.AddWithValue("@ObsOrActivity", SqlDbType.Int).Value = Obs_or_activity;


                DataTable dt = new DataTable();

                int row = cmd.ExecuteNonQuery();





            }
            catch (Exception exp)
            {
                throw exp;
            }
            finally
            {
                if (connectionIndicator)
                {
                    con.Close();
                    connectionIndicator = false;
                }
            }
        }

        public static void EditInhouseRejectionCostObservation(int Item_Id, string observations, int Month, int Obs_or_activity)
        {
            try
            {

                if (connectionIndicator == false)
                {
                    con.Open();
                    connectionIndicator = true;
                }
                SqlCommand cmd = new SqlCommand("Usp_EditObservationActivitiesInhouseRejectionCost", con);
                cmd.CommandType = CommandType.StoredProcedure;


                cmd.Parameters.AddWithValue("@ItemId", SqlDbType.Int).Value = Item_Id;
                cmd.Parameters.AddWithValue("@Obs", SqlDbType.VarChar).Value = observations;
                cmd.Parameters.AddWithValue("@MonthsID", SqlDbType.Int).Value = Month;
                cmd.Parameters.AddWithValue("@ObsOrActivity", SqlDbType.Int).Value = Obs_or_activity;


                DataTable dt = new DataTable();

                int row = cmd.ExecuteNonQuery();





            }
            catch (Exception exp)
            {
                throw exp;
            }
            finally
            {
                if (connectionIndicator)
                {
                    con.Close();
                    connectionIndicator = false;
                }
            }
        }
        public static void EditCustomerRejectionCostObservation(int Item_Id, string observations, int Month, int Obs_or_activity)
        {
            try
            {

                if (connectionIndicator == false)
                {
                    con.Open();
                    connectionIndicator = true;
                }
                SqlCommand cmd = new SqlCommand("Usp_EditObservationActivitiesCustomerRejectionCost", con);
                cmd.CommandType = CommandType.StoredProcedure;


                cmd.Parameters.AddWithValue("@ItemId", SqlDbType.Int).Value = Item_Id;
                cmd.Parameters.AddWithValue("@Obs", SqlDbType.VarChar).Value = observations;
                cmd.Parameters.AddWithValue("@MonthsID", SqlDbType.Int).Value = Month;
                cmd.Parameters.AddWithValue("@ObsOrActivity", SqlDbType.Int).Value = Obs_or_activity;


                DataTable dt = new DataTable();

                int row = cmd.ExecuteNonQuery();





            }
            catch (Exception exp)
            {
                throw exp;
            }
            finally
            {
                if (connectionIndicator)
                {
                    con.Close();
                    connectionIndicator = false;
                }
            }
        }

        public static int SaveEmployees(string name, string id, string contact, string address, string email)
        {
            try
            {

                if (connectionIndicator == false)
                {
                    con.Open();
                    connectionIndicator = true;
                }
                SqlCommand cmd = new SqlCommand("Usp_SaveEmployee", con);
                cmd.CommandType = CommandType.StoredProcedure;


                cmd.Parameters.AddWithValue("@Name", SqlDbType.VarChar).Value = name;
                cmd.Parameters.AddWithValue("@id", SqlDbType.VarChar).Value = id;
                cmd.Parameters.AddWithValue("@contact", SqlDbType.VarChar).Value = contact;
                cmd.Parameters.AddWithValue("@address", SqlDbType.VarChar).Value = address;
                cmd.Parameters.AddWithValue("@email", SqlDbType.VarChar).Value = email;


                DataTable dt = new DataTable();

                int row = cmd.ExecuteNonQuery();

                return row;



            }
            catch (Exception exp)
            {
                throw exp;
            }
            finally
            {
                if (connectionIndicator)
                {
                    con.Close();
                    connectionIndicator = false;
                }
            }
        }

        public static int EditEmployees(string name, string id, string contact, string address, string email , string oldId)
        {
            try
            {

                if (connectionIndicator == false)
                {
                    con.Open();
                    connectionIndicator = true;
                }
                SqlCommand cmd = new SqlCommand("Usp_EditEmployee", con);
                cmd.CommandType = CommandType.StoredProcedure;


                cmd.Parameters.AddWithValue("@Name", SqlDbType.VarChar).Value = name;
                cmd.Parameters.AddWithValue("@id", SqlDbType.VarChar).Value = id;
                cmd.Parameters.AddWithValue("@contact", SqlDbType.VarChar).Value = contact;
                cmd.Parameters.AddWithValue("@address", SqlDbType.VarChar).Value = address;
                cmd.Parameters.AddWithValue("@email", SqlDbType.VarChar).Value = email;
                cmd.Parameters.AddWithValue("@PreviousId", SqlDbType.VarChar).Value = oldId;


                DataTable dt = new DataTable();

                int row = cmd.ExecuteNonQuery();

                return row;



            }
            catch (Exception exp)
            {
                throw exp;
            }
            finally
            {
                if (connectionIndicator)
                {
                    con.Close();
                    connectionIndicator = false;
                }
            }
        }

        public static DataTable GetEmpDetails(string id)
        {
            try
            {


                if (connectionIndicator == false)
                {
                    con.Open();
                    connectionIndicator = true;
                }
                SqlCommand cmd = new SqlCommand("Usp_GetEmployee", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Id", SqlDbType.VarChar).Value = id;
                DataTable dt = new DataTable();

                dt.Load(cmd.ExecuteReader());



                return dt;


            }
            catch (Exception exp)
            {
                throw exp;
            }
            finally
            {
                if (connectionIndicator)
                {
                    con.Close();
                    connectionIndicator = false;
                }
            }
        }

        public static string GetNameofEmployee(string id)
        {
            try
            {


                if (connectionIndicator == false)
                {
                    con.Open();
                    connectionIndicator = true;
                }
                SqlCommand cmd = new SqlCommand("Usp_GetEmployeeName", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Id", SqlDbType.VarChar).Value = id;
                DataTable dt = new DataTable();

                dt.Load(cmd.ExecuteReader());



                return dt.Rows[0][0].ToString();


            }
            catch (Exception exp)
            {
                throw exp;
            }
            finally
            {
                if (connectionIndicator)
                {
                    con.Close();
                    connectionIndicator = false;
                }
            }
        }

        public static string GetEmployeeAttendance(string id, string date)
        {
            try
            {


                if (connectionIndicator == false)
                {
                    con.Open();
                    connectionIndicator = true;
                }
                SqlCommand cmd = new SqlCommand("Usp_GetEmployeeAttendance", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Id", SqlDbType.VarChar).Value = id;
                cmd.Parameters.AddWithValue("@Date", SqlDbType.VarChar).Value = date;
                DataTable dt = new DataTable();

                dt.Load(cmd.ExecuteReader());



                return dt.Rows[0][0].ToString();


            }
            catch (Exception exp)
            {
                throw exp;
            }
            finally
            {
                if (connectionIndicator)
                {
                    con.Close();
                    connectionIndicator = false;
                }
            }
        }
        public static DataTable GetEmployeeAttendanceDetails(string id, string Fromdate,string toDate)
        {
            try
            {


                if (connectionIndicator == false)
                {
                    con.Open();
                    connectionIndicator = true;
                }
                SqlCommand cmd = new SqlCommand("Usp_GetEmployeeAttendanceDetails", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Id", SqlDbType.VarChar).Value = id;
                cmd.Parameters.AddWithValue("@FromDate", SqlDbType.VarChar).Value = Fromdate;
                cmd.Parameters.AddWithValue("@ToDate", SqlDbType.VarChar).Value = toDate;
                DataTable dt = new DataTable();

                dt.Load(cmd.ExecuteReader());



                return dt;


            }
            catch (Exception exp)
            {
                throw exp;
            }
            finally
            {
                if (connectionIndicator)
                {
                    con.Close();
                    connectionIndicator = false;
                }
            }
        }
        public static DataSet GetOverAllReportDetails(int ItemId, string Fromdate, string toDate)
        {
            try
            {


                if (connectionIndicator == false)
                {
                    con.Open();
                    connectionIndicator = true;
                }


                DataSet ds = new DataSet("TimeRanges");

                SqlCommand sqlComm = new SqlCommand("usp_GetOverAllReports", con);
              
                sqlComm.Parameters.AddWithValue("@ItemId", SqlDbType.Int).Value = ItemId;
                sqlComm.Parameters.AddWithValue("@FromDate", SqlDbType.VarChar).Value = Fromdate;
                sqlComm.Parameters.AddWithValue("@ToDate", SqlDbType.VarChar).Value = toDate;
                sqlComm.CommandType = CommandType.StoredProcedure;

                SqlDataAdapter da = new SqlDataAdapter();
                da.SelectCommand = sqlComm;

                da.Fill(ds);
                ds.Tables[0].TableName = "Overall Item Entries";
                ds.Tables[1].TableName = "Daily Entry";
                ds.Tables[2].TableName = "Rework";
                ds.Tables[3].TableName = "Inhouse Rejection";
                ds.Tables[4].TableName = "Customer Rejection";
                ds.Tables[5].TableName = "ItemProcesses";
                ds.Tables[6].TableName = "Item Materials";
                ds.Tables[7].TableName = "Delivery Compliance";
                ds.Tables[8].TableName = "Production Achievement";
                ds.Tables[9].TableName = "Machines";
                ds.Tables[10].TableName = "Machine Process";
                
                return ds;


            }
            catch (Exception exp)
            {
                throw exp;
            }
            finally
            {
                if (connectionIndicator)
                {
                    con.Close();
                    connectionIndicator = false;
                }
            }

        }
     
        public static int MarkAttendance(string Status, string Id, DateTime Date)
        {
            try
            {

                if (connectionIndicator == false)
                {
                    con.Open();
                    connectionIndicator = true;
                }
                SqlCommand cmd = new SqlCommand("Usp_MarkEmployeeAttendance", con);
                cmd.CommandType = CommandType.StoredProcedure;



                cmd.Parameters.AddWithValue("@Id", SqlDbType.VarChar).Value = Id;
                cmd.Parameters.AddWithValue("@Status", SqlDbType.VarChar).Value = Status;
                cmd.Parameters.AddWithValue("@Date", SqlDbType.DateTime).Value = Date;
                cmd.Parameters.AddWithValue("@UserId", SqlDbType.Int).Value = UserId;
               


                DataTable dt = new DataTable();

                int row = cmd.ExecuteNonQuery();
                return row;
              



            }
            catch (Exception exp)
            {
                throw exp;
            }
            finally
            {
                if (connectionIndicator)
                {
                    con.Close();
                    connectionIndicator = false;
                }
            }
        }

        public static DataTable GetDieAndMoldDataToEdit(int moldId)
        {
            try
            {


                if (connectionIndicator == false)
                {
                    con.Open();
                    connectionIndicator = true;
                }
                SqlCommand cmd = new SqlCommand("Usp_GetDieAndMoldToEdit", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Id", SqlDbType.VarChar).Value = moldId;
              
                DataTable dt = new DataTable();

                dt.Load(cmd.ExecuteReader());



                return dt;


            }
            catch (Exception exp)
            {
                throw exp;
            }
            finally
            {
                if (connectionIndicator)
                {
                    con.Close();
                    connectionIndicator = false;
                }
            }
           
        }

        public static DataTable GetDieAndMoldDataToEditByCode(string MoldCode, int ItemId)
        {
            try
            {


                if (connectionIndicator == false)
                {
                    con.Open();
                    connectionIndicator = true;
                }
                SqlCommand cmd = new SqlCommand("Usp_GetDieAndMoldToEditByCode", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Code", SqlDbType.VarChar).Value = MoldCode;
                cmd.Parameters.AddWithValue("@ItemId", SqlDbType.Int).Value = ItemId;

                DataTable dt = new DataTable();

                dt.Load(cmd.ExecuteReader());



                return dt;


            }
            catch (Exception exp)
            {
                throw exp;
            }
            finally
            {
                if (connectionIndicator)
                {
                    con.Close();
                    connectionIndicator = false;
                }
            }

        }

        public static DataTable GetDieAndMoldsByItem(int Item_Id)
        {
            try
            {


                if (connectionIndicator == false)
                {
                    con.Open();
                    connectionIndicator = true;
                }
                SqlCommand cmd = new SqlCommand("GetMoldsAndDiesOfSelectedItem", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ItemId", SqlDbType.Int).Value = Item_Id;

                DataTable dt = new DataTable();

                dt.Load(cmd.ExecuteReader());



                return dt;


            }
            catch (Exception exp)
            {
                throw exp;
            }
            finally
            {
                if (connectionIndicator)
                {
                    con.Close();
                    connectionIndicator = false;
                }
            }
        }

        public static int PerformDieAndMoldMaintenance(int MoldId)
        {
            try
            {

                if (connectionIndicator == false)
                {
                    con.Open();
                    connectionIndicator = true;
                }
                SqlCommand cmd = new SqlCommand("Usp_PerformDieAndMoldMaintenance", con);
                cmd.CommandType = CommandType.StoredProcedure;



                cmd.Parameters.AddWithValue("@Id", SqlDbType.Int).Value = MoldId;
                cmd.Parameters.AddWithValue("@UserId", SqlDbType.Int).Value = UserId;



                DataTable dt = new DataTable();

               int rows= cmd.ExecuteNonQuery();
               return rows;




            }
            catch (Exception exp)
            {
                throw exp;
            }
            finally
            {
                if (connectionIndicator)
                {
                    con.Close();
                    connectionIndicator = false;
                }
            }
        }

        public static int PerformTPMMachineMaintenance(int MachineId)
        {
            try
            {

                if (connectionIndicator == false)
                {
                    con.Open();
                    connectionIndicator = true;
                }
                SqlCommand cmd = new SqlCommand("Usp_PerformTPMMachineMoldMaintenance", con);
                cmd.CommandType = CommandType.StoredProcedure;



                cmd.Parameters.AddWithValue("@Id", SqlDbType.Int).Value = MachineId;
                cmd.Parameters.AddWithValue("@UserId", SqlDbType.Int).Value = UserId;



                DataTable dt = new DataTable();

                int rows = cmd.ExecuteNonQuery();
                return rows;




            }
            catch (Exception exp)
            {
                throw exp;
            }
            finally
            {
                if (connectionIndicator)
                {
                    con.Close();
                    connectionIndicator = false;
                }
            }
        }

        public static DataTable GetShiftTime()
        {
            try
            {


                if (connectionIndicator == false)
                {
                    con.Open();
                    connectionIndicator = true;
                }
                SqlCommand cmd = new SqlCommand("Usp_GetShiftTime", con);
                cmd.CommandType = CommandType.StoredProcedure;
               

                DataTable dt = new DataTable();

                dt.Load(cmd.ExecuteReader());



                return dt;


            }
            catch (Exception exp)
            {
                throw exp;
            }
            finally
            {
                if (connectionIndicator)
                {
                    con.Close();
                    connectionIndicator = false;
                }
            }
        }

        public static DataTable GetMachineByCode(string Machine_Code)
        {

            try
            {


                if (connectionIndicator == false)
                {
                    con.Open();
                    connectionIndicator = true;
                }
                SqlCommand cmd = new SqlCommand("Usp_GetMachineByCode", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Code", SqlDbType.VarChar).Value = Machine_Code;

                DataTable dt = new DataTable();

                dt.Load(cmd.ExecuteReader());



                return dt;


            }
            catch (Exception exp)
            {
                throw exp;
            }
            finally
            {
                if (connectionIndicator)
                {
                    con.Close();
                    connectionIndicator = false;
                }
            }
            
        }

        public static DataTable GetMachineItemCodeAndName(int ItemId)
        {

            try
            {


                if (connectionIndicator == false)
                {
                    con.Open();
                    connectionIndicator = true;
                }
                SqlCommand cmd = new SqlCommand("Usp_GetMachineItemCodeAndName", con);
                cmd.CommandType = CommandType.StoredProcedure;
                
                cmd.Parameters.AddWithValue("@ItemId", SqlDbType.VarChar).Value = ItemId;

                DataTable dt = new DataTable();

                dt.Load(cmd.ExecuteReader());



                return dt;


            }
            catch (Exception exp)
            {
                throw exp;
            }
            finally
            {
                if (connectionIndicator)
                {
                    con.Close();
                    connectionIndicator = false;
                }
            }

        }
        public static int RemoveMachineAssociationWithItem(string MachineCode, int ItemId)
        {


            try
            {

                if (connectionIndicator == false)
                {
                    con.Open();
                    connectionIndicator = true;
                }
                SqlCommand cmd = new SqlCommand("Usp_RemoveMachineItem", con);
                cmd.CommandType = CommandType.StoredProcedure;



                cmd.Parameters.AddWithValue("@Code", SqlDbType.VarChar).Value = MachineCode;
                cmd.Parameters.AddWithValue("@ItemId", SqlDbType.VarChar).Value = ItemId;
               



                DataTable dt = new DataTable();

                int rows = cmd.ExecuteNonQuery();
                return rows;




            }
            catch (Exception exp)
            {
                throw exp;
            }
            finally
            {
                if (connectionIndicator)
                {
                    con.Close();
                    connectionIndicator = false;
                }
            }
            
        }

        public static int SaveMachineItem(string Machine_Code, int Item_Id)
        {
            try
            {

                if (connectionIndicator == false)
                {
                    con.Open();
                    connectionIndicator = true;
                }
                SqlCommand cmd = new SqlCommand("Usp_AddMachineItem", con);
                cmd.CommandType = CommandType.StoredProcedure;



                cmd.Parameters.AddWithValue("@MachineCode", SqlDbType.VarChar).Value = Machine_Code;
                cmd.Parameters.AddWithValue("@ItemId", SqlDbType.VarChar).Value = Item_Id;
                cmd.Parameters.AddWithValue("@UserId", SqlDbType.VarChar).Value = UserId;





                DataTable dt = new DataTable();

                int rows = cmd.ExecuteNonQuery();
                return rows;




            }
            catch (Exception exp)
            {
                throw exp;
            }
            finally
            {
                if (connectionIndicator)
                {
                    con.Close();
                    connectionIndicator = false;
                }
            }
        }

        public  static int GetDailyEntryValue(int itemId, string Date)
        {
            try
            {


                if (connectionIndicator == false)
                {
                    con.Open();
                    connectionIndicator = true;
                }
                SqlCommand cmd = new SqlCommand("Usp_GetDailyEntryActualValue", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@ItemId", SqlDbType.Int).Value = itemId;
                cmd.Parameters.AddWithValue("@Date", SqlDbType.VarChar).Value = Date;

                DataTable dt = new DataTable();

                dt.Load(cmd.ExecuteReader());

                if (dt.Rows.Count > 0)
                {

                    return Convert.ToInt32(dt.Rows[0][0]);
                }
                else
                    return 0;

            }
            catch (Exception exp)
            {
                throw exp;
            }
            finally
            {
                if (connectionIndicator)
                {
                    con.Close();
                    connectionIndicator = false;
                }
            }
        }

        public static int DeleteMaterial(int Material_Id, int Item_Id)
        {

            try
            {

                if (connectionIndicator == false)
                {
                    con.Open();
                    connectionIndicator = true;
                }
                SqlCommand cmd = new SqlCommand("Usp_RemoveMaterial", con);
                cmd.CommandType = CommandType.StoredProcedure;



                cmd.Parameters.AddWithValue("@MaterialId", SqlDbType.Int).Value = Material_Id;
                cmd.Parameters.AddWithValue("@ItemId", SqlDbType.Int).Value = Item_Id;




                DataTable dt = new DataTable();

                int rows = cmd.ExecuteNonQuery();
                return rows;




            }
            catch (Exception exp)
            {
                throw exp;
            }
            finally
            {
                if (connectionIndicator)
                {
                    con.Close();
                    connectionIndicator = false;
                }
            }
         
        }


        public static int DeleteDieAndMold(string DieCode)
        {

            try
            {

                if (connectionIndicator == false)
                {
                    con.Open();
                    connectionIndicator = true;
                }
                SqlCommand cmd = new SqlCommand("Usp_DeleteDie", con);
                cmd.CommandType = CommandType.StoredProcedure;



                cmd.Parameters.AddWithValue("@DieCode", SqlDbType.VarChar).Value = DieCode;
                



                DataTable dt = new DataTable();

                int rows = cmd.ExecuteNonQuery();
                return rows;




            }
            catch (Exception exp)
            {
                throw exp;
            }
            finally
            {
                if (connectionIndicator)
                {
                    con.Close();
                    connectionIndicator = false;
                }
            }

        }

        internal static int RemoveEmployee(string empId)
        {
            try
            {

                if (connectionIndicator == false)
                {
                    con.Open();
                    connectionIndicator = true;
                }
                SqlCommand cmd = new SqlCommand("Usp_RemoveEmplyoee", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@EmpId", SqlDbType.VarChar).Value = empId;




                DataTable dt = new DataTable();

                int rows = cmd.ExecuteNonQuery();
                return rows;




            }
            catch (Exception exp)
            {
                throw exp;
            }
            finally
            {
                if (connectionIndicator)
                {
                    con.Close();
                    connectionIndicator = false;
                }
            }
        }

        public static int SaveUserDetails(string Name, string password, int Users_Type_Id, string ContactNumber, string Email, string Address)
        {
            try
            {


                if (connectionIndicator == false)
                {
                    con.Open();
                    connectionIndicator = true;
                }

                SqlCommand cmd = new SqlCommand("Usp_SaveUserDetails", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Name", SqlDbType.VarChar).Value = Name;
                cmd.Parameters.AddWithValue("@password", SqlDbType.VarChar).Value = password;
                cmd.Parameters.AddWithValue("@Users_Type_Id", SqlDbType.Int).Value = Users_Type_Id;
                cmd.Parameters.AddWithValue("@ContactNumber", SqlDbType.VarChar).Value = ContactNumber;
                cmd.Parameters.AddWithValue("@Email", SqlDbType.VarChar).Value = Email;
                cmd.Parameters.AddWithValue("@Address", SqlDbType.VarChar).Value = Address;
              

                int row = cmd.ExecuteNonQuery();
                return row;


            }
            catch (Exception exp)
            {
                throw exp;
            }
            finally
            {
                if (connectionIndicator)
                {
                    con.Close();
                    connectionIndicator = false;
                }
            }

        }

        public static int EditUserDetails(string Name, string password, int Users_Type_Id, string ContactNumber, string Email, string Address,int usersId)
        {
            try
            {


                if (connectionIndicator == false)
                {
                    con.Open();
                    connectionIndicator = true;
                }

                SqlCommand cmd = new SqlCommand("Usp_EditUserDetails", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Name", SqlDbType.VarChar).Value = Name;
                cmd.Parameters.AddWithValue("@password", SqlDbType.VarChar).Value = password;
                cmd.Parameters.AddWithValue("@Users_Type_Id", SqlDbType.Int).Value = Users_Type_Id;
                cmd.Parameters.AddWithValue("@ContactNumber", SqlDbType.VarChar).Value = ContactNumber;
                cmd.Parameters.AddWithValue("@Email", SqlDbType.VarChar).Value = Email;
                cmd.Parameters.AddWithValue("@Address", SqlDbType.VarChar).Value = Address;
                cmd.Parameters.AddWithValue("@UserId", SqlDbType.Int).Value = usersId;

                int row = cmd.ExecuteNonQuery();
                return row;


            }
            catch (Exception exp)
            {
                throw exp;
            }
            finally
            {
                if (connectionIndicator)
                {
                    con.Close();
                    connectionIndicator = false;
                }
            }

        }

        public static DataTable GetDailyEntryProdEfficiency(int Process_Id, int Item_Id, string Date)
        {
            try
            {


                if (connectionIndicator == false)
                {
                    con.Open();
                    connectionIndicator = true;
                }
                SqlCommand cmd = new SqlCommand("Usp_GetDailyEntryPocessItem", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@ItemId", SqlDbType.Int).Value = Item_Id;
                cmd.Parameters.AddWithValue("@ProcessId", SqlDbType.Int).Value = Process_Id;
                cmd.Parameters.AddWithValue("@ProcessId", SqlDbType.Int).Value = Process_Id;

                DataTable dt = new DataTable();

                dt.Load(cmd.ExecuteReader());



                return dt;


            }
            catch (Exception exp)
            {
                throw exp;
            }
            finally
            {
                if (connectionIndicator)
                {
                    con.Close();
                    connectionIndicator = false;
                }
            }
        }


        public static DataTable GetPriceofItem(int Item_Id)
        {
            try
            {


                if (connectionIndicator == false)
                {
                    con.Open();
                    connectionIndicator = true;
                }
                SqlCommand cmd = new SqlCommand("Usp_GetItemPrice", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@ItemId", SqlDbType.Int).Value = Item_Id;
               

                DataTable dt = new DataTable();

                dt.Load(cmd.ExecuteReader());



                return dt;


            }
            catch (Exception exp)
            {
                throw exp;
            }
            finally
            {
                if (connectionIndicator)
                {
                    con.Close();
                    connectionIndicator = false;
                }
            }
        }

        public static DataTable GetPriceofReworkItem(int Item_Id)
        {
            try
            {


                if (connectionIndicator == false)
                {
                    con.Open();
                    connectionIndicator = true;
                }
                SqlCommand cmd = new SqlCommand("Usp_GetPriceofReworkItem", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@ItemId", SqlDbType.Int).Value = Item_Id;


                DataTable dt = new DataTable();

                dt.Load(cmd.ExecuteReader());



                return dt;


            }
            catch (Exception exp)
            {
                throw exp;
            }
            finally
            {
                if (connectionIndicator)
                {
                    con.Close();
                    connectionIndicator = false;
                }
            }
        }

        public static DataTable GetPriceofInhouseRejectionOfItem(int Item_Id)
        {
            try
            {


                if (connectionIndicator == false)
                {
                    con.Open();
                    connectionIndicator = true;
                }
                SqlCommand cmd = new SqlCommand("Usp_GetPriceofInhouseRejectionOfItem", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@ItemId", SqlDbType.Int).Value = Item_Id;


                DataTable dt = new DataTable();

                dt.Load(cmd.ExecuteReader());



                return dt;


            }
            catch (Exception exp)
            {
                throw exp;
            }
            finally
            {
                if (connectionIndicator)
                {
                    con.Close();
                    connectionIndicator = false;
                }
            }
        }



        internal static string WorkersCount()
        {
            try
            {


                if (connectionIndicator == false)
                {
                    con.Open();
                    connectionIndicator = true;
                }
                SqlCommand cmd = new SqlCommand("usp_GetWorkerCount", con);
                cmd.CommandType = CommandType.StoredProcedure;

              


                DataTable dt = new DataTable();

                dt.Load(cmd.ExecuteReader());



                return dt.Rows[0][0].ToString();


            }
            catch (Exception exp)
            {
                throw exp;
            }
            finally
            {
                if (connectionIndicator)
                {
                    con.Close();
                    connectionIndicator = false;
                }
            }
        }

        internal static DataTable GetAllWorkers()
        {
            try
            {


                if (connectionIndicator == false)
                {
                    con.Open();
                    connectionIndicator = true;
                }
                SqlCommand cmd = new SqlCommand("usp_GetAllWorkerDetails", con);
                cmd.CommandType = CommandType.StoredProcedure;




                DataTable dt = new DataTable();

                dt.Load(cmd.ExecuteReader());



                return dt;


            }
            catch (Exception exp)
            {
                throw exp;
            }
            finally
            {
                if (connectionIndicator)
                {
                    con.Close();
                    connectionIndicator = false;
                }
            }
        }

        internal static int DeleteWorker(string workerId)
        {
            try
            {


                if (connectionIndicator == false)
                {
                    con.Open();
                    connectionIndicator = true;
                }
                SqlCommand cmd = new SqlCommand("Usp_DeleteWorker", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@EmpId", SqlDbType.VarChar).Value = workerId;


                int row = cmd.ExecuteNonQuery();
                return row;

            }
            catch (Exception exp)
            {
                throw exp;
            }
            finally
            {
                if (connectionIndicator)
                {
                    con.Close();
                    connectionIndicator = false;
                }
            }

        }
    }
}
