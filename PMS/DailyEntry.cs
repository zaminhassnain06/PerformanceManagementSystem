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
    public partial class DailyEntry : Form
    {
        private int Item_Id;

        public DailyEntry(int itemId)
        {
            InitializeComponent();
            Item_Id = itemId;
            if (itemId > 0)
            {
                string ItemName = ButtonsUtility.ItemNameUtility(itemId);
                labelItemName.Text = ItemName;
             //   comboBoxItemsDailyEntry.Visible = false;
             //   labelSelectItemDailyEntry.Visible = false;
                InitializeDropDownItemValues();
            }
            else if (Item_Id == 0)
            {
                
                InitializeDropDownItemValues();

            }
        }
        private void InitializeDropDownItemValues()
        {

            DataTable dt = new DataTable();
            dt = ButtonsUtility.InitializeItemDropDown();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                comboBoxItemsDailyEntry.Items.Add(dt.Rows[i][0]);

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

            Settings config = new Settings();
            config.Show();
            this.Close();
            
        }

       

        private void textBoxNameOfItem_TextChanged(object sender, EventArgs e)
        {
            if (textBoxCostofEachUnit.Text != "" && textBoxTotalUnits.Text!="")
            { 
                int TotalNoofItem = Convert.ToInt32(textBoxTotalUnits.Text);
                decimal CostofUnit = Convert.ToDecimal(textBoxCostofEachUnit.Text);
                decimal TotalCost = TotalNoofItem * CostofUnit;
                textBoxTotalCost.Text = TotalCost.ToString();
            }
            labelAlreadySaved.Visible = false;
            labelRecordupdatedSucessfullyDailyEntry.Visible = false;
            labelNotSaved.Visible = false;
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
            
            if(Item_Id>0)
            { 

            
                int noofRecordsAffected = ButtonsUtility.SaveDailyEntry(Item_Id, Convert.ToInt32(textBoxTotalUnits.Text), Convert.ToDecimal(textBoxTotalCost.Text), dateTimePickerDailyEntry.Text);

                if (noofRecordsAffected > 0)
                {
                    labelRecordupdatedSucessfullyDailyEntry.Text = "Record entered sucessfully.";
                    labelRecordupdatedSucessfullyDailyEntry.Visible = true;
                }
                else
                {

                    labelAlreadySaved.Visible = true;
                }
            }
            
            else if(Item_Id==0 && comboBoxItemsDailyEntry.Text=="")
            {

                labelPleaseSelectItem.Visible = true;


            }
            labelNotSaved.Visible = false;
             
        }

        private void dateTimePickerCustomerRejection_ValueChanged(object sender, EventArgs e)
        {
            if (comboBoxItemsDailyEntry.Text != "" && comboBoxItemsDailyEntry.Visible == true)
            {
                TextBoxesCalculatedValuesofItemSelectedbyCombo(comboBoxItemsDailyEntry.Text, dateTimePickerDailyEntry.Text);
            }
            else if (comboBoxItemsDailyEntry.Visible == false)
            {
                TextBoxesCalculatedValuesofItemSelectedbyCombo(labelItemName.Text, dateTimePickerDailyEntry.Text);
                labelEditErrorMessageforDate.Visible = false;

            }
            labelRecordupdatedSucessfullyDailyEntry.Visible = false;
            labelEditErrorMessageforDate.Visible = false;
            labelAlreadySaved.Visible = false;
            labelNotSaved.Visible = false;
        }
        private void TextBoxesCalculatedValuesofItemSelectedbyCombo(string itemName, string DateTime)
        {
            DataTable dt = new DataTable();
            dt = ButtonsUtility.TotalUnitsMadeDetailsByName(itemName, DateTime, "DailyEntry");
            if (Convert.ToInt32(dt.Rows[0][0]) > 0)
            {


                labelRecordupdatedSucessfullyDailyEntry.Visible = false;
                textBoxTotalUnits.Text = dt.Rows[0][1].ToString();
                
                textBoxTotalCost.Text = dt.Rows[0][2].ToString();
                Item_Id = Convert.ToInt32(dt.Rows[0][3]);
                GetCost();
            }
            else
            {
                //FixedEntryValueCheck = 1;// Controls change of value in Editabletextbox


                textBoxTotalUnits.Text = "";
                textBoxCostofEachUnit.Text = "";
                textBoxTotalCost.Text = "";
               

                Item_Id = Convert.ToInt32(dt.Rows[0][1]);
                GetCost();

            }
           // FixedEntryValueCheck = 0;// Controls change of value in Editabletextbox

        }

        private void GetCost()
        {
            DataTable dt = ButtonsUtility.GetPriceofItem(Item_Id);
            textBoxCostofEachUnit.Text = dt.Rows[0][0].ToString();
        }

        private void comboBoxItemsDailyEntry_SelectedIndexChanged(object sender, EventArgs e)
        {
            TextBoxesCalculatedValuesofItemSelectedbyCombo(comboBoxItemsDailyEntry.Text, dateTimePickerDailyEntry.Text);
            labelPleaseSelectItem.Visible = false;
            labelRecordupdatedSucessfullyDailyEntry.Visible = false;
            labelEditErrorMessageforDate.Visible = false;
            labelAlreadySaved.Visible = false;
            labelNotSaved.Visible = false;
           
        }

        private void buttonEditDailyEntry_Click(object sender, EventArgs e)
        {
            if(Item_Id>0 && textBoxTotalCost.Text=="")
            {
                labelEditErrorMessageforDate.Visible = true;
            }
            else if(Item_Id==0 && comboBoxItemsDailyEntry.Text=="")
            {
                labelPleaseSelectItem.Visible = true;
            }
            else{
            
           
                int noofRecordsAffected = ButtonsUtility.EditDailyEntry(Item_Id, Convert.ToInt32(textBoxTotalUnits.Text), Convert.ToDecimal(textBoxTotalCost.Text), dateTimePickerDailyEntry.Text);
           
            
                if (noofRecordsAffected > 0)
            
                {
               
                    labelRecordupdatedSucessfullyDailyEntry.Text = "Record updated sucessfully.";
               
                    labelRecordupdatedSucessfullyDailyEntry.Visible = true;
           
                }
                else
                {


                    labelNotSaved.Visible = true;
                }
            }
            labelAlreadySaved.Visible = false;
        }

        private void textBoxCostofEachUnit_KeyPress(object sender, KeyPressEventArgs e)
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

        

        private void labelDailyEntryDateTime_Click(object sender, EventArgs e)
        {

        }

        private void textBoxTotalUnits_TextChanged(object sender, EventArgs e)
        {
            if (textBoxTotalUnits.Text != "" && textBoxCostofEachUnit.Text != "")
            {
                textBoxTotalCost.Text = (Convert.ToDecimal(textBoxCostofEachUnit.Text) * Convert.ToInt32(textBoxTotalUnits.Text)).ToString();
            }
            labelAlreadySaved.Visible = false;
            labelRecordupdatedSucessfullyDailyEntry.Visible = false;
            labelNotSaved.Visible = false;
          
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
