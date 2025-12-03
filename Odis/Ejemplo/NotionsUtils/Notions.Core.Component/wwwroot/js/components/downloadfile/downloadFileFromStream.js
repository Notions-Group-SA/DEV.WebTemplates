class QuillBlazor
{
    constructor(dotnetHelper, idDiv, options)
    {
        this.containter = document.getElementById(idDiv);
        this.dotnetHelper = dotnetHelper;

        this.quill = new Quill('#' + idDiv, { theme: 'snow' }, options);

        this.quill.on('text-change', (delta, oldContents, source) => this.textChange(delta, oldContents, source));

        this.quill.on('selection-change', (range, oldRange, source) => this.focusOut(range, oldRange, source));
    }

    textChange(delta, oldContents, source)
    {
        try
        {
            if (this.dotnetHelper && source === 'user')
            {
                //const content = this.quill.root.innerHTML;
                //this.dotnetHelper.invokeMethodAsync('OnContentChanged', content);
            }
            else
                console.log('notificar: dotnetHelper es null');
        }
        catch (error) {
            console.log(error);
        }
    }

    focusOut(range, oldRange, source) 
    {
        try {
            if (range === null) {
                // Esto es equivalente a focusout/blur
                console.log('El editor perdió el foco');

                const content = this.quill.root.innerHTML;
                this.dotnetHelper.invokeMethodAsync('OnContentChanged', content);

            } else if (oldRange === null) {
                // Esto es equivalente a focusin/focus
                console.log('El editor recibió el foco');
            }
        }
        catch (error) {
            console.log(error);
        }
    }

    getContents()
    {
        try
        {
            if (this.quill)
                return this.quill.root.innerHTML;
            else
                console.log('notificar: dotnetHelper es null');
        }
        catch (error)
        {
            console.log(error);
        }
        return '';
    }

    setContents(contents)
    {
        try
        {
            if (this.quill)
            {
                this.quill.root.innerHTML = contents;
            }
        }
        catch (error)
        {
            console.log(error);
        }
    }
}

export function InitializeQuillBlazor(dotnetHelper, idDiv, options)
{
    try
    {
        let element = document.getElementById(idDiv)
        if (!element.quillBlazor)
        {
            element.quillBlazor = new QuillBlazor(dotnetHelper, idDiv, options);
            return true;
        }
    }
    catch (error)
    {
        console.log(error);
    }
    return false;
}

export function getContents(idDiv)
{
    try
    {
        let element = document.getElementById(idDiv);

        if (element.quillBlazor)
            return element.quillBlazor.getContents();
    }
    catch (error) {
        console.log(error)
    }
    return '';
}

export function setContents(idDiv, contents)
{
    try
    {
        let element = document.getElementById(idDiv);

        if (element.quillBlazor)
            element.quillBlazor.setContents(contents);

    }
    catch (error) {
        console.log(error)
    }
}