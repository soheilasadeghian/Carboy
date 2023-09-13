var tracker = {
    urls: [
            "/geo",
    ],
    f1: function (hash) {
        $.ajax({
            type: "POST",
            url: tracker.urls[0],
            data: JSON.stringify({ hash: hash }),
            contentType: 'application/json; charset=utf-8',
            dataType: "json",
            async: true,
            success: function (json) {
                $(document).trigger('new-position', [json])
            },
            failure: function (response) {
            }
        });
    }
}