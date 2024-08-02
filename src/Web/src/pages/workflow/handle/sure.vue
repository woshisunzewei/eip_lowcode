<template>
  <a-modal centered :zIndex="2000" width="700px" v-drag :destroyOnClose="true" :maskClosable="false" :visible="visible"
    :bodyStyle="{ padding: '1px' }" title="审核流程" @cancel="cancel">
    <div style="padding: 10px">
      <a-spin :spinning="spinning">
        <a-card :bodyStyle="{ 'max-height': '300px', 'overflow-y': 'auto' }" size="small"
          v-for="(item, index) in person" :key="index">
          <div slot="title">
            <a-badge :count="form.person.length">
              {{ item.activity.title }}
            </a-badge>
          </div>
          <div slot="extra">
            <!-- <a-space>
              <a-input-search placeholder="名称模糊查询..." allowClear style="width: 200px" />
            </a-space> -->
          </div>
          <a-checkbox-group v-model="form.person" v-if="item.persons.length > 0 && item.persons[0].pattern == '1'">
            <a-row>
              <a-col :span="6" v-for="(pitem, pindex) in item.persons" :key="pindex">
                <div class="padding-bottom-sm">
                  <a-checkbox :value="pitem.userId">
                    <img style="width: 32px; height: 32px; border-radius: 50%;" v-real-img="pitem.headImage" />
                    <span class="margin-left-sm">{{ pitem.name }}</span>
                    <!-- <span>【{{ pitem.organizationName }}】</span> -->
                  </a-checkbox>
                </div>
              </a-col>
            </a-row>
          </a-checkbox-group>

          <a-radio-group @change="radioGroupChange" v-if="item.persons.length > 0 && item.persons[0].pattern != '1'">
            <a-row>
              <a-col :span="6" v-for="(pitem, pindex) in item.persons" :key="pindex">
                <div class="padding-bottom-sm">
                  <a-radio :value="pitem.userId">
                    <img style="width: 32px; height: 32px; border-radius: 50%;" v-real-img="pitem.headImage" />
                    <span class="margin-left-sm">{{ pitem.name }}</span>
                    <!-- <span>【{{ pitem.organizationName }}】</span> -->
                  </a-radio>
                </div>
              </a-col>
            </a-row>
          </a-radio-group>
        </a-card>

        <a-card style="margin-top: 4px" size="small" title="意见信息" v-if="commentMessage || commentFile">
          <a-form-model ref="form" :model="form" layout="vertical" :rules="rules" v-if="commentMessage">
            <a-form-model-item prop="commentId">
              <a-select placeholder="请选择常用意见" allow-clear v-model="form.commentId" show-search
                :filter-option="filterOption" @change="commentChange">
                <div slot="dropdownRender" slot-scope="menu">
                  <v-nodes :vnodes="menu" />
                  <a-divider style="margin: 4px 0" />
                  <div style="padding: 4px 8px; cursor: pointer" @click="commentAdd">
                    <a-icon type="plus" /> 新建意见
                  </div>
                </div>
                <a-select-option v-for=" t in comments " :key="t.commentId">
                  {{ t.content }}
                </a-select-option>
              </a-select>
            </a-form-model-item>
            <a-form-model-item prop="comment">
              <a-textarea :auto-size="{ minRows: 4 }" allow-clear v-model="form.comment" placeholder="请输入意见" />
            </a-form-model-item>

            <a-form-model-item v-if="commentFile">
              <a-upload :file-list="files.fileList" @change="filesHandleChange" :customRequest="filesBeforeUpload"
                :remove="removeFile">
                <a-button> <a-icon type="upload" /> 点击上传附件 </a-button>
              </a-upload></a-form-model-item>
          </a-form-model>
        </a-card>

        <a-card style="margin-top: 4px" size="small" title="通知类型" v-if="notice.length > 0">
          <a-checkbox-group v-model="form.notice">
            <a-row>
              <a-col :span="6" v-for="( item, index ) in notice " :key="index">
                <div class="padding-bottom-sm">
                  <a-checkbox :value="index">
                    {{ item.remark }}
                  </a-checkbox>
                </div>
              </a-col>
            </a-row>
          </a-checkbox-group>
        </a-card>

        <a-card :bodyStyle="{ 'padding': '2px' }" style="margin-top: 4px" size="small" v-if="commentSign">
          <div slot="title">
            在线签字
          </div>
          <div slot="extra">
            <a-button type="primary" size="small" @click="signReset">
              <a-icon type="form" /> 重签
            </a-button>
          </div>

          <vue-esign ref="esign" style="border: 1px dashed #c2c1c1" :isCrop="sign.isCrop" :lineWidth="sign.lineWidth"
            :lineColor="sign.lineColor" :bgColor.sync="sign.bgColor" />
        </a-card>

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
          <a-form-model layout="vertical">
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
import { commentMy, commentSave, fileUpload, fileDel } from "@/services/workflow/handle/sure";
import { newGuid } from "@/utils/util";
import vueEsign from "vue-esign";
export default {
  components: {
    vueEsign,
    VNodes: {
      functional: true,
      render: (h, ctx) => ctx.props.vnodes,
    },
  },
  name: "sure",
  data() {
    return {
      config: {
        labelCol: { span: 4 },
        wrapperCol: { span: 20 },
      },
      files: {
        loading: false,
        imageUrl: "",
        file: null,
        fileList: [],
        check: false,
      },
      form: {
        commentId: undefined,
        comment: "",
        notice: [],
        person: [],
        sign: ''
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
      sign: {
        lineWidth: 6,
        lineColor: "#000000",
        bgColor: "#fff",
        isCrop: false,
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
    commentMessage: {
      type: Boolean,
      default: false,
    },
    commentFile: {
      type: Boolean,
      default: false,
    },
    commentSign: {
      type: Boolean,
      default: false,
    },
    title: {
      type: String,
    },

    notice: {
      type: Array,
    },
    person: {
      type: Array,
    },
    taskId: {
      type: String,
    },
  },
  mounted() {
    this.initComment();
  },
  methods: {
    /**
    * 重画
    */
    signReset() {
      this.$refs.esign.reset();
    },

    /**
    * 单选确认
    */
    radioGroupChange(e) {
      this.form.person.push(e.target.value)
    },
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
      commentSave({
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
      for (let index = 0; index < that.notice.length; index++) {
        that.form.notice.push(index);
      }
    },

    /**
     * 取消
     */
    cancel() {
      this.$emit("update:visible", false);
    },
    /**
        *移除文件
        * @param {*} file
        */
    removeFile(file) {
      let that = this;
      fileDel({ id: file.fileId }).then((result) => {
        if (result.code === that.eipResultCode.success) {
          that.$message.success(result.msg);
        } else {
          that.$message.error(result.msg);
        }
      });
    },
    /**
     * 值改变
     * @param {*} info
     */
    filesHandleChange(info) {
      this.files.file = info.file;
      this.files.fileList = info.fileList;
    },
    /**
     * 自定义上传文件
     * @param {*} data
     */
    filesBeforeUpload(data) {
      var file = data.file;
      let that = this;
      this.files.loading = true;
      let fd = new FormData(); //表单格式
      fd.append("file", file); //添加file表单数据
      fd.append("correlationId", this.taskId); //添加file表单数据
      fd.append("single", false); //添加file表单数据
      fileUpload(fd).then((result) => {
        if (result.code === that.eipResultCode.success) {
          that.$message.success(result.msg);
          that.files.file.status = "done";
          that.files.file.fileId = result.data[0].fileId;
          that.files.fileList[that.files.fileList.length - 1].url =
            result.data[0].path;

          that.files.loading = false;
        } else {
          that.$message.error(result.msg);
        }
      });
      return false;
    },
    /**
     * 保存
     */
    save() {
      let that = this;
      if (this.commentMessage) {
        this.$refs.form.validate((valid) => {
          if (valid) {
            that.$emit("confirm", that.form);
            that.cancel();
          } else {
            return false;
          }
        });
      } else {
        that.$emit("confirm", that.form);
        that.cancel();
      }
    },
  },
};
</script>
<style lang="less" scoped>
/deep/ .ant-checkbox-group {
  width: 100% !important;
}

/deep/ .ant-radio-group {
  width: 100% !important;
}
</style>