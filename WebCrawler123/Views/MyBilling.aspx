<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="MyBilling.aspx.cs" MasterPageFile="~/MasterPages/VendorDashboard.master" Inherits="WebCrawler.Views.MyBilling" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    
 
    <script type="text/javascript">

        if (sessionStorage.getItem("Id") == "" || sessionStorage.getItem("Id") == null) {
            //window.location = "VendorLogin.aspx";
        }
        var coordInfo;
        var notesInfo;
        var mapInfo;
        var addressInfo = "NA";


        $(document).ready(function ($) {
            
        });
    </script>
     <style>
         #MyBilling {
             background-color: #1e3040 !important;
        }
    </style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">

    <!-- Page Heading -->
    <div class="row">
                <div class="col-lg-12">
                    <h1 class="page-header">
                        Vendor <small>Billing Information</small>
                    </h1>
                    <ol class="breadcrumb">
                        <li class="active">
                            <i class="fa fa-dashboard"></i> Now working on this section
                        </li>
                    </ol>
                </div>
            </div>

</asp:Content>


