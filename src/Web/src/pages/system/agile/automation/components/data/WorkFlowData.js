const memoryData = []

const addData = [{
        title: '数据节点',
        listData: [
            { type: 1, name: '新增数据', imgUrl: require('@/assets/img/1.png') },
            { type: 2, name: '更新数据', imgUrl: require('@/assets/img/2.png') },
            { type: 3, name: '获取单条数据', imgUrl: require('@/assets/img/3.png') },
            { type: 4, name: '获取多条数据', imgUrl: require('@/assets/img/4.png') },
            { type: 5, name: '删除数据', imgUrl: require('@/assets/img/5.png') }
        ]
    },
    {
        title: '消息节点',
        listData: [
            { type: 6, name: '发送站内通知', imgUrl: require('@/assets/img/6.png') },
            { type: 7, name: '发送短信', imgUrl: require('@/assets/img/7.png') },
            { type: 12, name: '发送邮件', imgUrl: require('@/assets/img/7.png') },
            { type: 13, name: '发送服务号', imgUrl: require('@/assets/img/7.png') }
        ]
    },
    {
        title: '分支节点',
        listData: [
            { type: 8, name: '条件分支', imgUrl: require('@/assets/img/8.png') },
            { type: 9, name: '并行分支', imgUrl: require('@/assets/img/9.png') }
        ]
    },
    // {
    //     title: '连接器',
    //     listData: [
    //         { type: 10, name: '连接器', imgUrl: require('@/assets/img/10.png') }
    //     ]
    // },
    {
        title: '构件',
        listData: [
            { type: 31, name: '子流程', imgUrl: require('@/assets/img/31.png') },
            { type: 32, name: '封装业务', imgUrl: require('@/assets/img/32.png') }
        ]
    },
    {
        title: '开发者',
        listData: [
            { type: 21, name: '发送 API 请求', imgUrl: require('@/assets/img/21.png') },
            { type: 22, name: '代码块', imgUrl: require('@/assets/img/22.png') }
        ]
    }
    // ,
    // {
    //     title: '人工节点',
    //     listData: [
    //         { type: 11, name: '发起审批', imgUrl: require('@/assets/img/11.png') }
    //     ]
    // }
]

export {
    memoryData,
    addData
}