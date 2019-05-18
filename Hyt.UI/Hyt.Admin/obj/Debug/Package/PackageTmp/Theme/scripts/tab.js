// 基于JQ的tab切换插件
// 创建一个闭包     
(function($) {     
  //插件主要内容     
  $.fn.tab = function(options) {     
    // 处理默认参数   
    var opts = $.extend({}, $.fn.tab.defaults, options);     
    return this.each(function() {  
		var $this=$(this),
		$tabNavObj=$(opts.tabNavObj,$this),
		$tabContentObj=$(opts.tabContentObj,$this),
		$tabNavBtns=$(opts.tabNavBtn,$tabNavObj),
		$tabContentBlocks=$(opts.tabContent,$tabContentObj);
		$tabNavBtns.bind(opts.eventType,function(){
			var $that=$(this),
			_index=$tabNavBtns.index($that);
			$that.addClass(opts.currentClass).siblings(opts.tabNavBtn).removeClass(opts.currentClass);
			$tabContentBlocks.eq(_index).show().siblings(opts.tabContent).hide();
			//扩展方法传递
			if(opts.onActiveTab != null && typeof(opts.onActiveTab) == "function"){
					opts.onActiveTab(_index,$that);
			}
			
		}).eq(0).trigger(opts.eventType);
    }); 
    // 保存JQ的连贯操作结束
  };
	//插件主要内容结束
    
  // 插件的defaults     
  $.fn.tab.defaults = {     
    tabNavObj:'.tabNav',
		tabNavBtn:'li',
		tabContentObj:'.tabContent',
		tabContent:'.list',
		currentClass:'menuon',
		eventType:'click',
		onActiveTab: null
  };     
// 闭包结束     
})(jQuery); 