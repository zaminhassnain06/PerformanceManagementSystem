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
    public partial class CostOfCustomerRejectionKPI : Form
    {
        private int Item_Id = 0;
        private int Month_Id = 0;
       
        public CostOfCustomerRejectionKPI(int itemId)
        {
            InitializeComponent();
            if (itemId > 0)
            {
                labelItemNameCostOfRejectionKPI.Text = ButtonsUtility.ItemNameUtility(itemId);
                labelItemNameCostOfRejectionKPI.Visible = true;
                Item_Id = itemId;
                labelSelectItemDailyEntry.Visible = false;
                comboBoxItemsReworkRatioKPI.Visible = false;
                labelPleaseSelectItem.Visible = false;
                GetGridCalculations();
                GetObervationActivities();
           

            }
            else if (itemId == 0)
            {
                labelItemNameCostOfRejectionKPI.Visible = false;
                labelSelectItemDailyEntry.Visible = true;
                comboBoxItemsReworkRatioKPI.Visible = true;
                labelPleaseSelectItem.Visible = true;
                InitializeDropDownItemValues();
            }
            GetMonths();
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

        private void InitializeDropDownItemValues()
        {

            DataTable dt = new DataTable();
            dt = ButtonsUtility.InitializeItemDropDown();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                comboBoxItemsReworkRatioKPI.Items.Add(dt.Rows[i][0]);

            }
        }
        private void GetGridCalculations()
        {
            DataTable dt = new DataTable();
            dt = ButtonsUtility.GetCalculationsCustRejectionCostMonthly(Item_Id,dateTimePickerCostCustRej.Value.Year);

            int rows = dt.Rows.Count;

            try
            {
                dataGridViewCalCostOfRejectionKPI.Rows.Clear();

                for (int i = 0; i < rows; i++)
                {

                    dataGridViewCalCostOfRejectionKPI.Rows.Add(dt.Rows[i][0], dt.Rows[i][1], dt.Rows[i][2], dt.Rows[i][3]);


                    if (dt.Rows[i][3].ToString() == "Achieved")
                    {

                        dataGridViewCalCostOfRejectionKPI.Rows[i].Cells[3].Style.BackColor = Color.LightGreen;
                        dataGridViewCalCostOfRejectionKPI.Rows[i].Cells[3].Style.ForeColor = Color.Black;


                    }
                    else
                    {


                        dataGridViewCalCostOfRejectionKPI.Rows[i].Cells[3].Style.BackColor = Color.IndianRed;
                        dataGridViewCalCostOfRejectionKPI.Rows[i].Cells[3].Style.ForeColor = Color.Black;
                    }

                }



                if (chartCostOfRejectionKPI.Series.IndexOf("Cost Achieved") != -1)
                {
                    chartCostOfRejectionKPI.Series.Clear();
                }
                if (chartCostOfRejectionKPI.Series.IndexOf("Cost Not Achieved") != -1)
                {
                    chartCostOfRejectionKPI.Series.Clear();
                }
                chartCostOfRejectionKPI.Series.Add("Cost Achieved");
                chartCostOfRejectionKPI.Series.Add("Cost Not Achieved");

                chartCostOfRejectionKPI.Series["Cost Achieved"].Color = Color.LightGreen;
                chartCostOfRejectionKPI.Series["Cost Not Achieved"].Color = Color.IndianRed;
                for (int i = 0; i < rows; i++)
                {
                    if (dt.Rows[i][3].ToString() == "Achieved")
                    {

                        chartCostOfRejectionKPI.Series["Cost Achieved"].Points.AddXY(dt.Rows[i][0].ToString(), dt.Rows[i][1]);
                        chartCostOfRejectionKPI.Series["Cost Not Achieved"].Points.AddXY(dt.Rows[i][0].ToString(), 0);
                        chartCostOfRejectionKPI.Series["Cost Achieved"].Points[i].IsValueShownAsLabel = true;
                        chartCostOfRejectionKPI.Series["Cost Not Achieved"].Points[i].IsValueShownAsLabel = false;
                    }
                    else
                    {
                        chartCostOfRejectionKPI.Series["Cost Achieved"].Points.AddXY(dt.Rows[i][0].ToString(), 0);
                        chartCostOfRejectionKPI.Series["Cost Not Achieved"].Points.AddXY(dt.Rows[i][0].ToString(), dt.Rows[i][1]);
                        chartCostOfRejectionKPI.Series["Cost Achieved"].Points[i].IsValueShownAsLabel = false;
                        chartCostOfRejectionKPI.Series["Cost Not Achieved"].Points[i].IsValueShownAsLabel = true;

                    }


                }
                chartCostOfRejectionKPI.ChartAreas[0].AxisX.Interval = 1;


                chartCostOfRejectionKPI.Series[0].IsVisibleInLegend = false;

            }
            catch (Exception exp)
            {
                throw exp;
            }
        }

        private void buttonMenuCostOfRejectionKPI_Click(object sender, EventArgs e)
        {
            ButtonsUtility.Menu(Item_Id);
            this.Close();
        }

        private void buttonLogoutCostOfRejectionKPI_Click(object sender, EventArgs e)
        {
            ButtonsUtility.NavigateToLogin();
            this.Close();
        }

        private void buttonExitCostOfRejectionKPI_Click(object sender, EventArgs e)
        {
            ButtonsUtility.ExitProgram();
        }

        private void comboBoxItemsReworkRatioKPI_SelectedIndexChanged(object sender, EventArgs e)
        {
            Item_Id = ButtonsUtility.GetITemId(comboBoxItemsReworkRatioKPI.Text);
            //if (comboBoxMonths.Text != "")
            //{
            //    GetGridCalculations();

                
            //}
            checkTarget.Checked = false;
            labelItemNameCostOfRejectionKPI.Text = comboBoxItemsReworkRatioKPI.Text;
            labelItemNameCostOfRejectionKPI.Visible = true;
            labelPleaseSelectItem.Visible = false;
            if (Item_Id > 0)
            {
                ButtonsUtility.SaveMonthsforObservationsActivitiesCustomerRejectionCost(Item_Id);

                GetGridCalculations();
                GetObervationActivities();
            }
            if (Item_Id > 0 && Month_Id > 0)
            {
                decimal MonthTarget = ButtonsUtility.GetMonthTargetCostofCustRejection(Month_Id, Item_Id, dateTimePickerCostCustRej.Value.Year);


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

        private void comboBoxMonths_SelectedIndexChanged(object sender, EventArgs e)
        {
            Month_Id = ButtonsUtility.GetMonthId(comboBoxMonths.Text);


            if (Item_Id > 0)
            {
                decimal MonthTarget = ButtonsUtility.GetMonthTargetCostofCustRejection(Month_Id, Item_Id, dateTimePickerCostCustRej.Value.Year);

              //  GetGridCalculations();
                if (MonthTarget >= 0)
                {
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

        private void checkTarget_CheckedChanged(object sender, EventArgs e)
        {
            if (checkTarget.Checked == true && Item_Id > 0)
            {
                textBoxTarget.ReadOnly = false;
            }
            else
            {
                textBoxTarget.ReadOnly = true;
            }
        }

        private void textBoxTarget_TextChanged(object sender, EventArgs e)
        {
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
            if (e.KeyChar == 13 && buttonSaveTarget.Visible == true)
            {
                buttonSaveTarget.PerformClick();
            }
            else if (e.KeyChar == 13 && buttonSaveTarget.Visible == false)
            {
                buttonEdit.PerformClick();
            }
        }

        private void buttonSaveTarget_Click(object sender, EventArgs e)
        {
            int row = ButtonsUtility.InsertTargetCustRejcectionkCost(Item_Id, Month_Id, Convert.ToDecimal(textBoxTarget.Text), dateTimePickerCostCustRej.Value.Year);

            if (row > 0)
            {
                labelTargetSaved.Visible = true;
                buttonEdit.Visible = true;
                buttonSaveTarget.Visible = false;
                GetGridCalculations();
                this.Refresh();
            }
        }

        private void buttonEdit_Click(object sender, EventArgs e)
        {
            int row = ButtonsUtility.EditTargetCustRejCost(Item_Id, Month_Id, Convert.ToDecimal(textBoxTarget.Text), dateTimePickerCostCustRej.Value.Year);

            if (row > 0)
            {
                labelTargetSaved.Visible = true;
                GetGridCalculations();
            }
        }

        private void GetObervationActivities()
        {

            DataSet ds = new DataSet("TimeRanges");
            ds = ButtonsUtility.GetActivitesObservationCustomerRejectionCost(Item_Id);
            dataGridViewCostOfRejectionKPIObs.DataSource = ds.Tables[0];
            dataGridViewCostOfRejectionKPIObs.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

            dataGridViewAcitvityCostOfRejectionKPI.DataSource = ds.Tables[1];
            dataGridViewAcitvityCostOfRejectionKPI.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
        }

        private void dataGridViewCostOfRejectionKPIObs_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            DataGridViewRow dgvObsRow = dataGridViewCostOfRejectionKPIObs.CurrentRow;
            int rowindex = dataGridViewCostOfRejectionKPIObs.CurrentCell.RowIndex;
            ButtonsUtility.EditCustomerRejectionCostObservation(Item_Id, dgvObsRow.Cells[1].Value == DBNull.Value ? "" : dgvObsRow.Cells[1].Value.ToString(), rowindex + 1, 0);
            GetObervationActivities();   
        }

        private void dataGridViewAcitvityCostOfRejectionKPI_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            DataGridViewRow dgvObsRow = dataGridViewAcitvityCostOfRejectionKPI.CurrentRow;
            int rowindex = dataGridViewAcitvityCostOfRejectionKPI.CurrentCell.RowIndex;
            ButtonsUtility.EditCustomerRejectionCostObservation(Item_Id, dgvObsRow.Cells[1].Value == DBNull.Value ? "" : dgvObsRow.Cells[1].Value.ToString(), rowindex + 1, 1);
            GetObervationActivities(); 
        }

        private void buttonBackCostOfRejectionKPI_Click(object sender, EventArgs e)
        {
            //if (Back == 0)
            //{
            //    OverAll_KPIs KPI = new OverAll_KPIs(0);
            //    KPI.Show();
            //    this.Close();

            //}
            //else
            //{
                ButtonsUtility.Menu(Item_Id);
                this.Close();
            //}
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

        private void costofReworkRejectionRatioToolStripMenuItem_Click(object sender, EventArgs e)
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

        private void attendanceDetailsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            GetAttendanceDetails AttendacneDetails = new GetAttendanceDetails();
            AttendacneDetails.Show();
            this.Close();
        }

        private void markAttendanceToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Worker_Details workerdetails = new Worker_Details();
            workerdetails.Show();
            this.Close();
        }

        private void dateTimePickerCostCustRej_ValueChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(comboBoxItemsReworkRatioKPI.Text))
            {
                Item_Id = ButtonsUtility.GetITemId(comboBoxItemsReworkRatioKPI.Text);
                //if (comboBoxMonths.Text != "")
                //{
                //    GetGridCalculations();


                //}
                checkTarget.Checked = false;
                labelItemNameCostOfRejectionKPI.Text = comboBoxItemsReworkRatioKPI.Text;
                labelItemNameCostOfRejectionKPI.Visible = true;
                labelPleaseSelectItem.Visible = false;
                if (Item_Id > 0)
                {
                    ButtonsUtility.SaveMonthsforObservationsActivitiesCustomerRejectionCost(Item_Id);

                    GetGridCalculations();
                    GetObervationActivities();
                }
                if (Item_Id > 0 && Month_Id > 0)
                {
                    decimal MonthTarget = ButtonsUtility.GetMonthTargetCostofCustRejection(Month_Id, Item_Id, dateTimePickerCostCustRej.Value.Year);


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
        //private void costofReworkRejectionRatioToolStripMenuItem_Click(object sender, EventArgs e)
        //{
        //    CostofRework CR = new CostofRework(0);
        //    CR.Show();
        //    this.Close();

        //}

    }
}
