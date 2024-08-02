import Vue from "vue";

/**
 * 结果代码
 */
const Resultcode = {
  success: 200,
  error: 500,
};
Vue.prototype.eipResultCode = Resultcode;

/**
 * 当前数据库类型
 */
Vue.prototype.eipDbType = "mysql";

/**
 * 空Guid
 */
Vue.prototype.eipEmptyGuid = "00000000-0000-0000-0000-000000000000";

const Msg = {
  delete: "删除后不可恢复,确认删除?",
  loading: "加载中,请稍等...",
  delloading: "正在删除中...",
  saveLoading: "保存中...",
  copyloading: "正在复制保存中...",
  buy: "当前版本未开放,请联系(QQ:1039318332)购买高级版本",
};

Vue.prototype.eipMsg = Msg;

/**
 * 权限类型
 */
const privilegeMaster = {
  role: 0, //角色
  organization: 1, //组织架构
  group: 2, //组
  post: 3, //岗位
  user: 4, //用户
};
Vue.prototype.eipPrivilegeMaster = privilegeMaster;

/**
 * 权限归属
 */
const privilegeAccess = {
  menu: 0, //菜单
  menubutton: 1, //模块按钮
  field: 2, //字段
  data: 3, //数据
  workflow: 4, //数据
};
Vue.prototype.eipPrivilegeAccess = privilegeAccess;

/**
 * 分页参数
 */
Vue.prototype.eipPage = {
  size: 20,
  sizeOptions: [
    "10",
    "20",
    "30",
    "50",
    "100",
    "150",
    "200",
    "500",
    "1000",
    "2000",
  ],
};

/**
 *敏捷开发设计对象
 */
Vue.prototype.eipAgileDesigner = {
  kfd: null,
};

/**
 * 生成表前缀
 */
Vue.prototype.eipTablePrefix = "";

/**
 * 获取减去顶部高度
 */
Vue.prototype.eipHeaderHeight = function () {
  var height = window.innerHeight;
  var setting = this.$store.state.setting;
  height -= setting.multiPage ? 91 : 50;
  return height;
};

/**
 * 工作流处理类型
 */
Vue.prototype.eipWorkflowDoType = {
  审核: 1,
  知会: 2,
  加签: 3,
  阅示: 4,
  流程监控: 5,
  阅示审批: 6,
};

/**
 * 敏捷开发菜单类型
 */
Vue.prototype.eipAgileMenuType = {
  group: 1,
  worksheet: 2,
  iframe: 3,
  portal: 4,
  list: 5,
  form: 6,
  page: 7,
};

/**
 * 按钮触发类型
 */
Vue.prototype.eipButtonTriggerType = {
  method: 1, //方法
  script: 2, //脚本
  api: 3, //api
  automation: 4, //自动化
  print: 5, //打印
  export: 6,
};

/**
 * 系统字段
 */
Vue.prototype.eipSystemColumns = [
  {
    name: "Id",
    description: "自增键",
    isShow: false,
    isSearch: true,
  },
  {
    name: "RelationId",
    description: "记录ID",
    isShow: false,
    isSearch: true,
  },
  {
    name: "CreateTime",
    description: "创建时间",
    isShow: true,
    isSearch: true,
  },
  {
    name: "CreateUserName",
    description: "创建人",
    isShow: true,
    isSearch: true,
  },
  {
    name: "UpdateTime",
    description: "修改时间",
    isShow: false,
    isSearch: false,
  },
  {
    name: "UpdateUserName",
    description: "修改人",
    isShow: false,
    isSearch: false,
  },
  {
    name: "CreateUserId",
    description: "创建人Id",
    isShow: false,
    isSearch: true,
  },
  {
    name: "CreateOrganizationId",
    description: "创建人组织机构Id",
    isShow: false,
    isSearch: false,
  },
  {
    name: "CreateOrganizationName",
    description: "创建人组织机构名称",
    isShow: false,
    isSearch: false,
  },
  {
    name: "UpdateUserId",
    description: "修改人Id",
    isShow: false,
    isSearch: false,
  },
  {
    name: "UpdateOrganizationId",
    description: "修改人组织机构Id",
    isShow: false,
    isSearch: false,
  },
  {
    name: "UpdateOrganizationName",
    description: "修改人组织机构名称",
    isShow: false,
    isSearch: false,
  },
];
/**
 * 确认类型
 */
Vue.prototype.eipButtonDataConfirmType = {
  none: 1, //方法
  single: 2, //单条
  multiple: 3, //多条
};
/**
 * 格式化
 */
Vue.prototype.eipFormat = [
  {
    value: "Image",
    label: "图片",
  },
  {
    value: "File",
    label: "文件",
  },
  {
    value: "Batch",
    label: "子表",
  },
  {
    value: "User",
    label: "人员",
  },
  {
    value: "Organization",
    label: "组织架构",
  },
  {
    value: "YYYY",
    label: "年",
  },
  {
    value: "YYYYMM",
    label: "年月",
  },
  {
    value: "YYYYMMDD",
    label: "年月日",
  },
  {
    value: "YYYYMMDDHH",
    label: "年月日时",
  },
  {
    value: "YYYYMMDDHHMM",
    label: "年月日时分",
  },
  {
    value: "YYYYMMDDHHMMSS",
    label: "年月日时分秒",
  },
  {
    value: "HHMMSS",
    label: "时分秒",
  },
  {
    value: "Map",
    label: "地图",
  },
  {
    value: "WebSite",
    label: "网址",
  },
  {
    value: "QrCode",
    label: "二维码",
  },
  {
    value: "BarCode",
    label: "条形码",
  },
  {
    value: "Label",
    label: "文字",
  },
  {
    value: "Rate",
    label: "评分",
  },
  {
    value: "District",
    label: "地区",
  },
  {
    value: "CorrelationRecord",
    label: "关联表",
  },
  {
    value: "Switch",
    label: "是否",
  },
  {
    value: "Dictionary",
    label: "字典",
  },
  {
    value: "TreeSelect",
    label: "树",
  },
];

/**
 * 格式化
 */
Vue.prototype.eipMask = [
  {
    value: "all",
    label: "全掩盖",
  },
  {
    value: "name",
    label: "姓名-显示前一个字,后一个字",
  },
  {
    value: "phone",
    label: "手机号-显示前3位,后4位",
  },
  {
    value: "email",
    label: "邮箱-显示前3位，@和之后的字",
  },
  {
    value: "money",
    label: "金额-全掩盖，虚拟为5位",
  },
  {
    value: "idcard",
    label: "身份证件-显示后4位",
  },
  {
    value: "address",
    label: "住址-显示前4个字，后4个字",
  },
  {
    value: "YYYYMMDD",
    label: "年月日",
  },
  {
    value: "carNo",
    label: "车牌号-显示前1个字，后2位",
  },
];

/**
 * 系统表单字典
 */
Vue.prototype.eipSystemFiled = {
  title: "表单字段",
  children: [
    {
      title: "记录ID",
      key: "RelationId",
      isLeaf: true,
    },
    {
      title: "创建用户Id",
      key: "CreateUserId",
      isLeaf: true,
    },
    {
      title: "创建用户名",
      key: "CreateUserName",
      isLeaf: true,
    },
    {
      title: "创建用户组织Id",
      key: "CreateOrganizationId",
      isLeaf: true,
    },
    {
      title: "创建用户组织名",
      key: "CreateOrganizationName",
      isLeaf: true,
    },
  ],
};
/**
 * 比较符对应类型
 */
(Vue.prototype.opControls = {
  input: [
    { value: "cn", title: "包含" },
    { value: "eq", title: "等于" },
    { value: "ne", title: "不等" },
    { value: "bw", title: "开始于" },
    { value: "bn", title: "不开始于" },
    { value: "ew", title: "结束于" },
    { value: "en", title: "不结束于" },
    { value: "nc", title: "不包含" },
    { value: "nu", title: "空值" },
    { value: "nn", title: "非空值" },
  ],
  select: [
    { value: "eq", title: "等于" },
    { value: "ne", title: "不等" },
  ],
  bool: [
    { value: "eq", title: "等于" },
    { value: "ne", title: "不等" },
  ],
  datetime: [
    { value: "date", title: "等于" },
    { value: "daterange", title: "之间" },
    { value: "lt", title: "小于" },
    { value: "le", title: "小于等于" },
    { value: "gt", title: "大于" },
    { value: "ge", title: "大于等于" },
  ],
  number: [
    { value: "eq", title: "等于" },
    { value: "lt", title: "小于" },
    { value: "le", title: "小于等于" },
    { value: "gt", title: "大于" },
    { value: "ge", title: "大于等于" },
  ],
}),
  /**
   * 系统配置
   */
  (Vue.prototype.eipConfig = {
    loginCaptcha: false, //登录验证码
  });

/**
 * 常用正则表达式
 * 若有\需要写2个\\进行转义
 */
Vue.prototype.eipPattern = [
  {
    title: "email",
    pattern: "^\\w+([-+.]\\w+)*@\\w+([-.]\\w+)*\\.\\w+([-.]\\w+)*$",
  },
  { title: "手机号码", pattern: "^1[34578]\\d{9}$" },
  {
    title: "身份证号码",
    pattern: "(^\\d{15}$)|(^\\d{18}$)|(^\\d{17}(\\d|X|x)$)",
  },

  { title: "数字", pattern: "^[0-9]*$" },
  { title: "n位的数字", pattern: "^\\d{n}$" },
  { title: "至少n位的数字", pattern: "^\\d{n,}$" },
  { title: "m-n位的数字", pattern: "^\\d{m,n}$" },
  { title: "零和非零开头的数字", pattern: "^(0|[1-9][0-9]*)$" },
  {
    title: "非零开头的最多带两位小数的数字",
    pattern: "^([1-9][0-9]*)+(.[0-9]{1,2})?$",
  },
  { title: "带1-2位小数的正数或负数", pattern: "^(\\-)?\\d+(\\.\\d{1,2})?$" },
  { title: "正数、负数、和小数", pattern: "^(\\-|\\+)?\\d+(\\.\\d+)?$" },
  { title: "有两位小数的正实数", pattern: "^[0-9]+(.[0-9]{2})?$" },
  { title: "有1~3位小数的正实数", pattern: "^[0-9]+(.[0-9]{1,3})?$" },
  { title: "非零的正整数", pattern: "^[1-9]\\d*$" },
  { title: "非零的负整数", pattern: '^\\-[1-9][]0-9"*$ ' },
  { title: "非负整数", pattern: "^\\d+$" },
  { title: "非正整数", pattern: "^-[1-9]\\d*|0$" },
  { title: "非负浮点数", pattern: "^\\d+(\\.\\d+)?$" },
  { title: "非正浮点数", pattern: "^((-\\d+(\\.\\d+)?)|(0+(\\.0+)?))$" },
  { title: "正浮点数", pattern: "^[1-9]\\d*\\.\\d*|0\\.\\d*[1-9]\\d*$" },
  { title: "负浮点数", pattern: "^-([1-9]\\d*\\.\\d*|0\\.\\d*[1-9]\\d*)$" },
  { title: "浮点数", pattern: "^(-?\\d+)(\\.\\d+)?$" },
];

/**
 * 列表配置
 */
Vue.prototype.eipTableConfig = {
  columns: [], //表格控件
  search: {
    num: 4, //显示个数
    searchButton: false,
    columns: [], //查询条件
  },
  filter: [], //筛选条件
  button: [], //按钮
  style: {
    rowHeight: 40,
    paging: true,
    columnIsCurrent: true,
    rowIsCurrent: true,
    select: "checkbox",
    border: "full",
    filter: undefined,
    filterWidth: 100,
    filterMultiple: false,
    filterExpandAll: true,
    filterShowLine: false,
    subtableLayout: "tree",
    subtableLayoutPercentage: 50,
    showTree: false,
    showTreeLine: false,
    showTreeAccordion: false,
    showTreeExpandAll: false,
    showTreeParent: undefined,
    showTreeSon: undefined,
  },
  phone: {
    style: 1, //卡片模版
    title: undefined, //标题
    subTitle: undefined, //摘要
    columns: [], //显示字段
    cover: undefined, //封面
    coverPosition: undefined, //显示位置
    coverShowType: undefined, //显示方式
  },
  action: {
    cellDbClick: false, //双击
  },
  read: {
    cache: true, //读取缓存
    refresh: "",
  },
  subtable: [], //子表
  controls: [], //所有控件
};

/**
 * 组织性质
 */
Vue.prototype.eipOrganizationNatures = [
  {
    value: 0,
    name: "集团",
  },
  {
    value: 1,
    name: "公司",
  },
  {
    value: 2,
    name: "分公司",
  },
  {
    value: 3,
    name: "子公司",
  },
  {
    value: 4,
    name: "部门",
  },
];
