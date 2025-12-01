class MapsBlazor
{
    version = 'MapsBlazor.0.0.5';

    constructor(dotnetHelper, idDiv, options)
    {
        console.log(`MapsBlazor v:${this.version}`);

        this.container = document.getElementById(idDiv); // Corregido: "containter" -> "container"
        this.dotnetHelper = dotnetHelper;
        
        this.readyPromise = this.InitilizeMapsService(idDiv, options);
    }

    async InitilizeMapsService(idDiv, options)
    {
        try {
            this.maps = new MapsService(idDiv,
                {
                    apiKey: options.apiKey,
                    center: options.center,
                    zoom: options.zoom,
                    clusterable: options.clusterable
                });

            // Esperar a que MapsService esté completamente inicializado
            await new Promise(resolve => setTimeout(resolve, 100));
            this.isReady = true;
            return true;
        }
        catch (error) {
            console.error('Error inicializando MapsService:', error);
            this.isReady = false;
            return false;
        }

    }

    async waitUntilReady() {
        if (this.readyPromise) {
            await this.readyPromise;
        }
    }

    async addMarker(markerInfo)
    {
        await this.waitUntilReady();

        if (markerInfo.draggable)
        {
            markerInfo.draggendCallback = this.draggendCallback.bind(this);
        }

        if (markerInfo.callable)
        {
            markerInfo.clickMarkerCallback = this.clickMarkerCallback.bind(this);
        }

        await this.maps.addMarker( markerInfo);
    }

    async addPolygon(polygonInfo) 
    {
        await this.waitUntilReady();
        await this.maps.addPolygon(polygonInfo);
    }

    async centerMap(coordenada)
    {
        await this.waitUntilReady();
        await this.maps.centerMap(coordenada);
    }

    async showInfoWindow(content)
    {
        await this.waitUntilReady();
        await this.maps.showInfoWindow(content);
    }

    draggendCallback(positionEvent) {
        this.dotnetHelper.invokeMethodAsync('OnMarkerCoordenadaChanged', positionEvent);
    }

    clickMarkerCallback(markerClickEvent) {
        this.dotnetHelper.invokeMethodAsync('OnMarkerClick', markerClickEvent);
    }
}

export async function InitializeMapsBlazor(dotnetHelper, idMaps , options)
{
    try
    {
        let element = document.getElementById(idMaps);
        if (!element.mapsBlazor)
        {
            element.mapsBlazor = new MapsBlazor(dotnetHelper, idMaps, {
                apiKey: options.apiKey,
                center: options.center,
                zoom: options.zoom,
                clusterable: options.clusterable,
                idMap: idMaps
            });

            return true;
        }

        return false;
    }
    catch (error)
    {
        console.log(error);
        console.log(error.stack);
    }
    return false;
}

export async function AddMarker(idMaps, markerInfo)
{
    try
    {
        let element = document.getElementById(idMaps);

        if(!element.mapsBlazor)
        {
            await InitializeMapsBlazor(dotnetHelper, idMaps , options);
        }

        if (element.mapsBlazor)
        {
            await element.mapsBlazor.addMarker(markerInfo);
        }
    }
    catch (error)
    {
        console.log(error);
    }
    return true;
}

export async function CenterMap(idMaps, coordenada)
{
    try
    {
        let element = document.getElementById(idMaps);
        if(!element.mapsBlazor)
        {
            await InitializeMapsBlazor(dotnetHelper, idMaps , options);
        }
        if (element.mapsBlazor)
        {
            await element.mapsBlazor.centerMap(coordenada);
        }
    }
    catch (error)
    {
        console.log(error);
    }
    return true;
}

export async function AddPolygon(idMaps , polygonInfo)
{
    try
    {
        let element = document.getElementById(idMaps);
        if (element.mapsBlazor)
        {
            await element.mapsBlazor.addPolygon(polygonInfo);
        }
    }
    catch (error)
    {
        console.log(error);
    }
    return true;
}

export async function ShowInfoWindow(idMaps, infoWindowInfo)
{
    try
    {
        let element = document.getElementById(idMaps);
        if (element.mapsBlazor)
        {
            await element.mapsBlazor.showInfoWindow(infoWindowInfo);
        }
    }
    catch (error)
    {
        console.log(error);
    }
}

export function DisposeMaps(idMaps) 
{
    try
    {
        let element = document.getElementById(idMaps);
        if (element && element.mapsBlazor) {
            
            if (element.mapsBlazor.maps && typeof element.mapsBlazor.maps.dispose === 'function') 
            {
                element.mapsBlazor.maps.dispose();
            }
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

export async function InitializeAndObjects(dotnetHelper, idMaps, options, markerInfo, polygonInfo)
{
    try
    {
        let element = document.getElementById(idMaps);

        if (!element.mapsBlazor)
            await InitializeMapsBlazor(dotnetHelper, idMaps, options);
        
        if (element.mapsBlazor)
        {
            if (markerInfo)
                await element.mapsBlazor.addMarker(markerInfo);

            if (polygonInfo)
                await element.mapsBlazor.addPolygon(polygonInfo);
        }
    }
    catch (error) {
        console.log(error);
    }
    return true;
}