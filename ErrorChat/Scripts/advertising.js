$(document).ready(function () {
    makeRequest("POST", "/relation/GetAdvertising", { "imageId": imageId }, "json", function (data) {
        if (data.Exist == true && data.Ad.ImageUrl != null ) {
            var images = data.Ad.ImageUrl.split(";");
            $("#advertising-place a img").attr("src", images[0]);
            if (images.length > 2) {
                $("#advertising-place-2 a img").attr("src", images[1]);
            }

            if (data.Ad.AdvertisingLink != null)
            {
                var adLink = data.Ad.AdvertisingLink;
                $("#advertising-place a").attr("href", adLink);
                $("#advertising-place-2 a").attr("href", adLink);
            }
        }
    });


    makeRequest("POST", "/relation/ModeratorPanel", { "imageId": imageId }, "html", function (data) {
        if (data != "") {
            $("#moderatorPanel").append(data);
        }
    }); 
  
    $("#buy-ad").click(function () {
        var parser = document.createElement('a');
        parser.href = returnUrl;
        var paths = parser.pathname.split('/');
        var imageId = paths[paths.length - 3];
        var ownerId_Url = paths[paths.length - 2];
        window.location.href = "/relation/advertising/"+imageId+"/"+ownerId_Url+"/";
    }); 
       

    function makeRequest(type, url, data, dataType, success)
    {
        $.ajax({
            type: type,
            url: url,
            data: data,
            dataType: dataType,
            success: success
        });
    }
});