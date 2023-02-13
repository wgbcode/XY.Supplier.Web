#pragma checksum "D:\GIT.NEWARE.WORK\neware-mes\Sources\XY.Supplier.Web\Views\Purchase\Quote.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "663c9035604c06dc7d68893535bda840664851d7"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Views_Purchase_Quote), @"mvc.1.0.view", @"/Views/Purchase/Quote.cshtml")]
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
#nullable restore
#line 1 "D:\GIT.NEWARE.WORK\neware-mes\Sources\XY.Supplier.Web\Views\Purchase\Quote.cshtml"
using XY.Supplier.Web;

#line default
#line hidden
#nullable disable
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"663c9035604c06dc7d68893535bda840664851d7", @"/Views/Purchase/Quote.cshtml")]
    public class Views_Purchase_Quote : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<XY.Supplier.Web.Models.Purchase.Quote_Model>
    {
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
            WriteLiteral("\r\n");
#nullable restore
#line 4 "D:\GIT.NEWARE.WORK\neware-mes\Sources\XY.Supplier.Web\Views\Purchase\Quote.cshtml"
  
    Layout = "~/Views/Shared/Normal.cshtml";

#line default
#line hidden
#nullable disable
            WriteLiteral(@"
<script type=""text/javascript"">
    //<![CDATA[

    function step(ID,pqID) {
        require(['xy', 'boss'], function (xy, boss) {
            boss.load('/purchase/Quote_Detail?inquiryID=' + ID + '&quoteID=' + pqID + '&backurl=' + xy.url(true));
        });
    }

    require(['jquery', 'xy', 'boss', 'bootstrap', 'bootstrapSelect', 'laydate'], function ($, xy, boss, bootstrap, bootstrapSelect, laydate) {
        $(function () {
            //初始化日期样式
            var dBegin = '");
#nullable restore
#line 20 "D:\GIT.NEWARE.WORK\neware-mes\Sources\XY.Supplier.Web\Views\Purchase\Quote.cshtml"
                      Write(DateTime.Now.AddMonths(-1).ToString("yyyy.MM.dd 00:00:00"));

#line default
#line hidden
#nullable disable
            WriteLiteral("\'\r\n            var dEnd = \'");
#nullable restore
#line 21 "D:\GIT.NEWARE.WORK\neware-mes\Sources\XY.Supplier.Web\Views\Purchase\Quote.cshtml"
                    Write(DateTime.Now.ToString("yyyy.MM.dd 23:59:59"));

#line default
#line hidden
#nullable disable
            WriteLiteral(@"'

            //初始化日期样式
            laydate.render({
                elem: '#Begin',
                theme: boss.configPrimaryColor,
                type: 'datetime',
                value: dBegin,
                format: 'yyyy.MM.dd HH:mm:ss'
            });
            laydate.render({
                elem: '#End',
                theme: boss.configPrimaryColor,
                type: 'datetime',
                value: dEnd,
                format: 'yyyy.MM.dd HH:mm:ss'
            });
            boss.tableInit($('#tablePur'));

            //点击查询
            $('#Search').on('click', function () {
                boss.load(""/purchase/quote?status="" + $(""#OrderStatus"").val() + ""&userId="" + $(""#UserId"").val() + ""&orderKey="" + $(""#OrderKey"").val()
                    + ""&begin="" + xy.urlencode($(""#Begin"").val()) + ""&end="" + xy.urlencode($(""#End"").val())  + ""&pagesize="" + $(""#Pagesize"").val() + '&backurl=' + xy.url(true));
            });

            //初始化下拉模糊匹配
            $('.selec");
            WriteLiteral(@"tpicker').selectpicker();
        });
    });

    //]]>
</script>

<div class=""container-fluid"">
    <div class=""boss_table_top"">
        <div class=""fl mb10"">
            <label class=""mr3"" for=""Parent"">询价单号</label>
            <input type=""text"" id=""OrderKey""");
            BeginWriteAttribute("value", " value=\"", 2058, "\"", 2083, 1);
#nullable restore
#line 58 "D:\GIT.NEWARE.WORK\neware-mes\Sources\XY.Supplier.Web\Views\Purchase\Quote.cshtml"
WriteAttributeValue("", 2066, Model.OrderKey, 2066, 17, false);

#line default
#line hidden
#nullable disable
            EndWriteAttribute();
            WriteLiteral(@" maxlength=""64"" class=""form-control mr5"" style=""width:100px;"" placeholder=""询价单号"" />
        </div>
        <div class=""fl mb10"">
            <label class=""mr3"" for=""UserId"">发起人</label>
            <div class=""fl mr5"" style=""width:220px;"">
                ");
#nullable restore
#line 63 "D:\GIT.NEWARE.WORK\neware-mes\Sources\XY.Supplier.Web\Views\Purchase\Quote.cshtml"
           Write(Html.DropDownList("UserId", Model.UserList, new
                {
                    @class = "form-control form-control-sm selectpicker",
                    style = "width:auto;min-width:100px;",
                    data_live_search = "true"
                }));

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n            </div>\r\n        </div>\r\n        <div class=\"fl mb10\">\r\n            <label for=\"Begin\" class=\"mr3\">发布时间</label>\r\n            <input id=\"Begin\"");
            BeginWriteAttribute("value", " value=\"", 2768, "\"", 2790, 1);
#nullable restore
#line 73 "D:\GIT.NEWARE.WORK\neware-mes\Sources\XY.Supplier.Web\Views\Purchase\Quote.cshtml"
WriteAttributeValue("", 2776, Model.Begin, 2776, 14, false);

#line default
#line hidden
#nullable disable
            EndWriteAttribute();
            WriteLiteral(" type=\"text\" maxlength=\"19\" autocomplete=\"off\" class=\"form-control cp mr3\" style=\"width:160px;\" />\r\n        </div>\r\n        <div class=\"fl mb10\">\r\n            <label for=\"End\" class=\"mr3\">至</label>\r\n            <input id=\"End\"");
            BeginWriteAttribute("value", " value=\"", 3017, "\"", 3037, 1);
#nullable restore
#line 77 "D:\GIT.NEWARE.WORK\neware-mes\Sources\XY.Supplier.Web\Views\Purchase\Quote.cshtml"
WriteAttributeValue("", 3025, Model.End, 3025, 12, false);

#line default
#line hidden
#nullable disable
            EndWriteAttribute();
            WriteLiteral(" type=\"text\" maxlength=\"19\" autocomplete=\"off\" class=\"form-control cp mr5\" style=\"width:160px;\" />\r\n        </div>\r\n        <div class=\"fl mb10\">\r\n            <label class=\"mr3\" for=\"OrderStatus\">状态</label>\r\n            ");
#nullable restore
#line 81 "D:\GIT.NEWARE.WORK\neware-mes\Sources\XY.Supplier.Web\Views\Purchase\Quote.cshtml"
       Write(Html.DropDownList("OrderStatus", Model.OrderStatusList, new
            {
                @class = "form-control form-control-sm mr5",
                style = "width:auto;min-width:100px;"
            }));

#line default
#line hidden
#nullable disable
            WriteLiteral(@"
        </div>
        <div class=""fl mb10"">
            <button id=""Search"" type=""button"" class=""btn btn-sm btn-primary"">
                <i class=""fa fa-search"" aria-hidden=""true""></i>
                <span>查询</span>
            </button>
        </div>
    </div>
    <div class=""boss_table_body table-responsive"">
        <table id=""tablePur"" class=""table table-bordered table-hover"" style=""width: 30rem;"">
            <thead>
                <tr>
                    <th class=""w1p""><input id=""SelectAll"" type=""checkbox"" /></th>
                    <th class=""w3p"">#</th>
                    <th class=""w10p"">询价单号</th>
                    <th class=""w10p"">发起人</th>
                    <th class=""w10p"">发布时间</th>
                    <th class=""w10p"">截止时间</th>
                    <th class=""w10p"">状态</th>
                </tr>
            </thead>
            <tbody>
");
#nullable restore
#line 108 "D:\GIT.NEWARE.WORK\neware-mes\Sources\XY.Supplier.Web\Views\Purchase\Quote.cshtml"
                 if (Model.OrderList != null && Model.OrderList.Count() > 0)
                {
                    var i = 0;
                    var pages = Convert.ToInt32(XY.Http.QueryString(Context.Request, "page", "1"));
                    var pagesize = Convert.ToInt32(XY.Http.QueryString(Context.Request, "pagesize", "30"));
                    foreach (var dr in Model.OrderList)
                    {
                        i++;

#line default
#line hidden
#nullable disable
            WriteLiteral("                <tr>\r\n                    <td><input type=\"checkbox\"");
            BeginWriteAttribute("value", " value=\"", 4879, "\"", 4912, 1);
#nullable restore
#line 117 "D:\GIT.NEWARE.WORK\neware-mes\Sources\XY.Supplier.Web\Views\Purchase\Quote.cshtml"
WriteAttributeValue("", 4887, dr["pi_id"].ToString(), 4887, 25, false);

#line default
#line hidden
#nullable disable
            EndWriteAttribute();
            WriteLiteral(" /></td>\r\n                    <td>");
#nullable restore
#line 118 "D:\GIT.NEWARE.WORK\neware-mes\Sources\XY.Supplier.Web\Views\Purchase\Quote.cshtml"
                    Write(((pages - 1) * pagesize + i).ToString());

#line default
#line hidden
#nullable disable
            WriteLiteral("</td>\r\n                    <td><img class=\"detail-icon\"");
            BeginWriteAttribute("onclick", " onclick=\"", 5044, "\"", 5112, 5);
            WriteAttributeValue("", 5054, "step(", 5054, 5, true);
#nullable restore
#line 119 "D:\GIT.NEWARE.WORK\neware-mes\Sources\XY.Supplier.Web\Views\Purchase\Quote.cshtml"
WriteAttributeValue("", 5059, dr["pi_id"].ToString(), 5059, 25, false);

#line default
#line hidden
#nullable disable
            WriteAttributeValue("", 5084, ",", 5084, 1, true);
#nullable restore
#line 119 "D:\GIT.NEWARE.WORK\neware-mes\Sources\XY.Supplier.Web\Views\Purchase\Quote.cshtml"
WriteAttributeValue("", 5085, dr["pq_id"].ToString(), 5085, 25, false);

#line default
#line hidden
#nullable disable
            WriteAttributeValue("", 5110, ");", 5110, 2, true);
            EndWriteAttribute();
            WriteLiteral(" src=\"/images/arrow-right-y.png\" /><span>");
#nullable restore
#line 119 "D:\GIT.NEWARE.WORK\neware-mes\Sources\XY.Supplier.Web\Views\Purchase\Quote.cshtml"
                                                                                                                                                          Write(dr["pi_order"].ToString());

#line default
#line hidden
#nullable disable
            WriteLiteral("</span></td>\r\n                    <td>");
#nullable restore
#line 120 "D:\GIT.NEWARE.WORK\neware-mes\Sources\XY.Supplier.Web\Views\Purchase\Quote.cshtml"
                   Write(dr["name"].ToString());

#line default
#line hidden
#nullable disable
            WriteLiteral("</td>\r\n                    <td>");
#nullable restore
#line 121 "D:\GIT.NEWARE.WORK\neware-mes\Sources\XY.Supplier.Web\Views\Purchase\Quote.cshtml"
                    Write(dr["public_time"].ToString() != "" ? DateTime.Parse(dr["public_time"].ToString()).ToString("yyyy.MM.dd") :"");

#line default
#line hidden
#nullable disable
            WriteLiteral("</td>\r\n                    <td>");
#nullable restore
#line 122 "D:\GIT.NEWARE.WORK\neware-mes\Sources\XY.Supplier.Web\Views\Purchase\Quote.cshtml"
                    Write(dr["end_time"].ToString() != "" ? DateTime.Parse(dr["end_time"].ToString()).ToString("yyyy.MM.dd") :"");

#line default
#line hidden
#nullable disable
            WriteLiteral("</td>\r\n                    <td>");
#nullable restore
#line 123 "D:\GIT.NEWARE.WORK\neware-mes\Sources\XY.Supplier.Web\Views\Purchase\Quote.cshtml"
                    Write(dr["quote_status"] == DBNull.Value?"待报价":(XY.Enums.boss.Tools.purchase_quote_status((XY.Enums.boss.purchase_quote_status)dr["quote_status"])));

#line default
#line hidden
#nullable disable
            WriteLiteral("</td>\r\n                </tr>\r\n");
#nullable restore
#line 125 "D:\GIT.NEWARE.WORK\neware-mes\Sources\XY.Supplier.Web\Views\Purchase\Quote.cshtml"
                    }
                }

#line default
#line hidden
#nullable disable
            WriteLiteral("            </tbody>\r\n        </table>\r\n    </div>\r\n\r\n    ");
#nullable restore
#line 131 "D:\GIT.NEWARE.WORK\neware-mes\Sources\XY.Supplier.Web\Views\Purchase\Quote.cshtml"
Write(Html.Pager(XY.Http.RequestUrl(Context.Request), Model.OrderList));

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n</div>");
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
        public global::Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper<XY.Supplier.Web.Models.Purchase.Quote_Model> Html { get; private set; }
    }
}
#pragma warning restore 1591
