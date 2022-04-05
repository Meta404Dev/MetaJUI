
# MetaJUI v0.2

MetaJUI是为JEngine定制的UI框架，当然你也可以通过很简单的修改，移植到自己的工程项目

# 快速使用

## 1.导入到项目

将 **UnityProject** 目录的下的所有文件全部Copy到自己的Unity工程目录下

## 2.创建自己的UI预制体

打开Unity，点击上方的 **MetaTools/UI自动生成器** （快捷键：Ctrl+Shift+Alt+U）

- 在预制体自动生成设置中，输入你想要创建的UI名字，这里我们以 **UITest** 为例
- 输入完成后，点击 **生成UI预制体** 按钮，此时，在路径**Assets/HotUpdateResources/UI/** 中会出现刚才生成的UI预制体
- 编辑UI，具体的UI控件的命名规则查看 **Assets/3rd/MetaJUI/Editor/UIAutoCreate/Template/UIViewAutoCreateConfig** 

## 3.自动生成代码
- 把UI预制体拖到View根节点处
- 如果是第一次生成代码，点击 **生成MVC代码** ，如果生成成功，日志窗口会打印出生成的代码路径，默认的代码生成路径在 **/HotUpdateScripts/Game/UI/**
- 如果你用的是VisualStudio，需要在解决方案资源管理器中点击**刷新**，点击**显示所有文件**，然后右键文件夹**包括在项目中**
- 
## 4.游戏中的使用方法

打开普通UI
```
UITool.Open<UITest>();
```

关闭普通UI
```
UITool.Close<UITest>();
```

打开栈UI
```
UITool.OpenStack<UITest1>();
UITool.OpenStack<UITest2>();
UITool.OpenStack<UITest3>();
```
关闭栈UI
```
UITool.CloseStack();
UITool.CloseStack();
UITool.CloseStack();
```
获取到一个UI
```
UITool.Get<UITest>();
```
清除所有UI，包括缓存
```
UITool.Clear();
```

## 5.自定义路径

打开**Assets/3rd/MetaJUI/Editor/UIAutoCreate/UIAutoCreatePathSetting** 
所有路径都可以自定义修改

## 6.修改UI生成代码模板

模板文件在**Assets/3rd/MetaJUI/Editor/UIAutoCreate/Template/**  目录下
可以自由修改为你想要的模板

## 7.使用虚拟列表


# MetaJUI优势
- 使用简单，自动生成UI预制体，自动生成UI代码
- 可以自定义UI模板
- UI代码完全脱离Monobehavior，可以轻松的完成热更

# 更新日志
## v0.2
- 新增了虚拟列表

设置好虚拟列表预制体**ScrollViewVertical**

在热更层的UI打开界面进行注册

注册高度
```
GetView().scrollTestRect.OnHeight += (index) => { return 150; };
```
注册刷新事件
```
GetView().scrollTestRect.OnFill += (index, go) =>
        {
            go.GetComponentInChildren<Text>().text = index.ToString();
        };
```
初始化列表大小
```
GetView().scrollTestRect.InitData(100);
```



# Todo
- 红点系统
- 常用的UI集合
-- UI确认框
-- UI消息Tip