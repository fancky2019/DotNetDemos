(function () {
    "use strict";

    let menusFuncation = function () {
        $.ajax({
            type: "GET",
            url: '/GetMenus',
            data: { userName: 'fancky' },
            dataType: "JSON",
            async: false,//设置同步请求
            success: function (result) {
                var parentMenus = new Array()
                result.forEach(menu => {
                    if (menu.parentID == 0) {
                        parentMenus.push(menu);
                    }
                });

                parentMenus.forEach(p => {

                    var children = new Array()
                    result.forEach(c => {
                        if (c.parentID == p.id) {
                            children.push(c);
                        }
                    });
                    let id = p.url.substring(1, p.url.length);
                    let menuName = id;
                    let currentMenu = $.cookie('currentMenu');



                    if (children.length == 0) {
                        //父节点是否是选中状态
                        let activeClass = "";
                        if (!isEmpty(currentMenu)) {
                            if (currentMenu == menuName) {
                                activeClass = "active";
                            }
                        }

                        let parentNodeHtml = ' <li> <a class="app-menu__item ' + activeClass + '"  href="' + p.url + '"> <i class="app-menu__icon fa fa-file-code-o"></i><span class="app-menu__label">' + p.menuName + '</span></a></li>';
                        $(".app-menu").append(parentNodeHtml);


                    }
                    else {
                        //设置父节点是否展开
                        let expandedClass = "";
                        children.forEach(child => {
                            menuName = child.url.substring(1, child.url.length);
                            if (!isEmpty(currentMenu)) {
                                if (currentMenu == menuName) {
                                    expandedClass = "is-expanded";
                                }
                            }
                        });


                        let html = '<li class="treeview ' + expandedClass + '">' +
                            ' <a class="app-menu__item" href = "#" data-toggle="treeview">' +
                            ' <i class="app-menu__icon fa fa-laptop"></i>' +
                            ' <span class="app-menu__label">' + p.menuName + '</span> <i class="treeview-indicator fa fa-angle-right"></i>' +
                            ' </a >' +
                            ' <ul class="treeview-menu">';
                        // $(".app-menu").html(parentNodeHtml); 
                        children.forEach(child => {
                            //子节点是否是选中状态
                            let childrenActiveClass = "";
                            menuName = child.url.substring(1, child.url.length);
                            if (!isEmpty(currentMenu)) {
                                if (currentMenu == menuName) {
                                    childrenActiveClass = "active";
                                }
                            }

                            let childrenHtml = ' <li><a class="treeview-item ' + childrenActiveClass + '"   href="' + child.url + '"><i class="icon fa fa-circle-o"></i>' + child.menuName + '</a></li>';
                            html += childrenHtml;
                        });
                        html += '     </ul></li >';
                        $(".app-menu").append(html);
                    }
                });






                ////ajax 默认异步，生成html之后再绑定事件，否则找不到html元素进而无法绑定事件。
                //var treeviewMenu = $('.app-menu');

                //// Toggle Sidebar
                //$('[data-toggle="sidebar"]').click(function (event) {
                //    event.preventDefault();
                //    $('.app').toggleClass('sidenav-toggled');
                //});

                //// Activate sidebar treeview toggle
                //$("[data-toggle='treeview']").click(function (event) {
                //    event.preventDefault();
                //    if (!$(this).parent().hasClass('is-expanded')) {
                //        treeviewMenu.find("[data-toggle='treeview']").parent().removeClass('is-expanded');
                //    }
                //    $(this).parent().toggleClass('is-expanded');
                //});

                //// Set initial active toggle
                //$("[data-toggle='treeview.'].is-expanded").parent().toggleClass('is-expanded');

                ////Activate bootstrip tooltips
                //$("[data-toggle='tooltip']").tooltip();


                //$(".app-menu__item").click(function (event) {
                //    debugger;
                //    event.preventDefault();

                //    let hrefURL = $(this).attr('href');

                //    //have children node
                //    if (hrefURL.indexOf("#") == -1) {

                //        let href = hrefURL.substring(1, hrefURL.length);
                //        $.cookie('currentMenu', href);
                //        let currentMenu = $.cookie('currentMenu');
                //        debugger;
                //        let m = 0;
                //    }

                //});

                //$(".treeview-item").click(function (event) {
                //    debugger;
                //    event.preventDefault();
                //    let hrefURL = $(this).attr('href');
                //    let href = hrefURL.substring(1, hrefURL.length);
                //});







            }
        });

    };

    menusFuncation();
    //
    //判断字符是否为空的方法
    function isEmpty(obj) {
        if (typeof obj == "undefined" || obj == null || obj == "") {
            return true;
        } else {
            return false;
        }
    }
    //let displayCurrentActiveNode = function () {
    //    let currentMenu = $.cookie('currentMenu');
    //    let sourceEvent = $.cookie('sourceEvent');
    //    if (!isEmpty(currentMenu)) {
    //        if (!isEmpty(sourceEvent)) {

    //            let idStr = "#" + currentMenu;
    //            $("#" + currentMenu).addClass("active");
    //            let id = $("#" + currentMenu).attr("id");
    //            let hrefff = $("#" + currentMenu).attr('href');
    //            let hreff = $(idStr).attr('href');
    //            let sss = "ss";
    //            let hra = $(idStr);
    //            alert($("#UserManager").attr('href'))
    //            //parent node
    //            if (sourceEvent == "app-menu__item") {
    //                //let aElements = treeviewMenu.find('a');
    //                //aElements.forEach(p => {
    //                //    if (p.attr("href") == currentMenu) {
    //                //        let m = 0;
    //                //        debugger;
    //                //    }
    //                //});

    //                //for (let item of aElements) {
    //                //    let href = item.href;
    //                //    let hrefs = href.split('/');
    //                //    let last = hrefs[hrefs.length - 1];//TPS#
    //                //    if (last.indexOf('#') != -1) {
    //                //        last = last.substring(0, last.length - 1);//TPS
    //                //    }
    //                //    if (last == currentMenu) {
    //                //        item.classList.add("active");
    //                //        let n = 0;
    //                //    }
    //                //    debugger;
    //                //    //if (item.attr("href") == currentMenu) {
    //                //    //    let m = 0;
    //                //    //    debugger;
    //                //    //}
    //                //}



    //            }
    //            //children node
    //            else if (sourceEvent == "treeview-item") {
    //                $("#" + currentMenu).parent().parent().addClass("is-expanded");
    //            }
    //        }
    //    }
    //}

    //displayCurrentActiveNode();
})();