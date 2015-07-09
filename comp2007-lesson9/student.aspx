<%@ Page Title="Student Details" Language="C#" MasterPageFile="~/site.Master" AutoEventWireup="true" CodeBehind="student.aspx.cs" Inherits="comp2007_lesson9.student" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <h1>Student Details</h1>
    <h5>All fields are required</h5>

    <fieldset>
        <label for="txtLastName" class="col-sm-2">Last Name:</label>
        <asp:TextBox ID="txtLastName" runat="server" MaxLength="50" />
    </fieldset>
    <fieldset>
        <label for="txtFirstMidName" class="col-sm-2">First Name:</label>
        <asp:TextBox ID="txtFirstMidName" runat="server" MaxLength="50" />
    </fieldset>
    <fieldset>
        <label for="txtEnrollmentDate" class="col-sm-2">Enrollment Date:</label>
        <asp:TextBox ID="txtEnrollmentDate" runat="server" TextMode="Date" />
        <asp:RangeValidator ID="RangeValidator1" runat="server" ErrorMessage="Must be a Date"
            ControlToValidate="txtEnrollmentDate" CssClass="alert alert-danger"
            Type="Date" MinimumValue="2000-01-01" MaximumValue="2999-12-31"></asp:RangeValidator>
    </fieldset>

    <div class="col-sm-offset-2">
        <asp:Button ID="btnSave" runat="server" Text="Save" CssClass="btn btn-primary"
             OnClick="btnSave_Click" />
    </div>
    <div>
        <h1>Courses</h1>
        <asp:GridView ID="grdCourses" runat="server" CssClass="table table-striped table-hover"
        AutoGenerateColumns="false" OnRowDeleting="grdCourses_RowDeleting" DataKeyNames="EnrollmentID" >
        <Columns>
            <asp:BoundField DataField="Name" HeaderText="Department" SortExpression="Name"/>
            <asp:BoundField DataField="Title" HeaderText="Course Title" SortExpression="Title" />
            <asp:BoundField DataField="Grade" HeaderText="Grade" SortExpression="Grade"/>
            <asp:CommandField HeaderText="Delete" DeleteText="Delete" ShowDeleteButton="true" />
        </Columns>
    </asp:GridView>
    </div>
</asp:Content>
