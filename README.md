# ReadBookOnWeb
## 1.在浏览器端自己编辑目录上传内容，并以类似于word目录浏览似的分章节浏览内容，可全文检索并设置字体大小

## 2.web端采用asp.net mvc，数据库为MSSQL


## 3.功能主要采用ztree和ueditor插件完成

## 4.文件夹db为数据库脚本，code为web端代码
# 5.web端目录：
### （1）Database
### （2）DbFactory
*(主要采用工厂模式，可切换底层数据库，本项目采用MSSQL,数据库采用了比较轻型的Dapper)*
### （3）Module
*(业务逻辑层，DAO和Bll,此处业务分为了两块，一个添加书本业务，一个是编辑目录业务)*
### （4）web端

![enter image description here](http://47.98.220.197:85/web%E7%AB%AF%E8%AF%B4%E6%98%8E.png)

## 功能演示
###   1.添加书
![enter image description here](http://47.98.220.197:85/addbook.gif)
### 2.编辑目录

![enter image description here](http://47.98.220.197:85/editbook.gif)
### 3. 查看书籍

![enter image description here](http://47.98.220.197:85/readbook.gif)


# 说明
这是第一版，还有很多不足，后续有时间在改进

# 项目演示地址
http://47.98.220.197:8085/

