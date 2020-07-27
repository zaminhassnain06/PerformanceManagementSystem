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
    public partial class EquipFailureRateKPI : Form
    {

        private int WorkShop_Id = 0;
        private int Month_Id = 0;
     
        public EquipFailureRateKPI()
        {
            InitializeComponent();

            InitializeDropDownWorkShop();

            PopulateObservations();

            GetMonths();
        }
        private void PopulateObservations()
        {
            DataTable dt = new DataTable();

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
        private void InitializeDropDownWorkShop()
        {
            DataTable dt = new DataTable();
            dt = ButtonsUtility.ViewShops();


           
                comboBoxShop.Items.Clear();
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    comboBoxShop.Items.Add(dt.Rows[i][0]);

                }
            
        }

        private void buttonMenuEquipFailureRateKPI_Click(object sender, EventArgs e)
        {
            ButtonsUtility.Menu(0);
            this.Close();
        }

        private void buttonLogoutEquipFailureRateKPI_Click(object sender, EventArgs e)
        {
            ButtonsUtility.NavigateToLogin();
            this.Close();
        }

        private void buttonExitEquipFailureRateKPI_Click(object sender, EventArgs e)
        {
            ButtonsUtility.ExitProgram();
        }

        private void buttonEditEqpFailRate_Click(object sender, EventArgs e)
        {
            DailyEntry_Machine dailtEntryMachine = new DailyEntry_Machine(0);
            dailtEntryMachine.Show();
            this.Close();


        }

        private void EquipFailureRateKPI_Load(object sender, EventArgs e)
        {

        }

        private void comboBoxShop_SelectedIndexChanged(object sender, EventArgs e)
        {          
            WorkShop_Id = ButtonsUtility.GetWorkShopId(comboBoxShop.Text);
           
            checkTarget.Checked = false;
            labelWorkShopnameKPI.Text = comboBoxShop.Text;
            labelWorkShopnameKPI.Visible = true;
            labelPleaseSelectShop.Visible = false;
            if (WorkShop_Id > 0)
            {
                ButtonsUtility.SaveMonthsforObservationsActivitiesEquipFailRate(WorkShop_Id);
                GetGridCalculations();
                GetObervationActivities();
            }
            if (WorkShop_Id > 0 && Month_Id > 0)
            {
                DataTable dt = new DataTable();
                dt = ButtonsUtility.GetMonthTargetEquipFailureRateShopMonthly(Month_Id, WorkShop_Id,dateTimePickerEquipFailRate.Value.Year);
                decimal MonthTarget = Convert.ToDecimal(dt.Rows[0][0]);


                if (MonthTarget >= 0)
                {
                    GetGridCalculations();
                    textBoxTarget.Text = MonthTarget.ToString();
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

        private void GetGridCalculations()
        {
            DataTable dt = new DataTable();
            dt = ButtonsUtility.GetCalculationsEquipmentFailureRateMonthlyWorkshop(WorkShop_Id,dateTimePickerEquipFailRate.Value.Year);
            int rows = dt.Rows.Count;
            try
            {
                dataGridViewCalcEquipFailureRateKPI.Rows.Clear();

                for (int i = 0; i < rows; i++)
                {

                    dataGridViewCalcEquipFailureRateKPI.Rows.Add(dt.Rows[i][0], dt.Rows[i][1], dt.Rows[i][2], dt.Rows[i][3], dt.Rows[i][4], dt.Rows[i][5]);


                    if (dt.Rows[i][5].ToString() == "Achieved")
                    {

                        dataGridViewCalcEquipFailureRateKPI.Rows[i].Cells[5].Style.BackColor = Color.LightGreen;
                        dataGridViewCalcEquipFailureRateKPI.Rows[i].Cells[5].Style.ForeColor = Color.Black;


                    }
                    else
                    {


                        dataGridViewCalcEquipFailureRateKPI.Rows[i].Cells[5].Style.BackColor = Color.IndianRed;
                        dataGridViewCalcEquipFailureRateKPI.Rows[i].Cells[5].Style.ForeColor = Color.Black;
                    }

                }



                if (chartEquipFailureRateKPI.Series.IndexOf("Equipment Failure Rate Achieved %") != -1)
                {
                    chartEquipFailureRateKPI.Series.Clear();
                }
                if (chartEquipFailureRateKPI.Series.IndexOf("Equipment Failure Rate Not Achieved %") != -1)
                {
                    chartEquipFailureRateKPI.Series.Clear();
                }
                chartEquipFailureRateKPI.Series.Add("Equipment Failure Rate Achieved %");
                chartEquipFailureRateKPI.Series.Add("Equipment Failure Rate Not Achieved %");

                chartEquipFailureRateKPI.Series["Equipment Failure Rate Achieved %"].Color = Color.LightGreen;
                chartEquipFailureRateKPI.Series["Equipment Failure Rate Not Achieved %"].Color = Color.IndianRed;
                for (int i = 0; i < rows; i++)
                {
                    if (dt.Rows[i][5].ToString() == "Achieved")
                    {

                        chartEquipFailureRateKPI.Series["Equipment Failure Rate Achieved %"].Points.AddXY(dt.Rows[i][0].ToString(), dt.Rows[i][3]);
                        chartEquipFailureRateKPI.Series["Equipment Failure Rate Not Achieved %"].Points.AddXY(dt.Rows[i][0].ToString(), 0);
                        chartEquipFailureRateKPI.Series["Equipment Failure Rate Achieved %"].Points[i].IsValueShownAsLabel = true;
                        chartEquipFailureRateKPI.Series["Equipment Failure Rate Not Achieved %"].Points[i].IsValueShownAsLabel = false;
                    }
                    else
                    {
                        chartEquipFailureRateKPI.Series["Equipment Failure Rate Achieved %"].Points.AddXY(dt.Rows[i][0].ToString(), 0);
                        chartEquipFailureRateKPI.Series["Equipment Failure Rate Not Achieved %"].Points.AddXY(dt.Rows[i][0].ToString(), dt.Rows[i][3]);
                        chartEquipFailureRateKPI.Series["Equipment Failure Rate Achieved %"].Points[i].IsValueShownAsLabel = false;
                        chartEquipFailureRateKPI.Series["Equipment Failure Rate Not Achieved %"].Points[i].IsValueShownAsLabel = true;

                    }


                }

                chartEquipFailureRateKPI.ChartAreas[0].AxisX.Interval = 1;


                chartEquipFailureRateKPI.Series[0].IsVisibleInLegend = false;

            }
            catch (Exception exp)
            {
                throw exp;
            }
        }

        private void buttonSaveTarget_Click(object sender, EventArgs e)
        {
            int row = ButtonsUtility.InsertTargetEquipmentFailureWorkShop(WorkShop_Id, Month_Id, Convert.ToDecimal(textBoxTarget.Text), Convert.ToInt32(textBoxNoOfDays.Text),dateTimePickerEquipFailRate.Value.Year);

            if (row > 0)
            {
                labelTargetSaved.Visible = true;
                buttonEdit.Visible = true;
                buttonSaveTarget.Visible = false;
                GetGridCalculations();
                this.Refresh();
            }
        }

        private void checkTarget_CheckedChanged(object sender, EventArgs e)
        {
            if(checkTarget.Checked==true && WorkShop_Id>0)
            {
                textBoxNoOfDays.ReadOnly = false;
                textBoxTarget.ReadOnly = false;
            }
            else
            {
                textBoxTarget.ReadOnly = true;
                textBoxTarget.ReadOnly = true;
            }
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

        private void textBoxTarget_TextChanged(object sender, EventArgs e)
        {
            buttonSaveTarget.Enabled = true;
        }

        private void textBoxNoOfDays_KeyPress(object sender, KeyPressEventArgs e)
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

        private void textBoxNoOfDays_TextChanged(object sender, EventArgs e)
        {
            buttonSaveTarget.Enabled = true;
        }

        private void buttonEdit_Click(object sender, EventArgs e)
        {
            int row = ButtonsUtility.EditTargetEquipFailureRateMonethWorkShop(WorkShop_Id, Month_Id, Convert.ToDecimal(textBoxTarget.Text), Convert.ToInt32(textBoxNoOfDays.Text), dateTimePickerEquipFailRate.Value.Year);

            if (row > 0)
            {
                labelTargetSaved.Visible = true;
                GetGridCalculations();
            }
        }
        private void GetObervationActivities()
        {

            DataSet ds = new DataSet("TimeRanges");
            ds = ButtonsUtility.GetActivitesObservationEquipFailRateMonthlyWorkShop(WorkShop_Id);
            dataGridViewObsEquipFailureRateKPI.DataSource = ds.Tables[0];
            dataGridViewObsEquipFailureRateKPI.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

            dataGridViewEquipFailureRateKPI.DataSource = ds.Tables[1];
            dataGridViewEquipFailureRateKPI.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
        }

        private void dataGridViewObsEquipFailureRateKPI_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            DataGridViewRow dgvObsRow = dataGridViewObsEquipFailureRateKPI.CurrentRow;
            int rowindex = dataGridViewObsEquipFailureRateKPI.CurrentCell.RowIndex;
            ButtonsUtility.EditEquipFailureRateShopMonthlyObservation(WorkShop_Id, dgvObsRow.Cells[1].Value == DBNull.Value ? "" : dgvObsRow.Cells[1].Value.ToString(), rowindex + 1, 0);
            GetObervationActivities();     
        }

        private void dataGridViewEquipFailureRateKPI_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            DataGridViewRow dgvObsRow = dataGridViewEquipFailureRateKPI.CurrentRow;
            int rowindex = dataGridViewEquipFailureRateKPI.CurrentCell.RowIndex;
            ButtonsUtility.EditEquipFailureRateShopMonthlyObservation(WorkShop_Id, dgvObsRow.Cells[1].Value == DBNull.Value ? "" : dgvObsRow.Cells[1].Value.ToString(), rowindex + 1, 1);
            GetObervationActivities();   
        }

        private void comboBoxMonths_SelectedIndexChanged(object sender, EventArgs e)
        {
            Month_Id = ButtonsUtility.GetMonthId(comboBoxMonths.Text);

            if (WorkShop_Id > 0)
            {
                DataTable dt = new DataTable();
                dt = ButtonsUtility.GetMonthTargetEquipFailureRateShopMonthly(Month_Id, WorkShop_Id,dateTimePickerEquipFailRate.Value.Year);
                decimal MonthTarget = Convert.ToDecimal(dt.Rows[0][0]);

               // GetGridCalculations();
                //string someString = dataGridViewReworkRatioCalculatios[5, 1].Value.ToString();
                if (MonthTarget >= 0)
                {
                    textBoxTarget.Text = MonthTarget.ToString();
                    textBoxNoOfDays.Text=dt.Rows[0][1].ToString();
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

        private void buttonBackEquipFailureRateKPI_Click(object sender, EventArgs e)
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

        private void dateTimePickerEquipFailRate_ValueChanged(object sender, EventArgs e)
        {
            if(!string.IsNullOrEmpty(comboBoxShop.Text))
            {
                WorkShop_Id = ButtonsUtility.GetWorkShopId(comboBoxShop.Text);

                checkTarget.Checked = false;
                labelWorkShopnameKPI.Text = comboBoxShop.Text;
                labelWorkShopnameKPI.Visible = true;
                labelPleaseSelectShop.Visible = false;
                if (WorkShop_Id > 0)
                {
                    ButtonsUtility.SaveMonthsforObservationsActivitiesEquipFailRate(WorkShop_Id);
                    GetGridCalculations();
                    GetObervationActivities();
                }
                if (WorkShop_Id > 0 && Month_Id > 0)
                {
                    DataTable dt = new DataTable();
                    dt = ButtonsUtility.GetMonthTargetEquipFailureRateShopMonthly(Month_Id, WorkShop_Id, dateTimePickerEquipFailRate.Value.Year);
                    decimal MonthTarget = Convert.ToDecimal(dt.Rows[0][0]);


                    if (MonthTarget >= 0)
                    {
                        GetGridCalculations();
                        textBoxTarget.Text = MonthTarget.ToString();
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
        }
    }
}
