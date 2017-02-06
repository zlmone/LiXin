// JavaScript Document

function  getHeight(){
	var all_h = $(window).height();
	var top_h = $("#head").outerHeight();
	var foot_h = $("#footer").outerHeight();
	var main_h = $("#main").outerHeight();
	var new_h = all_h - top_h -foot_h;
	if(main_h < new_h ){
		$("#main").outerHeight(new_h);
	}
	else{
		$("#main").outerHeight(main_h);
	}
			
	var left_h = $(".main-l").outerHeight();
	var right_h = $(".main-r").outerHeight();
	if(left_h > new_h){
		$(".main-l").outerHeight(left_h);
	}
	if(left_h < new_h){
		$(".main-l").outerHeight(new_h);
	}
			
}

window.onload = function() {
    window.onresize = getHeight;
    getHeight();
};