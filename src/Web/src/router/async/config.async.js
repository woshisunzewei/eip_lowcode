import routerMap from './router.map'
import { parseRoutes } from '@/utils/routerUtil'

// 异步路由配置
const routesConfig = [
    'login',
    'oauth',
    'ssocode',
    'ssouserid',
    'dingTalkLogin',
    'root',
    {
        router: 'exp404',
        path: '*',
        name: '404'
    },
    {
        router: 'exp403',
        path: '/403',
        name: '403'
    }
]

const options = {
    mode: 'history', //打开模式
    routes: parseRoutes(routesConfig, routerMap)
}

export default options