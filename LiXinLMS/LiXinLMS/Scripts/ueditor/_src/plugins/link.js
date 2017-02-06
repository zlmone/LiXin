﻿/// import core
/// commands 超链接,取消链接
/// commandsName  Link,Unlink
/// commandsTitle  超链接,取消链接
/// commandsDialog  dialogs\link\link.html
/**
 * 超链接
 * @function
 * @name baidu.editor.execCommand
 * @param   {String}   cmdName     link插入超链接
 * @param   {Object}  options         url地址，title标题，target是否打开新页
 * @author zhanyi
 */
/**
 * 取消链接
 * @function
 * @name baidu.editor.execCommand
 * @param   {String}   cmdName     unlink取消链接
 * @author zhanyi
 */
(function() {

    function optimize(range) {
        var start = range.startContainer, end = range.endContainer;

        if (start = domUtils.findParentByTagName(start, 'a', true)) {
            range.setStartBefore(start);
        }
        if (end = domUtils.findParentByTagName(end, 'a', true)) {
            range.setEndAfter(end);
        }
    }

    UE.commands['unlink'] = {
        execCommand: function() {
            var as,
                range = new dom.Range(this.document),
                tds = this.currentSelectedArr,
                bookmark;
            if (tds && tds.length > 0) {
                for (var i = 0, ti; ti = tds[i++];) {
                    as = domUtils.getElementsByTagName(ti, 'a');
                    for (var j = 0, aj; aj = as[j++];) {
                        domUtils.remove(aj, true);
                    }
                }
                if (domUtils.isEmptyNode(tds[0])) {
                    range.setStart(tds[0], 0).setCursor();
                } else {
                    range.selectNodeContents(tds[0]).select();
                }
            } else {
                range = this.selection.getRange();
                if (range.collapsed && !domUtils.findParentByTagName(range.startContainer, 'a', true)) {
                    return;
                }
                bookmark = range.createBookmark();
                optimize(range);
                range.removeInlineStyle('a').moveToBookmark(bookmark).select();
            }
        },
        queryCommandState: function() {
            return !this.highlight && this.queryCommandValue('link') ? 0 : -1;
        }
    };

    function doLink(range, opt) {

        optimize(range = range.adjustmentBoundary());
        var start = range.startContainer;
        if (start.nodeType == 1) {
            start = start.childNodes[range.startOffset];
            if (start && start.nodeType == 1 && start.tagName == 'A' && /^(?:https?|ftp|file)\s*:\s*\/\//.test(start[browser.ie ? 'innerText' : 'textContent'])) {
                start.innerHTML = opt.href;
            }
        }
        range.removeInlineStyle('a');
        if (range.collapsed) {
            var a = range.document.createElement('a');
            domUtils.setAttributes(a, opt);
            a.innerHTML = opt.href;
            range.insertNode(a).selectNode(a);
        } else {
            range.applyInlineStyle('a', opt);
        }
    }

    UE.commands['link'] = {
        queryCommandState: function() {
            return this.highlight ? -1 : 0;
        },
        execCommand: function(cmdName, opt) {
            var range = new dom.Range(this.document),
                tds = this.currentSelectedArr;

            if (tds && tds.length) {
                for (var i = 0, ti; ti = tds[i++];) {
                    if (domUtils.isEmptyNode(ti)) {
                        ti.innerHTML = opt.href;
                    }
                    doLink(range.selectNodeContents(ti), opt);
                }
                range.selectNodeContents(tds[0]).select();
            } else {
                doLink(range = this.selection.getRange(), opt);

                range.collapse().select(browser.gecko ? true : false);

            }
        },
        queryCommandValue: function() {

            var range = new dom.Range(this.document),
                tds = this.currentSelectedArr,
                as,
                node;
            if (tds && tds.length) {
                for (var i = 0, ti; ti = tds[i++];) {
                    as = ti.getElementsByTagName('a');
                    if (as[0])
                        return as[0];
                }
            } else {
                range = this.selection.getRange();


                if (range.collapsed) {
                    node = this.selection.getStart();
                    if (node && (node = domUtils.findParentByTagName(node, 'a', true))) {
                        return node;
                    }
                } else {
                    //trace:1111  如果是<p><a>xx</a></p> startContainer是p就会找不到a
                    range.shrinkBoundary();
                    var start = range.startContainer.nodeType == 3 || !range.startContainer.childNodes[range.startOffset] ? range.startContainer : range.startContainer.childNodes[range.startOffset],
                        end = range.endContainer.nodeType == 3 || range.endOffset == 0 ? range.endContainer : range.endContainer.childNodes[range.endOffset - 1],
                        common = range.getCommonAncestor();


                    node = domUtils.findParentByTagName(common, 'a', true);
                    if (!node && common.nodeType == 1) {

                        var as = common.getElementsByTagName('a'),
                            ps, pe;

                        for (var i = 0, ci; ci = as[i++];) {
                            ps = domUtils.getPosition(ci, start), pe = domUtils.getPosition(ci, end);
                            if ((ps & domUtils.POSITION_FOLLOWING || ps & domUtils.POSITION_CONTAINS)
                                &&
                                (pe & domUtils.POSITION_PRECEDING || pe & domUtils.POSITION_CONTAINS)) {
                                node = ci;
                                break;
                            }
                        }
                    }

                    return node;
                }
            }


        }
    };


})();