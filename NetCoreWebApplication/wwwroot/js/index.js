$(function () {

    // createProductMenus();
    // statisticsAnalysis("second");
    //  initEChart();
});

let initEChart = function (xAxisData, yAxisData, timeUint) {
    // 基于准备好的dom，初始化echarts实例
    //不能通过Jquery获取id
    var myChart = echarts.init(document.getElementById('main'));

    // 指定图表的配置项和数据
    var option = {
        title: {
            text: 'OCG请求量',
            subtext: '上海直达软件技术有限公司',
            left: 'center'
        },
        tooltip: {
            trigger: 'axis',
            axisPointer: {
                animation: false
            }
        },
        legend: {
            data: ['请求数量'],
            left: 10
        },
        toolbox: {
            feature: {
                dataZoom: {
                    yAxisIndex: 'none'
                },
                restore: {},
                saveAsImage: {}
            }
        },

        axisPointer: {
            link: {xAxisIndex: 'all'}
        },

        dataZoom: [
            {
                show: true,
                realtime: true,
                start: 30,
                end: 70
                // xAxisIndex: [0, 1]
            }
        ],

        grid: [{
            left: 50,
            right: 50,
            bottom: '70px'
            // height: '75%'
        }],


        xAxis: {
            type: 'category',
            boundaryGap: false,
            axisLine: {onZero: true},
            data: xAxisData
            // axisLabel: {
            //     interval: 0,//横轴信息全部显示
            //     rotate: 60,//60度角倾斜显示
            //     formatter: function (val) {
            //         return val.split("").join("\n");
            //     } //横轴信息文字竖直显示
            // }

        },
        yAxis: {
            name: timeUint,
            type: 'value',
            minInterval: 1
        },
        series: [{
            name: '请求数量',
            type: 'line',
            hoverAnimation: false,
            data: yAxisData
        }]
    };

    // 使用刚指定的配置项和数据显示图表。
    myChart.setOption(option, true);

    //根据窗口的大小变动图表 --- 重点
    window.onresize = function(){
        myChart.resize();
        //myChart1.resize();    //若有多个图表变动，可多写

    }
};
let statisticsAnalysis = function () {

    let filePath = $("#uploadPath").val();
    if (filePath == "") {
        alert("选择上传的文件");
        return;
    }
    let statisticsKind = $("#statisticsKind").val();
    let timeUint = "条/s";
    switch (statisticsKind) {
        case "second":
            timeUint = "条/s";
            break;
        case "minute":
            timeUint = "条/min";
            break;
        case "hour":
            timeUint = "条/h";
            break;
    }
    // // //var map = new Map();
    // let url="/statisticsAnalysis?unitStr='second'";
    // $.get(url, function (data) {
    //     debugger;
    // });

    $.ajax({
        type: "get",//向后台请求的方式，有post，get两种方法
        url: "/statisticsAnalysis",//url填写的是请求的路径
        cache: false,//缓存是否打开
        data: {//请求的数据，
            fileName: $("#uploadPath").val(),
            unitStr: statisticsKind
        },
        dataType: 'json',//请求的数据类型
        success:

            function (data) {//请求的返回成功的方法

                let xAxisData = new Array();
                let yAxisData = new Array();
                //处理后台返回的HashMap
                for (let key in data) {
                    //key    data[key]
                    // console.log(key + "  " + data[key]);
                    xAxisData.push(key);
                    yAxisData.push(data[key]);
                }

                initEChart(xAxisData, yAxisData, timeUint);
            }

        ,
        error: function (XMLHttpRequest, textStatus, errorThrown) {//请求的失败的返回的方法
            alert("服务异常！");
        }

    })
    ;

    // $.ajax({
    //     type: "post",//向后台请求的方式，有post，get两种方法
    //     url: "/deleteUser",//url填写的是请求的路径
    //     cache: false,//缓存是否打开
    //     data: {//请求的数据，
    //         "userIds": userId
    //     },
    //     dataType: 'json',//请求的数据类型
    //     success: function (data) {//请求的返回成功的方法
    //         if (data && data.success) {
    //             alert("已经删除成功。");
    //             postData();
    //         } else {
    //             alert("删除失败，原因：" + data.msg);
    //         }
    //     },
    //     error: function (XMLHttpRequest, textStatus, errorThrown) {//请求的失败的返回的方法
    //         alert("删除失败，请稍后再次尝试删除！");
    //     }
    //
    // });
};

$("#statisticsAnalysis").click(function (e) {

    statisticsAnalysis();
});
$("#statisticsKind").change(function () {

    statisticsAnalysis();
});

$("#upload").click(function (e) {
    let type = "file";          //后台接收时需要的参数名称，自定义即可
    let id = "uploadFile";            //即input的id，用来寻找值
    let formData = new FormData();
    formData.append(type, $("#" + id)[0].files[0]);    //生成一对表单属性
    $.ajax({
        type: "POST",           //因为是传输文件，所以必须是post
        url: '/upload',         //对应的后台处理类的地址
        data: formData,
        processData: false,
        contentType: false,
        success: function (data) {
            alert('上传成功！');
            // alert(data);
            $("#uploadPath").val(data);
        }
    });
});


/*
按钮出发File,File-->change-->Input显示路径
 */
$("#openFile").click(function () {
    $("#uploadFile").click();

});


$("#uploadFile").change(function () {

    // $("#selectFilePath").val($("#uploadFile").val());

    // $("#selectFilePath").val($("#uploadFile").val());
    // let file = $("#uploadFile");
    let fullName = $("#uploadFile").val();
    let fileNames = fullName.split("\\");
    let fileName=fileNames[fileNames.length-1]
    $("#selectFilePath").val(fileName);
});







