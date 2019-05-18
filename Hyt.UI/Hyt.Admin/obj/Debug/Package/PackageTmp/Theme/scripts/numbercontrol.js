// 基于JQ的数字增加减少
// 创建一个闭包     
(function($) { 
  //插件主要内容     


  $.fn.numbercontrol = function(options) { 
	
	  //模板方法
		Template = function(){
			/*
				<div class="number_btn"><button title="减少" class="btn cutbtn wd28 btn_ht26"><span class="icon_minus_sign"></span></button><input type="text" class="first" value="110"/><button title="增加" class="btn addbtn wd28 btn_ht26"><span class="icon_plus_sign"></span></button></div>   </td>
			*/    
		}
    // 处理默认参数   
    var opts = $.extend({}, $.fn.numbercontrol.defaults, options);  
		   
    return this.each(function() {  
		var $this=$(this);
		//$numberboxObj=$(opts.numberboxObj,$this), 再次选择
		if($this.attr("type").toLowerCase() != "text")
			return false;
		
		$numberboxObj=$this.parent("td");
		//设置和获取参数
		var template = Template.getMultiLine();
		$numberboxObj.prepend(template);
		//template = template.replace("{x}",$numberboxObj.html());
		$numberboxObj.children(".number_btn").children("input").replaceWith($this);
  	$minNumber=opts.minNumber;
		$maxNumber=opts.maxNumber;
		$step=opts.step;
		$cutActiveNum=opts.cutActiveNum;
		$addActiveNum=opts.addActiveNum;
		var cutbtn=$numberboxObj.children(".number_btn").children(opts.leftbtn);
		var addbtn=$numberboxObj.children(".number_btn").children(opts.rightbtn);
			//减少数方法
			cutbtn.bind(opts.eventType,function(){
				var $that=$(this);
				var inputvar=parseInt($that.next("input").val());
				if(!isNaN(inputvar) && inputvar > $minNumber){
						$that.next("input").val(inputvar-$step);
					}
				//扩展方法传递
				if($cutActiveNum != null && typeof($cutActiveNum) == "function"){
						$cutActiveNum($that.next("input"));
				}
			});
			//增加数方法
			addbtn.bind(opts.eventType,function(){
				var $that=$(this);
				var inputvar= parseInt($that.prev("input").val());
				if(!isNaN(inputvar) && inputvar < $maxNumber){
						$that.prev("input").val(inputvar+$step);
					}
				//扩展方法传递
				if($addActiveNum != null && typeof($addActiveNum) == "function"){
						$addActiveNum($that.prev("input"));
				}
					
			});
			
		
    }); 
    // 保存JQ的连贯操作结束
  };
	//插件主要内容结束
    
  // 插件的defaults     
  $.fn.numbercontrol.defaults = {
		numberboxObj:".number_btn",
		eventType:'click',
		leftbtn:".cutbtn",
		rightbtn:".addbtn",
		minNumber : 0,
		maxNumber : 5,
		cutActiveNum:null,
		addActiveNum:null,
		step:1
  };     
// 闭包结束     
})(jQuery); 