/*
 * 会员提 TableBind 核心 1.0.0 
 * 
 *自动绑定table列表，
 *
 */
function TableBind(options) {
    var opts = $.extend({
        ajaxOptons: {                       //ajax请求参数，同$.ajax()的请求参数
            url: null
        },
        table: null,                         //table对象
        data: [],                           //数据源
        key: null,                          //主键
        rowCss: "even_color",               //交替行样式
        hoverCss: "trhover",
        alternatelyCss: "odd_color",        //交替行样式
        selectedCss: "",                    //选中样式
        showMask:true,                      //显示load的Mask层
        onBind: function (data) { },        //绑定时事件
        beforeRowBind: function (data) { },     //行绑定时事件
        afterRowBind: function (data, tr) { },    //行绑定后事件
        onSelectedRow: function (oldTr, newTr) { },   //选中行事件
        onMoveRow: function (oldTr, newTr, isUp) { },  //移动行事件
        onDelete: function (data) { }

    }, options);

    this.CheckAllBoxSelecter = null;
    

    this.GetOptions = function() {
        return opts;
    };

    function _init(that) {

        //判断是否没有传入table
        if (opts.table == null) {
            that.tableloader.Remove();
            return that;
        } else {
            opts.table = $(opts.table);
        }

        //清理所以旧数据， 重新绑定采用的是删后增的方式
        _Clear();

        //判断是否有数据
        if (opts.data.length == 0) {
            that.tableloader.Remove();
            return that;
        }

        //判读是否有主键字段
        if (opts.key == null) {
            that.tableloader.Remove();
            return that;
        }

        //绑定前执行用户的绑定事件
        opts.onBind(opts.data);

        //将数据执行绑定
        that.InsertRow(opts.data);          //插入数据

        this.CheckAllBoxSelecter = UI.CheckAllbox(opts.table, ".checkall");  //生产checkbox选中器对象
        
        //判断是否启用Ajaxloader，如果启用将移除loader
        if (!!that.tableloader)
            that.tableloader.Remove();   //移除loader
        return true;
    };

    //格式化模板列
    this.FormatTemplate = function(template, data) {

        //绑定json数据到行
        //templateRow.JsonBind(data);
        for (var column in data) {
            var pattern = new RegExp(eval("/\{" + column + "\}/g"));
            if (pattern.test(template))
                template = template.replace(pattern, data[column]);
        }

        //jQuery包装模板对象
        template = $(template);  

        //生产随机Id，ID有tableId+随机数+key键值
        var id = (opts.table.attr("id") || Math.round(Math.random(100) * 100 / 100)) + data[opts.key];
        template.attr("id", id);
        
        //缓存key键值到行数据，方便以后查找和直接使用
        template.attr("key", data[opts.key]);

        return template;
    };

    //变换模板数据行，该方法用于动态数据列绑定
    this.ChangeTemplate = function (template) {
        
        //包装模板数据        
        template = $(template);
        
        //用新模板数据替换旧数据模板
        $("tr[template='true']", opts.table).html(template.html());

        //判断是否有数据如果没有就设置空行数据
        if (opts.data.length == 0) {
            $(".nodatarow", opts.table).remove();
            setNoDataRow();
        }

    };

    //根据key主键获取tr行
    this.GetItemByKey = function(key) {
        return $("tr[key='" + key + "']", opts.table);
    };

    //根据key主键获取Json对象
    this.GetDateByKey = function(key) {
        for (var i = 0; i < opts.data; i++) {
            if (data[i][opts.key] == key) {
                return data[i];
            }
        }
        return null;
    };

    //更新行
    this.UpdateRow = function(newData) {

        //执行用户的行绑定
        opts.beforeRowBind(newData);

        //格式化数据
        var template = $("tr[template='true']", opts.table)[0].outerHTML;
        var templateRow = this.FormatTemplate(template, row);
        var tr = this.GetDateByKey(newdata[opts.key]);
        tr.html($(templateRow).html());

        //执行绑定后事件
        opts.afterRowBind(tr);

        //更新数据源数据
        for (var i = 0; i < opts.data; i++) {
            if (opts.data[i][opts.key] == newData[opts.key]) {
                opts.data[i] = newData;
                break;
            }
        }
    };

    //删除行 key: 可以是主键值 也可以是tr行
    this.DelRow = function (key) {
        //删除回调事件
        if (opts.onDelete(key) == false)
            return null;

        var deleteKeys = new Array();

        //如果key是字符数表示是key键的键值，如果是object表示是一行数据即tr行，这个行可恩能够是通过其他方法的来的
        if (typeof(key) == "string" || typeof (key) == "number") {
            this.GetItemByKey(key).remove(); //通过key键值删除
            deleteKeys.push(key);
        } else
            //通过行数据删除，有可能是一组数据
            $(key).each(function(i, e) {
                key = $(e).attr("key");
                deleteKeys.push(key);
                if($(e).attr("template") == null)
                    $(e).remove();
            });


        //遍历被删除的key键值队列，依次将他们从Data数据中删除掉
        $(deleteKeys).each(function (index, item) {
            //遍历数组,并将删除的键值从data数据中删除
            for (var i = 0; i < opts.data.length; i++) {
                if (opts.data[i][opts.key] == item) {
                    opts.data.splice(i, 1); //移除
                    i--;
                }
            }
        });
        
        //从新计数并设置背景颜色，主要是实现交替行效果
        _setTableBackGroundColor();
        
        //如果数据清空，将显示空行数据
        if (opts.data.length == 0) {
            setNoDataRow();
        }

        return true;
    };

    this.GetTrKeyValue = function(tr) {
        return $(tr, opts.table).attr("key");
    };

    //插入行
    this.InsertRow = _insert; //插入新行

    //设置当前行 
    this.Selected = function (key) {
        //获取之前的当前行
        var oldTr = this.GetCurrentRow();
        
        //新当前行
        var tr = GetItemByKey(key);
        _setSelectRow(tr);                  //切换当前
        opts.onSelectedRow(oldTr, tr);      //触发改变当前行事件
        return tr;
    };

    //获取当前行
    this.GetCurrentRow = function() {
        return $("tr.selected", opts.table);
    };

    //获取选择行
    this.GetSelectItems = function() {
        var items = null;

        //判断是否使用的checkbox的选择器，如果没有就使用当前行
        if (this.CheckAllBoxSelecter == null && this.GetCurrentRow().length > 0) {
            items = [this.GetCurrentRow()];   //当前行为选中行
        } else {
            //通过CheckAllBoxSelecter选择器来获取所有选中的行
            items = this.CheckAllBoxSelecter.GetCheckboxSelectedItem();
        }

        return items;
    };

    //移动行
    this.Move = function(isUp) {
        var tr = this.GetCurrentRow();

        if ($(tr).length == 0)
            return false;

        var oldTr;
        //是否是上移
        if (isUp) {

            oldTr = $(tr).prev();

            //上移操作
            //如果tr以上没有节点将不做任何操作
            if (oldTr.length == 0)
                return false;

            //上移节点
            oldTr.before($(tr));

        } else {

            oldTr = $(tr).next();

            //下移操作
            //如果tr以下没有节点将不做任何操作
            if (oldTr.length == 0)
                return false;

            //下移节点
            oldTr.after($(tr));
        }

        //促发move事件
        opts.onMoveRow(oldTr, tr, isUp);

        //重新计算背景颜色，以实现交替行样式
        _setTableBackGroundColor();

        if (tr.hasClass(opts.selectedCss)) {
            //删除以前的选择样式
            $(tr).removeClass(opts.rowCss);
            $(tr).removeClass(opts.alternatelyCss);
        }
    };

    //清除空行
    this.Clear = function () {
        _Clear();
        opts.data = [];
    };

    function _Clear() {
        //查找所有tr并删除
        $("tr", opts.table).each(function (i, e) {
            //只删除tbody中的行
            if ($(e).parent()[0].tagName.toLowerCase() == "tbody") {
                $(e).remove();
            }
        });


        //设置空行
        setNoDataRow();
    };


    function setNoDataRow() {
        
        //空行容器默认是要放在tbody中，如果没有tbody就最加在table最后一行
        var panel;
        
        //判读table是否有tbody
        if ($("tbody", opts.table).length > 0) {
            panel = $("tbody", opts.table);         //容器用tbody
        } else {
            panel = opts.table;                     //容器用table本身
        }

        //查找末班航
        var colspanNum = $("tr[template='true']", opts.table).children().length;
        
        //找到第一个td并去除中对齐和右对齐，"暂无数据"几个字要左对齐
        var panelTdClass = $("tr[template='true']", opts.table).children("td:first").attr("class");
        if (panelTdClass) {
            panelTdClass = panelTdClass.replace("align_c", "").replace("align_r", "");
        }
        
        //添加暂无数据行
        panel.append('<tr class="nodatarow p10_l"><td colspan="' + colspanNum + '" class="' + panelTdClass + ' align_l">暂无数据</td></tr>');
    }

    //根据数据插入到新行
    function _insert(dataSource) {

        var $that = this;

        var insertDataList = new Array();

        //遍历数据源
        $(dataSource).each(function(index, data) {

            //判读是否有空数据行nodedatarow，如果有就移除空数据行
            if (!!$(".nodatarow",opts.table)) {
                $(".nodatarow", opts.table).remove();    //移除空数据行
            }


            //执行用户的行绑定
            opts.beforeRowBind(data);

            //检查是否已有行
            if ($("tr[key='" + data[opts.key] + "']", opts.table).length > 0)
                return;

            //查找模板行
            var template = $("tr[template='true']", opts.table)[0].outerHTML;
            var templateRow = $that.FormatTemplate(template, data);        //格式数据模板  
            templateRow.attr("template", null);

            //由于模板行默认是隐藏的，所以再生产新行后要设置成现实
            templateRow.show();

            //行添加到table
            //如果有tbody就将tr插入到tbody中，如果没有就插入到最后
            var panel;
            if ($("tbody", opts.table).length > 0) {
                panel = $("tbody", opts.table);
            } else {
                panel = opts.table;
            }

            //追加到数据容器中
            panel.append(templateRow);

            //执行绑定后事件
            opts.afterRowBind(templateRow);

            //点击行事件,当点击一行时将这一行设置成当前行
            templateRow.click(function () {
                _setSelectRow($(this));
            });

            //从新计算行背景，主要是为了实现交替行
            _setTableBackGroundColor();

            //设置checkbox选择器
            $that.CheckAllBoxSelecter = UI.CheckAllbox(opts.table, ".checkall");

            insertDataList.push(data);
        });

        $(insertDataList).each(function (index, item) {
            for (var i = 0; i < opts.data.length; i++) {
                if (opts.data[i][opts.key] == item[opts.key])
                    return false;
            }
            opts.data.push(item);
        });
    };

    //设置当前项
    function _setSelectRow(tr) {

        if (opts.selectedCss == null) {
            return false;
        }

        //读取父节点
        var parentTable = $(tr).parent();
        //判断是否点击的是thead行，如果是就不做任何操作并退出
        if (parentTable[0].tagName.toLowerCase() == "thead")
            return false;

        //查找到父节点是table为止
        while (parentTable[0].tagName.toLowerCase() != "table") {

            //如果找到父节点都到了body表示DOM有错误
            if (parentTable[0].tagName.toLowerCase() == "body") {
                return false;
            }
            parentTable = parentTable.parent();
        }

        //如果有tbody将父节点设置tbody上
        if (parentTable.children("tbody").length > 0) {
            parentTable = parentTable.children("tbody");
        }

       //获取当前行，并移除当前行样式
        this.GetCurrentRow().each(function (i, e) {
            $(e).removeClass(opts.selectedCss);
        });

        //重新计算背景颜色，以实现交替行
        _setTableBackGroundColor();

        //删除以前的选择样式
        $(tr).removeClass(opts.rowCss);
        $(tr).removeClass(opts.alternatelyCss);

        //设置当前行
        $(tr).addClass(opts.selectedCss);

    };

    //设置table行的背景颜色
    function _setTableBackGroundColor() {
        //移除行背景样式
        $("tr", opts.table).removeClass(opts.rowCss);               //移除正常行样式
        $("tr", opts.table).removeClass(opts.alternatelyCss);       //移除交替行样式

        //设置数据行样式
        $("tr:odd", opts.table).addClass(opts.rowCss);              //设置奇数行样式
        $("tr:even", opts.table).addClass(opts.alternatelyCss);     //设置偶数行样式
        
        //设置鼠标响应行样式
        $("tr", opts.table).hover(function () {
            $(this).addClass(opts.hoverCss);    //添加响应行样式
        }, function () {
            $(this).removeClass(opts.hoverCss); //移除响应行样式
        });
        return true;
    };

    //设置loader的Mask层
    this.tableloader = {};
    
    //判断是否启用loader
    if (opts.showMask && !!opts.table)
        
        //创建遮罩
        this.tableloader = UI.Mask({
            obj: $(opts.table),
            zindex: 1000
        });
    else
        //移除遮罩
        this.tableloader.Remove = function () {
            return false;
        };


    //判读是否是ajax请table
    if (opts.ajaxOptons.url != null) {
        var that = this;
        
        //处理Ajax请求的data数据，将它们序列化成Json字符
        if (opts.ajaxOptons.data) {
            for (var key in opts.ajaxOptons.data) {
                if (opts.ajaxOptons.data[key] != null && typeof(opts.ajaxOptons.data[key]) == "object")
                    opts.ajaxOptons.data[key] = JSON.stringify(opts.ajaxOptons.data[key]);
            }
        }

        //成功回调函数，将用户的回调函数进行装饰，省我们想要的能去除Loader的新回调
        opts.ajaxOptons.success = function (data) {
            
            //判断是否有数据
            if (data == null) {
                return false;
            }
            
            //缓存数据源
            opts.data = data;
            
            //初始化数据
            _init(that);
            
            //移除loader
            tableloader.Remove();
            return true;
        };
        
        //错误事件
        opts.ajaxOptons.error = function(XMLHttpRequest, textStatus, errorThrown) {
            //统一的错误信息处理
            if ($.isFunction(opts.ajaxOptons.error)) {
                opts.ajaxOptons.error(XMLHttpRequest, textStatus, errorThrown);
            } else {
                //默认错误处理
                var error = JSON.parse(XMLHttpRequest.responseText);
                alert(error.ErrorMessage);
            }
            this.tableloader.Remove();
        };

        //组合参数
        var ajaxoptions = {};
        ajaxoptions = $.extend(opts.ajaxOptons, ajaxoptions);
        
        //Ajax请求数据
        $.ajax(ajaxoptions);
    }
    else {
        _init(this);
    }
    return this;
}