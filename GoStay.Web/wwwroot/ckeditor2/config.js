/*
Copyright (c) 2003-2012, CKSource - Frederico Knabben. All rights reserved.
For licensing, see LICENSE.html or http://ckeditor.com/license
*/

CKEDITOR.editorConfig = function( config )
{
    config.enterMode = CKEDITOR.ENTER_BR;
    config.toolbar = 'Full';
    config.filebrowserBrowseUrl = './ckeditor/ckfinder/ckfinder.html';
    config.filebrowserImageBrowseUrl = './ckeditor/ckfinder/ckfinder.html?type=Images';
    config.filebrowserFlashBrowseUrl = './ckeditor/ckfinder/ckfinder.html?type=Flash';
    //config.filebrowserUploadUrl = './ckeditor/ckfinder/core/connector/php/connector.php?command=QuickUpload&type=Files';
    //config.filebrowserUploadUrl = './AddPictureNewsContent';

    //config.filebrowserImageUploadUrl = './ckeditor/ckfinder/core/connector/php/connector.php?command=QuickUpload&type=Images';
    config.filebrowserImageUploadUrl = './AddPictureNewsContent';

    //config.filebrowserFlashUploadUrl = './ckeditor/ckfinder/core/connector/php/connector.php?command=QuickUpload&type=Flash';
};
