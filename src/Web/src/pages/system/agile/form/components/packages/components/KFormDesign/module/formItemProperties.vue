<template>
  <div class="properties-centent kk-checkbox">
    <div class="properties-body">
      <eip-empty
        class="hint-box"
        v-show="selectItem.key === ''"
        description="未选择控件"
      />

      <a-form v-show="selectItem.key !== ''">
        <a-form-item v-if="isDefined(selectItem.label)" label="标签">
          <Input
            @change="labelChange"
            v-model="selectItem.label"
            placeholder="请输入"
          />
        </a-form-item>

        <a-form-item
          v-if="!hideModel && isDefined(selectItem.model)"
          label="字段名称"
        >
          <Input v-model="selectItem.model" placeholder="请输入" />
        </a-form-item>
        <!-- input type start -->
        <a-form-item v-if="selectItem.type === 'input'" label="输入框type">
          <Input v-model="options.type" placeholder="请输入" />
        </a-form-item>
        <!-- input type end -->
        <a-form-item
          v-if="
            typeof options.rangePlaceholder !== 'undefined' && options.range
          "
          label="占位内容"
        >
          <Input placeholder="请输入" v-model="options.rangePlaceholder[0]" />
          <Input placeholder="请输入" v-model="options.rangePlaceholder[1]" />
        </a-form-item>

        <a-form-item
          v-else-if="isDefined(options.placeholder)"
          label="占位内容"
        >
          <Input placeholder="请输入" v-model="options.placeholder" />
        </a-form-item>
        <a-form-item
          v-if="selectItem.type === 'textarea'"
          label="自适应内容高度"
        >
          <InputNumber
            class="eip-width-full"
            v-model="options.minRows"
            placeholder="最小高度"
          />
          <InputNumber
            class="eip-width-full"
            v-model="options.maxRows"
            placeholder="最大高度"
          />
        </a-form-item>
        <a-form-item v-if="isDefined(options.width)" label="宽度">
          <Input placeholder="请输入" v-model="options.width" />
        </a-form-item>
        <a-form-item v-if="isDefined(options.height)" label="高度">
          <InputNumber v-model="options.height" />
        </a-form-item>

        <!-- EIP -->
        <KDataList v-if="selectItem.type === 'dataList'" v-model="options" />
        <KDataShow
          v-if="selectItem.type === 'dataShow'"
          v-model="options"
          :familyOptions="familyOptions"
          :sizeOptions="sizeOptions"
        />
        <KCorrelationRecord
          v-if="selectItem.type === 'correlationRecord'"
          v-model="options"
        />

        <KDictionary
          v-if="selectItem.type === 'dictionary'"
          v-model="options"
        />

        <KSerialNo v-if="selectItem.type === 'serialNo'" v-model="options" />

        <a-form-item v-if="isDefined(options.step)" label="步长">
          <InputNumber
            class="eip-width-full"
            v-model="options.step"
            placeholder="请输入"
          />
        </a-form-item>
        <a-form-item v-if="isDefined(options.min)" label="最小值">
          <InputNumber
            class="eip-width-full"
            v-model="options.min"
            placeholder="请输入"
          />
        </a-form-item>
        <a-form-item v-if="isDefined(options.max)" label="最大值">
          <InputNumber
            class="eip-width-full"
            v-model="options.max"
            placeholder="请输入"
          />
        </a-form-item>

        <a-form-item v-if="isDefined(options.maxLength)" label="最大长度">
          <InputNumber
            class="eip-width-full"
            v-model="options.maxLength"
            placeholder="请输入"
          />
        </a-form-item>
        <a-form-item
          v-if="
            isDefined(options.minLimit) || ['batch'].includes(selectItem.type)
          "
          label="最小行数"
        >
          <InputNumber
            class="eip-width-full"
            v-model="options.minLimit"
            :min="0"
            placeholder="请输入"
          />
        </a-form-item>
        <a-form-item v-if="isDefined(options.tabBarGutter)" label="标签间距">
          <InputNumber
            class="eip-width-full"
            v-model="options.tabBarGutter"
            placeholder="请输入"
          />
        </a-form-item>
        <a-form-item v-if="isDefined(options.precision)" label="数值精度">
          <InputNumber
            class="eip-width-full"
            :min="0"
            :max="50"
            v-model="options.precision"
            placeholder="请输入"
          />
        </a-form-item>
        <a-form-item v-if="isDefined(options.dictCode)" label="dictCode">
          <Input v-model="options.dictCode" />
        </a-form-item>
        <!-- 选项配置及动态数据配置 start -->
        <a-form-item v-if="isDefined(options.options)" label="选项配置">
          <Radio buttonStyle="solid" v-model="options.dynamic">
            <RadioButton :value="false">静态数据</RadioButton>
            <RadioButton :value="true">动态数据</RadioButton>
          </Radio>
          <KChangeOption v-if="!options.dynamic" v-model="options.options" />
        </a-form-item>

        <!-- EIP -->
        <KDataSource
          v-if="options.dynamic"
          v-model="options"
          :type="selectItem.type"
        />
        <KMap v-if="selectItem.type == 'map'" v-model="options"></KMap>

        <a-form-item v-if="['district'].includes(selectItem.type)" label="类型">
          <Radio buttonStyle="solid" v-model="options.levelType">
            <RadioButton :value="1">省</RadioButton>
            <RadioButton :value="2">市</RadioButton>
            <RadioButton :value="3">区县</RadioButton>
            <RadioButton :value="4">乡镇</RadioButton>
          </Radio>
        </a-form-item>

        <!-- 选项配置及动态数据配置 end -->
        <!-- tabs配置 start -->
        <a-form-item
          v-if="
            ['tabs', 'selectInputList', 'collapse'].includes(selectItem.type)
          "
          :label="selectItem.type === 'tabs' ? '页签配置' : '列选项配置'"
        >
          <KChangeOption v-model="selectItem.columns" type="tab" />
        </a-form-item>
        <!-- tabs配置 end -->
        <a-form-item v-if="selectItem.type === 'grid'" label="栅格间距">
          <InputNumber
            class="eip-width-full"
            v-model="selectItem.options.gutter"
            placeholder="请输入"
          />
        </a-form-item>
        <a-form-item v-if="selectItem.type === 'grid'" label="列配置项">
          <KChangeOption v-model="selectItem.columns" type="colspan" />
        </a-form-item>

        <a-form-item v-if="selectItem.type === 'switch'" label="默认值">
          <ASwitch v-model="options.defaultValue" />
        </a-form-item>
        <a-form-item
          v-if="['number', 'slider'].indexOf(selectItem.type) >= 0"
          label="默认值"
        >
          <InputNumber
            class="eip-width-full"
            :step="options.step"
            :min="options.min || -Infinity"
            :max="options.max || Infinity"
            v-model="options.defaultValue"
          />
        </a-form-item>
        <a-form-item v-if="selectItem.type === 'rate'" label="默认值">
          <Rate
            v-model="options.defaultValue"
            :allowHalf="options.allowHalf"
            :count="options.max"
          />
        </a-form-item>
        <a-form-item
          v-if="selectItem.type === 'select' && !options.dynamic"
          label="默认值"
        >
          <Select
            :options="options.options"
            v-model="options.defaultValue"
            :allowClear="options.clearable"
            :mode="options.multiple ? 'multiple' : ''"
          />
        </a-form-item>
        <a-form-item v-if="selectItem.type === 'radio'" label="默认值">
          <Radio :options="options.options" v-model="options.defaultValue" />
        </a-form-item>
        <a-form-item v-if="selectItem.type === 'checkbox'" label="默认值">
          <Checkbox :options="options.options" v-model="options.defaultValue" />
        </a-form-item>
        <!-- 日期选择器默认值 start -->
        <a-form-item v-if="selectItem.type === 'date'" label="默认值">
          <a-select
            v-if="!options.range"
            v-model="options.defaultValueDate"
            placeholder="默认值"
            allowClear
          >
            <a-select-option value="now">当前时间</a-select-option>
          </a-select>

          <a-select
            v-if="options.range"
            v-model="options.rangeDefaultValueDate[0]"
            placeholder="默认值"
            allowClear
          >
            <a-select-option value="now">当前时间</a-select-option>
          </a-select>

          <a-select
            v-if="options.range"
            v-model="options.rangeDefaultValueDate[1]"
            placeholder="默认值"
            allowClear
          >
            <a-select-option value="now">当前时间</a-select-option>
          </a-select>
        </a-form-item>
        <!-- 日期选择器默认值 start -->
        <a-form-item
          v-if="selectItem.type === 'date'"
          label="格式化"
          help="YYYY-MM-DD HH:mm:ss"
        >
          <a-select v-model="options.format" placeholder="格式化" allowClear>
            <a-select-option value="YYYY">年</a-select-option>
            <a-select-option value="YYYY-MM">年-月</a-select-option>
            <a-select-option value="YYY-MM-DD">年-月-日</a-select-option>
            <a-select-option value="YYYY-MM-DD HH">年-月-日 时</a-select-option>
            <a-select-option value="YYYY-MM-DD HH:mm"
              >年-月-日 时:分</a-select-option
            >
            <a-select-option value="YYYY-MM-DD HH:mm:ss"
              >年-月-日 时:分:秒</a-select-option
            >
          </a-select>
        </a-form-item>
        <!-- 日期选择器默认值 start -->
        <a-form-item
          v-if="
            ![
              'number',
              'radio',
              'checkbox',
              'date',
              'rate',
              'select',
              'switch',
              'slider',
              'html',
              'user',
              'dataShow',
            ].includes(selectItem.type) && isDefined(options.defaultValue)
          "
          label="默认值"
        >
          <Input
            v-model="options.defaultValue"
            :placeholder="isDefined(options.format) ? '请输入' : options.format"
          />
        </a-form-item>
        <!-- 修改html -->
        <a-form-item v-if="selectItem.type === 'html'" label="默认值">
          <eip-editor
            ref="sqleditor"
            v-model="options.defaultValue"
            :height="300"
            :toolbar="tinymce.toolbar"
            :plugins="tinymce.plugins"
            :menubar="tinymce.menubar"
          ></eip-editor>
          <!-- <Textarea v-model="options.defaultValue" :autoSize="{ minRows: 4, maxRows: 8 }" /> -->
        </a-form-item>

        <a-form-item v-if="selectItem.type === 'divider'" label="备注">
          <eip-editor
            ref="editor"
            v-model="options.remark"
            :height="200"
            :toolbar="tinymce.toolbar"
            :plugins="tinymce.plugins"
          ></eip-editor>
        </a-form-item>
        <a-form-item v-if="selectItem.type === 'divider'" label="样式">
          <a-select v-model="options.style" placeholder="样式" allowClear>
            <a-select-option :value="1">风格1</a-select-option>
            <a-select-option :value="2">风格2</a-select-option>
            <a-select-option :value="3">风格3</a-select-option>
            <a-select-option :value="4">风格4</a-select-option>
            <a-select-option :value="5">风格5</a-select-option>
            <a-select-option :value="6">风格6</a-select-option>
            <a-select-option :value="7">风格7</a-select-option>
            <a-select-option :value="8">风格8</a-select-option>
          </a-select>
        </a-form-item>

        <a-form-item v-if="selectItem.type === 'divider'" label="配色">
          <eip-color
            :value="options.backgroundColor"
            @change="
              (value) => {
                options.backgroundColor = value;
              }
            "
          ></eip-color>
        </a-form-item>
        <a-form-item v-if="selectItem.type === 'divider'" label="标题颜色">
          <eip-color
            :value="options.titleColor"
            @change="
              (value) => {
                options.titleColor = value;
              }
            "
          ></eip-color>
        </a-form-item>

        <!-- 页签位置 start -->
        <a-form-item v-if="selectItem.type === 'tabs'" label="页签位置">
          <Radio buttonStyle="solid" v-model="options.tabPosition">
            <RadioItem value="top">上</RadioItem>
            <RadioItem value="right">右</RadioItem>
            <RadioItem value="bottom">下</RadioItem>
            <RadioItem value="left">左</RadioItem>
          </Radio>
        </a-form-item>
        <!-- 页签位置 end -->
        <!-- 页签类型 start -->
        <a-form-item v-if="selectItem.type === 'tabs'" label="页签类型">
          <Radio buttonStyle="solid" v-model="options.type">
            <RadioButton value="line">line</RadioButton>
            <RadioButton value="card">card</RadioButton>
          </Radio>
        </a-form-item>
        <!-- 页签类型 end -->
        <!-- 页签大小 start -->
        <a-form-item v-if="isDefined(options.size)" label="大小">
          <Radio buttonStyle="solid" v-model="options.size">
            <RadioButton value="large">大</RadioButton>
            <RadioButton value="default">默认</RadioButton>
            <RadioButton value="small">小</RadioButton>
          </Radio>
        </a-form-item>
        <!-- 页签大小 end -->
        <a-form-item v-if="selectItem.type === 'button'" label="类型">
          <Radio buttonStyle="solid" v-model="options.type">
            <RadioItem value="primary">Primary</RadioItem>
            <RadioItem value="default">Default</RadioItem>
            <RadioItem value="dashed">Dashed</RadioItem>
            <RadioItem value="danger">Danger</RadioItem>
          </Radio>
        </a-form-item>
        <!-- 下载方式 start -->
        <a-form-item v-if="isDefined(options.downloadWay)" label="下载方式">
          <Radio buttonStyle="solid" v-model="options.downloadWay">
            <RadioButton value="a">a标签</RadioButton>
            <RadioButton value="ajax">ajax</RadioButton>
            <RadioButton value="dynamic">动态函数</RadioButton>
          </Radio>
          <Input
            v-show="options.downloadWay === 'dynamic'"
            v-model="options.dynamicFun"
            placeholder="动态函数名"
          />
        </a-form-item>
        <!-- 下载方式 end -->
        <a-form-item v-if="selectItem.type === 'button'" label="按钮操作">
          <Radio buttonStyle="solid" v-model="options.handle">
            <RadioButton value="submit">提交</RadioButton>
            <RadioButton value="reset">重置</RadioButton>
            <RadioButton value="dynamic">动态函数</RadioButton>
          </Radio>
          <Input
            v-show="options.handle === 'dynamic'"
            v-model="options.dynamicFun"
            placeholder="动态函数名"
          />
        </a-form-item>
        <a-form-item v-if="selectItem.type === 'alert'" label="辅助描述">
          <Input v-model="options.description" />
        </a-form-item>
        <a-form-item v-if="selectItem.type === 'alert'" label="类型">
          <Radio buttonStyle="solid" v-model="options.type">
            <RadioItem value="success">success</RadioItem>
            <RadioItem value="info">info</RadioItem>
            <RadioItem value="warning">warning</RadioItem>
            <RadioItem value="error">error</RadioItem>
          </Radio>
        </a-form-item>
        <a-form-item v-if="selectItem.type === 'alert'" label="操作属性">
          <kCheckbox v-model="options.showIcon" label="显示图标" />
          <kCheckbox v-model="options.banner" label="无边框" />
          <kCheckbox v-model="options.closable" label="可关闭" />
        </a-form-item>
        <!-- 上传图片 -->
        <a-form-item v-if="selectItem.type === 'uploadImg'" label="样式">
          <Radio buttonStyle="solid" v-model="options.listType">
            <RadioButton value="text">text</RadioButton>
            <RadioButton value="picture">picture</RadioButton>
            <RadioButton value="picture-card">card</RadioButton>
          </Radio>
        </a-form-item>
        <!-- 上传数量 -->
        <a-form-item v-if="isDefined(options.limit)" label="最大上传数量">
          <InputNumber
            class="eip-width-full"
            :min="1"
            v-model="options.limit"
          />
        </a-form-item>

        <!-- scrollY -->
        <a-form-item v-if="isDefined(options.scrollY)" label="scrollY">
          <InputNumber
            class="eip-width-full"
            :min="0"
            v-model="options.scrollY"
          />
        </a-form-item>

        <!-- 上传地址 -->
        <a-form-item v-if="isDefined(options.action)" label="上传地址">
          <Input v-model="options.action" placeholder="请输入" />
        </a-form-item>
        <!-- 删除地址 -->
        <a-form-item v-if="isDefined(options.remove)" label="删除地址">
          <Input v-model="options.remove" placeholder="请输入" />
        </a-form-item>
        <!-- 文件name -->
        <a-form-item v-if="isDefined(options.fileName)" label="文件name">
          <Input v-model="options.fileName" placeholder="请输入" />
        </a-form-item>
        <!-- 文件name -->
        <a-form-item v-if="isDefined(options.fileName)" label="文件name">
          <Input v-model="options.fileName" placeholder="请输入" />
        </a-form-item>
        <!-- 接受上传的文件类型 -->
        <a-form-item
          v-if="isDefined(options.accept)"
          label="接受上传的文件类型"
        >
          <Textarea
            v-model="options.accept"
            placeholder="image/*,.pdf,.doc,.docx等"
          ></Textarea>
        </a-form-item>
        <!-- 文字对齐方式 -->
        <a-form-item v-if="selectItem.type === 'text'" label="文字对齐方式">
          <Radio buttonStyle="solid" v-model="options.textAlign">
            <RadioButton value="left">左</RadioButton>
            <RadioButton value="center">居中</RadioButton>
            <RadioButton value="right">右</RadioButton>
          </Radio>
        </a-form-item>
        <!-- 文字字体 -->
        <a-form-item v-if="selectItem.type === 'text'" label="字体属性设置">
          <eip-color
            :value="options.color"
            @change="
              (value) => {
                options.color = value;
              }
            "
          ></eip-color>
          <Select
            :options="familyOptions"
            v-model="options.fontFamily"
            style="width: 36%; margin-left: 2%; vertical-align: bottom"
          />
          <Select
            :options="sizeOptions"
            v-model="options.fontSize"
            style="width: 35%; margin-left: 2%; vertical-align: bottom"
          />
          <Select
            :options="fontWeightOptions"
            v-model="options.fontWeight"
            style="width: 35%; margin-left: 2%; vertical-align: bottom"
          />
        </a-form-item>
        <a-form-item v-if="selectItem.type === 'text'" label="操作属性">
          <kCheckbox v-model="options.showRequiredMark" label="显示必选标记" />
        </a-form-item>

        <a-form-item
          v-if="
            typeof options.hidden !== 'undefined' ||
            typeof options.disabled !== 'undefined' ||
            typeof options.readonly !== 'undefined' ||
            typeof options.clearable !== 'undefined' ||
            typeof options.multiple !== 'undefined' ||
            typeof options.range !== 'undefined' ||
            typeof options.showTime !== 'undefined' ||
            typeof options.allowHalf !== 'undefined' ||
            typeof options.showInput !== 'undefined' ||
            typeof options.animated !== 'undefined' ||
            typeof options.showArrow !== 'undefined' ||
            typeof options.bordered !== 'undefined' ||
            typeof options.accordion !== 'undefined' ||
            typeof options.rowClickConfirm !== 'undefined' ||
            typeof options.rowDoubleClickConfirm !== 'undefined' ||
            typeof options.topOrg !== 'undefined' ||
            typeof options.copy !== 'undefined' ||
            typeof options.hoverable !== 'undefined' ||
            typeof options.delete !== 'undefined' ||
            typeof options.disabledDateNow !== 'undefined' ||
            typeof options.disabledTimeNow !== 'undefined' ||
            typeof options.only !== 'undefined' ||
            typeof options.isPaging !== 'undefined' ||
            typeof options.showFooter !== 'undefined' ||
            typeof options.canDrag !== 'undefined' ||
            typeof options.defaultValueFirst !== 'undefined' ||
            typeof options.relationModel !== 'undefined'
          "
          label="操作属性"
        >
          <kCheckbox
            v-if="isDefined(options.hidden)"
            v-model="options.hidden"
            label="隐藏"
          />
          <kCheckbox
            v-if="isDefined(options.disabled)"
            v-model="options.disabled"
            label="禁用"
          />
          <kCheckbox
            v-if="isDefined(options.readonly)"
            v-model="options.readonly"
            label="只读"
          />
          <kCheckbox
            v-if="isDefined(options.clearable)"
            v-model="options.clearable"
            label="可清除"
          />
          <kCheckbox
            v-if="isDefined(options.multiple)"
            v-model="options.multiple"
            label="多选"
          />
          <kCheckbox
            v-if="isDefined(options.range)"
            v-model="options.range"
            label="范围选择"
          />
          <kCheckbox
            v-if="isDefined(options.showTime)"
            v-model="options.showTime"
            label="时间选择器"
          />
          <kCheckbox
            v-if="isDefined(options.allowHalf)"
            v-model="options.allowHalf"
            label="允许半选"
          />
          <kCheckbox
            v-if="isDefined(options.showInput)"
            v-model="options.showInput"
            label="显示输入框"
          />
          <kCheckbox
            v-if="isDefined(options.showLabel)"
            v-model="options.showLabel"
            label="显示Label"
          />
          <kCheckbox
            v-if="isDefined(options.chinesization)"
            v-model="options.chinesization"
            label="汉化"
          />
          <kCheckbox
            v-if="isDefined(options.hideSequence)"
            v-model="options.hideSequence"
            label="隐藏序号"
          />
          <kCheckbox
            v-if="isDefined(options.drag)"
            v-model="options.drag"
            label="允许拖拽"
          />
          <kCheckbox
            v-if="isDefined(options.showSearch)"
            v-model="options.showSearch"
            label="可搜索"
          />
          <kCheckbox
            v-if="isDefined(options.treeCheckable)"
            v-model="options.treeCheckable"
            label="可勾选"
          />
          <kCheckbox
            v-if="isDefined(options.animated)"
            v-model="options.animated"
            label="动画切换"
          />
          <kCheckbox
            title="勾选后移除FormItem嵌套且表单无法获取该组件数据"
            v-model="options.noFormItem"
            label="移除FormItem"
          />
          <kCheckbox
            v-if="isDefined(options.rowClickConfirm)"
            v-model="options.rowClickConfirm"
            label="单击确定"
          />
          <kCheckbox
            v-if="isDefined(options.rowDoubleClickConfirm)"
            v-model="options.rowDoubleClickConfirm"
            label="双击确定"
          />

          <kCheckbox
            v-if="isDefined(options.topOrg)"
            v-model="options.topOrg"
            label="只查询下级"
          />
          <kCheckbox
            v-if="isDefined(options.checkStrictly)"
            v-model="options.checkStrictly"
            label="父子节点选中状态不再关联"
          />
          <kCheckbox
            v-if="isDefined(options.copy)"
            v-model="options.copy"
            label="复制"
          />
          <kCheckbox
            v-if="isDefined(options.delete)"
            v-model="options.delete"
            label="删除"
          />
          <kCheckbox
            v-if="isDefined(options.disabledDateNow)"
            v-model="options.disabledDateNow"
            label="小于当前日期"
          />
          <kCheckbox
            v-if="isDefined(options.disabledTimeNow)"
            v-model="options.disabledTimeNow"
            label="小于当前时间"
          />
          <kCheckbox
            v-if="isDefined(options.only)"
            v-model="options.only"
            label="唯一"
          />

          <kCheckbox
            v-if="isDefined(options.isPaging)"
            v-model="options.isPaging"
            label="分页"
          />

          <kCheckbox
            v-if="isDefined(options.showFooter)"
            v-model="options.showFooter"
            label="显示底部"
          />
          <kCheckbox
            v-if="isDefined(options.canDrag)"
            v-model="options.canDrag"
            label="拖动"
          />

          <kCheckbox
            v-if="isDefined(options.defaultValueFirst)"
            v-model="options.defaultValueFirst"
            label="默认选择第一个"
          />

          <kCheckbox
            v-if="isDefined(options.relationModel)"
            v-model="options.relationModel"
            label="关联字段名称"
          />

          <kCheckbox
            v-if="isDefined(options.showArrow)"
            v-model="options.showArrow"
            label="是否展示当前面板上的箭头"
          />
          <kCheckbox
            v-if="isDefined(options.accordion)"
            v-model="options.accordion"
            label="手风琴模式	"
          />
          <kCheckbox
            v-if="isDefined(options.bordered)"
            v-model="options.bordered"
            label="显示边框"
          />
          <kCheckbox
            v-if="isDefined(options.hoverable)"
            v-model="options.hoverable"
            label="鼠标移过时可浮起"
          />
        </a-form-item>
        <a-form-item
          v-if="typeof options.expandIconPosition !== 'undefined'"
          label="图标位置"
        >
          <a-radio-group
            v-model="options.expandIconPosition"
            button-style="solid"
          >
            <a-radio-button value="left"> 左 </a-radio-button>
            <a-radio-button value="right"> 右 </a-radio-button>
          </a-radio-group>
        </a-form-item>
        <a-form-item
          v-if="
            typeof options.defaultType !== 'undefined' &&
            isDefined(options.accordion) &&
            !options.accordion
          "
          label="展开类型"
        >
          <a-radio-group v-model="options.defaultType" button-style="solid">
            <a-radio-button value="0"> 所有 </a-radio-button>
            <a-radio-button value="1"> 第一个 </a-radio-button>
            <a-radio-button value="2"> 不展开 </a-radio-button>
          </a-radio-group>
        </a-form-item>
        <a-form-item
          v-if="isDefined(options.footerName) && options.showFooter"
          label="底部显示名称"
        >
          <Input
            v-model="options.footerName"
            placeholder="请输入底部显示名称"
          />
        </a-form-item>
        <a-form-item
          v-if="isDefined(options.footerName) && options.showFooter"
          label="底部显示配置"
        >
          <KFooter v-model="options.footerOptions" :list="selectItem.list" />
        </a-form-item>

        <a-form-item
          v-if="isDefined(selectItem.rules) && selectItem.rules.length > 0"
          label="校验"
        >
          <kCheckbox v-model="selectItem.rules[0].required" label="必填" />
          <Input
            v-model="selectItem.rules[0].message"
            placeholder="必填校验提示信息"
          />
          <KChangeOption v-model="selectItem.rules" type="rules" />
        </a-form-item>

        <!-- 表格选项 -->
        <a-form-item v-if="selectItem.type === 'table'" label="表格样式CSS">
          <Input v-model="selectItem.options.customStyle" />
        </a-form-item>
        <a-form-item v-if="selectItem.type === 'table'" label="属性">
          <kCheckbox v-model="selectItem.options.bordered" label="显示边框" />
          <kCheckbox v-model="selectItem.options.bright" label="鼠标经过点亮" />
          <kCheckbox v-model="selectItem.options.small" label="紧凑型" />
        </a-form-item>

        <a-form-item v-if="selectItem.type === 'table'" label="提示">
          <p style="line-height: 26px">请点击右键增加行列，或者合并单元格</p>
        </a-form-item>

        <a-form-item v-if="isDefined(selectItem.help)" label="帮助信息">
          <Input v-model="selectItem.help" placeholder="请输入" />
        </a-form-item>

        <!-- 前缀 -->
        <a-form-item label="前缀" v-if="isDefined(options.addonBefore)">
          <Input v-model="options.addonBefore" placeholder="请输入" />
        </a-form-item>

        <!-- 后缀 -->
        <a-form-item label="后缀" v-if="isDefined(options.addonAfter)">
          <Input v-model="options.addonAfter" placeholder="请输入" />
        </a-form-item>

        <a-form-item v-if="['input'].includes(selectItem.type)" label="默认值">
          <a-select
            v-model="options.defaultValueType"
            placeholder="请选择默认值"
            allowClear
          >
            <a-select-opt-group>
              <span slot="label">当前人员信息</span>
              <a-select-option value="userid"> 用户Id </a-select-option>
              <a-select-option value="username"> 用户姓名 </a-select-option>
              <a-select-option value="orgid"> 用户部门Id </a-select-option>
              <a-select-option value="orgname"> 用户部门名称 </a-select-option>
            </a-select-opt-group>
            <a-select-opt-group>
              <span slot="label">日期时间相关选项</span>
              <a-select-option value="shorttime">
                短日期格式(2018-4-15)
              </a-select-option>
              <a-select-option value="longtime">
                长日期格式(2018年4月15日)
              </a-select-option>
              <a-select-option value="shorttimemmhh">
                短时间格式(23:59)
              </a-select-option>
              <a-select-option value="longtimemmhh">
                长时间格式(23时59分)
              </a-select-option>
              <a-select-option value="shortdatetime">
                短日期时间格式(2018-4-15 22:31)
              </a-select-option>
              <a-select-option value="longdatetime">
                长日期时间格式(2018年4月15日 22时31分)
              </a-select-option>
            </a-select-opt-group>
            <a-select-opt-group>
              <span slot="label">流程发起者相关选项</span>
              <a-select-option value="senduserid"> 发起者Id </a-select-option>
              <a-select-option value="sendusername">
                发起者姓名
              </a-select-option>
              <a-select-option value="sendorgid">
                发起者部门Id
              </a-select-option>
              <a-select-option value="sendorgname">
                发起者部门名称
              </a-select-option>
            </a-select-opt-group>
            <a-select-opt-group>
              <span slot="label">流程发起日期事件相关选项</span>
              <a-select-option value="sendshorttime">
                流程发起短日期格式(2018-4-15)</a-select-option
              >
              <a-select-option value="sendlongtime">
                流程发起长日期格式(2018年4月15日)
              </a-select-option>
              <a-select-option value="sendshorttimemmhh">
                流程发起短时间格式(23:59)
              </a-select-option>
              <a-select-option value="sendlongtimemmhh">
                流程发起长时间格式(23时59分)
              </a-select-option>
              <a-select-option value="sendshortdatetime">
                流程发起短日期时间格式(2018-4-15 22:31)
              </a-select-option>

              <a-select-option value="sendlongdatetime">
                流程发起长日期时间格式(2018年4月15日 22时31分)
              </a-select-option>
            </a-select-opt-group>

            <a-select-opt-group>
              <span slot="label">其他值</span>
              <a-select-option value="billno">
                根据规则生成单号</a-select-option
              >
              <a-select-option value="defaultvalue"> 固定值</a-select-option>
              <a-select-option value="api">接口获取</a-select-option>
            </a-select-opt-group>
          </a-select>
        </a-form-item>

        <a-form-item
          v-if="
            ['input'].includes(selectItem.type) &&
            options.defaultValueType == 'defaultvalue'
          "
          label="固定值"
        >
          <a-input
            :allowClear="true"
            v-model="options.defaultValue"
            placeholder="请输入固定值"
          ></a-input>
        </a-form-item>
        <a-form-item
          v-if="
            ['input'].includes(selectItem.type) &&
            options.defaultValueType == 'api'
          "
          label="接口地址"
        >
          <a-input
            :allowClear="true"
            v-model="options.defaultValueApiPath"
            placeholder="请输入接口地址"
          ></a-input>
        </a-form-item>
        <a-form-item
          v-if="
            ['input'].includes(selectItem.type) &&
            options.defaultValueType == 'billno'
          "
          label="规则"
        >
          <a-input
            :allowClear="true"
            v-model="options.defaultValueBillNo"
            placeholder="请输入规则"
          ></a-input>
        </a-form-item>

        <a-form-item v-if="['user'].includes(selectItem.type)" label="默认值">
          <a-select
            v-model="options.defaultValueType"
            placeholder="请选择默认值"
            allowClear
          >
            <a-select-option value="username">当前用户姓名</a-select-option>
          </a-select>
        </a-form-item>

        <a-form-item
          v-if="['organization'].includes(selectItem.type)"
          label="默认值"
        >
          <a-select
            v-model="options.defaultValueType"
            placeholder="请选择默认值"
            allowClear
          >
            <a-select-option value="orgname">当前用户部门名称</a-select-option>
            <a-select-option value="orgcompanyname"
              >当前用户公司名称</a-select-option
            >
          </a-select>
        </a-form-item>

        <a-form-item
          v-if="['organization'].includes(selectItem.type)"
          label="数据范围"
        >
          <a-select
            v-model="options.range"
            placeholder="请选择数据范围"
            allowClear
          >
            <a-select-option :value="0">所有</a-select-option>
            <a-select-option :value="1">当前部门及下级部门</a-select-option>
            <a-select-option :value="2">下级部门</a-select-option>
            <a-select-option :value="3">当前公司</a-select-option>
            <a-select-option :value="4">当前公司及下级部门</a-select-option>
            <a-select-option :value="5">当前公司至登录人部门</a-select-option>
          </a-select>
        </a-form-item>

        <a-form-item
          v-if="typeof options.marginTop !== 'undefined'"
          label="距离顶部高度(px)"
        >
          <a-input-number
            class="eip-width-full"
            v-model="options.marginTop"
            placeholder="请输入距离顶部高度(px)"
          />
        </a-form-item>

        <a-form-item
          v-if="typeof options.titleIcon !== 'undefined'"
          label="图标"
        >
          <eip-icon
            :name="options.titleIcon"
            @click="iconClick"
            @clear="iconClear"
          ></eip-icon>
        </a-form-item>

        <a-form-item
          v-if="typeof options.hoverable !== 'undefined'"
          label="选项"
        >
        </a-form-item>

        <!-- 按钮配置及动态数据配置 start -->
        <a-form-item
          v-if="typeof options.buttons !== 'undefined'"
          label="按钮配置"
        >
          <KButton v-model="options.buttons" :list="selectItem.list" />
        </a-form-item>
        <!-- 按钮配置及动态数据配置 end -->

        <a-form-item
          v-if="
            ['switch'].includes(selectItem.type) &&
            typeof options.txt !== 'undefined' &&
            typeof options.txt.yes !== 'undefined'
          "
          label="'是'显示字（子表使用）"
        >
          <a-input
            :allowClear="true"
            v-model="options.txt.yes"
            placeholder="请输入'是'显示字（子表使用）"
          ></a-input>
        </a-form-item>

        <a-form-item
          v-if="
            ['switch'].includes(selectItem.type) &&
            typeof options.txt !== 'undefined' &&
            typeof options.txt.no !== 'undefined'
          "
          label="'否'显示字（子表使用）"
        >
          <a-input
            :allowClear="true"
            v-model="options.txt.no"
            placeholder="请输入'否'显示字（子表使用）"
          ></a-input>
        </a-form-item>

        <a-form-item v-if="['slider'].includes(selectItem.type)" label="方向">
          <kCheckbox v-model="selectItem.options.vertical" label="垂直方向" />
        </a-form-item>

        <a-form-item
          v-if="typeof options.labelColor !== 'undefined'"
          label="标题颜色"
        >
          <eip-color
            :value="options.labelColor"
            @change="
              (color) => {
                options.labelColor = color;
              }
            "
          ></eip-color>
        </a-form-item>

        <a-form-item
          v-if="typeof options.labelStyle !== 'undefined'"
          label="标题样式"
        >
          <a-radio-group v-model="options.labelStyle" button-style="solid">
            <a-radio-button value=""> 无 </a-radio-button>
            <a-radio-button value="bold">
              <a-icon type="bold" />
            </a-radio-button>
            <a-radio-button value="italic">
              <a-icon type="italic" />
            </a-radio-button>
            <a-radio-button value="underline">
              <a-icon type="underline" />
            </a-radio-button>
            <a-radio-button value="line-through">
              <a-icon type="strikethrough" />
            </a-radio-button>
          </a-radio-group>
        </a-form-item>
        <a-form-item
          v-if="typeof options.labelFamily !== 'undefined'"
          label="标题字体"
        >
          <Select :options="familyOptions" v-model="options.labelFamily" />
        </a-form-item>
        <a-form-item
          v-if="typeof options.labelSize !== 'undefined'"
          label="标题大小"
        >
          <a-input-number v-model="options.labelSize" /> px
        </a-form-item>

        <a-form-item v-if="options.span" label="栅格数">
          <Slider v-model="options.span" :min="1" :max="24" />
        </a-form-item>
      </a-form>
    </div>
  </div>
</template>

<script>
/*
 * author kcz
 * date 2019-11-20
 * description 表单控件属性设置组件,因为配置数据是引用关系，所以可以直接修改
 */
import KChangeOption from "../../KChangeOption/index.vue";
import kCheckbox from "../../KCheckbox/index.vue";
import KButton from "../../KButton/index.vue";
import KFooter from "../../KFooter/index.vue";
import { pluginManager } from "../../../utils/index";
const Slider = pluginManager.getComponent("aSlider").component;
const Input = pluginManager.getComponent("input").component;
const InputNumber = pluginManager.getComponent("number").component;
const Rate = pluginManager.getComponent("rate").component;
const Checkbox = pluginManager.getComponent("checkbox").component;
const Radio = pluginManager.getComponent("radio").component;
const RadioButton = pluginManager.getComponent("radioButton").component;
const RadioItem = pluginManager.getComponent("radioItem").component;
const Textarea = pluginManager.getComponent("textarea").component;
const Select = pluginManager.getComponent("select").component;
const ASwitch = pluginManager.getComponent("switch").component;

import KDataSource from "../../KDataSource/index";
import KMap from "../../KMap/index";
import KDataList from "../../KDataList/index.vue";
import KDataShow from "../../KDataShow/index.vue";
import KCorrelationRecord from "../../KCorrelationRecord/index.vue";
import KDictionary from "../../KDictionary/index.vue";

import KSerialNo from "../../KSerialNo/index.vue";
export default {
  name: "formItemProperties",
  components: {
    KChangeOption,
    kCheckbox,
    KButton,
    KFooter,
    Input,
    Slider,
    InputNumber,
    Rate,
    ASwitch,
    Checkbox,
    Radio,
    RadioItem,
    RadioButton,
    Textarea,
    Select,
    KDataSource,
    KMap,
    KDataList,
    KDataShow,
    KSerialNo,
    KCorrelationRecord,
    KDictionary,
  },
  data() {
    return {
      tinymce: {
        plugins: "",
        toolbar: "",
        height: 100,
      },
      familyOptions: [
        // 字体选择设置
        {
          value: "",
          label: "默认",
        },
        {
          value: "SimSun",
          label: "宋体",
        },
        {
          value: "FangSong",
          label: "仿宋",
        },
        {
          value: "Microsoft YaHei",
          label: "微软雅黑",
        },
        {
          value: "庞门正道标题体",
          label: "庞门正道标题体",
        },
        {
          value: "SimHei",
          label: "黑体",
        },
        {
          value: "PingFangSC-Regular",
          label: "苹方",
        },
        {
          value: "KaiTi",
          label: "楷体",
        },
        {
          value: "LiSu",
          label: "隶书",
        },
      ],
      sizeOptions: [
        //字号选择设置
        {
          value: "26pt",
          label: "一号",
        },
        {
          value: "24pt",
          label: "小一",
        },
        {
          value: "22pt",
          label: "二号",
        },
        {
          value: "18pt",
          label: "小二",
        },
        {
          value: "16pt",
          label: "三号",
        },
        {
          value: "15pt",
          label: "小三",
        },
        {
          value: "14pt",
          label: "四号",
        },
        {
          value: "12pt",
          label: "小四",
        },
        {
          value: "10.5pt",
          label: "五号",
        },
        {
          value: "9pt",
          label: "小五",
        },
      ],
      fontWeightOptions: [
        {
          value: "normal",
          label: "默认值",
        },
        {
          value: "bold",
          label: "粗体",
        },
        {
          value: "bolder",
          label: "更粗",
        },
        {
          value: "lighter",
          label: "更细",
        },
        {
          value: "inherit",
          label: "继承父",
        },
        {
          value: "100",
          label: "100",
        },
        {
          value: "200",
          label: "200",
        },
        {
          value: "300",
          label: "300",
        },
        {
          value: "400",
          label: "400",
        },
        {
          value: "500",
          label: "500",
        },
        {
          value: "600",
          label: "600",
        },
        {
          value: "700",
          label: "700",
        },
        {
          value: "800",
          label: "800",
        },
        {
          value: "900",
          label: "900",
        },
      ],
    };
  },
  computed: {
    options() {
      return this.selectItem.options || {};
    },
  },
  props: {
    selectItem: {
      type: Object,
      required: true,
    },
    hideModel: {
      type: Boolean,
      default: false,
    },
  },
  methods: {
    /**
     *图标点击
     */
    iconClick(icon) {
      this.options.titleIcon = icon.name;
    },

    /**
     * 清空图标
     */
    iconClear() {
      this.options.titleIcon = null;
    },

    /**
     * 标签改变
     */
    labelChange() {
      // var model = pinyin.convert(this.selectItem.label)
      var model = this.selectItem.model;
      if (!this.hideModel && typeof this.selectItem.model !== "undefined") {
        this.selectItem.model = model;
        if (
          ["select", "treeSelect", "cascader", "checkbox", "radio"].includes(
            this.selectItem.type
          )
        ) {
          this.selectItem.options.dynamicKey = model;
        }
      }

      if (typeof this.options.placeholder !== "undefined") {
        this.options.placeholder = "请输入" + this.selectItem.label;
        this.selectItem.rules[0].message = "请输入" + this.selectItem.label;
        if (
          [
            "select",
            "cascader",
            "date",
            "time",
            "user",
            "organization",
            "correlationRecord",
            "district",
            "dataList",
            "uploadFile",
            "uploadImg",
            "icon",
          ].includes(this.selectItem.type)
        ) {
          this.selectItem.rules[0].message = "请选择" + this.selectItem.label;
          this.options.placeholder = "请选择" + this.selectItem.label;
        }
      }
    },
    /**
     * 判断是否已定义
     * @param {*} value
     */
    isDefined(value) {
      return typeof value !== "undefined";
    },
  },
};
</script>
