using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

//reference the EF models
using comp2007_lesson9.Models;
using System.Web.ModelBinding;
using System.Linq.Dynamic;

namespace comp2007_lesson9
{
    public partial class departments : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //if loading the page for the first time, populate student grid
            if (!IsPostBack)
            {
                Session["SortColumn"] = "DepartmentID";
                Session["SortDirection"] = "ASC";
                GetDepartments();
            }
        }

        protected void GetDepartments()
        {
            //connect to EF
            using (comp2007Entities db = new comp2007Entities())
            {

                //query the students table using EF and LINQ
                var Department = from d in db.Departments
                               select d;

                //apend the current direction to the sort column
                String SortString = Session["SortColumn"].ToString() + " " + Session["SortDirection"].ToString();

                //bind the result to the gridview
                grdDepartments.DataSource = Department.AsQueryable().OrderBy(SortString).ToList();
                grdDepartments.DataBind();

            }
        }
        protected void grdDepartments_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            //store which row was clicked
            Int32 selectedRow = e.RowIndex;

            //get the selected StudentID using the grid's Data Key collection
            Int32 DepartmentID = Convert.ToInt32(grdDepartments.DataKeys[selectedRow].Values["DepartmentID"]);

            //use EF to remove the selected student from the db
            using (comp2007Entities db = new comp2007Entities())
            {

                Department d = (from objS in db.Departments
                             where objS.DepartmentID == DepartmentID
                             select objS).FirstOrDefault();

                //do the delete
                db.Departments.Remove(d);
                db.SaveChanges();
            }

            //refresh the grid
            GetDepartments();
        }

        protected void grdDepartments_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            //set the new page #
            grdDepartments.PageIndex = e.NewPageIndex;
            GetDepartments();
        }

        protected void grdDepartments_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (IsPostBack)
            {
                if (e.Row.RowType == DataControlRowType.Header)
                {
                    Image SortImage = new Image();

                    for (int i = 0; i <= grdDepartments.Columns.Count - 1; i++)
                    {
                        if (grdDepartments.Columns[i].SortExpression == Session["SortColumn"].ToString())
                        {
                            if (Session["SortDirection"].ToString() == "DESC")
                            {
                                SortImage.ImageUrl = "images/desc.jpg";
                                SortImage.AlternateText = "Sort Descending";
                            }
                            else
                            {
                                SortImage.ImageUrl = "images/asc.jpg";
                                SortImage.AlternateText = "Sort Ascending";
                            }

                            e.Row.Cells[i].Controls.Add(SortImage);

                        }
                    }
                }

            }
        }

        protected void ddlPageSize_SelectedIndexChanged(object sender, EventArgs e)
        {
            //set the page size
            grdDepartments.PageSize = Convert.ToInt32(ddlPageSize.SelectedValue);
            GetDepartments();
        }

        protected void grdDepartments_Sorting(object sender, GridViewSortEventArgs e)
        {
            //get the column to sort
            Session["SortColumn"] = e.SortExpression;

            //reload the grid
            GetDepartments();

            //toggle the direction
            if (Session["SortDirection"].ToString() == "ASC")
            {
                Session["SortDirection"] = "DESC";
            }
            else
            {
                Session["SortDirection"] = "ASC";
            }
        }
    }
}