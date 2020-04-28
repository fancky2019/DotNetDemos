
(function () {
    $("#FileUpLoad").addClass('active');
    $("#Expand").addClass('is-expanded');
    
})();


$("#btnSetProgress").click(function () {
    // 200ms 打印一次当前的上传进度
    let i = 1;
    setInterval(function () {
        if (i <= 10) {
            $(".progress-bar").css("width", i * 10 + "%");
            // $(".progress-bar").attr("aria-valuenow",i*10)
            $(".progress-bar").html(i * 10 + "%");
            i++;
        }

    }, 200);

});


/*
按钮触发File,File-->change-->Input显示路径
 */
$("#openTPSFile").click(function () {
    $("#uploadTPSFile").click();
});


//弹出文件对话框，选择的文件发生改变
$("#uploadTPSFile").change(function () {

    // $("#selectFilePath").val($("#uploadFile").val());

    // $("#selectFilePath").val($("#uploadFile").val());
    // let file = $("#uploadFile");
    let fullName = $("#uploadTPSFile").val();
    let fileNames = fullName.split("\\");
    let fileName = fileNames[fileNames.length - 1]
    $("#selectTPSFilePath").val(fileName);
});


//  上传，同时显示进度
$("#uploadTPS").click(function (e) {


    $(".progress-bar").css("width", 0 + "%");
    $(".progress-bar").attr("aria-valuenow", 0)
    $(".progress-bar").html(0 + "%");

    // 50ms 打印一次当前的上传进度
    var interval = setInterval(function () {
        $.ajax({
            type: "get",
            dataType: 'json',
            url: "/fileupload/uploadStatus",
            success: function (progress) {
                $(".progress-bar").css("width", progress + "%");
                $(".progress-bar").attr("aria-valuenow", progress)
                $(".progress-bar").html(progress + "%");
                if (progress == 100) {
                    clearInterval(interval);
                }
            }
        });
    }, 50);


    let type = "file";          //后台接收时需要的参数名称，自定义即可
    let id = "uploadTPSFile";            //即input的id，用来寻找值
    let formData = new FormData();
    formData.append(type, $("#" + id)[0].files[0]);    //生成一对表单属性
    $.ajax({
        type: "POST",           //因为是传输文件，所以必须是post
        url: '/fileupload/upload',         //对应的后台处理类的地址
        data: formData,
        processData: false,
        contentType: false,
        success: function (data) {
            // alert('上传成功！');
            // alert(data);
            $("#uploadTPSPath").val(data);
        }
    });
});

$("#btnDeleteFile").click(function (e) {

    let filePath = $("#uploadTPSPath").val();


    //后台@RequestBody对象接收参数，必须设置：
    //一、参数JSON序列化；二、指定contentType。
    $.ajax({
        type: "post",//向后台请求的方式，有post，get两种方法
        url: '/fileupload/deleteFile',         //对应的后台处理类的地址
        cache: false,//缓存是否打开
        data: JSON.stringify({filePath: filePath}),
        dataType: 'json',//请求的数据类型
        contentType: 'application/json',
        success: function (data) {//请求的返回成功的方法
            alert(data.message);
        },
        error: function (XMLHttpRequest, textStatus, errorThrown) {//请求的失败的返回的方法
            alert("删除失败，请稍后再次尝试删除！");
        }

    });


    //$.post(URL,data,callback); //无法指定 contentType，报错
    // $.post('/fileupload/deleteFile',JSON.stringify({filePath: filePath}), function (data) {//请求的返回成功的方法
    //     alert(data.message);
    // });
});

$("#btnQuery").click(function (e) {

    //$.get(URL,callback);
    //get
    // $.get("/fileupload/getQuery?customerNo=fancky", function (data) {
    //     // for (let val of data) {
    //     //    val.imageURL
    //     // }
    //     let str=data;
    //     debugger;
    // });

    $.ajax({
        type: "GET",
        url: "/fileupload/getQuery",
        data: {customerNo: 'fancky', tPSQueueCount: 5},
        dataType: "JSON",
        success: function (result) {
            debugger;
        }
    });

});


