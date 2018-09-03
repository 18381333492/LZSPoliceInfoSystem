/**
 * common.client.js 1.0.0
 * Most modern mobile touch slider and framework with hardware accelerated transitions
 * 
 * Copyright 2016-forever, TangTai
 * args-ajax
 * params-dialog
 * 
 * Released on: March 10, 2017
 */
(function ($) {
    //check window.client
    if (!window.client) {
        window.client = {
            ajax: new ajax(),
            form: new form(),
            string: new string(),
            regex: new regex(),
            cookie: new cookie(),
            datetime: new datetime(),
            localStorage: new storage()
        }
        
        /****************************
         * 前端常用的对象的封装
         *
        ****************************/

        function regex() {

            //验证是否为空
            function isEmpty(val) {
                val = val.replace(/(^\s*)|(\s*$)/g, "");//去掉前后空格
                if (val.length == 0)
                    return true;
                return false;
            }

            //是否手机号码格式
            function isPhone(val) {
                val = val.replace(/\D/g, '');//去掉所有的空格
                var regex = /^((13[0-9]|14[0-9]|15[0-9]|17[0-9]|18[0-9])\d{8})*$/;
                if (val.length > 0 && regex.exec(val))
                    return true;
                return false;
            }

            //验证是否是邮箱
            function isEmail(val) {
                var regex = /^([a-zA-Z0-9]+[_|\_|\.]?)*[a-zA-Z0-9]+@([a-zA-Z0-9]+[_|\_|\.]?)*[a-zA-Z0-9]+\.[a-zA-Z]{2,3}$/;
                if (regex.test(val))
                    return true;
                return false;
            }

            //验证是否是数字
            function isNumber(val) {
                var regex = /^[0-9]*$/;
                if (regex.test(val))
                    return true;
                return false;
            }

            //验证是否是金额
            function isMoney(val) {
                var regex = /(^[1-9]([0-9]+)?(\.[0-9]{1,2})?$)|(^(0){1}$)|(^[0-9]\.[0-9]([0-9])?$)/;
                if (regex.test(val))
                    return true;
                return false;
            }

            //验证是否是数字和字母的组合(忽略大小写)
            function isLetterNum(val) {
                var regex = /^[A-Za-z0-9]+$/;
                if (regex.test(val))
                    return true;
                return false;
            }
            /**返回的接口**/
            return {
                isEmpty: isEmpty,
                isPhone: isPhone,
                isEmail: isEmail,
                isNumber: isNumber,
                isMoney:isMoney,
                isLetterNum: isLetterNum
            }

        }

        function string() {

            /*去掉字符串前后字符*/
            function trim(val, char) {
                char = char == undefined ? "" : char;
                return val.replace(/(^\s*)|(\s*$)/g, char);
            }

            /*去掉字符串开始的字符*/
            function trimSta(val, char) {
                char = char == undefined ? "" : char;
                return val.replace(/(^\s*)/g, char);
            }

            /*去掉字符串结束的字符*/
            function trimEnd(val, char) {
                char = char == undefined ? "" : char;
                return val.replace(/(\s*$)/g, char);
            }

            /*
            *字符串的插入
            *{params:val(要插入的字符串),
                     staIndex(开始擦入的索引)
                     str(插入的字符串)}
            */
            function insert(val, staIndex, str) {
                var strArray = val.split("");
                if (staIndex < strArray.length) {
                    strArray[staIndex] = str + strArray[staIndex];
                }
                return strArray.join("");
            }

            /**返回的接口**/
            return {
                trim: trim,
                insert: insert,
                trimSta: trimSta,
                trimEnd: trimEnd
            }
        }

        /****注意：js操作浏览器cookie的封装****/
        function cookie() {
            /*!
            * jQuery Cookie Plugin v1.4.1  (cookie的封装)
            * https://github.com/carhartl/jquery-cookie
            *
            * Copyright 2013 Klaus Hartl
            * Released under the MIT license
            */
            (function (n) { typeof define == "function" && define.amd ? define(["jquery"], n) : typeof exports == "object" ? n(require("jquery")) : n(jQuery) })(function (n) { function i(n) { return t.raw ? n : encodeURIComponent(n) } function f(n) { return t.raw ? n : decodeURIComponent(n) } function e(n) { return i(t.json ? JSON.stringify(n) : String(n)) } function o(n) { n.indexOf('"') === 0 && (n = n.slice(1, -1).replace(/\\"/g, '"').replace(/\\\\/g, "\\")); try { return n = decodeURIComponent(n.replace(u, " ")), t.json ? JSON.parse(n) : n } catch (i) { } } function r(i, r) { var u = t.raw ? i : o(i); return n.isFunction(r) ? r(u) : u } var u = /\+/g, t = n.cookie = function (u, o, s) { var y, a, h, v, c, p; if (o !== undefined && !n.isFunction(o)) return s = n.extend({}, t.defaults, s), typeof s.expires == "number" && (y = s.expires, a = s.expires = new Date, a.setTime(+a + y * 864e5)), document.cookie = [i(u), "=", e(o), s.expires ? "; expires=" + s.expires.toUTCString() : "", s.path ? "; path=" + s.path : "", s.domain ? "; domain=" + s.domain : "", s.secure ? "; secure" : ""].join(""); for (h = u ? undefined : {}, v = document.cookie ? document.cookie.split("; ") : [], c = 0, p = v.length; c < p; c++) { var w = v[c].split("="), b = f(w.shift()), l = w.join("="); if (u && u === b) { h = r(l, o); break } u || (l = r(l)) === undefined || (h[b] = l) } return h }; t.defaults = {}; n.removeCookie = function (t, i) { return n.cookie(t) === undefined ? !1 : (n.cookie(t, "", n.extend({}, i, { expires: -1 })), !n.cookie(t)) } });

            //设置cookie tip:expires(保存的天数)
            function setCookie(name, value, expires) {
                if (name) {
                    value = JSON.stringify(value);
                    jQuery.cookie(name, value, { expires: expires });
                }
            }

            //获取cookie
            function getCookie(name) {
                if (name) {
                    var value = jQuery.cookie(name);
                    if (value)
                        value = JSON.parse(value);
                    return value;
                }
            }

            //移除cookie
            function removeCookie(name) {
                if (name) {
                    jQuery.removeCookie(name);
                }
            }
            //返回的接口
            return {
                setCookie: setCookie,
                getCookie: getCookie,
                removeCookie: removeCookie
            }
        }

        /****注意：浏览器的localStorage的最大空间只有5M****/
        function storage() {

            // 检查浏览器是否支持localStorage
            function isSupport() {
                if (window.localStorage) {
                    return true;
                }
                else {
                    return alert("浏览器不支持localStorage,请升级浏览器!");
                }
            }

            /*设置localStorage*/
            function setStorage(name, value) {
                if (isSupport() && name) {
                    value = JSON.stringify(value);
                    window.localStorage.setItem(name, value);
                }
            }

            /*获取localStorage*/
            function getStorage(name) {
                if (isSupport() && name) {
                    var value = window.localStorage.getItem(name);
                    if (value)
                        value = JSON.parse(value);
                    return value;
                }
            }

            /*移除localStorage*/
            function removeStorage(name) {
                if (isSupport() && name) {
                    window.localStorage.removeItem(name);
                }
            }

            /**返回的接口**/
            return {
                setStorage: setStorage,
                getStorage: getStorage,
                removeStorage: removeStorage
            }

        }

        /*前端ajax的封装*/
        function ajax() {

            //ajax的get请求
            function getRequest(url, params, success, fail, async) {
                ajaxRequest(url, params, success, fail, async, "GET");
            }

            //ajax的post请求
            function postRequest(url, params, success, fail, async) {
                ajaxRequest(url, params, success, fail, async, "POST");
            }

            function ajaxRequest(url, params, success, fail, async, type) {
                $.ajax({
                    url: url,
                    data: params,
                    type: type,
                    dataType: 'json',
                    async: (async == null) ? true : async,
                    success: function (r) {
                        if (r.success) {
                            success(r);
                        }
                        else {
                            if (fail) {
                                fail(r);//手动提示错误
                            }
                            else
                                alert(r.info);
                        }
                    },
                    // jqXHR 是经过jQuery封装的XMLHttpRequest对象
                    // textStatus 可能为null、 'timeout（超时）'、 'error（错误）'、 'abort(中止)'和'parsererror（解析错误)'等
                    // errorMsg 是错误信息字符串(响应状态的文本描述部分，例如'Not Found'或'Internal Server Error')
                    error: function (jqXHR, textStatus, errorMsg) {
                        switch (jqXHR.status) {
                            case 404: alert('链接地址错误!'); break;
                            case 500: alert('服务器内部错误!'); break;
                        }
                    }
                });
            }

            return {
                getRequest: getRequest,
                postRequest: postRequest
            }
        }

        /****tip：js对日期的处理****/
        function datetime() {

            /*
            * js时间的格式化.
            * @version 1.0.0
            * @param   date(要格式化的时间字符串或日期), format(要格式的格式类型)
            * format("yyyy-MM-dd hh:mm:ss.S") ==> 2006-07-02 08:09:04.423
            * format("yyyy-M-d h:m:s.S")      ==> 2006-7-2 8:9:4.18 
            * @return {string}
            */
            function format(date, format) {
                if (typeof (date) === "string")
                    date = new Date(date.replace(/-/g, "/"));//处理苹果时间格式兼容性的问题
                var o = {
                    "M+": date.getMonth() + 1, //月份 
                    "d+": date.getDate(), //日 
                    "h+": date.getHours(), //小时 
                    "m+": date.getMinutes(), //分 
                    "s+": date.getSeconds(), //秒 
                    "q+": Math.floor((date.getMonth() + 3) / 3), //季度 
                    "S": date.getMilliseconds() //毫秒 
                };
                for (var time in o) {
                    if (isNaN(o[time])) {
                        return "";
                    }
                }
                if (/(y+)/.test(format))
                    format = format.replace(RegExp.$1, (date.getFullYear() + "").substr(4 - RegExp.$1.length));
                for (var k in o)
                    if (new RegExp("(" + k + ")").test(format))
                        format = format.replace(RegExp.$1, (RegExp.$1.length == 1) ? (o[k]) : (("00" + o[k]).substr(("" + o[k]).length)));
                return format;
            }

            //获取当月的第一天
            function getCurrentMonthFirstDay(value) {
                var date = value == undefined ? new Date() : value;
                date.setDate(1);
                return date;
            }

            //获取某月的有多少天
            function GetDaysInMonth(value) {
                var date = value == undefined ? new Date() : value;
                date = typeof (value) == "string" ? new Date(date) : date;
                return new Date(date.getFullYear(), date.getMonth() + 1, 0).getDate();
            }

            //返回的接口
            return {
                format: format,
                GetDaysInMonth: GetDaysInMonth,
                getCurrentMonthFirstDay: getCurrentMonthFirstDay
            }
        }

        /****表单的处理****/
        function form() {

            /*
            * 将表单的值序列化成Json对象.
            * @version 1.0.0
            * @param   form(要序列化Jquery表单对象)
            * @return {json}
            */
            function parseJson(form) {
                var array = form.serializeArray();
                var obj = {};
                for (var i = 0; i < array.length; i++) {
                    var key = array[i]["name"];
                    var value = array[i]["value"];
                    obj[key] = value;
                }
                return obj;
            }

            return {
                parseJson: parseJson
            }
        }
    }
    else return;
})(jQuery)