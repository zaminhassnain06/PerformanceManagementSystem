using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

namespace PMS
{
    public partial class LoginForm : Form
    {
        public LoginForm()
        {
            InitializeComponent();
        }

        private void buttonExit_Click(object sender, EventArgs e) 
        {
            ButtonsUtility.ExitProgram();
        }

        private void Login_Click(object sender, EventArgs e)
        {
            

             if(textBoxUserName.Text=="" ) //if both test name and password are missing
             {

                 labelUserNameMissing.Visible = true;
                 if (textBoxPassword.Text == "")
                 {
                     labelPasswordMisssing.Visible = true;
                 }
             }
             else if (textBoxPassword.Text == "")
             {
                 labelUserNameMissing.Visible = false;
                 labelPasswordMisssing.Visible = true;
             }
             else {
                 labelUserNameMissing.Visible = false;
                 labelPasswordMisssing.Visible = false;
                 ButtonsUtility.Login(textBoxUserName.Text.Trim(), textBoxPassword.Text.Trim(),this);
                 //SuccessfulLogin();
             }

             
            
        }
        public void LoginFailed()
        {
         
           
            labelLoginFailedMessage.Visible = true;
           
        }
        public void SuccessfulLogin()
        {
            this.Close();
        }

        private void textBoxPassword_TextChanged(object sender, EventArgs e)
        {
            
        }

        private void textBoxPassword_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                Login.PerformClick();
            }
        }

        

        
       
    }
}
