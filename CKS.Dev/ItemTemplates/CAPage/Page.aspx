<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Import Namespace="Microsoft.SharePoint.ApplicationPages" %>
<%@ Import Namespace="Microsoft.SharePoint" %>

<%@ Register TagPrefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=12.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=12.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="wssawc" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=12.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c"%>
<%@ Register TagPrefix="wssuc" TagName="ToolBar" src="~/_controltemplates/ToolBar.ascx" %>
<%@ Register TagPrefix="wssuc" TagName="InputFormSection" src="~/_controltemplates/InputFormSection.ascx" %>
<%@ Register TagPrefix="wssuc" TagName="InputFormControl" src="~/_controltemplates/InputFormControl.ascx" %>
<%@ Register TagPrefix="wssuc" TagName="ButtonSection" src="~/_controltemplates/ButtonSection.ascx" %>

<%@ Page Language="C#" 
    AutoEventWireup="true" 
    CodeBehind="$fileinputname$.aspx.cs"
    Inherits="$rootnamespace$.$safeitemrootname$" 
    MasterPageFile="~/_admin/admin.master" %>

<asp:Content ID="PageHead" ContentPlaceHolderID="PlaceHolderAdditionalPageHead" runat="server">
</asp:Content>

<asp:Content ID="Main" ContentPlaceHolderID="PlaceHolderMain" runat="server">
    <asp:Panel ID="inputControls" runat="server">
        <table border="0" cellspacing="0" cellpadding="0" width="100%">
            <tr>
                <td width="50%">
                    <!-- **************************************
                     use sharepoint buttonsection control
                     to display the "ok" and "cancel" buttons -->
                    <wssuc:buttonsection runat="server" topbuttons="true" bottomspacing="5" showsectionline="false"
                        showstandardcancelbutton="false">
                    <Template_Buttons>
                        <asp:Button UseSubmitBehavior="false" runat="server"
                            class="ms-ButtonHeightWidth"
                            Text="<%$Resources:wss,multipages_okbutton_text%>"
                            id="btnSubmitTop" ToolTip="Save current settings"
                            accesskey="<%$Resources:wss,okbutton_accesskey%>"
                            Enabled="true"/>
                        <asp:Button UseSubmitBehavior="false" runat="server"
                            class="ms-ButtonHeightWidth"
                            Text="<%$Resources:wss,multipages_cancelbutton_text%>"
                            CausesValidation="False"
                            id="btnCancelTop"
                            CommandName="Cancel"
                            accesskey="<%$Resources:wss,multipages_cancelbutton_accesskey%>"
                            Enabled="true"/>
                    </Template_Buttons>
                </wssuc:buttonsection>
                    <!-- ************************************** -->
                    <!-- **************************************
                     display the web application selector
                     using the inputformsecton and
                     webapplicationselector controls -->
                    <wssuc:inputformsection runat="server" title="Web Application" description="Select a Web Application">
                    <Template_InputFormControls>
                    <tr>
                        <td>
                        <SharePoint:WebApplicationSelector id="WebAppSelector" runat="server"
						  			    TypeLabelCssClass="ms-listheaderlabel"
						  			    HoverCellActiveCssClass = "ms-viewselectorhover"
									    HoverCellInActiveCssClass = "ms-viewselector" /><br />
							    <span style="padding-left:6px;"><asp:Literal ID="litWebAppName" runat="server" /></span>
                        </td>
                    </tr>
                    </Template_InputFormControls>
                </wssuc:inputformsection>
                    <!-- *************************************** -->
                    <!-- ***************************************
                    display an input form section -->
                    <wssuc:inputformsection runat="server" title="Input Form Section" description="An input form section description">
                    <Template_InputFormControls>
  			            <wssuc:InputFormControl runat="server"
				            LabelText="An Input Form Control">
				            <Template_Control>
					            <wssawc:InputFormTextBox Title="Configuration Site URL" class="ms-input" ID="txtConfigurationSiteURL" Columns="75" Runat="server" MaxLength=255 EnableViewState="true" />
					            <wssawc:InputFormRequiredFieldValidator
						            ID="ReqValtxtConfigurationSiteURL"
						            ControlToValidate="txtConfigurationSiteURL"
						            ErrorMessage="You must enter a valid URL!"
						            Runat="server"/>
				            </Template_Control>
			            </wssuc:InputFormControl>
                    </Template_InputFormControls>
                </wssuc:inputformsection>
                    <!-- ************************************** -->
                </td>
            </tr>
        </table>
    </asp:Panel>
    <!-- Literal Control to display messages -->
    <div style="font-size: 12pt; color: red; font-weight: bold;">
        <asp:Literal ID="litMessages" runat="server" />
    </div>
</asp:Content>

<asp:Content ID="PageTitle" ContentPlaceHolderID="PlaceHolderPageTitle" runat="server">
    CKSDev Administration Page
</asp:Content>

<asp:Content ID="PageTitleInTitleArea" ContentPlaceHolderID="PlaceHolderPageTitleInTitleArea" runat="server">
    CKSDev Administration Page
</asp:Content>

<asp:Content ID="PageDescription" ContentPlaceHolderID="PlaceHolderPageDescription" runat="server">
    Manage configuration settings.
</asp:Content>

