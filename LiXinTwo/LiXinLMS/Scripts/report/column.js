function DrawImageColumn(Data)
{
    var chart = {
        chart: {
            renderTo: Data.DivID,
            type: 'column'
        },
        title: {
            text: Data.title
        },
        subtitle: {
            text: Data.subtitle
        },
        xAxis: {
            categories: Data.xAxis,
            labels: {
                rotation: -45,
                align: 'right',
                style: {
                    fontSize: '11px',
                    fontFamily: '"Lucida Grande", "Lucida Sans Unicode", Verdana, Arial, Helvetica, sans-serif'
                }
            }
        },
        yAxis: {
            min: 0,
            title: {
                text: (Data.yText != null && Data.yText != undefined) ? Data.yText : ""
            }
        },
        tooltip: {
            formatter: function ()
            {
                return '' +
                    this.x + ': ' + this.y + ((Data.yText != null && Data.yText != undefined) ? Data.yText : "");
            }
        },
        plotOptions: {
            column: {
                pointPadding: 0.2,
                borderWidth: 0
            },
            showInLegend: true
        },
        legend:{
            enabled: true
        },
        series: Data.series
    };
    return chart;
}