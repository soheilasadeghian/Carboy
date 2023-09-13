var lastLocation = "";
var marker = null;
var markermap = null;
function init(src) {
    
    $(document).on('new-position', function (event, result) {

        var latlng = new google.maps.LatLng(result.lat, result.lng);
        marker.setPosition(latlng);

    });

    markermap = new google.maps.Map(document.getElementById('map'), {
        zoom: 7
    });

    var image = 'https://carman.carboy.info/content/marker.png';

    marker = new google.maps.Marker({
        position: src,
        map: markermap
       
    });

   // 
}

