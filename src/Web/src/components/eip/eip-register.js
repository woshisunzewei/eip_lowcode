import Vue from "vue";
import eipIcon from "./eip-icon";
import eipMaterial from "./eip-material";
import eipGroup from "./eip-group";
import eipOrganization from "./eip-organization";
import eipPost from "./eip-post";
import eipPrivilegeMasterUser from "./eip-privilegemasteruser";
import eipRole from "./eip-role";
import eipUser from "./eip-user";
import eipSearch from "./eip-search";
import eipSearchPro from "./eip-search-pro";
import eipSearchProItem from "./eip-search-pro-item";
import eipToolbar from "./eip-toolbar";
import eipColumn from "./eip-column";
import eipSql from "./eip-sql";
import eipEditor from "./eip-editor";
import eipColor from "./eip-color";
import eipFile from "./eip-file";
import eipRule from "./eip-rule";
import eipEmpty from "./eip-empty";
import eipCode from "./eip-code";
import eipCron from "./eip-cron";
import eipWorkflowProcessInstanceStatus from "./workflow/eip-workflow-processInstance-status";

let eipList = [
  eipIcon,
  eipMaterial,
  eipGroup,
  eipOrganization,
  eipPost,
  eipPrivilegeMasterUser,
  eipRole,
  eipUser,
  eipSearch,
  eipSearchPro,
  eipSearchProItem,
  eipToolbar,
  eipColumn,
  eipSql,
  eipEditor,
  eipColor,
  eipFile,
  eipRule,
  eipEmpty,
  eipCode,
  eipCron,
  eipWorkflowProcessInstanceStatus,
];

eipList.forEach((ele) => {
  Vue.component(ele.name, ele);
});
