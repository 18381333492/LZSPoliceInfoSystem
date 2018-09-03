
/**
/* @description:对js时间处理的封装
/* @author:[汤台]
/* @time:2016-09-19
/* @version:1.0.0
*/

function datetime() {

    /*
    * js时间的格式化.
    * @version 1.0.0
    * @param   date(要格式化的时间字符串), format(要格式的格式类型)
    * format("yyyy-MM-dd hh:mm:ss.S") ==> 2006-07-02 08:09:04.423
    * format("yyyy-M-d h:m:s.S")      ==> 2006-7-2 8:9:4.18 
    * @return {string}
    */
    function format(date, format) {
        var fmt = new Date(date.replace(/-/g, "/"));//处理苹果时间格式兼容性的问题
        var o = {
            "M+": this.getMonth() + 1, //月份 
            "d+": this.getDate(), //日 
            "h+": this.getHours(), //小时 
            "m+": this.getMinutes(), //分 
            "s+": this.getSeconds(), //秒 
            "q+": Math.floor((this.getMonth() + 3) / 3), //季度 
            "S": this.getMilliseconds() //毫秒 
        };
        for (var time in o) {
            if (isNaN(o[time])) {
                return "";
            }
        }
        if (/(y+)/.test(fmt))
            fmt = fmt.replace(RegExp.$1, (this.getFullYear() + "").substr(4 - RegExp.$1.length));
        for (var k in o)
            if (new RegExp("(" + k + ")").test(fmt))
                fmt = fmt.replace(RegExp.$1, (RegExp.$1.length == 1) ? (o[k]) : (("00" + o[k]).substr(("" + o[k]).length)));
        return fmt;
    }

    return {
        format: format
    }
}