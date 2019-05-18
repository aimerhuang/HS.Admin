var DAO = {
    /*******************************
   * 弹出登录框
   * useroptions :  JSON参数对象
   * @cancel : 点击取消登录后的回调函数
   * 在弹出页面定义该函数组,以便登录框通知
   *******************************/
    LoginBox: function (useroptions) { },

    /*******************************
    * 文件上传
    *******************************/
    Upload: function () { },

    /*******************************
    * 商品选择
    * useroptions:          JSON参数对象
    * @selectedProducts     已经被选择的商品列表
                            可以是商品系统编号数组如：[1,2,3]
                            也可以是商品选择组件返回的JSON结果集如：{ name: '备电 4400mAh', pid: '1',erp:'70012541', price: [{ leave: '1', price: 100.23 }, { leave: '2', price: 200.32 }] ,baseprice:100,courier:100,category:{name:'保护',sysno:1}}
    * @onselect             回调函数
    * @categorySysNo        商品分类系统编号:用户过滤商品分类
    * @AssociationAttr      过滤管理属性JSON参数对象
    * @selectSingleNode     是否是单选
    * @otherFilter          动态过滤条件
    * @selectedIsReadOnly   已选择商品只读
    * @syncWebFront         选择商品与前端展示商品同步（只显示前端都能看到的商品）
    *   ==productSysNo：商品编号   如 AssociationAttr:{productSysNo:1}
    *******************************/
    SelectProduct: function (useroptions) { },

    /*******************************
   * 促销商品选择
   2013-09-23 余勇 创建
   * useroptions: JSON参数对象
   * @selectedProducts 已经被选择的商品列表
                       可以是商品系统编号数组如：[1,2,3]
                       也可以是商品选择组件返回的JSON结果集如：{ name: '备电 4400mAh', pid: '1',erp:'70012541', price: [{ leave: '1', price: 100.23 }, { leave: '2', price: 200.32 }] ,baseprice:100,courier:100,category:{name:'保护',sysno:1}}
   * @onselect   回调函数
   * @categorySysNo 商品分类系统编号:用户过滤商品分类
   * @AssociationAttr 过滤管理属性JSON参数对象
   * @selectSingleNode 是否是单选
   * @otherFilter 动态过滤条件
   *   ==productSysNo：商品编号   如 AssociationAttr:{productSysNo:1}
   *******************************/
    SelectPromotionProduct: function (useroptions) { },

    /*****************************
    * 仓库选择
    * target : 分类选择按钮
    * useroptions :  JSON参数对象
    *@chkStyle   单选或多选 default:checkbox,radio
    *@callback  回调接收Json([{"id":"1","name":"节点1"},{"id":"2","name":"节点2"}])
    ******************************/
    SelectWhareHouse: function (useroptions) { },

    /*****************************
    * 仓库选择（弹窗方式）
    * useroptions :  JSON参数对象
    *@chkStyle   单选或多选 default:checkbox,radio
    *@callback  回调接收Json([{"id":"1","name":"节点1"},{"id":"2","name":"节点2"}])
    ******************************/
    SelectWhareHouseDialog: function (useroptions) { },

    /*
    分销商选择
    2013-09-09 郑荣华 创建
    *pdSysNo    商品系统编号
    *status     分销商状态
    *@width     弹出层宽度
    *@height    弹出层高度
    *callBack   回调函数 返回选择的分销商系统编号数组
    */
    SelectDsDealer: function (useroptions) { },

    /*
    取件单选择
    2013-07-06 郑荣华 创建
    *whSysNo    仓库系统编号
    *sysNoFilter不要显示的取件单编号数组
    *status     取件单状态
    *@width     弹出层宽度
    *@height    弹出层高度
    *callBack   回调函数 返回选择的取件单编号数组
    */
    SelectPickUp: function (useroptions) { },

    /*
   配送方式选择
   2013-07-06 郑荣华 创建
   *WareHouseSysNo    仓库系统编号
   *sysNoFilter不要显示的配送方式编号数组
   *parentSysNo父级系统编号，默认为3（第三方快递）
   *status     配送方式状态
   *@width     弹出层宽度
   *@height    弹出层高度
   *callBack   回调函数 返回选择的取件单编号数组
   */
    SelectDeliveryType: function (useroptions) { },
    /*****************************
    * 选择商品分类
    *控件为弹出层方式呈现

    *2013-06-25 邵斌 创建
    * selectButton : 分类选择按钮
    * valueContainer:接收值容器控件
    * useroptions :  JSON参数对象
    *@isMulti           是否启用多选
    *@width             弹出层宽度
    *@height            弹出层高度
    *@onlyLeaftSelect   是否只能选择子节点
    *@position          相对选择按钮显示位置
    *@zIndex            显示位置
    *@margin            弹出层外框边距   vertical:垂直位置 horizontal:水平位置
    *@callBack          选择后回调函数
    取值：选择的商品分类的ID值放在valueContainer:接收值容器控件的"PdCategory"自定义属性中
    ******************************/
    SelectProductCategory: function (selectButton, valueContainer, useroptions) { },

    /*****************************
    * 批量选择商品分类
    *控件为弹出层方式呈现

    *2016-05-05 陈海裕 创建
    * selectButton : 分类选择按钮
    * useroptions :  JSON参数对象
    * beforeBinding : 数据绑定前执行方法
    * afterBinding : 数据绑定后执行方法
    *@isMulti           是否启用多选
    *@width             弹出层宽度
    *@height            弹出层高度
    *@onlyLeaftSelect   是否只能选择子节点
    *@position          相对选择按钮显示位置
    *@zIndex            显示位置
    *@margin            弹出层外框边距   vertical:垂直位置 horizontal:水平位置
    *@callBack          选择后回调函数
    ******************************/
    BatchSelectProductCategory: function (selectButton, useroptions, beforeBinding, afterBinding) { },

    /*****************************
    * 选择地区
    *控件为弹出层方式呈现
 
    *2013-08-06 郑荣华 创建
    * selectButton : 分类选择按钮
    * valueContainer:接收值容器控件
    * useroptions :  JSON参数对象
    *@isMulti           是否启用多选
    *@isAll             是否包括省市区，为false只包括省市
    *@width             弹出层宽度
    *@height            弹出层高度
    *@onlyLeaftSelect   是否只能选择子节点
    *@position          相对选择按钮显示位置
    *@zIndex            显示位置
    *@margin            弹出层外框边距   vertical:垂直位置 horizontal:水平位置
    *@callBack          选择后回调函数
    取值：选择的地区的ID值放在valueContainer:接收值容器控件的"BsArea"自定义属性中
    ******************************/
    SelectArea: function (selectButton, valueContainer, useroptions) { },

    /*******************************
    * 商品基础属性选择
    *******************************/
    SelectAttribute: function () { },

    /*******************************
    * 商品基础属性选择
    *******************************/
    SelectAttributeTest: function () { },

    /*******************************
   * 软件分类选择组件

   *2014-01-17 唐永勤 创建
   *******************************/
    SelectFeSoftCategory: function (useroptions) { },

    /*******************************
    * 商品基础属性分组选择
    * useroptions   ：JSON用户参数
    * @callBack 回调函数
    *******************************/
    SelectAttributeGroup: function () { },

    /*******************************
    * 表格加入隔行样式
    * useroptions : 参数对象
    *@tableid   隔行样式的表格ID
    *@cssclass 样式名称
    *@row   样式开始的行
    *******************************/
    InterlineStyle: function () { },

    /*******************************
    * 商品价格调价申请
    * useroptions       : 参数对象
    * @productSysNoList : 商品编号列表数组
    * @priceType        :价格类型枚举，对应Hyt.Model.WorkflowStatus.ProductStatus.产品价格来源枚举值。该值可以是组合值如：0,10 （基础价格+会员等级价）
    * @width            : 对话框宽度
    * @height           :对话框高度
    * @success   回调函数,点击选择后（单选为点击分类名称）调用
    * @autoClose  成功后是否自动关闭
    *******************************/
    PriceAdjustmentRequest: function (userOptions) { },

    /*******************************
    * 分销商商品价格调价申请
    * useroptions       : 参数对象
    * @productSysNoList : 商品编号列表数组
    * @priceType        :价格类型枚举，对应Hyt.Model.WorkflowStatus.ProductStatus.产品价格来源枚举值。该值可以是组合值如：0,10 （基础价格+会员等级价）
    * @width            : 对话框宽度
    * @height           :对话框高度
    * @success   回调函数,点击选择后（单选为点击分类名称）调用
    * @autoClose  成功后是否自动关闭
    *******************************/
    PriceDistributionRequest: function (userOptions) { },
    /*******************************
    * 订单选择
    *******************************/
    SelectOrder: function (userOptions) { },

    /*******************************
    * 促销规则选择
    *******************************/
    SelectPromotionRule: function (callback) { },

    /*******************************
    * 促销选择
    *******************************/
    SelectPromotion: function (useroptions) { },

    /*******************************
* 商品描述模板选择
*******************************/
    SelectPdTemplate: function (callback) { },

    /*******************************
    * 商品描述模块选择
    *******************************/
    SelectPdModule: function (callback) { },

    /*******************************
    * 仓库信息多选
    selectButton:激发按钮
    selectContainer:<select ></select>
    callbackfun:回调函数
    *******************************/
    MultipleSelectWarehouse: function (selectButton, selectContainer, callbackfun) { },

    /*******************************
    * 系统用户选择
    *******************************/
    SelectSyUser: function (callback) { },

    /*******************************
   * 常用文本选择
   *******************************/
    SelectBsText: function (callback) { }
};
DAO.SelectPdTemplate = function (callback) {
    window._ActiveSelectPdTemplateCallBack = function (data) {
        if ($.isFunction(callback)) {
            callback(data);
        }
    },
    UI.DialogOpen('/Product/SelectPdTemplate/1?type=10', {
        width: 850,
        height: 450,
        title: '选择商品描述模板',
        init: function () {

        }
    }, false);
};

DAO.SelectPdModule = function (callback) {
    window._ActiveSelectPdTemplateCallBack = function (data) {
        if ($.isFunction(callback)) {
            callback(data);
        }
    },
    UI.DialogOpen('/Product/SelectPdTemplate/1?type=20', {
        width: 850,
        height: 450,
        title: '选择商品描述模块',
        init: function () {

        }
    }, false);
};

//#region 交替样式
DAO.InterlineStyle = function (useroptions) {
    var options = $.extend({
        tableid: 'item_list',
        cssclass: 'stag_color',
        row: 1
    }, useroptions);
    $('#' + options.tableid + ' tbody tr:even').addClass(options.cssclass);
    //var rows = $('#' + options.tableid + ' tbody tr');
    //if (rows) {
    //    rows.each(function () {
    //        if (options.row % 2 != 0) {
    //            $(this).addClass(options.cssclass);
    //        }
    //        options.row++;
    //    });
    //}
};
//#endregion
DAO.SelectAttributeTest = function (useroptions) {
    UI.DialogOpen('/Product/SelectAttribute', {
        width: '800px',
        height: '450px',
        title: '选择商品基础属性',
        init: function () {
            this.button(
                {
                    name: '确认',
                    callback: function () {
                        var $iframe = this.iframe.contentWindow;
                        $iframe.CallBack();
                    },
                    focus: true
                }, {
                    name: '取消',
                    callback: function () {

                    }
                }
            );
        }
    }, false);
};




DAO.SelectWhareHouse = function (target, useroptions) {
    var options = $.extend({
        chkStyle: 'checkbox',//单选或多选 default:checkbox,radio
        value: '',//页面需要绑定的值
        id: 'SelectWhareHouse' + Math.round(Math.random() * 100),
        expandAll: true,//是否展开全部节点
        areaNoCheck: true,//地区节点是否可被选中true为不能选中，false反之
        callback: function () { },//回调接收Json({"id":"1","name":"节点"})
        restcallback: function () { }//重置回调
    }, useroptions);

    target = $(target);

    window._ActiveSelectWhareHouseCallBack = function (data) {
        if ($.isFunction(useroptions.callback)) {
            useroptions.callback.call(window, data);
        }
    };
    window._ActiveSelectWhareHouseRestCallBack = function () {
        if ($.isFunction(useroptions.restcallback)) {
            useroptions.restcallback.call(window);
        }
    };
    if ($('#' + options.id).length > 0) return;
    var url = "/Warehouse/WhareHouseSelector?chkStyle=" + options.chkStyle + "&expandAll=" + options.expandAll + '&areaNoCheck=' + options.areaNoCheck;
    var $div = $('<div id="' + options.id + '" ><iframe src="' + url + '" height="100%" width="100%" frameborder="0"></iframe></div>');
    var width = 282;
    var height = 360;

    var left = target.offset().left;
    if ((left + width) > $(window).width()) {
        left = $(window).width() - width;
    }

    $div.css({
        width: width,
        height: height,
        position: 'absolute',
        zIndex: '199',
        left: left,
        top: target.offset().top + target.height() + 6
    });

    $(document).mousedown(function () {
        if ($div) {
            $div.remove();
        }
    });

    $('body').after($div);

    return {//隐藏弹出层
        Remove: function () {
            $div.remove();
        }
    };
};

DAO.SelectOutStocks = function (useroptions) {
    var options = $.extend({
        WarehouseSysNo: null,
        DeliveryTypeSysNo: null,
        SysNoFilter: null,
        width: 1024,
        height: 540,
        callback: function () { }
    }, useroptions);
    //拼接URL参数
    var parameters = "a=1";//方便拼接字符

    if (options.WarehouseSysNo != null)
        parameters += "&WarehouseSysNo=" + options.WarehouseSysNo;
    if (options.DeliveryTypeSysNo != null)
        parameters += "&DeliveryTypeSysNo=" + options.DeliveryTypeSysNo;
    if (options.SysNoFilter) {
        parameters += "&SysNoFilter=" + options.SysNoFilter;  //要过滤的取件单系统编号
    }
    //if (options.SysNoFilter.length > 0) {
    //    parameters += "&SysNoFilter=" + options.SysNoFilter.AsDelimited("&SysNoFilter=");  //要过滤的取件单系统编号
    //}

    //伪回调
    window.top._ActiveSelectOutStocksCallBack = function (data) {
        if ($.isFunction(useroptions.callBack)) {
            useroptions.callBack.call(window, data);
        }
    };
    UI.DialogOpen('/Logistics/SelectOutStockList/1?' + parameters, {
        width: options.width, height: options.height, title: '选择出库单', init: function () {
            this.button(
                {
                    name: '确认',
                    callback: function () {
                        var $iframe = this.iframe.contentWindow;
                        $iframe.CallBack();
                    },
                    focus: true
                }, {
                    name: '取消',
                    callback: function () {

                    }
                }
            );
        }
    }, false);
};

// 在地图上选择出库单 2014-03-04 唐文均 创建
DAO.SelectOutStocksFromMap = function (useroptions) {
    var options = $.extend({
        WarehouseSysNo: null,
        DeliveryTypeSysNo: null,
        SysNoFilter: null,
        width: 1024,
        height: 540,
        callback: function () { }
    }, useroptions);
    //拼接URL参数
    var parameters = "a=1";//方便拼接字符

    if (options.DeliveryUserSysNo)
        parameters += "&DeliveryUserSysNo=" + options.DeliveryUserSysNo;
    if (options.WarehouseSysNo != null)
        parameters += "&WarehouseSysNo=" + options.WarehouseSysNo;
    if (options.DeliveryTypeSysNo != null)
        parameters += "&DeliveryTypeSysNo=" + options.DeliveryTypeSysNo;
    if (options.SysNoFilter) {
        parameters += "&SysNoFilter=" + options.SysNoFilter;  //要过滤的取件单系统编号
    }
    //if (options.SysNoFilter.length > 0) {
    //    parameters += "&SysNoFilter=" + options.SysNoFilter.AsDelimited("&SysNoFilter=");  //要过滤的取件单系统编号
    //}

    //伪回调
    window.top._ActiveSelectOutStocksCallBack = function (data) {
        if ($.isFunction(useroptions.callBack)) {
            useroptions.callBack.call(window, data);
        }
    };
    UI.DialogOpen('/Logistics/SelectOutStockListFromMap?' + parameters, {
        width: options.width, height: options.height, title: '选择出库单', init: function () {
            this.button(
                {
                    name: '确认',
                    callback: function () {
                        var $iframe = this.iframe.contentWindow;
                        $iframe.CallBack();
                    },
                    focus: true
                }, {
                    name: '取消',
                    callback: function () {

                    }
                }
            );
        }
    }, false);
};

DAO.SelectProduct = function (useroptions) {

    //合并参数
    var options = $.extend({
        selectedProducts: [],
        selectedIsReadOnly: false,
        onselect: function (data) {
        },
        associationAttr: null,
        categorySysNo: null,
        selectSingleNode: false,
        width: '920px',
        height: '560px',
        title: '选择商品',
        otherFilter: function () {
            return null;
        },
        syncWebFront: false
    }, useroptions);


    //伪回调
    window._ActiveSelectProductCallBack = function (data) {
        useroptions.onselect.call(window, data);
    };

    //已选择商品容器
    window.top._ActiveSelectedProductList = new Array();
    var reg = new RegExp('^[0-9]+$');

    //更具数组内容来拼接成结果数组
    for (var i = 0; i < options.selectedProducts.length; i++) {

        //如果是object就视为JSON数据
        if (typeof (options.selectedProducts[i]) == "object") {

            //如果JSON数据定义了pid属性就视为有效
            if (!!options.selectedProducts[i].pid)
                window.top._ActiveSelectedProductList.push(options.selectedProducts[i].pid);    //加入结果集
        } else if (typeof (options.selectedProducts[i]) == "number") {
            //如果是数字将直接视为有效系统编号
            window.top._ActiveSelectedProductList.push(options.selectedProducts[i]);
        } else if (typeof (options.selectedProducts[i]) == "string" && reg.test(options.selectedProducts[i])) {
            window.top._ActiveSelectedProductList.push(options.selectedProducts[i]);
        }
    }

    url = '/Product/SelectProduct?r=' + Math.round(1000) + "&isSelector=true";

    if (options.associationAttr != null) {
        url = url + "&subfilter=AssociationAttribute&productsysno=" + options.associationAttr.productSysNo;

        if (options.associationAttr.warehouseSysNo != null) {
            url = url + "&warehouseSysNo=" + options.associationAttr.warehouseSysNo;
        }

        if (options.associationAttr.dealerSysNo != null) {
            url = url + "&dealerSysNo=" + options.associationAttr.dealerSysNo;
        }
    }

    if (options.categorySysNo != null) {
        url = url + "&category=" + options.categorySysNo;
    }

    if (options.selectSingleNode == true) {
        url = url + "&single=true";
    } else {
        url = url + "&single=false";
    }

    if (options.selectedIsReadOnly == true) {
        url = url + "&selectedIsReadOnly=true";
    }

    if (options.syncWebFront == true) {
        url = url + "&syncWebFront=true";
    }

    if (typeof (options.otherFilter) == "function") {
        var parameters = options.otherFilter();
        parameters = parameters == null ? "" : parameters;
        if (parameters.indexOf("&") == 0) {
            url = url + parameters;
        } else {
            url = url + "&" + parameters;
        }
    }

    options.init = function () {
        this.button(
            {
                name: '确认',
                callback: function () {
                    var $iframe = this.iframe.contentWindow;
                    return $iframe.CallBack();
                },
                focus: true
            }, {
                name: '取消',
                callback: function () {

                }
            }
        );
    };

    UI.DialogOpen(url, options, false);
};

DAO.SelectPromotionProduct = function (useroptions) {

    //合并参数
    var options = $.extend({
        selectedProducts: [],
        onselect: function (data) {
        },
        associationAttr: null,
        categorySysNo: null,
        selectSingleNode: false,
        width: '920px',
        height: '496px',
        title: '选择商品',
        data: '/Product/SearchProductPromotion',
        isshop: false,//是否门店选择商品
        otherFilter: function () {
            return null;
        }
    }, useroptions);

    //伪回调
    window._ActiveSelectProductCallBack = function (data) {
        useroptions.onselect.call(window, data);
    };

    //已选择商品容器
    window.top._ActiveSelectedProductList = new Array();

    //更具数组内容来拼接成结果数组
    for (var i = 0; i < options.selectedProducts.length; i++) {

        //如果是object就视为JSON数据
        if (typeof (options.selectedProducts[i]) == "object") {

            //如果JSON数据定义了pid属性就视为有效
            if (!!options.selectedProducts[i].pid)
                window.top._ActiveSelectedProductList.push(options.selectedProducts[i].pid);    //加入结果集
        } else if (typeof (options.selectedProducts[i]) == "number") {
            //如果是数字将直接视为有效系统编号
            window.top._ActiveSelectedProductList.push(options.selectedProducts[i]);
        }
    }

    url = '/Product/SelectPromotionProduct?r=' + Math.round(1000);
    if (options.isshop == true) //是否门店选择商品
    {
        url = url + "&isshop=true";
    }
    if (options.associationAttr != null) {
        if (options.associationAttr.productSysNo != null) {
            url = url + "&subfilter=AssociationAttribute&productsysno=" + options.associationAttr.productSysNo;
        }
        if (options.associationAttr.customerSysNo != null) {
            url = url + "&customerSysNo=" + options.associationAttr.customerSysNo;
        }

        if (options.associationAttr.warehouseSysNo != null) {
            url = url + "&warehouseSysNo=" + options.associationAttr.warehouseSysNo;
        }

        if (options.associationAttr.dealerSysNo != null) {
            url = url + "&dealerSysNo=" + options.associationAttr.dealerSysNo;
        }
    }
    if (options.categorySysNo != null) {
        url = url + "&category=" + options.categorySysNo;
    }

    if (options.selectSingleNode == true) {
        url = url + "&single=true";
    }

    if (options.data != null) {
        url = url + "&data=" + options.data;
    }

    if (typeof (options.otherFilter) == "function") {
        var parameters = options.otherFilter();
        parameters = parameters == null ? "" : parameters;
        if (parameters.indexOf("&") == 0) {
            url = url + parameters;
        } else {
            url = url + "&" + parameters;
        }
    }


    options.init = function () {
        this.button(
            {
                name: '确认',
                callback: function () {
                    var $iframe = this.iframe.contentWindow;
                    return $iframe.CallBack();
                },
                focus: true
            }, {
                name: '取消',
                callback: function () {

                }
            }
        );
    };
    UI.DialogOpen(url, options, false);
};


DAO.SelectWhareHouseDialog = function (useroptions) {
    //合并参数
    var options = $.extend({
        chkStyle: 'radio',//单选或多选 default:radio,checkbox
        areaSysNo: null,//为查询地区编号下所有仓库，未实现
        whName: null,//仓库名称模糊查询
        width: 300,
        height: 360,
        isAllWh: false
    }, useroptions);

    //拼接URL参数
    var parameters = "chkStyle=" + options.chkStyle;

    if (options.areaSysNo != null)
        parameters += "&AreaSysNo=" + options.areaSysNo;
    if (options.whName != null)
        parameters += "&whName=" + options.whName;
    if (options.isAllWh != null)
        parameters += "&isAllWh=" + options.isAllWh;
    if (options.isRma != null)
        parameters += "&isRma=" + options.isRma;
    //伪回调
    window.top._ActiveSelectWhareHouseDialogCallBack = function (data) {
        if ($.isFunction(useroptions.callBack)) {
            useroptions.callBack.call(window, data);
        }
    };
    UI.DialogOpen('/Warehouse/SelectWharehouse?' + parameters, {
        width: options.width,
        height: options.height,
        title: '选择仓库',
        init: function () {
            this.button(
                {
                    name: '确认',
                    callback: function () {
                        var $iframe = this.iframe.contentWindow;
                        $iframe.getValue();
                    },
                    focus: true
                }, {
                    name: '取消',
                    callback: function () {

                    }
                }
            );
        }
    }, false);
};


DAO.SelectDsDealer = function (useroptions) {
    //合并参数
    var options = $.extend({
        pdSysNo: null,
        status: null,
        width: 800,
        height: 512
    }, useroptions);

    //拼接URL参数
    var parameters = "x=1";//方便拼接字符

    if (options.status != null)
        parameters += "&Status=" + options.status;
    if (options.pdSysNo != null)
        parameters += "&ProductSysNo=" + options.pdSysNo;

    //伪回调
    window.top._ActiveSelectDsDealerCallBack = function (data) {
        if ($.isFunction(useroptions.callBack)) {
            useroptions.callBack.call(window, data);
        }
    };
    UI.DialogOpen('/Distribution/SelectDsDealer/1?' + parameters, {
        width: options.width,
        height: options.height,
        title: '选择分销商',
        init: function () {
            this.button(
                {
                    name: '确认',
                    callback: function () {
                        var $iframe = this.iframe.contentWindow;
                        $iframe.CallBack();
                    },
                    focus: true
                }, {
                    name: '取消',
                    callback: function () {

                    }
                }
            );
        }
    }, false);
};

DAO.SelectPickUp = function (useroptions) {
    //合并参数
    var options = $.extend({
        sysNoFilter: [],
        whSysNo: null,
        status: null,
        width: 800,
        height: 512
    }, useroptions);

    //拼接URL参数
    var parameters = "x=1";//方便拼接字符

    if (options.status != null)
        parameters += "&Status=" + options.status;
    if (options.whSysNo != null)
        parameters += "&WarehouseSysNo=" + options.whSysNo;

    if (options.sysNoFilter.length > 0) {
        parameters += "&SysNoFilter=" + options.sysNoFilter.AsDelimited(",");  //要过滤的取件单系统编号
    }

    //伪回调
    window.top._ActiveSelectPickUpCallBack = function (data) {
        if ($.isFunction(useroptions.callBack)) {
            useroptions.callBack.call(window, data);
        }
    };
    UI.DialogOpen('/Logistics/SelectPickUp/1?' + parameters, {
        width: options.width,
        height: options.height,
        title: '选择取件单',
        init: function () {
            this.button(
                {
                    name: '确认',
                    callback: function () {
                        var $iframe = this.iframe.contentWindow;
                        $iframe.CallBack();
                    },
                    focus: true
                }, {
                    name: '取消',
                    callback: function () {

                    }
                }
            );
        }
    }, false);
};

DAO.SelectDeliveryType = function (useroptions) {
    //合并参数
    var options = $.extend({
        sysNoFilter: [],
        status: 1,
        parentSysNo: 3,//默认属于第三方快递
        WareHouseSysNo: null,
        width: 600,
        height: 470
    }, useroptions);

    //拼接URL参数
    var parameters = "";//方便拼接字符   
    parameters += "&Status=" + options.status;
    parameters += "&ParentSysNo=" + options.parentSysNo;
    if (options.WareHouseSysNo != null)
        parameters += "&WareHouseSysNo=" + options.WareHouseSysNo;
    if (options.sysNoFilter.length > 0) {
        parameters += "&SysNoFilter=" + options.sysNoFilter.AsDelimited(",");  //要过滤的取件单系统编号
    }

    //伪回调
    window.top._ActiveSelectDeliveryTypeCallBack = function (data) {
        if ($.isFunction(useroptions.callBack)) {
            useroptions.callBack.call(window, data);
        }
    };
    UI.DialogOpen('/Logistics/SelectDeliveryType/1?' + parameters, {
        width: options.width,
        height: options.height,
        title: '选择配送方式',
        init: function () {
            this.button(
                {
                    name: '保存',
                    callback: function () {
                        var $iframe = this.iframe.contentWindow;
                        $iframe.CallBack();
                    },
                    focus: true
                }, {
                    name: '取消',
                    callback: function () {

                    }
                }
            );
        }
    }, false);
};

//创建客户，成功返回新的客户和收货地址实体，错误时error有值
//参考Views/OrderCreate.cshtml 765行DAO.CreateCustomer方法
DAO.CreateCustomer = function (useroptions) {
    //伪回调
    window._ActiveCreateCustomerCallBack = function (data) {
        useroptions.onselect.call(window, data);
    };
    UI.DialogOpen('/CRM/CreateCustomer', {
        width: '615px',
        height: '330px',
        title: '创建会员',
        init: function () {
            this.button(
                {
                    name: '保存',
                    callback: function () {

                        var $iframe = this.iframe.contentWindow;
                        //alert(useroptions.from);
                        $iframe.CallBack(useroptions.from, useroptions.fromNo);
                        return false;
                    },
                    focus: true
                }, {
                    name: '取消',
                    callback: function () {

                    }
                }
            );
        }
    }, false);
};
DAO.Upload = function (useroptions) {
    var options = $.extend({
        config: 'default',
        callback: function () { },
        cancel: function () { }
    }, useroptions);

    //伪回调
    window._ActiveFileUploadCallBack = function (data) {
        $.isFunction(useroptions.callback) && useroptions.callback.call(window, data);
    };
    UI.DialogOpen('/Shared/Upload?config=' + options.config, {
        width: '650px',
        height: '436px',
        title: '选择文件',
        init: function () {
            this.button(
                {
                    name: '确认',
                    callback: function () {
                        var $iframe = this.iframe.contentWindow;
                        $iframe.CallBack();
                        return false;
                    },
                    focus: true
                }, {
                    name: '取消',
                    callback: function () {
                        options.cancel();
                    }
                }
            );
        }
    }, false);
};
DAO.LoginBox = function (url, useroptions) {
    var options = $.extend({
        required: null,
        cancel: function () { }
    }, useroptions);

    art.dialog.open('/Account/MiniLogin', {
        id: "minilogin",
        title: '登 录',
        cancel: false,
        width: '420px',
        height: '150px',
        init: function () {
        }
    }, false);
};

DAO.SelectProductCategory = function (selectButton, valueContainer, useroptions) {
    var options = $.extend({
        isMulti: false,
        width: 290,
        height: 360,
        position: "bottom left",
        zIndex: 999,
        url: "/product/ProductCategorySelector",
        onlyLeaftSelect: true,
        margin: {
            vertical: 0,         //垂直位置
            horizontal: 0        //水平位置
        },
        callBack: function (nodes) { }
    }, useroptions);

    var newId = $(selectButton).attr("id");
    newId = newId + "_PdCategory_Single_Select_Panel";
    var randNum = Math.round(Math.random(1000) * 1000 / 1000);
    var htmlTemplate = '<div id="' + newId + '" style="position:absolute; z-index:' + options.zIndex + '; display:none;">'
                       + '<iframe id="frame_' + newId + '" name="frame_' + newId + '" frameborder="0" border="0" scrolling="no" src="' + options.url + '?IsMultipleSelect=' + options.isMulti + '&Width=' + options.width + '&Height=' + options.height + '&OnlyLeaftSelect=' + options.onlyLeaftSelect + '&r=' + randNum + '" style="width: ' + options.width + 'px; height: ' + options.height + 'px;">'
                        + '</iframe>'
                        + '</div>';
    $(selectButton).after(htmlTemplate);
    //$("body").append(htmlTemplate);

    //计算弹出层位置
    var setPosition = function (element) {
        //根据点击的对象设置弹出框位置
        var left = $(element).position().left;      //取得按钮位置
        var top = $(element).position().top;        //取得按钮位置

        //设置顶对齐
        if (options.position.toLocaleLowerCase().indexOf("top") >= 0) {
            top = top - options.height() - options.margin.vertical;
        }

        //设置低对齐
        if (options.position.toLocaleLowerCase().indexOf("bottom") >= 0) {
            top = top + $(element).height() + options.margin.vertical;
        }

        //设置左对齐
        if (options.position.toLocaleLowerCase().indexOf("left") >= 0) {
            left = left - options.width - options.margin.horizontal + $(element).width();
        }

        //设置右对齐
        if (options.position.toLocaleLowerCase().indexOf("right") >= 0) {
            left = left + options.width + options.margin.horizontal;
        }

        return { top: top, left: left };
    };
    var panel = $("#" + newId);
    //设置弹出层宽高和位置
    panel.width(options.width);
    panel.height(options.height);

    //为按钮添加点击事件
    $(selectButton).bind("click", function () {
        //读取新加入的层
        var panel = $("#" + newId);

        //按钮作为开关设置弹出层显示不显示
        if (panel.is(":visible")) {
            panel.hide();
            return false;
        } else {
            var position = setPosition(selectButton);
            panel.css("top", position.top);
            panel.css("left", position.left);
            panel.show();
        }

        //设置已选择的分类
        var valueList = $(valueContainer).attr("pdcategory") == null ? [] : $(valueContainer).attr("pdcategory").split(",");

        //判读用哪种方式绑定初始值
        //如果页面还没有加载则，使用延迟方法绑定
        if (!!$("iframe")[0].contentWindow.SetCheckedNode) {
            $("iframe")[0].contentWindow.SetCheckedNode(valueList);
        } else {
            $("iframe").load(function () {
                $("iframe")[0].contentWindow.SetCheckedNode(valueList);
            });
        }

        $(document).one("click", function () {
            panel.hide();
        });
        return false;
    });


    //选择后回调
    window._ActiveSelectProductCategoryCallBack = function (data) {
        if (data != null) {


            UI.BindDataFromZTreeNodes(valueContainer, data);
            //初始化结果到自定义属性。自定义属性用来暂存现在值
            valueContainer.attr("pdcategory", "");
            var sysNoStr = "";
            sysNoStr = !!valueContainer.attr("pdcategory") ? valueContainer.attr("pdcategory") : "";
            for (var i = 0; i < data.length; i++) {
                if (sysNoStr.length > 0)
                    sysNoStr += ",";
                sysNoStr += data[i].id;
            }
            valueContainer.attr("pdcategory", sysNoStr);
            if ($.isFunction(useroptions.callBack))
                useroptions.callBack.call(window, data);
        }

        var panel = $("#" + newId);
        panel.hide();
    };
};

DAO.BatchSelectProductCategory = function (selectButton, useroptions, beforeBinding, afterBinding) {
    var options = $.extend({
        isMulti: false,
        width: 290,
        height: 360,
        position: "bottom left",
        zIndex: 999,
        url: "/product/BatchCategorySelector",
        onlyLeaftSelect: true,
        margin: {
            vertical: 0,         //垂直位置
            horizontal: 0        //水平位置
        },
        callBack: function (nodes) { }
    }, useroptions);

    var newId = $(selectButton).attr("id");
    newId = newId + "_PdCategory_Single_Select_Panel";
    var randNum = Math.round(Math.random(1000) * 1000 / 1000);
    var htmlTemplate = '<div id="' + newId + '" style="position:absolute; z-index:' + options.zIndex + '; display:none;">'
                       + '<iframe id="frame_' + newId + '" name="frame_' + newId + '" frameborder="0" border="0" scrolling="no" src="' + options.url + '?IsMultipleSelect=' + options.isMulti + '&Width=' + options.width + '&Height=' + options.height + '&OnlyLeaftSelect=' + options.onlyLeaftSelect + '&r=' + randNum + '" style="width: ' + options.width + 'px; height: ' + options.height + 'px;">'
                        + '</iframe>'
                        + '</div>';
    $(selectButton).after(htmlTemplate);

    //计算弹出层位置
    var setPosition = function (element) {
        //根据点击的对象设置弹出框位置
        var left = $(element).position().left;      //取得按钮位置
        var top = $(element).position().top;        //取得按钮位置

        //设置顶对齐
        if (options.position.toLocaleLowerCase().indexOf("top") >= 0) {
            top = top - options.height() - options.margin.vertical;
        }

        //设置低对齐
        if (options.position.toLocaleLowerCase().indexOf("bottom") >= 0) {
            top = top + $(element).height() + options.margin.vertical;
        }

        //设置左对齐
        if (options.position.toLocaleLowerCase().indexOf("left") >= 0) {
            left = left - options.width - options.margin.horizontal + $(element).width();
        }

        //设置右对齐
        if (options.position.toLocaleLowerCase().indexOf("right") >= 0) {
            left = left + options.width + options.margin.horizontal;
        }

        return { top: top, left: left };
    };
    var panel = $("#" + newId);
    //设置弹出层宽高和位置
    panel.width(options.width);
    panel.height(options.height);

    //为按钮添加点击事件
    $(selectButton).bind("click", function () {
        //设置已选择的分类
        var valueList = new Array();

        // 绑定数据前执行方法
        if (beforeBinding && typeof (beforeBinding) == 'function') {
            beforeBinding(function (data) {
                if (!data || data.length == 0) {
                    return;
                }
                valueList = data;
                //读取新加入的层
                var panel = $("#" + newId);

                //按钮作为开关设置弹出层显示不显示
                if (panel.is(":visible")) {
                    panel.hide();
                    return false;
                } else {
                    var position = setPosition(selectButton);
                    panel.css("top", position.top);
                    panel.css("left", position.left);
                    panel.show();
                }

                //判读用哪种方式绑定初始值
                //如果页面还没有加载则，使用延迟方法绑定
                if (!!$("iframe#frame_" + newId)[0].contentWindow.SetCheckedNode) {
                    $("iframe#frame_" + newId)[0].contentWindow.SetCheckedNode(valueList);
                    $("iframe#frame_" + newId)[0].contentWindow.productsCategories = valueList;
                } else {
                    $("iframe#frame_" + newId).load(function () {
                        $("iframe#frame_" + newId)[0].contentWindow.SetCheckedNode(valueList);
                    });
                }

                // 绑定数据后执行方法
                if (afterBinding && typeof (afterBinding) == 'function') {
                    afterBinding();
                }

                $(document).one("click", function () {
                    $("iframe#frame_" + newId)[0].contentWindow.ClearCheckedNode();
                    panel.hide();
                });
                return false;
            });
        }

        ////读取新加入的层
        //var panel = $("#" + newId);

        ////按钮作为开关设置弹出层显示不显示
        //if (panel.is(":visible")) {
        //    panel.hide();
        //    return false;
        //} else {
        //    var position = setPosition(selectButton);
        //    panel.css("top", position.top);
        //    panel.css("left", position.left);
        //    panel.show();
        //}

        ////判读用哪种方式绑定初始值
        ////如果页面还没有加载则，使用延迟方法绑定
        //if (!!$("iframe#frame_" + newId)[0].contentWindow.SetCheckedNode) {
        //    $("iframe#frame_" + newId)[0].contentWindow.SetCheckedNode(valueList);
        //} else {
        //    $("iframe#frame_" + newId).load(function () {
        //        $("iframe#frame_" + newId)[0].contentWindow.SetCheckedNode(valueList);
        //    });
        //}

        //// 绑定数据后执行方法
        //if (afterBinding && typeof (afterBinding) == 'function') {
        //    afterBinding();
        //}

        //$(document).one("click", function () {
        //    panel.hide();
        //});
        //return false;
    });


    //选择后回调
    window._ActiveSelectBatchCategoryCallBack = function (data) {
        if (data != null) {


            //UI.BindDataFromZTreeNodes(valueContainer, data);
            ////初始化结果到自定义属性。自定义属性用来暂存现在值
            //valueContainer.attr("pdcategory", "");
            //var sysNoStr = "";
            //sysNoStr = !!valueContainer.attr("pdcategory") ? valueContainer.attr("pdcategory") : "";
            //for (var i = 0; i < data.length; i++) {
            //    if (sysNoStr.length > 0)
            //        sysNoStr += ",";
            //    sysNoStr += data[i].id;
            //}
            //valueContainer.attr("pdcategory", sysNoStr);
            if ($.isFunction(useroptions.callBack))
                useroptions.callBack.call(window, data);
        }

        var panel = $("#" + newId);
        panel.hide();
    };
};

DAO.SelectArea = function (selectButton, valueContainer, useroptions) {
    var options = $.extend({
        isMulti: false,
        width: 290,
        height: 360,
        position: "bottom left",
        zIndex: 999,
        url: "/Basic/AreaSelector",
        onlyLeaftSelect: true,
        isAll: true,
        margin: {
            vertical: 0,         //垂直位置
            horizontal: 0        //水平位置
        },
        callBack: function (nodes) { }
    }, useroptions);

    var newId = $(selectButton).attr("id");
    newId = newId + "_BsArea_Single_Select_Panel";
    var randNum = Math.round(Math.random(1000) * 1000 / 1000);
    var htmlTemplate = '<div id="' + newId + '" style="position:absolute; z-index:' + options.zIndex + '; display:none;">'
                       + '<iframe id="frame_' + newId + '" name="frame_' + newId + '" frameborder="0" border="0" scrolling="no" src="' + options.url + '?IsMultipleSelect=' + options.isMulti + '&Width=' + options.width + '&Height=' + options.height + '&isAll=' + options.isAll + '&OnlyLeaftSelect=' + options.onlyLeaftSelect + '&r=' + randNum + '" style="width: ' + options.width + 'px; height: ' + options.height + 'px;">'
                        + '</iframe>'
                        + '</div>';
    $(selectButton).after(htmlTemplate);
    //$("body").append(htmlTemplate);

    //计算弹出层位置
    var setPosition = function (element) {
        //根据点击的对象设置弹出框位置
        var left = $(element).position().left;      //取得按钮位置
        var top = $(element).position().top;        //取得按钮位置

        //设置顶对齐
        if (options.position.toLocaleLowerCase().indexOf("top") >= 0) {
            top = top - options.height() - options.margin.vertical;
        }

        //设置低对齐
        if (options.position.toLocaleLowerCase().indexOf("bottom") >= 0) {
            top = top + $(element).height() + options.margin.vertical;
        }

        //设置左对齐
        if (options.position.toLocaleLowerCase().indexOf("left") >= 0) {
            left = left - options.width - options.margin.horizontal + $(element).width();
        }

        //设置右对齐
        if (options.position.toLocaleLowerCase().indexOf("right") >= 0) {
            left = left + options.width + options.margin.horizontal;
        }

        return { top: top, left: left };
    };
    var panel = $("#" + newId);
    //设置弹出层宽高和位置
    panel.width(options.width);
    panel.height(options.height);

    //为按钮添加点击事件
    $(selectButton).bind("click", function () {
        //读取新加入的层
        var panel = $("#" + newId);

        //按钮作为开关设置弹出层显示不显示
        if (panel.is(":visible")) {
            panel.hide();
            return false;
        } else {
            var position = setPosition(selectButton);
            panel.css("top", position.top);
            panel.css("left", position.left);
            panel.show();
        }


        //设置已选择的分类
        var valueList = $(valueContainer).attr("selAreaSysNo") == null ? [] : $(valueContainer).attr("selAreaSysNo").split(",");
        //判读用哪种方式绑定初始值
        //如果页面还没有加载则，使用延迟方法绑定
        if (!!$("iframe")[0].contentWindow.SetCheckedNode) {
            $("iframe")[0].contentWindow.SetCheckedNode(valueList);
        } else {
            $("iframe").load(function () {
                $("iframe")[0].contentWindow.SetCheckedNode(valueList);
            });
        }

        $(document).one("click", function () {
            panel.hide();
        });
        return false;
    });


    //选择后回调
    window._ActiveSelectAreaCallBack = function (data) {
        if (data != null) {

            UI.BindDataFromZTreeNodes(valueContainer, data);
            //初始化结果到自定义属性。自定义属性用来暂存现在值
            valueContainer.attr("selAreaSysNo", "");
            var sysNoStr = "";
            sysNoStr = !!valueContainer.attr("selAreaSysNo") ? valueContainer.attr("selAreaSysNo") : "";
            for (var i = 0; i < data.length; i++) {
                if (sysNoStr.length > 0)
                    sysNoStr += ",";
                sysNoStr += data[i].id;
            }
            valueContainer.attr("selAreaSysNo", sysNoStr);
            if ($.isFunction(useroptions.callBack))
                useroptions.callBack.call(window, data);
        }

        var panel = $("#" + newId);
        panel.hide();
    };
};
DAO.SelectBrand = function (useroptions) {
    ///唐永勤添加
    ///选择品牌插件
    ///返回值{sysno:sysno, name:name}
    ///调用例子 DAO.SelectBrand({ callback: function (data) { alert(data.sysno + '/'+data.name) } });
    var options = $.extend({
        callBack: function () { }
    }, useroptions);

    //伪回调
    window.top._ActiveBrandSelectorCallBack = function (data) {
        $.isFunction(options.callBack) && options.callBack.call(window, data);
    };
    UI.DialogOpen('/Product/BrandSelector', {
        width: '910px',
        height: '450px',
        title: '选择品牌',
        init: function () {
            this.button(
                {
                    name: '确认',
                    callback: function () {
                        var $iframe = this.iframe.contentWindow;
                        $iframe.CallBack();
                    },
                    focus: true
                }, {
                    name: '取消'
                }
            );
        }
    }, false);
};

DAO.SelectBrand = function (useroptions) {
    ///唐永勤添加
    ///选择品牌插件
    ///返回值{sysno:sysno, name:name}
    ///调用例子 DAO.SelectBrand({ callback: function (data) { alert(data.sysno + '/'+data.name) } });
    var options = $.extend({
        callBack: function () { }
    }, useroptions);

    //伪回调
    window.top._ActiveBrandSelectorCallBack = function (data) {
        $.isFunction(options.callBack) && options.callBack.call(window, data);
    };
    UI.DialogOpen('/Product/BrandSelector', {
        width: '910px',
        height: '450px',
        title: '选择品牌',
        init: function () {
            this.button(
                {
                    name: '确认',
                    callback: function () {
                        var $iframe = this.iframe.contentWindow;
                        $iframe.CallBack();
                    },
                    focus: true
                }, {
                    name: '取消'
                }
            );
        }
    }, false);
};

DAO.SelectFreightModule = function (useroptions) {
    ///唐永勤添加
    ///选择品牌插件
    ///返回值{sysno:sysno, name:name}
    ///调用例子 DAO.SelectBrand({ callback: function (data) { alert(data.sysno + '/'+data.name) } });
    var options = $.extend({
        callBack: function () { }
    }, useroptions);

    //伪回调
    window.top._ActiveFreightModuleSelectorCallBack = function (data) {
        $.isFunction(options.callBack) && options.callBack.call(window, data);
    };
    UI.DialogOpen('/Logistics/FreightModuleSelector', {
        width: '910px',
        height: '450px',
        title: '选择运费模板',
        init: function () {
            this.button(
                {
                    name: '确认',
                    callback: function () {
                        var $iframe = this.iframe.contentWindow;
                        $iframe.CallBack();
                    },
                    focus: true
                }, {
                    name: '取消'
                }
            );
        }
    }, false);
};

DAO.SelectPdProduct = function (useroptions) {
    ///王耀发添加
    ///选择品牌插件
    ///返回值{sysno:sysno, name:name}
    ///调用例子 DAO.SelectBrand({ callback: function (data) { alert(data.sysno + '/'+data.name) } });
    var options = $.extend({
        callBack: function () { }
    }, useroptions);

    //伪回调
    window.top._ActivePdProductSelectorCallBack = function (data) {
        $.isFunction(options.callBack) && options.callBack.call(window, data);
    };
    UI.DialogOpen('/Product/ProductSelector', {
        width: '1300px',
        height: '600px',
        title: '选择商品',
        init: function () {
            this.button(
                {
                    name: '确认',
                    callback: function () {
                        var $iframe = this.iframe.contentWindow;
                        $iframe.CallBack();
                    },
                    focus: true
                }, {
                    name: '取消'
                }
            );
        }
    }, false);
};

DAO.SelectDealerMallProduct = function (useroptions) {
    ///王耀发添加
    ///选择品牌插件
    ///返回值{sysno:sysno, name:name}
    ///调用例子 DAO.SelectBrand({ callback: function (data) { alert(data.sysno + '/'+data.name) } });
    var options = $.extend({
        callBack: function () { }
    }, useroptions);

    //伪回调
    window.top._ActivePdProductSelectorCallBack = function (data) {
        $.isFunction(options.callBack) && options.callBack.call(window, data);
    };
    UI.DialogOpen('/Product/DealerMallProductSelector', {
        width: '1300px',
        height: '600px',
        title: '选择商品',
        init: function () {
            this.button(
                {
                    name: '确认',
                    callback: function () {
                        var $iframe = this.iframe.contentWindow;
                        $iframe.CallBack();
                    },
                    focus: true
                }, {
                    name: '取消'
                }
            );
        }
    }, false);
};

DAO.SelectCrCustomer = function (useroptions) {
    ///王耀发添加
    ///选择账号
    ///返回值{sysno:sysno, name:name}
    ///调用例子 DAO.SelectBrand({ callback: function (data) { alert(data.sysno + '/'+data.name) } });
    var options = $.extend({
        callBack: function () { }
    }, useroptions);

    //伪回调
    window.top._ActiveCrCustomerSelectorCallBack = function (data) {
        $.isFunction(options.callBack) && options.callBack.call(window, data);
    };
    UI.DialogOpen('/CRM/CrCustomerSelector', {
        width: '900px',
        height: '500px',
        title: '选择账号',
        init: function () {
            this.button(
                {
                    name: '确认',
                    callback: function () {
                        var $iframe = this.iframe.contentWindow;
                        $iframe.CallBack();
                    },
                    focus: true
                }, {
                    name: '取消'
                }
            );
        }
    }, false);
};

DAO.SelectAttribute = function (useroptions) {
    ///唐永勤添加
    ///选择属性插件
    ///返回值数组对象[{SysNo: sysno, AttributeName: AttributeName, BackEndName:BackEndName}]
    ///调用例子 DAO.SelectAttribute({arrAttributeIds:[274,271,278,279] , callBack:function(data){  alert(JSON.stringify(data)); }});
    var options = $.extend({
        width: '910px',
        height: '540px',
        arrAttributeIds: [],
        initDisabled: false,
        callBack: function () { }
    }, useroptions);

    //伪回调
    window.top._ActiveAttributeSelectorCallBack = function (data) {
        $.isFunction(options.callBack) && options.callBack.call(window, data);
    };

    window.top._ArrAttributeIds = options.arrAttributeIds;
    window.top._initDisabled = options.initDisabled;
    UI.DialogOpen('/Product/ProductAttributeSelector', {
        width: options.width,
        height: options.height,
        title: '选择属性',
        init: function () {
            this.button(
                {
                    name: '确认',
                    callback: function () {
                        var $iframe = this.iframe.contentWindow;
                        $iframe.CallBack();
                    },
                    focus: true
                }, {
                    name: '取消'
                }
            );
        }
    }, false);
};


DAO.SelectAttributeGroup = function (useroptions) {
    ///唐永勤添加 2013-07-12 
    ///选择属性插件
    ///返回值数组对象[{SysNo: sysno, AttributeName: AttributeName, BackEndName:BackEndName}]
    ///调用例子 DAO.SelectAttributeGroup({arrAttributeGroupIds:[23,25] , callBack:function(data){  alert(JSON.stringify(data)); }});
    var options = $.extend({
        width: '910px',
        height: '540px',
        arrAttributeGroupIds: [],
        initDisabled: false,
        callBack: function () { }
    }, useroptions);

    //伪回调
    window.top._ActiveAttributeGroupSelectorCallBack = function (data) {
        $.isFunction(options.callBack) && options.callBack.call(window, data);
    };

    window.top._ArrAttributeGroupIds = options.arrAttributeGroupIds;
    window.top._initDisabled = options.initDisabled;
    UI.DialogOpen('/Product/ProductAttributeGroupSelector', {
        width: options.width,
        height: options.height,
        title: '选择属性组',
        init: function () {
            this.button(
                {
                    name: '确认',
                    callback: function () {
                        var $iframe = this.iframe.contentWindow;
                        $iframe.CallBack();
                    },
                    focus: true
                }, {
                    name: '取消'
                }
            );
        }
    }, false);
};

DAO.CopyProduct = function (useroptions) {
    ///唐永勤添加 2013-07-16 
    ///克隆商品插件
    ///返回值对象{SysNo: sysno, AttributeName: AttributeName, BackEndName:BackEndName}]
    ///调用例子 DAO.CopyProduct({productSysno:20 , callBack:function(data){  alert(JSON.stringify(data)); }});
    var options = $.extend({
        width: '600px',
        height: '185px',
        productSysno: 0,
        callBack: function () { }
    }, useroptions);

    //伪回调
    window.top._ActiveCopyProductCallBack = function (data) {
        $.isFunction(options.callBack) && options.callBack.call(window, data);
    };


    UI.DialogOpen('/Product/CopyProduct?sysNo=' + options.productSysno, {
        width: options.width,
        height: options.height,
        title: '克隆商品',
        init: function () {
            this.button(
                {
                    name: '确认',
                    callback: function () {
                        var $iframe = this.iframe.contentWindow;
                        $iframe.CallBack();
                        return false;
                    },
                    focus: true
                }, {
                    name: '取消'
                }
            );
        }
    }, false);
};

DAO.SelectFeSoftCategory = function (useroptions) {
    ///唐永勤添加
    ///选择软件分类插件
    ///返回值{sysno:sysno, name:name}
    ///调用例子 DAO.SelectBrand({ callback: function (data) { alert(data.sysno + '/'+data.name) } });
    var options = $.extend({
        callBack: function () { }
    }, useroptions);

    //伪回调
    window.top._ActiveBrandSelectorCallBack = function (data) {
        $.isFunction(options.callBack) && options.callBack.call(window, data);
    };
    UI.DialogOpen('/Front/FeSoftCategorySelector', {
        width: '910px',
        height: '450px',
        title: '选择软件分类',
        init: function () {
            this.button(
                {
                    name: '确认',
                    callback: function () {
                        var $iframe = this.iframe.contentWindow;
                        $iframe.CallBack();
                    },
                    focus: true
                }, {
                    name: '取消'
                }
            );
        }
    }, false);
};

DAO.PriceAdjustmentRequest = function (userOptions) {
    //合并参数
    var options = $.extend({
        width: 460,
        height: 310,
        productSysNoList: [],
        priceType: [],
        success: null,
        autoClose: true
    }, userOptions);

    if (options.productSysNoList.length > 1) {
        options.height += 70;
    }

    //验证参数
    if (options.productSysNoList.length == 0 || options.productSysNoList[0] == "") {
        UI.tips.tip_alert('tips_warning', '请设置您要调价的商品！！');
    }

    //拼接URL参数
    var parameters = "ProductSysNoList=" + options.productSysNoList.AsDelimited("&ProductSysNoList=");  //商品系统编号
    //如果有指定价格来源类型将拼接字符串
    if (options.priceType.length > 0 && options.priceType[0] != "") {
        parameters += "&priceSourceType=" + options.priceType.AsDelimited("&priceSourceType=");         //价格来源系统枚举编号
    }

    //伪回调
    window.parent._ActivePriceHistoryCallBack = function (success) {
        if ($.isFunction(options.success))
            options.success.call();
        if (options.autoClose)
            childWindow.Close.call();
    };

    var childWindow;

    UI.DialogOpen("/Product/AddPriceHistory/?" + parameters, {
        title: "商品调价申请",
        width: options.width,
        height: options.height,
        footerClass: "align_c clearfix",
        resize: true,
        init: function () {
            this.button(
                {
                    name: '提交申请',
                    callback: function () {
                        var $iframe = this.iframe.contentWindow;
                        $iframe.SaveCallback(this);
                        childWindow = $iframe;
                        return false;
                    },
                    focus: true
                }, {
                    name: '取消',
                    callback: function () {
                        return true;
                    }
                }
            );
        }
    }, false);
};


//分销商统一调价
DAO.PriceDistributionRequest = function (userOptions) {
    //合并参数
    var options = $.extend({
        width: 460,
        height: 310,
        productSysNoList: [],
        dealerSysNo: null,
        success: null,
        autoClose: true
    }, userOptions);
    if (options.productSysNoList.length > 1) {
        options.height += 70;
    }

    //拼接URL参数
    var parameters = "ProductSysNoList=" + options.productSysNoList.AsDelimited("&ProductSysNoList=");  //商品系统编号
    if (dealerSysNo != null) {
        parameters += "&dealerSysNo=" + options.dealerSysNo;         //价格来源系统枚举编号
    }

    //伪回调
    window.parent._ActivePriceHistoryCallBack = function (success) {
        if ($.isFunction(options.success))
            options.success.call();
        if (options.autoClose)
            childWindow.Close.call();
    };

    var childWindow;
    UI.DialogOpen("/Distribution/AddPriceHistory/?" + parameters, {
        title: "商品调价申请",
        width: options.width,
        height: options.height,
        footerClass: "align_c clearfix",
        resize: true,
        init: function () {
            this.button(
                {
                    name: '提交申请',
                    callback: function () {
                        var $iframe = this.iframe.contentWindow;
                        $iframe.SaveCallback(this);
                        childWindow = $iframe;
                        return false;
                    },
                    focus: true
                }, {
                    name: '取消',
                    callback: function () {
                        return true;
                    }
                }
            );
        }
    }, false);
};

DAO.SelectOrder = function (userOptions) {
    ///朱家宏添加 2013-07-17 
    ///选择订单插件
    ///返回值[订单SysNo]
    ///useroptions.onlinepay:在线支付查询(true or false) 
    ///调用例子 DAO.SelectOrder({onlinepay: true,callback:function (data) { $("#txtOrderSysNo").val(data); }});
    window._ActiveSelectOrderSysNo = function (data) {
        userOptions.callback(data);
    };
    var urlparams = "";
    if (userOptions.onlinepay)
        urlparams = "?onlinepay=" + userOptions.onlinepay;
    UI.DialogOpen('/Order/Selector' + urlparams, {
        width: '980px',
        height: '530px',
        title: '选择订单'
    }, false);
};

//仓库多选 ，朱成果 2013/08/02
DAO.MultipleSelectWarehouse = function (selectButton, selectContainer, callbackfun) {
    var options = {
        width: 300,
        height: 360,
        position: "bottom left",
        zIndex: 999,
        url: "/Order/MultipleSelectWarehouse",
        margin: {
            vertical: 0,         //垂直位置
            horizontal: 0        //水平位置
        }
    }
    var newId = $(selectButton).attr("id");
    newId = newId + "_MultipleSelectWarehouse_Panel";
    var randNum = Math.round(Math.random(1000) * 1000 / 1000);
    var htmlTemplate = '<div id="' + newId + '" style="position:absolute; z-index:' + options.zIndex + '; display:none;">'
                       + '<iframe id="frame_' + newId + '" name="frame_' + newId + '" frameborder="0" border="0" scrolling="no" src=' + options.url + ' Width=' + options.width + '&Height=' + options.height + '&r=' + randNum + '" style="width: ' + options.width + 'px; height: ' + options.height + 'px;">'
                        + '</iframe>'
                        + '</div>';
    $(selectButton).after(htmlTemplate);
    //计算弹出层位置
    var setPosition = function (element) {
        //根据点击的对象设置弹出框位置
        var left = $(element).position().left;      //取得按钮位置
        var top = $(element).position().top;        //取得按钮位置

        //设置顶对齐
        if (options.position.toLocaleLowerCase().indexOf("top") >= 0) {
            top = top - options.height() - options.margin.vertical;
        }

        //设置低对齐
        if (options.position.toLocaleLowerCase().indexOf("bottom") >= 0) {
            top = top + $(element).height() + options.margin.vertical;
        }

        //设置左对齐
        if (options.position.toLocaleLowerCase().indexOf("left") >= 0) {
            left = left - options.width - options.margin.horizontal + $(element).width();
        }

        //设置右对齐
        if (options.position.toLocaleLowerCase().indexOf("right") >= 0) {
            left = left + options.width + options.margin.horizontal;
        }

        return { top: top, left: left };
    };
    var panel = $("#" + newId);
    //设置弹出层宽高和位置
    panel.width(options.width);
    panel.height(options.height);
    $(selectButton).bind("click", function () {
        //读取新加入的层
        var panel = $("#" + newId);

        //按钮作为开关设置弹出层显示不显示
        if (panel.is(":visible")) {
            panel.hide();
            return false;
        } else {
            var position = setPosition(selectButton);
            panel.css("top", position.top);
            panel.css("left", position.left);
            panel.show();
        }
        $(document).one("click", function () {
            panel.hide();
        });
        return false;
    });
    //1.判断select选项中 是否存在Value="paraValue"的Item   
    function jsSelectIsExitItem(objSelect, objItemValue) {
        var isExit = false;
        for (var i = 0; i < objSelect.find("option").length; i++) {
            if (objSelect.get(0).options[i].value == objItemValue) {
                isExit = true;
                break;
            }
        }
        return isExit;
    }
    //选择后回调
    window._ActiveMultipleSelectWarehouseCallBack = function (data) {
        if (data != null) {
            if (selectContainer != null) {
                for (var i = 0; i < data.length; i++) {
                    if (!jsSelectIsExitItem(selectContainer, data[i].id)) {
                        $("<option></option>")
                        .val(data[i].id)
                        .html(data[i].name)
                        .appendTo(selectContainer);
                    }
                }
            }
            if (callbackfun != null) {
                callbackfun.call(window, data);
            }
        }

        var panel = $("#" + newId);
        panel.hide();
    };

};

DAO.SelectPromotionRule = function (callback) {
    ///朱家宏添加 2013-08-26 
    ///选择促销规则
    ///调用例子 DAO.SelectPromotionRule(function (rule) { alert(rule.sysNo);alert(rule.name); });
    window._ActiveSelectPromotionRule = function (rule) {
        callback(rule);
    };
    UI.DialogOpen('/Promotion/PromotionRuleSelector', {
        width: '1080px',
        height: '530px',
        title: '选择促销规则'
    }, false);
};


DAO.SelectPromotion = function (useroptions) {
    ///朱家宏添加 2013-08-26 
    ///选择促销 
    ///调用例子 DAO.SelectPromotion({promotionType:10,multiple:false,callBack:function(a){console.log(a);} })
    ///promotionType:促销类型
    ///multiple：true多选 false单选 (默认多选)

    //合并参数
    var options = $.extend({
        promotionType: 0,
        width: 900,
        height: 570,
        multiple: true,
        IsOverlay: 0 //是否促销叠加
    }, useroptions);

    var parameters = "?_multiple=" + options.multiple;
    parameters += "&_isOverlay=" + options.IsOverlay;
    if (options.promotionType != 0)
        parameters += "&_pType=" + options.promotionType;

    //伪回调
    window.top._ActiveSelectPromotionCallBack = function (data) {
        if ($.isFunction(useroptions.callBack)) {
            useroptions.callBack.call(window, data);
        }
    };
    UI.DialogOpen('/Promotion/PromotionSelector/' + parameters, {
        width: options.width,
        height: options.height,
        title: '促销选择',
        init: function () {
            var $iframe = this.iframe.contentWindow;
            this.button(
                {
                    name: '保存',
                    callback: function () {
                        $iframe.CallBack();
                    },
                    focus: true
                }, {
                    name: '取消',
                    callback: function () {

                    }
                }
            );
        }
    }, false);
};

DAO.SelectSyUser = function (callback) {
    ///朱家宏添加 2013-10-10 
    ///选择系统用户插件
    ///返回值[用户]
    ///调用例子 DAO.SelectSyUser(function (data) { $("#txtOrderSysNo").val(data); });
    window._ActiveSelectSyUser = function (data) {
        callback(data);
    };
    UI.DialogOpen('/Sys/SysUserSelector', {
        width: '980px',
        height: '530px',
        title: '选择用户'
    }, false);
};

DAO.SelectBsText = function (txtoptions) {

    //合并参数
    var options = $.extend({
        title: '选择文本',
        width: 500,
        height: 400,
        dataurl: "/Basic/DoSelectTextQuery?type=3"
    }, txtoptions);

    var parameters = "?dataurl=" + options.dataurl;
    //伪回调
    window._ActiveSelectBsText = function (data) {
        if ($.isFunction(txtoptions.callBack)) {
            txtoptions.callBack.call(window, data);
        }
    };
    UI.DialogOpen('/Basic/BsSelectText' + parameters, {
        width: options.width + "px",
        height: options.height + "px",
        title: options.title
    }, false);
};

DAO.SelectBarcode = function (useroptions) {
    ///唐永勤添加
    ///选择品牌插件
    ///返回值{sysno:sysno, name:name}
    ///调用例子 DAO.SelectBrand({ callback: function (data) { alert(data.sysno + '/'+data.name) } });
    var options = $.extend({
        callBack: function () { }
    }, useroptions);

    //伪回调
    window.top._ActiveBrandSelectorCallBack = function (data) {
        $.isFunction(options.callBack) && options.callBack.call(window, data);
    };
    UI.DialogOpen('/Product/BarCodeList/', {
        width: '910px',
        height: '450px',
        title: '选择条形码',
        init: function () {
            this.button(
                {
                    name: '确认',
                    callback: function () {
                        var $iframe = this.iframe.contentWindow;
                        $iframe.CallBack();
                    },
                    focus: true
                }, {
                    name: '取消'
                }
            );
        }
    }, false);
};
DAO.SelectPdProductList = function (useroptions) {
    ///唐永勤添加
    ///选择品牌插件
    ///返回值{sysno:sysno, name:name}
    ///调用例子 DAO.SelectBrand({ callback: function (data) { alert(data.sysno + '/'+data.name) } });
    var options = $.extend({
        callBack: function () { }
    }, useroptions);

    //伪回调
    window.top._ActiveProductListSelectorCallBack = function (data) {
        $.isFunction(options.callBack) && options.callBack.call(window, data);
    };
    UI.DialogOpen('/Product/PdProductList/', {
        width: '910px',
        height: '450px',
        title: '选择商品',
        init: function () {
            this.button(
                {
                    name: '确认',
                    callback: function () {
                        var $iframe = this.iframe.contentWindow;
                        $iframe.CallBack();
                    },
                    focus: true
                }, {
                    name: '取消'
                }
            );
        }
    }, false);
};