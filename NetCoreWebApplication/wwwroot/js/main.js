(function () {
    "use strict";


    var treeviewMenu = $('.app-menu');

    // Toggle Sidebar
    $('[data-toggle="sidebar"]').click(function (event) {
        event.preventDefault();
        $('.app').toggleClass('sidenav-toggled');
    });

    // Activate sidebar treeview toggle
    $("[data-toggle='treeview']").click(function (event) {
        event.preventDefault();
        if (!$(this).parent().hasClass('is-expanded')) {
            treeviewMenu.find("[data-toggle='treeview']").parent().removeClass('is-expanded');
        }
        $(this).parent().toggleClass('is-expanded');
    });

    // Set initial active toggle
    $("[data-toggle='treeview.'].is-expanded").parent().toggleClass('is-expanded');

    //Activate bootstrip tooltips
    $("[data-toggle='tooltip']").tooltip();


    $(".app-menu__item").click(function (event) {

        //event.preventDefault();

        let hrefURL = $(this).attr('href');

        //have children node
        if (hrefURL.indexOf("#") == -1) {

            let href = hrefURL.substring(1, hrefURL.length);
            $.cookie('currentMenu', href);
            let currentMenu = $.cookie('currentMenu');

            let m = 0;
        }

    });

    $(".treeview-item").click(function (event) {

        //event.preventDefault();
        let hrefURL = $(this).attr('href');
        let href = hrefURL.substring(1, hrefURL.length);

        $.cookie('currentMenu', href);

        // return false;//��ִ��a��ǩ��href
    });


    //��jquery1.7֮����on()������1.7֮ǰ��live(),����Ϊδ��������Ԫ�ذ��¼�.
    $("body").on("click", "#myid", function () {
        alert(1)
    });

})();
