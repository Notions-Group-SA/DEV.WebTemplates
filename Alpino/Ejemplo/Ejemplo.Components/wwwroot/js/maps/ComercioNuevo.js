// ComercioNuevo.js - Improved version
let map;
let poligonos = [];
let markers = [];
let dotNetHelper;
let IdRubro = 0;
let mapOptionsLocal = null;
let mapInitialized = false;

// Main initialization function called from Blazor
export async function initMapComponent(helper, apikey) {
    console.log("initMapComponent called");

    // Store the .NET helper reference
    dotNetHelper = helper;

    try {
        // Check if map element exists
        const mapElement = document.getElementById('googlemap');
        if (!mapElement) {
            console.error("Map container element not found!");
            return;
        }

        // Ensure Google Maps API is loaded
        if (typeof google === 'undefined' || typeof google.maps === 'undefined') {
            await loadGoogleMapsScript(apikey);
        }

        // Initialize the map if not already done
        if (!mapInitialized) {
            await initMap();
            mapInitialized = true;
        }
    } catch (error) {
        console.error("Error initializing map:", error);
        // Notify Blazor component of error
        try {
            await dotNetHelper.invokeMethodAsync('OnMapError', error.message);
        } catch (e) {
            console.error("Failed to report error to Blazor:", e);
        }
    }
}

// Load Google Maps script as a Promise
function loadGoogleMapsScript(apikey) {
    console.log("Loading Google Maps script");
    return new Promise((resolve, reject) => {
        // Check if script is already being loaded
        if (document.querySelector('script[src*="maps.googleapis.com/maps/api/js"]')) {
            console.log("Google Maps script is already loading");

            // Set up a check interval to see when Google Maps is available
            const checkGoogleMaps = setInterval(() => {
                if (typeof google !== 'undefined' && typeof google.maps !== 'undefined') {
                    clearInterval(checkGoogleMaps);
                    resolve();
                }
            }, 100);
            return;
        }

        const script = document.createElement('script');
        script.src = `https://maps.googleapis.com/maps/api/js?key=${apikey}&loading=async&callback=googleMapsLoaded`;
        script.async = true;
        script.defer = true;

        // Define a global callback that will be called when the script loads
        window.googleMapsLoaded = () => {
            console.log("Google Maps script loaded");
            resolve();
        };

        script.onerror = (error) => {
            console.error("Failed to load Google Maps script", error);
            reject(new Error("Failed to load Google Maps API"));
        };

        document.head.appendChild(script);
    });
}

// Initialize the map
async function initMap() {
    console.log("Initializing map");
    try {
        // Get map options from API
        const response = await fetch('api/maps/GetOptions');
        if (!response.ok) {
            throw new Error(`Failed to fetch map options: ${response.status}`);
        }

        const options = await response.json();
        if (!options || options.length === 0) {
            throw new Error("No map options returned from API");
        }

        const mapOptions = {
            zoom: options[0].zoom,
            center: {
                lat: options[0].centerLat,
                lng: options[0].centerLong
            }
        };

        mapOptionsLocal = mapOptions;

        // Check if map element exists again before creating map
        const mapElement = document.getElementById('googlemap');
        if (!mapElement) {
            throw new Error("Map container element not found when initializing map");
        }

        // Initialize the map
        map = new google.maps.Map(mapElement, mapOptions);

        // Notify Blazor component map is ready
        if (dotNetHelper) {
            try {
                await dotNetHelper.invokeMethodAsync('OnMapReady');
            } catch (e) {
                console.warn("Failed to notify Blazor of map ready state:", e);
            }
        }

        // Load polygons
        await requestPoligonos();
        return true;
    } catch (error) {
        console.error("Error in initMap:", error);
        throw error;
    }
}

// Load polygons for zones
async function requestPoligonos() {
    console.log("Requesting polygons");
    try {
        const response = await fetch('api/maps/getZonasHabilitacionComercio');
        if (!response.ok) {
            throw new Error(`Failed to fetch zones: ${response.status}`);
        }

        const datas = await response.json();

        // Clear existing polygons
        poligonos.forEach(p => p.setMap(null));
        poligonos = [];

        // Add new polygons
        datas.forEach(data => {
            let poligono = new google.maps.Polygon(data);
            poligono.setMap(map);
            poligonos.push(poligono);

            // Add marker at center of polygon
            if (data.centro) {
                let marker = new google.maps.Marker({
                    position: data.centro,
                    map: map,
                    icon: data.centro.icon || null
                });
            }
        });

        return true;
    } catch (error) {
        console.error("Error loading polygon data:", error);
        throw error;
    }
}

// Show address on map
export async function mostrarDomicilio(calle, numero, localidad) {
    console.log(`Mostrando domicilio: ${calle} ${numero}, Localidad: ${localidad}`);
    try {
        // Ensure map is initialized
        if (!map) {
            console.error("Map not initialized when trying to show address");
            return false;
        }

        // Remove existing markers
        deleteMarkers();

        // Fetch marker data
        const response = await fetch(`api/maps/GetMarker_Direccion?Direccion=${encodeURIComponent((calle || '') + ' ' + (numero || ''))}&IdLocalidad=${localidad}`);
        if (!response.ok) {
            throw new Error(`Failed to fetch address markers: ${response.status}`);
        }

        const data = await response.json();
        await cargaMarkers(data);
        return true;
    } catch (error) {
        console.error("Error showing address on map:", error);
        if (dotNetHelper) {
            try {
                await dotNetHelper.invokeMethodAsync('OnMapError', `Error showing address: ${error.message}`);
            } catch (e) { }
        }
        return false;
    }
}

// Show locality on map
export async function mostrarLocalidad(idLocalidad) {
    console.log(`Mostrando localidad: ${idLocalidad}`);
    try {
        // Ensure map is initialized
        if (!map) {
            console.error("Map not initialized when trying to show locality");
            return false;
        }

        // Remove existing markers
        deleteMarkers();

        // Fetch marker data
        const response = await fetch(`api/Maps/GetMarker_Localidad?IdLocalidad=${idLocalidad}`);
        if (!response.ok) {
            throw new Error(`Failed to fetch locality markers: ${response.status}`);
        }

        const data = await response.json();
        await cargaMarkers(data);
        return true;
    } catch (error) {
        console.error("Error showing locality on map:", error);
        if (dotNetHelper) {
            try {
                await dotNetHelper.invokeMethodAsync('OnMapError', `Error showing locality: ${error.message}`);
            } catch (e) { }
        }
        return false;
    }
}

// Add markers to the map
async function cargaMarkers(markerData) {
    if (!markerData || !Array.isArray(markerData)) {
        console.warn("No marker data or invalid marker data provided");
        return;
    }

    console.log(`Loading ${markerData.length} markers`);

    for (let x = 0; x < markerData.length; x++) {
        const marker = markerData[x];
        if (!marker.position || typeof marker.position.lat !== 'number' || typeof marker.position.lng !== 'number') {
            console.warn(`Invalid marker position for marker ${x}`, marker);
            continue;
        }

        await agregarMarker(
            marker.position.lat,
            marker.position.lng,
            marker.icon,
            marker.label,
            marker.infoContent || '',
            (x + 1) * 10
        );
    }
}

function agregarMarker(latitud, longitud, icono, label, infoContent, timeAnimacion) {
    return new Promise(resolve => {
        window.setTimeout(function () {
            try {
                if (!map) {
                    console.error("Map not initialized when adding marker");
                    resolve(false);
                    return;
                }

                // Create info window if content is provided
                let markerInfoWindow = infoContent ?
                    new google.maps.InfoWindow({ content: infoContent }) :
                    null;

                // Create marker
                let marker = new google.maps.Marker({
                    position: { lat: latitud, lng: longitud },
                    map: map,
                    title: '',
                    animation: google.maps.Animation.DROP,
                    icon: icono,
                    label: label,
                    draggable: true
                });

                markers.push(marker);

                if (markerInfoWindow) {
                    marker.addListener('click', () => {
                        markerInfoWindow.open(map, marker);
                    });
                }

                if (map) {
                    map.panTo(marker.getPosition());
                }

                if (dotNetHelper) {
                    dotNetHelper.invokeMethodAsync('UpdateCoordenadas', latitud, longitud)
                        .then(() => verificarZona(latitud, longitud))
                        .catch(error => console.error("Error updating coordinates:", error));
                }

                google.maps.event.addListener(marker, 'drag', function (event) {
                    if (dotNetHelper) {
                        dotNetHelper.invokeMethodAsync('UpdateCoordenadas', event.latLng.lat(), event.latLng.lng())
                            .then(() => verificarZona(event.latLng.lat(), event.latLng.lng()))
                            .catch(error => console.error("Error updating coordinates on drag:", error));
                    }
                });

                google.maps.event.addListener(marker, 'dragend', function (event) {
                    if (dotNetHelper) {
                        dotNetHelper.invokeMethodAsync('UpdateCoordenadas', event.latLng.lat(), event.latLng.lng())
                            .then(() => verificarZona(event.latLng.lat(), event.latLng.lng()))
                            .catch(error => console.error("Error updating coordinates on dragend:", error));
                    }
                });

                resolve(true);
            } catch (error) {
                console.error("Error adding marker:", error);
                resolve(false);
            }
        }, timeAnimacion);
    });
}

function setMapOnAll(map) {
    for (let i = 0; i < markers.length; i++) {
        markers[i].setMap(map);
    }
}

function clearMarkers() {
    setMapOnAll(null);
}

function deleteMarkers() {
    clearMarkers();
    markers = [];
}

export function perteneceAZona(idRubro, latitud, longitud) {
    console.log(`Checking zone: Rubro ${idRubro}, Lat ${latitud}, Lng ${longitud}`);
    IdRubro = idRubro;
    return verificarZona(latitud, longitud);
}

async function verificarZona(latitud, longitud) {
    if (IdRubro === 0 || latitud === null || longitud === null) {
        console.warn("Missing data for zone verification:", { IdRubro, latitud, longitud });
        return false;
    }

    try {
        const response = await fetch(`api/maps/getHabilitacionComercio_by_Rubro_PorLatitudLongitud?idRubro=${IdRubro}&latitud=${latitud}&longitud=${longitud}`);
        if (!response.ok) {
            throw new Error(`HTTP error: ${response.status}`);
        }

        const text = await response.text();
        const coordenadaHabilitada = text === 'true';

        if (dotNetHelper) {
            await dotNetHelper.invokeMethodAsync('UpdateEsZonaHabilitada', coordenadaHabilitada);
        }

        return coordenadaHabilitada;
    } catch (error) {
        console.error("Error verifying zone:", error);
        return false;
    }
}
export function isMapReady() {
    return !!map && mapInitialized;
}

export function resetMap() {
    if (map) {
        deleteMarkers();
        poligonos.forEach(p => p.setMap(null));
        poligonos = [];
        mapInitialized = false;
    }
}