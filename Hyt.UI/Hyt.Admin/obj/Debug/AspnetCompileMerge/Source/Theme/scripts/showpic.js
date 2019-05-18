//图片弹出
var strFrameCity;  //存放层的HTML代码
document.writeln('<iframe id=cityLayer name=cityLayer scrolling=no frameborder=0 style="width: 100%; height: 100%;position: absolute;z-index: 9998;filter:Alpha(Opacity=90,Style=0); opacity:90; display: none;"></iframe>');

var framestyle = document.getElementById("cityLayer").style;;
function showp(tt, link) //主调函数show,value
{
    if (arguments.length > 2) { alert("对不起!传入本控件的参数太多!"); return; }
    if (arguments.length == 0) { alert("对不起!您没有传回本控件任何参数!"); return; }
   
    var ttop = tt.offsetTop;     //TT控件的定位点高
    var toftop = tt.offsetTop-20;
   // var tbottom = tt.offsetBottom;
    var thei = tt.clientHeight;  //TT控件本身的高
    var tleft = tt.clientWidth+10; // tt.offsetLeft;    //TT控件的定位点宽
    var ttyp = tt.type;          //TT控件的类型

 
    while (tt = tt.offsetParent) { ttop += tt.offsetTop; tleft += tt.offsetLeft; }
    var topd = (ttyp == "image") ? ttop - thei : ttop - thei + 6;
   
    framestyle.top = toftop + "px";
    framestyle.left = tleft + "px";

    framestyle.width = 600 + "px";

    var inhtml = "<img src='" + link + "' width='" + framestyle.width + "' alt=''/> ";
    strFrameCity = '<div id="showmenu">' + inhtml + '</div>';

    framestyle.display = '';
    window.frames.cityLayer.document.writeln(strFrameCity);
    window.frames.cityLayer.document.close();  //解决ie进度条不结束的问题
}

function closeLayerCity()               //这个层的关闭
{
    framestyle.display = "none";
}