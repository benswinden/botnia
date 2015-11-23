
var plantNamePrefixString = "Common,Rare,Medium,Large,Small,Long,Excellent,Dwarf,Alpine,Northern,scouring,flowered,Brittle,Fragrant,fragile,field,interrupted,Bristly,mountain,Thread-leaf,False,Cliff,Early,loving,faint,weak,strong,deep,red,green,blue,bog,arctic,pond,continental,wide,free,southern,eastern,western,planar,astral,nether,digital,powerful,hanging,creeping,variegated,skyward,sad,smiling,forest,yellow,magenta,stiff,bog,thread-leaf,highland,Black,Gray,Speckled,striped,white,almond,Swamp,Bear,fox,lion,tiger,bind,Bolean,weeping,Mahogany,River,silver,spice,sweet,water,bitter,hairy,hispid,running,black-eyed,blackhaw,black-head,bow,box,brittle,buckbutterfly,clumpfoot,meadow,skunk,cart,rum,whiskey,wild,colve,colic,collard,brilliant,cutleaf,thin-leaved,three-leaved,two-leaved,single-leaved,silky,fern-leaf,golden,pale,cotton,bank,belle,bulbous,lamb's,land,scurvy,upland,poorland,gloriosahenbit,spotted,devil's,dew,flowering,duck,dye,extinguisher,fever,coast,mosquito,sword,flux,fume,gall,night-scented,rogue's,queen's,king's,peasant's,child's,ground,grape,poison,violet,wind";
var plantNameSuffixString = "horsetail,goosefood,rush,club-moss,fern,tree,Potamogeto,moss,tofieldia,asphodel,pondweed,brake,sedge,alder,Ambrosia,Ash,bamboo,lily,Birch,cress,cap,weed,wood,brier,bush,leaf,eye,flower,cabbage,bay,buckeye,sycamore,root,jalap,carrot,ironwood,cherry,clover,colwortconeflower,cornel,corydalis,cress,nest,toes,foot,daisy,deadnettle,bit,needle,nose,plague,dewberry,leafwort,flax,ferns,wood,garlic,tongue,cane,hellebore,hemp,hedge,leek,tassel,mulberry,nightshade,oak,onion,parsley,pasnip,pellitory,rose,grass,thistle";
var plantNameSuffixArray;
var plantNamePrefixArray;

$(document).ready(function(){

    // Name
        // Turn name strings into arrays
    plantNamePrefixArray = plantNamePrefixString.split(",");
    plantNameSuffixArray = plantNameSuffixString.split(",");

    var currentPlantName = generatePlantName();
    $('#name').append(currentPlantName);

    // Description
    readFromFile('/public/source/Plant_Descriptions.txt', createChain, '#description', getRandomInt(16,23), getRandomInt(5,7));

    // Morphology
    readFromFile('/public/source/Plant_Morphology.txt', createChain, '#morphology', getRandomInt(10,15),  getRandomInt(5,7));

    // Ecology
    readFromFile('/public/source/Plant_Ecology.txt', createChain, '#ecology', getRandomInt(7,12), getRandomInt(4,7));

    // Miscellaneous
    // readFromFile('/public/source/Plant_General.txt', createChain, '#misc', getRandomInt(5,25), getRandomInt(4,7));
    // readFromFile('/public/source/Darwin_Transmutation.txt', createChain, '#misc', getRandomInt(15,25), getRandomInt(5,7));
    readFromFile('/public/source/Darwin_SelfFertilisation.txt', createChain, '#misc', getRandomInt(15,25), getRandomInt(5,7));
});


function readFromFile(filePath, callback, id, lengthOfChain, numWords) {

    var stringFromFile;

    // Retrieve data
    $.post("/getStringFromFile", {filePath:filePath}, function(data){

        // Callback
        if( data != null) {

            for (var i = 0; i < data.length; i++){

                stringFromFile += data[i];
            }

            callback(stringFromFile, id, lengthOfChain, numWords);
        }
    });
}

function createChain(text, id, lengthOfChain, numWords) {

    var m;

    var regex = new RegExp("([.^\\w]+ ){" + numWords + "}", "g");

    // x = new markov(text, type, regex);
    m = new markov(text, "string", regex);

    var markovString = "";
    var outString = "";

    for (var i = 0; i < 1; i++) {

        markovString = m.gen(lengthOfChain);

        markovString = cleanNumbers(markovString);
        markovString = checkForPeriod(markovString);

        checkGrammar(markovString, function(results) {
            $(id).append(results);
        });
    }
}

function returnString(text) {
    return text;
}

// Generate Name
function generatePlantName() {

    var prefix = capitalizeFirstLetter( plantNamePrefixArray[getRandomInt(0, plantNamePrefixArray.length)].toLowerCase() );
    var suffix = capitalizeFirstLetter( plantNameSuffixArray[getRandomInt(0, plantNameSuffixArray.length)].toLowerCase() );
    return prefix + " " + suffix;
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

function checkForPeriod(string)  {

    // Check for a space at the extended
    if (string.charAt(string.length - 1) == ' ') string = string.substring(0, string.length - 1);

    if (string.charAt(string.length - 1) != '.') string += '.';
    return string;
}

function capitalizeFirstLetter(string) {
    return string.charAt(0).toUpperCase() + string.slice(1);
}

function getRandomInt(min, max) {
  return Math.floor(Math.random() * (max - min)) + min;
}
