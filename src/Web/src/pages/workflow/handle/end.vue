<template>
  <a-modal :zIndex="10001" width="600px" v-drag :destroyOnClose="true" :maskClosable="false" :visible="visible"
    :bodyStyle="{ padding: '1px' }" title="结束流程" @cancel="cancel">
    <div style="padding: 10px">
      <a-spin :spinning="spinning">
        <a-form-model ref="sureForm" :model="form" layout="vertical" :rules="rules">
          <a-row>
            <a-col>
              <a-form-model-item label="常用意见" prop="commentId">
                <a-select placeholder="请选择常用意见" allow-clear v-model="form.commentId" show-search
                  :filter-option="filterOption" @change="commentChange">
                  <div slot="dropdownRender" slot-scope="menu">
                    <v-nodes :vnodes="menu" />
                    <a-divider style="margin: 4px 0" />
                    <div style="padding: 4px 8px; cursor: pointer" @click="commentAdd">
                      <a-icon type="plus" /> 新建意见
                    </div>
                  </div>
                  <a-select-option v-for="t in comments" :key="t.commentId">
                    {{ t.content }}
                  </a-select-option>
                </a-select>
              </a-form-model-item>
              <a-form-model-item label="意见" prop="comment">
                <a-input allow-clear v-model="form.comment" type="textarea" placeholder="请输入意见" />
              </a-form-model-item>
            </a-col>
          </a-row>
        </a-form-model>
      </a-spin>
    </div>

    <template slot="footer">
      <a-space>
        <a-button key="back" @click="cancel" :disabled="loading"><a-icon type="close" />取消</a-button>
        <a-button key="submit" @click="save" type="primary" :loading="loading"><a-icon type="save" />提交</a-button>
      </a-space>
    </template>

    <a-modal title="新增意见" v-drag :destroyOnClose="true" :maskClosable="false" v-if="comment.visible"
      :bodyStyle="{ padding: '1px' }" :visible="comment.visible" @cancel="comment.visible = false">
      <div style="padding: 10px">
        <a-spin :spinning="spinning">
          <a-form-model ref="form" layout="vertical">
            <a-row>
              <a-col>
                <a-form-model-item label="意见" prop="comment">
                  <a-input allow-clear v-model="comment.value" type="textarea" placeholder="请输入意见" />
                </a-form-model-item>
              </a-col>
            </a-row>
          </a-form-model>
        </a-spin>
      </div>
      <template slot="footer">
        <a-space>
          <a-button key="back" @click="comment.visible = false"><a-icon type="close" />取消</a-button>
          <a-button key="submit" @click="saveComment" type="primary"><a-icon type="save" />提交</a-button>
        </a-space>
      </template>
    </a-modal>
  </a-modal>
</template>

<script>
import { commentMy } from "@/services/workflow/handle/sure";
import { newGuid } from "@/utils/util";
export default {
  components: {
    VNodes: {
      functional: true,
      render: (h, ctx) => ctx.props.vnodes,
    },
  },
  name: "end",
  data() {
    return {
      config: {
        labelCol: { span: 4 },
        wrapperCol: { span: 20 },
      },
      form: {
        commentId: undefined,
        comment: "",
      },
      comment: {
        visible: false,
        value: "",
      },
      comments: [],
      rules: {
        comment: [
          {
            required: true,
            message: "请输入意见",
            trigger: "blur",
          },
        ],
      },

      loading: false,
      spinning: false,
    };
  },

  props: {
    visible: {
      type: Boolean,
      default: false,
    },
    title: {
      type: String,
    },
  },
  mounted() {
    this.initComment();
  },
  methods: {
    /**
     * 意见选择
     */
    commentChange(value) {
      var comment = this.comments.filter((f) => f.commentId == value);
      if (comment.length > 0) {
        this.form.comment = comment[0].content;
      }
    },
    /**
     * 新增意见
     */
    commentAdd() {
      this.comment.visible = true;
    },

    /**
     * 保存意见
     */
    saveComment() {
      let that = this;
      save({
        content: this.comment.value,
        type: 1,
        commentId: newGuid(),
      }).then((result) => {
        that.comment.visible = false;
        that.initComment();
      });
    },
    /**
     *
     * @param {搜索} input
     * @param {*} option
     */
    filterOption(input, option) {
      return (
        option.componentOptions.children[0].text
          .toLowerCase()
          .indexOf(input.toLowerCase()) >= 0
      );
    },
    /**
     *
     */
    initComment() {
      let that = this;
      commentMy().then((result) => {
        that.comments = result.data;
      });
    },

    /**
     * 取消
     */
    cancel() {
      this.$emit("update:visible", false);
    },

    /**
     * 保存
     */
    save() {
      let that = this;
      this.$refs.sureForm.validate((valid) => {
        if (valid) {
          this.$emit("confirm", that.form);
        } else {
          return false;
        }
      });
    },
  },
};
</script>