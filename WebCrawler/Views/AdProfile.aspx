<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/MasterPages/AdDashboard.Master" CodeBehind="AdProfile.aspx.cs" Inherits="WebCrawler.Views.AdProfile" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
     

    <script src="../Scripts/map/jquery.min.js"></script>
    <script src="../Scripts/map/bootstrap.min.js"></script>
    <script src="../Scripts/map/platform.js"></script>
    

    


    </asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    
    <style>
       
      html, body {
        height: 100%;
        margin: 0;
        padding: 0;
      }
      .controls {
        margin-top: 10px;
        border: 1px solid transparent;
        border-radius: 2px 0 0 2px;
        box-sizing: border-box;
        -moz-box-sizing: border-box;
        height: 32px;
        outline: none;
        box-shadow: 0 2px 6px rgba(0, 0, 0, 0.3);
      }


 </style>


    <div class="MainWrapper">

        <div class="content-header">
            <h2>
                Manage Profile
            </h2>
        </div>


        <div class="row">
            <div class="col-xs-12 col-sm-12 col-md-12">
                <div class="box box-primary">
                    <div class="box-header with-border">
                        <h3 class="box-title">Profile Details</h3>
                    </div>
                     <div class="row">
                        <div class="controll-group col-xs-12 col-sm-12 col-md-6">
                            <label for="adName">Name:</label>
                            <input type="text" name="type" id="adName" class="form-control input-sm makeReadOnly " placeholder="Add Ad Title"  />
                        </div>
                          <div class="controll-group col-xs-12 col-sm-12 col-md-6">
                            <label for="adEmail">Email:</label>
                            <input type="text" name="type" id="adEmail" class="form-control input-sm makeReadOnly " placeholder="Add Ad Title"  />
                        </div>
                    </div>
                     <div class="row">
                        <div class="controll-group col-xs-12 col-sm-12 col-md-6">
                            <label for="AdContact">Contact No:</label>
                            <input type="text" name="type" id="AdContact" class="form-control input-sm makeReadOnly " placeholder="Add Ad Title"  />
                        </div>
                          <div class="controll-group col-xs-12 col-sm-12 col-md-6">
                            <label for="AdDes">Description:</label>
                            <input type="text" name="type" id="AdDes" class="form-control input-sm makeReadOnly " placeholder="Add Ad Title"  />
                        </div>
                    </div>
                    <div class="row">
                        <div class="controll-group col-xs-12 col-sm-12 col-md-6">
                            <label for="AdWebsite">Website:</label>
                            <input type="text" name="type" id="AdWebsite" class="form-control input-sm makeReadOnly " placeholder="Add Ad Title"  />
                        </div>
                    </div>
                     <div class="row">
                        <div class="controll-group col-xs-12 col-sm-12 col-md-6">
                            <input onclick="updatePasswordAd()" style="    float: left !important;" type="button"  value="Change Password" class="btn btn-primary" />
                        </div>
                    </div>
                </div>
                
            </div>
        </div>
        
 

        <div class="row">
            <div class="controll-group">
                
                <input type="button" onclick="BackToDashboard()" value="Cancel" class="btn btn-default"  style="margin-left: 12px;"/>
                <input type="button" id="editAdProfile" value="Edit" class="btn btn-primary" />

            </div>
        </div>
     </div>


    <script>

        $(document).ready(function() {
            getProfile(0);
        });

        function BackToDashboard() {
            window.location = "AdDashboard.aspx";
            return false;
        }

        function updatePasswordAd() {
            window.location = "updatepassword.aspx";
        }

        function getProfile(parameters) {
            $.ajax({
                url: "/api/Advertiser",
                type: 'Get',
                data: {
                    vendorId: sessionStorage.getItem("Id")
                },
                success: function (vendorInfo) {
                    if (vendorInfo !== null && vendorInfo!==undefined) {
                        $("#adName").val(vendorInfo.name);
                        $("#adEmail").val(vendorInfo.email);
                        $("#AdContact").val(vendorInfo.contactNumber);
                        $("#AdDes").val(vendorInfo.description);
                        $("#AdWebsite").val(vendorInfo.website);
                        $(".makeReadOnly").prop("disabled", true);
                    }

                },
                error: function(sds,asd,sd) {
                    var dfd = asd;
                }
                
            });
        }


        $(document).on("click", "#editAdProfile", function (e) {
            debugger 
            var val = document.getElementById('editAdProfile').value;
            if (val === "Edit") {
                document.getElementById('editAdProfile').innerHTML = "Update";
                document.getElementById('editAdProfile').value = "Update";
                e.preventDefault();
                $(".makeReadOnly").prop("disabled", false); // instead of alert($('.bk_name').html());  
            } else {
                updateProfile();
            }

            
        });

        function updateProfile() {
            if (!ValidateByClassName("makeReadOnly")) {
                return false;
            }


            $.ajax({
                url: "/api/Advertiser",
                type: 'post',
                data: {
                    Id:sessionStorage.getItem("Id"),
                      name:  $("#adName").val(),
                      email:  $("#adEmail").val(),
                      contactNumber: $("#AdContact").val(),
                      description:  $("#AdDes").val(),
                      website:  $("#AdWebsite").val()
                },
                success: function (vendorInfo) {
                    window.location = "AdDashboard.aspx";
                    return false;
                },
                error: function (sds, asd, sd) {
                    var dfd = asd;
                }

            });
        }

        function ValidateByClassName(className) {
            var requiredControls = $('.' + className);
            //now find required fields
            var counter = 0;
            for (var i = 0; i < requiredControls.length; i++) {
                var requiredField = $(requiredControls[i]);
                var fieldvalue = requiredField.val();
                var fieldType = requiredField.attr('type');
                var isError = false;
                switch (fieldType) {
                    case "email":
                        isError = !validateEmail(requiredField);
                        break;
                }
                if (fieldvalue === null || fieldvalue === undefined || (fieldvalue === "" || (fieldvalue.length >= requiredField.data('length'))) || isError) {
                    counter++;
                    requiredField.addClass('input-validation-error');
                } else {
                    requiredField.removeClass('input-validation-error');
                }
            }
            if (counter > 0) {
                //implementing focus back to error
                var divId = $('.' + className + ".input-validation-error")[0].id;
                if ($("#" + divId).length > 0)
                    $("#" + divId).focus();
                return false;
            }
            return true;
        }
    </script>

</asp:Content>


