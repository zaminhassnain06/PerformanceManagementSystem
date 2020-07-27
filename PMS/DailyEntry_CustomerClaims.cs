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
    public partial class DailyEntry_CustomerClaims : Form
    {
        private int Item_Id;
        int FixedEntryValueCheck = 0;
        public DailyEntry_CustomerClaims(int itemId)
        {
            InitializeComponent();
            Item_Id = itemId;
            if (itemId > 0)
            {
                string ItemName = ButtonsUtility.ItemNameUtility(itemId);
                labelItemNameCustomerClaims.Text = ItemName;
                comboBoxItemsDailyEntryDelivaryCompliance.Visible = false;
                labelSelectItemDailyEntryCustomerClaims.Visible = false;
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
                comboBoxItemsDailyEntryDelivaryCompliance.Items.Add(dt.Rows[i][0]);

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
            CustomerClaimsKPI cust = new CustomerClaimsKPI(Item_Id);
            cust.Show();
            this.Close();
        }

       

       

        
        private void buttonDailyEntrySave_Click(object sender, EventArgs e)
        {
            if(textBoxAcutalCustomerClaims.Text=="" || textBoxExpectedCustomerClaims.Text=="")
            {
                labelRequiredFieldsCustomerClaims.Visible = true;
            }
            else if (comboBoxItemsDailyEntryDelivaryCompliance.Text == "")
            {
                labelPleaseSelectItemCustomerClaims.Visible = true;
            }
            else
            {
                if (Item_Id > 0)
                {


                    int noofRecordsAffected = ButtonsUtility.SaveDailyEntryCustomerClaims(Item_Id, Convert.ToInt32(textBoxExpectedCustomerClaims.Text), Convert.ToInt32(textBoxAcutalCustomerClaims.Text), dateTimePickerDailyEntryCustomerClaims.Text);

                    if (noofRecordsAffected > 0)
                    {
                        labelRecordupdatedSucessfullyDailyEntryCustomerClaims.Text = "Record entered sucessfully.";
                        labelRecordupdatedSucessfullyDailyEntryCustomerClaims.Visible = true;
                    }
                    else
                    {
                        labelRecordupdatedSucessfullyDailyEntryCustomerClaims.Text = "Cannot Save already saved record. Please edit it.";
                        labelRecordupdatedSucessfullyDailyEntryCustomerClaims.Visible = true;
                    }
                }

                else if (Item_Id == 0 && comboBoxItemsDailyEntryDelivaryCompliance.Text == "")
                {

                    labelPleaseSelectItemCustomerClaims.Visible = true;


                }
            }
        }

        private void dateTimePickerCustomerRejection_ValueChanged(object sender, EventArgs e)
        {
            if (comboBoxItemsDailyEntryDelivaryCompliance.Text != "" && comboBoxItemsDailyEntryDelivaryCompliance.Visible == true)
            {
                TextBoxesCalculatedValuesofItemSelectedbyCombo(comboBoxItemsDailyEntryDelivaryCompliance.Text, dateTimePickerDailyEntryCustomerClaims.Text);
            }
            else if (comboBoxItemsDailyEntryDelivaryCompliance.Visible == false)
            {
                TextBoxesCalculatedValuesofItemSelectedbyCombo(labelItemNameCustomerClaims.Text, dateTimePickerDailyEntryCustomerClaims.Text);
                labelEditErrorMessageforDateCustomerClaims.Visible = false;

            }
            labelRecordupdatedSucessfullyDailyEntryCustomerClaims.Visible = false;
            labelEditErrorMessageforDateCustomerClaims.Visible = false;
        }
        private void TextBoxesCalculatedValuesofItemSelectedbyCombo(string itemName, string DateTime)
        {
            DataTable dt = new DataTable();
            dt = ButtonsUtility.TotalUnitsMadeDetailsByNameforCustomerClaims(itemName, DateTime);
            if (Convert.ToInt32(dt.Rows[0][0]) > 0)
            {


                labelRecordupdatedSucessfullyDailyEntryCustomerClaims.Visible = false;
                textBoxExpectedCustomerClaims.Text = dt.Rows[0][1].ToString();
                textBoxAcutalCustomerClaims.Text = dt.Rows[0][2].ToString();
               
                Item_Id = Convert.ToInt32(dt.Rows[0][3]);
                
            }
            else
            {
                FixedEntryValueCheck = 1;// Controls change of value in Editabletextbox


                textBoxExpectedCustomerClaims.Text = "";
                textBoxAcutalCustomerClaims.Text = "";
               
               

                Item_Id = Convert.ToInt32(dt.Rows[0][1]);

            }
            FixedEntryValueCheck = 0;// Controls change of value in Editabletextbox

        }

        private void comboBoxItemsDailyEntry_SelectedIndexChanged(object sender, EventArgs e)
        {
            TextBoxesCalculatedValuesofItemSelectedbyCombo(comboBoxItemsDailyEntryDelivaryCompliance.Text, dateTimePickerDailyEntryCustomerClaims.Text);
            labelPleaseSelectItemCustomerClaims.Visible = false;
            labelRecordupdatedSucessfullyDailyEntryCustomerClaims.Visible = false;
            labelEditErrorMessageforDateCustomerClaims.Visible = false;
        }

        private void buttonEditDailyEntry_Click(object sender, EventArgs e)
        {
           if(Item_Id==0 && comboBoxItemsDailyEntryDelivaryCompliance.Text=="")
            {
                labelPleaseSelectItemCustomerClaims.Visible = true;
            }
            else{


                int noofRecordsAffected = ButtonsUtility.EditDailyEntryCustomerClaims(Item_Id, Convert.ToInt32(textBoxExpectedCustomerClaims.Text), Convert.ToInt32(textBoxAcutalCustomerClaims.Text), dateTimePickerDailyEntryCustomerClaims.Text);
           
            
                if (noofRecordsAffected > 0)
            
                {
               
                    labelRecordupdatedSucessfullyDailyEntryCustomerClaims.Text = "Record updated sucessfully.";
               
                    labelRecordupdatedSucessfullyDailyEntryCustomerClaims.Visible = true;
           
                }
                else
                {
                    labelRecordupdatedSucessfullyDailyEntryCustomerClaims.Text = "Record is not already saved. Please save the record first.";

                    labelRecordupdatedSucessfullyDailyEntryCustomerClaims.Visible = true;
                }
            }
        }

    
     

        private void labelDailyEntryDateTime_Click(object sender, EventArgs e)
        {

        }

        private void textBoxDelivaryCompliance_KeyPress(object sender, KeyPressEventArgs e)
        {
            if ((!char.IsNumber(e.KeyChar)) && (!char.IsControl(e.KeyChar)))
            {

                e.Handled = true;
            }
        }

        //private void textBoxDelivaryCompliance_TextChanged(object sender, EventArgs e)
        //{
        //    if (textBoxExpectedCustomerClaims.Text != "" && textBoxAcutalCustomerClaims.Text != "")
        //    {
        //        decimal ComplianceRatio = (Convert.ToDecimal(Convert.ToDecimal(textBoxAcutalCustomerClaims.Text) / Convert.ToDecimal(textBoxExpectedCustomerClaims.Text)) * 100);
        //        decimal RoundedPlanned = (Math.Round(ComplianceRatio, 2));
        //        textBoxDelCompPercnt.Text = (RoundedPlanned.ToString());
        //    }
        //    else
        //        textBoxDelCompPercnt.Text = "";

        //    labelRequiredFields.Visible = false;
        //}

        //private void textBoxAcutalDelivaryCompliance_TextChanged(object sender, EventArgs e)
        //{
        //    if (textBoxExpectedCustomerClaims.Text != "" && textBoxAcutalCustomerClaims.Text != "")
        //    {
        //        decimal ComplianceRatio = (Convert.ToDecimal(Convert.ToDecimal(textBoxAcutalCustomerClaims.Text) / Convert.ToDecimal(textBoxExpectedCustomerClaims.Text)) * 100);
        //        decimal RoundedPlanned = (Math.Round(ComplianceRatio, 2));
        //        textBoxDelCompPercnt.Text = (RoundedPlanned.ToString());
        //    }
        //    else
        //        textBoxDelCompPercnt.Text = "";

        //    labelRequiredFields.Visible = false;
        //}

        private void textBoxAcutalDelivaryCompliance_KeyPress(object sender, KeyPressEventArgs e)
        {
            if ((!char.IsNumber(e.KeyChar)) && (!char.IsControl(e.KeyChar)))
            {

                e.Handled = true;
            }
            if (e.KeyChar == 13)
            {
                buttonDailyEntrySaveCustomerClaims.PerformClick();
            }
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
