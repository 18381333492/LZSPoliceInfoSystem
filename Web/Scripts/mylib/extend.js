

/*
**tip:基于easyui插件的重写和扩展
**date:[2017-8-2]
**author:[TT]
*/

void function () {

    $.extend($.fn.textbox.defaults, {
       // height: 30,
       // width: 200,
       // required: true,
        missingMessage:'亲,该项为必填项哦'
    });

    //$.extend($.fn.numberbox.defaults, {
    //    height: 30,
    //    width: 200,
    //    required: true,
    //    missingMessage: '亲,该项为必填项哦'
    //});

    $.extend($.fn.combobox.defaults, {
        height: 30,
        width: 200,
        panelHeight: 'auto',
        editable: false
    });


    //重写datebox的formatter **处理两次的formatter***
    $.extend($.fn.datebox.defaults, {
        formatter: function (date) {
            var y = date.getFullYear();
            var m = date.getMonth() + 1;
            var d = date.getDate();
            return y + '-' + (m < 10 ? ('0' + m) : m) + '-' + (d < 10 ? ('0' + d) : d);
        },
        parser: function (s) {
            if (!s) return new Date();
            var ss = (s.split('-'));
            var y = parseInt(ss[0], 10);
            var m = parseInt(ss[1], 10);
            var d = parseInt(ss[2], 10);
            if (!isNaN(y) && !isNaN(m) && !isNaN(d)) {
                return new Date(y, m - 1, d);
            } else {
                return new Date();
            }
        }
    });

    //重写datebox的按钮
    $.extend($.fn.datebox.defaults, {
        editable:false,//禁止手动输入
        buttons: [{
            text: '今天',
            handler: function (target,date) {
                $(target).datebox('setValue', new Date().toString());
                $(target).datebox('hidePanel');//隐藏组件
               }
            },
            {
            text: '清空',
            handler: function (target) {
                $(target).datebox('setValue', "");
                $(target).datebox('hidePanel');//隐藏组件
            }
            },
            {
            text: '关闭',
            handler: function (target) {
                $(target).datebox('hidePanel');//隐藏组件
            }
            }
        ]
    });


    /**
    *重写分页控件的显示
    */
    $.extend($.fn.pagination.defaults, {
        beforePageText: '第',//页数文本框前显示的汉字 
        afterPageText: '页    共 {pages} 页',
        displayMsg: '当前显示 {from} - {to} 条记录  共{total}条记录',
    });

    /**
    *重写datagrid
    */
    
    $.extend($.fn.datagrid.defaults, {
        fit: true,
        border: false,
        singleSelect: false,
        rownumbers: true,
        striped: true,
        pagination: true,
        loadMsg: "正在加载...",
        pageSize: 20,
    });

    //重写validatebox验证规则
    $.extend($.fn.validatebox.defaults.rules, {
        isPhone: {
            validator: function (value, param) {
                var regex = /^((13[0-9]|14[0-9]|15[0-9]|17[0-9]|18[0-9])\d{8})*$/;
                return regex.exec(value);
            },
            message: '亲,手机号码格式错误!'
        },
        equals: {
            validator: function (value, param) {
                return value == $(param[0]).val();
            },
            message: '两次密码输入不一致'
        }
        
    });


}();
