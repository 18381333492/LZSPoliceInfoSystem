
/**
/* 引用的Js:
/* kindeditor.js
/* zh_CN.js
/* @description:对KindEditor的插件的封装
/* @author:[汤台]
/* @time:2016-09-19
/* @version:1.0.0
*/

function kindeditor() {

    var editor;

    /*配置编辑器的工具栏，其中”/”表示换行，”|”表示分隔符。*/
    var items = {
        /*小型工具栏*/
        small:
        [
        'undo', 'redo', '|', 'preview', '|', 'justifyleft', 'justifycenter', 'justifyright',
        'justifyfull',  '|', 'insertorderedlist', 'insertunorderedlist', 'indent', 'outdent',
         'quickformat', '|','formatblock', 'fontname', 'fontsize', '|', 'forecolor', 'hilitecolor', 'bold',
        'italic', 'underline', 'lineheight', 'emoticons', '|', 'fullscreen'
        ],
        /*简单工具栏*/
        single: 
        [
        'source', '|', 'undo', 'redo', '|', 'preview', 'print', 'cut', 'copy', 'paste',
        'plainpaste', 'wordpaste', '|', 'justifyleft', 'justifycenter', 'justifyright',
        'justifyfull', 'insertorderedlist', 'insertunorderedlist', 'indent', 'outdent', 'subscript',
        'superscript', 'clearhtml', 'quickformat', 'selectall', '|',  '/',
        'formatblock', 'fontname', 'fontsize', '|', 'forecolor', 'hilitecolor', 'bold',
        'italic', 'underline', 'strikethrough', 'lineheight', 'removeformat', '|', 'image', 'multiimage',
        'insertfile', 'table', 'hr', 'emoticons', 'pagebreak', '|', 'fullscreen'
        ],
        /*默认工具栏*/
        defaults:
        [
        'source', '|', 'undo', 'redo', '|', 'preview', 'print', 'template', 'code', 'cut', 'copy', 'paste',
        'plainpaste', 'wordpaste', '|', 'justifyleft', 'justifycenter', 'justifyright',
        'justifyfull', 'insertorderedlist', 'insertunorderedlist', 'indent', 'outdent', 'subscript',
        'superscript', 'clearhtml', 'quickformat', 'selectall', '|', 'fullscreen', '/',
        'formatblock', 'fontname', 'fontsize', '|', 'forecolor', 'hilitecolor', 'bold',
        'italic', 'underline', 'strikethrough', 'lineheight', 'removeformat', '|', 'image', 'multiimage',
        'flash', 'media', 'insertfile', 'table', 'hr', 'emoticons', 'baidumap', 'pagebreak',
        'anchor', 'link', 'unlink', '|', 'about'
        ]
    }

    /*
    * 创建KindEditor编辑器（create）.
    * @author [汤台]
    * @version 1.0.0
    * @param   id(标签唯一标识), title, icon, fn(确认回调函数)
    * @return {dialog}
    */
    function create(id, param) {
        //默认的对象
        var defaults = {
            items: items.single,
            /*服务端图片上传路径*/
            uploadJson: "/HandleProgram/PictureUpload.ashx",
            afterCreate: null
        }
        defaults = $.extend(defaults, param);
        /**
        *创建编辑器
        */

        editor = KindEditor.create(id,
            {/*属性的配置*/
                items: defaults.items,
                uploadJson: defaults.uploadJson,
                bodyClass: 'ke-content',
                filterMode:false,//true时过滤HTML代码，false时允许输入任何代码。
                /*创建编辑器之后的回调函数*/
                afterCreate: defaults.afterCreate  /*创建编辑器之后的回调函数*/
            });
    }

    /*
    * 设置html
    * @author [汤台]
    * @version 1.0.0
    * @param   html(标签内容)
    * @return  {void}
    */
    function sethtml(str) {
        editor.html(str);
    }

    /*
    * 设置html
    * @author [汤台]
    * @version 1.0.0
    * @param   html(获取标签内容)
    * @return  {void}
    */
    function gethtml() {
       return editor.html();
    }

    /*
    * 设置text
    * @author [汤台]
    * @version 1.0.0
    * @param   text(文本内容)
    * @return  {void}
    */
    function settext(str) {
        editor.text(str);
    }

    /*
    * 设置html
    * @author [汤台]
    * @version 1.0.0
    * @param   text(获取文本内容)
    * @return  {void}
    */
    function gettext() {
        return editor.text();
    }


    return {
        items:items,//工具栏配置项
        create: create,
        sethtml: sethtml, //html获取/设置
        gethtml: gethtml,
        settext: settext, //文本获取/设置
        gettext: gettext,
    }
}