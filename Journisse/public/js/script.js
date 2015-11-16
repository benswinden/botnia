
$(document).ready(function(){

    // readFromFile('/public/source/Darwin_SelfFertilisation.txt');
    //readFromFile('/public/source/Darwin_Transmutation.txt');
    // readFromFile('/public/source/Carron_Narrative.txt');
    readFromFile('/public/source/Dampier_Voyage.txt');
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
    m = new markov(text, "string", /([.^\w]+ ){5}/g);

    var markovString = "";
    var outString = "";

    for (var i = 0; i < 1; i++) {

        markovString = m.gen(20);

        markovString = cleanNumbers(markovString);

        $('body').append("<p>1: " + markovString + "</p>");

        checkGrammar(markovString, function(results) {

            $('body').append("<p>2: " + results + "</p>");
        });
    }
}

// Remove all numbers
function cleanNumbers(text) {

    return text.replace(/[0-9]/g, '');
}

// Checks for simple spelling errors and grammar
function checkGrammar(text, callback) {

    $.post("/grammarCheck", {text:text}, function(data){

        if( data != null) {
            console.log("data found");
            callback(data);
        }
    });
}
