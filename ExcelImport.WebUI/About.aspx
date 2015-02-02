<%@ Page Title="About Us" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true"
    CodeBehind="About.aspx.cs" Inherits="ExcelImport.WebUI.About" %>

<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
</asp:Content>
<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
    <h2>
        About
    </h2>
    <p>
        <input id="isValid" type="text" runat="server"></input>
        <textarea id="something" runat="server" rows="100" cols="100"></textarea>
        <button>submit</button>
    </p>
</asp:Content>
