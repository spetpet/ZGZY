<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ui_test.aspx.cs" Inherits="ZGZY.WebUI.admin.html.ui_test" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
</head>
<body>
    		<script type="text/javascript">



    		    $(function () {



    		        var colors = Highcharts.getOptions().colors,
                        categories = ['一楼库存构成', '二楼库存构成', '三楼库存构成', ],
                        name = '库存结构';
    		        
                    var data=<%=json_sb.ToString()%> ;
    		        var browserData = [];
    		        var versionsData = [];
    		        for (var i = 0; i < data.length; i++) {

    		            // add browser data
    		            browserData.push({
    		                name: categories[i],
    		                y: data[i].y,
    		                color: data[i].color
    		            });

    		            // add version data
    		            for (var j = 0; j < data[i].drilldown.data.length; j++) {
    		                var brightness = 0.2 - (j / data[i].drilldown.data.length) / 5;
    		                versionsData.push({
    		                    name: data[i].drilldown.categories[j],
    		                    y: data[i].drilldown.data[j],
    		                    color: Highcharts.Color(data[i].color).brighten(brightness).get()
    		                });
    		            }
    		        }

    		        // Create the chart
    		        $('#container').highcharts({
    		            credits: {
    		                enabled: false
    		            },
    		            chart: {
    		                type: 'pie'
    		            },
    		            title: {
    		                text: '实时库存查询',
    		                style:{"fontSize": "30px"}
    		            },
    		            yAxis: {
    		                title: {
    		                    text: '库存托数'
    		                }
    		            },
    		            plotOptions: {
    		                pie: {
    		                    shadow: false,
    		                    center: ['50%', '50%']
    		                }
    		            },
    		            tooltip: {
    		                valueSuffix: '托'
    		            },
    		            series: [{
    		                name: '楼层',
    		                data: browserData,
    		                size: '60%',
    		                dataLabels: {
    		                    formatter: function () {
    		                        return this.y > 5 ? this.point.name : null;
    		                    },
    		                    color: 'black',
    		                    distance: -30,
    		                    style:{"fontSize": "15px"}
    		                }
    		            }, {
    		                name: '货主',
    		                data: versionsData,
    		                size: '80%',
    		                innerSize: '60%',
    		                dataLabels: {
    		                    formatter: function () {
    		                        // display only if larger than 1
    		                        return this.y > 5 ? '<b>' + this.point.name + ':</b> ' + this.y + '托' : null;
    		                    }
    		                }
    		            }]
    		        });
    		    });


		</script>


<div id="ui_test_layout" class="easyui-layout" data-options="fit:true,border:false">
    <div data-options="region:'center',border:false">
        <div id="container" style="width: 600px; height: 500px; margin: 50px auto"></div>
    </div>
</div>
</body>
</html>
