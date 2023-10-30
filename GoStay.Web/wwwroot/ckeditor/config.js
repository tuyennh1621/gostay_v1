/**
 * @license Copyright (c) 2003-2019, CKSource - Frederico Knabben. All rights reserved.
 * For licensing, see https://ckeditor.com/legal/ckeditor-oss-license
 * Tích hợp và hướng dẫn bởi https://trungtrinh.com - Website chia sẻ bách khoa toàn thư */

CKEDITOR.editorConfig = function (config) {
    //config.extraPlugins = 'uploadimage';
    //config.filebrowserUploadMethod = 'form';
    //config.removeDialogTabs = 'image:advanced;image:Link;link:advanced;link:upload';
/*    config.extraPlugins = 'pasteUploadImage';*/
    config.filebrowserBrowseUrl = './ckeditor/ckfinder/ckfinder.html';
    config.filebrowserImageBrowseUrl = './ckeditor/ckfinder/ckfinder.html?type=Images';
    config.filebrowserFlashBrowseUrl = './ckeditor/ckfinder/ckfinder.html?type=Flash';
    //config.filebrowserUploadUrl = './ckeditor/ckfinder/core/connector/php/connector.php?command=QuickUpload&type=Files';
    //config.filebrowserUploadUrl = './AddPictureNewsContent';

    //config.filebrowserImageUploadUrl = './ckeditor/ckfinder/core/connector/php/connector.php?command=QuickUpload&type=Images';
    config.filebrowserImageUploadUrl = './AddPictureNewsContent';

    config.filebrowserFlashUploadUrl = './ckeditor/ckfinder/core/connector/php/connector.php?command=QuickUpload&type=Flash';
};
