using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RuleCheck
{
    public class ACheckErrorObj
    {
        public string ruleId = "";
        /// <summary>
        /// 错误描述
        /// </summary>
        public string errorDescription = "";
        /// <summary>
        /// 错误类型
        /// </summary>
        public string errorType = "";

        /// <summary>
        /// 错误图层
        /// </summary>
        public string errorLayer = "";
        /// <summary>
        /// 要素ID
        /// </summary>
        public long featureId = -1;

        /// <summary>
        /// 要素标识码
        /// </summary>
        public long featureBSM = -1;

        public string errorLevel = "3";

       
    }
}
