#pragma checksum "D:\GIT.NEWARE.WORK\neware-mes\Sources\XY.Supplier.Web\Views\Purchase\Order_Print_Detail.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "5c8b52866297ec4fea74e167e9d90c0cb1fbc7ee"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Views_Purchase_Order_Print_Detail), @"mvc.1.0.view", @"/Views/Purchase/Order_Print_Detail.cshtml")]
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
#line 1 "D:\GIT.NEWARE.WORK\neware-mes\Sources\XY.Supplier.Web\Views\Purchase\Order_Print_Detail.cshtml"
using System.Data;

#line default
#line hidden
#nullable disable
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"5c8b52866297ec4fea74e167e9d90c0cb1fbc7ee", @"/Views/Purchase/Order_Print_Detail.cshtml")]
    public class Views_Purchase_Order_Print_Detail : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<XY.Supplier.Web.Models.Purchase.PurchaseOrder_Print_Detail>
    {
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
            WriteLiteral("\r\n");
#nullable restore
#line 4 "D:\GIT.NEWARE.WORK\neware-mes\Sources\XY.Supplier.Web\Views\Purchase\Order_Print_Detail.cshtml"
  
    Layout = "~/Views/Shared/Normal.cshtml";

#line default
#line hidden
#nullable disable
            WriteLiteral(@"<style>
    input {
        padding: 0.14rem 0.5rem !important;
        border: 1px solid #ced4da !important;
    }

    .tips {
        margin-top: 6rem;
    }

    .tips .tip {
        margin-bottom: 6px;
    }
</style>
<script type=""text/javascript"">
    //<![CDATA[

    var isPrintOutPackage = false

    require(['jquery', 'bootstrap', 'bootstrapSelect', 'xyvalid', 'boss', 'laydate', 'bootstrapTableLocale', 'layer', 'xy'], function ($, bootstrap, bootstrapSelect, v, boss, laydate, bt, layer, xy) {
        $(function () {
            boss.tableInit($('#tableOrderItem'));
            //是否客户端变量
            var isClient = false;
            try {
                if (!isObjNull(dotnetPrint)) {
                    isClient = true;
                }
            }
            catch (error) {
                console.log(error);
            }
            //打印类型 0:web 1:client
            //setup();
            var printType = 0;
            //客户端使用时显示参数配置按钮
            try {
   ");
            WriteLiteral(@"             if (!isObjNull(dotnetPrint)) {
                    $('#PrintConfig').show();
                    printType = 1;
                }
            }
            catch (error) {
                console.log(error);
            }

            // 展示物料条纹对话框
            $('#Print').on('click', function () {

                $('#tbOrderPrintInput').bootstrapTable({
                    clickEdit: true,
                    columns: [{
                        checkbox: true
                    },
                    {
                        title: '序列',
                        field: 'index',
                        formatter: function (value, row, index) {
                            return row.index = index + 1;
                        }
                    }, {
                        title: '标签内数量',
                        field: 'labelQty',
                    }, {
                        title: '份数',
                        field: 'copies',
                    }, {
         ");
            WriteLiteral(@"               title: '备注',
                        field: 'remark',
                    }
                    ],
                    onClickCell: function (field, value, row, $element) {
                        $element.attr('contenteditable', true);
                        $element.blur(function () {
                            let index = $element.parent().data('index');
                            let tdValue = $element.html();
                            saveDataSerialNumber(index, field, tdValue);
                        })
                    },

                });

                $('#dialogOrderPrintInput').modal({show:true})
            });

            // 物料条纹表单新增行
            $('#addRow').on('click', function () {
                var row = {
                    ""labelQty"": """",
                    ""copies"": """",
                    ""remark"": """",
                };
                $('#tbOrderPrintInput').bootstrapTable('append', row)
            })

            // 物料条纹表单删");
            WriteLiteral(@"除行
            $('#removeRow').on('click', function () {
                var rows = $('#tbOrderPrintInput').bootstrapTable('getSelections');
                if (rows.length == 0) {
                    layer.msg(""请选择要删除的数据"");
                    return;
                }
                var indexs = [];
                for (var i = 0; i < rows.length; i++) {
                    indexs[i] = rows[i].index;
                }
                $('#tbOrderPrintInput').bootstrapTable('remove', {
                    field: 'index',
                    values: indexs
                });
            })

            //保存物料条纹打印数据
            function saveDataSerialNumber(index, field, tdValue) {
                //更新单元格
                $('#tbOrderPrintInput').bootstrapTable('updateCell', {
                    index: index,
                    field: field,
                    value: tdValue
                })
            }


            // 展示物料条纹打印数据确认弹框
            $('#btnSubmit').on('click', f");
            WriteLiteral(@"unction () {
                // 拿到表单数据
                var rows = $('#tbOrderPrintInput').bootstrapTable('getData');
                console.log('rows', rows)

                // 标签内数量和份数校验
                if (rows.length === 0) {
                    layer.msg(""物料条码数据为空"")
                    return false
                }
                var [checkLabelQty, checkCopiesExit, checkCopiesNum] = [false, false, false];
                var labelQtys = [];
                rows.forEach((item) => {
                    labelQtys.push(item.labelQty)
                    if (item.labelQty === '') {
                        checkLabelQty = true
                    } else if (item.copies === '') {
                        checkCopiesExit = true;
                    } else if (item.copies <= 0 || item.copies >= 1000) {
                        checkCopiesNum = true;
                    }
                });
                if (checkLabelQty) {
                    layer.msg(""标签内数量不能为空"")
                  ");
            WriteLiteral(@"  return false
                } else if (checkCopiesExit) {
                    layer.msg(""份数不能为空"")
                    return false
                } else if (checkCopiesNum) {
                    layer.msg(""份数必须大于1并小于1000"")
                    return false
                }
                PrintLabel(labelQtys)
            })

            // 外包装弹框
            function showPrintOutPackgae(){
                layer.confirm('是否需要打印外包装标签', {
                    title: ""提示"",
                    icon: 3,
                    closeBtn: 0,
                    btn: ['是', '否'],
                    shade: 0.3,
                    cancel: function (index, layero) {
                        //console.log('关闭x号');
                    }, //按钮
                }, function () {
                    isPrintOutPackage = true;
                    layer.closeAll(); //关闭所有层
                    
                }, function () {
                    //layer.msg('否', { icon: 1 });
                    isPrintO");
            WriteLiteral(@"utPackage = false;
                })
            };


            function PrintLabel(labelQtys) {
                var idArray = boss.tableSelectValue($('#tableOrderItem'));
                if (idArray != null && idArray.length > 0) {
                    //选中的数据集合
                    var table = [];
                    //遍历数据行
                    $(""#tableOrderItem"").find(""tr"").each(function () {
                        var row = {};
                        var isChecked = true;
                        //遍历数据列
                        $(this).find(""td"").each(function () {
                            var input = $(this).find(""input"");
                            var objName = $(this).attr(""name"");
                            var value;
                            //未内嵌input则直接取文本值
                            if (input == undefined || input.length <= 0) {
                                value = $(this).text().trim();
                            }
                            else {
      ");
            WriteLiteral(@"                          var type = $(input).attr(""type"");
                                //判断input type
                                switch (type) {
                                    case ""checkbox"":
                                        isChecked = $(input).prop(""checked"");
                                        //未选中
                                        if (!isChecked)
                                            return false;
                                        value = $(input).val().trim();
                                        break;
                                    default:
                                        value = $(input).val().trim();
                                        break;
                                }
                            }
                            if (objName != undefined && objName.length > 0)
                                eval(""row."" + objName + ""='"" + value.replace(""'"", ""\\'"") + ""'"");
                        });
              ");
            WriteLiteral(@"          if (!isObjNull(row) && isChecked) {
                            //校验标签数量是否超过采购数量
                            //var labelQty = parseFloat(row.LabelQty);
                            var qty = parseFloat(row.Qty);
                            //var copies = parseFloat(row.Copies);
                            var rule = parseInt(row.Rule);
                            var errMsg = """";
                            //if (isNaN(copies)) {
                            //    errMsg = ""请输入份数！"";
                            //}
                            //if (isNaN(labelQty)) {
                            //    errMsg = ""请输入标签内数量！"";
                            //}
                            if (isNaN(qty)) {
                                errMsg = ""采购数量异常！"";
                            }
                            if (isNaN(rule)) {
                                errMsg = ""规则解析异常！"";
                            }
                            labelQtys.forEach((labelQty) => {
                ");
            WriteLiteral(@"                if (labelQty <= 0 || labelQty > qty) {
                                    errMsg = ""标签内数量不能小于等于0或不能超过采购数量！"";
                                }
                            })
                            //if (copies <= 0 || copies >= 1000) {
                            //    errMsg = ""份数必须大于1并小于1000！"";
                            //}
                            if (errMsg != """") {
                                //alert(errMsg);
                                layer.msg(errMsg);
                                table = [];
                                return false;
                            }
                            table.push(row);
                        }
                    });

                    if (table.length <= 0)
                        return false;
                    //将用户输入的标签宽度和高度通过方法传输至后端
                    var width = document.getElementById(""Width"").value;
                    var height = document.getElementById(""Height"").value;
              ");
            WriteLiteral("      var para = { purchaseID:");
#nullable restore
#line 267 "D:\GIT.NEWARE.WORK\neware-mes\Sources\XY.Supplier.Web\Views\Purchase\Order_Print_Detail.cshtml"
                                       Write(Model.PurchaseID);

#line default
#line hidden
#nullable disable
            WriteLiteral(@" , itemStr: xy.jsonstr(table), width1: width, height1: height, isPrintOutPackage: isPrintOutPackage };

                    if (printType === 0) {
                        //请求生成二维码
                         //print(""order_generate_qrcode1"", para, webPrint, null);
                        //print(""order_generate_qrcode"", para, webPrint1, null);
                        print(""order_generate_qrcode"", para, PrintFile, null);
                    }
                    else {
                        try {
                            //判断上次打印是否结束
                            if (!dotnetPrint.isPrint()) {
                                $.ajax({
                                    type: ""post"",
                                    url: ""/purchase/order_qrcode"",
                                    data: { purchaseID:");
#nullable restore
#line 282 "D:\GIT.NEWARE.WORK\neware-mes\Sources\XY.Supplier.Web\Views\Purchase\Order_Print_Detail.cshtml"
                                                  Write(Model.PurchaseID);

#line default
#line hidden
#nullable disable
            WriteLiteral(@" , itemStr: xy.jsonstr(table)},
                                    contentType: ""application/x-www-form-urlencoded"",
                                    dataType: ""json"",
                                    beforeSend: function () {
                                        $(""#Print"").attr(""disabled"", true);
                                    },
                                    success: function (res) {
                                        if (res != null) {
                                            //调用打印
                                            dotnetPrint.purchaseItemPrint(res);
                                        } else {
                                            //alert(""调用打印失败！"");
                                            layer.msg(""调用打印失败"");
                                        }
                                        $(""#Print"").attr(""disabled"", false);
                                    },
                                    error: function (XMLHttpRequest, te");
            WriteLiteral(@"xtStatus, errorThrown) {
                                        console.log(XMLHttpRequest);
                                        console.log(textStatus);
                                        console.log(errorThrown);
                                        alert(""打印失败！"");
                                        $(""#Print"").attr(""disabled"", false);
                                    }
                                });
                            }
                            else {
                                alert(""上一打印任务尚未结束！"");
                            }
                        }
                        catch (error) {
                            alert(""网页端打印不需要此设置，请输入标签宽度、高度后点击打印按钮直接打印！"");
                            console.log(error);
                        }
                    }
                }
                else {
                    //alert('请至少选择一条记录！');
                    layer.msg(""请至少选择一条记录！"");
                    return false;
                }
   ");
            WriteLiteral(@"         }

            function PrintFile(res) {
                //图片打印
                printJS({
                    printable: res,
                    type: 'image'
                    //style: '{size:auto;margin: 0cm 0cm 0cm 0cm;}' //去除页眉页脚
                });
            }

            //打印参数设置
            $('#PrintConfig').on('click', function () {
                //请求生成二维码
                    try {
                        dotnetPrint.openConfigSetting();
                    }
                    catch (error) {
                        alert(""网页端打印不需要此设置，请输入标签宽度、高度后点击打印按钮直接打印！"");
                        console.log(error);
                    }
            });
            var selected_device;

            var errorCallback = function (errorMessage) {
                alert(""错误: 未能成功连接到打印机，请检查！"");
                    }
            var errorCallback1 = function (errorMessage) {
                    }

            //斑马打印机用
            function setup() {
                if (!i");
            WriteLiteral(@"sClient) {
                    //首先从应用程序中获取默认设备。

                    BrowserPrint.getDefaultDevice(""printer"", function (device) {
                        //添加设备
                        selected_device = device;
                    }, function (error) {
                        alert(""软件连接失败，请检查右下角是否启动了Zebra Browser Print"");
                    })
                }
            }
            //返回
            $('#Cancel').on('click', function () {
                window.location.href = '");
#nullable restore
#line 367 "D:\GIT.NEWARE.WORK\neware-mes\Sources\XY.Supplier.Web\Views\Purchase\Order_Print_Detail.cshtml"
                                   Write(Html.Raw(Model.Backurl));

#line default
#line hidden
#nullable disable
            WriteLiteral(@"';
            });

            //采购订单物料表格单击选中事件
            $('#tableOrderItem').find(""input:checkbox"").on('click', function () {
                //全选
                if ($(this).parent().is('th')) {
                    var isChecked = this.checked;
                    if (isChecked) {
                        $("".label-qty"").show();
                    }
                    else {
                        $("".label-qty"").hide();
                    }
                }
                //单选
                else {
                    if ($(this).is(':checked')) {
                        $(this).parent().parent().find(""td input:eq(1)"").show();
                        $(this).parent().parent().find(""td input:eq(2)"").show();
                        $(this).parent().parent().find(""td input:eq(3)"").show();
                        $(this).parent().parent().find(""td input:eq(4)"").show();
                    }
                    else {
                        $(this).parent().parent().find(""td i");
            WriteLiteral(@"nput:eq(1)"").hide();
                        $(this).parent().parent().find(""td input:eq(2)"").hide();
                        $(this).parent().parent().find(""td input:eq(3)"").hide();
                        $(this).parent().parent().find(""td input:eq(4)"").hide();
                    }
                }
            });

            //打印
            /*
                * apiName : 接口名
                * table : 物料参数集合
                * callback : 打印操作
                * client : 如果是客户端打印则需传入客户端打印对象
            */
            function print(apiName, para, callback, client) {
                $.ajax({
                    type: ""post"",
                    url: ""/purchase/"" + apiName,
                    data: para,
                    contentType: ""application/x-www-form-urlencoded"",
                    dataType: ""json"",
                    beforeSend: function () {
                        $(""#Print"").attr(""disabled"", true);
                    },
                    success: function (res) {
            WriteLiteral(@"
                        if (res != null) {
                            //调用打印
                            if (client == null)
                                callback(res);
                            else
                                callback(res, client);
                        } else {
                            alert(""打印失败！"");
                        }
                        $(""#Print"").attr(""disabled"", false);
                    },
                    error: function (XMLHttpRequest, textStatus, errorThrown) {
                        console.log(XMLHttpRequest);
                        console.log(textStatus);
                        console.log(errorThrown);
                        alert(""打印失败！"");
                        $(""#Print"").attr(""disabled"", false);
                    }
                });
            }
            //web端打印
            function webPrint1(res) {
                //遍历List表中所有图片的ASCii码
                //$.each(res, function (index, value) {
         ");
            WriteLiteral(@"       //    window.open(value, '_blank').location;
                //});

                //图片打印
                printJS({
                    printable: res,
                    type: 'image'
                });
            }
            //web端打印
            function webPrint(res) {
                //将要打印的图像发至打印机
                //$.each(res, function (index, value) {
                //    var printString1 = value;
                //    selected_device.send(printString1, undefined, errorCallback1);
                //});
                //$.each(res, function (index, value) {
                //    var printString2 = value;
                //    selected_device.send(printString2, undefined, errorCallback1);
                //});
                //$.each(res, function (index, value) {
                //    var printString3 = value;
                //    selected_device.send(printString3, undefined, errorCallback1);
                //});
                //$.each(res, function (index, val");
            WriteLiteral(@"ue) {
                //    var printString4 = value;
                //    selected_device.send(printString4, undefined, errorCallback1);
                //});
                //$.each(res, function (index, value) {
                //    var printString5 = value;
                //    selected_device.send(printString5, undefined, errorCallback1);
                //});
                //$.each(res, function (index, value) {
                //    var printString6 = value;
                //    selected_device.send(printString6, undefined, errorCallback1);
                //});

                //便利List表中所有图片的ASCii码
                $.each(res, function (index, value) {
                    var printString9 = ""^XA^FO0,0^XGR:ZLOGO"" + index + "".GRF,1,1^FS^XZ"";//调用该模板指令生成ZPL指令
                    console.log(printString9)
                    selected_device.send(printString9, undefined, errorCallback);//发送ZPL指令调用打印方法打印所需要的图像
                });
            }

            //客户端打印
            fun");
            WriteLiteral(@"ction clientPrint(res, client) {
                client.purchaseItemPrint(res);
            }
            //双击选择/取消选择
            $('#tableOrderItem').find(""tr"").each(function () {
                var tr = $(this);
                tr.dblclick(function () {
                    var rowCheckbox = tr.find(""input:checkbox"").first();
                    if (rowCheckbox.length > 0) {
                        if (rowCheckbox.is("":checked"")) {
                            //显示
                            tr.find(""td input:eq(1)"").show();
                            tr.find(""td input:eq(2)"").show();
                            tr.find(""td input:eq(3)"").show();
                            tr.find(""td input:eq(4)"").show();
                        }
                        else {
                            //隐藏
                            tr.find(""td input:eq(1)"").hide();
                            tr.find(""td input:eq(2)"").hide();
                            tr.find(""td input:eq(3)"").hide();
        ");
            WriteLiteral(@"                    tr.find(""td input:eq(4)"").hide();
                        }
                    }
                });
            });

            //判断对象是否为NULL
            function isObjNull(obj) {
                return Object.prototype.isPrototypeOf(obj) && Object.keys(obj).length === 0;
            }


            /// 22.10.20 供应商编码版本打印

");
            WriteLiteral(@"
        });
    });

    //]]>
</script>

<div class=""container-fluid"">
    <div class=""boss_table_top clearfix"">
        <div class=""fl mb10"">
            <label class=""mr3 b"" style=""color: #000; font-size: .95rem;"">物料条码打印</label>
        </div>
        <div class=""fr mb10"">
            <div class=""fl mb10"">
                <label class=""mr3"" for=""Width"">标签宽度（mm）:</label>
                <input type=""number"" id=""Width"" value=""90"" step=""1"" class=""mr3"" style=""width:200px;"" placeholder=""请输入标签宽度"" />
            </div>
            <div class=""fl mb10"">
                <label class=""mr3"" for=""Height"">标签高度（mm）:</label>
                <input type=""number"" id=""Height"" value=""40"" step=""1"" class=""mr3"" style=""width:200px;"" placeholder=""请输入标签高度"" />
            </div>
            <button id=""Print"" type=""button"" class=""btn btn-sm btn-warning"">
                <i class=""fa fa-print"" aria-hidden=""true""></i>
                <span>打印</span>
            </button>
            <button id=""PrintConfig"" ");
            WriteLiteral(@"type=""button"" class=""btn btn-sm btn-warning"">
                <i class=""fa fa-cog"" aria-hidden=""true""></i>
                <span>客户端打印参数设置</span>
            </button>
            <button id=""Cancel"" type=""button"" class=""btn btn-sm btn-warning"">
                <i class=""fas fa-arrow-left"" aria-hidden=""true""></i>
                <span>返回</span>
            </button>
        </div>
    </div>
    <div class=""boss_table_body table-responsive"">
        <table id=""tableOrderItem"" class=""table table-bordered table-hover"">
            <thead>
                <tr>
                    <th class=""w2p""><input id=""SelectAll"" type=""checkbox"" /></th>
                    <th class=""w10p"">名称</th>
                    <th class=""w10p"">编码</th>
                    <th class=""w20p"">备注</th>
                    <th class=""w10p text-right"">数量</th>
                    <th class=""w10p text-right"">标签内数量</th>
                    <th class=""w10p text-right"">份数</th>
");
            WriteLiteral("                    <th class=\"w10p\">规则</th>\r\n");
            WriteLiteral("                </tr>\r\n            </thead>\r\n            <tbody>\r\n");
#nullable restore
#line 853 "D:\GIT.NEWARE.WORK\neware-mes\Sources\XY.Supplier.Web\Views\Purchase\Order_Print_Detail.cshtml"
                 if (Model.OrderItemList != null && Model.OrderItemList.Rows.Count > 0)
                {
                    foreach (DataRow dr in Model.OrderItemList.Rows)
                    {

#line default
#line hidden
#nullable disable
            WriteLiteral("                        <tr>\r\n                            <td name=\"ItemID\"><input type=\"checkbox\"");
            BeginWriteAttribute("value", " value=\"", 38193, "\"", 38217, 1);
#nullable restore
#line 858 "D:\GIT.NEWARE.WORK\neware-mes\Sources\XY.Supplier.Web\Views\Purchase\Order_Print_Detail.cshtml"
WriteAttributeValue("", 38201, dr["item_id"], 38201, 16, false);

#line default
#line hidden
#nullable disable
            EndWriteAttribute();
            WriteLiteral(" /></td>\r\n                            <td name=\"ItemName\">");
#nullable restore
#line 859 "D:\GIT.NEWARE.WORK\neware-mes\Sources\XY.Supplier.Web\Views\Purchase\Order_Print_Detail.cshtml"
                                           Write(dr["name"]);

#line default
#line hidden
#nullable disable
            WriteLiteral("</td>\r\n                            <td name=\"ItemCode\">");
#nullable restore
#line 860 "D:\GIT.NEWARE.WORK\neware-mes\Sources\XY.Supplier.Web\Views\Purchase\Order_Print_Detail.cshtml"
                                           Write(dr["code"]);

#line default
#line hidden
#nullable disable
            WriteLiteral("</td>\r\n                            <td name=\"remark\"><input type=\"text\" class=\"w100p\" style=\"display:none;\" /></td>\r\n                            <td name=\"Qty\" class=\"text-right\">");
#nullable restore
#line 862 "D:\GIT.NEWARE.WORK\neware-mes\Sources\XY.Supplier.Web\Views\Purchase\Order_Print_Detail.cshtml"
                                                         Write(dr["item_qty"].ToString().TrimEnd('0').TrimEnd('.'));

#line default
#line hidden
#nullable disable
            WriteLiteral("</td>\r\n");
#nullable restore
#line 863 "D:\GIT.NEWARE.WORK\neware-mes\Sources\XY.Supplier.Web\Views\Purchase\Order_Print_Detail.cshtml"
                             if ((dr["rule"].ToString() == "1"))
                            {

#line default
#line hidden
#nullable disable
            WriteLiteral("                                <td name=\"LabelQty\"><input type=\"number\" class=\"label-qty text-right w100p\" value=\"1\" readonly=\"readonly\" style=\"display:none;\" /></td>\r\n");
#nullable restore
#line 866 "D:\GIT.NEWARE.WORK\neware-mes\Sources\XY.Supplier.Web\Views\Purchase\Order_Print_Detail.cshtml"
                            }
                            else
                            {

#line default
#line hidden
#nullable disable
            WriteLiteral("                                <td name=\"LabelQty\"><input type=\"number\" step=\"1\" class=\"label-qty text-right w100p\" style=\"display:none;\" /></td>\r\n");
#nullable restore
#line 870 "D:\GIT.NEWARE.WORK\neware-mes\Sources\XY.Supplier.Web\Views\Purchase\Order_Print_Detail.cshtml"
                            }

#line default
#line hidden
#nullable disable
            WriteLiteral("                            <td name=\"Copies\"><input type=\"number\" step=\"1\" class=\"label-qty text-right w100p\"");
            BeginWriteAttribute("value", " value=", 39242, "", 39287, 1);
#nullable restore
#line 871 "D:\GIT.NEWARE.WORK\neware-mes\Sources\XY.Supplier.Web\Views\Purchase\Order_Print_Detail.cshtml"
WriteAttributeValue("", 39249, (dr["rule"].ToString()=="0")?1:null, 39249, 38, false);

#line default
#line hidden
#nullable disable
            EndWriteAttribute();
            WriteLiteral(" style=\"display:none;\" /></td>\r\n\r\n                            <td>");
#nullable restore
#line 873 "D:\GIT.NEWARE.WORK\neware-mes\Sources\XY.Supplier.Web\Views\Purchase\Order_Print_Detail.cshtml"
                            Write(XY.Enums.boss.Tools.item_rule((XY.Enums.boss.item_rule)dr["rule"]));

#line default
#line hidden
#nullable disable
            WriteLiteral("</td>\r\n                            <td name=\"Rule\" style=\"display:none\"><input type=\"number\"");
            BeginWriteAttribute("value", " value=", 39514, "", 39534, 1);
#nullable restore
#line 874 "D:\GIT.NEWARE.WORK\neware-mes\Sources\XY.Supplier.Web\Views\Purchase\Order_Print_Detail.cshtml"
WriteAttributeValue("", 39521, dr["rule"], 39521, 13, false);

#line default
#line hidden
#nullable disable
            EndWriteAttribute();
            WriteLiteral(" /></td>\r\n                        </tr>\r\n");
#nullable restore
#line 876 "D:\GIT.NEWARE.WORK\neware-mes\Sources\XY.Supplier.Web\Views\Purchase\Order_Print_Detail.cshtml"

                    }
                }

#line default
#line hidden
#nullable disable
            WriteLiteral(@"            </tbody>
        </table>
    </div>

    <div class=""tips"">
        <p class=""tip"">温馨提示</p>
        <p class=""tip"">1.打印标签时，若外箱包装内物料无小包装，则点击打印标签时，不需要打印外包装标签</p>
        <p class=""tip"">2.若外箱包装内物料有小包装，则标签内数量为小包装内物料数量，份数为小包装数量，并且点击打印后需要选择外包装标签</p>
    </div>
</div>


<!--物料条码打印对话框-->
<div class=""modal fade"" id=""dialogOrderPrintInput"" tabindex=""-1"" role=""dialog"" aria-labelledby=""dialogTitle"" aria-hidden=""true"" data-backdrop=""static"">
    <div class=""modal-dialog modal-dialog-centered modal-lg"" role=""document"">
        <div class=""modal-content"" style=""width:100%; background-color: #ecf0f5;"">
            <div class=""modal-header"" style=""font-family: 'Arial Negreta', 'Arial Normal', 'Arial';font-size:18px; "">
                <span class=""modal-title"" id=""dialogTitle"">物料条码打印</span>
                <button type=""button"" class=""close"" data-dismiss=""modal"" aria-label=""关闭"">
                    <span aria-hidden=""true"">&times;</span>
                </button>
            </div>
         ");
            WriteLiteral(@"   <div class=""modal-body fz0875r"">
                <div style=""margin-bottom:15px;"">
                    <button type=""button"" id=""addRow"" class=""btn btn-warning btn-sm w100"">添加行</button>
                    <button type=""button"" id=""removeRow"" class=""btn btn-secondary btn-sm w100"">删除行</button>
                </div>
                <div style=""overflow: hidden"">
                    <table id=""tbOrderPrintInput""></table>
                </div>
            </div>
            <div class=""modal-footer"" style=""background-color: #ecf0f5;margin:auto;"">
                <button type=""button"" id=""btnSubmit"" class=""btn btn-warning btn-sm w100"">确定</button>
                <button type=""button"" class=""btn btn-secondary btn-sm w100"" data-dismiss=""modal"">取消</button>
            </div>
        </div>
    </div>
</div>


");
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
        public global::Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper<XY.Supplier.Web.Models.Purchase.PurchaseOrder_Print_Detail> Html { get; private set; }
    }
}
#pragma warning restore 1591