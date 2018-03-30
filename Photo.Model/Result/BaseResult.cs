using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Photo.Model.Result
{
    public class BaseResult
    {
        public BaseResult()
        {

        }

        /// <summary>
        /// 返回的代码
        /// </summary>
        public int Code { get; set; } = 100;
        /// <summary>
        /// 返回的信息
        /// </summary>
        public string Msg { get; set; } = "";

        /// <summary>
        /// 错误时保存到Log的信息，为空时，使用Msg。因为有些错误信息不好直接返回给前端，采用这种方法
        /// </summary>
        private string _logmsg;
        public string LogMsg
        {
            set => _logmsg = value;
            get => string.IsNullOrEmpty(_logmsg) ? Msg : _logmsg;
        }
        /// <summary>
        /// 返回的值
        /// </summary>
        public object Data { get; set; }
    }
}
