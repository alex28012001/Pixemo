$(document).ready(function () {
    $('#uploadImage').click(function () {
        if (window.FormData !== undefined) {
            var files = document.getElementById('uploadFile').files;

            var formdata = new FormData();
            formdata.append("imageId", imageId);

            //после покупки рекламы у покупателя есть форма в которую можно вписать url своего сайта или еще чего то , чтобы при нажатии на картинку рекламы человек переходил на эту ссылку
            //параметр savedLink нужен чтобы проверить человек первый раз сохраняет ссылку или нет. Если 1 раз то она сохраняется, а если не 1 раз , то у него это не получится т.к ссылка уже сохранена!
            //если ссылка сохранена мы disable-им  поле с ссылкой, и изменяем параметр savedLink на true т.к ссылка уже сохранена .Проверяем на сервере этот параметр и делаем вывод сохранять или нет
            //это делается для того чтобы нельяз было изменять ссылку на рекламу 

            var adLink = $("#advertisingLink").attr("disabled");
            if (adLink == undefined && $("#advertisingLink").val() != "") {
                $("#advertisingLink").prop('disabled', true);
                formdata.append("savedLink", "false");
                formdata.append("advertisingLink", $("#advertisingLink").val());
            }

            else {
                formdata.append("savedLink", "true");
            }

            if (files.length > 0) {
                for (var i = 0; i < files.length; i++) {
                    formdata.append("file" + i, files[i]);
                }
            }

            $.ajax({
                type: "POST",
                url: "/relation/UploadAdvertising",
                contentType: false,
                processData: false,
                data: formdata,
                success: function (data) {
                    if (data.Successed) {
                        if (data.ImagesUrl != null) {
                            var images = data.ImagesUrl;
                            $("#advertising-place a img").attr("src", images[0].replace(";",""));
                            if (images.length > 1) {
                                $("#advertising-place-2 a img").attr("src", images[1].replace(";", ""));
                            }  
                        }
                        if (data.AdLink != null) {
                            var adLink = data.AdLink;
                            $("#advertising-place a").attr("href", adLink);
                            $("#advertising-place-2 a").attr("href", adLink);
                        }
                    }
                    else {
                        alert(data.ErrorMessage)
                    }

                }
            });
        }

        else {
            alert("Ваш браузер устарел!");
        }
    });
});