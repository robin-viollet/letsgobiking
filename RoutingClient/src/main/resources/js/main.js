map = new OpenLayers.Map("Map");

var fromProjection = new OpenLayers.Projection('EPSG:4326'); // Transform from WGS 1984
var toProjection = new OpenLayers.Projection('EPSG:900913'); // to Spherical Mercator Projection
var zoom = 13;

function position(lon, lat){
    return new OpenLayers.LonLat(lon,lat).transform(fromProjection, toProjection);
}
function center(lon, lat){
    map.setCenter(position(lon,lat), zoom);
    alert("Test");
}

var mapnik = new OpenLayers.Layer.OSM();
map.addLayer(mapnik);

var lon = 4.8320114, lat = 45.7578137;

center(lon, lat);

var markers = new OpenLayers.Layer.Markers("Markers");
map.addLayer(markers);

markers.addMarker(new OpenLayers.Marker(position(lon, lat)));