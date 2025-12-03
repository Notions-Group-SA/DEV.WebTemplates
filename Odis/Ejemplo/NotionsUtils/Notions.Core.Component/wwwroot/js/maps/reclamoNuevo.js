let map;
let dataCoordenadas = [];
let markers = [];
let luminarias = [];
let marker;
let selectedluminaria;
let infowindow;
let dotNetHelper;
let EsReclamoInventario = false;
let motivoId = null;
let doneTypingInterval = 500;
let typingTimer;

// Initialize the map component
//window.initMapComponent = function (helper, apikey, EsInventario, motivoId, userLat, userLong) {
export function initMapComponent(helper, apikey, EsInventario, motivoId, userLat, userLong) {
    dotNetHelper = helper;
    EsReclamoInventario = EsInventario;
    motivoId = motivoId;

    // Load Google Maps API script dynamically
    if (typeof google === 'undefined' || typeof google.maps === 'undefined') {
        loadGoogleMapsScript(apikey).then(() => {
            initMap(userLat, userLong);
        });
    } else {
    initMap(userLat, userLong);
    }
};

// Load Google Maps API script
function loadGoogleMapsScript(apikey) {
    return new Promise((resolve, reject) => {
        const script = document.createElement('script');
        script.src = `https://maps.googleapis.com/maps/api/js?key=${apikey}`;

        script.async = true;
        script.defer = true;
        script.onload = resolve;
        script.onerror = reject;
        document.head.appendChild(script);
    });
}

// Initialize the map
function initMap(userLat, userLong) {
    fetch('api/maps/GetOptions')
        .then(response => response.json())
        .then(options => {
            const mapOptions = {
                zoom: options[0].zoom,
                center: {
                    lat: options[0].centerLat,
                    lng: options[0].centerLong
                }
            };

            map = new google.maps.Map(document.getElementById('googlemap'), mapOptions);
        })
        .catch(error => console.error('Error loading map options:', error));
}

// Update inventory status
//window.updateEstadoInventario = function (EsInventario, MoptivoId) {
export function updateEstadoInventario(EsInventario, MoptivoId) {
    EsReclamoInventario = EsInventario;
    motivoId = MoptivoId;

    if (map && EsReclamoInventario) {
        map.setZoom(17.75);
        cargarInventario();
    }
};

// Load inventory markers
//window.cargarInventario = function () {
export function cargarInventario() {
    fetch(`api/maps/getMarkersBy_MotivoIncidente?id_motivo=${motivoId}`)
        .then(response => response.json())
        .then(data => addLuminaria(data))
        .catch(error => console.error('Error loading inventory:', error));
};

// Search coordinates by address
//window.buscarCoordenadas = function (calle, numero, localidad) {
export function buscarCoordenadas(calle, numero, localidad) {
    deleteMarkers();

    fetch(`api/maps/GetMarker_Direccion?Direccion=${calle ?? ''} ${numero ?? ''}&IdLocalidad=${localidad}`)
        .then(response => response.json())
        .then(data => {
            if (EsReclamoInventario) {
                dataCoordenadas = data;
                map.setZoom(17.75);
                map.setCenter({ lat: data[0].Latitud, lng: data[0].Longitud });
            } else {
                cargarPins(data);
            }
        })
        .catch(error => console.error('Error searching coordinates:', error));
};

// Change locality
//window.cambiarLocalidad = function (idLocalidad) {
export function cambiarLocalidad(idLocalidad) {
    deleteMarkers();

    fetch(`api/Maps/GetMarker_Localidad?IdLocalidad=${idLocalidad}`)
        .then(response => {
            console.log('Respuesta del servicio:', response); // Imprime la respuesta completa
            return response.json();
        })
        .then(data => {
            console.log(data);
            cargarPins(data);
        })
        .catch(error => console.error('Error changing locality:', error));
};

// Add marker to the map
function agregarpin(latitud, longitud, icono, label, infoWindow, timeAnimacion) {
    window.setTimeout(function () {

        // Update coordinates in .NET
        if (!latitud)
            latitud = -33.1469623;

        if (!longitud)
            longitud = -59.3204404;

        let markerInfoWindow = new google.maps.InfoWindow({
            content: infoWindow
        });

        marker = new google.maps.Marker({
            position: { lat: latitud, lng: longitud },
            map: map,
            title: '',
            animation: google.maps.Animation.DROP,
            icon: EsReclamoInventario ? '../imgs/focoverde.png' : icono,
            label: label,
            draggable: !EsReclamoInventario
        });

        dotNetHelper.invokeMethodAsync('UpdateCoordenadas', latitud, longitud);

        // Add drag event listener
        google.maps.event.addListener(marker, 'drag', function (event) {
            dotNetHelper.invokeMethodAsync('UpdateCoordenadas', event.latLng.lat(), event.latLng.lng());
        });

        google.maps.event.addListener(marker, 'dragend', function (event) {
            dotNetHelper.invokeMethodAsync('UpdateCoordenadas', event.latLng.lat(), event.latLng.lng());
        });

        map.panTo(marker.getPosition());
        markers.push(marker);

    }, timeAnimacion);
}

// Load markers based on data
function cargarPins(json) {
    for (let x = 0; x < json.length; x++) {
        agregarpin(
            json[x].position.lat,
            json[x].position.lng,
            json[x].icon,
            json[x].label,
            '',
            (x + 1) * 10
        );
    }
}

// Get inventory item details
function requestInfoWindow(id_luminaria) {
    fetch(`api/maps/getItemInventario/${id_luminaria}`)
        .then(response => response.json())
        .then(data => {
            let template = `
                <div class="card-body p-0 text-gray" style="width: 14rem; text-transform: capitalize; padding: 0px;">
                    <h6 class="my-1 text-gray">${data.descripcion}</h5>
                    <p class="card-text my-1 text-gray">${data.subtipoInventario}</p>
                    <h6 class="card-subtitle my-1 mb-2 text-gray">Código: ${!data.cod ? 's/c' : data.cod}</h6>
                </div>
            `;

            infowindow = new google.maps.InfoWindow({
                title: data.descripcion,
                content: template
            });

            infowindow.open(map, marker);
        })
        .catch(error => console.error('Error getting inventory item details:', error));
}

// Add luminaria markers
function addLuminaria(data) {
    let list = [];

    for (let i = 0; i < data.length; i++) {
        let luminaria = new google.maps.Marker({
            position: { lat: data[i].lat, lng: data[i].lng },
            map: map,
            icon: data[i].icon || '../imgs/focoverde.png',
            id: data[i].id
        });

        luminaria.addListener('click', function () {
            if (infowindow) infowindow.close();
            requestInfoWindow(this.id);

            if (selectedluminaria) {
                selectedluminaria.setMap(map);
            }

            // Update inventory item in .NET
            dotNetHelper.invokeMethodAsync('UpdateInventario', this.id, this.getPosition().lat(), this.getPosition().lng());

            selectedluminaria = this;
            selectedluminaria.setMap(null);

            if (!marker) {
                marker = new google.maps.Marker({
                    position: { lat: this.getPosition().lat(), lng: this.getPosition().lng() },
                    map: map,
                    title: '',
                    animation: google.maps.Animation.DROP,
                    icon: '../imgs/camera_icon_verde.png',
                    label: '',
                    draggable: false
                });

                marker.addListener('click', function () {
                    infowindow.open(map, marker);
                });

                map.setZoom(17.75);
            }
            else {
                marker.setPosition(this.getPosition());
                marker.setAnimation(google.maps.Animation.DROP);

                map.setZoom(17.75);
                setTimeout(function () {
                    marker.setAnimation(null);
                }, 200);
            }
        });

        list.push(luminaria);
    }

    luminarias = list;
}

// Set map on all markers
function setMapOnAll(map) {
    for (let i = 0; i < markers.length; i++) {
        markers[i].setMap(map);
    }
}

// Clear markers from the map
function clearMarkers() {
    setMapOnAll(null);
}

// Delete all markers
function deleteMarkers() {
    clearMarkers();
    markers = [];
}

// Get current position
//window.getCurrentPosition = function () {
export function getCurrentPosition() {
    if ("geolocation" in navigator) {
        navigator.geolocation.getCurrentPosition(function (position) {
            infoWindow = new google.maps.InfoWindow({ map: map });
            let pos = { lat: position.coords.latitude, lng: position.coords.longitude };
            clearMarkers();
            agregarpin(position.coords.latitude, position.coords.longitude, '', '', '', 100);

            // Update UI elements
            document.getElementById('cargaDireccion').style.display = 'none';
            document.getElementById('mapa').style.display = 'block';
            document.getElementById('volverEscribirDireccion').style.display = 'block';

            // Update coordinates in .NET
            dotNetHelper.invokeMethodAsync('UpdateCoordenadas', position.coords.latitude, position.coords.longitude);
        }, function (error) {
            switch (error.code) {
                case error.PERMISSION_DENIED:
                    alert('Alguna vez nos denegaste esta opción de obtener tu posicionamiento para este sitio. Si queres que esta opcion funcione debés eliminar el blockeo desde las opciones de tu navegador.');
                    break;
                case error.POSITION_UNAVAILABLE:
                    alert('Tenés bloqueado el GPS para este sitio');
                    break;
                case error.TIMEOUT:
                    alert('timeout, volve a intentarlo mas tarde.');
                    break;
                case error.UNKNOWN_ERROR:
                    alert('Error desconocido.');
                    break;
            }
        });
    } else {
        alert("geolocation no habilitada!");
    }
};

// Focus on element
//window.focusElement = function (elementId) {
export function focusElement(elementId) {
    const element = document.getElementById(elementId);
    if (element) {
        element.focus();
    }
};