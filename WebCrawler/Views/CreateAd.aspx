<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CreateAd.aspx.cs" MasterPageFile="~/MasterPages/AdDashboard.master" Inherits="WebCrawler.Views.CreateAd" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
     

    <script src="../Scripts/map/gmaps.js"></script>
    <script src="../Scripts/map/jquery.min.js"></script>
    <script src="../Scripts/map/bootstrap.min.js"></script>
    <script src="../Scripts/map/platform.js"></script>
    <script async defer src="https://maps.googleapis.com/maps/api/js?key=AIzaSyC67qOLCwRi1_6Z8g1zKA6OQkJekYIBDz8&&libraries=places&sensor=false&callback=initMap"></script>
    

    <script type="text/javascript">

        if (sessionStorage.getItem("Id") == "" || sessionStorage.getItem("Id") == null) {
            window.location = "AdLogin.aspx";
        }
        var sponsorFlag = false;
        var couponFlag = false;
        var isCustom = 1;
        var isVideo = false;
        var isImg = false;

        var xhttp = new XMLHttpRequest();
        var coordInfo;
        var mapInfo, addressInfo="", place_id="";
        var map;
        function initMap() {
            map = new google.maps.Map(document.getElementById('map'), {
                center: { lat: 31.498579759659158, lng: 74.36851501464844 },
                zoom: 15



            });
            google.maps.event.addListener(map, 'click', function (event) {


               var dfsdf= getAddress(event.latLng);
                debugger;
                coordInfo = event.latLng;
                mapInfo = map;

                addMarker(coordInfo, mapInfo);


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
        //var myLatlng = new google.maps.LatLng(value.lat, value.lon);
        function getAddress(latLng) {
            var geocoder = new google.maps.Geocoder();

            geocoder.geocode({ 'latLng': latLng },
              function (results, status) {
                  if (status == google.maps.GeocoderStatus.OK) {
                      if (results[0]) {
                          //document.getElementById("address").value = results[0].formatted_address;
                          addressInfo = results[0].formatted_address;
                          place_id = results[0].place_id;
                          var sf = latLng;
                          debugger;
                          console.log("Place id:" + results[0].place_id);
                      }
                      else {
                          document.getElementById("address").value = "No results";
                      }
                  }
                  else {
                     document.getElementById("address").value = status;
                  }
              });

            //return addressOnClick;
        }

        function addMarker(location, map) {

            //debugger;
            var marker = new google.maps.Marker({
                position: location,
                label: "N",
                map: map,
                draggable: true,
                editable: true

            });



        }

        $(document).ready(function ($) {
            
          

            LoadInterests();
            LoadAdTypes();
            LoadBidTypes();

            $('#pac-input').keydown(function (e) {
                if (e.which === 13) {
                     return false;
                }
            });

            $("#adTypeDdl").change(function (e) {

                if (e.target.selectedOptions[0].label == "Sponsor")
                {
                    document.getElementById("sponsorSection").style.display = "block";
                    document.getElementById("couponSection").style.display = "none";
                    sponsorFlag = true;
                    couponFlag = false;
                    
                }
                else {
                    document.getElementById("sponsorSection").style.display = "none";
                    document.getElementById("couponSection").style.display = "none";
                    sponsorFlag = false;
                    couponFlag = false;
                }

                if (e.target.selectedOptions[0].label == "Coupon") {
                    $("#website-url-text").text("Coupon URL:");
                } else {
                    $("#website-url-text").text("Website URL:");
                }
                
            });


            //$('input:radio[name=sponsorType]').change(function () {
                
            //    if (this.value == 'spVideoBtn') {
            //        document.getElementById("spImage").style.display = "none";
            //        document.getElementById("spVideo").style.display = "block";
            //        adFlag = true;
            //    }
            //    else {
            //        document.getElementById("spImage").style.display = "block";
            //        document.getElementById("spVideo").style.display = "none";
            //        adFlag = false;
            //    }
            //});

            $('#img-checkbox').change(function () {
                isVideo = false;
                $("#video-block").css('display', 'none');

                if ($(this).is(":checked")) {
                    isImg = true;
                    $("#img-block").css('display', 'block');
                } else {
                    isImg = false;
                    $("#img-block").css('display', 'none');
                }
            });
            $('#video-checkbox').change(function () {
                isImg = false;
                $("#img-block").css('display', 'none');

                if ($(this).is(":checked")) {
                    isVideo = true;
                    $("#video-block").css('display', 'block');
                } 
            });


            $('#spFacts').keyup(function () {

                var the_input_len;
                var the_input;
                the_input = $('#spFacts').val();
                the_input_len = $('#spFacts').val().length;
                the_input_len = 155 - the_input_len;

                if (the_input_len >= 0) {
                    $('#charcounter').html(the_input_len);  //code
                };
                if (the_input_len < 0) {
                    alert('Sorry, the text is too long. The maximum character length is 155.');
                    $('#spFacts').val($('#spFacts').val().slice(0, 155));
                    return false;
                };
            });
        });





        function LoadInterests() {


            document.getElementById('interestCategory').innerHTML += "<option value=\"1\">Loading...</option>";
            $.ajax({
                url: "/api/User/AllAppCategory",
                type: 'POST',
                data: {
                },
                success: function (vendorInfo) {

                    document.getElementById('interestCategory').innerHTML = "<option value=\"1\">Select Category</option>";
                    for (var i = 0; i < vendorInfo.length; i++) {

                        document.getElementById('interestCategory').innerHTML += "<option value=\"" + vendorInfo[i].Id + "\">" + vendorInfo[i].categoryName + "</option>";

                    }
                    LoadSubCategories();

                },
                statusCode: {
                    404: function () {
                        alert('Failed');
                        return false;
                    }
                }
            });
        }

        function LoadSubCategories() {

            var catId = document.getElementById("interestCategory").value;
            document.getElementById('adInterest').innerHTML = "<option value=\"1\">Loading...</option>";
            $.ajax({
                url: "/api/User/AllAppSubCategory",
                type: 'POST',
                data: {
                    Id: catId
                },
                success: function (vendorInfo) {

                    document.getElementById('adInterest').innerHTML = "<option value=\"70\">Select Interest</option>";
                    //debugger;
                    for (var i = 0; i < vendorInfo.length; i++) {

                        document.getElementById('adInterest').innerHTML += "<option value=\"" + vendorInfo[i].Id + "\">" + vendorInfo[i].subCategoryName + "</option>";

                    }
                },
                statusCode: {
                    404: function () {
                        alert('Failed');
                        return false;
                    }
                }
            });
        }

        function LoadAdTypes() {


            document.getElementById('adTypeDdl').innerHTML += "<option value=\"1\">Loading...</option>";
            $.ajax({
                url: "/api/VendorDashboard/GetAllAdTypes",
                type: 'POST',
                data: {
                },
                success: function (info) {

                    document.getElementById('adTypeDdl').innerHTML = "<option value=\"1\">Select Ad Type</option>";

                    for (var i = 0; i < info.length; i++) {

                        document.getElementById('adTypeDdl').innerHTML += "<option value=\"" + info[i].Id + "\">" + info[i].title + "</option>";

                    }

                },
                statusCode: {
                    404: function () {
                        alert('Failed');
                        return false;
                    }
                }
            });
        }

        function LoadBidTypes() {


            document.getElementById('bidTypeContainer').innerHTML = "Loading Bid Types...";
            $.ajax({
                url: "/api/VendorDashboard/GetAllBidTypes",
                type: 'POST',
                data: {
                },
                success: function (info) {
                    document.getElementById('bidTypeContainer').innerHTML = "";

                    for (var i = 0; i < info.length; i++) {

                        document.getElementById('bidTypeContainer').innerHTML += "<input type=\"radio\" name=\"bidType\" value=\"" + info[i].Id + "\" checked=\"checked\" /><span>" + info[i].title + "</span><br/>";

                    }

                },
                statusCode: {
                    404: function () {
                        alert('Failed');
                        return false;
                    }
                }
            });
        }


        function ValidateAd() {

            var postBack = true;

            var e = document.getElementById("adTypeDdl");
            var adType = e.options[e.selectedIndex].value;
            var dailyBudget = document.getElementById("dailyBudget").value;

            //debugger;

            var bidType = $('input[name=bidType]:checked').val();

            var e = document.getElementById("adInterest");
            var adInterest = e.options[e.selectedIndex].value;

            var e = document.getElementById("interestCategory");
            var interestCategory = e.options[e.selectedIndex].value;

            var adCustomInterest = document.getElementById("adCustomInterest").value;

            var spFacts = document.getElementById("spFacts").value;

            var cWebUrl = document.getElementById("cWebUrl").value;
            var cPhone = document.getElementById("cPhone").value;

            var adTitle = document.getElementById("adTitle").value;
           
            if (addressInfo == "" || addressInfo == undefined) {
                alert("Select Ad-Location");
                postBack = false;
                return false;
            }
            var locationName = addressInfo;

            var lati = coordInfo.lat();
            var longi = coordInfo.lng();


            var costPerAction = "";
            var costPerConversion = "";
            var selctedCost = $("#bidCost").val();
            var bidTypeOption = $('input[name=bidType]:checked').val();

            if (selctedCost === "" || selctedCost === undefined) {
                alert("Please Select Bid Cost!");
                postBack = false;
                return false;
            }

            if (bidTypeOption === "1") {
                costPerAction = selctedCost;
            } else if (bidTypeOption === "2") {
                costPerConversion = selctedCost;
            }
            
            
            if (cWebUrl == "") {
                alert("Please enter web Url");
                postBack = false;
                return false;
            }
            if (cPhone == "") {
                alert("Please enter Phone");
                postBack = false;
                return false;
            }





            if (dailyBudget == "") {
                alert("Please enter daily budget");
                postBack = false;
                return false;
            }
            if (adCustomInterest == "") {
                adCustomInterest = interestCategory;
                isCustom = 0;
            }
            else {
                isCustom = 1;
            }

            if(sponsorFlag)
            {
                if (spFacts == "") {

                    spFacts = ""
                }
                var files = $("#photoUrl").get(0).files;
                if ((isImg) && files.length === 0) {
                    alert("Please upload Photo.");
                    return false;
                }
                var urlVid=$("#video-url-tb").val();
                if ((isVideo) && urlVid==="") {
                    alert("Please provide Video URL.");
                    return false;
                }

            }

          

            if(postBack)
            {
                CreateAd(place_id, adType, bidTypeOption, dailyBudget, adInterest, adCustomInterest, interestCategory, spFacts, cWebUrl, cPhone, isCustom, adTitle, lati, longi,locationName, selctedCost);
            }
            return false;
        }
        function CreateAd(placeId, adType, bidTypeOption, dailyBudget, adInterest, adCustomInterest, interestCategory, spFacts, cWebUrl, cPhone, isCustom, adTitle, lati, longi, locationName, selctedCost) {





            document.getElementById('adBtn').innerHTML = "Creating Ad please wait..";
            $.ajax({
                url: "/api/VendorDashboard/CreateAd",
                type: 'POST',
                data: {
                    vendorId: sessionStorage.getItem("Id"),
                    adTypeId: adType,
                    bidTypeId: bidTypeOption,
                    dailyBudget: dailyBudget,
                    interestId: adInterest,
                    categoryId: interestCategory,
                    sponsorFacts: spFacts,
                    sponsorWebsite: cWebUrl,
                    sponsorPhone: cPhone,
                    sponorLogo: '',
                    isCustom: isCustom,
                    customInterest:adCustomInterest,
                    isVisible: 0,
                    couponUrl: cWebUrl,
                    adTitle: adTitle,
                    lati: lati,
                    longi: longi,
                    locationName: locationName+"|"+placeId,
                    costPerAction: selctedCost,
                    costPerConversion: selctedCost

                },
                success: function (data) {

                    //alert("done");
                    //debugger;
                    if (sponsorFlag && (isImg || isVideo)) {
                        UploadFile(data);
                    }
                    else {
                        window.location.href = "AdDashboard.aspx";
                    }

                },
                statusCode: {
                    404: function () {
                        alert('Failed');
                        return false;
                    }
                }
            });
        }

        function UploadFile(adId) {

            var data = new FormData();
            var files = $("#photoUrl").get(0).files;
            data.append("UploadedImage", files[0]);
            var uniquekey = {
                adId: adId
            };
            data.append("adId", adId);
            data.append("videoUrl", $("#video-url-tb").val());
            data.append("isImg", isImg);
            data.append("isVideo", isVideo);
            // Make Ajax request with the contentType = false, and procesDate = false
            var ajaxRequest = $.ajax({
                type: "POST",
                url: "/api/VendorDashboard/uploadfile",
                contentType: false,
                processData: false,
                data: data
            });

            ajaxRequest.done(function (xhr, textStatus) {
                window.location.href = "AdDashboard.aspx";

            });
        }

        function cancelAd() {
            window.location = "AdDashboard.aspx";
        }
    </script>


    </asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    
    <style>
         #map {
            display: block;
            width: 100%;
            height: 500px;
        }

        #map {
            background: #58B;
            width:100%;
            height:400px;
        }
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
        .page-header {
            padding-bottom: 9px;
            margin: 20px 0 20px;
            border-bottom: 1px solid #cac4c4;
            color:#808080;
                font-size: 15px;
        }
        .page-headerOr {
            padding-bottom: 9px;
            margin: 20px 0 0px;
            color: #000;
            text-align: center;
        }
        .vendorDropdown {
            width: 100%;
            height: 31px;
            border-color: #ccc;
            border-radius: 4px;
        }
        #spImage, #sponsorSection, #couponSection {
            /*display:none;*/
        }
        .adHeading {
            width:100%;
            font-size: 24px;
        }

        #bidTypeContainer span {
            margin-left: 10px;
        }

        #CreateAd {
             background-color: #1e3040 !important;
        }
 </style>


    <div class="MainWrapper">

        <div class="content-header">
            <h2>
                Create Your Ad
            </h2>
        </div>


        <div class="row">
            <div class="col-xs-12 col-sm-12 col-md-6">
                <div class="box box-primary">
                    <div class="box-header with-border">
                        <h3 class="box-title">Budget Details</h3>
                    </div>
                    <div class="row">
                        <div class="controll-group col-xs-12 col-sm-12 col-md-12">
                            <label for="dailyBudget">Ad Title</label>
                            <input type="text" name="type" id="adTitle" class="form-control input-sm" placeholder="Add Ad Title" required />
                        </div>
                    </div>
                    <div class="row">
                        <div class="controll-group col-xs-12 col-sm-12 col-md-6">
                            <label for="adTypeDdl">Select Ad Type</label>
                            <select id="adTypeDdl" class="vendorDropdown">
                                <option value="1">Blinking Interest</option> 
                                <option value="2">Colorful Interest</option>
                                <option value="3">Coupon</option>
                                <option value="4">Event</option>
                                <option value="5">Sponsor</option>
                            </select>
                        </div>
                        <div class="controll-group col-xs-12 col-sm-12 col-md-6">
                            <label for="dailyBudget">Enter Daily Budget</label>
                            <input type="text" name="type" id="dailyBudget" class="form-control input-sm" placeholder="Add Daily Budget" required />
                        </div>
                    </div>
                    <div class="row">
                        <div class="controll-group col-xs-12 col-sm-12 col-md-6">
                            <label for="type" id="website-url-text">Website URl</label>
			    	        <input type="text" name="type" id="cWebUrl" class="form-control input-sm" />
			            </div>

                        <div class="controll-group col-xs-12 col-sm-12 col-md-6">
                            <label for="img">Phone Number</label>
			    	        <input type="text" name="img" id="cPhone" class="form-control input-sm" />
			            </div>
                    </div>
                     <div class="row">
                         <div class="controll-group col-xs-12 col-sm-12 col-md-6" >

                        <label for="bidType">Select Bid Type</label>
                        <br/>
                        <span id="bidTypeContainer">
                                
                            <input type="radio" name='bidType' value="1" checked="checked" />
                            <span>Per user action ($0.20/action)</span>
                            <br/>
                            <input type="radio" name='bidType' value="2" />
                            <span>Per 1000 Impressions ($0.20/Impression)</span>
                        </span>
                         
                    </div>
                          <div class="controll-group col-xs-12 col-sm-12 col-md-6">
                             <label for="adTypeDdl">Select Cost ($)</label>
                             <select id="bidCost" class="vendorDropdown">
                                <option value="0.5">0.5</option> 
                                <option value="1.0">1.0</option>
                                <option value="1.5">1.5</option>
                                <option value="2.0">2.0</option>
                                <option value="2.5">2.5</option>
                                <option value="3.0">3.0</option>
                                <option value="3.5">3.5</option>
                                <option value="4.0">4.0</option>
                                <option value="4.5">4.5</option>
                                <option value="5.0">5.0</option>
                                <option value="5.5">5.5</option>
                                <option value="6.0">6.0</option>
                                <option value="6.5">6.5</option>
                                <option value="7.0">7.0</option>
                                <option value="7.5">7.5</option>
                                <option value="8.0">8.0</option>
                                <option value="8.5">8.5</option>
                                <option value="9.0">9.0</option>
                                <option value="9.5">9.5</option>
                                <option value="10.0">10.0</option>
                            </select>
			            </div>
                     </div>
                </div>
                
            </div>


            <div class="col-xs-12 col-sm-12 col-md-6">
                <div class="box box-primary" style="height: 400px;">
                    <div class="box-header with-border">
                        <h3 class="box-title">Select your Interest</h3>
                    </div>

                    <div class="controll-group">
                        <label for="interestCategory">Select Interest Category</label>
			            <select id="interestCategory" class="vendorDropdown" onchange="LoadSubCategories()">
                        </select>
			        </div>


                    <div class="controll-group">

                        <label for="adInterest">Select Interest</label>
			            <select id="adInterest" class="vendorDropdown">
                                                    
                        </select>
			        </div>

                    <div class="controll-group-or">
                        <h4>
                            OR
                        </h4>
                    </div>


                    <div class="controll-group">
                        <label for="adCustomInterest">Enter Custom Interest</label>
			    	    <input type="text" name="type" id="adCustomInterest" class="form-control input-sm"  />
			        </div>
                </div>
            </div>
        </div>
        <div class="row" id="sponsorSection">
            <div class="col-xs-12 col-sm-12 col-md-12">
                <div class="box box1 box-primary">
                    <div class="box-header with-border">
                        <h3 class="box-title">Sponsor Information</h3>
                    </div>


                    <div class="controll-group col-xs-12 col-sm-12 col-md-6">
                        <label for="spFacts">Enter your map fact (<span id="charcounter">155</span> character remaining)</label>
                        <textarea class="form-control input-sm" id="spFacts"></textarea>
			        </div>
                    
                    <div class="controll-group col-xs-12 col-sm-12 col-md-6">
                                                <br/>

                         <input type="radio" name='sponsorType' id="img-checkbox" value="spImageBtn" />
                        <span>Photo of map location</span>

                        <br/>
                        <input type="radio" id="video-checkbox" name='sponsorType' value="spVideoBtn" />
                        <span>Video of map location</span>
                        <br/>
                       
                        <div  class="col-lg-6" id="img-block"  >
                            <br/>
                            <label for="img">Upload Image</label>
			    	        <input type="file" required=""  value=""  id="photoUrl" class="form-control input-sm" />
                        </div>
                        <div  class="col-lg-6" id="video-block" style="display: none">
                            <br/>
                            <label for="img">Video URL</label>
			    	        <input type="text" id="video-url-tb"  class="form-control input-sm" />
                        </div>                        
                    </div>
                </div>            
            </div>
            </div>
        </div>        
        <div class="row" id="couponSection">
            <div class="col-xs-12 col-sm-12 col-md-12">
                <div class="box box1 box-primary">
                    <div class="box-header with-border">
                        <h3 class="box-title">Coupon Information</h3>
                    </div>
                    

                </div>
                
            </div>
        </div>
          <div class="row">
            <div class="col-xs-12 col-sm-12 col-md-12">
                 <label for="interestCategory">Select Ad-Location</label>
                  <input id="pac-input" class="controls" type="text" placeholder="Search Box" />
                  <div id="map"></div>
            </div>
          </div>


        

        <div class="row">
            <div class="controll-group">
                
                <button class="btn btn-default" style="margin-left: 12px;"  onclick="cancelAd()">Cancel</button>

                <button class="btn btn-primary" id="adBtn" onclick="return ValidateAd()"> Submit Ad for reveiw</button>           
            </div>
        </div>
     </div>



</asp:Content>


