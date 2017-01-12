<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Interests.aspx.cs" MasterPageFile="~/MasterPages/Site1.master" Inherits="WebCrawler.Views.Interests" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <script src="../Scripts/jquery-1.8.2.min.js"></script>
    <script>



        $(document).ready(function () {
            
            LoadVendors();
            LoadCategories();
            
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

                    document.getElementById('vendorTable').innerHTML = "";
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


        function LoadVendors() {
            document.getElementById('vendorMenu').innerHTML += "<option value=\"1\">Loading...</option>";
            document.getElementById('selectVendor').innerHTML += "<option value=\"1\">Loading...</option>";
            $.ajax({
                url: "/api/User/allVendors",
                type: 'POST',
                data: {
                },
                success: function (vendorInfo) {

                    document.getElementById('vendorMenu').innerHTML = "<option value=\"1\">Select Vendor</option>";
                    document.getElementById('selectVendor').innerHTML = "<option value=\"1\">Select Vendor</option>";
                    for (var i = 0; i < vendorInfo.length; i++) {

                        document.getElementById('vendorMenu').innerHTML += "<option value=\"" + vendorInfo[i].Id + "\">" + vendorInfo[i].name + "</option>";
                        document.getElementById('selectVendor').innerHTML += "<option value=\"" + vendorInfo[i].Id + "\">" + vendorInfo[i].name + "</option>";

                    }
                    LoadSelectionVendorProfiles();

                },
                statusCode: {
                    404: function () {
                        alert('Failed');
                    }
                }
            });
        }


        function LoadCategories() {


            document.getElementById('vendorCategory').innerHTML += "<option value=\"1\">Loading...</option>";
            $.ajax({
                url: "/api/User/AllAppCategory",
                type: 'POST',
                data: {
                },
                success: function (vendorInfo) {

                    document.getElementById('vendorCategory').innerHTML = "<option value=\"1\">Select Category</option>";
                    for (var i = 0; i < vendorInfo.length; i++) {

                        document.getElementById('vendorCategory').innerHTML += "<option value=\"" + vendorInfo[i].Id + "\">" + vendorInfo[i].categoryName + "</option>";

                    }
                    LoadSubCategories();
                    
                },
                statusCode: {
                    404: function () {
                        alert('Failed');
                    }
                }
            });
        }

        function LoadSubCategories() {

            var catId = document.getElementById("vendorCategory").value;
            document.getElementById('vendorSubCategory').innerHTML = "<option value=\"1\">Loading...</option>";
            $.ajax({
                url: "/api/User/AllAppSubCategory",
                type: 'POST',
                data: {
                    Id:catId
                },
                success: function (vendorInfo) {

                    document.getElementById('vendorSubCategory').innerHTML = "<option value=\"1\">Select Interest</option>";
                    //debugger;
                    for (var i = 0; i < vendorInfo.length; i++) {

                        document.getElementById('vendorSubCategory').innerHTML += "<option value=\"" + vendorInfo[i].Id + "\">" + vendorInfo[i].subCategoryName + "</option>";

                    }
                },
                statusCode: {
                    404: function () {
                        alert('Failed');
                    }
                }
            });
        }

        function LoadVendorProfiles() {


            var vendorId = document.getElementById("vendorMenu").value;
            //debugger;
            document.getElementById('vendorProfile').innerHTML = "<option value=\"1\">Loading...</option>";
            $.ajax({
                url: "/api/User/VendorProfile",
                type: 'POST',
                data: {
                    vendorId: vendorId
                },
                success: function (vendorInfo) {

                    document.getElementById('vendorProfile').innerHTML = "<option value=\"1\">Select Profile</option>";
                    //debugger;
                    for (var i = 0; i < vendorInfo.length; i++) {

                        document.getElementById('vendorProfile').innerHTML += "<option value=\"" + vendorInfo[i].Id + "\">" + vendorInfo[i].profileName + "</option>";

                    }
                },
                statusCode: {
                    404: function () {
                        alert('Failed');
                    }
                }
            });
        }
        function LoadSelectionVendorProfiles() {


            var vendorId = document.getElementById("selectVendor").value;
            //debugger;
            document.getElementById('selectProfile').innerHTML = "<option value=\"1\">Loading...</option>";
            $.ajax({
                url: "/api/User/VendorProfile",
                type: 'POST',
                data: {
                    vendorId: vendorId
                },
                success: function (vendorInfo) {

                    document.getElementById('selectProfile').innerHTML = "<option value=\"1\">Select Profile</option>";
                    //debugger;
                    for (var i = 0; i < vendorInfo.length; i++) {

                        document.getElementById('selectProfile').innerHTML += "<option value=\"" + vendorInfo[i].Id + "\">" + vendorInfo[i].profileName + "</option>";

                    }
                    LoadInterest();
                },
                statusCode: {
                    404: function () {
                        alert('Failed');
                    }
                }
            });
        }

        function AddInterest() {

            document.getElementById('vendorTable').innerHTML = "Adding. Please wait...";
            $.ajax({
                url: "/api/User/AddVendorInterest",
                type: 'POST',
                data: {
                    vendorId: $("#vendorMenu").val(),
                    settingId: $("#vendorProfile").val(),
                    color: $("#colorMenu").val(),
                    categoryId: $("#vendorCategory").val(),
                    subCategoryId: $("#vendorSubCategory").val(),
                    isActive: $("#statusMenu").val(),
                    radius:document.getElementById("radius").value

                },
                success: function (vendorInfo) {

                    debugger;
                    //var fetch = JSON.parse(vendorInfo);
                    document.getElementById('vendorTable').innerHTML = "";
                    //debugger;
                    for (var i = 0; i < vendorInfo.length; i++) {

                        //document.getElementById('vendorTable').innerHTML += "<tr><td>" + vendorInfo[i].name + "</td><td>" + vendorInfo[i].email + "</td><td>" + vendorInfo[i].contactNumber + "</td><td><div><span class=\"stateTitle\">Code:&nbsp</span><span class=\"stateFigur\">" + vendorInfo[i].unlock_code + "</span></div><br/><div><span class=\"stateTitle\">Total Limit:&nbsp</span ><span class=\"stateFigur\">" + vendorInfo[i].tokenLimit + "</span></div><br/><div><span class=\"stateTitle\">Current Count:&nbsp</span><span class=\"stateFigur\">" + vendorInfo[i].currentTokenCount + "</span></div></td><td>" + vendorInfo[i].description + "</td><td><img class=\"img-thumbnail\" src=\"../UploadedFiles/" + vendorInfo[i].logoImage + "\" width=\"40\" alt=\"\"></td><td>" + vendorInfo[i].website + "</td><td><div class=\"btn btn-danger btn-primary\" id=\"" + vendorInfo[i].Id + "\" onclick=\"RemoveVendor(id)\">Remove</div></td></tr>";

                    }
                    //LoadVendorProfiles();
                    LoadInterest();
                },
                statusCode: {
                    404: function () {
                        alert('Failed');
                    }
                }
            });
        }

        function LoadInterest() {

            document.getElementById('vendorTable').innerHTML = "Loading. Please wait...";
            $.ajax({
                url: "/api/User/AllVendorInterest",
                type: 'POST',
                data: {
                    Id: $("#selectProfile").val()

                },
                success: function (vendorInfo) {

                    debugger;
                    //var fetch = JSON.parse(vendorInfo);
                    document.getElementById('vendorTable').innerHTML = "";
                    //debugger;
                    for (var i = 0; i < vendorInfo.length; i++) {

                        document.getElementById('vendorTable').innerHTML += "<tr><td>" + vendorInfo[i].categoryName + "</td><td>" + vendorInfo[i].subCategoryName + "</td><td>" + vendorInfo[i].radius + "</td><td>" + vendorInfo[i].color + "</td><td>" + vendorInfo[i].isActive + "</td><td><div class=\"btn btn-danger btn-primary\" id=\"" + vendorInfo[i].Id + "\" onclick=\"RemoveVendor(id)\">Remove</div></td></tr>";

                    }
                    //LoadVendorProfiles();
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
                        Qlumi <small>Vendor Interests</small>
                    </h1>
                    
                    <ol class="breadcrumb" style="text-align:right;">
                        <li class="active">
                            <button type="button" class="btn btn-sm btn-primary" data-toggle="modal" data-target="#myModal">Add Interest</button>
                        </li>
                    </ol>
                </div>
            </div>

            <div class="row">
			    <div class="col-xs-6 col-sm-6 col-md-6">
			        <div class="form-group">
			            <select id="selectVendor" class="vendorDropdown" onchange="LoadSelectionVendorProfiles()">
                        </select>
			        </div>
			    </div>
                <div class="col-xs-6 col-sm-6 col-md-6">
			        <div class="form-group">
			            <select id="selectProfile" class="vendorDropdown" onchange="LoadInterest()">
                                                    
                        </select>
			        </div>
			    </div>
            </div>
            <div class="row">
                    <div class="col-lg-12">
                        <div class="table-responsive">
                            <table class="table table-bordered table-hover table-striped table-responsive">
                                <thead>
                                    <tr>
                                        <th>Category</th>
                                        <th>Interest</th>
                                        <th>Radius</th>
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
                        <h4 class="modal-title">Add Interest Information</h4>
                      </div>
                      <div class="modal-body">
                           <div class="row">
			    				        <div class="col-xs-6 col-sm-6 col-md-6">
			    					        <div class="form-group">
			                                    <select id="vendorMenu" class="vendorDropdown" onchange="LoadVendorProfiles()">
                                                </select>
			    					        </div>
			    				        </div>
                                        <div class="col-xs-6 col-sm-6 col-md-6">
			    					        <div class="form-group">
			                                    <select id="vendorCategory" class="vendorDropdown" onchange="LoadSubCategories()">
                                                </select>
			    					        </div>
			    				        </div>
			    				        
			    			        </div>
                                    <div class="row">
			    				        <div class="col-xs-6 col-sm-6 col-md-6">
			    					        <div class="form-group">
			                                    <select id="statusMenu" class="vendorDropdown">
                                                    <option value="1">Select Status</option>
                                                    <option value="1">Enabled</option>
                                                    <option value="0">Disabled</option>
                                                </select>
			    					        </div>
			    				        </div>
			    				        <div class="col-xs-6 col-sm-6 col-md-6">
			    					        <div class="form-group">
			    						        <select id="colorMenu" class="vendorDropdown">
                                                    <option value="9b26af">Select Color</option>
                                                    <option value="da3637">red</option>
                                                    <option value="3e3f94">Blue</option>
                                                    <option value="4c862d">Green</option>
                                                </select>
			    					        </div>
			    				        </div>
			    			        </div>
                                  <div class="row">
                                      <div class="col-xs-6 col-sm-6 col-md-6">
			    					        <div class="form-group">
			                                    <select id="vendorSubCategory" class="vendorDropdown">
                                                    
                                                </select>
			    					        </div>
			    				        </div>
                                    <div class="col-xs-6 col-sm-6 col-md-6">
			    				        <div class="form-group">
			    					        <input type="text" name="type" id="radius" class="form-control input-sm" placeholder="Radius" required>
			    				        </div>
			    			        </div>
                                  </div>
                                  <div class="row">
                                      <div class="col-xs-6 col-sm-6 col-md-6">
			    					        <div class="form-group">
			                                    <select id="vendorProfile" class="vendorDropdown">
                                                    
                                                </select>
			    					        </div>
			    				        </div>
                                </div>
			    			
			    			        <div id="fetch1" onclick="AddInterest()" class="btn btn-info btn-block">Add Interest</div>
                      </div>
                      <div class="modal-footer">
                        <button type="button" class="btn btn-default" data-dismiss="modal">Close</button>
                      </div>
                    </div>

                  </div>
                </div>


        <h1>Experimentation</h1>

        <img src="../Images/test.png" width="100" class="testImage" style="background-color:#3e3f94; border-radius:100px;" />
        <img src="../Images/test.png" width="100" class="testImage" style="background-color:#4c862d; border-radius:100px;" />
        <img src="../Images/test.png" width="100" class="testImage" style="background-color:#da3637; border-radius:100px;" />
        <img src="../Images/test.png" width="100" class="testImage" style="background-color:#880E4F; border-radius:100px;" />
        </div>
</asp:Content>
