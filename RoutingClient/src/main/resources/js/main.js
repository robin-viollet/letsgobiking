// var map = new ol.Map("Map");
//
var fromProjection = new ol.proj.Projection('EPSG:4326'); // Transform from WGS 1984
var toProjection = new ol.proj.Projection('EPSG:900913'); // to Spherical Mercator Projection
// var zoom = 13;

/*var route = new ol.Geometry.Polyline({
    factor: 1e6
}).readGeometry("ghrlHkr~s@?DICqELI?IAsCi@ICKAuA_@i@GSA?_@?M?eA?S?W?S@wB@k@?U@{H?MI@I@u@FkBPwBTcALI@?M[mGAMC?B?KiB?KAM[aFASg@sI?CmAt@y@f@?BVlELpC", {
    dataProjection: 'EPSG:4326',
    featureProjection: 'EPSG:3857'
});

var routeFeature = new ol.Feature({
    type: 'route',
    geometry: route
});

var vectorLayer = new ol.layer.VectorLayer({
    source: new ol.source.Vector({
        features: [routeFeature]
    })
})

this.map.addLayer(vectorLayer);*/

// var mapnik = new ol.source.OSM();
// map.addLayer(mapnik);

const map = new ol.Map({
    target: 'map',
    layers: [
        new ol.layer.Tile({
            source: new ol.source.OSM(),
        }),
    ],
    view: new ol.View({
        center: [0, 0],
        zoom: 2,
    }),
});
function setRoute(path) {
    const route = new ol.format.Polyline({
        factor: 1e5,
        geometryLayout: 'XY'
    }).readGeometry(path, {
        dataProjection: 'EPSG:4326',
        featureProjection: 'EPSG:3857'
    })/*.transform(fromProjection, toProjection)*/;

    const routeFeature = new ol.Feature({
        type: 'route',
        geometry: route
    });

    map.getView().fit(route);

    /*
    const styles = {
        'route': new ol.style.Style({
            stroke: new ol.style.Stroke({
                width: 6,
                color: [237, 212, 0, 0.8],
            }),
        }),
        'icon': new ol.style.Style({
            image: new ol.style.Icon({
                anchor: [0.5, 1],
                src: 'https://openlayers.org/en/main/examples/data/icon.png',
            }),
        }),
        'geoMarker': new ol.style.Style({
            image: new ol.style.Circle({
                radius: 7,
                fill: new ol.style.Fill({color: 'black'}),
                stroke: new ol.style.Stroke({
                    color: 'white',
                    width: 2,
                }),
            }),
        }),
    };*/

    const vectorLayer = new ol.layer.Vector({
        source: new ol.source.Vector({
            features: [routeFeature]
        })/*,
    style: function (feature) {
        return styles[feature.get('type')];
    }*/
    });

    map.addLayer(vectorLayer);
}