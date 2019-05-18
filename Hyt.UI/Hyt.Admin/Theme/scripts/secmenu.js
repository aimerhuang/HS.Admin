// JavaScript Document
$(document).ready(function(){
	/*=====二级菜单鼠标滚动=====*/
	var temp="<p class='son_topscroll'><a></a></p><p class='son_downscroll'><a></a></p>";
	var NavsonObj=$('#navson');
	NavsonObjcon=NavsonObj.children("div");


		



	NavsonObjcon.children('ul').each(function(index, element) {
		var lilen=$(this).children('li').length;
		
		
		
		if(lilen*42>NavsonObj.height()){
			$(this).before(temp);
			Click_UpScroll($(this));
			Click_DownScroll($(this));
			
			//alert($(this).prev('.son_topscroll').length);
			

			$(this).mousewheel(function(event,delta){
				var moveHeight=(lilen+2)*42-NavsonObj.height();
				//alert(moveHeight);
				
				if(delta<0){
					son_moveDown($(this),moveHeight);
				}else{
					son_moveUp($(this));
				}
			});
		};		
		
		

		

    });
	//滑动高度
	function son_solltop(Obj){
	return isNaN(parseInt(Obj.css("top"))) ? 0 : parseInt(Obj.css("top"));
	}
	//上滑动事件
    function son_moveUp(Obj) {
        var currentOffset;
        currentOffset = son_solltop(Obj);
        if (currentOffset == 0) {
        } else {
            Obj.css({ top: "+=" + 42 });
        }
    }
		//下滑动事件
    function son_moveDown(Obj,H) {
        var currentOffset;
        currentOffset = son_solltop(Obj);
        if (Math.abs(currentOffset) < H ) {
            Obj.css({ top: "-=" + 42 });
        }
    }



    //时间控制器
    var moveEventHandler=null;
    function moveRemoveEventHandler(_this) {
        if (moveEventHandler != null)
            window.clearInterval(_this);
    }



    //向上点击滑动事件
    function Click_UpScroll(Obj) {
        Obj.prev().prev().mousedown(function () {

            moveEventHandler = window.setInterval(function () { son_moveUp(Obj) }, 100);
        });

        Obj.prev().prev().mouseup(function () {
            moveRemoveEventHandler(moveEventHandler);
        }).mouseout(function () {
            moveRemoveEventHandler(moveEventHandler);
        });

    }
    //向下点击滑动事件
    function Click_DownScroll(Obj) {
		var lilen=Obj.children('li').length;

        Obj.prev().mousedown(function () {
			var H=(lilen+2)*42-NavsonObj.height();
            moveEventHandler = window.setInterval(function () { son_moveDown(Obj,H) }, 100);
        });

        Obj.prev().mouseup(function () {
            moveRemoveEventHandler(moveEventHandler);
        }).mouseout(function () {
            moveRemoveEventHandler(moveEventHandler);
        });

    }



	
});