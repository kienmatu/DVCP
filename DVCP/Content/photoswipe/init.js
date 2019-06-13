$('.imgpost').each(function () {
    var $pic = $(this),
        getItems = function () {
            var items = [];
            $pic.find('img').each(function () {
                var width = this.naturalWidth;
                var height = this.naturalHeight;
                var $href = $(this).attr('src'),
                    //$size = $(this).data('size').split('x'),
                    $width = width,//$size[0],
                    $height = height;//$size[1];
                if (width >= 50 && height >= 50) {
                    var item = {
                        src: $href,
                        w: $width,
                        h: $height
                    }

                    items.push(item);
                }
                
                //console.log(width.toString());
            });

            return items;
        }

    var items = getItems();
    var $pswp = $('.pswp')[0];
    $pic.on('click', 'img', function (event) {
        event.preventDefault();

        var $index = $(this).index('img') - 1;
        if (items.length < $index) {
            $index = items.length - 1;
        }
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