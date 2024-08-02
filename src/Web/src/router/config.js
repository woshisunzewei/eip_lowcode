import TabsView from '@/layouts/tabs/TabsView'
import BlankView from '@/layouts/BlankView'

// 路由配置
const options = {
    routes: [{
            path: '/login',
            name: '登录页',
            component: () =>
                import ('@/pages/account/login')
        },
        {
            path: '/oauth',
            name: '登录页',
            component: () =>
                import ('@/pages/account/oauth')
        },
        {
            path: '/ssocode',
            name: '登录页',
            component: () =>
                import ('@/pages/account/ssocode')
        },
        {
            path: '/ssouserid',
            name: '登录页',
            component: () =>
                import ('@/pages/account/ssouserid')
        },
        {
            path: '/dingTalkLogin',
            name: '登录页',
            component: () =>
                import ('@/pages/dingtalk/login')
        },
        {
            path: '*',
            name: '404',
            component: () =>
                import ('@/pages/exception/404'),
        },
        {
            path: '/403',
            name: '403',
            component: () =>
                import ('@/pages/exception/403'),
        },
        {
            path: '/',
            name: '首页',
            component: TabsView,
            redirect: '/login',
            children: [{
                path: 'dashboard',
                name: 'Dashboard',
                meta: {
                    icon: 'dashboard'
                },
                component: BlankView,
                children: [{
                        path: 'workplace',
                        name: '工作台',
                        meta: {
                            page: {
                                closable: false
                            }
                        },
                        component: () =>
                            import ('@/pages/dashboard/workplace'),
                    },
                    {
                        path: 'analysis',
                        name: '首页',
                        component: () =>
                            import ('@/pages/dashboard/analysis'),
                    }
                ]
            }, ]
        },
    ]
}

export default options