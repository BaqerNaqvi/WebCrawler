<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="VendorRegistration.aspx.cs" Inherits="WebCrawler.Views.VendorRegistration" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Login</title>
    <link href="../Styles/css/style.css" rel="stylesheet" />
    <script src="../Scripts/jquery-1.8.2.js"></script>

    <script>
        //location.href = '../m/mIndex.aspx';
        $(document).ready(function ($) {

            $("#form1").submit(function (event) {
                //alert();
                event.preventDefault();

                //return false;
            });


        });

        function AddVendor() {

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
                data: {
                    name: document.getElementById('name').value,
                    email: document.getElementById('email').value,
                    contactNumber: document.getElementById('phone').value,
                    unlock_code: 'qlumi_welcome',
                    website: document.getElementById('website').value,
                    logoImage: '',
                    password: document.getElementById('loginPassword').value,
                    tokenLimit: "10000",
                    unlockCodeAge: "360"

                },
                success: function (vendorInfo) {

                    if (vendorInfo.length > 0) {
                        //debugger;
                        UploadLogo(vendorInfo[0].Id);

                        sessionStorage.setItem("name", vendorInfo[0].name);
                        sessionStorage.setItem("email", vendorInfo[0].email);
                        sessionStorage.setItem("Id", vendorInfo[0].Id);
                        sessionStorage.setItem("contactNumber", vendorInfo[0].contactNumber);
                        sessionStorage.setItem("logoImage", vendorInfo[0].logoImage);
                        sessionStorage.setItem("unlock_code", vendorInfo[0].unlock_code);
                        sessionStorage.setItem("tokenLimit", vendorInfo[0].tokenLimit);
                        sessionStorage.setItem("curentTokenCount", vendorInfo[0].currentTokenCount);
                        sessionStorage.setItem("unlockCodeAge", vendorInfo[0].unlockCodeAge);

                        document.getElementById('noti').innerHTML = "";


                        document.getElementById('website').value = "";
                        document.getElementById('fetch1').innerHTML = "Get Started";
                        window.location = "VendorPortal.aspx";
                    }
                    else {
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

            //debugger;
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
    </script>
    <style>
        .login input {
          padding: 10px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
      <div class="login">
          <div class="logoContainer">
              <img src="../Images/logo.png" width="110" />
          </div>
          
          <div class="login-triangle"></div>
  
          <h2 class="login-header">Sign up</h2>
           <div class="login-container">


               <p><input type="text" id="name" placeholder="Name" /></p>
               <p><input type="email" id="email" placeholder="Email" /></p>            
               <p><input type="text" id="phone" placeholder="Phone" /></p>
               <p><input type="text" id="website" placeholder="Website" /></p>
               <p><input type="password" id="loginPassword" placeholder="Password" /></p>

               <p>
                   <label>Upload Logo</label>
			    	<input type="file" id="fileUpload" />

               </p>
               <div class="noti" id="noti"></div>
               <p><input type="submit" id="fetch1" onclick="AddVendor()" value="Get Started" /></p>
           </div>
            
        </div>
    </form>
</body>
</html>

