<template>
  <a-modal width="1100px" v-drag :destroyOnClose="true" :maskClosable="false" :title="title" :visible="visible"
    :bodyStyle="{ padding: '1px', height: '620px' }" @cancel="cancel">
    <a-spin :spinning="spinning">
      <a-form-model ref="form" :model="base" :rules="rules" :label-col="config.labelCol"
        :wrapper-col="config.wrapperCol">
        <a-tabs default-active-key="1" @change="changeTab">
          <a-tab-pane key="1" :forceRender="true">
            <span slot="tab"> <a-icon type="form" />基本 </span>
            <a-row>
              <a-col>
                <a-form-model-item label="名称" prop="title">
                  <a-input v-model="base.title" placeholder="请输入名称" allow-clear />
                </a-form-model-item>
                <a-form-model-item label="表单" prop="formId">
                  <a-select placeholder="请选择表单" allow-clear v-model="base.formId" @change="formChange">
                    <a-select-option v-for="(item, i) in forms" :key="i" :value="item.configId">{{ item.name
                      }}</a-select-option>
                  </a-select>
                </a-form-model-item>
                <a-form-model-item label="审签类型" prop="commentMessage">
                  <a-switch checked-children="有" un-checked-children="无" v-model="base.commentMessage" />
                </a-form-model-item>
                <a-form-model-item label="签章" prop="commentSign">
                  <a-switch checked-children="有" un-checked-children="无" v-model="base.commentSign" />
                </a-form-model-item>
                <a-form-model-item label="附件" prop="commentFile">
                  <a-switch checked-children="有" un-checked-children="无" v-model="base.commentFile" />
                </a-form-model-item>
                <a-form-model-item label="历史意见" prop="isOpinion">
                  <a-switch checked-children="有" un-checked-children="无" v-model="base.isOpinion" />
                </a-form-model-item>
                <a-form-model-item label="归档" prop="isArchive">
                  <a-switch checked-children="是" un-checked-children="否" v-model="base.isArchive" />
                </a-form-model-item>
              </a-col>
            </a-row>
          </a-tab-pane>

          <a-tab-pane key="2" :forceRender="true">
            <span slot="tab"> <a-icon type="user" />处理人 </span>
            <div style="padding: 10px">
              <a-tabs type="card" default-active-key="1">
                <a-tab-pane key="1">
                  <span slot="tab"> <a-icon type="team" />流程处理人 </span>
                  <a-card style="margin-top: 0px" size="small">
                    <vxe-table ref="tableUser" size="small" :height="240" :data="user.approve" row-key>
                      <template #loading>
                        <a-spin></a-spin>
                      </template>

                      <template #empty>
                        <eip-empty />
                      </template>
                      <vxe-column type="seq" title="序号" align="center" width="60"></vxe-column>
                      <vxe-column title="排序" width="50" align="center">

                        <template #default>
                          <span class="drag-btn" style="cursor: move">
                            <a-icon type="drag" />
                          </span>
                        </template>
                      </vxe-column>
                      <vxe-column width="140" align="center">

                        <template #header>
                          <a-button type="primary" size="small" @click="userSetAdd">
                            <a-icon type="plus" />处理人
                          </a-button>
                        </template>

                        <template #default="{ row }">
                          <a-button size="small" @click="userSetEdit(row)">
                            <a-icon type="edit" />
                          </a-button>
                          <a-divider type="vertical" />
                          <a-popconfirm title="确定删除处理人?" ok-text="确定" cancel-text="取消" @confirm="userSetDel(row)">
                            <a-button @click.stop="" size="small" type="danger">
                              <a-icon type="delete" />
                            </a-button>
                          </a-popconfirm>
                        </template>
                      </vxe-column>
                      <vxe-column field="name" title="处理人"></vxe-column>
                    </vxe-table>
                  </a-card>
                  <a-card style="margin-top: 10px" size="small">
                    <a-row :gutter="[24, 24]" type="flex" justify="start">
                      <a-col :span="4">处理策略：</a-col>
                      <a-col :span="20">
                        <a-radio-group v-model="user.strategy">
                          <a-radio :value="0">列表中的第一处理人</a-radio>
                          <a-radio :value="1">发送给列表中的所有处理人</a-radio>
                          <a-radio :value="2">弹出选择审批人</a-radio>
                        </a-radio-group>
                      </a-col>
                      <a-col :span="4">选人规则：</a-col>
                      <a-col :span="20">
                        <a-radio-group v-model="user.chosen">
                          <a-radio :value="0">非必选</a-radio>
                          <a-radio :value="1">必选</a-radio>
                        </a-radio-group>
                      </a-col>

                      <a-col :span="4">无处理人时：</a-col>
                      <a-col :span="20">
                        <a-radio-group v-model="user.no">
                          <a-radio :value="0">不能提交</a-radio>
                          <a-radio :value="1">跳过本步骤</a-radio>
                        </a-radio-group>
                      </a-col>

                      <a-col :span="4">自动同意策略：</a-col>
                      <a-col :span="20">
                        <a-checkbox-group v-model="user.auto">
                          <a-checkbox :value="0">处理人就是提交人</a-checkbox>
                          <a-checkbox :value="1">处理人和上一步相同</a-checkbox>
                          <a-checkbox :value="2">处理人已经审批过</a-checkbox>
                        </a-checkbox-group>
                      </a-col>

                      <a-col :span="4">通过策略：</a-col>
                      <a-col :span="20">
                        <a-radio-group v-model="user.pass">
                          <a-radio :value="0">一人同意即可</a-radio>
                          <a-radio :value="1">所有人必须同意</a-radio>
                          <a-radio :value="2">通过百分比</a-radio>
                          <a-radio :value="3">处理后不知会</a-radio>
                        </a-radio-group>
                      </a-col>

                      <a-col v-if="user.pass == 2" :span="4">通过百分比：</a-col>
                      <a-col v-if="user.pass == 2" :span="20">
                        <a-input-number :default-value="user.passConfig" :min="0" :max="100"
                          :formatter="(value) => `${value}%`" :parser="(value) => value.replace('%', '')" />
                      </a-col>
                    </a-row>
                  </a-card>
                </a-tab-pane>
                <a-tab-pane key="2" force-render>
                  <span slot="tab">
                    <a-icon type="user-add" />加签处理人
                  </span>
                </a-tab-pane>
              </a-tabs>
            </div>
          </a-tab-pane>

          <a-tab-pane key="3" :forceRender="true">
            <span slot="tab"> <a-icon type="ordered-list" />按钮 </span>
            <div class="tab-margin">
              <vxe-table ref="tableButton" id="workflowactivitybutton" size="small" row-id="buttonId" :checkbox-config="{
    checkRowKeys: button.check,
    trigger: 'row',
    highlight: true,
    range: true,
    reserve: true,
  }" :height="button.height" :data="button.data">

                <template #loading>
                  <a-spin></a-spin>
                </template>

                <template #empty>
                  <eip-empty />
                </template>
                <vxe-column type="seq" title="序号" align="center" width="60"></vxe-column>
                <vxe-column field="showPc" title="PC显示" align="center" width="80">

                  <template v-slot="{ row }">
                    <a-switch :checked="row.showPc" @change="row.showPc = !row.showPc" />
                  </template>
                </vxe-column>
                <vxe-column field="showMobile" title="移动端" align="center" width="80">

                  <template v-slot="{ row }">
                    <a-switch :checked="row.showMobile" @change="row.showMobile = !row.showMobile" />
                  </template>
                </vxe-column>
                <vxe-column field="name" title="标题" width="150">

                  <template #default="{ row }">
                    <a-input v-model="row.name"></a-input>
                  </template>
                </vxe-column>
                <vxe-column field="script" title="事件" width="150">

                  <template #default="{ row }">
                    <a-input v-model="row.script"></a-input>
                  </template>
                </vxe-column>
                <vxe-column field="name" title="按钮" width="150">

                  <template v-slot="{ row }">
                    <a-button :type="row.style">
                      <a-icon :type="row.icon" :theme="row.theme" />
                      {{ row.name }}</a-button>
                  </template>
                </vxe-column>
                <vxe-column field="remark" title="备注" min-width="150" showOverflow="ellipsis"></vxe-column>
              </vxe-table>
            </div>
          </a-tab-pane>

          <a-tab-pane key="4" :forceRender="true">
            <span slot="tab"> <a-icon type="unordered-list" />字段</span>
            <div class="tab-margin">
              <div style="padding: 10px 0">
                <a-button type="primary" @click="columnReset">字段重置</a-button>
              </div>
              <vxe-table ref="tableColumn" id="workflowactivitycolumn" size="small" :height="column.height"
                :data="column.data">

                <template #loading>
                  <a-spin></a-spin>
                </template>

                <template #empty>
                  <eip-empty />
                </template>
                <vxe-column type="seq" align="center" title="序号" width="60"></vxe-column>
                <vxe-column field="name" title="字段名" width="180" showOverflow="ellipsis"></vxe-column>
                <vxe-column field="description" title="说明" width="180" showOverflow="ellipsis"></vxe-column>
                <vxe-column field="isRead" title="读" align="center" width="80">

                  <template #header>
                    读<br /> <a-switch v-model="column.allRead" @change="allReadChange" />
                  </template>

                  <template v-slot="{ row }">
                    <a-switch :checked="row.isRead" @change="row.isRead = !row.isRead" />
                  </template>
                </vxe-column>
                <vxe-column field="isWrite" title="写" align="center" width="80">

                  <template #header>
                    写<br /> <a-switch v-model="column.allWrite" @change="allWriteChange" />
                  </template>

                  <template v-slot="{ row }">
                    <a-switch :checked="row.isWrite" @change="row.isWrite = !row.isWrite" />
                  </template>
                </vxe-column>
                <vxe-column field="isDelete" title="删" align="center" width="80">

                  <template #header>
                    删<br /> <a-switch v-model="column.allDelete" @change="allDeleteChange" />
                  </template>

                  <template v-slot="{ row }">
                    <a-switch :checked="row.isDelete" @change="row.isDelete = !row.isDelete" />
                  </template>
                </vxe-column>
                <vxe-column field="isHide" title="隐藏" align="center" width="80">

                  <template #header>
                    隐藏<br /> <a-switch v-model="column.allHide" @change="allHideChange" />
                  </template>

                  <template v-slot="{ row }">
                    <a-switch :checked="row.isHide" @change="row.isHide = !row.isHide" />
                  </template>
                </vxe-column>
                <vxe-column field="isDefault" title="默认值" align="center" width="80">

                  <template #header>
                    默认值<br /> <a-switch v-model="column.allDefault" @change="allDefaultChange" />
                  </template>

                  <template v-slot="{ row }">
                    <a-space v-if="row.type == 'batch'">
                      <a-tooltip>
                        <template slot="title">
                          点击设置子表按钮权限
                        </template>
                        <a-icon @click="batchButton(row)" type="block" :style="{ fontSize: '20px' }"></a-icon>
                      </a-tooltip>

                      <a-tooltip>

                        <template slot="title">
                          点击设置子表字段权限
                        </template>
                        <a-icon @click="batchField(row)" type="setting" :style="{ fontSize: '20px' }"></a-icon>
                      </a-tooltip>
                    </a-space>
                    <a-switch v-else :checked="row.isDefault" @change="row.isDefault = !row.isDefault" />
                  </template>
                </vxe-column>
                <vxe-column title="验证规则" width="100" align="center">

                  <template v-slot="{ row }">
                    <a-badge :dot="row.validator && row.validator.length > 0">
                      <a-button type="primary" @click="setValidator(row)">验证</a-button>
                    </a-badge>

                  </template>
                </vxe-column>
              </vxe-table>
            </div>
          </a-tab-pane>

          <a-tab-pane key="5" :forceRender="true">
            <span slot="tab"> <a-icon type="sound" />通知 </span>
            <div class="tab-margin">
              <vxe-table ref="tableNotice" size="small" :height="notice.height" :data="notice.data">

                <template #loading>
                  <a-spin></a-spin>
                </template>

                <template #empty>
                  <eip-empty />
                </template>
                <vxe-column type="seq" title="序号" align="center" width="60"></vxe-column>
                <vxe-column width="80" align="center">

                  <template #header>
                    <a-button type="primary" size="small" @click="noticeAdd">
                      <a-icon type="plus" />
                    </a-button>
                  </template>

                  <template #default="{ row }">
                    <a-popconfirm title="确定删除通知?" ok-text="确定" cancel-text="取消" @confirm="noticeDel(row)">
                      <a-button @click.stop="" size="small" type="danger">
                        <a-icon type="delete" />
                      </a-button>
                    </a-popconfirm>
                  </template>
                </vxe-column>
                <vxe-column field="remark" title="备注" width="150">

                  <template v-slot="{ row }">
                    <a-input placeholder="请输入备注" v-model="row.remark" />
                  </template>
                </vxe-column>
                <vxe-column field="use" title="启用" width="60" align="center">

                  <template v-slot="{ row }">
                    <a-switch checked-children="是" un-checked-children="否" v-model="row.use" />
                  </template>
                </vxe-column>
                <vxe-column field="userchosen" title="用户选择" width="130" align="center">

                  <template v-slot="{ row }">
                    <a-switch checked-children="是" un-checked-children="否" v-model="row.userchosen" />
                  </template>
                </vxe-column>
                <vxe-column field="trigger" title="触发类型" align="center" width="180">

                  <template v-slot="{ row }">
                    <a-select v-model="row.trigger">
                      <a-select-option :value="0">下一步成功</a-select-option>
                      <a-select-option :value="1">退回成功</a-select-option>
                      <a-select-option :value="2">拒绝成功</a-select-option>
                    </a-select>
                  </template>
                </vxe-column>
                <vxe-column title="设置" align="center">

                  <template v-slot="{ row }">
                    <a-space size="small">
                      <a-select style="width: 160px" v-model="row.type" @change="noticeTypeChange(row)">
                        <a-select-option :value="0">邮件</a-select-option>
                        <a-select-option :value="1">站内</a-select-option>
                        <a-select-option :value="2">短信</a-select-option>
                        <a-select-option :value="3">微信公众号</a-select-option>
                        <a-select-option :value="4">微信小程序</a-select-option>
                        <a-select-option :value="5">企业微信</a-select-option>
                        <a-select-option :value="7">钉钉</a-select-option>
                        <a-select-option :value="8">App</a-select-option>
                      </a-select>
                      <a-badge :dot="row.config">
                        <a-button @click="noticeSetting(row)">
                          <a-icon type="setting" />
                        </a-button>
                      </a-badge>

                    </a-space>
                  </template>
                </vxe-column>
              </vxe-table>
            </div>
          </a-tab-pane>

          <a-tab-pane key="6" :forceRender="true">
            <span slot="tab"> <a-icon type="clock-circle" />超时 </span>
            <div class="tab-margin">
              <vxe-table ref="tableOverTime" id="workflowactivitytimeout" size="small" :height="timeout.height"
                :data="timeout.data">

                <template #loading>
                  <a-spin></a-spin>
                </template>

                <template #empty>
                  <eip-empty />
                </template>
                <vxe-column type="seq" title="序号" align="center" width="60"></vxe-column>
                <vxe-column width="80" align="center">

                  <template #header>
                    <a-button type="primary" size="small" @click="timeoutAdd">
                      <a-icon type="plus" />
                    </a-button>
                  </template>

                  <template #default="{ row }">
                    <a-popconfirm title="确定删除超时?" ok-text="确定" cancel-text="取消" @confirm="timeoutDel(row)">
                      <a-button @click.stop="" size="small" type="danger">
                        <a-icon type="delete" />
                      </a-button>
                    </a-popconfirm>
                  </template>
                </vxe-column>
                <vxe-column field="remark" title="备注" width="250">

                  <template v-slot="{ row }">
                    <a-input placeholder="请输入超时发送事由标题" v-model="row.remark" />
                  </template>
                </vxe-column>
                <vxe-column field="use" title="启用" width="60" align="center">

                  <template v-slot="{ row }">
                    <a-switch checked-children="是" un-checked-children="否" v-model="row.use" />
                  </template>
                </vxe-column>
                <vxe-column title="设置" align="center" width="210">

                  <template v-slot="{ row }">
                    <a-space size="small">
                      <a-select @change="timeoutTypeChange(row)" style="width: 140px" v-model="row.type">
                        <a-select-option :value="0">邮件</a-select-option>
                        <a-select-option :value="1">站内</a-select-option>
                        <a-select-option :value="2">短信</a-select-option>
                        <a-select-option :value="3">微信公众号</a-select-option>
                        <a-select-option :value="4">微信小程序</a-select-option>
                        <a-select-option :value="5">企业微信</a-select-option>
                        <a-select-option :value="7">钉钉</a-select-option>
                        <a-select-option :value="8">App</a-select-option>
                      </a-select>
                      <a-badge :dot="row.config != null">
                        <a-button @click="timeoutSetting(row)">
                          <a-icon type="setting" />
                        </a-button>
                      </a-badge>

                    </a-space>
                  </template>
                </vxe-column>
              </vxe-table>
            </div>
          </a-tab-pane>

          <a-tab-pane key="7" :forceRender="true">
            <span slot="tab"> <a-icon type="file-done" />事件 </span>
            <div class="tab-margin">
              <vxe-table ref="tableEvent" size="small" :height="event.height" :data="event.data">

                <template #loading>
                  <a-spin></a-spin>
                </template>

                <template #empty>
                  <eip-empty />
                </template>
                <vxe-column type="seq" title="序号" align="center" width="60"></vxe-column>
                <vxe-column width="80" align="center">

                  <template #header>
                    <a-button size="small" type="primary" @click="eventAdd">
                      <a-icon type="plus" />
                    </a-button>
                  </template>

                  <template #default="{ row }">
                    <a-popconfirm title="确定删除事件?" ok-text="确定" cancel-text="取消" @confirm="eventDel(row)">
                      <a-button @click.stop="" size="small" type="danger">
                        <a-icon type="delete" />
                      </a-button>
                    </a-popconfirm>
                  </template>
                </vxe-column>
                <vxe-column field="remark" title="备注" width="220">

                  <template v-slot="{ row }">
                    <a-input placeholder="请输入事件备注信息" v-model="row.remark" />
                  </template>
                </vxe-column>
                <vxe-column field="use" title="启用" width="60" align="center">

                  <template v-slot="{ row }">
                    <a-switch checked-children="是" un-checked-children="否" v-model="row.use" />
                  </template>
                </vxe-column>
                <vxe-column title="触发类型" align="center" width="210">

                  <template v-slot="{ row }">
                    <a-space size="small">
                      <a-select style="width: 180px" v-model="row.trigger">
                        <a-select-option :value="0">下一步前</a-select-option>
                        <a-select-option :value="1">下一步后</a-select-option>
                        <a-select-option :value="2">退回前</a-select-option>
                        <a-select-option :value="3">退回后</a-select-option>
                        <a-select-option :value="4">拒绝前</a-select-option>
                        <a-select-option :value="5">拒绝后</a-select-option>
                        <a-select-option :value="6">强制结束前</a-select-option>
                        <a-select-option :value="7">强制结后后</a-select-option>
                      </a-select>
                    </a-space>
                  </template>
                </vxe-column>
                <vxe-column title="设置" align="center" width="200">

                  <template v-slot="{ row }">
                    <a-space size="small">
                      <a-select @change="eventTypeChange(row)" style="width: 80px" v-model="row.type">
                        <a-select-option :value="0">JS</a-select-option>
                        <a-select-option :value="1">接口</a-select-option>
                      </a-select>
                      <a-badge :dot="row.config != null">
                        <a-button @click="eventSetting(row)">
                          <a-icon type="setting" />
                        </a-button>
                      </a-badge>

                    </a-space>
                  </template>
                </vxe-column>
              </vxe-table>
            </div>
          </a-tab-pane>
        </a-tabs>
      </a-form-model>
    </a-spin>

    <template slot="footer">
      <a-space>
        <a-button key="back" @click="cancel" :disabled="loading"><a-icon type="close" />取消</a-button>
        <a-button key="submit" @click="save" type="primary" :loading="loading"><a-icon type="save" />提交</a-button>
      </a-space>
    </template>
    <eip-workflow-event-js ref="eventjs" v-if="event.setting.js.visible" :visible.sync="event.setting.js.visible"
      :title="event.setting.js.title" :edit="event.setting.value" @ok="eventOk"></eip-workflow-event-js>

    <eip-workflow-event-interface ref="eventinterface" v-if="event.setting.interface.visible"
      :visible.sync="event.setting.interface.visible" :title="event.setting.interface.title" :edit="event.setting.value"
      @ok="eventOk"></eip-workflow-event-interface>

    <eip-workflow-validator ref="validator" v-if="validator.visible" :visible.sync="validator.visible"
      :validator="validator.data" :title="validator.title" @ok="validatorOk"></eip-workflow-validator>

    <eip-workflow-notice-email v-if="notice.setting.email.visible" :visible.sync="notice.setting.email.visible"
      :column="column.data" :edit="notice.setting.value" title="邮件通知" @ok="noticeOk"></eip-workflow-notice-email>

    <eip-workflow-notice-ding-talk v-if="notice.setting.dingtalk.visible"
      :visible.sync="notice.setting.dingtalk.visible" :column="column.data" :edit="notice.setting.value" title="钉钉通知"
      @ok="noticeOk"></eip-workflow-notice-ding-talk>

    <eip-workflow-notice-system v-if="notice.setting.system.visible" :visible.sync="notice.setting.system.visible"
      :column="column.data" :edit="notice.setting.value" title="站内通知" @ok="noticeOk"></eip-workflow-notice-system>

    <eip-workflow-notice-sms v-if="notice.setting.sms.visible" :visible.sync="notice.setting.sms.visible"
      :column="column.data" :edit="notice.setting.value" title="短信通知" @ok="noticeOk"></eip-workflow-notice-sms>

    <eip-workflow-notice-wei-xin-work v-if="notice.setting.weixinwork.visible"
      :visible.sync="notice.setting.weixinwork.visible" :column="column.data" :edit="notice.setting.value"
      title="企业微信通知" @ok="noticeOk"></eip-workflow-notice-wei-xin-work>

    <eip-workflow-notice-wei-xin-mini-program v-if="notice.setting.weixinminiprogram.visible"
      :visible.sync="notice.setting.weixinminiprogram.visible" :column="column.data" :edit="notice.setting.value"
      title="微信小程序" @ok="noticeOk"></eip-workflow-notice-wei-xin-mini-program>

    <eip-workflow-notice-wei-xin-mp v-if="notice.setting.weixinmp.visible"
      :visible.sync="notice.setting.weixinmp.visible" :column="column.data" :edit="notice.setting.value" title="微信公众号通知"
      @ok="noticeOk"></eip-workflow-notice-wei-xin-mp>

    <eip-workflow-notice-app v-if="notice.setting.app.visible" :visible.sync="notice.setting.app.visible"
      :column="column.data" :edit="notice.setting.value" title="App通知" @ok="noticeOk"></eip-workflow-notice-app>


    <eip-workflow-notice-email v-if="timeout.setting.email.visible" :visible.sync="timeout.setting.email.visible"
      :column="column.data" :edit="timeout.setting.value" title="邮件通知" @ok="timeoutOk"></eip-workflow-notice-email>

    <eip-workflow-notice-ding-talk v-if="timeout.setting.dingtalk.visible"
      :visible.sync="timeout.setting.dingtalk.visible" :column="column.data" :edit="timeout.setting.value" title="钉钉通知"
      @ok="timeoutOk"></eip-workflow-notice-ding-talk>

    <eip-workflow-notice-system v-if="timeout.setting.system.visible" :visible.sync="timeout.setting.system.visible"
      :column="column.data" :edit="timeout.setting.value" title="站内通知" @ok="timeoutOk"></eip-workflow-notice-system>

    <eip-workflow-notice-sms v-if="timeout.setting.sms.visible" :visible.sync="timeout.setting.sms.visible"
      :column="column.data" :edit="timeout.setting.value" title="短信通知" @ok="timeoutOk"></eip-workflow-notice-sms>

    <eip-workflow-notice-wei-xin-work v-if="timeout.setting.weixinwork.visible"
      :visible.sync="timeout.setting.weixinwork.visible" :column="column.data" :edit="timeout.setting.value"
      title="企业微信通知" @ok="timeoutOk"></eip-workflow-notice-wei-xin-work>

    <eip-workflow-notice-wei-xin-mini-program v-if="timeout.setting.weixinminiprogram.visible"
      :visible.sync="timeout.setting.weixinminiprogram.visible" :column="column.data" :edit="timeout.setting.value"
      title="微信小程序" @ok="timeoutOk"></eip-workflow-notice-wei-xin-mini-program>

    <eip-workflow-notice-wei-xin-mp v-if="timeout.setting.weixinmp.visible"
      :visible.sync="timeout.setting.weixinmp.visible" :column="column.data" :edit="timeout.setting.value"
      title="微信公众号通知" @ok="timeoutOk"></eip-workflow-notice-wei-xin-mp>

    <eip-workflow-notice-app v-if="timeout.setting.app.visible" :visible.sync="timeout.setting.app.visible"
      :column="column.data" :edit="timeout.setting.value" title="App通知" @ok="timeoutOk"></eip-workflow-notice-app>


    <eip-workflow-batch-button ref="batchButton" v-if="column.batch.button.visible"
      :visible.sync="column.batch.button.visible" :button="column.batch.button.data" :title="column.batch.button.title"
      @ok="batchButtonOk"></eip-workflow-batch-button>

    <eip-workflow-batch-field ref="batchField" v-if="column.batch.field.visible"
      :visible.sync="column.batch.field.visible" :field="column.batch.field.data" :title="column.batch.field.title"
      @ok="batchFieldOk"></eip-workflow-batch-field>

    <eip-workflow-user-set ref="userset" v-if="userset.visible" :visible.sync="userset.visible" title="处理人员"
      :edit="userset.edit" :column="column.data" @ok="userSetOk"></eip-workflow-user-set>
  </a-modal>
</template>

<script>
import eipWorkflowEventJs from "../components/event/js";
import eipWorkflowEventInterface from "../components/event/interface";
import eipWorkflowValidator from "../components/validator/index";
import eipWorkflowNoticeEmail from "../components/notice/email";
import eipWorkflowNoticeSystem from "../components/notice/system";
import eipWorkflowNoticeSms from "../components/notice/sms";
import eipWorkflowNoticeApp from "../components/notice/app";
import eipWorkflowNoticeWeiXinWork from "../components/notice/weixinwork";
import eipWorkflowNoticeWeiXinMiniProgram from "../components/notice/weixinminiprogram";
import eipWorkflowNoticeWeiXinMp from "../components/notice/weixinmp";
import eipWorkflowNoticeDingTalk from "../components/notice/dingtalk";
import eipWorkflowBatchButton from "../components/batch/button";
import eipWorkflowBatchField from "../components/batch/field";
import eipWorkflowUserSet from "../components/user/set";
import {
  findForm,
  findFormColumns,
  findButton,
} from "@/services/workflow/process/activity/task";
import { mapGetters } from "vuex";
import Sortable from "sortablejs";

export default {
  components: {
    eipWorkflowEventJs,
    eipWorkflowEventInterface,
    eipWorkflowValidator,
    eipWorkflowNoticeEmail,
    eipWorkflowNoticeSystem,
    eipWorkflowNoticeSms,
    eipWorkflowNoticeApp,
    eipWorkflowNoticeWeiXinWork,
    eipWorkflowNoticeWeiXinMiniProgram,
    eipWorkflowNoticeWeiXinMp,
    eipWorkflowNoticeDingTalk,

    eipWorkflowBatchButton,
    eipWorkflowBatchField,

    eipWorkflowUserSet,
  },
  name: "activityTask",
  data() {
    return {
      loading: false,
      spinning: false,
      config: {
        labelCol: {
          span: 3,
        },
        wrapperCol: {
          span: 19,
        },
      },
      rules: {
        title: [
          {
            required: true,
            message: "请输入名称",
            trigger: "blur",
          },
        ],
        formId: [
          {
            required: true,
            message: "请选择表单",
            trigger: ["blur", "change"],
          },
        ],
      },
      base: {
        title: "",
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
      button: {
        height: "540px",
        data: [],
        check: [], //选中按钮
      },
      column: {
        height: "490px",
        data: [],
        row: {},
        original: [],
        allRead: false,//所有读
        allWrite: false,//所有写
        allDelete: false,//所有删除
        allHide: false,//所有隐藏
        allDefault: false,
        batch: {
          button: {
            title: '',
            visible: false,
            data: []
          },

          field: {
            title: '',
            visible: false,
            data: []
          }
        }//子表控制
      },
      notice: {
        height: "540px",
        data: [],
        setting: {
          value: null,
          email: {
            visible: false,
            title: "",
          },
          dingtalk: {
            visible: false,
            title: "",
          },
          system: {
            visible: false,
            title: "",
          },
          sms: {
            visible: false,
            title: "",
          },
          weixinminiprogram: {
            visible: false,
            title: "",
          },
          weixinmp: {
            visible: false,
            title: "",
          },
          weixinwork: {
            visible: false,
            title: "",
          },
          app: {
            visible: false,
            title: "",
          },
        },
      },
      timeout: {
        height: "540px",
        data: [],
        setting: {
          value: null,
          email: {
            visible: false,
            title: "",
          },
          system: {
            visible: false,
            title: "",
          },
          sms: {
            visible: false,
            title: "",
          },
          weixinminiprogram: {
            visible: false,
            title: "",
          },
          weixinmp: {
            visible: false,
            title: "",
          },
          weixinwork: {
            visible: false,
            title: "",
          },
          app: {
            visible: false,
            title: "",
          },
          dingtalk: {
            visible: false,
            title: "",
          },
        },
      },
      userset: {
        visible: false,
        edit: {},
      },
      event: {
        height: "540px",
        data: [],
        setting: {
          value: null,
          js: {
            visible: false,
            title: "",
          },
          interface: {
            visible: false,
            title: "",
          },
        },
      },
      //验证
      validator: {
        visible: false,
        data: [],
        title: "",
      },
      forms: [], //节点配置表单
      obj: {},
    };
  },
  props: {
    visible: {
      type: Boolean,
      default: false,
    },
    data: {
      type: Object,
    },
    title: {
      type: String,
    },
    formId: {
      type: String,
    }, //默认表单Id
  },
  computed: {
    ...mapGetters("account", ["systemAgile"]),
  },
  mounted() {
    this.buttonInit();
    this.userInit();
    this.columnInit();
    this.eventInit();
    this.timeoutInit();
    this.noticeInit();
    this.formInit();
  },
  methods: {
    /**
   * 所有默认改变
   */
    allDefaultChange() {
      let that = this;
      this.column.data.forEach(item => {
        item.isDefault = that.column.allDefault;
      })
    },
    /**
   * 所有读取改变
   */
    allReadChange() {
      let that = this;
      this.column.data.forEach(item => {
        item.isRead = that.column.allRead;
      })
    },
    /**
     * 所有写改变
     */
    allWriteChange() {
      let that = this;
      this.column.data.forEach(item => {
        item.isWrite = that.column.allWrite;
      })
    },

    /**
     * 所有删除改变
     */
    allDeleteChange() {
      let that = this;
      this.column.data.forEach(item => {
        item.isDelete = that.column.allDelete;
      })
    },

    /**
     * 所有隐藏改变
     */
    allHideChange() {
      let that = this;
      this.column.data.forEach(item => {
        item.isHide = that.column.allHide;
      })
    },
    /**
     *选项卡改变事件
     */
    changeTab(activeKey) { },

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
      this.$refs.form.validate((valid, error) => {
        if (valid) {
          that.loading = true;
          that.spinning = true;
          that.data.props.base = that.base;
          that.data.props.user = that.user;
          that.data.text.text = that.base.title;
          that.data.props.event = that.event.data;
          that.data.props.timeout = that.timeout.data;
          that.data.props.notice = that.notice.data;
          that.data.props.button = that.button.data.filter(
            (f) => f.showMobile || f.showPc
          );
          that.data.props.column = that.column.data;
          this.$emit("ok", that.data);
          this.$emit("update:visible", false);
        } else {
          let messages = [];
          for (const key in error) {
            messages.push(error[key][0].message);
          }
          that.$message.error(messages.join(","));
        }
      });
    },

    /**
     * 初始化表单
     */
    formInit() {
      let that = this;
      findForm({
        configType: 2,
      }).then(function (result) {
        if (result.code === that.eipResultCode.success) {
          that.forms = result.data;
          //其他信息
          if (that.data) {
            that.base = that.$utils.clone(that.data.props.base, true);
            if (!that.base.formId && that.formId) {
              that.base.formId = that.formId;
            }
          } else {
            that.base.formId = that.formId;
          }

          if (that.base.formId) {
            that.columnSync();
          }
        }
      });
    },

    /**
     * 字段初始化
     */
    columnInit() {
      let that = this;
      if (that.data) {
        //默认勾选
        that.column.data = that.$utils.clone(that.data.props.column, true);
      }
    },

    /**
     * 表单切换
     */
    formChange(id) {
      let that = this;
      if (id) {
        that.column.data = [];
        that.columnSync();
      } else {
        this.column.data = [];
      }
    },
    /**
      * 字段重置
      */
    async columnReset() {
      let that = this;
      that.column.data = [];
      var result = that.systemAgile.filter(f => f.configId == this.base.formId)[0];
      if (result && result.columnJson) {
        //如果具有敏捷开发信息
        that.column.original = JSON.parse(result.columnJson);
        var column = that.column.original.filter(
          (f) =>
            !["batch", "text", "divider"].includes(f.type)
        );

        column.forEach((item) => {
          var d = {}
          d.name = item.model;
          d.description = item.label;
          d.type = item.type
          d.isRead = true;
          d.isWrite = true;
          d.isDelete = true;
          d.isHide = false;
          d.isDefault = false;
          d.validator = [];
          that.column.data.push(d);
        });

        //子表
        that.column.original.filter((f) => f.type == "batch").forEach((item) => {
          var batchColumns = item.list;
          //循环子表字段
          var batchFields = [];
          batchColumns.forEach(batchColumn => {
            var d = {}
            d.name = batchColumn.model;
            d.description = batchColumn.label;
            d.type = batchColumn.type
            d.isRead = true;
            d.isWrite = true;
            d.isDelete = true;
            d.isHide = false;
            d.isDefault = false;
            d.validator = [];
            batchFields.push(d);
          })
          var d = {}
          d.name = item.model;
          d.description = item.label;
          d.type = 'batch'
          d.isRead = true;
          d.isWrite = true;
          d.isDelete = true;
          d.isHide = false;
          d.isDefault = false;
          d.validator = [];
          d.batch = {
            buttons: item.options.buttons,
            fields: batchFields
          }
          that.column.data.push(d);
        });
      } else {
        findFormColumns(this.base.formId).then(function (result) {
          if (result.code === that.eipResultCode.success) {
            result.data.forEach((item) => {
              item.isRead = true;
              item.isWrite = true;
              item.isDelete = true;
              item.isHide = false;
              item.isDefault = false;
              item.validator = [];
              that.column.data.push(item);
            });
          }
        });
      }
    },
    /**
     * 同步字段
     */
    async columnSync() {
      let that = this;
      var result = that.systemAgile.filter(f => f.configId == this.base.formId)[0];
      if (result && result.columnJson) {
        //如果具有敏捷开发信息
        that.column.original = JSON.parse(result.columnJson);
        var column = that.column.original.filter(
          (f) =>
            !["batch", "text", "divider"].includes(f.type)
        );

        column.forEach((item) => {
          var have = that.column.data.filter((f) => f.name == item.model);
          if (have.length == 0) {
            var d = {}
            d.name = item.model;
            d.description = item.label;
            d.type = item.type
            d.isRead = true;
            d.isWrite = true;
            d.isDelete = true;
            d.isHide = false;
            d.isDefault = false;
            d.validator = [];
            that.column.data.push(d);
          }
        });

        //子表
        that.column.original.filter((f) => f.type == "batch").forEach((item) => {
          var have = that.column.data.filter((f) => f.name == item.model);
          if (have.length == 0) {
            var batchColumns = item.list;
            //循环子表字段
            var batchFields = [];
            batchColumns.forEach(batchColumn => {
              var d = {}
              d.name = batchColumn.model;
              d.description = batchColumn.label;
              d.type = batchColumn.type
              d.isRead = true;
              d.isWrite = true;
              d.isDelete = true;
              d.isHide = false;
              d.isDefault = false;
              d.validator = [];
              batchFields.push(d);
            })
            var d = {}
            d.name = item.model;
            d.description = item.label;
            d.type = 'batch'
            d.isRead = true;
            d.isWrite = true;
            d.isDelete = true;
            d.isHide = false;
            d.isDefault = false;
            d.validator = [];
            d.batch = {
              buttons: item.options.buttons,
              fields: batchFields
            }
            that.column.data.push(d);
          }
        });
      } else {
        findFormColumns(this.base.formId).then(function (result) {
          if (result.code === that.eipResultCode.success) {
            result.data.forEach((item) => {
              var have = that.column.data.filter((f) => f.name == item.name);
              if (have.length == 0) {
                item.isRead = true;
                item.isWrite = true;
                item.isDelete = true;
                item.isHide = false;
                item.isDefault = false;
                item.validator = [];
                that.column.data.push(item);
              }
            });
          }
        });
      }
    },

    /**
   * 子表权限
   */
    batchButton(item) {
      this.column.batch.button.visible = true;
      this.column.batch.button.title = "按钮权限设置:" + item.description;
      this.column.batch.button.data = item.batch.buttons;
    },
    /**
     * 子表按钮确定
     */
    batchButtonOk(button) {
    },

    /**
    * 子表字段权限
    */
    batchField(item) {
      this.column.batch.field.visible = true;
      this.column.batch.field.title = "字段权限设置:" + item.description;
      this.column.batch.field.data = item.batch.fields;
    },

    /**
     * 子表字段确定
     */
    batchFieldOk(button) {
    },

    /**
     * 初始化审核人员
     */
    userInit() {
      let that = this;
      if (that.data) {
        that.user = that.$utils.clone(that.data.props.user, true);
      }
    },

    /**
     * 初始化按钮
     */
    buttonInit() {
      let that = this;
      findButton({
        size: 100,
        sord: "asc",
        sidx: "OrderNo",
      }).then(function (result) {
        if (result.code === that.eipResultCode.success) {
          result.data.forEach((item) => {
            item.showPc = false;
            item.showMobile = false;
          });
          that.button.data = result.data;
          if (that.data) {
            that.data.props.button.forEach((item) => {
              var check = that.button.data.filter(
                (f) => f.buttonId == item.buttonId
              );
              if (check.length > 0) {
                check[0].name = item.name;
                check[0].script = item.script;
                check[0].showPc = item.showPc;
                check[0].showMobile = item.showMobile;
              }
            });
          }
        }
      });
    },

    /**
     * 通知初始化
     */
    noticeInit() {
      let that = this;
      if (that.data) {
        that.notice.data = that.$utils.clone(that.data.props.notice, true);
      }
    },

    /**
     * 添加通知
     */
    noticeAdd() {
      this.notice.data.push({
        remark: "",
        use: true, //是否启用
        userchosen: false,
        trigger: 0, //触发类型:下一步成功,退回成功,拒绝成功
        type: 0, //通知类型:邮件,站内,短信,微信,钉钉,QQ,
        config: null, //具体配置
      });
    },
    /**
     *删除
     */
    noticeDel(row) {
      var delIndex = -1;
      this.notice.data.forEach((item, index) => {
        if (item._X_ROW_KEY == row._X_ROW_KEY) {
          delIndex = index;
        }
      });
      if (delIndex != -1) {
        this.notice.data.splice(delIndex, 1);
      }
    },

    /**
     *
     */
    noticeTypeChange(row) {
      row.config = null;
    },

    /**
     * 消息设置
     */
    noticeSetting(row) {
      this.notice.setting.value = row;
      switch (row.type) {
        case 0:
          this.notice.setting.email.visible = true;
          break;
        case 1:
          this.notice.setting.system.visible = true;
          break;
        case 2:
          this.notice.setting.sms.visible = true;
          break;
        case 3:
          this.notice.setting.weixinmp.visible = true;
          break;
        case 4:
          this.notice.setting.weixinminiprogram.visible = true;
          break;
        case 5:
          this.notice.setting.weixinwork.visible = true;
          break;
        case 7:
          this.notice.setting.dingtalk.visible = true;
          break;
        case 8:
          this.notice.setting.app.visible = true;
          break;
      }
    },

    /**
     * 消息设置成功
     * @param {*} value
     */
    noticeOk(value) {
      this.notice.setting.value.config = value;
    },

    /**
     * 超时通知
     */
    timeoutAdd() {
      this.timeout.data.push({
        remark: "",
        use: true, //是否启用
        trigger: 0, //触发类型:下一步成功,退回成功,拒绝成功
        type: 0, //通知类型:邮件,站内,短信,微信,钉钉,QQ,
        config: null, //具体配置
      });
    },

    /**
     *超时删除
     */
    timeoutDel(row) {
      var delIndex = -1;
      this.timeout.data.forEach((item, index) => {
        if (item._X_ROW_KEY == row._X_ROW_KEY) {
          delIndex = index;
        }
      });
      if (delIndex != -1) {
        this.timeout.data.splice(delIndex, 1);
      }
    },

    /**
     *
     */
    timeoutTypeChange(row) {
      row.config = null;
    },

    /**
     * 消息设置
     */
    timeoutSetting(row) {
      this.timeout.setting.value = row;
      switch (row.type) {
        case 0:
          this.timeout.setting.email.visible = true;
          break;
        case 1:
          this.timeout.setting.system.visible = true;
          break;
        case 2:
          this.timeout.setting.sms.visible = true;
          break;
        case 3:
          this.timeout.setting.weixinmp.visible = true;
          break;
        case 4:
          this.timeout.setting.weixinminiprogram.visible = true;
          break;
        case 5:
          this.timeout.setting.weixinwork.visible = true;
          break;
        case 7:
          this.timeout.setting.dingtalk.visible = true;
          break;
        case 8:
          this.timeout.setting.app.visible = true;
          break;
      }
    },

    /**
     * 消息设置成功
     * @param {*} value
     */
    timeoutOk(value) {
      this.timeout.setting.value.config = value;
    },

    /**
     * 超时通知初始化
     */
    timeoutInit() {
      let that = this;
      if (that.data) {
        that.timeout.data = that.$utils.clone(that.data.props.timeout, true);
      }
    },

    /**
     * 事件通知初始化
     */
    eventInit() {
      let that = this;
      if (that.data) {
        that.event.data = that.$utils.clone(that.data.props.event, true);
      }
    },

    /**
     * 事件通知
     */
    eventAdd() {
      this.event.data.push({
        remark: "",
        use: true, //是否启用
        trigger: 0, //触发类型:下一步开始前,下一步开始后,退回开始前,退回开始后,拒绝开始前,拒绝开始后
        type: 0, //通知类型:JS,接口
        config: null, //具体配置
      });
    },

    /**
     *
     */
    eventOk(value) {
      this.event.setting.value.config = value;
    },

    /**
     * 删除
     */
    eventDel(row) {
      var delIndex = -1;
      this.event.data.forEach((item, index) => {
        if (item._X_ROW_KEY == row._X_ROW_KEY) {
          delIndex = index;
        }
      });
      if (delIndex != -1) {
        this.event.data.splice(delIndex, 1);
      }
    },

    /**
     *
     */
    eventTypeChange(row) {
      row.config = null;
    },

    /**
     * 事件配置
     */
    eventSetting(row) {
      this.event.setting.value = row;
      switch (row.type) {
        case 0:
          this.event.setting.js.title = "事件配置-js";
          this.event.setting.js.visible = true;
          break;
        case 1:
          this.event.setting.interface.title = "事件配置-接口";
          this.event.setting.interface.visible = true;
          break;
      }
    },

    /**
     *
     * @param {*} row
     */
    isReadChange(row) {
      row.isRead = !row.isRead;
    },
    /**
     *
     * @param {*} row
     */
    isWriteChange(row) {
      row.isWrite = !row.isWrite;
    },
    /**
     *
     * @param {*} row
     */
    isDeleteChange(row) {
      row.isDelete = !row.isDelete;
    },
    /**
     *
     * @param {*} row
     */
    isHideChange(row) {
      row.isHide = !row.isHide;
    },
    /**
     *
     * @param {*} row
     */
    isDefaultChange(row) {
      row.isDefault = !row.isDefault;
    },

    /**
     * 处理人员新增
     */
    userSetAdd() {
      this.userset.edit = null;
      this.userset.visible = true;
    },

    /**
     * 保存完毕
     */
    userSetOk(data) {
      if (this.userset.edit) {
        this.userset.edit = data;
      } else {
        this.user.approve.push(data);
      }
      this.userSetDrop();
    },

    /**
     * 处理人编辑
     */
    userSetEdit(row) {
      this.userset.edit = row;
      this.userset.visible = true;
    },

    /**
     * 处理人编辑
     */
    userSetDel(row) {
      var delIndex = -1;
      this.user.approve.forEach((item, index) => {
        if (item._X_ROW_KEY == row._X_ROW_KEY) {
          delIndex = index;
        }
      });
      if (delIndex != -1) {
        this.user.approve.splice(delIndex, 1);
      }
      this.userSetDrop();
    },

    /**
     * 拖拽
     */
    userSetDrop() {
      let that = this;
      const xTable = this.$refs.tableUser;
      Sortable.create(
        xTable.$el.querySelector(".body--wrapper>.vxe-table--body tbody"),
        {
          handle: ".drag-btn",
          onEnd: ({ newIndex, oldIndex }) => {
            const currRow = that.user.approve.splice(oldIndex, 1)[0];
            that.user.approve.splice(newIndex, 0, currRow);
          },
        }
      );
    },
    /**
     * 设置验证规则
     * @param {*} row
     */
    setValidator(row) {
      this.column.row = row;
      this.validator.data = row.validator;
      this.validator.title = "验证规则设置：" + row.description;
      this.validator.visible = true;
    },

    /**
     *验证填写完成
     */
    validatorOk(validator) {
      this.column.row.validator = validator;
    },
  },
};
</script>

<style lang="less" scoped>
.tab-margin {
  margin: 10px;
}

/deep/ .ant-tabs-bar {
  margin: 0 0 4px 0 !important;
}

/deep/ .ant-tabs .ant-tabs-left-content {
  border-left: 0 !important;
  padding-left: 0 !important;
}
</style>