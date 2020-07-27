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
    public partial class AddProcess : Form
    {
        private int Item_Id;
        private int Process_Id;
        private string Machine_Code = "";
     
        int FixedEntryValueCheck = 0;
        public AddProcess(int itemId, int AddorEdit)
        {
            InitializeComponent();
            Item_Id = itemId;
            if(AddorEdit==1) //1 is edit
            {   
                comboBoxItemsProcess.Visible=true;
                labelSelectProcess.Visible=true;
                InitializeDropDownItemValues(itemId,Machine_Code);
                buttonDailyEntrySave.Visible=false;
            }
            GetMachines();
          
            //buttonDailyEntrySave.Visible = false;  

            
            
        }
        private void InitializeDropDownItemValues(int itemId,string MachineCode)
        {

            DataTable dt = new DataTable();
            dt = ButtonsUtility.InitializeItemMachineDropDownProcesses(itemId,MachineCode);
            comboBoxItemsProcess.Items.Clear();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                comboBoxItemsProcess.Items.Add(dt.Rows[i][0]);

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
            AddNewItem AddItem = new AddNewItem(Item_Id);
            AddItem.Show();
            this.Close();
        }

       

        private void textBoxNameOfItem_TextChanged(object sender, EventArgs e)
        {
            labelRecordupdatedSucessfullyProcess.Visible = false;
            labelRequiredFileds.Visible = false;
            labelDelete.Visible = false;
            
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
            if (textBoxEnterProcess.Text == "" || textBoxNoofOperatos.Text == "" || textBoxCycleTime.Text == "" || textBoxRatingFactor.Text == "" || comboBoxMachineSelection.Text == "")
            {
                labelRequiredFileds.Visible=true;
            }
            //else if(labelRecordupdatedSucessfullyProcess.Visible == true)
            //{
            //    textBoxEnterProcess.Text="";
            //    textBoxNoofOperatos.Text="" ;
            //    textBoxCycleTime.Text="" ;
            //    textBoxRatingFactor.Text = ".";
            //    textBoxCapacity.Text = "";
                
            //}
            else{
                string input = comboBoxMachineSelection.Text;
                Machine_Code = input.Remove(input.IndexOf("-"));
            
                int noofRecordsAffected = ButtonsUtility.SaveProcess(Item_Id, textBoxEnterProcess.Text,Convert.ToInt32(textBoxNoofOperatos.Text), Convert.ToInt32(textBoxCycleTime.Text),Convert.ToDecimal(textBoxRatingFactor.Text),Convert.ToDecimal(textBoxCapacity.Text),Machine_Code);

                if (noofRecordsAffected > 0)
                {
                  labelRecordupdatedSucessfullyProcess.Text = "Record entered sucessfully.";
                   labelRecordupdatedSucessfullyProcess.Visible = true;
                  // InitializeDropDownItemValues(Item_Id);
                   comboBoxItemsProcess.Visible = true;
                   labelSelectProcess.Visible = true;
                   InitializeDropDownItemValues(Item_Id,Machine_Code);
               }
                else
                {
                    labelEditIt.Visible = true;
                    labelRecordupdatedSucessfullyProcess.Visible = false;
                }
            }
            
           
             
        }

       
       

        private void comboBoxItemsDailyEntry_SelectedIndexChanged(object sender, EventArgs e)
        {
           
            labelPleaseSelectItem.Visible = false;
            labelRecordupdatedSucessfullyProcess.Visible = false;
            labelEditErrorMessageforDate.Visible = false;
        }

        

        private void textBoxCostofEachUnit_KeyPress(object sender, KeyPressEventArgs e)
        {
            if ((!char.IsNumber(e.KeyChar)) && (!char.IsControl(e.KeyChar)))
            {

                e.Handled = true;
            }
        }

       

        private void textBoxCycleTime_KeyPress(object sender, KeyPressEventArgs e)
        {
            
                if ((!char.IsNumber(e.KeyChar)) && (!char.IsControl(e.KeyChar)))
                {

                    e.Handled = true;
                }
            
        }

        private void textBoxNoofOperatos_KeyPress(object sender, KeyPressEventArgs e)
        {
            
                if ((!char.IsNumber(e.KeyChar)) && (!char.IsControl(e.KeyChar)))
            {

                e.Handled = true;
            }
        }

        private void textBoxRatingFactor_KeyPress(object sender, KeyPressEventArgs e)
        {
            if(e.KeyChar==46 && textBoxRatingFactor.Text.IndexOf('.')!=-1)//check if user has entered more than one dot .
            {
                e.Handled=true;
                return;
            }
             if ((!char.IsNumber(e.KeyChar)) && (!char.IsControl(e.KeyChar)) && (e.KeyChar!=46))  //46 is ASCCI Code for dot "." 
            {

                e.Handled = true;
            }
             if (e.KeyChar == 13 && buttonDailyEntrySave.Visible == true)
             {
                 buttonDailyEntrySave.PerformClick();
             }
             else if (e.KeyChar == 13 && buttonDailyEntrySave.Visible == false)
             {
                 buttonEditProcess.PerformClick();
             }
        }

        private void textBoxCycleTime_TextChanged(object sender, EventArgs e)
        {
            if (textBoxRatingFactor.Text != "" && textBoxCycleTime.Text != "")
            {
                decimal capacity = (3600 / (Convert.ToDecimal(textBoxCycleTime.Text))) * (Convert.ToDecimal(textBoxRatingFactor.Text)/100);
                decimal CapacityRounded = (Math.Round(capacity, 2));
                textBoxCapacity.Text = CapacityRounded.ToString();
            }
            else
            {
                textBoxCapacity.Text = "";
            }
            labelRecordupdatedSucessfullyProcess.Visible = false;
            labelRequiredFileds.Visible = false;
            labelDelete.Visible = false;
        }

        private void textBoxRatingFactor_TextChanged(object sender, EventArgs e)
        {

            if (textBoxRatingFactor.Text != "" && textBoxCycleTime.Text != "")
            {
                decimal capacity = (3600 / (Convert.ToDecimal(textBoxCycleTime.Text))) * (Convert.ToDecimal(textBoxRatingFactor.Text)/100);
                decimal CapacityRounded = (Math.Round(capacity, 2));
                textBoxCapacity.Text = CapacityRounded.ToString();
            }
            else
            {
                textBoxCapacity.Text = "";
            }
            labelRecordupdatedSucessfullyProcess.Visible = false;
            labelRequiredFileds.Visible = false;
            labelDelete.Visible = false;
        }

        private void textBoxEnterProcess_TextChanged(object sender, EventArgs e)
        {
            labelRecordupdatedSucessfullyProcess.Visible = false;
            labelRequiredFileds.Visible = false;
            labelDelete.Visible = false;
        }

       

        private void comboBoxItemsProcess_SelectedIndexChanged(object sender, EventArgs e)
        {
            Process_Id = ButtonsUtility.GetProcessId(comboBoxItemsProcess.Text,Item_Id);
            GetProcessDetails();
            labelRecordupdatedSucessfullyProcess.Visible = false;
            labelRequiredFileds.Visible = false;
            labelDelete.Visible = false;

        }
        private void GetProcessDetails ()
        {
            DataTable dt = new DataTable();
            dt = ButtonsUtility.GetProcessDetails(Process_Id);

            textBoxEnterProcess.Text = dt.Rows[0][0].ToString();
            
            textBoxNoofOperatos.Text = dt.Rows[0][1].ToString();
            textBoxCycleTime.Text =dt.Rows[0][2].ToString();
            textBoxRatingFactor.Text = dt.Rows[0][3].ToString();
            textBoxCapacity.Text = dt.Rows[0][4].ToString();
            string MachineCode = "";
            string MachineName = "";
            string MachineNameCode = "";
            MachineCode = dt.Rows[0][5].ToString();
            MachineName = dt.Rows[0][6].ToString();
            if (MachineCode != "")
            {

                MachineNameCode = MachineCode + " - " + MachineName;
                comboBoxMachineSelection.Text = MachineNameCode;
            }
            
        }

        private void buttonEditProcess_Click(object sender, EventArgs e)
        {
            if (textBoxEnterProcess.Text == "" || textBoxNoofOperatos.Text == "" || textBoxCycleTime.Text == "" || textBoxRatingFactor.Text == "" || comboBoxMachineSelection.Text == "")
            {
                labelRequiredFileds.Visible = true;
            }
            else if (labelRecordupdatedSucessfullyProcess.Visible == true)
            {
                textBoxEnterProcess.Text = "";
                textBoxNoofOperatos.Text = "";
                textBoxCycleTime.Text = "";
                textBoxRatingFactor.Text = "";
                textBoxCapacity.Text = "";
            }
            else
            {


                int noofRecordsAffected = ButtonsUtility.EditProcess(Item_Id, textBoxEnterProcess.Text, Convert.ToInt32(textBoxNoofOperatos.Text), Convert.ToInt32(textBoxCycleTime.Text), Convert.ToDecimal(textBoxRatingFactor.Text), Convert.ToDecimal(textBoxCapacity.Text), Process_Id,Machine_Code);

                if (noofRecordsAffected > 0)
                {
                    labelRecordupdatedSucessfullyProcess.Text = "Record entered sucessfully.";
                    labelRecordupdatedSucessfullyProcess.Visible = true;
                }
            }
            labelEditIt.Visible = false;
        }

        private void GetMachines()
        {
            DataTable dt = new DataTable();
            dt = ButtonsUtility.GetMachineItemCodeAndName(Item_Id);
            string MachineCode = "";
            string MachineName = "";
            string MachineNameCode = "";

            if (dt.Rows.Count < 1)
            {
                // labelPleaseCreateAMachine.Visible = true; ;
            }
            else
            {
                comboBoxMachineSelection.Items.Clear();
                for (int i = 0; i < dt.Rows.Count; i++)
                {

                    MachineCode = dt.Rows[i][0].ToString();
                    MachineName = dt.Rows[i][1].ToString();

                    MachineNameCode = MachineCode + " - " + MachineName;
                    // comboBoxMachineSelection.Items.Add(dt.Rows[i][0]);
                    comboBoxMachineSelection.Items.Add(MachineNameCode);
                }
            }

        }

        private void comboBoxMachineSelection_SelectedIndexChanged(object sender, EventArgs e)
        {
            string input = comboBoxMachineSelection.Text;
            Machine_Code = input.Remove(input.IndexOf("-"));
            labelPlsSelectMachine.Visible = false;
            labelRequiredFileds.Visible = false;
            labelRecordupdatedSucessfullyProcess.Visible = false;
            labelDelete.Visible = false;
            if(comboBoxItemsProcess.Visible==true)
            {
                InitializeDropDownItemValues(Item_Id,Machine_Code);
            }
        }

        private void buttonDelete_Click(object sender, EventArgs e)
        {
            DialogResult dialog = MessageBox.Show("Are you sure you want to Delete the Process?", "Delete Process", MessageBoxButtons.YesNo,MessageBoxIcon.Warning);
            if (dialog == DialogResult.Yes)
            {
                DeleteProcess();
            }
           
        }
        private void DeleteProcess()
        {
            if (textBoxEnterProcess.Text == "" || textBoxNoofOperatos.Text == "" || textBoxCycleTime.Text == "" || textBoxRatingFactor.Text == "" || comboBoxMachineSelection.Text == "")
            {
                labelRequiredFileds.Visible = true;
            }
            else
            {
                int rows = ButtonsUtility.DeleteProcess(Process_Id);
                if (rows > 0)
                {

                    textBoxEnterProcess.Clear();
                    textBoxNoofOperatos.Clear();
                    textBoxCycleTime.Clear();
                    textBoxRatingFactor.Clear();
                    textBoxCapacity.Clear();
                    comboBoxMachineSelection.Text = "";
                    comboBoxItemsProcess.Text = "";
                    //InitializeDropDownItemValues(Item_Id,Machine_Code);
                    labelDelete.Visible = true;
                    labelRequiredFileds.Visible = false;

                }
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
