const view = new ol.View({
    center: [0, 0],
    zoom: 2,
});

const map = new ol.Map({
    target: 'map',
    layers: [
        new ol.layer.Tile({
            source: new ol.source.OSM(),
        }),
    ],
    view: view,
});

const styles = {
    'routeW': new ol.style.Style({
        stroke: new ol.style.Stroke({
            width: 6,
            color: [237, 212, 0, 0.8],
        }),
    }),
    'routeB': new ol.style.Style({
        stroke: new ol.style.Stroke({
            width: 6,
            color: [212, 237, 0, 0.8],
        }),
    }),
    'start': new ol.style.Style({
        image: new ol.style.Icon({
            anchor: [0.5, 1],
            src: 'https://openlayers.org/en/main/examples/data/geolocation_marker_heading.png',
        }),
    }),
    'end': new ol.style.Style({
        image: new ol.style.Icon({
            anchor: [0.5, 1],
            rotation: Math.PI,
            src: 'https://openlayers.org/en/main/examples/data/geolocation_marker_heading.png',
        }),
    }),
    'station': new ol.style.Style({
        image: new ol.style.Circle({
            radius: 7,
            fill: new ol.style.Fill({color: 'black'}),
            stroke: new ol.style.Stroke({
                color: 'white',
                width: 2,
            }),
        }),
    })
};

const vectorLayer = new ol.layer.Vector({
    style: function (feature) {
        return styles[feature.get('type')];
    }
});

const vector = new ol.source.Vector({
    features: []
});

vectorLayer.setSource(vector);

map.addLayer(vectorLayer);

function addRoute(path, start, end){
    const features = [];

    const route = new ol.format.Polyline({
        factor: 1e5,
        geometryLayout: 'XY'
    }).readGeometry(path, {
        dataProjection: 'EPSG:4326',
        featureProjection: 'EPSG:3857'
    });

    const routeFeature = new ol.Feature({
        type: start || end ? 'routeW' : 'routeB',
        geometry: route
    });

    features.push(routeFeature)

    console.log(route);

    map.getView().fit(route);

    if (start){
        const startMarker = new ol.Feature({
            type: 'start',
            geometry: new ol.geom.Point(route.getCoordinateAt(0)),
        });

        features.push(startMarker);

    } else if (end){

        const endMarker = new ol.Feature({
            type: 'end',
            geometry: new ol.geom.Point(route.getCoordinateAt(1)),
        });

        features.push(endMarker)
    } else {
        const startMarker = new ol.Feature({
            type: 'station',
            geometry: new ol.geom.Point(route.getCoordinateAt(0)),
        });

        features.push(startMarker);

        const endMarker = new ol.Feature({
            type: 'station',
            geometry: new ol.geom.Point(route.getCoordinateAt(1)),
        });

        features.push(endMarker)
    }

    vector.addFeatures(features);
}

function clearRoute(){
    map.setView(view);
    vector.clear();
}