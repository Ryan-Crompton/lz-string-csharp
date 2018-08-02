var fs = require('fs');
var lzString = require('lz-string');

function shuffleArray(array) {
    for (var i = array.length - 1; i > 0; i--) {
        var j = Math.floor(Math.random() * (i + 1));
        var temp = array[i];
        array[i] = array[j];
        array[j] = temp;
    }
    return array;
}

var chars = '0123456789'
    + 'ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz'
    + '!$%^&*()_+|~-=`{}[]:;<>?,./';
var charsArray = chars.split('');
var outputArray = [];
var emptyArray = Array.apply(null, Array(10000)).map(Number.prototype.valueOf,0);

for(var i = 0; i < 1000; i++){
    var str = emptyArray.map(() => shuffleArray(charsArray)[0]).toString();
    var compressed = lzString.compressToEncodedURIComponent(str);
    outputArray.push({Key: str, Value: compressed});
}

fs.writeFile("compress.json", JSON.stringify(outputArray), { encoding: "utf-16le"}, function (err) {
    if (err) throw err;
    console.log('complete');
});