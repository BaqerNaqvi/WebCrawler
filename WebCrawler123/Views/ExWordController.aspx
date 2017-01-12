<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ExWordController.aspx.cs" MasterPageFile="~/MasterPages/Site1.master" Inherits="WebCrawler.Views.ExWordController" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <link href="../Styles/MainStyles.css" rel="stylesheet" />

    <script src="../Scripts/jquery-1.8.2.js"></script>
    <script>



        $(document).ready(function () {
            //TestPostMethod();

            LoadExWord();

            $(document).keypress(function (e) {
                if (e.which == 13) {

                    var word = document.activeElement.id;

                    if (word == "adword") {
                        AddWord(1)
                    }
                    if (word == "url") {
                        AddWordExWord(2)
                    }
                }
            });
        });

        function FetchData() {
            var url1 = document.getElementById('url').value;
            var re = /^(http[s]?:\/\/){0,1}(www\.){0,1}[a-zA-Z0-9\.\-]+\.[a-zA-Z]{2,5}[\.]{0,1}/;
            if (!re.test(url1)) {
                alert("Please Enter Valid URL..");
                return false;
            }

            document.getElementById('fetch').innerHTML = "Fetching Data. Please wait....";
            //debugger;
            $.ajax({
                url: "/api/Values/Get?status=2&crawlUrl=" + url1 + "",
                type: 'GET',
                success: function (data) {
                    //debugger;
                    var fetch = JSON.parse(data);
                    document.getElementById('mWraper').innerHTML = "";
                    for (var i = 0; i < fetch.length; i++) {

                        //debugger;
                        document.getElementById('mWraper').innerHTML += "<div class=\"heading\">" + fetch[i] + "</div>";

                    }

                    document.getElementById('fetch').innerHTML = "Crawler Successfull! Look for the results below.";
                },
                statusCode: {
                    404: function () {
                        alert('Failed');
                    }
                }
            });
        }



        function FetchWebsiteUrlFromGoogle() {

            var pId = "ChIJFXjIjXpO4DsR6IVFLCyThgY";
            var key = "AIzaSyCm19dj2kmNWVu6sPAx85em5IEI4UCNTl4";
            $.ajax({
                url: 'https://maps.googleapis.com/maps/api/place/details/json',
                type: 'GET',
                data: {
                    placeid: pId,
                    key: key
                },
                success: function (data) {

                    alert("done");
                    var fetch = JSON.parse(data);
                    document.getElementById('mWraper').innerHTML = "";
                    for (var i = 0; i < fetch.length; i++) {

                        debugger;
                        document.getElementById('mWraper').innerHTML += "<div class=\"heading\">" + fetch[i] + "</div>";

                    }
                    debugger;
                    document.getElementById('fetch').innerHTML = "Crawler Successfull! Look for the results below.";
                },
                statusCode: {
                    404: function () {
                        alert('Failed');
                    }
                }
            });
        }




        function AddWord(table) {

            var word = document.getElementById('adword').value;

            if (word == "") {
                document.getElementById('fetch').innerHTML = "Please enter valid word";
                return false;
            }
            document.getElementById('fetch').innerHTML = "Adding. Please wait...";
            $.ajax({
                url: "/api/Values/Get?status=3&table=include&word=" + word + "",
                type: 'GET',
                success: function (data) {

                    var fetch = JSON.parse(data);
                    document.getElementById('mWraper').innerHTML = "";
                    for (var i = 0; i < fetch.length; i++) {

                        document.getElementById('mWraper').innerHTML += "<div class=\"col-lg-3 text-center\"><div class=\"panel panel-default\"><div class=\"panel-body\"><div class=\"WordContainer\"><div class=\"word\">" + fetch[i].word + "</div><div class=\"removeWord\" id=\"" + fetch[i].Id + "\" onclick=\"RemoveIncWord(id)\">Remove</div></div></div></div></div>";

                    }
                    document.getElementById('adword').value = "";
                    document.getElementById('fetch').innerHTML = "Add words you want to include in crawl result";
                },
                statusCode: {
                    404: function () {
                        alert('Failed');
                    }
                }
            });
        }
        function AddWordExWord(table) {

            var word = document.getElementById('url').value;
            if (word == "") {
                document.getElementById('fetch').innerHTML = "Please enter valid word";
                return false;
            }
            document.getElementById('fetch').innerHTML = "Adding. Please wait...";
            $.ajax({
                url: "/api/Values/Get?status=3&table=exclude&word=" + word + "",
                type: 'GET',
                success: function (data) {

                    var fetch = JSON.parse(data);
                    document.getElementById('exWraper').innerHTML = "";
                    for (var i = 0; i < fetch.length; i++) {

                        document.getElementById('exWraper').innerHTML += "<div class=\"col-lg-3 text-center\"><div class=\"panel panel-default\"><div class=\"panel-body\"><div class=\"WordContainer\"><div class=\"word\">" + fetch[i].word + "</div><div class=\"removeWord\" id=\"" + fetch[i].Id + "\" onclick=\"RemoveExWord(id)\">Remove</div></div></div></div></div>";

                    }
                    document.getElementById('url').value = "";
                    document.getElementById('fetch').innerHTML = "Add words you want to remove from crawl result";
                },
                statusCode: {
                    404: function () {
                        alert('Failed');
                    }
                }
            });
        }



        function RemoveIncWord(id) {

            document.getElementById('fetch').innerHTML = "Removing Word. Please wait...";

            $.ajax({
                url: "/api/Values/Get?status=333&Id=" + id + "&table=include",
                type: 'GET',
                success: function (data) {

                    var fetch = JSON.parse(data);

                    document.getElementById('mWraper').innerHTML = "";
                    for (var i = 0; i < fetch.length; i++) {

                        document.getElementById('mWraper').innerHTML += "<div class=\"col-lg-3 text-center\"><div class=\"panel panel-default\"><div class=\"panel-body\"><div class=\"WordContainer\"><div class=\"word\">" + fetch[i].word + "</div><div class=\"removeWord\" id=\"" + fetch[i].Id + "\" onclick=\"RemoveIncWord(id)\">Remove</div></div></div></div></div>";

                    }
                    //debugger;
                    document.getElementById('fetch').innerHTML = "Add words you want to include in crawl result";
                },
                statusCode: {
                    404: function () {
                        alert('Failed');
                    }
                }
            });
        }
        function RemoveExWord(id) {

            document.getElementById('fetch').innerHTML = "Removing Word. Please wait...";
            $.ajax({
                url: "/api/Values/Get?status=333&table=exclude1&Id=" + id + "&table=include",
                type: 'GET',
                success: function (data) {

                    var fetch = JSON.parse(data);
                    document.getElementById('exWraper').innerHTML = "";
                    for (var i = 0; i < fetch.length; i++) {

                        document.getElementById('exWraper').innerHTML += "<div class=\"col-lg-3 text-center\"><div class=\"panel panel-default\"><div class=\"panel-body\"><div class=\"WordContainer\"><div class=\"word\">" + fetch[i].word + "</div><div class=\"removeWord\" id=\"" + fetch[i].Id + "\" onclick=\"RemoveExWord(id)\">Remove</div></div></div></div></div>";

                    }
                    //debugger;
                    document.getElementById('fetch').innerHTML = "Add words you want to include in crawl result";
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
                url: "/api/Values/Get?status=33&table=include",
                type: 'GET',
                success: function (data) {

                    var fetch = JSON.parse(data);
                    document.getElementById('mWraper').innerHTML = "";
                    for (var i = 0; i < fetch.length; i++) {

                        document.getElementById('mWraper').innerHTML += "<div class=\"col-lg-3 text-center\"><div class=\"panel panel-default\"><div class=\"panel-body\"><div class=\"WordContainer\"><div class=\"word\">" + fetch[i].word + "</div><div class=\"removeWord\" id=\"" + fetch[i].Id + "\" onclick=\"RemoveIncWord(id)\">Remove</div></div></div></div></div>";

                    }
                    //debugger;
                    document.getElementById('fetch').innerHTML = "Add words you want to include in crawl result";
                },
                statusCode: {
                    404: function () {
                        alert('Failed');
                    }
                }
            });
        }
        function LoadExWord() {

            document.getElementById('fetch').innerHTML = "Loading. Please wait...";
            $.ajax({
                url: "/api/Values/Get?status=33&table=exclude",
                type: 'GET',
                success: function (data) {

                    var fetch = JSON.parse(data);
                    document.getElementById('exWraper').innerHTML = "";
                    for (var i = 0; i < fetch.length; i++) {

                        document.getElementById('exWraper').innerHTML += "<div class=\"col-lg-3 text-center\"><div class=\"panel panel-default\"><div class=\"panel-body\"><div class=\"WordContainer\"><div class=\"word\">" + fetch[i].word + "</div><div class=\"removeWord\" id=\"" + fetch[i].Id + "\" onclick=\"RemoveExWord(id)\">Remove</div></div></div></div></div>";

                    }
                    //debugger;
                    document.getElementById('fetch').innerHTML = "Add words you want to remove from crawl result";
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
                            <input type="text" name="Add Words" id="url" class="form-control input-sm" placeholder="Remove Words.." required />
                        </li>
                        <li class="active">
                            <button type="button" id="urlSubmit1"  class="btn btn-sm btn-primary" onclick="AddWordExWord(2)" >Add Word</button>
                        </li>
                        
                    </ol>
                </div>
            </div>
            <div class="row" id="exWraper">

                    

                    

            </div>
    </asp:Content>

