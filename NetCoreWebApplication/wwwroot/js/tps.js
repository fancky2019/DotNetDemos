var $table = $("#myTable");
var TableInit = function () {
    var oTable = new Object();
    oTable.QueryUrl = '/tpsData?';
    oTable.Init = function () {
        $table.bootstrapTable({


            //url: '/Home/GetList',     //请求后台的URL（*）
            //method: 'post',                      //请求方式（*）
            //toolbar: '#toolbar',                //工具按钮用哪个容器
            //striped: true,                      //是否显示行间隔色
            //cache: false,                       //是否使用缓存，默认为true，所以一般情况下需要设置一下这个属性（*）
            //pagination: true,                   //是否显示分页（*）
            //sortable: false,                     //是否启用排序
            //sortOrder: "asc",                   //排序方式
            //queryParams: oTable.queryParams,//传递参数（*）
            //sidePagination: "server",           //分页方式：client客户端分页，server服务端分页（*）
            //pageNumber: 1,                       //初始化加载第一页，默认第一页
            //pageSize: 10,                       //每页的记录行数（*）
            //pageList: [10, 25, 50, 100],        //可供选择的每页的行数（*）
            //strictSearch: true,
            //clickToSelect: true,                //是否启用点击选中行
            //height: 460,                        //行高，如果没有设置height属性，表格自动根据记录条数觉得表格高度
            //uniqueId: "id",                     //每一行的唯一标识，一般为主键列
            //cardView: false,                    //是否显示详细视图
            //detailView: false,                   //是否显示父子表


            method: 'GET',//数据请求方式
            url: oTable.QueryUrl,//请求数据的地址
            contentType: "json",
            toolbar: '#toolbar',                //工具按钮用哪个容器
            striped: true,
            pagination: true,
            onlyInfoPagination: false,
            pageNumber: 1,
            pageSize: 10,
            pageList: [10, 25, 50, 100],
            uniqueId: "ID",
            sidePagination: "server", //服务端请求client
            queryParams: oTable.queryParams,
            queryParamsType: "limit",
            sortOrder: "desc",//默认排序方式，降序排列
            //showColumns: true, //显示下拉框勾选要显示的列
            showExport: true,
            exportDataType: "basic",
            rowStyle: rowStyle,//通过自定义函数设置行样式
            columns: [
                // {
                //     field: '',
                //     checkbox: true,
                //     align: 'center',
                //     valign: 'middle'
                // },
                // {
                //     field: 'ID',
                //     title: 'ID',
                //     width: 100,
                //     align: 'center',
                //     valign: 'middle',
                //     sortable: true,
                //     sortname: 'ID',//排序字段名称
                //     formatter: idFormatter
                // },
                {
                    field: 'orderType',
                    title: 'orderType',
                    width: 100,
                    align: 'center',
                    valign: 'middle'
                },
                {
                    field: 'customerNo',
                    title: 'customerNo',
                    width: 100,
                    align: 'center',
                    valign: 'middle'
                },
                {
                    field: 'logTime',
                    title: 'logTime',
                    width: 100,
                    align: 'center',
                    valign: 'middle'
                },
                {
                    field: 'enqueueTime',
                    title: 'enqueueTime',
                    width: 100,
                    align: 'center',
                    valign: 'middle'
                },
                {
                    field: 'dequeueTime',
                    title: 'dequeueTime',
                    width: 100,
                    align: 'center',
                    valign: 'middle'
                },

                {
                    field: 'tPSQueueCount',
                    title: '排队个数',
                    width: 100,
                    align: 'center',
                    valign: 'middle'
                    //sortable: true,
                    //sortname: 'ID',//排序字段名称
                    //formatter: idFormatter
                }],
            //formatLoadingMessage: function () {
            //    return "请稍等，正在加载中...";
            //},
            //formatNoMatches: function () {  //没有匹配的结果
            //    return '无符合条件的记录';
            //},
            onLoadSuccess: function () {
            },
            onLoadError: function () {
            }
        });
    }
    oTable.queryParams = function (params) {
        var temp = {
            take: params.limit,      //Take
            skip: params.offset,//Skip
            tPSQueueCount: $("#tPSQueueCount").val(),
            customerNo: $("#sysNo").val(),
            filePath: $("#uploadTPSPath").val(),
        // name: $("#tPSQueueCount").val()
        //sex: $("#sex").val()
        //sortOrder: params.sortOrder,
        //sortName: params.sortName
    }
        ;
        return temp;
    }

    function idFormatter(value, row, index) {
        return row.ID;
    }

    function nameFormatter(value, row, index) {
        return row.Name;
    }

    function operateFormatter(value, row, index) {
        //return '<a class="edit" style="cursor:pointer;" title="修改">修改</a> ' + '<a class="delete" style="cursor:pointer;" title="删除">删除</a>';


        return '<a class="edit" style="cursor:pointer;" title="修改"  onclick="edit(\'' + row.ID + '\')">修改</a> <a class="delete" style="cursor:pointer;" title="删除">删除</a>';
    }

    window.operateEvents = {
        'click .edit': function (e, value, row, index) {
            $.ajax({
                url: '/Home/UpdateRow',
                data: {id: row.ID},
                success: function (result) {
                    if (result.state) {

                        $table.bootstrapTable('refresh');
                    }
                }
            });
            $table.bootstrapTable('refresh');
        },
        'click .delete': function (e, value, row, index) {
            //删除操作
            $.confirm({
                title: '提示',
                //cancelButton: false // hides the cancel button.
                //title: false, // hides the title.
                closeIcon: true, // hides the close icon.
                content: '确认删除该条记录？',
                animation: 'zoom',
                closeAnimation: 'scale',
                autoClose: 'cancel|3000',
                buttons: {
                    ok: {
                        text: '确认',
                        keys: ['enter'],
                        action: function () {
                            $.ajax({
                                url: '/Home/DeleteRow',
                                data: {id: row.ID},
                                success: function (result) {
                                    if (result.state) {
                                        $table.bootstrapTable('refresh');
                                        //$table.bootstrapTable('refresh', { query: { Skip: 0, Take: 10 } });
                                    }
                                }
                            });

                        }
                    },
                    cancel: {
                        text: '取消',
                        keys: ['esc'],
                        action: function () {
                            //$.alert({
                            //    title: 'Alert!',
                            //    content: 'Simple alert!',
                            //});

                        }
                    }
                }


            });
        }
    }
    return oTable;
}

//初始化表格
$(function () {
    //$("#TPS").addClass('active');
    //$("#Expand").addClass('is-expanded');


    var myTable = new TableInit();
    myTable.Init();

    //
    //$('.selectpicker').selectpicker({
    //    noneSelectedText: "==请选择==",

    // $.post('/home/GetSexes', function (res) {
    //     var txt = '';
    //     //txt += '<option value=' + -1+ '>' + '--不限--' + '</option>';//非多选时候添加此空白选项
    //     for (var i = 0; i < res.length; i++) {
    //         txt += '<option value=' + i + '>' + res[i] + '</option>';
    //     }
    //
    //     $('#sex').html(txt);
    //     $('#sex').selectpicker('refresh');
    //
    // });
    //$('.selectpicker').selectpicker('refresh');//刷新插件
    //$('#sex').on('changed.bs.select', function (e) {
    //    alert(e.target.nodeValue);
    //    // do something...
    //});
    // $('#myModal').modal('hide');
});

function operateFormatter(value, row, index) {
    //return '<a class="edit" style="cursor:pointer;" title="修改">修改</a> ' + '<a class="delete" style="cursor:pointer;" title="删除">删除</a>';


    return '<a class="edit" style="cursor:pointer;" title="修改"  onclick="edit(\'' + row.ID + '\')">修改</a> <a class="delete" style="cursor:pointer;" title="删除">删除</a>';
}

function jsonDateFormat(jsonDate) {
    //json日期格式转换为正常格式
    var jsonDateStr = jsonDate.toString();//此处用到toString（）是为了让传入的值为字符串类型，目的是为了避免传入的数据类型不支持.replace（）方法
    try {
        var k = parseInt(jsonDateStr.replace("/Date(", "").replace(")/", ""), 10);
        if (k < 0)
            return null;

        var date = new Date(parseInt(jsonDateStr.replace("/Date(", "").replace(")/", ""), 10));
        var month = date.getMonth() + 1 < 10 ? "0" + (date.getMonth() + 1) : date.getMonth() + 1;
        var day = date.getDate() < 10 ? "0" + date.getDate() : date.getDate();
        var hours = date.getHours() < 10 ? "0" + date.getHours() : date.getHours();
        var minutes = date.getMinutes() < 10 ? "0" + date.getMinutes() : date.getMinutes();
        var seconds = date.getSeconds() < 10 ? "0" + date.getSeconds() : date.getSeconds();
        var milliseconds = date.getMilliseconds();
        return date.getFullYear() + "-" + month + "-" + day + " " + hours + ":" + minutes + ":" + seconds;
    } catch (ex) {
        return "时间格式转换错误";
    }
}


/*
row :该行绑定的model
 */
function rowStyle(row, index) {
    var style = {};

    if (row.tPSQueueCount > 1) {
        style = {css: {'background': '#ed5565'}};
    }
    return style;
}


//查询
$("#btnSearch").click(function () {
    //$('#tableList').bootstrapTable({ pageNumber: 1, pageSize: 10 });
    //$table.bootstrapTable('destroy');
    //var myTable = new TableInit();
    //myTable.Init();
    //alert($table.queryParams.limit);
    //$("#sex").attr("disabled", false);//禁用
    //$("#sex").removeAttr("disabled");
    // $('#sex').prop('disabled', true);//禁用,禁用之后要刷新
    // $('#sex').selectpicker('refresh');
    // var sex = $("#sex").val();//多选是个数组

    $table.bootstrapTable('selectPage', 1);
    $table.bootstrapTable('refresh');
});


// //请求参数设置
// function queryParams(params) {
//     return {
//         offset: params.offset,      //从数据库第几条记录开始
//         limit: params.limit,        //找多少条
//      //   name: $(...).val()   //其他自定义参数，从页面获取
// }
// };
//
// $('#myTable').bootstrapTable({
//     url: '/tps/tpsData',             //请求后台的URL（*）
//     method: 'get',                      //请求方式（*）
//     toolbar: '#toolbar',                //工具按钮用哪个容器
//     striped: true,                      //是否显示行间隔色
//     cache: false,                       //是否使用缓存，默认为true，所以一般情况下需要设置一下这个属性（*）
//     pagination: true,                   //是否显示分页（*）
//     sortable: true,                     //是否启用排序
//     sortOrder: "asc",                   //排序方式
//     queryParams: 'queryParams',         //传递参数（*）
//     sidePagination: "server",           //分页方式：client客户端分页，server服务端分页（*）
//     pageNumber: 1,                       //初始化加载第一页，默认第一页
//     pageSize: 10,                       //每页的记录行数（*）
//     pageList: [10, 25, 50, 100],        //可供选择的每页的行数（*）
//     smartDisplay: false,
//     search: false,                      //是否显示表格搜索，此搜索是客户端搜索，不会进服务端，所以，个人感觉意义不大
//     strictSearch: true,
//     showColumns: false,                 //是否显示所有的列
//     showRefresh: false,                 //是否显示刷新按钮
//     minimumCountColumns: 2,             //最少允许的列数
//     clickToSelect: true,                //是否启用点击选中行
//     height: 500,                        //行高，如果没有设置height属性，表格自动根据记录条数觉得表格高度
//     uniqueId: "ID",                     //每一行的唯一标识，一般为主键列
//     showToggle: false,                   //是否显示详细视图和列表视图的切换按钮
//     cardView: false,                    //是否显示详细视图
//     detailView: false,                   //是否显示父子表
//     columns: [
//         {
//             field: 'logTime',
//             title: 'logTime',
//             width: 100,
//             align: 'center',
//             valign: 'middle'
//         },
//         {
//             field: 'enqueueTime',
//             title: 'enqueueTime',
//             width: 100,
//             align: 'center',
//             valign: 'middle'
//         },
//         {
//             field: 'dequeueTime',
//             title: 'dequeueTime',
//             width: 100,
//             align: 'center',
//             valign: 'middle'
//         },
//
//         {
//             field: 'tPSQueueCount',
//             title: '排队个数',
//             width: 100,
//             align: 'center',
//             valign: 'middle'
//             //sortable: true,
//             //sortname: 'ID',//排序字段名称
//             //formatter: idFormatter
//         }
//     ]                    //列设置
// });