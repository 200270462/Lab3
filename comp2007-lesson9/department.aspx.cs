using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using comp2007_lesson9.Models;
using System.Web.ModelBinding;


namespace comp2007_lesson9
{
    public partial class department : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //if the page isn't posted back, check the url for an id to see know add or edit
            if (!IsPostBack)
            {
                if (Request.QueryString.Keys.Count > 0)
                {
                    //we have a url parameter if the count is > 0 so populate the form
                    GetDepartment();
                }
            }
        }

        protected void GetDepartment()
        {
            //connect
            using (comp2007Entities conn = new comp2007Entities())
            {
                //get id from url parameter and store in a variable
                Int32 DepartmentID = Convert.ToInt32(Request.QueryString["DepartmentID"]);

                var d = (from dep in conn.Departments
                         where dep.DepartmentID == DepartmentID
                         select dep).FirstOrDefault();

                //populate the form from our department object
                txtName.Text = d.Name;
                txtBudget.Text = d.Budget.ToString();

                var objD = (from c in conn.Courses
                            join dep in conn.Departments on c.DepartmentID equals d.DepartmentID
                            where c.DepartmentID == DepartmentID
                            select new { c.CourseID, c.Title, c.Credits});

                grdCourses.DataSource = objD.ToList();
                grdCourses.DataBind();
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            //connect
            using (comp2007Entities conn = new comp2007Entities())
            {
                //instantiate a new deparment object in memory
                Department d = new Department();

                //decide if updating or adding, then save
                if (Request.QueryString.Count > 0)
                {
                    Int32 DepartmentID = Convert.ToInt32(Request.QueryString["DepartmentID"]);

                    d = (from dep in conn.Departments
                         where dep.DepartmentID == DepartmentID
                         select dep).FirstOrDefault();
                }

                //fill the properties of our object from the form inputs
                d.Name = txtName.Text;
                d.Budget = Convert.ToDecimal(txtBudget.Text);

                if (Request.QueryString.Count == 0)
                {
                    conn.Departments.Add(d);
                }
                conn.SaveChanges();

                //redirect to updated departments page
                Response.Redirect("departments.aspx");
            }
        }

        protected void grdCourses_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            Int32 CourseID = Convert.ToInt32(grdCourses.DataKeys[e.RowIndex].Values["CourseID"]);

            using (comp2007Entities db = new comp2007Entities())
            {
                //get selected record
                Course objC = (from c in db.Courses
                                   where c.CourseID == CourseID
                                   select c).FirstOrDefault();

                //Delete
                db.Courses.Remove(objC);
                db.SaveChanges();

                //Refresh the data on the page
                GetDepartment();
            }
        }
    }
}