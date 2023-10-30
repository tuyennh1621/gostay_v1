$(document).on("keypress", 'form', function (e) {
    if (e.target.className.indexOf("allowEnter") == -1) {
        var code = e.keyCode || e.which;
        if (code == 13) {
            e.preventDefault();
            return false;
        }
    }
});
function botCheck() {
    var botPattern = "(googlebot\/|Googlebot-Mobile|Googlebot-Image|Google favicon|Mediapartners-Google|bingbot|slurp|java|wget|curl|Commons-HttpClient|Python-urllib|libwww|httpunit|nutch|phpcrawl|msnbot|jyxobot|FAST-WebCrawler|FAST Enterprise Crawler|biglotron|teoma|convera|seekbot|gigablast|exabot|ngbot|ia_archiver|GingerCrawler|webmon |httrack|webcrawler|grub.org|UsineNouvelleCrawler|antibot|netresearchserver|speedy|fluffy|bibnum.bnf|findlink|msrbot|panscient|yacybot|AISearchBot|IOI|ips-agent|tagoobot|MJ12bot|dotbot|woriobot|yanga|buzzbot|mlbot|yandexbot|purebot|Linguee Bot|Voyager|CyberPatrol|voilabot|baiduspider|citeseerxbot|spbot|twengabot|postrank|turnitinbot|scribdbot|page2rss|sitebot|linkdex|Adidxbot|blekkobot|ezooms|dotbot|Mail.RU_Bot|discobot|heritrix|findthatfile|europarchive.org|NerdByNature.Bot|sistrix crawler|ahrefsbot|Aboundex|domaincrawler|wbsearchbot|summify|ccbot|edisterbot|seznambot|ec2linkfinder|gslfbot|aihitbot|intelium_bot|facebookexternalhit|yeti|RetrevoPageAnalyzer|lb-spider|sogou|lssbot|careerbot|wotbox|wocbot|ichiro|DuckDuckBot|lssrocketcrawler|drupact|webcompanycrawler|acoonbot|openindexspider|gnam gnam spider|web-archive-net.com.bot|backlinkcrawler|coccoc|integromedb|content crawler spider|toplistbot|seokicks-robot|it2media-domain-crawler|ip-web-crawler.com|siteexplorer.info|elisabot|proximic|changedetection|blexbot|arabot|WeSEE:Search|niki-bot|CrystalSemanticsBot|rogerbot|360Spider|psbot|InterfaxScanBot|Lipperhey SEO Service|CC Metadata Scaper|g00g1e.net|GrapeshotCrawler|urlappendbot|brainobot|fr-crawler|binlar|SimpleCrawler|Livelapbot|Twitterbot|cXensebot|smtbot|bnf.fr_bot|A6-Indexer|ADmantX|Facebot|Twitterbot|OrangeBot|memorybot|AdvBot|MegaIndex|SemanticScholarBot|ltx71|nerdybot|xovibot|BUbiNG|Qwantify|archive.org_bot|Applebot|TweetmemeBot|crawler4j|findxbot|SemrushBot|yoozBot|lipperhey|y!j-asr|Domain Re-Animator Bot|AddThis)";
    var re = new RegExp(botPattern, 'i');
    var userAgent = navigator.userAgent;
    if (re.test(userAgent)) {
        return true;
    } else {
        return false;
    }
}
(function (d, s, id) {

    if (botCheck()) {
        return;
    }

    var _keyGGMapAPI ='AIzaSyBK3AZ1XLhsOCk6Gx6toG6KKE6sTOtgN6s'

    var js, fjs = d.getElementsByTagName(s)[0];
    if (d.getElementById(id)) { return; }
    js = d.createElement(s); js.id = id;
    js.onload = function () {
        // remote script has loaded
    };
    js.src = "https://maps.googleapis.com/maps/api/js?key=" + _keyGGMapAPI + "&libraries=places&callback=initAutocomplete";
    fjs.parentNode.insertBefore(js, fjs);
}(document, 'script', 'googleapis'));

var map, markerCluster;
var isBusy = false;
var markers = [], markers_markerCluster = [], circles = [];
var arrIDVipMarker = [];
var localLat = parseFloat(localStorage.getItem("lat")) || 21.01945312057496;
var localLlon = parseFloat(localStorage.getItem("lon")) || 105.83039115211488;

var staticlocalLat = localLat;
var staticlocalLon = localLlon;

var latlng = { lat: localLat, lng: localLlon };

function binlist(__nearlat, __nearlon, radius) {
    $.ajax({
        type: "GET",
        traditional: true,
        data:
        {
            "lat": __nearlat,
            "lon": __nearlon,
        },
        url: "/Hotels/getNearbyLocation",
    }).done(function (result) {
        $("#mapList").html(result);
    });

    $.ajax({
        type: "GET",
        traditional: true,
        data:
        {
            "lat": __nearlat,
            "lon": __nearlon,
        },
        url: "/Hotels/getstringNearbyLocation",
    }).done(function (result) {
        var jsonData = JSON.parse(result);
        
        for (var i = 0; i < jsonData.length; i++) {

            var Data = jsonData[i];

           
            
            var infowindow = new google.maps.InfoWindow();

            var marker;

            marker = new google.maps.Marker({
                map: map,
                animation: google.maps.Animation.DROP,
                icon: 'assets/images/marker.png',
                //title: Data.Value,
                //label: Data.Value,
                position: { lat: parseFloat(Data.Lat), lng: parseFloat(Data.Lon) }
            });


            google.maps.event.addListener(marker, 'click', (function (marker, i) {
                return function () {
                    infowindow.setContent(jsonData[i].Value);
                    infowindow.open(map, marker);
                }
            })(marker, i));

            markers.push(marker);

        }
    });

    $("#property_map_label").html("Tìm các khách sạn trong vòng bán kính: " + Math.round((radius / 1000), 2) + " Km tính từ tâm bản đồ");
}

function initAutocomplete() {
    map = new google.maps.Map(document.getElementById('map-main'), {
        center: latlng,
        zoom: 14,
        mapTypeId: 'roadmap'
    });
    markers.push(new google.maps.Marker({
        map: map,
       /* icon: 'assets/images/marker.png',*/
        animation: google.maps.Animation.BOUNCE,
        title: 'Vị trí của bạn',
        position: latlng
    }));
    var radius = 1000;
    var geocoder = new google.maps.Geocoder();
    geocoder.geocode({ 'location': latlng }, function (results, status) {
        if (status === 'OK') {
            if (results[0]) {

                $("#pac-input").val(results[0].formatted_address);
                binlist(map.getCenter().lat(), map.getCenter().lng(), radius);
               // _2014Index.getNearMe(id_lt, id_tt, localLat.toString(), localLlon.toString(), radius, '0', ID_LOAINHA, ID_HUONGNHA, fromGia, toGia, fromDientich, toDientich, getNearMe_callback);

            } else {
               // _2014Index.getNearMe(id_lt, id_tt, localLat.toString(), localLlon.toString(), radius, '0', ID_LOAINHA, ID_HUONGNHA, fromGia, toGia, fromDientich, toDientich, getNearMe_callback);

            }
        } else {
            //_2014Index.getNearMe(id_lt, id_tt, localLat.toString(), localLlon.toString(), radius, '0', ID_LOAINHA, ID_HUONGNHA, fromGia, toGia, fromDientich, toDientich, getNearMe_callback);

        }
    });

    var input = document.getElementById('pac-input');
    var _searchPanel = document.getElementById('property_map_searchPanel');
    var btnList = document.getElementById('btnList');
    var _options = {
        componentRestrictions: { country: 'vn' },
    };

    var searchBox = new google.maps.places.SearchBox(input, _options);

    map.controls[google.maps.ControlPosition.TOP_CENTER].push(_searchPanel);

    map.addListener('bounds_changed', function () {
        searchBox.setBounds(map.getBounds());
        //
    });

    function setMapOnAll(map) {
        for (var i = 0; i < markers.length; i++) {
            markers[i].setMap(map);
        }
    }

    var DefauleIconcir = {
        path: google.maps.SymbolPath.CIRCLE,
        scale: 18,
        fillColor: "#2d3e52",
        fillOpacity: 0.95,
        strokeWeight: 1.3,
        strokeColor: "#FFF"
    }
    var SmallIconcir = {
        path: google.maps.SymbolPath.CIRCLE,
        scale: 8,
        fillColor: "#2d3e52",//fillColor: "#4d90fe",
        fillOpacity: 0.95,
        strokeWeight: 1.3,
        strokeColor: "#FFF"
    }
    function replacelabelmaker(label, zoomlv, tinvip) {
        if (zoomlv >= 17) {
            var _lable = { color: '#FFFFFF', fontWeight: 'bold', fontSize: '10px', text: label };
            return _lable;
        }
        else {
            if (tinvip < 80)
                return "";
            else {
                var _lable = { color: '#FFFFFF', fontWeight: 'bold', fontSize: '10px', text: label };
                return _lable;
            }
        }
    }
    
    function binHowfar(_lat, _lng) {
        if (_lat > 0) {
            var __lat = _lat - staticlocalLat;
            __lat = Math.pow(__lat, 2);

            var __lng = _lng - staticlocalLon;
            __lng = Math.pow(__lng, 2);

            var _far = Math.round(Math.sqrt(__lng + __lat) * 1000) / 10;
            if (_far <= 0.5) {
                return ' Gần bạn';
            }
            else {
                return (' Cách bạn: ' + _far + 'km');
            }

        }
    }
    function replaceContent(index, id, header, gia, dientich, strcontact, img, lat, lon, _mobile, _idu) {

        var contentString = "<div onclick=\"focusPosition(" + index + "," + lat + "," + lon + ")\" class=\"divItemNha\">";
        contentString += "<div style=\"float: left; width: 80px;\">";
        contentString += "<a target=\"_blank\" href=\"https://nhadat24h.net/ban-/xem-ID" + id + "\">" + replaceImage(img) + "</a>";
        contentString += "</div><div style=\"float: right;width:248px;\">";
        contentString += "<div><a target=\"_blank\" href=\"https://nhadat24h.net/ban-/xem-ID" + id + "\">" + header + "</a></div>";
        contentString += "<label style=\"color:#0774B9;\">Giá: <strong style=\"color:#FF5C01;\">" + replaceGia(gia) + "</strong> - Diện Tích: <strong style=\"color:#FF5C01;\">" + dientich + "</strong> M²</label>";
        contentString += "<div style=\"\"><i class=\"fas fa-map-marked-alt\"></i> " + binHowfar(lat, lon) + "</div>";
        contentString += "<div style=\"\"><i class=\"fa fa-phone\"></i> Điện thoại: " + _mobile + "</div>";

        contentString += "<div style=\"color:Gray; font-style:italic;\">" + strcontact + "</div></div></div>";
        return contentString;
    }
    function clearAllMarker() {
        markers.forEach(function (marker) { marker.setMap(null); });
        markers = [];

        markers_markerCluster.forEach(function (marker) { marker.setMap(null); });
        markers_markerCluster = [];


        if (markerCluster) {
            markerCluster.clearMarkers();
        }
        //       markerCluster = new MarkerClusterer(map, markers_markerCluster);

        //       markerCluster.setMap(null);
        $("#ulLeftListFormap_vip").html('')
        $("#ulLeftListFormap").html('')
    }
    function replaceImage(img) {
        var strIMG = "/Images/2021/no-image120.png";
        if (img)
            strIMG = img;
        return "<img style=\"width:75px;\" atl=\"\" src=\"" + strIMG + "\">";
    }
    function moveToLocation(_radius) {

        navigator.geolocation.getCurrentPosition(
            function (position) {
                if (isBusy)
                    return;

                isBusy = true;

                clearAllMarker();

                var center = new google.maps.LatLng(position.coords.latitude, position.coords.longitude);

                markers.push(new google.maps.Marker({
                    map: map,
                    animation: google.maps.Animation.BOUNCE,
                    title: 'Vị trí của bạn',
                    position: center,

                }));


                map.panTo(center);

                //_2014Index.getNearMe(id_lt, id_tt, position.coords.latitude.toString(), position.coords.longitude.toString(), _radius, '0', ID_LOAINHA, ID_HUONGNHA, fromGia, toGia, fromDientich, toDientich, getNearMe_callback);

            },
            function errorCallback(error) {
                alert('Định vị chưa sẵn sàng!');
            },
            {
                maximumAge: Infinity,
                timeout: 5000
            }
        );

    }


    function getNearMe_callback(response) {
        if (response.error != null) {
            //alert(response.error);
            return;
        }
        else {
            try {
                var jsonData = JSON.parse(response.value);
                for (var i = 0; i < jsonData.length; i++) {

                    var Data = jsonData[i];
                    var infowindow = new google.maps.InfoWindow();
                    var marker;
                    marker = new google.maps.Marker({
                        map: map,
                        icon: replaceIcon(parseInt(Data.TINVIP), map.getZoom()),
                        title: Data.HEADER,
                        label: replacelabelmaker(replaceGia(Data.fGIATIEN), map.getZoom(), parseInt(Data.TINVIP)),
                        position: { lat: parseFloat(Data._LAT), lng: parseFloat(Data._LON) }
                    });

                    // console.log(Data.ID_N +' - '+ Data.TINVIP);

                    var content = replaceContent(i, Data.ID_N, Data.HEADER, Data.fGIATIEN, Data.DIENTICH, Data.DESCRIPTION, Data.url_pixbe, parseFloat(Data._LAT), parseFloat(Data._LON), Data._MOBILE, Data._IDU);

                    google.maps.event.addListener(marker, 'click', (function (marker, i) {
                        return function () {
                            infowindow.setContent(replaceContent(i, jsonData[i].ID_N, jsonData[i].HEADER, jsonData[i].fGIATIEN, jsonData[i].DIENTICH,
                                jsonData[i].DESCRIPTION, jsonData[i].url_pixbe, parseFloat(jsonData[i]._LAT), parseFloat(jsonData[i]._LON), jsonData[i]._MOBILE, jsonData[i]._IDU));
                            infowindow.open(map, marker);
                        }
                    })(marker, i));

                    if (parseInt(jsonData[i].TINVIP) >= 80) {
                        if (!arrIDVipMarker.includes(jsonData[i].ID_N)) {
                            arrIDVipMarker.push(jsonData[i].ID_N);
                            infowindow.setContent(content);
                            infowindow.open(map, marker);
                        }
                        $("#ulLeftListFormap_vip").append(content);
                    }
                    else {


                        markers_markerCluster.push(marker)

                    }


                    markers.pushIfNotExist(marker, function (e) {

                    });
                    $("#ulLeftListFormap").append(content);

                }

                markers[0].setAnimation(null);
                //icon: "/images/2018/mappinvip2sao.png"

                if (map.getZoom() < 18) {

                    //markerCluster = new MarkerClusterer({ markers_markerCluster, map });

                    //markerCluster = new MarkerClusterer(map, markers_markerCluster);
                }

                isBusy = false;
                $("#divLoading").html("Đang xem: " + jsonData.length + " Nhà đất");
            }
            catch (err) {
                markers[0].setAnimation(null);
                isBusy = false;
                console.log(err);
            }

        }
    }

 
    map.addListener('dragend', function () {
        if (isBusy)
            return;

        isBusy = true;
        if (typeof (Storage) !== "undefined") {
            localStorage.setItem("localLat", map.getCenter().lat().toString());
            localStorage.setItem("localLng", map.getCenter().lng().toString());
        }
        clearAllMarker();
        var bounds = map.getBounds();
        var ne = bounds.getNorthEast();
        var _lat = ne.lat() - map.getCenter().lat();
        var _lon = ne.lng() - map.getCenter().lng();
        _lat = Math.pow(_lat, 2);
        _lon = Math.pow(_lon, 2);

        var _far = Math.round(Math.sqrt(_lat + _lon) * 100000);



        var radius = 50000;
        if (_far <= 1000) {
            radius = 1000
        }
        else if (_far <= 50000) {
            radius = _far
        }

        markers.push(new google.maps.Marker({
            map: map,
          /*  icon: 'assets/images/marker.png',*/
            animation: google.maps.Animation.BOUNCE,
            title: 'Quét các bất động sản trong vòng bán kính ' + radius + '(m) tính từ điểm này',
            position: { lat: map.getCenter().lat(), lng: map.getCenter().lng() }
        }));

        try {
            binlist(map.getCenter().lat(), map.getCenter().lng(), radius);
            isBusy = false;
          //  _2014Index.getNearMe(id_lt, id_tt, map.getCenter().lat().toString(), map.getCenter().lng().toString(), radius, '0', ID_LOAINHA, ID_HUONGNHA, fromGia, toGia, fromDientich, toDientich, getNearMe_callback);
        }
        catch {
            isBusy = false;
        }

        
        $("#pac-input").val('');

    });

    map.addListener('zoom_changed', function () {

        if (isBusy)
            return;

        isBusy = true;

        clearAllMarker();

        var bounds = map.getBounds();
        var ne = bounds.getNorthEast();
        var _lat = ne.lat() - map.getCenter().lat();
        var _lon = ne.lng() - map.getCenter().lng();
        _lat = Math.pow(_lat, 2);
        _lon = Math.pow(_lon, 2);

        var _far = Math.round(Math.sqrt(_lat + _lon) * 100000);



        var radius = 50000;
        if (_far <= 1000) {
            radius = 1000
        }
        else if (_far <= 50000) {
            radius = _far
        }

        markers.push(new google.maps.Marker({
            map: map,
   /*         icon: 'assets/images/marker.png',*/
            title: 'Quét các bất động sản trong vòng bán kính: ' + radius + 'm tính từ điểm này',
            position: { lat: map.getCenter().lat(), lng: map.getCenter().lng() }
        }));

        binlist(map.getCenter().lat(), map.getCenter().lng(), radius);
        isBusy = false;
    });

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
            if (typeof (Storage) !== "undefined") {

                //localStorage.setItem("lat", place.geometry.location.lat().toString());
                //localStorage.setItem("localLng", place.geometry.location.lng().toString());

                //id_tt = getID_TT(place.formatted_address);

               // localStorage.setItem("id_tt", id_tt);
            }

        });
        map.fitBounds(bounds);

        var bounds = map.getBounds();
        var ne = bounds.getNorthEast();
        var _lat = ne.lat() - map.getCenter().lat();
        var _lon = ne.lng() - map.getCenter().lng();
        _lat = Math.pow(_lat, 2);
        _lon = Math.pow(_lon, 2);

        var _far = Math.round(Math.sqrt(_lat + _lon) * 100000);

        var radius = 50000;
        if (_far <= 50000) {
            radius = _far
        }

        

       // _2014Index.getNearMe(id_lt, id_tt, map.getCenter().lat().toString(), map.getCenter().lng().toString(), radius, '0', ID_LOAINHA, ID_HUONGNHA, fromGia, toGia, fromDientich, toDientich, getNearMe_callback);
    });
}
