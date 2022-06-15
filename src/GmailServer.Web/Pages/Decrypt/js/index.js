$(function () {
    var btnDecryptElement = $("#btnDecrypt");
    //var btnEncryptElement = $("#btnEncrypt");
    var btnClearResultElement = $("#btnClearResult");
    var btnClearPlainElement = $("#btnClearPlain");
    var btnCopyElement = $("#btnCopy");
    var keyElement = $("#key");
    var cipherTextElement = $("#cipherText");
    var resultElement = $("#result");

    btnClearResultElement.on('click', function (e) {
        e.preventDefault();
        if (confirm("Are you sure?")) {
            resultElement.val('');
        }
    });

    btnClearPlainElement.on('click', function (e) {
        e.preventDefault();
        cipherTextElement.val('');
    });

    btnCopyElement.on('click', function (e) {
        e.preventDefault();
        var copyText = document.getElementById("result");
        copyText.select();
        copyText.setSelectionRange(0, 99999);
        navigator.clipboard.writeText(copyText.value);
        $("#copied-alert").slideDown(300).delay(1500).slideUp(400);
    });

    btnDecryptElement.on('click', function (e) {
        e.preventDefault();
        var key = keyElement.val();
        if (!key) {
            alert("Please, Enter KEY!!!");
            return;
        }
        var cipherTextVal = cipherTextElement.val();
        if (!cipherTextVal) {
            alert("Please, Enter Cipher Text!!!");
            return;
        }

        var ciphers = cipherTextVal.split('\n');
        var result = ``;

        ciphers.forEach((cipher, index) => {
            if (cipher) {
                var decryptData = Decrypt(key, cipher);
                result += `${decryptData}\n`;
            }
        });
        resultElement.val(result);
    });
});