<%@ Page Title="Home Page" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true"
    CodeBehind="Producers.aspx.cs" Inherits="ExcelImport.WebUI.Producers" %>

<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
</asp:Content>
<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
   
    <asp:Repeater runat="server" ID="rptRecipes" OnItemDataBound="rptRecipes_OnItemDataBound">
        <HeaderTemplate>
            <ul>
                <table>
                    <tr>
                        <td valign="top"><input type="checkbox" ID="Checkall"  /></td>
                    </tr>
        </HeaderTemplate>
        <ItemTemplate>
            <tr>
                <td valign="top"><input type="checkbox" runat="server" ID="chkRecipe"  /></td>
                <td>
                    <div clientidmode="Inherit" runat="server" ID="divRecipe">
                        <asp:Label runat="server" ID="lblRecipe" CssClass="name" />
                        <div class="hide">
                            <asp:Label runat="server" ID="lblProductType" CssClass="ProductType" /><br/>
                            <asp:Label runat="server" ID="lblVariation" CssClass="Variation" /><br/>
                            <asp:Label runat="server" ID="lblExceptions" CssClass="Exceptions" /><br/>
                            <asp:Label runat="server" ID="lblBrand" CssClass="Brand" /><br/>
                            <asp:Label runat="server" ID="lblProducerName" CssClass="ProducerName" /><br/>
                            <asp:Label runat="server" ID="lblFirstName" CssClass="FirstName" /><br/>
                            <asp:Label runat="server" ID="lblLastName" CssClass="LastName" /><br/>
                            <asp:Label runat="server" ID="lblAddressLine1" CssClass="AddressLine1" /><br/>
                            <asp:Label runat="server" ID="lblAddressLine2" CssClass="AddressLine2" /><br/>
                            <asp:Label runat="server" ID="lblState" CssClass="State" /><br/>
                            <asp:Label runat="server" ID="lblPostCode" CssClass="PostCode" /><br/>
                            <asp:Label runat="server" ID="lblCountry" CssClass="Country" /><br/>
                            <asp:Label runat="server" ID="lblCoOrdinates" CssClass="CoOrdinates" /><br/>
                            <asp:Label runat="server" ID="lblBiography" CssClass="Biography" />
                        </div>
                    </div>
                </td>
            </tr>
        </ItemTemplate>
        <FooterTemplate>
                </table>
            </ul>
        </FooterTemplate>
    </asp:Repeater>

</asp:Content>
