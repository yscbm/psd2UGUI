#target photoshop
#include "utils/json2.js"


main();

function main () {
	var fileOut =new File( app.activeDocument.path+"/1234.png");
	//alert(fileOut);
	var exportOptions = new ExportOptionsSaveForWeb();
	exportOptions.PNG8 = true;
	exportOptions.format = SaveDocumentType.PNG;
   	app.activeDocument.exportDocument(fileOut, ExportType.SAVEFORWEB,exportOptions);
   	//app.activeDocument.close(SaveOptions.DONOTSAVECHANGES);
}



