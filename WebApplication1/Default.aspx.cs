using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Todo
{
    public partial class _Default : Page
    {
        public SqlConnection conn = new SqlConnection();

        SqlCommand cmd = new SqlCommand();
        int active_count;
        SqlDataReader dr;
        DataSet ds = new DataSet();
        public string connectionString = @"Data Source = (localdb)\MSSQLLocalDB; Initial Catalog = master; Integrated Security = True";

        protected void Page_Load(object sender, EventArgs e)
        {
            txt_task.Focus();

            // the cursor will point the add task textbox and will be ready for the user to type


            conn.ConnectionString = connectionString;

            if (!IsPostBack)
            {
                gridbind();
            }



        }

        protected void gridbind()
        {
            conn.ConnectionString = connectionString;
            cmd = new SqlCommand("select * from tasks", conn);
            conn.Open();
            cmd.ExecuteReader();
            conn.Close();
        }


        protected void Grid_tasks_RowEditing(object sender, GridViewEditEventArgs e)
        {
            //Set the edit index.

            grid_tasks.EditIndex = e.NewEditIndex;

            //Bind data to the GridView control here.

            gridbind();

        }

        protected void grid_tasks_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            // When delete from the grid view is clicked 

            conn.ConnectionString = connectionString;
            conn.Open();
            string name = grid_tasks.DataKeys[e.RowIndex].Value.ToString();

            cmd = new SqlCommand("delete from tasks where name=@name", conn);
            cmd.Parameters.AddWithValue("@name", name);
            cmd.ExecuteNonQuery();

            conn.Close();

            // re-render the grid view
            gridbind();
        }

        protected void btn_add_Click(object sender, EventArgs e)
        {

            try
            {
                conn.ConnectionString = connectionString;
                conn.Open();
                string insert_query = "insert into tasks (name,status) values ('" + txt_task.Text + "','" + ddl_status.SelectedItem.ToString() + "')";
                cmd = new SqlCommand(insert_query, conn);

                cmd.ExecuteNonQuery();


                conn.Close();
                this.grid_tasks.DataBind();
                Response.Redirect("Default.aspx");
            }
            catch (Exception ex)
            {
                Response.Write(ex.Message);
            }


        }



        protected void rbtn_all_CheckedChanged(object sender, EventArgs e)
        {
            // When the All tasks radio button is checked

            try
            {
                conn.ConnectionString = connectionString;
                conn.Open();

                string sql_query = "select * from tasks";
                SqlDataAdapter adapter = new SqlDataAdapter(sql_query, conn);
                adapter.Fill(ds);

                // We cannot define both datasource and datasourceID. Hence the DataSourceID is nullified

                grid_tasks.DataSourceID = "";

                // changing the data source by nullifying the datasourceID

                grid_tasks.DataSource = ds;
                this.grid_tasks.DataBind();

                conn.Close();


            }
            catch (Exception ex)
            {
                Response.Write(ex.Message);
            }

        }

        protected void rbtn_active_CheckedChanged(object sender, EventArgs e)
        {

            //  When the Active tasks radio button is checked
            try
            {
                conn.ConnectionString = connectionString;
                conn.Open();
                string sql_query = "select * from tasks where status='Active'";

                SqlDataAdapter adapter = new SqlDataAdapter(sql_query, conn);
                adapter.Fill(ds);


                grid_tasks.DataSourceID = "";
                grid_tasks.DataSource = ds;
                this.grid_tasks.DataBind();

                conn.Close();


            }
            catch (Exception ex)
            {
                Response.Write(ex.Message);
            }

        }

        protected void rbtn_complete_CheckedChanged(object sender, EventArgs e)
        {
            // When the Completed tasks radio button is checked
            try
            {
                conn.ConnectionString = connectionString;
                conn.Open();
                string sql_query = "select * from tasks where status='Complete'";

                SqlDataAdapter adapter = new SqlDataAdapter(sql_query, conn);
                adapter.Fill(ds);
                grid_tasks.DataSourceID = "";
                grid_tasks.DataSource = ds;
                this.grid_tasks.DataBind();

                conn.Close();


            }
            catch (Exception ex)
            {
                Response.Write(ex.Message);
            }
        }

        protected void grid_tasks_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                DataRowView drv = e.Row.DataItem as DataRowView;
                if (drv["status"].ToString().Equals("Complete"))
                {
                    e.Row.BackColor = System.Drawing.Color.Tomato;
                    // Completed tasks now possess the backcolor - Tomato

                }
                else
                {
                    e.Row.BackColor = System.Drawing.Color.Transparent;
                    active_count++;
                }
            }

            lbl_totalactive.Text = active_count.ToString();
            
            //Total active task is displayed
        }

        protected void lbtn_delete_Click(object sender, EventArgs e)
        {
            // When the Delete all completed tasks link button is checked
            try
            {
                conn.ConnectionString = connectionString;
                conn.Open();
                string sql_query = "delete from tasks where status='Complete'";
                
                cmd = new SqlCommand(sql_query, conn);

                cmd.ExecuteNonQuery();


                conn.Close();
                this.grid_tasks.DataBind();

                conn.Close();


            }
            catch (Exception ex)
            {
                Response.Write(ex.Message);
            }

        }

       
    }
}
