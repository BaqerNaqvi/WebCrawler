<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ForgotPassword.aspx.cs" Inherits="WebCrawler.Views.ForgotPassword" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Forgot Password</title>
    <link href="../Styles/css/style.css" rel="stylesheet" />
    <script src="../Scripts/jquery-1.8.2.js"></script>

    <script>
        //location.href = '../m/mIndex.aspx';
        $(document).ready(function ($) {
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


        });


        function ForgetPass() {

            //alert();
            var a = 0
            //debugger;
            jQuery.support.cors = true;

            if (document.getElementById('forgetemail').value == "") {
                $("#forgetmessage").html("Please Enter Your Email ");
                return false;
            }

            document.getElementById('subForgetBtn').innerHTML = "Please wait..";
            $.ajax({
                url: '/api/User',
                type: 'POST',
                data: {
                    Email: document.getElementById('forgetemail').value,
                    twitter: 1,
                    password: "",


                },
                success: function (data) {

                    if (data != 0) {
                        alert("Email Sent! Please check your email address to reset password");
                        //window.location.href = "index.aspx";
                        document.getElementById('subForgetBtn').innerHTML = "Submit";
                        CloseForgotOverlay();


                    }
                    else {
                        document.getElementById('forgetmessage').innerHTML = "Wrong Email ";
                        document.getElementById('subForgetBtn').innerHTML = "Submit";
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
    <form id="form1" runat="server" class="form">
      
            <div class="container">

              <div id="login-form">

                <h3>Login</h3>

                <fieldset>

                  <form action="javascript:void(0);" method="get">

                    <input type="email" required value="Email" onBlur="if(this.value=='')this.value='Email'" onFocus="if(this.value=='Email')this.value='' "> <!-- JS because of IE support; better: placeholder="Email" -->

                    <input type="password" required value="Password" onBlur="if(this.value=='')this.value='Password'" onFocus="if(this.value=='Password')this.value='' "> <!-- JS because of IE support; better: placeholder="Password" -->

                    <input type="submit" value="Login">

                    <footer class="clearfix">

                      <p><span class="info">?</span><a href="#">Forgot Password</a></p>

                    </footer>

                  </form>

                </fieldset>

              </div> <!-- end login-form -->

            </div>
      
    </form>
</body>
</html>
