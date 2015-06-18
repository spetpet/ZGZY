<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ui_floorview.aspx.cs" Inherits="ZGZY.WebUI.admin.html.ui_floorview" %>

<!DOCTYPE HTML>
<html>
	<head>
		<meta http-equiv="Content-Type" content="text/html; charset=utf-8">
		<title>Highcharts Example</title>


		
	</head>
	<body>
        
        <script type="text/javascript">
            $(function () {

                var gaugeOptions = {

                    chart: {
                        type: 'solidgauge'
                    },

                    title: null,

                    pane: {
                        center: ['50%', '85%'],
                        size: '140%',
                        startAngle: -90,
                        endAngle: 90,
                        background: {
                            backgroundColor: (Highcharts.theme && Highcharts.theme.background2) || '#EEE',
                            innerRadius: '60%',
                            outerRadius: '100%',
                            shape: 'arc'
                        }
                    },

                    tooltip: {
                        enabled: false
                    },
                    credits: {
                        enabled: false
                    },

                    // the value axis
                    yAxis: {
                        stops: [
                            [0.1, '#55BF3B'], // green
                            [0.5, '#DDDF0D'], // yellow
                            [0.9, '#DF5353'] // red
                        ],
                        lineWidth: 0,
                        minorTickInterval: null,
                        //tickPixelInterval: 4000,
                        tickWidth: 0,
                        title: {
                            y: -70
                        },
                        labels: {
                            y: 16
                        }
                    },

                    plotOptions: {
                        solidgauge: {
                            dataLabels: {
                                y: 5,
                                borderWidth: 0,
                                useHTML: true
                            }
                        }
                    }
                };

                // The speed gauge
                $('#floor1').highcharts(Highcharts.merge(gaugeOptions, {
                    yAxis: {
                        min: 0,
                        max: 3324,
                        
                        title: {
                            text: '一楼库存', style: {'fontSize':'20px'}
                        }
                    },

                    credits: {
                        enabled: false
                    },

                    series: [{
                        name: '托数（托数）',
                        data: [<%=f1.ToString()%>],
                        dataLabels: {
                            format: '<div style="text-align:center"><span style="font-size:25px;color:' +
                                ((Highcharts.theme && Highcharts.theme.contrastTextColor) || 'black') + '">{y}托</span><br/>' +
                                   '<span style="font-size:12px;color:silver">剩余<%=(3324-f1).ToString()%>个可用库位</span></div>'
                        },
                        tooltip: {
                            valueSuffix: ' 托'
                        }
                    }]

                }));

                
                $('#floor2').highcharts(Highcharts.merge(gaugeOptions, {
                    yAxis: {
                        min: 0,
                        max: 2725,
                        title: {
                            text: '二楼库存', style: { 'fontSize': '20px' }
                        }
                    },

                    series: [{
                        name: '托数',
                        data: [<%=f2.ToString()%>],
                        dataLabels: {
                            format: '<div style="text-align:center"><span style="font-size:25px;color:' +
                                ((Highcharts.theme && Highcharts.theme.contrastTextColor) || 'black') + '">{y}托</span><br/>' +
                                   '<span style="font-size:12px;color:silver">剩余<%=(2725-f2).ToString()%>个可用库位</span></div>'
                        },
                        tooltip: {
                            valueSuffix: ' revolutions/min'
                        }
                    }]

                }));

                $('#floor3').highcharts(Highcharts.merge(gaugeOptions, {
                    yAxis: {
                        min: 0,
                        max: 1206,
                        title: {
                            text: '三楼库存', style: { 'fontSize': '20px' }
                        }
                    },

                    series: [{
                        name: '托数',
                        data: [<%=f3.ToString()%>],
                        dataLabels: {
                            format: '<div style="text-align:center"><span style="font-size:25px;color:' +
                                ((Highcharts.theme && Highcharts.theme.contrastTextColor) || 'black') + '">{y}托</span><br/>' +
                                   '<span style="font-size:12px;color:silver">剩余<%=(1206-f3).ToString()%>个可用库位</span></div>'
                        },
                        tooltip: {
                            valueSuffix: ' revolutions/min'
                        }
                    }]

                }));


                $('#floor3_cold').highcharts(Highcharts.merge(gaugeOptions, {
                    yAxis: {
                        min: 0,
                        max: 678,
                        title: {
                            text: '三楼冷库库存', style: { 'fontSize': '20px' }
                        }
                    },

                    series: [{
                        name: '托数',
                        data: [<%=fc.ToString()%>],
                        dataLabels: {
                            format: '<div style="text-align:center"><span style="font-size:25px;color:' +
                                ((Highcharts.theme && Highcharts.theme.contrastTextColor) || 'black') + '">{y}托</span><br/>' +
                                   '<span style="font-size:12px;color:silver">剩余<%=(678-fc).ToString()%>个可用库位</span></div>'
                        },
                        tooltip: {
                            valueSuffix: ' revolutions/min'
                        }
                    }]

                }));
               


            });
		</script>
<div style="width: 1000px; height: 400px; margin: 0 auto">
	<div id="floor1" style="width: 300px; height: 200px; float: left"></div>
	<div id="floor2" style="width: 300px; height: 200px; float: left"></div>
    <div id="floor3" style="width: 300px; height: 200px; float: left"></div>
    <div id="floor3_cold" style="width: 300px; height: 200px; float: left"></div>
    
</div>


	</body>
</html>
