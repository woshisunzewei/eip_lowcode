/*
 * author kcz
 * date 2019-11-20
 * description 表单控件项
 */

// 内置控件
export const defaultSchemaList = [
  {
    type: "input", // 表单类型
    label: "输入框", // 标题文字
    icon: "icon-write",
    options: {
      labelSize: 14,
      labelStyle: "",
      labelColor: "#000000",
      labelFamily: "",
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
    },
    model: "", // 数据字段
    key: "",
    help: "",
    rules: [
      //验证规则
      {
        required: false, // 必须填写
        message: "必填项",
      },
    ],
    dataType: null, //数据类型
    null: true, //为空, //数据类型
  },
  {
    type: "textarea", // 表单类型
    label: "文本框", // 标题文字
    icon: "icon-edit",
    options: {
      labelSize: 14,
      labelStyle: "",
      labelColor: "#000000",
      labelFamily: "",
      span: 24,
      width: "100%", // 宽度
      minRows: 4,
      maxRows: 6,
      maxLength: 512,
      defaultValue: "",
      clearable: false,
      showLabel: true,
      hidden: false, // 是否隐藏，false显示，true隐藏
      disabled: false,
      placeholder: "请输入",
    },
    model: "", // 数据字段
    key: "",
    help: "",
    rules: [
      {
        required: false,
        message: "必填项",
      },
    ],
    dataType: null, //数据类型
    null: true, //为空
  },
  {
    type: "number", // 表单类型
    label: "数字输入框", // 标题文字
    icon: "icon-number",
    options: {
      labelSize: 14,
      labelStyle: "",
      labelColor: "#000000",
      labelFamily: "",
      size: "default",
      span: 24,
      width: "100%", // 宽度
      defaultValue: 0, // 默认值
      min: 0, // 可输入最小值
      max: 9999999999, // 可输入最大值
      precision: null,
      step: 1, // 步长，点击加减按钮时候，加减多少
      showLabel: true,
      hidden: false, // 是否隐藏，false显示，true隐藏
      disabled: false, //是否禁用
      placeholder: "请输入",
    },
    model: "", // 数据字段
    key: "",
    help: "",
    rules: [
      {
        required: false,
        message: "必填项",
      },
    ],
    dataType: null, //数据类型
    null: true, //为空
  },
  {
    type: "select", // 表单类型
    label: "下拉选择器", // 标题文字
    icon: "icon-xiala",
    options: {
      labelSize: 14,
      labelStyle: "",
      labelColor: "#000000",
      labelFamily: "",
      maxLength: 1024,
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
      dynamicKey: "",
      dynamic: false,
      dynamicConfig: {
        dataSourceId: undefined, //数据源Id
        inParams: [], //输入参数赋值
        title: "",
        key: undefined, //键
        value: undefined, //值
        sidx: undefined, //排序字段
        sord: "desc", //排序方式desc,asc
        map: [], //数据映射,选择后会将映射数据值对应显示上
      },
      options: [
        {
          value: "1",
          label: "下拉框1",
        },
        {
          value: "2",
          label: "下拉框2",
        },
      ],
      showSearch: true, // 是否显示搜索框，搜索选择的项的值，而不是文字
    },
    model: "",
    key: "",
    help: "",
    rules: [
      {
        required: false,
        message: "必填项",
      },
    ],
    dataType: null, //数据类型
    null: true, //为空
  },
  {
    type: "checkbox",
    label: "多选框",
    icon: "icon-duoxuan1",
    options: {
      labelSize: 14,
      labelStyle: "",
      labelColor: "#000000",
      labelFamily: "",
      span: 24,
      width: "100%", // 宽度
      disabled: false, //是否禁用
      showLabel: true,
      hidden: false, // 是否隐藏，false显示，true隐藏
      defaultValue: [],
      dynamicKey: "",
      dynamicConfig: {
        dataSourceId: undefined, //数据源Id
        inParams: [], //输入参数赋值
        title: "",
        key: undefined, //键
        value: undefined, //值
        sidx: undefined, //排序字段
        sord: "desc", //排序方式desc,asc
        map: [], //数据映射,选择后会将映射数据值对应显示上
      },
      dynamic: false,
      options: [
        {
          value: "1",
          label: "选项1",
        },
        {
          value: "2",
          label: "选项2",
        },
        {
          value: "3",
          label: "选项3",
        },
      ],
    },
    model: "",
    key: "",
    help: "",
    rules: [
      {
        required: false,
        message: "必填项",
      },
    ],

    dataType: null, //数据类型
    null: true, //为空
  },
  {
    type: "radio", // 表单类型
    label: "单选框", // 标题文字
    icon: "icon-danxuan-cuxiantiao",
    options: {
      labelSize: 14,
      labelStyle: "",
      labelColor: "#000000",
      labelFamily: "",
      span: 24,
      width: "100%", // 宽度
      disabled: false, //是否禁用
      showLabel: true,
      hidden: false, // 是否隐藏，false显示，true隐藏
      defaultValue: "", // 默认值
      dynamicConfig: {
        dataSourceId: undefined, //数据源Id
        inParams: [], //输入参数赋值
        title: "",
        key: undefined, //键
        value: undefined, //值
        sidx: undefined, //排序字段
        sord: "desc", //排序方式desc,asc
        map: [], //数据映射,选择后会将映射数据值对应显示上
      },
      dynamicKey: "",
      dynamic: false,
      options: [
        {
          value: "1",
          label: "选项1",
        },
        {
          value: "2",
          label: "选项2",
        },
        {
          value: "3",
          label: "选项3",
        },
      ],
    },
    model: "",
    key: "",
    help: "",
    rules: [
      {
        required: false,
        message: "必填项",
      },
    ],

    dataType: null, //数据类型
    null: true, //为空
  },
  {
    type: "date", // 表单类型
    label: "日期选择框", // 标题文字
    icon: "icon-calendar",
    options: {
      labelSize: 14,
      labelStyle: "",
      labelColor: "#000000",
      labelFamily: "",
      span: 24,
      width: "100%", // 宽度
      defaultValueDate: "", // 默认值，字符串 12:00:00
      rangeDefaultValueDate: ["", ""], // 默认值，字符串 12:00:00
      range: false, // 范围日期选择，为true则会显示两个时间选择框（同时defaultValue和placeholder要改成数组），
      showTime: false, // 是否显示时间选择器
      disabled: false, // 是否禁用
      showLabel: true,
      hidden: false, // 是否隐藏，false显示，true隐藏
      clearable: false, // 是否显示清除按钮
      placeholder: "请选择",
      rangePlaceholder: ["开始时间", "结束时间"],
      format: "YYYY-MM-DD", // 展示格式  （请按照这个规则写 YYYY-MM-DD HH:mm:ss，区分大小写）
      size: "default",
      disabledDateNow: false, // 设置当前之前日期不可点击
      disabledTimeNow: false, // 设置当前之前时间不可点击
    },
    model: "",
    key: "",
    help: "",
    rules: [
      {
        required: false,
        message: "必填项",
      },
    ],

    dataType: null, //数据类型
    null: true, //为空
  },
  {
    type: "time", // 表单类型
    label: "时间选择框", // 标题文字
    icon: "icon-time",
    options: {
      labelSize: 14,
      labelStyle: "",
      labelColor: "#000000",
      labelFamily: "",
      span: 24,
      width: "100%", // 宽度
      defaultValue: "", // 默认值，字符串 12:00:00
      disabled: false, // 是否禁用
      showLabel: true,
      hidden: false, // 是否隐藏，false显示，true隐藏
      clearable: false, // 是否显示清除按钮
      placeholder: "请选择",
      format: "HH:mm:ss", // 展示格式
      size: "default",
    },
    model: "",
    key: "",
    help: "",
    rules: [
      {
        required: false,
        message: "必填项",
      },
    ],
    dataType: null, //数据类型
    null: true, //为空
  },
  {
    type: "rate", // 表单类型
    label: "评分", // 标题文字
    icon: "icon-pingfen_moren",
    options: {
      labelSize: 14,
      labelStyle: "",
      labelColor: "#000000",
      labelFamily: "",
      span: 24,
      width: "100%", // 宽度
      defaultValue: 0,
      max: 5, // 最大值
      disabled: false, // 是否禁用
      showLabel: true,
      hidden: false, // 是否隐藏，false显示，true隐藏
      allowHalf: false, // 是否允许半选
    },
    model: "",
    key: "",
    help: "",
    rules: [
      {
        required: false,
        message: "必填项",
      },
    ],
    dataType: null, //数据类型
    null: true, //为空
  },
  {
    type: "slider", // 表单类型
    label: "滑动输入条", // 标题文字
    icon: "icon-menu",
    options: {
      labelSize: 14,
      labelStyle: "",
      labelColor: "#000000",
      labelFamily: "",
      span: 24,
      width: "100%", // 宽度
      defaultValue: 0, // 默认值， 如果range为true的时候，则需要改成数组,如：[12,15]
      disabled: false, // 是否禁用
      showLabel: true,
      hidden: false, // 是否隐藏，false显示，true隐藏
      min: 0, // 最小值
      max: 100, // 最大值
      step: 1, // 步长，取值必须大于 0，并且可被 (max - min) 整除
      showInput: false, // 是否显示输入框，range为true时，请勿开启
      // range: false // 双滑块模式
      vertical: false, //值为 true 时，Slider 为垂直方向
      //marks: [1, 100], //刻度标记，key 的类型必须为 number 且取值在闭区间 [min, max] 内，每个标签可以单独设置样式
      // range: false // 双滑块模式
    },
    model: "",
    key: "",
    help: "",
    rules: [
      {
        required: false,
        message: "必填项",
      },
    ],
    dataType: null, //数据类型
    null: true, //为空
  },
  {
    type: "uploadFile", // 表单类型
    label: "上传文件", // 标题文字
    icon: "icon-upload",
    options: {
      labelSize: 14,
      labelStyle: "",
      labelColor: "#000000",
      labelFamily: "",
      span: 24,
      defaultValue: "[]",
      maxLength: 1024,
      multiple: false,
      disabled: false,
      showLabel: true,
      relationModel: true,
      hidden: false,
      drag: false,
      downloadWay: "a",
      dynamicFun: "",
      width: "100%",
      limit: 3,
      data: "{}",
      fileName: "file",
      headers: {},
      action: "",
      remove: "",
      placeholder: "上传",
      accept: ".docx,.xlsx,.pdf,image/*",
    },
    model: "",
    key: "",
    help: "",
    rules: [
      {
        required: false,
        message: "必填项",
      },
    ],
    dataType: null, //数据类型
    null: true, //为空
  },
  {
    type: "uploadImg",
    label: "上传图片",
    icon: "icon-image",
    options: {
      labelSize: 14,
      labelStyle: "",
      labelColor: "#000000",
      labelFamily: "",
      span: 24,
      maxLength: 1024,
      defaultValue: "[]",
      multiple: false,
      showLabel: true,
      hidden: false,
      relationModel: true,
      disabled: false,
      width: "100%",
      data: "{}",
      limit: 3,
      placeholder: "上传",
      fileName: "image",
      headers: {},
      action: "",
      remove: "",
      listType: "picture-card",
    },
    model: "",
    key: "",
    help: "",
    rules: [
      {
        required: false,
        message: "必填项",
      },
    ],
    dataType: null, //数据类型
    null: true, //为空
  },
  {
    type: "treeSelect", // 表单类型
    label: "树选择器", // 标题文字
    icon: "icon-tree",
    options: {
      labelSize: 14,
      labelStyle: "",
      labelColor: "#000000",
      labelFamily: "",
      span: 24,
      disabled: false, //是否禁用
      defaultValue: undefined, // 默认值
      multiple: false,
      showLabel: true,
      hidden: false, // 是否隐藏，false显示，true隐藏
      clearable: false, // 是否显示清除按钮
      showSearch: true, // 是否显示搜索框，搜索选择的项的值，而不是文字
      treeCheckable: false,
      placeholder: "请选择",
      dynamicKey: "",
      dynamic: false,
      options: [],
      dynamicConfig: {
        dataSourceId: undefined, //数据源Id
        inParams: [], //输入参数赋值
        title: "",
        key: undefined, //键
        value: undefined, //值
        parentId: undefined, //父级
        sidx: undefined, //排序字段
        sord: "desc", //排序方式desc,asc
        map: [], //数据映射,选择后会将映射数据值对应显示上
      },
    },
    model: "",
    key: "",
    help: "",
    rules: [
      {
        required: false,
        message: "必填项",
      },
    ],
    dataType: null, //数据类型
    null: true, //为空
  },
  {
    type: "cascader", // 表单类型
    label: "级联选择器", // 标题文字
    icon: "icon-guanlian",
    options: {
      labelSize: 14,
      labelStyle: "",
      labelColor: "#000000",
      labelFamily: "",
      span: 24,
      disabled: false, //是否禁用
      showLabel: true,
      hidden: false, // 是否隐藏，false显示，true隐藏
      defaultValue: undefined, // 默认值
      showSearch: true, // 是否显示搜索框，搜索选择的项的值，而不是文字
      placeholder: "请选择",
      clearable: false, // 是否显示清除按钮
      dynamicKey: "",
      dynamic: false,
      options: [],
      dynamicConfig: {
        dataSourceId: undefined, //数据源Id
        inParams: [], //输入参数赋值
        title: "",
        key: undefined, //键
        value: undefined, //值
        parentId: undefined, //父级
        sidx: undefined, //排序字段
        sord: "desc", //排序方式desc,asc
        map: [], //数据映射,选择后会将映射数据值对应显示上
      },
    },
    model: "",
    key: "",
    help: "",
    rules: [
      {
        required: false,
        message: "必填项",
      },
    ],
    dataType: null, //数据类型
    null: true, //为空
  },
  {
    type: "batch",
    label: "动态表格",
    icon: "icon-biaoge",
    list: [],
    options: {
      labelSize: 14,
      labelStyle: "",
      labelColor: "#000000",
      labelFamily: "",
      span: 24,
      scrollY: 300,
      disabled: false,
      hidden: false, // 是否隐藏，false显示，true隐藏
      showLabel: false,
      hideSequence: false,
      multiple: true, // 是否允许多选
      copy: true, //复制
      delete: true, //删除
      width: "100%",
      size: "default",
      selectRows: [], //选择数据
      showFooter: false, //显示底部
      footerName: "合计", //底部显示名称
      footerOptions: [], //底部显示配置
      canDrag: false, //能否拖动
      buttons: [
        {
          label: "按钮",
          type: "primary", //类型
          icon: "plus",
          isShow: true, //图标
          trigger: {
            type: "add",
            option: {
              dynamicConfig: {
                dataSourceId: undefined, //数据源Id
                inParams: [], //输入参数赋值
                title: "",
              },
              columns: [], //显示列
              map: [], //数据映射
              search: {
                labelCol: 8,
                wrapperCol: 16,
                num: 4, //显示个数
                columns: [],
              }, //查询条件
              height: 600,
              width: 900,
              centered: false,
              isPaging: false,
              multiple: true,
            },
          },
        },
      ],
    },
    rules: [
      {
        required: false,
        message: "必填项",
      },
    ],
    model: "",
    key: "",
    help: "",
  },
  {
    type: "selectInputList",
    label: "选择输入列",
    icon: "icon-biaoge",
    columns: [{ label: "选项1", value: 1, list: [] }],
    options: {
      labelSize: 14,
      labelStyle: "",
      labelColor: "#000000",
      labelFamily: "",
      span: 24,
      disabled: false,
      multiple: true, // 是否允许多选
      hidden: false, // 是否隐藏，false显示，true隐藏
      showLabel: false,
      width: "100%",
    },
    model: "",
    key: "",
    help: "",
    dataType: null, //数据类型
    null: true, //为空
  },
  {
    type: "editor",
    label: "富文本",
    icon: "icon-LC_icon_edit_line_1",
    list: [],
    options: {
      labelSize: 14,
      labelStyle: "",
      labelColor: "#000000",
      labelFamily: "",
      span: 24,
      height: 300,
      placeholder: "请输入",
      defaultValue: "",
      chinesization: true,
      hidden: false, // 是否隐藏，false显示，true隐藏
      disabled: false,
      showLabel: false,
      width: "100%",
    },
    model: "",
    key: "",
    help: "",
    rules: [
      {
        required: false,
        message: "必填项",
      },
    ],
    dataType: null, //数据类型
    null: true, //为空
  },
  {
    type: "switch", // 表单类型
    label: "开关", // 标题文字
    icon: "icon-kaiguan3",
    options: {
      labelSize: 14,
      labelStyle: "",
      labelColor: "#000000",
      labelFamily: "",
      showLabel: true,
      span: 24,
      defaultValue: false, // 默认值 Boolean 类型
      hidden: false, // 是否隐藏，false显示，true隐藏
      disabled: false, // 是否禁用
      txt: {
        yes: "是", //"是"显示字（子表使用）
        no: "否", //"否"显示字（子表使用）
      },
    },
    model: "",
    key: "",
    help: "",
    rules: [
      {
        required: false,
        message: "必填项",
      },
    ],
    dataType: null, //数据类型
    null: true, //为空
  },
  {
    type: "button", // 表单类型
    label: "按钮", // 标题文字
    icon: "icon-button-remove",
    options: {
      span: 24,
      size: "default",
      type: "primary",
      handle: "submit",
      dynamicFun: "",
      showLabel: false,
      hidden: false, // 是否隐藏，false显示，true隐藏
      disabled: false, // 是否禁用，false不禁用，true禁用
    },
    key: "",
  },
  {
    type: "alert",
    label: "警告提示",
    icon: "icon-zu",
    options: {
      span: 24,
      type: "success",
      description: "",
      showIcon: false,
      banner: false,
      showLabel: false,
      hidden: false, // 是否隐藏，false显示，true隐藏
      closable: false,
    },
    key: "",
  },
  {
    type: "text",
    label: "文字",
    icon: "icon-zihao",
    options: {
      span: 24,
      textAlign: "left",
      hidden: false, // 是否隐藏，false显示，true隐藏
      showRequiredMark: false,
      noFormItem: true,
      color: "rgba(0, 0, 0, 0.9)",
      fontFamily: "",
      fontSize: "10.5pt",
      fontWeight: "normal",
    },
    key: "",
  },
  {
    type: "html",
    label: "HTML",
    icon: "icon-ai-code",
    options: {
      labelSize: 14,
      labelStyle: "",
      labelColor: "#000000",
      labelFamily: "",
      span: 24,
      noFormItem: true,
      hidden: false, // 是否隐藏，false显示，true隐藏
      defaultValue: "<strong>HTML</strong>",
    },
    key: "",
  },
  {
    type: "divider",
    label: "分割线",
    icon: "icon-fengexian",
    options: {
      span: 24,
      style: 5, //样式
      backgroundColor: "#1890ff",
      titleColor: "#ffffff",
      remark: "",
      orientation: "left",
      noFormItem: true,
    },
    key: "",
    model: "",
  },
  {
    type: "card",
    label: "卡片布局",
    icon: "icon-qiapian",
    list: [],
    options: {
      noFormItem: true,
      span: 24,
      size: "small",
      marginTop: 10,
      titleIcon: "", //标题图标
      hoverable: true, //鼠标移过时可浮起
      bordered: true, //显示边框
      hidden: false, // 是否隐藏，false显示，true隐藏
    },
    key: "",
    model: "",
  },
  {
    type: "tabs",
    label: "标签页布局",
    icon: "icon-tabs",
    options: {
      span: 24,
      tabBarGutter: null,
      type: "line",
      tabPosition: "top",
      size: "default",
      noFormItem: true,
      animated: true,
    },
    columns: [{ label: "标签1", value: 1, list: [] }],
    key: "",
    model: "",
  },
  {
    type: "collapse",
    label: "折叠面板布局",
    cicon: "pic-right",
    options: {
      span: 24,
      bordered: true,
      accordion: false,
      noFormItem: true,
      showArrow: true,
      expandIconPosition: "left",
      defaultType: "0",
    },
    columns: [{ label: "选项1", value: 1, list: [] }],
    key: "",
    model: "",
  },
  {
    type: "grid",
    label: "栅格布局",
    icon: "icon-zhage",
    columns: [
      {
        span: 12,
        list: [],
      },
      {
        span: 12,
        list: [],
      },
    ],
    options: {
      span: 24,
      hidden: false, // 是否隐藏，false显示，true隐藏
      noFormItem: true,
      gutter: 0,
    },
    key: "",
    model: "",
  },
  {
    type: "table",
    label: "表格布局",
    icon: "icon-biaoge",
    trs: [
      {
        tds: [
          {
            colspan: 1,
            rowspan: 1,
            style: "",
            list: [],
          },
          {
            colspan: 1,
            rowspan: 1,
            style: "",
            list: [],
          },
        ],
      },
      {
        tds: [
          {
            colspan: 1,
            rowspan: 1,
            style: "",
            list: [],
          },
          {
            colspan: 1,
            rowspan: 1,
            style: "",
            list: [],
          },
        ],
      },
    ],
    options: {
      span: 24,
      labelBold: false,
      bordered: true,
      bright: false,
      small: true,
      customStyle: "",
      hidden: false,
    },
    key: "",
    model: "",
  },
];
