using BloodBankManagementSystem.BLL;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BloodBankManagementSystem.DAL
{
    public class userDAL
    {
        
        //Create Static String to Connect Database
        private IDbConnection conn;
        private IDbDataAdapter adapter;
        //Create a Connection String to Connect Database
        public userDAL(IDbConnection conn, IDbDataAdapter adapter)
        {
            this.conn = conn;
            this.adapter = adapter;
        }
        public userDAL()
        {
            string myconnstrng = ConfigurationManager.ConnectionStrings["connstrng"].ConnectionString;

            //Connecting DAtabase
            conn = new SqlConnection(myconnstrng);
            adapter = new SqlDataAdapter();
        }

        #region SELECT data from database
        public DataTable Select()
        {
            //Create a DataTable to Hold the Data from Database
            var dataSet = new DataSet();

            try
            {
                // WRite SQL Qery to Get Data from Database
                string sql = "SELECT * FROM tbl_users";

                //Create SQL Command to Execute Query
                var cmd = conn.CreateCommand();
                cmd.CommandText = sql;
                cmd.Connection = conn;

                //Open Database Connection
                conn.Open();

                //Transfer Data from SqlData Adapter to DataTable
                adapter.Fill(dataSet);
            }
            catch(Exception ex)
            {
                //Display Error Message if there's any exceptional errors
                MessageBox.Show(ex.Message);
            }
            finally
            {
                //Close Database Connection
                conn.Close();
            }

            DataTable myTable = dataSet.Tables[0];
            return myTable;
        }
        #endregion

        #region Insert Data into Database for User Module
        public bool Insert(userBLL u)
        {
            //Create a boolean variable and set its default value to false
            bool isSuccess = false;
            

            try
            {
                //Create a String Variable to Store the INSERT Query
                String sql = "INSERT INTO tbl_users(username, email, password, full_name, contact, address, added_date, image_name) VALUES (@username, @email, @password, @full_name, @contact, @address, @added_date, @image_name)";

                //Create a SQL Command to pass the value in our query
                var cmd = conn.CreateCommand();
                cmd.CommandText = sql;
                cmd.Connection = conn;
                
                //Create the Parameter to pass get the value from UI and pass it on SQL Query above
                var usernameParameter = cmd.CreateParameter();
                usernameParameter.ParameterName = "@username";
                usernameParameter.Value = u.username;
                
                var emailParameter = cmd.CreateParameter();
                emailParameter.ParameterName = "@email";
                emailParameter.Value = u.email;
                
                var passwordParameter = cmd.CreateParameter();
                passwordParameter.ParameterName = "@password";
                passwordParameter.Value = u.password;
                
                var full_nameParameter = cmd.CreateParameter();
                full_nameParameter.ParameterName = "@full_name";
                full_nameParameter.Value = u.full_name;
                
                var contactParameter = cmd.CreateParameter();
                contactParameter.ParameterName = "@contact";
                contactParameter.Value = u.contact;
                
                var addressParameter = cmd.CreateParameter();
                addressParameter.ParameterName = "@address";
                addressParameter.Value = u.address;
                
                var added_dateParameter = cmd.CreateParameter();
                added_dateParameter.ParameterName = "@added_date";
                added_dateParameter.Value = u.added_date;
                
                var image_nameParameter = cmd.CreateParameter();
                image_nameParameter.ParameterName = "@image_name";
                image_nameParameter.Value = u.image_name;
                

                //Open Database Connection
                conn.Open();

                //Create an Integer VAriable to hold the value after the query is executed
                int rows = cmd.ExecuteNonQuery();

                //The value of rows will be greater than 0 if the query is executed successfully
                //Else it'll be 0

                if(rows>0)
                {
                    //Query Executed Successfully
                    isSuccess = true;
                }
                else
                {
                    //FAiled to Execute Query
                    isSuccess = false;
                }
            }
            catch(Exception ex)
            {
                //DIsplay Error Message if there's any exceptional errors
                MessageBox.Show(ex.Message);
            }
            finally
            {
                //Close Database Connection
                conn.Close();
            }

            return isSuccess;
        }
        #endregion

        #region UPDATE data in database (User Module)
        public bool Update(userBLL u)
        {
            //Create a Boolean variable and set its default value to false
            bool isSuccess = false;

            try
            {
                //Create a string variable to hold the sql query
                string sql = "UPDATE tbl_users SET username=@username, email=@email, password=@password, full_name=@full_name, contact=@contact, address=@address, added_date=@added_date, image_name=@image_name WHERE user_id=@user_id";

                //Create Sql Command to execute query and also pass the values to sql query
                var cmd = conn.CreateCommand();
                cmd.CommandText = sql;
                cmd.Connection = conn;
                
                //Now Pass the values to SQL Query
                var usernameParameter = cmd.CreateParameter();
                usernameParameter.ParameterName = "@username";
                usernameParameter.Value = u.username;
                
                var emailParameter = cmd.CreateParameter();
                emailParameter.ParameterName = "@email";
                emailParameter.Value = u.email;
                
                var passwordParameter = cmd.CreateParameter();
                passwordParameter.ParameterName = "@password";
                passwordParameter.Value = u.password;
                
                var full_nameParameter = cmd.CreateParameter();
                full_nameParameter.ParameterName = "@full_name";
                full_nameParameter.Value = u.full_name;
                
                var contactParameter = cmd.CreateParameter();
                contactParameter.ParameterName = "@contact";
                contactParameter.Value = u.contact;
                
                var addressParameter = cmd.CreateParameter();
                addressParameter.ParameterName = "@address";
                addressParameter.Value = u.address;
                
                var added_dateParameter = cmd.CreateParameter();
                added_dateParameter.ParameterName = "@added_date";
                added_dateParameter.Value = u.added_date;
                
                var image_nameParameter = cmd.CreateParameter();
                image_nameParameter.ParameterName = "@image_name";
                image_nameParameter.Value = u.image_name;
                
                var user_idParameter = cmd.CreateParameter();
                user_idParameter.ParameterName = "@user_id";
                user_idParameter.Value = u.user_id;
                
                //open Database Connection
                conn.Open();

                //Create an integer variable to hold the value after the query is executed
                int rows = cmd.ExecuteNonQuery();

                //If the query is executed successfully then the value of rows will be greater than 0
                //else it'll be 0

                if(rows>0)
                {
                    //Query Executed Successfully
                    isSuccess = true;

                }
                else
                {
                    //Failed to Execute Query
                    isSuccess = false;
                }
            }
            catch(Exception ex)
            {
                //Display error message if there's any exceptional error
                MessageBox.Show(ex.Message);
            }
            finally
            {
                //Close Database Connection
                conn.Close();
            }

            return isSuccess;
        }
        #endregion

        #region Delete Data from Database (User Module)
        public bool Delete(userBLL u)
        {
            //Create a boolean variable and set its default value to false
            bool isSuccess = false;

            try
            {
                //Create a string variable to hold the sql query to delete data
                String sql = "DELETE FROM tbl_users WHERE user_id=@user_id";

                //Create Sql Command to Execute the Query
                var cmd = conn.CreateCommand();
                cmd.CommandText = sql;
                cmd.Connection = conn;
                
                //Pass the value thorugh parameters
                var user_idParameter = cmd.CreateParameter();
                user_idParameter.ParameterName = "@user_id";
                user_idParameter.Value = u.user_id;

                //Open the DAtabase Connection
                conn.Open();

                //Create an integer variable to hold the value after query is executed
                int rows = cmd.ExecuteNonQuery();

                //If the query is executed Successfully then the value of rows will be greater than zero(0)
                //Else it'll be zero(0)

                if(rows>0)
                {
                    //Query executed Successfully
                    isSuccess = true;
                }
                else
                {
                    //Failed to Execute Query
                    isSuccess = false;
                }
            }
            catch(Exception ex)
            {
                //Display Error Message if there's any Excetionl errors
                MessageBox.Show(ex.Message);
            }
            finally
            {

                //CLose Database Connection
                conn.Close();
            }

            return isSuccess;
        }
        #endregion

        #region SEARCH
        public DataTable Search(string keywords)
        {

            //Dataset to Hold the data from database temporarily
            var dataSet = new DataSet();

            //Write the Code to SEarh the Users
            try
            {
                // Write the SQL Query to SEarch the User from DAtabaes
                String sql = "SELECT * FROM tbl_users WHERE user_id LIKE '%" + keywords + "%' OR full_name LIKE '%" + keywords + "%' OR address LIKE '%" + keywords + "%'";

                //Create SQL Command to Execute the Query
                var cmd = conn.CreateCommand();
                cmd.CommandText = sql;
                cmd.Connection = conn;
                
                adapter.SelectCommand = cmd;

                //open Database Connetion
                conn.Open();
                
                //Pass the data from adapter to dataTable
                adapter.Fill(dataSet); 
            }
            catch(Exception ex)
            {
                //Display Error Messages if there's any exceptional errors
                MessageBox.Show(ex.Message);
            }
            finally
            {
                //Close the DAtabase Connection
                conn.Close();
            }

            DataTable myTable = dataSet.Tables[0];
            return myTable;
        }
        #endregion


        #region
        public userBLL GetIDFromUsername(string username)
        {
            userBLL u = new userBLL();
            
            //Dataset to Hold the data from database temporarily
            var dataSet = new DataSet();

            try
            {
                //SQL Query to get the ID from USERNAME
                string sql = "SELECT user_id FROM tbl_users WHERE username='"+ username +"'";

                var cmd = conn.CreateCommand();
                cmd.CommandText = sql;
                cmd.Connection = conn;
                
                adapter.SelectCommand = cmd;

                //Open Database Connection
                conn.Open();

                //Fill the data in dataTable from Adapter
                var usersFound = adapter.Fill(dataSet);

                //If there's user based on the username then get the user_id
                if(usersFound > 0)
                {
                    u.user_id = int.Parse(dataSet.Tables[0].Rows[0]["user_id"].ToString());
                }
            }
            catch(Exception ex)
            {
                //Display Error Message if there's any exceptional errors
                MessageBox.Show(ex.Message);
            }
            finally
            {
                //Close Database Connection
                conn.Close();
            }

            return u;
        }
        #endregion
    }
}
