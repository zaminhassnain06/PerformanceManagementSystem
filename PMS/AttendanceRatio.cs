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

    public partial class AttendanceRatio : Form
    {

        private int Month_Id = 0;
        public AttendanceRatio()
        {
            InitializeComponent();
           GetMonths();

           ButtonsUtility.SaveMonthsforObservationsActivitiesAttendanceRatio();

           GetObervationActivities();
           GetGridCalculations(DateTime.Now.Year);
        }

        private void GetObervationActivities()
        {

            DataSet ds = new DataSet("TimeRanges");
            ds = ButtonsUtility.GetActivitesObservationAttendanceRatio();
            dataGridViewObsAttendanceRatio.DataSource = ds.Tables[0];
            dataGridViewObsAttendanceRatio.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

            dataGridViewAcitvityAttendanceRatio.DataSource = ds.Tables[1];
            dataGridViewAcitvityAttendanceRatio.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
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

        
        private void GetGridCalculations(int year)
        {
            DataTable dt = new DataTable();

            dt = ButtonsUtility.GetCalculationsAttendanceRatioMonthly(year);

            int rows = dt.Rows.Count;
            try
            {
                dataGridViewAttendanceRatio.Rows.Clear();

                for (int i = 0; i < rows; i++)
                {

                    dataGridViewAttendanceRatio.Rows.Add(dt.Rows[i][0], dt.Rows[i][1], dt.Rows[i][2], dt.Rows[i][3]);


                    if (dt.Rows[i][3].ToString() == "Achieved")
                    {

                        dataGridViewAttendanceRatio.Rows[i].Cells[3].Style.BackColor = Color.LightGreen;
                        dataGridViewAttendanceRatio.Rows[i].Cells[3].Style.ForeColor = Color.Black;


                    }
                    else
                    {


                        dataGridViewAttendanceRatio.Rows[i].Cells[3].Style.BackColor = Color.IndianRed;
                        dataGridViewAttendanceRatio.Rows[i].Cells[3].Style.ForeColor = Color.Black;
                    }

                }



                if (chartAttendanceRatio.Series.IndexOf("Attendance Ratio Achieved %") != -1)
                {
                    chartAttendanceRatio.Series.Clear();
                }
                if (chartAttendanceRatio.Series.IndexOf("Attendance Ratio Not Achieved %") != -1)
                {
                    chartAttendanceRatio.Series.Clear();
                }
                chartAttendanceRatio.Series.Add("Attendance Ratio Achieved %");
                chartAttendanceRatio.Series.Add("Attendance Ratio Not Achieved %");

                chartAttendanceRatio.Series["Attendance Ratio Achieved %"].Color = Color.LightGreen;
                chartAttendanceRatio.Series["Attendance Ratio Not Achieved %"].Color = Color.IndianRed;
                for (int i = 0; i < rows; i++)
                {
                    if (dt.Rows[i][3].ToString() == "Achieved")
                    {

                        chartAttendanceRatio.Series["Attendance Ratio Achieved %"].Points.AddXY(dt.Rows[i][0].ToString(), dt.Rows[i][1]);
                        chartAttendanceRatio.Series["Attendance Ratio Not Achieved %"].Points.AddXY(dt.Rows[i][0].ToString(), 0);
                    //    chartAttendanceRatio.Series["Attendance Ratio Achieved %"].IsValueShownAsLabel = true;
                        chartAttendanceRatio.Series["Attendance Ratio Achieved %"].Points[i].IsValueShownAsLabel = true;
                        chartAttendanceRatio.Series["Attendance Ratio Not Achieved %"].Points[i].IsValueShownAsLabel = false;
                    }
                    else
                    {
                        chartAttendanceRatio.Series["Attendance Ratio Achieved %"].Points.AddXY(dt.Rows[i][0].ToString(), 0);
                        chartAttendanceRatio.Series["Attendance Ratio Not Achieved %"].Points.AddXY(dt.Rows[i][0].ToString(), dt.Rows[i][1]);
                       // chartAttendanceRatio.Series["Attendance Ratio Not Achieved %"].IsValueShownAsLabel = true;
                        chartAttendanceRatio.Series["Attendance Ratio Achieved %"].Points[i].IsValueShownAsLabel = false;
                        chartAttendanceRatio.Series["Attendance Ratio Not Achieved %"].Points[i].IsValueShownAsLabel = true;
                    }
                    chartAttendanceRatio.ChartAreas[0].AxisX.Interval = 1;


                    chartAttendanceRatio.Series[0].IsVisibleInLegend = false;


                }

            }
            catch (Exception exp)
            {
                throw exp;
            }
           
        }
        private void buttonMenuAttendanceRatio_Click(object sender, EventArgs e)
        {
            ButtonsUtility.Menu(0);
            this.Close();
        }

        private void buttonLogoutAttendanceRatio_Click(object sender, EventArgs e)
        {
            ButtonsUtility.NavigateToLogin();
            this.Close();
        }

        private void buttonExitAttendanceRatio_Click(object sender, EventArgs e)
        {
            ButtonsUtility.ExitProgram();
        }

        private void comboBoxMonths_SelectedIndexChanged(object sender, EventArgs e)
        {
            Month_Id = ButtonsUtility.GetMonthId(comboBoxMonths.Text);


            if (Month_Id > 0)
            {
                DataTable dt = new DataTable();
                dt= ButtonsUtility.GetMonthTargetAttendanceRatio(Month_Id, dateTimePickerAttndRatio.Value.Year);
                decimal MonthTarget = Convert.ToDecimal(dt.Rows[0][0]);
                int noOfWorkingdays = Convert.ToInt32(dt.Rows[0][1]);
                    
                GetGridCalculations(dateTimePickerAttndRatio.Value.Year);
                if (MonthTarget > 0)
                {
                    textBoxTarget.Text = MonthTarget.ToString();
                    textBoxWorkingDays.Text = noOfWorkingdays.ToString();
                    buttonSaveTarget.Visible = false;
                    buttonEdit.Visible = true;
                }
                else
                {
                    buttonSaveTarget.Visible = true;
                    buttonEdit.Visible = false;
                }

            }
        }

        private void checkTarget_CheckedChanged(object sender, EventArgs e)
        {
            if (checkTarget.Checked == true)
            {
                textBoxTarget.ReadOnly = false;
                textBoxWorkingDays.ReadOnly = false;
            }
            else
            {
                textBoxTarget.ReadOnly = true;
                textBoxWorkingDays.ReadOnly = true;
            }
        }

        private void textBoxTarget_TextChanged(object sender, EventArgs e)
        {
            labelMissing.Visible = false;
            buttonSaveTarget.Enabled = true;
        }

        private void textBoxTarget_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 46 && textBoxTarget.Text.IndexOf('.') != -1)//check if user has entered more than one dot .
            {
                e.Handled = true;
                return;
            }
            if ((!char.IsNumber(e.KeyChar)) && (!char.IsControl(e.KeyChar)) && (e.KeyChar != 46))  //46 is ASCCI Code for dot "." 
            {

                e.Handled = true;
            }
        }

        private void buttonSaveTarget_Click(object sender, EventArgs e)
        {
            if(textBoxWorkingDays.Text!="" && textBoxTarget.Text!="")
            {
                int row = ButtonsUtility.InsertTargetAttendanceRatio(Month_Id, Convert.ToDecimal(textBoxTarget.Text), Convert.ToInt32(textBoxWorkingDays.Text), dateTimePickerAttndRatio.Value.Year);

            if (row > 0)
            {
                labelTargetSaved.Visible = true;
                buttonEdit.Visible = true;
                buttonSaveTarget.Visible = false;
                GetGridCalculations(dateTimePickerAttndRatio.Value.Year);
                this.Refresh();
            }
            }
            else
            {
                labelMissing.Visible = true;
            }
        
        }

        private void textBoxWorkingDays_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 46 && textBoxTarget.Text.IndexOf('.') != -1)//check if user has entered more than one dot .
            {
                e.Handled = true;
                return;
            }
            if ((!char.IsNumber(e.KeyChar)) && (!char.IsControl(e.KeyChar)) && (e.KeyChar != 46))  //46 is ASCCI Code for dot "." 
            {

                e.Handled = true;
            }
            if (e.KeyChar == 13 && buttonSaveTarget.Visible == true)
            {
                buttonSaveTarget.PerformClick();
            }
            else if (e.KeyChar == 13 && buttonSaveTarget.Visible == false)
            {
                buttonEdit.PerformClick();
            }
        }

        private void textBoxWorkingDays_TextChanged(object sender, EventArgs e)
        {
            labelMissing.Visible = false;
        }

        private void buttonEdit_Click(object sender, EventArgs e)
        {
            int row = ButtonsUtility.EditTargetAttendanceRatio(Convert.ToInt32(textBoxWorkingDays.Text), Month_Id, Convert.ToDecimal(textBoxTarget.Text), dateTimePickerAttndRatio.Value.Year);

            if (row > 0)
            {
                labelTargetSaved.Visible = true;
                GetGridCalculations(dateTimePickerAttndRatio.Value.Year);
            }
        }

        private void dataGridViewObsAttendanceRatio_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            DataGridViewRow dgvObsRow = dataGridViewObsAttendanceRatio.CurrentRow;
            int rowindex = dataGridViewObsAttendanceRatio.CurrentCell.RowIndex;
            ButtonsUtility.EditCustomerAttendanceRatioObservation(dgvObsRow.Cells[1].Value == DBNull.Value ? "" : dgvObsRow.Cells[1].Value.ToString(), rowindex + 1, 0);
            GetObervationActivities();   
        }

        private void dataGridViewAcitvityAttendanceRatio_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            DataGridViewRow dgvObsRow = dataGridViewAcitvityAttendanceRatio.CurrentRow;
            int rowindex = dataGridViewAcitvityAttendanceRatio.CurrentCell.RowIndex;
            ButtonsUtility.EditCustomerAttendanceRatioObservation(dgvObsRow.Cells[1].Value == DBNull.Value ? "" : dgvObsRow.Cells[1].Value.ToString(), rowindex + 1, 1);
            GetObervationActivities();   
        }

        private void buttonBackAttendanceRatio_Click(object sender, EventArgs e)
        {
            ButtonsUtility.Menu(0);
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

        private void dateTimePickerAttndRatio_ValueChanged(object sender, EventArgs e)
        {
            GetGridCalculations(dateTimePickerAttndRatio.Value.Year);
        }

    }
}
