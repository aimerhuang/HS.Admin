var UI = {
    /*******************************
    * 富文本编辑器
    * @selector : JQuery对象
    * @useroptions:用户配置项 详细设置参考:http://www.kindsoft.net/docs/option.html
    *******************************/
    Editor: function (selector, useroptions) { },

    /*******************************
    * 弹出框对象
    * remarks: 详细设置参考:http://www.planeart.cn/demo/artDialog/_doc/API.html
    * 被废弃的对象  禁止使用
    *******************************/
    Dialog: art.dialog,

    /*******************************
    * 关闭弹出框
    @ id:待关闭的对话框ID
           不传则关闭当前弹出的iframe对话框
    *******************************/
    CloseDialog: function (id) { },

    /*******************************
    * 获取弹出该弹出框的父页面window对象
    *******************************/
    DialogOpener: function () { },

    /*******************************
    *	弹出提示信息
    *	{
            @ content : 信息内容
            @ callback : 回调函数
            @ width : 对话框宽度,默认200px,内容超过15字可以根据内容长度可适当增加
        }
     *******************************/
    Alert: function (useroptions) { },

    /*******************************
    *	弹出确认信息
    *	{
            @ content : 信息内容
            @ ok : 点击确认后的回调函数
            @ cancel : 点击取消后的回调函数
            @ width : 对话框宽度,默认200px,内容超过15字可以根据内容长度可适当增加
        }
     *******************************/
    Confirm: function (useroptions) { },

    /*******************************
    * 弹出框
    * @useroptions:用户配置项 详细设置参考:http://www.planeart.cn/demo/artDialog/_doc/API.html
    *                       扩展了 {parent:true/false} 参数,默认true 该参数用来设置获取的dialog对象位置  设置true的时候获取的对象是top  false的时候为self
    *******************************/
    DialogBox: function (useroptions) { },

    /******************************* 
    * 弹出iframe
    * @url : 嵌入iframe的url地址
    * @options:用户配置项 详细设置参考:http://www.planeart.cn/demo/artDialog/_doc/API.html
    * @cache : 是否开启缓存,默认开启 true/false
    * remark: 操作iframe参考 http://www.planeart.cn/demo/artDialog/_doc/iframeTop.html
     *******************************/
    DialogOpen: function (url, useroptions, cache) { },

    /*******************************
    * 日历
    * @useroptions:用户配置项 详细设置参考:http://www.my97.net/dp/demo/index.htm
    *******************************/
    Date: function (useroptions) { },

    /*******************************
    * 树形控件
    * 使用方法参考:http://www.ztree.me/v3/api.php
    *******************************/
    ZTree: $.fn.zTree,

    /*******************************
    * AJAX等待锁对象遮罩
    * useroptions :  JSON参数对象
    * @obj :    要遮掉的JQUERY DOM对象
    * @opacity: 不透明度 0-1  未公开
    * @bgcolor:   背景颜色  未公开
    * @id :     唯一ID,创建了唯一ID后可以防止重复创建遮罩
    * @zindex:  Z轴深度
    * return : 返回一个对象,对象包含有可调用的Remove方法
    *******************************/
    Mask: function (useroptions) { },

    AutoComplete: function () { },



    /*========前端赵丽AND陈建=========*/

    //tips 弹出提示框，包括弹出与消失
    tips: {
        //specil_css指不同的弹出提示的区分不同的css用不同的css可以出现不同的提示信息，
        //word表示弹出的提示信息，指中间显示的中文信息.
        //此处弹出的控制是动态弹出，先用透明度隐藏，然后动态移除节点，因此这里的两个setTimeout方法中的时间必须后者大于前者
        //flag指弹出提示是否自动消失的选择，如果弹出提示不需要自动隐藏设置为true，如果需要自动隐藏请忽略这个参数，直接不用设置值或者设置为false
        tip_alert: function (specil_css, word, flag) { },

        //下面的函数是弹出的删除

        tip_delete: function () { }
    },
    /********************/
    //btn指点击弹出的button
    //bomstr指右边的弹出框的id
    //closebtn指右边弹出框上面右边的x,可以关闭弹出框
    //deletebtn指弹出框下面的清除按钮
    /********************/
    searchbox: function (btn, bomstr, closebtn, deletebtn) { },

    /*******************/
    //下面的方法是用于开发票的显示与隐藏
    //bomstr指包含两个框的外面那个框的id
    //actdomstr代表需要隐藏与显示的那些dom
    //changestr代表触发改变的dom的标示
    /*******************/
    invoice: function (bomstr, actdomstr, changestr) { },

    /********************/
    //下面是取地区那种模拟下拉选择框
    //页面调用Area方法，参数依次为省的select控件id，市的id，区域的id，和数组(包含省市区联动反绑定的区域id，回调函数)。
    //取值只需要区第三个select控件的值(区域select控件的值)
    //示例代码:初始化联动控件Area($("#province"), $("#city"), $("#area"), { callback: function (type) { } });
    //反向绑定Area($("#province"), $("#city"), $("#area"), { a: a, callback: function (type) { } });
    //取值$("#area").val() 其中province为省的select控件id，city市id，area区域id，a为反绑定时的区域id，callback为回调函数type参数提供开始加载和加载完成状态
    /********************/
    Area: function (province, city, area, attr) { },


    /*=================/
		tab选项卡
	 	tabNavBox:'#tabboxs', 				最大的BOX容器
        tabNavObj:'.tabNav',  				选项卡UL样式
		tabNavBtn:'li',								选项卡下面的LI
		tabContentObj:'.tabContent', 	控制下面box
		tabContent:'.list',						控制box下面的隐藏显示层
		currentClass:'menuon', 				选项卡的样式
		eventType:'click',    				选项卡的点击方式
		onActiveTab: null							选项卡的点击的扩展方法
		controlUnit:true,    					控制选项可不可会
		controlClass:null							启用选项卡样式

	======================*/
    Tab: function (useroptions) { },


    /*****************************
   * 设置table 选择(checkbox) 插件
   * tableSelector: 要进行设置的table选择器如:"#tableName"
   * checkallcss:  全选checkbox样式（标题行的全选）选择器，如:".checkall"
   * onChangeCallBack：checkbox的change事件
   * 返回：选择器对象
   * 提供方法：GetCheckboxSelectedItem():返回被选中的所有tr对象。    
   * 2013-07-04 邵斌 重构
   * 2013-07-31 邵斌 重构   加入onchange事件
   ******************************/
    CheckAllbox: function (tableSelector, checkallcss) { },



    /*TabelReplace表格的颜色交替显示与hover
      tableboxclass 包含table的div
      oddclass  单数行的样式
      oddclass  双数行的样式
      hoverclass hover的样式
      */
    TabelReplace: function (tableboxclass, oddclass, evenclass, hoverclass) { },



    /*创建选项卡
    creatuserid选项唯一userid
    creaturl打开的页面url
    creattext 选项卡的标题
    */
    OpenCreatTab: function (creattext, creaturl) { },

    /*********
    删除选项卡
    objid当前选项卡的userid
    *******/
    CloseTab: function (objdelx, url, title) { },
    
    RefreshTab: function (url) { },

    /*Numbercontrol数字增减
		numberinputselect:".boxs_listtabel input[class='number_input']", //筛选input
		eventType:'click',  //事件的触发方式
		minNumber : 0,  //最小数
		maxNumber : 100,	//最大数
		cutActiveNum:null, //减少时回调函数
		addActiveNum:null, //增加时回调函数
		change:null,       //数字改变事件
		step:1  //一次的增减数
	*/
    Numbercontrol: function (useroptions) { },




    /*******************************
    * 绑定zTree选择节点到指定的控件上
    * element :  指定的html控件
    * treeNodes :  zTree的节点对象集合
    *******************************/
    BindDataFromZTreeNodes: function (element, treeNodes) { },

    ///<summary>
    ///局部AJAX操作提示信息
    ///<summary>
    ///<returns>Object</returns>
    Prompt: {
        ///<summary>
        ///AJAX运行中提示
        ///<summary>
        ///<param name="msg" type="string">消息内容,默认'数据处理中'</param>
        ///<returns>undefined</returns>
        Runing: function (msg) { },
        ///<summary>
        ///AJAX成功提示
        ///<summary>
        ///<param name="msg" type="string">消息内容,默认'数据处理中'</param>
        ///<returns>undefined</returns>
        Success: function (msg) { },
        ///<summary>
        ///AJAX错误提示
        ///<summary>
        ///<param name="msg" type="string">消息内容,默认'数据处理中'</param>
        ///<returns>undefined</returns>
        Error: function (msg) { }
    },

    /*******************************
    * 绑定数据到指定控件上
    * element :  指定的html控件
    * data    :  数据
    *******************************/
    BindDataToElement: function (element, data) { },


    /*******************************
    * 自动绑定table列表，将json对象数组的数据安装table中提供的模板绑定到table
    * 注意：需要依赖 Plugins/TableBind.yui.js
    *
    * useroptions :  指定的html控件
    * @ajaxOptons       请求参数，同$.ajax()的请求参数
    * @table table对象，可以是table的jQuery对象或者是jquery筛选语法
    * @data             用于绑定的数据源
    * @key              数据主键同数据库中的主键
    * @rowCss           普通行样式
    * @hoverCss         鼠标响应行样式
    * @alternatelyCss   交替行样式 
    * @selectedCss      被选行样式
    * @onBind           数据绑定时触发事件
    * @beforeRowBind    行绑定数据前时触发事件
    * @afterRowBind     行绑定数据后时触发事件
    * @onSelectedRow    选中行时触发事件
    * @onMoveRow        在移动行时触发事件
    * @onMoveRow        在移动行时触发事件
    * @onDelete         在删除行时触发
    *
    *返回TableBind对象实体
    *提供方法
    * GetItemByKey(key)         根据主键获取某行tr行对象
    * GetDateByKey(key)         根据主键获取json数据
    * UpdateRow(newData)        更新行数据
    * DelRow(key)               根据主键或者是行对象删除行
    * GetTrKeyValue(tr)         提取行的主键值
    * InsertRow(data)           插入新行,数据必须是数组
    * Selected(key)             根据主键选中行
    * GetCurrentRow(key)        获取当前行
    * GetSelectItems(key)       获取所以被选中的行
    * Move(isUp)                移动行，isUp: true-向上移动 false-向下移动
    * Clear()                   清除所以数据行
    *******************************/
    TableBind: function (useroptions) { },

    /*******************************
		陈建创建：
		后台促销，tooltip提示
    *******************************/
    Promotion_tip: function () { },
    /*******************************
		陈建创建：
		后台促销，组的收起和展开
    *******************************/
    Promotion_group: function () { },

    /*******************************
		余勇创建：
		图表数据绑定
    *******************************/
    Highcharts: function (options) {
    },

    /*******************************
		朱成果创建：
		显示百度地图
        mapdiv;显示地图对象
        cityNo:城市编号
        cityName:城市名称
        mapX:经度
        mapY：纬度
    *******************************/
    ShowBaiduMap:function(mapOptions)
    {

    },
    
     /*******************************
		余勇创建：
		常用文本选择
    *******************************/
    BsTextSelect: function (obj,options) {
    },
    
};

/*******************************
		朱成果创建：
		显示百度地图
        mapdiv;显示地图对象
        cityNo:城市编号
        cityName:城市名称
        mapX:经度
        mapY：纬度
    *******************************/
UI.ShowBaiduMap=function(mapOptions)
{
    var url = '/order/showbaidumap?cityName=' + mapOptions.cityName + '&cityNo=' + mapOptions.cityNo + '&x=' + mapOptions.mapX + '&y=' + mapOptions.mapY;
    $('<iframe>', {
        src: url,
        id: 'mapFrame',
        frameborder: 0,
        scrolling: 'no',
        width: '100%',
        height: '100%'
    }).appendTo(mapOptions.mapdiv);
}

UI.Prompt = {
    Runing: function (msg, element) {
        element && UI.Prompt.DisabledControl(element);
        UI.Prompt.RuningQueue.push(msg);//加入运行队列

        //如果正在运行并且运行队列大于一,则重设,否则添加
        if (UI.Prompt.IsRuning && UI.Prompt.RuningQueue.length > 1) {
            UI.Prompt.ResetRuningQueue();
        } else {
            //如果运行队列为空,则移除所有显示的TIPS 并且清空通知队列
            UI.Prompt.RemoveTips();
            UI.Prompt.NotifyQueue = [];

            var html = $('<div class="tips tips_shrot tips_warning">'
                + '<span class="icons"></span>'
                + msg
                + '<span class="c6 runingqueue"></span>'
                + '</div>');
            $('body').append(html);
        }
    },
    Success: function (msg, element) {
        UI.Prompt.NotifyQueue.push({ type: 'success', content: msg });
        UI.Prompt.RuningQueue.length > 0 && UI.Prompt.RuningQueue.pop();
        UI.Prompt.ShowNotify();
    },
    Error: function (msg, element) {
        element && UI.Prompt.EnabledControl(element);
        UI.Prompt.NotifyQueue.push({ type: 'wrong', content: msg });
        UI.Prompt.RuningQueue.length > 0 && UI.Prompt.RuningQueue.pop();
        UI.Prompt.ShowNotify();
    },
    ShowNotify: function () {
        if (UI.Prompt.RuningQueue.length == 0) {
            UI.Prompt.RemoveTips(); //如果运行队列为空，则移除所有TIPS

            var notify = UI.Prompt.NotifyQueue.shift();

            if (typeof notify !== 'undefined') {
                var html = $('<div class="tips tips_shrot tips_' + notify.type + '">'
                    + '<a href="javascript:/*close*/;" class="prompt-delete" title="关闭">&times;</a><span class="icons"></span>'
                    + notify.content
                    + '</div>');
                html.find('.prompt-delete').click(function () {
                    UI.Prompt.ShowNotify();
                    sign && clearTimeout(sign);
                });
                if (UI.Prompt.NotifyQueue.length > 0) {
                    html.find('.prompt-delete').attr('title', '下一条').text('∨');
                }
                $('body').append(html);

                var sign = setTimeout(function () {
                    UI.Prompt.ShowNotify();
                }, notify.type === 'wrong' ? 6000 : 3000);
            }
        } else {
            UI.Prompt.ResetRuningQueue();
        }
    },
    RemoveTips: function () {
        $('.tips').remove();
    },
    IsRuning: function () {
        return $('.tips_warning').length > 0;
    },
    ResetRuningQueue: function () {
        if (UI.Prompt.RuningQueue.length > 1) {
            $('.tips_warning .runingqueue').text('(队列:' + UI.Prompt.RuningQueue.length + ')');
        } else {
            $('.tips_warning .runingqueue').text('');
        }
    },
    DisabledControl: function (element) {
        element = $(element);
        element.attr('disabled', 'disabled').addClass('disabled');
    },
    EnabledControl: function (element) {
        element = $(element);
        element.removeAttr('disabled', 'disabled').removeClass('disabled');
    },
    RuningQueue: [],
    NotifyQueue: []
};

UI.tips.tip_alert = function (specil_css, word, flag) {
    var true_flag = flag || false;
    var html = [];
    html.push('<div class="tips ' + specil_css + '" id="JS_tip">');
    html.push('<a href="javascript:;" onclick="UI.tips.tip_delete();">&times;</a><span class="icons"></span>' + word + '');
    html.push('</div>');
    var str = html.join('');
    $("body").append(str);
    if ($("#JS_tip") && (!true_flag)) {
        setTimeout(function () {
            $("#JS_tip").animate({
                opacity: 0
            }, 500, function () {
                $(this).remove();
            });
        }, 6000);
        //setTimeout(function () {
        //    $("#JS_tip").remove();
        //}, 10500);
    }
}

UI.tips.tip_delete = function () {
    if ($("#JS_tip")) {
        $("#JS_tip").animate({
            opacity: 0
        }, 500);
        setTimeout(function () {
            $("#JS_tip").remove();
        }, 500);
    }
}

UI.searchbox = function (btn, bomstr, closebtn, deletebtn) {
    var js_searchbox = $(bomstr);
		var WinH=$(window).height();
		js_searchbox.children('.search_body').css('max-height',WinH-132)
    btn.bind("click", function () {
        js_searchbox.animate({ right: 0 + "px" }, 300, function () {

            $(".case").one("click", function () {
                $(bomstr).animate({
                    right: -271 + "px"
                }, 300);
            });

        });
        return false;
    });

    closebtn.click(function () {
        $(bomstr).animate({
            right: -271 + "px"
        }, 300);
    });
    deletebtn.click(function () {
        $("input", bomstr).val('');
        $("select", bomstr).val('');
    });
}

UI.invoice = function (bomstr, actdomstr, changestr) {
    $(changestr, bomstr).bind("change", function () {
        $(actdomstr, bomstr).toggleClass("hide");
    });
}

UI.Area = function (province, city, area, attr) {
    var postUrl = "/Ajax/GetArea";        //地区数据请求地址

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
                    province.append("<option value='" + item.SysNo + "'>" + item.AreaName + "</option>");
                }
            });
            attr.callback.call($(this), 'end');
        });
    }

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
        var p, c;
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
                }
                else {
                    alert("数据异常，应该传入区域id");
                    return;
                }
            }
        });

        $.ajax({
            url: postUrl,
            cache: true,
            async: false,
            type: "post",
            data: {},
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
        province.val(p);
        city.val(c);
        area.val(a);
    }


}


// tab选项卡
UI.Tab = function (options) {
    // 处理默认参数   
    var opts = $.extend({}, UI.Tab.defaults, options);

    $(opts.tabNavBox).each(function () {
        var $this = $(this),
		$tabNavObj = $(opts.tabNavObj, $this),
		$tabContentObj = $(opts.tabContentObj, $this),
		$tabNavBtns = $(opts.tabNavBtn, $tabNavObj),
		$tabContentBlocks = $(opts.tabContent, $tabContentObj);

        $tabNavBtns.bind(opts.eventType, function () {
            var $that = $(this);
            var _index;
            //判断是否有controlClass控制样式
            if (opts.controlClass != null) {
                //$(opts.controlClass,$tabNavObj)
                $tabActiveNavBtns = $("." + opts.controlClass, $tabNavObj);
                _index = $tabActiveNavBtns.index($that);

            } else {
                if (opts.controlUnit == false) return;
                _index = $tabNavBtns.index($that);

            }

            if ($tabNavObj.attr("init") == undefined) {
                $tabNavObj.attr("init", "true");
                OpenTabEvent(_index, true);
            }

            //扩展方法传递
            if (opts.onActiveTab != null && typeof (opts.onActiveTab) == "function") {
                var result = opts.onActiveTab(_index, $that);
                if (result != null && result == false)
                    return false;
            }

            if (_index == -1) return;
            OpenTabEvent(_index, true);

        }).eq(0).trigger(opts.eventType);
    });
    // 保存JQ的连贯操作结束

    //根据index打开选项卡
    function OpenTabEvent(mun, isClick) {
        if (mun == null) return;
        $tabNavBtns = $(opts.tabNavBtn, $(opts.tabNavObj, opts.tabNavBox));
        $tabContentBlocks = $(opts.tabContent, $(opts.tabContentObj, opts.tabNavBox));
        $tabNavBtns.eq(mun).addClass(opts.controlClass);
        $tabNavBtns.eq(mun).addClass(opts.currentClass).siblings(opts.tabNavBtn).removeClass(opts.currentClass);
        $tabContentBlocks.eq(mun).show().siblings(opts.tabContent).hide();

        //扩展方法传递
        if (isClick == null && opts.onActiveTab != null && typeof (opts.onActiveTab) == "function") {
            var result = opts.onActiveTab(mun, $tabNavBtns);
            if (result != null && result == false)
                return false;
        }

    }


    this.Active = OpenTabEvent;
    return this;
};
//插件主要内容结束
//插件的defaults     
UI.Tab.defaults = {
    tabNavBox: '#tabboxs',
    tabNavObj: '.tabNav',
    tabNavBtn: 'li',
    tabContentObj: '.tabContent',
    tabContent: '.list',
    currentClass: 'menuon',
    eventType: 'click',
    onActiveTab: null,
    controlUnit: true,
    controlClass: null
};
//----------------
UI.Numbercontrol = function (options) {

    //模板方法
    Template = function () {
        /*
            <div class="number_btn"><button title="减少" class="btn cutbtn wd28 btn_ht26" type="button"><span class="icon_minus_sign"></span></button><button title="增加" class="btn addbtn wd28 btn_ht26" type="button"><span class="icon_plus_sign"></span></button></div>
        */
    }
    //模板方法
    Template2 = function () {
        /*
            <div class="number_btn"><button title="减少" class="btn cutbtn wd28 btn_ht26 disabled" type="button"><span class="icon_minus_sign"></span></button><button title="增加" class="btn addbtn wd28 btn_ht26 disabled" type="button"><span class="icon_plus_sign"></span></button></div>
        */
    }
		
    // 处理默认参数   
    var opts = $.extend({}, UI.Numbercontrol.defaults, options);
		if(opts.available){
			return $(opts.numberinputselect).each(function () {
					if($(this).prev().attr("type") == "button"){
							if($(this).prev(opts.leftbtn).length > 0)
								return this;
					}
				
					var $this = $(this); 
					//var regNumber = /^\d+(\.\d{0,})?$/;
					//var regNumber = /^[0-9]{0}([0-9]|[.])+$/;
					var regNumber = /^\d*\.{0,1}\d{0,}$/;
					//正则只能输入数字
					$this.keyup(function() {
									if(!regNumber.test(this.value))
										//this.value = this.value.substring(0,this.value.length-1);
										this.value = '';
							}).bind("afterpaste", function (e) {
									if(!regNumber.test(this.value))
										this.value = '';
							});
					
					
					//$numberboxObj=$(opts.numberboxObj,$this), 再次选择
					if ($this.attr("type").toLowerCase() != "text")
							return false;
	
					$numberboxObj = $this.parent();
					//设置和获取参数
					var template = Template.getMultiLine();
					$this.before(template);
					$this.prev().children(":eq(0)").after($this);
					//$numberboxObj.prepend(template);
					//template = template.replace("{x}",$numberboxObj.html());
					//$numberboxObj.children("div").children("input").replaceWith($this);
					$minNumber = opts.minNumber;
					$maxNumber = opts.maxNumber;
					$step = opts.step;
					$cutActiveNum = opts.cutActiveNum;
					$addActiveNum = opts.addActiveNum;
					
					//文本数据改变
					$this.bind('change',function(){
							if ($.isFunction(opts.change))
									opts.change($this);
							return false;
					})
	
					//var cutbtn = $numberboxObj.children(opts.numberboxObj).children(opts.leftbtn);
					//var addbtn = $numberboxObj.children(opts.numberboxObj).children(opts.rightbtn);
					//减少数方法
					$this.prev().bind("click",function () {
							var $that = $(this);
							var inputvar = parseInt($that.next("input").val());
							if (!isNaN(inputvar) && ($minNumber == null || inputvar > $minNumber)) {
									$that.next("input").val(inputvar - $step);
							}
							//扩展方法传递
							if ($cutActiveNum != null && typeof ($cutActiveNum) == "function") {
									$cutActiveNum($that.next("input"));
							}
	
							//修改事件
							if ($.isFunction(opts.change))
									opts.change($that.next("input"));
									
							return false;
					});
					//增加数方法
					$this.next().bind("click", function () {
							
							var $that = $(this);
							var inputvar = parseInt($.trim($that.prev("input").val()) == '' ? '0' : $that.prev("input").val());
							if (!isNaN(inputvar) && ($maxNumber == null || inputvar < $maxNumber)) {
									$that.prev("input").val(inputvar + $step);
							}
							//扩展方法传递
							if ($addActiveNum != null && typeof ($addActiveNum) == "function") {
									$addActiveNum($that.prev("input"));
							}
	
							//修改事件
							if ($.isFunction(opts.change))
									opts.change($that.prev("input"));
							return false;
					});
				});
				// 保存JQ的连贯操作结束			
			}else{
				return $(opts.numberinputselect).each(function(){
					var $this = $(this); 
					//设置和获取参数
					var template2 = Template2.getMultiLine();
					$this.before(template2);
					$this.prev().children(":eq(0)").after($this);
					$this.addClass('disabled').attr('disabled','disabled');
				});				
			}
};
// 插件Numbercontrol的defaults     
UI.Numbercontrol.defaults = {
		numberbox:".boxs_listtabel",
    numberinputselect: ".boxs_listtabel input[class='number_input']",
    //numberboxObj: ".number_btn",
    eventType: 'click',
    leftbtn: ".cutbtn",
    rightbtn: ".addbtn",
    minNumber: 0,
    maxNumber: null,
    cutActiveNum: null,
    addActiveNum: null,
    change: null,
		available:true,
    step: 1
};


//checkallbox全选
//2013-07-04 邵斌 扩展获取所有被选择的tr行
UI.CheckAllbox = function (tableSelector, checkallcss, onChangeCallBack) {

    //获取全选checkbox对象
    var checkboxobj = $("input[type='checkbox']" + checkallcss + "", tableSelector);

    //绑定全选checkbox的click事件
    checkboxobj.live("click", function () {

        //判断点击的状态并同时修改列表中的所以checkbox状态,做状态同步
        if ($(this).attr("checked") == "checked") {
            //全选
            $(this).parents(tableSelector).find("input[type='checkbox'][disabled!='disabled']").attr("checked", true);
        }
        if ($(this).attr("checked") != "checked") {
            //取消全选
            $(this).parents(tableSelector).find("input[type='checkbox']").attr("checked", false);
        }
        
    });


    $("input[type='checkbox']", tableSelector).live("change", function () {
         
        //判断是否处罚外部change事件
        if (typeof (onChangeCallBack) == "function") {
            onChangeCallBack(this);
        }

        //如果当前checkbox为选中就不管全选checkbox状态，如果是取消将同步全选checkbox
        //如果是选中就返回
        if ($(this).is(":checked"))
            return true;

        var allCheckBox = $("input.checkall[type='checkbox']", $(this).closest("table"));
        if (allCheckBox.length > 0) {
            //清理全选checkbox
            allCheckBox.attr("checked", null);
        }
        return true;

    });

    //返回所有备选选择的tr行
    this.GetCheckboxSelectedItem = function () {
        //如果的到的父容器为空将不做任何操作
        var dataTable = $(tableSelector);
        var tableId = tableSelector;
        if (dataTable.length == 0) {
            return dataTable;
        }

        //的到table除全选以外所有的checkbox
        var checkedItems = $("tr input[type='checkbox']:checked", tableSelector).not($(".checkall", tableSelector))
        var result = new Array();   //结果集
        var tr;                     //tr临时变量

        //遍历结果集并找到tr
        for (var i = 0; i < checkedItems.length; i++) {

            tr = $(checkedItems[i]);
            //如果当前项不是tr就像上查找直到找到tr
            while (tr[0].tagName.toLowerCase() != "tr") {

                //如果找到父节点都到了body表示DOM有错误
                if ($(tr).parent()[0].tagName.toLowerCase() == "body") {
                    return false;
                }

                tr = $(tr).parent();
            }

            //组合结果集
            result.push(tr);
        }

        return result;
    }


    return this;
}

//表格的颜色交替显示与hover
UI.TabelReplace = function (tableboxclass, oddclass, evenclass, hoverclass) {
    if (typeof (tableboxclass) != 'string') return;
    $(tableboxclass + " tr:odd").addClass(oddclass);
    $(tableboxclass + ' tr:even').addClass(evenclass);
    $(tableboxclass + " tr").hover(function () {
        $(this).addClass(hoverclass)
    }, function () {
        $(this).removeClass(hoverclass)
    });
}


/*创建选项卡
creatuserid选项唯一userid
creaturl打开的页面url
creattext 选项卡的标题
*/
UI.OpenCreatTab = function (creattext, creaturl) {
    //随机数生成

    function GetRandomNum(Min, Max) {
        var Range = Max - Min;
        var Rand = Math.random();
        return (Min + Math.round(Rand * Range));
    }
    var win = window;
    while (window.parent) {
        win = win.parent;
        //判断是不是最顶层
        if (win == win.parent) {
            var breadname = win.$(".current").children("span:last").html() + ' > ' + creattext;
            var userid = win.CreatTabEvent(GetRandomNum(0, 888), creattext, creaturl, breadname);
            return userid;
        }
    }
};
/*********
删除选项卡
objid当前选项卡的userid
*******/
UI.CloseTab = function (objid, url, title,isReload) {
 var win = window;
    while (window.parent) {
        win = win.parent;
        //判断是不是最顶层
        if (win == win.parent) {
            //如果objid为空，则取当前选项卡的userid
            if (objid == null) {
                var userid = win.$("#carousel_ul").find(".menuon").attr("userid");
                objid = userid;
            }
            if (objid != null || url) {
                //刷新指定url窗体
                if (url) {
                    var node = win.$('#iframewrap').find("iframe[src='" + url + "']");
                    if ($(node).length > 0) {
                       win.SetCurTab(node.attr("userid"));
                       win.CloseCurTab(objid);
                       if (isReload && node[0].contentWindow.doSearch) {
                           window.setTimeout(node[0].contentWindow.doSearch(1), 1000);
                       } else {
                           node.attr("src", url);
                       }
                    } else if (title) {
                        userid= UI.OpenCreatTab(title, url);
                        win.SetCurTab(userid);
                        win.CloseCurTab(objid);
                    }
                } else {
                    win.ReloadFrame();
                    win.UseridDelTab(objid);
                }
                return;
            }
        }
    }
};

UI.RefreshTab = function(url, title) {
    if (url) {
        var win = window;
        while (window.parent) {
            win = win.parent;
            //判断是不是最顶层
            if (win == win.parent) {
                //刷新指定url窗体
                var node = win.$('#iframewrap').find("iframe[src='" + url + "']");
                if ($(node).length > 0) {
                    node.attr("src", url);
                } else if (title) {
                    UI.OpenCreatTab(title, url);
                }
            }
        }
    }
};


UI.Mask = function (optionsin) {

    var options = $.extend({
        opacity: 0.2,
        bgcolor: '#dadada',
        id: 'mask_' + Math.round(Math.random() * 10000000),
        zindex: 99
    }, optionsin);

    var obj = options.obj;

    //不传入对象 
    //如果已存在遮罩 直接退出
    if (typeof obj == 'undefined' || $('#' + options.id).length > 0) return;

    var $maskFrame = $(''
        + '<div id="' + options.id + '">'
        + ' <div style="position:position;top:0;left:0;height:' + obj.height() + 'px;width:' + obj.width() + 'px;"></div>'
        + ' <div class="mask_screen" style="height:' + obj.height() + 'px;width:' + obj.width() + 'px;position:absolute;top:0;left:0;color:red;"></div>'
        + ' <div class="mask_content" style="height:100%;width:100%;position:absolute;top:0;left:0;">'
        + '     <div style="text-align:center;margin-top:' + obj.height() / 2 + 'px"><img src="/Theme/Images/loading.gif"/></div>'
        + ' </div>'
        + '</div>'
    ); //2013-6-18 杨浩 修改 $maskFrame

    $maskFrame.css({
        position: 'absolute',
        width: obj.width(),
        height: obj.height(),
        top: obj.offset().top,
        left: obj.offset().left,
        zIndex: options.zindex
    })
        .find('.mask_screen').css({
            //opacity: options.opacity,
            //backgroundColor: options.bgcolor
            opacity: '0.2',
            backgroundColor: '#dadada'
        })
        .find('.mask_content').css({
            zIndex: options.zindex
        })
        .find('iframe').css({
            opacity: 0
        });

    obj.after($maskFrame);
    var _overflow = obj.css('overflow');
    obj.css({ overflow: 'hidden' });

    return {
        Remove: function () {
            $maskFrame.remove();
            obj.css({ overflow: _overflow });
        }
    };
};

UI.Date = function (useroptions) {
    return WdatePicker(useroptions);
};

UI.CloseDialog = function (id) {
    if (typeof id !== 'undefined') {
        art.dialog.list[id].close();
    } else {
        art.dialog.close();
    }
};

UI.DialogOpener = function () {
    return art.dialog.opener;
};

UI.Alert = function (useroptions) {
    var options = $.extend({
        content: '',
        callback: function () {
        },
        width: '200px',
        icon: 'warning'
    }, useroptions);
    return UI.DialogBox({
        id: 'dialog_alert',
        title: '提示',
        parent: true,
        lock: true,
        fixed: true,
        padding: '20px 15px 20px 10px',
        ok: true,
        width: options.width,
        content: options.content,
        close: options.callback,
        icon: options.icon
    });
};

UI.Confirm = function (useroptions) {
    var options = $.extend({
        content: '',
        ok: function () {
        },
        cancel: function () {
        },
        id: 'dialog_confirm',
        width: '200px'
    }, useroptions);

    return UI.DialogBox({
        id: options.id,
        title: '确认操作',
        parent: true,
        lock: true,
        fixed: true,
        icon: 'question',
        padding: '20px 15px 20px 10px',
        ok: options.ok,
        width: options.width,
        content: options.content,
        cancel: options.cancel
    });
};

UI.DialogBox = function (useroptions) {
    var options = $.extend({
        title: '提示信息',
        width: '300px',
        parent: true
    }, useroptions);

    if (options.parent) {
        //顶层artDialog对象
        return art.dialog.top.art.dialog(options);
    }
    //当前artDialog对象
    return art.dialog(options);
};

UI.DialogOpen = function (url, useroptions, cache) {
    var options = $.extend({}, useroptions);

    //当前artDialog对象
    return art.dialog.open(url, options, cache);
};

UI.Editor = function (selector, useroptions) {
    var options = $.extend({
        items: [
            'source', '|', 'undo', 'redo', '|', 'preview', 'cut', 'copy', 'paste', 'plainpaste', 'wordpaste', '|', 'justifyleft', 'justifycenter', 'justifyright', 'justifyfull', 'insertorderedlist', 'insertunorderedlist', 'indent', 'outdent', 'subscript', 'superscript', 'clearhtml', 'quickformat', 'selectall', '|', 'fullscreen', '/', '|', 'formatblock', 'fontname', 'fontsize', '|', 'forecolor', 'hilitecolor', 'bold', 'italic', 'underline', 'strikethrough', 'lineheight', 'removeformat', '|', 'image', 'multiimage', 'table', 'anchor', 'link', 'unlink'
        ],
        uploadJson: '/Theme/Plugins/Upload/upload_json.ashx',
        allowFileManager: true
    }, useroptions);
    return KindEditor.create(selector, options);


};

UI.BindDataFromZTreeNodes = function (element, treeNodes) {
    //如果没有任何值就将弹出层隐藏
    if (treeNodes == null) {
        return false;
    }

    //存格式化后的值变量
    var valList = "";
    var valTxt = "";
    var newLine = "\r\n";

    //容器置空
    element.html("");
    element.val("");

    //容器控件的标签名称
    var tagName = element[0].tagName.toLocaleLowerCase();

    //根据容器控件类型来设置换行符
    if (tagName == "textarea") {
        newLine = newLine;
    } else if (tagName == "select") {
        newLine = "";
    } else {
        newLine = "<br/>";
    }

    for (var i = 0; i < treeNodes.length; i++) {
        //拼接值字符串,分类ID默认用“，”分割
        if (valList.length > 0) {
            valList += ",";
            valTxt = newLine;
        }
        valList += treeNodes[i].id;
        valTxt += treeNodes[i].FullName;

        //设置显示文本
        if (tagName == "input" && element.attr("type") == "text") {
            //处理Input控件
            element.val(element.val() + valTxt);
        } else if (tagName == "select") {
            //处理select控件
            element.html(element.html() + "<option value='" + treeNodes[i].id + "'>" + valTxt + "</option>");
        } else if (tagName == "textarea") {
            element.val(element.val() + valTxt);
        } else {
            //其他控件默认都插入html
            element.html(element.html() + valTxt);
        }
    }

};

UI.BindDataToElement = function (element, data) {
    //存格式化后的值变量
    var valList = "";

    //容器控件的标签名称
    var tagName = element[0].tagName.toLocaleLowerCase();

    //设置显示文本
    if (tagName == "input" && element.attr("type").toLowerCase() == "text") {
        //处理Input控件 文本框
        element.val(data);
    } else if (tagName == "input" && element.attr("type").toLowerCase() == "hidden") {
        //处理Input控件 隐藏域
        element.val(data);
    } else if (tagName == "input" && element.attr("type").toLowerCase() == "checkbox") {
        //处理Input控件 多选控件
        if (eval(data) == 1 || eval(data) == true)
            element.attr("checked", "checked");
        else
            element.attr("checked", null);
    } else if (tagName == "input" && element.attr("type").toLowerCase() == "radio") {
        //处理Input控件 单选控件
        if (eval(data) == 1 || eval(data) == true)
            element.attr("checked", "checked");
        else
            element.attr("checked", null);
    } else if (tagName == "select") {
        //处理select控件
        if (element.children("option[value='" + data + "']").length > 0) {
            element.children("option[selected='selected']").attr("selected", null);
            element.children("option[value='" + data + "']").attr("selected", "selected");
        }
    } else if (tagName == "textarea") {
        element.val(data);
    } else if (tagName == "img") {
        element.attr("src", data);
    } else {
        //其他控件默认都插入html
        element.html(data);
    }
};

//可配置的自动完成组件 杨文兵
/* 自动完成组件调用示例
UI.AutoComplete("id",configObject);

config示例
{
    postUrl:"/ajax?word={0}",
    width:200
    height:400, 高度 最小200
    showHeader:true/false;
    columns:[{header:"姓名",width:60,render:function(data){return data.name} }],
    left:0,
    top:0,
    callBack:function(o){
        //回调函数
    }

    data:ajax请求返回的数据
    selectIndex:当前选中行的索引
    table:显示表格的jquery选择器
    className:自动完成层的jquery选择器
    delayhide:  延迟关闭自动完成层
}
*/

UI.AutoComplete = function (inputID, config) {
    var input = document.getElementById(inputID);
    var jel = $(input);

    config.top = config.top || 0;
    config.left = config.left || 0;
    config.height = config.height || 200;
    config.width = config.width || 200;

    config.className = ".autocomplete" + inputID;
    if ($(config.className).length < 1) {
        var headerHtml = "";
        if (config.showHeader == true) {
            headerHtml += "<table class=\"autoHeader\" style=\"width:100%\" border=\"0\" cellspacing=\"0\" cellpadding=\"0\"><tr>"
            for (var i = 0; i < config.columns.length; i++) {
                var width = "";
                if (config.columns[i].hasOwnProperty("width") == true) width = " width=\"" + config.columns[i].width + "\"";
                headerHtml += "<th" + width + ">" + config.columns[i].header + "</th>";
            }
            headerHtml += "</tr></table>"
        }
        jel.after("<div class=\"autocomplete" + inputID + " autoComplete\" style=\"width:" + config.width + "px;\">" + headerHtml
            + "<div class=\"autoTable\" style=\"height:" + config.height + "px;overflow-y:auto;\"><table id=\"autotable" + inputID
            + "\" style=\"width:100%\" border=\"0\" cellspacing=\"0\" cellpadding=\"0\"></table></div></div>");
        $(config.className).css("top", jel.offset().top + jel.height() + 5 + config.top)
                           .css("left", jel.offset().left + config.left);
    }
    config.datas = [];
    config.selectIndex = -1;
    config.id = inputID;
    config.table = $("#autotable" + inputID);
    config.start = false;
    input.config = config;
    jel.blur(function () {
        this.config.delayhide = setTimeout(function () { $(config.className).hide(); }, 500);
    }).keydown(function (event) {
        config = this.config;
        $(config.className).show();

        switch (event.keyCode) {
            case 27://esc
                $(config.className).hide();
                break;
            case 13://enter                
                if (config.selectIndex >= 0) {
                    config.callback(config.datas[config.selectIndex]);
                    $(config.className).hide();
                }
                return false;
            case 38://up                
                config.table.find("tr.o").removeClass("o");
                config.selectIndex--;

                if (config.selectIndex < 0) {
                    config.selectIndex = config.table.find("tr").length - 1;
                }
                //alert(config.selectIndex);
                config.table.find("tr:eq(" + config.selectIndex + ")").addClass("o");
                UI.AutoComplete.setScroll($(config.className).find(".autoTable"), config.table.find("tr.o"));
                return false;
            case 40://down
                config.table.find("tr.o").removeClass("o");
                config.selectIndex++;

                if (config.selectIndex > config.table.find("tr").length - 1) {
                    config.selectIndex = 0;
                }
                //alert(config.selectIndex);
                config.table.find("tr:eq(" + config.selectIndex + ")").addClass("o");
                UI.AutoComplete.setScroll($(config.className).find(".autoTable"), config.table.find("tr.o"));
                return false;
            default: break;
        }
    })
         .keyup(function (event) {
        if (event.keyCode == 13) return;
             var word = jel.val();
             if (word != "") {
                 if (jel.attr("keyword") != word) {
                     if (config.start && config.timeout) {
                         clearTimeout(config.timeout);
                     }
                     config.start = true;
                     config.timeout = setTimeout(function () {
                         config.start = false;
                         search(config, jel);
                     }, 400);
                 }
             }

         });
    if (config.btn) {
        config.btn.click(function () {
            search(config, jel);
            return false;
        });
    }

    function search(config, jel) {
        var word = jel.val();
        if (word != "") {
            jel.attr("keyword", word);
            config.selectIndex = -1;
            $.ajax({
                type: 'get',
                contentType: "application/json",
                url: config.postUrl.replace("{0}", encodeURI(word)),
                data: { dealer: jel.attr('dealer') },
                dataType: 'json',
                success: function (datas) {

                    if (datas != null && datas.constructor == Array && datas.length > 0) {
                        $(config.className).show();
                        config.datas = datas;
                        var tabHtml = "";
                        for (var i = 0; i < datas.length; i++) {
                            tabHtml += "<tr>";
                            for (var j = 0; j < config.columns.length; j++) {
                                var width = "";
                                if (config.columns[j].hasOwnProperty("width") == true) width = " width=\"" + config.columns[j].width + "\"";
                                tabHtml += "<td" + width + ">" + config.columns[j].render(datas[i]) + "</td>";
                            }
                            tabHtml += "</tr>";
                        }
                        config.table.html(tabHtml);

                        $(config.className).find(".autoTable").scroll(function () {
                            if (!config.delayhide) config.delayhide = "";
                            window.clearTimeout(config.delayhide);
                        });

                        var scrollTop = 0;
                        config.table.find("tr").each(function (i) {
                            $(this).attr("itemindex", i).attr("scrollTop", scrollTop)
                            .mouseenter(function () {
                                config.table.find("tr.o").removeClass("o");
                                $(this).addClass("o");
                                config.selectIndex = parseInt($(this).attr("itemindex"));
                            }).click(function () {

                                config.callback(config.datas[config.selectIndex]);
                                $(config.className).hide();
                            });
                            scrollTop += $(this).height();
                        });

                        //config.selectIndex = 0;

                    } else {
                        config.datas = [];
                        config.table.html("<tr><td>没有找到符合条件的数据</td></tr>");
                    }
                }
            });
        }
    }
};

UI.AutoComplete.setScroll = function (div, tr) {
    //简单滚动条实现
    div.scrollTop(tr.attr("scrollTop"));
};

UI.TableBind = function (useroptions) {
    return TableBind(useroptions);
};

//后台促销，tooltip提示
UI.Promotion_tip = function () {
    var tagObj = $('.tagfull');
    //鼠标移出移进效果
    tagObj.hover(
		function () {
		    var taginnerObj = $(this).children('.tagfull-inner');
		    var innertxt = taginnerObj.attr('datatitle');
		    $(this).children('.tooltip').children('.tooltip-inner').text(innertxt);
		    $(this).children('.tooltip').fadeIn('slow');
		},
		function () {
		    $(this).children('.tooltip').fadeOut('slow');
		}
	);
};

//后台促销，组的收起和展开
UI.Promotion_group = function () {
    //事件按钮
    var clickobj = $('.togglebtn');
    clickobj.toggle(
		function () {
		    $(this).children('span').eq(0).removeClass('icon_down_arrow').addClass('icon_up_arrow');
		    $(this).children('span').eq(1).text('收起');
		    $(this).parents("tbody").next('tbody[class="Promotion_color"]').show();
		}, function () {
		    $(this).children('span').eq(0).removeClass('icon_up_arrow').addClass('icon_down_arrow');
		    $(this).children('span').eq(1).text('展开');
		    $(this).parents("tbody").next('tbody[class="Promotion_color"]').hide();
		}
	);

};




/**********陈建**********/
//图片查看
/********************/
$.fn.Diapic = function(options) {
    var opts = {
        title: '',
        maxwidth: 800,
        maxheight: 500,

        Activate: function(obj) {
        },        //点击时事件
        CloseActivate: function(obj) {
        }   //关闭后事件
    }

    opts = $.extend({}, opts, options);
    //当前对象
    var ConObj = $(this);

    //HTML模板
    var template = '<div class="cjoutdiv"  style="height:{documentH}px;"></div>'
        + '<div class="cjpicbox">'
        + '	<div class="picb"><img src="{imgsrc}" title="{title}" width="{width}" /></div>'
        + '  <div class="pictitle">'
        + '  	<div class="picclose">close ×</div>'
        + '  	<div class="pictit">{title2}</div>'
        + '  </div>'
        + '</div>';

    //var browsH = document.documentElement.clientHeight;
    //var browsW = document.documentElement.clientWidth;
    ConObj.bind('click', function() {
        var browsW = $(window).width(); //获取浏览器显示区域的宽度
        var browsH = $(window).height(); //获取浏览器显示区域的高度
        var documentH = $(document).height(); //获取页面的文档高度
        var scrollH = $(document).scrollTop(); //获取滚动条到顶部的垂直高度
        var $that = $(this);
        if ($that.length > 0) {
            var pic = new Image(); //新建一个图片对象  
            pic.src = $that.attr('src'); //将图片的src属性赋值给新建的图片对象的src 
            var realTit = ($that.attr('title') == '' || $that.attr('title') == null) ? '' : $that.attr('title'); //真实的宽度

            var imgW = $that.width(); // 图片定义宽度    
            var imgH = $that.height(); // 图片定义高度   

            var realW, realH
            if (pic.width > opts.maxwidth) {
                realW = pic.width * 0.6;
                realH = pic.height * 0.6;
            } else {
                realW = pic.width; //真实的宽度
                realH = pic.height; //真实的高度
            }
            //var realH=(pic.height>opts.maxwidth)?pic.height:opts.maxwidth;//真实的高度
            //alert(pic.height);

            //处理模板
            var Htmltem = template;
            Htmltem = Htmltem.replace('{documentH}', documentH);
            Htmltem = Htmltem.replace('{title}', realTit);
            Htmltem = Htmltem.replace('{title2}', realTit);
            Htmltem = Htmltem.replace('{imgsrc}', pic.src);
            Htmltem = Htmltem.replace('{width}', realW);
            Htmltem = Htmltem.replace('{height}', realH);
            //开始生成
            $('body').append(Htmltem);
            $('.cjpicbox').css({ 'top': scrollH + parseInt((browsH - realH) / 2), 'left': parseInt((browsW - realW) / 2) });
            //$('.cjpicbox').animate({'top':parseInt((browsH - realH) / 2),'left': parseInt((browsW - realW) / 2),width:realW,height:realH+26},500);
            //$('.cjpicbox img').animate({width:realW,height:realH},500);


            //回调函数
            var result = opts.Activate($that);


        }
    });

    $('.cjpicbox div[class="picclose"]').live('click', function() {
        $(this).parents('.cjpicbox').remove();
        $('.cjoutdiv').remove();
        //回调函数
        var result = opts.CloseActivate($(this).parents('.cjpicbox'));
    });

};

UI.Highcharts = function (options) {
    var opts = {
        mapType: 'area',
        id: "",
        xCategories: [],
        tooltipName: "",
        seriesData: []
    };
    opts = $.extend({}, opts, options);
    if (opts.id == "") return false;
    return new Highcharts.Chart({
        chart: {
            type: opts.mapType,
            renderTo: opts.id,
            backgroundColor: '#ebebeb'
        },
        credits: {
            enabled: false
        },
        legend: {
            enabled: false
        },
        title: {
            text: null
        },
        subtitle: {
            text: null
        },
        xAxis: {
            lineWidth: 1,
            lineColor: '#c0c0c0',
            categories: opts.xCategories
        },
        yAxis: {
            title: {
                text: null
            },
            min: 0,
            lineColor: '#c0c0c0',
            labels: {

                formatter: function () {
                    return this.value;
                }
            }
        },
        tooltip: {

            shared: true,
            crosshairs: true,
            borderRadius: 4,
            backgroundColr: '#fcfcfc',
            borderColor: '#fcb322',
            formatter: function () {
                return '<span style="color:#fcb322;">' + opts.tooltipName + '：</span>' + '<span style="font-weight:bold;">' + this.y + '</span>';
            }
        },
        plotOptions: {
            area: {
                stacking: 'normal',
                lineColor: '#fbba37',
                lineWidth: 1,
                shadow: false,
                marker: {
                    radius: 3,
                    lineWidth: 1,
                    lineColor: '#fcb322'
                }
            }
        },
        series: [{
            name: 'Asia',
            color: '#f3ecdd',
            data: opts.seriesData
        }]
    });
};

UI.BsTextSelect = function(obj,options) {
    var opts = {
        url: '/Basic/GetCodeList',
        type: "3",
        title: "",
        id: "Remarks"
    };
    opts = $.extend({}, opts, options);
    var html =  '<div style="float: left">'
               + '<textarea style="height: 60px; width:400px" name="' + opts.id + '" id="' + opts.id + '"></textarea>'
               + '</div>'
               + '<div style="clear: both;"/>'
               + '</div>'
               + '<div style="height:20px;">'
               + '<span id="c_' + opts.id + '" class="prompt" name="tips">填写' + opts.title + '原因</span>'
               + '</div>'
               + '<div>'
               + '<select style="width:250px" name="selRemark"><option value="">选择' + opts.title + '原因</option></select>&nbsp;&nbsp;'
               + '最多输入<span style="color:red;font-weight:bold">50</span>字';
    var div = obj;
    div.empty();
    div.html(html);
    $(div).find("textarea").keyup(function (e) { //绑定输入文本域keyup事件，用于检测输入的文字是否超出
        var txt = $(this).val();
        var wordLimit = 50;
        if (txt.length <= wordLimit) {
            var num = wordLimit - txt.length;
            $(div).children("div").last().children("span").text(num);
        } else {
            $(this).val(txt.substr(0, wordLimit));
        }
        if (!txt) {
            $(div).find("span[name='tips']").removeClass().addClass("error").addClass("m10_l");
            $(this).addClass("inputerror");
        } else {
            $(div).find("span[name='tips']").removeClass().addClass("success").addClass("m10_l");
            $(this).removeClass();
        }
    }).end().find("select").change(function () { //绑定下拉选择原因事件
        var textarea = $(div).find("textarea");
        if ($(this).val() != "") {
            var statusTxt = $(this).find("option:selected").text();
            textarea.val(statusTxt);
        } else {
            textarea.val("");
        }
        textarea.triggerHandler("keyup");
    });
    $.post(opts.url, { type: opts.type }, function (data) {
        $.each(data, function (idx, item) {
            if (item.value) {
                $(div).find("select").append("<option value='" + item.value + "'>" + item.text + "</option>");
            }
        });
    });
};


//处理键盘事件 禁止后退键（Backspace）密码或单行、多行文本框除外
function banBackSpace(e){   
    var ev = e || window.event;//获取event对象   
    var obj = ev.target || ev.srcElement;//获取事件源   
    var t = obj.type || obj.getAttribute('type');//获取事件源类型  
    //获取作为判断条件的事件类型
    var vReadOnly = obj.getAttribute('readonly');
    var vEnabled = obj.getAttribute('enabled');
    //处理null值情况
    vReadOnly = (vReadOnly == null) ? false : vReadOnly;
    vEnabled = (vEnabled == null) ? true : vEnabled;
    //当敲Backspace键时，事件源类型为密码或单行、多行文本的，
    //并且readonly属性为true或enabled属性为false的，则退格键失效
    var flag1=(ev.keyCode == 8 && (t=="password" || t=="text" || t=="textarea")&&(vReadOnly==true || vEnabled!=true))?true:false;
    //当敲Backspace键时，事件源类型非密码或单行、多行文本的，则退格键失效
    var flag2=(ev.keyCode == 8 && t != "password" && t != "text" && t != "textarea")?true:false;        
    //判断
    if(flag2){return false;}
    if(flag1){return false;}   
}
//禁止后退键 作用于Firefox、Opera
document.onkeypress=banBackSpace;
//禁止后退键  作用于IE、Chrome
document.onkeydown=banBackSpace;
//处理键盘事件 禁止后退键结束


/*仓库选择组件，调用需要引用select2的两个js脚本 2016-5-27 杨浩 创建
    //<script type="text/javascript" src='@Url.Content("~/Theme/Plugins/select2/select2.js")'></script>
    //<script type="text/javascript" src='@Url.Content("~/Theme/Plugins/select2/select2_locale_zh-CN.js")'></script>
    obj:仓库选择的input的jQuery对象
    deliveryTypeSysNo:仓库配送方式编号,可不传
    warehouseType:仓库类型(10:仓库，20：门店)
    placeholder：文本框的提示信息
 */
UI.SelectWhareHouse = function (obj, deliveryTypeSysNo, warehouseType, placeholder, isAll) {
    obj.select2({
        language: 'zh-CN',
        placeholder: placeholder, //文本框的提示信息 "快速查询"
        minimumInputLength: 0, //至少输入n个字符，才去加载数据
        allowClear: true, //是否允许用户清除文本信息
        ajax: {
            url: '/Ajax/QuickSearchWarehouse', //地址
            dataType: 'text', //接收的数据类型
            //contentType:'application/json',
            data: function (term, pageNo) { //在查询时向服务器端传输的数据
                term = $.trim(term);
                return {
                    autNumber: term, //联动查询的字符
                    pageSize: 15, //一次性加载的数据条数
                    pageNo: pageNo, //页码
                    deliveryType: deliveryTypeSysNo,
                    warehouseType: warehouseType,
                    isAll: isAll,
                    time: new Date() //测试
                }
            },
            results: function (data, pageNo) {
                if (data.length > 0) { //如果没有查询到数据，将会返回空串
                    var dataObj = eval("(" + data + ")"); //将接收到的JSON格式的字符串转换成JSON数据
                    var more = (pageNo * 15) < dataObj.total; //用来判断是否还有更多数据可以加载
                    return {
                        results: dataObj.result,
                        more: more
                    };
                } else {
                    return { results: data };
                }
            }
        },
        initSelection: function (element, callback) { //初始化，其中doName是自定义的一个属性，用来存放text的值
            var id = $(element).val();
            var text = $(element).attr("doName");
            if (id != '' && text != "") {
                callback({ id: id, text: text });
            }
        },
        formatResult: formatAsText //渲染查询结果项
    });
    //格式化查询结果,将查询回来name放在div里显示
    function formatAsText(item) {
        var itemFmt = "<div style='color:#4F4F4F;display:inline'>" + item.name + "</div>";
        return itemFmt;
    }
};


