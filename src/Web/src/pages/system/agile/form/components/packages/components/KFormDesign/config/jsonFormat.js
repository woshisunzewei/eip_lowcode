/*
 * author kcz
 * date 2019-11-20
 * description 默认导入json数据格式
 */
export default `{
	"list": [],
	"config": {
		"layout": "horizontal",
		"labelCol": {
			"xs": 4,
			"sm": 4,
			"md": 4,
			"lg": 4,
			"xl": 4,
			"xxl": 4
		},
		"wrapperCol": {
			"xs": 18,
			"sm": 18,
			"md": 18,
			"lg": 18,
			"xl": 18,
			"xxl": 18
		},
		"event": {
            "pc": {
				"onload": "",
				"change": "",
				"submitBefore": "",
				"submitAfter": ""
            },
            "mobile": {
				"onload": "",
				"change": "",
				"submitBefore": "",
				"submitAfter": ""
            }
          },
		"buttons":[{
			"triggerType": 1,
			"style": "",
			"name": "关闭",
			"icon": "close",
			"theme": "outlined",
			"method": "cancel",
			"script": null
		},
		{
			"triggerType": 1,
			"style": "primary",
			"name": "提交",
			"icon": "save",
			"theme": "outlined",
			"method": "saveClose",
			"script": null
		}],
		"hideRequiredMark": false,
		"customStyle": "",
		"labelLayout": "flex",
		"labelWidth": 100,
		"labelBold": true,
		"showFormType":"modal",
		"showFormWidthUnit":"px",
		"showFormWidth":"600",
		"modalCentered":false,
		"maskClosable":false,
		"zIndex":1000
	}
}`;