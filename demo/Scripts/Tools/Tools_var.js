



//�ֻ�������֤
function checkPhone(phone){ 
    if(!(/^1(3|4|5|7|8)\d{9}$/.test(phone))){ 
        //alert("�ֻ���������������");  
        return false; 
    } else{return false;}
}
//�̶��绰��֤
function checkTel(tel){
if(!/^(\(\d{3,4}\)|\d{3,4}-|\s)?\d{7,14}$/.test(tel)){
//alert('�̶��绰����������');
return false;
}else{return false;}
}
//��֤����
function checkEmail(str){
var re = /^(\w-*\.*)+@(\w-?)+(\.\w{2,})+$/
if(re.test(str)){
return true;
}else{
return false;
}
}

//�����ַ�����
        function StrLength(len,str) {
            if (str.length >= len) {
                str = str.substring(0, len) + " ...";
            }
            return str;
        }
//����ʱ��
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

//����ʱ����ȡ���ţ�
//<input type="button" id="btn" value="��ѻ�ȡ��֤��" onclick="settime(this)" /> 
var countdown=60; 
function settime(val) { 
if (countdown == 0) { 
val.removeAttribute("disabled");    
val.value="��ѻ�ȡ��֤��"; 
countdown = 60; 
} else { 
val.setAttribute("disabled", true); 
val.value="���·���(" + countdown + ")"; 
countdown--; 
} 
setTimeout(function() { 
settime(val) 
},1000) 
} 