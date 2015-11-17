var express = require('express');
var exphbs  = require('express-handlebars');
var bodyParser = require("body-parser");
var gingerbread = require('gingerbread');

var app = express();

app.engine('handlebars', exphbs({defaultLayout: 'main'}));
app.set('view engine', 'handlebars');

app.use(express.static(__dirname + '/public'))
app.use(bodyParser.urlencoded({ extended: false }));



app.get('/', function (req, res) {

    res.render('home');
    console.log("Page loaded.");
});

app.post('/getStringFromFile',function(req,res){

    var filePath = req.body.filePath;

    filePath = __dirname + filePath;
    var lines = [];
    var readingDescription = false;

    var rl = require('readline').createInterface({
      input: require('fs').createReadStream(filePath)
    });

    rl.on('line', function (line) {

      if (readingDescription == false && line.substring(0, 2) == '/*')
        readingDescription = true;
      else if (readingDescription == true && line.substring(0, 2) == '*/')
        readingDescription = false;

      if (readingDescription == false)
        lines.push(line);
    });

    rl.on('close', function() {
        res.send(lines);
    });
});

app.post('/grammarCheck', function (req,res){

    var text = req.body.text;

    gingerbread(text, function (error, text, result, corrections) {

        if (error == null) {
            console.log("\nText: " + text);
            console.log("\nResult: " + result);

            // Log corrections
            for (var i=0;i<corrections.length;i++) {
                console.log("\nCorrection: " + i + ": " + JSON.stringify(corrections[i]));
            }

            res.send(result);
        }
        else {
            console.log("Error: " + error);
        }
    });
});

var server = app.listen(3333, function () {

  var host = server.address().address;
  var port = server.address().port;

  console.log('App listening at http://%s:%s', host, port);

});
