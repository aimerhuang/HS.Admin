/*=author:余勇 封装退换货操作=*/
//获取取件方式
function getPickUp(handleDepartment, pickUpShipTypeSysNo) {
    var content = '';
    if ($("#WarehouseSysNo").val() != "") {
        $.ajax({
            async: false,
            type: "get",
            url: "/Ajax/GetLgPickupType?wareHouseSysNo=" + $("#WarehouseSysNo").val() + "&handleDepartment=" + handleDepartment,
            success: function (data) {
                if (data) {
                    $.each(data, function (idx, item) {
                            content += "<option value='" + item.SysNo + "'>" + item.PickupTypeName + "</option>";
                       
                    });
                    $("#PickUpShipTypeSysNo").html(content);
                    $("#PickUpShipTypeSysNo").val(pickUpShipTypeSysNo);
                    displayPickupShipType($("#PickUpShipTypeSysNo"));
                    $("#PickUpShipTypeSysNo").change(function () {
                        displayPickupShipType($(this));
                    });
            }}
        });
    }
}
//获取退款方式
function getRefundType(reFundTypeStatus, orderSysNo, handleDepartment, reFundType,shopNo) {
    var content = '';
    $.ajax({
        async: false,
        type: "get",
        url: "/Ajax/LoadRefundType?orderSysNo=" + orderSysNo + "&handleDepartment=" + handleDepartment + "&shopNo=" + shopNo,
        success: function (data) {
            if (data) {
                $.each(data, function (idx, item) {
                    content += "<option value='" + item.Value + "'>" + item.Text + "</option>";
                });
                $("#RefundType").html(content);
                $("#RefundType").val(reFundType);
                if ($("#RefundType").val() == reFundTypeStatus) {
                    $("#bankDiv").removeClass("hide");
                } else {
                    $("#bankDiv").addClass("hide");
                }
                $("#RefundType").change(function () {
                    if ($(this).val() == reFundTypeStatus) {
                        $("#bankDiv").removeClass("hide");
                    } else {
                        $("#bankDiv").addClass("hide");
                    }
                });
            }
        }
    });
}  //获取商品列表数据,type=1表示退货
function getProductData(type) {
    var orderItem = [];
    $(".product_list").each(function() {
        var outStore = {};
        var outItem = [];
        $(this).find("tr").each(function(i) {
            if ($(this).find("td > input").eq(0).attr("checked")) {
                var stockOutItemSysNo = $(this).find("td>input").eq(0).val();

                var productName = $(this).find("td").eq(2).text();
                var originalPrice = $(this).find("td").eq(3).text().toDecimal();
                var rmaQuantity = $(this).find("td").eq(7).find("input").val();
                var returnPriceType = 10;
                var refundAmount;
                var rmaReason;
                var productSysNo;
                if (type == 1) {
                    if ($(this).find(".CustomPrice").attr("checked") == "checked") {
                        returnPriceType = 20;
                    }                    
                    refundAmount = $(this).find("td").eq(9).find("input").val();
                    rmaReason = $(this).find("td").eq(10).find("select").val();
                    productSysNo = $(this).find("td").eq(11).text();
                } else {
                    rmaReason = $(this).find("td").eq(8).find("select").val();
                    productSysNo = $(this).find("td").eq(9).text();
                }
                var item = {
                    StockOutItemSysNo: stockOutItemSysNo,
                    ProductSysNo: productSysNo,
                    RmaQuantity: rmaQuantity,
                    ProductName: productName,
                    OriginalPrice: originalPrice,
                    RefundAmount: refundAmount,
                    RmaReason: rmaReason,
                    ReturnPriceType:returnPriceType
                };
                outItem.push(item);
            }
        });
        if (outItem.length>0){
            outStore["ReturnWhStockOutItemEx"] = outItem;
            orderItem.push(outStore);
        }
    });
    return orderItem;
}

//订购商品列表数量改变事件,type=1表示退货
function quantityChange(input,type) {
    var tr = $(input).parents("tr");
    var returnAmount=0.00;
    if (tr.find(".checktd:eq(0)").attr("checked") == "checked") {
        var quantity = Number($(input).val());  //当前数量
        var price = tr.find("td:eq(3)").text().toDecimal();  //单价
        var allowQuantity = tr.find("td").eq(6).html(); //允许退换的最大数量
        if (quantity > allowQuantity) {
            $(input).val(allowQuantity);
            quantity = allowQuantity;
        }
        returnAmount=(price * quantity).toFixed(2);
    }
    else {
        $(input).val(0);
    }
}
//金额汇总
function amountTotal() {
    var total = 0;
    $(".product_list tr").each(function () {
        var tr = $(this);
        if (tr.find(".checktd").attr("checked") == "checked") {
            total += Number(tr.find(".ReturnAmount").val()) || 0;
        }
    });
    $("#RefundAmount").val(total.toFixed(2));
}

$("input.checkall").click(function () {
    var state = $(this).attr("checked");
    if (state) { state = true; } else { state = false }
    $("input.checktd").each(function () {
        $(this).attr("checked", state);
    });
});

//绑定地址
function setDefaultArea(dpdCountry, dpdProvince, dpdCity, dpdArea, a) {
    if (a == null || a == "") {
        Area(dpdCountry,dpdProvince, dpdCity, dpdArea, {
            a: null,
            callback: function (type) {
            }
        });
    } else {
        Area(dpdCountry,dpdProvince, dpdCity, dpdArea, { a: a, callback: function (type) { } });
    }
}
//获取取货地址数据
function getPickUpShipAddress() {
    return Ajax.getInputValues("pickup");
};
//获取收货地址数据
function getSoReceiveAddress() {
    return Ajax.getInputValues("Receive");
};
//根据商品列表checkbox启用禁用录入框
function setInputDisable() {
    $(".product_list tr").each(function() {
        if ($(this).find("input[type='checkbox']").attr("checked") != "checked") {
            $(this).find("input,select,button").attr("disabled", "disabled");
        }
    });
    $(".product_list input[type='checkbox']").removeAttr("disabled");

    $(".product_list input[type='checkbox']").click(function() {
        if ($(this).closest("tr").find("[type='checkbox']").attr("checked")) {
            $(this).closest("tr").find("input,select,button").removeAttr("disabled");
        } else {
            $(this).closest("tr").find("input,select,button").attr("disabled", "disabled");
            $(".product_list input[type='checkbox']").removeAttr("disabled");
        }
    });
}