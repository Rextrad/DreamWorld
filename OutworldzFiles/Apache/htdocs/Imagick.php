<?php
header();
echo "Running";
$image = new Imagick();
echo "Imagick Running";
echo "made magic";
$image->newImage(1, 1, new ImagickPixel('#ffffff'));
$image->setImageFormat('png');
$pngData = $image->getImagesBlob();
/* Send headers and output the image */

echo strpos($pngData, "\x89PNG\r\n\x1a\n") === 0 ? 'Ok' : 'Failed';

