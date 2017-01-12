<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Home.aspx.cs" MasterPageFile="~/MasterPages/Site1.master" Inherits="WebCrawler.Views.Home" %>




<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <script src="../Scripts/jquery-1.8.2.min.js"></script>

    <script>


        var totalIphone = 10;
        var totalAndroid = 20;
        $(document).ready(function () {
            //TestPostMethod();s
            LoadStats();
            LoadVendorRanking();

        });

        function LoadStats() {

            document.getElementById("iPhoneUsers").innerHTML = "<span class=\"calPlaceholder\">Calculating..</span>";
            document.getElementById("androidUsers").innerHTML = "<span class=\"calPlaceholder\">Calculating..</span>";
            document.getElementById("qlumiUsers").innerHTML = "<span class=\"calPlaceholder\">Calculating..</span>";
            document.getElementById("totalVendors").innerHTML = "<span class=\"calPlaceholder\">Calculating..</span>";
            document.getElementById("iPhonePerVendor").innerHTML = "<span class=\"calPlaceholder\">Calculating..</span>";
            document.getElementById("androidPerVendor").innerHTML = "<span class=\"calPlaceholder\">Calculating..</span>";
            document.getElementById("userPerVendor").innerHTML = "<span class=\"calPlaceholder\">Calculating..</span>";
            document.getElementById("donut-example").innerHTML = "<h2>Loading Donut..</h2>";
            $.ajax({
                url: "/api/User/ApplicationStats",
                type: 'POST',
                data: {
                },
                success: function (info) {
                    debugger;
                    document.getElementById("iPhoneUsers").innerHTML = info[0].TotalIPhoneUsers;
                    document.getElementById("androidUsers").innerHTML = info[0].TotalAndroidUsers;
                    document.getElementById("qlumiUsers").innerHTML = info[0].TotalQlumiUsers;
                    document.getElementById("totalVendors").innerHTML = info[0].TotalQlumiVendors;
                    document.getElementById("iPhonePerVendor").innerHTML = info[0].AverageIPhonePerVendor;
                    document.getElementById("androidPerVendor").innerHTML = info[0].AverageAndroidPerVendor;
                    document.getElementById("userPerVendor").innerHTML = info[0].AverageTotalPerVendor;

                    document.getElementById("donut-example").innerHTML = "";
                    totalIphone = info[0].TotalIPhoneUsers;
                    totalAndroid = info[0].TotalAndroidUsers;

                    $.getScript('http://cdnjs.cloudflare.com/ajax/libs/raphael/2.1.0/raphael-min.js', function () {
                        $.getScript('http://cdnjs.cloudflare.com/ajax/libs/morris.js/0.5.0/morris.min.js', function () {



                            Morris.Donut({
                                element: 'donut-example',
                                data: [
                                 { label: "Android", value: totalAndroid },
                                 { label: "iPhone", value: totalIphone }
                                ]
                            });


                        });
                    });
                },
                statusCode: {
                    404: function () {
                        alert('Failed');
                    }
                }
            });
        }

        function LoadVendorRanking() {

            document.getElementById("topVendor").innerHTML = "<h3>Loading..</h3>";
        $.ajax({
            url: "/api/User/VendorRanking",
            type: 'POST',
            data: {
            },
            success: function (info) {
                debugger;

                document.getElementById("topVendor").innerHTML = "";
                for (var i = 0; i < info.length; i++)
                {
                    info[0].vendorId
                    document.getElementById("topVendor").innerHTML += "<a href=\"#\" class=\"list-group-item\"><span class=\"badge\">" + info[i].count + "</span><i class=\"fa fa-fw fa-user\"></i>" + info[i].vendorName + "</a>";
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
    <style>
        .calPlaceholder {
            font-size:11px !important;
        }
    </style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">

    
    <div class="container-fluid">

            <!-- Page Heading -->
            <div class="row">
                <div class="col-lg-12">
                    <h1 class="page-header">
                        Qlumi <small>Statistics Overview</small>
                    </h1>
                    <ol class="breadcrumb">
                        <li class="active">
                            <i class="fa fa-dashboard"></i> Dashboard
                        </li>
                    </ol>
                </div>
            </div>
            <!-- /.row -->

            <!-- /.row -->

        
            <div class="row">
                <div class="col-lg-3 col-md-6">
                    <div class="panel panel-primary">
                        <div class="panel-heading">
                            <div class="row">
                                <div class="col-xs-3">
                                    <i class="fa fa-android fa-5x"></i>
                                </div>
                                <div class="col-xs-9 text-right">
                                    <div class="huge" id="androidUsers">0</div>
                                    <div>Android Users</div>
                                </div>
                            </div>
                        </div>
                        <a href="#">
                            <div class="panel-footer">
                                <span class="pull-left">View Details</span>
                                <span class="pull-right"><i class="fa fa-arrow-circle-right"></i></span>
                                <div class="clearfix"></div>
                            </div>
                        </a>
                    </div>
                </div>
                <div class="col-lg-3 col-md-6">
                    <div class="panel panel-primary">
                        <div class="panel-heading">
                            <div class="row">
                                <div class="col-xs-3">
                                    <i class="fa fa-apple fa-5x"></i>
                                </div>
                                <div class="col-xs-9 text-right">
                                    <div class="huge" id="iPhoneUsers">0</div>
                                    <div >IPhone Users</div>
                                </div>
                            </div>
                        </div>
                        <a href="#">
                            <div class="panel-footer">
                                <span class="pull-left">View Details</span>
                                <span class="pull-right"><i class="fa fa-arrow-circle-right"></i></span>
                                <div class="clearfix"></div>
                            </div>
                        </a>
                    </div>
                </div>
                <div class="col-lg-3 col-md-6">
                    <div class="panel panel-primary">
                        <div class="panel-heading">
                            <div class="row">
                                <div class="col-xs-3">
                                    <i class="fa fa-user fa-5x"></i>
                                </div>
                                <div class="col-xs-9 text-right">
                                    <div class="huge" id="qlumiUsers">0</div>
                                    <div>Qlumi Users</div>
                                </div>
                            </div>
                        </div>
                        <a href="#">
                            <div class="panel-footer">
                                <span class="pull-left">View Details</span>
                                <span class="pull-right"><i class="fa fa-arrow-circle-right"></i></span>
                                <div class="clearfix"></div>
                            </div>
                        </a>
                    </div>
                </div>
                <div class="col-lg-3 col-md-6">
                    <div class="panel panel-primary">
                        <div class="panel-heading">
                            <div class="row">
                                <div class="col-xs-3">
                                    <i class="fa fa-male fa-5x"></i>
                                </div>
                                <div class="col-xs-9 text-right">
                                    <div class="huge" id="totalVendors">0</div>
                                    <div>Total Vendors</div>
                                </div>
                            </div>
                        </div>
                        <a href="#">
                            <div class="panel-footer">
                                <span class="pull-left">View Details</span>
                                <span class="pull-right"><i class="fa fa-arrow-circle-right"></i></span>
                                <div class="clearfix"></div>
                            </div>
                        </a>
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-lg-4 col-md-6">
                    <div class="panel panel-primary">
                        <div class="panel-heading">
                            <div class="row">
                                <div class="col-xs-3">
                                    <i class="fa fa-apple fa-5x"></i>
                                </div>
                                <div class="col-xs-9 text-right">
                                    <div class="huge" id="iPhonePerVendor">0</div>
                                    <div>IPhone Users Per Vendor</div>
                                </div>
                            </div>
                        </div>
                        <a href="#">
                            <div class="panel-footer">
                                <span class="pull-left">View Details</span>
                                <span class="pull-right"><i class="fa fa-arrow-circle-right"></i></span>
                                <div class="clearfix"></div>
                            </div>
                        </a>
                    </div>
                </div>
                <div class="col-lg-4 col-md-6">
                    <div class="panel panel-primary">
                        <div class="panel-heading">
                            <div class="row">
                                <div class="col-xs-3">
                                    <i class="fa fa-android fa-5x"></i>
                                </div>
                                <div class="col-xs-9 text-right">
                                    <div class="huge" id="androidPerVendor">0</div>
                                    <div>Android Users Per Vendor</div>
                                </div>
                            </div>
                        </div>
                        <a href="#">
                            <div class="panel-footer">
                                <span class="pull-left">View Details</span>
                                <span class="pull-right"><i class="fa fa-arrow-circle-right"></i></span>
                                <div class="clearfix"></div>
                            </div>
                        </a>
                    </div>
                </div>
                <div class="col-lg-4 col-md-6">
                    <div class="panel panel-primary">
                        <div class="panel-heading">
                            <div class="row">
                                <div class="col-xs-3">
                                    <i class="fa fa-male fa-5x"></i>
                                </div>
                                <div class="col-xs-9 text-right">
                                    <div class="huge" id="userPerVendor">0</div>
                                    <div>Total Users Per Vendor</div>
                                </div>
                            </div>
                        </div>
                        <a href="#">
                            <div class="panel-footer">
                                <span class="pull-left">View Details</span>
                                <span class="pull-right"><i class="fa fa-arrow-circle-right"></i></span>
                                <div class="clearfix"></div>
                            </div>
                        </a>
                    </div>
                </div>
            </div>



            
            <!-- /.row -->

            <div class="row">
                <div class="col-lg-12">
                    <div class="panel panel-default">
                        <div class="panel-heading">
                            <h3 class="panel-title"><i class="fa fa-bar-chart-o fa-fw"></i> Usability Chart</h3>
                        </div>
                        <div class="panel-body">
                            <div id="morris-area-chart"></div>
                        </div>
                    </div>
                </div>
            </div>
            <!-- /.row -->

            <div class="row">

                <div class="col-lg-6">
                    <div class="panel panel-default">
                        <div class="panel-heading">
                            <h3 class="panel-title"><i class="fa fa-long-arrow-right fa-fw"></i> Donut Chart</h3>
                        </div>
                        <div class="panel-body">
                            <div id="donut-example"></div>
                            <div class="text-right">
                            </div>
                        </div>
                    </div>
                </div>

                <div class="col-lg-6">
                    <div class="panel panel-default">
                        <div class="panel-heading">
                            <h3 class="panel-title"><i class="fa fa-clock-o fa-fw"></i>Top Vendors</h3>
                        </div>
                        <div class="panel-body">
                            <div class="list-group" id="topVendor">
                                
                            </div>
                            <div class="text-right">
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <!-- /.row -->

        </div>
</asp:Content>


