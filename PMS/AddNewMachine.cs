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
    public partial class AddNewMachine : Form
    {
        private int WorkShopId = 0;
        private int MachineId = 0;
        private string Machine_Code = "";
        int Edit = 0;
        public AddNewMachine(int AddEdit)
        {
            InitializeComponent();
            Edit = AddEdit;
            if (AddEdit == 0)
            {
                buttonEditMachine.Visible = false;
                comboBoxMachineSelection.Visible = false;
                labelSelectMachine.Visible = false;
                buttonDelete.Visible = false ;
            }
            else
            {
                buttonEditMachine.Visible = true ;
                buttonDelete.Visible = true;
                buttonSaveWS.Visible = false;
                
            }
            GetWorkShops();
        }
        private void GetMachines()
        {
            DataTable dt = new DataTable();
            dt = ButtonsUtility.ViewMachines(WorkShopId);
            string MachineCode = "";
            string MachineName = "";
            string MachineNameCode = "";

            comboBoxMachineSelection.Items.Clear();
            if (dt.Rows.Count < 1)
            {
                labelPleaseCreateAMachine.Visible = true; ;
            }
            else
            {
                comboBoxMachineSelection.Items.Clear();
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    MachineName = dt.Rows[i][0].ToString();
                    MachineCode = dt.Rows[i][2].ToString();
                    MachineNameCode = MachineCode + " - " + MachineName;
                    comboBoxMachineSelection.Items.Add(MachineNameCode);

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
                comboBoxWorkShopMachine.Items.Clear();
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    comboBoxWorkShopMachine.Items.Add(dt.Rows[i][0]);

                }
            }

           
           
        }
        private void buttonSave_Click(object sender, EventArgs e)
        {
            if (textBoxMachineName.Text == "" || textBoxShiftTimeMachine.Text == ""|| textBoxCode.Text=="" || comboBoxNoOfMonthd.Text=="")
            {
                labelCompulsaryFieds.Visible = true;
            }
            else if(comboBoxWorkShopMachine.Text=="")
            {
                labelSelectWorkShopMachine.Visible = true;
            }
            else
            {
                int rowseffected = ButtonsUtility.SaveMachine(textBoxMachineName.Text, WorkShopId, Convert.ToDecimal(textBoxShiftTimeMachine.Text), textBoxCode.Text, Convert.ToInt32(comboBoxNoOfMonthd.Text));
                if (rowseffected > 0)
                {
                     labelMachineInfoInsertSucess.Text ="Machine information inserted sucessfully.";
                    labelMachineInfoInsertSucess.Visible = true;
                    buttonEditMachine.Visible = true;
                    comboBoxMachineSelection.Visible = true;
                    labelSelectMachine.Visible = true;
                    buttonDelete.Visible = true;
                   
                    GetMachines();
                }
                else
                {
                    comboBoxMachineSelection.Visible = true;
                    labelSelectMachine.Visible = true;
                    GetMachines();
                    labelMachineInfoInsertSucess.Text = "You cannot save already saved machine. Please edit its information if required.";
                   buttonEditMachine.Visible = true;
                   comboBoxMachineSelection.Text = textBoxMachineName.Text;
                   buttonDelete.Visible = true;

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
            
             if (textBoxMachineName.Text == "" || textBoxShiftTimeMachine.Text == "" || textBoxCode.Text=="" ||  comboBoxNoOfMonthd.Text=="")
            {
                labelCompulsaryFieds.Visible = true;
            }
            else if(comboBoxWorkShopMachine.Text=="")
            {
                labelSelectWSFirstMachine.Visible = true;
            }
            else if(comboBoxMachineSelection.Text=="")
             {
                 labelSelectaMachinePlease.Visible = true;
                 
             }
             else
             {
                 int rowseffected = ButtonsUtility.EditMachine(textBoxMachineName.Text, MachineId, Convert.ToDecimal(textBoxShiftTimeMachine.Text), WorkShopId, textBoxCode.Text, Convert.ToInt32(comboBoxNoOfMonthd.Text));
                if (rowseffected > 0)
                {
                    GetMachines();
                    labelMachineinfoUpdateSucessful.Visible = true;
                    labelMachineInfoInsertSucess.Visible = false;
                }
             }
        }

        private void textBoxFactoryName_TextChanged(object sender, EventArgs e)
        {
            labelMachineinfoUpdateSucessful.Visible = false;
            labelMachineInfoInsertSucess.Visible = false;
            labelDelete.Visible = false;
        }

        private void comboBoxWorkShop_SelectedIndexChanged(object sender, EventArgs e)
        {
           
            WorkShopId=ButtonsUtility.GetWorkShopId( comboBoxWorkShopMachine.Text);
            labelMachineinfoUpdateSucessful.Visible = false;
            labelMachineInfoInsertSucess.Visible = false;
            labelDelete.Visible = false;
            textBoxCode.Clear();
            textBoxMachineName.Clear();
            textBoxShiftTimeMachine.Clear();
            comboBoxNoOfMonthd.Text = "";
            if (comboBoxMachineSelection.Visible==true)
            {
                comboBoxMachineSelection.Text = "";
                GetMachines();
            }
            if(Edit>0)
            {
                GetMachines();
            }
        }

   
        private void textBoxShitTimeMachine_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 46 && textBoxShiftTimeMachine.Text.IndexOf('.') != -1)//check if user has entered more than one dot .
            {
                e.Handled = true;
                return;
            }
            if ((!char.IsNumber(e.KeyChar)) && (!char.IsControl(e.KeyChar)) && (e.KeyChar != 46))  //46 is ASCCI Code for dot "." 
            {

                e.Handled = true;
            }
            if (e.KeyChar == 13 && buttonSaveWS.Visible == true)
            {
                buttonSaveWS.PerformClick();
            }
            else if (e.KeyChar == 13 && buttonSaveWS.Visible == false)
            {
                buttonEditMachine.PerformClick();
            }
        }

        
        private void textBoxShitTimeMachine_TextChanged(object sender, EventArgs e)
        {
            labelMachineinfoUpdateSucessful.Visible = false;
            labelMachineInfoInsertSucess.Visible = false;
            labelDelete.Visible = false;
        }

        private void comboBoxMachineSelection_SelectedIndexChanged(object sender, EventArgs e)
        {
            
            DataTable dt = new DataTable();


            string input = comboBoxMachineSelection.Text;
            Machine_Code = input.Remove(input.IndexOf("-"));

            dt = ButtonsUtility.GetMachineId(Machine_Code);

           
            string output = input.Substring(input.IndexOf('-') + 1);

            textBoxMachineName.Text = output;
            MachineId =Convert.ToInt32(dt.Rows[0][0]);
            
            textBoxShiftTimeMachine.Text = dt.Rows[0][1].ToString();
            textBoxCode.Text = dt.Rows[0][2].ToString();
            comboBoxNoOfMonthd.Text = dt.Rows[0][3].ToString();

            labelMachineinfoUpdateSucessful.Visible = false;
            labelDelete.Visible = false;
            
            
        }

        private void buttonDelete_Click(object sender, EventArgs e)
        {

            DialogResult dialog = MessageBox.Show("Are you sure you want to Delete the Machine?", "Delete Machine", MessageBoxButtons.YesNo,MessageBoxIcon.Warning);
            if (dialog == DialogResult.Yes)
            {
                DeleteMachine();
            }
           

        }

        private void DeleteMachine()
        {
             int rows=ButtonsUtility.DeleteMachine(MachineId);
            if(rows>0)
            {
             
                textBoxCode.Clear();
                textBoxMachineName.Clear();
                textBoxShiftTimeMachine.Clear();
                comboBoxNoOfMonthd.Text = "";
                comboBoxWorkShopMachine.Text = "";
                comboBoxMachineSelection.Items.Clear();
                comboBoxMachineSelection.Text = "";
                GetMachines();
                labelDelete.Visible = true;
                
            }
        }
        private void comboBoxNoOfMonthd_SelectedIndexChanged(object sender, EventArgs e)
        {
            labelMachineinfoUpdateSucessful.Visible = false;
            labelDelete.Visible = false;
        }

        private void textBoxCode_TextChanged(object sender, EventArgs e)
        {
            labelMachineinfoUpdateSucessful.Visible = false;
            labelDelete.Visible = false;
        }

        private void buttonBack_Click(object sender, EventArgs e)
        {
            Settings config = new Settings();
            config.Show();
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
