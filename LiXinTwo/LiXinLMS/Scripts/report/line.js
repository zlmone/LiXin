function DrawImageLine(Data)
{
    var chart = {
        chart: {
            renderTo: Data.DivID,
            type: 'line'
        },
        title: {
            text: Data.title,
            x: -20 //center
        },
        subtitle: {
            text: Data.subtitle,
            x: -20
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
            title: {
                text: Data.yText
            },
            plotLines: [{
                value: 0,
                width: 1,
                color: '#808080'
            }],
            min: Data.ymin

        },
        tooltip: {
            formatter: function ()
            {
                return '<b>' + this.series.name + '</b><br/>' + this.x + ': ' + this.y;
            }
        },
        legend: {
            enabled: true
        },
        series: Data.series
    };
    return chart;
}