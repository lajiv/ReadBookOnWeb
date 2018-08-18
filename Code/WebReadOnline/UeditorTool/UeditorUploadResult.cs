using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebReadOnline.UeditorTool
{
    /// <summary>
    /// 百度编辑器上传返回结果
    /// </summary>
    public class UeditorUploadResult
    {
        public UeditorUploadState State { get; set; }
        public string Url { get; set; }
        public string OriginFileName { get; set; }

        public string ErrorMessage { get; set; }
    }

    public enum UeditorUploadState
    {
        Success = 0,
        SizeLimitExceed = -1,
        TypeNotAllow = -2,
        FileAccessError = -3,
        NetworkError = -4,
        Unknown = 1,
    }
}