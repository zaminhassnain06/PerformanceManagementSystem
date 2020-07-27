using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.Sql;
using System.Data.SqlClient;

namespace PMS
{
    public partial class DashBoardAdmin : Form
    {
        private int Item_Id = 0;
        private int Month_Id = 0;
        public DashBoardAdmin(string User_Type, string UserName,int itemId)
        {
            InitializeComponent();
            
            labelUserName.Text = UserName;
            labelUserName.Select();
         
            if (User_Type == "2")
            {
               // buttonCreateNewWS.Visible = false;
              //  buttonUsersDetails.Visible = false;


            }
            else if (User_Type == "3")
            {
                //buttonCreateNewWS.Visible = false;
                //// buttonFacProfitMargin.Visible=false;
             
            }
            Item_Id = itemId;
            
            comboBoxItems.Visible = true;
            labelPleaseSelectItem.Visible = true;
            InitializeDropDownItemValues();

            GetMonths();

        }
        private void GetMonths()
        {
            DataTable dt = new DataTable();
            dt = ButtonsUtility.GetMonths();
         
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                comboBoxMonths.Items.Add(dt.Rows[i][0]);

            }
            int mn=DateTime.Now.Month;
         
           comboBoxMonths.SelectedIndex = mn-1;
           Month_Id = mn ;
           GetAttendanceRatio();
        }
        private void InitializeDropDownItemValues()
        {

            DataTable dt = new DataTable();
            dt = ButtonsUtility.InitializeItemDropDown();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                comboBoxItems.Items.Add(dt.Rows[i][0]);
                if(Convert.ToInt32(dt.Rows[i][2])==Item_Id && Item_Id>0)
                {

                    comboBoxItems.SelectedIndex = i;
                }
                else if (Item_Id==0)
                {
                    comboBoxItems.SelectedIndex = 0;

                }
            }
            
        }

        
        private void buttonExit_Click(object sender, EventArgs e)
        {
            ButtonsUtility.ExitProgram();
        }

        private void buttonLogout_Click(object sender, EventArgs e)
        {
            this.Close();
            ButtonsUtility.NavigateToLogin();
        }

        

        


        private void buttonAttendanceRatio_Click(object sender, EventArgs e)
        {
            AttendanceRatio Attendacne_Ratio = new AttendanceRatio();
            Attendacne_Ratio.Show();
            this.Close();
        }

       

     
        private void GetAttendanceRatio()
        {
            DataTable dt = new DataTable();
            dt = ButtonsUtility.GetMonthAttendacneRatioOverAllKPIResult(Month_Id);
            labelAttendanceRatio.Visible = true;
            if (dt.Rows.Count > 0)
            {
                
                if (dt.Rows[0][1].ToString() == "Achieved")
                {

                    labelAttendanceRatio.Text = dt.Rows[0][0].ToString() + " %";
                    labelAttendanceRatio.ForeColor = Color.Green;
                    if (Convert.ToDouble(dt.Rows[0][0]) != 0.00)
                    {
                        chartAttendanceRatio.Visible = true;
                        GetGridCalculationsAttendanceRatio();
                    }
                    else
                    {
                        chartAttendanceRatio.Visible = false;
                    }
                }
                else if (dt.Rows[0][1].ToString() == "Not Achieved")
                {

                    labelAttendanceRatio.Text = dt.Rows[0][0].ToString() + " %";
                    labelAttendanceRatio.ForeColor = Color.Red;
                    if (Convert.ToDouble(dt.Rows[0][0]) != 0.00)
                    {
                        chartAttendanceRatio.Visible = true;
                        GetGridCalculationsAttendanceRatio();
                    }
                    else
                    {
                        chartAttendanceRatio.Visible = false;
                    }
                }
                else 
                {
                    labelAttendanceRatio.Text = "No Record.";
                    labelAttendanceRatio.ForeColor = Color.Red;
                    chartAttendanceRatio.Visible = false;
                }
            }
            else
                chartAttendanceRatio.Visible = false;
        }

        private void GetEquipFailRatio()
        {
            DataTable dt = new DataTable();
            dt = ButtonsUtility.GetMontEquipFailRateAllKPIResult(Month_Id);
         
            if (dt.Rows.Count > 0)
            {
             
                if (dt.Rows[0][1].ToString() == "Achieved")
                {

                    labelEuipFailRate.Text = dt.Rows[0][0].ToString() + " %";
                    labelEuipFailRate.ForeColor = Color.Green;
                    if (dt.Rows[0][0].ToString()!="0.000")
                    { 
                    chartEqipFailRate.Visible = true;
                    GetGridCalculationsEquipFailRate();
                    }
                    else
                    {
                        chartEqipFailRate.Visible = false;
                    }
                }
                else if (dt.Rows[0][1].ToString() == "Not Achieved")
                {

                    labelEuipFailRate.Text = dt.Rows[0][0].ToString() + " %";
                    labelEuipFailRate.ForeColor = Color.Red;
                    if (dt.Rows[0][0].ToString() != "0.000")
                    {
                        chartEqipFailRate.Visible = true;
                        GetGridCalculationsEquipFailRate();
                    }
                    else
                    {
                        chartEqipFailRate.Visible = false;
                    }
                }
                else if (dt.Rows[0][1].ToString() == "NoRecord Entered")
                {
                    labelEuipFailRate.Text = "No Record.";
                    labelEuipFailRate.ForeColor = Color.Red;
                    chartEqipFailRate.Visible = false;
                }
            }
            else
                chartEqipFailRate.Visible = false;
        }


        void GetKPIValues()
        {
            DataTable dt = new DataTable();
            DataTable dtProdAchivOEE = new DataTable();
            dt = ButtonsUtility.GetIdAndMonthOverAllKPIResult(Month_Id, Item_Id);
            dtProdAchivOEE = ButtonsUtility.GetIdAndMonthOverAllKPIProdAchivAndOEEResult(Month_Id, Item_Id);
            if (dt.Rows.Count > 0)
            {
                //Rework Ratio 
                if (dt.Rows[0][1].ToString() == "Achieved")
                {

                    labelReworkRatipPerc.Text = dt.Rows[0][0].ToString() + " %";
                    labelReworkRatipPerc.ForeColor = Color.Green;
                    if (dt.Rows[0][0].ToString() != "0.00")
                    {
                        chartReworkRatio.Visible = true;
                        GetGridCalculationsReworkRatio();
                    }
                    else
                    {
                        chartReworkRatio.Visible = false;
                    }
                }
                else if (dt.Rows[0][1].ToString() == "Not Achieved")
                {

                    labelReworkRatipPerc.Text = dt.Rows[0][0].ToString() + " %";
                    labelReworkRatipPerc.ForeColor = Color.Red;
                    if (dt.Rows[0][0].ToString() != "0.00")
                    {
                        chartReworkRatio.Visible = true;
                        GetGridCalculationsReworkRatio();
                    }
                    else
                    {
                        chartReworkRatio.Visible = false;
                    }
                }
                else if (dt.Rows[0][1].ToString() == "NoRecord Entered")
                {
                    labelReworkRatipPerc.Text = "No Record.";
                    labelReworkRatipPerc.ForeColor = Color.Red;
                    chartReworkRatio.Visible = false;
                }
                //Cost of Rework //5
                if (dt.Rows[0][5].ToString() == "Achieved")
                {

                    labelCostofReworkPerc.Text =  dt.Rows[0][4].ToString()  ;
                    labelCostofReworkPerc.ForeColor = Color.Green;
                    if (dt.Rows[0][4].ToString() != "0")
                    {
                        chartCostRework.Visible = true;
                        GetGridCalculationsReworkCost();
                    }
                    else
                    {
                        chartCostRework.Visible = false;
                    }
                }

                else if (dt.Rows[0][5].ToString() == "Not Achieved")
                {

                    labelCostofReworkPerc.Text = "Rs " + dt.Rows[0][4].ToString() ;
                    labelCostofReworkPerc.ForeColor = Color.Red;
                    if (dt.Rows[0][4].ToString() != "0")
                    {
                        chartCostRework.Visible = true;
                        GetGridCalculationsReworkCost();
                    }
                    else
                    {
                        chartCostRework.Visible = false;
                    }
                }
                else if (dt.Rows[0][5].ToString() == "NoRecord Entered")
                {
                    labelCostofReworkPerc.Text = "No Record.";
                    labelCostofReworkPerc.ForeColor = Color.Red;
                    chartCostRework.Visible = false;
                }


                ///Cust Rej Ratio 3
                if (dt.Rows[0][3].ToString() == "Achieved")
                {

                    labelCustRejRatioPerc.Text = dt.Rows[0][2].ToString() + " %";
                    labelCustRejRatioPerc.ForeColor = Color.Green;
                    if (dt.Rows[0][2].ToString() != "0.00")
                    {
                        chartCustRejRatio.Visible = true;
                        GetGridCalculationsCustRejRatio();
                    }
                    else
                    {
                        chartCustRejRatio.Visible = false;
                    }
                }
                else if (dt.Rows[0][3].ToString() == "Not Achieved")
                {

                    labelCustRejRatioPerc.Text = dt.Rows[0][2].ToString() + " %";
                    labelCustRejRatioPerc.ForeColor = Color.Red;
                    if (dt.Rows[0][2].ToString() != "0.00")
                    {
                        chartCustRejRatio.Visible = true;
                        GetGridCalculationsCustRejRatio();
                    }
                    else
                    {
                        chartCustRejRatio.Visible = false;
                    }
                }
                else if (dt.Rows[0][3].ToString() == "NoRecord Entered")
                {
                    labelCustRejRatioPerc.Text = "No Record.";
                    labelCustRejRatioPerc.ForeColor = Color.Red;
                    chartCustRejRatio.Visible = false ;
                }
                // Cust Rej Cost 7

                if (dt.Rows[0][7].ToString() == "Achieved")
                {

                    labelCostofCustRejPerc.Text = "Rs " + dt.Rows[0][6].ToString();
                    labelCostofCustRejPerc.ForeColor = Color.Green;
                    if (Convert.ToInt32(dt.Rows[0][6]) != 0)
                    {
                        chartCustRejCost.Visible = true;
                        GetGridCalculationsCustRejCost();
                    }
                    else
                    {
                        chartCustRejCost.Visible = false;
                    }
                }
                else if (dt.Rows[0][7].ToString() == "Not Achieved")
                {

                    labelCostofCustRejPerc.Text =  "Rs "+dt.Rows[0][6].ToString() ;
                    labelCostofCustRejPerc.ForeColor = Color.Red;
                    if (Convert.ToInt32(dt.Rows[0][6]) != 0)
                    {
                        chartCustRejCost.Visible = true;
                        GetGridCalculationsCustRejCost();
                    }
                    else
                    {
                        chartCustRejCost.Visible = false;
                    }
                }
                else if (dt.Rows[0][7].ToString() == "NoRecord Entered")
                {
                    labelCostofCustRejPerc.Text = "No Record.";
                    labelCostofCustRejPerc.ForeColor = Color.Red;
                    chartCustRejCost.Visible = false;
                }

                // Inhouse Rej Ratio 9

                if (dt.Rows[0][9].ToString() == "Achieved")
                {

                    labelInhouseRejRatio.Text = dt.Rows[0][8].ToString() + " %";
                    labelInhouseRejRatio.ForeColor = Color.Green;
                    if (dt.Rows[0][8].ToString() != "0.00")
                    {
                        chartInhouseRejRatio.Visible = true;
                        GetGridCalculationsInhouseRejRatio();
                    }
                    else
                    {
                        chartInhouseRejRatio.Visible = false;
                    }
                }
                else if (dt.Rows[0][9].ToString() == "Not Achieved")
                {

                    labelInhouseRejRatio.Text = dt.Rows[0][8].ToString() + " %";
                    labelInhouseRejRatio.ForeColor = Color.Red;
                    if (dt.Rows[0][8].ToString() != "0.00")
                    {
                        chartInhouseRejRatio.Visible = true;
                        GetGridCalculationsInhouseRejRatio();
                    }
                    else
                    {
                        chartInhouseRejRatio.Visible = false;
                    }

                }
                else if (dt.Rows[0][9].ToString() == "NoRecord Entered")
                {
                    labelInhouseRejRatio.Text = "No Record.";
                    labelInhouseRejRatio.ForeColor = Color.Red;
                    chartInhouseRejRatio.Visible = false;
                }


                // Inhouse Rej Cost 11

                if (dt.Rows[0][11].ToString() == "Achieved")
                {

                    labelCostOfInhouseRej.Text ="Rs "+ dt.Rows[0][10].ToString() ;
                    labelCostOfInhouseRej.ForeColor = Color.Green;
                    if (Convert.ToInt32(dt.Rows[0][10]) != 0)
                    {
                        chartInhouseRejCost.Visible = true;
                        GetGridCalculationsInhouseRejCost();
                    }
                    else
                    {
                        chartInhouseRejCost.Visible = false;
                    }
                }
                else if (dt.Rows[0][11].ToString() == "Not Achieved")
                {

                    labelCostOfInhouseRej.Text = "Rs " + dt.Rows[0][10].ToString() ;
                    labelCostOfInhouseRej.ForeColor = Color.Red;
                    if (Convert.ToInt32(dt.Rows[0][10]) != 0)
                    {
                        chartInhouseRejCost.Visible = true;
                        GetGridCalculationsInhouseRejCost();
                    }
                    else
                    {
                        chartInhouseRejCost.Visible = false;
                    }
                }
                else if (dt.Rows[0][11].ToString() == "NoRecord Entered")
                {
                    labelCostOfInhouseRej.Text = "No Record.";
                    labelCostOfInhouseRej.ForeColor = Color.Red;
                    chartInhouseRejCost.Visible = false;
                }

                // Customer Claims  13

                if (dt.Rows[0][13].ToString() == "Achieved")
                {

                    labelCustClaimPerc.Text = dt.Rows[0][12].ToString() ;
                    labelCustClaimPerc.ForeColor = Color.Green;
                    if (Convert.ToInt32(dt.Rows[0][12]) != 0)
                    {
                        chartCustomerClaims.Visible = true;
                        GetGridCalculationsCustomerClaims();
                    }
                    else
                    {
                        chartCustomerClaims.Visible = false;
                    }
                }
                else if (dt.Rows[0][13].ToString() == "Not Achieved")
                {

                    labelCustClaimPerc.Text = dt.Rows[0][12].ToString() ;
                    labelCustClaimPerc.ForeColor = Color.Red;
                    if (Convert.ToInt32(dt.Rows[0][10]) != 0)
                    {
                        chartCustomerClaims.Visible = true;
                        GetGridCalculationsCustomerClaims();
                    }
                    else
                    {
                        chartCustomerClaims.Visible = false;
                    }
                }
                else if (dt.Rows[0][13].ToString() == "NoRecord Entered")
                {
                    labelCustClaimPerc.Text = "No Record.";
                    labelCustClaimPerc.ForeColor = Color.Red;
                    chartCustomerClaims.Visible = false;
                }

                // Del Comp  15

                if (dt.Rows[0][15].ToString() == "Achieved")
                {

                    labelDelCompPerc.Text = dt.Rows[0][14].ToString() + " %";
                    labelDelCompPerc.ForeColor = Color.Green;
                    if (dt.Rows[0][14].ToString() != "0.00")
                    {
                        chartDelComp.Visible = true;
                        GetGridCalculationsDeliveryComp();
                    }
                    else
                    {
                        chartDelComp.Visible = false;
                    }
                }
                else if (dt.Rows[0][15].ToString() == "Not Achieved")
                {

                    labelDelCompPerc.Text = dt.Rows[0][14].ToString() + " %";
                    labelDelCompPerc.ForeColor = Color.Red;
                    if (dt.Rows[0][14].ToString() != "0.00")
                    {
                        chartDelComp.Visible = true;
                        GetGridCalculationsDeliveryComp();
                    }
                    else
                    {
                        chartDelComp.Visible = false;
                    }
                }
                else if (dt.Rows[0][15].ToString() == "NoRecord Entered")
                {
                    labelDelCompPerc.Text = "No Record.";
                    labelDelCompPerc.ForeColor = Color.Red;
                    chartDelComp.Visible = false;
                }

                // Yield Var  17

                if (dt.Rows[0][17].ToString() == "Achieved")
                {

                    labelMatYieldPerc.Text = "Rs " + dt.Rows[0][16].ToString() ;
                    labelMatYieldPerc.ForeColor = Color.Green;
                    if (Convert.ToDouble(dt.Rows[0][16]) != 0.00)
                    {
                        chartYield.Visible = true;
                        GetGridCalculationsMatYieldVar();
                    }
                    else
                    {
                        chartYield.Visible = false;
                    }
                    
                }
                else if (dt.Rows[0][17].ToString() == "Not Achieved")
                {

                    labelMatYieldPerc.Text = "Rs " + dt.Rows[0][16].ToString() ;
                    labelMatYieldPerc.ForeColor = Color.Red;
                    if (Convert.ToDouble(dt.Rows[0][16]) != 0.00)
                    {
                        chartYield.Visible = true;
                        GetGridCalculationsMatYieldVar();
                    }
                    else
                    {
                        chartYield.Visible = false;
                    }
                  
                }
                else if (dt.Rows[0][17].ToString() == "NoRecord Entered")
                {
                    labelMatYieldPerc.Text = "No Record.";
                    labelMatYieldPerc.ForeColor = Color.Red;
                    chartYield.Visible = false;
                   
                }

                // ProdEffi 19

                if (dt.Rows[0][19].ToString() == "Achieved")
                {

                    labelProfEffi.Text = dt.Rows[0][18].ToString() + " %";
                    labelProfEffi.ForeColor = Color.Green;
                    if (Convert.ToDouble(dt.Rows[0][18]) != 0.00)
                    {
                        chartProductionEffi.Visible = true;
                        GetGridCalculationsProdEffi();
                    }
                    else
                    {
                        chartProductionEffi.Visible = false;
                    }
                }
                else if (dt.Rows[0][19].ToString() == "Not Achieved")
                {

                    labelProfEffi.Text = dt.Rows[0][18].ToString() + " %";
                    labelProfEffi.ForeColor = Color.Red;
                    if (Convert.ToDouble(dt.Rows[0][18]) != 0.00)
                    {

                        chartProductionEffi.Visible = true;
                        GetGridCalculationsProdEffi();
                    }
                    else
                    {
                        chartProductionEffi.Visible = false;
                    }
                }
                else if (dt.Rows[0][19].ToString() == "NoRecord Entered")
                {
                    labelProfEffi.Text = "No Record.";
                    labelProfEffi.ForeColor = Color.Red;
                    chartProductionEffi.Visible = false;
                }


                // ProdAchiv 21

                if (dtProdAchivOEE.Rows[0][1].ToString() == "Achieved")
                {

                    labelProdAchivRatePerc.Text = dtProdAchivOEE.Rows[0][0].ToString() + " %";
                    labelProdAchivRatePerc.ForeColor = Color.Green;
                    if (Convert.ToDouble(dtProdAchivOEE.Rows[0][0]) != 0.00)
                    {

                        chartProdAchivRate.Visible = true;
                        GetGridCalculationsProdAchiv();
                    }
                    else
                    {
                        chartProdAchivRate.Visible = false;
                    }
                }
                else if (dtProdAchivOEE.Rows[0][1].ToString() == "Not Achieved")
                {

                    labelProdAchivRatePerc.Text = dtProdAchivOEE.Rows[0][0].ToString() + " %";
                    labelProdAchivRatePerc.ForeColor = Color.Red;
                    if (Convert.ToDouble(dtProdAchivOEE.Rows[0][0]) != 0.00)
                    {
                        chartProdAchivRate.Visible = true;
                        GetGridCalculationsProdAchiv();
                    }
                    else
                    {
                        chartProdAchivRate.Visible = false;
                    }
                }
                else if (dtProdAchivOEE.Rows[0][1].ToString() == "NoRecord Entered")
                {
                    labelProdAchivRatePerc.Text = "No Record.";
                    labelProdAchivRatePerc.ForeColor = Color.Red;
                    chartProdAchivRate.Visible = false;
                }



                // OEE 23

                if (dtProdAchivOEE.Rows[0][3].ToString() == "Achieved")
                {

                    labelOverAllEquipPerc.Text = dtProdAchivOEE.Rows[0][2].ToString() + " %";
                    labelOverAllEquipPerc.ForeColor = Color.Green;
                    if (Convert.ToDouble(dtProdAchivOEE.Rows[0][2]) != 0.00)
                    {
                        chartOEE.Visible = true;
                        GetGridCalculationsOEE();
                    }
                    else
                    {
                        chartOEE.Visible = false;
                    }
                   
                }
                else if (dtProdAchivOEE.Rows[0][3].ToString() == "Not Achieved")
                {

                    labelOverAllEquipPerc.Text = dtProdAchivOEE.Rows[0][2].ToString() + " %";
                    labelOverAllEquipPerc.ForeColor = Color.Red;
                    if (Convert.ToDouble(dtProdAchivOEE.Rows[0][2]) != 0.00)
                    {
                        chartOEE.Visible = true;
                        GetGridCalculationsOEE();
                    }
                    else
                    {
                        chartOEE.Visible = false;
                    }
                   
                }
                else if (dtProdAchivOEE.Rows[0][3].ToString() == "NoRecord Entered")
                {
                    labelOverAllEquipPerc.Text = "No Record.";
                    labelOverAllEquipPerc.ForeColor = Color.Red;
                    chartOEE.Visible = false;
                   
                }
            }
        }
       

        private void buttonReworkRatioDashBoard_Click(object sender, EventArgs e)
        {
            ReworkRatioKPI RRKPI = new ReworkRatioKPI(0); //rework ratio without item number direct click from main screen of dash board
            RRKPI.Show();
            this.Close();
        }

        private void buttonCustRejRatioDashBoard_Click(object sender, EventArgs e)
        {
            CustomerRejectionKPI CustRej = new CustomerRejectionKPI(0);
            CustRej.Show();
            this.Close();
        }

        private void buttonInhouseRejDashBoard_Click(object sender, EventArgs e)
        {
            InhouseRejectionKPI InhouseRej = new InhouseRejectionKPI(0);
            InhouseRej.Show();
            this.Close();
        }

        private void buttonProducEffiDashBoard_Click(object sender, EventArgs e)
        {
            ProductionEfficiencyKPI ProdEffi = new ProductionEfficiencyKPI(0);
            ProdEffi.Show();
            this.Close();
        }

        private void buttonCostOfReworkDashBoard_Click(object sender, EventArgs e)
        {
            CostofRework Cost_of_Rework = new CostofRework(0);
            Cost_of_Rework.Show();
            this.Close();
        }

        private void buttonCostofRej_Click(object sender, EventArgs e)
        {
            CostOfInhouseRejectionKPI CostofRej = new CostOfInhouseRejectionKPI(0);
            CostofRej.Show();
            this.Close();
        }

        private void buttonProductionAchvRate_Click(object sender, EventArgs e)
        {
            ProductionAchivRateKPI ProductionAchievementRate = new ProductionAchivRateKPI(0);
            ProductionAchievementRate.Show();
            this.Close();
        }

        
        private void buttonEqpFailureRate_Click(object sender, EventArgs e)
        {
            EquipFailureRateKPI Equip_Failure_Rate = new EquipFailureRateKPI();
            Equip_Failure_Rate.Show();
            this.Close();
        }

        private void buttonCustClaims_Click(object sender, EventArgs e)
        {
            CustomerClaimsKPI CustClaims = new CustomerClaimsKPI(0);
            CustClaims.Show();
            this.Close();
        }

     

        private void buttonDelComp_Click(object sender, EventArgs e)
        {
            DelivaryComplianceKPI DelivaryCompliance = new DelivaryComplianceKPI(0);
            DelivaryCompliance.Show();
            this.Close();
        }

        private void buttonOverAllEquip_Click(object sender, EventArgs e)
        {
            OEEKPI OEE = new OEEKPI(0);
            OEE.Show();
            this.Close();
        }

        private void buttonDailyEntry_Click(object sender, EventArgs e)
        {
            DailyEntry Daily_Entry = new DailyEntry(0);
            Daily_Entry.Show();
            this.Close();

        }

        private void buttonFactoryAddEditDashboard_Click(object sender, EventArgs e)
        {
            AddNewFactory AddFactory = new AddNewFactory();
            AddFactory.Show();
            this.Close();
        }

        private void buttonAddWShop_Click(object sender, EventArgs e)
        {
            AddNewWorkShop WorkShop = new AddNewWorkShop(0);// 0 for add new
            WorkShop.Show();
            this.Close();
        }

        private void buttonEditWorkShop_Click(object sender, EventArgs e)
        {
            AddNewWorkShop workshop = new AddNewWorkShop(1); //1 for editing exisiting
            workshop.Show();
            this.Close();
        }

        private void buttonAddMachine_Click(object sender, EventArgs e)
        {
            AddNewMachine NewMachine = new AddNewMachine(0);
            NewMachine.Show();
            this.Close();
        }

        private void buttonEditMachineDetails_Click(object sender, EventArgs e)
        {
            AddNewMachine NewMachine = new AddNewMachine(1);
            NewMachine.Show();
            this.Close();

        }


        private void buttonCustomerRejCost_Click(object sender, EventArgs e)
        {
            CostOfCustomerRejectionKPI CustRejCost = new CostOfCustomerRejectionKPI(0);
            CustRejCost.Show();
            this.Close();
        }

        private void buttonAddDie_Click(object sender, EventArgs e)
        {
            AddNewDieAndMolds DieMold = new AddNewDieAndMolds(0);
            DieMold.Show();
            this.Close();
        }

        private void buttonEditDie_Click(object sender, EventArgs e)
        {
            AddNewDieAndMolds DieMold = new AddNewDieAndMolds(1);
            DieMold.Show();
            this.Close();
        }

        private void buttonUsersDetails_Click(object sender, EventArgs e)
        {
            AddNewUser user = new AddNewUser();
            user.Show();
            this.Close();

        }

        private void buttonAddWorker_Click(object sender, EventArgs e)
        {
            AddNewWorker worker = new AddNewWorker("");
            worker.Show();
            this.Close();
        }

        private void buttonWorkerDetails_Click(object sender, EventArgs e)
        {
            Worker_Details workerdetails = new Worker_Details();
            workerdetails.Show();
            this.Close();
        }

        private void buttonMatYieldVar_Click(object sender, EventArgs e)
        {
            YieldVarianceKPI YieldVar = new YieldVarianceKPI(0);

            YieldVar.Show();
            this.Close();
        }

        private void buttonKPI_Click(object sender, EventArgs e)
        {
            OverAll_KPIs KPI = new OverAll_KPIs(0);
            KPI.Show();
            this.Close();
        }

     

        private void comboBoxItems_SelectedIndexChanged(object sender, EventArgs e)
        {
            Item_Id = ButtonsUtility.GetITemId(comboBoxItems.Text);
            if (comboBoxItems.Text != "")
            {


                labelPleaseSelectItem.Visible = false;
            }
            if (comboBoxMonths.Text == "")
            {
                labelPleaseSelectMonth.Visible = true;
            }



            if (Item_Id > 0 && Month_Id > 0)
            {



                GetKPIValues();
                GetEquipFailRatio();



            }
            dateTimePickerFrom.Enabled = true;
            dateTimePickerTo.Enabled = true;
            buttonExport.Enabled = true;
        }

        private void GetGridCalculationsReworkRatio()
        {
            DataTable dt = new DataTable();
         
            dt = ButtonsUtility.GetCalculationsReworkRatioMonthDate(Item_Id, Month_Id, dateTimePickerDashboard.Value.Year);
            int rows = dt.Rows.Count;
           
            try
            {



                if (chartReworkRatio.Series.IndexOf("Ach%") != -1)
                {
                    chartReworkRatio.Series.Clear();
                }
             
                chartReworkRatio.Series.Add("Ach%");
              

                chartReworkRatio.Series["Ach%"].Color = Color.CornflowerBlue;
              
                for (int i = 0; i < rows; i++)
                {
                                       


                        chartReworkRatio.Series["Ach%"].Points.AddXY(dt.Rows[i][0], dt.Rows[i][3]);
                         chartReworkRatio.Series["Ach%"]["PixelPointWidth"] = "15";
                        chartReworkRatio.Series["Ach%"].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Area ;
                    
                  

                }
                 chartReworkRatio.ChartAreas[0].InnerPlotPosition.Height = (float)77.91;
                 chartReworkRatio.ChartAreas[0].InnerPlotPosition.Width = (float)77.92;


                chartReworkRatio.ChartAreas[0].AxisX.MajorGrid.LineWidth = 0;
                chartReworkRatio.ChartAreas[0].AxisY.MajorGrid.LineWidth = 0;

                chartReworkRatio.Series[0].IsVisibleInLegend = false;

            }
            catch (Exception exp)
            {
                throw exp;
            }
        }

        private void GetGridCalculationsReworkCost()
        {
            DataTable dt = new DataTable();
            dt = ButtonsUtility.GetCalculationsReworkCostMonthDate(Item_Id, Month_Id,dateTimePickerDashboard.Value.Year);


            int rows = dt.Rows.Count;
            try
            {



                if (chartCostRework.Series.IndexOf("Cost") != -1)
                {
                    chartCostRework.Series.Clear();
                }

                chartCostRework.Series.Add("Cost");


                chartCostRework.Series["Cost"].Color = Color.CornflowerBlue;
               ;
                for (int i = 0; i < rows; i++)
                {




                    chartCostRework.Series["Cost"].Points.AddXY(dt.Rows[i][0], dt.Rows[i][1]);
                    chartCostRework.Series["Cost"]["PixelPointWidth"] = "15";
                  
                    chartCostRework.Series["Cost"].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Area;

                    


                }
                chartCostRework.ChartAreas[0].InnerPlotPosition.Height = (float)77.91;
                chartCostRework.ChartAreas[0].InnerPlotPosition.Width = (float)77.92;


                chartCostRework.ChartAreas[0].AxisX.MajorGrid.LineWidth = 0;
                chartCostRework.ChartAreas[0].AxisY.MajorGrid.LineWidth = 0;

                chartCostRework.Series[0].IsVisibleInLegend = false;

            }
            catch (Exception exp)
            {
                throw exp;
            }
          
        }


        private void GetGridCalculationsCustRejRatio()
        {
            DataTable dt = new DataTable();
            dt = ButtonsUtility.GetCalculationsCustRejRatioMonthDate(Item_Id, Month_Id, dateTimePickerDashboard.Value.Year);


            int rows = dt.Rows.Count;
            try
            {



                if (chartCustRejRatio.Series.IndexOf("Ratio") != -1)
                {
                    chartCustRejRatio.Series.Clear();
                }

                chartCustRejRatio.Series.Add("Ratio");


                chartCustRejRatio.Series["Ratio"].Color = Color.CornflowerBlue;
                ;
                for (int i = 0; i < rows; i++)
                {




                    chartCustRejRatio.Series["Ratio"].Points.AddXY(dt.Rows[i][0], dt.Rows[i][3]);
                    chartCustRejRatio.Series["Ratio"]["PixelPointWidth"] = "15";

                    chartCustRejRatio.Series["Ratio"].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Area;




                }
                chartCustRejRatio.ChartAreas[0].InnerPlotPosition.Height = (float)77.91;
                chartCustRejRatio.ChartAreas[0].InnerPlotPosition.Width = (float)77.92;


                chartCustRejRatio.ChartAreas[0].AxisX.MajorGrid.LineWidth = 0;
                chartCustRejRatio.ChartAreas[0].AxisY.MajorGrid.LineWidth = 0;

                chartCustRejRatio.Series[0].IsVisibleInLegend = false;

            }
            catch (Exception exp)
            {
                throw exp;
            }

        }


            private void GetGridCalculationsCustRejCost()
        {
            DataTable dt = new DataTable();
            dt = ButtonsUtility.GetCalculationsCustRejCostMonthDate(Item_Id, Month_Id, dateTimePickerDashboard.Value.Year);


            int rows = dt.Rows.Count;
            try
            {



                if (chartCustRejCost.Series.IndexOf("Cost") != -1)
                {
                    chartCustRejCost.Series.Clear();
                }

                chartCustRejCost.Series.Add("Cost");


                chartCustRejCost.Series["Cost"].Color = Color.CornflowerBlue;
                ;
                for (int i = 0; i < rows; i++)
                {




                    chartCustRejCost.Series["Cost"].Points.AddXY(dt.Rows[i][0], dt.Rows[i][1]);
                    chartCustRejCost.Series["Cost"]["PixelPointWidth"] = "15";

                    chartCustRejCost.Series["Cost"].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Area;




                }
                chartCustRejCost.ChartAreas[0].InnerPlotPosition.Height = (float)77.91;
                chartCustRejCost.ChartAreas[0].InnerPlotPosition.Width = (float)77.92;


                chartCustRejCost.ChartAreas[0].AxisX.MajorGrid.LineWidth = 0;
                chartCustRejCost.ChartAreas[0].AxisY.MajorGrid.LineWidth = 0;

                chartCustRejCost.Series[0].IsVisibleInLegend = false;

            }
            catch (Exception exp)
            {
                throw exp;
            }

        }

            private void GetGridCalculationsInhouseRejRatio()
            {
                DataTable dt = new DataTable();
                dt = ButtonsUtility.GetCalculationsInhouseRejRatioMonthDate(Item_Id, Month_Id, dateTimePickerDashboard.Value.Year);


                int rows = dt.Rows.Count;
                try
                {



                    if (chartInhouseRejRatio.Series.IndexOf("Ratio") != -1)
                    {
                        chartInhouseRejRatio.Series.Clear();
                    }

                    chartInhouseRejRatio.Series.Add("Ratio");


                    chartInhouseRejRatio.Series["Ratio"].Color = Color.CornflowerBlue;
                    ;
                    for (int i = 0; i < rows; i++)
                    {




                        chartInhouseRejRatio.Series["Ratio"].Points.AddXY(dt.Rows[i][0], dt.Rows[i][3]);
                        chartInhouseRejRatio.Series["Ratio"]["PixelPointWidth"] = "15";

                        chartInhouseRejRatio.Series["Ratio"].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Area;




                    }
                    chartInhouseRejRatio.ChartAreas[0].InnerPlotPosition.Height = (float)77.91;
                    chartInhouseRejRatio.ChartAreas[0].InnerPlotPosition.Width = (float)77.92;


                    chartInhouseRejRatio.ChartAreas[0].AxisX.MajorGrid.LineWidth = 0;
                    chartInhouseRejRatio.ChartAreas[0].AxisY.MajorGrid.LineWidth = 0;

                    chartInhouseRejRatio.Series[0].IsVisibleInLegend = false;

                }
                catch (Exception exp)
                {
                    throw exp;
                }

            }


            private void GetGridCalculationsInhouseRejCost()
            {
                DataTable dt = new DataTable();
                dt = ButtonsUtility.GetCalculationsInhouseRejCostMonthDate(Item_Id, Month_Id, dateTimePickerDashboard.Value.Year);


                int rows = dt.Rows.Count;
                try
                {



                    if (chartInhouseRejCost.Series.IndexOf("Cost") != -1)
                    {
                        chartInhouseRejCost.Series.Clear();
                    }

                    chartInhouseRejCost.Series.Add("Cost");


                    chartInhouseRejCost.Series["Cost"].Color = Color.CornflowerBlue;
                    ;
                    for (int i = 0; i < rows; i++)
                    {




                        chartInhouseRejCost.Series["Cost"].Points.AddXY(dt.Rows[i][0], dt.Rows[i][1]);
                        chartInhouseRejCost.Series["Cost"]["PixelPointWidth"] = "15";

                        chartInhouseRejCost.Series["Cost"].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Area;




                    }
                    chartInhouseRejCost.ChartAreas[0].InnerPlotPosition.Height = (float)77.91;
                    chartInhouseRejCost.ChartAreas[0].InnerPlotPosition.Width = (float)77.92;


                    chartInhouseRejCost.ChartAreas[0].AxisX.MajorGrid.LineWidth = 0;
                    chartInhouseRejCost.ChartAreas[0].AxisY.MajorGrid.LineWidth = 0;

                    chartInhouseRejCost.Series[0].IsVisibleInLegend = false;

                }
                catch (Exception exp)
                {
                    throw exp;
                }

            }

            private void GetGridCalculationsCustomerClaims()
            {
                DataTable dt = new DataTable();
                dt = ButtonsUtility.GetCalculationsCustomerClaimsMonthDate(Item_Id, Month_Id, dateTimePickerDashboard.Value.Year);


                int rows = dt.Rows.Count;
                try
                {



                    if (chartCustomerClaims.Series.IndexOf("Claims") != -1)
                    {
                        chartCustomerClaims.Series.Clear();
                    }

                    chartCustomerClaims.Series.Add("Claims");


                    chartCustomerClaims.Series["Claims"].Color = Color.CornflowerBlue;
                    ;
                    for (int i = 0; i < rows; i++)
                    {




                        chartCustomerClaims.Series["Claims"].Points.AddXY(dt.Rows[i][0], dt.Rows[i][1]);
                        chartCustomerClaims.Series["Claims"]["PixelPointWidth"] = "15";

                        chartCustomerClaims.Series["Claims"].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Area;




                    }
                    chartCustomerClaims.ChartAreas[0].InnerPlotPosition.Height = (float)77.91;
                    chartCustomerClaims.ChartAreas[0].InnerPlotPosition.Width = (float)77.92;


                    chartCustomerClaims.ChartAreas[0].AxisX.MajorGrid.LineWidth = 0;
                    chartCustomerClaims.ChartAreas[0].AxisY.MajorGrid.LineWidth = 0;

                    chartCustomerClaims.Series[0].IsVisibleInLegend = false;

                }
                catch (Exception exp)
                {
                    throw exp;
                }

            }


            private void GetGridCalculationsDeliveryComp()
            {
                DataTable dt = new DataTable();
                dt = ButtonsUtility.GetCalculationsDelClaimsMonthDate(Item_Id, Month_Id, dateTimePickerDashboard.Value.Year);


                int rows = dt.Rows.Count;
                try
                {



                    if (chartDelComp.Series.IndexOf("Ratio") != -1)
                    {
                        chartDelComp.Series.Clear();
                    }

                    chartDelComp.Series.Add("Ratio");


                    chartDelComp.Series["Ratio"].Color = Color.CornflowerBlue;
                    ;
                    for (int i = 0; i < rows; i++)
                    {




                        chartDelComp.Series["Ratio"].Points.AddXY(dt.Rows[i][0], dt.Rows[i][1]);
                        chartDelComp.Series["Ratio"]["PixelPointWidth"] = "15";

                        chartDelComp.Series["Ratio"].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Area;




                    }
                    chartDelComp.ChartAreas[0].InnerPlotPosition.Height = (float)77.91;
                    chartDelComp.ChartAreas[0].InnerPlotPosition.Width = (float)77.92;


                    chartDelComp.ChartAreas[0].AxisX.MajorGrid.LineWidth = 0;
                    chartDelComp.ChartAreas[0].AxisY.MajorGrid.LineWidth = 0;

                    chartDelComp.Series[0].IsVisibleInLegend = false;

                }
                catch (Exception exp)
                {
                    throw exp;
                }

            }


            private void GetGridCalculationsProdAchiv()
            {
                DataTable dt = new DataTable();
                dt = ButtonsUtility.GetCalculationsProdAchivMonthDate(Item_Id, Month_Id, dateTimePickerDashboard.Value.Year);


                int rows = dt.Rows.Count;
                try
                {



                    if (chartProdAchivRate.Series.IndexOf("Ratio") != -1)
                    {
                        chartProdAchivRate.Series.Clear();
                    }

                    chartProdAchivRate.Series.Add("Ratio");


                    chartProdAchivRate.Series["Ratio"].Color = Color.CornflowerBlue;
                    ;
                    for (int i = 0; i < rows; i++)
                    {




                        chartProdAchivRate.Series["Ratio"].Points.AddXY(dt.Rows[i][0], dt.Rows[i][1]);
                        chartProdAchivRate.Series["Ratio"]["PixelPointWidth"] = "15";

                        chartProdAchivRate.Series["Ratio"].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Area;




                    }
                    chartProdAchivRate.ChartAreas[0].InnerPlotPosition.Height = (float)77.91;
                    chartProdAchivRate.ChartAreas[0].InnerPlotPosition.Width = (float)77.92;


                    chartProdAchivRate.ChartAreas[0].AxisX.MajorGrid.LineWidth = 0;
                    chartProdAchivRate.ChartAreas[0].AxisY.MajorGrid.LineWidth = 0;

                    chartProdAchivRate.Series[0].IsVisibleInLegend = false;

                }
                catch (Exception exp)
                {
                    throw exp;
                }

            }

            private void GetGridCalculationsProdEffi()
            {
                DataTable dt = new DataTable();
                dt = ButtonsUtility.GetCalculationsProdEffiMonthDate(Item_Id, Month_Id, dateTimePickerDashboard.Value.Year);


                int rows = dt.Rows.Count;
                try
                {



                    if (chartProductionEffi.Series.IndexOf("Ratio") != -1)
                    {
                        chartProductionEffi.Series.Clear();
                    }

                    chartProductionEffi.Series.Add("Ratio");


                    chartProductionEffi.Series["Ratio"].Color = Color.CornflowerBlue;
                    ;
                    for (int i = 0; i < rows; i++)
                    {




                        chartProductionEffi.Series["Ratio"].Points.AddXY(dt.Rows[i][0], dt.Rows[i][1]);
                        chartProductionEffi.Series["Ratio"]["PixelPointWidth"] = "15";

                        chartProductionEffi.Series["Ratio"].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Area;




                    }
                    chartProductionEffi.ChartAreas[0].InnerPlotPosition.Height = (float)77.91;
                    chartProductionEffi.ChartAreas[0].InnerPlotPosition.Width = (float)77.92;


                    chartProductionEffi.ChartAreas[0].AxisX.MajorGrid.LineWidth = 0;
                    chartProductionEffi.ChartAreas[0].AxisY.MajorGrid.LineWidth = 0;

                    chartProductionEffi.Series[0].IsVisibleInLegend = false; 

                }
                catch (Exception exp)
                {
                    throw exp;
                }

            }

            private void GetGridCalculationsEquipFailRate()
            {
                DataTable dt = new DataTable();
                dt = ButtonsUtility.GetCalculationsEquipFailRateMonthDate(Item_Id, Month_Id,dateTimePickerDashboard.Value.Year);


                int rows = dt.Rows.Count;
                try
                {



                    if (chartEqipFailRate.Series.IndexOf("Ratio") != -1)
                    {
                        chartEqipFailRate.Series.Clear();
                    }

                    chartEqipFailRate.Series.Add("Ratio");


                    chartEqipFailRate.Series["Ratio"].Color = Color.CornflowerBlue;
                    ;
                    for (int i = 0; i < rows; i++)
                    {




                        chartEqipFailRate.Series["Ratio"].Points.AddXY(dt.Rows[i][0], dt.Rows[i][1]);
                        chartEqipFailRate.Series["Ratio"]["PixelPointWidth"] = "15";

                        chartEqipFailRate.Series["Ratio"].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Area;




                    }
                    chartEqipFailRate.ChartAreas[0].InnerPlotPosition.Height = (float)77.91;
                    chartEqipFailRate.ChartAreas[0].InnerPlotPosition.Width = (float)77.92;


                    chartEqipFailRate.ChartAreas[0].AxisX.MajorGrid.LineWidth = 0;
                    chartEqipFailRate.ChartAreas[0].AxisY.MajorGrid.LineWidth = 0;

                    chartEqipFailRate.Series[0].IsVisibleInLegend = false;

                }
                catch (Exception exp)
                {
                    throw exp;
                }

            }


            private void GetGridCalculationsOEE()
            {
                DataTable dt = new DataTable();
                dt = ButtonsUtility.GetCalculationsOEEDateDashBoard(Item_Id, Month_Id,dateTimePickerDashboard.Value.Year);


                int rows = dt.Rows.Count;
                try
                {



                    if (chartOEE.Series.IndexOf("Ratio") != -1)
                    {
                        chartOEE.Series.Clear();
                    }

                    chartOEE.Series.Add("Ratio");


                    chartOEE.Series["Ratio"].Color = Color.CornflowerBlue;
                    ;
                    for (int i = 0; i < rows; i++)
                    {




                        chartOEE.Series["Ratio"].Points.AddXY(dt.Rows[i][0], dt.Rows[i][1]);
                        chartOEE.Series["Ratio"]["PixelPointWidth"] = "15";

                        chartOEE.Series["Ratio"].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Area;




                    }
                    chartOEE.ChartAreas[0].InnerPlotPosition.Height = (float)77.91;
                    chartOEE.ChartAreas[0].InnerPlotPosition.Width = (float)77.92;


                    chartOEE.ChartAreas[0].AxisX.MajorGrid.LineWidth = 0;
                    chartOEE.ChartAreas[0].AxisY.MajorGrid.LineWidth = 0;

                    chartOEE.Series[0].IsVisibleInLegend = false;

                }
                catch (Exception exp)
                {
                    throw exp;
                }

            }

            private void GetGridCalculationsAttendanceRatio()
            {
                DataTable dt = new DataTable();
                dt = ButtonsUtility.GetCalculationsAttendanceDateDashBoard( Month_Id, dateTimePickerDashboard.Value.Year);


                int rows = dt.Rows.Count;
                try
                {



                    if (chartAttendanceRatio.Series.IndexOf("Ratio") != -1)
                    {
                        chartAttendanceRatio.Series.Clear();
                    }

                    chartAttendanceRatio.Series.Add("Ratio");


                    chartAttendanceRatio.Series["Ratio"].Color = Color.CornflowerBlue;
                    ;
                    for (int i = 0; i < rows; i++)
                    {




                        chartAttendanceRatio.Series["Ratio"].Points.AddXY(dt.Rows[i][0], dt.Rows[i][1]);
                        chartAttendanceRatio.Series["Ratio"]["PixelPointWidth"] = "15";

                        chartAttendanceRatio.Series["Ratio"].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Area;




                    }
                    chartAttendanceRatio.ChartAreas[0].InnerPlotPosition.Height = (float)77.91;
                    chartAttendanceRatio.ChartAreas[0].InnerPlotPosition.Width = (float)77.92;


                    chartAttendanceRatio.ChartAreas[0].AxisX.MajorGrid.LineWidth = 0;
                    chartAttendanceRatio.ChartAreas[0].AxisY.MajorGrid.LineWidth = 0;

                    chartAttendanceRatio.Series[0].IsVisibleInLegend = false;

                }
                catch (Exception exp)
                {
                    throw exp;
                }

            }


            private void GetGridCalculationsMatYieldVar()
            {
                DataTable dt = new DataTable();
                dt = ButtonsUtility.GetCalculationsMatYieldDateDashBoard(Item_Id,Month_Id,dateTimePickerDashboard.Value.Year);


                int rows = dt.Rows.Count;
                try
                {



                    if (chartYield.Series.IndexOf("Cost") != -1)
                    {
                        chartYield.Series.Clear();
                    }

                    chartYield.Series.Add("Cost");


                    chartYield.Series["Cost"].Color = Color.CornflowerBlue;
                    ;
                    for (int i = 0; i < rows; i++)
                    {




                        chartYield.Series["Cost"].Points.AddXY(dt.Rows[i][0], dt.Rows[i][1]);
                        chartYield.Series["Cost"]["PixelPointWidth"] = "15";

                        chartYield.Series["Cost"].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Area;




                    }
                    chartYield.ChartAreas[0].InnerPlotPosition.Height = (float)77.91;
                    chartYield.ChartAreas[0].InnerPlotPosition.Width = (float)77.92;


                    chartYield.ChartAreas[0].AxisX.MajorGrid.LineWidth = 0;
                    chartYield.ChartAreas[0].AxisY.MajorGrid.LineWidth = 0;

                    chartYield.Series[0].IsVisibleInLegend = false;

                }
                catch (Exception exp)
                {
                    throw exp;
                }

            }
        private void comboBoxMonths_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBoxMonths.Text != "")
            {
                labelPleaseSelectMonth.Visible = false;
                
            }
            if (comboBoxItems.Text == "")
            {


                labelPleaseSelectItem.Visible = true;
            }

            Month_Id = ButtonsUtility.GetMonthId(comboBoxMonths.Text);
            if(Month_Id>0)
            {
                GetAttendanceRatio();
            }

            if (Item_Id > 0 && Month_Id > 0)
            {



                GetKPIValues();
                GetEquipFailRatio();


            }
        }

       
      

        private void buttonReworkRatioDashBoard_Click_1(object sender, EventArgs e)
        {
            ReworkRatioKPI RRKPI = new ReworkRatioKPI(0); //rework ratio without item number direct click from main screen of dash board
            RRKPI.Show();
            this.Close();
        }

      

        
   
       

        private void buttonEditIItem_Click(object sender, EventArgs e)
        {
            AddNewItem NewItem = new AddNewItem(1);

            NewItem.Show();
            this.Close();
        }

        private void buttonConfig_Click(object sender, EventArgs e)
        {
            Settings Config = new Settings();
            Config.Show();
            this.Close();
        }

     

        private void editWorkshopToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AddNewWorkShop workshop = new AddNewWorkShop(1); //1 for editing exisiting
            workshop.Show();
            this.Close();
        }

        private void editMachineToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AddNewMachine NewMachine = new AddNewMachine(1);
            NewMachine.Show();
            this.Close();
        }

        private void customerRejectionRatioToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CostofRework Cost_of_Rework = new CostofRework(0);
            Cost_of_Rework.Show();
            this.Close();
        }

        private void dailyEntryProductionAchievementToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DailyEntry_ProductionAchievement ProdAchiv = new DailyEntry_ProductionAchievement(0);
            ProdAchiv.Show();
            this.Close();
        }

        private void dailyEntryCustomerClaimsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DailyEntry_CustomerClaims CustClaims = new DailyEntry_CustomerClaims(0);
            CustClaims.Show();
            this.Close();
        }

        private void dailyEntryProductionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DailyEntry Daily_Entry = new DailyEntry(0);
            Daily_Entry.Show();
            this.Close();
        }

        private void createWorkShopToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AddNewWorkShop WorkShop = new AddNewWorkShop(0);// 0 for add new
            WorkShop.Show();
            this.Close();
        }

        private void createMachineToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AddNewMachine NewMachine = new AddNewMachine(0);
            NewMachine.Show();
            this.Close();
        }

        private void createItemToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
            AddNewItem NewItem = new AddNewItem(0);

            NewItem.Show();
        }

        private void createDieMoldToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AddNewDieAndMolds DieMold = new AddNewDieAndMolds(0);
            DieMold.Show();
            this.Close();
        }

        private void createWorkerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AddNewWorker worker = new AddNewWorker("");
            worker.Show();
            this.Close();
        }

        private void createUserTypeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AddNewUser user = new AddNewUser();
            user.Show();
            this.Close();
        }

        private void editItemDetailsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AddNewItem NewItem = new AddNewItem(1);

            NewItem.Show();
            this.Close();
        }

        private void editDieMoldDetailsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AddNewDieAndMolds DieMold = new AddNewDieAndMolds(1);
            DieMold.Show();
            this.Close();
        }

        private void factoryDetailsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AddNewFactory AddFactory = new AddNewFactory();
            AddFactory.Show();
            this.Close();
        }

        private void overAllEntriesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DailyEntryOverAll OverAll = new DailyEntryOverAll(0);
            OverAll.Show();
            this.Close();
        }

        private void tPMMachineToolStripMenuItem_Click(object sender, EventArgs e)
        {
            TPM_Machines TpmMachines = new TPM_Machines();
            TpmMachines.Show();
            this.Close();
        }

        private void tPMDieMoldToolStripMenuItem_Click(object sender, EventArgs e)
        {
            TPM_DieandMolds TpmDieMolds = new TPM_DieandMolds();
            TpmDieMolds.Show();
            this.Close();
        }

        private void reworkRatioToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ReworkRatioKPI RRKPI = new ReworkRatioKPI(0); //rework ratio without item number direct click from main screen of dash board
            RRKPI.Show();
            this.Close();
        }

        private void inhouseRejectionRatioToolStripMenuItem_Click(object sender, EventArgs e)
        {
            InhouseRejectionKPI InhouseRej = new InhouseRejectionKPI(0);
            InhouseRej.Show();
            this.Close();
        }

        private void inhouseRejectionCostToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CostOfInhouseRejectionKPI CostofRej = new CostOfInhouseRejectionKPI(0);
            CostofRej.Show();
            this.Close();
        }

        private void customerRejectionRatioToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            CustomerRejectionKPI CustRej = new CustomerRejectionKPI(0);
            CustRej.Show();
            this.Close();
        }

        private void customerRejectionCostToolStripMenuItem_Click(object sender, EventArgs e)
        {


            CostOfCustomerRejectionKPI CustRejCost = new CostOfCustomerRejectionKPI(0);
            CustRejCost.Show();
            this.Close();
        }

        private void productionEfficiencyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ProductionEfficiencyKPI ProdEffi = new ProductionEfficiencyKPI(0);
            ProdEffi.Show();
            this.Close();
        }

        private void productionAchievementRateToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ProductionAchivRateKPI ProductionAchievementRate = new ProductionAchivRateKPI(0);
            ProductionAchievementRate.Show();
            this.Close();
        }

        private void equipmentFailureRateToolStripMenuItem_Click(object sender, EventArgs e)
        {

            EquipFailureRateKPI Equip_Failure_Rate = new EquipFailureRateKPI();
            Equip_Failure_Rate.Show();
            this.Close();
        }

        private void oEEToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OEEKPI OEE = new OEEKPI(0);
            OEE.Show();
            this.Close();
        }

        private void materialYieldVarianceToolStripMenuItem_Click(object sender, EventArgs e)
        {

            YieldVarianceKPI YieldVar = new YieldVarianceKPI(0);

            YieldVar.Show();
            this.Close();
        }

        private void attendanceRatioToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AttendanceRatio Attendacne_Ratio = new AttendanceRatio();
            Attendacne_Ratio.Show();
            this.Close();
        }

        private void dailyEntryReworkToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DailyEntry_ReworkRatio ReworkRatioEntryDaily = new DailyEntry_ReworkRatio(0);
            ReworkRatioEntryDaily.Show();
            this.Close();
        }

        private void dailyEntryCustomerRejectionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DailyEntry_CustomerRejection DailyEntryCustRej = new DailyEntry_CustomerRejection(0);
            DailyEntryCustRej.Show();
            this.Close();
        }

        private void dailyEntryProductionEfficiencyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DailyEntry_ProductionEfficiency DaiyEntryProdEff = new DailyEntry_ProductionEfficiency(0);
            DaiyEntryProdEff.Show();
            this.Close();
        }

        private void dailyEntryInhouseRejectionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DailyEntry_InhouseRejectionInhouseRejection DailyEntryInhouseRej = new DailyEntry_InhouseRejectionInhouseRejection(0);
            DailyEntryInhouseRej.Show();
            this.Close();
        }

        private void dailyEntryDeliveryComplianceToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DailyEntry_DelivaryCompliance DelComp = new DailyEntry_DelivaryCompliance(0);
            DelComp.Show();
            this.Close();
        }

        private void materialYieldVarianceDailyEntryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DailyEntry_Material DailyEntry = new DailyEntry_Material(0);
            DailyEntry.Show();
            this.Close();
        }

        private void dailyEntryEquipmentFailureRateToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DailyEntry_Machine dailtEntryMachine = new DailyEntry_Machine(0);
            dailtEntryMachine.Show();
            this.Close();
        }

        private void workerAttendanceToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Worker_Details workerdetails = new Worker_Details();
            workerdetails.Show();
            this.Close();
        }

        private void customerRejectionRatioToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            CostofRework CR = new CostofRework(0);
            CR.Show();
            this.Close();
          
        }

        private void DashBoardAdmin_Load(object sender, EventArgs e)
        {

        }

        private void customerClaimsToolStripMenuItem_Click(object sender, EventArgs e)
        {

            CustomerClaimsKPI Cust = new CustomerClaimsKPI(0);
            Cust.Show();
            this.Close();
        }

        private void deliveryComplianceToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DelivaryComplianceKPI Del = new DelivaryComplianceKPI(0);
            Del.Show();
            this.Close();
        }

        private void buttonExport_Click(object sender, EventArgs e)
        {
            string Path = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            DataSet ds = new DataSet();
            if(Item_Id>0)
            {
                labelPleaseSelectItem.Visible = false;
           
                if (dateTimePickerFrom.Value < dateTimePickerTo.Value)
            
                {
             
                    ds = ButtonsUtility.GetOverAllReportDetails(Item_Id, dateTimePickerFrom.Text, dateTimePickerTo.Text);
                
               
                    labelReportisGenerating.Visible = true;
                
                    Reports.ExportDataSetToExcelOverAllReport(ds, Path, comboBoxItems.Text);
                
                    labelReportisGenerating.Visible = false;
            
                }
            
                else
            
                {
                
                    DialogResult dialog = MessageBox.Show("From Date is always less then To Date. ", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            
                }
            }
            else
            {
                labelPleaseSelectItem.Visible = true;
            }
        }

        private void workerAttendanceToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            Worker_Details workerdetails = new Worker_Details();
            workerdetails.Show();
            this.Close();
        }

        private void attendanceDetailsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            GetAttendanceDetails AttendacneDetails = new GetAttendanceDetails();
            AttendacneDetails.Show();
            this.Close();
        }

   
       


        

     

       
        
    }
}
