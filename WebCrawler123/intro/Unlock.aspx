<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="WebCrawler._Default" %>

<!DOCTYPE html>

<html lang="en" xmlns="http://www.w3.org/1999/xhtml">
<head>
    <meta charset="utf-8" />
    <title>Qlumi - Welcome</title>

    <script src="../Scripts/jquery-1.8.2.js"></script>
    <script>
        
        
        $(document).ready(function () {
            GetRemainingUnlockCodes();
        });
        
        function GetRemainingUnlockCodes() {
            //Set email address to get remaining codes
            var email = "ethan@meridians.info";

            $.ajax({
                url: '/api/User/RemainingUnlockCodes',
                type: 'POST',
                data: {
                    email: email
                },
                success: function (data) {
                    //debugger;
                    if (data != 0) {

                        
                        var remainingToken = data.vendorInfo[0].tokenLimit - data.vendorInfo[0].currentTokenCount;
                        document.getElementById('tokenCount').innerHTML = remainingToken;
                        debugger;
                    }
                    else {
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
        body {
            background-color:white;
            padding:0; margin:0;
           

        }
        .logo {
            vertical-align:middle;
            margin-top:10%;
        }

a:link { color:blue; text-decoration: none; }
a:visited { color:blue; text-decoration: none; }
a:hover { color:blue; text-decoration: none; }
a:active { color:blue; text-decoration: underline; }


    </style>
</head>
<body>
 <br><br>
<center>
<font color="black" face="Helvetica"><font size=+3>
Unlock Qlumi</font></center>
<br><br>

<center><img src="./images/Qlumi_1024_small.png" border=0 width=128 height=128></center>
<br><br>
<dir>

<font size=-1>Oh snap! Qlumi requires an unlock code.<br><br>
<u id="tokenCount">00</u> <font color=red><b>free</b></font> unlock codes remaining.<br><br>
Yes, we're unapologetic app snobs. Shame on us! Please register with a valid email address and we will email you an unlock code ASAP. (If you don't receive it, check your Spam folder.) But wait, there's more. You're joining an exclusive club! Only the first 10,000 unlock codes for Qlumi will be free. Once we run out of free unlock codes, you will need to receive an unlock code from one of our corporate partners or purchase the app from us.</font>
</dir>


</body>
</html>