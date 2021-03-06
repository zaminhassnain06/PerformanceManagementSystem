﻿using System;
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
    public partial class AddNewDieAndMolds : Form
    {
        private int Mold_Id=0;
        private int Item_Id=0;
        private string Mold_Code="";
    
        public AddNewDieAndMolds(int moldId)
        {
            InitializeComponent();
            
            if (moldId > 0)
            {
                Mold_Id = moldId;
                InitializeDropDownItemValues();
              //  GetMoldData(moldId);
                buttonEditDailyEntry.Visible = true;
                labelDie.Visible = true;
                comboBoxDie.Visible = true;
                buttonDelete.Visible = true;
         
               

               
            }
            else
            { 
           
                InitializeDropDownItemValues();
            }
          
        }

        private void GetMoldData(int moldId)
        {
            DataTable dt = new DataTable();
            dt = ButtonsUtility.GetDieAndMoldDataToEdit(moldId);
            if(dt.Rows.Count>0)
            {
                textBoxName.Text = dt.Rows[0][0].ToString();
                textBoxCode.Text = dt.Rows[0][1].ToString();
                textBoxPlannedShots.Text = dt.Rows[0][2].ToString();
                comboBoxItems.Text = dt.Rows[0][3].ToString();
                buttonDailyEntrySave.Visible = false;
               
            }

            Mold_Code = textBoxCode.Text;
        }
        private void InitializeDropDownItemValues()
        {

            DataTable dt = new DataTable();
            dt = ButtonsUtility.InitializeItemDropDown();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                comboBoxItems.Items.Add(dt.Rows[i][0]);

            }
        }
        private void InitializeComboBoxDie(int Item_Id)
        {
            DataTable dt = new DataTable();
            dt = ButtonsUtility.InitializeItemDropDownDieAndMolds(Item_Id);
            string DieCode = "";
            string DieName = "";
            string DieNameCode = "";
            comboBoxDie.Items.Clear();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                DieName =  dt.Rows[i][0].ToString();
                DieCode = dt.Rows[i][1].ToString();
                DieNameCode = DieCode + " - " + DieName;
                comboBoxDie.Items.Add(DieNameCode);

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

       


     

        private void buttonDailyEntrySave_Click(object sender, EventArgs e)
        {
            
            
            if(textBoxCode.Text==""|| textBoxName.Text=="" ||textBoxPlannedShots.Text=="")
            {
                labelRequiredFileds.Visible = true;
            }
            else if(comboBoxItems.Text=="")
            {
                labelPleaseSelectItem.Visible = true;
            }
            else
            { 
                labelRequiredFileds.Visible =false;
                labelPleaseSelectItem.Visible = false;


            
                int noofRecordsAffected = ButtonsUtility.SaveDieAndMold(Item_Id,textBoxName.Text,textBoxCode.Text, Convert.ToInt32(textBoxPlannedShots.Text));

                if (noofRecordsAffected > 0)
                {
                    labelRecordupdatedSucessfully.Text = "Record entered sucessfully.";
                    labelRecordupdatedSucessfully.Visible = true;
                    buttonEditDailyEntry.Visible = true;
                    buttonDelete.Visible = true;
                    labelDie.Visible = true;
                    comboBoxDie.Visible = true;
                    InitializeComboBoxDie(Item_Id);
                }
                else
                {
                    Mold_Code = textBoxCode.Text;

                    labelAlreadyExist.Visible = true;
                    buttonEditDailyEntry.Visible = true;
                    buttonDelete.Visible = true;
                    
                }
            }
            
            
             
        }

       
        private void TextBoxesCalculatedValuesofItemSelectedbyCombo(string itemName)
        {
            Item_Id= ButtonsUtility.GetITemId(itemName);
           
            if(comboBoxDie.Visible==true)
            {
                InitializeComboBoxDie(Item_Id);
            }


                labelRecordupdatedSucessfully.Visible = false;
               labelPleaseSelectItem.Visible=false;

        }

        

        

        private void buttonEditDailyEntry_Click(object sender, EventArgs e)
        {

            if (textBoxCode.Text == "" || textBoxName.Text == "" || textBoxPlannedShots.Text == "")
            {
                labelRequiredFileds.Visible = true;
            }
            else if (comboBoxItems.Text == "")
            {
                labelPleaseSelectItem.Visible = true;
            }
            else{


                int noofRecordsAffected = ButtonsUtility.EditMoldsAndDie(Item_Id, textBoxName.Text, textBoxCode.Text, Convert.ToInt32(textBoxPlannedShots.Text), Mold_Code);
           
            
                if (noofRecordsAffected > 0)
            
                {

                    Mold_Code = textBoxCode.Text;

                    labelRecordupdatedSucessfully.Text = "Record edited sucessfully.";
               
                    labelRecordupdatedSucessfully.Visible = true;
                    InitializeComboBoxDie(Item_Id);
                  
                }
                else
                {
                    labelCodeExist.Visible = true;
                }
               
            }
        }

      

        private void textBoxPlannedShots_KeyPress(object sender, KeyPressEventArgs e)
        {
            if ((!char.IsNumber(e.KeyChar)) && (!char.IsControl(e.KeyChar)))
            {

                e.Handled = true;
            }
            if (e.KeyChar == 13 && buttonDailyEntrySave.Visible==true)
            {
                buttonDailyEntrySave.PerformClick();
            }
            else if(e.KeyChar==13 && buttonDailyEntrySave.Visible==false)
            {
                buttonEditDailyEntry.PerformClick();
            }
        }

        private void textBoxName_TextChanged(object sender, EventArgs e)
        {
            labelRecordupdatedSucessfully.Visible =false;
            labelRequiredFileds.Visible = false;
            labelAlreadyExist.Visible = false;

        }

        private void textBoxCode_TextChanged(object sender, EventArgs e)
        {
            labelRecordupdatedSucessfully.Visible = false;
            labelRequiredFileds.Visible = false;
            labelAlreadyExist.Visible = false;
        }

        private void textBoxPlannedShots_TextChanged(object sender, EventArgs e)
        {
            labelRecordupdatedSucessfully.Visible = false;
            labelRequiredFileds.Visible = false;
            labelCodeExist.Visible = false;
            labelAlreadyExist.Visible = false;
            
        }

        private void comboBoxItems_SelectedIndexChanged(object sender, EventArgs e)
        {
            TextBoxesCalculatedValuesofItemSelectedbyCombo(comboBoxItems.Text);
            if(comboBoxDie.Visible ==true)
            {
                labelPleaseSelectdie.Visible = true;
            }
            labelRecordupdatedSucessfully.Visible = false;
            labelRequiredFileds.Visible = false;
            labelAlreadyExist.Visible = false;
            labelCodeExist.Visible = false;
        }

        private void labelRequiredFileds_Click(object sender, EventArgs e)
        {

        }

        private void labelRecordupdatedSucessfully_Click(object sender, EventArgs e)
        {

        }

        private void comboBoxDie_SelectedIndexChanged(object sender, EventArgs e)
        {
            labelPleaseSelectdie.Visible = false;
            labelRecordupdatedSucessfully.Visible = false;
            labelRequiredFileds.Visible = false;
            labelAlreadyExist.Visible = false;
            labelCodeExist.Visible = false;

            string input = comboBoxDie.Text;
            Mold_Code = input.Remove(input.IndexOf("-"));

            DataTable dt = new DataTable();
            dt = ButtonsUtility.GetDieAndMoldDataToEditByCode(Mold_Code,Item_Id);
            if (dt.Rows.Count > 0)
            {
                textBoxName.Text = dt.Rows[0][0].ToString();
                textBoxCode.Text = dt.Rows[0][1].ToString();
                textBoxPlannedShots.Text = dt.Rows[0][2].ToString();
                comboBoxItems.Text = dt.Rows[0][3].ToString();
               

            }
        }

        private void buttonDelete_Click(object sender, EventArgs e)
        {
            if (comboBoxDie.Text == "")
            {
                labelPleaseSelectdie.Visible = true;
            }
            else
            {
                int rows = ButtonsUtility.DeleteDieAndMold(Mold_Code);

                if (rows > 0)
                {

                    textBoxName.Clear();
                    textBoxCode.Clear();
                    textBoxPlannedShots.Clear();

                    comboBoxItems.Text = "";
                    comboBoxDie.Text = "";

                    InitializeDropDownItemValues();

                    InitializeComboBoxDie(Item_Id);

                    labelRecordupdatedSucessfully.Text = "Record deleted sucessfully.";

                    labelRecordupdatedSucessfully.Visible = true;


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

        private void CostOfReworkRatioToolStripMenuItem_Click(object sender, EventArgs e)
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


      

       
      

        
        
    }
}
