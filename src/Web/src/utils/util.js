import enquireJs from 'enquire.js'
import { JSEncrypt } from 'jsencrypt'
export function isDef(v) {
    return v !== undefined && v !== null
}

/**
 * Remove an item from an array.
 */
export function remove(arr, item) {
    if (arr.length) {
        const index = arr.indexOf(item)
        if (index > -1) {
            return arr.splice(index, 1)
        }
    }
}

export function isRegExp(v) {
    return _toString.call(v) === '[object RegExp]'
}

export function enquireScreen(call) {
    const handler = {
        match: function() {
            call && call(true)
        },
        unmatch: function() {
            call && call(false)
        }
    }
    enquireJs.register('only screen and (max-width: 767.99px)', handler)
}

const _toString = Object.prototype.toString

export function newGuid() {
    var guid = "";
    for (var i = 1; i <= 32; i++) {
        var n = Math.floor(Math.random() * 16.0).toString(16);
        guid += n;
        if ((i == 8) || (i == 12) || (i == 16) || (i == 20))
            guid += "-";
    }
    return guid;
}

export function selectTableRow(table, method, that, single = true) {
    var rows = table.getCheckboxRecords();
    if (rows.length == 0) {
        that.$message.warning("请选择需要操作的数据");
    } else if (single) {
        if (rows.length > 1) {
            that.$message.warning("您选择了多条数据,请取消多余勾选！");
        } else {
            method(rows[0]);
        }
    } else {
        method(rows);
    }
}
export function selectTableRowRadio(table, method, that) {
    var row = table.getRadioRecord();
    if (row) {
        method(row);
    } else {
        that.$message.warning("请选择需要操作的数据");
    }
}

export function deleteConfirm(msg, method, that) {
    that.$confirm({
        title: "删除提示?",
        content: msg,
        okText: "确定",
        okType: "danger",
        cancelText: "取消",
        onOk() {
            method()
        },
        onCancel() {},
    });
}
export function operationConfirm(msg, method, that) {
    that.$confirm({
        title: "操作提示?",
        content: msg,
        okText: "确定",
        okType: "danger",
        cancelText: "取消",
        onOk() {
            method()
        },
        onCancel() {},
    });
}

/**
 * 加密
 * @param {*} publicKey 
 * @param {*} data 
 * @returns 
 */
export function encryptedData(data) {
    let publicKey = "MIGfMA0GCSqGSIb3DQEBAQUAA4GNADCBiQKBgQC9ds1UxO/ARslpqzBA4AIY7ejOJyYoe980wYSpMpfYb/cLyHl66c4uqm567CrigYxkt1Nv24h4L8Cy4v2aJm6T/Oi46a5DRHfbYFyHGwzan+iUqsNb37xPHkTd1KDuRYbvZsYZD15wUHPWczECXdj2FO0C4VDx6kriwJQv9PoqnwIDAQAB"
        // 新建JSEncrypt对象
    let encryptor = new JSEncrypt();
    // 设置公钥
    encryptor.setPublicKey(publicKey);
    // 加密数据
    return encryptor.encrypt(data);
}

/**
 * 解密
 * @param {*} privateKey 
 * @param {*} data 
 * @returns 
 */
export function decryptData(privateKey, data) {
    // 新建JSEncrypt对象
    let decrypt = new JSEncrypt();
    // 设置私钥
    decrypt.setPrivateKey(privateKey);
    // 解密数据
    return decrypt.decrypt(data);
}

/**
 * 浏览器信息
 */
export function getBrowser() {
    // 获取浏览器 userAgent
    var ua = navigator.userAgent;
    // 是否为 Opera
    var isOpera = ua.indexOf("opr") > -1;
    // 返回结果
    if (isOpera) {
        return "Opera";
    }
    // 是否为 IE
    var isIE =
        ua.indexOf("compatible") > -1 && ua.indexOf("MSIE") > -1 && !isOpera;
    var isIE11 = ua.indexOf("Trident") > -1 && ua.indexOf("rv:11.0") > -1;
    // 返回结果
    if (isIE11) {
        return "IE11";
    } else if (isIE) {
        // 检测是否匹配
        var re = new RegExp("MSIE (\\d+\\.\\d+);");
        re.test(ua);
        // 获取版本
        var ver = parseFloat(RegExp["$1"]);
        // 返回结果
        if (ver == 7) {
            return "IE7";
        } else if (ver == 8) {
            return "IE8";
        } else if (ver == 9) {
            return "IE9";
        } else if (ver == 10) {
            return "IE10";
        } else {
            return "IE";
        }
    }

    // 是否为 Edge
    var isEdge = ua.indexOf("Edg") > -1;
    // 返回结果
    if (isEdge) {
        return "Edge";
    }

    // 是否为 Firefox
    var isFirefox = ua.indexOf("Firefox") > -1;
    // 返回结果
    if (isFirefox) {
        return "Firefox";
    }

    // 是否为 Safari
    var isSafari = ua.indexOf("Safari") > -1 && ua.indexOf("Chrome") == -1;
    // 返回结果
    if (isSafari) {
        return "Safari";
    }
    /**
     * 360浏览器（极速内核）
     */
    function check360() {
        var result = false;
        for (var key in navigator.plugins) {
            // np-mswmp.dll只在360浏览器下存在
            if (navigator.plugins[key].filename == "internal-nacl-plugin") {
                return !result;
            }
        }
        return result;
    }
    // 是否为 Chrome
    var isChrome =
        ua.indexOf("Chrome") > -1 &&
        ua.indexOf("Safari") > -1 &&
        ua.indexOf("Edge") == -1 &&
        ua.indexOf("QQBrowser") == -1 &&
        ua.indexOf("2345Explorer") == -1 &&
        check360() == false;
    // 返回结果
    if (isChrome) {
        return "Chrome";
    }

    // 是否为 QQ
    var isQQ = ua.indexOf("QQBrowser") > -1;
    // 返回结果
    if (isQQ) {
        return "QQ浏览器";
    }

    // 是否为搜狗浏览器
    var isMaxthon = ua.indexOf("se 2.x") > -1;
    // 返回结果
    if (isMaxthon) {
        return "搜狗浏览器";
    }

    // 是否为2345浏览器
    var is2345Explorer = ua.indexOf("2345Explorer") > -1;
    // 返回结果
    if (is2345Explorer) {
        return "2345浏览器";
    }


    var is360 = check360() && ua.indexOf("Safari") > -1;
    if (is360) {
        return "360浏览器";
    }

    // 都不是
    return "";
}