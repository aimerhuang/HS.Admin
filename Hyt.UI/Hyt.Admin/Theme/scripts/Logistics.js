function Logistics(){
    this.host = "http://112.74.67.53:8090/Express/";
    var template = function (time, context) {
        return '<tr><td width="25%">' + time + '</td><td width="75%">' + context + '</td></tr>';
    };
    //物流提供者
   this.logisticsProvider = {
        "0": {//快递100
            get: function (data) {
                var html = "";
                data = eval("(" + data + ")").data;
                for (var i = data.length - 1; i >= 0; i--) {
                    html += template(data[i].time, data[i].context);
                }
                return html;
            }
        },
        "2": {//有信达
            get: function (data) {
                var html = "";
                data = eval("(" + data + ")").data;
                for (var i = data.length - 1; i >= 0; i--) {
                    html += template(data[i].time, data[i].content);
                }
                return html;
            }
        },
        "3": {//威时沛运
            get: function (data) {
                var html = "";
                data = eval("(" + data + ")").data;
                for (var i = data.length - 1; i >= 0; i--) {                
                    html += template(data[i].optime, data[i].notes);
                }
                return html;
            }
        },
        "4": {//高捷个人物品
            get: function (data) {
                var html = "";
                data = eval("(" + data + ")").data;
                for (var i = data.length - 1; i >= 0; i--) {
                    html += template(data[i].remark, data[i].acceptTime);
                }
                return html;
            }
        },
        "5": {//高捷BC
            get: function (data) {

            }
        }

    };
    /*
        获取物流轨迹html代码
        @orderSysNo 订单系统编号
        @target 需要写入html目标对象
    */
    this.LogisticsLocusHtml = function (orderSysNo, target, logisticsProvider, fuc) {
        var _logisticsProvider = 0;
        if (logisticsProvider) {
            _logisticsProvider = logisticsProvider
        }
        $.getJSON(this.host + "ExpressService.svc/SearchLogisticsTrackingJsonP?jsoncallback=?&logisticsProvider=" + _logisticsProvider + "&orderSysNo=" + orderSysNo, function (data) {
            var result = data.SearchLogisticsTrackingJsonPResult;
            var html = "";
            if (!result.IsError) {
                html = _logistics.logisticsProvider[result.LogisticsType.toString()].get(result.Data);
            } else {
                html = result.ErrMsg;
            }

            !fuc ? $(target).html(html) : fuc(target, html);
        });
    }
}
var _logistics = new Logistics();

