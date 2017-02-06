var timeGoInInterval;
var timeExamInterval;

//考试
var Exam = {};

Exam.SingleNoBackQID = ''; //单题不可逆已做题目的ID，如：1,2,3……

///////字段
Exam.autoSaveAnswerTimer = 0;
//5分钟自动保存计时
Exam.timeGoInToTicker = 0;
//进入考试倒计时
Exam.timeExamTicker = 0;
//考试倒计时
Exam.currentQuestionIndex = 1;
//当前显示的第几题

//////属性
Exam.Examination = null;
//考试基本信息
Exam.ExampaperShow = null;
//试卷信息
Exam.UserAnswer = null;
//学员答案
Exam.AnswerResult = new Array();
//学员回答的结果

//获取题型
Exam.GetQuestionType = function (type) {
    switch (type) {
        case '问答题':
            return 1;
        case '单选题':
            return 2;
        case '多选题':
            return 3;
        case '判断题':
            return 4;
        case '填空题':
            return 5;
        case '情景题':
            return 6;
        default:
            return 0;
    }
};

//////方法
//单个显示
Exam.GetShowSingleQuestion = function (qId, order) {
    if (Exam.Examination.ExamShowWay == 0) {
        $(".singleQuestion").hide();
        $(".singleQuestion[id='q" + qId + "']").show();
    } else {
        Exam.GetShowQuestionList(3, 0, order);
    }
};

//获取要显示的试题信息（type：0:整卷显示；1：可逆；2：不可逆；3：整体和可逆  index: 上一题：-1;下一题：1； order：题序）
Exam.GetShowQuestionList = function (type, index, order) {
    if (type == 3)
        Exam.currentQuestionIndex = order;
    else
        Exam.currentQuestionIndex += index;
    if (type != 0 && (Exam.currentQuestionIndex < Exam.ExampaperShow.QuestionList.length || Exam.currentQuestionIndex > 1))
        $("#btnPreQuestion,#btnNextQuestion").removeAttr("disabled");
    if (type != 0 && Exam.currentQuestionIndex == 1)
        $("#btnPreQuestion").attr("disabled", "disabled");
    if (type != 0 && Exam.currentQuestionIndex == Exam.ExampaperShow.QuestionList.length)
        $("#btnNextQuestion").attr("disabled", "disabled");

    $("#questionList").html("");
    for (var i = 0; i < Exam.ExampaperShow.QuestionList.length; i++) {
        var qu = Exam.ExampaperShow.QuestionList[i];
        if (type == 0 || (type != 0 && qu.QuestionOrder == Exam.currentQuestionIndex)) {
            $("#questionList").append($("#questionShowTemplate").render(qu));

            //如果是填空题生成输入框
            if (qu.QType == 5) {
                var count = qu.FillBlankCount;
                var arr = new Array();
                for (var j = 1; j <= count; j++) {
                    arr.push(j);
                }
                $("#q" + qu.QuestionID + " .questionAnswer").append($("#fillBlankTemplate").render(arr));
                $("#q" + qu.QuestionID + " .questionAnswer input").attr("questionID", qu.QuestionID).bind("change", function () {
                    Exam.UpdateUserAnswer($(this).attr("questionID"), 5);
                });
            }
            //是否标志
            if ($("#order" + qu.QuestionID).attr("signflag") == '1')
                $("#q" + qu.QuestionID + " a[sign='sign']").eq(0).removeClass('signf').addClass('signt');
            //初始化学员答案
            for (var j = 0; j < Exam.AnswerResult.length; j++) {
                var temparr = Exam.AnswerResult[j].split('!!***!!');
                if (parseInt(temparr[0]) == qu.QuestionID) {
                    var an = temparr[1];
                    if (an.length > 0) {
                        var siq = $("#q" + qu.QuestionID);
                        switch (siq.attr("type")) {
                            case "1":
                                //问答
                                {
                                    siq.find("textarea").eq(0).val(an);
                                }
                                break;
                            case "2":
                                //单选
                            case "3":
                                //多选
                            case "4":
                                //判断
                                {
                                    siq.find("input").each(function () {
                                        if ((',' + an + ',').indexOf(',' + $(this).attr("order") + ',') >= 0) {
                                            $(this).attr("checked", true);
                                        }
                                    });
                                }
                                break;
                            case "5":
                                //填空
                                {
                                    var tfa = an.split('##**##');
                                    var c = 0;
                                    siq.find("input").each(function () {
                                        $(this).val(tfa.length > c ? tfa[c] : '');
                                        c++;
                                    });
                                }
                                break;
                            case "6":
                                //情景题
                                {
                                    if (siq.find("textarea").length > 0) {
                                        siq.find("textarea").eq(0).val(an);
                                    } else {
                                        siq.find("input").each(function () {
                                            if ((',' + an + ',').indexOf(',' + $(this).attr("order") + ',') >= 0) {
                                                $(this).attr("checked", true);
                                            }
                                        });
                                    }
                                }
                                break;
                        }
                    }
                }
            }
        }
    }
};

//右侧试题状态按钮筛选
///type(0：全部；1：未完成；2：已完成；3：已标记)
Exam.QuestionStatusSelect = function (obj, type) {
    $(obj).parent().find("li").removeClass('On');
    $(obj).addClass('On');
    switch (type) {
        case 0:
            $("#questionCountDetail .singlequ").show();
            break;
        case 1:
            $("#questionCountDetail .singlequ").hide();
            $("#questionCountDetail .singlequ[doflag='0']").show();
            break;
        case 2:
            $("#questionCountDetail .singlequ").hide();
            $("#questionCountDetail .singlequ[doflag='1']").show();
            break;
        case 3:
            $("#questionCountDetail .singlequ").hide();
            $("#questionCountDetail .singlequ[signflag='1']").show();
            break;
    }
};

//整体显示题型按钮筛选
///type(1：问答；2：单选；3多选；4：判断；5：填空；6：多媒体)
Exam.QuestionTypeSelect = function (obj,type) {
    $(obj).parent().find("li").removeClass('On');
    $(obj).addClass('On');
    var t = Exam.GetQuestionType(type);
    if (t == 0)
        $(".singleQuestion").show();
    else {
        $(".singleQuestion").hide();
        $(".singleQuestion[type='" + t + "']").show();
    }
};

//更改右侧标志状态
//signflag(0:未标记；1：标记)
Exam.ChangeSign = function (qid, obj) {
    var issign = $(obj).attr("signflag") == '0' ? '1' : '0';
    if (issign == '1')
        $(obj).attr("signflag", issign).removeClass('signf').addClass('signt');
    else
        $(obj).attr("signflag", issign).removeClass('signt').addClass('signf');
    var qu = $("#questionCountDetail #order" + qid);
    if (issign == '1')
        qu.removeClass("done").removeClass("nodone").removeClass("nosign").addClass("sign").attr("signflag", "1");
    else {
        if (qu.attr("doflag") == '1')
            qu.removeClass("sign").removeClass("nodone").removeClass("nosign").addClass("done").attr("signflag", "0");
        else
            qu.removeClass("done").removeClass("sign").removeClass("nosign").addClass("nodone").attr("signflag", "0");
    }
    Exam.ChangeTotal("");
};

//更改右侧统计(answer:学员答案)
Exam.ChangeTotal = function (answer) {
    if ($("#questionCountDetail").length > 0) {
        var done = $("#questionCountDetail div[doflag='1']").length;
        var nodone = $("#questionCountDetail div[doflag='0']").length;
        var sign = $("#questionCountDetail div[signflag='1']").length;
        $("#quOverTotal").html(done);
        $("#quNoTotal").html(nodone);
        $("#quSignTotal").html(sign);
    } else {
        if (answer != "" && answer.length > 0) {
            if (("," + Exam.SingleNoBackQID + ",").indexOf("," + answer + ",") < 0) {
                Exam.SingleNoBackQID += Exam.SingleNoBackQID == "" ? answer : ("," + answer);
            }
            $("#quOverTotal").text(Exam.SingleNoBackQID.split(',').length);
        }
    }
};

//获取学员答案
Exam.GetUserAnswer = function () {
    var count = 0;
    var str = '';
    for (var i = 0; i < Exam.AnswerResult.length; i++) {
        if (Exam.AnswerResult[i].split('!!***!!')[1].length == 0) {
            count++;
        }
        str += str == "" ? Exam.AnswerResult[i] : ('**!!!**' + Exam.AnswerResult[i]);
    }
    $("#userAnswer").val(str);
    return count;
};

//汇总学员答案（qID:试题ID，type：试题类型）
Exam.UpdateUserAnswer = function (qId, type) {
    var an = '';
    switch (type) {
        case 1:
            //问答
            an = $("#q" + qId + " .questionAnswer textarea").eq(0).val();
            break;
        case 2:
            //单选
        case 3:
            //多选
        case 4:
            //判断
            {
                $("#q" + qId + " .questionAnswer input").each(function () {
                    if ($(this).attr("checked")) {
                        an += an == "" ? ($(this).attr("order")) : (',' + $(this).attr("order"));
                    }
                });
            }
            break;
        case 5:
            //填空
            {
                $("#q" + qId + " .questionAnswer input").each(function () {
                    an += an == "" ? ($(this).val()) : ('##**##' + $(this).val());
                });
            }
            break;
        case 6:
            //情景
            {
                if ($("#q" + qId + " .questionAnswer textarea").length > 0) {
                    an = $("#q" + qId + " .questionAnswer textarea").eq(0).val();
                } else {
                    $("#q" + qId + " .questionAnswer input").each(function () {
                        if ($(this).attr("checked")) {
                            an += an == "" ? ($(this).attr("order")) : (',' + $(this).attr("order"));
                        }
                    });
                }
            }
            break;
    }
    for (var i = 0; i < Exam.AnswerResult.length; i++) {
        if (('**!!!**' + Exam.AnswerResult[i]).indexOf('**!!!**' + qId + '!!***!!') == 0) {
            Exam.AnswerResult[i] = qId + '!!***!!' + an;
        }
    }
    var qu = $("#questionCountDetail #order" + qId).eq(0);
    if (an == '') {
        if (qu.attr("signflag") == '1')
            qu.removeClass("done").removeClass("nodone").removeClass("nosign").addClass("sign").attr("doflag", "0");
        else
            qu.removeClass("sign").removeClass("done").removeClass("nosign").addClass("nodone").attr("doflag", "0");
    } else {
        if (qu.attr("signflag") == '1')
            qu.removeClass("done").removeClass("nodone").removeClass("nosign").addClass("sign").attr("doflag", "1");
        else
            qu.removeClass("nodone").removeClass("sign").removeClass("nosign").addClass("done").attr("doflag", "1");
    }
    Exam.ChangeTotal(an == "" ? "" : ("" + qId + ""));
};

//更新进入倒计时
Exam.GoInToTicker = function () {
    timeGoInInterval = setInterval("changeGoIoTime()", 1000);
};

//考试交卷
Exam.SubmitExam = function (str) {
    $("#btnSubmitExam").attr("disabled", true);
    var count = Exam.GetUserAnswer();
    if (str != undefined && str != "") {
        //时间到提交
        $.post("/ExamTest/SubmitStudentAnswer?euid=" + $("#examUserID").val() + "&submitType=2&courseType=" + $("#txtsourceType").val() + "&pecent=" + Exam.Examination.PercentScore + "&passScore=" + Exam.Examination.PassScore, $("#submitAnswerForm").formSerialize(), function (data) {
            if (data.result == 1) {
                clearInterval(timeExamInterval);
                window.location.href = '/ExamTest/MyExaminationList';
            } else {
                clearInterval(timeExamInterval);
                if (data.pass == 1) {
                    art.dialog.tips("恭喜，您已通过本次测试，可查看获取学时情况", 3, function () { window.location.href = getQueryString("backurl") });
                    //window.location.href = getQueryString("backurl");
                }
                else {
                    art.dialog.tips("抱歉，您未能通过本次测试，本课程还剩余" + data.nopassnumber + "次考试机会", 3, function () { window.location.href = getQueryString("backurl"); });
                }
            }
            $("#btnSubmitExam").attr("disabled", false);
        });
    } else {
        //提前提交
        if (count > 0) {
            art.dialog.confirm("您还有" + count + '题没有做，确定提交吗？', function () {
                $.post("/ExamTest/SubmitStudentAnswer?euid=" + $("#examUserID").val() + "&courseType=" + $("#txtsourceType").val() + "&submitType=2&pecent=" + Exam.Examination.PercentScore + "&passScore=" + Exam.Examination.PassScore, $("#submitAnswerForm").formSerialize(), function (data) {
                    if (data.result == 1) {
                        clearInterval(timeExamInterval);
                        window.location.href = '/ExamTest/MyExaminationList';
                    } else {
                        clearInterval(timeExamInterval);
                        //window.location.href = getQueryString("backurl");
                        if (data.pass == 1) {
                            art.dialog.tips("恭喜，您已通过本次测试，可查看获取学时情况", 3, function () { window.location.href = getQueryString("backurl"); });
                            //window.location.href = getQueryString("backurl");
                        }
                        else {
                            art.dialog.tips("抱歉，您未能通过本次测试，本课程还剩余" + data.nopassnumber + "次考试机会", 3, function () { window.location.href = getQueryString("backurl"); });
                        }
                    }
                    $("#btnSubmitExam").attr("disabled", false);
                });
            }, function () {
                $("#btnSubmitExam").attr("disabled", false);
            });
        } else {
            art.dialog.confirm("确定提交吗？", function () {
                $.post("/ExamTest/SubmitStudentAnswer?euid=" + $("#examUserID").val() + "&submitType=2&courseType=" + $("#txtsourceType").val() + "&pecent=" + Exam.Examination.PercentScore + "&passScore=" + Exam.Examination.PassScore, $("#submitAnswerForm").formSerialize(), function (data) {
                    if (data.result == 1) {
                        clearInterval(timeExamInterval);
                        window.location.href = '/ExamTest/MyExaminationList';
                    } else {
                        clearInterval(timeExamInterval);
                        if (data.pass == 1) {
                            art.dialog.tips("恭喜，您已通过本次测试，可查看获取学时情况", 3, function () { window.location.href = getQueryString("backurl"); });
                            //window.location.href = getQueryString("backurl");
                        }
                        else {
                            art.dialog.tips("抱歉，您未能通过本次测试，本课程还剩余" + data.nopassnumber + "次考试机会", 3, function () { window.location.href = getQueryString("backurl"); });
                        }
                    }
                    $("#btnSubmitExam").attr("disabled", false);
                });
            }, function () {
                $("#btnSubmitExam").attr("disabled", false);
            });
        }
    }
};

//开始考试并更新进入倒计时
Exam.StartExamTest = function () {
    $.getJSON('/ExamTest/JudgeCanExamTest?euID=' + $("#examUserID").val() + '&flag=1', function (data) {
        if (data.result == 1) {
            $("#examBaseInfor").remove();
            $("#examTestMain").show();
            //显示头部信息
            $("#examTest").html($("#examTopTemplate").render(Exam.Examination));

            //显示操作按钮
            if (Exam.Examination.ExamShowWay == 0) {
                var arr = Exam.ExampaperShow.QuestionTypeStrShow.split(',');
                arr.push("显示全部");
                $("#operation").html($("#examOperationAllShowTemplate").render(arr));

                $("#questionCountDetail").html($("#questionCountDetailTemplate").render(Exam.ExampaperShow.QuestionTypeStrShow.split(',')));
            } else if (Exam.Examination.ExamShowWay == 1) {
                $("#operation").html($("#examOperationSingleBackTemplate").render(1));
                $("#questionCountDetail").html($("#questionCountDetailTemplate").render(Exam.ExampaperShow.QuestionTypeStrShow.split(',')));
            } else {
                $("#operation").html($("#examOperationSingleNoBackTemplate").render(1));
            }
            //显示右侧的按钮
            var count = 0;
            for (var i = 0; i < Exam.UserAnswer.length; i++) {
                if (Exam.UserAnswer[i].Answer != "") {
                    count++;
                    Exam.SingleNoBackQID += Exam.SingleNoBackQID == "" ? Exam.UserAnswer[i].Qid : ("," + Exam.UserAnswer[i].Qid);
                }
                Exam.AnswerResult.push(Exam.UserAnswer[i].Qid + '!!***!!' + Exam.UserAnswer[i].Answer);
            }
            var countTotal = { QuAllTotal: Exam.ExampaperShow.QuestionList.length, QuOverTotal: count, QuNoTotal: Exam.ExampaperShow.QuestionList.length - count, QuSignTotal: 0 };
            $("#questionCountTotal").html($("#questionCountTotalTemplate").render(countTotal));

            //单题不可逆时，屏蔽右侧的一些东西
            if (Exam.Examination.ExamShowWay == 2) {
                $(".examTest_op center,#questionCountTotal table tbody tr:last").remove();
                $("#quOverTotal").text(Exam.SingleNoBackQID.length == 0 ? 0 : Exam.SingleNoBackQID.split(',').length);
            }

            //加载右侧的试题分部（整体显示和单题可逆时存在）
            if (Exam.Examination.ExamShowWay == 0 || Exam.Examination.ExamShowWay == 1) {
                for (var i = 0; i < Exam.ExampaperShow.QuestionList.length; i++) {
                    switch (Exam.ExampaperShow.QuestionList[i].QType) {
                        case 1:
                            //问答题
                            $("div[type='问答题']").eq(0).append($("#questionSingleShowTemplate").render(Exam.ExampaperShow.QuestionList[i]));
                            break;
                        case 2:
                            //单选题
                            $("div[type='单选题']").eq(0).append($("#questionSingleShowTemplate").render(Exam.ExampaperShow.QuestionList[i]));
                            break;
                        case 3:
                            //多选题
                            $("div[type='多选题']").eq(0).append($("#questionSingleShowTemplate").render(Exam.ExampaperShow.QuestionList[i]));
                            break;
                        case 4:
                            //判断题
                            $("div[type='判断题']").eq(0).append($("#questionSingleShowTemplate").render(Exam.ExampaperShow.QuestionList[i]));
                            break;
                        case 5:
                            //填空题
                            $("div[type='填空题']").eq(0).append($("#questionSingleShowTemplate").render(Exam.ExampaperShow.QuestionList[i]));
                            break;
                        case 6:
                            //情景题
                            $("div[type='情景题']").eq(0).append($("#questionSingleShowTemplate").render(Exam.ExampaperShow.QuestionList[i]));
                            break;
                    }
                }
            }
            //初始化试题
            Exam.GetShowQuestionList(Exam.Examination.ExamShowWay, 0);

            //初始化试题分值
            Exam.InitQuestionScore();

            //开始计时
            Exam.timeExamTicker = Exam.Examination.ExamLength * 60;
            timeExamInterval = setInterval("changeExamTime()", 1000);
        }
        else {
            //art.dialog({ title: '温馨提示', content: data.message, width: 200, height: 50, fixed: true, lock: true, time: 3 });
            art.dialog.tips(data.message, 3);
        }
    });
};

//初始化试题分值
Exam.InitQuestionScore = function () {
    var str = '';
    var order = '';
    for (var i = 0; i < Exam.ExampaperShow.QuestionList.length; i++) {
        var qu = Exam.ExampaperShow.QuestionList[i];
        str += str == "" ? (qu.QuestionID + ',' + qu.Score) : (';' + qu.QuestionID + ',' + qu.Score);
        order += order == "" ? (qu.QuestionID + ',' + qu.QuestionOrder) : (';' + qu.QuestionID + ',' + qu.QuestionOrder);
    }
    $("#questionOrder").val(order);
    $("#questionScore").val(str);
};

//试题附件显示
Exam.BrowQuestionFiles = function (qId)
{
    for (var i = 0; i < Exam.ExampaperShow.QuestionList.length; i++)
    {
        if (Exam.ExampaperShow.QuestionList[i].QuestionID == qId)
        {
            var template = $("#questionFileTemplate").render(Exam.ExampaperShow.QuestionList[i]);
            //openPopWindow({ width: 350, height: 350, content: template });
            art.dialog({
                width: 550,
                height: 350,
                content: template
            });
            player();
        }
    }

};


//更新进入倒计时

function changeGoIoTime() {
    Exam.timeGoInToTicker--;
    if (Exam.timeGoInToTicker > 0) {
        var str = '';
        var hour = parseInt(Exam.timeGoInToTicker / 3600);
        var minite = parseInt(Exam.timeGoInToTicker % 3600 / 60);
        var second = Exam.timeGoInToTicker % 3600 % 60;
        str = hour + '小时' + minite + '分钟' + second + '秒';
        $("#leftTime").html(str);
    } else {
        $("#leftTime").html('0小时0分钟0秒');
        $("#btnGoExam").removeAttr("disabled");
        clearInterval(timeGoInInterval);
    }
};

//更新考试倒计时

function changeExamTime() {
    //自动保存答案
    Exam.autoSaveAnswerTimer++;
    if (Exam.autoSaveAnswerTimer == 300) {
        Exam.autoSaveAnswerTimer = 0;
        Exam.GetUserAnswer();
        $.post("/ExamTest/SubmitStudentAnswer?euid=" + $("#examUserID").val() + "&submitType=1", $("#submitAnswerForm").formSerialize());
    }

    Exam.timeExamTicker--;
    if (Exam.timeExamTicker > 0) {
        if (Exam.timeExamTicker == 300)
            art.dialog({
                title: '温馨提示',
                content: '离考试结束，还有5分钟！',
                width: 200,
                height: 50,
                fixed: true,
                lock: true,
                time: 3000
            });
        var hour = parseInt(Exam.timeExamTicker / 3600);
        var minite = parseInt(Exam.timeExamTicker % 3600 / 60);
        var second = Exam.timeExamTicker % 3600 % 60;
        var str = hour + '小时' + minite + '分钟' + second + '秒';
        $("#examlefttime").html(str);
    } else {
        clearInterval(timeExamInterval);
        Exam.SubmitExam('timeout');
    }
};

//获取url参数
function getQueryString(name) { var reg = new RegExp("(^|&)" + name + "=([^&]*)(&|$)", "i"); var r = window.location.search.substr(1).match(reg); if (r != null) return unescape(r[2]); return null; }