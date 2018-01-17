~function ($) {
    $(function () {
        if (/MSIE [6-9]\./.test(navigator.userAgent)) {

            var _hideph = function (e) {
                $(this).parent().find('.phholder')[this.value === '' ? 'show' : 'hide']();
            }
            $("input[data-placeholder]").each(function () {

                $this = $(this);
                var wrap = $this.wrap('<span class="placeholder-wrap" style="float:' + $this.css('float') + '"></span>').parent();
                var _lineHeight = $this.data('line-height');
                _lineHeight ? 0 : _lineHeight = $this.css('line-height');
                if (/^\d+$/.test(_lineHeight)) _lineHeight += 'px';
                var defaultDisplay = $this.value === '' ? 'blovk' : 'none';
                $this.after($('<div class="phholder" style="display:'+defaultDisplay+';position: absolute; top: -10px; left: 0px; z-index:0; width: ' + $this.width() + 'px;line-height:' + _lineHeight + ';"><span class="placeholder" aria-hidden="true" style="text-indent:-7px;line-height:' + _lineHeight + ';font-size:' + $this.css('fontSize') + '">' + $this.data('placeholder') + '</span></div>').on('focus click', function (e) {
                    $(this).parent().find('input').focus();
                }));
                $this.on('keyup change blur drop dragend mousedown mouseup', _hideph)
                $this[0].onpaste = _hideph;
                $this[0].onpropertychange = _hideph;
                $this.removeAttr('data-placeholder');
            });
            $("textarea[data-placeholder]").each(function () {

                $this = $(this);
                var wrap = $this.wrap('<span class="placeholder-wrap" style="float:' + $this.css('float') + '"></span>').parent();
                var _lineHeight = $this.data('line-height');
                _lineHeight ? 0 : _lineHeight = $this.css('line-height');
                if (/^\d+$/.test(_lineHeight)) _lineHeight += 'px';
                $this.after($('<div class="phholder" style="position: absolute; top: -40px; left: 0px; z-index:0; width: ' + ($this.width() - 10) + 'px;line-height:' + _lineHeight + ';"><span onclick="$(this).parent().prev().focus();" class="placeholder" aria-hidden="true" style="margin-left:10px;line-height:' + _lineHeight + ';font-size:' + $this.css('fontSize') + '">' + $this.data('placeholder') + '</span></div>').on('focus click', function (e) {
                    $(this).parent().find('input').focus();
                }));
                $this.on('keyup change blur drop dragend mousedown mouseup', _hideph)
                $this[0].onpaste = _hideph;
                $this[0].onpropertychange = _hideph;
                $this.removeAttr('data-placeholder');
            });
        } else {
            $("input[data-placeholder],textarea[data-placeholder]").each(function () { $this = $(this); $this.attr('placeholder', $this.data('placeholder')) })
        }
    });
} (jQuery)