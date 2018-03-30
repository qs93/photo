using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Photo.Common
{
    /// <summary>
    /// 自定义输出异常
    /// </summary>
    public class OutEx : Exception
    {
        /// <summary>
        /// 异常代码
        /// </summary>
        public int Code { get; set; }

        public string logMessage { get; set; }

        /// <summary>
        /// 输出异常
        /// </summary>
        /// <param name="message">异常信息</param>
        public OutEx(string message) : base(message)
        {
            
        }

        /// <summary>
        /// 输出异常>带代码
        /// </summary>
        /// <param name="code">异常代码</param>
        /// <param name="message">异常信息</param>
        public OutEx(int code,string message):base(message)
        {
            Code = code;
        }
    }

    /// <summary>
    /// 不保存到Log中
    /// </summary>
    public class NotLogOutEx : Exception
    {
        /// <summary>
        /// 输出异常
        /// </summary>
        /// <param name="message">异常信息</param>
        public NotLogOutEx(string message) : base(message)
        {

        }

        /// <summary>
        /// 异常代码
        /// </summary>
        public int Code { get; set; }
        /// <summary>
        /// 输出异常>带代码
        /// </summary>
        /// <param name="code">异常代码</param>
        /// <param name="message">异常信息</param>
        public NotLogOutEx(int code, string message) : base(message)
        {
            Code = code;
        }
    }
}
