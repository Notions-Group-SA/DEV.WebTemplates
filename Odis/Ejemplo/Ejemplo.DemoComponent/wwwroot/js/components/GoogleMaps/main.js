class MapsBlazor
{
    constructor(dotnetHelper, idMaps, apikey, zoom, center)
    {
        this.dotnetHelper = dotnetHelper;

        this.maps = new GoogleMapsService(idMaps,
            {
                apiKey: apikey,
                center: center,
                zoom: zoom
            });

        this.maps.onMarkerDrop((lat, lng) => this.dragOutMarker(lat, lng));

        this.maps.onPositionChanged((lat, lng) => this.dragOutMarker(lat, lng));
    }

    dragOutMarker(lat, lng)
    {
        try
        {
            if (this.dotnetHelper)
            {
                this.dotnetHelper.invokeMethodAsync('OnMarkerCoordenadaChanged', { lat: lat, lng: lng });
            }
            else
                console.log('notificar: dotnetHelper es null');
        }
        catch (error) {
            console.log(error);
        }
    }

    async SetDireccion(direccion)
    {
        try
        {
            await this.maps.isReady();
            await this.maps.geocodeAddress(direccion);
        }
        catch (error)
        {
            console.log(error);
        }
    }

    GetCoordenadas()
    {
        return maps.getCurrentPosition();
    }
}

export function InitializeMapsBlazor(dotnetHelper, idMaps, apikey, zoom, center) {
    try
    {
        let element = document.getElementById(idMaps);
        if (element && !element.mapsBlazor)
        {
            element.mapsBlazor = new MapsBlazor(dotnetHelper, idMaps, apikey, zoom, center);
        }
    }
    catch (error)
    {
        console.log(error);
        console.log(error.stack);
    }
    return true;
}

export async function SetDireccion(idMaps, direccion)
{
    try
    {
        let element = document.getElementById(idMaps);
        if (element.mapsBlazor)
        {
            await element.mapsBlazor.SetDireccion(direccion);
        }
    }
    catch (error)
    {
        console.log(error);
    }
    return true;
}

export function getCoordenadas(idMaps)
{
    try
    {
        let element = document.getElementById(idMaps);
        if (element.mapsBlazor)
        {
            return element.mapsBlazor.GetCoordenadas();
        }
    }
    catch (error)
    {
        console.log(error);
    }
    return null;
}

export function DisposeMaps(idMaps) {
    try {
        let element = document.getElementById(idMaps);
        if (element && element.mapsBlazor) 
        {
            element.mapsBlazor = null;
            console.log(`Instancia de mapa ${idMaps} eliminada`);
            return true;
        }
        return false;
    }
    catch (error) {
        console.error('Error en DisposeMaps:', error);
        return false;
    }
}


