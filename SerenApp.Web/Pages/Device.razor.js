export function RenderChart(chart, data) {
    new Chart(
        chart,
        {
            type: 'line',
            data,
            options: {
                scales: {
                    x: {
                        type: 'time',
                        display: true,
                    },
                    BodyTemperature: {
                        type: 'linear',
                        display: true,
                    },
                    BloodPressure: {
                        type: 'linear',
                        display: true,
                    },
                    BloodOxygen: {
                        type: 'linear',
                        display: true,
                    },
                    Battery: {
                        type: 'linear',
                        display: true,
                    },
                    HeartFrequency: {
                        type: 'linear',
                        display: true,
                    },
                    WalkCount: {
                        type: 'linear',
                        display: true,
                    }
                }
            }
        }
    );
}
