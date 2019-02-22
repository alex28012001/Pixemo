$(document).ready(function () {
   
    var comleteAjaxQuery = true;
    var offsetMedia = 1; //сдвиг картинок (по страницам), 1 т.к сначало идет 1 страница
    var offsetGroups = 0; //сдвиг картинок (по группам), 0 т.к сначала нет никакого сдвига

    var ajaxImagesUrl = "media/Images/";
    var ajaxVideosUrl = "media/Videos/";
 

    $(".dropdown-menu li span").click(function () { //при переходе на параметр выпадаещего меню параметр отображается на кнопке
        $("#media-type").html("<span class='hide-sm'>"+ $(this).text()+"</span><span class='caret' style = 'margin-left:5px'></span>");
        $("#media-type").val($(this).text());

        if ($(this).text() == "Images") {
            $("#searchVideos").removeClass("active");
            $("#searchImages").addClass("active");
        }
        else {
            $("#searchImages").removeClass("active");
            $("#searchVideos").addClass("active");
        }
    });
   

    $("#sendQuery").click(function () {
        var position = $("#searchImages").offset().top - 60; //прокрутить окно до кнопки , а 10 это размер кнопки в пикселях
        $("HTML, BODY").animate({ scrollTop: position }, 700);
        
        Clear();
        
        var mediaType = $("#media-type").val(); //или Images , или Videos
        if (mediaType == "Images") {
            if ($("#query").val() != "") 
                makeRequest( $("#query").val(), ajaxImagesUrl);
            else
                makeRequest( "природа", ajaxImagesUrl);
        }
        else {
            if ($("#query").val() != "")
                makeRequest( $("#query").val(), ajaxVideosUrl);
            else
                makeRequest( "природа", ajaxVideosUrl);
        }
    });
    

    //при загрузки страницы сразу же показывать картинки
    if ($("#query").val() != "")
        makeRequest($("#query").val(), ajaxImagesUrl);
    else
        makeRequest("природа", ajaxImagesUrl);
    

    document.onkeydown = function (evt) { //нажатие на enter
        var keyCode = evt ? (evt.which ? evt.which : evt.keyCode) : event.keyCode;
        if (keyCode == 13) {
            $("#sendQuery").click();
        }
    }

    $("#searchImages").click(function () {
        $("#searchVideos").removeClass("active");
        $("#searchImages").addClass("active");

        $("#media-type").html("<span class='hide-sm'>Images</span><span class='caret' style = 'margin-left:5px'></span>");
        $("#media-type").val("Images");

        Clear();
        if ($("#query").val() != "")
            makeRequest($("#query").val(),ajaxImagesUrl);
        else
            makeRequest( "природа", ajaxImagesUrl);
    });

    $("#searchVideos").click(function () {
        $("#searchImages").removeClass("active");
        $("#searchVideos").addClass("active");

        $("#media-type").html("<span class='hide-sm'>Video</span><span class='caret' style = 'margin-left:5px'></span>");
        $("#media-type").val("Video");

        Clear();
        if ($("#query").val() != "")
            makeRequest( $("#query").val(), ajaxVideosUrl);
        else
            makeRequest( "природа", ajaxVideosUrl);
    });

   

    $(window).scroll(function () {
        if (comleteAjaxQuery == true && ($(window).scrollTop() + $(window).height()) >= $(document).height() - ($(document).height() / 2)) {
                //++offsetGroups; //сдвигаем группы
                ++offsetMedia; //сдвигаем картинки

                // определяет что искать, картинки или видеозаписи
                var mediaTypeUrl = ($("#searchImages").hasClass("active") ? ajaxImagesUrl : ajaxVideosUrl );
                makeRequest( $("#query").val(), mediaTypeUrl, offsetMedia, offsetGroups);
        }
    });

    function Clear() {
        $("#Result").empty();
        offsetMedia = 1;
        offsetGroups = 0;
    }

    function makeRequest(query, mediaTypeUrl, offsetMedia, LocaleOffsetGroups) {
        $.ajax({
            type: "POST",
            url: mediaTypeUrl,
            data: { 'query': query, 'outOffsetMedia': offsetMedia, 'outOffsetGroups': LocaleOffsetGroups },
            //contentType: "application/x-www-form-urlencoded",
            dataType: "json",

            beforeSend: function (jqXHR, settings) {
                comleteAjaxQuery = false;
                $(".loader").show();
            },

            success: function (massData) {
                $(".loader").hide();
                if (massData[1] != null) {
                    offsetGroups = massData[1];
                }
                var data = null;
                if (massData.length == 2) {
                     data = massData[0];
                }
                else {
                    data = massData;
                }
               

                for (var i = 0; i < data.length; i++) {
                    var media = null;

                    if (mediaTypeUrl == ajaxImagesUrl) { //проверка запрос на картинки или на видео
                        if (data[i].Sizes != null) {  //проверка, эта картинка принадлежит вк или pixabay , если true , то вк                   

                            var vkImageTitle = data[i].Title == "" ? "info" : data[i].Title; 

                            media = "<div class='images'><div class='divImg effect4'><a href='media/info/" +
                                     data[i].Id + "/" + data[i].OwnerId + "/" + "'>" +"<img src='" +
                                     data[i].Sizes[data[i].Sizes.length - 1].Url + "' class='img-responsive' alt='" +
                                     vkImageTitle + "'/><div class='caption'><div class='caption-text'><h1>"+
                                     vkImageTitle + "</h1></div></div></a></div></div>";
                        }
                               

                        else {                           
                            media = "<div class='images act'><span id='idImg' style='display:none'>" +
                                    data[i].Id + "</span><span id='urlImg' style='display:none'>" + data[i].FullHDImageURL +
                                    "</span><div class='divImg effect4'><img src='" + data[i].WebformatURL +
                                    "' class='img-responsive' alt='" + data[i].Title + "'/><div class='caption'>" +
                                    "<div class='caption-text'><h1>" + data[i].Title + "</h1></div></div></div></div>";                           
                        }
                    }


                    else {
                        //проверка, это видео принадлежит вк или pixabay
                        if (data[i].BigPoster != null || data[i].MediumPoster != null || data[i].SmallPoster != null) {
                            media = "<div class='images'>  <div class='thumb-wrap'> <iframe src='" + data[i].Url +
                            "' frameborder='0'></iframe></div></div>"
                        }

                        else {
                            media = "<div class='images'><video controls src='" + data[i].Url + "'> </video></div>"
                        }
                    }


                    $("#Result").append(media);
                }
                $(".loader").hide();
                comleteAjaxQuery = true;
            }
        });
    } 
});          
