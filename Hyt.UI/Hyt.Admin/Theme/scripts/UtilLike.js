var GetUtilLike = function (val, type, id) {
    $.ajax({
        cache: true,
        type: "POST",
        url: "/InventorySheet/GetUtilLike",
        data: { "type": type, "keyWord": val },
        async: false,
        success: function (data) {
            // alert(JSON.stringify(data));

            $("#" + id).children().children("tbody").eq(1).html("");
            if (type == 1) {//查询会员
                for (var i = 0; i < data.length; i++) {
                    var html = "<tr style=\"border: none; cursor:pointer;\" onmouseover=\"btnonmouseover(this,id)\"  onclick=\"btnSub('" + data[i].SysNo + "','" + data[i].UserName + "',this)\"  >";
                    html += "<td style=\"border: none; cursor:pointer;\">" + data[i].Account + "</td>";
                    html += "<td style=\"border: none; cursor:pointer;\">" + data[i].UserName + "</td>";
                    html += "<td style=\"border: none; cursor:pointer;\">" + data[i].MobilePhoneNumber + "</td>";
                    html += "<td style=\"border: none; cursor:pointer;\">" + data[i].SysNo + "</td>";
                    html += "</tr>";
                    $("#" + id).children().children("tbody").eq(1).append(html);
                }
            } else { //查询商品
                for (var i = 0; i < data.length; i++) {
                    var html = "<tr style=\"border: none; cursor:pointer;\" onmouseover=\"btnonmouseover(this,id)\"  onclick=\"btnSub('" + data[i].SysNo + "','" + data[i].ErpCode + "',this)\"  >";
                    html += "<td style=\"border: none; cursor:pointer;\">" + data[i].ErpCode + "</td>";
                    html += "<td style=\"border: none; cursor:pointer;\">" + data[i].Barcode + "</td>";
                    html += "<td style=\"border: none; cursor:pointer;\">" + data[i].EasName + "</td>";
                    html += "<td style=\"border: none; cursor:pointer;\">" + data[i].SysNo + "</td>";
                    html += "</tr>";
                    $("#" + id).children().children("tbody").eq(1).append(html);
                }
            }
        }
    });
}

var btnonmouseover = function(th, id) {
    $(th).parent().children(".stag_color").removeClass("stag_color");
    $(th).addClass("stag_color");
}

var btnSub = function (id, name, th) {
    $(th).parent().parent().parent().parent().children().eq(0).val(name);
    $(th).parent().parent().parent().parent().children().eq(1).val(id);
}


//失去焦点检查
function inspect(th, type)
{
    var val=$(th).val();
    if (val != "")
    {
        $.ajax({
            cache: true,
            type: "POST",
            url: "/InventorySheet/GetUtilInspect",
            data: { "type": type, "keyWord": val },
            async: false,
            success: function (data) {
                if (data.length == 0)
                {
                    $(th).val("");
                    $(th).next().val("");
                }
            }
        });
    }
    
}