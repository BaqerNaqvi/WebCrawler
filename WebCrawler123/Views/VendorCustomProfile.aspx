<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="VendorCustomProfile.aspx.cs" MasterPageFile="~/MasterPages/VendorDashboard.master" Inherits="WebCrawler.VendorCustomProfile" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <script src="../Scripts/jquery-1.8.2.min.js"></script>
    <script>


        if (sessionStorage.getItem("Id") == "" || sessionStorage.getItem("Id") == null) {
            window.location = "VendorLogin.aspx";
        }
        $(document).ready(function () {


            LoadVendorPro();
            LoadCategories();
            LoadVendorProfilesInDropDown();
            LoadFirstInterest();
            $(document).keypress(function (e) {
                if (e.which == 13) {

                    var word = document.activeElement.id;

                }
            });

        });



        function AddVendorsProfile() {

            document.getElementById('vendorTableProfile').innerHTML = "Adding. Please wait...";
            $.ajax({
                url: "/api/User/AddVendorProfile",
                type: 'POST',
                data: {
                    vendorId: sessionStorage.getItem("Id"),
                    profileName: document.getElementById('profileName').value,
                    color: $("#colorMenu").val(),
                    colorId: '1',
                    isActive: $("#statusMenu").val()

                },
                success: function (vendorInfo) {

                    debugger;
                    //var fetch = JSON.parse(vendorInfo);
                    document.getElementById('vendorTableProfile').innerHTML = "";
                    debugger;
                    for (var i = 0; i < vendorInfo.length; i++) {

                        document.getElementById('vendorTableProfile').innerHTML += "<tr><td>" + vendorInfo[i].name + "</td><td>" + vendorInfo[i].email + "</td><td>" + vendorInfo[i].contactNumber + "</td><td><div><span class=\"stateTitle\">Code:&nbsp</span><span class=\"stateFigur\">" + vendorInfo[i].unlock_code + "</span></div><br/><div><span class=\"stateTitle\">Total Limit:&nbsp</span ><span class=\"stateFigur\">" + vendorInfo[i].tokenLimit + "</span></div><br/><div><span class=\"stateTitle\">Current Count:&nbsp</span><span class=\"stateFigur\">" + vendorInfo[i].currentTokenCount + "</span></div></td><td>" + vendorInfo[i].description + "</td><td><img class=\"img-thumbnail\" src=\"../UploadedFiles/" + vendorInfo[i].logoImage + "\" width=\"40\" alt=\"\"></td><td>" + vendorInfo[i].website + "</td><td><div class=\"btn btn-danger btn-primary\" id=\"" + vendorInfo[i].Id + "\" onclick=\"RemoveVendor(id)\">Remove</div></td></tr>";

                    }
                    LoadVendorPro();
                    LoadVendorProfilesInDropDown();
                },
                statusCode: {
                    404: function () {
                        alert('Failed');
                    }
                }
            });
        }

        function LoadVendorPro() {

            document.getElementById('vendorTableProfile').innerHTML = "Loading Profiles..";
            $.ajax({
                url: "/api/User/AllVendorProfiles",
                type: 'POST',
                data: {
                    userId: sessionStorage.getItem("Id")
                },
                success: function (vendorInfo) {

                    
                    document.getElementById('vendorTableProfile').innerHTML = "";
                    for (var i = 0; i < vendorInfo.length; i++) {

                        document.getElementById('vendorTableProfile').innerHTML += "<tr><td>" + vendorInfo[i].name + "</td><td>" + vendorInfo[i].profileName + "</td><td>" + vendorInfo[i].color + "</td><td>" + vendorInfo[i].isActive + "</td><td><div class=\"btn btn-danger btn-primary\" id=\"" + vendorInfo[i].Id + "\" onclick=\"RemoveVendor(id)\">Remove</div></td></tr>";

                    }
                    
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
                    Id: catId
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

        function LoadVendorProfilesInDropDown() {


            //debugger;
            document.getElementById('vendorProfile').innerHTML = "<option value=\"1\">Loading...</option>";
            $.ajax({
                url: "/api/User/VendorProfile",
                type: 'POST',
                data: {
                    vendorId: sessionStorage.getItem("Id")
                },
                success: function (vendorInfo) {

                    document.getElementById('selectProfile').innerHTML = "<option value=\"1\">Select Profile</option>";
                    document.getElementById('vendorProfile').innerHTML = "<option value=\"1\">Select Profile</option>";
                    //debugger;
                    for (var i = 0; i < vendorInfo.length; i++) {

                        document.getElementById('selectProfile').innerHTML += "<option value=\"" + vendorInfo[i].Id + "\">" + vendorInfo[i].profileName + "</option>";
                        document.getElementById('vendorProfile').innerHTML += "<option value=\"" + vendorInfo[i].Id + "\">" + vendorInfo[i].profileName + "</option>";

                    }
                    sessionStorage.setItem("vpId", vendorInfo[0].Id);
                    debugger;
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
                    vendorId: sessionStorage.getItem("Id"),
                    settingId: $("#vendorProfile").val(),
                    color: $("#colorMenu").val(),
                    categoryId: $("#vendorCategory").val(),
                    subCategoryId: $("#vendorSubCategory").val(),
                    isActive: $("#statusMenu").val(),
                    radius: document.getElementById("radius").value

                },
                success: function (vendorInfo) {

                    document.getElementById('vendorTable').innerHTML = "";
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


        function LoadFirstInterest() {

            document.getElementById('vendorTable').innerHTML = "Loading. Please wait...";
            $.ajax({
                url: "/api/User/AllVendorInterest",
                type: 'POST',
                data: {
                    Id: sessionStorage.getItem("vpId")
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
         #VendorCustomProfile {
             background-color: #1e3040 !important;
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
                            <button type="button" class="btn btn-sm btn-primary" data-toggle="modal" data-target="#myModalProfile">Add Profile</button>
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
                                <tbody id="vendorTableProfile">
                                    
                                </tbody>
                            </table>
                        </div>
                    </div>
                </div>

            <div id="myModalProfile" class="modal fade" role="dialog">
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
			    						        <input type="text" name="type" id="profileName" class="form-control input-sm" placeholder="Profile Name" required />
			    					        </div>
			    				        </div>
                                        <div class="col-xs-6 col-sm-6 col-md-6">
			    					        <div class="form-group">
			    						        <select id="Select1" class="vendorDropdown">
                                                    <option value="9b26af">Purple</option>
                                                    <option value="3e3f94">Blue</option>
                                                    <option value="4c862d">Green</option>
                                                    <option value="4c862d">Red</option>
                                                </select>
			    					        </div>
			    				        </div>
			    			        </div>
                                    <div class="row">
			    				        <div class="col-xs-6 col-sm-6 col-md-6">
			    					        <div class="form-group">
			                                    <select id="Select2" class="vendorDropdown">
                                                    <option value="1">Enabled</option>
                                                    <option value="0">Disabled</option>
                                                </select>
			    					        </div>
			    				        </div>
			    				        
			    			        </div>

			    			
			    			        <div id="fetch1Profile" onclick="AddVendorsProfile()" class="btn btn-info btn-block">Add Profile</div>
                      </div>
                      <div class="modal-footer">
                        <button type="button" class="btn btn-default" data-dismiss="modal">Close</button>
                      </div>
                    </div>

                  </div>
                </div>






















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
			                                    <select id="vendorCategory" class="vendorDropdown" onchange="LoadSubCategories()">
                                                </select>
			    					        </div>
			    				        </div>
                                        <div class="col-xs-6 col-sm-6 col-md-6">
			    					        <div class="form-group">
			                                    <select id="vendorProfile" class="vendorDropdown">
                                                    
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
			    			
			    			        <div id="fetch1" onclick="AddInterest()" class="btn btn-info btn-block">Add Interest</div>
                      </div>
                      <div class="modal-footer">
                        <button type="button" class="btn btn-default" data-dismiss="modal">Close</button>
                      </div>
                    </div>

                  </div>
                </div>

        </div>
</asp:Content>
