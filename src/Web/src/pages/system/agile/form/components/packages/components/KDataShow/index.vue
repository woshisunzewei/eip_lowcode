<template>
  <div>
    <a-form-item label="显示类型">
      <a-select v-model="value.type" placeholder="请选择显示类型">
        <a-select-option value="label">文字</a-select-option>
        <a-select-option value="img">图片</a-select-option>
        <a-select-option value="qrCode">二维码</a-select-option>
        <a-select-option value="barCode">条形码</a-select-option>
      </a-select>
    </a-form-item>
    <a-form-item label="内容">
      <eip-editor ref="editor" v-model="value.defaultValue" :height="tinymce.height" :toolbar="tinymce.toolbar"
        :plugins="tinymce.plugins" :menubar="tinymce.menubar"></eip-editor>
      <a-button block type="primary" @click="condition.visible = true">
        点击设置表单字段
      </a-button>
    </a-form-item>
    <a-form-item v-if="value.type == 'label'" label="字体属性设置">
      <eip-color :value="value.label.color" @change="(color) => { value.label.color = color }"></eip-color>
      <a-select :options="familyOptions" v-model="value.label.fontFamily"
        style="width: 36%; margin-left: 2%; vertical-align: bottom" />
      <a-select :options="sizeOptions" v-model="value.label.fontSize"
        style="width: 35%; margin-left: 2%; vertical-align: bottom" />
    </a-form-item>

    <a-form-item v-if="value.type == 'qrCode'">
      <span slot="label">
        容错级别
        <a-tooltip title="容错率是指二维码被遮挡多少后，仍可以扫描出来的能力。容错率越高，二维码越容易被扫描，二维码图片也越复杂。
0 - 提供了最少的错误校正
1 - 可以提供大约30%的错误校正
2 - 大约20%的错误校正
3 - 提供了最大的错误校正，大约7%的错误能被校正">
          <a-icon type="question-circle-o" />
        </a-tooltip>
      </span>
      <a-input-number id="inputNumber" v-model="value.qrCode.correctLevel" :min="0" :max="3" />
    </a-form-item>

    <a-form-item v-if="value.type == 'qrCode'">
      <span slot="label">
        尺寸
        <a-tooltip title="尺寸, 长宽一致, 包含外边距">
          <a-icon type="question-circle-o" />
        </a-tooltip>
      </span>
      <a-input-number v-model="value.qrCode.size" :min="10" />
    </a-form-item>

    <a-form-item v-if="value.type == 'qrCode'">
      <span slot="label">
        外边距
        <a-tooltip title="二维码图像的外边距, 默认 20px">
          <a-icon type="question-circle-o" />
        </a-tooltip>
      </span>
      <a-input-number v-model="value.qrCode.margin" :min="0" />
    </a-form-item>

    <a-form-item v-if="value.type == 'qrCode'" label="背景色">
      <eip-color :value="value.qrCode.backgroundColor"
        @change="(color) => { value.qrCode.backgroundColor = color }"></eip-color>
    </a-form-item>

    <a-form-item v-if="value.type == 'qrCode'" label="实点颜色">
      <eip-color :value="value.qrCode.colorDark" @change="(color) => { value.qrCode.colorDark = color }"></eip-color>
    </a-form-item>

    <a-form-item v-if="value.type == 'qrCode'" label="空白区颜色">
      <eip-color :value="value.qrCode.colorLight" @change="(color) => { value.qrCode.colorLight = color }"></eip-color>
    </a-form-item>

    <a-form-item v-if="value.type == 'barCode'">
      <span slot="label">
        类型
        <a-tooltip title="要使用的条形码类型">
          <a-icon type="question-circle-o" />
        </a-tooltip>
      </span>
      <a-select v-model="value.barCode.format" style="width: 200px">
        <a-select-option value="CODE128">
          CODE128
        </a-select-option>
        <a-select-option value="CODE39">
          CODE39
        </a-select-option>
      </a-select>
    </a-form-item>

    <a-form-item v-if="value.type == 'barCode'">
      <span slot="label">
        宽度
        <a-tooltip title="设置条之间的宽度">
          <a-icon type="question-circle-o" />
        </a-tooltip>
      </span>
      <a-input-number v-model="value.barCode.width" :min="0" />
    </a-form-item>

    <a-form-item v-if="value.type == 'barCode'">
      <span slot="label">
        高度
      </span>
      <a-input-number v-model="value.barCode.height" :min="0" />
    </a-form-item>

    <a-form-item v-if="value.type == 'barCode'">
      <span slot="label">
        显示文字
        <a-tooltip title="是否在条形码下方显示文字">
          <a-icon type="question-circle-o" />
        </a-tooltip>
      </span>
      <a-switch v-model="value.barCode.displayValue" />
    </a-form-item>

    <a-form-item v-if="value.type == 'barCode'">
      <span slot="label">
        粗体或变斜体
        <a-tooltip title="使文字加粗体或变斜体 bold italic">
          <a-icon type="question-circle-o" />
        </a-tooltip>
      </span>
      <a-input v-model="value.barCode.fontOptions" />
    </a-form-item>

    <a-form-item v-if="value.type == 'barCode'">
      <span slot="label">
        字体
      </span>
      <a-select :options="familyOptions" v-model="value.barCode.font" style="width: 100%" />
    </a-form-item>

    <a-form-item v-if="value.type == 'barCode'" label="水平对齐方式">
      <a-select v-model="value.barCode.textAlign" style="width: 100%">
        <a-select-option value="left">左</a-select-option>
        <a-select-option value="center">中</a-select-option>
        <a-select-option value="right">右</a-select-option>
      </a-select>
    </a-form-item>

    <a-form-item v-if="value.type == 'barCode'" label="垂直位置">
      <a-select v-model="value.barCode.textPosition" style="width: 100%">
        <a-select-option value="top">上</a-select-option>
        <a-select-option value="bottom">下</a-select-option>
        <a-select-option value="none">不显示</a-select-option>
      </a-select>
    </a-form-item>

    <a-form-item v-if="value.type == 'barCode'">
      <span slot="label">
        间距
        <a-tooltip title="设置条形码和文本之间的间距">
          <a-icon type="question-circle-o" />
        </a-tooltip>
      </span>
      <a-input-number v-model="value.barCode.textMargin" :min="0" />
    </a-form-item>

    <a-form-item v-if="value.type == 'barCode'">
      <span slot="label">
        文本大小
      </span>
      <a-input-number v-model="value.barCode.fontSize" :min="0" />
    </a-form-item>

    <a-form-item v-if="value.type == 'barCode'" label="背景色">
      <eip-color :value="value.barCode.background"
        @change="(color) => { value.barCode.background = color }"></eip-color>
    </a-form-item>

    <a-form-item v-if="value.type == 'barCode'" label="条和文本的颜色">
      <eip-color :value="value.barCode.lineColor" @change="(color) => { value.barCode.lineColor = color }"></eip-color>
    </a-form-item>

    <a-form-item v-if="value.type == 'barCode'">
      <span slot="label">
        边距 <a-tooltip title="设置条形码周围的空白边距">
          <a-icon type="question-circle-o" />
        </a-tooltip>
      </span>
      <a-input-number v-model="value.barCode.margin" :min="0" />
    </a-form-item>

    <a-modal v-drag :zIndex="99999" width="370px" :visible="condition.visible" :bodyStyle="{ padding: '1px' }"
      :destroyOnClose="true" :maskClosable="false" :title="condition.title" @cancel="conditionCancel" :footer="null">
      <a-spin :spinning="condition.tree.spinning">
        <a-row>
          <a-col>
            <a-card class="eip-admin-card-small" size="small">
              <a-directory-tree default-expand-all :style="condition.tree.style" :tree-data="condition.tree.data"
                :expandAction="false" @select="onSelect" v-if="condition.tree.data.length > 0"></a-directory-tree>
              <eip-empty :style="condition.tree.style" v-if="condition.tree.data.length == 0" description="无相关信息" />
            </a-card>
          </a-col>
        </a-row>
      </a-spin>
    </a-modal>
  </div>
</template>

<script>

/*
 * author 孙泽伟
 * date 2022-03-28
 * description 数据显示
 */
export default {
  name: "KDataShow",
  components: {},
  data() {
    return {
      tinymce: {
        plugins: "",
        toolbar: "",
        height: 100,
        menubar: "",
      },

      condition: {
        title: "选择字段",
        visible: false,

        tree: {
          type: "",
          style: {
            overflow: "auto",
            height: "400px",
          },
          data: [],
          spinning: false,
        },
      },
    };
  },
  props: {
    value: {
      type: Object,
      required: true,
    },
    familyOptions: Array,
    sizeOptions: Array
  },
  created() {
    this.conditionInit();
  },
  methods: {
    /**
     * 初始化条件
     */
    conditionInit() {
      let that = this;

      that.condition.tree.spinning = true;
      let fieldSchema = this.eipAgileDesigner.kfd.getFieldSchema();
      fieldSchema
        .filter((f) => !["batch", "text", "divider", "dataShow"].includes(f.type))
        .forEach((item) => {
          that.condition.tree.data.push({
            key: item.model,
            isLeaf: true,
            title: item.label != "" ? item.label : item.model,
          });
        })

      //得到子表
      fieldSchema
        .filter((f) => f.type == "batch")
        .forEach((item) => {
          var childrens = [];
          if (item.list.length > 0) {
            var children = {
              title: item.model + (item.label != "" ? "(" + item.label + ")" : ""),
            }

            item.list.forEach((control) => {
              childrens.push({
                key: item.model + "." + control.model,
                isLeaf: true,
                title: control.model + (control.label != "" ? "(" + control.label + ")" : ""),
              });
            });
            children.children = childrens;
            that.condition.tree.data.push(children);
          }
        });
      that.condition.tree.data.push(this.eipSystemFiled)
      that.condition.tree.spinning = false;
    },

    /** */
    conditionCancel() {
      this.condition.visible = false;
    },

    /**
     * 选择
     */
    onSelect(keys, event) {
      if (event.node.getNodeChildren().length == 0) {
        let arrayTrees = this.$utils.toTreeArray(this.condition.tree.data)
        var data = this.$utils.find(arrayTrees, f => f.key == keys[0]);
        if (data) {
          let html = "<span id='" + encodeURIComponent(keys[0]) + "' class='non-editable'>" + data.title + "</span>";
          this.$refs.editor.insertIndex(html, 0);
        }

        this.conditionCancel();
      }
    },
  },
};
</script>

<style lang="less" scoped>
.ant-form-item {
  margin-bottom: 0;
}
</style>
