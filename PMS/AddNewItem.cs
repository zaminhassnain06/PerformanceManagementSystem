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
    public partial class AddNewItem : Form
    {
        private int Item_Id = 0;
        public AddNewItem(int ItemId)
        {
            InitializeComponent();
            if(ItemId!=0)   //greater than 0 means we  will edit item
            {
               // FetchItemDetails(ItemId);
                
                buttonCreateNewItemAddNewItems.Visible = false;
                buttonEditItem.Visible = true;
                //buttonEditProcess.Visible = true;
                //buttonAddProcess.Visible = true;
                //buttonMachine.Visible = true;
                //buttonAddMaterial.Visible = true;
                //buttonEditMaterial.Visible = true;
                InitializeDropDownItemValues();
                comboBoxItems.Visible = true;
                labelSelectItemDailyEntry.Visible = true;
                labelPleaseSelectItem.Visible = true;
            }
            labelRework.Visible = true;
        }

        private void InitializeDropDownItemValues()
        {

            DataTable dt = new DataTable();
            dt = ButtonsUtility.InitializeItemDropDown();
            comboBoxItems.Items.Clear();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                comboBoxItems.Items.Add(dt.Rows[i][0]);

            }
        }
        private void buttonExitAddNewItem_Click(object sender, EventArgs e)
        {
            ButtonsUtility.ExitProgram();
        }

        private void buttonLogoutAddNewItem_Click(object sender, EventArgs e)
        {
            this.Close();
            ButtonsUtility.NavigateToLogin();
        }

        private void ButtonMenuAddNewItem_Click(object sender, EventArgs e)
        {
            ButtonsUtility.Menu(Item_Id);
            this.Close();
        }

        private void buttonCreateNewItemAddNewItems_Click(object sender, EventArgs e)
        {
            if (textBoxNameOfItem.Text == "" || textBoxItemCode.Text == "" ||  textBoxUOM.Text == "" || textBoxCostofEachUnit.Text == "" || textBoxRework.Text == "" || textBoxInhouseRej.Text=="")
            {
                labelrequiredFields.Visible = true;
            }
            else if (labelNewItemCreatedSucessfully.Visible==true)
            {
                textBoxNameOfItem.Text = "";
                textBoxItemCode.Text = "";         
                textBoxUOM.Text = "";
            }
            int row = ButtonsUtility.InsertNewItem(textBoxNameOfItem.Text.Trim(), textBoxtemDescription.Text.Trim(), textBoxItemCode.Text.Trim(),  textBoxUOM.Text.Trim(),Convert.ToDecimal(textBoxCostofEachUnit.Text.Trim()), Convert.ToDecimal(textBoxRework.Text.Trim()), Convert.ToDecimal(textBoxInhouseRej.Text.Trim()));
            
            if(row>0)
            { 
            
                buttonAddProcess.Visible = true;
                buttonMachine.Visible = true;
                labelCodeExisits.Visible = false;
                labelNewItemCreatedSucessfully.Visible = true;
                InitializeDropDownItemValues();
                comboBoxItems.Visible = true;
                labelSelectItemDailyEntry.Visible = true;
                labelPleaseSelectItem.Visible = true;
                buttonDelete.Visible = true;
                buttonAddMaterial.Visible = true;
                labelPleaseSelectItem.Visible = false;
            }
            else
            {
                labelCodeExisits.Visible = true;
                
            }
        }


       

        

        private void FetchItemDetails(int ItemId)
        {
            DataTable dt = new DataTable();
            dt = ButtonsUtility.FetchItemDetails(ItemId);

            textBoxNameOfItem.Text = dt.Rows[0][0].ToString();
            textBoxtemDescription.Text = dt.Rows[0][1].ToString();
            textBoxItemCode.Text = dt.Rows[0][2].ToString();
            labelItemCode.Text = dt.Rows[0][2].ToString();
            textBoxUOM.Text = dt.Rows[0][3].ToString();
            textBoxCostofEachUnit.Text = dt.Rows[0][4].ToString();
            textBoxRework.Text = dt.Rows[0][5].ToString();
            textBoxInhouseRej.Text = dt.Rows[0][6].ToString();
            



        }

        private void buttonEditItem_Click(object sender, EventArgs e)
        {
            if (textBoxNameOfItem.Text == "" || textBoxItemCode.Text == ""  || textBoxUOM.Text == "" || textBoxCostofEachUnit.Text == "" || textBoxRework.Text == "" || textBoxInhouseRej.Text == "")
            {
                labelrequiredFields.Visible = true;
            }
            else
            {
                int row = ButtonsUtility.EditItemValues(textBoxNameOfItem.Text.Trim(), textBoxtemDescription.Text.Trim(), textBoxItemCode.Text.Trim(),  textBoxUOM.Text.Trim(), labelCodeOfItem.Text, Convert.ToDecimal(textBoxCostofEachUnit.Text.Trim()), Convert.ToDecimal(textBoxRework.Text.Trim()), Convert.ToDecimal(textBoxInhouseRej.Text.Trim()));

                if (row > 0)
                {
                    labelProductEdit.Visible = true;
                    InitializeDropDownItemValues();
                }
            }
        }

        private void buttonAddProcess_Click(object sender, EventArgs e)
        {
            int itemId = ButtonsUtility.GetITemId(textBoxNameOfItem.Text);
            AddProcess add_process = new AddProcess(itemId,0);
            add_process.Show();
            this.Close();
        }

        private void buttonEditProcess_Click(object sender, EventArgs e)
        {
            int itemId = ButtonsUtility.GetITemId(textBoxNameOfItem.Text);
            AddProcess add_process = new AddProcess(itemId, 1);
            add_process.Show();
            this.Close();
        }

        private void buttonMachine_Click(object sender, EventArgs e)
        {
             int itemId = ButtonsUtility.GetITemId(textBoxNameOfItem.Text);
             AddMachineAssociationWithItem MachineWithItem = new AddMachineAssociationWithItem(itemId);
             MachineWithItem.Show();
             this.Close();
        }

        private void buttonMaterial_Click(object sender, EventArgs e)
        {
            int itemId = ButtonsUtility.GetITemId(textBoxNameOfItem.Text);
            AddMaterial Material = new AddMaterial(itemId,0);
            Material.Show();
            this.Close();
        }

        private void buttonEditMaterial_Click(object sender, EventArgs e)
        {
            int itemId = ButtonsUtility.GetITemId(textBoxNameOfItem.Text);
            AddMaterial Material = new AddMaterial(itemId, 1);
            Material.Show();
            this.Close();
        }

        private void comboBoxItems_SelectedIndexChanged(object sender, EventArgs e)
        {
            Item_Id = ButtonsUtility.GetITemId(comboBoxItems.Text);
            FetchItemDetails(Item_Id);
            if(Item_Id>0)
            {
                buttonEditProcess.Visible = true;
                buttonAddProcess.Visible = true;
                buttonMachine.Visible = true;
                buttonAddMaterial.Visible = true;
                buttonEditMaterial.Visible = true;
                buttonEditItem.Visible=true;
                buttonDelete.Visible = true;
                labelNewItemCreatedSucessfully.Visible = false;
                labelDeletionSucess.Visible = false;
                labelProductEdit.Visible = false;
                labelPleaseSelectItem.Visible = false;
            }
        }

        private void textBoxUOM_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13 && buttonCreateNewItemAddNewItems.Visible == true)
            {
                buttonCreateNewItemAddNewItems.PerformClick();
            }
            else if (e.KeyChar == 13 && buttonCreateNewItemAddNewItems.Visible == false)
            {
                buttonEditItem.PerformClick();
            }
        }

       
        private void textBoxItemCode_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13 && buttonCreateNewItemAddNewItems.Visible == true)
            {
                buttonCreateNewItemAddNewItems.PerformClick();
            }
            else if (e.KeyChar == 13 && buttonCreateNewItemAddNewItems.Visible == false)
            {
                buttonEditItem.PerformClick();
            }
        }

        private void buttonDelete_Click(object sender, EventArgs e)
        {
            DialogResult dialog = MessageBox.Show("Are you sure you want to Delete the Item?", "Delete Item", MessageBoxButtons.YesNo,MessageBoxIcon.Warning);
            if (dialog == DialogResult.Yes)
            {
                DelteItem();
            }
          

        }
        private void DelteItem()
        {
            ButtonsUtility.DeleteItem(Item_Id);
            textBoxNameOfItem.Clear();
            textBoxtemDescription.Clear();
            textBoxItemCode.Clear();
            textBoxUOM.Clear();
            comboBoxItems.Text = "";
            InitializeDropDownItemValues();
            labelDeletionSucess.Visible = true;
        }

        private void textBoxNameOfItem_TextChanged(object sender, EventArgs e)
        {
            labelNewItemCreatedSucessfully.Visible = false;
        }

        private void textBoxtemDescription_TextChanged(object sender, EventArgs e)
        {
            labelNewItemCreatedSucessfully.Visible = false;
            labelDeletionSucess.Visible = false;
            labelProductEdit.Visible = false;
        }

        private void textBoxItemCode_TextChanged(object sender, EventArgs e)
        {
            labelNewItemCreatedSucessfully.Visible = false;
            labelDeletionSucess.Visible = false;
            labelProductEdit.Visible = false;
        }

       

        private void textBoxUOM_TextChanged(object sender, EventArgs e)
        {
            labelNewItemCreatedSucessfully.Visible = false;
            labelDeletionSucess.Visible = false;
            labelProductEdit.Visible = false;
        }

        private void Back_Click(object sender, EventArgs e)
        {
            Settings config = new Settings();
            config.Show();
            this.Close();
        }

        private void textBoxCostofEachUnit_TextChanged(object sender, EventArgs e)
        {
            labelNewItemCreatedSucessfully.Visible = false;
            labelDeletionSucess.Visible = false;
            labelProductEdit.Visible = false;
        }

        private void textBoxCostofEachUnit_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 46 && textBoxCostofEachUnit.Text.IndexOf('.') != -1)//check if user has entered more than one dot .
            {
                e.Handled = true;
                return;
            }
            if ((!char.IsNumber(e.KeyChar)) && (!char.IsControl(e.KeyChar)) && (e.KeyChar != 46))  //46 is ASCCI Code for dot "." 
            {

                e.Handled = true;
            }

            if (e.KeyChar == 13 && buttonCreateNewItemAddNewItems.Visible == true)
            {
                buttonCreateNewItemAddNewItems.PerformClick();
            }
            else if (e.KeyChar == 13 && buttonCreateNewItemAddNewItems.Visible == false)
            {
                buttonEditItem.PerformClick();
            }
        }

        private void textBoxRework_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 46 && textBoxRework.Text.IndexOf('.') != -1)//check if user has entered more than one dot .
            {
                e.Handled = true;
                return;
            }
            if ((!char.IsNumber(e.KeyChar)) && (!char.IsControl(e.KeyChar)) && (e.KeyChar != 46))  //46 is ASCCI Code for dot "." 
            {

                e.Handled = true;
            }

            if (e.KeyChar == 13 && buttonCreateNewItemAddNewItems.Visible == true)
            {
                buttonCreateNewItemAddNewItems.PerformClick();
            }
            else if (e.KeyChar == 13 && buttonCreateNewItemAddNewItems.Visible == false)
            {
                buttonEditItem.PerformClick();
            }
        }

        private void textBoxInhouseRej_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 46 && textBoxInhouseRej.Text.IndexOf('.') != -1)//check if user has entered more than one dot .
            {
                e.Handled = true;
                return;
            }
            if ((!char.IsNumber(e.KeyChar)) && (!char.IsControl(e.KeyChar)) && (e.KeyChar != 46))  //46 is ASCCI Code for dot "." 
            {

                e.Handled = true;
            }

            if (e.KeyChar == 13 && buttonCreateNewItemAddNewItems.Visible == true)
            {
                buttonCreateNewItemAddNewItems.PerformClick();
            }
            else if (e.KeyChar == 13 && buttonCreateNewItemAddNewItems.Visible == false)
            {
                buttonEditItem.PerformClick();
            }
        }

        private void textBoxRework_TextChanged(object sender, EventArgs e)
        {
            labelNewItemCreatedSucessfully.Visible = false;
            labelDeletionSucess.Visible = false;
            labelProductEdit.Visible = false;
        }

        private void textBoxInhouseRej_TextChanged(object sender, EventArgs e)
        {
            labelNewItemCreatedSucessfully.Visible = false;
            labelDeletionSucess.Visible = false;
            labelProductEdit.Visible = false;
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
