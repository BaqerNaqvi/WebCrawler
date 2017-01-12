<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AppCategory.aspx.cs" MasterPageFile="~/MasterPages/Site1.master" Inherits="WebCrawler.Views.AppCategory" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <script src="../Scripts/jquery-1.8.2.min.js"></script>
    <script>



        $(document).ready(function () {
            //TestPostMethod();
            LoadCategories();

        });

       

        function LoadCategories() {
            document.getElementById('vendorTable').innerHTML = "";
            $.ajax({
                url: "/api/User/AllCategories",
                type: 'POST',
                data: {
                },
                success: function (catInfo) {

                    document.getElementById('vendorTable').innerHTML = "";
                    for (var i = 0; i < catInfo.length; i++) {

                        document.getElementById('vendorTable').innerHTML += "<tr><td>" + catInfo[i].categoryName + "</td><td>" + catInfo[i].categoryType + "</td><td><img class=\"img-thumbnail\" src=\"../Images/hdpi/" + catInfo[i].iconName + "_green.png\" width=\"54\" alt=\"\"></td><td><img class=\"img-thumbnail\" src=\"../Images/hdpi/" + catInfo[i].iconMarkerName + "_blue.png\" width=\"54\" alt=\"\"></td><td>" + catInfo[i].color + "</td><td><div class=\"btn btn-danger btn-primary\" id=\"" + catInfo[i].Id + "\" onclick=\"RemoveVendor(id)\">Remove</div></td></tr>";

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
    </style>
    <div class="container-fluid">

            <!-- Page Heading -->
            <div class="row">
                <div class="col-lg-12">
                    <h1 class="page-header">
                        Qlumi <small>App Category</small>
                    </h1>
                    
                    <ol class="breadcrumb" style="text-align:right;">
                        <li class="active">
                            <button type="button" class="btn btn-sm btn-primary" data-toggle="modal" data-target="#myModal">Add Category</button>
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
                                        <th>Type</th>
                                        <th>Icon</th>
                                        <th>Icon Marker</th>
                                        <th>Color</th>
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
                        <h4 class="modal-title">Add Category Information</h4>
                      </div>
                      <div class="modal-body">
                            <form role="form">
			    			        <div class="row">
			    				        <div class="col-xs-6 col-sm-6 col-md-6">
			    					        <div class="form-group">
			                                    <input type="text" name="first_name" id="first_name" class="form-control input-sm" placeholder="Name" required>
			    					        </div>
			    				        </div>
			    				        <div class="col-xs-6 col-sm-6 col-md-6">
			    					        <div class="form-group">
			    						        <input type="text" name="type" id="type" class="form-control input-sm" placeholder="Category Type" required>
			    					        </div>
			    				        </div>
			    			        </div>
                                    <div class="row">
			    				        <div class="col-xs-6 col-sm-6 col-md-6">
			    					        <div class="form-group">
			                                    <div class="dropdown">
                                                  <button class="btn btn-default dropdown-toggle" type="button" data-toggle="dropdown">Icon Name
                                                  <span class="caret"></span></button>
                                                  <ul class="dropdown-menu">
                                                    <li><a href="#">Bank Red</a></li>
                                                    <li><a href="#">Bank Green</a></li>
                                                    <li><a href="#">Bank Blue</a></li>
                                                  </ul>
                                                </div>
			    					        </div>
			    				        </div>
			    				        <div class="col-xs-6 col-sm-6 col-md-6">
			    					        <div class="form-group">
			    						        <div class="dropdown">
                                                  <button class="btn btn-default dropdown-toggle" type="button" data-toggle="dropdown">Icon Marker Name
                                                  <span class="caret"></span></button>
                                                  <ul class="dropdown-menu">
                                                    <li><a href="#">Bank Marker red</a></li>
                                                    <li><a href="#">Bank Marker blue</a></li>
                                                    <li><a href="#">Bank Marker green</a></li>
                                                  </ul>
                                                </div>
			    					        </div>
			    				        </div>
			    			        </div>
                                    <div class="form-group">
			    						<textarea class="form-control" placeholder="Description"></textarea>
			    					</div>

			    			        <div class="row">
			    				        <div class="col-xs-6 col-sm-6 col-md-6">
			    					        <label>Upload Logo</label>
			    				        </div>
			    				        <div class="col-xs-6 col-sm-6 col-md-6">
			    					        <div class="form-group">
			    						        <input type="file" />
			    					        </div>
			    				        </div>
			    			        </div>
			    			
			    			        <input type="submit" value="Register" class="btn btn-info btn-block">
			    		
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