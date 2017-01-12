<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Vendors.aspx.cs" MasterPageFile="~/MasterPages/Site1.master" Inherits="WebCrawler.Views.Vendors" %>




<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <script src="../Scripts/jquery-1.8.2.min.js"></script>
    <script>



        $(document).ready(function () {
            //TestPostMethod();
            LoadVendors();

            $(document).keypress(function (e) {
                if (e.which == 13) {

                    var word = document.activeElement.id;

                }
            });

        });

        function AddVendors() {

            var website = document.getElementById('website').value;
            var re = /^(http[s]?:\/\/){0,1}(www\.){0,1}[a-zA-Z0-9\.\-]+\.[a-zA-Z]{2,5}[\.]{0,1}/;
            if (!re.test(website)) {
                alert("Please Enter Valid URL..");
                return false;
            }

            document.getElementById('fetch1').innerHTML = "Adding. Please wait...";
            $.ajax({
                url: "/api/User/RegisterVendor",
                type: 'POST',
                data:{
                    name: document.getElementById('name').value,
                    email:document.getElementById('email').value,
                    contactNumber: document.getElementById('phone').value,
                    unlock_code:'IOSQLUMI',
                    description: document.getElementById('description').value,
                    website: document.getElementById('website').value,
                    logoImage: '',
                    password: 'searay',
                    tokenLimit: document.getElementById('code').value,
                    unlockCodeAge: $("#tokenAge").val()
                        
                },
                success: function (vendorInfo) {

                    debugger;
                    //var fetch = JSON.parse(vendorInfo);
                    if(vendorInfo.length > 0)
                    {
                        document.getElementById('vendorTable').innerHTML = "";
                        debugger;
                        UploadLogo(vendorInfo[0].Id);
                        for (var i = 0; i < vendorInfo.length; i++) {

                            document.getElementById('vendorTable').innerHTML += "<tr><td>" + vendorInfo[i].name + "</td><td>" + vendorInfo[i].email + "</td><td>" + vendorInfo[i].contactNumber + "</td><td><div><span class=\"stateTitle\">Code:&nbsp</span><span class=\"stateFigur\">" + vendorInfo[i].unlock_code + "</span></div><br/><div><span class=\"stateTitle\">Total Limit:&nbsp</span ><span class=\"stateFigur\">" + vendorInfo[i].tokenLimit + "</span></div><br/><div><span class=\"stateTitle\">Code Age:&nbsp</span ><span class=\"stateFigur\">" + vendorInfo[i].unlockCodeAge + "</span></div><br/><div><span class=\"stateTitle\">Current Count:&nbsp</span><span class=\"stateFigur\">" + vendorInfo[i].currentTokenCount + "</span></div></td><td>" + vendorInfo[i].description + "</td><td><img class=\"img-thumbnail\" src=\"../UploadedFiles/" + vendorInfo[i].logoImage + "\" width=\"40\" alt=\"\"></td><td>" + vendorInfo[i].website + "</td><td><div class=\"btn btn-danger btn-primary\" id=\"" + vendorInfo[i].Id + "\" onclick=\"RemoveVendor(id)\">Remove</div></td></tr>";

                        }
                        document.getElementById('website').value = "";
                        document.getElementById('fetch1').innerHTML = "Add Vendor";
                        window.location.reload();
                    }
                    else
                    {
                        alert("vendor already exist! Please try different email.");
                    }
                    
                },
                statusCode: {
                    404: function () {
                        alert('Failed');
                    }
                }
            });
        }
        function UploadLogo(vendorId) {

            debugger;
            var data = new FormData();

            var files = $("#fileUpload").get(0).files;

            // Add the uploaded image content to the form data collection
            if (files.length > 0) {
                data.append("UploadedImage", files[0]);
            }
            else {

                alert("Please upload vendor logo");

            }
            var uniquekey = {
                vendorId: vendorId
            };
            data.append("vendorId", vendorId);
            // Make Ajax request with the contentType = false, and procesDate = false
            var ajaxRequest = $.ajax({
                type: "POST",
                url: "/api/User/uploadfile",
                contentType: false,
                processData: false,
                data: data
            });

            ajaxRequest.done(function (xhr, textStatus) {
                //alert("Image uploaded");
               
            });
        }
        function RemoveVendor(vendorId) {

            $.ajax({
                url: "/api/User/RemoveVendor",
                type: 'POST',
                data: {
                    Id:vendorId
                },
                success: function (data) {
                    LoadVendors();
                    alert("Vendor successfully delete !");
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

                    document.getElementById('vendorTable').innerHTML = "";
                    for (var i = 0; i < vendorInfo.length; i++) {

                        document.getElementById('vendorTable').innerHTML += "<tr><td>" + vendorInfo[i].name + "</td><td>" + vendorInfo[i].email + "</td><td>" + vendorInfo[i].contactNumber + "</td><td><div><span class=\"stateTitle\">Code:&nbsp</span><span class=\"stateFigur\">" + vendorInfo[i].unlock_code + "</span></div><br/><div><span class=\"stateTitle\">Total Limit:&nbsp</span ><span class=\"stateFigur\">" + vendorInfo[i].tokenLimit + "</span></div><br/><br/><div><span class=\"stateTitle\">Code Age:&nbsp</span ><span class=\"stateFigur\">" + vendorInfo[i].unlockCodeAge + "</span></div><div><span class=\"stateTitle\">Current Count:&nbsp</span><span class=\"stateFigur\">" + vendorInfo[i].currentTokenCount + "</span></div></td><td>" + vendorInfo[i].description + "</td><td><img class=\"img-thumbnail\" src=\"../UploadedFiles/" + vendorInfo[i].logoImage + "\" width=\"40\" alt=\"\"></td><td>" + vendorInfo[i].website + "</td><td><div class=\"btn btn-danger btn-primary\" id=\"" + vendorInfo[i].Id + "\" onclick=\"RemoveVendor(id)\">Remove</div></td></tr>";

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

        .vendorDropdown {
            width: 100%;
            height: 31px;
            border-color: #ccc;
            border-radius: 4px;
        }
    </style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">

    <div class="container-fluid">
            <!-- Page Heading -->
            <div class="row">
                <div class="col-lg-12">
                    <h1 class="page-header">
                        Qlumi <small>Vendors</small>
                    </h1>
                    
                    <ol class="breadcrumb" style="text-align:right;">
                        <li class="active">
                            <button type="button" class="btn btn-sm btn-primary" data-toggle="modal" data-target="#myModal">Add Vendor</button>
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
                                        <th>Name</th>
                                        <th>Email</th>
                                        <th>Phone</th>
                                        <th>Code Stats</th>
                                        <th>Description</th>
                                        <th>Logo</th>
                                        <th>Website</th>
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
                        <h4 class="modal-title">Add Vendor Information</h4>
                      </div>
                      <div class="modal-body">
                            <form role="form">
			    			        <div class="row">
			    				        <div class="col-xs-6 col-sm-6 col-md-6">
			    					        <div class="form-group">
			                                    <input type="text" name="first_name" id="name" class="form-control input-sm" placeholder="Name" required>
			    					        </div>
			    				        </div>
			    				        <div class="col-xs-6 col-sm-6 col-md-6">
			    					        <div class="form-group">
			    						        <input type="email" name="email" id="email" class="form-control input-sm" placeholder="Email Address" required>
			    					        </div>
			    				        </div>
			    			        </div>
                                    <div class="row">
			    				        <div class="col-xs-6 col-sm-6 col-md-6">
			    					        <div class="form-group">
			                                    <input type="text" name="phone" id="phone" class="form-control input-sm" placeholder="Phone" required />
			    					        </div>
			    				        </div>
			    				        <div class="col-xs-6 col-sm-6 col-md-6">
			    					        <div class="form-group">
			    						        <input type="text" name="website" id="website" class="form-control input-sm" placeholder="Website" required />
			    					        </div>
			    				        </div>
			    			        </div>
                                    <div class="row">
                                    <div class="col-xs-6 col-sm-6 col-md-6">
                                    <div class="form-group">
                                        
                                            <textarea class="form-control" id="description" placeholder="Description"></textarea>
                                        </div>
                                    </div>
                                    <div class="col-xs-6 col-sm-6 col-md-6">
                                        <div class="form-group">
                                            <input type="text" name="code" id="code" class="form-control input-sm" placeholder="Code Limit" required />
                                        </div>
			    						
			    					</div>
                                    </div>
			    			        <div class="row">
			    				        <div class="col-xs-6 col-sm-6 col-md-6">
                                            
                                            <div class="form-group">
			    						        <select id="tokenAge" class="vendorDropdown">
                                                    <option value="30">30 Days</option>
                                                    <option value="180">180 Days</option>
                                                    <option value="365">365 Days</option>
                                                    <option value="730">730 Days</option>
                                                </select>
			    					        </div>
			    				        </div>
			    				        <div class="col-xs-6 col-sm-6 col-md-6">
			    					        <div class="form-group">
                                                <label>Upload Logo</label>
			    						        <input type="file" id="fileUpload" />
			    					        </div>
			    				        </div>
			    			        </div>
			    			
			    			        <div id="fetch1" onclick="AddVendors()" class="btn btn-info btn-block">Register</div>
			    		
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


