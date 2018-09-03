
///----------滑动加载插件------------///

///author:汤台----date:2016-3-28----///

(function ($) {
    $.extend({
        //滑动加载
        'slidepage': function (option) {
            //默认值
            var defaults = {
                url: '',//请求的路径
                page: 1,//页面
                rows: 10,//每页的数量
                params: '',//额外的参数
                sort: 'ID',
                callback: null//回调函数
            }
            var STATE = true;//是否加载标志
            option = $.extend(defaults, option);//替换默认值
            Loading();//第一次加载

            $(window).scroll(function () {//绑定滚动事件
                if ($(window).scrollTop() == $(document).height() - $(window).height()) {
                    if (STATE) {
                        Loading();
                    }
                }
            });

            //滑动请求
            function Loading() {
                $.ajax({
                    url: option.url,
                    type: 'POST',
                    dataType: 'json',
                    data: option.params + '&page=' + option.page + '&rows=' + option.rows,
                    success: function (data) {
                        if (data.rows.length > 0) {
                            option.callback.call(this, data);//(js的call调用一个对象的一个方法，以另一个对象替换当前对象（函数可以看成一个对象)
                            option.page = option.page + 1;
                        }
                        else {
                            STATE = false;
                        }
                    }
                });
            }
        }
    });
})(jQuery)