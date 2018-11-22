using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace WebApiCore
{
    //---------------------------------------------------------------
    //   Json扩展方法类                                                   
    // —————————————————————————————————————————————————             
    // | varsion 1.0                                   |             
    // | creat by gd 2014.7.31                         |             
    // | 联系我:@大白2013 http://weibo.com/u/2239977692 |            
    // —————————————————————————————————————————————————             
    //                                                               
    // *使用说明：                                                    
    //    使用当前扩展类添加引用: using Extensions.JsonExtension;                      
    //    使用所有扩展类添加引用: using Extensions;                         
    // -------------------------------------------------------------- 

    public static class JsonExtension
    {
       

        #region Newtonsoft.Json
        /// <summary>
        /// 对象序列化成Json字符串
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static string ObjectToJson(this object obj)
        {
            if (obj == null)
            {
                return string.Empty;
            }
            return JsonConvert.SerializeObject(obj);
        }


        /// <summary>
        /// Json字符串序列化成对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static T JsonToObject<T>(this string obj) where T : class
        {
            if (obj == null)
            {
                return default(T);
            }
            return JsonConvert.DeserializeObject<T>(obj);
        }



        /// <summary>
        /// JSON dynamic 对象 序列化成实体对象
        /// </summary>
        /// <typeparam name="T">需要返回的实例类型</typeparam>
        /// <param name="json">需要反序列化的json字符串</param>
        /// <returns></returns>
        public static T JsonToObject<T>(dynamic json) where T : class
        {
            if (json == null)
            {
                return default(T);
            }
            return JsonConvert.DeserializeObject<T>(Convert.ToString(json));
        }

        #endregion 

    }
}
