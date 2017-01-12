<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="VendorUsers.aspx.cs" MasterPageFile="~/MasterPages/VendorDashboard.master" Inherits="WebCrawler.Views.VendorUsers" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <script src="../Scripts/jquery-1.8.2.min.js"></script>
    <script>



        if (sessionStorage.getItem("Id") == "" || sessionStorage.getItem("Id") == null) {
            window.location = "AdLogin.aspx";
        }
        $(document).ready(function () {
            //TestPostMethod();
            LoadUsers();

        });

        function SearchUser() {

            document.getElementById('loadingText').innerHTML = "Searching Please wait...";
            //var userId = document.getElementById('userIdInfo').value;
            $.ajax({
                url: "/api/User/SearchVendorUserInfo",
                type: 'POST',
                data: {
                    Id: 0,
                    name: document.getElementById('searchByName').value,
                    unlock_code: sessionStorage.getItem("unlock_code"),
                },
                success: function (catInfo) {

                    debugger;
                    document.getElementById('loadingText').innerHTML = "";
                    document.getElementById('vendorTable').innerHTML = "";
                    if (catInfo.length > 0) {
                        for (var i = 0; i < catInfo.length; i++) {

                            document.getElementById('vendorTable').innerHTML += "<tr><td>" + catInfo[i].name + "</td><td>" + catInfo[i].qlumi_userId + "</td><td>" + catInfo[i].unlock_code + "</td><td>" + catInfo[i].requestOrigin + "</td></tr>";

                        }
                    }
                    else {
                        document.getElementById('vendorTable').innerHTML = "<tr>No Result</tr>";
                    }


                },
                statusCode: {
                    404: function () {
                        alert('Failed');
                    }
                }
            });
        }
        function LoadUsers() {

            document.getElementById('loadingText').innerHTML = "Loading Please wait...";
            document.getElementById('vendorTable').innerHTML = "";
            $.ajax({
                url: "/api/User/VendorUsers",
                type: 'POST',
                data: {
                    userId:sessionStorage.getItem("Id"),
                    unlock_code: sessionStorage.getItem("unlock_code")
                },
                success: function (catInfo) {

                    document.getElementById('vendorTable').innerHTML = "";
                    document.getElementById('loadingText').innerHTML = "";
                    for (var i = 0; i < catInfo.contactList.length; i++) {

                        //debugger;
                        document.getElementById('vendorTable').innerHTML += "<tr><td>" + catInfo.contactList[i].name + "</td><td>" + catInfo.contactList[i].qlumi_userId + "</td><td>" + catInfo.contactList[i].unlock_code + "</td><td>" + catInfo.contactList[i].requestOrigin + "</td></tr>";

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
    <style>
        .table {
            table-layout:fixed;
        }

        .table tr td {
            word-wrap: break-word;
        }
        #loadingTextPop {
            width:100%;
            text-align:center;
            padding:5px 0px;
            color:black;
            font-size:12px;
        }
         #VendorUsers {
             background-color: #1e3040 !important;
        }
    </style>
    <div class="container-fluid">

            <!-- Page Heading -->
            <div class="row">
                <div class="col-lg-12">
                    <h1 class="page-header">
                        Vendor <small>User Inforamtion</small>
                    </h1>
                    
                </div>
            </div>
            <div class="row">
			    <div class="col-xs-3 col-sm-3 col-md-3">
			    	<div class="form-group">
			            <input type="text" name="Contact" id="searchByName" class="form-control input-sm" placeholder="Enter Name"/>
			    	</div>
			    </div>
                
                <div class="col-xs-3 col-sm-3 col-md-3">
			    	<div class="form-group">
			    		<div class="btn btn- btn-default" id="searchBtn" onclick="SearchUser()"> Search</div>
			    	</div>
			    </div>
			</div>
            <input type="hidden" id="userIdInfo" />
            <div class="row">
                    <div class="col-lg-12">
                        

                        <div class="table-responsive">
                            <table class="table table-bordered table-hover table-striped table-responsive">
                                <thead>
                                    <tr>
                                        <th>Name</th>
                                        <th>qlumi ID</th>
                                        <th>Unlock Code</th>
                                        <th>OS</th>
                                    </tr>
                                </thead>
                                <h2 id="loadingText"></h2>
                                <tbody id="vendorTable">
                                </tbody>
                            </table>
                        </div>
                    
                </div>

        </div>
</asp:Content>