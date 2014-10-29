!function(a,t){"use strict";var i={"font-styles":function(a,t){var i=t&&t.size?" btn-"+t.size:"";return"<li class='dropdown'><a class='btn dropdown-toggle"+i+"' data-toggle='dropdown' href='#'><i class='icon-font'></i>&nbsp;<span class='current-font'>"+a.font_styles.normal+"</span>&nbsp;<b class='caret'></b></a><ul class='dropdown-menu'><li><a data-wysihtml5-command='formatBlock' data-wysihtml5-command-value='div' tabindex='-1'>"+a.font_styles.normal+"</a></li><li><a data-wysihtml5-command='formatBlock' data-wysihtml5-command-value='h1' tabindex='-1'>"+a.font_styles.h1+"</a></li><li><a data-wysihtml5-command='formatBlock' data-wysihtml5-command-value='h2' tabindex='-1'>"+a.font_styles.h2+"</a></li><li><a data-wysihtml5-command='formatBlock' data-wysihtml5-command-value='h3' tabindex='-1'>"+a.font_styles.h3+"</a></li><li><a data-wysihtml5-command='formatBlock' data-wysihtml5-command-value='h4'>"+a.font_styles.h4+"</a></li><li><a data-wysihtml5-command='formatBlock' data-wysihtml5-command-value='h5'>"+a.font_styles.h5+"</a></li><li><a data-wysihtml5-command='formatBlock' data-wysihtml5-command-value='h6'>"+a.font_styles.h6+"</a></li></ul></li>"},emphasis:function(a,t){var i=t&&t.size?" btn-"+t.size:"";return"<li><div class='btn-group'><a class='btn"+i+"' data-wysihtml5-command='bold' title='CTRL+B' tabindex='-1'>"+a.emphasis.bold+"</a><a class='btn"+i+"' data-wysihtml5-command='italic' title='CTRL+I' tabindex='-1'>"+a.emphasis.italic+"</a><a class='btn"+i+"' data-wysihtml5-command='underline' title='CTRL+U' tabindex='-1'>"+a.emphasis.underline+"</a></div></li>"},lists:function(a,t){var i=t&&t.size?" btn-"+t.size:"";return"<li><div class='btn-group'><a class='btn"+i+"' data-wysihtml5-command='insertUnorderedList' title='"+a.lists.unordered+"' tabindex='-1'><i class='icon-list'></i></a><a class='btn"+i+"' data-wysihtml5-command='insertOrderedList' title='"+a.lists.ordered+"' tabindex='-1'><i class='icon-th-list'></i></a><a class='btn"+i+"' data-wysihtml5-command='Outdent' title='"+a.lists.outdent+"' tabindex='-1'><i class='icon-indent-right'></i></a><a class='btn"+i+"' data-wysihtml5-command='Indent' title='"+a.lists.indent+"' tabindex='-1'><i class='icon-indent-left'></i></a></div></li>"},link:function(a,t){var i=t&&t.size?" btn-"+t.size:"";return"<li><div class='bootstrap-wysihtml5-insert-link-modal modal hide fade'><div class='modal-header'><a class='close' data-dismiss='modal'>&times;</a><h3>"+a.link.insert+"</h3></div><div class='modal-body'><input value='http://' class='bootstrap-wysihtml5-insert-link-url input-xlarge'><label class='checkbox'> <input type='checkbox' class='bootstrap-wysihtml5-insert-link-target' checked>"+a.link.target+"</label></div><div class='modal-footer'><a href='#' class='btn' data-dismiss='modal'>"+a.link.cancel+"</a><a href='#' class='btn btn-primary' data-dismiss='modal'>"+a.link.insert+"</a></div></div><a class='btn"+i+"' data-wysihtml5-command='createLink' title='"+a.link.insert+"' tabindex='-1'><i class='icon-share'></i></a></li>"},image:function(a,t){var i=t&&t.size?" btn-"+t.size:"";return"<li><div class='bootstrap-wysihtml5-insert-image-modal modal hide fade'><div class='modal-header'><a class='close' data-dismiss='modal'>&times;</a><h3>"+a.image.insert+"</h3></div><div class='modal-body'><input value='http://' class='bootstrap-wysihtml5-insert-image-url input-xlarge'></div><div class='modal-footer'><a href='#' class='btn' data-dismiss='modal'>"+a.image.cancel+"</a><a href='#' class='btn btn-primary' data-dismiss='modal'>"+a.image.insert+"</a></div></div><a class='btn"+i+"' data-wysihtml5-command='insertImage' title='"+a.image.insert+"' tabindex='-1'><i class='icon-picture'></i></a></li>"},html:function(a,t){var i=t&&t.size?" btn-"+t.size:"";return"<li><div class='btn-group'><a class='btn"+i+"' data-wysihtml5-action='change_view' title='"+a.html.edit+"' tabindex='-1'><i class='icon-pencil'></i></a></div></li>"},color:function(a,t){var i=t&&t.size?" btn-"+t.size:"";return"<li class='dropdown'><a class='btn dropdown-toggle"+i+"' data-toggle='dropdown' href='#' tabindex='-1'><span class='current-color'>"+a.colours.black+"</span>&nbsp;<b class='caret'></b></a><ul class='dropdown-menu'><li><div class='wysihtml5-colors' data-wysihtml5-command-value='black'></div><a class='wysihtml5-colors-title' data-wysihtml5-command='foreColor' data-wysihtml5-command-value='black'>"+a.colours.black+"</a></li><li><div class='wysihtml5-colors' data-wysihtml5-command-value='silver'></div><a class='wysihtml5-colors-title' data-wysihtml5-command='foreColor' data-wysihtml5-command-value='silver'>"+a.colours.silver+"</a></li><li><div class='wysihtml5-colors' data-wysihtml5-command-value='gray'></div><a class='wysihtml5-colors-title' data-wysihtml5-command='foreColor' data-wysihtml5-command-value='gray'>"+a.colours.gray+"</a></li><li><div class='wysihtml5-colors' data-wysihtml5-command-value='maroon'></div><a class='wysihtml5-colors-title' data-wysihtml5-command='foreColor' data-wysihtml5-command-value='maroon'>"+a.colours.maroon+"</a></li><li><div class='wysihtml5-colors' data-wysihtml5-command-value='red'></div><a class='wysihtml5-colors-title' data-wysihtml5-command='foreColor' data-wysihtml5-command-value='red'>"+a.colours.red+"</a></li><li><div class='wysihtml5-colors' data-wysihtml5-command-value='purple'></div><a class='wysihtml5-colors-title' data-wysihtml5-command='foreColor' data-wysihtml5-command-value='purple'>"+a.colours.purple+"</a></li><li><div class='wysihtml5-colors' data-wysihtml5-command-value='green'></div><a class='wysihtml5-colors-title' data-wysihtml5-command='foreColor' data-wysihtml5-command-value='green'>"+a.colours.green+"</a></li><li><div class='wysihtml5-colors' data-wysihtml5-command-value='olive'></div><a class='wysihtml5-colors-title' data-wysihtml5-command='foreColor' data-wysihtml5-command-value='olive'>"+a.colours.olive+"</a></li><li><div class='wysihtml5-colors' data-wysihtml5-command-value='navy'></div><a class='wysihtml5-colors-title' data-wysihtml5-command='foreColor' data-wysihtml5-command-value='navy'>"+a.colours.navy+"</a></li><li><div class='wysihtml5-colors' data-wysihtml5-command-value='blue'></div><a class='wysihtml5-colors-title' data-wysihtml5-command='foreColor' data-wysihtml5-command-value='blue'>"+a.colours.blue+"</a></li><li><div class='wysihtml5-colors' data-wysihtml5-command-value='orange'></div><a class='wysihtml5-colors-title' data-wysihtml5-command='foreColor' data-wysihtml5-command-value='orange'>"+a.colours.orange+"</a></li></ul></li>"}},l=function(a,t,l){return i[a](t,l)},s=function(t,l){this.el=t;var s=l||e;for(var o in s.customTemplates)i[o]=s.customTemplates[o];this.toolbar=this.createToolbar(t,s),this.editor=this.createEditor(l),window.editor=this.editor,a("iframe.wysihtml5-sandbox").each(function(t,i){a(i.contentWindow).off("focus.wysihtml5").on({"focus.wysihtml5":function(){a("li.dropdown").removeClass("open")}})})};s.prototype={constructor:s,createEditor:function(i){i=i||{},i=a.extend(!0,{},i),i.toolbar=this.toolbar[0];var l=new t.Editor(this.el[0],i);if(i&&i.events)for(var s in i.events)l.on(s,i.events[s]);return l},createToolbar:function(t,i){var s=this,o=a("<ul/>",{"class":"wysihtml5-toolbar",style:"display:none"}),r=i.locale||e.locale||"en";for(var d in e){var c=!1;void 0!==i[d]?i[d]===!0&&(c=!0):c=e[d],c===!0&&(o.append(l(d,n[r],i)),"html"===d&&this.initHtml(o),"link"===d&&this.initInsertLink(o),"image"===d&&this.initInsertImage(o))}if(i.toolbar)for(d in i.toolbar)o.append(i.toolbar[d]);return o.find("a[data-wysihtml5-command='formatBlock']").click(function(t){var i=t.target||t.srcElement,l=a(i);s.toolbar.find(".current-font").text(l.html())}),o.find("a[data-wysihtml5-command='foreColor']").click(function(t){var i=t.target||t.srcElement,l=a(i);s.toolbar.find(".current-color").text(l.html())}),this.el.before(o),o},initHtml:function(a){var t="a[data-wysihtml5-action='change_view']";a.find(t).click(function(){a.find("a.btn").not(t).toggleClass("disabled")})},initInsertImage:function(t){var i,l=this,s=t.find(".bootstrap-wysihtml5-insert-image-modal"),o=s.find(".bootstrap-wysihtml5-insert-image-url"),e=s.find("a.btn-primary"),n=o.val(),r=function(){var a=o.val();o.val(n),l.editor.currentView.element.focus(),i&&(l.editor.composer.selection.setBookmark(i),i=null),l.editor.composer.commands.exec("insertImage",a)};o.keypress(function(a){13==a.which&&(r(),s.modal("hide"))}),e.click(r),s.on("shown",function(){o.focus()}),s.on("hide",function(){l.editor.currentView.element.focus()}),t.find("a[data-wysihtml5-command=insertImage]").click(function(){var t=a(this).hasClass("wysihtml5-command-active");return t?!0:(l.editor.currentView.element.focus(!1),i=l.editor.composer.selection.getBookmark(),s.appendTo("body").modal("show"),s.on("click.dismiss.modal",'[data-dismiss="modal"]',function(a){a.stopPropagation()}),!1)})},initInsertLink:function(t){var i,l=this,s=t.find(".bootstrap-wysihtml5-insert-link-modal"),o=s.find(".bootstrap-wysihtml5-insert-link-url"),e=s.find(".bootstrap-wysihtml5-insert-link-target"),n=s.find("a.btn-primary"),r=o.val(),d=function(){var a=o.val();o.val(r),l.editor.currentView.element.focus(),i&&(l.editor.composer.selection.setBookmark(i),i=null);var t=e.prop("checked");l.editor.composer.commands.exec("createLink",{href:a,target:t?"_blank":"_self",rel:t?"nofollow":""})};o.keypress(function(a){13==a.which&&(d(),s.modal("hide"))}),n.click(d),s.on("shown",function(){o.focus()}),s.on("hide",function(){l.editor.currentView.element.focus()}),t.find("a[data-wysihtml5-command=createLink]").click(function(){var t=a(this).hasClass("wysihtml5-command-active");return t?!0:(l.editor.currentView.element.focus(!1),i=l.editor.composer.selection.getBookmark(),s.appendTo("body").modal("show"),s.on("click.dismiss.modal",'[data-dismiss="modal"]',function(a){a.stopPropagation()}),!1)})}};var o={resetDefaults:function(){a.fn.wysihtml5.defaultOptions=a.extend(!0,{},a.fn.wysihtml5.defaultOptionsCache)},bypassDefaults:function(t){return this.each(function(){var i=a(this);i.data("wysihtml5",new s(i,t))})},shallowExtend:function(t){var i=a.extend({},a.fn.wysihtml5.defaultOptions,t||{},a(this).data()),l=this;return o.bypassDefaults.apply(l,[i])},deepExtend:function(t){var i=a.extend(!0,{},a.fn.wysihtml5.defaultOptions,t||{}),l=this;return o.bypassDefaults.apply(l,[i])},init:function(a){var t=this;return o.shallowExtend.apply(t,[a])}};a.fn.wysihtml5=function(t){return o[t]?o[t].apply(this,Array.prototype.slice.call(arguments,1)):"object"!=typeof t&&t?void a.error("Method "+t+" does not exist on jQuery.wysihtml5"):o.init.apply(this,arguments)},a.fn.wysihtml5.Constructor=s;var e=a.fn.wysihtml5.defaultOptions={"font-styles":!0,color:!1,emphasis:!0,lists:!0,html:!1,link:!0,image:!0,events:{},parserRules:{classes:{"wysiwyg-color-silver":1,"wysiwyg-color-gray":1,"wysiwyg-color-white":1,"wysiwyg-color-maroon":1,"wysiwyg-color-red":1,"wysiwyg-color-purple":1,"wysiwyg-color-fuchsia":1,"wysiwyg-color-green":1,"wysiwyg-color-lime":1,"wysiwyg-color-olive":1,"wysiwyg-color-yellow":1,"wysiwyg-color-navy":1,"wysiwyg-color-blue":1,"wysiwyg-color-teal":1,"wysiwyg-color-aqua":1,"wysiwyg-color-orange":1},tags:{b:{},i:{},br:{},ol:{},ul:{},li:{},h1:{},h2:{},h3:{},h4:{},h5:{},h6:{},blockquote:{},u:1,img:{check_attributes:{width:"numbers",alt:"alt",src:"url",height:"numbers"}},a:{check_attributes:{href:"url",target:"alt",rel:"alt"}},span:1,div:1,code:1,pre:1}},stylesheets:["./lib/css/wysiwyg-color.css"],locale:"en"};"undefined"==typeof a.fn.wysihtml5.defaultOptionsCache&&(a.fn.wysihtml5.defaultOptionsCache=a.extend(!0,{},a.fn.wysihtml5.defaultOptions));var n=a.fn.wysihtml5.locale={en:{font_styles:{normal:"Normal text",h1:"Heading 1",h2:"Heading 2",h3:"Heading 3",h4:"Heading 4",h5:"Heading 5",h6:"Heading 6"},emphasis:{bold:"Bold",italic:"Italic",underline:"Underline"},lists:{unordered:"Unordered list",ordered:"Ordered list",outdent:"Outdent",indent:"Indent"},link:{insert:"Insert link",cancel:"Cancel",target:"Open link in new window"},image:{insert:"Insert image",cancel:"Cancel"},html:{edit:"Edit HTML"},colours:{black:"Black",silver:"Silver",gray:"Grey",maroon:"Maroon",red:"Red",purple:"Purple",green:"Green",olive:"Olive",navy:"Navy",blue:"Blue",orange:"Orange"}}}}(window.jQuery,window.wysihtml5);