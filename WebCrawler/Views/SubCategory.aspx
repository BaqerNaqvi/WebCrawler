<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SubCategory.aspx.cs" MasterPageFile="~/MasterPages/Site1.master" Inherits="WebCrawler.Views.SubCategory" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <script src="../Scripts/jquery-1.8.2.min.js"></script>
    <script>



        $(document).ready(function () {
            //TestPostMethod();
            LoadVendors();
            LoadVendorProfiles();
            $(document).keypress(function (e) {
                if (e.which == 13) {

                    var word = document.activeElement.id;

                }
            });

        });

        function AddVendors() {

            document.getElementById('vendorTable').innerHTML = "Adding. Please wait...";
            $.ajax({
                url: "/api/User/AddVendorProfile",
                type: 'POST',
                data: {
                    vendorId: $("#vendorMenu").val(),
                    profileName: document.getElementById('profileName').value,
                    color: $("#colorMenu").val(),
                    colorId: '1',
                    isActive: $("#statusMenu").val()

                },
                success: function (vendorInfo) {

                    debugger;
                    //var fetch = JSON.parse(vendorInfo);
                    document.getElementById('vendorTable').innerHTML = "";
                    debugger;
                    for (var i = 0; i < vendorInfo.length; i++) {

                        document.getElementById('vendorTable').innerHTML += "<tr><td>" + vendorInfo[i].name + "</td><td>" + vendorInfo[i].email + "</td><td>" + vendorInfo[i].contactNumber + "</td><td><div><span class=\"stateTitle\">Code:&nbsp</span><span class=\"stateFigur\">" + vendorInfo[i].unlock_code + "</span></div><br/><div><span class=\"stateTitle\">Total Limit:&nbsp</span ><span class=\"stateFigur\">" + vendorInfo[i].tokenLimit + "</span></div><br/><div><span class=\"stateTitle\">Current Count:&nbsp</span><span class=\"stateFigur\">" + vendorInfo[i].currentTokenCount + "</span></div></td><td>" + vendorInfo[i].description + "</td><td><img class=\"img-thumbnail\" src=\"../UploadedFiles/" + vendorInfo[i].logoImage + "\" width=\"40\" alt=\"\"></td><td>" + vendorInfo[i].website + "</td><td><div class=\"btn btn-danger btn-primary\" id=\"" + vendorInfo[i].Id + "\" onclick=\"RemoveVendor(id)\">Remove</div></td></tr>";

                    }
                    LoadVendorProfiles();
                },
                statusCode: {
                    404: function () {
                        alert('Failed');
                    }
                }
            });
        }

        function LoadVendorProfiles() {

            document.getElementById('vendorTable').innerHTML = "Loading Profiles..";
            $.ajax({
                url: "/api/User/AllProfiles",
                type: 'POST',
                data: {
                },
                success: function (vendorInfo) {

                    debugger;
                    //var vendorInfo = JSON.parse(data);

                    document.getElementById('vendorTable').innerHTML = "";
                    for (var i = 0; i < vendorInfo.length; i++) {

                        document.getElementById('vendorTable').innerHTML += "<tr><td>" + vendorInfo[i].name + "</td><td>" + vendorInfo[i].profileName + "</td><td>" + vendorInfo[i].color + "</td><td>" + vendorInfo[i].isActive + "</td><td><div class=\"btn btn-danger btn-primary\" id=\"" + vendorInfo[i].Id + "\" onclick=\"RemoveVendor(id)\">Remove</div></td></tr>";

                    }
                },
                statusCode: {
                    404: function () {
                        alert('Failed');
                    }
                }
            });
        }
        function LoadVendors() {

            $.ajax({
                url: "/api/User/allVendors",
                type: 'POST',
                data: {
                },
                success: function (vendorInfo) {

                    document.getElementById('vendorMenu').innerHTML = "";
                    for (var i = 0; i < vendorInfo.length; i++) {

                        document.getElementById('vendorMenu').innerHTML += "<option value=\"" + vendorInfo[i].Id + "\">" + vendorInfo[i].name + "</option>";

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

        .vendorDropdown {
            width: 100%;
            height: 31px;
            border-color: #ccc;
            border-radius: 4px;
        }
    </style>
    <div class="container-fluid">

            <!-- Page Heading -->
            <div class="row">
                <div class="col-lg-12">
                    <h1 class="page-header">
                        Qlumi <small>Vendor Profile</small>
                    </h1>
                    
                    <ol class="breadcrumb" style="text-align:right;">
                        <li class="active">
                            <button type="button" class="btn btn-sm btn-primary" data-toggle="modal" data-target="#myModal">Add Profile</button>
                        </li>
                    </ol>
                </div>
            </div>

        
            <div class="row">
                    <div class="col-lg-12">
                        <div class="table-responsive">
                            <table class="table table-bordered table-hover table-striped table-responsive">
                                <thead>
                                    <tr>
                                        <th>Vendor</th>
                                        <th>Profile Name</th>
                                        <th>Color</th>
                                        <th>Status</th>
                                        <th>Action</th>
                                    </tr>
                                </thead>
                                <tbody id="vendorTable">
                                    
                                </tbody>
                            </table>
                        </div>
                    </div>
                </div>

            <div id="myModal" class="modal fade" role="dialog">
                  <div class="modal-dialog">

                    <!-- Modal content-->
                    <div class="modal-content">
                      <div class="modal-header">
                        <button type="button" class="close" data-dismiss="modal">&times;</button>
                        <h4 class="modal-title">Add Profile Information</h4>
                      </div>
                      <div class="modal-body">
                           <div class="row">
			    				        <div class="col-xs-6 col-sm-6 col-md-6">
			    					        <div class="form-group">
			                                    <select id="vendorMenu" class="vendorDropdown">
                                                </select>
			    					        </div>
			    				        </div>
			    				        <div class="col-xs-6 col-sm-6 col-md-6">
			    					        <div class="form-group">
			    						        <input type="text" name="type" id="profileName" class="form-control input-sm" placeholder="Profile Name" required>
			    					        </div>
			    				        </div>
			    			        </div>
                                    <div class="row">
			    				        <div class="col-xs-6 col-sm-6 col-md-6">
			    					        <div class="form-group">
			                                    <select id="statusMenu" class="vendorDropdown">
                                                    <option value="1">Enabled</option>
                                                    <option value="0">Disabled</option>
                                                </select>
			    					        </div>
			    				        </div>
			    				        <div class="col-xs-6 col-sm-6 col-md-6">
			    					        <div class="form-group">
			    						        <select id="colorMenu" class="vendorDropdown">
                                                    <option value="9b26af">Purple</option>
                                                    <option value="3e3f94">Blue</option>
                                                    <option value="4c862d">Green</option>
                                                    <option value="4c862d">Red</option>
                                                </select>
			    					        </div>
			    				        </div>
			    			        </div>

			    			
			    			        <div id="fetch1" onclick="AddVendors()" class="btn btn-info btn-block">Add Profile</div>
                      </div>
                      <div class="modal-footer">
                        <button type="button" class="btn btn-default" data-dismiss="modal">Close</button>
                      </div>
                    </div>

                  </div>
                </div>
        </div>
</asp:Content>