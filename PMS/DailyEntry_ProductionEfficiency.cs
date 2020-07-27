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
    public partial class DailyEntry_ProductionEfficiency : Form
    {
        private int Item_Id=0;
        int FixedEntryValueCheck = 0;
        int Process_Id;
        decimal Capacity;
        int No_of_workers=0;
        decimal RoundedPlannedManHour;
        decimal ActualManhours;
        public DailyEntry_ProductionEfficiency(int itemId)
        {
            InitializeComponent();
            Item_Id = itemId;
            if (itemId > 0)
            {
                string ItemName = ButtonsUtility.ItemNameUtility(itemId);
                labelDailyProductionEfficiency.Text = ItemName;
             //   comboBoxDailyEntryProductionEfficiency.Visible = false;
                //labelSelectItemDailyEntryProductionEfficiency.Visible = false;
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
                comboBoxDailyEntryProductionEfficiency.Items.Add(dt.Rows[i][0]);

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
            ProductionEfficiencyKPI ProdEffi = new ProductionEfficiencyKPI(Item_Id);
            ProdEffi.Show();
            this.Close();
        }

       

       
       

        private void buttonDailyEntrySave_Click(object sender, EventArgs e)
        {
            if(Item_Id>0 && Process_Id>0)
            {
                if (textBoxVolume.Text == "" || textBoxActualNoOfOpr.Text == "" || textBoxHoursWorked.Text == "" || textBoxActualManHour.Text == "" || textBoxPlannedManHour.Text == "" || textBoxEffiDailyProdEffic.Text=="")
                {
                    labelRequiredValues.Visible = true;
                }
                else if (labelRecordupdatedSucessfullyDailyEntryProductionEfficiency.Visible == true)
                {
                    textBoxHoursWorked.Text = "";
                    textBoxVolume.Text = "";
                    textBoxActualNoOfOpr.Text = "";
                }
                else
                {
                    int noofRecordsAffected = ButtonsUtility.SaveDailyEntryProcess(Item_Id, Process_Id,Convert.ToInt32(textBoxVolume.Text), Convert.ToInt32(textBoxActualNoOfOpr.Text), Convert.ToDecimal(textBoxHoursWorked.Text), dateTimePickerDailyEntryProductionEfficiency.Text,ActualManhours,RoundedPlannedManHour,Convert.ToDecimal(textBoxEffiDailyProdEffic.Text));

                    if (noofRecordsAffected > 0)
                    {
                        labelRecordupdatedSucessfullyDailyEntryProductionEfficiency.Text = "Record entered sucessfully.";
                        labelRecordupdatedSucessfullyDailyEntryProductionEfficiency.Visible = true;
                    }
                    else
                    {
                        labelRecordupdatedSucessfullyDailyEntryProductionEfficiency.Text = "Cannot save the already saved record. Please Edit it.";
                        labelRecordupdatedSucessfullyDailyEntryProductionEfficiency.Visible = true;

                    }
                }
            }
            
            else if(Item_Id==0 && comboBoxDailyEntryProductionEfficiency.Text=="")
            {

                labelPleaseSelectItemDailyEntryProductionEfficiency.Visible = true;


            }
            else if (Process_Id==0 && comboBoxSelectProcess.Text=="")
            {
                labelPleaseselectProcess.Visible = true;
            }
             
        }

      
        private void TextBoxesCalculatedValuesofItemSelectedbyCombo()
        {
            DataTable dt = new DataTable();
            dt = ButtonsUtility.GetProcessDetailsOnChange(Process_Id, Item_Id, dateTimePickerDailyEntryProductionEfficiency.Text);
            if(dt.Rows.Count>0)
            {
                textBoxVolume.Text = dt.Rows[0][0].ToString();
                textBoxActualNoOfOpr.Text = dt.Rows[0][1].ToString();
                textBoxHoursWorked.Text = dt.Rows[0][2].ToString();
                textBoxActualManHour.Text = dt.Rows[0][3].ToString();
                ActualManhours = Convert.ToDecimal(dt.Rows[0][3]);
                textBoxPlannedManHour.Text = dt.Rows[0][4].ToString();
                RoundedPlannedManHour=Convert.ToDecimal(dt.Rows[0][4]);
                textBoxEffiDailyProdEffic.Text = dt.Rows[0][5].ToString();
                    
            }
            else
            {
                textBoxVolume.Text = "";
                textBoxActualNoOfOpr.Text = "";
                textBoxHoursWorked.Text = "";
                textBoxActualManHour.Text = "";
                textBoxPlannedManHour.Text = "";
                RoundedPlannedManHour = 0;
                ActualManhours = 0;
                textBoxEffiDailyProdEffic.Text = "";

            }

            labelPleaseselectProcess.Visible = false;

        }

        private void comboBoxItemsDailyEntry_SelectedIndexChanged(object sender, EventArgs e)
        {
            
                textBoxVolume.Text = "";
                textBoxActualNoOfOpr.Text = "";
                textBoxHoursWorked.Text = "";
                textBoxActualManHour.Text = "";
                textBoxPlannedManHour.Text = "";
                RoundedPlannedManHour = 0;
                ActualManhours = 0;
                textBoxEffiDailyProdEffic.Text = "";
                comboBoxSelectProcess.Text = "";
                TextBoxesCalculatedValuesofItemSelectedbyCombo();

            
            Item_Id=ButtonsUtility.GetITemId(comboBoxDailyEntryProductionEfficiency.Text);
            labelPleaseSelectItemDailyEntryProductionEfficiency.Visible = false;
            labelRecordupdatedSucessfullyDailyEntryProductionEfficiency.Visible = false;
            labelEditErrorMessageforDatebuttonMenuDailyEntryProductionEfficiency.Visible = false;
            GetProcess();
        }
        private void GetProcess()
        {
            DataTable dt = new DataTable();
            dt = ButtonsUtility.GetProcesses(Item_Id);

            comboBoxSelectProcess.Items.Clear();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                comboBoxSelectProcess.Items.Add(dt.Rows[i][0]);

            }
        }

        private void buttonEditDailyEntry_Click(object sender, EventArgs e)
        {
            if (Item_Id > 0 && Process_Id > 0)
            {
                if (textBoxVolume.Text == "" || textBoxActualNoOfOpr.Text == "" || textBoxHoursWorked.Text == "")
                {
                    labelRequiredValues.Visible = true;
                }
                else if (labelRecordupdatedSucessfullyDailyEntryProductionEfficiency.Visible == true)
                {
                    textBoxHoursWorked.Text = "";
                    textBoxVolume.Text = "";
                    textBoxActualNoOfOpr.Text = "";
                }

                else
                {


                    int noofRecordsAffected = ButtonsUtility.EditDailyEntryProductionEfficiency(Item_Id, Process_Id,Convert.ToInt32(textBoxVolume.Text), Convert.ToInt32(textBoxActualNoOfOpr.Text), Convert.ToDecimal(textBoxHoursWorked.Text), dateTimePickerDailyEntryProductionEfficiency.Text,ActualManhours,RoundedPlannedManHour,Convert.ToDecimal(textBoxEffiDailyProdEffic.Text));


                    if (noofRecordsAffected > 0)
                    {

                        labelRecordupdatedSucessfullyDailyEntryProductionEfficiency.Text = "Record updated sucessfully.";

                        labelRecordupdatedSucessfullyDailyEntryProductionEfficiency.Visible = true;

                    }
                    
                }
            }
            else if (Item_Id == 0 && comboBoxDailyEntryProductionEfficiency.Text == "")
            {

                labelPleaseSelectItemDailyEntryProductionEfficiency.Visible = true;


            }
            else if (Process_Id == 0 && comboBoxSelectProcess.Text == "")
            {
                labelPleaseselectProcess.Visible = true;
            }
            
        }

       
      

        private void labelDailyEntryDateTime_Click(object sender, EventArgs e)
        {

        }

        private void comboBoxSelectProcess_SelectedIndexChanged(object sender, EventArgs e)
        {
            DataTable dt = new DataTable();
            dt=ButtonsUtility.GetProcessIdAndCapacity(comboBoxSelectProcess.Text, Item_Id);
            Process_Id = Convert.ToInt32(dt.Rows[0][0]);

            Capacity = Convert.ToDecimal(dt.Rows[0][1]);
            No_of_workers = Convert.ToInt32(dt.Rows[0][2]);

            if (Item_Id > 0 && Process_Id > 0 && dateTimePickerDailyEntryProductionEfficiency.Text!="")
            {
                TextBoxesCalculatedValuesofItemSelectedbyCombo();
            }
           // dt = ButtonsUtility.GetDailyEntryProdEfficiency(Process_Id, Item_Id);
           // decimal PlannedManHour = ((No_of_workers / Capacity) * (Convert.ToInt32(textBoxVolume.Text)));
            //decimal RoundedPlannedManHour = (Math.Round(PlannedManHour, 2));

            //textBoxPlannedManHour.Text = RoundedPlannedManHour.ToString();
            //labelRecordupdatedSucessfullyDailyEntryProductionEfficiency.Visible = false;

            labelPleaseselectProcess.Visible = false;
        }

        private void textBoxVolume_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 46 && textBoxVolume.Text.IndexOf('.') != -1)//check if user has entered more than one dot .
            {
                e.Handled = true;
                return;
            }
            if ((!char.IsNumber(e.KeyChar)) && (!char.IsControl(e.KeyChar)) && (e.KeyChar != 46))  //46 is ASCCI Code for dot "." 
            {

                e.Handled = true;
            }
        }

        private void textBoxVolume_TextChanged(object sender, EventArgs e)
        {
            if (textBoxVolume.Text != "" && textBoxActualNoOfOpr.Text!="" && textBoxHoursWorked.Text!="") 
            {
                labelRequiredValues.Visible = false;
               // decimal PlannedManHour = ((No_of_workers / Capacity) * (Convert.ToInt32(textBoxVolume.Text)));
              //  decimal PlannedManHour = (((Convert.ToInt32(textBoxActualNoOfOpr.Text) * Convert.ToDecimal(textBoxHoursWorked.Text) / Capacity) * (Convert.ToDecimal(textBoxVolume.Text))));
                decimal PlannedManHour = ((1 / Capacity) * (Convert.ToInt32(textBoxVolume.Text)));
                decimal RoundedPlannedManHours = (Math.Round(PlannedManHour, 2));
                RoundedPlannedManHour = RoundedPlannedManHours;
                textBoxPlannedManHour.Text = RoundedPlannedManHours.ToString();
                labelRecordupdatedSucessfullyDailyEntryProductionEfficiency.Visible = false;
            }
            else if (textBoxVolume.Text == "" && textBoxVolume.Text == "")
            {
                textBoxPlannedManHour.Text = "";
            }
        }

        private void textBoxActualNoOfOpr_KeyPress(object sender, KeyPressEventArgs e)
        {
            if ((!char.IsNumber(e.KeyChar)) && (!char.IsControl(e.KeyChar)))
            {

                e.Handled = true;
            }
        }

        private void textBoxActualNoOfOpr_TextChanged(object sender, EventArgs e)
        {
            if (textBoxActualNoOfOpr.Text != "" && textBoxHoursWorked.Text != "")
            {
                labelRequiredValues.Visible = false;
                decimal AcutalManhour=(Convert.ToInt32(textBoxActualNoOfOpr.Text) * Convert.ToDecimal(textBoxHoursWorked.Text));
                decimal ratioRoundedActualManhour = (Math.Round(AcutalManhour, 2));
                ActualManhours = ratioRoundedActualManhour;
                textBoxActualManHour.Text = ratioRoundedActualManhour.ToString();
                labelRecordupdatedSucessfullyDailyEntryProductionEfficiency.Visible = false;

            }
            if (textBoxVolume.Text != "" && textBoxActualNoOfOpr.Text != "" && textBoxHoursWorked.Text != "")
            {
                //   decimal PlannedManHour = ((Convert.ToInt32(textBoxActualNoOfOpr.Text) * Convert.ToDecimal(textBoxHoursWorked.Text) / Capacity) * (Convert.ToInt32(textBoxVolume.Text)));
                decimal PlannedManHour = ((1 / Capacity) * (Convert.ToInt32(textBoxVolume.Text)));
                decimal RoundedPlannedManHours = (Math.Round(PlannedManHour, 2));
                RoundedPlannedManHour = RoundedPlannedManHours;
                textBoxPlannedManHour.Text = RoundedPlannedManHours.ToString();
                labelRecordupdatedSucessfullyDailyEntryProductionEfficiency.Visible = false;
            }
            else
            {
                textBoxActualManHour.Text = "";
                ActualManhours = 0;
            }
        }

        private void textBoxHoursWorked_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 46 && textBoxHoursWorked.Text.IndexOf('.') != -1)//check if user has entered more than one dot .
            {
                e.Handled = true;
                return;
            }
            if ((!char.IsNumber(e.KeyChar)) && (!char.IsControl(e.KeyChar)) && (e.KeyChar != 46))  //46 is ASCCI Code for dot "." 
            {

                e.Handled = true;
            }
            if (e.KeyChar == 13)
            {
                buttonDailyEntrySavebuttonMenuDailyEntryProductionEfficiency.PerformClick();
            }
        }

        private void textBoxHoursWorked_TextChanged(object sender, EventArgs e)
        {
            if (textBoxActualNoOfOpr.Text != "" && textBoxHoursWorked.Text != "")
            {
                labelRequiredValues.Visible = false;
                labelRecordupdatedSucessfullyDailyEntryProductionEfficiency.Visible = false;
                decimal AcutalManhour = (Convert.ToDecimal(textBoxActualNoOfOpr.Text) * Convert.ToDecimal(textBoxHoursWorked.Text));
                decimal ratioRoundedActualManhour = (Math.Round(AcutalManhour, 2));
                ActualManhours = ratioRoundedActualManhour;
                textBoxActualManHour.Text = ratioRoundedActualManhour.ToString();

            }
            if (textBoxVolume.Text != "" && textBoxActualNoOfOpr.Text != "" && textBoxHoursWorked.Text != "")
            {
                //decimal PlannedManHour = ((Convert.ToInt32(textBoxActualNoOfOpr.Text) * Convert.ToDecimal(textBoxHoursWorked.Text) / Capacity) * (Convert.ToInt32(textBoxVolume.Text)));
                decimal PlannedManHour = ((1 / Capacity) * (Convert.ToInt32(textBoxVolume.Text)));
                RoundedPlannedManHour = (Math.Round(PlannedManHour, 2));

                textBoxPlannedManHour.Text = RoundedPlannedManHour.ToString();
                labelRecordupdatedSucessfullyDailyEntryProductionEfficiency.Visible = false;
            }
            else
            {
                textBoxActualManHour.Text = "";
                ActualManhours = 0;
            }
        }

        private void textBoxActualManHour_TextChanged(object sender, EventArgs e)
        {
            if (textBoxActualManHour.Text != "" && textBoxPlannedManHour.Text != "")
            {

               // decimal PlannedManhour = Convert.ToDecimal(textBoxPlannedManHour.Text);
              //  decimal Actual =Convert.ToDecimal((Convert.ToInt32(textBoxActualManHour.Text)));
                decimal Efficiency = (RoundedPlannedManHour * 100) / (ActualManhours);
                decimal ratioRoundedEffi = (Math.Round(Efficiency, 2));
                textBoxEffiDailyProdEffic.Text = ratioRoundedEffi.ToString();
                
            }
            else
                textBoxEffiDailyProdEffic.Text = "";
        }

        private void textBoxPlannedManHour_TextChanged(object sender, EventArgs e)
        {
            if (textBoxActualManHour.Text != "" && textBoxPlannedManHour.Text != "")
            {
                decimal Efficiency = (RoundedPlannedManHour * 100) / (ActualManhours);
                decimal ratioRoundedEffi = (Math.Round(Efficiency, 2));
                textBoxEffiDailyProdEffic.Text = ratioRoundedEffi.ToString();
            }
            else
                textBoxEffiDailyProdEffic.Text = "";
        
        }

        private void dateTimePickerDailyEntrybuttonMenuDailyEntryProductionEfficiency_ValueChanged(object sender, EventArgs e)
        {
            if (comboBoxSelectProcess.Text != "")
            {
                TextBoxesCalculatedValuesofItemSelectedbyCombo();

            } 
            labelRecordupdatedSucessfullyDailyEntryProductionEfficiency.Visible = false;
            labelEditErrorMessageforDatebuttonMenuDailyEntryProductionEfficiency.Visible = false;
        }

        private void DailyEntry_ProductionEfficiency_Load(object sender, EventArgs e)
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
