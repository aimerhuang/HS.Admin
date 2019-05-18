// JavaScript Document   ---zhaoli
//specil_css指不同的弹出提示的区分不同的css用不同的css可以出现不同的提示信息，
//word表示弹出的提示信息，指中间显示的中文信息.
//此处弹出的控制是动态弹出，先用透明度隐藏，然后动态移除节点，因此这里的两个setTimeout方法中的时间必须后者大于前者
function tips(specil_css,word){
	var html=[];
	html.push('<div class="tips '+specil_css+'" id="JS_tip">');
    	html.push('<a href="javascript:;" onclick="tip_delete();">&times;</a><span class="icons"></span>'+word+'');
    html.push('</div>');
	var str=html.join('');
	$("body").append(str);
	if($("#JS_tip")){
		setTimeout(function(){
			$("#JS_tip").animate({
				opacity:0
				},500);
			},2500);
		setTimeout(function(){
			$("#JS_tip").remove();
			},3000);
		}
	}
function tip_delete(){
	if($("#JS_tip")){
		$("#JS_tip").animate({
			opacity:0
			},500);
		setTimeout(function(){
			$("#JS_tip").remove();
			},500);
		}
	}