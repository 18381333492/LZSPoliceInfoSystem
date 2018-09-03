
//********************************多张图片上传   Tip: 支持火狐,谷歌浏览器以及IE10+以上 ie9及以下不支持*****************************************//
//********************************多张图片上传   Tip: 支持火狐,谷歌浏览器以及IE10+以上 ie9及以下不支持*****************************************//
    /**
    /* 引用的Js:
    /* jquery.easyui.min.js
    /* @description:多张张图片上传插件的封装
    /* @author:[汤台]
    /* @time:2016-09-19
    /* @version:1.0.0
    */
function dpictures() {

    var sUrlArray = [];//全局变量

    var element;
    //默认参数
    var defaults = {
        url: "/Upload/Upload.ashx",
        width: 120,
        height: 120,
        path: '',//上传的目录
        VirtualDirectory: '',//虚拟目录
    }

    /*!
    * method:创建图片上传插件
    * author:[汤台]
    * version:[1.0.0]
    */
    function create(id, option) {
        //替换默认参数
        defaults = $.extend(defaults, option);
        //设置虚拟目录
        defaults.url = defaults.VirtualDirectory + defaults.url;
        element = document.getElementById(id);
        init();
    }


    /*!
    * method:图片插件初始化
    * author:[汤台]
    * version:[1.0.0]
    */
    function init() {
        $(element).load(defaults.VirtualDirectory + "/Scripts/plug-in/dpictures/dpictures.html",
            function (data) {
                $(element).html("");
                data = data.trim();
                $(element).append(data);//追加html
                //设置图片按钮的样式和图片路径
                $(element).find('.picture_btn').height(defaults.height);
                $(element).find('.picture_btn').width(defaults.width);
                $(element).find('.picture_btn').attr("src", defaults.VirtualDirectory + "/Scripts/plug-in/dpictures/dpictures.png");
                //绑定图片上传事件
                bingEvent();

                //初始化图片的显示
                if ($(element).next().val())
                    appendHtml($(element).next().val());
            });
    }

    /*!
    * method:绑定事件
    * author:[汤台]
    * version:[1.0.0]
    */
    function bingEvent() {
        $(element).find(".picture_btn").on("click", function () {
            $(element).find("input[name=d_picture_file]").click();
        });
        //值发生改变触发
        $(element).find("input[name=d_picture_file]").on("change", function () {
            if ($(this).val())
                Up();
        });
    }

    var orderIndex = 0;
    //绑定删除和跟换事件
    function bingCancelandUpdate() {
        
        //避免重复绑定事件先接触绑定
        $(element).find(".delete_img").unbind("click");
        $(element).find(".s_picture_btn").unbind("click");
        $(element).find("input[name=s_picture_file]").unbind("change");

        //删除事件的绑定
        $(element).find(".delete_img").on("click", function () {
            //当前li标签所有在索引
            var Index = $(this).parent().index();
            //移除元素
            $(this).parent().remove();
            //移除值
            sUrlArray.splice(Index, 1);
            //修改值
            $(element).next().val(sUrlArray.join());
        });
        //单图片上传绑定
        $(element).find(".s_picture_btn").on("click", function () {
            orderIndex = $(this).parent().index();
            $(element).find("input[name=s_picture_file]").click();
        });
        //值发生改变触发
        $(element).find("input[name=s_picture_file]").on("change", function () {
            if ($(this).val())
                Up(true, orderIndex);//单图片上传
        });
    }


    //上传成功之后显示图片
    function appendHtml(url) {
        if (url) {
            var html = [];
            url = url.split(',');
            $(url).each(function () {
                html.push('<li>');
                html.push('<img class="s_picture_btn" src="' + defaults.VirtualDirectory + this + '" style="height:' + defaults.height + 'px; width:' + defaults.width + 'px" />');
                html.push('<img src="' + defaults.VirtualDirectory + '/Scripts/plug-in/dpictures/close.png" class="delete_img" />');
                html.push('</li>');
                sUrlArray.push(this);
            });
            $(html.join('')).insertBefore($('.picture_btn', $(element)).parent());
            ///绑定删除和跟换事件
            bingCancelandUpdate();
        }
    }

    /*!
    * method:上传图片
    * author:[汤台]
    * version:[1.0.0]
    */
    function Up(bIsSingle, orderIndex) {
        var formName = bIsSingle == true ? ".s_picture_form" : ".d_picture_form";
        //利用easyui的表单提交上传图片
        $(element).find(formName).form('submit', {
            url: defaults.url,
            queryParams: { path: defaults.path },//额外参数
            success: function (data) {
                data = JSON.parse(data);
                if (data.error == 0) {
                    if (bIsSingle) {
                        //单图片返回结果处理
                        $(element).find('li').eq(orderIndex).find(".s_picture_btn").attr("src", defaults.VirtualDirectory + data.url);
                        sUrlArray[orderIndex] = data.url;
                    }
                    else
                        appendHtml(data.url);
                    //更新值
                    $(element).next().val(sUrlArray.join());
                } else {
                    alert(data.message);
                }
            }
        });
    }

    return {
        create: create,
    }
}
