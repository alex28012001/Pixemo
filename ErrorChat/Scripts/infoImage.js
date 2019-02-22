$(document).ready(function () {     
    $(".containerImages").on("click", ".act", function () {
        var id = $(this).find("#idImg").html();
        var url = $(this).find("#urlImg").html();
       
        var parser = document.createElement('a');
        parser.href = url;

        var paths = parser.pathname.split('/'); // /media/info/...
        var path = paths[paths.length - 1]; //cat345358385.jpg
        var imageUrlId = path.replace(/(.jpg)$/,'');
        
        window.location.href = "media/info/" + id + "/" + imageUrlId + "/";
    });
});
