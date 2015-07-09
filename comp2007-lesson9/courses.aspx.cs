﻿using System;
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
    public partial class courses : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //if loading the page for the first time, populate course grid
            if (!IsPostBack)
            {
                Session["SortColumn"] = "CourseID";
                Session["SortDirection"] = "ASC";
                GetCourses();
            }
        }

        protected void GetCourses()
        {
            //connect to EF
            using (comp2007Entities db = new comp2007Entities())
            {
                //query the courses table using EF and LINQ
                var Course = from c in db.Courses
                               select new {c.CourseID, c.Title, c.Credits, c.Department.Name};

                //apend the current direction to the sort column
                String SortString = Session["SortColumn"].ToString() + " " + Session["SortDirection"].ToString();

                //bind the result to the gridview
                grdCourses.DataSource = Course.AsQueryable().OrderBy(SortString).ToList();
                grdCourses.DataBind();

            }
        }

        protected void grdCourses_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            //store which row was clicked
            Int32 selectedRow = e.RowIndex;

            //get the selected StudentID using the grid's Data Key collection
            Int32 CourseID = Convert.ToInt32(grdCourses.DataKeys[selectedRow].Values["CourseID"]);

            //use EF to remove the selected student from the db
            using (comp2007Entities db = new comp2007Entities())
            {

                Course c = (from objS in db.Courses
                             where objS.CourseID == CourseID
                             select objS).FirstOrDefault();

                //do the delete
                db.Courses.Remove(c);
                db.SaveChanges();
            }

            //refresh the grid
            GetCourses();
        }
        protected void grdCourses_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            //set the new page #
            grdCourses.PageIndex = e.NewPageIndex;
            GetCourses();
        }

        protected void ddlPageSize_SelectedIndexChanged(object sender, EventArgs e)
        {
            //set the page size
            grdCourses.PageSize = Convert.ToInt32(ddlPageSize.SelectedValue);
            GetCourses();
        }

        protected void grdCourses_Sorting(object sender, GridViewSortEventArgs e)
        {
            //get the column to sort
            Session["SortColumn"] = e.SortExpression;

            //reload the grid
            GetCourses();

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

        protected void grdCourses_RowDataBound(object sender, GridViewRowEventArgs e)
        {
             if (IsPostBack) { 
                if (e.Row.RowType == DataControlRowType.Header)
                {
                    Image SortImage = new Image();
                
                    for (int i = 0; i <= grdCourses.Columns.Count -1; i++) {
                        if (grdCourses.Columns[i].SortExpression == Session["SortColumn"].ToString())
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
    }
}