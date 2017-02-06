function DrawImageradar(Data) {
    var chart = {
        chart: {
            renderTo: Data.DivID,
            type: 'line',
            polar: 1
        },
        title: {
            text: Data.title
        },
        subtitle: {
            text: Data.subtitle
        },
        pane: {
            center: ['50%', '50%'],
            size: '90%',
            startAngle: 0,
            endAngle: 360
        },
        xAxis: {
            categories: Data.xAxis
        },
        yAxis: {
            min: 0,
            tickPixelInterval: 50
        },
        tooltip: {
            formatter: function() {
                return '' + this.x + ':' + this.y;
            }
        },
        plotOptions: {
            column: {
                stacking: 'normal',
                pointPadding: 0,
                groupPadding: 0
            }
        },
        series: Data.series
    };
    return chart;
}