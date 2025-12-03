class ChartBlazor {
    constructor() {
        this.instances = new Map();
        this.loadedResources = {
            chart: false
        };
    }

    async loadResources() {
        if (!this.loadedResources.chart) {
            try {
                await this.loadCSS('assets/css/main.css');
                await this.loadCSS('assets/css/color_skins.css');
                await this.loadScript('assets/plugins/chartjs/Chart.bundle.js');
                await this.loadScript('assets/plugins/chartjs/polar_area_chart.js');
                await this.loadScript('assets/bundles/mainscripts.bundle.js');
                this.loadedResources.chart = true;
                console.log('Chart.js resources loaded successfully');
            } catch (error) {
                console.error('Error loading Chart.js resources:', error);
                throw error;
            }
        }
    }

    loadScript(src) {
        return new Promise((resolve, reject) => {
            if (document.querySelector(`script[src="${src}"]`)) {
                console.log(`Script ${src} already loaded`);
                resolve();
                return;
            }
            const script = document.createElement('script');
            script.src = src;
            script.onload = () => {
                console.log(`Script loaded: ${src}`);
                resolve();
            };
            script.onerror = (error) => {
                console.error(`Error loading script: ${src}`, error);
                reject(error);
            };
            document.head.appendChild(script);
        });
    }

    loadCSS(href) {
        return new Promise((resolve, reject) => {
            if (document.querySelector(`link[href="${href}"]`)) {
                console.log(`CSS ${href} already loaded`);
                resolve();
                return;
            }
            const link = document.createElement('link');
            link.rel = 'stylesheet';
            link.href = href;
            link.onload = () => {
                console.log(`CSS loaded: ${href}`);
                resolve();
            };
            link.onerror = (error) => {
                console.error(`Error loading CSS: ${href}`, error);
                reject(error);
            };
            document.head.appendChild(link);
        });
    }

    async initializeChart() {
        try {
            await this.loadResources();
            console.log('ChartBlazor initialized successfully');
            return true;
        }
        catch (error) {
            console.error('Error initializing ChartBlazor:', error);
            return false;
        }
    }

    async drawChart(idCanvas, config) {
        try {
            await this.loadResources();

            if (typeof Chart === 'undefined') {
                throw new Error('Chart.js is not loaded');
            }

            const canvasElement = document.getElementById(idCanvas);
            if (!canvasElement) {
                throw new Error(`Canvas element with id '${idCanvas}' not found`);
            }

            const existingChart = getChart(idCanvas);
            if (existingChart) {
                console.log(`Destroying existing chart for canvas: ${idCanvas}`);
                existingChart.destroy();
                this.instances.delete(idCanvas);
            }

            const newChart = new Chart(canvasElement.getContext("2d"), config);

            this.instances.set(idCanvas, newChart);

            console.log(`Chart created successfully for canvas: ${idCanvas}`);
            return newChart;
        }
        catch (error) {
            console.error(`Error drawing chart for canvas ${idCanvas}:`, error);
            throw error;
        }
    }

    destroyChart(idCanvas) {
        const chart = this.instances.get(idCanvas);
        if (chart) {
            chart.destroy();
            this.instances.delete(idCanvas);
            console.log(`Chart destroyed for canvas: ${idCanvas}`);
            return true;
        }
        return false;
    }

    getChart(idCanvas) {
        return this.instances.get(idCanvas);
    }

    updateChart(idCanvas, newConfig) {
        const chart = this.instances.get(idCanvas);
        if (chart) {
            Object.assign(chart.config, newConfig);
            chart.update();
            console.log(`Chart updated for canvas: ${idCanvas}`);
            return true;
        }
        console.warn(`No chart found for canvas: ${idCanvas}`);
        return false;
    }

    dispose() {
        this.instances.forEach((chart, canvasId) => {
            chart.destroy();
            console.log(`Chart destroyed for canvas: ${canvasId}`);
        });
        this.instances.clear();
        console.log('All charts disposed');
    }
}

export async function initializeChart(idCanvas) {
    try {
        let element = document.getElementById(idCanvas);
        element.chartBlazor = new ChartBlazor();
        return await element.chartBlazor.initializeChart();
    }
    catch (exception) {
        console.log(exception)
    }
}

export async function drawChart(idCanvas, config) {
    try {
        let element = document.getElementById(idCanvas);
        return await element.chartBlazor.drawChart(idCanvas, config);
    }
    catch (exception) {
        console.log(exception)
    }
}

export function destroyChart(idCanvas) {
    try {
        let element = document.getElementById(idCanvas);
        return element.chartBlazor.destroyChart(idCanvas);
    }
    catch (exception) {
        console.log(exception)
    }
}

export function getChart(idCanvas) {
    try {
        let element = document.getElementById(idCanvas);
        return element.chartBlazor.getChart(idCanvas);
    }
    catch (exception) {
        console.log(exception)
    }
}

export function updateChart(idCanvas, newConfig) {
    try {
        let element = document.getElementById(idCanvas);
        return element.chartBlazor.updateChart(idCanvas, newConfig);
    }
    catch (exception) {
        console.log(exception)
    }
}

export function disposeAllCharts() {
    try {
        let element = document.getElementById(idCanvas);
        return element.chartBlazor.dispose();
    }
    catch (exception) {
        console.log(exception)
    }
}