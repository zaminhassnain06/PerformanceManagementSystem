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

namespace PMS
{
    public partial class DailyEntry_ReworkRatio : Form
    {
        private int Item_Id=0;
        private string KPI = "ReworkRatio";
        private int FixedEntryValueCheck = 0; // Controls change of value in Editabletextbox
        public DailyEntry_ReworkRatio(int itemId)
        {
            InitializeComponent();
            Item_Id = itemId;
            if (Item_Id > 0)
            {
               string ItemName = ButtonsUtility.ItemNameUtility(itemId);
                labelItemNameReworkRatio.Text = ItemName;
              //  comboBoxItemsReworkRatio.Visible = false;
               // labelSelectItemReworkRatio.Visible = false;
                TextBoxesCalculatedValues(itemId);
                TextBoxesCalculatedValuesofItemSelectedbyCombo(labelItemNameReworkRatio.Text, DateTime.Today.ToString("yyyy-MM-dd"), KPI);
                InitializeDropDownItemValues();
            }
            else if(Item_Id==0)
            {
                labelItemNameReworkRatio.Text = "Rework Ratio";
                InitializeDropDownItemValues();
                textBoxUnitsReworked.ReadOnly = true;
                
            }
        }

        private void InitializeDropDownItemValues()
        {

            DataTable dt = new DataTable();
            dt = ButtonsUtility.InitializeItemDropDown();
            for(int i=0;i<dt.Rows.Count;i++)
            {
                comboBoxItemsReworkRatio.Items.Add(dt.Rows[i][0]);

            }
        }
        private void TextBoxesCalculatedValues(int itemId)
        {
            DataTable dt=new DataTable();
            dt = ButtonsUtility.TotalUnitsMadeDetails(itemId);
            if(dt.Rows.Count==0)
            {

            }
            else
            {
                textBoxTotalUnitsReworkRatio.Text = dt.Rows[0][1].ToString();
                textBoxCostofEachUnitReworkRatio.Text = dt.Rows[0][2].ToString();
             //   textBoxTotalCostReworkRatio.Text = dt.Rows[0][3].ToString();
            }

        }

        private void buttonMenuDailyEntry_Click(object sender, EventArgs e)
        {
            ButtonsUtility.Menu(Item_Id);
            this.Close();
        }

        private void buttonLogoutDailyEntry_Click(object sender, EventArgs e)
        {
            ButtonsUtility.NavigateToLogin();
            this.Close();
        }

        private void buttonExitDailyEntry_Click(object sender, EventArgs e)
        {
            ButtonsUtility.ExitProgram();
        }

        private void buttonBackDailyEntry_Click(object sender, EventArgs e)
        {
            ReworkRatioKPI ReworkRatioKpi = new ReworkRatioKPI(Item_Id);
            ReworkRatioKpi.Show();
            this.Close();

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBoxNameOfItem_TextChanged(object sender, EventArgs e)
        {
            //if (textBoxTotalUnitsReworkRatio.Text != "")
            //{
            //    int TotalNoofItem = Convert.ToInt32(textBoxTotalUnitsReworkRatio.Text);
            //    int CostofUnit = Convert.ToInt32(textBoxCostofEachUnitReworkRatio.Text);
            //    int TotalCost = TotalNoofItem * CostofUnit;
            //   // textBoxTotalCostReworkRatio.Text = TotalCost.ToString();
            //    labelDailyEntryMissing.Visible = false;
            //    buttonDailyEntryReworkRatio.Visible = false;
            //    labelRecordupdatedSucessfully.Visible = false;
            //}
        }

        private void textBoxTotalUnits_KeyPress(object sender, KeyPressEventArgs e)
        {
            if ((!char.IsNumber(e.KeyChar)) && (!char.IsControl(e.KeyChar)))
            {

                e.Handled = true;
            }
        }

        private void buttonDailyEntrySave_Click(object sender, EventArgs e)
        {
            if (textBoxUnitsReworked.Text == "")
            {
                labelEnterunitsrejectedfirstReworkRatio.Visible = true;
            } 
            else if(Item_Id>0)
            { 

                int noofRecordsAffected= ButtonsUtility.SaveDailyEntryofKPI(Item_Id, Convert.ToInt32(textBoxUnitsReworked.Text), Convert.ToDecimal(textBoxCostofReworking.Text), dateTimePickerReworkRatio.Text,KPI);

                if (noofRecordsAffected > 0)
                {
                    labelRecordupdatedSucessfully.Visible = true;
                }
                else
                {
                    labelAlreadySaved.Visible = true;
                }
            }
             else if (Item_Id == 0 && comboBoxItemsReworkRatio.Text == "")
             {

                 labelPleaseSelectItemReworkRatio.Visible = true;


             }
        }

        private void textBoxUnitsReworked_KeyPress(object sender, KeyPressEventArgs e)
        {
            if ((!char.IsNumber(e.KeyChar)) && (!char.IsControl(e.KeyChar)))
            {

                e.Handled = true;
            }
            if (e.KeyChar == 13)
            {
                buttonDailyEntrySave.PerformClick();
            }
        }

        private void textBoxUnitsReworked_TextChanged(object sender, EventArgs e)
        {
            labelEnterunitsrejectedfirstReworkRatio.Visible = false;
            if (textBoxCostofEachUnitReworkRatio.Text != "" && FixedEntryValueCheck == 0)// Controls  FixedEntryValueCheck change of value in Editabletextbox
            {
                if (textBoxUnitsReworked.Text != "")
                {
                    decimal CostofEachUnit = Convert.ToDecimal(textBoxCostofEachUnitReworkRatio.Text);
                    int UnitsReworked = Convert.ToInt32(textBoxUnitsReworked.Text);
                    decimal TotalReworkCost = CostofEachUnit * UnitsReworked;
                    textBoxCostofReworking.Text = TotalReworkCost.ToString();
                }
                else if (textBoxUnitsReworked.Text == "")
                {
                    textBoxCostofReworking.Text = "";
                }
            }
            FixedEntryValueCheck = 0;// Controls  FixedEntryValueCheck change of value in Editabletextbox
            labelRecordupdatedSucessfully.Visible = false;
        }

        private void comboBoxItemsReworkRatio_SelectedIndexChanged(object sender, EventArgs e)
        {
            TextBoxesCalculatedValuesofItemSelectedbyCombo(comboBoxItemsReworkRatio.Text,dateTimePickerReworkRatio.Text,KPI);
            labelPleaseSelectItemReworkRatio.Visible = false;
            labelRecordupdatedSucessfully.Visible = false;
        }

        private void labelSelectItemReworkRatio_Click(object sender, EventArgs e)
        {

        }

        private void TextBoxesCalculatedValuesofItemSelectedbyCombo(string itemName,string DateTime,string KPIs)
        {
            DataTable dt = new DataTable();
            dt = ButtonsUtility.TotalUnitsMadeDetailsByName(itemName, DateTime,KPIs);
            if (Convert.ToInt32(dt.Rows[0][0]) > 0)
            {
                labelDailyEntryMissing.Visible = false;
                buttonDailyEntryReworkRatio.Visible = false;
                labelRecordupdatedSucessfully.Visible = false;
                textBoxTotalUnitsReworkRatio.Text = dt.Rows[0][1].ToString();
              
              
                Item_Id = Convert.ToInt32(dt.Rows[0][2]);
                GetReworkCost();
                if (!string.IsNullOrEmpty(dt.Rows[0][3].ToString()))
                {
                    textBoxUnitsReworked.Text =dt.Rows[0][3].ToString();
                    textBoxCostofReworking.Text = dt.Rows[0][4].ToString();
                }
               
                
            }
            else
            {
                FixedEntryValueCheck = 1;// Controls change of value in Editabletextbox
                labelDailyEntryMissing.Visible = true;
                buttonDailyEntryReworkRatio.Visible = true;
                textBoxTotalUnitsReworkRatio.Text = "";
                textBoxCostofEachUnitReworkRatio.Text = "";
            
                textBoxUnitsReworked.Text = "";
                textBoxCostofReworking.Text = "";

                Item_Id = Convert.ToInt32(dt.Rows[0][1]);
                GetReworkCost();

            }
            FixedEntryValueCheck = 0;// Controls change of value in Editabletextbox
            if(Item_Id>0)
            {
                textBoxUnitsReworked.ReadOnly = false;
            }
        }

        private void GetReworkCost()
        {
            DataTable dt = ButtonsUtility.GetPriceofReworkItem(Item_Id);
            textBoxCostofEachUnitReworkRatio.Text = dt.Rows[0][0].ToString();
        }
        private void dateTimePickerReworkRatio_ValueChanged(object sender, EventArgs e)
        {
            if (comboBoxItemsReworkRatio.Text != "" && comboBoxItemsReworkRatio.Visible == true)
            {
                TextBoxesCalculatedValuesofItemSelectedbyCombo(comboBoxItemsReworkRatio.Text, dateTimePickerReworkRatio.Text,KPI);
            }
            else if (comboBoxItemsReworkRatio.Visible == false)
            {
                TextBoxesCalculatedValuesofItemSelectedbyCombo(labelItemNameReworkRatio.Text, dateTimePickerReworkRatio.Text,KPI);

            }

        }

        
        private void buttonEditReworkRatio_Click(object sender, EventArgs e)
        {
            //if (Item_Id > 0 && textBoxTotalCostReworkRatio.Text == "")
            //{
            //    labelEditErrorMessageforDateReworkRatio.Visible = true;
            //}
            //else 
                if (Item_Id == 0 && comboBoxItemsReworkRatio.Text == "")
            {
                labelPleaseSelectItemReworkRatio.Visible = true;
            }
            else if (textBoxUnitsReworked.Text == "")
            {
                labelRecordupdatedSucessfully.Text = "Record is not already saved. Please save the record first.";

                labelRecordupdatedSucessfully.Visible = true;
            }
            else
            {
                int noofRecordsAffected=ButtonsUtility.EditDailyEntryofReworkRatio(Item_Id, Convert.ToInt32(textBoxUnitsReworked.Text), Convert.ToDecimal(textBoxCostofReworking.Text), dateTimePickerReworkRatio.Text,KPI);
                if (noofRecordsAffected>0)
                {
                    labelRecordupdatedSucessfully.Visible = true;
                    labelAlreadySaved.Visible = false;
                }
                else
                {
                    labelRecordupdatedSucessfully.Text = "Record is not already saved. Please save the record first.";

                    labelRecordupdatedSucessfully.Visible = true;
                }
            }
        }

        private void buttonDailyEntryReworkRatio_Click(object sender, EventArgs e)
        {
            DailyEntry daily_Entry = new DailyEntry(Item_Id);
            daily_Entry.Show();
            this.Close();
        }

        private void textBoxCostofReworking_TextChanged(object sender, EventArgs e)
        {

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

        private void markAttendanceToolStripMenuItem_Click(object sender, EventArgs e)
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
