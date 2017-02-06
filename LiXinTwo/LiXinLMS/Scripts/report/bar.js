function DrawImageBar(Data) {
    var chart = {
        chart: {
            renderTo: Data.DivID,
            type: 'bar'
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
        credits: {
            enabled: false
        },
        tooltip: {
            formatter: function() {
                return '' +
                    this.x + ': ' + this.y + ((Data.yText != null && Data.yText != undefined) ? Data.yText : "");
            }
        },
        plotOptions: {
            bar: {
                dataLabels: {
                    enabled: true
                }
            }
        },
        series: Data.series
    };
    return chart;
}