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
    public partial class Worker_Details : Form
    {
        string empId = "";
        public Worker_Details()
        {
            InitializeComponent();
            GetWorkersCount();
        }

        private void GetWorkersCount()
        {
            labelWorkersCount.Text = ButtonsUtility.WorkersCount();
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
            this.Close();
        }

       

        private void buttonEdit_Click(object sender, EventArgs e)
        {
            if (empId != "")
            {
                labelrequired.Visible = false;
                AddNewWorker worker = new AddNewWorker(empId);
                worker.Show();
                this.Close();
            }
            else
                labelrequired.Visible = true;

        }

        private void textBoxID_TextChanged(object sender, EventArgs e)
        {
            
           
            empId = textBoxID.Text;
            labelnotfound.Visible = false;
            labelrequired.Visible = false;
            labelnotfound.Visible = false;
            labelDelete.Visible = false;
            textBoxName.Clear();
            textBoxStatus.Clear();
            labelSave.Visible = false;
            labelArrival.Visible = false;
        }

        private void buttonGet_Click(object sender, EventArgs e)
        {
            if (textBoxID.Text == "")
            {
                labelrequired.Visible = true;
            }
            else
            {
                
                string name = ButtonsUtility.GetNameofEmployee(textBoxID.Text);
                textBoxName.Text = name;
                if (name != "No Record Found")
                {
                    string Attendance = ButtonsUtility.GetEmployeeAttendance(textBoxID.Text, dateTimePicker.Text);
                    textBoxStatus.Text = Attendance;
                    buttonArrival.Visible = true;
                    buttonEditInfo.Visible = true;
                   // buttonStatus.Visible = true;
                 //   buttonAbsent.Visible = true;
                    buttonDeparture.Visible = true;
                    buttonDelete.Visible = true;
                   
                }
                else
                    labelnotfound.Visible = true;
            }
        }

        private void buttonPresent_Click(object sender, EventArgs e)
        {
            if (textBoxName.Text != "No Record Found" && !string.IsNullOrWhiteSpace(textBoxName.Text))
            {
                DateTime myDate = dateTimePicker.Value.Date +
                   DateTime.Now.TimeOfDay; 
                textBoxStatus.Text = "Present";
                int rows;
                rows = ButtonsUtility.MarkAttendance("Present", @textBoxID.Text, myDate);
                if (rows > 0)
                {
                    labelSave.Visible = true;
                    labelArrival.Visible = false;
                }
            }
            else
            {
                textBoxStatus.Text = "Please click Get information button";
            }
        }


        private void buttonAbsent_Click(object sender, EventArgs e)
        {
            textBoxStatus.Text = "Absent";
            int rows;
            rows = ButtonsUtility.MarkAttendance("Absent", @textBoxID.Text, dateTimePicker.Value);
            if (rows > 0)
            {
                labelSave.Visible = true;
            }

        }

        private void buttonStatus_Click(object sender, EventArgs e)
        {
            string Attendance = ButtonsUtility.GetEmployeeAttendance(textBoxID.Text, dateTimePicker.Text);
            textBoxStatus.Text = Attendance;
            buttonArrival.Visible = true;
            buttonEditInfo.Visible = true;
           // buttonStatus.Visible = true;
           // buttonAbsent.Visible = true;
        }

        private void dateTimePicker_ValueChanged(object sender, EventArgs e)
        {
            if ((textBoxName.Text != "" || textBoxName.Text != "No Record Found") && textBoxID.Text != "")
            {
                string Attendance = ButtonsUtility.GetEmployeeAttendance(textBoxID.Text, dateTimePicker.Text);
                textBoxStatus.Text = Attendance;
                buttonArrival.Visible = true;
                buttonEditInfo.Visible = true;
                textBoxID.Clear();
                textBoxName.Clear();
                textBoxStatus.Clear();
             //   buttonStatus.Visible = true;
              //  buttonAbsent.Visible = true;
            }
        }

        private void buttonDelete_Click(object sender, EventArgs e)
        {
            DialogResult dialog = MessageBox.Show("Are you sure you want to Delete the Worker?", "Delete Worker", MessageBoxButtons.YesNo,MessageBoxIcon.Warning);
            if (dialog == DialogResult.Yes)
            {
                Delete();
            }
            
        }

        private void Delete()
        {
            if (textBoxID.Text != "")
            {
                int rows = ButtonsUtility.RemoveEmployee(empId);
                if (rows > 0)
                {
                    textBoxID.Clear();
                    textBoxName.Clear();
                    textBoxStatus.Clear();
                    labelDelete.Visible = true;
                }
            }

        }

        private void buttonBack_Click(object sender, EventArgs e)
        {
            Settings config = new Settings();
            config.Show();
            this.Close();
        }

        private void buttonDeparture_Click(object sender, EventArgs e)
        {

            if (textBoxName.Text != "No Record Found" && !string.IsNullOrWhiteSpace(textBoxName.Text))
            {
                DateTime myDate = dateTimePicker.Value.Date +
                    DateTime.Now.TimeOfDay; 


                if (textBoxStatus.Text == "Present")
                {
                    int rows;
                    rows = ButtonsUtility.MarkAttendance("Departure", @textBoxID.Text, myDate);
                    if (rows > 0)
                    {
                        labelSave.Visible = true;
                        labelArrival.Visible = false;
                    }
                }
                else
                {
                    labelArrival.Visible = true;
                }
            }
            else
            {
                textBoxStatus.Text = "Please click Get information button";
            }
        }

        private void buttonDetails_Click(object sender, EventArgs e)
        {
            GetAttendanceDetails AttendacneDetails = new GetAttendanceDetails();
            AttendacneDetails.Show();
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

        private void textBoxName_TextChanged(object sender, EventArgs e)
        {


            if (textBoxName.Text != "No Record Found" &&  ! string.IsNullOrWhiteSpace( textBoxName.Text))
                {
                    buttonArrival.Enabled = true;
                  
                    buttonDeparture.Enabled = true;
                  
                   
                }
                else
                {
                    buttonArrival.Enabled = false;
                    buttonDeparture.Enabled = false;
                    textBoxName.Text = "No Record Found";

                }
        }

        private void textBoxID_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13 )
            {
                buttonGet.PerformClick();
            }
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

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            WorkerView wrk = new WorkerView();
            wrk.Show();
            this.Close();
        }
    }
}
