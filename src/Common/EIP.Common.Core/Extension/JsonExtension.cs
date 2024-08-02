/**************************************************************
* Copyright (C) 2023 www.eipflow.com 孙泽伟版权所有(盗版必究)
*
* 作者: 孙泽伟(QQ 1039318332)
* 创建时间: 2018/10/30 22:40:15
* 文件名: 
* 描述: 
* 
* 修改历史
* 修改人：
* 时间：
* 修改说明：
*
**************************************************************/
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace EIP.Common.Core.Extension
{
    /// <summary>
    /// Json扩展
    /// </summary>
    public static class JsonExtension
    {
        /// <summary>
        /// 字符串序列化为集合对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="jsonStr"></param>
        /// <returns></returns>
        public static List<T> JsonStringToList<T>(this string jsonStr)
        {
            return JsonConvert.DeserializeObject<List<T>>(jsonStr);
        }

        /// <summary>
        /// 字符串序列化为集合对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="jsonStr"></param>
        /// <returns></returns>
        public static T JsonStringToObject<T>(this string jsonStr)
        {
            return JsonConvert.DeserializeObject<T>(jsonStr);
        }

        /// <summary>
        /// 字符串序列化为集合对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static string ListToJsonString<T>(this IEnumerable<T> t)
        {
            return JsonConvert.SerializeObject(t);
        }

        /// <summary>
        /// 字符串序列化为集合对象
        /// </summary>
        /// <returns></returns>
        public static string ObjectToJsonString<T>(this T t)
        {
            return JsonConvert.SerializeObject(t);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        public static DataTable JsonToDataTable(string json)
        {
            DataTable table = new DataTable();
            //JsonStr为Json字符串
            JArray array = JsonConvert.DeserializeObject(json) as JArray;//反序列化为数组
            if (array.Count > 0)
            {
                StringBuilder columns = new StringBuilder();

                JObject objColumns = array[0] as JObject;
                //构造表头
                foreach (JToken jkon in objColumns.AsEnumerable<JToken>())
                {
                    string name = ((JProperty)(jkon)).Name;
                    columns.Append(name + ",");
                    table.Columns.Add(name);
                }
                //向表中添加数据
                for (int i = 0; i < array.Count; i++)
                {
                    DataRow row = table.NewRow();
                    JObject obj = array[i] as JObject;
                    foreach (JToken jkon in obj.AsEnumerable<JToken>())
                    {

                        string name = ((JProperty)(jkon)).Name;
                        string value = ((JProperty)(jkon)).Value.ToString();
                        row[name] = value;
                    }
                    table.Rows.Add(row);
                }
            }
            return table;
        }
    }

    /// <summary>
    /// 属性选项
    /// </summary>
    public enum PropertyOption
    {
        /// <summary>
        /// 包含/展示 某些属性
        /// </summary>
        Include,
        /// <summary>
        /// 排除/隐藏 某些属性
        /// </summary>
        Exclude
    }

    /// <inheritdoc />
    /// <summary>
    /// 属性展示设置
    /// 设置包含某个属性或排除某个属性
    /// </summary>
    public class PropertyDisplayResolver : DefaultContractResolver
    {
        private readonly PropertyOption _propertyOption;
        private readonly IEnumerable<string> _propertyNames;

        /// <inheritdoc />
        /// <summary>
        /// 初始化实例
        /// </summary>
        /// <param name="propertyOption">属性选项：Include包含/Exclude排除</param>
        /// <param name="propertyNames">属性名称列表</param>
        public PropertyDisplayResolver(PropertyOption propertyOption, IEnumerable<string> propertyNames)
        {
            _propertyOption = propertyOption;
            _propertyNames = propertyNames;
        }

        /// <inheritdoc />
        /// <summary>
        /// 初始化实例
        /// </summary>
        /// <param name="propertyOption">属性选项：Include包含/Exclude排除</param>
        /// <param name="propertyNames">属性名称</param>
        public PropertyDisplayResolver(PropertyOption propertyOption, params string[] propertyNames)
        {
            _propertyOption = propertyOption;
            _propertyNames = propertyNames;
        }

        /// <inheritdoc />
        /// <summary>
        /// 重写 DefaultContractResolver 里的 CreateProperties创建属性方法
        /// Creates properties for the given <see cref="T:Newtonsoft.Json.Serialization.JsonContract" />.
        /// </summary>
        /// <param name="type">The type to create properties for.</param>
        /// <param name="memberSerialization">The member serialization mode for the type.</param>
        /// <returns>Properties for the given <see cref="T:Newtonsoft.Json.Serialization.JsonContract" />.</returns>
        protected override IList<JsonProperty> CreateProperties(Type type, MemberSerialization memberSerialization)
        {
            var properties = base.CreateProperties(type, memberSerialization).ToList();
            //此处进行筛选
            return _propertyOption == PropertyOption.Include ? properties.FindAll(p => _propertyNames.Contains(p.PropertyName))
                : properties.FindAll(p => !_propertyNames.Contains(p.PropertyName));
        }
    }
}