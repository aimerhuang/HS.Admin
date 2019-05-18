// JavaScript Document
	
//高级查询
//btn指点击弹出的button
//bomstr指右边的弹出框的id
//closebtn指右边弹出框上面右边的x,可以关闭弹出框
//deletebtn指弹出框下面的清除按钮
function searchbox(btn,bomstr,closebtn,deletebtn){
	var js_searchbox=$(bomstr);
	btn.bind("click",function(){	
			js_searchbox.animate({right:0 +"px"},300,function(){
	
					$(".case").one("click",function(){
						$(bomstr).animate({
						right: -263+"px"
						},300);
					});
					
				});			
			 return false;
	});
	
	closebtn.click(function(){
		$(bomstr).animate({
				right: -263+"px"
				},300);
		});
	deletebtn.click(function(){
		$("input",bomstr).val('');
		$("select",bomstr).val('');
		});
	}
//下面的方法是用于开发票的显示与隐藏
//bomstr指包含两个框的外面那个框的id
//actdomstr代表需要隐藏与显示的那些dom
//changestr代表触发改变的dom的标示，
function invoice(bomstr,actdomstr,changestr){
	$(changestr,bomstr).bind("change",function(){
		$(actdomstr,bomstr).toggleClass("hide");
		})
	}

