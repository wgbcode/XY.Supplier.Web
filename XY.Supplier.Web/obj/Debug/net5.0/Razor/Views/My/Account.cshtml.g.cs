#pragma checksum "D:\GIT.NEWARE.WORK\neware-mes\Sources\XY.Supplier.Web\Views\My\Account.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "d59b6a5660fd84ae79b9875cdace26704ef544a6"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Views_My_Account), @"mvc.1.0.view", @"/Views/My/Account.cshtml")]
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
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"d59b6a5660fd84ae79b9875cdace26704ef544a6", @"/Views/My/Account.cshtml")]
    public class Views_My_Account : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<XY.Supplier.Web.Models.My.Account_Model>
    {
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
            WriteLiteral("\r\n");
#nullable restore
#line 3 "D:\GIT.NEWARE.WORK\neware-mes\Sources\XY.Supplier.Web\Views\My\Account.cshtml"
  
    Layout = "~/Views/Shared/Normal.cshtml";

#line default
#line hidden
#nullable disable
            WriteLiteral(@"
<style>
    .card {
        min-width: 1000px;
    }
</style>

<script type=""text/javascript"">
    //<![CDATA[

    require(['jquery', 'xyvalid', 'boss'], function ($, v, boss) {
        $(function () {
            var _form = $('form:first');
            _form.xyvInit();
        });
    });

    //]]>
</script>

<form method=""post""");
            BeginWriteAttribute("action", " action=\"", 458, "\"", 467, 0);
            EndWriteAttribute();
            WriteLiteral(@">
    <div class=""container-fluid"">
        <div class=""card"">
            <div class=""card-header b"">账户资料</div>
            <div class=""card-body"">
                <div class=""row justify-content-center mt1r mb05r"">
                    <div class=""col-xl-1 col-lg-1 col-md-1 col-xs-1 col-sm-1 col-1 gray"">公司名称</div>
                    <div class=""col-xl-9 col-lg-9 col-md-9 col-xs-9 col-sm-9 col-9"">");
#nullable restore
#line 33 "D:\GIT.NEWARE.WORK\neware-mes\Sources\XY.Supplier.Web\Views\My\Account.cshtml"
                                                                               Write(Model.SupplierInfo.name);

#line default
#line hidden
#nullable disable
            WriteLiteral(@"</div>
                </div>
                <div class=""row justify-content-center mb05r"">
                    <div class=""col-xl-1 col-lg-1 col-md-1 col-xs-1 col-sm-1 col-1 gray"">软件名称</div>
                    <div class=""col-xl-9 col-lg-9 col-md-9 col-xs-9 col-sm-9 col-9"">NEWARE WMS 供应商系统</div>
                </div>
                <div class=""row justify-content-center mb1r"">
                    <div class=""col-xl-1 col-lg-1 col-md-1 col-xs-1 col-sm-1 col-1 gray"">软件版本</div>
                    <div class=""col-xl-9 col-lg-9 col-md-9 col-xs-9 col-sm-9 col-9"">");
#nullable restore
#line 41 "D:\GIT.NEWARE.WORK\neware-mes\Sources\XY.Supplier.Web\Views\My\Account.cshtml"
                                                                                Write(XY.Supplier.Web.Tools.Version);

#line default
#line hidden
#nullable disable
            WriteLiteral("</div>\r\n                </div>\r\n            </div>\r\n        </div>\r\n    </div>\r\n</form>");
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
        public global::Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper<XY.Supplier.Web.Models.My.Account_Model> Html { get; private set; }
    }
}
#pragma warning restore 1591
