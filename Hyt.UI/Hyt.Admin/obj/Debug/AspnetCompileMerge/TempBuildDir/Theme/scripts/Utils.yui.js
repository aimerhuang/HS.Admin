/*=author:余勇 封装ajax处理，便于复杂条件查询及ajax操作=*/
/*=author:余勇 添加get方式提交方法 日期：2013-07-04=*/
function Ajax() {

    /************** ajax 处理 **************
    * url:              调用地址
    * attrName:           在查询条件页面控件上加上的属性名：属性名=参数名 比如：<select name="status" id="status" Search="Status">中的Search="Status" 提交时会自动构造该参数
    * fnPrepareParams:  准备参数：function(params){ }，调用 params.setParams(key, value) 添加参数
    * fnSucc:           服务器调用成功后执行的函数: function(data){ }，data 为服务器返回的用户json数据
    */
    //post方式提交
    this.post = function (url, attrName, fnPrepareParams, fnSucc) { ajaxfn(url, attrName, fnPrepareParams, fnSucc, "post"); };

    //get方式提交
    this.get = function (url, attrName, fnPrepareParams, fnSucc) { ajaxfn(url, attrName, fnPrepareParams, fnSucc, "get"); };

    //post方式提交josn参数
    this.postJosn = function (url, attrName, fnPrepareParams, fnSucc) { ajaxfn(url, attrName, fnPrepareParams, fnSucc, "postjosn"); };

    //通过属性取得参数对像
    this.getInputValues = function (attrName) {
        return getParas(attrName);
    };
    function getParaObj(attrName) {
        /**参数**/
        var p = new params();
        if (attrName == "" || !attrName) {
            return p;
        }
        var f = $("[" + attrName + "]");
        f.each(function () {
            var me = $(this);
            var key = $.trim(me.attr(attrName));
            var value = me.val();
            
            p.setParams(key, value);
        });
        return p;
    }
    function getParas(attrName) {
        var p= getParaObj(attrName);
        return p.getParams();
    }

     function ajaxfn(url, attrName, fnPrepareParams, fnSucc, type) {
         if (!type) type = "post";
         var contentType = "application/x-www-form-urlencoded";
         var p = getParaObj(attrName);
        if ($.isFunction(fnPrepareParams)) {
            fnPrepareParams(p);
        }
        var params = p.getParams();
        if (type == "postjosn") {
            params = JSON.stringify(params);
            type = "post";
            contentType = "application/json; charset=utf-8";
        }
        $.ajax({
            type: type,
            url: url,
            data: params,
            contentType: contentType,
            
            success: function (ret) {
                if (!$.isFunction(fnSucc)) return;
                //登录超时执行
                if (ret.IsLogout) {
                    eval(ret.Callback);
                    return;
                }
                fnSucc(ret);
            },
            error: function (event, xhr, opt) {
                //if (cover) cover.Remove();
                var resText = xhr.responseText;
                //alert("系统错误请联系管理员！错误信息：" + "[" + opt.url + ": " + xhr.status + " " + xhr.statusText + "]");
            }
        });

        /**参数**/
     };
     function params() {
         var p = {};
         this.setParams = function (key, value) {
             if (key == "") return;
             if (typeof p[key] == "undefined")
                 p[key] = value;
             else {
                 p[key] += "," + value;
             }
         };
         this.getParams = function () {
             return p;
         };
     }
}

var Ajax = new Ajax();

/*=author:余勇 弹出提示 日期：2013-07-19=*/
var Utils = function () {
    //显示警告提示
    this.alert = function (msg, fn, icon) { showDialogBox(msg, fn, icon); }; //提示窗口
    function showDialogBox(msg, fn, icon) {
        icon = icon || null;
        var fun = fn || null;
        var config = {
            content: msg,
            callback: fun,
            cancel: false,
            padding: '10px 20px'
        };
        if (icon) {
            config["icon"] = icon;
        }
        UI.Alert(config);
    }
    this.MaskStart = function (jqobj) {
        if (window.cover) { window.cover.Remove(); }
        window.cover = UI.Mask({ obj: jqobj, opacity: 0.6 });
    };
    this.MaskStop = function () {
        window.cover.Remove();
    };
};
var Utils = new Utils();
