<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="WebCrawler.Views.Login" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
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
            //alert();
            document.getElementById('noti').innerHTML = "";
            document.getElementById('loginBtn').value = "Please wait...";
            $.ajax({
                url: '/api/User/Post',
                type: 'POST',
                data: {
                    email: email,
                    password: password,
                    name: 'login'

                },
                success: function (data) {
                    //debugger;
                    if (data != 0) {

                        debugger;

                        sessionStorage.setItem("username", JSON.parse(data).Id);
                        document.getElementById('loginBtn').value = "Done";
                        document.getElementById('noti').innerHTML = "";
                        window.location = "Home.aspx";


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


        function RegisterAdmin() {

            var name = document.getElementById('name').value;
            var email = document.getElementById('email').value;
            var username = document.getElementById('username').value;
            var password = document.getElementById('password').value;
            document.getElementById('start').innerText = "Please wait...";
            //debugger;
            //check mail
            $.ajax({
                url: '/api/User/Post',
                type: 'POST',
                data: {
                    email: email,
                    name: name,
                    username: username,
                    password: password

                },
                success: function (data) {
                    //debugger;
                    if (data != 0) {

                        document.getElementById('start').innerText = "Done!";
                        sessionStorage.setItem("username", JSON.parse(data).Id);
                        window.location = "Home.aspx";


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
               <p><input type="email" id="loginEmail" placeholder="Email"></p>
               
            <p><input type="password" id="loginPassword" placeholder="Password"></p>
            <div class="noti" id="noti"></div>
            <p><input type="submit" id="loginBtn" onclick="AuthenticateUser()" value="Log in"></p>
           </div>
            
        </div>
    </form>
</body>
</html>
