using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using XY.Pager;

namespace XY.Supplier.Web
{
    public static class Tools
    {
        public static string Version = "4.0.10";
        public static string VersionTime = "2022.12.6";

        #region KindEditor

        /// <summary>
        /// KindEditor上传成功
        /// </summary>
        /// <param name="url"></param>
        /// <param name="domain"></param>
        /// <returns></returns>
        public static ContentResult KindEditorSucceed(string url, string domain)
        {
            if (domain.Length > 0)
            {
                return new ContentResult
                {
                    Content = "<script type=\"text/javascript\">document.domain=\"" + domain + "\"</script><pre>{\"error\":0,\"url\":\"" + url + "\"}</pre>",
                    ContentType = XY.Enums.Tools.HttpContentType(XY.Enums.HttpContentType.Html)
                };
            }
            else
            {
                return new ContentResult
                {
                    Content = "{\"error\":0,\"url\":\"" + url + "\"}",
                    ContentType = XY.Enums.Tools.HttpContentType(XY.Enums.HttpContentType.Json)
                };
            }
        }

        /// <summary>
        /// KindEditor上传失败
        /// </summary>
        /// <param name="message"></param>
        /// <param name="domain"></param>
        public static ContentResult KindEditorFaild(string message, string domain)
        {
            if (domain.Length > 0)
            {
                return new ContentResult
                {
                    Content = "<script type=\"text/javascript\">document.domain=\"" + domain + "\"</script><pre>{\"error\":1,\"message\":\"" + message + "\"}</pre>",
                    ContentType = XY.Enums.Tools.HttpContentType(XY.Enums.HttpContentType.Html)
                };
            }
            else
            {
                return new ContentResult
                {
                    Content = "{\"error\":1,\"message\":\"" + message + "\"}",
                    ContentType = XY.Enums.Tools.HttpContentType(XY.Enums.HttpContentType.Json)
                };
            }
        }

        #endregion

        #region Layer

        /// <summary>
        /// Layer对话框提交完成（动作：1弹出警告框；2刷新父页面；3始终关闭对话框）
        /// 限iframe模式子页面调用
        /// </summary>
        /// <param name="alertMsg">弹出警告内容（为空不弹出）</param>
        /// <param name="isRefreshParent">是否刷新父页面</param>
        /// <param name="domain">域名</param>
        /// <returns></returns>
        public static ContentResult LayerSubmitCompleted(string alertMsg, bool isRefreshParent, string domain)
        {
            StringBuilder js = new StringBuilder();

            //设置域名（解除跨域安全限制）
            domain = Text.Trim(domain);
            if (domain.Length > 0)
                js.Append($"document.domain = '{domain}';");

            //弹出警告
            alertMsg = XY.Text.Trim(alertMsg);
            if (alertMsg.Length > 0)
                js.Append("alert('" + alertMsg + "');");

            //父页面刷新
            if (isRefreshParent)
                js.Append("parent.location.href = parent.location.href;");

            //关闭Dialog
            js.Append("var index = parent.layer.getFrameIndex(window.name);parent.layer.close(index);");

            //响应
            return new ContentResult
            {
                Content = "<script type=\"text/javascript\">" + js.ToString() + "</script>",
                ContentType = Enums.Tools.HttpContentType(Enums.HttpContentType.Html)
            };
        }

        /// <summary>
        /// Layer对话框提交完成（动作：1弹出警告框；2跳转父页面；3始终关闭对话框）
        /// 限iframe模式子页面调用
        /// </summary>
        /// <param name="alertMsg">弹出警告内容（为空不弹出）</param>
        /// <param name="parentUrl">父页面跳转地址（为空不跳转）</param>
        /// <param name="domain">域名</param>
        /// <returns></returns>
        public static ContentResult LayerSubmitCompleted(string alertMsg, string parentUrl, string domain)
        {
            StringBuilder js = new StringBuilder();

            //设置域名（解除跨域安全限制）
            domain = Text.Trim(domain);
            if (domain.Length > 0)
                js.Append($"document.domain = '{domain}';");

            //弹出警告
            alertMsg = Text.Trim(alertMsg);
            if (alertMsg.Length > 0)
                js.Append("alert('" + alertMsg + "');");

            //父页面跳转
            parentUrl = Text.Trim(parentUrl);
            if (parentUrl.Length > 0)
                js.Append("parent.location.href = '" + parentUrl + "';");
            else
                js.Append("parent.location.href = parent.location.href;");

            //关闭Dialog
            js.Append("var index = parent.layer.getFrameIndex(window.name);parent.layer.close(index);");

            //响应
            return new ContentResult
            {
                Content = "<script type=\"text/javascript\">" + js.ToString() + "</script>",
                ContentType = Enums.Tools.HttpContentType(Enums.HttpContentType.Html)
            };
        }

        #endregion

        #region Pager(HtmlHelper extension)

        /// <summary>
        /// 构造分页链接URL
        /// </summary>
        /// <param name="url">当前URL(无参数)</param>
        /// <param name="paras">当前URL参数</param>
        /// <param name="page">目标页索引</param>
        /// <returns></returns>
        private static string pagerUrl(string url, Dictionary<string, string> urlParameters, int page)
        {
            if (urlParameters == null)
                urlParameters = new Dictionary<string, string>();

            if (urlParameters.ContainsKey("page"))
            {
                urlParameters["page"] = page.ToString();
            }
            else
            {
                urlParameters.Add("page", page.ToString());
            }

            return url + "?" + XY.Convert.DictionaryToString(urlParameters, "&", "=");

        }

        /// <summary>
        /// 构造pagesize change url
        /// </summary>
        /// <param name="url">当前URL(无参数)</param>
        /// <param name="urlParameters">当前URL参数</param>
        /// <returns></returns>
        private static string pagesizeUrl(string url, Dictionary<string, string> urlParameters)
        {
            if (urlParameters == null)
                urlParameters = new Dictionary<string, string>();

            //过滤pagesize参数
            if (urlParameters.ContainsKey("pagesize"))
            {
                urlParameters.Remove("pagesize");
            }

            //pagesize切换时page参数固定为1
            if (urlParameters.ContainsKey("page"))
            {
                urlParameters["page"] = "1";
            }
            else
            {
                urlParameters.Add("page", "1");
            }

            return url + "?" + XY.Convert.DictionaryToString(urlParameters, "&", "=");
        }

        /// <summary>
        /// 分页
        /// </summary>
        /// <param name="helper"></param>
        /// <param name="rawUrl">当前URL</param>
        /// <param name="pager">分页数据</param>
        /// <returns></returns>
        public static IHtmlContent Pager(this IHtmlHelper helper, string rawUrl, IPager pager)
        {
            /* Pager html demo
            <div class="boss_table_bottom">
                <div class="fl">
                    <div class="btn-group" role="group">
                        <a href="#" type="button" class="btn btn-sm btn-outline-warning disabled">首页</a>
                        <a href="#" type="button" class="btn btn-sm btn-outline-warning">上一页</a>
                        <a href="#" type="button" class="btn btn-sm btn-outline-warning">...</a>
                        <a href="#" type="button" class="btn btn-sm btn-outline-warning">3</a>
                        <a href="#" type="button" class="btn btn-sm btn-outline-warning">4</a>
                        <a href="#" type="button" class="btn btn-sm btn-outline-warning">5</a>
                        <a href="#" type="button" class="btn btn-sm btn-outline-warning">6</a>
                        <a href="#" type="button" class="btn btn-sm btn-outline-warning">7</a>
                        <a href="#" type="button" class="btn btn-sm btn-warning">8</a>
                        <a href="#" type="button" class="btn btn-sm btn-outline-warning">9</a>
                        <a href="#" type="button" class="btn btn-sm btn-outline-warning">10</a>
                        <a href="#" type="button" class="btn btn-sm btn-outline-warning">11</a>
                        <a href="#" type="button" class="btn btn-sm btn-outline-warning">12</a>
                        <a href="#" type="button" class="btn btn-sm btn-outline-warning">13</a>
                        <a href="#" type="button" class="btn btn-sm btn-outline-warning">...</a>
                        <a href="#" type="button" class="btn btn-sm btn-outline-warning">下一页</a>
                        <a href="#" type="button" class="btn btn-sm btn-outline-warning">尾页</a>
                    </div>
                </div>
                <div class="fr">
                    <div class="fl">
                        <select id="Pagesize" name="Pagesize" class="form-control form-control-sm" style="color:#212529;border:#ffc107 1px solid;border-right:none;border-top-right-radius:0px;border-bottom-right-radius:0px;">
                            <option value='10'>每页10</option>
                            <option value='15'>每页15</option>
                            <option value='20' selected="selected">每页20</option>
                            <option value='25'>每页25</option>
                            <option value='30'>每页30</option>
                            <option value='35'>每页35</option>
                            <option value='40'>每页40</option>
                            <option value='45'>每页45</option>
                            <option value='50'>每页50</option>
                            <option value='100'>每页100</option>
                        </select>
                    </div>
                    <div class="fl">
                        <span class="badge badge-warning" style="font-size:.875rem;line-height:1.5;padding:.25rem .5rem;font-weight:400;border:#ffc107 1px solid;color:#212529;background-color:white;
                                    border-top-left-radius:0px;border-bottom-left-radius:0px;">共 6 页 60 条记录</span>
                    </div>
                </div>
            </div>
            */

            //string rawUrl = System.Web.HttpContext.Current.Request.Url.ToString();

            rawUrl = rawUrl.Trim();
            HtmlContentBuilder hcb = new HtmlContentBuilder();

            if (pager != null)
            {
                //过滤参数                
                int leng = rawUrl.IndexOf("?");
                leng = leng < 0 ? rawUrl.Length : leng;
                string url = rawUrl.Substring(0, leng);

                //当前URL参数
                var urlParameters = leng == rawUrl.Length ? null : XY.Http.UrlParametersDictionary(rawUrl);
                var curUrlParameters = urlParameters == null ? new Dictionary<string, string>() : new Dictionary<string, string>(urlParameters);
                //首页
                string firstUrl = "#";
                if (pager.PageIndex > 1)
                    firstUrl = pagerUrl(url, urlParameters, 1);

                //上页
                string prevUrl = "#";
                if (pager.PageIndex > 1)
                    prevUrl = pagerUrl(url, urlParameters, pager.PageIndex - 1);

                //下页
                string nextUrl = "#";
                if (pager.PageIndex < pager.PageTotal)
                    nextUrl = pagerUrl(url, urlParameters, pager.PageIndex + 1);

                //尾页
                string lastUrl = "#";
                if (pager.PageTotal > 0)
                    lastUrl = pagerUrl(url, urlParameters, pager.PageTotal);

                hcb.AppendHtml("<div class='boss_table_bottom'>");

                hcb.AppendHtml("<div class='fl mr20'>");
                //页尺寸选择
                hcb.AppendHtml("<div class='fl'>");
                hcb.AppendHtml("<script type='text/javascript'>\r\n");
                hcb.AppendHtml("//<![CDATA[\r\n");
                hcb.AppendHtml("function pagerChangePagesize(pagesize) {\r\n");
                hcb.AppendHtml("var url = '" + pagesizeUrl(url, curUrlParameters) + "';\r\n");
                hcb.AppendHtml("window.location.href = url + '&pagesize=' + pagesize;\r\n");
                hcb.AppendHtml("}\r\n");
                hcb.AppendHtml("//]]>\r\n");
                hcb.AppendHtml("</script>");
                hcb.AppendHtml("<select id='Pagesize' name='Pagesize' onchange='pagerChangePagesize(this.value)' class='form-control form-control-sm' style='color:#212529;border:#ffc107 1px solid;border-right:none;border-top-right-radius:0px;border-bottom-right-radius:0px;'>");
                hcb.AppendHtml("<option value='10' " + (pager.PageSize == 10 ? "selected='selected'" : "") + ">每页10</option>");
                hcb.AppendHtml("<option value='15' " + (pager.PageSize == 15 ? "selected='selected'" : "") + ">每页15</option>");
                hcb.AppendHtml("<option value='20' " + (pager.PageSize == 20 ? "selected='selected'" : "") + ">每页20</option>");
                hcb.AppendHtml("<option value='25' " + (pager.PageSize == 25 ? "selected='selected'" : "") + ">每页25</option>");
                hcb.AppendHtml("<option value='30' " + (pager.PageSize == 30 ? "selected='selected'" : "") + ">每页30</option>");
                hcb.AppendHtml("<option value='35' " + (pager.PageSize == 35 ? "selected='selected'" : "") + ">每页35</option>");
                hcb.AppendHtml("<option value='40' " + (pager.PageSize == 40 ? "selected='selected'" : "") + ">每页40</option>");
                hcb.AppendHtml("<option value='45' " + (pager.PageSize == 45 ? "selected='selected'" : "") + ">每页45</option>");
                hcb.AppendHtml("<option value='50' " + (pager.PageSize == 50 ? "selected='selected'" : "") + ">每页50</option>");
                hcb.AppendHtml("<option value='100' " + (pager.PageSize == 100 ? "selected='selected'" : "") + ">每页100</option>");
                hcb.AppendHtml("</select>");
                hcb.AppendHtml("</div>");
                //分页信息
                hcb.AppendHtml("<div class='fl'>");
                hcb.AppendHtml($"<span class='badge badge-primary' style='font-size:.875rem;line-height:1.5;padding:.25rem .5rem;font-weight:400;border:#ffc107 1px solid;color:#212529;background-color:white;border-top-left-radius:0px;border-bottom-left-radius:0px;'>共 {pager.PageTotal} 页 {pager.RecordTotal} 条记录</span>");
                hcb.AppendHtml("</div>");
                hcb.AppendHtml("</div>");//end fr(right)

                hcb.AppendHtml("<div class='fl'>");
                hcb.AppendHtml("<div class='btn-group' role='group'>");
                hcb.AppendHtml("<a href='" + firstUrl + "' type='button' class='btn btn-sm btn-outline-warning' " + (pager.PageIndex <= 1 ? "disabled" : "") + ">首页</a>");
                hcb.AppendHtml("<a href='" + prevUrl + "' type='button' class='btn btn-sm btn-outline-warning' " + (pager.PageIndex <= 1 ? "disabled" : "") + ">上一页</a>");

                //页码总数在5页内
                if (pager.PageTotal <= 5)
                {
                    for (int i = 1; i <= pager.PageTotal; i++)
                    {
                        hcb.AppendHtml("<a href='" + pagerUrl(url, urlParameters, i) + "' type='button' class='btn btn-sm " + (i == pager.PageIndex ? "btn-warning" : "btn-outline-warning") + "'>" + i.ToString() + "</a>");
                    }
                }
                //页码总数超过5页
                else
                {
                    //前置...
                    if (pager.PageIndex > 5 + 1)
                    {
                        hcb.AppendHtml("<a href='" + pagerUrl(url, urlParameters, pager.PageIndex - (5 + 1)) + "' type='button' class='btn btn-sm btn-outline-warning'>...</a>");
                    }

                    //页码导航
                    int begin = 1;
                    if (pager.PageIndex > 5)
                        begin = pager.PageIndex - 5;

                    int end = pager.PageIndex + 5;
                    if (end > pager.PageTotal)
                        end = pager.PageTotal;

                    for (int i = begin; i <= end; i++)
                    {
                        hcb.AppendHtml("<a href='" + pagerUrl(url, urlParameters, i) + "' type='button' class='btn btn-sm " + (i == pager.PageIndex ? "btn-warning" : "btn-outline-warning") + "'>" + i.ToString() + "</a>");
                    }

                    //后置...
                    if (pager.PageIndex + 5 < pager.PageTotal)
                    {
                        hcb.AppendHtml("<a href='" + pagerUrl(url, urlParameters, pager.PageIndex + (5 + 1)) + "' type='button' class='btn btn-sm btn-outline-warning'>...</a>");
                    }
                }

                hcb.AppendHtml("<a href='" + nextUrl + "' type='button' class='btn btn-sm btn-outline-warning' " + (pager.PageIndex >= pager.PageTotal ? "disabled" : "") + ">下一页</a>");
                hcb.AppendHtml("<a href='" + lastUrl + "' type='button' class='btn btn-sm btn-outline-warning' " + ((pager.PageIndex >= pager.PageTotal || pager.PageIndex <= 0) ? "disabled" : "") + ">尾页</a>");
                hcb.AppendHtml("</div>\r\n"); // end btn-group
                hcb.AppendHtml("</div>"); //end fl(left)

                hcb.AppendHtml("</div>"); //end boss_table_bottom
            }

            return hcb;
        }

        #endregion

        #region Http

        public static string HttpGet(string url, string token = "")
        {
            var apiClient = new HttpClient();
            if (!string.IsNullOrWhiteSpace(token))
                apiClient.DefaultRequestHeaders.Add("Authorization", token);

            var response = apiClient.GetAsync(url).Result;
            if (response.IsSuccessStatusCode)
            {
                var result = response.Content.ReadAsStringAsync().Result;
                return result;
            }
            else
                return default;
        }

        public static T HttpGet<T>(string url, string token = "")
        {
            var apiClient = new HttpClient();
            if (!string.IsNullOrWhiteSpace(token))
                apiClient.DefaultRequestHeaders.Add("Authorization", token);

            var response = apiClient.GetAsync(url).Result;
            if (response.IsSuccessStatusCode)
            {
                var result = response.Content.ReadAsStringAsync().Result;
                return XY.Serializable.JsonStringToObject<T>(result);
            }
            else
                return default;
        }

        public static T HttpPost<T>(string url, object para, string token = "")
        {
            var apiClient = new HttpClient();
            if (!string.IsNullOrWhiteSpace(token))
                apiClient.DefaultRequestHeaders.Add("Authorization", token);

            var paraStr = XY.Serializable.ObjectToJsonString(para);
            HttpContent httpContent = new StringContent(paraStr);
            httpContent.Headers.ContentType = new MediaTypeWithQualityHeaderValue("application/json");
            var response = apiClient.PostAsync(url, httpContent).Result;
            if (response.IsSuccessStatusCode)
                return XY.Serializable.JsonStringToObject<T>(response.Content.ReadAsStringAsync().Result);
            else
                return default;
        }

        #endregion
    }
}
