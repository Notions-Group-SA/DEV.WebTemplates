

export async function initializeSparkLine(idSparkLine, valores, options)
{
    try {

        let element = $('#' + idSparkLine);
        element.sparkline(valores, options );
        //element.sparkline(valores, {
        //    type: 'bar',
        //    barColor: '#00ced1',
        //    height: '40px',
        //    barWidth: 3,
        //    barSpacing: 5
        //});       
    }
    catch (exception) {
        console.log(exception)
    }

    return true;
}

