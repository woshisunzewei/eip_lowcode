export const commonNodes = [{
        type: 'begin',
        text: { text: '开始' },
        icon: 'play-circle',
        props: {
            base: {
                title: "开始",
                formId: undefined,
                formName: "",
                isOpinion: false,
                isArchive: false,
                titleRule: "您有一条待审批流程",
            },
            timeout: [], //超时
            button: [], //流程按钮
            notice: [], //通知
            event: [], //事件
            column: [], //字段
        }
    },
    {
        type: 'task',
        text: { text: '任务' },
        icon: 'user',
        props: {
            base: {
                title: "任务",
                formId: undefined,
                formName: "",
                isOpinion: false,
                isArchive: false,
                commentMessage: true,
                commentSign: false,
                commentFile: false
            },
            //处理用户
            user: {
                strategy: 0, //处理策略
                chosen: 0, //选人规则
                no: 0, //无对应处理人
                auto: [], //自动同意规则
                pass: 0, //通过策略
                passConfig: 0, //通过策略配置
                approve: [], //审核处理人
                addActivity: [], //加签处理人
            },
            timeout: [], //超时
            button: [], //流程按钮
            notice: [], //通知
            event: [], //事件
            column: [], //字段
        }
    },
    {
        type: 'fork',
        text: { text: '条件' },
        icon: 'share-alt',
        props: {
            base: {
                title: "条件",
            },
        },
    },
    {
        type: 'join',
        text: { text: '合并' },
        icon: 'pull-request',
        props: {
            base: {
                title: "合并",
                type: 0, //默认为条件
                pass: 0, //默认需要所有人通过才可以下一步
                passPercent: 0, //百分百
            },
        }
    },
    {
        type: 'subprocess',
        text: { text: '子流' },
        icon: 'apartment',
        props: {
            //基础属性
            base: {
                title: "子流",
                formId: undefined,
                formName: "",
                subProcessProcessId: undefined,
                subProcessName: "",
                async: true,
                autoEndStart: true,
            },
            output: [],
            input: [],
            //处理用户
            user: {
                strategy: 0, //处理策略
                chosen: 0, //选人规则
                no: 0, //无对应处理人
                auto: [], //自动同意规则
                pass: 0, //通过策略
                passConfig: 0, //通过策略配置
                approve: [], //审核处理人
                addActivity: [], //加签处理人
            },
        }
    },
    {
        type: 'x-lane',
        text: { text: '横泳' },
        icon: 'column-width'
    },
    {
        type: 'y-lane',
        text: { text: '纵泳' },
        icon: 'column-height'
    },
    {
        type: 'end',
        text: { text: '结束' },
        icon: 'stop',
        props: {
            //基础属性
            base: {
                title: "结束",
                formId: undefined,
                formName: "",
            },
            notice: [], //通知
            event: [], //事件
        }
    }
]

export const highNodes = [{
    type: 'subprocess',
    text: { text: '子流程' },
    icon: 'apartment'
}]

export const laneNodes = []