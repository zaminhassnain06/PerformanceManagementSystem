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
    public partial class DailyEntry_Material : Form
    {
        private int Item_Id=0;

        private int Material_Id;

        private string Material_Code = "";

        public DailyEntry_Material(int itemId)
        {
            InitializeComponent();
            Item_Id = itemId;
            if (itemId > 0)
            {
                string ItemName = ButtonsUtility.ItemNameUtility(itemId);
                labelItemName.Text = ItemName;
                //comboBoxItemsDailyEntry.Visible = false;
               // labelSelectItemDailyEntry.Visible = false;
                IntializeMaterialsDropDown();
                TextBoxesCalculatedValuesofItemSelectedbyCombo(labelItemName.Text, DateTime.Today.ToString("yyyy-MM-dd"));
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
                comboBoxItemsDailyEntry.Items.Add(dt.Rows[i][0]);

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
 
            YieldVarianceKPI OverAllKpi = new YieldVarianceKPI(Item_Id);
            OverAllKpi.Show();
            this.Close();
            
        }

       

        private void textBoxNameOfItem_TextChanged(object sender, EventArgs e)
        {
            //if(textBoxCostofEachUnit.Text!="" )
            //{ 
            //int TotalNoofItem = Convert.ToInt32(textBoxTotalMaterial.Text);
            //int CostofUnit = Convert.ToInt32(textBoxCostofEachUnit.Text);
            //int TotalCost = TotalNoofItem * CostofUnit;
            //textBoxTotalCost.Text = TotalCost.ToString();
            //}
        }

        private void textBoxTotalUnits_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 46 && textBoxMultiple.Text.IndexOf('.') != -1)//check if user has entered more than one dot .
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
                buttonSave.PerformClick();
            }
        }

        private void buttonDailyEntrySave_Click(object sender, EventArgs e)
        {
            if (Item_Id > 0)
            {
                if (textBoxTotalMaterial.Text == "")
                {
                    labelPleaseEnterTotalMaterialUsed.Visible = true;
                }
                else if (comboBoxMaterial.Text == "")
                {
                    labelPleaseSelectMaterial.Visible = true;
                }
                else
                {

                    int noofRecordsAffected = ButtonsUtility.SaveDailyEntryMaterial(Item_Id, Material_Id, Convert.ToDecimal(textBoxTotalMaterial.Text), Convert.ToDecimal(textBoxTotalCost.Text), Convert.ToDecimal(textBoxPlannedMaterialUsed.Text), Convert.ToDecimal(textBoxPlannedCost.Text), dateTimePickerMaterial.Text);


                    if (noofRecordsAffected > 0)
                    {

                        labelRecordupdatedSucessfullyDailyEntry.Text = "Record entered sucessfully.";

                        labelRecordupdatedSucessfullyDailyEntry.Visible = true;

                        buttonEdit.Visible = true;

                    }

                    else
                    {

                        labelRecordupdatedSucessfullyDailyEntry.Text = "Record already saved. Please edit already save record.";

                        labelRecordupdatedSucessfullyDailyEntry.Visible = true;

                    }

                }
            }

            else if (Item_Id == 0 && comboBoxItemsDailyEntry.Text == "")
            {

                labelPleaseSelectItem.Visible = true;


            }
             
        }

        private void dateTimePickerCustomerRejection_ValueChanged(object sender, EventArgs e)
        {
            if (comboBoxItemsDailyEntry.Text != "" && comboBoxMaterial.Text!="")
            {
                TextBoxesCalculatedValuesofItemSelectedbyComboDateTime(comboBoxItemsDailyEntry.Text, dateTimePickerMaterial.Text);
                GetEditableIdWithDateChange();
            }
            else if (comboBoxItemsDailyEntry.Text != "" && comboBoxMaterial.Text == "")
            {
                TextBoxesCalculatedValuesofItemSelectedbyCombo(comboBoxItemsDailyEntry.Text, dateTimePickerMaterial.Text);
            }

            else if (comboBoxItemsDailyEntry.Visible == false)
            {
                TextBoxesCalculatedValuesofItemSelectedbyCombo(labelItemName.Text, dateTimePickerMaterial.Text);
            }
                labelEditErrorMessageforDate.Visible = false;

            //}
            labelRecordupdatedSucessfullyDailyEntry.Visible = false;
            labelEditErrorMessageforDate.Visible = false;
        }

        private void TextBoxesCalculatedValuesofItemSelectedbyComboDateTime(string itemName, string DateTime )
        {
            DataTable dt = new DataTable();
            dt = ButtonsUtility.TotalUnitsMadeDetailsByName(itemName, DateTime, "DailyEntry");
            if (Convert.ToInt32(dt.Rows[0][0]) > 0)
            {


              
                if (comboBoxMaterial.Text == "")
                {
                    textBoxTotalMaterial.Text = "";
                    textBoxCostofEachUnit.Text = "";
                    textBoxTotalCost.Text = "";
                }
                textBoxDailyProduction.Text = dt.Rows[0][1].ToString();
                
                
                Item_Id = Convert.ToInt32(dt.Rows[0][3]);

                labelPleaseEnterdailyEntry.Visible = false;
                buttonDailyEntry.Visible = false;

              
                
            }
            else
            {
                //FixedEntryValueCheck = 1;// Controls change of value in Editabletextbox


                textBoxTotalMaterial.Text = "";
                textBoxCostofEachUnit.Text = "";
                textBoxTotalCost.Text = "";

                labelPleaseEnterdailyEntry.Visible = true;
                buttonDailyEntry.Visible = true;

                buttonSave.Visible = false;
                buttonEdit.Visible = false;
                

                Item_Id = Convert.ToInt32(dt.Rows[0][1]);

            }
        }
        private void TextBoxesCalculatedValuesofItemSelectedbyCombo(string itemName, string DateTime )
        {
            DataTable dt = new DataTable();
            dt = ButtonsUtility.TotalUnitsMadeDetailsByName(itemName, DateTime, "DailyEntry");
            if (Convert.ToInt32(dt.Rows[0][0]) > 0)
            {


              
                if (comboBoxMaterial.Text == "")
                {
                    textBoxTotalMaterial.Text = "";
                    textBoxCostofEachUnit.Text = "";
                    textBoxTotalCost.Text = "";
                }
                textBoxDailyProduction.Text = dt.Rows[0][1].ToString();
                
                
                Item_Id = Convert.ToInt32(dt.Rows[0][3]);
                IntializeMaterialsDropDown();

                labelPleaseEnterdailyEntry.Visible = false;
                buttonDailyEntry.Visible = false;
            }
            else
            {
                //FixedEntryValueCheck = 1;// Controls change of value in Editabletextbox


                textBoxTotalMaterial.Text = "";
                textBoxCostofEachUnit.Text = "";
                textBoxTotalCost.Text = "";

                labelPleaseEnterdailyEntry.Visible = true;
                buttonDailyEntry.Visible = true;

                buttonSave.Visible = false;
                buttonEdit.Visible = false;
                

                Item_Id = Convert.ToInt32(dt.Rows[0][1]);

            }
           // FixedEntryValueCheck = 0;// Controls change of value in Editabletextbox

        }

        private void comboBoxItemsDailyEntry_SelectedIndexChanged(object sender, EventArgs e)
        {
            textBoxDailyProduction.Text = "";
            TextBoxesCalculatedValuesofItemSelectedbyCombo(comboBoxItemsDailyEntry.Text, dateTimePickerMaterial.Text);
            labelPleaseSelectItem.Visible = false;
            labelRecordupdatedSucessfullyDailyEntry.Visible = false;
            labelEditErrorMessageforDate.Visible = false;
            
           
           
        }

        private void IntializeMaterialsDropDown()
        {
            DataTable dt = new DataTable();
            dt = ButtonsUtility.InitializeItemDropDownMaterials(Item_Id);
           
               
            
                comboBoxMaterial.Items.Clear();
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    comboBoxMaterial.Items.Add(dt.Rows[i][0]);

                }
            
        }

        private void buttonEditDailyEntry_Click(object sender, EventArgs e)
        {
            if(Item_Id>0 && textBoxTotalCost.Text=="")
            {
                labelEditErrorMessageforDate.Visible = true;
            }
            else if(Item_Id==0 && comboBoxItemsDailyEntry.Text=="")
            {
                labelPleaseSelectItem.Visible = true;
            }
            else{


                int noofRecordsAffected = ButtonsUtility.EditDailyEntryMaterial(Item_Id, Material_Id, Convert.ToDecimal(textBoxTotalMaterial.Text), Convert.ToDecimal(textBoxTotalCost.Text), Convert.ToDecimal(textBoxPlannedMaterialUsed.Text), Convert.ToDecimal(textBoxPlannedCost.Text), dateTimePickerMaterial.Text);
           
            
                if (noofRecordsAffected > 0)
            
                {
               
                    labelRecordupdatedSucessfullyDailyEntry.Text = "Record updated sucessfully.";
               
                    labelRecordupdatedSucessfullyDailyEntry.Visible = true;
           
                }
                else
                {
                    labelRecordupdatedSucessfullyDailyEntry.Text = "Record is not already saved. Please save the record first.";

                    labelRecordupdatedSucessfullyDailyEntry.Visible = true;
                }
            }
        }

        private void textBoxCostofEachUnit_KeyPress(object sender, KeyPressEventArgs e)
        {
            if ((!char.IsNumber(e.KeyChar)) && (!char.IsControl(e.KeyChar)))
            {

                e.Handled = true;
            }
        }

        

        private void labelDailyEntryDateTime_Click(object sender, EventArgs e)
        {

        }

        private void textBoxTotalUnits_TextChanged(object sender, EventArgs e)
        {
            if (textBoxCostofEachUnit.Text != ""  && textBoxTotalMaterial.Text != "")
            {
                //decimal costofEachUnitOfMultiple = Convert.ToDecimal(textBoxCostofEachUnit.Text) / Convert.ToDecimal(textBoxMultiple.Text);

                //textBoxTotalCost.Text = (costofEachUnitOfMultiple * Convert.ToDecimal(textBoxTotalMaterial.Text)).ToString();
                decimal costofEachUnitOfMultiple =ButtonsUtility.CalculateTotalCostofMaterial(Convert.ToDecimal(textBoxCostofEachUnit.Text), Convert.ToDecimal(textBoxMultiple.Text), Convert.ToDecimal(textBoxTotalMaterial.Text));
                textBoxTotalCost.Text = (costofEachUnitOfMultiple).ToString();


            }
            labelPleaseEnterTotalMaterialUsed.Visible = false;
            labelRecordupdatedSucessfullyDailyEntry.Visible = false;
        }

       

        private void comboBoxMaterial_SelectedIndexChanged(object sender, EventArgs e)
        {
            string input = comboBoxMaterial.Text;
            Material_Code = input.Remove(input.IndexOf("-"));




            GetMaterialDetails(Material_Code);
            labelRecordupdatedSucessfullyDailyEntry.Visible = false;
            labelPleaseSelectMaterial.Visible = false;
        }

        private void GetEditableIdWithDateChange()
        {
            string input = comboBoxMaterial.Text;
            Material_Code = input.Remove(input.IndexOf("-"));




            GetMaterialDetails(Material_Code);
            labelRecordupdatedSucessfullyDailyEntry.Visible = false;
            labelPleaseSelectMaterial.Visible = false;
        }
        private void GetMaterialDetails(string Code)
        {
            DataTable dt = new DataTable();
            DataTable dtdaily = new DataTable();
            dt = ButtonsUtility.GetMaterials(Material_Code, Item_Id);

            dtdaily = ButtonsUtility.GetMaterialsDaily(Material_Code, Item_Id, dateTimePickerMaterial.Text);


            if (dt.Rows.Count > 0)
            {
                textBoxUnit.Text = dt.Rows[0][2].ToString(); //unit
                textBoxMultiple.Text = dt.Rows[0][3].ToString();  //Multiple
                textBoxCostofEachUnit.Text = dt.Rows[0][4].ToString(); //costofEach
                decimal Multiples = Convert.ToDecimal(dt.Rows[0][3]);

                if (!string.IsNullOrEmpty(textBoxDailyProduction.Text ))
                {
                    //textBoxPlannedMaterialUsed
                    textBoxPlannedMaterialUsed.Text = (Multiples * Convert.ToInt32(textBoxDailyProduction.Text)).ToString();
                    //Planned cost
                    textBoxPlannedCost.Text = ((Convert.ToInt32(textBoxDailyProduction.Text)) * (Convert.ToDecimal(dt.Rows[0][4]))).ToString();
                }
                if (textBoxTotalMaterial.Text != "")
                {
                    decimal costofEachUnitOfMultiple = Convert.ToDecimal(dt.Rows[0][4]) / Multiples;

                    textBoxTotalCost.Text = (costofEachUnitOfMultiple * Convert.ToDecimal(textBoxTotalMaterial.Text)).ToString();
                }
                Material_Id = Convert.ToInt32(dt.Rows[0][5]);
            }
            if(dtdaily.Rows.Count>0)
            {
                textBoxTotalMaterial.Text = dtdaily.Rows[0][0].ToString();
                textBoxTotalCost.Text = dtdaily.Rows[0][1].ToString();
                labelPleaseEnterdailyEntry.Visible = false;
                buttonDailyEntry.Visible = false;
                buttonEdit.Visible = true;

            }
            else
            {
                textBoxTotalMaterial.Text = "";
                textBoxTotalCost.Text = "";
                labelPleaseEnterTotalMaterialUsed.Visible = true;
                //labelPleaseEnterdailyEntry.Visible = true;
                //buttonDailyEntry.Visible = true;

            }
        }
        private void buttonDailyEntry_Click(object sender, EventArgs e)
        {
            DailyEntry DE = new DailyEntry(Item_Id);
            DE.Show();
            this.Close();
        }

        private void textBoxDailyProduction_TextChanged(object sender, EventArgs e)
        {
            if (textBoxDailyProduction.Text != "")
            {
                labelPleaseEnterdailyEntry.Visible = false;
                buttonDailyEntry.Visible = false;
                buttonSave.Visible = true;
                
            }
            

        }

        private void pictureBoxDEMM_Click(object sender, EventArgs e)
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

        private void textBoxPlannedMaterialUsed_TextChanged(object sender, EventArgs e)
        {

        }
       


        
        
    }
}
