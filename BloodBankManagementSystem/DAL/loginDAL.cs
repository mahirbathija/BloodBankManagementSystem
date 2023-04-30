using BloodBankManagementSystem.BLL;
using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace BloodBankManagementSystem.DAL
{
    public class loginDAL
    {
        //Create Static String to Connect Database

        private IDbConnection conn;
        private IDbDataAdapter adapter;

        public loginDAL()
        {
            string myconnstrng = ConfigurationManager.ConnectionStrings["connstrng"].ConnectionString;
            
            //Connecting DAtabase
            conn = new SqlConnection(myconnstrng);
            adapter = new SqlDataAdapter();
        }

        public loginDAL(IDbConnection conn, IDbDataAdapter adapter)
        {
            this.conn = conn;
            this.adapter = adapter;
        }
        

        public bool loginCheck(loginBLL l)
        {
            //Create a Boolean Variable and SEt its default value to false
            bool isSuccess = false;

            try
            {
                //SQL Query to Check Login BAsed on Usename and Password
                var sql = "SELECT * FROM tbl_users WHERE username=@username AND password=@password";

                //Create SQL Command to Pass the value to SQL Query
                var cmd = conn.CreateCommand();
                cmd.CommandText = sql;
                cmd.Connection = conn;


                //Pass the value to SQL Query Using Parameters
                var userParameter = cmd.CreateParameter();
                userParameter.ParameterName = "@username";
                userParameter.Value = l.username;
                cmd.Parameters.Add(userParameter);
                
                var passwordParameter = cmd.CreateParameter();
                passwordParameter.ParameterName = "@password";
                passwordParameter.Value = l.password;
                cmd.Parameters.Add(passwordParameter);
                
                //SQl Data Adapter to Get the Data from Database
                adapter.SelectCommand = cmd;

                //Dataset to Hold the data from database temporarily
                var dataSet = new DataSet();
                
                //Fill the data from adapter to dataset
                var usersFound = adapter.Fill(dataSet);
                
                //Check whether user exists or not
                if(usersFound == 1)
                {
                    //User Exists and Login Successful
                    isSuccess = true;
                }
            }
            catch(Exception ex)
            {
                //Display Error Message if there's any Exception Errors
                MessageBox.Show(ex.Message);
            }
            finally
            {
                //Close Database Connection
                conn.Close();
            }

            return isSuccess;
        }
    }
}
