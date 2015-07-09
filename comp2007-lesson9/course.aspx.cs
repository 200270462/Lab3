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
    public partial class course : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                GetDepartments();

                //get the course if editing
                if (!String.IsNullOrEmpty(Request.QueryString["CourseID"]))
                {
                    GetCourse();
                }
            }
        }

        protected void GetCourse()
        {
            //populate the existing course for editing
            using (comp2007Entities db = new comp2007Entities())
            {
                Int32 CourseID = Convert.ToInt32(Request.QueryString["CourseID"]);

                Course objC = (from c in db.Courses
                               where c.CourseID == CourseID
                               select c).FirstOrDefault();

                //populate the form
                txtTitle.Text = objC.Title;
                txtCredits.Text = objC.Credits.ToString();
                ddlDepartment.SelectedValue = objC.DepartmentID.ToString();

                var objS = (from s in db.Students
                            join en in db.Enrollments on s.StudentID equals en.StudentID
                            join c in db.Courses on en.CourseID equals c.CourseID
                            where c.CourseID == CourseID
                            select new { s.StudentID, s.LastName, s.FirstMidName, s.EnrollmentDate });

                grdStudents.DataSource = objS.ToList();
                grdStudents.DataBind();
            }
        }

        protected void GetDepartments()
        {
            using (comp2007Entities db = new comp2007Entities())
            {
                var deps = (from d in db.Departments
                            orderby d.Name
                            select d);

                ddlDepartment.DataSource = deps.ToList();
                ddlDepartment.DataBind();
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            //do insert or update
            using (comp2007Entities db = new comp2007Entities())
            {
                Course objC = new Course();

                if (!String.IsNullOrEmpty(Request.QueryString["CourseID"]))
                {
                    Int32 CourseID = Convert.ToInt32(Request.QueryString["CourseID"]);
                    objC = (from c in db.Courses
                            where c.CourseID == CourseID
                            select c).FirstOrDefault();
                }

                //populate the course from the input form
                objC.Title = txtTitle.Text;
                objC.Credits = Convert.ToInt32(txtCredits.Text);
                objC.DepartmentID = Convert.ToInt32(ddlDepartment.SelectedValue);

                if (String.IsNullOrEmpty(Request.QueryString["CourseID"]))
                {
                    //add
                    db.Courses.Add(objC);
                }

                //save and redirect
                db.SaveChanges();
                Response.Redirect("courses.aspx");
            }
        }

        protected void grdStudents_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            Int32 StudentID = Convert.ToInt32(grdStudents.DataKeys[e.RowIndex].Values["StudentID"]);

            using (comp2007Entities db = new comp2007Entities())
            {
                //get selected record
                Student objE = (from s in db.Students
                                   where s.StudentID == StudentID
                                   select s).FirstOrDefault();

                //Delete
                db.Students.Remove(objE);
                db.SaveChanges();

                //Refresh the data on the page
                GetCourse();
            }
        }
    }
}