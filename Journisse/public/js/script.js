
$(document).ready(function(){

    // readFromFile('/public/source/Darwin_SelfFertilisation.txt');
    readFromFile('/public/source/Darwin_Transmutation.txt');
});


function readFromFile(filePath, callback) {

    var stringFromFile;

    // Retrieve data
    $.post("/getStringFromFile", {filePath:filePath}, function(data){

        // Callback
        if( data != null) {

            for (var i = 0; i < data.length; i++){

                stringFromFile += data[i];
            }

            createChain(stringFromFile);
        }
    });

}

function createChain(text) {

    var m;

    // x = new markov(text, type, regex);
    m = new markov(text, "string", /([.^\w]+ ){2}/g); //sherlock holmes corpus

    var markovString = "";
    var outString = "";

    for (var i = 0; i < 5; i++) {

      markovString += "<p>" + m.gen(20) + "</p>";
    }

    $('body').append(markovString);
}
