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
    public partial class AddMaterial : Form
    {
        private int Item_Id=0;
        private int Material_Id;
        private string Material_Code = "";
      
      
        public AddMaterial(int itemId, int AddorEdit)
        {
            InitializeComponent();
            Item_Id = itemId;
            if(AddorEdit==1) //1 is edit
            {   
                comboBoxMaterials.Visible=true;
                labelMaterialList.Visible=true;
                InitializeDropDownValues(itemId);
                buttonSave.Visible=false;
                buttonDelete.Visible = true;
            }
            else if(AddorEdit==0)// 0 is add
            {
                comboBoxMaterials.Visible=false;
                labelMaterialList.Visible=false;
                buttonEdit.Visible=false;
                buttonDelete.Visible = false;
               

            }
            
        }
        private void InitializeDropDownValues(int itemId)
        {

            DataTable dt = new DataTable();
            dt = ButtonsUtility.InitializeItemDropDownMaterials(itemId);
            comboBoxMaterials.Items.Clear();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                comboBoxMaterials.Items.Add(dt.Rows[i][0]);

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
            labelRecordupdatedSucessfully.Visible = false;
            labelRequiredFileds.Visible = false;
            
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
            if (textBoxName.Text == "" || textBoxCode.Text == "" || textBoxUnit.Text == "" || textBoxMultiple.Text == "" || textBoxCost.Text=="")
            {
                labelRequiredFileds.Visible=true;
            }
            
            else{


                int noofRecordsAffected = ButtonsUtility.SaveMaterial(Item_Id, textBoxName.Text, textBoxCode.Text, textBoxUnit.Text, Convert.ToDecimal(textBoxMultiple.Text), Convert.ToDecimal(textBoxCost.Text));

                if (noofRecordsAffected > 0)
                {
                  labelRecordupdatedSucessfully.Text = "Record entered sucessfully.";
                   labelRecordupdatedSucessfully.Visible = true;

                   comboBoxMaterials.Visible = true;
                   labelMaterialList.Visible = true;
                   InitializeDropDownValues(Item_Id);
                   buttonEdit.Visible = true;
                   buttonDelete.Visible = true;
               }
                else
                {
                    labelRecordupdatedSucessfully.Text = "Cannot save already saved material please edit it.";

                    labelRecordupdatedSucessfully.Visible = true;

                    buttonEdit.Visible = true;
                    buttonDelete.Visible = true;
                    comboBoxMaterials.Visible = true;
                    labelMaterialList.Visible = true;

                }
            }
            
           
             
        }

       
       

        private void comboBoxItemsDailyEntry_SelectedIndexChanged(object sender, EventArgs e)
        {
           
            labelPleaseSelectMaterial.Visible = false;
            labelRecordupdatedSucessfully.Visible = false;
            labelEditErrorMessageforDate.Visible = false;
        }

        

        private void textBoxCostofEachUnit_KeyPress(object sender, KeyPressEventArgs e)
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
            if(e.KeyChar==46 && textBoxMultiple.Text.IndexOf('.')!=-1)//check if user has entered more than one dot .
            {
                e.Handled=true;
                return;
            }
             if ((!char.IsNumber(e.KeyChar)) && (!char.IsControl(e.KeyChar)) && (e.KeyChar!=46))  //46 is ASCCI Code for dot "." 
            {

                e.Handled = true;
            }
             if (e.KeyChar == 13)
             {
                 buttonSave.PerformClick();
             }
        }

       
       
        private void textBoxEnterProcess_TextChanged(object sender, EventArgs e)
        {
            labelRecordupdatedSucessfully.Visible = false;
            labelRequiredFileds.Visible = false;
        }

       

        private void comboBoxItemsProcess_SelectedIndexChanged(object sender, EventArgs e)
        {
            string input = comboBoxMaterials.Text;
            Material_Code = input.Remove(input.IndexOf("-"));




            GetMaterialDetails(Material_Code);
            labelRecordupdatedSucessfully.Visible = false;
            labelRequiredFileds.Visible = false;
        }
        private void GetMaterialDetails (string Code)
        {
            DataTable dt = new DataTable();
            dt = ButtonsUtility.GetMaterials(Material_Code, Item_Id);

            textBoxName.Text = dt.Rows[0][0].ToString();
            textBoxCode.Text = dt.Rows[0][1].ToString();
            textBoxUnit.Text = dt.Rows[0][2].ToString();
            textBoxMultiple.Text = dt.Rows[0][3].ToString();
            textBoxCost.Text = dt.Rows[0][4].ToString();


            Material_Id = Convert.ToInt32(dt.Rows[0][5]);

        }
        private void buttonEditProcess_Click(object sender, EventArgs e)
        {
            if (textBoxName.Text == "" || textBoxCode.Text == "" || textBoxUnit.Text == "" || textBoxMultiple.Text == "" || textBoxCost.Text=="")
            {
                labelRequiredFileds.Visible = true;
            }
            else if (labelRecordupdatedSucessfully.Visible == true)
            {
                textBoxName.Text = "";
                textBoxCode.Text = "";
                textBoxUnit.Text = "";
                textBoxMultiple.Text = "";
                textBoxCost.Text = "";
            }
            else
            {



                int noofRecordsAffected = ButtonsUtility.EditMaterial(Item_Id, textBoxName.Text, textBoxCode.Text, textBoxUnit.Text, Convert.ToDecimal(textBoxMultiple.Text), Convert.ToDecimal(textBoxCost.Text), Material_Id);

                if (noofRecordsAffected > 0)
                {
                    labelRecordupdatedSucessfully.Text = "Record entered sucessfully.";
                    labelRecordupdatedSucessfully.Visible = true;
                    InitializeDropDownValues(Item_Id);

                }
            }
        }

        private void textBoxAmount_KeyPress(object sender, KeyPressEventArgs e)
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

        private void buttonDelete_Click(object sender, EventArgs e)
        {
            DialogResult dialog = MessageBox.Show("Are you sure you want to Delete the Material?", "Delete Material", MessageBoxButtons.YesNo,MessageBoxIcon.Warning);
            if (dialog == DialogResult.Yes)
            {
                DeleteMaterial();
            }
        }

        private void DeleteMaterial()
        {
            if (comboBoxMaterials.Text == "")
            {
                labelPleaseSelectMaterial.Visible = true;
            }
            else
            {
                int rows = ButtonsUtility.DeleteMaterial(Material_Id, Item_Id);

                if (rows > 0)
                {

                    textBoxName.Text = "";
                    textBoxCode.Text = "";
                    textBoxCost.Text = "";
                    textBoxMultiple.Text = "";
                    textBoxUnit.Text = "";
                    comboBoxMaterials.Text = "";

                    labelRecordupdatedSucessfully.Text = "Record deleted sucessfully.";

                    labelRecordupdatedSucessfully.Visible = true;

                    buttonSave.Visible = true;
                    InitializeDropDownValues(Item_Id);



                }
            }
        }

        private void textBoxUnit_TextChanged(object sender, EventArgs e)
        {
            labelRecordupdatedSucessfully.Visible = false;
            labelRequiredFileds.Visible = false;
        }

        private void textBoxMultiple_TextChanged(object sender, EventArgs e)
        {
            labelRecordupdatedSucessfully.Visible = false;
            labelRequiredFileds.Visible = false;
        }

        private void textBoxCost_TextChanged(object sender, EventArgs e)
        {
            labelRecordupdatedSucessfully.Visible = false;
            labelRequiredFileds.Visible = false;
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
            CustomerRejectionKPI CRK = new CustomerRejectionKPI(0);
            CRK.Show();
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
