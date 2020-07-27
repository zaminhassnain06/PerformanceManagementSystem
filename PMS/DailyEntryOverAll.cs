using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.OleDb;

namespace PMS
{
    public partial class DailyEntryOverAll : Form
    {
        private int Item_Id;

        public DailyEntryOverAll(int itemId)
        {
            InitializeComponent();
            Item_Id = itemId;
            if (itemId > 0)
            {
                string ItemName = ButtonsUtility.ItemNameUtility(itemId);
                labelItemName.Text = ItemName;
             //   comboBoxItemsDailyEntry.Visible = false;
             //   labelSelectItemDailyEntry.Visible = false;
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

            Settings config = new Settings();
            config.Show();
            this.Close();
            
        }

       

        private void textBoxNameOfItem_TextChanged(object sender, EventArgs e)
        {
            if (textBoxCostofEachUnit.Text != "" && textBoxTotalUnits.Text!="")
            { 
                int TotalNoofItem = Convert.ToInt32(textBoxTotalUnits.Text);
                decimal CostofUnit = Convert.ToDecimal(textBoxCostofEachUnit.Text);
                decimal TotalCost = TotalNoofItem * CostofUnit;
                textBoxTotalCost.Text = TotalCost.ToString();
            }
            labelAlreadySaved.Visible = false;
            labelRecordupdatedSucessfullyDailyEntry.Visible = false;
            labelNotSaved.Visible = false;
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

            if (Item_Id > 0)   //daily entry
            {
                if (textBoxTotalUnits.Text != "")
                {
                    int noofRecordsAffected = ButtonsUtility.SaveDailyEntry(Item_Id, Convert.ToInt32(textBoxTotalUnits.Text), Convert.ToDecimal(textBoxTotalCost.Text), dateTimePickerDailyEntry.Text);

                    if (noofRecordsAffected > 0)
                    {
                        labelRecordupdatedSucessfullyDailyEntry.Text = "Record entered sucessfully.";
                        labelRecordupdatedSucessfullyDailyEntry.Visible = true;
                    }
                    else
                    {

                        labelAlreadySaved.Visible = true;
                    }
                }
                if (textBoxUnitsReworked.Text != "")
                {
                    int noofRecordsAffected = ButtonsUtility.SaveDailyEntryofKPI(Item_Id, Convert.ToInt32(textBoxUnitsReworked.Text), Convert.ToDecimal(textBoxCostofReworking.Text), dateTimePickerDailyEntry.Text, "ReworkRatio");

                    if (noofRecordsAffected > 0)
                    {
                        labelRecordupdatedSucessfullyDailyEntry.Visible = true;
                    }
                    else
                    {
                        labelAlreadySaved.Visible = true;
                    }

                }

                if (textBoxUnitsInhouseRejection.Text != "")
                {
                    int noofRecordsAffected = ButtonsUtility.SaveDailyEntryofKPI(Item_Id, Convert.ToInt32(textBoxUnitsInhouseRejection.Text), Convert.ToDecimal(textBoxTotalCostofInhouseRejection.Text), dateTimePickerDailyEntry.Text, "InhouseRatio");

                    if (noofRecordsAffected > 0)
                    {
                        labelRecordupdatedSucessfullyDailyEntry.Visible = true;
                    }
                    else
                    {
                        labelAlreadySaved.Visible = true;
                    }

                }
                if (textBoxUnitsCustomerRejection.Text != "")
                {
                    int noofRecordsAffected = ButtonsUtility.SaveDailyEntryofKPI(Item_Id, Convert.ToInt32(textBoxUnitsCustomerRejection.Text), Convert.ToDecimal(textBoxCostofCustomerRejection.Text), dateTimePickerDailyEntry.Text, "CustomerRejection");

                    if (noofRecordsAffected > 0)
                    {
                        labelRecordupdatedSucessfullyDailyEntry.Visible = true;
                    }
                    else
                    {
                        labelAlreadySaved.Visible = true;
                    }
                }
                if (textBoxAcutalCustomerClaims.Text != "" || textBoxExpectedCustomerClaims.Text != "")
                {
                    int noofRecordsAffected = ButtonsUtility.SaveDailyEntryCustomerClaims(Item_Id, Convert.ToInt32(textBoxExpectedCustomerClaims.Text), Convert.ToInt32(textBoxAcutalCustomerClaims.Text), dateTimePickerDailyEntry.Text);

                    if (noofRecordsAffected > 0)
                    {
                        labelRecordupdatedSucessfullyDailyEntry.Visible = true;
                    }
                    else
                    {
                        labelAlreadySaved.Visible = true;
                    }

                }
                if (textBoxAcutalDelivaryCompliance.Text != "" || textBoxPlannedDelivaryCompliance.Text != "")
                {

                    int noofRecordsAffected = ButtonsUtility.SaveDailyEntryDelCompliance(Item_Id, Convert.ToInt32(textBoxPlannedDelivaryCompliance.Text), Convert.ToInt32(textBoxAcutalDelivaryCompliance.Text), Convert.ToDecimal(textBoxDelCompPercnt.Text), dateTimePickerDailyEntry.Text);

                    if (noofRecordsAffected > 0)
                    {
                        labelRecordupdatedSucessfullyDailyEntry.Visible = true;
                    }
                    else
                    {

                        labelAlreadySaved.Visible = true;
                    }
                }

                else if (Item_Id == 0 && comboBoxItemsDailyEntry.Text == "")
                {

                    labelPleaseSelectItem.Visible = true;


                }






                labelNotSaved.Visible = false;

            }
        }

        private void dateTimePickerCustomerRejection_ValueChanged(object sender, EventArgs e)
        {
            if (comboBoxItemsDailyEntry.Text != "" && comboBoxItemsDailyEntry.Visible == true)
            {
                TextBoxesCalculatedValuesofItemSelectedbyCombo(comboBoxItemsDailyEntry.Text, dateTimePickerDailyEntry.Text);
                TextBoxesCalculatedValuesofItemSelectedbyComboRework(comboBoxItemsDailyEntry.Text, dateTimePickerDailyEntry.Text, "ReworkRatio");
                TextBoxesCalculatedValuesofItemSelectedbyComboInhouseRej(comboBoxItemsDailyEntry.Text, dateTimePickerDailyEntry.Text, "InhouseRatio");
                TextBoxesCalculatedValuesofItemSelectedbyComboCustRejRatio(comboBoxItemsDailyEntry.Text, dateTimePickerDailyEntry.Text, "CustomerRejection");
                TextBoxesCalculatedValuesofItemSelectedbyComboCustClaims(comboBoxItemsDailyEntry.Text, dateTimePickerDailyEntry.Text);
                TextBoxesCalculatedValuesofItemSelectedbyComboDelComp(comboBoxItemsDailyEntry.Text, dateTimePickerDailyEntry.Text);

            }
            else if (comboBoxItemsDailyEntry.Visible == false)
            {
                TextBoxesCalculatedValuesofItemSelectedbyCombo(labelItemName.Text, dateTimePickerDailyEntry.Text);
                labelEditErrorMessageforDate.Visible = false;

            }
            labelRecordupdatedSucessfullyDailyEntry.Visible = false;
            labelEditErrorMessageforDate.Visible = false;
            labelAlreadySaved.Visible = false;
            labelNotSaved.Visible = false;
            labelReportisGenerating.Visible = false;

            if (!string.IsNullOrEmpty(dateTimePickerDailyEntry.Text) && Item_Id > 0 )
            {
                buttonBrowse.Enabled = true;
                buttonImport.Enabled = true;
            }
        }

        private void TextBoxesCalculatedValuesofItemSelectedbyComboCustClaims(string itemName, string DateTime)
        {
            DataTable dt = new DataTable();
            dt = ButtonsUtility.TotalUnitsMadeDetailsByNameforCustomerClaims(itemName, DateTime);
            if (Convert.ToInt32(dt.Rows[0][0]) > 0)
            {


              //  labelRecordupdatedSucessfullyDailyEntryCustomerClaims.Visible = false;
                if (dt.Rows[0][1] is int)
                 textBoxExpectedCustomerClaims.Text = dt.Rows[0][1].ToString();
                if (dt.Rows[0][2] is int)
                    textBoxAcutalCustomerClaims.Text = dt.Rows[0][2].ToString();
                if (dt.Rows[0][3] is int)
                 Item_Id = Convert.ToInt32(dt.Rows[0][3]);

            }
            else
            {
                //FixedEntryValueCheck = 1;// Controls change of value in Editabletextbox


                textBoxExpectedCustomerClaims.Text = "";
                textBoxAcutalCustomerClaims.Text = "";


                if (dt.Rows[0][1] is int || dt.Rows[0][1] is decimal)
                    Item_Id = Convert.ToInt32(dt.Rows[0][1]);

            }
         //   FixedEntryValueCheck = 0;// Controls change of value in Editabletextbox

        }


        private void TextBoxesCalculatedValuesofItemSelectedbyComboDelComp(string itemName, string DateTime)
        {
            DataTable dt = new DataTable();
            dt = ButtonsUtility.TotalUnitsMadeDetailsByNameforDailyComp(itemName, DateTime);
            if (Convert.ToInt32(dt.Rows[0][0]) > 0)
            {

                if (dt.Rows[0][1] is int || dt.Rows[0][1] is decimal)
                     textBoxPlannedDelivaryCompliance.Text = dt.Rows[0][1].ToString();
                if (dt.Rows[0][2] is int || dt.Rows[0][2] is decimal)
                    textBoxAcutalDelivaryCompliance.Text = dt.Rows[0][2].ToString();
                if (dt.Rows[0][3] is int || dt.Rows[0][3] is decimal)
                    textBoxDelCompPercnt.Text = dt.Rows[0][3].ToString();
                if (dt.Rows[0][4] is int || dt.Rows[0][4] is decimal)
                    Item_Id = Convert.ToInt32(dt.Rows[0][4]);

            }
            else
            {
                textBoxPlannedDelivaryCompliance.Text = "";
                textBoxAcutalDelivaryCompliance.Text = "";
                textBoxDelCompPercnt.Text = "";

                if (dt.Rows[0][1] is int || dt.Rows[0][1] is decimal)
                  Item_Id = Convert.ToInt32(dt.Rows[0][1]);
            }

        }

        private void TextBoxesCalculatedValuesofItemSelectedbyCombo(string itemName, string DateTime)
        {
            DataTable dt = new DataTable();
            dt = ButtonsUtility.TotalUnitsMadeDetailsByName(itemName, DateTime, "DailyEntry");
            if (Convert.ToInt32(dt.Rows[0][0]) > 0)
            {


                labelRecordupdatedSucessfullyDailyEntry.Visible = false;
                if (dt.Rows[0][1] is int || dt.Rows[0][1] is decimal)
                    textBoxTotalUnits.Text = dt.Rows[0][1].ToString();

                if (dt.Rows[0][2] is int || dt.Rows[0][2] is decimal)
                    textBoxTotalCost.Text = dt.Rows[0][2].ToString();
                if (dt.Rows[0][3] is int || dt.Rows[0][3] is decimal)
                    Item_Id = Convert.ToInt32(dt.Rows[0][3]);
                GetCost();
            }
            else
            {
              


                textBoxTotalUnits.Text = "";
                textBoxCostofEachUnit.Text = "";
                textBoxTotalCost.Text = "";

                if (dt.Rows[0][1] is int || dt.Rows[0][1] is decimal)
                    Item_Id = Convert.ToInt32(dt.Rows[0][1]);
                GetCost();

            }
        

        }

        private void GetCost()
        {
            DataTable dt = ButtonsUtility.GetPriceofItem(Item_Id);
            if (dt.Rows[0][0] is decimal || dt.Rows[0][0] is int)
                textBoxCostofEachUnit.Text = dt.Rows[0][0].ToString();
           
        }

        private void comboBoxItemsDailyEntry_SelectedIndexChanged(object sender, EventArgs e)
        {
            TextBoxesCalculatedValuesofItemSelectedbyCombo(comboBoxItemsDailyEntry.Text, dateTimePickerDailyEntry.Text);
            
            TextBoxesCalculatedValuesofItemSelectedbyComboRework(comboBoxItemsDailyEntry.Text, dateTimePickerDailyEntry.Text, "ReworkRatio");
            
            TextBoxesCalculatedValuesofItemSelectedbyComboInhouseRej(comboBoxItemsDailyEntry.Text, dateTimePickerDailyEntry.Text, "InhouseRatio");

            TextBoxesCalculatedValuesofItemSelectedbyComboCustRejRatio(comboBoxItemsDailyEntry.Text, dateTimePickerDailyEntry.Text, "CustomerRejection");

            TextBoxesCalculatedValuesofItemSelectedbyComboCustClaims(comboBoxItemsDailyEntry.Text, dateTimePickerDailyEntry.Text);

            TextBoxesCalculatedValuesofItemSelectedbyComboDelComp(comboBoxItemsDailyEntry.Text, dateTimePickerDailyEntry.Text);
            labelPleaseSelectItem.Visible = false;
            labelRecordupdatedSucessfullyDailyEntry.Visible = false;
            labelEditErrorMessageforDate.Visible = false;
            labelAlreadySaved.Visible = false;
            labelNotSaved.Visible = false;
            checkBoxReport.Checked = false;
            labelReportisGenerating.Visible = false;
            if (!string.IsNullOrEmpty(dateTimePickerDailyEntry.Text) && Item_Id > 0)
            {
                buttonBrowse.Enabled = true;
                buttonImport.Enabled = true;
            }
        }


        private void TextBoxesCalculatedValuesofItemSelectedbyComboCustRejRatio(string itemName, string DateTime, string KPIs)
        {
            DataTable dt = new DataTable();
            dt = ButtonsUtility.TotalUnitsMadeDetailsByName(itemName, DateTime, KPIs);
            if (Convert.ToInt32(dt.Rows[0][0]) > 0)
            {
                
                Item_Id = Convert.ToInt32(dt.Rows[0][4]);
              //  GetCostCustRej();
                if (dt.Rows[0][5] != null)
                {
                 
                    textBoxUnitsCustomerRejection.Text = dt.Rows[0][5].ToString();
                 
                }
            }
            else
            {

                if (dt.Rows[0][1] is int || dt.Rows[0][1] is decimal)
                    Item_Id = Convert.ToInt32(dt.Rows[0][1]);
                textBoxUnitsCustomerRejection.Clear();           
            }
         
            if (Item_Id > 0)
            {
                textBoxUnitsCustomerRejection.ReadOnly = false;
            }
        }

       
        private void TextBoxesCalculatedValuesofItemSelectedbyComboInhouseRej(string itemName, string DateTime, string KPIs)
        {
            DataTable dt = new DataTable();
            dt = ButtonsUtility.TotalUnitsMadeDetailsByName(itemName, DateTime, KPIs);
            if (Convert.ToInt32(dt.Rows[0][0]) > 0)
            {
                if (dt.Rows[0][3] is int || dt.Rows[0][3] is decimal)
                    Item_Id = Convert.ToInt32(dt.Rows[0][3]);
                GetInhouseRejCost();

                if (dt.Rows[0][4] is int || dt.Rows[0][4] is decimal)                   
                    textBoxUnitsInhouseRejection.Text = dt.Rows[0][4].ToString();
                if (dt.Rows[0][5] is int  || dt.Rows[0][5] is decimal)
                        textBoxTotalCostofInhouseRejection.Text = dt.Rows[0][5].ToString();
                
            }
            else
            {
                
                // textBoxTotalCostInhouseRejection.Text = "";
                textBoxUnitsInhouseRejection.Text = "";
                textBoxTotalCostofInhouseRejection.Text = "";
                if (dt.Rows[0][1] is int || dt.Rows[0][1] is decimal)
                    Item_Id = Convert.ToInt32(dt.Rows[0][1]);
                GetInhouseRejCost();
            }
          
            if (Item_Id > 0)
            {
                textBoxUnitsInhouseRejection.ReadOnly = false;
            }
        }
        private void GetInhouseRejCost()
        {
            DataTable dt = ButtonsUtility.GetPriceofInhouseRejectionOfItem(Item_Id);
            if (dt.Rows[0][0] is int || dt.Rows[0][0] is decimal)
                textBoxCostofEachUnitInhouseRejection.Text = dt.Rows[0][0].ToString();
        }
        private void TextBoxesCalculatedValuesofItemSelectedbyComboRework(string itemName, string DateTime, string KPIs)
        {
            DataTable dt = new DataTable();
            dt = ButtonsUtility.TotalUnitsMadeDetailsByName(itemName, DateTime, KPIs);
            if (Convert.ToInt32(dt.Rows[0][0]) > 0)
            {


                if (dt.Rows[0][2]  is int || dt.Rows[0][2] is decimal)
                {
                    Item_Id = Convert.ToInt32(dt.Rows[0][2]);
                }
                GetReworkCost();
                if (dt.Rows[0][3] is int || dt.Rows[0][3] is decimal)
                {
                    textBoxUnitsReworked.Text = Convert.ToInt32(dt.Rows[0][3]).ToString();
                }
                if (dt.Rows[0][4] is int || dt.Rows[0][4] is decimal)
                {
                    textBoxCostofReworking.Text = Convert.ToInt32(dt.Rows[0][4]).ToString();
                }

            }
            else
            {
              
                textBoxCostofEachUnitReworkRatio.Text = "";

                textBoxUnitsReworked.Text = "";
                textBoxCostofReworking.Text = "";
                if (dt.Rows[0][1] is int || dt.Rows[0][1] is decimal)
                    Item_Id = Convert.ToInt32(dt.Rows[0][1]);
                GetReworkCost();

            }
         
            if (Item_Id > 0)
            {
                textBoxUnitsReworked.ReadOnly = false;
            }
        }

        private void GetReworkCost()
        {
            DataTable dt = ButtonsUtility.GetPriceofReworkItem(Item_Id);
            if (dt.Rows[0][0] is int || dt.Rows[0][0] is decimal) 
            {
                textBoxCostofEachUnitReworkRatio.Text = dt.Rows[0][0].ToString();
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
            
           
                int noofRecordsAffected = ButtonsUtility.EditDailyEntry(Item_Id, Convert.ToInt32(textBoxTotalUnits.Text), Convert.ToDecimal(textBoxTotalCost.Text), dateTimePickerDailyEntry.Text);
           
            
                if (noofRecordsAffected > 0)
            
                {
               
                    labelRecordupdatedSucessfullyDailyEntry.Text = "Record updated sucessfully.";
               
                    labelRecordupdatedSucessfullyDailyEntry.Visible = true;
           
                }
                else
                {


                    labelNotSaved.Visible = true;
                }
            }

             if(Item_Id>0 && textBoxDelCompPercnt.Text!="")
             {
                 
                int noofRecordsAffected = ButtonsUtility.EditDailyEntryDelCompliance(Item_Id, Convert.ToInt32(textBoxPlannedDelivaryCompliance.Text), Convert.ToInt32(textBoxAcutalDelivaryCompliance.Text), Convert.ToDecimal(textBoxDelCompPercnt.Text), dateTimePickerDailyEntry.Text);
           
            
                if (noofRecordsAffected > 0)
            
                {
               
                      labelRecordupdatedSucessfullyDailyEntry.Text = "Record updated sucessfully.";
               
                    labelRecordupdatedSucessfullyDailyEntry.Visible = true;
           
                }
                else
                {
                     labelNotSaved.Visible = true;
                }
             }

             if (Item_Id > 0 && textBoxAcutalCustomerClaims.Text != "" && textBoxExpectedCustomerClaims.Text!="")
             {
                 int noofRecordsAffected = ButtonsUtility.EditDailyEntryCustomerClaims(Item_Id, Convert.ToInt32(textBoxExpectedCustomerClaims.Text), Convert.ToInt32(textBoxAcutalCustomerClaims.Text), dateTimePickerDailyEntry.Text);


                 if (noofRecordsAffected > 0)
                 {

                     labelRecordupdatedSucessfullyDailyEntry.Text = "Record updated sucessfully.";

                     labelRecordupdatedSucessfullyDailyEntry.Visible = true;

                 }
                 else
                 {
                     labelNotSaved.Visible = true;
                 }
             }


             if (Item_Id > 0 && textBoxCostofCustomerRejection.Text != "")
             {
                 int noofRecordsAffected = ButtonsUtility.EditDailyEntryofReworkRatio(Item_Id, Convert.ToInt32(textBoxUnitsCustomerRejection.Text), Convert.ToDecimal(textBoxCostofCustomerRejection.Text), dateTimePickerDailyEntry.Text, "CustomerRejection");

                 if (noofRecordsAffected > 0)
                 {

                     labelRecordupdatedSucessfullyDailyEntry.Text = "Record updated sucessfully.";

                     labelRecordupdatedSucessfullyDailyEntry.Visible = true;

                 }
                 else
                 {
                     labelNotSaved.Visible = true;
                 }

             }

             if (Item_Id > 0 && textBoxUnitsInhouseRejection.Text != "")
             {
                 int noofRecordsAffected = ButtonsUtility.EditDailyEntryofReworkRatio(Item_Id, Convert.ToInt32(textBoxUnitsInhouseRejection.Text), Convert.ToDecimal(textBoxTotalCostofInhouseRejection.Text), dateTimePickerDailyEntry.Text, "InhouseRatio");

                 if (noofRecordsAffected > 0)
                 {

                     labelRecordupdatedSucessfullyDailyEntry.Text = "Record updated sucessfully.";

                     labelRecordupdatedSucessfullyDailyEntry.Visible = true;

                 }
                 else
                 {
                     labelNotSaved.Visible = true;
                 }
             }

             if (Item_Id > 0 &&  textBoxUnitsReworked.Text != "")
             {
                 int noofRecordsAffected = ButtonsUtility.EditDailyEntryofReworkRatio(Item_Id, Convert.ToInt32(textBoxUnitsReworked.Text), Convert.ToDecimal(textBoxCostofReworking.Text), dateTimePickerDailyEntry.Text, "ReworkRatio");
                 if (noofRecordsAffected > 0)
                 {
                     labelRecordupdatedSucessfullyDailyEntry.Text = "Record updated sucessfully.";

                     labelRecordupdatedSucessfullyDailyEntry.Visible = true;
                 }
                 else
                 {
                     labelNotSaved.Visible = true;
                 }
             }

            labelAlreadySaved.Visible = false;
        }

        private void textBoxCostofEachUnit_KeyPress(object sender, KeyPressEventArgs e)
        {
            if ((!char.IsNumber(e.KeyChar)) && (!char.IsControl(e.KeyChar)))
            {

                e.Handled = true;
            }
            if (e.KeyChar == 13)
            {
                buttonDailyEntrySave.PerformClick();
            }
        }

        

        private void labelDailyEntryDateTime_Click(object sender, EventArgs e)
        {

        }

        private void textBoxTotalUnits_TextChanged(object sender, EventArgs e)
        {
            if (textBoxTotalUnits.Text != "" && textBoxCostofEachUnit.Text != "")
            {
                textBoxTotalCost.Text = (Convert.ToDecimal(textBoxCostofEachUnit.Text) * Convert.ToInt32(textBoxTotalUnits.Text)).ToString();
            }
            labelAlreadySaved.Visible = false;
            labelRecordupdatedSucessfullyDailyEntry.Visible = false;
            labelNotSaved.Visible = false;
          
        }

        private void DailyEntryOverAll_Load(object sender, EventArgs e)
        {

        }

        private void textBoxUnitsReworked_TextChanged(object sender, EventArgs e)
        {
            if (textBoxCostofEachUnitReworkRatio.Text != "" )// Controls  FixedEntryValueCheck change of value in Editabletextbox
            {
                if (textBoxUnitsReworked.Text != "")
                {
                    decimal CostofEachUnit = Convert.ToDecimal(textBoxCostofEachUnitReworkRatio.Text);
                    int UnitsReworked = Convert.ToInt32(textBoxUnitsReworked.Text);
                    decimal TotalReworkCost = CostofEachUnit * UnitsReworked;
                    textBoxCostofReworking.Text = TotalReworkCost.ToString();
                }
                else if (textBoxUnitsReworked.Text == "")
                {
                    textBoxCostofReworking.Text = "";
                }
            }





        }

        private void labelTotalUnitsReworked_Click(object sender, EventArgs e)
        {

        }

        private void textBoxPlannedDelivaryCompliance_KeyPress(object sender, KeyPressEventArgs e)
        {
            if ((!char.IsNumber(e.KeyChar)) && (!char.IsControl(e.KeyChar)))
            {

                e.Handled = true;
            }
        }

        private void textBoxAcutalDelivaryCompliance_KeyPress(object sender, KeyPressEventArgs e)
        {
            if ((!char.IsNumber(e.KeyChar)) && (!char.IsControl(e.KeyChar)))
            {

                e.Handled = true;
            }
            if (e.KeyChar == 13)
            {
                buttonDailyEntrySave.PerformClick();
            }
        }

        private void textBoxAcutalDelivaryCompliance_TextChanged(object sender, EventArgs e)
        {
            if (textBoxPlannedDelivaryCompliance.Text != "" && textBoxAcutalDelivaryCompliance.Text != "")
            {
                decimal ComplianceRatio = (Convert.ToDecimal(Convert.ToDecimal(textBoxAcutalDelivaryCompliance.Text) / Convert.ToDecimal(textBoxPlannedDelivaryCompliance.Text)) * 100);
                decimal RoundedPlanned = (Math.Round(ComplianceRatio, 2));
                textBoxDelCompPercnt.Text = (RoundedPlanned.ToString());
            }
            else
                textBoxDelCompPercnt.Text = "";

        }

        private void textBoxPlannedDelivaryCompliance_TextChanged(object sender, EventArgs e)
        {
            if (textBoxPlannedDelivaryCompliance.Text != "" && textBoxAcutalDelivaryCompliance.Text != "")
            {
                decimal ComplianceRatio = (Convert.ToDecimal(Convert.ToDecimal(textBoxAcutalDelivaryCompliance.Text) / Convert.ToDecimal(textBoxPlannedDelivaryCompliance.Text)) * 100);
                decimal RoundedPlanned = (Math.Round(ComplianceRatio, 2));
                textBoxDelCompPercnt.Text = (RoundedPlanned.ToString());
            }
            else
                textBoxDelCompPercnt.Text = "";
        }

        private void textBoxExpectedCustomerClaims_KeyPress(object sender, KeyPressEventArgs e)
        {
            if ((!char.IsNumber(e.KeyChar)) && (!char.IsControl(e.KeyChar)))
            {

                e.Handled = true;
            }
        }

        private void textBoxAcutalCustomerClaims_KeyPress(object sender, KeyPressEventArgs e)
        {
            if ((!char.IsNumber(e.KeyChar)) && (!char.IsControl(e.KeyChar)))
            {

                e.Handled = true;
            }
            if (e.KeyChar == 13)
            {
               // buttonDailyEntrySave.PerformClick();
            }
        }

        private void textBoxUnitsCustomerRejection_KeyPress(object sender, KeyPressEventArgs e)
        {
            if ((!char.IsNumber(e.KeyChar)) && (!char.IsControl(e.KeyChar)))
            {

                e.Handled = true;
            }
            if (e.KeyChar == 13)
            {
              //  buttonDailyEntrySave.PerformClick();
            }
        }

        private void textBoxUnitsCustomerRejection_TextChanged(object sender, EventArgs e)
        {
            if (textBoxCostofEachUnit.Text != "" )// Controls  FixedEntryValueCheck change of value in Editabletextbox
            {
                if (textBoxUnitsCustomerRejection.Text != "")
                {
                    decimal CostofEachUnit = Convert.ToDecimal(textBoxCostofEachUnit.Text);
                    int UnitsReworked = Convert.ToInt32(textBoxUnitsCustomerRejection.Text);
                    decimal TotalReworkCost = CostofEachUnit * UnitsReworked;
                    textBoxCostofCustomerRejection.Text = TotalReworkCost.ToString();
                }
                else if (textBoxUnitsCustomerRejection.Text == "")
                {
                    textBoxCostofCustomerRejection.Text = "";
                }
            }
        }

        private void textBoxUnitsInhouseRejection_KeyPress(object sender, KeyPressEventArgs e)
        {
            if ((!char.IsNumber(e.KeyChar)) && (!char.IsControl(e.KeyChar)))
            {

                e.Handled = true;
            }
            if (e.KeyChar == 13)
            {
             //   buttonDailyEntrySave.PerformClick();
            }
        }

        private void textBoxUnitsInhouseRejection_TextChanged(object sender, EventArgs e)
        {
            if (textBoxCostofEachUnitInhouseRejection.Text != "" &&  textBoxUnitsInhouseRejection.Text != "")// Controls  FixedEntryValueCheck change of value in Editabletextbox
            {
                if (textBoxUnitsInhouseRejection.Text != "")
                {

                    decimal CostofEachUnit = Convert.ToDecimal(textBoxCostofEachUnitInhouseRejection.Text);
                    int UnitsReworked = Convert.ToInt32(textBoxUnitsInhouseRejection.Text);
                    decimal TotalReworkCost = CostofEachUnit * UnitsReworked;
                    textBoxTotalCostofInhouseRejection.Text = TotalReworkCost.ToString();
                }
                else if (textBoxUnitsInhouseRejection.Text == "")
                {
                    textBoxTotalCostofInhouseRejection.Text = "";
                }
            }
        }

        private void textBoxUnitsReworked_KeyPress(object sender, KeyPressEventArgs e)
        {
            if ((!char.IsNumber(e.KeyChar)) && (!char.IsControl(e.KeyChar)))
            {

                e.Handled = true;
            }
            if (e.KeyChar == 13)
            {
            //    buttonDailyEntrySave.PerformClick();
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

        private void checkBoxReport_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBoxReport.Checked == true && Item_Id>0)
            {
                dateTimePickerFrom.Enabled = true;
                dateTimePickerTo.Enabled = true;
                buttonExport.Enabled = true;
                labelReportisGenerating.Visible = false;
            }
            else if(checkBoxReport.Checked == true && Item_Id<=0)
            {
                labelPleaseSelectItem.Visible=true;
                labelReportisGenerating.Visible = false;
            }
            else 
            {


                dateTimePickerFrom.Enabled = false;
                dateTimePickerTo.Enabled = false;
                buttonExport.Enabled = false;
                labelReportisGenerating.Visible = false;

            }
        }

        private void buttonExport_Click(object sender, EventArgs e)
        {

            string Path = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            DataSet ds = new DataSet();
            if (dateTimePickerFrom.Value < dateTimePickerTo.Value)
            {
                ds = ButtonsUtility.GetOverAllReportDetails(Item_Id, dateTimePickerFrom.Text, dateTimePickerTo.Text);
                //  Reports.ExportDataSetToExcel(dt, Application.StartupPath);
                labelReportisGenerating.Visible = true;
                Reports.ExportDataSetToExcelOverAllReport(ds, Path, comboBoxItemsDailyEntry.Text);
                labelReportisGenerating.Visible = false;
            }
            else
            {
                DialogResult dialog = MessageBox.Show("From Date is always less then To Date. ", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void dateTimePickerFrom_ValueChanged(object sender, EventArgs e)
        {
            labelReportisGenerating.Visible = false;
        }

        private void dateTimePickerTo_ValueChanged(object sender, EventArgs e)
        {
            labelReportisGenerating.Visible = false;
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

        private void buttonProductionEfficeinecy_Click(object sender, EventArgs e)
        {
            DailyEntry_ProductionEfficiency DaiyEntryProdEff = new DailyEntry_ProductionEfficiency(0);
            DaiyEntryProdEff.Show();
            this.Close();
        }

        private void buttonProdAchiv_Click(object sender, EventArgs e)
        {
            DailyEntry_ProductionAchievement ProdAchiv = new DailyEntry_ProductionAchievement(0);
            ProdAchiv.Show();
            this.Close();
        }

        private void buttonEqipFailureRate_Click(object sender, EventArgs e)
        {
            DailyEntry_Machine dailtEntryMachine = new DailyEntry_Machine(0);
            dailtEntryMachine.Show();
            this.Close();
        }

        private void buttonMatYieldVar_Click(object sender, EventArgs e)
        {
            DailyEntry_Material DailyEntry = new DailyEntry_Material(0);
            DailyEntry.Show();
            this.Close();
        }

        private  DataTable GetMaterialDetails(string Code,int idItem, string date)
        {
            DataTable dt = new DataTable();
            DataTable dtdaily = new DataTable();
            //dt = ButtonsUtility.GetMaterials(Code, idItem);

            dtdaily = ButtonsUtility.GetMaterialsDaily(Code, idItem, date);
            return dtdaily;


            
        }
        private void buttonImport_Click(object sender, EventArgs e)
        {
            if(!string.IsNullOrEmpty( textBoxPath.Text))
            { 
                string pathCon = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + textBoxPath.Text + "; Extended Properties=\"Excel 8.0;HDR=Yes\";";
                OleDbConnection conn = new OleDbConnection(pathCon);
                OleDbDataAdapter adapter = new OleDbDataAdapter("Select * from [EntrySheet$]", conn);

                DataTable dt = new DataTable();
            
                    adapter.Fill(dt);

              
                    DataTable dtShiftTime = new DataTable();
                    dtShiftTime = ButtonsUtility.GetShiftTime();

                    foreach (DataRow row in dt.Rows)
                    {
                        int item_id = 0;
                        DataTable dtCost = new DataTable();
                        string date = string.Empty;
                        int TotalUnits = 0;
                        if (string.IsNullOrEmpty(row[0].ToString()))
                        {
                            MessageBox.Show("Please Enter Item Name in the Sheet.", "Item Name Missing");
                            break;
                        }
                        else
                        {
                            item_id = ButtonsUtility.GetITemId(row[0].ToString());
                            dtCost = ButtonsUtility.Getcost(item_id);
                        }

                        decimal itemCost = Convert.ToDecimal(dtCost.Rows[0][0]);
                        decimal itemReworkCost = Convert.ToDecimal(dtCost.Rows[0][1]); ;
                        decimal itemInhouseRejectionCost = Convert.ToDecimal(dtCost.Rows[0][2]); ;

                        if (string.IsNullOrEmpty(row[1].ToString()))
                        {
                            MessageBox.Show("Please Enter Date in the Sheet.", "Date Missing");
                            break;
                        }
                        else
                        {
                            date = (Convert.ToDateTime(row[1])).ToString("yyyy-MM-dd");
                        }
                        if (string.IsNullOrEmpty(row[2].ToString()))
                        {
                            MessageBox.Show("Please Enter Total Units Made in the Sheet.", "Total Units Made Missing");
                            break;
                        }
                        else
                        {
                            TotalUnits = Convert.ToInt32(row[2]);
                            ButtonsUtility.SaveDailyEntry(item_id, TotalUnits, TotalUnits * itemCost, date);
                        }

                        ////Production Achievement Rate///

                        if (!string.IsNullOrEmpty(row[3].ToString()))
                        {
                            decimal productionAchivementRate = Convert.ToDecimal(Convert.ToDecimal(row[2]) / Convert.ToDecimal(row[3])) * 100;
                            decimal roundedProductionAchivementRate = (Math.Round(productionAchivementRate, 2));


                            ButtonsUtility.SaveDailyEntryProductionAchievement(item_id, Convert.ToInt32(row[3].ToString()), Convert.ToInt32(row[2].ToString()), roundedProductionAchivementRate, date);
                        }
                        else
                        {
                            MessageBox.Show("Please add both Planned Production Data.", "Planned Production Information Missing");
                            break;
                        }
                        ////Production Achievement Rate///

                        if (!string.IsNullOrEmpty(row[4].ToString()))
                        {
                            int unitsReworked = Convert.ToInt32(row[4]);
                            ButtonsUtility.SaveDailyEntryofKPI(item_id, unitsReworked, unitsReworked * itemReworkCost, date, "ReworkRatio");
                        }
                        if (!string.IsNullOrEmpty(row[5].ToString()))
                        {
                            int unitsInhouseRejection = Convert.ToInt32(row[5]);
                            ButtonsUtility.SaveDailyEntryofKPI(item_id, unitsInhouseRejection, unitsInhouseRejection * itemInhouseRejectionCost, date, "InhouseRatio");
                        }
                        if (!string.IsNullOrEmpty(row[6].ToString()))
                        {
                            int unitsCustomerRejection = Convert.ToInt32(row[6]);
                            ButtonsUtility.SaveDailyEntryofKPI(item_id, unitsCustomerRejection, unitsCustomerRejection * itemCost, date, "CustomerRejection");
                        }
                        if (!string.IsNullOrEmpty(row[7].ToString()) || !string.IsNullOrEmpty(row[8].ToString()))
                        {
                            if (!string.IsNullOrEmpty(row[8].ToString()) && !string.IsNullOrEmpty(row[7].ToString()))
                            {
                                ButtonsUtility.SaveDailyEntryCustomerClaims(item_id, Convert.ToInt32(row[7]), Convert.ToInt32(row[8]), date);

                            }
                            else
                            {
                                MessageBox.Show("Please add both Actual Customer Claims and Expected Customer Claims If you want to store Customer Claims Data.", "Customer Claims Information Missing");

                            }
                        }

                        if (!string.IsNullOrEmpty(row[9].ToString()) || !string.IsNullOrEmpty(row[10].ToString()))
                        {
                            if (!string.IsNullOrEmpty(row[9].ToString()) && !string.IsNullOrEmpty(row[10].ToString()))
                            {
                                decimal complianceRatio = (Convert.ToDecimal(Convert.ToDecimal(row[10]) / Convert.ToDecimal(row[9]) * 100));
                                decimal roundedPlanned = (Math.Round(complianceRatio, 2));
                                ButtonsUtility.SaveDailyEntryDelCompliance(item_id, Convert.ToInt32(row[9]), Convert.ToInt32(row[10]), roundedPlanned, date);
                            }
                            else
                            {
                                MessageBox.Show("Please add both Planned Delivery and Actual Delivery If you want to store Delivery Complaince Data.", "Delivery Complaince Information Missing");

                            }
                        }




                        ////////////////Daily Entry Machine////////////////
                        DataTable dtMachineInfo = new DataTable();
                        if (!string.IsNullOrEmpty(row[11].ToString()) || !string.IsNullOrEmpty(row[12].ToString()) || !string.IsNullOrEmpty(row[13].ToString()) || !string.IsNullOrEmpty(row[14].ToString()))
                        {
                            if (!string.IsNullOrEmpty(row[11].ToString()) && !string.IsNullOrEmpty(row[12].ToString()) && !string.IsNullOrEmpty(row[13].ToString()) && !string.IsNullOrEmpty(row[14].ToString()))
                            {
                                int workShopId = ButtonsUtility.GetWorkShopId(row[11].ToString());
                                dtMachineInfo = ButtonsUtility.GetMachineId(row[12].ToString(), true);

                                DataTable dtProcessInfo = ButtonsUtility.GetProcessIdAndCapacity(row[13].ToString(), item_id);



                                decimal ratio = (Convert.ToDecimal(row[13]) / Convert.ToDecimal(dtShiftTime.Rows[0][0])) * 100;
                                decimal ratioDailyTimeEntry = (Math.Round(ratio, 2));


                                ButtonsUtility.SaveMachineDailyDownTime(Convert.ToInt32(dtMachineInfo.Rows[0][0]), workShopId, Convert.ToDecimal(row[14]), ratioDailyTimeEntry, date, item_id, Convert.ToInt32(dtProcessInfo.Rows[0][0]));

                            }
                            else
                            {

                                MessageBox.Show("Please add Workshop name, Machine Name, Machine Process,Down Time for Equipment Failure Rate Entry.", "Equipment Failure Rate Information Missing");
                            }
                        }



                        ////////// daily entry machine/////


                        ///// daily entry production efficiency///

                        if (!string.IsNullOrEmpty(row[15].ToString()) || !string.IsNullOrEmpty(row[16].ToString()) || !string.IsNullOrEmpty(row[17].ToString()) || !string.IsNullOrEmpty(row[18].ToString()))
                        {
                            if (!string.IsNullOrEmpty(row[15].ToString()) && !string.IsNullOrEmpty(row[16].ToString()) && !string.IsNullOrEmpty(row[17].ToString()) && !string.IsNullOrEmpty(row[18].ToString()))
                            {
                                DataTable dtProcessInfoProdEffi = ButtonsUtility.GetProcessIdAndCapacity(row[15].ToString(), item_id);

                                decimal acutalManhour = (Convert.ToInt32(row[17]) * Convert.ToDecimal(row[18]));
                                decimal ratioRoundedActualManhour = (Math.Round(acutalManhour, 2));
                                decimal roundedActualManhours = ratioRoundedActualManhour;

                                decimal capacity = Convert.ToDecimal(dtProcessInfoProdEffi.Rows[0][1]);

                                decimal plannedManHour = ((1 / capacity) * (Convert.ToInt32(row[16])));
                                decimal roundedPlannedManHours = (Math.Round(plannedManHour, 2));
                                decimal roundedPlannedManHour = roundedPlannedManHours;


                                decimal efficiency = (roundedPlannedManHour * 100) / (roundedActualManhours);
                                decimal ratioRoundedEffi = (Math.Round(efficiency, 2));
                                decimal effiDailyProdEffic = ratioRoundedEffi;

                                ButtonsUtility.SaveDailyEntryProcess(item_id, Convert.ToInt32(dtProcessInfoProdEffi.Rows[0][0]), Convert.ToInt32(row[16]), Convert.ToInt32(row[17]), Convert.ToDecimal(row[18]), date, roundedActualManhours, roundedPlannedManHour, effiDailyProdEffic);

                            }
                            else
                            {
                                MessageBox.Show("Please add Process name, Volume of Process, Actual No of Operators,Hours Worked for Production Efficiency Calculation.", "Production Efficiency Information Missing");
                            }
                        }

                        //////// daily entry production efficiency///



                        ///// daily entry material

                        if (!string.IsNullOrEmpty(row[12].ToString()) || !string.IsNullOrEmpty(row[19].ToString()) || !string.IsNullOrEmpty(row[20].ToString()))
                        {
                            if (!string.IsNullOrEmpty(row[12].ToString()) && !string.IsNullOrEmpty(row[19].ToString()) && !string.IsNullOrEmpty(row[20].ToString()))
                            {
                                DataTable dtMaterialInfo = ButtonsUtility.GetMaterialInfoDetailsbyName(row[19].ToString());

                                DataTable dtMaterialDailyData = GetMaterialDetails(dtMachineInfo.Rows[0][3].ToString(), item_id, date);
                                decimal actualCost = ButtonsUtility.CalculateTotalCostofMaterial(Convert.ToDecimal(dtMaterialDailyData.Rows[0][4].ToString()), Convert.ToDecimal(dtMaterialDailyData.Rows[0][3].ToString()), Convert.ToDecimal(row[20]));

                                decimal plannedMaterialUsed = (Convert.ToDecimal(dtMaterialDailyData.Rows[0][1].ToString()) * Convert.ToInt32(row[2]));

                                decimal plannedCost = ((Convert.ToInt32(row[2])) * (Convert.ToDecimal(dtMaterialDailyData.Rows[0][2])));

                                ButtonsUtility.SaveDailyEntryMaterial(item_id, Convert.ToInt32(dtMaterialInfo.Rows[0][0]), Convert.ToDecimal(row[20]), actualCost, plannedMaterialUsed, plannedCost, date);

                            }

                            else
                            {
                                MessageBox.Show("Please add Machine name, Material Name, Total Material Used for Material Yield Variance Entry.", "Material Yield Variance Information Missing");
                            }

                        }
                        ///// daily entry material
                        //if(dt.Rows.Count>0)
                        //{
                        //    textBoxTotalUnits.Text = dt.Rows[0][0].ToString();
                        //    textBoxUnitsReworked.Text = dt.Rows[0][1].ToString();
                        //    textBoxUnitsInhouseRejection.Text = dt.Rows[0][2].ToString();
                        //    textBoxUnitsCustomerRejection.Text = dt.Rows[0][3].ToString();
                        //    textBoxExpectedCustomerClaims.Text = dt.Rows[0][4].ToString();
                        //    textBoxAcutalCustomerClaims.Text = dt.Rows[0][5].ToString();
                        //    textBoxPlannedDelivaryCompliance.Text = dt.Rows[0][6].ToString();
                        //    textBoxAcutalDelivaryCompliance.Text = dt.Rows[0][7].ToString();
                        //}
                    }
            }
            else
            {
                MessageBox.Show("Please Select a xls file first.", "File Error");
            }
        }

        private void buttonBrowse_Click(object sender, EventArgs e)
        {
            OpenFileDialog FileDialog = new OpenFileDialog();
            if (FileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                this.textBoxPath.Text = FileDialog.FileName;
            }
        }  
    }
}
