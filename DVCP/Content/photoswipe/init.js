$('.imgpost').each(function () {
    var $pic = $(this),
        getItems = function () {
            var items = [];
            $pic.find('img').each(function () {
                var $href = $(this).attr('src'),
                    //$size = $(this).data('size').split('x'),
                    $width =  900,//$size[0],
                    $height = 600;//$size[1];

                var item = {
                    src: $href,
                    w: $width,
                    h: $height
                }

                items.push(item);
                //console.log(item);
            });

            return items;
        }

    var items = getItems();
    var $pswp = $('.pswp')[0];
    $pic.on('click', 'img', function (event) {
        event.preventDefault();

        var $index = $(this).index();
        var options = {
            index: $index,
            bgOpacity: 0.7,
            showHideOpacity: true
        }

        // Initialize PhotoSwipe
        var lightBox = new PhotoSwipe($pswp, PhotoSwipeUI_Default, items, options);
        lightBox.init();
    });
});