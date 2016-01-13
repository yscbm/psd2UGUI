#target photoshop
#include "utils/json2.js"
//include("json2.js");

var STR_CHOOSE_DIR = "选择生成路径"
var defaultPath;// = "/E/worksapce_ps/png/"

var fileName = "text";
var txtFile;
var win;

var docParent;			//原始窗口
var exportOptions;		//输出格式设置
var dir;				//输出路径

main();

function main () {
	//输出设置
	exportOptions = new ExportOptionsSaveForWeb();
	exportOptions.PNG8 = false;
	exportOptions.format = SaveDocumentType.PNG;

	defaultPath = "E:\\worksapce_ps\\png\\"
	dir = Folder.selectDialog("选择生成路径",defaultPath);
	txtFile = new File(dir+"/" + app.activeDocument.name.split('.')[0] + ".json");   
	alert("txtFile  "+txtFile);
    if(txtFile.exists)
        txtFile.remove();        
    txtFile.encoding = "UTF8";
    txtFile.open("e", "TEXT");

    var ans={};
    //ans.name = "123";
    
	win = new Window("palette{text:'导出进度',bounds:[0,0,20,150],UIschedule:StaticText{text:'1',bounds:[10,10,100,230]},}");
	win.center();
	win.show();
	docParent = app.activeDocument;
	exportLayers(app.activeDocument,ans);

	var myObjectInJSON =  JSON.stringify(ans,null,"\t");
	//alert(myObjectInJSON );
	txtFile.write(myObjectInJSON);

	txtFile.close();
}

function exportLayers(layer,vv){
	var nameAndType = getNameAndType(layer.name)
	if(nameAndType.type == null)
	{
		alert(nameAndType.name +"没有定义类型");

		//return ;
	}

	if(layer.typename == "ArtLayer")
	{
		//alert(layer.name);
		//txtFile.write(layer.name);
		vv.name = nameAndType.name;
		vv.type = nameAndType.type;
		vv.scale = nameAndType.scale;
		vv.resource = nameAndType.resource;
		
		
		vv.child = null;
		win.UIschedule.text = layer.name;//显示进度，好像没啥用
		if(vv.text ==null)//不是字体，需要切图
		{
			toSmartAndOpen(layer);
		}
		

	}
	else
	{
		//alert(vv);
		vv.name = nameAndType.name;
		vv.type = nameAndType.type;
		vv.scale = nameAndType.scale;

		

		vv.child = new Array();
		//for(var i =0 ;i<layer.layers.length;i++)
		for(var i =layer.layers.length-1 ;i>=0;i--)
		{
			//txtFile.write(layer.layers[i].name);
			//alert(layer.layers[i].name);
			vv.child[layer.layers.length-1-i] ={};
			if(layer.layers[i].kind == LayerKind.TEXT)//文字
			{
				//alert(layer.layers[i].name);
				//alert(layer.layers[i].textItem);
				var textItem = layer.layers[i].textItem;
				vv.child[layer.layers.length-1-i].text = textItem.contents;//layer.layers[i].textItem;
				vv.child[layer.layers.length-1-i].font = textItem.font;
				vv.child[layer.layers.length-1-i].color = textItem.color.rgb;
				//vv.child[layer.layers.length-1-i].size = textItem.size.as('px');
				app.activeDocument.activeLayer = layer.layers[i];
				vv.child[layer.layers.length-1-i].size =Math.floor(getTextSize()) ;
				//alert(textItem.size);
			}
			
			exportLayers(layer.layers[i],vv.child[layer.layers.length-1-i]);
				

			
			//位置信息
			vv.child[layer.layers.length-1-i].bounds = new Array();
			for(var j = 0 ;j<4;j++ )
			{
				var aa;
				if(j==1||j==3)
				{
					aa = Math.floor (app.activeDocument.height.as('px')- layer.layers[i].bounds[j].as('px'));//ps坐标左上为起点
					vv.child[layer.layers.length-1-i].bounds[4-j] = aa;
				}
				else
				{
					aa = layer.layers[i].bounds[j].as('px');
					vv.child[layer.layers.length-1-i].bounds[j] = aa;
				}
				
			}
			
			//判断是否为文字
		}
		if(nameAndType.type == "btn")
		{
			vv.bounds = vv.child[0].bounds;
		}
	}
}
function toSmartAndOpen(layer){
	//alert("toSmartAndOpen" +layer.name);
	app.activeDocument.activeLayer = layer;
	try//变智能图层
   	{
      var idAction = stringIDToTypeID( "newPlacedLayer" );
      executeAction( idAction, undefined, DialogModes.NO );
   	}
   	catch(e)
   	{
      alert("toSmart   " + e)
   	}

    try//打开
   	{
      var idAction = stringIDToTypeID( "placedLayerEditContents" );
      executeAction( idAction, undefined, DialogModes.NO );
   	}
   	catch(e)
   	{
      alert("openSmart  " + e)
   	}
   	//信息存文件
   	//txtFile.write(app.activeDocument.activeLayer.name+"  \n");

	//导出
	//var fileOut = dir+"/" + app.activeDocument.activeLayer.name+".png";
	//var fileOut = dir+"/" +"123.png";
	//alert(fileOut);
	
   	var fileOut = new File(dir+"/" + app.activeDocument.activeLayer.name.split(',')[3]+".png");
   	app.activeDocument.exportDocument(fileOut, ExportType.SAVEFORWEB,exportOptions);
   	app.activeDocument.close(SaveOptions.DONOTSAVECHANGES);


   	app.activeDocument = docParent;
}
function getNameAndType(strName){
	var nameAndType = {};
	nameAndType.name = strName.split(',')[0].split('.')[0];
	nameAndType.type = strName.split(',')[1];
	nameAndType.scale = strName.split(',')[2];
	nameAndType.resource = strName.split(',')[3];

	return nameAndType;
}
function getTextSize(){  
	var ref = new ActionReference();  
	ref.putEnumerated( charIDToTypeID("Lyr "), charIDToTypeID("Ordn"), charIDToTypeID("Trgt") );   
	var desc = executeActionGet(ref).getObjectValue(stringIDToTypeID('textKey'));  
	var textSize =  desc.getList(stringIDToTypeID('textStyleRange')).getObjectValue(0).getObjectValue(stringIDToTypeID('textStyle')).getDouble (stringIDToTypeID('size'));  
	if (desc.hasKey(stringIDToTypeID('transform'))) {  
	            var mFactor = desc.getObjectValue(stringIDToTypeID('transform')).getUnitDoubleValue (stringIDToTypeID("yy") );  
	    textSize = (textSize* mFactor).toFixed(2);  
	    }  
	return textSize;  
} 