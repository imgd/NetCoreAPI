using System;

namespace WebApiCore
{
    /// <summary>
    /// 业务输出实体
    /// </summary>
    /// <typeparam name="T"></typeparam>
    [Serializable]
    public class Result<T>
    {
        public int code { get; set; }
        public string message { get; set; }
        public T data { get; set; }

        public Result() { }
        public Result(int code, string message)
        {
            this.code = code;
            this.message = message;
            this.data = default(T);
        }
        public Result(int code, string message, T data)
        {
            this.code = code;
            this.message = message;
            this.data = data;
        }
        public void SetResult(int code, string message)
        {
            this.code = code;
            this.message = message;
            this.data = default(T);
        }
        public void SetResult(int code, string message, T data)
        {
            this.code = code;
            this.message = message;
            this.data = data;
        }
    }

}
