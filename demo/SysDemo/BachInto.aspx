<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="BachInto.aspx.cs" Inherits="demo.SysDemo.BachInto" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>导入</title>
    <link href="../newcss/table.css" rel="stylesheet" type="text/css" />
    <link id="ShowCss" href="../newcss/font_size_small.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript" src="../newjs/jquery-1.4.2.js"></script>
    <script type="text/javascript" src="../newjs/jquery.select.js"></script>
    <script type="text/javascript">
        $(function () {
            var cssurl = $("#ShowCss").attr("href");
            $("#ShowCss").attr("href", cssurl.replace("small", $("#hfcss").val()))
        });

        function one() {
            window.location.href = "BachInto.aspx";
        }
        function two() {
            window.location.href = "BachThree.aspx";
        }
        function three() {
            window.location.href = "PutOutAllQF.aspx";
        }

        function getfileurl() {
            var url = $("#FileUpload1").val(); //上传控件中的文件全路径
            if (url == "") {
                alert("请选择XLS文件");
                $("#ddlway1").html("");
                $("#msg").html("请选择XLS文件!");
                $("#isgoing").html("");
                $("#Button1").show();
                return false;
            }
            else if (url.indexOf(".xls") == -1) {
                alert("请选择正确的XLS文件");
                $("#ddlway1").html("");
                $("#msg").html("请选择正确的XLS文件!");
                $("#isgoing").html("");
                $("#Button1").show();
                return false;
            }
            else {
                //此时已经为XLS文件了
                $("#HiddenField1").val(url); //保存当前选择的Excel文件的全路径
                //                    alert($("#FileUpload1").val());
                //                   alert("AAA"+$("#File1").text());
                //alert("fff");    
                $("#Button6").click();
                $("#HiddenFieldWorkBook").val("--请选择--");
                intimsg();
                $("#isgoing").html("");
                $("#Button1").show();
            }
        }
        //初始化提示框信息
        function intimsg() {
            if ($("#HiddenField1").val() == "") {
                $("#msg").html("请选择批量收费的Excel文件!");
                $("#isgoing").html("");
                $("#Button1").show();
            }
            else if ($("#HiddenFieldWorkBook").val() == "--请选择--" || $("#HiddenFieldWorkBook").val() == "" || $("#ddlway option:selected").text() == "--请选择--") {
                $("#msg").html("请从最近选择的Excel文件中选择一个工作簿!")
                $("#isgoing").html("");
                $("#Button1").show(); ;
                // alert("nnn" + $("#ddlway option:selected").text() + "-----------" + $("#HiddenFieldWorkBook").val());
            }
            else {
                //alert("aaa" + $("#HiddenFieldWorkBook").val());
                $("#msg").html("您当前选中的工作簿的名称为:" + $("#ddlway option:selected").text() + "!");
            }
        }
        $(function () {
            //intimsg();
            if ($("#msg").html() == "") {
                if ($("#HiddenField1").val() == "") {
                    $("#msg").html("请选择批量收费的Excel文件!");
                    $("#isgoing").html("");
                    $("#Button1").show();
                }
                else {
                    // Nlist();
                    //alert($("#HiddenField1").val());
                    $("#msg").html("请从最近选择的Excel文件中选择一个工作簿!")
                    $("#isgoing").html("");
                    $("#Button1").show();
                }
            }
            accway();
            $("#dldway").change(function () {
                accway();
            });
            function accway() {
                var dldway = $("#dldway").val();
                if (dldway == "1") {
                    $("#message").html("导入收费的Excel表格必须和批量未收导出的合并格式一致！");
                } else if (dldway == "3") {
                    $("#message").html("导入收费的Excel表格必须和批量未收导出的明细格式一致！");
                }
                else {
                    $("#message").html("导入收费的Excel表格里只需要(银行账号)、(户号)和(实收金额)三个字段！");
                }
            }
            //工作簿选择事件
            $("#ddlway").change(function () {
                var bookname = $("#ddlway option:selected").text();
                //alert(bookname);
                if (bookname == "--请选择--") {
                    $("#msg").html("请从最近选择的Excel文件中选择一个工作簿!");
                    $("#isgoing").html("");
                    $("#Button1").show();
                    return false;
                }
                else {
                    $("#HiddenFieldWorkBook").val(bookname);   //保存当前选择的Excel文件中所有工作簿中被选中的工作簿名称 
                    //                    alert($("#HiddenFieldWorkBook").val());
                    intimsg();
                    $("#isgoing").html("");
                    $("#Button1").show();
                }
            });
            //开始批量修改操作
            $("#Button1").click(function () {
                $("#isgoing").html("正在批量结算数据中，请耐心等待……");
                var bookname = $("#ddlway option:selected").text();
                // alert("当前选择的工作簿：" + bookname);
                $("#HiddenFieldWorkBook").val(bookname);
                // alert("当前选择的工作簿2：" + $("#HiddenFieldWorkBook").val());
                if ($("#HiddenFieldWorkBook").val() != "" && $("#HiddenFieldWorkBook").val() != "--请选择--") {
                    //alert("目前已经选中的1个工作簿了，可以进行后续的操作了！");
                }
                else {
                    alert("请先选择1个工作簿！");
                    intimsg();
                    $("#isgoing").html("");
                    $("#Button1").show();
                    return false;
                }
                if ($("#ddlPayer").val() == "") {
                    alert("请先选择1个收费人员！");
                    intimsg();
                    $("#isgoing").html("");
                    $("#Button1").show();
                    return false;
                }
            });

        })
        $(function () {
            $("#btnInto").click(function () {
                $("#btnInto").hide();
                $("#isgoing").html("正在上传数据中，请耐心等待……");
                $("#pldr").show();
            });
        })
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <input id="hfcss" runat="server" type="hidden" />
    <!--当前位置-->
    <%--<div class="location_box">
        <div class="location_left">
            <img src="../newimg/Location.png" width="11" height="15" />您当前的位置：<a>首页</a>&nbsp;&nbsp;&#8250;&#8250;&nbsp;&nbsp;<a
                class="cur">批量收费</a></div>
    </div>
    <div class="audit-tab" style="margin: 0px 0px 10px 0px;">
        <ul class="tab-ul" style="margin-left: 10px">
            <li onclick="three()">批量生成未收</li>
            <li onclick="one()" class="cur">批量导入已收</li>
            <li onclick="two()">批量在线收费</li>
        </ul>
    </div>
    <div id="step01" runat="server">
        <div class="stepbox">
            <ul class="stepul">
                <li class="step1">
                    <p>
                        1.导入Excel表格数据</p>
                    <i class="s01"></i></li>
                <li class="step3">
                    <p>
                        2.确认批量收费收费</p>
                </li>
            </ul>
        </div>
        <div id="number03" class="content">
            <div style="width: 80%; margin: auto; text-align: left;">
                <div style="height: 60px; line-height: 60px;">
                    <span style="float: left;">收费方式：</span>
                    <div class="sel" style="float: left; margin-top: 10px;">
                        <asp:DropDownList ID="dldway" runat="server" Style="width: 120px; line-height: 30px;
                            height: 30px;">
                            <asp:ListItem Value="1" Selected="True">户号(合并欠费)</asp:ListItem>
                            <asp:ListItem Value="3">户号(明细欠费)</asp:ListItem>
                            <asp:ListItem Value="2">银行账号</asp:ListItem>
                        </asp:DropDownList>
                    </div>
                </div>
                <div style="height: 30px; line-height: 30px;">
                    <span id="message" runat="server" style="line-height: 32px; color: Red;"></span>
                </div>
                <div style="height: 48px; line-height: 48px;">
                    <span style="float: left; margin-top: -10px;">选择文件：</span> <a class="cho_file2">
                        <asp:FileUpload ID="FileUpload1" runat="server" onchange="getfileurl();" />请选择需要导入的文件</a></div>
            </div>
            <div>
                <asp:TextBox ID="TextBox1" runat="server" Enabled="false" Visible="false"></asp:TextBox></div>
            <div style="border: 0px solid red; width: 90%;">
                <span id="ddlway1" runat="server" style="margin-left: 63px;"></span>
            </div>
            <div class="jddh" style="border: 0px solid red; overflow: hidden; line-height: 32px;
                border-top: 0px; margin-left: 60px;">
                <div class="jddhin" style="border: 0px solid green; width: 100%; line-height: 32px;
                    height: 100%; overflow: hidden;">
                    <span id="msg" runat="server" style="line-height: 32px; color: Red;"></span>
                </div>
                <div class="jddhin" style="border: 0px solid green; width: 100%; line-height: 32px;
                    height: 100%; overflow: hidden;">
                    <span id="isgoing" runat="server" style="line-height: 32px;"></span>
                </div>
            </div>
            <div style="width: 120px; margin-left: 122px; margin-top: 10px; margin-bottom: 10px;">
                <asp:Button ID="btnInto" runat="server" Text="导入批量收费数据" Style="width: 200px; height: 40px;
                    border-radius: 3px; background: #f78705; color: #fff; font-size: 16px; border: solid 1px #f78705"
                    OnClick="btnInto_Click" /></div>
            <div style="float: left; text-align: left; display: none;">
                工作簿名：<asp:TextBox ID="txtSheetName" runat="server" Visible="false"></asp:TextBox></div>
            <br />--%>
            <%--<div style="height: 30px; line-height: 30px;">
                <span id="test" style="display: none;">
                    <asp:Button ID="Button6" runat="server" Text="自动上传" OnClick="Button6_Click" />
                </span>
            </div>--%>
            <div style="height: 30px; line-height: 30px;">
                <span id="test" style="display: none;">
                    <asp:Button ID="Button2" runat="server" Text="导入" OnClick="Button8_Click" />
                </span>
            </div>
            <div style="height: 30px; line-height: 30px;">
                <span id="test" style="display: none;">
                    <asp:Button ID="Button3" runat="server" Text="导出" OnClick="Button9_Click" />
                </span>
            </div>
            <asp:HiddenField ID="HiddenField2" runat="server" />
            <asp:HiddenField ID="HiddenField3" runat="server" />
            <asp:HiddenField ID="hfId" runat="server" />
            <asp:HiddenField ID="hfnodeId" runat="server" />
            <asp:HiddenField ID="hfwith" runat="server" />
            <asp:HiddenField ID="hfhinght" runat="server" />
            <asp:HiddenField ID="hfmoney" runat="server" />
            <asp:HiddenField ID="hfcpom" runat="server" />
            <asp:HiddenField ID="hfcount" runat="server" />
            <asp:HiddenField ID="HiddenField1" runat="server" />
            <asp:HiddenField ID="hfname" runat="server" />
            <asp:HiddenField ID="hftishi" runat="server" />
            <asp:HiddenField ID="HiddenFieldWorkBook" runat="server" />
        </div>
    </div>
    <div id="step02" runat="server">
        <div class="stepbox">
            <ul class="stepul">
                <li class="step1">
                    <p>
                        1.导入Excel表格数据</p>
                    <i class="s03"></i></li>
                <li class="step22">
                    <p>
                        2.确认批量收费收费</p>
                </li>
            </ul>
        </div>
        <div id="number01" class="content">
            <div class="screenbox2">
                <div class="shoufei_content">
                    <div class="content">
                        <dl class="zhijie" style="overflow: hidden; margin-top: 20%;">
                            <dt></dt>
                            <dd>
                                <asp:Button ID="Button1" runat="server" OnClientClick="$('#plsf').show();" CssClass="btn btnNext"
                                    Text="批量收费" OnClick="Button1_Click" /></dd>
                        </dl>
                    </div>
                </div>
            </div>
        </div>
        <div id="ShowTips" runat="server" visible="false" class="content" style="border: 1px solid #ccc;width:400px;">
            <span style="color: Red;" id="ShowCount" runat="server"            >本次收费成功【0】个用户，失败【0】个用户</span>
            <hr />
            <div id="ShowError" runat="server" style="max-height: 220px;overflow-y: auto;">
            </div>
        </div>
    </div>
    <div id="pldr" runat="server" style="position: fixed; width: 100%; height: 100%;
        top: 0; z-index: 777; display: none;">
        <div class="batch_set" style="position: fixed; margin-left: 30%; margin-top: 15%;
            z-index: 8888; width: 480px; height: 60px;">
            <img src="../newimg/loadingPortrait.gif" width="32" height="32" style="margin-left: 20px;
                float: left; margin-top: 14px;" />
            <span style="float: left; margin-left: 20px; line-height: 60px; color: red; font-size: 14px;">
                请等待，正在上传数据中……</span>
        </div>
        <div class="black_bj">
        </div>
    </div>
    <div id="plsf" runat="server" style="position: fixed; width: 100%; height: 100%;
        top: 0; z-index: 777; display: none;">
        <div class="batch_set" style="position: fixed; margin-left: 30%; margin-top: 15%;
            z-index: 8888; width: 480px; height: 60px;">
            <img src="../newimg/loadingPortrait.gif" width="32" height="32" style="margin-left: 20px;
                float: left; margin-top: 14px;" />
            <span style="float: left; margin-left: 20px; line-height: 60px; color: red; font-size: 14px;">
                请等待，正在进行批量收费中……</span>
        </div>
        <div class="black_bj">
        </div>
    </div>
    </form>
</body>
</html>
