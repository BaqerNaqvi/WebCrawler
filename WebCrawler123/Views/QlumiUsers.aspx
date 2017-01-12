<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="QlumiUsers.aspx.cs" MasterPageFile="~/MasterPages/Site1.master" Inherits="WebCrawler.Views.QlumiUsers" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <script src="../Scripts/jquery-1.8.2.min.js"></script>
    <script>



        $(document).ready(function () {
            //TestPostMethod();
            LoadCategories();
            document.getElementById('email').innerHTML = "hello";
           
        });


        function ShowOverlay(Id)
        {
            $("#myModal").modal('show');
            LoadUserInPopup(Id);
        }

        function LoadUserInPopup(Id) {

            document.getElementById('loadingTextPop').innerHTML = "Loading Please wait...";


            document.getElementById('name').innerHTML = "";
            document.getElementById('email').innerHTML = "";
            document.getElementById('contactNumber').innerHTML = "";
            $.ajax({
                url: "/api/User/UserDetail",
                type: 'POST',
                data: {
                    userId:Id
                },
                success: function (catInfo) {

                    document.getElementById('loadingTextPop').innerHTML = "";
                    
                    document.getElementById('userIdInfo').value = catInfo.contactList[0].userId;
                    //debugger;
                    document.getElementById('name').value = catInfo.contactList[0].name;
                    document.getElementById('email').value = catInfo.contactList[0].email;
                    document.getElementById('contactNumber').value = catInfo.contactList[0].contactNumber;
                    document.getElementById('uPass').value = catInfo.contactList[0].password;
                    
                },
                statusCode: {
                    404: function () {
                        alert('Failed');
                    }
                }
            });
        }

        function SearchUser() {

            document.getElementById('loadingText').innerHTML = "Searching Please wait...";
            //var userId = document.getElementById('userIdInfo').value;
            $.ajax({
                url: "/api/User/SearchUserInfo",
                type: 'POST',
                data: {
                    Id: 0,
                    name: document.getElementById('searchByName').value,
                    email: document.getElementById('searchByEmail').value,
                    unlock_code: document.getElementById('searchByUnlockCode').value,
                    contactNumber: document.getElementById('searchByPhone').value
                },
                success: function (catInfo) {

                    debugger;
                    document.getElementById('loadingText').innerHTML = "";
                    document.getElementById('vendorTable').innerHTML = "";
                    if (catInfo.length > 0) {
                        for (var i = 0; i < catInfo.length; i++) {

                            document.getElementById('vendorTable').innerHTML += "<tr><td>" + catInfo[i].name + "</td><td>" + catInfo[i].email + "</td><td>" + catInfo[i].password + "</td><td>" + catInfo[i].contactNumber + "</td><td>" + catInfo[i].unlock_code + "</td><td>" + catInfo[i].requestOrigin + "</td><td><div class=\"btn btn-danger btn-primary myButton\" id=\"" + catInfo[i].Id + "\" onclick=\"ShowOverlay(id)\">Update</div></td></tr>";

                        }
                    }
                    else
                    {
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
        function UpdateUserInfo() {

            document.getElementById('loadingTextPop').innerHTML = "Updating Please wait...";
            var userId = document.getElementById('userIdInfo').value;
            $.ajax({
                url: "/api/User/UpdateUserInfo",
                type: 'POST',
                data: {
                    Id: userId,
                    name: document.getElementById('name').value,
                    email: document.getElementById('email').value,
                    contactNumber: document.getElementById('contactNumber').value,
                    password: document.getElementById('uPass').value
                },
                success: function (catInfo) {

                    document.getElementById('loadingTextPop').innerHTML = "";
                    LoadCategories();
                    $("#myModal").modal('hide');
                },
                statusCode: {
                    404: function () {
                        alert('Failed');
                    }
                }
            });
        }
        function LoadCategories() {
            
            document.getElementById('loadingText').innerHTML = "Loading Please wait...";
            document.getElementById('vendorTable').innerHTML = "";
            $.ajax({
                url: "/api/User/AllUsers",
                type: 'POST',
                data: {

                },
                success: function (catInfo) {

                    document.getElementById('vendorTable').innerHTML = "";
                    document.getElementById('loadingText').innerHTML = "";
                    for (var i = 0; i < catInfo.contactList.length; i++) {

                        //debugger;
                        document.getElementById('vendorTable').innerHTML += "<tr><td>" + catInfo.contactList[i].name + "</td><td>" + catInfo.contactList[i].email + "</td><td>" + catInfo.contactList[i].password + "</td><td>" + catInfo.contactList[i].contactNumber + "</td><td>" + catInfo.contactList[i].unlock_code + "</td><td>" + catInfo.contactList[i].requestOrigin + "</td><td><div class=\"btn btn-danger btn-primary myButton\" id=\"" + catInfo.contactList[i].Id + "\" onclick=\"ShowOverlay(id)\">Update</div></td></tr>";

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
    </style>
    <div class="container-fluid">

            <!-- Page Heading -->
            <div class="row">
                <div class="col-lg-12">
                    <h1 class="page-header">
                        Qlumi <small>User Inforamtion</small>
                    </h1>
                    
                    <ol class="breadcrumb" style="text-align:right;">
                        <li class="active">
                            <button type="button" class="btn btn-sm btn-primary" data-toggle="modal" data-target="#myModal">Add User</button>
                        </li>
                    </ol>
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
			            <input type="text" name="Contact" id="searchByEmail" class="form-control input-sm" placeholder="Enter Email"/>
			    	</div>
			    </div>
                <div class="col-xs-3 col-sm-3 col-md-3">
			    	<div class="form-group">
			            <input type="text" name="Contact" id="searchByUnlockCode" class="form-control input-sm" placeholder="Enter Code"/>
			    	</div>
			    </div>
                <div class="col-xs-3 col-sm-3 col-md-3">
			    	<div class="form-group">
			            <input type="text" name="Contact" id="searchByPhone" class="form-control input-sm" placeholder="Enter Phone"/>
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
                                        <th>Email</th>
                                        <th>Password</th>
                                        <th>Contact</th>
                                        <th>Unlock Code</th>
                                        <th>OS</th>
                                        <th>Action</th>
                                    </tr>
                                </thead>
                                <h2 id="loadingText"></h2>
                                <tbody id="vendorTable">
                                </tbody>
                            </table>
                        </div>
                    
                </div>

            <div id="myModal" class="modal fade" role="dialog">
                  <div class="modal-dialog">

                    <!-- Modal content-->
                    <div class="modal-content">
                      <div class="modal-header">
                        <button type="button" class="close" data-dismiss="modal">&times;</button>
                        <h4 class="modal-title">Update User Information</h4>
                      </div>
                      <div class="modal-body">
                            <form role="form">
                                    <div class="row" id="loadingTextPop">

                                    </div>
                                    
			    			        <div class="row">
			    				        <div class="col-xs-6 col-sm-6 col-md-6">
			    					        <div class="form-group">
			                                    <input type="text" name="Name" id="name" class="form-control input-sm" placeholder="Name" required />
			    					        </div>
			    				        </div>
			    				        <div class="col-xs-6 col-sm-6 col-md-6">
			    					        <div class="form-group">
			    						        <input type="email" name="email" id="email" class="form-control input-sm" placeholder="Email" required />
			    					        </div>
			    				        </div>
			    			        </div>
			    			        <div class="row">
			    				        <div class="col-xs-6 col-sm-6 col-md-6">
			    					        <div class="form-group">
			                                    <input type="text" name="Contact" id="contactNumber" class="form-control input-sm" placeholder="Contact" required>
			    					        </div>
			    				        </div>
			    				        <div class="col-xs-6 col-sm-6 col-md-6">
			    					        <div class="form-group">
			                                    <input type="text" name="Password" id="uPass" class="form-control input-sm" placeholder="Password" required>
			    					        </div>
			    				        </div>
			    			        </div>

                                    <div class="row">
			    				        <div class="col-xs-6 col-sm-6 col-md-6">
			    					        <div class="form-group">
			    						        <div class="btn btn-info btn-block" onclick="UpdateUserInfo()"> Update </div>
			    					        </div>
			    				        </div>
			    			        </div>
			    			
			    			        
			    		
			    		        </form>
                      </div>
                      <div class="modal-footer">
                        <button type="button" class="btn btn-default" data-dismiss="modal">Close</button>
                      </div>
                    </div>

                  </div>
                </div>
        </div>
</asp:Content>