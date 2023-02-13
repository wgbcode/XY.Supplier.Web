//<![CDATA[

/************************************************************
入口模块
************************************************************/

//全局配置变量
var configs = {
    'site_domain': 'supplier.xy.com',
    'site_url': 'http://supplier.xy.com:8004'
};

//设置域
if (configs.site_domain.length > 0)
    document.domain = configs.site_domain;

//require模块
require.config({

    //预定义
    paths: {
        //// http://jquery.com
        //jquery: configs.site_url + "/scripts/jquery.min",
        //// http://www.bootcss.com
        //bootstrap: configs.site_url + "/scripts/bootstrap4/js/bootstrap",
        ////https://popper.js.org/
        //popper: configs.site_url + "/scripts/popper/popper.1.12.9.min",
        //// http://www.layui.com/laydate
        //laydate: configs.site_url + "/scripts/laydate/laydate",
        //// http://layer.layui.com
        //layer: configs.site_url + "/scripts/layer/layer",
        //// https://www.bootstrapselect.cn/
        //bootstrapSelect: configs.site_url + "/scripts/bootstrapSelect/js/bootstrap-select",
        //hoverDelay: configs.site_url + "/scripts/hoverDelay/hoverDelay",
        //// https://github.com/highlightjs/highlight.js
        ////<link type="text/css" rel="stylesheet" href="/scripts/highlightjs/styles/default.css">
        //hljs: configs.site_url + "/scripts/highlightjs/highlight.pack",
        //swiper: configs.site_url + "/scripts/swiper/js/swiper",

        // http://jquery.com
        jquery:"/scripts/jquery.min",
        // http://www.bootcss.com
        bootstrap:"/scripts/bootstrap4/js/bootstrap",
        //https://popper.js.org/
        popper:"/scripts/popper/popper.1.12.9.min",
        // http://www.layui.com/laydate
        laydate:"/scripts/laydate/laydate",
        // http://layer.layui.com
        layer: "/scripts/layer/layer",
        // https://www.bootstrapselect.cn/
        bootstrapSelect:"/scripts/bootstrapSelect/js/bootstrap-select",
        hoverDelay:"/scripts/hoverDelay/hoverDelay",
        // https://github.com/highlightjs/highlight.js
        //<link type="text/css" rel="stylesheet" href="/scripts/highlightjs/styles/default.css">
        hljs:"/scripts/highlightjs/highlight.pack",
        swiper:"/scripts/swiper/js/swiper",

        bootstrapTable: "/scripts/bootstrapTable/js/bootstrap-table.min",
        bootstrapTableEditor: "/scripts/bootstrapTable/js/bootstrap-table-editor",
        bootstrapTableLocale: "/scripts/bootstrapTable/js/bootstrap-table-zh-CN.min",

        xy: configs.site_url + "/scripts/xy.min",
        xyvalid: configs.site_url + "/scripts/xyvalid.min",
        boss: configs.site_url + "/scripts/boss" ///scripts/boss.min
        //print: configs.site_url + "/scripts/Print/print.min"
    },

    //非CMD规范模块
    shim: {
        'bootstrap': {
            deps: ['jquery', 'popper'],
            exports: 'bootstrap'
        },
        'popper': {
            //deps: ['jquery'],
            exports: 'popper'
        },
        'xyvalid': {
            deps: ['jquery'],
            exports: 'xyvalid'
        },
        'layer': {
            deps: ['jquery'],
            exports: 'layer'
        },
        'bootstrapSelect': {
            deps: ['jquery', 'popper', 'bootstrap'],
            exports: 'bootstrapSelect'
        },
        'hoverDelay': {
            deps: ['jquery'],
            exports: 'hoverDelay'
        },
        'hljs': {
            deps: ['jquery'],
            exports: 'hljs'
        },
        'swiper': {
            deps: ['jquery'],
            exports: 'swiper'
        },
        'bootstrapTable': {
            deps: ['jquery', 'bootstrap'],
            exports: 'bootstrapTable'
        },
        'bootstrapTableEditor': {
            deps: ['jquery', 'bootstrap', 'bootstrapTable'],
            exports: 'bootstrapTableEditor'
        },
        'bootstrapTableLocale': {
            deps: ['bootstrapTable'],
            exports: 'bootstrapTableLocale'
        },
    }

});

//]]>