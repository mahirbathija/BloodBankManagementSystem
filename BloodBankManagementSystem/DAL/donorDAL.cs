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
    class donorDAL
    {
        //Create Static String to Connect Database
        private IDbConnection conn;
        private IDbDataAdapter adapter;
        //Create a Connection String to Connect Database
        public donorDAL(IDbConnection conn, IDbDataAdapter adapter)
        {
            this.conn = conn;
            this.adapter = adapter;
        }
        public donorDAL()
        {
            string myconnstrng = ConfigurationManager.ConnectionStrings["connstrng"].ConnectionString;

            //Connecting DAtabase
            conn = new SqlConnection(myconnstrng);
            adapter = new SqlDataAdapter();
        }

        #region SELECT to display data in DataGridView from database
        public DataTable Select()
        {
            //Dataset to Hold the data from database temporarily
            var dataSet = new DataSet();


            try
            {
                //Write SQL Query to SElect the DAta from DAtabase
                string sql = "SELECT * FROM tbl_donors";

                var cmd = conn.CreateCommand();
                cmd.CommandText = sql;
                cmd.Connection = conn;

                //open Database Connection
                conn.Open();

                //SQl Data Adapter to Get the Data from Database
                adapter.SelectCommand = cmd;

                

                //Fill the data from adapter to dataset
               adapter.Fill(dataSet); 

            }
            catch(Exception ex)
            {
                //Display Message if there's any Exceptional Errors
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
        #region INSERT data to database
        public bool Insert(donorBLL d)
        {
            //Create a Boolean Variable and SEt its default value to false
            bool isSuccess = false;


            try
            {
                //Write the Query to INSERT data into database
                string sql = "INSERT INTO tbl_donors (first_name, last_name, email, contact, gender, address, blood_group, added_date, image_name, added_by) VALUES (@first_name, @last_name, @email, @contact, @gender, @address, @blood_group, @added_date, @image_name, @added_by)";

                var cmd = conn.CreateCommand();
                cmd.CommandText = sql;
                cmd.Connection = conn;


                //Pass the value to SQL Query Using Parameters
                var first_nameParameter = cmd.CreateParameter();
                first_nameParameter.ParameterName = "@first_name";
                first_nameParameter.Value = d.first_name;
                cmd.Parameters.Add(first_nameParameter);

                var last_nameParameter = cmd.CreateParameter();
                last_nameParameter.ParameterName = "@last_name";
                last_nameParameter.Value = d.last_name;
                cmd.Parameters.Add(last_nameParameter);

               
                var emailParameter = cmd.CreateParameter();
                emailParameter.ParameterName = "@email";
                emailParameter.Value = d.email;
                cmd.Parameters.Add(emailParameter);

               
                var contactParameter = cmd.CreateParameter();
                contactParameter.ParameterName = "@contact";
                contactParameter.Value = d.contact;
                cmd.Parameters.Add(contactParameter);

                
                var genderParameter = cmd.CreateParameter();
                genderParameter.ParameterName = "@gender";
                genderParameter.Value = d.gender;
                cmd.Parameters.Add(genderParameter);
                
                
                var addressParameter = cmd.CreateParameter();
                addressParameter.ParameterName = "@address";
                addressParameter.Value = d.address;
                cmd.Parameters.Add(addressParameter);

               
                var blood_groupParameter = cmd.CreateParameter();
                blood_groupParameter.ParameterName = "@blood_group";
                blood_groupParameter.Value = d.blood_group;
                cmd.Parameters.Add(blood_groupParameter);


                
                var added_dateParameter = cmd.CreateParameter();
                added_dateParameter.ParameterName = "@added_date";
                added_dateParameter.Value = d.added_date;
                cmd.Parameters.Add(added_dateParameter);

                
                var imageParameter = cmd.CreateParameter();
                imageParameter.ParameterName = "@image_name";
                imageParameter.Value = d.image_name;
                cmd.Parameters.Add(imageParameter);


                
                var added_byParameter = cmd.CreateParameter();
                added_byParameter.ParameterName = "@added_by";
                added_byParameter.Value = d.added_by;
                cmd.Parameters.Add(added_byParameter);


                //Open DAtabase Connection
                conn.Open();

                //SQl Data Adapter to Get the Data from Database
                adapter.SelectCommand = cmd;

                

                //Create an Integer Variable to Check whether the query was executed Successfully or Not
                int rows = cmd.ExecuteNonQuery();

                //If the Query is Executed Successfully the value of rows willb e greater than Zero else it will be Zero
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
                //Display Error Message if there's any Exceptional Errors
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
        #region UPDATE donors in DAtabase
        public bool Update(donorBLL d)
        {
            //Create a Boolean Variable and SEt its Default Value to FAlse
            bool isSuccess = false;
          

            try
            {
                //Create a SQL Query to Update Donors
                string sql = "UPDATE tbl_donors SET first_name=@first_name, last_name=@last_name, email=@email, contact=@contact, gender=@gender, address=@address, blood_group=@blood_group, image_name=@image_name, added_by=@added_by WHERE donor_id=@donor_id";


                var cmd = conn.CreateCommand();
                cmd.CommandText = sql;
                cmd.Connection = conn;


                
                var first_nameParameter = cmd.CreateParameter();
                first_nameParameter.ParameterName = "@first_name";
                first_nameParameter.Value = d.first_name;
                cmd.Parameters.Add(first_nameParameter);

               
                var last_nameParameter = cmd.CreateParameter();
                last_nameParameter.ParameterName = "@last_name";
                last_nameParameter.Value = d.last_name;
                cmd.Parameters.Add(last_nameParameter);

               
                var emailParameter = cmd.CreateParameter();
                emailParameter.ParameterName = "@email";
                emailParameter.Value = d.email;
                cmd.Parameters.Add(emailParameter);

                
                var contactParameter = cmd.CreateParameter();
                contactParameter.ParameterName = "@contact";
                contactParameter.Value = d.contact;
                cmd.Parameters.Add(contactParameter);

                
                var genderParameter = cmd.CreateParameter();
                genderParameter.ParameterName = "@gender";
                genderParameter.Value = d.gender;
                cmd.Parameters.Add(genderParameter);

                
                var addressParameter = cmd.CreateParameter();
                addressParameter.ParameterName = "@address";
                addressParameter.Value = d.address;
                cmd.Parameters.Add(addressParameter);

                
                var blood_groupParameter = cmd.CreateParameter();
                blood_groupParameter.ParameterName = "@blood_group";
                blood_groupParameter.Value = d.blood_group;
                cmd.Parameters.Add(blood_groupParameter);


               
                var donor_idParameter = cmd.CreateParameter();
                donor_idParameter.ParameterName = "@donor_id";
                donor_idParameter.Value = d.donor_id;
                cmd.Parameters.Add(donor_idParameter);

               
                var imageParameter = cmd.CreateParameter();
                imageParameter.ParameterName = "@image_name";
                imageParameter.Value = d.image_name;
                cmd.Parameters.Add(imageParameter);


               
                var added_byParameter = cmd.CreateParameter();
                added_byParameter.ParameterName = "@added_by";
                added_byParameter.Value = d.added_by;
                cmd.Parameters.Add(added_byParameter);

      

                //Open Database Connection
                conn.Open();

                //Create an Integer Variable to check whether the query executed Successfully or not
                int rows = cmd.ExecuteNonQuery();

                //If the Query is Executed Successfully then its value will be greater than 0 else it will be 0
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
                //Display Error Message if there's any exceptional errors
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
        #region DELETE donros from Database
        public bool Delete(donorBLL d)
        {
            //Create a Boolean variable and set its default value to false
            bool isSuccess = false;
            try
            {
                //Write the Query to Delete Donors from Database
                string sql = "DELETE FROM tbl_donors WHERE donor_id=@donor_id";

                var cmd = conn.CreateCommand();
                cmd.CommandText = sql;
                cmd.Connection = conn;


                //Pass the value to SQL Query Using Parameters
                var donorParameter = cmd.CreateParameter();
                donorParameter.ParameterName = "@donor_id";
                donorParameter.Value = d.donor_id;
                cmd.Parameters.Add(donorParameter);

                //Open Database Connection
                conn.Open();

                //Create an Integer Variable to check whether the query executed Successfully or not
                int rows = cmd.ExecuteNonQuery();


                //If the Query executed succesfully then the value of rows will be greater than 0 else it will be 0
                if (rows>0)
                {
                    //Query Executed Successfully
                    isSuccess = true;
                }
                else
                {
                    //FAiled to Exeute Query
                    isSuccess = false;
                }
            }
            catch(Exception ex)
            {
                //Display Error Message if there's any Exceptional Errors
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

        #region Count Donors for Specific Blood Group
        public string countDonors(string blood_group)
        {
         

            //Create astring variable for donor count and set its default value to 0
            string donors = "0";

            try
            {
                //SQL Query to Count donors for Specific Blood Group
                string sql = "SELECT * FROM tbl_donors WHERE blood_group = '"+ blood_group +"'";

                //Create SQL Command to Pass the value to SQL Query
                var cmd = conn.CreateCommand();
                cmd.CommandText = sql;
                cmd.Connection = conn;

                //SQl Data Adapter to Get the Data from Database
                adapter.SelectCommand = cmd;

                //Dataset to Hold the data from database temporarily
                var dataSet = new DataSet();

                //Fill the data from adapter to dataset
                var  dataFound = adapter.Fill(dataSet);

                int totalRows = 0;
                foreach (DataTable table in dataSet.Tables)
                {
                    totalRows += table.Rows.Count;
                }

                //Get the Total Number of Donors Based on Blood Group
                donors = totalRows.ToString();

               
           
            }
            catch(Exception ex)
            {
                //Display error message if there's any
                MessageBox.Show(ex.Message);
            }
            finally
            {
                //Close Database Connection
                conn.Close();
            }

            return donors;
        }
        #endregion

        #region Method to Search Donors
        public DataTable Search(string keywords)
        {



            //Dataset to Hold the data from database temporarily
            var dataSet = new DataSet();

            try
            {
                //Write the Code to Search Donors based on Keywords Typed on TextBox
                //Write SQL Query to SEarch Donors
                string sql = "SELECT * FROM tbl_donors WHERE donor_id LIKE '%"+ keywords +"%' OR first_name LIKE '%"+keywords+"%' OR last_name LIKE '"+keywords+"' OR email LIKE '%"+ keywords +"%' OR blood_group LIKE '"+keywords+"'";


                //Create SQL Command to Pass the value to SQL Query
                var cmd = conn.CreateCommand();
                cmd.CommandText = sql;
                cmd.Connection = conn;


                //Open Database Connection
                conn.Open();

                //SQl Data Adapter to Get the Data from Database
                adapter.SelectCommand = cmd;

                

                //Fill the data from adapter to dataset
                var searchFound = adapter.Fill(dataSet);

               

               
            }
            catch(Exception ex)
            {
                //Display Error Message if there's any Exceptional Errors
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
    }
}
