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
using AutoMapper;
using System.Collections.Generic;

namespace EIP.Common.Core.Extension
{
    /// <summary>
    /// AutoMapper扩展:实体映射转换
    /// 基于:AutoMapper第三方组件
    /// </summary>
    public static class AutoMapperExtension
    {
        /// <summary>
        /// 实体映射
        /// </summary>
        /// <typeparam name="TDestination">目标类型</typeparam>
        /// <param name="source">待转换的对象</param>
        /// <returns></returns>
        public static TDestination MapTo<TSource, TDestination>(this TSource source)
        {
            if (source == null) return default(TDestination);
            var config = new MapperConfiguration(ctx => ctx.CreateMap<TSource, TDestination>());
            var mapper = config.CreateMapper();
            return mapper.Map<TDestination>(source);
        }

        /// <summary>
        /// 实体映射
        /// </summary>
        /// <typeparam name="TDestination">目标类型</typeparam>
        /// <param name="source">待转换的对象</param>
        /// <returns></returns>
        public static TDestination MapTo<TSource, TDestination>(this TSource source, MapperConfigurationExpression configurationExpression)
        {
            if (source == null) return default(TDestination);
            var config = new MapperConfiguration(configurationExpression);
            var mapper = config.CreateMapper();
            return mapper.Map<TDestination>(source);
        }

        /// <summary>
        /// 无具体映射关系使用
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <typeparam name="TDestination"></typeparam>
        /// <param name="source"></param>
        /// <returns></returns>
        public static List<TDestination> MapToList<TSource, TDestination>(this IEnumerable<TSource> source)
        {
            if (source == null) return new List<TDestination>();
            var config = new MapperConfiguration(cfg => cfg.CreateMap<TSource, TDestination>());
            var mapper = config.CreateMapper();
            return mapper.Map<List<TDestination>>(source);
        }

        /// <summary>
        /// 无具体映射关系使用
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <typeparam name="TDestination"></typeparam>
        /// <param name="source"></param>
        /// <returns></returns>
        public static List<TDestination> MapToList<TSource, TDestination>(this IEnumerable<TSource> source, MapperConfigurationExpression configurationExpression)
        {
            if (source == null) return new List<TDestination>();
            var config = new MapperConfiguration(configurationExpression);
            var mapper = config.CreateMapper();
            return mapper.Map<List<TDestination>>(source);
        }
    }
}
