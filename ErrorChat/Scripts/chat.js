$(document).ready(function () {
    var chatBox = $(".chat-box"); 

    var chat = $.connection.chatHub;
    chat.client.SendAllClients = function (name, msg) {
        var date = new Date().toLocaleString().replace(/,/, '');
        var message = "<div class='message message-inner'><span class='message-information'><span class='message-date'>" + date +
            "</span><span class='message-author'>" + name + "</span></span><div class='message-text'>" + msg + "</div></div>";

        $(".chat-box").append(message).animate({  //опустить чат до самого низа
            scrollTop: chatBox.get(0).scrollHeight
        });
    }


    $.connection.hub.start().done(function () {
        $("#btnSend").click(function () {          
            if (IsAuthenticated) { //если пользователь авторизован
                var messageText = $("#message").val();
                if (messageText.trim() != "") {
                    chat.server.send(userName, messageText, $("#titleChat").html());
                    $("#message").val("");

                    var date = new Date().toLocaleString().replace(/,/, '');
                    var message = "<div class='message message-out'><span class='message-information'><span class='message-date'>" + date +
                        "</span><span class='message-author'>" + userName + "</span></span><div class='message-text'>" + messageText + "</div></div>";

                    $(".chat-box").append(message).animate({  //опустить чат до самого низа
                        scrollTop: chatBox.get(0).scrollHeight
                    });
                }               
            }
            else {
                location.href = "/account/Login?returnUrl=" + location.pathname;
            }           
        });
    });

    document.onkeydown = function (evt) { //нажатие на enter
        var keyCode = evt ? (evt.which ? evt.which : evt.keyCode) : event.keyCode;
        if (keyCode == 13) {
            $("#btnSend").click();
        }
    }


    $.ajax({
        type: "POST",
        url: "/relation/GetMessagesByChatRoomTitle",
        data: { "title": $("#titleChat").html() },
        beforeSend: function () {
            $(".loader").show();
        },
        success: function (data) {
            for (var i = 0; i < data.length; i++) {
                var dateMs = data[i].Date.replace(/[^0-9 +]/g, '');
                var dateMsInt = parseInt(dateMs);
                var fullDate = new Date(dateMsInt);
                var date = fullDate.toLocaleString().replace(/,/, '');
                var message = null;

                if (data[i].User.UserName == userName) {
                    message = "<div class='message message-out'><span class='message-information'><span class='message-date'>" + date +
                        "</span><span class='message-author'>" + data[i].User.UserName +
                        "</span></span><div class='message-text'>" + data[i].Text + "</div></div>";
                }
                else {
                    message = "<div class='message message-inner'><span class='message-information'><span class='message-author'>" + data[i].User.UserName +
                        "</span><span class='message-date'>" + date +
                        "</span></span><div class='message-text'>" + data[i].Text + "</div></div>";
                }

                chatBox.append(message);
            }
            $(".loader").hide();

           chatBox.scrollTop(chatBox.get(0).scrollHeight);//опустить чат до самого низа
        }
    });   
});
