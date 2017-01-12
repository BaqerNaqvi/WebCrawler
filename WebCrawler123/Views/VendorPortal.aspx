<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="VendorPortal.aspx.cs" MasterPageFile="~/MasterPages/VendorDashboard.master" Inherits="WebCrawler.Views.test" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    
    <script type="text/javascript" src="http://maps.google.com/maps/api/js?sensor=false"></script>
    <script src="../Scripts/map/gmaps.js"></script>
    <script src="../Scripts/map/jquery.min.js"></script>
    <script src="../Scripts/map/bootstrap.min.js"></script>
    <script src="../Scripts/map/platform.js"></script>
    <script async defer src="https://maps.googleapis.com/maps/api/js?key=AIzaSyC67qOLCwRi1_6Z8g1zKA6OQkJekYIBDz8&&libraries=places&sensor=false&callback=initMap"></script>
    
 
    <script type="text/javascript">

        if (sessionStorage.getItem("Id") == "" || sessionStorage.getItem("Id") == null) {
            window.location = "VendorLogin.aspx";
        }
        var coordInfo;
        var notesInfo;
        var mapInfo;
        var addressInfo = "NA";


        $(document).ready(function ($) {
            // Get the modal form 
            LoadMarkersFromDataBase(sessionStorage.getItem("Id"));
            var modal = document.getElementById('myModal');
            var span = document.getElementsByClassName("cancelNoteBtn")[0];
            span.onclick = function () {
                modal.style.display = "none";
            }
            window.onclick = function (event) {
                if (event.target == modal) {
                    modal.style.display = "none";
                }
            }


            //Get the modal Detail
            var modal1 = document.getElementById('myModal1');
            
            window.onclick = function (event) {
                if (event.target == modal1) {
                    modal1.style.display = "none";
                }
            }

            document.getElementById('unlockCode').innerHTML = sessionStorage.getItem("unlock_code");

            LoadVendorUsersCount(sessionStorage.getItem("Id"));
        });

        var xhttp = new XMLHttpRequest();
        var map;
        function initMap() {
            map = new google.maps.Map(document.getElementById('map'), {
                center: { lat: 31.498579759659158, lng: 74.36851501464844 },
                zoom: 15

                

            });
            google.maps.event.addListener(map, 'click', function (event) {


                getAddress(event.latLng);

                coordInfo = event.latLng;
                mapInfo = map;

                ShowModal();


            });
           
            initAutocomplete(map);
            var infoWindow = new google.maps.InfoWindow({ map: map });
            // geolocation
            if (navigator.geolocation) {
                //alert();
                navigator.geolocation.getCurrentPosition(function (position) {
                    var pos = {
                        lat: position.coords.latitude,
                        lng: position.coords.longitude
                    };

                    infoWindow.setPosition(pos);
                    infoWindow.setContent("Current Location");
                    map.setCenter(pos);
                }, function () {
                    handleLocationError(true, infoWindow, map.getCenter());
                });
            } else {
                // Browser doesn't support Geolocation
                handleLocationError(false, infoWindow, map.getCenter());
            }


            function handleLocationError(browserHasGeolocation, infoWindow, pos) {
                infoWindow.setPosition(pos);
                infoWindow.setContent(browserHasGeolocation ?
                        'Error: You need SSL request for location.' :
                        'Error: You need SSL request for location.');

            }


            
        }
        //var myLatlng = new google.maps.LatLng(value.lat, value.lon);
        function getAddress(latLng) {
            var geocoder = new google.maps.Geocoder();

            geocoder.geocode({ 'latLng': latLng },
              function (results, status) {
                  if (status == google.maps.GeocoderStatus.OK) {
                      if (results[0]) {
                          //document.getElementById("address").value = results[0].formatted_address;
                          addressInfo = results[0].formatted_address;
                      }
                      else {
                          ///document.getElementById("address").value = "No results";
                      }
                  }
                  else {
                      //document.getElementById("address").value = status;
                  }
              });

            //return addressOnClick;
        }

        function addMarker(location, map, note) {

            //debugger;
            var marker = new google.maps.Marker({
                position: location,
                label: "N",
                map: map,
                draggable: true,
                editable: true

            });

            attachNote(marker, note);


        }

        function attachNote(marker, note) {

            //debugger;
            //var infowindow = new google.maps.InfoWindow({
            //    content: note
            //});

            marker.addListener('click', function () {

                GetMarkerFromDataBase(marker.position.lat(), marker.position.lng());
            });

        }

        function ShowModal() {
            var modal = document.getElementById('myModal');
            modal.style.display = "block";
        }

        function HideModal() {

            //debugger;
            var modal = document.getElementById('myModal');
            modal.style.display = "none";
        }

        function ShowModalDetail() {

            var modal1 = document.getElementById('myModal1');
            modal1.style.display = "block";
        }

        function HideModalDetail() {

            //debugger;
            var modal = document.getElementById('myModal1');
            modal.style.display = "none";
        }

        function AddGpsNoteToDatabase() {
            var noteTitle = document.getElementById('noteTitle').value;
            var noteDes = document.getElementById('noteDes').value;
            document.getElementById('notesHead').innerHTML = "Create Note...";

            $.ajax({
                url: '/api/Location/AddVendorGpsNote',
                type: 'POST',
                data: {
                    address: addressInfo,
                    color: "",
                    sender_name: sessionStorage.getItem("name"),
                    sender_userId: sessionStorage.getItem("Id"),
                    lati: coordInfo.lat(),
                    longi: coordInfo.lng(),
                    title: noteTitle,
                    description: noteDes,
                    deliveryStatus: "0"


                },
                success: function (data) {
                    //debugger;

                    document.getElementById('notesHead').innerHTML = "Successfully Created!";
                    addMarker(coordInfo, mapInfo, noteTitle);
                    HideModal();
                    window.location = "VendorPortal.aspx";

                },
                statusCode: {
                    404: function () {
                        alert('Failed');;
                    }
                }
            });



        }

        function GetMarkerFromDataBase(lati, longi) {

            ShowModalDetail();
            document.getElementById('desLoading').innerHTML = "<div class=\"noteRow\" id=\"nTitle\">Fetching Note...</div>";
            $.ajax({
                url: '/api/Location/FetchVendorNotesByCoord',
                type: 'POST',
                data: {
                    vendorId: sessionStorage.getItem("Id"),
                    lati: lati,
                    longi: longi
                },
                success: function (data) {

                    //debugger;
                    document.getElementById('desLoading').innerHTML = "<div class=\"noteRow\" id=\"nTitle\">" + data.result[0].title + "</div><div class=\"noteRow\" id=\"nDes\">" + data.result[0].description + "</div><div class=\"noteRow\"><div class=\"addNoteBtn\" onclick=\"DeleteMarkerFromDataBase(" + data.result[0].Id + "," + data.result[0].lati + "," + data.result[0].longi + ")\">Delete Note</div><div class=\"cancelNoteBtn1\" onclick=\"HideModalDetail()\">Close</div></div>";

                    document.getElementById('notesHead1').innerHTML = "Notes Detail";

                },
                statusCode: {
                    404: function () {
                        alert('Failed');;
                    }
                }
            });
        }

        function DeleteMarkerFromDataBase(Id, lati, longi) {


            var retVal = confirm("Do you want to Delete this Note ?");
            if (retVal == true) {
                document.getElementById('desLoading').innerHTML = "<div class=\"noteRow\" id=\"nTitle\">Deleting Note...</div>";
                $.ajax({
                    url: '/api/Location/DeleteVendorNotesByCoord',
                    type: 'POST',
                    data: {
                        vendorId: Id,
                        lati: lati,
                        longi: longi
                    },
                    success: function (data) {
                        HideModalDetail();
                        window.location = "VendorPortal.aspx";
                    },
                    statusCode: {
                        404: function () {
                            alert('Failed');;
                        }
                    }
                });
                return true;
            }
            else {
                return false;
            }
            
        }

        function LoadMarkersFromDataBase(vendorId) {
            document.getElementById('vendorTable').innerHTML = "<tr>Loading Notes</tr>";
            
            $.ajax({
                url: '/api/Location/FetchVendorNotesById',
                type: 'POST',
                data: {
                    userId: vendorId
                },
                success: function (data) {

                    //debugger;

                    document.getElementById('totalNotes').innerHTML = data.result.length;
                    for (var i = 0; i < data.result.length; i++) {
                        var myLatlng = new google.maps.LatLng(data.result[i].lati, data.result[i].longi);
                        addMarker(myLatlng, map, data.result[i].title);
                        

                        
                    }

                    if (data.result.length > 0) {
                        var status = "<span class=\"dStatusGreen\">Delivered!</span>";
                        document.getElementById('vendorTable').innerHTML = "";
                        for (var i = 0; i < data.result.length; i++) {

                            if (data.result[i].deliveryStatus == "0")
                            {
                                status = "<span class=\"dStatusRed\">Pending!</span>";
                            }
                            document.getElementById('vendorTable').innerHTML += "<tr><td>" + data.result[i].title + "</td><td>" + data.result[i].description + "</td><td>" + status + "</td><td>" + data.result[i].address + "</td><td><div class=\"btn btn-danger btn-primary myButton\" id=\"" + data.result[i].Id + "\" onclick=\"DeleteMarkerFromDataBase(" + data.result[i].Id + "," + data.result[i].lati + "," + data.result[i].longi + ")\">Delete</div></td></tr>";

                        }
                    }
                    else {
                        document.getElementById('vendorTable').innerHTML = "<tr>No Result</tr>";
                    }
                    //debugger;

                },
                statusCode: {
                    404: function () {
                        alert('Failed');;
                    }
                }
            });
        }
        
        function LoadVendorUsersCount(vendorId) {
            document.getElementById('totalUsers').innerHTML = "00";
            $.ajax({
                url: '/api/User/VendorUsers',
                type: 'POST',
                data: {
                    userId: vendorId,
                    unlock_code: sessionStorage.getItem("unlock_code")
                },
                success: function (data) {

                    debugger;

                    document.getElementById('totalUsers').innerHTML = data.contactList.length;
                    

                },
                statusCode: {
                    404: function () {
                        alert('Failed');;
                    }
                }
            });
        }

        function SendAllNotes() {
            debugger;
            document.getElementById('vendorTable').innerHTML = "<tr>Loading Notes</tr>";
            $.ajax({
                url: '/api/Location/SendAllVendorNotes',
                type: 'POST',
                data: {
                    vendorId: sessionStorage.getItem("Id"),
                    unlock_code: sessionStorage.getItem("unlock_code")
                },
                success: function (data) {

                    for (var i = 0; i < data.result.length; i++) {
                        var myLatlng = new google.maps.LatLng(data.result[i].lati, data.result[i].longi);
                        addMarker(myLatlng, map, data.result[i].title);



                    }

                    if (data.result.length > 0) {
                        var status = "<span class=\"dStatusGreen\">Delivered!</span>";
                        document.getElementById('vendorTable').innerHTML = "";
                        for (var i = 0; i < data.result.length; i++) {

                            if (data.result[i].deliveryStatus == "0") {
                                status = "<span class=\"dStatusRed\">Pending!</span>";
                            }
                            document.getElementById('vendorTable').innerHTML += "<tr><td>" + data.result[i].title + "</td><td>" + data.result[i].description + "</td><td>" + status + "</td><td>" + data.result[i].address + "</td><td><div class=\"btn btn-danger btn-primary myButton\" id=\"" + data.result[i].Id + "\" onclick=\"DeleteMarkerFromDataBase(" + data.result[0].Id + "," + data.result[0].lati + "," + data.result[0].longi + ")\">Delete</div></td></tr>";

                        }
                    }
                    else {
                        document.getElementById('vendorTable').innerHTML = "<tr>No Result</tr>";
                    }
                    //debugger;

                },
                statusCode: {
                    404: function () {
                        alert('Failed');;
                    }
                }
            });
        }


</script>

<script>
    // This example adds a search box to a map, using the Google Place Autocomplete
    // feature. People can enter geographical searches. The search box will return a
    // pick list containing a mix of places and predicted search terms.

    // This example requires the Places library. Include the libraries=places
    // parameter when you first load the API. For example:

    function initAutocomplete(MyMap) {
        var map = MyMap;

        // Create the search box and link it to the UI element.
        var input = document.getElementById('pac-input');
        var searchBox = new google.maps.places.SearchBox(input);
        map.controls[google.maps.ControlPosition.TOP_LEFT].push(input);

        // Bias the SearchBox results towards current map's viewport.
        map.addListener('bounds_changed', function () {
            searchBox.setBounds(map.getBounds());
        });

        var markers = [];
        // Listen for the event fired when the user selects a prediction and retrieve
        // more details for that place.
        searchBox.addListener('places_changed', function () {
            var places = searchBox.getPlaces();

            if (places.length == 0) {
                return;
            }

            // Clear out the old markers.
            markers.forEach(function (marker) {
                marker.setMap(null);
            });
            markers = [];

            // For each place, get the icon, name and location.
            var bounds = new google.maps.LatLngBounds();
            places.forEach(function (place) {
                if (!place.geometry) {
                    console.log("Returned place contains no geometry");
                    return;
                }
                var icon = {
                    url: place.icon,
                    size: new google.maps.Size(71, 71),
                    origin: new google.maps.Point(0, 0),
                    anchor: new google.maps.Point(17, 34),
                    scaledSize: new google.maps.Size(25, 25)
                };

                // Create a marker for each place.
                markers.push(new google.maps.Marker({
                    map: map,
                    icon: icon,
                    title: place.name,
                    position: place.geometry.location
                }));

                if (place.geometry.viewport) {
                    // Only geocodes have viewport.
                    bounds.union(place.geometry.viewport);
                } else {
                    bounds.extend(place.geometry.location);
                }
            });
            map.fitBounds(bounds);
        });
    }

    </script>
<style>
        html,
        body,
        #map {
            display: block;
            width: 100%;
            height: 100%;
        }

        #map {
            background: #58B;
            width:100%;
            height:400px;
        }


        /* The Modal (background) */
        .modal {
            display: none; /* Hidden by default */
            position: fixed; /* Stay in place */
            z-index: 1; /* Sit on top */
            padding-top: 100px; /* Location of the box */
            left: 0;
            top: 0;
            width: 100%; /* Full width */
            height: 100%; /* Full height */
            overflow: auto; /* Enable scroll if needed */
            background-color: rgb(0,0,0); /* Fallback color */
            background-color: rgba(0,0,0,0.4); /* Black w/ opacity */
        }
        .modal1 {
            display: none; /* Hidden by default */
            position: fixed; /* Stay in place */
            z-index: 1; /* Sit on top */
            padding-top: 100px; /* Location of the box */
            left: 0;
            top: 0;
            width: 100%; /* Full width */
            height: 100%; /* Full height */
            overflow: auto; /* Enable scroll if needed */
            background-color: rgb(0,0,0); /* Fallback color */
            background-color: rgba(0,0,0,0.4); /* Black w/ opacity */
        }

        /* Modal Content */
        .modal-content {
            background-color: #fffeab;
            margin: auto;
            border: 1px solid #888;
            width: 300px;
            height:300px;
        }
        .modal-content1 {
            background-color: #fffeab;
            margin: auto;
            border: 1px solid #888;
            width: 300px;
            display: table;
            padding-bottom: 10px;
        }
        /* The Close Button */
        .close {
            background-color:#d6d591;
            color: #000;
            float: right;
            width:100%;
            padding:5px 0px;
            font-size: 12px;
            font-family:Arial;
            font-weight: bold;
            text-align:center;
        }

        .close:hover,
        .close:focus {
            color: #000;
            text-decoration: none;
            cursor: pointer;
        }

        .noteTitle {
            width: 272px;
            height: 30px;
            margin-left: 10px;
            font-size: 12px;
            color: black;
            padding-left: 5px;
            margin-top: 26px;
            background-color: #e8e79f;
            outline: none;
            border: none;
        }

        #nTitle {
            width: 272px;
            margin-left: 10px;
            font-size: 13px;
            color: black;
            padding-left: 5px;
            margin-top: 26px;
            font-weight:bold;
            font-family:Arial;
        }

        #nDes {
            width: 272px;
            margin-left: 10px;
            font-size: 12px;
            color: black;
            padding-left: 5px;
            margin-top: 26px;
            font-family:Arial;
        }

        .noteDes {
            width: 272px;
            height: 90px;
            margin-left: 10px;
            font-size: 12px;
            color: black;
            padding-left: 5px;
            margin-top: 26px;
            background-color: #e8e79f;
            outline: none;
            border: none;
        }
        .noteRow {
            width:100%;
            float:left;
            margin-top:20px;
        }
        .addNoteBtn {
            background-color:#ccc;
            padding:5px 10px;
            float:right;
            font-size:12px;
            cursor:pointer;
            color:black;
            font-family:Arial;
            margin-right:10px;
        }

        .cancelNoteBtn {
            background-color:#ccc;
            padding:5px 10px;
            float:right;
            font-size:12px;
            cursor:pointer;
            color:black;
            font-family:Arial;
            margin-right:10px;
        }
        .cancelNoteBtn1 {
            background-color:#ccc;
            padding:5px 10px;
            float:right;
            font-size:12px;
            cursor:pointer;
            color:black;
            font-family:Arial;
            margin-right:10px;
        }

        .addNoteBtn:hover {
            background-color:#d6d591;
        } 

        .cancelNoteBtn:hover {
            background-color:#d6d591;
        }
        .cancelNoteBtn1:hover {
            background-color:#d6d591;
        }

        #page-wrapper {
            min-height:700px;
        }

        .dStatusGreen {
            color: green !important;
        }

        .dStatusRed {
            color:red !important;
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

      #pac-input {
        background-color: #fff;
        font-family: Roboto;
        font-size: 15px;
        font-weight: 300;
        margin-left: 12px;
        padding: 0 11px 0 13px;
        text-overflow: ellipsis;
        width: 300px;
      }

      #pac-input:focus {
        border-color: #4d90fe;
      }

      .pac-container {
        font-family: Roboto;
      }

      #type-selector {
        color: #fff;
        background-color: #4d90fe;
        padding: 5px 11px 0px 11px;
      }

      #type-selector label {
        font-family: Roboto;
        font-size: 13px;
        font-weight: 300;
      }
      #target {
        width: 345px;
      }

    .panel-primary > .panel-heading {
            height: 90px !important;
    }

    .huge {
        font-size: 200% !important;
    }
     .vendorDropdown {
            width: 100%;
            height: 31px;
            border-color: #ccc;
            border-radius: 4px;
        }

      #VendorPortal {
             background-color: #1e3040 !important;
        }
    </style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">

    <!-- Page Heading -->
    <div class="row">
                <div class="col-lg-12">
                    <h1 class="page-header">
                        Vendor <small>Statistics Overview</small>
                    </h1>
                    <ol class="breadcrumb">
                        <li class="active">
                            <i class="fa fa-dashboard"></i> Select Location to Add Notes
                        </li>
                    </ol>
                </div>
            </div>

    <input id="pac-input" class="controls" type="text" placeholder="Search Box" />
    <div id="map"></div>

<div class="row">
        <div class="col-lg-12">
            <h1 class="page-header">
                <small>Stats</small>
            </h1>
        </div>
    </div>

    
    <div class="row">
                <div class="col-lg-4 col-md-6">
                    <div class="panel panel-primary">
                        <div class="panel-heading">
                            <div class="row">
                                <div class="col-xs-3">
                                    <i class="fa fa-unlock-alt fa-5x"></i>
                                </div>
                                <div class="col-xs-9 text-right">
                                    <div class="huge" id="unlockCode">0</div>
                                    <div>Unlock Code</div>
                                </div>
                            </div>
                        </div>
                        <a href="#">
                            <div class="panel-footer">
                                <span class="pull-left">View Details</span>
                                <span class="pull-right"><i class="fa fa-arrow-circle-right"></i></span>
                                <div class="clearfix"></div>
                            </div>
                        </a>
                    </div>
                </div>
                <div class="col-lg-4 col-md-6">
                    <div class="panel panel-primary">
                        <div class="panel-heading">
                            <div class="row">
                                <div class="col-xs-3">
                                    <i class="fa fa-sticky-note fa-5x"></i>
                                </div>
                                <div class="col-xs-9 text-right">
                                    <div class="huge" id="totalNotes">10</div>
                                    <div>Vendor Notes</div>
                                </div>
                            </div>
                        </div>
                        <a href="#">
                            <div class="panel-footer">
                                <span class="pull-left">View Details</span>
                                <span class="pull-right"><i class="fa fa-arrow-circle-right"></i></span>
                                <div class="clearfix"></div>
                            </div>
                        </a>
                    </div>
                </div>
                <div class="col-lg-4 col-md-6">
                    <div class="panel panel-primary">
                        <div class="panel-heading">
                            <div class="row">
                                <div class="col-xs-3">
                                    <i class="fa fa-male fa-5x"></i>
                                </div>
                                <div class="col-xs-9 text-right">
                                    <div class="huge" id="totalUsers">90</div>
                                    <div>Total Users</div>
                                </div>
                            </div>
                        </div>
                        <a href="#">
                            <div class="panel-footer">
                                <span class="pull-left">View Details</span>
                                <span class="pull-right"><i class="fa fa-arrow-circle-right"></i></span>
                                <div class="clearfix"></div>
                            </div>
                        </a>
                    </div>
                </div>
            </div>

    <div class="row">
        <div class="col-lg-12">
            <h1 class="page-header">
                <small>Recent Notes</small>
            </h1>
            <ol class="breadcrumb" style="text-align:right;">
                <li class="active">
                    <div class="btn btn-sm btn-primary" onclick="SendAllNotes()">Send All</div>
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
                                        <th>Title</th>
                                        <th>Description</th>
                                        <th>Delivery Status</th>
                                        <th>Address</th>
                                        <th>Action</th>
                                    </tr>
                                </thead>
                                <h2 id="loadingText"></h2>
                                <tbody id="vendorTable">
                                </tbody>
                            </table>
                        </div>
                    
                </div>
    </div>

    <div id="myModal" class="modal">

        <!-- Modal content -->
        <div class="modal-content">
        <div class="close" id="notesHead">Create Note On Map</div>
        <input type="text" class="noteTitle" id="noteTitle" placeholder="Enter Note Title" />
        <textarea class="noteDes" id="noteDes" placeholder="Enter Description"></textarea>
        <div class="noteRow">
            <div class="addNoteBtn" onclick="AddGpsNoteToDatabase()">Add Note</div>
            <div class="cancelNoteBtn">Cancel</div>
        </div>

        </div>

    </div>


    <!-- Modal content -->
    <div id="myModal1" class="modal1">
        <div class="modal-content1">
            <div class="close" id="notesHead1">Notes Detail</div>
            <div class="noteRow" id="desLoading">
                        
            </div>
                    
        </div>
    </div>
</asp:Content>


