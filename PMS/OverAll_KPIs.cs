using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PMS
{
    public partial class OverAll_KPIs : Form
    {
        private int Item_Id = 0;
        private int Month_Id = 0;
        public OverAll_KPIs(int ItemId)
        {
            InitializeComponent();
            if (ItemId > 0)
            {

                labelItemName.Text = ButtonsUtility.ItemNameUtility(ItemId).ToUpper();
                labelItemName.Visible = true;
                Item_Id = ItemId;
                labelSelectItem.Visible = false;
                comboBoxItems.Visible = false;
                labelPleaseSelectItem.Visible = false;
             
            }
            else if (ItemId == 0)
            {
                labelItemName.Visible = false;
                labelSelectItem.Visible = true;
                comboBoxItems.Visible = true;
                labelPleaseSelectItem.Visible = true;
                InitializeDropDownItemValues();
            }
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
        }
        private void InitializeDropDownItemValues()
        {

            DataTable dt = new DataTable();
            dt = ButtonsUtility.InitializeItemDropDown();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                comboBoxItems.Items.Add(dt.Rows[i][0]);

            }
        }

        private void buttonOverAllEquip_Click(object sender, EventArgs e)
        {
            OEEKPI OEE = new OEEKPI(Item_Id);
            OEE.Show();
            this.Close();
        }

        private void buttonDelComp_Click(object sender, EventArgs e)
        {
            DelivaryComplianceKPI DelivaryCompliance = new DelivaryComplianceKPI(Item_Id);
            DelivaryCompliance.Show();
            this.Close();
        }

     
        private void buttonCustClaims_Click(object sender, EventArgs e)
        {
            //needed
        }

        private void buttonEqpFailureRate_Click(object sender, EventArgs e)
        {
            EquipFailureRateKPI Equip_Failure_Rate = new EquipFailureRateKPI();
            Equip_Failure_Rate.Show();
            this.Close();
        }


        private void buttonProjAchvRate_Click(object sender, EventArgs e)
        {
            ProductionAchivRateKPI ProductionAchievementRate = new ProductionAchivRateKPI(Item_Id);
            ProductionAchievementRate.Show();
            this.Close();
        }

        private void buttonCostofRej_Click(object sender, EventArgs e)
        {
            CostOfCustomerRejectionKPI CostofRej = new CostOfCustomerRejectionKPI(Item_Id);
            CostofRej.Show();
            this.Close();
        }

        private void buttonCostOfReworkDashBoard_Click(object sender, EventArgs e)
        {
            CostofRework Cost_of_Rework = new CostofRework(Item_Id);
            Cost_of_Rework.Show();
            this.Close();
        }

        private void buttonProducEffiDashBoard_Click(object sender, EventArgs e)
        {
            //prod needed
        }

        private void buttonInhouseRejDashBoard_Click(object sender, EventArgs e)
        {
            InhouseRejectionKPI InhouseRej = new InhouseRejectionKPI(Item_Id);
            InhouseRej.Show();
            this.Close();
        }

        private void buttonCustRejRatioDashBoard_Click(object sender, EventArgs e)
        {
            CustomerRejectionKPI CustRej = new CustomerRejectionKPI(Item_Id);
            CustRej.Show();
            this.Close();
        }

        private void buttonReworkRatioDashBoard_Click(object sender, EventArgs e) //Rework Ratio
        {
            ReworkRatioKPI RRKPI = new ReworkRatioKPI(Item_Id);
            RRKPI.Show();
            this.Close();
        }

        private void buttonEmployeesRetention_Click(object sender, EventArgs e)
        {
           
        }

        private void buttonAttendanceRatio_Click(object sender, EventArgs e)
        {
            AttendanceRatio Attendacne_Ratio = new AttendanceRatio();
            Attendacne_Ratio.Show();
            this.Close();
        }

        private void buttonTPMMachines_Click(object sender, EventArgs e)
        {
            TPM_Machines TpmMachines = new TPM_Machines();
            TpmMachines.Show();
            this.Close();

        }

        private void buttonTPMMouldsDies_Click(object sender, EventArgs e)
        {
            TPM_DieandMolds TpmDieMolds = new TPM_DieandMolds();
            TpmDieMolds.Show();
            this.Close();
        }

       
        private void buttonWorkersSkill_Click(object sender, EventArgs e)
        {
            //needed
        }

        private void buttonMenuOverAllKPI_Click(object sender, EventArgs e)
        {
            ButtonsUtility.Menu(0);
            this.Close();
        }

        private void buttonBackOverAllKPI_Click(object sender, EventArgs e)
        {
            ButtonsUtility.Menu(0);
            this.Close();
        }

        private void buttonLogoutOverAllKPI_Click(object sender, EventArgs e)
        {
            ButtonsUtility.NavigateToLogin();
            this.Close();
        }

        private void buttonExitOverAllKPI_Click(object sender, EventArgs e)
        {
            ButtonsUtility.ExitProgram();
        }

        private void buttonDailyEntry_Click(object sender, EventArgs e)
        {
            DailyEntry Daily_Entry = new DailyEntry(Item_Id);
            Daily_Entry.Show();
            this.Close();
        }

        private void buttonMatYieldVar_Click(object sender, EventArgs e)
        {

        }

        private void comboBoxItems_SelectedIndexChanged(object sender, EventArgs e)
        {
            Item_Id = ButtonsUtility.GetITemId(comboBoxItems.Text);
            if (comboBoxItems.Text != "")
            {
               

                labelPleaseSelectItem.Visible = false;
            }
            if (comboBoxMonths.Text=="")
            {
                labelPleaseSelectMonth.Visible = true;
            }

            labelItemName.Text = comboBoxItems.Text.ToUpper();
            labelItemName.Visible = true;

            
            if (Item_Id > 0 && Month_Id > 0)
            {



                GetKPIValues();



            }
        }

        void GetKPIValues()
        {
             DataTable dt =new DataTable();
             dt=ButtonsUtility.GetIdAndMonthOverAllKPIResult(Month_Id, Item_Id);
             if (dt.Rows.Count > 0)
             {
                 //Rework Ratio 
                 if (dt.Rows[0][1].ToString() == "Achieved")
                 {

                     labelReworkRatipPerc.Text = dt.Rows[0][0].ToString() + " %";
                     labelReworkRatipPerc.ForeColor = Color.Green;
                 }
                 else if (dt.Rows[0][1].ToString() == "Not Achieved")
                 {

                     labelReworkRatipPerc.Text = dt.Rows[0][0].ToString() + " %";
                     labelReworkRatipPerc.ForeColor = Color.Red;
                 }
                 else if (dt.Rows[0][1].ToString() == "NoRecord Entered")
                 {
                     labelReworkRatipPerc.Text = "No Record.";
                     labelReworkRatipPerc.ForeColor = Color.Red;
                 }
                 //Cost of Rework //5
                 if (dt.Rows[0][5].ToString() == "Achieved")
                 {

                     labelCostofReworkPerc.Text = dt.Rows[0][4].ToString() + " Rs";
                     labelCostofReworkPerc.ForeColor = Color.Green;
                 }
                 else if (dt.Rows[0][5].ToString() == "Not Achieved")
                 {

                     labelCostofReworkPerc.Text = dt.Rows[0][4].ToString() + " Rs";
                     labelCostofReworkPerc.ForeColor = Color.Red;
                 }
                 else if (dt.Rows[0][5].ToString() == "NoRecord Entered")
                 {
                     labelCostofReworkPerc.Text = "No Record.";
                     labelCostofReworkPerc.ForeColor = Color.Red;
                 }


                 ///Cust Rej Ratio 3
                 if (dt.Rows[0][3].ToString() == "Achieved")
                 {

                     labelCustRejRatioPerc.Text = dt.Rows[0][2].ToString() + " %";
                     labelCustRejRatioPerc.ForeColor = Color.Green;
                 }
                 else if (dt.Rows[0][3].ToString() == "Not Achieved")
                 {

                     labelCustRejRatioPerc.Text = dt.Rows[0][2].ToString() + " %";
                     labelCustRejRatioPerc.ForeColor = Color.Red;
                 }
                 else if (dt.Rows[0][3].ToString() == "NoRecord Entered")
                 {
                     labelCustRejRatioPerc.Text = "No Record.";
                     labelCustRejRatioPerc.ForeColor = Color.Red;
                 }
                 // Cust Rej Cost 7

                 if (dt.Rows[0][7].ToString() == "Achieved")
                 {

                     labelCostofCustRejPerc.Text = dt.Rows[0][6].ToString() + " Rs";
                     labelCostofCustRejPerc.ForeColor = Color.Green;
                 }
                 else if (dt.Rows[0][7].ToString() == "Not Achieved")
                 {

                     labelCostofCustRejPerc.Text = dt.Rows[0][6].ToString() + " Rs";
                     labelCostofCustRejPerc.ForeColor = Color.Red;
                 }
                 else if (dt.Rows[0][7].ToString() == "NoRecord Entered")
                 {
                     labelCostofCustRejPerc.Text = "No Record.";
                     labelCostofCustRejPerc.ForeColor = Color.Red;
                 }

                 // Inhouse Rej Ratio 9

                 if (dt.Rows[0][9].ToString() == "Achieved")
                 {

                     labelInhouseRejRatio.Text = dt.Rows[0][8].ToString() + " %";
                     labelInhouseRejRatio.ForeColor = Color.Green;
                 }
                 else if (dt.Rows[0][9].ToString() == "Not Achieved")
                 {

                     labelInhouseRejRatio.Text = dt.Rows[0][8].ToString() + " %";
                     labelInhouseRejRatio.ForeColor = Color.Red;
                 }
                 else if (dt.Rows[0][9].ToString() == "NoRecord Entered")
                 {
                     labelInhouseRejRatio.Text = "No Record.";
                     labelInhouseRejRatio.ForeColor = Color.Red;
                 }


                 // Inhouse Rej Cost 11

                 if (dt.Rows[0][11].ToString() == "Achieved")
                 {

                     labelCostOfInhouseRej.Text = dt.Rows[0][10].ToString() + " Rs";
                     labelCostOfInhouseRej.ForeColor = Color.Green;
                 }
                 else if (dt.Rows[0][11].ToString() == "Not Achieved")
                 {

                     labelCostOfInhouseRej.Text = dt.Rows[0][10].ToString() + " Rs";
                     labelCostOfInhouseRej.ForeColor = Color.Red;
                 }
                 else if (dt.Rows[0][11].ToString() == "NoRecord Entered")
                 {
                     labelCostOfInhouseRej.Text = "No Record.";
                     labelCostOfInhouseRej.ForeColor = Color.Red;
                 }

                 // Customer Claims  13

                 if (dt.Rows[0][13].ToString() == "Achieved")
                 {

                     labelCustClaimPerc.Text = dt.Rows[0][12].ToString() + " %";
                     labelCustClaimPerc.ForeColor = Color.Green;
                 }
                 else if (dt.Rows[0][13].ToString() == "Not Achieved")
                 {

                     labelCustClaimPerc.Text = dt.Rows[0][12].ToString() + " %";
                     labelCustClaimPerc.ForeColor = Color.Red;
                 }
                 else if (dt.Rows[0][13].ToString() == "NoRecord Entered")
                 {
                     labelCustClaimPerc.Text = "No Record.";
                     labelCustClaimPerc.ForeColor = Color.Red;
                 }

                 // Del Comp  15

                 if (dt.Rows[0][15].ToString() == "Achieved")
                 {

                     labelDelCompPerc.Text = dt.Rows[0][14].ToString() + " %";
                     labelDelCompPerc.ForeColor = Color.Green;
                 }
                 else if (dt.Rows[0][15].ToString() == "Not Achieved")
                 {

                     labelDelCompPerc.Text = dt.Rows[0][14].ToString() + " %";
                     labelDelCompPerc.ForeColor = Color.Red;
                 }
                 else if (dt.Rows[0][15].ToString() == "NoRecord Entered")
                 {
                     labelDelCompPerc.Text = "No Record.";
                     labelDelCompPerc.ForeColor = Color.Red;
                 }

                 // Yield Var  17

                 if (dt.Rows[0][17].ToString() == "Achieved")
                 {

                     labelMatYieldPerc.Text = dt.Rows[0][16].ToString() + " Rs";
                     labelMatYieldPerc.ForeColor = Color.Green;
                 }
                 else if (dt.Rows[0][17].ToString() == "Not Achieved")
                 {

                     labelMatYieldPerc.Text = dt.Rows[0][16].ToString() + " Rs";
                     labelMatYieldPerc.ForeColor = Color.Red;
                 }
                 else if (dt.Rows[0][17].ToString() == "NoRecord Entered")
                 {
                     labelMatYieldPerc.Text = "No Record.";
                     labelMatYieldPerc.ForeColor = Color.Red;
                 }

                 // ProdEffi 19

                 if (dt.Rows[0][19].ToString() == "Achieved")
                 {

                     labelProfEffi.Text = dt.Rows[0][18].ToString() + " %";
                     labelProfEffi.ForeColor = Color.Green;
                 }
                 else if (dt.Rows[0][19].ToString() == "Not Achieved")
                 {

                     labelProfEffi.Text = dt.Rows[0][18].ToString() + " %";
                     labelProfEffi.ForeColor = Color.Red;
                 }
                 else if (dt.Rows[0][19].ToString() == "NoRecord Entered")
                 {
                     labelProfEffi.Text = "No Record.";
                     labelProfEffi.ForeColor = Color.Red;
                 }


                 // ProdAchiv 21

                 if (dt.Rows[0][21].ToString() == "Achieved")
                 {

                     labelProdAchivRatePerc.Text = dt.Rows[0][20].ToString() + " %";
                     labelProdAchivRatePerc.ForeColor = Color.Green;
                 }
                 else if (dt.Rows[0][21].ToString() == "Not Achieved")
                 {

                     labelProdAchivRatePerc.Text = dt.Rows[0][20].ToString() + " %";
                     labelProdAchivRatePerc.ForeColor = Color.Red;
                 }
                 else if (dt.Rows[0][21].ToString() == "NoRecord Entered")
                 {
                     labelProdAchivRatePerc.Text = "No Record.";
                     labelProdAchivRatePerc.ForeColor = Color.Red;
                 }



                 // OEE 23

                 if (dt.Rows[0][23].ToString() == "Achieved")
                 {

                     labelOverAllEquipPerc.Text = dt.Rows[0][22].ToString() + " %";
                     labelOverAllEquipPerc.ForeColor = Color.Green;
                 }
                 else if (dt.Rows[0][23].ToString() == "Not Achieved")
                 {

                     labelOverAllEquipPerc.Text = dt.Rows[0][22].ToString() + " %";
                     labelOverAllEquipPerc.ForeColor = Color.Red;
                 }
                 else if (dt.Rows[0][23].ToString() == "NoRecord Entered")
                 {
                     labelOverAllEquipPerc.Text = "No Record.";
                     labelOverAllEquipPerc.ForeColor = Color.Red;
                 }
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


            if (Item_Id > 0 && Month_Id > 0)
            {



                GetKPIValues();



            }
            
        }

        
    }
}
