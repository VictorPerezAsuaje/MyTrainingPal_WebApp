const LoadThisWeekWorkouts = (dataDTO) =>
{
	let chart = new CanvasJS.Chart("chartContainer", {
		animationEnabled: true,

		axisX: {
			interval: 1,
			intervalType: "day",
		},
		axisY: {
			suffix: "%"
		},
		toolTip: {
			shared: true
		},
		legend: {
			reversed: true,
			verticalAlign: "center",
			horizontalAlign: "right"
		},
		data: [...dataDTO].map(x => {
			return {
				type: "stackedColumn100",
				name: x.key,
				showInLegend: true,
				yValueFormatString: "#,##0\"\"",
				dataPoints: x.value.map(y => {
					return {
						x: new Date(y.xPoint),
						y: y.yPoint
                    }
                })
			};
        }),
	});
	chart.render();
}