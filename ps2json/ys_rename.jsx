
var widgets = new Array();

main();

	

function main () {

	// var dlgStr  ="dialog{text:'jjb',bounds:[0,0,400,300],\
	// 	UINameText:Button{text:'jjb',bounds:[10,10,100,230]},\
	// 	UINameText2:Button{text:'jjb2',bounds:[170,60,180,70]},\
	// 	OkBtn:EditText{text:'jjbb',bounds:[10,200,100,30]},\
	// }"
	// var win = new Window(dlgStr);
	// win.center();
	// win.show();
	//alert("yes1");

	if(app.activeDocument==0){

		alert("没有PSD打开");
	}

	new DialogBuilder().bulid();

}

function DialogBuilder() {
	widgets[0]=new UIName();
	widgets[1]=new UIType();
	widgets[2]=new UIScale();
	widgets[3]=new UIResource();
	widgets[4]=new ButtonOK();
	widgets[5]=new ButtonCancel();

	this.bulid = function(){
		var dlgStr = "dialog{text:'重命名',bounds:[0,0,400,300],";
		for (var i = widgets.length - 1; i >= 0; i--) {
			dlgStr +=widgets[i].bulid();
		};
		dlgStr+="}";
		var win = new Window(dlgStr);
		for (var i = widgets.length - 1; i >= 0; i--) {
			dlgStr +=widgets[i].onCreate(win);
		};



		win.center();
		win.show();
	}
}
function UIName() {
	this.bulid = function () {
		var str="UINameText:StaticText{text:'UI命名',bounds:[10,10,100,40],},";
		str+="UINameEditText:EditText{text:'name',bounds:[10,30,300,50],}";
		return str;
	}
	this.onCreate = function(win){
		var name = app.activeDocument.activeLayer.name.split(',');
		win.UINameEditText.text = name[0];
		//var kv = name[0].split('_');
		
		// var typesDictionary = widgets[1].getTypesDictionary();

		// if(typesDictionary[kv[0]]!=null)
		// {
		// 	win.UINameEditText.text = name[0].substr (kv[0].length + 1);
		// 	return;
		// }
		// win.UINameEditText.text = name[0];
	}
}

function UIType(){

	var types = new Array();
    var typesDictionary = {};
    var id = 0;
    types[id]="面板(p)";
    typesDictionary["p"] = types[id];
    typesDictionary[types[id++]] = "p";

    types[id]="背景(bg)";
    typesDictionary["bg"] = types[id];
    typesDictionary[types[id++]] = "bg";

    types[id]="按钮(btn)";
    typesDictionary["btn"] = types[id];
    typesDictionary[types[id++]] = "btn";

	types[id]="按钮idle(idle)";
    typesDictionary["idle"] = types[id];
    typesDictionary[types[id++]] = "idle";

    types[id]="按钮pressed(pressed)";
    typesDictionary["pressed"] = types[id];
    typesDictionary[types[id++]] = "pressed";

	types[id]="按钮select(select)";
    typesDictionary["select"] = types[id];
    typesDictionary[types[id++]] = "select";

    types[id]="点9图(9)";
    typesDictionary["9"] = types[id];
    typesDictionary[types[id++]] = "9";

    types[id]="文字(txt)";
    typesDictionary["txt"] = types[id];
    typesDictionary[types[id++]] = "txt";

    types[id]="横进度条(prgh)";
    typesDictionary["prgh"] = types[id];
    typesDictionary[types[id++]] = "prgh";

    types[id]="竖进度条(prgv)";
    typesDictionary["prgv"] = types[id];
    typesDictionary[types[id++]] = "prgv";

    types[id]="圆进度条(prgr)";
    typesDictionary["prgr"] = types[id];
    typesDictionary[types[id++]] = "prgr";

    types[id]="滑动列表(list)";
    typesDictionary["list"] = types[id];
    typesDictionary[types[id++]] = "list";




	this.bulid = function () {
		var str="UITypeText:StaticText{text:'UI类型',bounds:[10,60,100,90],},";
		str+="UITypeList:DropDownList{bounds:[10,80,300,100],properties: {items: ["
		for(var i = 0; i < types.length; i++) {
            str += "'" + types[i] + "'";
            if(i < types.length - 1)
                str += ',';
        }
		str+="]}}";
		return str;
	}
	this.onCreate = function(win){
		win.UITypeList.selection =0;
	}
	this.getVal = function(text){
		return typesDictionary[text];
		// for(var i = 0; i < items.length; i++) {
  //           if(items[i].text == text)
  //               return items[i].value;
  //       }
	}
	this.getTypes = function(){
		return types;
	}
	this.getTypesDictionary = function(){
		return typesDictionary;
	}
}

function UIScale(){
	var scales = new Array();
    var scalesDictionary = {};
    scales[0] = "按比例";
    scalesDictionary[scales[0]] = "ch";
    scalesDictionary["ch"] =scales[0] ;

    scales[1] = "固定比例";
    scalesDictionary[scales[1]] = "st";
    scalesDictionary["st"] =scales[1] ;


	this.bulid = function () {
		var str="UIScaleText:StaticText{text:'UI比例',bounds:[10,110,100,140],},";
		str+="UIScaleList:DropDownList{bounds:[10,130,300,150],properties: {items: ['按比例','固定比例']}}";
		return str;
	}
	this.onCreate = function(win){
		win.UIScaleList.selection =0;
	}
	this.getVal = function(text){
		return scalesDictionary[text];
	}
}

function UIResource() {
	this.bulid = function () {
		var str="UIResourceText:StaticText{text:'UI图片资源',bounds:[10,160,100,190],},";
		str+="UIResourceEditText:EditText{text:'resource',bounds:[10,180,300,200],}";
		return str;
	}
	this.onCreate = function(win){
		var name = app.activeDocument.activeLayer.name.split(',');
		if(name[3] == null)
		{
			win.UIResourceEditText.text = "";
		}
		else
		{
			win.UIResourceEditText.text = name[3];
		}
		
		//var kv = name[0].split('_');
		
		// var typesDictionary = widgets[1].getTypesDictionary();

		// if(typesDictionary[kv[0]]!=null)
		// {
		// 	win.UINameEditText.text = name[0].substr (kv[0].length + 1);
		// 	return;
		// }
		// win.UINameEditText.text = name[0];
	}
}
function ButtonOK(){
	this.bulid = function () {
		var str="ButtonOK:Button{text:'OK',bounds:[260,270,320,290],},";
		return str;
	}
	this.onCreate = function(win){
		win.ButtonOK.onClick = function() {

            if(win.UINameEditText.text == "") {
            	alert("命名不能为空！");
        	}
        	else if(win.UITypeList.selection == null){
        		alert("类型不能为空！");
        	}
        	else{
        		var strName =  win.UINameEditText.text+","+widgets[1].getVal(win.UITypeList.selection.text)+","
        		+widgets[2].getVal(win.UIScaleList.selection.text)+","+win.UIResourceEditText.text;

        		if(widgets[1].getVal(win.UITypeList.selection.text)=="bg" && win.UIResourceEditText.text == "")//若bg没命名图片资源默认为名字
        		{
        			strName += win.UINameEditText.text;
        		}
        		app.activeDocument.activeLayer.name =strName;
        		win.close(1);
        	}
        }
	}
}
function ButtonCancel(){
	this.bulid = function () {
		var str="ButtonCancel:Button{text:'Cancel',bounds:[80,270,140,290],},";
		return str;
	}
	this.onCreate = function(win){
		win.ButtonCancel.onClick = function() {
            win.close(1);
        }
	}
}


