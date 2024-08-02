import Vue from 'vue'
import KFormDesign from '@/pages/system/agile/form/components/packages/index.js'
Vue.use(KFormDesign)
import { nodeSchema } from "@/pages/system/agile/form/components/packages/utils/index";
// 引入自定义组件
import organization from "@/pages/system/agile/form/components/control/organization";
import icon from "@/pages/system/agile/form/components/control/icon";
import user from "@/pages/system/agile/form/components/control/user";
import sign from "@/pages/system/agile/form/components/control/sign";
import serialNo from "@/pages/system/agile/form/components/control/serialNo";
import district from "@/pages/system/agile/form/components/control/district";
import dataList from "@/pages/system/agile/form/components/control/dataList";
import correlationRecord from "@/pages/system/agile/form/components/control/correlationRecord";
import autoComplete from "@/pages/system/agile/form/components/control/autoComplete";
import dataShow from "@/pages/system/agile/form/components/control/dataShow";
import dictionary from "@/pages/system/agile/form/components/control/dictionary";
import map from "@/pages/system/agile/form/components/control/map";
nodeSchema.addSchemas(
    [{
            type: "organization", // 组件类型
            label: "组织架构", // 组件名称s
            cicon: "cluster",
            component: organization, // 组件
            options: {
                labelSize: 14,
                labelStyle: '',
                labelColor: '#000000',
                maxLength: 512,
                labelFamily: '',
                span: 24,
                size: "default",
                defaultValueType: undefined, // 默认值
                defaultValue: null,
                multiple: false, // 多选
                disabled: false, //是否禁用
                hidden: false, // 是否隐藏，false显示，true隐藏
                width: "100%", // 宽度
                clearable: true, // 可清除
                topOrg: true, //顶级部门
                checkStrictly: true, //父子节点选中状态不再关联
                chosen: [],
                placeholder: "请选择", // 占位内容
                showLabel: true,
            },
            model: "", // 数据字段
            key: "",
            rules: [
                // 校验规则
                {
                    required: false,
                    message: "必填项",
                },
            ],
            dataType: null, //数据类型
            null: true //为空
        },
        {
            type: "user", // 组件类型
            label: "人员选择", // 组件名称
            component: user, // 组件
            cicon: "user",
            options: {
                labelSize: 14,
                labelStyle: '',
                labelColor: '#000000',
                labelFamily: '',
                maxLength: 512,
                span: 24,
                size: "default",
                defaultValueType: undefined, // 默认值
                defaultValue: null,
                multiple: false, // 多选
                disabled: false, //是否禁用
                hidden: false, // 是否隐藏，false显示，true隐藏
                width: "100%", // 宽度
                clearable: true, // 可清除
                chosen: [],
                topOrg: true, //顶级部门
                specificOrg: '', //指定组织范围字段
                placeholder: "请选择", // 占位内容
                showLabel: true,
            },
            model: "", // 数据字段
            key: "",
            rules: [
                // 校验规则
                {
                    required: false,
                    message: "必填项",
                },
            ],
            dataType: null, //数据类型
            null: true //为空
        },
        {
            type: "correlationRecord",
            label: "关联记录",
            component: correlationRecord,
            cicon: "block",
            options: {
                labelSize: 14,
                labelStyle: '',
                labelColor: '#000000',
                labelFamily: '',
                maxLength: 512,
                span: 24,
                multiple: false, // 多选
                disabled: false, //是否禁用
                isPaging: true, //分页
                hidden: false, // 是否隐藏，false显示，true隐藏
                clearable: true, // 可清除
                placeholder: "请选择", // 占位内容
                showLabel: true,
                showType: 'select', //显示方式
                rowDoubleClickConfirm: false, //双击确定
                rowClickConfirm: false, //点击确定
                canAdd: true, //能够新增
                canDetail: true, //能够查看详情
                dialog: {
                    width: "600",
                    widthUnit: 'px',
                    type: 'modal',
                    centered: false,
                    closable: false,
                    maskClosable: true,
                    zIndex: 1000
                }, //弹出位置
                dynamicConfig: {
                    table: undefined, //工作表
                    condition: [], //条件
                    title: ''
                },
                filter: {
                    groupOp: "AND",
                    rules: [],
                    groups: []
                },
                columns: [], //显示列
                map: [], //数据映射,选择后会将映射数据值对应显示上
                search: {
                    labelCol: 8,
                    wrapperCol: 16,
                    num: 6, //显示个数
                    columns: []
                },
            },
            model: "", // 数据字段
            key: "",
            rules: [{
                required: false,
                message: "必填项",
            }],
            dataType: null, //数据类型
            null: true //为空
        },
        {
            type: "dataList",
            label: "数据选择",
            component: dataList,
            cicon: "book",
            options: {
                labelSize: 14,
                labelStyle: '',
                labelColor: '#000000',
                maxLength: 512,
                labelFamily: '',
                span: 24,
                multiple: true, // 多选
                disabled: false, //是否禁用
                isPaging: true, //分页
                hidden: false, // 是否隐藏，false显示，true隐藏
                clearable: true, // 可清除
                placeholder: "请选择", // 占位内容
                showLabel: true,
                rowDoubleClickConfirm: false, //双击确定
                rowClickConfirm: false, //点击确定
                dialog: {
                    width: "600",
                    widthUnit: 'px',
                    type: 'modal',
                    centered: false,
                    closable: false,
                    maskClosable: true,
                    zIndex: 1000
                }, //弹出位置
                dynamicConfig: {
                    dataSourceId: undefined, //数据源Id
                    inParams: [], //输入参数赋值
                    title: '',
                },
                columns: [], //显示列
                map: [], //数据映射,选择后会将映射数据值对应显示上
                search: {
                    labelCol: 8,
                    wrapperCol: 16,
                    num: 6, //显示个数
                    columns: []
                }, //查询条件
            },
            model: "", // 数据字段
            key: "",
            rules: [{
                required: false,
                message: "必填项",
            }],
            dataType: null, //数据类型
            null: true //为空
        },

        {
            type: "serialNo", // 组件类型
            label: "自动编号", // 组件名称
            cicon: "ordered-list",
            component: serialNo, // 组件
            options: {
                labelSize: 14,
                labelStyle: '',
                labelColor: '#000000',
                labelFamily: '',
                span: 24,
                maxLength: 16,
                length: 4, //位数
                incremental: true, //编号超出位数后继续递增
                beginNumber: 1, //开始值
                repeat: '', //重复
                format: 'yyyyMMdd', //日期格式
                hidden: false, // 是否隐藏，false显示，true隐藏
                rule: '', //表达式
                loadDisplay: false, //加载页面就显示
                userOccupation: false, //用户占用
            },
            model: "", // 数据字段
            key: "",
            rules: [
                // 校验规则
                {
                    required: false,
                    message: "必填项",
                },
            ],
            dataType: null, //数据类型
            null: true //为空
        },
        {
            type: "map", // 组件类型
            label: "位置选择", // 组件名称
            cicon: "environment",
            component: map, // 组件
            options: {
                labelSize: 14,
                labelStyle: '',
                labelColor: '#000000',
                labelFamily: '',
                span: 24,
                maxLength: 512,
                hidden: false, // 是否隐藏，false显示，true隐藏
                clearable: true, // 可清除
                disabled: false, //是否禁用
                placeholder: "请选择", // 占位内容
                dialog: {
                    width: "600",
                    widthUnit: 'px',
                    type: 'modal',
                    centered: false,
                    closable: false,
                    maskClosable: true,
                    zIndex: 1000
                },
                map: [], //数据映射,选择后会将映射数据值对应显示上
            },
            model: "", // 数据字段
            key: "",
            rules: [
                // 校验规则
                {
                    required: false,
                    message: "必填项",
                },
            ],
            dataType: null, //数据类型
            null: true //为空
        },
        {
            type: "district",
            label: "省市区",
            component: district,
            cicon: "block",
            options: {
                labelSize: 14,
                labelStyle: '',
                labelColor: '#000000',
                labelFamily: '',
                span: 24,
                size: "default",
                defaultValue: undefined, // 默认值
                //multiple: true, // 多选
                disabled: false, //是否禁用
                hidden: false, // 是否隐藏，false显示，true隐藏
                width: "100%", // 宽度
                clearable: true, // 可清除
                placeholder: "请选择", // 占位内容
                showLabel: true,
                levelType: 3 //类型1省，2省市，3省市区，4省市区乡镇
            },
            model: "", // 数据字段
            key: "",
            rules: [{
                required: false,
                message: "必填项",
            }],
            dataType: null, //数据类型
            null: true //为空
        },
        {
            type: "dictionary", // 表单类型
            label: "字典", // 标题文字
            cicon: "unordered-list",
            component: dictionary,
            options: {
                labelSize: 14,
                labelStyle: '',
                labelColor: '#000000',
                labelFamily: '',
                maxLength: 512,
                size: "default",
                span: 24,
                width: "100%", // 宽度
                defaultValue: undefined, // 下拉选框请使用undefined为默认值
                multiple: false, // 是否允许多选
                disabled: false, // 是否禁用
                clearable: true, // 是否显示清除按钮
                showLabel: true,
                hidden: false, // 是否隐藏，false显示，true隐藏
                defaultValueFirst: false,
                placeholder: "请选择", // 默认提示文字
                dictionaryId: undefined, //字典Id
                dictionaryShowType: 1, //字典显示形式,1下拉，2单选，3复选
                dictionaryLevel: 1, //层级,默认下一级
                showSearch: true // 是否显示搜索框，搜索选择的项的值，而不是文字
            },
            model: "",
            key: "",
            help: "",
            rules: [{
                required: false,
                message: "必填项"
            }],
            dataType: null, //数据类型
            null: true //为空
        },
        {
            type: "icon", // 组件类型
            label: "图标选择", // 组件名称
            cicon: "border",
            component: icon, // 组件
            options: {
                labelSize: 14,
                labelStyle: '',
                labelColor: '#000000',
                labelFamily: '',
                span: 24,
                size: "default",
                defaultValue: undefined, // 默认值
                disabled: false, //是否禁用
                hidden: false, // 是否隐藏，false显示，true隐藏
                width: "100%", // 宽度
                clearable: true, // 可清除
                placeholder: "请选择", // 占位内容
            },
            model: "", // 数据字段
            key: "",
            rules: [
                // 校验规则
                {
                    required: false,
                    message: "必填项",
                },
            ],
            dataType: null, //数据类型
            null: true //为空
        },
        {
            type: "sign",
            label: "手写签名",
            component: sign,
            cicon: "highlight",
            options: {
                labelSize: 14,
                labelStyle: '',
                labelColor: '#000000',
                labelFamily: '',
                span: 24,
                size: "default",
                defaultValue: undefined, // 默认值
                disabled: false, //是否禁用
                hidden: false, // 是否隐藏，false显示，true隐藏
                width: "100%", // 宽度
            },
            model: "", // 数据字段
            key: "",
            rules: [{
                required: false,
                message: "必填项",
            }],
            dataType: null, //数据类型
            null: true //为空
        },
        {
            type: "dataShow",
            label: "数据显示",
            component: dataShow,
            cicon: "font-size",
            options: {
                labelSize: 14,
                labelStyle: '',
                labelColor: '#000000',
                labelFamily: '',
                span: 24,
                maxLength: 512,
                hidden: false, // 是否隐藏，false显示，true隐藏
                width: "100", // 宽度
                defaultValue: "",
                showLabel: true,
                type: 'label', //显示类型：label（文本），img（图片），qrcode(二维码)，barcode(条形码)
                label: {
                    color: "#000",
                    fontFamily: "SimHei",
                    fontSize: "10.5pt"
                },
                img: {},
                qrCode: {
                    correctLevel: 1, //容错级别
                    size: 100, //尺寸, 长宽一致, 包含外边距
                    margin: 10, //二维码图像的外边距, 默认 20px
                    colorDark: "#000000", //实点的颜色
                    colorLight: "#FFFFFF", //空白区的颜色
                    bgSrc: undefined, //欲嵌入的背景图地址
                    gifBgSrc: "rgba(0,0,0,0)", //欲嵌入的背景图 gif 地址,设置后普通的背景图将失效。设置此选项会影响性能
                    backgroundColor: "#FFFFFF", //背景色
                    backgroundDimming: "rgba(255,255,255,1)", //背景色叠加在背景图上的颜色, 在解码有难度的时有一定帮助
                    logoSrc: undefined, //嵌入至二维码中心的 LOGO 地址
                    logoScale: 0.2, //用于计算 LOGO 大小的值, 过大将导致解码失败, LOGO 尺寸计算公式 logoScale*(size-2*margin), 默认 0.2
                    logoMargin: 0, //LOGO 标识周围的空白边框, 默认为0
                    logoBackgroundColor: "rgba(255,255,255,1)", //Logo 背景色,需要设置 logo margin
                    logoCornerRadius: 0, //LOGO 标识及其边框的圆角半径, 默认为0
                    whiteMargin: true, //若设为 true, 背景图外将绘制白色边框
                    dotScale: 1, //数据区域点缩小比例,默认为0.35
                    autoColor: false //若为 true, 图像将被二值化处理, 未指定阈值则使用默认值
                },
                barCode: {
                    format: "CODE39", //选择要使用的条形码类型
                    width: 3, //设置条之间的宽度
                    height: 50, //高度
                    displayValue: true, //是否在条形码下方显示文字
                    text: "456", //覆盖显示的文本
                    fontOptions: "bold italic", //使文字加粗体或变斜体
                    font: "fantasy", //设置文本的字体
                    textAlign: "left", //设置文本的水平对齐方式
                    textPosition: "top", //设置文本的垂直位置
                    textMargin: 5, //设置条形码和文本之间的间距
                    fontSize: 15, //设置文本的大小
                    background: "#eee", //设置条形码的背景
                    lineColor: "#2196f3", //设置条和文本的颜色。
                    margin: 15 //设置条形码周围的空白边距
                }
            },
            model: "", // 数据字段
            key: "",
            dataType: null, //数据类型
            null: true //为空
        },
        {
            type: "autoComplete", // 表单类型
            label: "输入补全", // 标题文字
            component: autoComplete,
            cicon: "font-colors",
            options: {
                labelSize: 14,
                labelStyle: '',
                labelColor: '#000000',
                labelFamily: '',
                size: "default",
                span: 24,
                type: "text",
                width: "100%", // 宽度
                defaultValue: undefined, // 默认值
                defaultValueType: undefined, //类型
                defaultValueBillNo: undefined, //规则编号
                defaultValueApiPath: undefined, //接口地址
                placeholder: "请输入", // 没有输入时，提示文字
                clearable: false,
                maxLength: 128,
                addonBefore: "",
                addonAfter: "",
                showLabel: true,
                hidden: false, // 是否隐藏，false显示，true隐藏
                disabled: false, // 是否禁用，false不禁用，true禁用
                only: false, //值唯一
                dynamic: true,
                dynamicConfig: {
                    dataSourceId: undefined, //数据源Id
                    inParams: [], //输入参数赋值
                    title: '',
                    key: undefined, //键
                    value: undefined, //值
                    sidx: undefined, //排序字段
                    sord: 'desc', //排序方式desc,asc
                },
            },
            model: "",
            key: "",
            help: "",
            rules: [{
                required: false,
                message: "必填项"
            }],
            dataType: null, //数据类型
            null: true //为空
        },
    ],
);

// 添加分组
nodeSchema.addSchemaGroup({
    title: "业务组件",
    list: ["organization", "icon", "user", "sign", "district", "dataShow", "dataList", "serialNo", "map", "correlationRecord", "dictionary", "autoComplete"]
});