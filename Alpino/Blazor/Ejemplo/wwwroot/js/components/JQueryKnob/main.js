class JQueryKnob {
    constructor(idElement) {
        this.instances = new Map();
        this.loadedResources = {
            knob: false
        };
        this.element = $('#' + idElement);
        this.initializeKnob();
    }

    async loadResources() {
        if (!this.loadedResources.knob) {
            try {
                await this.loadScript('assets/plugins/jquery-knob/jquery.knob.min.js');
                this.loadedResources.knob = true;
                console.log('jquery.knob.min.js resources loaded successfully');
            }
            catch (error) {
                console.error('Error loading jquery.knob.min.js resources:', error);
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

    async initializeKnob() {
        try {
            await this.loadResources();
            console.log('JQueryKnobBlazor initialized successfully');

            // Definir la función draw directamente en el callback
            this.element.knob({
                draw: function () {
                    // Aquí 'this' se refiere a la instancia del jQuery Knob
                    if (this.$.data('skin') == 'tron') {
                        var a = this.angle(this.cv)        // Angle
                            , sa = this.startAngle          // Previous start angle
                            , sat = this.startAngle         // Start angle
                            , ea                            // Previous end angle
                            , eat = sat + a                 // End angle
                            , r = true;

                        this.g.lineWidth = this.lineWidth;

                        this.o.cursor
                            && (sat = eat - 0.3)
                            && (eat = eat + 0.3);

                        if (this.o.displayPrevious) {
                            ea = this.startAngle + this.angle(this.value);
                            this.o.cursor
                                && (sa = ea - 0.3)
                                && (ea = ea + 0.3);
                            this.g.beginPath();
                            this.g.strokeStyle = this.previousColor;
                            this.g.arc(this.xy, this.xy, this.radius - this.lineWidth, sa, ea, false);
                            this.g.stroke();
                        }

                        this.g.beginPath();
                        this.g.strokeStyle = r ? this.o.fgColor : this.fgColor;
                        this.g.arc(this.xy, this.xy, this.radius - this.lineWidth, sat, eat, false);
                        this.g.stroke();

                        this.g.lineWidth = 2;
                        this.g.beginPath();
                        this.g.strokeStyle = this.o.fgColor;
                        this.g.arc(this.xy, this.xy, this.radius - this.lineWidth + 1 + this.lineWidth * 2 / 3, 0, 2 * Math.PI, false);
                        this.g.stroke();

                        return false;
                    }
                }
            });

            return true;
        }
        catch (error) {
            console.error('Error initializing JQueryKnob/main.js:', error);
            return false;
        }
    }

    destroyKnob() {
        if (this.instances) {
            try {
                if (this.element.data('knob')) {
                    this.element.trigger('configure', { draw: null });
                }
                this.instances.delete(this.element);
            } catch (error) {
                console.warn('Error destruyendo Knob:', error);
            }
        }
    }
}
export function initializeKnob(idElement) {
    try {
        const element = document.getElementById(idElement);
        if (!element.jqueryknob) {
            element.jqueryknob = new JQueryKnob(idElement);
        }
        return true;
    }
    catch (exception) {
        console.log(exception);
    }
    return false;
}

export function drawDial(idElement) {
    try {
        $('#' + idElement).trigger('change');
        return true;
    }
    catch (exception) {
        console.log(exception);
    }
    return false;
}

export function updateKnobValue(idElement, newValue) {
    try {
        const element = document.getElementById(idElement);
        if (element && element.jqueryknob) {
            $('#' + idElement).val(newValue).trigger('change');
            return true;
        }
    }
    catch (exception) {
        console.log(exception);
    }
    return false;
}

export function destroyKnob(idElement) {
    try {
        const element = document.getElementById(idElement);
        if (element && element.jqueryknob) {
            return element.jqueryknob.destroyKnob();
        }
    }
    catch (exception) {
        console.log(exception);
    }
    return false;
}