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
    public partial class DailyEntry_Machine : Form
    {
        private int WorkShopId = 0;
        private int MachineId = 0;
        private int ItemId = 0;
        private int ProcessId = 0;
        private string Machine_Code = "";
        int Edit = 0;
        public DailyEntry_Machine(int AddEdit)
        {
            InitializeComponent();
            Edit = AddEdit;
            if (AddEdit == 0)
            {
               // buttonEditWS.Visible = false;
               // comboBoxMachineSelectiondailyEntryMachine.Visible = false;
              //  labelSelectMachinedailyEntryMachine.Visible = false;
            }
            else
            {
               // buttonEditWS.Visible = true ;
               
               // buttonSaveWS.Visible = false;
            }
            GetWorkShops();
           
            GetShiftTime();
        }

        private void GetShiftTime()
        {
            DataTable dt = new DataTable();
            dt = ButtonsUtility.GetShiftTime();

            textBoxShitTimeMachinedailyEntryMachine.Text = dt.Rows[0][0].ToString();
        }

        private void InitializeDropDownItemValues()
        {

            DataTable dt = new DataTable();
            dt = ButtonsUtility.InitializeItemByMachineDropDown(MachineId);
            if(dt.Rows.Count<1)
            {
                labelAssociateMachineWithItem.Visible = true;
                comboBoxItem.Text = "";
            }
            comboBoxItem.Items.Clear();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                comboBoxItem.Items.Add(dt.Rows[i][0]);

            }
        }
        private void GetMachines()
        {
            DataTable dt = new DataTable();
            dt = ButtonsUtility.ViewMachines(WorkShopId);
            string MachineCode = "";
            string MachineName = "";
            string MachineNameCode = "";
            comboBoxMachineSelectiondailyEntryMachine.Text = "";
            comboBoxMachineSelectiondailyEntryMachine.Items.Clear();
            if (dt.Rows.Count < 1)
            {
                labelPleaseCreateAMachine.Visible = true; ;
            }
            else
            {
               
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    MachineName = dt.Rows[i][0].ToString();
                    MachineCode = dt.Rows[i][2].ToString();

                    MachineNameCode = MachineCode + " - " + MachineName;

                    comboBoxMachineSelectiondailyEntryMachine.Items.Add(MachineNameCode);

                }
            }

        }
        private void GetWorkShops()
        {


            DataTable dt = new DataTable();
            dt=ButtonsUtility.ViewShops();
            

            if (dt.Rows.Count  <1)
            {
                labelcreateworkShopFirst.Visible = true; ;
            }
            else
            {
                comboBoxWorkShopMachinedailyEntryMachine.Items.Clear();
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    comboBoxWorkShopMachinedailyEntryMachine.Items.Add(dt.Rows[i][0]);

                }
            }

           
           
        }
        private void buttonSave_Click(object sender, EventArgs e)
        {
            if (textBoxDownTimeDailyEntry.Text == "")
            {
                labelCompulsaryFieds.Visible = true;
            }
            else if(comboBoxWorkShopMachinedailyEntryMachine.Text=="" )
            {
                labelSelectWorkShopMachinedailyEntryMachine.Visible = true;
            }
            else if(comboBoxMachineSelectiondailyEntryMachine.Text=="")
            {
                labelSelectaMachinePleasedailyEntryMachine.Visible = true;
            }
            else
            {
                int rowseffected = ButtonsUtility.SaveMachineDailyDownTime(MachineId, WorkShopId, Convert.ToDecimal(textBoxDownTimeDailyEntry.Text), Convert.ToDecimal(textBoxRatioDailyTimeEntry.Text),dateTimePickerDailyEntryMachine.Text,ItemId,ProcessId);
                if (rowseffected > 0)
                {
                    labelMachineInfoInsertSucess.Visible = true;
                }
                else
                {
                    labelPleasePresseditButtonforalreadyseavedentry.Visible = true;
                }
            }
        }

        private void buttonMenu_Click(object sender, EventArgs e)
        {
            ButtonsUtility.Menu(0);
            this.Close();
        }

        private void buttonLogOut_Click(object sender, EventArgs e)
        {
            ButtonsUtility.NavigateToLogin();
            this.Close();
        }

        private void buttonExit_Click(object sender, EventArgs e)
        {
            ButtonsUtility.ExitProgram();
        }

        private void textBoxFactoryName_KeyPress(object sender, KeyPressEventArgs e)
        { 
            labelMachineInfoInsertSucess.Visible = false;
            labelMachineinfoUpdateSucessful.Visible = false;
        }

        private void buttonEdit_Click(object sender, EventArgs e)
        {
            
             if (textBoxMachineNamedailyEntryMachine.Text == "" || textBoxShitTimeMachinedailyEntryMachine.Text == "")
            {
                labelCompulsaryFieds.Visible = true;
            }
            else if(comboBoxWorkShopMachinedailyEntryMachine.Text=="")
            {
                labelSelectWSFirstMachinedailyEntryMachine.Visible = true;
            }
            else if(comboBoxMachineSelectiondailyEntryMachine.Text=="")
             {
                 labelSelectaMachinePleasedailyEntryMachine.Visible = true;
                 
             }
             else
             {
               
                 int rowseffected = ButtonsUtility.EditMachineDailyEntry( MachineId,WorkShopId,Convert.ToDecimal(textBoxDownTimeDailyEntry.Text),Convert.ToDecimal(textBoxRatioDailyTimeEntry.Text),dateTimePickerDailyEntryMachine.Text,ItemId,ProcessId);
                if (rowseffected > 0)
                {
                    GetMachines();
                    labelMachineinfoUpdateSucessful.Visible = true;
                    labelPleasePresseditButtonforalreadyseavedentry.Visible = false;
                }
             }
        }

        private void textBoxFactoryName_TextChanged(object sender, EventArgs e)
        {
            labelMachineinfoUpdateSucessful.Visible = false;
            labelMachineInfoInsertSucess.Visible = false;
        }

        private void comboBoxWorkShop_SelectedIndexChanged(object sender, EventArgs e)
        {
            labelPleaseCreateAMachine.Visible = false;
            labelSelectWSFirstMachinedailyEntryMachine.Visible = false;
            textBoxDownTimeDailyEntry.Text = "";
            textBoxRatioDailyTimeEntry.Text = "";
            labelPleasePresseditButtonforalreadyseavedentry.Visible = false;
            WorkShopId=ButtonsUtility.GetWorkShopId( comboBoxWorkShopMachinedailyEntryMachine.Text);
            labelMachineinfoUpdateSucessful.Visible = false;
            labelMachineInfoInsertSucess.Visible = false;
            comboBoxMachineSelectiondailyEntryMachine.Text="";
            GetMachines();
            labelPleasePresseditButtonforalreadyseavedentry.Visible = false;   
        }

   
        private void textBoxShitTimeMachine_KeyPress(object sender, KeyPressEventArgs e)
        {
            if ((!char.IsNumber(e.KeyChar)) && (!char.IsControl(e.KeyChar)))
            {

                e.Handled = true;
            }
        }

        
        private void textBoxShitTimeMachine_TextChanged(object sender, EventArgs e)
        {
            labelMachineinfoUpdateSucessful.Visible = false;
            labelMachineInfoInsertSucess.Visible = false;
        }

        private void comboBoxMachineSelection_SelectedIndexChanged(object sender, EventArgs e)
        {
            labelSelectaMachinePleasedailyEntryMachine.Visible = false;
            textBoxDownTimeDailyEntry.Text = "";
            textBoxRatioDailyTimeEntry.Text = "";
            labelPleasePresseditButtonforalreadyseavedentry.Visible = false;

            textBoxMachineNamedailyEntryMachine.Text = comboBoxMachineSelectiondailyEntryMachine.Text;
            DataTable dt = new DataTable();


            string input = comboBoxMachineSelectiondailyEntryMachine.Text;
            Machine_Code = input.Remove(input.IndexOf("-"));
            dt = ButtonsUtility.GetMachineId(Machine_Code);
            MachineId =Convert.ToInt32(dt.Rows[0][0]);
            
            //textBoxShitTimeMachinedailyEntryMachine.Text = dt.Rows[0][1].ToString();

            labelMachineinfoUpdateSucessful.Visible = false;
            InitializeDropDownItemValues();

            labelPleasePresseditButtonforalreadyseavedentry.Visible = false;
        }

        private void textBoxDownTimeDailyEntry_KeyPress(object sender, KeyPressEventArgs e)
        {

            if (e.KeyChar == 46 && textBoxDownTimeDailyEntry.Text.IndexOf('.') != -1)//check if user has entered more than one dot .
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
                buttonSaveWS.PerformClick();
            }
            labelMachineinfoUpdateSucessful.Visible = false;
            labelMachineInfoInsertSucess.Visible = false;
            labelCompulsaryFieds.Visible = false;
            labelcreateworkShopFirst.Visible = false;
        }

        private void dateTimePickerDailyEntryMachine_ValueChanged(object sender, EventArgs e)
        {
            textBoxDownTimeDailyEntry.Text = "";
            textBoxRatioDailyTimeEntry.Text = "";
            if (comboBoxWorkShopMachinedailyEntryMachine.Text == "")
            {
                labelSelectWSFirstMachinedailyEntryMachine.Visible = true;
            }
            else if(comboBoxMachineSelectiondailyEntryMachine.Text=="")
            {
                labelSelectaMachinePleasedailyEntryMachine.Visible = true;
            }
            else 
            {
                GetPreviousEntriesDownTime(dateTimePickerDailyEntryMachine.Text);



            }
            labelPleasePresseditButtonforalreadyseavedentry.Visible = false;
        }

        private void GetPreviousEntriesDownTime(string dateTime)
        {
            int downTime = ButtonsUtility.GetDownTime(dateTime, MachineId, WorkShopId,ItemId,ProcessId);
            if(downTime>0)
            {
                textBoxDownTimeDailyEntry.Text = downTime.ToString();
                decimal ratio = (downTime / Convert.ToDecimal(textBoxShitTimeMachinedailyEntryMachine.Text)) * 100;
                decimal ratioRounded = (Math.Round(ratio, 2));
                textBoxRatioDailyTimeEntry.Text = ratioRounded.ToString();
            }
        }

        private void textBoxDownTimeDailyEntry_TextChanged(object sender, EventArgs e)
        {
            if(textBoxDownTimeDailyEntry.Text!="")
            {
                decimal ratio = (Convert.ToDecimal(textBoxDownTimeDailyEntry.Text) / Convert.ToDecimal(textBoxShitTimeMachinedailyEntryMachine.Text)) * 100;
                decimal ratioRounded = (Math.Round(ratio, 2));
                textBoxRatioDailyTimeEntry.Text = ratioRounded.ToString();
            }
            else
                textBoxRatioDailyTimeEntry.Text = "";


        }

        private void comboBoxItem_SelectedIndexChanged(object sender, EventArgs e)
        {
            ItemId = ButtonsUtility.GetITemId(comboBoxItem.Text);
            if (comboBoxItem.Text != "")
            {
                
                
                labelPleaseSelectItem.Visible = false;
            }
            if(ItemId>0)
            {
                
                GetProcess();
            }
            else if(ItemId==0)
            {
                comboBoxProcess.Items.Clear();
            }


            labelPleasePresseditButtonforalreadyseavedentry.Visible = false;
        }

        

  
        private void GetProcess()
        {
            comboBoxProcess.Items.Clear();
            DataTable dt = new DataTable();
            dt = ButtonsUtility.GetProcessesForMachine(ItemId,MachineId);

          
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                comboBoxProcess.Items.Add(dt.Rows[i][0]);

            }
        }

        private void DailyEntry_Machine_Load(object sender, EventArgs e)
        {

        }

        private void comboBoxProcess_SelectedIndexChanged(object sender, EventArgs e)
        {
            DataTable dt = new DataTable();
            dt = ButtonsUtility.GetProcessIdAndCapacity(comboBoxProcess.Text, ItemId);
            ProcessId = Convert.ToInt32(dt.Rows[0][0]);

            textBoxDownTimeDailyEntry.Text = "";

            labelPleaseSelectProcess.Visible = false;

            GetPreviousEntriesDownTime(dateTimePickerDailyEntryMachine.Text);
            labelPleasePresseditButtonforalreadyseavedentry.Visible = false;
        }

        private void buttonBack_Click(object sender, EventArgs e)
        {
            EquipFailureRateKPI Eqp = new EquipFailureRateKPI();
            Eqp.Show();
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
