// JavaScript Document

function  getHeight(){
    var h = document.body.clientHeight; //$(window).height();
	$("#lay-bg").height(h);
	$("#lay-bg-img").height(h);
			
}
function  getWidth(){
	var w = $(window).width();
	$("#lay-bg").width(w);
}

window.onresize = function() {
    //	window.onresize = getHeight; 
    //	window.onresize = getWidth; 
    getHeight();
    getWidth();
};