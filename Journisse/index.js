var express = require('express');
var exphbs  = require('express-handlebars');
var fs = require('fs');
var app = express();

app.engine('handlebars', exphbs({defaultLayout: 'main'}));
app.set('view engine', 'handlebars');

app.use(express.static(__dirname + '/public'))

app.get('/', function (req, res) {

    res.render('home');
    console.log("Page loaded.");
});

app.post('/getStringFromFile',function(req,res){

    // var filePath = req.body.filePath;
    fs.readFile(__dirname + '/public/source/text.txt', 'utf8', function (err,data) {

      if (err) {
        return console.log(err);
      }

      res.end(data);
    });
});

var server = app.listen(3333, function () {

  var host = server.address().address;
  var port = server.address().port;

  console.log('App listening at http://%s:%s', host, port);

});
