<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="bi_map_stacked_area_order_list.aspx.cs" Inherits="DTcms.Web.admin.orders.bi_map_stacked_area_order_list" %>
<%@ Import namespace="DTcms.Common" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head>
<title>统计列表</title>
<link type="text/css" rel="stylesheet" href="../../scripts/ui/skins/Aqua/css/ligerui-all.css" />
<link type="text/css" rel="stylesheet" href="../images/style.css" />
<link type="text/css" rel="stylesheet" href="../../css/pagination.css" />
<link rel="stylesheet" type="text/css" href="../css/common.css" />
<script type="text/javascript" src="../../scripts/jquery/jquery-1.11.0.min.js"></script>
<script type="text/javascript" src="../../scripts/ui/js/ligerBuild.min.js"></script>
<script type="text/javascript" src="../../scripts/datepicker/js/bootstrap-datepicker.js"></script>
<link type="text/css" rel="stylesheet" href="../../scripts/datepicker/css/datepicker.css" />
<script type="text/javascript" src="../js/function.js"></script>
    <script type="text/javascript" src="../js/highcharts.js"></script>
<script type="text/javascript">
    $(function () {
        $('#txtBeginTime').datepicker({
            format: 'yyyy-mm-dd'
        });
        $('#txtEndTime').datepicker({
            format: 'yyyy-mm-dd'
        });
        
        Highcharts.theme = {
            colors: ["#7cb5ec", "#f7a35c", "#90ee7e", "#7798BF", "#aaeeee", "#ff0066", "#eeaaee",
               "#55BF3B", "#DF5353", "#7798BF", "#aaeeee"],
            chart: {
                backgroundColor: null
            },
            title: {
                style: {
                    fontSize: '16px',
                    fontWeight: 'bold',
                    textTransform: 'uppercase'
                }
            },
            tooltip: {
                borderWidth: 0,
                backgroundColor: 'rgba(219,219,216,0.8)',
                shadow: false
            },
            legend: {
                itemStyle: {
                    fontWeight: 'bold',
                    fontSize: '13px'
                }
            },
            xAxis: {
                gridLineWidth: 1,
                labels: {
                    style: {
                        fontSize: '12px'
                    }
                }
            },
            yAxis: {
                minorTickInterval: 'auto',
                title: {
                    style: {
                        textTransform: 'uppercase'
                    }
                },
                labels: {
                    style: {
                        fontSize: '12px'
                    }
                }
            },
            plotOptions: {
                candlestick: {
                    lineColor: '#404048'
                }
            },


            // General
            background2: '#F0F0EA'

        };

        // Apply the theme
        Highcharts.setOptions(Highcharts.theme);
        $('#container').highcharts({
            chart: {
                type: 'column'
            },
            title: {
                text: '各区域销售柱形堆叠图'
            },
            xAxis: {
                categories: [<%=x_title%>]
            },
            yAxis: {
                min: 0,
                title: {
                    text: 'Total fruit consumption'
                },
                stackLabels: {
                    enabled: true,
                    style: {
                        fontWeight: 'bold',
                        color: (Highcharts.theme && Highcharts.theme.textColor) || 'gray'
                    }
                }
            },
            legend: {
                align: 'right',
                x: -30,
                verticalAlign: 'top',
                y: 25,
                floating: true,
                backgroundColor: (Highcharts.theme && Highcharts.theme.background2) || 'white',
                borderColor: '#CCC',
                borderWidth: 1,
                shadow: false
            },
            tooltip: {
                formatter: function () {
                    return '<b>' + this.x + '</b><br/>' +
                        this.series.name + ': ' + this.y + '元<br/>' +
                        'Total: ' + this.point.stackTotal+'元';
                }
            },
            plotOptions: {
                column: {
                    stacking: 'normal',
                    dataLabels: {
                        enabled: true,
                        color: (Highcharts.theme && Highcharts.theme.dataLabelsColor) || 'white',
                        style: {
                            textShadow: '0 0 3px black'
                        }
                    }
                }
            },
            series: [<%=rtn%>]
        });
    });
</script>

</head>
<body class="mainbody">
<form id="form1" runat="server">
    <div class="tools_box">
	    <div class="tools_bar">
            <div class="search_box">
                总销售量<%=totalamount%>
                <asp:TextBox ID="txtBeginTime" runat="server" CssClass="txtInput"></asp:TextBox>
                <asp:TextBox ID="txtEndTime" runat="server" CssClass="txtInput"></asp:TextBox>
                <asp:Button ID="btnSearch" runat="server" Text="搜 索" CssClass="btnSearch" onclick="btnSearch_Click" />
		    </div>   
            <div style="margin-top: 7px">
                <span style="display:inline-block;color:Black;background-color:rgb(230,230,230);height: 30px;line-height: 30px;width: 150px;text-align: center;font-size: 18px;cursor:pointer;" onclick="location.href='bi_map_area_order_list.aspx'">折线图</span>
                <span style="display:inline-block;color:White;background-color:rgb(0,161,233);height: 30px;line-height: 30px;width: 150px;text-align: center;font-size: 18px;cursor:pointer;" onclick="location.href='bi_map_stacked_area_order_list.aspx'">柱形堆叠图</span>
            </div>         
        </div>        
    </div>
    <div id="container" style="width:100%; height:400px;"></div>
</form>
</body>
</html>
