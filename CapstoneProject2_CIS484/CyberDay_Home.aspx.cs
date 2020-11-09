﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Web.Configuration;
using System.Windows.Forms;
using System.Drawing;
using System.Net.Mail;
using System.Text;
using System.IO;

namespace CapstoneProject2_CIS484
{
    public partial class CyberDay_Home : System.Web.UI.Page
    {
        public static DateTime EventDateRequest;
        private System.Data.DataTable submissionDataTable = new System.Data.DataTable();
        public static int count = 1;
        public static AccessCode MasterAccessCode = new AccessCode();
        public static AccessCode MasterAccessCodeCluster = new AccessCode();
        public static int CoordinatorID = CyberDaySite1.CoordinatorID;
        public static string contactCode = "";
        public static string volunteerCode = "";
        public static string StudentCode = "";
        public static string instructorCode = "";
        public static string clusterCode = "";
        public static string clusterCode5 = "";
        public static string instructorCode5x = "";

        //public static Button addEvent = new Button();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                PopulateUploadedFiles();
            }
            //ScriptManager.RegisterStartupScript(
            //    UpdatePanel1,
            //    this.GetType(),
            //    "MyAction",
            //    "$(document).ready(function() { $('.js-example-basic-single').select2(); });",
            //    true);
            //ScriptManager.RegisterStartupScript(
            //    UpdatePanel2,
            //    this.GetType(),
            //    "MyAction",
            //    "$(document).ready(function() { $('.js-example-basic-single').select2();  $('.grid').masonry({ itemSelector: '.grid-item', columnWidth: 160,  gutter: 20   }); $(document).ready(function () {$('#manBt').click(function() {$('#manPan1').slideToggle('slow');});});});",
            //    true);
            CreateGrid();
            PopulateSequence();
            Email_Instructor_EventCreated();
        }

        protected void PopulateSequence()
        {
            submissionDataTable.Clear();
            AddRowsToGrid();

            // NOW BIND THE GRIDVIEW WITH THE DATATABLE.
            ContactSubmissionGrid.DataSource = submissionDataTable;
            ContactSubmissionGrid.DataBind();
        }

        private void CreateGrid()
        {
            // CREATE A GRID FOR DISPLAYING A LIST OF BOOKS.

            System.Data.DataColumn tColumn = null;
            // TABLE COLUMNS.

            tColumn = new System.Data.DataColumn("EventID", Type.GetType("System.String"));
            submissionDataTable.Columns.Add(tColumn);
            tColumn = new System.Data.DataColumn("Event Name", Type.GetType("System.String"));
            submissionDataTable.Columns.Add(tColumn);
            tColumn = new System.Data.DataColumn("Contact Name", System.Type.GetType("System.String"));
            submissionDataTable.Columns.Add(tColumn);
            tColumn = new System.Data.DataColumn("EventCode", System.Type.GetType("System.String"));
            submissionDataTable.Columns.Add(tColumn);
            //tColumn = new System.Data.DataColumn("Org Name", System.Type.GetType("System.String"));
            //submissionDataTable.Columns.Add(tColumn);
            //tColumn = new System.Data.DataColumn("Org Type", System.Type.GetType("System.String"));
            //submissionDataTable.Columns.Add(tColumn);
            //tColumn = new System.Data.DataColumn("Date Request", System.Type.GetType("System.String"));
            //submissionDataTable.Columns.Add(tColumn);
            //tColumn = new System.Data.DataColumn("Event Name", System.Type.GetType("System.String"));
            //submissionDataTable.Columns.Add(tColumn);
        }

        private void AddRowsToGrid()
        {
            //Queries Relevant to home page, fetching event info student info and more
            String sqlQuery = "Select * from Event";

            //Get connection string from web.config file
            string strcon = ConfigurationManager.ConnectionStrings["CyberCityDB"].ConnectionString;
            //create new sqlconnection and connection to database by using connection string from web.config file
            SqlConnection con = new SqlConnection(strcon);
            con.Open();
            SqlDataAdapter sqlAdapter = new SqlDataAdapter(sqlQuery, con);
            int count = 1;
            using (SqlCommand command = new SqlCommand(sqlQuery, con))
            {
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        //TableCell btnCell = new TableCell();
                        //TableCell btnCell2 = new TableCell();

                        //Button addEvent = new Button();
                        //addEvent.ID = "AddEvent" + count;
                        //addEvent.Text = "Add";
                        ////addEvent.OnClientClick();
                        //btnCell.Controls.Add(addEvent);
                        //Button deleteEvent = new Button();
                        //deleteEvent.ID = "DeleteEvent" + count;
                        //deleteEvent.Text = "Delete";
                        ////deleteEvent.OnClientClick();
                        //btnCell2.Controls.Add(deleteEvent);

                        //count++;
                       // submissionDataTable.Rows.Add(reader[0], reader[1], reader[2], reader[3], reader[4]);
                    }
                }
            }
        }

        //protected void GridView_RowDataBound(object sender,
        //    System.Web.UI.WebControls.GridViewRowEventArgs e)
        //{
        //    Button addEvent = new Button();
        //    addEvent.CssClass = "btn btn-primary";
        //    addEvent.ID = "AddEvent" + count;
        //    addEvent.Text = "Add";
        //    addEvent.CausesValidation = false;
        //    addEvent.UseSubmitBehavior = false;
        //    addEvent.Click += new EventHandler(addEvent_Click);
        //    e.Row.Cells[7].Controls.Add(addEvent);
        //    Button deleteEvent = new Button();
        //    deleteEvent.ID = "DeleteEvent" + count;
        //    deleteEvent.Text = "Delete";
        //    //deleteEvent.OnClientClick();
        //    e.Row.Cells[8].Controls.Add(deleteEvent);
        //    count++;
        //}

        protected void addEvent_Click(object sender, EventArgs e)
        {
            string RequestID = ContactSubmissionGrid.SelectedRow.Cells[0].Text;
            string OrgName = "";
            string OrgType = "";
            string EventName = "";
            string ContactName = "";
            string Phone = "";
            string Email = "";
            DateTime Date1 = new DateTime();
            AccessCode contact = new AccessCode();
            string code = contact.GenerateCode(true, true, true, true, 8);
            //MessageBox.Show(code);
            //Inserting teacher query
            //Get connection string from web.config file
            string strcon = ConfigurationManager.ConnectionStrings["CyberCityDB"].ConnectionString;
            //Inserting teacher query

            //Get connection string from web.config file
            //create new sqlconnection and connection to database by using connection string from web.config file
            SqlConnection con = new SqlConnection(strcon);
            String sqlQuery4 = "Select * from ContactRequest where RequestID = '" + RequestID + "'";
            SqlCommand cmd4 = new SqlCommand(sqlQuery4, con);

            con.Open();
            try
            {
                SqlDataReader reader = cmd4.ExecuteReader();

                while (reader.Read())
                {
                    RequestID = reader[0].ToString();
                    ContactName = reader[1].ToString();
                    Phone = reader[2].ToString();
                    Email = reader[3].ToString();
                    OrgName = reader[4].ToString();
                    OrgType = reader[5].ToString();
                    Date1 = Convert.ToDateTime(reader[6]);

                    EventName = reader[7].ToString();
                }
                reader.Close();
            }
            catch (System.Data.SqlClient.SqlException ex)
            {
                string msg = "Select Error in ContactRequest:";
                msg += ex.Message;
                throw new Exception(msg);
            }

            String sqlQuery = "Insert into AccessCode (Code, UserType, CoordinatorID) values (@Code, @UserType, @CoordinatorID)";
            String sqlQuery6 = "Insert into EventContact (ContactCode) values (@Code)";

            SqlCommand cmd = new SqlCommand(sqlQuery, con);
            cmd.Parameters.Add(new SqlParameter("@Code", code));
            cmd.Parameters.Add(new SqlParameter("@UserType", "EventContact"));
            cmd.Parameters.Add(new SqlParameter("@CoordinatorID", CyberDaySite1.CoordinatorID));

            SqlCommand cmd6 = new SqlCommand(sqlQuery6, con);
            cmd6.Parameters.Add(new SqlParameter("@Code", code));

            try
            {
                cmd.CommandType = CommandType.Text;
                cmd.ExecuteNonQuery();
            }
            catch (System.Data.SqlClient.SqlException ex)
            {
                string msg = "Insert/Update Error Insert into AccessCode:";
                msg += ex.Message;
                throw new Exception(msg);
            }
            try
            {
                cmd6.CommandType = CommandType.Text;
                cmd6.ExecuteNonQuery();
            }
            catch (System.Data.SqlClient.SqlException ex)
            {
                string msg = "Insert/Update Error Insert into EventContact 1:";
                msg += ex.Message;
                throw new Exception(msg);
            }
            String sqlQuery2 = "  Insert into Organization (Name, Type, ContactCode) values " +
                               "(@OrgName, @OrgType, @ContactCode);";
            SqlCommand cmd2 = new SqlCommand(sqlQuery2, con);
            cmd2.Parameters.Add(new SqlParameter("@OrgName", OrgName));
            cmd2.Parameters.Add(new SqlParameter("@ContactCode", code));
            cmd2.Parameters.Add(new SqlParameter("@OrgType", OrgType));

            try
            {
                cmd2.CommandType = CommandType.Text;
                cmd2.ExecuteNonQuery();
            }
            catch (System.Data.SqlClient.SqlException ex)
            {
                string msg = "Insert/Update Error Insert into Org:";
                msg += ex.Message;
                throw new Exception(msg);
            }

            String sqlQuery3 = "  Insert into Event (Date, Name) values " +
                               "('" + Date1.ToString("yyyy-MM-dd") + "', @EventName);";
            SqlCommand cmd3 = new SqlCommand(sqlQuery3, con);
            cmd3.Parameters.Add(new SqlParameter("@EventName", EventName));
            cmd3.Parameters.Add(new SqlParameter("@Date", Date1));

            try
            {
                cmd3.CommandType = CommandType.Text;
                cmd3.ExecuteNonQuery();
            }
            catch (System.Data.SqlClient.SqlException ex)
            {
                string msg = "Insert/Update Error Insert into Event:";
                msg += ex.Message;
                throw new Exception(msg);
            }
            //UPDATE table_name
            //SET column1 = value1, column2 = value2, ...
            //WHERE condition;
            String sqlQuery1 = "  Update  EventContact Set Name = @ContactName, OrganizationID =(select OrganizationID from Organization where Name=@OrgName), Phone = @Phone, Email = @Email, EventID=(select EventID from Event where Name=@EventName)" +
                               " where ContactCode = @ContactCode;";
            SqlCommand cmd1 = new SqlCommand(sqlQuery1, con);
            cmd1.Parameters.Add(new SqlParameter("@Email", Email));
            cmd1.Parameters.Add(new SqlParameter("@ContactCode", code));
            cmd1.Parameters.Add(new SqlParameter("@ContactName", ContactName));
            cmd1.Parameters.Add(new SqlParameter("@OrgName", OrgName));
            cmd1.Parameters.Add(new SqlParameter("@Phone", Phone));
            cmd1.Parameters.Add(new SqlParameter("@EventName", EventName));

            try
            {
                cmd1.CommandType = CommandType.Text;
                cmd1.ExecuteNonQuery();
            }
            catch (System.Data.SqlClient.SqlException ex)
            {
                string msg = "Insert/Update Error in EventContact:";
                msg += ex.Message;
                throw new Exception(msg);
            }

            String sqlQuery5 = " Delete from ContactRequest where RequestID = '" + RequestID + "'";
            SqlCommand cmd5 = new SqlCommand(sqlQuery5, con);

            try
            {
                cmd5.CommandType = CommandType.Text;
                cmd5.ExecuteNonQuery();
            }
            catch (System.Data.SqlClient.SqlException ex)
            {
                string msg = "Delete Error in ContactRequest:";
                msg += ex.Message;
                throw new Exception(msg);
            }

            con.Close();
            PopulateSequence();
            RequestListDDLUpdate.Update();
        }

        protected void btnAccessCodeEntry_Click(object sender, EventArgs e)
        {
            StudentSignUpDiv.Attributes.Add("style", "margin-top: 40px; display = none");
            StudentSignUpDiv.Visible = false;
            InstDiv.Attributes.Add("style", "margin-top: 40px; display = none");
            InstDiv.Visible = false;

            string code = txtAccessCodeEntry.Text;
            string type = "";
            contactCode = code;
            instructorCode = code;
            clusterCode = code;
            SqlConnection dbConnection = new SqlConnection(WebConfigurationManager.ConnectionStrings["CyberCityDB"].ConnectionString);
            dbConnection.Open();
            try
            {
                SqlCommand loginCommand = new SqlCommand();
                loginCommand.Connection = dbConnection;
                loginCommand.CommandText = "Select * from AccessCode where Code = @Code";
                loginCommand.Parameters.Add(new SqlParameter("@Code", code));
                //loginCommand.Parameters.Add(new SqlParameter("@UserType", type));
                SqlDataReader reader = loginCommand.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        type = reader[1].ToString();
                        
                        if (type.Equals("ClassCode"))
                        {
                            StudentSignUpDiv.Attributes.Add("style", "margin-top: 40px; display = normal");
                            StudentSignUpDiv.Visible = true;
                            lblAccessCodeStatus.Text = "Logged in as Student. Please Create Your Student Profile!";
                            string EventinfoQry = "Select * from Organization inner join Cluster on Cluster.OrganizationID = Organization.OrganizationID where Cluster.ClusterCode ='" + clusterCode + "'";

                            SqlConnection newcon = new SqlConnection(ConfigurationManager.ConnectionStrings["CyberCityDB"].ConnectionString);
                            newcon.Open();
                            SqlCommand nameCommand = new SqlCommand(EventinfoQry, newcon);
                            SqlDataReader OrgReader = nameCommand.ExecuteReader();
                            while (OrgReader.Read())
                            {
                                Label10.Text = (HttpUtility.HtmlEncode(OrgReader[1].ToString()));
                            }
                        }
                        else if (type.Equals("EventCode"))
                        {
                            InstDiv.Attributes.Add("style", "margin-top: 40px; display = normal");
                            InstDiv.Visible = true;
                            sqlsrcEventInfo.SelectCommand = "SELECT TOP(1000) Name, Date, ContactName as 'Event Contact' from EVENT where Event.EventCode ='" + code + "'";
                            sqlsrcEventInfo.DataBind();
                            grdviewEventInfo.DataBind();
                            sqlsrcEventActivities.SelectCommand = "SELECT TOP(1000) ActivityName as 'Activity Name', Time, Room from EVENTACTIVITIES inner join Event on EventActivities.EventID = Event.EventID where Event.EventCode = '" + code + "'";
                            sqlsrcEventActivities.DataBind();
                            grdviewEventActivities.DataBind();

                        }


                    }
                }
            }
            catch (System.Data.SqlClient.SqlException ex)
            {
                string msg = "Insert/Update Error:";
                msg += ex.Message;
                throw new Exception(msg);
            }
            finally
            {
                dbConnection.Close();
            }
        }

        protected void AddRequest_Click(object sender, EventArgs e)
        {
            Boolean dup = false;

            if (dup == false && ContactRequestNameText.Text != "" && ContactRequestPhoneText.Text != "" && OrganizationTypeList.SelectedIndex > -1 && ContactRequestEmailText.Text != "")
            {
                //If filled out and non duplicate it inserts into object
                string strcon = ConfigurationManager.ConnectionStrings["CyberCityDB"].ConnectionString;
                SqlConnection connection = new SqlConnection(strcon);
                SqlCommand cmd;
                int sub;
                try
                {
                    // open the Sql connection
                    connection.Open();
                    //Check for size in Note field and insert temporarily or permanently into DB if it does not exist
                    string sqlStatement =
                        "If Not Exists (select 1 from ContactRequest where ContactName= @ContactName and OrganizationName= @OrganizationName) " +
                        "Insert into ContactRequest (ContactName, Phone, Email, OrganizationName, OrganizationType, EventNameRequest, DateRequest) " +
                        "values(@ContactName, @Phone, @Email, @OrganizationName, @OrganizationType, @EventNameRequest, @DateRequest)";
                    String strDateFormat = "yyyy-MM-dd";
                    string x = EventDateRequest.ToString("yyyy-MM-dd");
                    cmd = new SqlCommand(sqlStatement, connection);
                    cmd.Parameters.AddWithValue("@ContactName", ContactRequestNameText.Text);
                    cmd.Parameters.AddWithValue("@OrganizationName", ContactRequestOrganizationNameText.Text);
                    cmd.Parameters.AddWithValue("@OrganizationType", OrganizationTypeList.SelectedValue.ToString());
                    DateTime to = DateTime.ParseExact(x, strDateFormat, CultureInfo.InvariantCulture);
                    cmd.Parameters.AddWithValue("@DateRequest", to);

                    cmd.Parameters.AddWithValue("@Phone", ContactRequestPhoneText.Text);
                    cmd.Parameters.AddWithValue("@Email", ContactRequestEmailText.Text);
                    cmd.Parameters.AddWithValue("@EventNameRequest", EventNameRequest.Text);

                    cmd.CommandType = CommandType.Text;
                    cmd.ExecuteNonQuery();
                    ResetButton_Click(sender, e);
                }
                //If it does not work
                catch (System.Data.SqlClient.SqlException ex)
                {
                    string msg = "Insert/Update Error:";
                    msg += ex.Message;
                    throw new Exception(msg);
                }
                finally
                {
                    // close the Sql Connection
                    connection.Close();
                }
            }
            //Failure alternatives
            else if (dup == true)
            {
                MessageBox.Show("Try Again", "Duplicate");
            }
            else
            {
                MessageBox.Show("Oops", "All fields must be filled");
            }

            PopulateSequence();
            RequestListDDLUpdate.Update();
        }

        protected void ResetButton_Click(object sender, EventArgs eventArgs)
        {
            ContactRequestNameText.Text = string.Empty;
            ContactRequestOrganizationNameText.Text = string.Empty;
            OrganizationTypeList.SelectedIndex = 0;
            ContactRequestPhoneText.Text = string.Empty;
            ContactRequestEmailText.Text = string.Empty;
            EventNameRequest.Text = string.Empty;
        }

        protected void EventRequestDate_SelectionChanged(object sender, EventArgs e)
        {
            EventDateRequest = EventRequestDate.SelectedDate;
            //MessageBox.Show(EventDateRequest.Date.ToShortDateString());
        }

        protected void DeleteEvent_OnClickEvent_Click(object sender, EventArgs e)
        {
            string RequestID = ContactSubmissionGrid.SelectedRow.Cells[0].Text;

            string strcon = ConfigurationManager.ConnectionStrings["CyberCityDB"].ConnectionString;
            //Inserting teacher query

            //Get connection string from web.config file
            //create new sqlconnection and connection to database by using connection string from web.config file
            SqlConnection con = new SqlConnection(strcon);

            con.Open();
            String sqlQuery = " Delete from ContactRequest where RequestID = '" + RequestID + "'";
            SqlCommand cmd = new SqlCommand(sqlQuery, con);

            try
            {
                cmd.CommandType = CommandType.Text;
                cmd.ExecuteNonQuery();
            }
            catch (System.Data.SqlClient.SqlException ex)
            {
                string msg = "Delete Error in ContactRequest:";
                msg += ex.Message;
                throw new Exception(msg);
            }
            PopulateSequence();
            RequestListDDLUpdate.Update();
        }

        protected void SubmitButton_Click(object sender, EventArgs e)
        {
            // Generate Cluster and Instructor Codes
            instructorCode5x = MasterAccessCode.GenerateCode(lowercase: true, uppercase: true, numbers: true, otherChar: true, codeSize: 8);
            AccessCode newAccessxx = new AccessCode();
            instructorCode5x = "349843";
            instructorCode5x += "v";
            //string instructorCode = "";
            //instructorCode = MasterAccessCode.GenerateCode(lowercase: true, uppercase: true, numbers: true, otherChar: true, codeSize: 8);
            //MessageBox.Show(instructorCode.ToString(), "Code 2 for instructor: ");

            SqlConnection sqlconnect = new SqlConnection(ConfigurationManager.ConnectionStrings["CyberCityDB"].ConnectionString);
            sqlconnect.Open();

            // Necessary information for insert statements
            string eventID = "";
            string orgID = "";

            // Find necessary information
            string sqlQuery2 = "SELECT EventID, ContactCode, OrganizationID FROM EVENTCONTACT WHERE CONTACTCODE = '" + contactCode + "'";
            SqlCommand cmd100 = new SqlCommand(sqlQuery2, sqlconnect);

            try
            {
                SqlDataReader reader = cmd100.ExecuteReader();

                while (reader.Read())
                {
                    eventID = reader[0].ToString();
                    orgID = reader[2].ToString();
                }
                reader.Close();
            }
            catch (System.Data.SqlClient.SqlException ex)
            {
                string msg = "Select Error in EventContact";
                msg += ex.Message;
                throw new Exception(msg);
            }

            // INSERT SQL Statements
            InsertClusterCode();
            // Insert - Access Part 1
            String sqlQuery_p1 = "INSERT INTO ACCESSCODE(Code, UserType) VALUES (@Code, @UserType)";
            SqlCommand cmd_p1 = new SqlCommand(sqlQuery_p1, sqlconnect);
            cmd_p1.Parameters.Add(new SqlParameter("@Code", instructorCode5x));
            cmd_p1.Parameters.Add(new SqlParameter("@UserType", "Instructor"));
            try
            {
                cmd_p1.CommandType = CommandType.Text;
                cmd_p1.ExecuteNonQuery();
            }
            catch (System.Data.SqlClient.SqlException ex)
            {
                string msg = "Insert Error into AccessCode";
                msg += ex.Message;
                throw new Exception(msg);
            }

            // Insert - Cluster Part 1
            String sqlQuery_p2 = "INSERT INTO CLUSTER(ClusterCode) VALUES (@ClusterCode)";
            SqlCommand cmd_p2 = new SqlCommand(sqlQuery_p2, sqlconnect);
            cmd_p2.Parameters.Add(new SqlParameter("@ClusterCode", clusterCode5));

            try
            {
                cmd_p2.CommandType = CommandType.Text;
                cmd_p2.ExecuteNonQuery();
            }
            catch (System.Data.SqlClient.SqlException ex)
            {
                string msg = "Insert Error into Cluster Part 1";
                msg += ex.Message;
                throw new Exception(msg);
            }

            // Insert - Instructor
            String sqlQuery4 = "INSERT INTO INSTRUCTOR(InstructorCode, Name, OrganizationID, Email, Phone, ContactCode)" +
                "VALUES (@InstructorCode, @Name, @OrganizationID, @Email, @Phone, @ContactCode)";
            SqlCommand cmd101 = new SqlCommand(sqlQuery4, sqlconnect);
            cmd101.Parameters.Add(new SqlParameter("@InstructorCode", instructorCode5x));
            cmd101.Parameters.Add(new SqlParameter("@Name", Instructor_tbFirstName.Text + ' ' + Instructor_tbLastName.Text));
            cmd101.Parameters.Add(new SqlParameter("@OrganizationID", orgID));
            cmd101.Parameters.Add(new SqlParameter("@Email", Instructor_tbEmail.Text));
            cmd101.Parameters.Add(new SqlParameter("@Phone", int.Parse(Instructor_tbPhone.Text)));
            cmd101.Parameters.Add(new SqlParameter("@ContactCode", contactCode));
            try
            {
                cmd101.CommandType = CommandType.Text;
                cmd101.ExecuteNonQuery();
            }
            catch (System.Data.SqlClient.SqlException ex)
            {
                string msg = "Insert Error into Instructor";
                msg += ex.Message;
                throw new Exception(msg);
            }

            // Update - Cluster Part 2
            String sqlQuery3 = "UPDATE Cluster SET InstructorCode = @InstructorCode, OrganizationID = @OrganizationID WHERE ClusterCode = @ClusterCode";
            SqlCommand cmd103 = new SqlCommand(sqlQuery3, sqlconnect);
            cmd103.Parameters.Add(new SqlParameter("@InstructorCode", instructorCode5x));
            cmd103.Parameters.Add(new SqlParameter("@OrganizationID", orgID));
            cmd103.Parameters.Add(new SqlParameter("@ClusterCode", clusterCode5));
            try
            {
                cmd103.CommandType = CommandType.Text;
                cmd103.ExecuteNonQuery();
            }
            catch (System.Data.SqlClient.SqlException ex)
            {
                string msg = "Insert Error into Cluster Part 2";
                msg += ex.Message;
                throw new Exception(msg);
            }
            sqlconnect.Close();
        }

        protected void InsertClusterCode()
        {
            SqlConnection sqlconnect = new SqlConnection(ConfigurationManager.ConnectionStrings["CyberCityDB"].ConnectionString);
            sqlconnect.Open();
            clusterCode5 = MasterAccessCode.GenerateCode(lowercase: true, uppercase: true, numbers: true, otherChar: true, codeSize: 8);
            String sqlqryyy = "INSERT INTO ACCESSCODE(Code, UserType) VALUES (@Code, @UserType)";
            SqlCommand cmd_p11 = new SqlCommand(sqlqryyy, sqlconnect);
            cmd_p11.Parameters.Add(new SqlParameter("@Code", clusterCode5));
            cmd_p11.Parameters.Add(new SqlParameter("@UserType", "Cluster"));
            try
            {
                cmd_p11.CommandType = CommandType.Text;
                cmd_p11.ExecuteNonQuery();
                //Email_CodeCaptured_Coordinator();
            }
            catch (System.Data.SqlClient.SqlException ex)
            {
                string msg = "Insert Error into AccessCode";
                msg += ex.Message;
                throw new Exception(msg);
            }
            sqlconnect.Close();
        }

        protected void Instructor_ResetButton_Click(object sender, EventArgs e)
        {
            Instructor_tbEmail.Text = string.Empty;
            Instructor_tbFirstName.Text = string.Empty;
            Instructor_tbLastName.Text = string.Empty;
            Instructor_tbPhone.Text = string.Empty;
        }

        protected void btnStudentSignUp_Click(object sender, EventArgs e)
        {
            // Generate Cluster and Instructor Codes
            string studentCode = "";
            studentCode = MasterAccessCode.GenerateCode(lowercase: true, uppercase: true, numbers: true, otherChar: true, codeSize: 8);
            lblStudentAccessCodeinput.Text = studentCode;

            SqlConnection sqlconnect = new SqlConnection(ConfigurationManager.ConnectionStrings["CyberCityDB"].ConnectionString);
            sqlconnect.Open();

            // Find necessary information
            string sqlQuery101 = "SELECT InstructorCode, OrganizationID FROM Cluster WHERE ClusterCode = '" + clusterCode + "'";
            SqlCommand cmd101 = new SqlCommand(sqlQuery101, sqlconnect);
            string Int_Code = "";
            string orgID = "";

            try
            {
                SqlDataReader reader = cmd101.ExecuteReader();

                while (reader.Read())
                {
                    Int_Code = reader[0].ToString();
                    orgID = reader[1].ToString();
                }
                reader.Close();
            }
            catch (System.Data.SqlClient.SqlException ex)
            {
                string msg = "Select Error in EventContact";
                msg += ex.Message;
                throw new Exception(msg);
            }

            // Insert Student Access Code into AccessCode Table
            String sqlQuery = "INSERT INTO ACCESSCODE(Code, UserType) VALUES (@Code, @UserType)";
            SqlCommand cmd = new SqlCommand(sqlQuery, sqlconnect);
            cmd.Parameters.Add(new SqlParameter("@Code", studentCode));
            cmd.Parameters.Add(new SqlParameter("@UserType", "Student"));
            try
            {
                cmd.CommandType = CommandType.Text;
                cmd.ExecuteNonQuery();
            }
            catch (System.Data.SqlClient.SqlException ex)
            {
                string msg = "Insert Error into AccessCode";
                msg += ex.Message;
                throw new Exception(msg);
            }

            //// Insert - Student
            //String sqlQuery2 = "INSERT INTO STUDENT(StudentCode, Name, InstructorCode, Notes)" +
            //    "VALUES (@StudentCode, @Name, @InstructorCode, @Notes)";
            //SqlCommand cmd2 = new SqlCommand(sqlQuery2, sqlconnect);
            //cmd2.Parameters.Add(new SqlParameter("@StudentCode", studentCode));
            //cmd2.Parameters.Add(new SqlParameter("@Name", Student_tbFirstName.Text + ' ' + Student_tbLastName.Text));
            //cmd2.Parameters.Add(new SqlParameter("@InstructorCode", instructorCode));
            //cmd2.Parameters.Add(new SqlParameter("@Notes", Student_tbNotes.Text));
            //try
            //{
            //    cmd2.CommandType = CommandType.Text;
            //    cmd2.ExecuteNonQuery();
            //}
            //catch (System.Data.SqlClient.SqlException ex)
            //{
            //    string msg = "Insert Error into Student";
            //    msg += ex.Message;
            //    throw new Exception(msg);

            //}

            // Insert - Student Part 3
            String sqlQuery2_p3 = "INSERT INTO STUDENT(StudentCode, InstructorCode)" +
                "VALUES (@StudentCode, @InstructorCode)";
            SqlCommand cmd2_p3 = new SqlCommand(sqlQuery2_p3, sqlconnect);
            cmd2_p3.Parameters.Add(new SqlParameter("@StudentCode", studentCode));
            cmd2_p3.Parameters.Add(new SqlParameter("@InstructorCode", Int_Code));
            try
            {
                cmd2_p3.CommandType = CommandType.Text;
                cmd2_p3.ExecuteNonQuery();
            }
            catch (System.Data.SqlClient.SqlException ex)
            {
                string msg = "Insert Error into Student Part 3";
                msg += ex.Message;
                throw new Exception(msg);
            }

            //// Insert - Student Part 1
            //String sqlQuery2_p1 = "UPDATE Student SET StudentCode = @StudentCode WHERE StudentCode = @StudentCode AND InstructorCode = @InstructorCode";
            //SqlCommand cmd2_p1 = new SqlCommand(sqlQuery2_p1, sqlconnect);
            //cmd2_p1.Parameters.Add(new SqlParameter("@InstructorCode", Int_Code));
            //cmd2_p1.Parameters.Add(new SqlParameter("@StudentCode", studentCode));

            //try
            //{
            //    cmd2_p1.CommandType = CommandType.Text;
            //    cmd2_p1.ExecuteNonQuery();
            //}
            //catch (System.Data.SqlClient.SqlException ex)
            //{
            //    string msg = "Insert Error into Student Part 1";
            //    msg += ex.Message;
            //    throw new Exception(msg);

            //}

            // Insert - Student Part 2

            String sqlQuery2_p2 = "UPDATE Student SET Name = @Name, Notes = @Notes, OrganizationID = @OrganizationID WHERE InstructorCode = @InstructorCode AND StudentCode = @StudentCode";

            SqlCommand cmd2_p2 = new SqlCommand(sqlQuery2_p2, sqlconnect);
            cmd2_p2.Parameters.Add(new SqlParameter("@Name", Student_tbFirstName.Text + ' ' + Student_tbLastName.Text));
            cmd2_p2.Parameters.Add(new SqlParameter("@Notes", Student_tbNotes.Text));
            cmd2_p2.Parameters.Add(new SqlParameter("@InstructorCode", Int_Code));
            cmd2_p2.Parameters.Add(new SqlParameter("@StudentCode", studentCode));
            cmd2_p2.Parameters.Add(new SqlParameter("@OrganizationID", orgID));

            try
            {
                cmd2_p2.CommandType = CommandType.Text;
                cmd2_p2.ExecuteNonQuery();
            }
            catch (System.Data.SqlClient.SqlException ex)
            {
                string msg = "Insert Error into Student";
                msg += ex.Message;
                throw new Exception(msg);
            }
        }

        protected void btnStudentSignUpReset_Click(object sender, EventArgs e)
        {
            Student_tbFirstName.Text = "";
            Student_tbLastName.Text = "";
            Student_tbNotes.Text = "";
        }

        protected void SubmitCoordinator_Click(object sender, EventArgs e)
        {
            //Inserting teacher query
            //Get connection string from web.config file
            string ct = ConfigurationManager.ConnectionStrings["CyberCityDB"].ConnectionString;
            SqlConnection con = new SqlConnection(ct);

            //Inserting Coordinator query

            String sqlQuery1 = "Insert into CoordinatorAuth (CoordinatorID, Username, Password) values ((select CoordinatorID from Coordinator where CoordinatorID = (select Max(CoordinatorID) from Coordinator)), @Username, @Password)";
            //Get connection string from web.config file
            //create new sqlconnection and connection to database by using connection string from web.config file
            SqlCommand cmd = new SqlCommand(sqlQuery1, con);
            cmd.Parameters.Add(new SqlParameter("@Username", UsernameTextBox.Text));
            cmd.Parameters.Add(new SqlParameter("@Password", PasswordHash.HashPassword(modalLRInput13.Text)));

            String sqlQuery = "Insert into Coordinator (Name, Email, Phone) values (@Name, @Email, @Phone);";
            SqlCommand command = new SqlCommand(sqlQuery, con);
            command.Parameters.Add(new SqlParameter("@Name", CoordinatorNameText.Text));
            command.Parameters.Add(new SqlParameter("@Email", EmailTextBox.Text));
            command.Parameters.Add(new SqlParameter("@Phone", PhoneTextBox.Text));
            con.Open();
            try
            {
                command.ExecuteNonQuery();

                //cmd.Parameters.Add(new SqlParameter("@Username", UsernameTextBox.Text));
                //cmd.Parameters.Add(new SqlParameter("@Password", PasswordHash.HashPassword(modalLRInput13.Text)));

                Console.Write("insert successful");
                //MessageBox.Show("insert teacher success");
            }
            catch (SqlException ex)
            {
                Console.Write(ex.Message);
                lblStatus.Text = "Database Error";
            }
            try
            {
                cmd.CommandType = CommandType.Text;
                cmd.ExecuteNonQuery();
                ResetCoordinator_Click(sender, e);
                //cmd.Parameters.Add(new SqlParameter("@Username", UsernameTextBox.Text));
                //cmd.Parameters.Add(new SqlParameter("@Password", PasswordHash.HashPassword(modalLRInput13.Text)));

                Console.Write("insert 2 successful");
                //MessageBox.Show("insert teacher success");

                ResetCoordinator_Click(sender, e);
            }
            catch (System.Data.SqlClient.SqlException ex)
            {
                string msg = "Inter CoordinatorAuth";
                msg += ex.Message;
                throw new Exception(msg);
            }
            con.Close();

            //con1.Close();
        }

        protected void PopulateCoordinator_Click(object sender, EventArgs e)
        {
            Random rnd = new Random();
            UsernameTextBox.Text = HttpUtility.HtmlEncode(CoordinatorNameText.Text);
            EmailTextBox.Text = HttpUtility.HtmlEncode(UsernameTextBox.Text) + "@edu.com";
            modalLRInput13.Text = "";
        }

        protected void ResetCoordinator_Click(object sender, EventArgs e)
        {
            //clear teacher input
            CoordinatorNameText.Text = string.Empty;
            EmailTextBox.Text = string.Empty;
            PhoneTextBox.Text = string.Empty;
            UsernameTextBox.Text = string.Empty;
            modalLRInput13.Text = string.Empty;
        }

        protected void EventList_SelectedIndexChanged(object sender, EventArgs e)
        {
            ////Queries Relevant to home page, fetching event info student info and more

            //string OrgName = "";
            //string OrgType = "";
            //string EventName = "";
            //string ContactName = "";
            //string Phone = "";
            //string Email = "";
            //string ContactCode = "";
            //string EventID = "";
            //DateTime Date1 = new DateTime();
            ////Inserting teacher query
            ////Get connection string from web.config file
            //string strcon = ConfigurationManager.ConnectionStrings["CyberCityDB"].ConnectionString;
            ////Inserting teacher query

            ////Get connection string from web.config file
            ////create new sqlconnection and connection to database by using connection string from web.config file
            //SqlConnection con = new SqlConnection(strcon);
            //String sqlQuery4 = "select C.ContactCode as ContactCode, C.Name as ContactName, format(E.Date, 'MM/dd/yyyy') as Date, O.Name as OrgName, O.Type as OrgType, E.Name as EventName" +
            //" from EventContact C" +
            //" inner join Organization O on O.OrganizationID = C.OrganizationID" +
            //" inner join Event E on C.EventID = E.EventID" +
            //" where C.EventID = '" + EventList.SelectedValue + "'";
            //SqlCommand cmd4 = new SqlCommand(sqlQuery4, con);

            //con.Open();
            //try
            //{
            //    SqlDataReader reader = cmd4.ExecuteReader();

            //    while (reader.Read())
            //    {
            //        ContactCode = reader[0].ToString();
            //        ContactName = reader[1].ToString();
            //        Date1 = Convert.ToDateTime(reader[2]);
            //        OrgName = reader[3].ToString();
            //        OrgType = reader[4].ToString();
            //        EventName = reader[5].ToString();
            //    }
            //    reader.Close();
            //}
            //catch (System.Data.SqlClient.SqlException ex)
            //{
            //    string msg = "Select Error in EventContact:";
            //    msg += ex.Message;
            //    throw new Exception(msg);
            //}
            //SqlDataAdapter sqlAdapter = new SqlDataAdapter(sqlQuery4, con);
            //DataSet ds = new DataSet();

            //sqlAdapter.Fill(ds);

            //EventInfoTable.DataSource = ds;
            //EventInfoTable.DataBind();

            //String sqlQuery1 = "select S.Name from Student S inner join Instructor I on S.InstructorCode = I.InstructorCode" +
            //" where I.ContactCode = '" + ContactCode + "';";
            //SqlDataAdapter sqlAdapter1 = new SqlDataAdapter(sqlQuery1, con);

            ////Fill table with data
            //DataTable dt = new DataTable();
            //sqlAdapter1.Fill(dt);
            //if (dt.Rows.Count > 0)
            //{
            //    StudentListBox.DataSource = dt;
            //    StudentListBox.DataTextField = "Name";
            //    StudentListBox.DataBind();
            //}

            //string sqlQuery2 = "select Name from Instructor where ContactCode = '" + ContactCode + "'";
            //SqlDataAdapter sqlAdapter2 = new SqlDataAdapter(sqlQuery2, con);

            //var items = new List<string>();

            //using (SqlCommand command = new SqlCommand(sqlQuery2, con))
            //{
            //    using (SqlDataReader reader = command.ExecuteReader())
            //    {
            //        while (reader.Read())
            //        {
            //            //Read info into List
            //            items.Add(reader.GetString(0));
            //        }
            //    }
            //}
            //InstructorRepeater.DataSource = items;
            //InstructorRepeater.DataBind();

            //string sqlQuery3 = "select V.VolunteerCode, V.Name from Volunteer V inner join EventVolunteers E on V.VolunteerCode = E.VolunteerCode where E.EventID = '" + EventList.SelectedValue + "'";
            //var items1 = new List<string>();

            //using (SqlCommand command = new SqlCommand(sqlQuery3, con))
            //{
            //    using (SqlDataReader reader = command.ExecuteReader())
            //    {
            //        while (reader.Read())
            //        {
            //            //Read info into List
            //            items1.Add(reader.GetString(1));
            //        }
            //    }
            //}
            //VolunteerRepeater.DataSource = items1;
            //VolunteerRepeater.DataBind();

            //con.Close();
        }
        protected void GvEventdisplay_SelectedIndexChanged(object sender, EventArgs e)
        {
            //Queries Relevant to home page, fetching event info student info and more

            string EventID = GvEventdisplay.SelectedRow.Cells[0].Text;
            string OrgName = "";
            string OrgType = "";
            string EventName = "";
            string ContactName = "";
            string Phone = "";
            string Email = "";
            string ContactCode = "";
            DateTime Date1 = new DateTime();
            //Inserting teacher query
            //Get connection string from web.config file
            string strcon = ConfigurationManager.ConnectionStrings["CyberCityDB"].ConnectionString;
            //Inserting teacher query

            //Get connection string from web.config file
            //create new sqlconnection and connection to database by using connection string from web.config file
            SqlConnection con = new SqlConnection(strcon);
            String sqlQuery4 = "select C.ContactCode as ContactCode, C.Name as ContactName, format(E.Date, 'MM/dd/yyyy') as Date, O.Name as OrgName, O.Type as OrgType, E.Name as EventName" +
            " from EventContact C" +
            " inner join Organization O on O.OrganizationID = C.OrganizationID" +
            " inner join Event E on C.EventID = E.EventID" +
            " where C.EventID = '" + EventID + "'";
            SqlCommand cmd4 = new SqlCommand(sqlQuery4, con);

            con.Open();
            try
            {
                SqlDataReader reader = cmd4.ExecuteReader();
                //MessageBox.Show(EventID, "helloo");
                while (reader.Read())
                {
                    ContactCode = reader[0].ToString();
                    ContactName = reader[1].ToString();
                    Date1 = Convert.ToDateTime(reader[2]);
                    OrgName = reader[3].ToString();
                    OrgType = reader[4].ToString();
                    EventName = reader[5].ToString();
                }
                reader.Close();
            }
            catch (System.Data.SqlClient.SqlException ex)
            {
                string msg = "Select Error in EventContact:";
                msg += ex.Message;
                throw new Exception(msg);
            }
            SqlDataAdapter sqlAdapter = new SqlDataAdapter(sqlQuery4, con);
            DataSet ds = new DataSet();

            sqlAdapter.Fill(ds);

            //EventInfoTable.DataSource = ds;
            //EventInfoTable.DataBind();

            String sqlQuery1 = "select S.Name from Student S inner join Instructor I on S.InstructorCode = I.InstructorCode" +
            " where I.ContactCode = '" + ContactCode + "';";
            SqlDataAdapter sqlAdapter1 = new SqlDataAdapter(sqlQuery1, con);

            //Fill table with data
            DataTable dt = new DataTable();
            sqlAdapter1.Fill(dt);
            if (dt.Rows.Count > 0)
            {
                StudentListBox.DataSource = dt;
                StudentListBox.DataTextField = "Name";
                StudentListBox.DataBind();
            }

            string sqlQuery2 = "select Name from Instructor where ContactCode = '" + ContactCode + "'";
            SqlDataAdapter sqlAdapter2 = new SqlDataAdapter(sqlQuery2, con);

            var items = new List<string>();

            using (SqlCommand command = new SqlCommand(sqlQuery2, con))
            {
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        //Read info into List
                        items.Add(reader.GetString(0));
                    }
                }
            }
            InstructorRepeater.DataSource = items;
            InstructorRepeater.DataBind();
            //MessageBox.Show(items[0]);

            string sqlQuery3 = "select V.VolunteerCode, V.Name from Volunteer V inner join EventVolunteers E on V.VolunteerCode = E.VolunteerCode where E.EventID = '" + EventID + "'";
            var items1 = new List<string>();

            using (SqlCommand command = new SqlCommand(sqlQuery3, con))
            {
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        //Read info into List
                        items1.Add(reader.GetString(1));
                    }
                }
            }
            VolunteerRepeater.DataSource = items1;
            VolunteerRepeater.DataBind();

            con.Close();

            string mag = "Row Info: " + EventID + " " + EventName + " " + Date1 + " " + OrgName + " " + OrgName + " " + ContactName + " " + ContactCode;
            foreach (GridViewRow row in GvEventdisplay.Rows)
            {
                if (row.RowIndex == GvEventdisplay.SelectedIndex)
                {
                    row.BackColor = ColorTranslator.FromHtml("#A1DCF2");
                    row.ToolTip = string.Empty;
                }
                else
                {
                    row.BackColor = ColorTranslator.FromHtml("#FFFFFF");
                    row.ToolTip = "Click to select this row.";
                }
            }
        }
        protected void GvEventdisplay_RowDataBound(object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                e.Row.Attributes["onclick"] = Page.ClientScript.GetPostBackClientHyperlink(GvEventdisplay, "Select$" + e.Row.RowIndex);
                e.Row.Attributes["style"] = "cursor:pointer";
            }
        }

        private void PopulateUploadedFiles()
        {
            string strcon = ConfigurationManager.ConnectionStrings["CyberCityDB"].ConnectionString;
            SqlConnection con = new SqlConnection(strcon);
            String sqlQuery = "SELECT * from [File]";
            List<File> allFiles = new List<File>();

            con.Open();
            using (SqlCommand cmd = new SqlCommand(sqlQuery, con))
            {
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        File newFile = new File()
                        {
                            FileID = Convert.ToInt32(reader["FileID"]),
                            FileName = reader["FileName"].ToString(),
                            FileSize = Convert.ToInt32(reader["FileSize"]),
                            ContentType = reader["ContentType"].ToString(),
                            FileExtension = reader["FileExtension"].ToString(),
                            FileContent = Encoding.ASCII.GetBytes(reader["FileContent"].ToString())
                        };
                        allFiles.Add(newFile);
                    }
                }
                FileList.DataSource = allFiles;
                FileList.DataBind();
            }
        }

        protected void btnUpload_Click(object sender, EventArgs e)
        {
            // Code for Upload file to database
            if (FileUpload1.HasFile)
            {
                HttpPostedFile file = FileUpload1.PostedFile;
                BinaryReader br = new BinaryReader(file.InputStream);
                byte[] buffer = br.ReadBytes(file.ContentLength);

                string strcon = ConfigurationManager.ConnectionStrings["CyberCityDB"].ConnectionString;
                SqlConnection con = new SqlConnection(strcon);
                String sqlQuery = "INSERT INTO [File](FileName,FileSize,ContentType,FileExtension,FileContent) VALUES(@param1,@param2,@param3,@param4,@param5)";
                List<File> allFiles = new List<File>();


                con.Open();

                //Int32 lastId = 0;
                //string lastIdSql = "SELECT TOP 1 FileID FROM [File] ORDER BY FileID DESC";
                //using (SqlCommand cmd = new SqlCommand(lastIdSql, con))
                //{
                //    if (cmd.ExecuteScalar() == null)
                //    {
                //        lastId = 1;
                //    }
                //    else
                //    {
                //        lastId = (Int32)cmd.ExecuteScalar();
                //        lastId += 1;
                //    }
                //}

                using (SqlCommand cmd = new SqlCommand(sqlQuery, con))
                {
                    cmd.Parameters.Add("@param1", SqlDbType.VarChar, 200).Value = file.FileName;
                    cmd.Parameters.Add("@param2", SqlDbType.Int).Value = file.ContentLength;
                    cmd.Parameters.Add("@param3", SqlDbType.VarChar, 200).Value = file.ContentType;
                    cmd.Parameters.Add("@param4", SqlDbType.VarChar, 10).Value = Path.GetExtension(file.FileName);
                    cmd.Parameters.Add("@param5", SqlDbType.VarBinary).Value = buffer;
                    cmd.CommandType = CommandType.Text;
                    cmd.ExecuteNonQuery();
                }

                PopulateUploadedFiles();
            }
        }

        protected void FileList_ItemCommand(object source, DataListCommandEventArgs e)
        {
            if (e.CommandName == "Download")
            {
                int fileID = Convert.ToInt32(e.CommandArgument);

                string strcon = ConfigurationManager.ConnectionStrings["CyberCityDB"].ConnectionString;
                SqlConnection con = new SqlConnection(strcon);
                String sqlQuery = "SELECT * from [File] WHERE FileID =" + fileID;

                con.Open();
                using (SqlCommand cmd = new SqlCommand(sqlQuery, con))
                {

                    File newFile = new File();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            newFile.FileID = Convert.ToInt32(reader["FileID"]);
                            newFile.FileName = reader["FileName"].ToString();
                            newFile.FileSize = Convert.ToInt32(reader["FileSize"]);
                            newFile.ContentType = reader["ContentType"].ToString();
                            newFile.FileExtension = reader["FileExtension"].ToString();
                            newFile.FileContent = (byte[])reader["FileContent"];
                        }

                        if (newFile.FileID > 0)
                        {
                            byte[] fileData = newFile.FileContent;
                            Response.AddHeader("Content-type", newFile.ContentType);
                            Response.AddHeader("Content-Disposition", "attachment; filename=" + newFile.FileName);

                            byte[] dataBlock = new byte[0x1000];
                            long fileSize;
                            int bytesRead;
                            long totalsBytesRead = 0;

                            using (Stream st = new MemoryStream(fileData))
                            {
                                fileSize = st.Length;
                                while (totalsBytesRead < fileSize)
                                {
                                    if (Response.IsClientConnected)
                                    {
                                        bytesRead = st.Read(dataBlock, 0, dataBlock.Length);
                                        Response.OutputStream.Write(dataBlock, 0, bytesRead);

                                        Response.Flush();
                                        totalsBytesRead += bytesRead;
                                    }
                                }
                            }
                            Response.End();
                        }

                    }
                }
            }
        }
        protected void ContactSubmissionGrid_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                e.Row.Attributes["onclick"] = Page.ClientScript.GetPostBackClientHyperlink(ContactSubmissionGrid, "Select$" + e.Row.RowIndex);
                e.Row.Attributes["style"] = "cursor:pointer";
            }
        }
        
        //SMTP information in WebConfig file MUST MATCH coordinator email address.
        public static String coordinatorEmailAddress = "SamSmith25d@gmail.com";
        public static String sendToEmailAddress = "elpheeti@dukes.jmu.edu";
        //public static String sendToEmailAddress = "maxwell.vaughan.mv@gmail.com";


        protected void Email_Instructor_EventCreated()
        {
            SmtpClient smtpClient = new SmtpClient();
            System.Net.Mail.MailMessage msg = new System.Net.Mail.MailMessage();
            msg.To.Add(sendToEmailAddress);
            msg.IsBodyHtml = true;
            msg.Subject = "Your invited to a CyberCity event";
            msg.Body = "Cyber City Coordinator, " + coordinatorEmailAddress + " has invited you to a Cyber City event! " +
               "The Access Code Instructors will need: " + MasterAccessCode.GetHashCode() + "  <br/>" +
                "The Class Code Volunteers and Parents will need: " + clusterCode.GetHashCode() + " <br />" +
                "Please encourage parents to create an account in order to sign photo release forms early. <br/>"+
                "<br />" +
                "<br />" +
                "Please Do Not Reply to this auto generated email.";
            smtpClient.Send(msg);
        }

        protected void Email_Volunteer_EventCreated()
        {
            SmtpClient smtpClient = new SmtpClient();
            System.Net.Mail.MailMessage msg = new System.Net.Mail.MailMessage();
            msg.To.Add(sendToEmailAddress);
            msg.IsBodyHtml = true;
            msg.Subject = "Your invited to a CyberCity event";
            msg.Body = "Cyber City Coordinator, " + coordinatorEmailAddress + " has invited you to a Cyber City event! " +
               "The Access Code you will need to access the system: " + MasterAccessCode.GetHashCode() + "  <br/>" +
                "The Class Code you need to distribute to Parents: " + clusterCode.GetHashCode() + " <br />" +
                "Please encourage parents to create an account in order to sign photo release forms early. <br/>" +
                "<br />" +
                "<br />" +
                "Please Do Not Reply to this auto generated email.";
            smtpClient.Send(msg);
        }

        protected void ContactSubmissionGrid_SelectedIndexChanged(object sender, EventArgs e)
        {
            string RequestID = ContactSubmissionGrid.SelectedRow.Cells[0].Text;

            foreach (GridViewRow row in ContactSubmissionGrid.Rows)
            {
                if (row.RowIndex == ContactSubmissionGrid.SelectedIndex)
                {
                    row.BackColor = ColorTranslator.FromHtml("#A1DCF2");
                    row.ToolTip = string.Empty;
                }
                else
                {
                    row.BackColor = ColorTranslator.FromHtml("#FFFFFF");
                    row.ToolTip = "Click to select this row.";
                }
            }
            MessageBox.Show(RequestID);
        }

        protected void btnSubmitCode_Click(object sender, EventArgs e)
        {
            instructorviewandedit.Attributes.Add("style", "margin - top: 40px; display = none");
            instructorviewandedit.Visible = false;

            VolunteerViewInfo.Attributes.Add("style", "margin-top: 40px; display = none");
            VolunteerViewInfo.Visible = false;

            ParentRegisterAndAttach.Attributes.Add("style", "margin-top: 40px; display = none");
            ParentRegisterAndAttach.Visible = false;

            string code = tbAccessCode.Text;
            string type = "";
            contactCode = code;
            instructorCode = code;
            volunteerCode = code;
            StudentCode = code;
            clusterCode = code;
            SqlConnection dbConnection = new SqlConnection(WebConfigurationManager.ConnectionStrings["CyberCityDB"].ConnectionString);
            dbConnection.Open();
            try
            {
                SqlCommand loginCommand = new SqlCommand();
                loginCommand.Connection = dbConnection;
                loginCommand.CommandText = "Select * from AccessCode where Code = @Code";
                loginCommand.Parameters.Add(new SqlParameter("@Code", code));
                //loginCommand.Parameters.Add(new SqlParameter("@UserType", type));
                SqlDataReader reader = loginCommand.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        type = reader[1].ToString();
                        if (type.Equals("Instructor"))
                        {
                            instructorviewandedit.Attributes.Add("style", "margin-top: 40px; display = normal");
                            instructorviewandedit.Visible = true;
                            string qry1 = "Select * from Instructor where InstructorCode ='" + code + "'";
                            string qry2 = "Select * from Organization inner join Instructor on Organization.OrganizationID = Instructor.OrganizationID where Instructor.InstructorCode ='" + code + "'";
                            SqlConnection aa = new SqlConnection(WebConfigurationManager.ConnectionStrings["CyberCityDB"].ConnectionString);
                            aa.Open();
                            SqlCommand instCom = new SqlCommand(qry1, aa);
                            SqlDataReader instReader = instCom.ExecuteReader();
                            while (instReader.Read())
                            {
                                tbName_Instructor.Text = (HttpUtility.HtmlEncode(instReader[1].ToString()));
                                tbEmail_Instructor.Text = (HttpUtility.HtmlEncode(instReader[3].ToString()));
                                tbPhone_Instructor.Text = (HttpUtility.HtmlEncode(instReader[4].ToString()));
                            }
                            SqlConnection bb = new SqlConnection(WebConfigurationManager.ConnectionStrings["CyberCityDB"].ConnectionString);
                            bb.Open();
                            SqlCommand instorg = new SqlCommand(qry2, bb);
                            SqlDataReader orgReader = instorg.ExecuteReader();
                            while (orgReader.Read())
                            {
                               lblOrganization_Show.Text = (HttpUtility.HtmlEncode(orgReader[1].ToString()));
                            }
                            sqlsrcViewStudents.ConnectionString = "SELECT Name, Age, Notes, MealTicket as 'Meal Ticket Confirmation' from Student where Student.InstructorCode = '" + code + "'";
                            sqlsrcViewStudents.DataBind();
                            

                        }
                        else if (type.Equals("Volunteer"))
                        {
                            VolunteerViewInfo.Attributes.Add("style", "margin-top: 40px; display = normal");
                            VolunteerViewInfo.Visible = true;
                            string qry3 = "Select * from Volunteer where Volunteer.VolunteerCode ='" + code + "'";
                            SqlConnection ff = new SqlConnection(WebConfigurationManager.ConnectionStrings["CyberCityDB"].ConnectionString);
                            ff.Open();
                            SqlCommand VolCom = new SqlCommand(qry3, ff);
                            SqlDataReader VolReader = VolCom.ExecuteReader();
                            while (VolReader.Read())
                            {
                                tbName_Volunteer.Text = (HttpUtility.HtmlEncode(VolReader[1].ToString()));
                                tbEmail_Volunteer.Text = (HttpUtility.HtmlEncode(VolReader[4].ToString()));
                                tbPhone_Volunteer.Text = (HttpUtility.HtmlEncode(VolReader[3].ToString()));
                            }
                        }
                        else if (type.Equals("Student"))
                        {
                            ParentRegisterAndAttach.Attributes.Add("style", "margin-top: 40px; display = normal");
                            ParentRegisterAndAttach.Visible = true;
                            string sqlQuery_StudentInfo = "SELECT * FROM STUDENT WHERE StudentCode = '" + code + "'";
                            string sqlQueryFindOrganizationName = "SELECT Organization.Name FROM Organization inner join Student on Student.OrganizationID = Organization.OrganizationID where Student.StudentCode = '" + code + "'";

                            SqlConnection sqlconnect = new SqlConnection(ConfigurationManager.ConnectionStrings["CyberCityDB"].ConnectionString);
                            sqlconnect.Open();
                            SqlCommand StudentStuff = new SqlCommand(sqlQuery_StudentInfo, sqlconnect);
                            SqlDataReader studentReader = StudentStuff.ExecuteReader();
                            while (studentReader.Read())
                            {
                                tbName_Student.Text = (HttpUtility.HtmlEncode(studentReader[1].ToString()));
                                tbNotes_Student.Text = (HttpUtility.HtmlEncode(studentReader[3].ToString()));
                                tbAge_Student.Text = (HttpUtility.HtmlEncode(studentReader[2].ToString()));
                            }
                            SqlConnection sqlcon = new SqlConnection(ConfigurationManager.ConnectionStrings["CyberCityDB"].ConnectionString);
                            sqlcon.Open();
                            SqlCommand orgCom = new SqlCommand(sqlQueryFindOrganizationName, sqlcon);
                            SqlDataReader orgReader = orgCom.ExecuteReader();
                            while (orgReader.Read())
                            {
                                //lblOrganization_Student_Show.Text = (HttpUtility.HtmlEncode(orgReader[1].ToString()));
                            }
                        }
                    }
                }
            }
            catch (System.Data.SqlClient.SqlException ex)
            {
                string msg = "Insert/Update Error:";
                msg += ex.Message;
                throw new Exception(msg);
            }
            finally
            {
                dbConnection.Close();
            }
        }


        protected void btnUpdateInstructorInfo_Click(object sender, EventArgs e)
        {
            // NEED INSTRUCTOR CODE TO BE STATIC HERE 
            SqlConnection sqlconnect = new SqlConnection(ConfigurationManager.ConnectionStrings["CyberCityDB"].ConnectionString);
            sqlconnect.Open();

            String sqlQuery_UpdateInstructor = "UPDATE Instructor SET Name = @Name, Email = @Email, Phone = @Phone WHERE InstructorCode = @InstructorCode";

            SqlCommand cmd_UpdateInstructor = new SqlCommand(sqlQuery_UpdateInstructor, sqlconnect);
            cmd_UpdateInstructor.Parameters.Add(new SqlParameter("@Name", tbName_Instructor.Text));
            cmd_UpdateInstructor.Parameters.Add(new SqlParameter("@Email", tbEmail_Instructor.Text));
            cmd_UpdateInstructor.Parameters.Add(new SqlParameter("@Phone", tbPhone_Instructor.Text));
            cmd_UpdateStudent.Parameters.Add(new SqlParameter("@InstructorCode", instructorCode));


            try
            {
                cmd_UpdateInstructor.CommandType = CommandType.Text;
                cmd_UpdateInstructor.ExecuteNonQuery();
            }
            catch (System.Data.SqlClient.SqlException ex)
            {
                string msg = "Update Info Error into Teacher";
                msg += ex.Message;
                throw new Exception(msg);
            }
        }

        protected void btnReset_Instructor_Click(object sender, EventArgs e)
        {
            tbName_Instructor.Text = "";
            tbEmail_Instructor.Text = "";
            tbPhone_Instructor.Text = "";
        }

        protected void btnUpdateVolunteerInfo_Click(object sender, EventArgs e)
        {
            SqlConnection sqlconnect = new SqlConnection(ConfigurationManager.ConnectionStrings["CyberCityDB"].ConnectionString);
            sqlconnect.Open();

            String sqlQuery_UpdateVolunteer = "UPDATE Volunteer SET Name = @Name, Email = @Email, Phone = @Phone WHERE VolunteerCode = @VolunteerCode";

            SqlCommand cmd_UpdateVolunteer = new SqlCommand(sqlQuery_UpdateVolunteer, sqlconnect);
            cmd_UpdateVolunteer.Parameters.Add(new SqlParameter("@Name", tbName_Volunteer.Text));
            cmd_UpdateVolunteer.Parameters.Add(new SqlParameter("@Email", tbEmail_Volunteer.Text));
            cmd_UpdateVolunteer.Parameters.Add(new SqlParameter("@Phone", tbPhone_Volunteer.Text));
            cmd_UpdateStudent.Parameters.Add(new SqlParameter("@VolunteerCode", volunteerCode));


            try
            {
                cmd_UpdateVolunteer.CommandType = CommandType.Text;
                cmd_UpdateVolunteer.ExecuteNonQuery();
            }
            catch (System.Data.SqlClient.SqlException ex)
            {
                string msg = "Update Info Error into Volunteer";
                msg += ex.Message;
                throw new Exception(msg);
            }
        }


        protected void btnClearStudent_Click(object sender, EventArgs e)
        {
            tbName_Student.Text = "";
            tbNotes_Student.Text = "";
            tbAge_Student.Text = "";
            rbtnMeal_No.Checked = false;
            rbtnMeal_Yes.Checked = false;
        }

        protected void btnUpdateStudent_Click(object sender, EventArgs e)
        {
            SqlConnection sqlconnect = new SqlConnection(ConfigurationManager.ConnectionStrings["CyberCityDB"].ConnectionString);
            sqlconnect.Open();

            string mealConfirmation = "";

            String sqlQuery_UpdateStudent= "UPDATE Student SET Name = @Name, Age = @Age, Notes = @Notes, MealTicket = @MealTicket WHERE StudentCode = @StudentCode";

            if (rbtnMeal_No.Checked == true)
            {
                mealConfirmation = "no";
            }
            if (rbtnMeal_Yes.Checked == true)
            {
                mealConfirmation = "yes";
            }


            SqlCommand cmd_UpdateStudent = new SqlCommand(sqlQuery_UpdateStudent, sqlconnect);
            cmd_UpdateStudent.Parameters.Add(new SqlParameter("@Name", tbName_Student.Text));
            cmd_UpdateStudent.Parameters.Add(new SqlParameter("@Age", tbAge_Student.Text));
            cmd_UpdateStudent.Parameters.Add(new SqlParameter("@Notes", tbNotes_Student.Text));
            cmd_UpdateStudent.Parameters.Add(new SqlParameter("@MealTicket", mealConfirmation.ToString()));
            cmd_UpdateStudent.Parameters.Add(new SqlParameter("@StudentCode", StudentCode));


            try
            {
                cmd_UpdateStudent.CommandType = CommandType.Text;
                cmd_UpdateStudent.ExecuteNonQuery();
            }
            catch (System.Data.SqlClient.SqlException ex)
            {
                string msg = "Update Info Error into Student";
                msg += ex.Message;
                throw new Exception(msg);
            }
        }
    }
}
