$(function () {

    //下载路径
    var downloadPath = $('input[name="downloadPath"]').val();
    //上传路径
    var uploadPath = $('input[name="uploadPath"]').val();

    var uploader = WebUploader.create({
        // swf文件路径
        swf: '~/Content/plugins/webuploader/Uploader.swf',
        // 文件接收服务端。
        server: uploadPath,
        // 选择文件的按钮。可选。
        // 内部根据当前运行是创建，可能是input元素，也可能是flash.
        pick: {
            id: '#picker',
            multiple: false
        },
        //限制文件数量
        fileNumLimit: 1,
        //自动上传文件
        auto: true,
        // 不压缩image, 默认如果是jpeg，文件上传前会压缩一把再上传！
        resize: false
    });

    //刪除已有文件
    $('.remove-item').on('click', function (e) {

        if (confirm('确认删除吗？')) {

            $(this).parent().remove();
        }

        e.stopPropagation();
    });

    //当文件被加入队列之前触发，此事件的handler返回值为false，则此文件不会被添加进入队列。
    uploader.on('beforeFileQueued', function (file) {

        if ($('.remove-item').length > 0) {
            alert('只能上传一个文件');
            return false;
        }
    });

    // 当有文件被添加进队列的时候
    uploader.on('fileQueued', function (file) {

        if ($('.remove-item').length > 0) {
            alert('只能上传一个文件');
            uploader.removeFile(file);
            return false;
        }

        var $list = $('#thelist');
        $list.append('<div id="' + file.id + '" class="item">' +
                         '<a class="file-name" data-path="" data-name="' + file.name + '" target="_self" style="display:inline-block;margin-right:10px;" >' + file.name + '</a>' +
                         '<a class="remove-item" href="javascript:void(0);" style="display:inline-block;text-decoration: none;">刪除</a>' +
                         //'<h4 class="info">' + file.name + '</h4>' +
                         //'<p class="state">等待上传...</p>' +
                     '</div>');

        //刪除
        $list.on('click', '.remove-item', function (e) {

            //注意这里事件委托
            if ($(this).parent().attr('id') != file.id) return false;

            if (confirm('确认删除吗？')) {

                uploader.removeFile(file);
                $(this).parent().remove();
            }

            e.stopPropagation();
            return false;
        });

    });

    // 文件上传过程中创建进度条实时显示。
    uploader.on('uploadProgress', function (file, percentage) {
        var $li = $('#' + file.id),
            $percent = $li.find('.progress .progress-bar');

        // 避免重复创建
        if (!$percent.length) {
            $percent = $('<div class="progress progress-striped active">' +
                            '<div class="progress-bar" role="progressbar" style="width: 0%">' +
                            '</div>' +
                         '</div>').appendTo($li).find('.progress-bar');
        }

        //$li.find('p.state').text('上传中');
        $percent.css('width', percentage * 100 + '%');
    });

    //文件上传失败会派送uploadError事件，成功则派送uploadSuccess事件。
    //不管成功或者失败，在文件上传完后都会触发uploadComplete事件。
    //上传成功
    uploader.on('uploadSuccess', function (file, response) {

        var href = downloadPath + '?filePath=' + response.Data.ViewUrl + '&fileName=' + file.name;
        var path = response.Data.ViewUrl;
        $('#' + file.id + ' a.file-name').attr('href', href);
        $('#' + file.id + ' a.file-name').data('path', path);

        //$('#' + file.id).find('p.state').text('已上传');
        //uploader.removeFile(file);
    });

    //上传失败
    uploader.on('uploadError', function (file, reason) {
        $('#' + file.id).find('p.state').text('上传出错');
    });

    //上传完成
    uploader.on('uploadComplete', function (file) {

        $('#' + file.id).find('.progress').fadeOut();
    });

    //开始上传
    $('#ctlBtn').on('click', function () {

        uploader.upload();
    });

    //其他业务代码
    //通知日期
    laydate.render({
        elem: '.updateTime',
        type: 'date'
    });

    var chooseUserWay = $('input[name="chooseUserWay"]').val();
    chooseUserWay = chooseUserWay ? chooseUserWay : $('input[name="chooseWay"]:checked').val();
    $('input[name="chooseWay"]').each(function (i, val) {

        if ($(val).val() == chooseUserWay) {

            $(val).prop('checked', true);
            return;
        }
    });

    if (chooseUserWay == 1) {

        $('.user-prechoose').hide();
        $('.user-prerole').show();
    } else if (chooseUserWay == 5) {

        $('.user-prerole').hide();
        $('.user-prechoose').show();
    }

    //指定方式'角色'
    $('#roleUser').on('change', function (e) {

        $('.user-prechoose').hide();
        $('.user-prerole').show();
    });

    //指定方式'指定个人'
    $('#chooseUser').on('change', function (e) {

        $('.user-prerole').hide();
        $('.user-prechoose').show();

        getUserList();
    });

    //选择'人员/角色'
    $('.way-choose').on('click', function (e) {

        var tempHtml = '<div class="user-show">' +
                            '<div class="show-title"><span class="title-text">请选择角色</span></div>' +
                            '<div class="show-content"></div>' +
                       '</div>';

        var options = {
            title: false,
            type: 1,
            skin: 'user-data',
            area: ['1000px', '500px'],
            closeBtn: false,
            shade: 0.3,
            move: false,
            content: tempHtml,
            btn: ['保 存', '取 消'],
            yes: function (index, layero) {

                var showHtml = '';
                $('.show-content input:checked').each(function (i, val) {

                    showHtml += '<span class="con-item" data-userid="' + $(val).val() + '" >' + $(val).siblings('label').text() + '</span>';
                });

                var choose = $('input[name="chooseWay"]:checked').val();
                if (choose == 1) {

                    $('.list-con .user-prerole').html(showHtml);
                }
                else if (choose == 5) {
                    $('.list-con .user-prechoose').html(showHtml);
                }

                layer.close(index);
            },
            btn2: function (index, layero) {
                layer.close(index);
            }
        };

        var chooseWay = $('input[name="chooseWay"]:checked').val();
        if (chooseWay == 1) {

            options.area = ['500px', '200px'];
            options.success = function (layero, index) {
                //按钮居中
                layero.find('.layui-layer-btn').css('text-align', 'center');
                var content = '<div class="content-item">' +
                                '<span class="item-wrap"><input type="checkbox" name="chiefEditor" value="1" id="chiefEditor" /><label for="chiefEditor">主编</label></span>' +
                                '<span class="item-wrap"><input type="checkbox" name="edited" value="2" id="edited" /><label for="edited">责编</label></span>' +
                                '<span class="item-wrap"><input type="checkbox" name="editor" value="3" id="editor" /><label for="editor">编辑</label></span>' +
                              '</div>';

                $('.show-content').html(content);
                $('.title-text').text('请选择角色');
                $('.show-content').addClass('show-small');
            };

            layer.open(options);
        } else {

            $.ajax({
                url: $('input[name="getUserListUrl"]').val(),
                dataType: 'json',
                type: 'get',
                success: function (data) {

                    if (data.Code < 0) {
                        alert(data.Msg);
                        return;
                    }

                    var result = data.Data;
                    var dataHtml = '', subItem = '';
                    for (var i = 1; i <= result.length; i++) {

                        subItem += '<span class="item-wrap">' +
                                        '<input type="checkbox"  value="' + result[i - 1].ID + '" name="chooseUser" id="user-id' + result[i - 1].ID + '" />' +
                                        '<label for="user-id' + result[i - 1].ID + '">' + result[i - 1].Name + '</label>' +
                                   '</span>';

                        if (i % 12 == 0) {
                            dataHtml += '<div class="content-item">' + subItem + '</div>';
                            subItem = '';
                        }

                        if (i == result.length && i % 12 != 0) {
                            dataHtml += '<div class="content-item">' + subItem + '</div>';
                        }
                    }

                    options.success = function (layero, index) {
                        //按钮居中
                        layero.find('.layui-layer-btn').css('text-align', 'center');

                        $('.show-content').html(dataHtml);
                        $('.title-text').text('请选择人员');
                        $('.show-content').removeClass('show-small');
                    };

                    layer.open(options);
                }
            });
        }
    });

    //关闭dialog
    $('.cancelUser').on('click', function () {

        var usershowIndex = $(this).data('usershowindex')
        layer.close(usershowIndex);
        $(document.body).attr('data-opening', '0');
    });

    // '保存'关闭弹窗
    $('.saveUser').on('click', function () {

        var showHtml = '';
        $('.show-content input:checked').each(function (i, val) {

            showHtml += '<span class="con-item" data-userid="' + $(val).val() + '" >' + $(val).siblings('label').text() + '</span>';
        });

        var choose = $('input[name="chooseWay"]:checked').val();
        if (choose == 1) {

            $('.list-con .user-prerole').html(showHtml);
        }
        else if (choose == 5) {
            $('.list-con .user-prechoose').html(showHtml);
        }

        //关闭dialog
        $('.cancelUser').trigger('click');
    });

    //获取用户列表
    function getUserList() {

        $.ajax({
            url: $('input[name="getUserListUrl"]').val(),
            dataType: 'json',
            type: 'get',
            success: function (data) {

                if (data.Code < 0) {
                    alert(data.Msg);
                    return;
                }

                var result = data.Data;
                var dataHtml = '', subItem = '';
                for (var i = 1; i <= result.length; i++) {

                    subItem += '<span class="item-wrap">' +
                                    '<input type="checkbox"  value="' + result[i - 1].ID + '" name="chooseUser" id="user-id' + result[i - 1].ID + '" />' +
                                    '<label for="user-id' + result[i - 1].ID + '">' + result[i - 1].Name + '</label>' +
                               '</span>';

                    if (i % 13 == 0) {
                        dataHtml += '<div class="content-item">' + subItem + '</div>';
                        subItem = '';
                    }

                    if (i == result.length && i % 13 != 0) {
                        dataHtml += '<div class="content-item">' + subItem + '</div>';
                    }
                }

                $('.show-content').html(dataHtml);
            }
        });
    }

    //返回
    $('.goBack').on('click', function (e) {
        window.history.back();
    });

    //防止二次提交
    var isPostBack = true;
    //保存
    $('#noticeSave').on('click', function () {

        if (!isPostBack) return;
        isPostBack = false;

        var noticeId = $('input[name="noticeId"]').val(),
            title = $('input[name="noticeTitle"]').val(),
         content = $('textarea[name="noticeContent"]').val();

        if ($.trim(title) == '') {

            alert('通知标题不能为空！');
            isPostBack = true;
            return false;
        }

        if (title.replace(/\s/g, '').length > 100) {
            alert('通知标题不能超过100个字符！');
            isPostBack = true;
            return false;
        }

        if ($.trim(content) == '') {

            alert('通知内容不能为空！');
            isPostBack = true;
            return false;
        }

        var chooseWay = $('input[name="chooseWay"]:checked').val(),
            selectedItem = $('.list-con span.con-item'),
            selectedFile = $('#thelist .file-name'),
            chooseUserOrRoleIds = '',
            chooseFileUrls = '',
            updateTime = $('input[name="updateTime"]').val();

        selectedItem.each(function (i, val) {

            if (i == selectedItem.length - 1)
                chooseUserOrRoleIds += $(val).data('userid');
            else
                chooseUserOrRoleIds += $(val).data('userid') + ',';
        });

        selectedFile.each(function (i, val) {

            if (i == selectedFile.length - 1)
                chooseFileUrls += $(val).data('path') + '>' + $(val).data('name');
            else
                chooseFileUrls += $(val).data('path') + '>' + $(val).data('name') + '|';
        });

        var saveUrl = $('input[name="saveNoticeUrl"]').val();
        var noticeParams = {
            ID: noticeId,
            Title: title.replace(/\s/g, ''),
            Content: content,
            ChooseUserWay: chooseWay,
            UserOrRoleIds: chooseUserOrRoleIds,
            UpdateTime: updateTime,
            AttachmentUrl: encodeURIComponent(chooseFileUrls)
        };

        $.post(saveUrl, noticeParams, function (data) {

            isPostBack = true;
            if (data.Code == 200) {

                //刷新界面
                location.href = '/';

            } else {
                alert(data.Msg);
            }
        });
    });
});