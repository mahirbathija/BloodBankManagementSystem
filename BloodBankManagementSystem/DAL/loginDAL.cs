using BloodBankManagementSystem.BLL;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BloodBankManagementSystem.DAL
{
    public class loginDAL
    {
        //Create Static String to Connect Database
        static string myconnstrng = ConfigurationManager.ConnectionStrings["connstrng"].ConnectionString;

        private IDbConnection conn;
        private IDbDataAdapter adapter;

        public loginDAL()
        {
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
                string sql = "SELECT * FROM tbl_users WHERE username=@username AND password=@password";

                IDbCommand cmd = conn.CreateCommand();
                //Create SQL Command to Pass the value to SQL Query
                cmd.CommandText = sql;
                cmd.Connection = conn;


                //Pass the value to SQL Query Using Parameters
                IDbDataParameter userParameter = cmd.CreateParameter();
                userParameter.ParameterName = "@username";
                userParameter.Value = l.username;
                cmd.Parameters.Add(userParameter);
                
                IDbDataParameter passwordParameter = cmd.CreateParameter();
                passwordParameter.ParameterName = "@password";
                passwordParameter.Value = l.password;
                cmd.Parameters.Add(passwordParameter);
                
                //SQl Data Adapeter to Get the Data from Database
                adapter.SelectCommand = cmd;

                //Dataset to Hold the data from database temporarily
                DataSet s = new DataSet();
                
                //Filld the data from adapter to dt
                adapter.Fill(s);
                foreach (DataTable table in s.Tables)
                {
                    //Chekc whether user exists or not
                    if(table.Rows.Count>0)
                    {
                        //User Exists and Login Successful
                        isSuccess = true;
                    }
                    else
                    {
                        //Login Failed
                        isSuccess = false;
                    }
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
