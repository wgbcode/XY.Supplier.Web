#pragma checksum "D:\GIT.NEWARE.WORK\neware-mes\Sources\XY.Supplier.Web\Views\Purchase\Order.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "f788398d7a68e77fbaa56aeca76ebb35519949db"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Views_Purchase_Order), @"mvc.1.0.view", @"/Views/Purchase/Order.cshtml")]
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
#line 1 "D:\GIT.NEWARE.WORK\neware-mes\Sources\XY.Supplier.Web\Views\Purchase\Order.cshtml"
using XY.Supplier.Web;

#line default
#line hidden
#nullable disable
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"f788398d7a68e77fbaa56aeca76ebb35519949db", @"/Views/Purchase/Order.cshtml")]
    public class Views_Purchase_Order : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<XY.Supplier.Web.Models.Purchase.PurchaseOrder_Model>
    {
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
            WriteLiteral("\r\n");
#nullable restore
#line 4 "D:\GIT.NEWARE.WORK\neware-mes\Sources\XY.Supplier.Web\Views\Purchase\Order.cshtml"
  
    Layout = "~/Views/Shared/Normal.cshtml";

#line default
#line hidden
#nullable disable
            WriteLiteral(@"
<script type=""text/javascript"">
    //<![CDATA[

    function step(purchaseID) {
        require(['xy', 'boss'], function (xy, boss) {
            boss.load('/purchase/order_detail?purchaseID=' + purchaseID + '&backurl=' + xy.url(true));
        });
    }

    require(['jquery', 'xy', 'boss', 'laydate'], function ($, xy, boss, laydate) {
        $(function () {
            //初始化日期样式
            //var dBegin = '");
#nullable restore
#line 20 "D:\GIT.NEWARE.WORK\neware-mes\Sources\XY.Supplier.Web\Views\Purchase\Order.cshtml"
                        Write(DateTime.Now.AddMonths(-1).ToString("yyyy-MM-dd HH:mm:ss"));

#line default
#line hidden
#nullable disable
            WriteLiteral("\'\r\n            //var dEnd = \'");
#nullable restore
#line 21 "D:\GIT.NEWARE.WORK\neware-mes\Sources\XY.Supplier.Web\Views\Purchase\Order.cshtml"
                      Write(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));

#line default
#line hidden
#nullable disable
            WriteLiteral("\'\r\n\r\n            //初始化日期样式\r\n            var dBegin = \'");
#nullable restore
#line 24 "D:\GIT.NEWARE.WORK\neware-mes\Sources\XY.Supplier.Web\Views\Purchase\Order.cshtml"
                     Write(DateTime.Parse(Model.Begin).ToString("yyyy.MM.dd 00:00:00"));

#line default
#line hidden
#nullable disable
            WriteLiteral("\'\r\n            var dEnd =  \'");
#nullable restore
#line 25 "D:\GIT.NEWARE.WORK\neware-mes\Sources\XY.Supplier.Web\Views\Purchase\Order.cshtml"
                    Write(DateTime.Parse(Model.End).ToString("yyyy.MM.dd 23:59:59"));

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
                boss.load(""/purchase/order?orderStatus="" + $(""#OrderStatus"").val() + ""&isDelayStatus="" + $(""#IsDelayStatus"").val() + ""&orderKey="" + $(""#OrderKey"").val()
                    + ""&begin="" + xy.urlencode($(""#Begin"").val()) + ""&end="" + xy.urlencode($(""#End"").val()) + ""&pagesize="" + $(""#Pagesize"").val());
            });
            //点击导出
            $('#Export').on(""click"", fu");
            WriteLiteral(@"nction () {
                var idArray = boss.tableSelectValue($('#tablePur'));
                if (idArray == null || idArray.length <= 0) {
                    alert('请至少选择一条记录！');
                    return false;
                }
                $(""#Export"").attr(""disabled"", true);
                postDownloadFile({ purchaseID: xy.jsonstr(idArray) }, ""/purchase/order_export"");
                $(""#Export"").attr(""disabled"", false);
            });
            //下载文件
            function postDownloadFile(params, url) {
                //params是post请求需要的参数，url是请求url地址
                var form = document.createElement(""form"");
                form.style.display = ""none"";
                form.action = url;
                form.method = ""post"";
                document.body.appendChild(form);
                //动态创建input并给value赋值
                for (var key in params) {
                    var input = document.createElement(""input"");
                    input.type = ""hidden"";
            ");
            WriteLiteral(@"        input.name = key;
                    input.value = params[key];
                    form.appendChild(input);
                }
                form.submit();
                form.remove();
            }
            //点击投屏
            $('#Shot').on('click', function () {
                window.location.href = ""/purchase/order_shot?orderStatus="" + $(""#OrderStatus"").val() + ""&purchaseOrderKey="" + $(""#PurchaseOrderKey"").val() + ""&isDelayStatus=""
                    + $(""#IsDelayStatus"").val() + ""&begin="" + xy.urlencode($(""#Begin"").val()) + ""&end="" + xy.urlencode($(""#End"").val());

            });
            //点击打印条码
            $('#Print').on('click', function () {
                var idArray = boss.tableSelectValue($('#tablePur'));
                if (idArray != null && idArray.length == 1) {
                    window.location.href = ""/purchase/order_print_detail?purchaseID="" + idArray[0] + '&backurl=' + xy.url(true);
                }
                else {
                    ale");
            WriteLiteral(@"rt('请选择一条记录！');
                    return false;
                }
            });
            //在父页面添加监听器,接收子页面的值：
            window.addEventListener('message', function (e) {

                alert(e.data);

            }, false);
        });
    });

    //]]>
</script>

<div class=""container-fluid"">
    <div class=""boss_table_top"">
        <div class=""fl mb10"">
            <label class=""mr3"">状态</label>
            ");
#nullable restore
#line 112 "D:\GIT.NEWARE.WORK\neware-mes\Sources\XY.Supplier.Web\Views\Purchase\Order.cshtml"
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
            <label class=""mr3"">延期</label>
            <select class=""form-control form-control-sm mr5"" data-val=""true"" data-val-required=""The IsDelayStatus field is required."" id=""IsDelayStatus"" name=""IsDelayStatus"" style=""width:auto;min-width:100px;"">
                <option value=""-1"" ");
#nullable restore
#line 122 "D:\GIT.NEWARE.WORK\neware-mes\Sources\XY.Supplier.Web\Views\Purchase\Order.cshtml"
                               Write(Model.IsDelayStatus == -1 ? "selected='selected'" : "");

#line default
#line hidden
#nullable disable
            WriteLiteral(">全部</option>\r\n                <option value=\"1\" ");
#nullable restore
#line 123 "D:\GIT.NEWARE.WORK\neware-mes\Sources\XY.Supplier.Web\Views\Purchase\Order.cshtml"
                              Write(Model.IsDelayStatus == 1 ? "selected='selected'" : "");

#line default
#line hidden
#nullable disable
            WriteLiteral(">是</option>\r\n                <option value=\"0\" ");
#nullable restore
#line 124 "D:\GIT.NEWARE.WORK\neware-mes\Sources\XY.Supplier.Web\Views\Purchase\Order.cshtml"
                              Write(Model.IsDelayStatus == 0 ? "selected='selected'" : "");

#line default
#line hidden
#nullable disable
            WriteLiteral(">否</option>\r\n            </select>\r\n        </div>\r\n        <div class=\"fl mb10\">\r\n            <label for=\"Begin\" class=\"mr3\">下单时间</label>\r\n            <input id=\"Begin\"");
            BeginWriteAttribute("value", " value=\"", 5402, "\"", 5424, 1);
#nullable restore
#line 129 "D:\GIT.NEWARE.WORK\neware-mes\Sources\XY.Supplier.Web\Views\Purchase\Order.cshtml"
WriteAttributeValue("", 5410, Model.Begin, 5410, 14, false);

#line default
#line hidden
#nullable disable
            EndWriteAttribute();
            WriteLiteral(" type=\"text\" maxlength=\"19\" autocomplete=\"off\" class=\"form-control cp mr3\" style=\"width:160px;\" />\r\n        </div>\r\n        <div class=\"fl mb10\">\r\n            <label for=\"End\" class=\"mr3\">至</label>\r\n            <input id=\"End\"");
            BeginWriteAttribute("value", " value=\"", 5651, "\"", 5671, 1);
#nullable restore
#line 133 "D:\GIT.NEWARE.WORK\neware-mes\Sources\XY.Supplier.Web\Views\Purchase\Order.cshtml"
WriteAttributeValue("", 5659, Model.End, 5659, 12, false);

#line default
#line hidden
#nullable disable
            EndWriteAttribute();
            WriteLiteral(" type=\"text\" maxlength=\"19\" autocomplete=\"off\" class=\"form-control cp mr5\" style=\"width:160px;\" />\r\n        </div>\r\n        <div class=\"fl mb10\">\r\n            <label class=\"mr3\" for=\"OrderKey\">订单号</label>\r\n            <input type=\"text\" id=\"OrderKey\"");
            BeginWriteAttribute("value", " value=\"", 5922, "\"", 5947, 1);
#nullable restore
#line 137 "D:\GIT.NEWARE.WORK\neware-mes\Sources\XY.Supplier.Web\Views\Purchase\Order.cshtml"
WriteAttributeValue("", 5930, Model.OrderKey, 5930, 17, false);

#line default
#line hidden
#nullable disable
            EndWriteAttribute();
            WriteLiteral(@" maxlength=""64"" class=""form-control mr5"" style=""width:200px;"" placeholder=""订单号"" />
        </div>
        <div class=""fl mb10"">
            <button id=""Search"" type=""button"" class=""btn btn-sm btn-primary"">
                <i class=""fa fa-search"" aria-hidden=""true""></i>
                <span>查询</span>
            </button>
        </div>
        <div class=""fr mr10"">
            <button id=""Export"" type=""button"" class=""btn btn-sm btn-warning mb10"">
                <i class=""fas fa-download"" aria-hidden=""true""></i>
                <span>导出</span>
            </button>
        </div>
        <div class=""fr mr10"">
            <button id=""Print"" type=""button"" class=""btn btn-sm btn-warning mb10"">
                <i class=""fa fa-print"" aria-hidden=""true""></i>
                <span>打印条码</span>
            </button>
        </div>
        <div class=""fr mr10"">
            <button id=""Shot"" type=""button"" class=""btn btn-sm btn-warning mb10"" style=""display:none;"">
                <i class=""fa "" aria");
            WriteLiteral(@"-hidden=""true""></i>
                <span>投屏模式</span>
            </button>
        </div>
    </div>
    <div class=""boss_table_body table-responsive"">
        <table id=""tablePur"" class=""table table-bordered table-hover"" style=""width: 36rem"">
            <thead>
                <tr>
                    <th><input id=""SelectAll"" type=""checkbox"" /></th>
                    <th class=""w3p"">#</th>
                    <th class=""w20p"">订单号</th>
                    <th class=""w27p"">下单时间</th>
                    <th class=""w27p"">要求交货时间</th>
                    <th class=""w10p"">延期</th>
                    <th class=""w16p"">状态</th>
                </tr>
            </thead>
            <tbody>
");
#nullable restore
#line 178 "D:\GIT.NEWARE.WORK\neware-mes\Sources\XY.Supplier.Web\Views\Purchase\Order.cshtml"
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
            BeginWriteAttribute("value", " value=\"", 8204, "\"", 8243, 1);
#nullable restore
#line 188 "D:\GIT.NEWARE.WORK\neware-mes\Sources\XY.Supplier.Web\Views\Purchase\Order.cshtml"
WriteAttributeValue("", 8212, dr["purchase_id"].ToString(), 8212, 31, false);

#line default
#line hidden
#nullable disable
            EndWriteAttribute();
            WriteLiteral(" /></td>\r\n                    <td>");
#nullable restore
#line 189 "D:\GIT.NEWARE.WORK\neware-mes\Sources\XY.Supplier.Web\Views\Purchase\Order.cshtml"
                    Write(((pages - 1) * pagesize + i).ToString());

#line default
#line hidden
#nullable disable
            WriteLiteral("</td>\r\n                    <td><img class=\"detail-icon\"");
            BeginWriteAttribute("onclick", " onclick=\"", 8375, "\"", 8434, 3);
            WriteAttributeValue("", 8385, "step(", 8385, 5, true);
#nullable restore
#line 190 "D:\GIT.NEWARE.WORK\neware-mes\Sources\XY.Supplier.Web\Views\Purchase\Order.cshtml"
WriteAttributeValue("", 8390, int.Parse(dr["purchase_id"].ToString()), 8390, 42, false);

#line default
#line hidden
#nullable disable
            WriteAttributeValue("", 8432, ");", 8432, 2, true);
            EndWriteAttribute();
            WriteLiteral(" src=\"/images/arrow-right-y.png\" /><span>");
#nullable restore
#line 190 "D:\GIT.NEWARE.WORK\neware-mes\Sources\XY.Supplier.Web\Views\Purchase\Order.cshtml"
                                                                                                                                                 Write(dr["purchase_order"].ToString());

#line default
#line hidden
#nullable disable
            WriteLiteral("</span></td>\r\n                    <td>");
#nullable restore
#line 191 "D:\GIT.NEWARE.WORK\neware-mes\Sources\XY.Supplier.Web\Views\Purchase\Order.cshtml"
                    Write(dr["issue_time"].ToString()==""?"": DateTime.Parse(dr["issue_time"].ToString()).ToString("yyyy.MM.dd HH:mm:ss"));

#line default
#line hidden
#nullable disable
            WriteLiteral("</td>\r\n                    <td>");
#nullable restore
#line 192 "D:\GIT.NEWARE.WORK\neware-mes\Sources\XY.Supplier.Web\Views\Purchase\Order.cshtml"
                    Write(dr["delivery_time"].ToString()==""?"": DateTime.Parse(dr["delivery_time"].ToString()).ToString("yyyy.MM.dd"));

#line default
#line hidden
#nullable disable
            WriteLiteral("</td>\r\n                    <td>");
#nullable restore
#line 193 "D:\GIT.NEWARE.WORK\neware-mes\Sources\XY.Supplier.Web\Views\Purchase\Order.cshtml"
                    Write(dr["is_delay"].ToString() == "0" ? "否":"是");

#line default
#line hidden
#nullable disable
            WriteLiteral("</td>\r\n                    <td>");
#nullable restore
#line 194 "D:\GIT.NEWARE.WORK\neware-mes\Sources\XY.Supplier.Web\Views\Purchase\Order.cshtml"
                    Write(XY.Enums.boss.Tools.purchase_status((XY.Enums.boss.purchase_status)dr["status"]));

#line default
#line hidden
#nullable disable
            WriteLiteral("</td>\r\n                </tr>\r\n");
#nullable restore
#line 196 "D:\GIT.NEWARE.WORK\neware-mes\Sources\XY.Supplier.Web\Views\Purchase\Order.cshtml"
                    }
                }

#line default
#line hidden
#nullable disable
            WriteLiteral("            </tbody>\r\n        </table>\r\n    </div>\r\n\r\n    ");
#nullable restore
#line 202 "D:\GIT.NEWARE.WORK\neware-mes\Sources\XY.Supplier.Web\Views\Purchase\Order.cshtml"
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
        public global::Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper<XY.Supplier.Web.Models.Purchase.PurchaseOrder_Model> Html { get; private set; }
    }
}
#pragma warning restore 1591
