////function DecryptData(pass, encryptData) {
////    var hashMD5 = CryptoJS.MD5(pass);
////    var hashString = hashMD5.toString(CryptoJS.enc.Base64);
////    var iv = CryptoJS.enc.Hex.parse('8f26e64f3ae5aae8bf81b445487f4833');
////    var Pass = CryptoJS.enc.Utf8.parse(hashString);
////    var Salt = CryptoJS.enc.Utf8.parse("123resultxyz@");
////    var key128Bits1000Iterations = CryptoJS.PBKDF2(
////        Pass.toString(CryptoJS.enc.Utf8),
////        Salt,
////        {
////            keySize: 128 / 32,
////            iterations: 1000
////        }
////    );
////    var cipherParams = CryptoJS.lib.CipherParams.create({
////        ciphertext: CryptoJS.enc.Base64.parse(encryptData)
////    });

////    var decrypted = CryptoJS.AES.decrypt(
////        cipherParams,
////        key128Bits1000Iterations,
////        {
////            mode: CryptoJS.mode.CBC,
////            iv: iv,
////            padding: CryptoJS.pad.Pkcs7
////        });
////    return decrypted.toString(CryptoJS.enc.Utf8);
////}


function Decrypt(key, cipherText) {
    var result = ``;
    var cipherBytes = Base64ToArrayBuffer(cipherText);
    var lengthKey = key.length;
    var uint8View = new Uint8Array(cipherBytes);

    uint8View.forEach(function (item, index) {
        var oldPosition = item - key.charCodeAt(index % lengthKey);
        result += `${String.fromCharCode(oldPosition)}`;
    });
    return result;
}

function Base64ToArrayBuffer(base64) {
    var binary_string = window.atob(base64);
    var len = binary_string.length;
    var bytes = new Uint8Array(len);
    for (var i = 0; i < len; i++) {
        bytes[i] = binary_string.charCodeAt(i);
    }
    return bytes.buffer;
}