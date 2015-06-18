<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ui_baxter.aspx.cs" Inherits="ZGZY.WebUI.admin.html.ui_baxter" %>

<!DOCTYPE HTML>
<html>
	<head>
		<meta http-equiv="Content-Type" content="text/html; charset=utf-8">
		<title>百特历史库存统计</title>

		
		<script type="text/javascript">
		    $(function () {

		        //汉化
		        Highcharts.setOptions({
		            lang: {
		                downloadJPEG: "下载JPEG 图片",

		                downloadPDF: "下载PDF文档",

		                downloadPNG: "下载PNG 图片",

		                downloadSVG: "下载SVG 矢量图",

		                exportButtonTitle: "导出图片",

		                printButtonTitle: "打印图表",

		                months: ['一月', '二月', '三月', '四月', '五月', '六月', '七月', '八月', '九月', '十月', '十一月', '十二月'],
		                numericSymbols: ["千", "M", "G", "T", "P", "E"],
		                shortMonths: ['1月', '2月', '3月', '4月', '5月', '6月', '7月', '8月', '9月', '10月', '11月', '12月'],
		                weekdays: ['周一', '周二', '周三', '周四', '周五', '周六', '周日']
		            }
		        });


		        // set the allowed units for data grouping
		        var groupingUnits = [[
                    'week',                         // unit name
                    [1]                             // allowed multiples
		        ], [
                    'month',
                    [1, 2, 3, 4, 6]
		        ]];

		        // create the chart
		        $('#container').highcharts('StockChart', {



		            navigator: {
		                series: {
		                    data: [<%=inv_str.ToString()%>]
		                    }
		                },

		                rangeSelector: {
		                    selected: 0
		                },

		                title: {
		                    text: '百特历史库存统计'
		                },


		                yAxis: [


		                    {

		                        title: {
		                            text: '出入库托数'
		                        },


		                        height: 200,
		                        lineWidth: 2,
		                        min: 0
		                    }, {
		                        title: {
		                            text: '库存托数'
		                        },
		                        plotLines: [{
		                            color: 'red',            //线的颜色，定义为红色
		                            dashStyle: 'solid',     //默认是值，这里定义为长虚线
		                            value: 11600,              //定义在那个值上显示标示线，这里是在x轴上刻度为3的值处垂直化一条线
		                            width: 2,               //标示线的宽度，2px
		                            label: {
		                                text: '库存上限'
		                            }
		                        }],
		                        top: 300,
		                        height: 100,
		                        offset: 0,
		                        lineWidth: 2,
		                        min: 0

		                    }],


		                series: [{
		                    type: 'spline',

		                    name: '入库托数',
		                    data: [<%=in_str.ToString()%>],
		                    dataGrouping: {
		                        units: groupingUnits
		                    }
		                },
                        {
                            type: 'spline',

                            name: '出库托数',
                            data: [<%=out_str.ToString()%>],
                            dataGrouping: {
                                units: groupingUnits
                            }
                        }, {
                            type: 'column',

                            name: '库存托数',
                            data: [<%=inv_str.ToString()%>],
		                    yAxis: 1

                        }]
		            });

		    });
		</script>
	</head>
	<body>

        <div id="container" style="height: 500px"></div>
	</body>
</html>
