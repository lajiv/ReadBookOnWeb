using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebReadOnline.UeditorTool;

namespace WebReadOnline.Controllers
{
    public class UtilityController : Controller
    {
        // GET: Utility
        #region 选择图标
        /// <summary>
        /// 图标的选择
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [AllowAnonymous]
        public ActionResult Icon()
        {
            return View();
        }
        #endregion

        #region 百度编辑器的后端接口
        /// <summary>
        /// 百度编辑器的后端接口
        /// </summary>
        /// <param name="action">执行动作</param>
        /// <returns></returns>
        public ActionResult UEditor()
        {
            string action = Request["action"];

            switch (action)
            {
                case "config":
                    return Content(JsonConvert.SerializeObject(UeditorConfig.Items));
                case "uploadimage":
                    return UEditorUpload(new UeditorUploadConfig()
                    {
                        AllowExtensions = UeditorConfig.GetStringList("imageAllowFiles"),
                        PathFormat = UeditorConfig.GetString("imagePathFormat"),
                        SizeLimit = UeditorConfig.GetInt("imageMaxSize"),
                        UploadFieldName = UeditorConfig.GetString("imageFieldName")
                    });
                case "uploadscrawl":
                    return UEditorUpload(new UeditorUploadConfig()
                    {
                        AllowExtensions = new string[] { ".png" },
                        PathFormat = UeditorConfig.GetString("scrawlPathFormat"),
                        SizeLimit = UeditorConfig.GetInt("scrawlMaxSize"),
                        UploadFieldName = UeditorConfig.GetString("scrawlFieldName"),
                        Base64 = true,
                        Base64Filename = "scrawl.png"
                    });
                case "uploadvideo":
                    return UEditorUpload(new UeditorUploadConfig()
                    {
                        AllowExtensions = UeditorConfig.GetStringList("videoAllowFiles"),
                        PathFormat = UeditorConfig.GetString("videoPathFormat"),
                        SizeLimit = UeditorConfig.GetInt("videoMaxSize"),
                        UploadFieldName = UeditorConfig.GetString("videoFieldName")
                    });
                case "uploadfile":
                    return UEditorUpload(new UeditorUploadConfig()
                    {
                        AllowExtensions = UeditorConfig.GetStringList("fileAllowFiles"),
                        PathFormat = UeditorConfig.GetString("filePathFormat"),
                        SizeLimit = UeditorConfig.GetInt("fileMaxSize"),
                        UploadFieldName = UeditorConfig.GetString("fileFieldName")
                    });
                case "listimage":
                    return ListFileManager(UeditorConfig.GetString("imageManagerListPath"), UeditorConfig.GetStringList("imageManagerAllowFiles"));
                case "listfile":
                    return ListFileManager(UeditorConfig.GetString("fileManagerListPath"), UeditorConfig.GetStringList("fileManagerAllowFiles"));
                case "catchimage":
                    return CrawlerHandler();
                default:
                    return Content(JsonConvert.SerializeObject(new
                    {
                        state = "action 参数为空或者 action 不被支持。"
                    }));
            }
        }
        /// <summary>
        /// 百度编辑器的文件上传
        /// </summary>
        /// <param name="uploadConfig">上传配置信息</param>
        /// <returns></returns>
        public ActionResult UEditorUpload(UeditorUploadConfig uploadConfig)
        {
            UeditorUploadResult result = new UeditorUploadResult() { State = UeditorUploadState.Unknown };

            byte[] uploadFileBytes = null;
            string uploadFileName = null;

            if (uploadConfig.Base64)
            {
                uploadFileName = uploadConfig.Base64Filename;
                uploadFileBytes = Convert.FromBase64String(Request[uploadConfig.UploadFieldName]);
            }
            else
            {
                var file = Request.Files[uploadConfig.UploadFieldName];
                uploadFileName = file.FileName;

                if (!CheckFileType(uploadConfig, uploadFileName))
                {
                    return Content(JsonConvert.SerializeObject(new
                    {
                        state = GetStateMessage(UeditorUploadState.TypeNotAllow)
                    }));
                }
                if (!CheckFileSize(uploadConfig, file.ContentLength))
                {
                    return Content(JsonConvert.SerializeObject(new
                    {
                        state = GetStateMessage(UeditorUploadState.SizeLimitExceed)
                    }));
                }

                uploadFileBytes = new byte[file.ContentLength];
                try
                {
                    file.InputStream.Read(uploadFileBytes, 0, file.ContentLength);
                }
                catch (Exception)
                {
                    return Content(JsonConvert.SerializeObject(new
                    {
                        state = GetStateMessage(UeditorUploadState.NetworkError)
                    }));
                }
            }

            result.OriginFileName = uploadFileName;

            var savePath = UeditorPathFormatter.Format(uploadFileName, uploadConfig.PathFormat);
            var localPath = Server.MapPath(savePath).Replace("\\Utility\\", "\\ueditor\\");// +"/ueditor/net";
            try
            {
                if (!Directory.Exists(Path.GetDirectoryName(localPath)))
                {
                    Directory.CreateDirectory(Path.GetDirectoryName(localPath));
                }
                System.IO.File.WriteAllBytes(localPath, uploadFileBytes);
                result.Url = savePath;
                result.State = UeditorUploadState.Success;
            }
            catch (Exception e)
            {
                result.State = UeditorUploadState.FileAccessError;
                result.ErrorMessage = e.Message;
            }

            return Content(JsonConvert.SerializeObject(new
            {
                state = GetStateMessage(result.State),
                url = result.Url,
                title = result.OriginFileName,
                original = result.OriginFileName,
                error = result.ErrorMessage
            }));
        }
        /// <summary>
        /// 百度编辑器的文件列表管理
        /// </summary>
        /// <param name="pathToList">文件列表目录</param>
        /// <param name="searchExtensions">扩展名</param>
        /// <returns></returns>
        public ActionResult ListFileManager(string pathToList, string[] searchExtensions)
        {
            int Start;
            int Size;
            int Total;
            String[] FileList;
            String[] SearchExtensions;
            SearchExtensions = searchExtensions.Select(x => x.ToLower()).ToArray();
            try
            {
                Start = String.IsNullOrEmpty(Request["start"]) ? 0 : Convert.ToInt32(Request["start"]);
                Size = String.IsNullOrEmpty(Request["size"]) ? UeditorConfig.GetInt("imageManagerListSize") : Convert.ToInt32(Request["size"]);
            }
            catch (FormatException)
            {
                return Content(JsonConvert.SerializeObject(new
                {
                    state = "参数不正确",
                    start = 0,
                    size = 0,
                    total = 0
                }));
            }
            var buildingList = new List<String>();
            try
            {
                var localPath = Server.MapPath(pathToList).Replace("\\Utility\\", "\\ueditor\\");
                buildingList.AddRange(Directory.GetFiles(localPath, "*", SearchOption.AllDirectories)
                    .Where(x => SearchExtensions.Contains(Path.GetExtension(x).ToLower()))
                    .Select(x => pathToList + x.Substring(localPath.Length).Replace("\\", "/")));
                Total = buildingList.Count;
                FileList = buildingList.OrderBy(x => x).Skip(Start).Take(Size).ToArray();
            }
            catch (UnauthorizedAccessException)
            {
                return Content(JsonConvert.SerializeObject(new
                {
                    state = "文件系统权限不足",
                    start = 0,
                    size = 0,
                    total = 0
                }));
            }
            catch (DirectoryNotFoundException)
            {
                return Content(JsonConvert.SerializeObject(new
                {
                    state = "路径不存在",
                    start = 0,
                    size = 0,
                    total = 0
                }));
            }
            catch (IOException)
            {
                return Content(JsonConvert.SerializeObject(new
                {
                    state = "文件系统读取错误",
                    start = 0,
                    size = 0,
                    total = 0
                }));
            }

            return Content(JsonConvert.SerializeObject(new
            {
                state = "SUCCESS",
                list = FileList == null ? null : FileList.Select(x => new { url = x }),
                start = Start,
                size = Size,
                total = Total
            }));
        }

        public ActionResult CrawlerHandler()
        {
            string[] sources = Request.Form.GetValues("source[]");
            if (sources == null || sources.Length == 0)
            {
                return Content(JsonConvert.SerializeObject(new
                {
                    state = "参数错误：没有指定抓取源"
                }));
            }

            UeditorCrawler[] crawlers = sources.Select(x => new UeditorCrawler(x).Fetch()).ToArray();
            return Content(JsonConvert.SerializeObject(new
            {
                state = "SUCCESS",
                list = crawlers.Select(x => new
                {
                    state = x.State,
                    source = x.SourceUrl,
                    url = x.ServerUrl
                })
            }));
        }
        private string GetStateMessage(UeditorUploadState state)
        {
            switch (state)
            {
                case UeditorUploadState.Success:
                    return "SUCCESS";
                case UeditorUploadState.FileAccessError:
                    return "文件访问出错，请检查写入权限";
                case UeditorUploadState.SizeLimitExceed:
                    return "文件大小超出服务器限制";
                case UeditorUploadState.TypeNotAllow:
                    return "不允许的文件格式";
                case UeditorUploadState.NetworkError:
                    return "网络错误";
            }
            return "未知错误";
        }
        /// <summary>
        /// 检测是否符合上传文件格式
        /// </summary>
        /// <param name="uploadConfig">配置信息</param>
        /// <param name="filename">文件名字</param>
        /// <returns></returns>
        private bool CheckFileType(UeditorUploadConfig uploadConfig, string filename)
        {
            var fileExtension = Path.GetExtension(filename).ToLower();
            var res = false;
            foreach (var item in uploadConfig.AllowExtensions)
            {
                if (item == fileExtension)
                {
                    res = true;
                    break;
                }
            }
            return res;
        }
        /// <summary>
        /// 检测是否符合上传文件大小
        /// </summary>
        /// <param name="uploadConfig">配置信息</param>
        /// <param name="size">文件大小</param>
        /// <returns></returns>
        private bool CheckFileSize(UeditorUploadConfig uploadConfig, int size)
        {
            return size < uploadConfig.SizeLimit;
        }


        #endregion
    }
}