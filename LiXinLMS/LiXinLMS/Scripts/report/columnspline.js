function DrawImagecs(Data) {
    var chart = {
        chart: {
            renderTo: Data.DivID
        },
        title: {
            text: Data.title
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
                text: Data.yText
            }
        },
        tooltip: {
            formatter: function() {
                var s;
                if (this.point.name) {
                    s = '' +
                        this.point.name + ': ' + this.y;
                } else {
                    s = '' + this.x + ': ' + this.y;
                }
                return s;
            }
        },
        series: [{
                type: 'column',
                name: Data.series[0].name,
                data: Data.series[0].data
            }, {
                type: 'spline',
                name: Data.series[1].name,
                data: Data.series[1].data
            }]
    };
    return chart;
}