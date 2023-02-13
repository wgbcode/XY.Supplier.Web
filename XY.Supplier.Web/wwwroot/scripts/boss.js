//<![CDATA[

/************************************************************
站点公共
************************************************************/

define(['jquery', 'xy', 'hoverDelay'], function ($, xy, hoverDelay) {

    //样式配置
    var configPrimaryColor = '#007db8';
    var configActiveColor = '#ffb600';

    //TABS翻页按钮宽度
    var _tabsButtonWidth = 38;
    var _tabsMoveTimer;
    var _tabsMoveUnit = 10;

    /*
        表格初始化
        _table: jQuery表格元素对象
    */
    var tableInit = function (_table) {
        if (_table == null || _table.length == 0)
            return false;
        
        var _tableCheckbox = _table.find("input:checkbox");
        _tableCheckbox.on('click', function () {
            //全选
            if ($(this).parent().is('th')) {
                var isChecked = this.checked;
                _tableCheckbox.not(this).prop('checked', isChecked);
                _tableCheckbox.not(this).each(function () {
                    var _rowTag = $(this).parent().parent();
                    if (isChecked) {
                        if (!_rowTag.hasClass('bg-primary'))
                            _rowTag.addClass('bg-primary');
                    }
                    else {
                        if (_rowTag.hasClass('bg-primary'))
                            _rowTag.removeClass('bg-primary');
                    }
                });
            }
            //单选
            else {
                if ($(this).is(':checked')) {
                    $(this).parent().parent().addClass('bg-primary');
                }
                else {
                    $(this).parent().parent().removeClass('bg-primary');
                }
            }
        });

        //双击选择/取消选择
        var _tbody = _table.find("tbody:first");
        _tbody.find("tr").each(function () {
            var _tr = $(this);
            _tr.dblclick(function () {
                var _rowCheckbox = _tr.find("input:checkbox").first();
                if (_rowCheckbox.length > 0) {
                    if (_rowCheckbox.is(":checked")) {
                        //取消
                        _rowCheckbox.prop('checked', false);
                        _tr.removeClass('bg-primary');
                    }
                    else {
                        //选中
                        _rowCheckbox.prop('checked', true);
                        _tr.addClass('bg-primary');
                    }
                }
            });
        });
    };

    /*
        获取表格中所有选中行的值(返回数组)
        _table: jQuery表格元素对象
    */
    var tableSelectValue = function (_table) {
        if (_table == null || _table.length == 0)
            return null;
        var s = new Array();
        _table.find("input:checked[type=checkbox]").each(function () {
            //$(this).is(':checked')
            if ($(this).attr('id') != 'SelectAll') {
                s.push($(this).val());
            }
        });
        return s;
    };

    /*
        标签初始化
    */
    var tabInit = function () {
        var _boss_tabs = xy.istop() ? $('#boss_tabs') : $('#boss_tabs', window.top.document);
        var _tabs = _boss_tabs.find('#boss_tabs_box');
        var _iframes = xy.istop() ? $('#boss_iframe') : $('#boss_iframe', window.top.document);
        //prev、next按钮hover事件
        _boss_tabs.find('#boss_tabs_prev,#boss_tabs_next').hover(
            function () {
                $(this).css('background-color', '#f6f6f6');
            },
            function () {
                $(this).css('background-color', '#ffffff');
            });
        //prev按钮click事件
        _boss_tabs.find('#boss_tabs_prev').mousedown(function () {
            _tabsMoveTimer = setInterval(function () {
                var _tabs = xy.istop() ? $('#boss_tabs_box') : $('#boss_tabs_box', window.top.document);
                var _tabsLeft = parseFloat(_tabs.css('left'));
                if (_tabsLeft < _tabsButtonWidth) {
                    _tabs.css('left', _tabsLeft + (_tabsButtonWidth - _tabsLeft >= _tabsMoveUnit ? _tabsMoveUnit : _tabsButtonWidth - _tabsLeft) + 'px');
                }
            }, 100);
        }).mouseup(function () {
            clearInterval(_tabsMoveTimer);
        });
        //next按钮click事件
        _boss_tabs.find('#boss_tabs_next').mousedown(function () {
            _tabsMoveTimer = setInterval(function () {
                var _tabs = xy.istop() ? $('#boss_tabs_box') : $('#boss_tabs_box', window.top.document);
                var _maxLength = $('#boss_tabs_next').position().left - _tabsButtonWidth;//视觉范围内允许的Tabs总长度（next按钮距左距离-prev按钮宽度）
                var _tabsLeft = parseFloat(_tabs.css('left'));
                var _tabsRight = _tabs.width() + _tabsLeft;//_tabs移动后右边界坐标
                var _maxRight = _maxLength + _tabsButtonWidth;//next按钮坐标
                if (_tabs.width() > _maxLength && _tabsRight > _maxRight) {
                    _tabs.css('left', _tabsLeft - (_tabsRight - _maxRight >= _tabsMoveUnit ? _tabsMoveUnit : _tabsRight - _maxRight) + 'px');
                }
            }, 100);
        }).mouseup(function () {
            clearInterval(_tabsMoveTimer);
        });
        //more按钮hover事件
        var _boss_tabs_more = _boss_tabs.find('#boss_tabs_more');
        _boss_tabs_more.hoverDelay({
            hoverDuring: 500,
            hoverEvent: function () {
                $(this).children().eq(0).css('background-color', '#f6f6f6');
                $(this).children().eq(1).removeClass('hide');
            },
            outDuring: 100,
            outEvent: function () {
                $(this).children().eq(0).css('background-color', '#ffffff');
                $(this).children().eq(1).addClass('hide');
            }
        });
        //为默认标签添加选中（点击）事件
        _tabs.children().first().click(function () {
            tabSelect($(this).attr('id'));
        });
        //关闭当前标签
        _boss_tabs_more.children("div:last").children().eq(0).click(function () {
            tabCloseCurrent();
        });
        //关闭其它标签
        _boss_tabs_more.children("div:last").children().eq(1).click(function () {
            tabCloseOther();
        });
        //关闭全部标签
        _boss_tabs_more.children("div:last").children().eq(2).click(function () {
            tabCloseAll();
        });
    };

    /*
        新增标签
        id: 标签唯一ID
        name: 标签名称
        url: 标签页面地址
    */
    var tabAdd = function (id, name, url) {
        var _tabs = xy.istop() ? $('#boss_tabs_box') : $('#boss_tabs_box', window.top.document);
        var _iframes = xy.istop() ? $('#boss_iframe') : $('#boss_iframe', window.top.document);
        var _tab = _tabs.find('#' + id); //待新增当前标签
        //标签已存在
        if (_tab.length > 0) {
            tabSelect(id);
        }
        //标签不存在
        else {
            //上一当前tab新增hover事件并移除当前效果
            _tabs.children('.boss_tab_curr:first').removeClass('boss_tab_curr').addClass('boss_tab_norm').hover(function () {
                    $(this).removeClass('boss_tab_norm').addClass('boss_tab_curr');
                }, function () {
                    $(this).removeClass('boss_tab_curr').addClass('boss_tab_norm');
                });
            //添加新标签
            var newTab = $("<div class='boss_tab boss_tab_curr' id='" + id + "'></div>");
            newTab.click(function () { //选中（点击）事件
                tabSelect(id);
            });
            var newTabSpan = $("<span>" + name + " </span>");
            var newTabI = $("<i class='fas fa-times'></i>");
            newTabI.click(function () { //关闭事件
                tabClose(id);
            });
            newTabI.hover(function () {
                $(this).addClass('active_color');
            }, function () {
                $(this).removeClass('active_color');
            });
            newTab.append(newTabSpan);
            newTab.append(newTabI);
            _tabs.append(newTab);
            //标签自动滚动
            //var _tabsWidth = _tabs.outerWidth(true);//直接使用_tabs.width()存在取值不准确问题，原因待查明
            var _tabsWidth = 0;
            _tabs.children().each(function () {
                _tabsWidth += $(this).outerWidth(true);
            });
            var _maxLength = $('#boss_tabs_next').position().left - _tabsButtonWidth;//视觉范围内允许的Tabs总长度（next按钮距左距离-prev按钮宽度）
            if (_tabsWidth > _maxLength) {
                _tabs.animate({ left: _maxLength + _tabsButtonWidth - _tabsWidth + 'px' }, 500);
            }
            //隐藏之前活动的iframe
            _iframes.children('.boss_iframe_active:first').removeClass('boss_iframe_active').addClass('boss_iframe_sleep');
            //loading...
            var _loading_img_width = 30;
            var _iframes_mask = _iframes.prev();
            _iframes_mask.removeClass('hide').css({
                "left": (_iframes.width() - _loading_img_width) / 2 + "px",
                "top": (_iframes.height() - _loading_img_width) / 2 + "px"
            });
            //添加新iframe
            var _iframe = $("<iframe id='iframe_" + id + "' src='" + url + "' class='boss_iframe_active'></iframe>");
            _iframes.append(_iframe);
            _iframe.on('load', function () {
                if (!_iframes_mask.hasClass('hide'))
                    _iframes_mask.addClass('hide');
            });
        }
    };

    /*
        选中标签
        id：标签ID
    */
    var tabSelect = function (id) {
        var _tabs = xy.istop() ? $('#boss_tabs_box') : $('#boss_tabs_box', window.top.document);
        var _iframes = xy.istop() ? $('#boss_iframe') : $('#boss_iframe', window.top.document);
        var tabCurrentLast = _tabs.children('.boss_tab_curr').not('#' + id).first();
        var tabCurrent = _tabs.children('#' + id);
        if (tabCurrent.length > 0) {
            //如果存在上一活动标签（直接切换标签）
            if (tabCurrentLast.length > 0) {
                //上一当前tab新增hover事件并移除当前效果
                tabCurrentLast.removeClass('boss_tab_curr').addClass('boss_tab_norm').hover(function () {
                        $(this).removeClass('boss_tab_norm').addClass('boss_tab_curr');
                    }, function () {
                        $(this).removeClass('boss_tab_curr').addClass('boss_tab_norm');
                    });
                //隐藏iframe
                _iframes.children('#iframe_' + tabCurrentLast.attr('id')).removeClass('boss_iframe_active').addClass('boss_iframe_sleep');
            }
            //设置当前标签
            tabCurrent.removeClass('boss_tab_norm').addClass('boss_tab_curr').unbind('mouseenter mouseleave');
            //标签自动滚动
            var _tabCurrentLeft = 0;//当前标签在tabs容器中的左边距
            tabCurrent.prevAll().each(function () {
                _tabCurrentLeft += $(this).outerWidth(true);
            });
            var _tabsLeft = _tabs.position().left;//tabs左偏移值 
            var _nextButtonLeft = $('#boss_tabs_next').position().left;//next按钮左边距
            if (_tabCurrentLeft + _tabsLeft < _tabsButtonWidth) {
                _tabs.animate({ left: _tabsButtonWidth - _tabCurrentLeft + 'px' }, 500);
            }
            if (_tabCurrentLeft + tabCurrent.outerWidth(true) + _tabsLeft > _nextButtonLeft) {
                _tabs.animate({ left: _tabsButtonWidth - _tabCurrentLeft + (_nextButtonLeft - _tabsButtonWidth) - tabCurrent.outerWidth(true) + 'px' }, 500);
            }
            //显示iframe
            _iframes.find('#iframe_' + id).removeClass('boss_iframe_sleep').addClass('boss_iframe_active');
        }
    };

    /*
        刷新标签页
        id：标签ID，如不传入该参数则刷新当前标签页
    */
    var tabRefresh = function (id) {
        var _iframes = xy.istop() ? $('#boss_iframe') : $('#boss_iframe', window.top.document);
        var _iframes_mask = _iframes.prev();
        var _iframe = id ? _iframes.children('#iframe_' + id) : _iframes.children('.boss_iframe_active:first');
        //loading
        var _loading_img_width = 30;
        _iframes_mask.removeClass('hide').css({
            "left": (_iframes.width() - _loading_img_width) / 2 + "px",
            "top": (_iframes.height() - _loading_img_width) / 2 + "px"
        });
        //刷新页面
        var _src = _iframe.attr('src');
        _iframe.attr('src', _src);
        _iframe.on('load', function () {
            if (!_iframes_mask.hasClass('hide'))
                _iframes_mask.addClass('hide');
        });
    };

    /*
        关闭指定标签
        id：标签ID
    */
    var tabClose = function (id) {
        var _tabs = xy.istop() ? $('#boss_tabs_box') : $('#boss_tabs_box', window.top.document);
        var _iframes = xy.istop() ? $('#boss_iframe') : $('#boss_iframe', window.top.document);
        var tabCurrent = _tabs.children('#' + id);
        //第一个默认标签不允许关闭
        if (tabCurrent.index() > 0) {
            var tabPrev = tabCurrent.prev();
            //删除
            tabCurrent.remove();
            _iframes.find('#iframe_' + tabCurrent.attr('id')).remove();
            //选中上一标签
            if (tabPrev.length > 0) {
                tabSelect(tabPrev.attr('id'));
            }
        }
    };

    /*
        关闭当前标签
    */
    var tabCloseCurrent = function () {
        var _tabs = xy.istop() ? $('#boss_tabs_box') : $('#boss_tabs_box', window.top.document);
        var tabCurrent = _tabs.children('.boss_tab_curr:first');
        //第一个默认标签不允许关闭
        if (tabCurrent.index() > 0) {
            tabClose(tabCurrent.attr('id'));
        }        
    };

    /*
        关闭其它标签
    */
    var tabCloseOther = function () {
        var _tabs = xy.istop() ? $('#boss_tabs_box') : $('#boss_tabs_box', window.top.document);
        var _iframes = xy.istop() ? $('#boss_iframe') : $('#boss_iframe', window.top.document);
        if (_tabs.children().length > 1) {
            _tabs.children().each(function () {
                var _tab = $(this);
                var _iframe = _iframes.find('#iframe_' + _tab.attr('id'));
                //其它（非默认）标签
                if (_tab.index() > 0) {
                    if (!_tab.hasClass('boss_tab_curr')) {
                        _tab.remove();
                        _iframe.remove();
                    }
                }
            });
        }
        //标签控制区位置还原
        _tabs.animate({ left: _tabsButtonWidth + 'px' }, 500);
    };

    /*
        关闭全部标签
    */
    var tabCloseAll = function () {
        var _tabs = xy.istop() ? $('#boss_tabs_box') : $('#boss_tabs_box', window.top.document);
        var _iframes = xy.istop() ? $('#boss_iframe') : $('#boss_iframe', window.top.document);
        if (_tabs.children().length > 1) {
            _tabs.children().each(function () {
                var _tab = $(this);
                var _iframe = _iframes.find('#iframe_' + _tab.attr('id'));
                //默认标签
                if (_tab.index() == 0) {
                    if (!_tab.hasClass('boss_tab_curr')) {
                        _tab.removeClass('boss_tab_norm').addClass('boss_tab_curr');
                    }
                    if (!_iframe.hasClass('boss_iframe_active')) {
                        _iframe.removeClass('boss_iframe_sleep').addClass('boss_iframe_active');
                    }
                }
                //其它标签
                else {
                    _tab.remove();
                    _iframe.remove();
                }
            });
        }
        //标签控制区位置还原
        _tabs.animate({ left: _tabsButtonWidth + 'px' }, 500);
    };

    /*
        以Loading效果在当前iframe中加载页面
        url：页面地址
     */
    var load = function (url) {
        var _iframes = xy.istop() ? $('#boss_iframe') : $('#boss_iframe', window.top.document);
        var _iframes_mask = _iframes.prev();
        var _iframe_active = _iframes.children('.boss_iframe_active:first');
        //loading
        var _loading_img_width = 30;
        _iframes_mask.removeClass('hide').css({
            "left": (_iframes.width() - _loading_img_width) / 2 + "px",
            "top": (_iframes.height() - _loading_img_width) / 2 + "px"
        });
        //加载页面
        _iframe_active.attr('src', url);
        _iframe_active.on('load', function () {
            if (!_iframes_mask.hasClass('hide'))
                _iframes_mask.addClass('hide');
        });
    };

    /*
        以Loading效果在当前iframe中提交表单
        form：待提交表单
     */
    var submit = function (form) {
        var _iframes = xy.istop() ? $('#boss_iframe') : $('#boss_iframe', window.top.document);
        var _iframes_mask = _iframes.prev();
        var _iframe_active = _iframes.children('.boss_iframe_active:first');
        //loading
        var _loading_img_width = 30;
        _iframes_mask.removeClass('hide').css({
            "left": (_iframes.width() - _loading_img_width) / 2 + "px",
            "top": (_iframes.height() - _loading_img_width) / 2 + "px"
        });
        //提交表单
        form.submit();
        _iframe_active.on('load', function () {
            if (!_iframes_mask.hasClass('hide'))
                _iframes_mask.addClass('hide');
        });
    };

    /*
        以Loading效果在当前iframe中POST数据
        url：提交地址
        data：提交数据
     */
    var post = function (url, data) {
        var _iframes = xy.istop() ? $('#boss_iframe') : $('#boss_iframe', window.top.document);
        var _iframes_mask = _iframes.prev();
        var _iframe_active = _iframes.children('.boss_iframe_active:first');
        //loading
        var _loading_img_width = 30;
        _iframes_mask.removeClass('hide').css({
            "left": (_iframes.width() - _loading_img_width) / 2 + "px",
            "top": (_iframes.height() - _loading_img_width) / 2 + "px"
        });
        //POST数据
        xy.post(url, data);
        _iframe_active.on('load', function () {
            if (!_iframes_mask.hasClass('hide'))
                _iframes_mask.addClass('hide');
        });
    };

    return {
        configPrimaryColor: configPrimaryColor,
        configActiveColor: configActiveColor,

        tableInit: tableInit,
        tableSelectValue: tableSelectValue,

        tabInit: tabInit,
        tabAdd: tabAdd,
        tabSelect: tabSelect,
        tabRefresh: tabRefresh,
        tabClose: tabClose,
        tabCloseCurrent: tabCloseCurrent,
        tabCloseOther: tabCloseOther,
        tabCloseAll: tabCloseAll,

        load: load,
        submit: submit,
        post: post
    };
});

//]]>