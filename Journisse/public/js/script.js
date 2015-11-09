
$(document).ready(function(){

    getStringFromFile("/source/test.txt");
});


function getStringFromFile(filePath) {

    // Retrieve data
    $.post("/getStringFromFile", { filePath: filePath }, function(data){

        // Callback
        if( data != null) {

            console.log("Data returned   " + data.length);

            for (var i=0;i<data.length;i++){
                $('body').append(data[i] + "</br>");
            }
        }
    });
}
