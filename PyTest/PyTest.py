import requests
from pyquery import PyQuery
import xlsxwriter

headers = {
    'user-agent': 'Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/79.0.3945.117 Safari/537.36',
    'cookie': 'PHPSESSID=ac3sb36dhs40flu0tbiisasj05; mfw_uuid=5e21231d-c1b3-5b5a-1209-580057588364; _r=cnblogs; _rp=a%3A2%3A%7Bs%3A1%3A%22p%22%3Bs%3A16%3A%22www.cnblogs.com%2F%22%3Bs%3A1%3A%22t%22%3Bi%3A1579229981%3B%7D; oad_n=a%3A5%3A%7Bs%3A5%3A%22refer%22%3Bs%3A24%3A%22https%3A%2F%2Fwww.cnblogs.com%2F%22%3Bs%3A2%3A%22hp%22%3Bs%3A15%3A%22www.cnblogs.com%22%3Bs%3A3%3A%22oid%22%3Bi%3A1029%3Bs%3A2%3A%22dm%22%3Bs%3A15%3A%22www.mafengwo.cn%22%3Bs%3A2%3A%22ft%22%3Bs%3A19%3A%222020-01-17+10%3A59%3A41%22%3B%7D; __mfwothchid=referrer%7Cwww.cnblogs.com; __omc_chl=; __mfwc=referrer%7Cwww.cnblogs.com; __mfwa=1579229972785.21227.1.1579229972785.1579229972785; __mfwlv=1579229972; __mfwvn=1; __omc_r=; UM_distinctid=16fb1713558248-003fe84efe3cff-7711a3e-1fa400-16fb1713559c3; uva=s%3A91%3A%22a%3A3%3A%7Bs%3A2%3A%22lt%22%3Bi%3A1579230029%3Bs%3A10%3A%22last_refer%22%3Bs%3A23%3A%22http%3A%2F%2Fwww.mafengwo.cn%2F%22%3Bs%3A5%3A%22rhost%22%3BN%3B%7D%22%3B; __mfwurd=a%3A3%3A%7Bs%3A6%3A%22f_time%22%3Bi%3A1579230029%3Bs%3A9%3A%22f_rdomain%22%3Bs%3A15%3A%22www.mafengwo.cn%22%3Bs%3A6%3A%22f_host%22%3Bs%3A3%3A%22www%22%3B%7D; __mfwuuid=5e21231d-c1b3-5b5a-1209-580057588364; Hm_lvt_8288b2ed37e5bc9b4c9f7008798d2de0=1579230083; __mfwb=2144ab3ec76e.5.direct; __mfwlt=1579230122; Hm_lpvt_8288b2ed37e5bc9b4c9f7008798d2de0=1579230132'
}


s = requests.Session()


value = []

def getList(maxNum):
    """
    获取列表页面数据
    :param maxNum: 最大抓取页数
    :return:
    """
    url = 'http://www.mafengwo.cn/gonglve/'
    s.get(url, headers = headers)
    for page in range(1, maxNum + 1):
        data = {'page': page}
        response = s.post(url, data = data, headers = headers)
        doc = PyQuery(response.text)
        items = doc('.feed-item').items()
        for item in items:
            if item('.type strong').text() == '游记':
                # 如果是游记，则进入内页数据抓取
                inner_url = item('a').attr('href')
                getInfo(inner_url)


def getInfo(url):
    """
    获取内页数据
    :param url: 内页链接
    :return:
    """
    headers2 = {
        'accept': '*/*',
        'accept-encoding': 'gzip, deflate, br',
        'accept-language': 'zh-CN,zh;q=0.9',
        'content-length': '6964',
        'content-type': 'multipart/form-data;boundary=----WebKitFormBoundaryoOAhuPRV5MAbmyVI',
        'user-agent':'Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/79.0.3945.117 Safari/537.36',
        'cookie':'PHPSESSID=ac3sb36dhs40flu0tbiisasj05; mfw_uuid=5e21231d-c1b3-5b5a-1209-580057588364; _r=cnblogs; _rp=a%3A2%3A%7Bs%3A1%3A%22p%22%3Bs%3A16%3A%22www.cnblogs.com%2F%22%3Bs%3A1%3A%22t%22%3Bi%3A1579229981%3B%7D; oad_n=a%3A5%3A%7Bs%3A5%3A%22refer%22%3Bs%3A24%3A%22https%3A%2F%2Fwww.cnblogs.com%2F%22%3Bs%3A2%3A%22hp%22%3Bs%3A15%3A%22www.cnblogs.com%22%3Bs%3A3%3A%22oid%22%3Bi%3A1029%3Bs%3A2%3A%22dm%22%3Bs%3A15%3A%22www.mafengwo.cn%22%3Bs%3A2%3A%22ft%22%3Bs%3A19%3A%222020-01-17+10%3A59%3A41%22%3B%7D; __mfwothchid=referrer%7Cwww.cnblogs.com; __omc_chl=; __mfwc=referrer%7Cwww.cnblogs.com; __mfwa=1579229972785.21227.1.1579229972785.1579229972785; __mfwlv=1579229972; __mfwvn=1; __omc_r=; UM_distinctid=16fb1713558248-003fe84efe3cff-7711a3e-1fa400-16fb1713559c3; uva=s%3A91%3A%22a%3A3%3A%7Bs%3A2%3A%22lt%22%3Bi%3A1579230029%3Bs%3A10%3A%22last_refer%22%3Bs%3A23%3A%22http%3A%2F%2Fwww.mafengwo.cn%2F%22%3Bs%3A5%3A%22rhost%22%3BN%3B%7D%22%3B; __mfwurd=a%3A3%3A%7Bs%3A6%3A%22f_time%22%3Bi%3A1579230029%3Bs%3A9%3A%22f_rdomain%22%3Bs%3A15%3A%22www.mafengwo.cn%22%3Bs%3A6%3A%22f_host%22%3Bs%3A3%3A%22www%22%3B%7D; __mfwuuid=5e21231d-c1b3-5b5a-1209-580057588364; Hm_lvt_8288b2ed37e5bc9b4c9f7008798d2de0=1579230083; __mfwb=2144ab3ec76e.8.direct; __mfwlt=1579232447; Hm_lpvt_8288b2ed37e5bc9b4c9f7008798d2de0=1579232448'
        }
    response = s.get(url, headers = headers)
    doc = PyQuery(response.text)
    title = doc('title').text()
    # 获取数据采集区
    item = doc('.tarvel_dir_list')
    if len(item) == 0:
        return
    time = item('.time').text()
    day = item('.day').text()
    people = item('.people').text()
    cost = item('.cost').text()
    # 数据格式化
    if time == '':
        pass
    else:
        time = time.split('/')[1] if len(time.split('/')) > 1 else ''

    if day == '':
        pass
    else:
        day = day.split('/')[1] if len(day.split('/')) > 1 else ''

    if people == '':
        pass
    else:
        people = people.split('/')[1] if len(people.split('/')) > 1 else ''

    if cost == '':
        pass
    else:
        cost = cost.split('/')[1] if len(cost.split('/')) > 1 else ''


    value.append([title, time, day, people, cost, url])


def write_excel_xlsx(value):
    """
    数据写入Excel
    :param value:
    :return:
    """
    index = len(value)

    workbook = xlsxwriter.Workbook('mfw.xlsx')
    sheet = workbook.add_worksheet()
    for i in range(1, index + 1):
        row = 'A' + str(i)
        sheet.write_row(row, value[i - 1])
    workbook.close()
    print("xlsx格式表格写入数据成功！")


def main():
    getList(5)
    write_excel_xlsx(value)

if __name__ == '__main__':
    main()