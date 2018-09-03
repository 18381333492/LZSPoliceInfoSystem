
/*
**tip:基于easyui后台js的公共函数的封装
**date:[2017-8-2]
**author:[TT]
*/

function adminPackage() {

    var basePath ='';
    var button= {
            enable: function () { $('#ok').eq(0).linkbutton('enable'); },
            disable: function () { $('#ok').eq(0).linkbutton('disable'); }
    }

    /*注册路由 
    @params area:域名
    control:控制器名
    */
    function routeRegister(control, area) {
        if (area)
            basePath = "/" + area + "/" + control + "/";
        else
            basePath = "/" + control + "/";
        var grid = $('#' + control);
        var toolbar = $('#' + control + "_toolbar");
        var form ='#dataForm';
        var searchform ='#searchForm';
        //****动态改变api接口*****//
        res.searchform = searchform;
        res.form = form;
        res.grid = grid;
        res.toolbar = toolbar;
    }


    //获取url
    function route(action, value) {
       return basePath + action;
    }


    /*
    * EasyUi提示框（messager.alert）.
    * @author [汤台]
    * @version 1.0.0
    * @param   msg, title, icon, fn(确认回调函数)
    * @return {dialog}
    * @icon   {String}error（X）,question(?),info,warning(!)
    */
    function alert(msg, title, icon, fn) {
        title = title || "提示"
        icon = icon || 'info';
        $.messager.alert(title, msg, icon, fn);
    }

    /*
    * EasyUi确认框（messager.confirm）.
    * @author [汤台]
    * @version 1.0.0
    * @param   tipStr, callback(确认回调函数)
    */
    function confirm(tipStr,callback) {
        $.messager.confirm('确认', tipStr, function (r) {
            if (r) {
                if (callback) callback();
            }
        });
    }

   
    /*
    * EasyUi弹出框（dialog）.
    * @author [汤台]
    * @version 1.0.0
    * @param  url, title, callback(确认的回调函数),width,height,loadCall(加载成功的回调函数)
    * @return {dialog}
    */
    function dialog(url, title, callback, width, height, loadCall, button) {
        width = width || 600;
        height = height || 400;
        var name = $('<div/>');
        var btn = [{
            id: 'ok',
            text: '<span style="padding-right:10px;">确 认</span>',
            iconCls: 'icon-ok',
            handler: callback,
        }, {
            text: '<span>关闭</span>',
            iconCls: 'icon-cancel',
            handler: function () { name.dialog('close'); }
        }];
        if (button) btn.push(button);
        var dlg =
            name.dialog({
                href: url,
                title: title,
                iconCls: 'icon-save',
                width: width,
                height: height,
                border: false,
                buttons: btn,
                onClose: function () { name.dialog("destroy"); },
                onLoad: function () {
                    //加载之后的动作   
                    //document.onkeydown = function (event) {
                    //    if (event.keyCode == "13") {
                    //        $('#ok').click();
                    //    }
                    // }
                    //回调页面(加载成功的函数
                    if (loadCall) loadCall();
                },
                modal: true
            });
        return dlg;
    }


    /*
    * ajax的请求的封装（form）.
    * @author [汤台]
    * @version 1.0.0
    * @param   url,data,callback,er_callback,async
    * @return {void}
    */
    function Ajax(url, data, callback, er_callback,type, async) {
        $.ajax({
            url: url,
            data: data,
            type: type||'POST',
            dataType: 'json',
            async: (async == null) ? true : async,
            success: function (r) {
                if (!r.over) { /*判断登录是否过期*/
                    if (r.success) {
                        callback(r);
                    }
                    else {
                        if (er_callback) {
                            er_callback(r);//手动提示错误
                        }
                        else {
                            alert(r.info);
                            $('#ok').eq(0).linkbutton('enable');//启用按钮
                        }
                    }
                }
                else {
                     alert("登录过期,请重新登录");
                }
            },
            // jqXHR 是经过jQuery封装的XMLHttpRequest对象
            // textStatus 可能为null、 'timeout（超时）'、 'error（错误）'、 'abort(中止)'和'parsererror（解析错误)'等
            // errorMsg 是错误信息字符串(响应状态的文本描述部分，例如'Not Found'或'Internal Server Error')
            error: function (jqXHR, textStatus, errorMsg) {
                switch (jqXHR.status) {
                    case 404: alert('链接地址错误!', null, 'error'); break;
                    case 500: alert('服务器内部错误!', null, 'error'); break;
                    default:  alert(jqXHR.status + ":" + jqXHR.statusText, null, 'error');
                }
            } 
        });
    }


    //提供给外部的api接口
    var res = {
        routeRegister: routeRegister,
        route: route,
        alert: alert,
        confirm: confirm,
        dialog: dialog,
        Ajax: Ajax,
        button: button
    }
    return res;
}

Array.prototype.toObject = function () {
    var obj = {};
    for (var i = 0; i < this.length; i++) {
        var key = this[i]["name"];
        var value = this[i]["value"];
        obj[key] = value;
    }
    return obj;
}