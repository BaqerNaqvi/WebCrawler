<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/MasterPages/AdDashboard.Master" CodeBehind="UpdatePassword.aspx.cs" Inherits="WebCrawler.Views.UpdatePassword" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
     

    <script src="../Scripts/map/jquery.min.js"></script>
    <script src="../Scripts/map/bootstrap.min.js"></script>
    <script src="../Scripts/map/platform.js"></script>
    

    


    </asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    
    <style>
     
         /* Optional: Makes the sample page fill the window. */
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
                Update Password
            </h2>
        </div>


        <div class="row">
            <div class="col-xs-12 col-sm-12 col-md-12">
                <div class="box box-primary">
                    <div class="box-header with-border">
                        <h3 class="box-title">Password Details</h3>
                    </div>
                     <div class="row">
                        <div class="controll-group col-xs-12 col-sm-12 col-md-6">
                            <label for="cpassword">Current Password:</label>
                            <input type="password"  id="cpassword" class="form-control input-sm  " placeholder="Enter Current Password"  />
                        </div>
                    </div>
                     <div class="row">
                        <div class="controll-group col-xs-12 col-sm-12 col-md-6">
                            <label for="npassword">New Password:</label>
                            <input type="password"  id="npassword" class="form-control input-sm  " placeholder="Enter New Password"  />
                        </div>
                          
                    </div>
                    <div class="row">
                        <div class="controll-group col-xs-12 col-sm-12 col-md-6">
                            <label for="ReNpassword">Repeat New Password:</label>
                            <input type="password" name="type" id="ReNpassword" class="form-control input-sm  " placeholder="Re-Enter New Password"  />
                        </div>
                          
                    </div>
                </div>              
            </div>
        </div>
        
 

        <div class="row">
            <div class="controll-group">
                
                <input type="button" onclick="BackToDashboard()" value="Cancel" class="btn btn-default"  style="margin-left: 12px;"/>
                <input type="button" id="updateprofilepassword" value="Update" class="btn btn-primary" />

            </div>
        </div>
     </div>


    <script>

        $(document).ready(function() {
        });

        function BackToDashboard() {
            window.location = "AdDashboard.aspx";
            return false;
        }


        $(document).on("click", "#updateprofilepassword", function (e) {
            
           
                updateProfilePass();
        });

        function updateProfilePass() {

            var cpass = $("#cpassword").val();
            var newpass = $("#npassword").val();
            var cnewpass = $("#ReNpassword").val();

            if (newpass ==="" || cpass==="" || newpass==="") {
                alert("Enter Data please!");
                return false;
            }

            if (newpass === cpass) {
                alert("Enter New Password!");
                return false;
            }
            
            if (newpass !== cnewpass) {
                alert("New Password should be matched!");
                return false;
            }

            document.getElementById('updateprofilepassword').innerHTML = "Wait..";
            document.getElementById('updateprofilepassword').value = "Wait..";

            $.ajax({
                url: "/api/User/UpDatePasswordByModel",
                type: 'POST',
                data: {
                    UserId: sessionStorage.getItem("Id"),
                    CurrentPass: cpass,
                    NewPass: newpass,
                    RepeatNewPass: cnewpass
                },
                success: function (response) {
                    if (response === 0) {
                        alert("Something went wrong. Try Again.");
                    } else {
                        window.location = "AdDashboard.aspx";
                        return false;
                    }
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


