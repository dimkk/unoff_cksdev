<%@ Page Language="C#" Inherits="$rootnamespace$.Manage$subnamespace$ApplicationPage, $SharePoint.Project.AssemblyFullName$" MasterPageFile="~/_layouts/dialog.master" %>
<%@ Register TagPrefix="wssuc" TagName="InputFormSection" src="~/_controltemplates/InputFormSection.ascx" %>
<%@ Register TagPrefix="wssuc" TagName="InputFormControl" src="~/_controltemplates/InputFormControl.ascx" %>
<%@ Register TagPrefix="wssuc" TagName="ButtonSection" src="~/_controltemplates/ButtonSection.ascx" %>
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>

<asp:Content ID="Content1" ContentPlaceHolderID="PlaceHolderDialogHeaderPageTitle" runat="server">
Manage $fileinputname$ Application
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="PlaceHolderDialogBodyMainSection" runat="server">
    <table border="0" cellpadding="0" cellspacing="0">
        <wssuc:InputFormSection runat="server"
            Title="Sample Title"
            Description="Sample Description">
            <Template_InputFormControls>
                <wssuc:InputFormControl runat="server">
                    <Template_Control>
                        <SharePoint:InputFormTextBox runat="server" ID="sampleField" />
                    </Template_Control>
                </wssuc:InputFormControl>
            </Template_InputFormControls>
        </wssuc:InputFormSection>
    </table>
</asp:Content>
