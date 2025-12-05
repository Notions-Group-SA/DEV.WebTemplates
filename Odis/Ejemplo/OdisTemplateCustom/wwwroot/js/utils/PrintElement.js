// ticketPdf.js - PDF handling for transaction tickets

// Global variable to store the PDF instance
let pdfInstance = null;
let pdfPromise = null;


const initPdf = function (elementId, fileName) {
    if (typeof html2pdf === 'undefined') {
        console.error('html2pdf is not loaded. Please include it via a script tag in your HTML.');
        return Promise.reject('html2pdf is not loaded');
    }

    const contentToPrint = document.getElementById(elementId);
    if (!contentToPrint) {
        console.error(`Element with ID ${elementId} not found`);
        return Promise.reject(`Element with ID ${elementId} not found`);
    }

    // Crear instancia y preparar el PDF (pero sin guardarlo todavía)
    pdfInstance = html2pdf().from(contentToPrint).set({
        filename: fileName,
        html2canvas: {
            scale: 2,
            useCORS: true,
            logging: true,
            scrollY: 0
        },
        jsPDF: { orientation: 'portrait' }
    });

    // Generar el PDF (esto tarda) y guardar la promesa
    pdfPromise = pdfInstance.toPdf(); // IMPORTANTE: no usamos .outputPdf ni .save directamente
    return pdfPromise;
}


// Initialize the PDF generation with the specified element and code
//const initPdf = function (elementId, fileName) {
//    // Check if html2pdf is available globally
//    if (typeof html2pdf === 'undefined') {
//        console.error('html2pdf is not loaded. Please include it via a script tag in your HTML.');
//        return Promise.reject('html2pdf is not loaded');
//    }

//    const contentToPrint = document.getElementById(elementId);
//    if (!contentToPrint) {
//        console.error(`Element with ID ${elementId} not found`);
//        return Promise.reject(`Element with ID ${elementId} not found`);
//    }

//    // Create the PDF instance with a promise to track when it's ready
//    pdfInstance = html2pdf().from(contentToPrint).set({
//        filename: fileName,
//        html2canvas: {
//            scale: 2,
//            useCORS: true,
//            logging: true,
//            scrollY: 0
//        },
//        jsPDF: { orientation: 'portrait' }
//    });

//    // Store the promise for the PDF generation
//    pdfPromise = pdfInstance.outputPdf('blob');

//    return pdfPromise;
//}

// Save the PDF to the user's device
const savePdf = function () {
    if (!pdfInstance) {
        console.error("PDF not initialized. Call initPdf first.");
        return Promise.reject("PDF not initialized");
    }

    return pdfPromise.then(() => {
        pdfInstance.save();
        return true;
    });
}

// Open the PDF in a new browser tab
const openPdf = function () {
    if (!pdfInstance || !pdfPromise) {
        console.error("PDF not initialized. Call initPdf first.");
        return Promise.reject("PDF not initialized");
    }

    return pdfPromise
        .then(() => {
            return pdfInstance.output('bloburl');
        })
        .then((bloburl) => {
            window.open(bloburl, '_blank');
            return true;
        })
        .catch(function (error) {
            console.error("Error opening PDF:", error);
            return false;
        });
}


// Share the PDF using the Web Share API (mobile devices)
const sharePdf = function (codigo) {
    if (!pdfInstance || !pdfPromise) {
        console.error("PDF not initialized. Call initPdf first.");
        return Promise.reject("PDF not initialized");
    }

    // Check if Web Share API is available
    if (!navigator.share) {
        console.error("Web Share API not supported in this browser");
        return Promise.reject("Web Share API not supported");
    }

    return pdfPromise
        .then(() => {
            return pdfInstance.output('blob');
        })
        .then((blob) => {
            const file = new File(
                [blob],
                `Entrada ${codigo}.pdf`,
                { type: 'application/pdf' }
            );

            const shareData = {
                title: 'Entrada',
                text: 'Entrada',
                files: [file]
            };

            return navigator.share(shareData);
        })
        .then(() => {
            console.log('Successful share');
            return true;
        })
        .catch((error) => {
            console.error('Error sharing', error);
            return false;
        });
}

// Export the functions as ES6 module
export { initPdf, savePdf, openPdf, sharePdf };