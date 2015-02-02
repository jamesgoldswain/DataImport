<%@ Page Title="Home Page" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true"
    CodeBehind="Default.aspx.cs" Inherits="ExcelImport.WebUI._Default" %>

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
                            <asp:Label runat="server" ID="lblDescription" CssClass="description" /><br/>
                            <asp:Label runat="server" ID="lblPreparationTime" CssClass="preparationTime" /><br/>
                            <asp:Label runat="server" ID="lblCookingTime" CssClass="cookingTime" /><br/>
                            <asp:Label runat="server" ID="lblServes" CssClass="serves" /><br/>
                            <asp:Label runat="server" ID="lblIngredients" CssClass="ingredients" /><br/>
                            <asp:Label runat="server" ID="lblMethod" CssClass="method" /><br/>
                            <asp:Label runat="server" ID="lblKeywords" CssClass="keywords" /><br/>
                            <asp:Label runat="server" ID="lblSuggestion" CssClass="suggestion" /><br/>
                            <asp:Label runat="server" ID="lblAssociatedAppliances" CssClass="associatedAppliances" /><br/>
                            <asp:Label runat="server" ID="lblProductCategory" CssClass="productCategory" /><br/>
                            <asp:Label runat="server" ID="lblMealType" CssClass="mealType" /><br/>
                            <asp:Label runat="server" ID="lblImage" CssClass="image" />
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
