// OH HAI

var express = require('express');
var stormpath = require('express-stormpath');

var app = express();

app.use(stormpath.init(app, {
  // Optional configuration options.
}));

app.listen(3000);

// Stormpath will let you know when it's ready to start authenticating users.
app.on('stormpath.ready', function () {
  console.log('Stormpath Ready!');
});
