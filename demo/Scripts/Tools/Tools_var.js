



//手机号码验证
function checkPhone(phone){ 
    if(!(/^1(3|4|5|7|8)\d{9}$/.test(phone))){ 
        //alert("手机号码有误，请重填");  
        return false; 
    } else{return false;}
}
//固定电话验证
function checkTel(tel){
if(!/^(\(\d{3,4}\)|\d{3,4}-|\s)?\d{7,14}$/.test(tel)){
//alert('固定电话有误，请重填');
return false;
}else{return false;}
}
//验证邮箱
function checkEmail(str){
var re = /^(\w-*\.*)+@(\w-?)+(\.\w{2,})+$/
if(re.test(str)){
return true;
}else{
return false;
}
}

//处理字符长度
        function StrLength(len,str) {
            if (str.length >= len) {
                str = str.substring(0, len) + " ...";
            }
            return str;
        }
//处理时间
        function FormatYMDTimeIsNUll(dateStr) {
            if (dateStr == null || dateStr == "") {
                return "";
            }
            var data = dateStr;
            data = data.replace('/', '-');
            data = data.replace('/', '-');
            data = data.replace('/', '-');
            data = data.replace('T', ' ');
            var index = data.indexOf(" ");
            data = data.substring(0, index + 1);
            if (data.trim() == "0001-01-01") {
                data = "";
            }
            return data;
        }

//倒计时（获取短信）
//<input type="button" id="btn" value="免费获取验证码" onclick="settime(this)" /> 
var countdown=60; 
function settime(val) { 
if (countdown == 0) { 
val.removeAttribute("disabled");    
val.value="免费获取验证码"; 
countdown = 60; 
} else { 
val.setAttribute("disabled", true); 
val.value="重新发送(" + countdown + ")"; 
countdown--; 
} 
setTimeout(function() { 
settime(val) 
},1000) 
} 