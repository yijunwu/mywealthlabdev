
var ActionType = { Zooming:1, Scrolling:2 };

var mouseDownX;
var mouseDownY;
var mouseUpX;
var mouseUpY;
var mouseDownSet=false;
var zooming=true;
var selectedChart;
var callingChart;
var is_ie = navigator.appName == 'Microsoft Internet Explorer';
var iebody=(document.compatMode && document.compatMode != "BackCompat")? document.documentElement : document.body;

//charts list
var chartList = "";
//Chart array
var pageCharts = new Array();
var scrollImages;
var images = new Array();
var scrollRect; //scroll region rect
var bAxisPos;
var scrollBars=new Array();
var action=ActionType.Zooming;

var zoomZone;

//*****************
// INIT
//*****************

this.getLocation = function(chObj) {

		var dynaObj=chObj;
		var overallTop=0; 
		var overallLeft=0;
		if (is_ie)
    {
      var objb = dynaObj.getBoundingClientRect();
      overallLeft += objb.left;
      overallTop += objb.top;
    }
    else
		while (dynaObj.offsetParent){
			overallTop=overallTop+dynaObj.offsetTop;
			overallLeft=overallLeft+dynaObj.offsetLeft; 
			dynaObj=dynaObj.offsetParent;
		}
		if ((chObj.style.position!="absolute") || (chObj.style.posTop==null)
												 || (chObj.style.top=="")){
		  chObj.style.posTop=overallTop;
		}
		if ((chObj.style.position!="absolute") || (chObj.style.posLeft==null)
												 || (chObj.style.left=="")){
		  chObj.style.posLeft=overallLeft;
		}
}

this.registerScrollSettings = function(toolType,chObjStr,imgCount,
                                       chartWidth,startPos,fileRoot,
                                       rLeft,rTop,rWidth,rHeight,bAxPos,mAction){                                      
  registerChart(toolType,chObjStr);
  
  if (document.getElementById(chObjStr)!=null){
    var _chObj = document.getElementById(chObjStr);
    _chObj.innerWidth=chartWidth;
    _chObj.startPos = startPos;
    _chObj.mAction = mAction;
  }
  
  bAxisPos=bAxPos;
  
  if (toolType==2){
    action=ActionType.Scrolling;
    scrollRect=new Array(rLeft,rTop,rWidth,rHeight);
    preload(imgCount,fileRoot,scrollRect);
  }
}

this.registerChart = function(toolType,chObjStr) {

  action=toolType;
  if (action==ActionType.Zooming){
    if (document.getElementById(chObjStr)!=null){
      var _chObj = document.getElementById(chObjStr);
      _chObj.oncontextmenu = blockEvent;
      _chObj.ondrag = blockEvent;
      _chObj.ondragstart = blockEvent;
      _chObj.onmousedown = function (event) { activateContent(chObjStr,true,event); };
      _chObj.onmouseup = function (event) { activateContent(chObjStr,false,event); };
//      _chObj.onmousemove = function (event) { zoomRect(chObjStr,event); };
    }
  }  
 
  var charts = chartList.split(';');
  var x;
  if (charts.length>0){
	  if (charts[0].length>0)
	  {
	    for (var i=0;i<charts.length;i++){
		    if (charts[i].indexOf(chObjStr)!=-1){
		      return;
		    }
	    }
	  }
  }
  chartList = (chartList.length>0) ? chartList+';' + chObjStr : chObjStr;
}

this.connectZoomLayer = function(chObjStr){

  if ((document.getElementById(chObjStr)!=null) 
      && (document.getElementById(chObjStr+'inlay')==null)){
    
	  var _chObj=document.getElementById(chObjStr);
	  
	  getLocation(_chObj);

	  var _layerElement = document.createElement('DIV');
	  var style = _layerElement.style; 
  	
	  style.visibility = "visible"; 
  	
	  if (action == ActionType.Zooming){
      style.height = '0px';
    }
    else if (action == ActionType.Scrolling){
      style.width = (scrollRect[2]-1)+'px';
      style.height = scrollRect[3]+'px';
      style.left = _chObj.style.posLeft + scrollRect[0] +"px"; 
      style.top = _chObj.style.posTop + scrollRect[1] + "px";
    	
      _layerElement.innerHTML = fillScrollArea(chObjStr);
	  }
	  
	  _layerElement.id=chObjStr+'inlay';

	  style.overflow = 'hidden';
	  style.zIndex = _chObj.style.zIndex + 10;      
	  style.backgroundColor = 'gray'; //background colour required
	  style.visibility='visible';   
	  style.filter='alpha(opacity=100)'; //IE
	  style.opacity=1.0;  //Mozilla
	  style.position = 'absolute';

    if (action == ActionType.Scrolling){
	    var scroller=makeScrollBar(_layerElement);
	    scrollBars[scrollBars.length]=scroller;
      document.body.appendChild(scroller);
      
      _chObj.oncontextmenu = blockEvent;
      _chObj.ondrag = blockEvent;
      _chObj.onmousemove = function (event) { handleDragScrollStop(chObjStr,event); };
      _chObj.onmouseup = function (event) { handleDragScrollStop(chObjStr,event); };
    }
    return _layerElement;
  }
  else
  {
	  return _layerElement;  
  }
}

this.getMap = function(id,i)
{
  return "#MAP"+id+""+i;
}

this.fillScrollArea = function(id)
{
  var _chObj=document.getElementById(id);
  var imgStr = "<div id='"+id+"inlayzone' onmouseup=\"handleDragScrollStop('"
                          +_chObj.id+"', event)\" onmousedown=\"handleDragScrollClick('"
                          +_chObj.id+"', event)\" onmousemove=\"handleDragIt('"
                          +id+"inlay',event)\"><table cellpadding=0 cellspacing=0><tr>";
  
  for(i=0; i<images.length; i++)
    imgStr = imgStr+"<td><IMG GALLERYIMG='no' USEMAP='"
                          +getMap(id,i)+"' parent='"+id+"inlayzone' class='inlayimg' id='"
                          +id+"img"+i+"' valign=top; Border=0></td>";

  imgStr = imgStr+"</tr></table>";
  imgStr = imgStr+"</div>";
  
  return imgStr;
}
		
//*****************
// SCROLLBAR
//*****************

var minEnd=0;
var maxEnd=1;
var vertScroll=0;
var horizScroll=1;
var scrollHeight=16;
var btnWidth=16;
var sliderWidth=32;

this.makeScrollBar = function(parent)
{
  var scrollBack = document.createElement('DIV');
  var style=scrollBack.style;
 
  scrollBack.ownerContentID = parent.id;
  scrollBack.ownerWidth = parent.style.width;
  scrollBack.contentElem = parent;
  scrollBack.index=scrollBars.length;
  scrollBack.contentScrollWidth = scrollBack.contentElem.scrollWidth;
  
  scrollBack.className = 'runner';
  
  scrollBack.scrollWrapper = null;
  scrollBack.upButton = null;
  scrollBack.dnButton = null;
  scrollBack.slider = null;
  scrollBack.buttonLength = 0;
  scrollBack.sliderLength = 0;
  scrollBack.scrollWrapperLength = 0
  scrollBack.dragZone = {left:0, top:0, right:0, bottom:0};
  
  var top=(parseInt(parent.style.top)*1)+bAxisPos-scrollHeight+3;
  style.top=top+'px';
  style.left=parent.style.left;
  style.height=scrollHeight+'px';
  style.width=parent.style.width;
  
  scrollBack.innerHTML = '<div></div>';

  style.zIndex = parent.style.zIndex + 10;      
  style.backgroundColor = 'gray';
  style.backgroundImage="url('"+scrollImages[2]+"')";
  style.filter='alpha(opacity=100)'; //IE
  style.opacity=1;  //Mozilla
  style.position = 'absolute';
  
  scrollBack.id=parent.id+'scroller';
  
  scrollBack.appendChild(addScrollEnd(scrollBack,minEnd,horizScroll));
  scrollBack.appendChild(addScrollEnd(scrollBack,maxEnd,horizScroll));  
  scrollBack.appendChild(addSlider(scrollBack,horizScroll));
     
  scrollBack.onmousedown = handleScrollClick;
  scrollBack.onmouseup = handleScrollStop;
  scrollBack.onmousemove = dragIt;
  scrollBack.oncontextmenu = blockEvent;
  scrollBack.ondrag = blockEvent;

  return scrollBack;
}

this.blockEvent = function(evt) {
  evt = (evt) ? evt : event;
  evt.cancelBubble = true;
  draggingEvent(evt);
  return false;
}

this.cancelEvent =function()
{
  window.event.returnValue = false;
}

this.draggingEvent = function(evt)
{
  evt = (evt) ? evt : event;
  
  if(window.event){
    evt.cancelBubble = true;
    if (action == ActionType.Zooming)
    {
      evt.returnValue = true;  //continue event proc ie
    }
    else
    {
      evt.returnValue = false; //need to block to permit dragscroll in IE
    }
  }else{
     evt = (evt) ? evt : event;
     evt.stopPropagation();
     evt.preventDefault();
  }
  evt.cancelBubble = true;
}

this.addScrollEnd = function(parent,end,orientation)
{
  var endObj=document.createElement('DIV');
  var style=endObj.style;
  
  if (orientation==1) //at the moment only horizontal supported
  {
	switch (end)
	{
	  case 0: 
	  {
		endObj.id=parent.id+'minEnd';
		endObj.className = 'decreaseVal';
		style.backgroundImage="url('"+scrollImages[0]+"')";
		style.left=0+'px';
		break;
	  }
	  case 1: 
	  {
		endObj.id=parent.id+'maxEnd';
		endObj.className = 'increaseVal';
		style.backgroundImage="url('"+scrollImages[1]+"')";
		style.left=(parseInt(parent.style.width)-btnWidth)+'px';
		break;
	  }
	}
    endObj.index=parent.index;
	style.top=0+'px';
	style.height=parent.style.height;
	style.backgroundColor = 'gray';
	style.opacity=1;
	style.filter='alpha(opacity=100)'; //IE
	style.width=btnWidth+'px';
	style.position = 'absolute';
  }
  
  return endObj;
}

this.addSlider = function(parent,orientation)
{
  var slider=document.createElement('DIV');
  if (orientation==1) //at the moment only horizontal supported
  {
    slider.id=parent.id+'Slider';
    slider.className = 'sliderRegion';
    slider.index=parent.index;
    parent.slider=slider;
    slider.style.zIndex = parent.style.zIndex + 10;
    slider.style.backgroundImage="url('"+scrollImages[3]+"')";
    var str=scrollImages[3]
    slider.style.left=btnWidth+'px';
	slider.style.top=0+'px';
	slider.style.height=parent.style.height;
	slider.style.backgroundColor = 'gray';
	slider.style.opacity=1;
	slider.style.filter='alpha(opacity=100)'; //IE
	slider.style.width=sliderWidth+'px';
	slider.style.position = 'absolute';
	slider.style.cursor='pointer';
  }
  
  return slider;
}

this.xxxstringPXValToInt = function(strVal)
{
  return strVal.replace('px','')*1.0;
}

this.preload = function (numImgs,imageName,scrollRect,chObjStr) 
{
	 var i = 0;
	 images = new Array();
	 // load name list
	 for(i=0; i<numImgs; i++) 
		loadChartImage(i,imageName);
}

this.loadChartImage = function (index, imageName)
{  
   images[index]=imageName+index;
//   imageObjs[index] = new Image();
//   imageObjs[index].src=images[index];
}

this.loadScrollImages = function (scrollURLs)
{
  scrollImages = scrollURLs;
  // start preloading
  for(i=0; i<scrollImages.length; i++) 
	loadImage(i,scrollImages[i]);
}
	
this.loadImage = function (index, imageName)
{  
   //create object
   imageObj = new Image();
   imageObj.src=imageName;
}

this.setScrollOffset = function (index,id)
{
  var _chObj=document.getElementById(id);
  var pos=1;
  if (_chObj.startPos>0)
    pos=(_chObj.startPos*1.0/100.0)*(_chObj.innerWidth*1.0);
  scrollEngaged = true;
  setTimeout("scrollBy(" + index + ", " + Math.round(pos) + ")", 10);
}

this.initLayers = function ()
{
  if (action == ActionType.Scrolling)
  {
      loadScrollImages(scrollStrings);
      var x;
      if (chartList.indexOf(';')!=-1)
      { 
	    pageCharts = chartList.split(';');
	    for (x in pageCharts)
	    { 
	      if (pageCharts[x].length>0) 
	      {
		    document.body.appendChild(connectZoomLayer(pageCharts[x]));
		    setImages(pageCharts[x]);
		    setScrollOffset(x,pageCharts[x]);
	      }
	    } 
      } 
      else
      { 
	    document.body.appendChild(connectZoomLayer(chartList));
	    setImages(chartList);
	    setScrollOffset(0,chartList);
      }
  }
}

this.setImages = function(id)
{
  if (document.images)
    {
      for(i=0; i<images.length; i++)
      {
        var img=document.getElementById(id+"img"+i);
        img.src= images[i];
      }
    }
}

/***************************
    EVENT HANDLING
****************************/
// Global variables
var scrollEngaged = false;
var scrollInterval;
var scrollBars = new Array();

var diff;
var offsetX;
var offsetY;
var engaged=false;
var dragEngaged=false;

this.dragIt = function(evt) {
  evt = (evt) ? evt : event;
  var target = (evt.target) ? evt.target : evt.srcElement;
  var x, y, width, height;
  if ((target.className=="sliderRegion") && (engaged==true)){
  
    if (evt.pageX){
      offsetX = evt.pageX;
      offsetY = evt.pageY;
    }else{
      offsetX = evt.clientX;
      offsetY = evt.clientY;
    }
    
    var sl=document.getElementById(target.id);
    getLocation(sl);
    barLeft=parseInt(sl.offsetParent.style.left)
    var pos=offsetX-barLeft-diff;
    var scroller = scrollBars[sl.index];
    var barLength = parseInt(scroller.style.width) - (btnWidth*2);
    if ((pos>btnWidth-1) && (pos<(barLength+1+btnWidth-sliderWidth)))
    {
      sl.style.left=pos+'px';
      var percentPos=(pos-btnWidth)/(barLength-sliderWidth-1);
      scroller.contentElem.scrollLeft=(scroller.contentElem.scrollWidth-(parseInt(scroller.style.width)))*percentPos;
    }
  }
}

this.getParentInlay=function(target){
  
  var node=target;
  while ((node.offsetParent!=null)){
    
    node=node.offsetParent;
    if (node.className=='inlayzone') return node;
  }
  return node;
}

var lastDragX=null;
var resetCount=0;
var debugMouseDown=false;
var debugNextMouseMove=false;

this.handleDragIt = function(inlay,evt) {

  evt = (evt) ? evt : event;
  var inlayElement=document.getElementById(inlay);
  var x, y, width, height;
  if (dragEngaged==true){
    
    if (evt.pageX){
      offsetX = evt.pageX;
      offsetY = evt.pageY;
    }else{
      offsetX = evt.clientX;
      offsetY = evt.clientY;
    }
    
    if (lastDragX!=null){
      inlayElement.scrollLeft=inlayElement.scrollWidth
                              -((inlayElement.scrollWidth-inlayElement.scrollLeft)+(offsetX-lastDragX));
    }  
    else
    {
      inlayElement.scrollLeft=inlayElement.scrollWidth-offsetX;
      resetCount++;
    }
    
    draggingEvent(evt);
  }
      
  lastDragX=offsetX;
  updateSlider(0);
}

// onmouse up handler
this.handleDragScrollStop = function(chObjStr,evt) {
    var inlayzone=document.getElementById(chObjStr+'inlayzone');
    inlayzone.style.cursor='default';
    scrollEngaged = false;
    dragEngaged = false;
    draggingEvent(evt);
}

this.handleDragScrollClick = function(chObjStr,evt) {

    evt = (evt) ? evt : event;
    if (evt.pageX){
      offsetX = evt.pageX;
    }else{
      offsetX = evt.clientX;
    }
    var inlayzone=document.getElementById(chObjStr+'inlayzone');
    if (document.getElementById(chObjStr).mAction==1){
      inlayzone.style.cursor='pointer';
      dragEngaged=true;
    }
    
    debugMouseDown=true;
    lastDragX=offsetX;
    draggingEvent(evt);
}

this.engage = function(evt) {
  evt = (evt) ? evt : event;
  var target = (evt.target) ? evt.target : evt.srcElement;
  if (target.className=="sliderRegion") 
  {
    engaged=true;
    if (evt.pageX) 
    {
      offsetX = evt.pageX;
      offsetY = evt.pageY;
    } 
    else 
    {
      offsetX = evt.clientX;
      offsetY = evt.clientY;
    }
    var sl=document.getElementById(target.id);
    getLocation(sl);
    
    var sliderLeft=(parseInt(sl.offsetParent.style.left)*1.0)+(parseInt(sl.style.left)*1.0);
    diff=offsetX-sliderLeft;
   }
}

this.handleScrollStop = function() {
    scrollEngaged = false;
    engaged = false;
}

var ix=0;

this.handleScrollClick = function(evt) {
    engage(evt);
    ix++;
    var fontSize, contentHeight;
    evt = (evt) ? evt : event;
    var target = (evt.target) ? evt.target : evt.srcElement;
    target = (target.nodeType == 3) ? target.parentNode : target;
    var index = target.index;
    fontSize=0;
    switch (target.className) {
        case "increaseVal" :
            scrollEngaged = true;
            scrollInterval=setInterval("scrollBy(" + index + ", 1)", 5);
            return false;
            break;
        case "decreaseVal" :
            scrollEngaged = true;
            scrollInterval=setInterval("scrollBy(" + index + ", -1)", 5);
            return false;
            break;
        case "sliderRegion" :
            break;
        case "runner" :
            scrollEngaged = true;
            var evtX = (evt.offsetX) ? evt.offsetX : ((evt.layerX) ? evt.layerX : -1);
            if (evtX >= 0) {
                var pageSize = parseInt(scrollBars[index].ownerWidth)*-1;
                var sliderElemStyle = scrollBars[index].slider.style;
                if (evtX > (parseInt(sliderElemStyle.left) + 
                    scrollBars[index].sliderLength)) {
                    pageSize = -pageSize;
                }
                scrollInterval = setInterval("scrollBy(" + index + ", " + pageSize + ")", 100);
                evt.cancelBubble = true;
                return false;
            }
    }
    return false;
}
var ctr=0;
this.scrollBy = function(index, val) {
    var scroller = scrollBars[index];
    var barLength = parseInt(scroller.style.width) - (btnWidth*2);
    
    inlayWidth=parseInt(scroller.style.width);
    
    if (scrollEngaged)
    {
      if (val>0)
      {
        if ((scroller.contentElem.scrollWidth-val)>(scroller.contentElem.scrollLeft+inlayWidth))
        {
          scroller.contentElem.scrollLeft=scroller.contentElem.scrollLeft+val;
          ctr++;
        }
        else  
        {
          scroller.contentElem.scrollLeft=scroller.contentElem.scrollWidth-inlayWidth;
          clearInterval(scrollInterval);
        }
      }
      else
        scroller.contentElem.scrollLeft=scroller.contentElem.scrollLeft+val;
      
      updateSlider(index);
    }
    else
      clearInterval(scrollInterval);
}

// Position slider after scrolling by arrow/page region
function updateSlider(index) {
    var scroller = scrollBars[index];
    var barLength = parseInt(scroller.style.width) - (btnWidth*2);
    var inlayZoneWidth = scroller.contentElem.scrollWidth;
    var inlayZoneDisp = scroller.contentElem.scrollLeft+(parseInt(scroller.style.width)*1)-(btnWidth*2);
    var percentDisp = scroller.contentElem.scrollLeft/(scroller.contentElem.scrollWidth-(parseInt(scroller.style.width)));
    if (percentDisp>1) percentDisp=1;
    
    if (scroller.contentElem.scrollLeft==0)
      scroller.slider.style.left=btnWidth+'px';
    else if (scroller.contentElem.scrollLeft>=(scroller.contentElem.scrollWidth-parseInt(scroller.style.width)))
    {
      scroller.slider.style.left  = parseInt(scroller.style.width)-(btnWidth+sliderWidth) + 'px';
    }
    else
    {
      scroller.slider.style.left  = (btnWidth+Math.round(percentDisp*(barLength-sliderWidth))) + 'px';
    }  
}
//*********************
