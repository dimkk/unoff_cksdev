<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Import Namespace="Microsoft.SharePoint.ApplicationPages" %>
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %>
<%@ Register TagPrefix="wssuc" TagName="InputFormSection" src="/_controltemplates/InputFormSection.ascx" %> 
<%@ Register TagPrefix="wssuc" TagName="InputFormControl" src="/_controltemplates/InputFormControl.ascx" %> 
<%@ Register TagPrefix="wssuc" TagName="ButtonSection" src="/_controltemplates/ButtonSection.ascx" %> 
<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Edit$fileinputname$.aspx.cs" Inherits="$rootnamespace$.$subnamespace$.ApplicationPages.Edit$fileinputname$" DynamicMasterPageFile="~masterurl/default.master" %>

<asp:Content ID="PageHead" ContentPlaceHolderID="PlaceHolderAdditionalPageHead" runat="server">

</asp:Content>

<asp:Content ID="Main" ContentPlaceHolderID="PlaceHolderMain" runat="server">

<table class="propertysheet" border="0" width="100%" cellspacing="0" cellpadding="0" id="diidProjectPageOverview">
    <wssuc:InputFormSection Title="<%$Resources:$rootnamespace$.$subnamespace$.$fileinputname$WebPart, CustomMessageSection_Title%>"
			Description="<%$Resources:$rootnamespace$.$subnamespace$.$fileinputname$WebPart, CustomMessageSection_Description%>"
			runat="server">
			<Template_InputFormControls>
				<wssuc:InputFormControl LabelText="<%$Resources:$rootnamespace$.$subnamespace$.$fileinputname$WebPart, CustomMessageSection_txtCustomMessage%>" runat="server">
					<Template_Control>
						<asp:TextBox runat="server" ID="txtCustomMessage"/>
					</Template_Control>
				</wssuc:InputFormControl>
			</Template_InputFormControls>
		</wssuc:InputFormSection>
        <wssuc:ButtonSection runat="server" ShowStandardCancelButton="false">
			<Template_Buttons>
				<asp:Button runat="server" CssClass="ms-ButtonHeightWidth" Text="<%$Resources:wss, multipages_okbutton_text%>" id="btnOk" onclick="btnOk_Click" accesskey="<%$Resources:wss,okbutton_accesskey%>"/>
                <asp:Button runat="server" CssClass="ms-ButtonHeightWidth" Text="<%$Resources:wss, multipages_cancelbutton_text%>" id="btnCancel" onclick="btnCancel_Click" CausesValidation="false" AccessKey="<%$Resources:wss,cancelbutton_accesskey%>"/>
			</Template_Buttons>
		</wssuc:ButtonSection>
</table>
</asp:Content>

<asp:Content ID="PageTitle" ContentPlaceHolderID="PlaceHolderPageTitle" runat="server">
    <asp:Literal ID="litPlaceHolderPageTitle" runat="server" />
</asp:Content>

<asp:Content ID="PageTitleInTitleArea" ContentPlaceHolderID="PlaceHolderPageTitleInTitleArea" runat="server" >
    <asp:Literal ID="litPlaceHolderPageTitleInTitleArea" runat="server" />
</asp:Content>
