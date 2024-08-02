import Vue from 'vue'
import App from '@/App.vue'
import { initRouter } from '@/router'
import './theme/index.less'
import store from '@/store'
import 'animate.css/source/animate.css'
import Plugins from '@/plugins'
import { initI18n } from '@/utils/i18n'
import bootstrap from '@/bootstrap'
import 'moment/locale/zh-cn'
import '@/utils/lazy_use'
import '@/utils/lazy_use_table'
import '@/utils/k-form-design'
import '@/utils/prototype'
import VueContextMenu from 'vue-contextmenu'
import vcolorpicker from 'vcolorpicker'

Vue.prototype.$echarts = window.echarts;
import '@/assets/css/globalCss.css'
//注册自定义组件
import '@/components/eip/eip-register'

//颜色选择
const router = initRouter(store.state.setting.asyncRoutes)
const i18n = initI18n('CN', 'US')

//打印
import { autoConnect, disAutoConnect, hiPrintPlugin } from './pages/system/agile/print/components/print'
Vue.use(hiPrintPlugin)
disAutoConnect(); // 取消自动连接
import Storage from 'vue-lsp'
const options = {
    namespace: 'eip__', // key prefix
    name: 'ls', // name variable Vue.[ls] or this.[$ls],
    storage: 'local' // storage name session, local, memory
}
Vue.use(Storage, options)

Vue.config.productionTip = false

Vue.use(VueContextMenu)
Vue.use(vcolorpicker)


Vue.use(Plugins)
bootstrap({ router, store, i18n, message: Vue.prototype.$message })
import '@/utils/plugins.js'

//移动端设计器配置
import '@/pages/system/agile/mobile/utils/mobile'

//全局
import loading from '@/components/loading/loading';
Vue.use(loading);

import VueAMap from 'vue-amap';
Vue.use(VueAMap);
window._AMapSecurityConfig = {
        securityJsCode: '79ed420415785560823cd71f6c54d89e',
    }
    // 初始化vue-amap 开始
VueAMap.initAMapApiLoader({
    key: "8aef12cf27d566522158230457b171cc",
    //插件根据自己项目中需要用到的自行添加
    plugin: [
        'AMap.Autocomplete',
        'AMap.PlaceSearch',
        'AMap.Scale',
        'AMap.OverView',
        'AMap.ToolBar',
        'AMap.MapType',
        "AMap.Geolocation", // 定位控件，用来获取和展示用户主机所在的经纬度位置
        "AMap.Geocoder", // 地理编码与逆地理编码服务，用于地址描述与坐标间的相互转换
        "AMap.CitySearch",
    ],
    v: '1.4.15',
    uiVersion: '1.0'
});

//记录最后一次使用时间
window.onload = function() {
    window.document.onmousedown = function() {
        window.sessionStorage.setItem('lastTime', new Date().getTime())
    }
}

new Vue({
    router,
    store,
    i18n,
    render: h => h(App),
}).$mount('#app')