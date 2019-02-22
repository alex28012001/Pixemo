$(document).ready(function () {
    GetCommentsById(imageId);

    var date = new Date();
    var dateJson = date.toJSON();

    //если пользователь не авторизирован на сайте , то перенаправляем его на login
    $.ajaxSetup({
        statusCode: {
            401: function () {
                location.href = "/account/Login?returnUrl=" + location.pathname;
            }
        }
    });

    $("#sendComment").click(function () {
        $.ajax({
            type: "POST",
            url: "/relation/SendComment",
            data: { "ImageID": imageId, "Text": $("#commentText").val() },
            dataType: "json",
            success: function (data) {
                if (data != null) {
                    var date = ParseCsharpDate(data.Date);
                    var comment = "<div class='comment'><h4>" + data.User.UserName +"</h4><h4 class='comment-date'>"+date+
                                  "</h4></div><h4>" + data.Text + "</h4><hr>"
                    $("#Comments").append(comment);
                    $("#commentText").val("");
                   
                    var commentLength = parseInt($(".comments-length h3").html());
                    var newCommentLength = ++commentLength;
                    $(".comments-length h3#length").html(newCommentLength);
                }
            }      
        });
    });

        function GetCommentsById(id) {
            $.ajax({
                type: "POST",
                url: "/relation/GetCommentsByImageID",
                data: { "imageId": id },
                dataType: "json",
                beforeSend: function () {
                    $(".loader").show();
                },
                
                success: function (data) {
                    
                    if (data.length > 0) {
                        $("#comments-inforamtion .comments-length #length").html(data.length); //кол-во коменнтариев

                        for (var i = 0; i < data.length; i++) {
                            var date = ParseCsharpDate(data[i].Date);
                            var comment = "<div class='comment'><h4>" + data[i].User.UserName +
                            "</h4><h4 class='comment-date'>" + date + "</h4></div> <span><h4>" + data[i].Text + "</h4></span><hr>"
                                           
                            $("#Comments").append(comment);
                        }
                    }
                   $(".loader").hide();
                }
            });
        }

        function ParseCsharpDate(date)
        {
            var dateMs = date.replace(/[^0-9 +]/g, '');
            var dateMsInt = parseInt(dateMs);
            var fullDate = new Date(dateMsInt);
            return fullDate.toLocaleString().replace(/,/, '');
        }
});