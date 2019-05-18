/*=author:陈建 左右上下移动控制=*/
var SellerScroll = function (options) {
    this.SetOptions(options);
    this.container = this.options.container //外DIV
    this.con_nner = this.options.con_nner; //内DIV
    this.lButton = this.options.lButton; //左按钮
    this.rButton = this.options.rButton; //右按钮
    this.oList = this.options.oList;  //ul
    this.showSum = this.options.showSum;  //一次滑动数
    this.showmun = this.options.showmun;  //显示条数
    this.direction = this.options.direction;  //方向控制

    this.iList = $("#" + this.options.oList + " > li");
    this.iListSum = this.iList.length;
    this.iListWidth = this.iList.outerWidth(true);
    this.moveWidth = this.iListWidth * this.showSum;
    this.dividers = Math.ceil(this.iListSum / this.showSum);
    //this.moveMaxOffset = (this.dividers - 1) * this.moveWidth; //左右最大滑动位置
    //this.moveMaxWidth = this.moveMaxOffset-((this.showmun -1)* this.iListWidth); //左右控制滑动位置


    this.iListHeight = this.iList.outerHeight(true);
    this.moveHeight = this.iListHeight * this.showSum;

    if (this.direction == "left") {
        this.LeftScroll();
        this.RightScroll();
    }
    else {
        this.TopScroll();
        this.DownScroll();
    }
};


SellerScroll.prototype = {
    SetOptions: function (options) {
        this.options = {
            container: "",
            con_nner: "",
            lButton: "",
            rButton: "",
            oList: "",
            showSum: 0,//一次滑动数
            showmun: 0,//显示个数
            direction: "",
            animateEffect: "fast"
        };
        $.extend(this.options, options || {});
    },

    //左右显示隐藏
    Arrowshow: function () {
        //var _this = this;
        var divwidth = $("#" + this.container).width();
        var ulwidth = $("#" + this.con_nner).width();
        var liwidth = this.iListSum * this.iListWidth;
        //alert(divwidth+","+ulwidth+","+liwidth);
        if (liwidth > divwidth) {
            $("#" + this.lButton).show();
            $("#" + this.rButton).show();
        } else {

            if (liwidth < divwidth) {
                $("#" + this.oList).css("left", "0px");
            }
            $("#" + this.lButton).hide();
            $("#" + this.rButton).hide();
        }
    },

    //左右控制滑动位置
    MoveMaxWidth_L: function () {
        var MaxOffset = this.iListSum * this.iListWidth; //左右最大滑动位
        var moveoff = (Math.floor($("#" + this.container).width() / this.iListWidth) * this.iListWidth);
        var xx = MaxOffset - moveoff;
        //alert(MaxOffset+","+moveoff+","+xx);
        return xx;

    },

    //上下控制滑动位置
    MoveMaxWidth_H: function () {

        var MaxOffset = (this.iListSum+1) * this.moveHeight; //左右最大滑动位
        return MaxOffset - ((Math.floor($("#" + this.con_nner).height() / this.iListHeight)) * this.iListHeight);
    },

    //获取left宽度
    ReturnLeft: function () {
        return isNaN(parseInt($("#" + this.oList).css("left"))) ? 0 : parseInt($("#" + this.oList).css("left"));
    },

    ReturnDown: function () {
        return isNaN(parseInt($("#" + this.oList).css("top"))) ? 0 : parseInt($("#" + this.oList).css("top"));
    },


    //向左滑动事件
    LeftScroll: function () {
				//if (this.dividers == 1) return;
        var _this = this, currentOffset;
        $("#" + this.lButton).mousedown(function () {
            _this.animateEffect = 50;
            _this.moveEventHandler = window.setInterval(function () { _this.moveLeftEvent(_this) }, 10);
        });

        $("#" + this.lButton).mouseup(function () {
            _this.animateEffect = "fast";
            _this.moveRemoveEventHandler(_this);
        }).mouseout(function () {
            _this.animateEffect = "fast";
            _this.moveRemoveEventHandler(_this);
        });
    },
    //向右滑动事件
    RightScroll: function () {
        //if (this.dividers == 1) return;
        var _this = this, currentOffset;
        $("#" + this.rButton).mousedown(function () {
            _this.animateEffect = 50;
            _this.moveEventHandler = window.setInterval(function () { _this.moveRightEvent(_this) }, 10);
        });

        $("#" + this.rButton).mouseup(function () {
            _this.animateEffect = "fast";
            _this.moveRemoveEventHandler(_this);
        }).mouseout(function () {
            _this.animateEffect = "fast";
            _this.moveRemoveEventHandler(_this);
        });
    },

    //向上滑动事件
    TopScroll: function () {
        if (this.dividers == 1) return;
        var _this = this, currentOffset;
        $("#" + this.lButton).mousedown(function () {
            _this.animateEffect = 50;
            _this.moveEventHandler = window.setInterval(function () { _this.moveUpEvent(_this) }, 10);
        });

        $("#" + this.lButton).mouseup(function () {
            _this.animateEffect = "fast";
            _this.moveRemoveEventHandler(_this);
        }).mouseout(function () {
            _this.animateEffect = "fast";
            _this.moveRemoveEventHandler(_this);
        });

    },
    //向下滑动事件
    DownScroll: function () {
        if (this.dividers == 1) return;
        var _this = this, currentOffset;
        $("#" + this.rButton).mousedown(function () {
            _this.animateEffect = 50;
            _this.moveEventHandler = window.setInterval(function () { _this.moveDownEvent(_this) }, 10);
        });

        $("#" + this.rButton).mouseup(function () {
            _this.animateEffect = "fast";
            _this.moveRemoveEventHandler(_this);
        }).mouseout(function () {
            _this.animateEffect = "fast";
            _this.moveRemoveEventHandler(_this);
        });

    },

    //时间控制器
    moveEventHandler: null,
    moveRemoveEventHandler: function (_this) {
        if (_this.moveEventHandler != null)
            window.clearInterval(_this.moveEventHandler);
    },
		//左滑动事件
    moveLeftEvent: function (_this) {
        var currentOffset;
        currentOffset = _this.ReturnLeft();
        if (currentOffset == 0) {
        } else {
            $("#" + _this.oList + ":not(:animated)").animate({ left: "+=" + _this.iListWidth }, _this.animateEffect);
        }
    },
		//右滑动事件
    moveRightEvent: function (_this) {
        var currentOffset;
        currentOffset = _this.ReturnLeft();

        if (Math.abs(currentOffset) < _this.MoveMaxWidth_L()) {
            $("#" + _this.oList + ":not(:animated)").animate({ left: "-=" + _this.iListWidth }, _this.animateEffect);
            //alert($("#"+_this.oList).css("left"));
        }
    },
		//上滑动事件
    moveUpEvent: function (_this) {
        var currentOffset;
        currentOffset = _this.ReturnDown();
        if (currentOffset == 0) {
        } else {
            $("#" + _this.oList + ":not(:animated)").animate({ top: "+=" + _this.moveHeight }, _this.animateEffect);
        }
    },
		//下滑动事件
    moveDownEvent: function (_this) {
        var currentOffset;
        currentOffset = _this.ReturnDown();
        if (Math.abs(currentOffset) < _this.MoveMaxWidth_H()) {
            $("#" + _this.oList + ":not(:animated)").animate({ top: "-=" + _this.moveHeight }, _this.animateEffect);
        }
    },
		
		tdscroll:function (event,delta){
			if (this.dividers == 1) return;
			var _this = this, currentOffset;
				if(delta>0){
					 _this.moveUpEvent(_this);
				}else{
					 _this.moveDownEvent(_this);
				}
		}

		
		
		

};
