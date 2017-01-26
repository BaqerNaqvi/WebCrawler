<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AdDashboard.aspx.cs" MasterPageFile="~/MasterPages/AdDashboard.master" Inherits="WebCrawler.Views.AdDashboard" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <script src="../Scripts/jquery-1.8.3.js"></script>
     
    <script type="text/javascript">

        if (sessionStorage.getItem("Id") == "" || sessionStorage.getItem("Id") == null) {
            window.location = "AdLogin.aspx";
        }
        var coordInfo;
        var notesInfo;
        var mapInfo;
        var addressInfo = "NA";


        $(document).ready(function ($) {
            LoadVendorAds();
        });


        function LoadVendorAds() {
            //alert(sessionStorage.getItem("Id"));

            document.getElementById('adsWrapper').innerHTML = "<div class=\"box-header with-border\"><h3 class=\"box-title\">Loading Ads. Please wait....</h3></div>";
            $.ajax({
                url: "/api/VendorDashboard/GetAdByVendorId",
                type: 'POST',
                data: {
                    vendorId: sessionStorage.getItem("Id")
                },
                success: function (adObj) {

                    document.getElementById('adsWrapper').innerHTML = "";
                    debugger;
                    var visible = "Yes";
                    var fileType = "File";
                    var fileUrl = "";
                    var innerhtml = "";
                    for (var i = 0; i < adObj.length; i++) {
                        var raw = "";

                        if (adObj[i].isVisible == 0)
                        {
                            visible = "No";
                        }
                      //  adObj[i].isVisible = 2;
                        if (adObj[i].AdType.title == "Sponsor")
                        {
                            fileUrl = "<img src=\"../UploadedFiles/AdImages/default.png\" width=\"50\"";
                            raw = "<div class=\"box box1 box-primary\"><div class=\"box-header with-border\"><h3 class=\"box-title\">" + adObj[i].AdType.title + "</h3></div><!--here--><div class='controll-group'><div class=\"col-xs-12 col-sm-12 col-md-2\"><div class=\"adTableHeading\">  Ad Title: </div></div><div class=\"col-xs-12 col-sm-12 col-md-10\"> <span>" + adObj[i].adTitle + "</span></div></div><div class='controll-group'><div class='controll-group'><div class=\"col-xs-12 col-sm-12 col-md-2\"><div class=\"adTableHeading\">  Ad Location: </div></div><div class=\"col-xs-12 col-sm-12 col-md-10\"> <span>" + adObj[i].locationName + "</span></div></div><div class='controll-group'><div class=\"col-xs-12 col-sm-12 col-md-2\"><div class=\"adTableHeading\">  Cost: </div></div><div class=\"col-xs-12 col-sm-12 col-md-10\"> <span>" + (adObj[i].costPerAction || adObj[i].costPerConversion) + "</span></div></div><!--there--><div class=\"controll-group\"><div class=\"col-xs-12 col-sm-12 col-md-2\"> <div class=\"adTableHeading\">  Bid Type: </div></div><div class=\"col-xs-12 col-sm-12 col-md-10\"> <span>" + adObj[i].BidType.title + "</span></div></div><div class=\"controll-group\"><div class=\"col-xs-12 col-sm-12 col-md-2\"> <div class=\"adTableHeading\">  Daily Budget: </div></div><div class=\"col-xs-12 col-sm-12 col-md-10\"> <span>" + adObj[i].dailyBudget + "</span></div>  </div><div class=\"controll-group\"><div class=\"col-xs-12 col-sm-12 col-md-2\"><div class=\"adTableHeading\"> Remaining Budget:</div></div><div class=\"col-xs-12 col-sm-12 col-md-10\"><span> $38</span></div>  </div><div class=\"controll-group\"><div class=\"col-xs-12 col-sm-12 col-md-2\"><div class=\"adTableHeading\"> Interest:</div></div><div class=\"col-xs-12 col-sm-12 col-md-10\"><span>" + adObj[i].customInterest + "</span></div>  </div><div class=\"controll-group\"><div class=\"col-xs-12 col-sm-12 col-md-2\"><div class=\"adTableHeading\"> Visible:</div></div><div class=\"col-xs-12 col-sm-12 col-md-10\"><span>" + visible + "</span></div>  </div><div class=\"controll-group\"><div class=\"col-xs-12 col-sm-12 col-md-2\"><div class=\"adTableHeading\"> Facts:</div></div><div class=\"col-xs-12 col-sm-12 col-md-10\"><span>" + adObj[i].sponsorFacts + "</span></div>  </div><div class=\"controll-group\"><div class=\"col-xs-12 col-sm-12 col-md-2\"><div class=\"adTableHeading\">" + fileType + "</div></div><div class=\"col-xs-12 col-sm-12 col-md-10\"><span>" + fileUrl + "</span></div>  </div>";
                        }
                        else if (adObj[i].AdType.title == "Coupon")
                        {
                            raw = "<div class=\"box box1 box-primary\"><div class=\"box-header with-border\"><h3 class=\"box-title\">" + adObj[i].AdType.title + "</h3></div><!--here--><div class='controll-group'><div class=\"col-xs-12 col-sm-12 col-md-2\"><div class=\"adTableHeading\">  Ad Title: </div></div><div class=\"col-xs-12 col-sm-12 col-md-10\"> <span>" + adObj[i].adTitle + "</span></div></div><div class='controll-group'><div class='controll-group'><div class=\"col-xs-12 col-sm-12 col-md-2\"><div class=\"adTableHeading\">  Ad Location: </div></div><div class=\"col-xs-12 col-sm-12 col-md-10\"> <span>" + adObj[i].locationName + "</span></div></div><div class='controll-group'><div class=\"col-xs-12 col-sm-12 col-md-2\"><div class=\"adTableHeading\">  Cost: </div></div><div class=\"col-xs-12 col-sm-12 col-md-10\"> <span>" + (adObj[i].costPerAction || adObj[i].costPerConversion) + "</span></div></div><!--there--><div class=\"controll-group\"><div class=\"col-xs-12 col-sm-12 col-md-2\"> <div class=\"adTableHeading\">  Bid Type: </div></div><div class=\"col-xs-12 col-sm-12 col-md-10\"> <span>" + adObj[i].BidType.title + "</span></div></div><div class=\"controll-group\"><div class=\"col-xs-12 col-sm-12 col-md-2\"> <div class=\"adTableHeading\">  Daily Budget: </div></div><div class=\"col-xs-12 col-sm-12 col-md-10\"> <span>" + adObj[i].dailyBudget + "</span></div>  </div><div class=\"controll-group\"><div class=\"col-xs-12 col-sm-12 col-md-2\"><div class=\"adTableHeading\"> Remaining Budget:</div></div><div class=\"col-xs-12 col-sm-12 col-md-10\"><span> $38</span></div>  </div><div class=\"controll-group\"><div class=\"col-xs-12 col-sm-12 col-md-2\"><div class=\"adTableHeading\"> Interest:</div></div><div class=\"col-xs-12 col-sm-12 col-md-10\"><span>" + adObj[i].customInterest + "</span></div>  </div><div class=\"controll-group\"><div class=\"col-xs-12 col-sm-12 col-md-2\"><div class=\"adTableHeading\"> Visible:</div></div><div class=\"col-xs-12 col-sm-12 col-md-10\"><span>" + visible + "</span></div>  </div><div class=\"controll-group\"><div class=\"col-xs-12 col-sm-12 col-md-2\"><div class=\"adTableHeading\"> Landing Page:</div></div><div class=\"col-xs-12 col-sm-12 col-md-10\"><span>" + adObj[i].sponsorWebsite + "</span></div>  </div><div class=\"controll-group\"><div class=\"col-xs-12 col-sm-12 col-md-2\"><div class=\"adTableHeading\">Phone No.</div></div><div class=\"col-xs-12 col-sm-12 col-md-10\"><span>" + adObj[i].sponsorPhone + "</span></div>  </div>";
                        }
                        else
                        {
                            raw = "<div class=\"box box1 box-primary\"><div class=\"box-header with-border\"><h3 class=\"box-title\">" + adObj[i].AdType.title + "</h3></div><!--here--><div class='controll-group'><div class=\"col-xs-12 col-sm-12 col-md-2\"><div class=\"adTableHeading\">  Ad Title: </div></div><div class=\"col-xs-12 col-sm-12 col-md-10\"> <span>" + adObj[i].adTitle + "</span></div></div><div class='controll-group'><div class=\"col-xs-12 col-sm-12 col-md-2\"><div class=\"adTableHeading\">  Ad Location: </div></div><div class=\"col-xs-12 col-sm-12 col-md-10\"> <span>" + adObj[i].locationName + "</span></div></div><div class='controll-group'><div class=\"col-xs-12 col-sm-12 col-md-2\"><div class=\"adTableHeading\">  Cost: </div></div><div class=\"col-xs-12 col-sm-12 col-md-10\"> <span>" + (adObj[i].costPerAction || adObj[i].costPerConversion) + "</span></div></div><!--there--><div class=\"controll-group\"><div class=\"col-xs-12 col-sm-12 col-md-2\"> <div class=\"adTableHeading\">  Bid Type: </div></div><div class=\"col-xs-12 col-sm-12 col-md-10\"> <span>" + adObj[i].BidType.title + "</span></div></div><div class=\"controll-group\"><div class=\"col-xs-12 col-sm-12 col-md-2\"> <div class=\"adTableHeading\">  Daily Budget: </div></div><div class=\"col-xs-12 col-sm-12 col-md-10\"> <span>" + adObj[i].dailyBudget + "</span></div>  </div><div class=\"controll-group\"><div class=\"col-xs-12 col-sm-12 col-md-2\"><div class=\"adTableHeading\"> Remaining Budget:</div></div><div class=\"col-xs-12 col-sm-12 col-md-10\"><span> $38</span></div>  </div><div class=\"controll-group\"><div class=\"col-xs-12 col-sm-12 col-md-2\"><div class=\"adTableHeading\"> Interest:</div></div><div class=\"col-xs-12 col-sm-12 col-md-10\"><span>" + adObj[i].customInterest + "</span></div>  </div><div class=\"controll-group\"><div class=\"col-xs-12 col-sm-12 col-md-2\"><div class=\"adTableHeading\"> Visible:</div></div><div class=\"col-xs-12 col-sm-12 col-md-10\"><span>" + visible + "</span></div>  </div>";
                            
                        }
                        if (adObj[i].isVisible !== 3) {
                            raw = raw + "<div class=\"adFooter\"><div class=\"btn btn-block btn-info btn-sm\" onclick=\"PauseAd(" + adObj[i].Id + ")\"> Pause</div><div class=\"btn btn-block btn-danger btn-sm\" onclick=\"RemoveAd(" + adObj[i].Id + ")\"> Delete</div><div class=\"btn btn-block btn-primary btn-sm\" onclick=\"StartAd(" + adObj[i].Id + ")\"> Run</div><div class=\"btn btn-block btn-primary btn-sm\" > View Progress</div>";
                            if (adObj[i].isVisible === 2) {  // 4 buttons 
                                raw = raw + "<div class=\"btn btn-block btn-info btn-sm\" onclick=\"PayNow()\"> Pay Now</div>";
                            }
                            raw = raw + "</div> </div>";
                        } else if (adObj[i].isVisible === 3) { // one button 
                            raw = raw + "<div class=\"adFooter\"><div class=\"btn btn-block btn-info btn-sm\" onclick=\"PayNow()\"> Pay Now</div></div>  </div>";
                        }

                        innerhtml = innerhtml + raw;
                    }//
                    document.getElementById('adsWrapper').innerHTML = innerhtml;


                },
                statusCode: {
                    404: function () {
                        alert('Failed');
                    }
                }
            });
        }


        function PauseAd(adId) {
            //alert(sessionStorage.getItem("Id"));

            document.getElementById('adsWrapper').innerHTML = "<div class=\"box-header with-border\"><h3 class=\"box-title\">Loading Ads. Please wait....</h3></div>";
            $.ajax({
                url: "/api/VendorDashboard/UpdateAdVisibility",
                type: 'POST',
                data: {
                    Id:adId,
                    vendorId: sessionStorage.getItem("Id"),
                    isVisible:0
                },
                success: function (adObj) {

                    document.getElementById('adsWrapper').innerHTML = "";
                    
                    location.reload();

                },
                statusCode: {
                    404: function () {
                        alert('Failed');
                    }
                }
            });
        }


        function StartAd(adId) {
            //alert(sessionStorage.getItem("Id"));

            document.getElementById('adsWrapper').innerHTML = "<div class=\"box-header with-border\"><h3 class=\"box-title\">Loading Ads. Please wait....</h3></div>";
            $.ajax({
                url: "/api/VendorDashboard/UpdateAdVisibility",
                type: 'POST',
                data: {
                    Id: adId,
                    vendorId: sessionStorage.getItem("Id"),
                    isVisible: 1
                },
                success: function (adObj) {

                    document.getElementById('adsWrapper').innerHTML = "";

                    location.reload();

                },
                statusCode: {
                    404: function () {
                        alert('Failed');
                    }
                }
            });
        }

        function RemoveAd(adId) {
            //alert(sessionStorage.getItem("Id"));

            var result = confirm("Want to delete this Ad?");
            if (result) {
                document.getElementById('adsWrapper').innerHTML = "<div class=\"box-header with-border\"><h3 class=\"box-title\">Loading Ads. Please wait....</h3></div>";
                $.ajax({
                    url: "/api/VendorDashboard/RemoveAd",
                    type: 'POST',
                    data: {
                        Id: adId,
                        vendorId: sessionStorage.getItem("Id")
                    },
                    success: function (adObj) {

                        document.getElementById('adsWrapper').innerHTML = "";

                        location.reload();

                    },
                    statusCode: {
                        404: function () {
                            alert('Failed');
                        }
                    }
                });
            }
            
        }

        function PayNow(user) {
            alert("");
            $.ajax({
                url: "/api/VendorDashboard/PayNow",
                type: 'POST',
                data: {
                    vendorId: sessionStorage.getItem("Id")
                },
                success: function (adObj) {
                    alert("s");


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

        #VendorDashboard {
             background-color: #1e3040 !important;
        }
        .btn-block {
            float: right !important
        }
    </style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">

    <!-- Page Heading -->
    <div class="row">
                <div class="col-lg-12">
                    <h1 class="page-header">
                        Advertiser <small>Statistics Overview</small>
                    </h1>
                    <ol class="breadcrumb" style="text-align:right;">
                        <li class="active">
                            <a href="CreateAd.aspx" class="btn btn-sm btn-primary" onclick="CreateAd()">Create Ad</a>
                        </li>
                    </ol>
                </div>
            </div>



    
    <div class="row">
                <div class="col-lg-4 col-md-6">
                    <div class="panel panel-primary">
                        <div class="panel-heading">
                            <div class="row">
                                <div class="col-xs-3">
                                    <i class="fa fa-usd fa-5x"></i>
                                </div>
                                <div class="col-xs-9 text-right">
                                    <div class="huge" id="unlockCode">0</div>
                                    <div>Daily Budgdet</div>
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
                                    <i class="fa fa-usd fa-5x"></i>
                                </div>
                                <div class="col-xs-9 text-right">
                                    <div class="huge" id="totalNotes">10</div>
                                    <div>Remaining Budget</div>
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
                                    <i class="fa fa-tags fa-5x"></i>
                                </div>
                                <div class="col-xs-9 text-right">
                                    <div class="huge" id="totalUsers">90</div>
                                    <div>Active Ads</div>
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
        <div class="col-lg-12" id="adsWrapper">
            
            
                
        </div>
    </div>

</asp:Content>

