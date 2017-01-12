<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ManageCategories.aspx.cs" MasterPageFile="~/MasterPages/Site1.master" Inherits="WebCrawler.Views.ManageCategories" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    

   <link href="../Styles/MainStyles.css" rel="stylesheet" />

    <script src="../Scripts/jquery-1.8.2.js"></script>
    <script>


        var username = sessionStorage.getItem("username");

        if (username == "" || username == null) {
            window.location = "Login.aspx";
        }
        $(document).ready(function () {
            //TestPostMethod();

            LoadIncWord();

            $(document).keypress(function (e) {
                if (e.which == 13) {

                    var word = document.activeElement.id;

                    if (word == "adword") {
                        AddWord(1)
                    }
                }
            });
        });




        function AddWord(table) {

            var word = document.getElementById('adword').value;

            if (word == "") {
                document.getElementById('fetch').innerHTML = "Please enter valid category";
                return false;
            }
            document.getElementById('fetch').innerHTML = "Adding. Please wait...";
            $.ajax({
                url: "/api/Values/Get?status=3&table=category&word=" + word + "",
                type: 'GET',
                success: function (data) {

                    var fetch = JSON.parse(data);
                    document.getElementById('mWraper').innerHTML = "";
                    for (var i = 0; i < fetch.length; i++) {

                        document.getElementById('mWraper').innerHTML += "<div class=\"col-lg-3 text-center\"><div class=\"panel panel-default\"><div class=\"panel-body\"><div class=\"WordContainer\"><div class=\"word\">" + fetch[i].word + "</div><div class=\"removeWord\" id=\"" + fetch[i].Id + "\" onclick=\"RemoveIncWord(id)\">Remove</div></div></div></div></div>";

                    }
                    document.getElementById('adword').value = "";
                    document.getElementById('fetch').innerHTML = "Add category you want to include in crawl result";
                },
                statusCode: {
                    404: function () {
                        alert('Failed');
                    }
                }
            });
        }


        function RemoveIncWord(id) {

            document.getElementById('fetch').innerHTML = "Removing Cat. Please wait...";

            $.ajax({
                url: "/api/Values/Get?status=333&Id=" + id + "&table=category",
                type: 'GET',
                success: function (data) {

                    var fetch = JSON.parse(data);

                    document.getElementById('mWraper').innerHTML = "";
                    for (var i = 0; i < fetch.length; i++) {

                        document.getElementById('mWraper').innerHTML += "<div class=\"col-lg-3 text-center\"><div class=\"panel panel-default\"><div class=\"panel-body\"><div class=\"WordContainer\"><div class=\"word\">" + fetch[i].word + "</div><div class=\"removeWord\" id=\"" + fetch[i].Id + "\" onclick=\"RemoveIncWord(id)\">Remove</div></div></div></div></div>";

                    }
                    //debugger;
                    document.getElementById('fetch').innerHTML = "Add category you want to include in crawl result";
                },
                statusCode: {
                    404: function () {
                        alert('Failed');
                    }
                }
            });
        }














        function LoadIncWord() {

            document.getElementById('fetch').innerHTML = "Loading. Please wait...";
            $.ajax({
                url: "/api/Values/Get?status=33&table=category",
                type: 'GET',
                success: function (data) {

                    var fetch = JSON.parse(data);
                    document.getElementById('mWraper').innerHTML = "";
                    for (var i = 0; i < fetch.length; i++) {

                        document.getElementById('mWraper').innerHTML += "<div class=\"col-lg-3 text-center\"><div class=\"panel panel-default\"><div class=\"panel-body\"><div class=\"WordContainer\"><div class=\"word\">" + fetch[i].word + "</div><div class=\"removeWord\" id=\"" + fetch[i].Id + "\" onclick=\"RemoveIncWord(id)\">Remove</div></div></div></div></div>";

                    }
                    //debugger;
                    document.getElementById('fetch').innerHTML = "Add category you want to include in crawl result";
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

    <%--<img src="../Images/logo.png" width="80" class="logo" />--%>

    <div class="row">
                <div class="col-lg-12">
                    <h1 class="page-header">
                        Qlumi <small>Category Filter</small>
                    </h1>
                    
                    <ol class="breadcrumb" style="text-align:right;">
                        <li>
                            <span id="fetch">Add category you want to include in crawl result</span>
                        </li>
                        <li>
                            <input type="text" name="Add Words" id="adword" class="form-control input-sm" placeholder="Add Words" required />
                        </li>
                        <li class="active">
                            <button type="button" id="urlSubmit"  class="btn btn-sm btn-primary" onclick="AddWord(1)" >Add Category</button>
                        </li>
                        
                    </ol>
                </div>
            </div>
            <div class="row" id="mWraper">
 

            </div>
    
    </asp:Content>
