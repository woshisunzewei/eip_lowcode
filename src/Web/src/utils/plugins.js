import Vue from 'vue'
//全局注册自定义指令，用于判断当前图片是否能够加载成功，可以加载成功则赋值为img的src属性，否则使用默认图片
Vue.directive('real-img', async function(el, binding) { //指令名称为：real-img
        let defaultURL = require('@/assets/empty-banner.png'); //错误默认显示图片
        let imgURL = binding.value; //获取图片地址
        if (imgURL) {
            let exist = await imageIsExist(imgURL);
            if (exist) {
                el.setAttribute('src', imgURL);
            } else {
                el.setAttribute('src', defaultURL);
            }
        } else {
            el.setAttribute('src', defaultURL);
        }
    })
    //全局注册自定义指令，用于判断当前图片是否能够加载成功，可以加载成功则赋值为img的src属性，否则使用默认图片
Vue.directive('real-img-user', async function(el, binding) { //指令名称为：real-img
    let defaultURL = require('@/assets/avatar.jpg'); //错误默认显示图片
    let imgURL = binding.value; //获取图片地址
    if (imgURL) {
        let exist = await imageIsExist(imgURL);
        if (exist) {
            el.setAttribute('src', imgURL);
        } else {
            el.setAttribute('src', defaultURL);
        }
    } else {
        el.setAttribute('src', defaultURL);
    }
})

/**
 * 检测图片是否存在
 * @param url
 */
let imageIsExist = function(url) {
    return new Promise((resolve) => {
        var img = new Image();
        img.onload = function() {
            if (this.complete == true) {
                resolve(true);
                img = null;
            }
        }
        img.onerror = function() {
            resolve(false);
            img = null;
        }
        img.src = url;
    })
}

/**
 * @directive 自定义属性
 * @todo 弹窗拖拽属性
 * @desc 使用在弹窗内部任意加载的html添加v-drag
 * @param .ant-modal-header 弹窗头部用来拖动的属性
 * @param .ant-modal 拖动的属性
 */
Vue.directive('drag', (el, binding, vnode, oldVnode) => {
    Vue.nextTick(() => {
        const sty = (function() {
            if (window.document.currentStyle) {
                return (dom, attr) => dom.currentStyle[attr]
            } else {
                return (dom, attr) => getComputedStyle(dom, false)[attr]
            }
        })()

        var aEls = document.getElementsByTagName("div"); //找到给定父元素下的给定标签名
        var arrHeader = []; //定义一个返回的数组
        var arrModal = []; //定义一个返回的数组
        for (var i = 0; i < aEls.length; i++) {
            var aClassName = aEls[i].className.split(" "); //将符合条件的每个元素的class属性都分割成数组
            for (var j = 0; j < aClassName.length; j++) {
                if (aClassName[j] == "ant-modal-header") { //如果包含class就添加到给定数组里
                    arrHeader.push(aEls[i]);
                    aEls[i].style.cssText += ';cursor:move;'
                    break; //该元素添加了就退出循环，防止HTML文件里某一元素有两个重复的class导致程序出错
                }
            }

            for (var j = 0; j < aClassName.length; j++) {
                if (aClassName[j] == "ant-modal") {
                    arrModal.push(aEls[i]);
                    break;
                }
            }
        }

        arrHeader.forEach((dialogHeaderEl, index) => {
            var dragDom = arrModal[index];
            dialogHeaderEl.onmousedown = (e) => {
                // 鼠标按下，计算当前元素距离可视区的距离
                const disX = e.clientX - dialogHeaderEl.offsetLeft
                const disY = e.clientY - dialogHeaderEl.offsetTop

                const screenWidth = document.body.clientWidth // body当前宽度
                const screenHeight = document.documentElement.clientHeight // 可见区域高度(应为body高度，可某些环境下无法获取)

                const dragDomWidth = dragDom.offsetWidth // 对话框宽度
                const dragDomheight = dragDom.offsetHeight // 对话框高度

                const minDragDomLeft = dragDom.offsetLeft
                const maxDragDomLeft = screenWidth - dragDom.offsetLeft - dragDomWidth

                const minDragDomTop = dragDom.offsetTop
                const maxDragDomTop = screenHeight - dragDom.offsetTop - dragDomheight

                // 获取到的值带px 正则匹配替换
                let styL = sty(dragDom, 'left')
                let styT = sty(dragDom, 'top')

                // 注意在ie中 第一次获取到的值为组件自带50% 移动之后赋值为px
                if (styL.includes('%')) {
                    // eslint-disable-next-line no-useless-escape
                    styL = +document.body.clientWidth * (+styL.replace(/\%/g, '') / 100)
                        // eslint-disable-next-line no-useless-escape
                    styT = +document.body.clientHeight * (+styT.replace(/\%/g, '') / 100)
                } else {
                    styL = +styL.replace(/\px/g, '')
                    styT = +styT.replace(/\px/g, '')
                }

                document.onmousemove = function(e) {
                    // 通过事件委托，计算移动的距离
                    let left = e.clientX - disX
                    let top = e.clientY - disY

                    // 边界处理
                    if (-(left) > minDragDomLeft) {
                        left = -(minDragDomLeft)
                    } else if (left > maxDragDomLeft) {
                        left = maxDragDomLeft
                    }

                    if (-(top) > minDragDomTop) {
                        top = -(minDragDomTop)
                    } else if (top > maxDragDomTop) {
                        top = maxDragDomTop
                    }
                    // 移动当前元素
                    dragDom.style.cssText += `;left:${left + styL}px;top:${top + styT}px;`
                }

                document.onmouseup = function(e) {
                    document.onmousemove = null
                    document.onmouseup = null
                }
            }
        })
    })
})