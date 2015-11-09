var express = require('express');
var exphbs  = require('express-handlebars');
var bodyParser = require("body-parser");
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

    var rl = require('readline').createInterface({
      input: require('fs').createReadStream(filePath)
    });

    rl.on('line', function (line) {
        lines.push(line);
    });

    rl.on('close', function() {
        res.send(lines);
    });
});

var server = app.listen(3333, function () {

  var host = server.address().address;
  var port = server.address().port;

  console.log('App listening at http://%s:%s', host, port);

});
