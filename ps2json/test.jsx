
main();

function main () {
var bb = app.activeDocument.activeLayer;
var amTextSize = getTextSize();   
alert(amTextSize);
//var dd=app.activeDocument;
//alert(dd.height.as("px"));

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