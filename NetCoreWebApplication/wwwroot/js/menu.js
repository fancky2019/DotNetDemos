(function () {
    "use strict";
    //FileUpLoad
    ///DW/gateway.html
    //var path = document.location.pathname;
    //let currentMenu = document.location.pathname.replace("/","");
    //debugger;
    let menusFuncation = function () {
        $.ajax({
            type: "GET",
            url: '/GetMenus',
            data: { userName: 'fancky' },
            dataType: "JSON",
            async: false,//设置同步请求
            success: function (result) {
                let currentMenu = document.location.pathname.replace("/", "");
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
                    //let id = p.url.substring(1, p.url.length);
                    //let menuName = id;
                    //let currentMenu = $.cookie('currentMenu');

     

                    if (children.length == 0) {
                        //父节点是否是选中状态
                        let activeClass = "";
                        if (!isEmpty(currentMenu)) {
                            if (currentMenu == p.menuName) {
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
                            //menuName = child.url.substring(1, child.url.length);
                            debugger;
                            if (!isEmpty(currentMenu)) {
                                if (currentMenu == child.menuName) {
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
                            //menuName = child.url.substring(1, child.url.length);
                            if (!isEmpty(currentMenu)) {
                                if (currentMenu == child.menuName) {
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

})();