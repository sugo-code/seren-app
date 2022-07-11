const defaultChartOpts = {
    type: 'line',
    options: {
        responsive: true,
        interaction: {
            mode: 'index',
            intersect: false,
        },
        plugins: {
            legend: {
                position: 'top',
            },
            decimation: {
                enabled: true,
            }
        },
        stacked: false,
        scales: {
            x: {
                type: 'time',
                time: {
                    unit: 'hour'
                }
            },
        }
    }
}

export function RenderHeartChart(chart, data) {
    const opts = {
        ...defaultChartOpts,
        data,
        options: {
            ...defaultChartOpts.options,
            scales: {
                ...defaultChartOpts.options.scales,
                yBloodPressure: {
                    type: 'linear',
                    position: 'right',
                    min: 50,
                    max: 200
                },
                yHeartFrequency: {
                    type: 'linear',
                    position: 'left',
                    min: 30,
                    max: 220
                }
            }
        }
    }
    new Chart(chart, opts);
}

export function RenderBodyChart(chart, data) {
    const opts = {
        ...defaultChartOpts,
        data,
        options: {
            ...defaultChartOpts.options,
            scales: {
                ...defaultChartOpts.options.scales,
                yBodyTemperature: {
                    type: 'linear',
                    position: 'right',
                    min: 30,
                    max: 50
                },
                yBloodOxygen: {
                    type: 'linear',
                    position: 'left',
                    min: 60,
                    max: 100
                }
            }
        }
    }
    new Chart(chart, opts);
}


export function RenderBatteryChart(chart, data) {
    const opts = {
        ...defaultChartOpts,
        data,
        options: {
            ...defaultChartOpts.options,
            scales: {
                ...defaultChartOpts.options.scales,
                yBattery: {
                    type: 'linear',
                    position: 'left',
                    min: 0,
                    max: 100
                }
            }
        }
    }
    new Chart(chart, opts);
}
