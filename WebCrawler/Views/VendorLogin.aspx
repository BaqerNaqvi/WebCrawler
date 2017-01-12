<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="VendorLogin.aspx.cs" Inherits="WebCrawler.Views.VendorLogin" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
      <style>
           .customControl {
        box-sizing: border-box;
    display: block;
    width: 100%;
    border-width: 1px;
    border-style: solid;
    padding: 16px;
    outline: 0;
    font-family: inherit;
    font-size: 0.95em;
    }
    </style>
    <title>Login</title>
    <link href="../Styles/css/style.css" rel="stylesheet" />
    <script src="../Scripts/jquery-1.8.2.js"></script>

    <script>
        //location.href = '../m/mIndex.aspx';
        $(document).ready(function ($) {

            <%Session["controlID"] = "This is my session";%>
            $(document).keypress(function (e) {
                if (e.which == 13) {
                    event.preventDefault();
                    //return false;
                }
            });

            $("#form1").submit(function (event) {
                //alert();
                event.preventDefault();

                //return false;
            });



            $('.form').find('input, textarea').on('keyup blur focus', function (e) {

                var $this = $(this),
                    label = $this.prev('label');

                if (e.type === 'keyup') {
                    if ($this.val() === '') {
                        label.removeClass('active highlight');
                    } else {
                        label.addClass('active highlight');
                    }
                } else if (e.type === 'blur') {
                    if ($this.val() === '') {
                        label.removeClass('active highlight');
                    } else {
                        label.removeClass('highlight');
                    }
                } else if (e.type === 'focus') {

                    if ($this.val() === '') {
                        label.removeClass('highlight');
                    }
                    else if ($this.val() !== '') {
                        label.addClass('highlight');
                    }
                }

            });

            $('.tab a').on('click', function (e) {

                e.preventDefault();

                $(this).parent().addClass('active');
                $(this).parent().siblings().removeClass('active');

                target = $(this).attr('href');

                $('.tab-content > div').not(target).hide();

                $(target).fadeIn(600);

            });
        });



        function AuthenticateUser() {
            //document.getElementById('submitBtn').innerHTML = "Please wait...";
            var email = document.getElementById('loginEmail').value;
            var password = document.getElementById('loginPassword').value;
            var usertype = document.getElementById('userType').value;
            //alert();
            document.getElementById('noti').innerHTML = "";
            document.getElementById('loginBtn').value = "Please wait...";
            $.ajax({
                url: '/api/User/LoginVendor',
                type: 'POST',
                data: {
                    email: email,
                    password: password,
                    name: 'login',
                    role: usertype
                },
                success: function (data) {
                    //debugger;
                    if (data != 0) {

                      //  debugger;

                        sessionStorage.setItem("name", data.name);
                        sessionStorage.setItem("email", data.email);
                        sessionStorage.setItem("Id", data.Id);
                        sessionStorage.setItem("contactNumber", data.contactNumber);
                        sessionStorage.setItem("logoImage", data.logoImage);
                        sessionStorage.setItem("unlock_code", data.unlock_code);
                        sessionStorage.setItem("tokenLimit", data.tokenLimit);
                        sessionStorage.setItem("curentTokenCount", data.currentTokenCount);
                        sessionStorage.setItem("unlockCodeAge", data.unlockCodeAge);
                        sessionStorage.setItem("name", data.role);
                        debugger;

                        document.getElementById('loginBtn').value = "Done";
                        document.getElementById('noti').innerHTML = "";

                        if (usertype === "0") { //&& data.role===0
                            window.location = "VendorPortal.aspx";
                        }
                        else if (usertype === "1") { //&& data.role === 1
                            window.location = "AdDashboard.aspx";
                        } else {
                            alert("You are not in current role!");
                        }
                        //debugger;


                    }
                    else {
                        document.getElementById('loginBtn').innerHTML = "Login";
                        document.getElementById('noti').innerHTML = "Wrong Email or Password. Try again";
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
    
</head>
<body>
    <form id="form1" runat="server">
      <div class="login">
          <div class="logoContainer">
              <img src="../Images/logo.png" width="110" />
          </div>
          
          <div class="login-triangle"></div>
  
          <h2 class="login-header">Log in</h2>
           <div class="login-container">
               <p><input type="email" id="loginEmail" placeholder="Email" /></p>
                <p>
                   <select class="customControl" id="userType">
                        <option value="0">Vendor</option>
                        <option value="1">Advertiser</option>
                   </select>
               </p>
            <p><input type="password" id="loginPassword" placeholder="Password" /></p>
            <div class="noti" id="noti"></div>
            <p><input type="submit" id="loginBtn" onclick="AuthenticateUser()" value="Log in" /></p>
           </div>
            
          <div class="bottomRow">
              <a href="VendorRegistration.aspx">Sign Up</a> | <a href="#">Forgot password?</a>

          </div>
        </div>
    </form>
</body>
</html>

