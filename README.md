# ReadBookOnWeb
在浏览器端自己编辑目录上传内容，并以类似于word目录浏览似的分章节浏览内容，可全文检索并设置字体大小
web端采用asp.net mvc，数据库为MSSQL
功能主要采用ztree和ueditor插件完成
文件夹db为数据库脚本，code为web端代码
web端目录：
1.Database
2.DbFactory
(主要采用工厂模式，可切换底层数据库，本项目采用MSSQL,数据库采用了比较轻型的Dapper)
3.Module
(业务逻辑层，DAO和Bll,此处业务分为了两块，一个添加书本业务，一个是编辑目录业务)
4.web端

