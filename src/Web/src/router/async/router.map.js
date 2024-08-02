// 视图组件
const view = {
    tabs: () =>
        import ('@/layouts/tabs'),
    blank: () =>
        import ('@/layouts/BlankView'),
    page: () =>
        import ('@/layouts/PageView')
}

// 路由组件注册
const routerMap = {
    login: {
        authority: '*',
        path: '/login',
        name: '登录',
        component: () =>
            import ('@/pages/account/login')
    },
    oauth: {
        authority: '*',
        path: '/oauth',
        name: '登录',
        component: () =>
            import ('@/pages/account/oauth')
    },
    ssocode: {
        authority: '*',
        path: '/ssocode',
        name: '登录',
        component: () =>
            import ('@/pages/account/ssocode')
    },
    ssouserid: {
        authority: '*',
        path: '/ssouserid',
        name: '登录',
        component: () =>
            import ('@/pages/account/ssouserid')
    },
    dingTalkLogin: {
        authority: '*',
        path: '/dingTalkLogin',
        name: '登录',
        component: () =>
            import ('@/pages/dingtalk/login')
    },
    root: {
        path: '/',
        name: '首页',
        redirect: '/login',
        component: view.tabs
    },
    rootblank: {
        path: '/',
        name: '首页',
        redirect: '/login',
        component: view.blank
    },
    dashboard: {
        name: 'Dashboard',
        component: view.blank
    },
    workplace: {
        name: '工作台',
        component: () =>
            import ('@/pages/dashboard/workplace')
    },
    analysis: {
        name: '首页',
        component: () =>
            import ('@/pages/dashboard/analysis')
    },

    analysisHello: {
        name: '首页',
        component: () =>
            import ('@/pages/dashboard/analysis/AnalysisHello')
    },
    exception: {
        name: '异常页',
        icon: 'warning',
        component: view.blank
    },
    exp403: {
        authority: '*',
        name: '403异常 ',
        path: '403',
        component: () =>
            import ('@/pages/exception/403')
    },
    exp404: {
        name: 'exp404',
        path: '404',
        component: () =>
            import ('@/pages/exception/404')
    },
    exp500: {
        name: 'exp500',
        path: '500',
        component: () =>
            import ('@/pages/exception/500')
    },
    config: {
        name: 'config',
        component: view.blank
    },
    system: {
        name: '系统管理',
        component: view.blank
    },
    report: {
        name: '系统报表',
        component: view.blank
    },

    configindex: {
        name: '系统参数',
        path: 'configindex',
        component: () =>
            import ('@/pages/system/config/index')
    },
    sms: {
        name: '短信管理',
        component: view.blank
    },
    smstemplate: {
        name: '短信模版',
        path: 'smstemplate',
        component: () =>
            import ('@/pages/system/smstemplate/list')
    },
    smsconfig: {
        name: '短信配置',
        path: 'smsconfig',
        component: () =>
            import ('@/pages/system/smsconfig/list')
    },
    dictionary: {
        name: '字典管理',
        path: 'dictionary',
        component: () =>
            import ('@/pages/system/dictionary/list')
    },
    district: {
        name: '行政区划',
        path: 'district',
        component: () =>
            import ('@/pages/system/district/list')
    },

    permissionsetting: {
        name: '权限信息',
        component: view.blank
    },
    systemsetting: {
        name: '系统设置',
        component: view.blank
    },
    systemmonitor: {
        name: '系统监控',
        component: view.blank
    },
    systemmonitorcap: {
        name: 'cap',
        path: 'systemmonitorcap',
        component: () =>
            import ('@/pages/system/monitor/cap')
    },
    systemmonitorsystem: {
        name: '系统',
        path: 'systemmonitorsystem',
        component: () =>
            import ('@/pages/system/monitor/system')
    },
    systemmonitorassemblies: {
        name: '程序集',
        path: 'systemmonitorassemblies',
        component: () =>
            import ('@/pages/system/monitor/assemblies')
    },
    systemmonitorapi: {
        name: 'api',
        path: 'systemmonitorapi',
        component: () =>
            import ('@/pages/system/monitor/api')
    },
    systemmonitoronlineuser: {
        name: 'api',
        path: 'systemmonitoronlineuser',
        component: () =>
            import ('@/pages/system/monitor/onlineuser')
    },
    log: {
        name: '系统日志',
        component: view.blank
    },
    smslog: {
        name: '短信日志',
        path: 'smslog',
        component: () =>
            import ('@/pages/system/log/sms')
    },
    operationlog: {
        name: '操作日志',
        path: 'operationlog',
        component: () =>
            import ('@/pages/system/log/operation')
    },
    loginlog: {
        name: '登录日志',
        path: 'loginlog',
        component: () =>
            import ('@/pages/system/log/login')
    },
    exceptionlog: {
        name: '异常日志',
        path: 'exceptionlog',
        component: () =>
            import ('@/pages/system/log/exception')
    },
    txtlog: {
        name: '文本日志',
        path: 'txtlog',
        component: () =>
            import ('@/pages/system/log/txt')
    },
    ratelimitlog: {
        name: '限流日志',
        path: 'ratelimitlog',
        component: () =>
            import ('@/pages/system/log/ratelimit')
    },
    material: {
        name: '素材维护',
        path: 'material',
        component: () =>
            import ('@/pages/system/material/list')
    },
    menu: {
        name: '模块维护',
        path: 'menu',
        component: () =>
            import ('@/pages/system/menu/list')
    },

    menubutton: {
        name: '模块按钮',
        path: 'menubutton',
        component: () =>
            import ('@/pages/system/menubutton/list')
    },

    data: {
        name: '数据权限',
        path: 'data',
        component: () =>
            import ('@/pages/system/data/list')
    },
    datasource: {
        name: '数据来源',
        path: 'datasource',
        component: () =>
            import ('@/pages/system/datasource/list')
    },
    databaselist: {
        name: '数据库配置',
        path: 'databaselist',
        component: () =>
            import ('@/pages/system/database/list')
    },
    usersetting: {
        name: '权限设置',
        component: view.blank
    },

    user: {
        name: '系统用户',
        path: 'user',
        component: () =>
            import ('@/pages/system/user/list')
    },
    organization: {
        name: '组织架构',
        path: 'organization',
        component: () =>
            import ('@/pages/system/organization/list')
    },
    role: {
        name: '角色维护',
        path: 'role',
        component: () =>
            import ('@/pages/system/role/list')
    },

    post: {
        name: '岗位维护',
        path: 'post',
        component: () =>
            import ('@/pages/system/post/list')
    },

    group: {
        name: '组维护',
        path: 'group',
        component: () =>
            import ('@/pages/system/group/list')
    },

    notice: {
        name: '公告维护',
        path: 'notice',
        component: () =>
            import ('@/pages/system/notice/list')
    },
    messagelog: {
        name: '所有消息',
        path: 'messagelog',
        component: () =>
            import ('@/pages/system/messagelog/list')
    },
    job: {
        name: '定时任务',
        component: view.blank
    },
    joblist: {
        name: '任务列表',
        path: 'joblist',
        component: () =>
            import ('@/pages/system/job/list')
    },

    agile: {
        name: '敏捷开发',
        component: view.blank
    },
    agilecodegeneration: {
        name: '代码生成器',
        path: 'agilecodegeneration',
        component: () =>
            import ('@/pages/system/agile/codegeneration/index')
    },
    agileautomation: {
        name: '自动化',
        path: 'agileautomation',
        component: () =>
            import ('@/pages/system/agile/automation/list')
    },
    agileformlist: {
        name: '表单构建',
        path: 'agileformlist',
        component: () =>
            import ('@/pages/system/agile/form/list')
    },
    agileprintlist: {
        name: '打印构建',
        path: 'agileprintlist',
        component: () =>
            import ('@/pages/system/agile/print/list')
    },
    agilemobilelist: {
        name: '表单构建',
        path: 'agilemobilelist',
        component: () =>
            import ('@/pages/system/agile/mobile/list')
    },

    agileappindex: {
        name: '应用管理',
        path: 'agileappindex',
        component: () =>
            import ('@/pages/system/agile/app/index')
    },
    database: {
        name: '数据库信息',
        component: view.blank
    },
    databasetable: {
        name: '数据库表',
        path: 'databasetable',
        component: () =>
            import ('@/pages/system/database/table')
    },
    databasespaceused: {
        name: '表空间',
        path: 'databasespaceused',
        component: () =>
            import ('@/pages/system/database/spaceused')
    },
    databaseproc: {
        name: '存储过程',
        path: 'databaseproc',
        component: () =>
            import ('@/pages/system/database/proc')
    },
    databaseview: {
        name: '视图',
        path: 'databaseview',
        component: () =>
            import ('@/pages/system/database/view')
    },
    workflow: {
        name: '工作流',
        component: view.blank
    },
    workflowconfig: {
        name: '流程设置',
        component: view.blank
    },
    workflowprocess: {
        name: '流程管理',
        path: 'workflowprocess',
        component: () =>
            import ('@/pages/workflow/process/list')
    },
    workflowbutton: {
        name: '流程按钮',
        path: 'workflowbutton',
        component: () =>
            import ('@/pages/workflow/button/list')
    },
    workflowcomment: {
        name: '流程意见',
        path: 'workflowcomment',
        component: () =>
            import ('@/pages/workflow/comment/list')
    },

    workflowmyconfig: {
        name: '个人设置',
        component: view.blank
    },

    workflowmyconfigcomment: {
        name: '我的意见',
        path: 'workflowmyconfigcomment',
        component: () =>
            import ('@/pages/workflow/myconfig/comment')
    },
    workflowsend: {
        name: '流程发起',
        component: view.blank
    },
    workflowsendlibrary: {
        name: '流程库',
        path: 'workflowsendlibrary',
        component: () =>
            import ('@/pages/workflow/send/library')
    },
    workflowdraft: {
        name: '草稿',
        path: 'workflowdraft',
        component: () =>
            import ('@/pages/workflow/draft/list')
    },
    workflowmodel: {
        name: '范本',
        path: 'workflowmodel',
        component: () =>
            import ('@/pages/workflow/model/list')
    },

    workflowmonitor: {
        name: '流程监控',
        component: view.blank
    },
    workflowmonitorlist: {
        name: '流程列表',
        path: 'workflowmonitorlist',
        component: () =>
            import ('@/pages/workflow/monitor/list')
    },

    agilerunlist: {
        name: '动态路由菜单',
        path: 'agilerunlist/:id',
        meta: {
            icon: 'project',
            params: {
                id: null
            }
        },
        component: () =>
            import ('@/pages/system/agile/run/list')
    },
    wechat: {
        name: '微信管理',
        component: view.blank
    },
    wechatuser: {
        name: '授权用户',
        path: 'wechatuser',
        component: () =>
            import ('@/pages/wechat/user/list')
    },
    wechatmp: {
        name: '微信公众号',
        component: view.blank
    },
    wechataccount: {
        name: '微信帐号',
        path: 'wechataccount',
        component: () =>
            import ('@/pages/wechat/account/list')
    },
    wechatmptemplate: {
        name: '微信公众号模版',
        path: 'wechatmptemplate',
        component: () =>
            import ('@/pages/wechat/mp/template/list')
    },

    wechatwork: {
        name: '企业微信',
        component: view.blank
    },

    wechatworkdepartment: {
        name: '组织架构',
        path: 'wechatworkdepartment',
        component: () =>
            import ('@/pages/wechat/work/department/list')
    },
    wechatworkuser: {
        name: '用户',
        path: 'wechatworkuser',
        component: () =>
            import ('@/pages/wechat/work/user/list')
    },

    dingtalk: {
        name: '钉钉',
        component: view.blank
    },
    dingtalkdepartment: {
        name: '组织架构',
        path: 'dingtalkdepartment',
        component: () =>
            import ('@/pages/dingtalk/department/list')
    },
    dingtalkuser: {
        name: '用户',
        path: 'dingtalkuser',
        component: () =>
            import ('@/pages/dingtalk/user/list')
    },
    agileappbuild: {
        name: '动态路由菜单',
        path: 'appbuild/:id',
        meta: {
            icon: 'project',
            params: {
                id: null
            }
        },
        component: () =>
            import ('@/pages/system/agile/app/build')
    },
    //默认100个一级
    app1: {
        name: '应用',
        component: view.blank
    },
    app2: {
        name: '应用',
        component: view.blank
    },
    app3: {
        name: '应用',
        component: view.blank
    },
    app4: {
        name: '应用',
        component: view.blank
    },
    app5: {
        name: '应用',
        component: view.blank
    },
    app6: {
        name: '应用',
        component: view.blank
    },
    app7: {
        name: '应用',
        component: view.blank
    },
    app8: {
        name: '应用',
        component: view.blank
    },
    app9: {
        name: '应用',
        component: view.blank
    },
    app10: {
        name: '应用',
        component: view.blank
    },
    app11: {
        name: '应用',
        component: view.blank
    },
    app12: {
        name: '应用',
        component: view.blank
    },
    app13: {
        name: '应用',
        component: view.blank
    },
    app14: {
        name: '应用',
        component: view.blank
    },
    app15: {
        name: '应用',
        component: view.blank
    },
    app16: {
        name: '应用',
        component: view.blank
    },
    app17: {
        name: '应用',
        component: view.blank
    },
    app18: {
        name: '应用',
        component: view.blank
    },
    app19: {
        name: '应用',
        component: view.blank
    },
    app20: {
        name: '应用',
        component: view.blank
    },
    app21: {
        name: '应用',
        component: view.blank
    },
    app22: {
        name: '应用',
        component: view.blank
    },
    app23: {
        name: '应用',
        component: view.blank
    },
    app24: {
        name: '应用',
        component: view.blank
    },
    app25: {
        name: '应用',
        component: view.blank
    },
    app26: {
        name: '应用',
        component: view.blank
    },
    app27: {
        name: '应用',
        component: view.blank
    },
    app28: {
        name: '应用',
        component: view.blank
    },
    app29: {
        name: '应用',
        component: view.blank
    },
    app30: {
        name: '应用',
        component: view.blank
    },
    app31: {
        name: '应用',
        component: view.blank
    },
    app32: {
        name: '应用',
        component: view.blank
    },
    app33: {
        name: '应用',
        component: view.blank
    },
    app34: {
        name: '应用',
        component: view.blank
    },
    app35: {
        name: '应用',
        component: view.blank
    },
    app36: {
        name: '应用',
        component: view.blank
    },
    app37: {
        name: '应用',
        component: view.blank
    },
    app38: {
        name: '应用',
        component: view.blank
    },
    app39: {
        name: '应用',
        component: view.blank
    },
    app40: {
        name: '应用',
        component: view.blank
    },
    app41: {
        name: '应用',
        component: view.blank
    },
    app42: {
        name: '应用',
        component: view.blank
    },
    app43: {
        name: '应用',
        component: view.blank
    },
    app44: {
        name: '应用',
        component: view.blank
    },
    app45: {
        name: '应用',
        component: view.blank
    },
    app46: {
        name: '应用',
        component: view.blank
    },
    app47: {
        name: '应用',
        component: view.blank
    },
    app48: {
        name: '应用',
        component: view.blank
    },
    app49: {
        name: '应用',
        component: view.blank
    },
    app50: {
        name: '应用',
        component: view.blank
    },
    app51: {
        name: '应用',
        component: view.blank
    },
    app52: {
        name: '应用',
        component: view.blank
    },
    app53: {
        name: '应用',
        component: view.blank
    },
    app54: {
        name: '应用',
        component: view.blank
    },
    app55: {
        name: '应用',
        component: view.blank
    },
    app56: {
        name: '应用',
        component: view.blank
    },
    app57: {
        name: '应用',
        component: view.blank
    },
    app58: {
        name: '应用',
        component: view.blank
    },
    app59: {
        name: '应用',
        component: view.blank
    },
    app60: {
        name: '应用',
        component: view.blank
    },
    app61: {
        name: '应用',
        component: view.blank
    },
    app62: {
        name: '应用',
        component: view.blank
    },
    app63: {
        name: '应用',
        component: view.blank
    },
    app64: {
        name: '应用',
        component: view.blank
    },
    app65: {
        name: '应用',
        component: view.blank
    },
    app66: {
        name: '应用',
        component: view.blank
    },
    app67: {
        name: '应用',
        component: view.blank
    },
    app68: {
        name: '应用',
        component: view.blank
    },
    app69: {
        name: '应用',
        component: view.blank
    },
    app70: {
        name: '应用',
        component: view.blank
    },
    app71: {
        name: '应用',
        component: view.blank
    },
    app72: {
        name: '应用',
        component: view.blank
    },
    app73: {
        name: '应用',
        component: view.blank
    },
    app74: {
        name: '应用',
        component: view.blank
    },
    app75: {
        name: '应用',
        component: view.blank
    },
    app76: {
        name: '应用',
        component: view.blank
    },
    app77: {
        name: '应用',
        component: view.blank
    },
    app78: {
        name: '应用',
        component: view.blank
    },
    app79: {
        name: '应用',
        component: view.blank
    },
    app80: {
        name: '应用',
        component: view.blank
    },
    app81: {
        name: '应用',
        component: view.blank
    },
    app82: {
        name: '应用',
        component: view.blank
    },
    app83: {
        name: '应用',
        component: view.blank
    },
    app84: {
        name: '应用',
        component: view.blank
    },
    app85: {
        name: '应用',
        component: view.blank
    },
    app86: {
        name: '应用',
        component: view.blank
    },
    app87: {
        name: '应用',
        component: view.blank
    },
    app88: {
        name: '应用',
        component: view.blank
    },
    app89: {
        name: '应用',
        component: view.blank
    },
    app90: {
        name: '应用',
        component: view.blank
    },
    app91: {
        name: '应用',
        component: view.blank
    },
    app92: {
        name: '应用',
        component: view.blank
    },
    app93: {
        name: '应用',
        component: view.blank
    },
    app94: {
        name: '应用',
        component: view.blank
    },
    app95: {
        name: '应用',
        component: view.blank
    },
    app96: {
        name: '应用',
        component: view.blank
    },
    app97: {
        name: '应用',
        component: view.blank
    },
    app98: {
        name: '应用',
        component: view.blank
    },
    app99: {
        name: '应用',
        component: view.blank
    },
    app100: {
        name: '应用',
        component: view.blank
    },

    bigscreen: {
        name: '大屏',
        component: () =>
            import ('@/pages/demo/bigscreen')
    }
}
export default routerMap