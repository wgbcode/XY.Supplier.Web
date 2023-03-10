#pragma checksum "D:\GIT.NEWARE.WORK\neware-mes\Sources\XY.Supplier.Web\Views\Common\Main.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "d23ebc18dc6e57e0ff42fb2b5142f9a9c5b683d7"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Views_Common_Main), @"mvc.1.0.view", @"/Views/Common/Main.cshtml")]
namespace AspNetCore
{
    #line hidden
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using Microsoft.AspNetCore.Mvc.ViewFeatures;
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"d23ebc18dc6e57e0ff42fb2b5142f9a9c5b683d7", @"/Views/Common/Main.cshtml")]
    public class Views_Common_Main : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<XY.Supplier.Web.Models.Common.Main_Model>
    {
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
            WriteLiteral("\r\n");
#nullable restore
#line 3 "D:\GIT.NEWARE.WORK\neware-mes\Sources\XY.Supplier.Web\Views\Common\Main.cshtml"
  
    Layout = "~/Views/Shared/Normal.cshtml";

#line default
#line hidden
#nullable disable
            WriteLiteral(@"
<style>
    html, body {
        height: 100%;
    }
</style>

<script type=""text/javascript"">
    //<![CDATA[

    //功能菜单点击事件
    function menuClick(pageID, pageName, pageUrl) {
        require(['jquery', 'boss'], function ($, boss) {
            boss.tabAdd(pageID, pageName, pageUrl);
        });
    }

    require(['jquery', 'xy', 'boss'], function ($, xy, boss) {

        $(function () {

            //Tab初始化
            boss.tabInit();

            //刷新
            $('#page_refresh').on('click', function () {
                boss.tabRefresh();
            });

            //退出
            $('#signout').on('click', function () {
                if (confirm('您确认退出吗？')) {
                    xy.go('/signout');
                }
            });

            //菜单控制（隐藏与显示）
            $('#boss_top_menu').on('click', function () {
                var _left = $('#boss_left');
                var _main = $('#boss_main');
                if (_left.hasClass('hide')) {
     ");
            WriteLiteral(@"               //显示菜单
                    _left.removeClass('hide');
                    _main.css('paddingLeft', '180px');
                    $(this).attr('title', '隐藏菜单');
                }
                else {
                    //隐藏菜单
                    _left.addClass('hide');
                    _main.css('paddingLeft', '0px');
                    $(this).attr('title', '显示菜单');
                }
            });

            //功能菜单
            var _menu = $('#boss_menu');
            //标题鼠标点击事件
            _menu.find("".boss_menu_item_t"").click(function () {
                var _c = $(this).next();
                //当前
                if (_c.css('display') == 'block') {
                    _c.slideUp(200);
                    $(this).children('.fa-angle-up').css('transform', 'rotate(0)')
                }
                //非当前
                else {
                    //如果存在未收缩则先收缩
                    _menu.find("".boss_menu_item_c"").each(function () {
                      ");
            WriteLiteral(@"  if ($(this).css('display') == 'block') {
                            $(this).slideUp(200);
                            $(this).prev().children('.fa-angle-up').css('transform', 'rotate(0)')
                        }
                    });
                    //然后展开
                    _c.slideDown(200);
                    $(this).find('.fa-angle-up').css('transform', 'rotate(-180deg)')
                }
            });
            //左侧菜单
            $('.boss_menu_link').on('click', function () {
                sessionStorage.removeItem('pageNumber') // 清除表格页码缓存

                $('.boss_menu_link').removeClass('active')
                $(this).addClass('active')
            });

            //默认选中第一个二级菜单
            var _arrow = $($('.boss_menu_item_t')[0]).find('.fa-angle-up')
            _arrow.css('transform', 'rotate(-180deg)')
            $($('.boss_menu_link')[0]).addClass('active')

            $('#page_refresh').click();

        });
    });

    //]]>
</script>

<di");
            WriteLiteral(@"v class=""boss"">

    <!--TOP-->
    <div class=""boss_top"">
        <div id=""boss_top_menu"" title=""隐藏菜单"">
            <i class=""fas fa-bars"" aria-hidden=""true""></i>
        </div>
        <div class=""boss_top_logo""><img src=""/images/logo_word.png"" /> <span class=""logo-word mr8 ml5"">WMS</span><span class=""date-logo"">");
#nullable restore
#line 112 "D:\GIT.NEWARE.WORK\neware-mes\Sources\XY.Supplier.Web\Views\Common\Main.cshtml"
                                                                                                                                     Write(XY.Supplier.Web.Tools.Version);

#line default
#line hidden
#nullable disable
            WriteLiteral("</span></div>\r\n");
            WriteLiteral(@"
        <div class=""fr"">
            <div id=""page_refresh"" class=""boss_top_btn"">
                <i class=""fas fa-sync-alt"" aria-hidden=""true""></i>
                <span>刷新页面</span>
            </div>
            <div id=""signout"" class=""boss_top_btn"">
                <i class=""fas fa-sign-out-alt"" aria-hidden=""true""></i>
                <span>安全退出</span>
            </div>
        </div>
    </div>

    <div class=""boss_main"" id=""boss_main"">
        <!--LEFT-->
        <div class=""boss_left"" id=""boss_left"">
            <div class=""boss_menu"" id=""boss_menu"">

                <div class=""boss_menu_item"">
                    <div class=""boss_menu_item_t"">
                        <i class=""fl fas fa-user mr5 w16""></i>
                        <span>账号中心</span>
                        <i class=""fr fas fa-angle-up""></i>
                    </div>
                    <div class=""boss_menu_item_c"" style=""display:block;"">
");
            WriteLiteral(@"                        <a href=""javascript:void(0);"" class=""boss_menu_link"" onclick=""menuClick('myAccount', '账号资料', '/my/account');"">
                            <i class=""fl fas fa-list mr5 w16""></i>账号资料
                        </a>
                        <a href=""javascript:void(0);"" class=""boss_menu_link"" onclick=""menuClick('myPassword', '修改密码', '/my/password');"">
                            <i class=""fl fas fa-list mr5 w16""></i>修改密码
                        </a>
                    </div>
                </div>

                <div class=""boss_menu_item"">
                    <div class=""boss_menu_item_t"">
                        <i class=""fl fas fa-shopping-cart mr5 w16""></i>
                        <span>订单管理</span>
                        <i class=""fr fas fa-angle-up""></i>
                    </div>
                    <div class=""boss_menu_item_c"" style=""display:none;"">
                        <a href=""javascript:void(0);"" class=""boss_menu_link"" onclick=""menuClick('purchaseOrder', '采购");
            WriteLiteral(@"订单', '/purchase/order');"">
                            <i class=""fl fas fa-list mr5 w16""></i>采购订单
                        </a>
                        <a href=""javascript:void(0);"" class=""boss_menu_link"" onclick=""menuClick('purchaseQuote', '报价', '/purchase/quote');"">
                            <i class=""fl fas fa-list mr5 w16""></i>报价
                        </a>
                        <a href=""javascript:void(0);"" class=""boss_menu_link"" onclick=""menuClick('transportOrder', '物流订单', '/transport/order');"">
                            <i class=""fl fas fa-list mr5 w16""></i>物流订单
                        </a>
                    </div>
                </div>

            </div>
        </div>

        <!--RIGHT-->
        <div class=""boss_right"">
            <!--tabs-->
            <div id=""boss_tabs"" class=""boss_tabs"">
                <div class=""boss_tabs_bg""></div>
                <div id=""boss_tabs_prev"" class=""boss_tabs_btn"" style=""left:0px;"">
                    <i class=""fas fa-angle-dou");
            WriteLiteral(@"ble-left""></i>
                </div>
                <div id=""boss_tabs_next"" class=""boss_tabs_btn"" style=""right:38px;border-left:1px #ddd solid;"">
                    <i class=""fas fa-angle-double-right""></i>
                </div>
                <div id=""boss_tabs_more"" class=""boss_tabs_more"">
                    <div class=""boss_tabs_btn"" style=""right:0px;"">
                        <i class=""fas fa-angle-down""></i>
                    </div>
                    <div class=""boss_tabs_more_hide hide"">
                        <a href=""javascript:void(0)"">关闭当前标签页</a>
                        <a href=""javascript:void(0)"">关闭其它标签页</a>
                        <a href=""javascript:void(0)"">关闭全部标签页</a>
                    </div>
                </div>
                <div id=""boss_tabs_box"" class=""boss_tabs_box"" style=""left:38px;"">
                    <div class=""boss_tab boss_tab_curr"" id=""myDefault"">
");
            WriteLiteral(@"                        <span>主页</span>
                    </div>
                </div>
            </div>
            <div class=""boss_frame"">
                <!--loading-->
                <div class=""boss_iframe_mask hide"">
                    <img src=""/images/loading.gif"" style=""max-width:30px;"" alt=""加载中..."" />
                </div>
                <!--iframe-->
                <div class=""boss_iframe"" id=""boss_iframe"">
                    <iframe id=""iframe_my_account"" class=""boss_iframe_active"" src=""/my/account""></iframe>
                </div>

            </div>
        </div>
    </div>

</div>");
        }
        #pragma warning restore 1998
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.ViewFeatures.IModelExpressionProvider ModelExpressionProvider { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.IUrlHelper Url { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.IViewComponentHelper Component { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.Rendering.IJsonHelper Json { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper<XY.Supplier.Web.Models.Common.Main_Model> Html { get; private set; }
    }
}
#pragma warning restore 1591
