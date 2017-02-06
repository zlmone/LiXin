function DrawImagePie(Data) {
    var chart = {
        chart: {
            renderTo: Data.DivID,
            type: 'pie',
            plotBackgroundColor: null,
            plotBorderWidth: null,
            plotShadow: false
        },
        title: {
            text: Data.title
        },
        tooltip: {
            formatter: function ()
            {
                if (Data.reportFormat == 1)
                {
                    return '<b>' + Data.PerName + this.point.name + '</b>: ' + '<b>' + Data.PerY + this.point.y + '</b>,' + this.percentage.toFixed(2) + ' %';
                }
                else
                {
                    return '<b>' + this.point.name + '</b>: '+ this.percentage.toFixed(2) + ' %';
                }
                
            }
        },
        plotOptions: {
            pie: {
                allowPointSelect: true,
                cursor: 'pointer',
                dataLabels: {
                    enabled: true,
                    color: '#000000',
                    connectorColor: '#000000',
                    formatter: function ()
                    {
                        if (Data.reportFormat == 1)
                        {
                            return '<b>' + Data.PerName + this.point.name + '</b>: ' + '<b>' + Data.PerY + this.point.y + '</b>,' + this.percentage.toFixed(2) + ' %';
                        }
                        else
                        {
                            return '<b>' + this.point.name + '</b>: '  + this.percentage.toFixed(2) + ' %';
                        }
                    }
                },
                showInLegend: true
            }
        },
        legend: {
            enabled: true
        },
        series: [{
            data: Data.pieseries
        }]
    };
    return chart;
}