var pos = {};

navigator.geolocation.getCurrentPosition(
    // geo-location permission granted
    function (position) {

        pos = {
            lat: position.coords.latitude,
            lng: position.coords.longitude,
            elevationInMeters:0
        };
    },

     //geo-location permission NOT granted
    function (error) {
        if (error.code == 1) {
            console.log("Error getting lat and long to put into geocoder");
        }
    }
);


function initMap() {
    var map = new google.maps.Map(document.getElementById('map'), {
        zoom: 8,
        center: { lat: 2, lng: 2 }
    });

    var geocoder = new google.maps.Geocoder();
    var elevator = new google.maps.ElevationService;

    document.getElementById('getGeoLocationSubmitButton').addEventListener('click', function () {
        geocodeAddress(geocoder, map);
        locationElevation(pos, elevator);
    });
}



function locationElevation(location, elevator) {
    // Initiate the location request
    elevator.getElevationForLocations({
           'locations': [location]
    },

    function (results, status) {
        if (status === 'OK')
        {
            // Retrieve the first result
            if (results[0])
            {
                pos.elevationInMeters = results[0].elevation * 3.28;
            }
            else
            {
                console.log('No results found');
            }
        }
        else
        {
            console.log('Elevation service failed due to: ' + status);
        }
    });
}


function geocodeAddress(geocoder, resultsMap) {
    $("#map").css({"opacity": 1, "margin-left":"5%"});

    var address = pos.lat + ", " + pos.lng;

    geocoder.geocode(
        {
            'address': address
        },
        function (results, status) {
            if (status === 'OK') {

                resultsMap.setCenter(results[0].geometry.location);

                // set values in form

                var location = {
                    county    : results[0]["address_components"][3]["long_name"],
                    city      : results[0]["address_components"][2]["long_name"],
                    state     : results[0]["address_components"][4]["long_name"],
                    country   : results[0]["address_components"][5]["long_name"],
                    street    : results[0]["address_components"][1]["long_name"],
                    streetNum : results[0]["address_components"][0]["long_name"],
                    zip: results[0]["address_components"][6]["long_name"],
                    lat: pos.lat.toString().substr(0, pos.lat.toString().indexOf(".")) + pos.lat.toString().substr(pos.lat.toString().indexOf("."), 6),
                    lng: pos.lng.toString().substr(0, pos.lng.toString().indexOf(".")) + pos.lng.toString().substr(pos.lng.toString().indexOf("."), 6),
                    elevation: pos.elevationInMeters.toString().substr(0, pos.elevationInMeters.toString().indexOf(".")) + pos.elevationInMeters.toString().substr(pos.elevationInMeters.toString().indexOf("."), 6)
                }

                // detect and place today's date
                var today = new Date();
                var dd = today.getDate();
                var mm = today.getMonth() + 1; //January is 0!
                var yyyy = today.getFullYear();

                if (dd < 10) {
                    dd = '0' + dd
                }

                if (mm < 10) {
                    mm = '0' + mm
                }

                today = yyyy + "-" + mm + '-' + dd;
                
                // stick all the values in
                $("#CollectedDate").val(today);
                $("#CollectorName").val(LoggedInUsername);
                $("#County").val(location.county);
                $("#State").val(location.state);
                $("#Country").val(location.country);
                $("#LocationDesc").val(location.streetNum + " " + location.street + " " + " " + location.city + " " + location.zip);
                $("#Lat").val(location.lat); 
                $("#Lng").val(location.lng);
                $("#Elevation").val(Math.floor(location.elevation));
           
                var marker = new google.maps.Marker({
                    map: resultsMap,
                    position: results[0].geometry.location
                });
            } else {
                alert('Geocode was not successful for the following reason: ' + status);
            }
        }
    );
}