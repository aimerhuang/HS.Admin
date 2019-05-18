
//页面调用Area方法，参数依次为省的select控件id，市的id，区域的id，和数组(包含省市区联动反绑定的区域id，回调函数)。
//取值只需要区第三个select控件的值(区域select控件的值)
//示例代码:初始化联动控件Area($("#province"), $("#city"), $("#area"), { callback: function (type) { } });
//反向绑定Area($("#province"), $("#city"), $("#area"), { a: a, callback: function (type) { } });
//取值$("#area").val() 其中province为省的select控件id，city市id，area区域id，a为反绑定时的区域id，callback为回调函数type参数提供开始加载和加载完成状态

var Area = function (country, province, city, area, attr) {

    var postUrl = "/Ajax/GetArea";        //地区数据请求地址
    country.append("<option value='-1'>请选择国家</option>");
    province.append("<option value='-1'>请选择省份</option>");
    city.append("<option value='-1'>请选择市/州</option>");
    area.append("<option value='-1'>请选择区/县</option>");
    if (attr.a != null) {  //判断设置的省市区值不为空就直接设置值
        loadAddress(attr.a);
    }
    else {
        $.post(postUrl, {}, function (data) {
            attr.callback.call($(this), 'begin');
            $.each(data, function (idx, item) {
                //增加权限判断
                if (item.SysNo != undefined) {
                    country.append("<option value='" + item.SysNo + "'>" + item.AreaName + "</option>");
                }
            });
            attr.callback.call($(this), 'end');
        });
    }
    country.change(function () {
        attr.callback.call($(this), 'begin');
        $.post(postUrl, { parentSysNo: country.val() }, function (data) {
            province.find("option").remove();
            province.append("<option value='-1'>请选择省/州</option>");
            city.find("option").remove();
            city.append("<option value='-1'>请选择市/州</option>");
            area.find("option").remove();
            area.append("<option value='-1'>请选择区/县</option>");
            $.each(data, function (idx, item) {
                province.append("<option value='" + item.SysNo + "'>" + item.AreaName + "</option>");
            });
            attr.callback.call($(this), 'end');
        });
    });
    province.change(function () {
        attr.callback.call($(this), 'begin');
        $.post(postUrl, { parentSysNo: province.val() }, function (data) {
            city.find("option").remove();
            city.append("<option value='-1'>请选择市/州</option>");
            area.find("option").remove();
            area.append("<option value='-1'>请选择区/县</option>");
            $.each(data, function (idx, item) {
                city.append("<option value='" + item.SysNo + "'>" + item.AreaName + "</option>");
            });
            attr.callback.call($(this), 'end');
        });
    });
    city.change(function () {
        attr.callback.call($(this), 'begin');
        $.post(postUrl, { parentSysNo: city.val() }, function (data) {
            area.find("option").remove();
            area.append("<option value='-1'>请选择区/县</option>");
            $.each(data, function (idx, item) {
                area.append("<option value='" + item.SysNo + "'>" + item.AreaName + "</option>");
            });
            attr.callback.call($(this), 'end');
        });
    });


    function loadAddress(a) {     //修改时设置值
        var y,p, c;
        attr.callback.call($(this), 'begin');

        $.ajax({
            url: "/Ajax/GetAreaSysNo",
            cache: true,
            async: false,
            type: "post",
            data: { areaSysNo: a },
            dataType: "json",
            success: function (data) {
                if (data.success) {
                    c = data.c;
                    p = data.p;
                    y = data.y;
                }
                else {
                    //alert("数据异常，应该传入区域id");
                    return;
                }
            }
        });


        $.ajax({
            url: postUrl,
            cache: true,
            async: false,
            type: "post",
            data: { parentSysNo: 0 },
            dataType: "json",
            success: function (data) {
                country.find("option").remove();
                country.append("<option value='-1'>请选择国家</option>");
                $.each(data, function (idx, item) {
                    country.append("<option value='" + item.SysNo + "'>" + item.AreaName + "</option>");
                });
            }
        });

        $.ajax({
            url: postUrl,
            cache: true,
            async: false,
            type: "post",
            data: {parentSysNo:y},
            dataType: "json",
            success: function (data) {
                province.find("option").remove();
                province.append("<option value='-1'>请选择省份</option>");
                $.each(data, function (idx, item) {
                    province.append("<option value='" + item.SysNo + "'>" + item.AreaName + "</option>");
                });
            }
        });

        $.ajax({
            url: postUrl,
            cache: true,
            async: false,
            type: "post",
            data: { parentSysNo: p },
            dataType: "json",
            success: function (data) {
                city.find("option").remove();
                city.append("<option value='-1'>请选择市/州</option>");
                $.each(data, function (idx, item) {
                    city.append("<option value='" + item.SysNo + "'>" + item.AreaName + "</option>");
                });
            }
        });

        $.ajax({
            url: postUrl,
            cache: true,
            async: false,
            type: "post",
            data: { parentSysNo: c },
            dataType: "json",
            success: function (data) {
                area.find("option").remove();
                area.append("<option value='-1'>请选择区/县</option>");
                $.each(data, function (idx, item) {
                    area.append("<option value='" + item.SysNo + "'>" + item.AreaName + "</option>");
                });
                attr.callback.call($(this), 'end');
            }
        });
        country.val(y);
        province.val(p);
        city.val(c);
        area.val(a);
    }

}