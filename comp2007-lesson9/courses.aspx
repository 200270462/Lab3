<%@ Page Title="" Language="C#" MasterPageFile="~/site.Master" AutoEventWireup="true" CodeBehind="courses.aspx.cs" Inherits="comp2007_lesson9.courses" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <h1>Courses</h1>

    <a href="course.aspx">Add Course</a>
    <label for="ddlPageSize">Record Per Page:</label>
    <asp:DropDownList ID="ddlPageSize" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlPageSize_SelectedIndexChanged">
        <asp:ListItem Value="3" Text="3"/>
        <asp:ListItem Value="5" Text="5"/>
        <asp:ListItem Value="99999" Text="All"/>
    </asp:DropDownList>

    <asp:GridView ID="grdCourses" runat="server" CssClass="table table-striped table-hover"
        AutoGenerateColumns="false" OnRowDeleting="grdCourses_RowDeleting" DataKeyNames="CourseID" 
        AllowPaging="true" PageSize="3" OnPageIndexChanging="grdCourses_PageIndexChanging" 
        AllowSorting="true" OnSorting="grdCourses_Sorting" OnRowDataBound="grdCourses_RowDataBound">
        <Columns>
            <asp:BoundField DataField="CourseID" HeaderText="Course ID" SortExpression="CourseID"/>
            <asp:BoundField DataField="Title" HeaderText="Title" SortExpression="Title" />
            <asp:BoundField DataField="Credits" HeaderText="Credits" SortExpression="Credits"/>
            <asp:BoundField DataField="Name" HeaderText="Department Name" SortExpression="Name"/>
            <asp:HyperLinkField HeaderText="Edit" Text="Edit" NavigateUrl="course.aspx"
                DataNavigateUrlFields="CourseID" 
                DataNavigateUrlFormatString="course.aspx?CourseID={0}" />
            <asp:CommandField HeaderText="Delete" DeleteText="Delete" ShowDeleteButton="true" />
        </Columns>
    </asp:GridView>
</asp:Content>
