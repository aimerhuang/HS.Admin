
//重写Ajax（用于ajax超时全局处理）
//2013-9-29 杨浩 创建
(function ($) {
    //备份jquery的ajax方法
    var _ajax = $.ajax;
    $.ajax = function(s) {
        var old = s.success;
        s.success = function(data, textStatus, jqXHR) {
            if (data && data.IsLogout) {
                eval(data.Callback);
            }
            if(old){
                old(data, textStatus, jqXHR);
            }
        };
        _ajax(s);
    };
})(jQuery);

//格式化字符串
//"黄波{0}{1}".format("很","牛逼")
//2013-7-31 黄波 创建
String.prototype.format = function () {
    var that = this;
    for (var i = 0; i < arguments.length; i++) {
        that = that.replace("{" + i + "}", arguments[i]);
    }
    return that;
}

//多次替换字符串
String.prototype.replaceAll = function (s1, s2) {
    return this.replace(new RegExp(s1, "gm"), s2);
}

//为function添加获取function中的第一个以/*开始和*/结尾的注释
//主要用于html模板字符串
//2013-06-27 邵斌 创建
Function.prototype.getMultiLine = function () {
    var lines = new String(this);
    lines = lines.substring(lines.indexOf("/*") + 3, lines.lastIndexOf("*/"));
    return lines;
}

//为数组扩展方法序列化成指定分隔符的字符串 
//2013-07-01 邵斌 创建
Array.prototype.AsDelimited = function (delimeter) {
    //设置参数
    var result = "";
    for (var i = 0; i < this.length; i++) {

        if (result.length > 0) {
            result += delimeter;
        }
        result += this[i];
    }
    return result;
}
//2013-8-85 杨文兵 创建 从数组中删除指定的元素
Array.prototype.del = function (p) {
    var newArray = [];
    if (p.constructor == Function) {
        for (var i = 0; i < this.length; i++) {
            if (p(this[i]) != true) {
                newArray.push(this[i]);
            }
        }
    } else {
        for (var i = 0; i < this.length; i++) {
            if (this[i] != p) {
                newArray.push(this[i]);
            }
        }
    }
    return newArray;
}
//2013-8-85 杨文兵 创建 判断数组中是否包含指定的元素
Array.prototype.contains = function (p) {
    if (p.constructor == Function) {
        for (var i = 0; i < this.length; i++) {
            if (p(this[i]) == true) {
                return true;
            }
        }
    } else {
        for (var i = 0; i < this.length; i++) {
            if (this[i] == p) {
                return true;
            }
        }
    }
    return false;
}

//将float货币类型转换成字符货币类型
// @sign : 货币符号  默认空
Number.prototype.toPrice = function (sign) {
    var num = this.toString().replace(/\$|\,/g, '');
    if (isNaN(num)) {
        num = "0";
    }
    var type = (num == (num = Math.abs(num)));
    num = Math.floor(num * 100 + 0.50000000001);
    var cents = num % 100;
    num = Math.floor(num / 100).toString();
    if (cents < 10) {
        cents = "0" + cents;
    }
    for (var i = 0; i < Math.floor((num.length - (1 + i)) / 3) ; i++) {
        num = num.substring(0, num.length - (4 * i + 3)) + ',' + num.substring(num.length - (4 * i + 3));
    }
    return (((typeof sign != 'undefined') ? sign : '￥') + ((type) ? '' : '-') + num + '.' + cents);
}
//将字符形式货币转换成float类型
// @fix : 小数点位数  默认两位
String.prototype.toDecimal = function (fix) {
    var num = this.toString().replace(/[^\d\.\-]/g, '');
    var fix_places = typeof fix == 'undefined' ? 2 : fix;
    return parseFloat(num).toFixed(fix_places);
}

//检查对象是否为数字
//2013-06-23 周唐炬 添加
function checkRate(obj) {
    //判断正整数 /^[1-9]+[0-9]*]*$/
    var re = /^[0-9]+.?[0-9]*$/;
    if (!re.test(obj)) {
        return false;
    }
    return true;
}
//限制文本框只能输入整型或double型数据并格式化
//2013-09-10 周唐炬 添加
function clearNoNum(input) {
    input.value += '';
    input.value = input.value.replace(/[^0-9|\.]/g, ''); //清除字符串中的非数字非.字符
    if (/^0+/) //清除字符串开头的0
        input.value = input.value.replace(/^0+/, '');
    if (!/\./.test(input.value)) //为整数字符串在末尾添加.00
        input.value += '.00';
    if (/^\./.test(input.value)) //字符以.开头时,在开头添加0
        input.value = '0' + input.value;
    input.value += '00';        //在字符串末尾补零
    input.value = input.value.match(/\d+\.\d{2}/)[0];
}
//生产折叠卡对象
//2013-07-03 邵斌 创建
$.fn.Accordion = function (options) {
    var opts = {
        title: "",           //标题
        active: 0,           //当前那个是当前显示
        collapsible: false,  //是否保持只有活动页被展开
        disabled: false,    //禁用折叠
        header: "h5",       //标题容器
        event: "click",     //展开关闭触发事件
        heightStyle: "",    //容器规定高度样式
        icons: "",           //标题icon

        activate: function (event, ui) { },         //页卡展开时事件
        beforeActivate: function (event, ui) { },   //页卡展开前事件
        create: function (event, ui) { }            //创建页卡时事件
    };

    var content = $(this);

    //折叠卡HTML模板
    var template = '<div class="tit_dispay tit_dispay_2">'
                   + '     <div class="more10r m5_t"><span class="btn_arrow btn_tableup"></span></div>'
                   + '     <{header} class="accordingtitle"><span class="{icon}"></span>{title}</{header}>'
                   + ' </div>';

    opts = $.extend(opts, options);

    //折叠卡分组
    var groupId = Math.round((Math.random(100) * 10000) / 100);

    //初始化函数
    function _init() {

        //遍历需要生产折叠卡的对象
        content.each(function (index, element) {
            //单个原始对象
            element = $(element);


            if (!!element.attr("init")) {
                return;
            }

            //初始化折叠卡标题
            var title = "";
            //如果没有设置标题将用对象的title属性为默认标题
            if (opts.title.length == 0) {
                title = (element.attr("title") == null) ? "未设置标题" : element.attr("title");
            }

            //生产新容器ID
            var newId = null;

            while (!(newId != null && $("#" + newId).length == 0)) {
                newId = "accordion" + "_" + (element.attr("id") == null ? Math.round((Math.random(10000) * 1000000)) : element.attr("id"));
            }

            //格式化模板数据
            var titleHtml = template;
            titleHtml = titleHtml.replace("{title}", title);
            titleHtml = titleHtml.replace("{icon}", opts.icons);
            titleHtml = titleHtml.replace("{header}", opts.header);
            titleHtml = titleHtml.replace("{header}", opts.header);

            //折叠卡新容器插入到原始对象的前面
            element.before('<div group="' + groupId + '" id="' + newId + '" accourdIndex="' + index + '">' + titleHtml + '</div>');
            element.addClass(opts.heightStyle);     //设置高度样式
            $("#" + newId).append(element);         //将原始对象移动新折叠卡对象中

            //如果是禁用将不对折叠按钮绑定事件
            if (!opts.disabled) {

                //为折叠对象按钮绑定事件，事件是由用户参数觉得，默认是click事件
                $("#" + newId + " .tit_dispay").live(opts.event, function (event) {

                    //执行绑定前事件
                    opts.beforeActivate(event, element);

                    //如果是自动折收将处理选前打开的折叠卡
                    if (opts.collapsible) {
                        $("div[group='" + groupId + "'] div.tit_dispay:visible").next().hide();     //隐藏已经打开的折叠卡
                        $("div[group='" + groupId + "'] div.tit_dispay:visible").children().children("span.btn_tableup").addClass("btn_tabledown").removeClass("btn_tableup");
                        //切换折叠按钮样式
                    }

                    var spanIcon = $("span", this);

                    //切换折叠按钮样式并切换显示内容容器
                    if (element.is(":visible")) {
                        spanIcon.removeClass("btn_tableup");
                        spanIcon.addClass("btn_tabledown");
                        element.hide();
                    } else {
                        spanIcon.removeClass("btn_tabledown");
                        spanIcon.addClass("btn_tableup");
                        element.show();
                    }

                    //执行激活事件
                    opts.activate(index, element);

                    return false;
                });
            }

            //是否是自动折叠
            if (opts.collapsible) {
                //显示默认设置的折叠卡
                if (index == opts.active) {
                    element.show();
                    $("#" + newId + " .tit_dispay span:eq(0)").removeClass("btn_tabledown");
                    $("#" + newId + " .tit_dispay span:eq(0)").addClass("btn_tableup");
                } else {
                    element.hide();
                    $("#" + newId + " .tit_dispay span:eq(0)").removeClass("btn_tableup");
                    $("#" + newId + " .tit_dispay span:eq(0)").addClass("btn_tabledown");
                }
            }

            //执行创建时事件
            opts.create(index, element);

            element.attr("title", null);
            element.attr("init", true);
        });

    }

    _init();

    this.Active = function (index) {
        $("div[group='" + groupId + "'] div.tit_dispay").next().hide();     //隐藏已经打开的折叠卡
        $("div[group='" + groupId + "'] div.tit_dispay:visible").children(":first").children(":first").trigger(opts.event);
        return true;
    }

    return this;
}


//在指定的范围内进行Json数据绑定和读取，
//可以将Json数据绑定到有dn属性的html控件上或从控件上读取数据并返回成json数据，
//data: json数据对象， 参数为null时可清除已绑定的数据，不带参数是为读取json数据
//范例：   
//$("div").JsonBind(data);   //绑定data到一个指定的div容器中的所有带dn属性的控件
//$("div").JsonBind(null);   //清空一个指定的div容器      
//var jsondata = $("div").JsonBind();       //从一个指定的div容器中读取数据并返回成一个json数据
//使用范围:如表单填写，和表单绑定数据
//2012-07-04 邵斌 创建
$.fn.JsonBind = function (data) {
    if (data != null) {
        for (var column in data) {
            $("*[dn='" + column + "']", this).each(function (i, e) {
                UI.BindDataToElement($(e), data[column]);
            });
        }
    } else if (typeof (data) == "undefined") {
        var jsonData = {};
        $("[dn]", this).each(function (i, e) {
            eval("jsonData." + $(e).attr("dn") + " = null;");
            jsonData[$(e).attr("dn")] = getData($(e));
        });
        return jsonData;
    } else {
        $("[dn]", this).each(function (i, e) {
            UI.BindDataToElement($(e), "");
        });
    }

    function getData(element) {
        var tagName = element[0].tagName.toLocaleLowerCase();
        //设置显示文本
        if (tagName == "input" && element.attr("type") == "text") {
            //处理Input控件
            return element.val();
        } else if (tagName == "input" && element.attr("type") == "checkbox") {
            return (element.attr("checked") != null);
        } else if (tagName == "input" && element.attr("type") == "hidden") {
            //处理Input控件
            return element.val();
        } else if (tagName == "select") {
            //处理select控件
            return element.val();

        } else if (tagName == "textarea") {
            return element.val();
        } else if (tagName == "img") {
            return element.attr("src");
        } else {
            //其他控件默认都插入html
            return element.html();
        }
    }

    return true;
}


//查询输入框默认值效果,点击输入框清空默认值，失去焦点又没录入数据则显示默认值
//参数text表示输入框默认值，如“请输入查询订单号...”
//2013-06-18 唐永勤 创建
$.fn.defaultValue = function (text) {
    $(this).data('dv', text);
    $(this).focus(function () {        
        if ($.trim($(this).val()) == $(this).data('dv')) {
            $(this).val('');
        }
    }).blur(function () {
        if ($.trim($(this).val()) == "") {
            $(this).val($(this).data('dv'));
        }
    });

    if ($.trim($(this).val()) == "") {
        $(this).val($(this).data('dv'));
    }
};
//限制输入框只能为数字类型包括小数点
//2013-07-18 余勇 创建
$.fn.numberInput = function() {
    $(this).blur(function() {
        this.value = this.value.replace(/[^\.\d]/g, '');
    });
    $(this).bind('keyup', function() {
        this.value = this.value.replace(/[^\.\d]/g, '');
    });
};
//限制textarea控件的最大长度
//2013-07-18 余勇 创建
$.fn.textareaInput = function(maxlength) {
    $(this).blur(function() {
        if (this.value.length > maxlength) {
            this.value = this.value.substring(0, maxlength);
        }
    });
};
//提交ajax时加载Mask，解决按钮重复提交的问题,
//调用方法： $(document).ready(function (e){$("#divid").ajaxLoadingMask();} ,$("#divid")为被遮罩的对象,isdisabled表示禁用按钮
//2013-07-18 余勇 创建
$.fn.ajaxLoadingMask = function(isdisabled) {
    var me = $(this);
    
    $(me).ajaxStart(function() {
        me.data = UI.Mask({
            obj: me,
            zindex: 9999
        });
        if (isdisabled) {
            var buttons = $(me).find("input[type=button],button");
            buttons.attr("disabled", "disabled");
        }
    });

    $(me).ajaxStop(function() {
        if (me.data != null && $.isFunction(me.data.Remove)) {
            me.data.Remove();
        }
        if (isdisabled) {
            var buttons = $(me).find("input[type=button],button");
            buttons.removeAttr("disabled");
            
            buttons.each(function (i) {
                if ($(this).hasClass("disabled")) {
                    $(this).attr("disabled", "disabled");
                }
            });
        }
    });
};
//$(function() {
//    $(document).ajaxError(function (event, request, settings) {
//        if (request.responseText != "") {
//            var data = jQuery.parseJSON(request.responseText);
//            if (data.IsLogout) {
//                eval(data.Callback);
//                return;
//            }
//        }
//    });
//});

//用于MVC参数适配JavaScript闭包函数
//2013-7-17 沈强 创建
/*
使用方式如下：
$.ajax({
    url: "@Url.Action("AjaxTest")",
    data: mvcParamMatch(sendData),//在此转换json格式，用于mvc参数提交，如果sendData自身就是数组，像这样使用mvcParamMatch(sendData,"Action中对应参数名")
    dataType: "json",
    type: "post",
    success:function(result) {
        alert(result.Message);
    }
});
*/
var mvcParamMatch = (function () {
    var MvcParameterAdaptive = {};
    //验证是否为数组
    MvcParameterAdaptive.isArray = Function.isArray || function (o) {
        return typeof o === "object" &&
                Object.prototype.toString.call(o) === "[object Array]";
    };

    //将数组转换为对象
    MvcParameterAdaptive.convertArrayToObject = function (/*数组名*/arrName, /*待转换的数组*/array, /*转换后存放的对象，不用输入*/saveOjb) {
        var obj = saveOjb || {};

        function func(name, arr) {
            for (var i in arr) {
                if (!MvcParameterAdaptive.isArray(arr[i]) && typeof arr[i] === "object") {
                    for (var j in arr[i]) {
                        if (MvcParameterAdaptive.isArray(arr[i][j])) {
                            func(name + "[" + i + "]." + j, arr[i][j]);
                        } else if (typeof arr[i][j] === "object") {
                            MvcParameterAdaptive.convertObject(name + "[" + i + "]." + j + ".", arr[i][j], obj);
                        } else {
                            if (typeof arr[i][j] !== "function") {
                                obj[name + "[" + i + "]." + j] = arr[i][j];
                            }
                        }
                    }
                } else {
                    if (typeof arr[i] !== "function") {
                        obj[name + "[" + i + "]"] = arr[i];
                    }
                }
            }
        }

        func(arrName, array);

        return obj;
    };

    //转换对象
    MvcParameterAdaptive.convertObject = function (/*对象名*/objName,/*待转换的对象*/turnObj, /*转换后存放的对象，不用输入*/saveOjb) {
        var obj = saveOjb || {};

        function func(name, tobj) {
            for (var i in tobj) {
                if (MvcParameterAdaptive.isArray(tobj[i])) {
                    MvcParameterAdaptive.convertArrayToObject(i, tobj[i], obj);
                } else if (typeof tobj[i] === "object") {
                    func(name + i + ".", tobj[i]);
                } else {
                    if (typeof tobj[i] !== "function") {
                        obj[name + i] = tobj[i];
                    }
                }
            }
        }

        func(objName, turnObj);
        return obj;
    };

    return function (json, arrName) {
        arrName = arrName || "";
        if (typeof json !== "object") throw new Error("请传入json对象");
        if (MvcParameterAdaptive.isArray(json) && !arrName) throw new Error("请指定数组名，对应Action中数组参数名称！");

        if (MvcParameterAdaptive.isArray(json)) {
            return MvcParameterAdaptive.convertArrayToObject(arrName, json);
        }
        return MvcParameterAdaptive.convertObject("", json);
    };
})();



//转换json日期 
//参数：
//  jsonDate 返回的json日期 如:Date(1234564896151)
//  formatString 格式字符串 如："yyyy-MM-dd"
//2013-07-18 邵斌 创建
function transJsonDate(jsonDate, formatString) {
    if (jsonDate == "" || jsonDate == null) return "";

    if (formatString == null) {
        formatString = "yyyy-MM-dd HH:mm:ss";
    }
    var jsonDate = jsonDate.toString();
    var date = jsonDate.replace(/\//gi, "");

    var d;
    eval("d = new " + date + ";");
    return FormatDateToString(d, formatString);
}

//转换json日期 
//参数：
//  x 日期
//  y 日期格式
//2013-07-18 邵斌 创建
function FormatDateToString(x, y) {
    var z = { M: x.getMonth() + 1, d: x.getDate(), h: x.getHours() % 12 == 0 ? 12 : x.getHours() % 12, m: x.getMinutes(), s: x.getSeconds(), H: x.getHours(), S: ("000").substring(0, 3 - x.getMilliseconds().toString().length) + x.getMilliseconds() };
    y = y.replace(/(M+|d+|h+|m+|s+|H+|S{1,3})/g, function (v) {
        return ((v.length > 1 ? "0" : "") + eval('z.' + v.slice(-1))).slice(0 - v.length);
    });
    return y.replace(/(y+)/g, function (v) { return x.getFullYear().toString().slice(-v.length) });
}

//计算天数差的函数，通用
//参数：
//  sDate1 日期
//  sDate2 被减日期
//2013-07-18 邵斌 创建
function DateDiff(sDate1, sDate2) {  //sDate1和sDate2是2002-12-18格式
    var aDate, oDate1, oDate2, iDays;
    aDate = sDate1.split("-");
    oDate1 = new Date(aDate[1] + '-' + aDate[2] + '-' + aDate[0]);  //转换为12-18-2002格式
    aDate = sDate2.split("-");
    oDate2 = new Date(aDate[1] + '-' + aDate[2] + '-' + aDate[0]);
    iDays = parseInt(Math.abs(oDate1 - oDate2) / 1000 / 60 / 60 / 24);  //把相差的毫秒数转换为天数
    return iDays;
}

//内容滚动表格
(function (jQuery, window, undefined) {

    var DataTableScroll = window.DTS = function (tableWrap, options) {
        return new DTS.fn.Init(tableWrap, options);
    }

    DTS.fn = DTS.prototype = {
        Init: function (tableWrap, options) {
            this.options = $.extend(DTS.defOptions, options);
            this.tableWrap = $(tableWrap);
            this.objects = {
                $scrollWrap: null
                , $scrollContent: null
                , $tableBodyWrap: null
                , $tableContent: null
            };

            this.Init();
        }
    };

    DTS.fn.Init.prototype = {
        Init: function () {
            this.ScrollWrap();//添加滑动区域

            this.objects = {
                $scrollWrap: this.tableWrap.find('.scrollwrap')
                , $scrollContent: this.tableWrap.find('.scrollcontent')
                , $tableBodyWrap: this.tableWrap.find('.table-body')
                , $tableContent: this.tableWrap.find('.table-body table')
            }

            //table容器样式设置
            this.tableWrap.css({ 'position': 'relative', 'width': this.options.width });
            //table BODY容器样式设置
            this.objects.$tableBodyWrap.css({ 'position': 'absolute', 'width': '100%', 'overflow': 'hidden' });

            var tableWrap = this.tableWrap,
                objects = this.objects,
                options = this.options;

            setTimeout(function () {
                DTS.SetHeight(tableWrap, objects, options);
            }, 50);

            this.RegEvent();
        }
        , ScrollWrap: function () {
            if (this.tableWrap.find(".scrollwrap").length == 0) {
                $('<div class="scrollwrap tablescrollwrap"><div class="scrollcontent"></div></div>')
                .css({
                    'position': 'absolute',
                    'top': this.options.headerHeight + 'px',
                    'right': '0',
                    'zIndex': '1',
                    'overflowX': 'hidden',
                    'overflowY': 'scroll'
                })
                .prependTo(this.tableWrap);
            }
        }
        , InRuleHeight: function (value) {
            if (typeof value !== 'undefined') {
                this.options.inRuleHeight = value;
                DTS.SetHeight(this.tableWrap, this.objects, this.options);
            } else {
                return this.options.inRuleHeight;
            }
        }
        , OutRuleHeight: function (value) {
            if (typeof value !== 'undefined') {
                this.options.outRuleHeight = value;
                DTS.SetHeight(this.tableWrap, this.objects, this.options);
            } else {
                return this.options.outRuleHeight;
            }
        }
        , RegEvent: function () {
            var tableWrap = this.tableWrap,
                objects = this.objects,
                options = this.options;

            tableWrap.mousewheel(function (event, delta) {
                var rowHeight = tableWrap.find('.table-body table tr:first').height();
                if (delta > 0) {
                    objects.$tableBodyWrap.scrollTop(objects.$tableBodyWrap.scrollTop() - (rowHeight * options.scrollSetp));
                } else {
                    objects.$tableBodyWrap.scrollTop(objects.$tableBodyWrap.scrollTop() + (rowHeight * options.scrollSetp));
                }
                objects.$scrollWrap.scrollTop(objects.$tableBodyWrap.scrollTop());
            });
            objects.$scrollWrap.scroll(function () {
                objects.$tableBodyWrap.scrollTop($(this).scrollTop());
            });
            $(window).resize(function () {
                DTS.SetHeight(tableWrap, objects, options);
            });
        }
        , ReInit: function () {
            this.ScrollWrap();//添加滑动区域

            this.objects = {
                $scrollWrap: this.tableWrap.find('.scrollwrap')
                , $scrollContent: this.tableWrap.find('.scrollcontent')
                , $tableBodyWrap: this.tableWrap.find('.table-body')
                , $tableContent: this.tableWrap.find('.table-body table')
            }

            //table容器样式设置
            this.tableWrap.css({ 'position': 'relative', 'width': this.options.width });
            //table BODY容器样式设置
            this.objects.$tableBodyWrap.css({ 'position': 'absolute', 'width': '100%', 'overflow': 'hidden' });

            DTS.SetHeight(this.tableWrap, this.objects, this.options);

            this.RegEvent();
        }
    };

    DTS.SetHeight = function (tableWrap, objects, options) {
        var tableHeight = $(window).height() - options.outRuleHeight - options.inRuleHeight;
        tableWrap.css({ 'height': tableHeight + 'px' });
        objects.$tableBodyWrap.css({ 'top': options.headerHeight + 'px', 'height': (tableHeight - options.headerHeight - options.inRuleHeight) + 'px' });
        objects.$scrollWrap.css('height', (tableHeight - options.headerHeight - options.inRuleHeight) + 'px');
        objects.$scrollContent.css({ 'height': objects.$tableContent.height() + 'px', 'lineHeight': objects.$tableContent.height() + 'px', 'width': '1px' });
    }

    DTS.defOptions = {
        //表头高度
        headerHeight: 0
        //内部排除高度,用于显示其他内容,例如分页
        , inRuleHeight: 0
        //外部排除高度,排除表格外固定的高度,用于表格在自适应时计算表格的高度
        , outRuleHeight: 0
        //滚动行数,滚轮一次所滚动的行数,以第一行高度为基准
        , scrollSetp: 5
        , width: '100%'
    }
})($, this);


String.prototype.Coding = function () {
    // public method for url encoding
    this.encode = function () {
        return escape(this._utf8_encode(this));
    };
    // public method for url decoding
    this.decode = function (string) {
        return this._utf8_decode(unescape(this));
    };
    // private method for UTF-8 encoding
    function _utf8_encode(string) {
        string = string.replace(/\r\n/g, "\n");
        var utftext = "";
        for (var n = 0; n < string.length; n++) {

            var c = string.charCodeAt(n);

            if (c < 128) {
                utftext += String.fromCharCode(c);
            } else if ((c > 127) && (c < 2048)) {
                utftext += String.fromCharCode((c >> 6) | 192);
                utftext += String.fromCharCode((c & 63) | 128);
            } else {
                utftext += String.fromCharCode((c >> 12) | 224);
                utftext += String.fromCharCode(((c >> 6) & 63) | 128);
                utftext += String.fromCharCode((c & 63) | 128);
            }

        }

        return utftext;
    };

    // private method for UTF-8 decoding
    function _utf8_decode(utftext) {
        var string = "";
        var i = 0;
        var c = c1 = c2 = 0;

        while (i < utftext.length) {

            c = utftext.charCodeAt(i);

            if (c < 128) {
                string += String.fromCharCode(c);
                i++;
            } else if ((c > 191) && (c < 224)) {
                c2 = utftext.charCodeAt(i + 1);
                string += String.fromCharCode(((c & 31) << 6) | (c2 & 63));
                i += 2;
            } else {
                c2 = utftext.charCodeAt(i + 1);
                c3 = utftext.charCodeAt(i + 2);
                string += String.fromCharCode(((c & 15) << 12) | ((c2 & 63) << 6) | (c3 & 63));
                i += 3;
            }
        }
        return string;
    }
};

String.prototype.encode = function (coding) {

    // private method for UTF-8 encoding
    function _utf8_encode(string) {
        string = string.replace(/\r\n/g, "\n");
        var utftext = "";
        for (var n = 0; n < string.length; n++) {

            var c = string.charCodeAt(n);

            if (c < 128) {
                utftext += String.fromCharCode(c);
            } else if ((c > 127) && (c < 2048)) {
                utftext += String.fromCharCode((c >> 6) | 192);
                utftext += String.fromCharCode((c & 63) | 128);
            } else {
                utftext += String.fromCharCode((c >> 12) | 224);
                utftext += String.fromCharCode(((c >> 6) & 63) | 128);
                utftext += String.fromCharCode((c & 63) | 128);
            }

        }

        return utftext;
    }

    if (!!coding && coding.toLocaleLowerCase() == "utf8")
        return escape(_utf8_encode(this.toString()));
    else
        return escape(this.toString());

};

String.prototype.decode = function (coding) {
    // private method for UTF-8 decoding
    function _utf8_decode(utftext) {
        var string = "";
        var i = 0;
        var c = c1 = c2 = 0;

        while (i < utftext.length) {

            c = utftext.charCodeAt(i);

            if (c < 128) {
                string += String.fromCharCode(c);
                i++;
            } else if ((c > 191) && (c < 224)) {
                c2 = utftext.charCodeAt(i + 1);
                string += String.fromCharCode(((c & 31) << 6) | (c2 & 63));
                i += 2;
            } else {
                c2 = utftext.charCodeAt(i + 1);
                c3 = utftext.charCodeAt(i + 2);
                string += String.fromCharCode(((c & 15) << 12) | ((c2 & 63) << 6) | (c3 & 63));
                i += 3;
            }
        }
        return string;
    }

    if (!!coding && coding.toLocaleLowerCase() == "utf8")
        return _utf8_decode(unescape(this.toString()));
    else
        return unescape(this.toString());

}
/*
    模板
    @ ：str  模板id或模板字符
    @ ：data 模板参数
*/
var myTempl = function (str, data) {
    data = data || {};
    str = (str.indexOf("#") == 0) ? $(str).html() : str;
    str = $.trim(str);
    var fn = new Function("o", "var p=[];with(o){p.push('" +
         str.replace(/[\r\t\n]/g, " ")
         .replace(/'(?=[^%]*%})/g, "\t")
         .split("'").join("\\'")
         .split("\t").join("'")
         .replace(/{%=(.+?)%}/g, "',$1,'")
         .split("{%").join("');")
         .split("%}").join("p.push('")
         + "');}return p.join('');");
    return fn.apply(data, [data]);
}
/*
  刷新当前异步数据
*/
var currentAsyncRequestFuc= function () {
    try {
        $(window).data('CurrentAsyncRequestFuc')();
    } catch (e) { }
}