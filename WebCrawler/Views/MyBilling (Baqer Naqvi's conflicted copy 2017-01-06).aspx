<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="MyBilling.aspx.cs" MasterPageFile="~/MasterPages/AdDashboard.master" Inherits="WebCrawler.Views.MyBilling" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <head>
        <title>Billing Info</title>
         <style>
         #MyBilling {
             background-color: #1e3040 !important;
        }

    </style>
    </head>
     <script src="../Scripts/jquery-1.8.2.min.js"></script>

    <script type="text/javascript">

        if (sessionStorage.getItem("Id") == "" || sessionStorage.getItem("Id") == null) {
            //window.location = "AdLogin.aspx";
        }
        var coordInfo;
        var notesInfo;
        var mapInfo;
        var addressInfo = "NA";


        $(document).ready(function ($) {
            getPaymentHistory();
        });
        function getPaymentHistory() {
            $.ajax({
                url: "/api/Billing",
                type: 'Get',
                data: {
                    vendorId: sessionStorage.getItem("Id")
                },
                success: function (response) {
                    debugger;
                    $("#MainContent_paymentduespan").text(response.DuePayment);
                    if (response.BillingHistory != null) {
                        var html = "";
                        for (var i = 0; i < response.BillingHistory.length; i++) {
                            html = html + " <tr><th scope='row'>" + response.BillingHistory[i].TranslationId + "</th><td>" + response.BillingHistory[i].InvoiceNo + "</td><td>" + response.BillingHistory[i].TransDateTimeStr +
                                "</td><td>" + response.BillingHistory[i].Amount + "</td></tr>";
                        }
                        $("#payment-history-table").empty();
                        $("#payment-history-table").append(html);
                        var url=document.location.href;
                        if ((url.indexOf("paymentId") !== -1)) {
                            $("#MainContent_Button1").css("display", 'none');
                            $("#MainContent_paymentduespan").text(0);
                            var temp = " <tr><th scope='row'>" + 3696 + "</th><td>457355</td><td>" + "12/28/2016" +
                                "</td><td>" + 95 + "</td></tr>";
                            $("#payment-history-table").prepend(temp);
                        }

                    }
                },
                statusCode: {
                    404: function () {
                        alert('Failed');
                    }
                }
            });
        }
    </script>
    
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
                            <i class="fa fa-dashboard"></i> Your Card has some issues. <a href="#">Click</a> to view.
                        </li>
                    </ol>
                </div>
            </div>
             <div class="row">
                <div class="col-lg-12 col-md-12">
                    <table class="table table-striped">
                              
                              <tbody>
                                <tr>
                                  <th scope="row">Due Payment:</th>
                                  <td>
                                      $
                                      <span id="paymentduespan" runat="server" class="badge"   >0</span>
                                  </td>
                                  <td>
                                      <asp:Button ID="Button1"  style="float: right;" class="btn btn-primary" runat="server" Text="Pay Now" OnClick="Button1_Click" />

                                  </td>
                                </tr>
                              
                              </tbody>
                            </table>
                    <table class="table table-striped">
                              <thead>
                                <tr>
                                  <th>Transaction Id</th>
                                  <th>Invoice #</th>
                                  <th>Date</th>
                                  <th>Payment($)</th>
                                </tr>
                              </thead>
                              <tbody id="payment-history-table">
                                <tr>
                                  <th scope="row"></th>
                                  <td>Loading...</td>
                                  <td></td>
                                </tr>
                            </tbody>
                            </table>
                </div>
            </div>
</asp:Content>


