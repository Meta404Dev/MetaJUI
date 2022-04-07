


# MetaJUI v0.4

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

因为热更层使用到了框架层的委托类型，第一次使用要进行ILRuntime的委托注册
RegisterMethodDelegateHelper.Register()
```
appdomain.DelegateManager.RegisterMethodDelegate<Int32, GameObject>();
```
RegisterFunctionDelegateHelper.Register()
```
appdomain.DelegateManager.RegisterFunctionDelegate<Int32, Int32>();
```

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



## 8.使用公共UI

公共UI需要以**UICommon**前缀命名，生成的代码也会将UI预制体的名字作为类名，所以需要避免重复
- 将公共UI作为一个单独的预制体进行编辑
- 编辑完了将公共UI拖入你想要的目标UI下面
- 生成公共UI的MVC代码
- 生成目标UI的View代码


## 9.使用红点系统

初始化结构（以我自己的项目为例, RedPointSystemConst为一个枚举）
```
        RedPointNode mainNode = new RedPointNode(RedPointSystemConst.main.ToString());
        {
            //家园
            RedPointNode jiayuanNode = mainNode.AddChildNode(RedPointSystemConst.家园.ToString());
            {
                //仓库
                RedPointNode cangkuNode = jiayuanNode.AddChildNode(RedPointSystemConst.家园_仓库.ToString());
                {
                    cangkuNode.AddChildNode(RedPointSystemConst.家园_仓库_装备.ToString());
                }

                //孵化槽
                RedPointNode fuhuacaoNode = jiayuanNode.AddChildNode(RedPointSystemConst.家园_孵化槽.ToString());
                {
                    fuhuacaoNode.AddChildNode(RedPointSystemConst.家园_孵化槽_孵化蛋.ToString());
                    fuhuacaoNode.AddChildNode(RedPointSystemConst.家园_孵化槽_抽奖券.ToString());
                    fuhuacaoNode.AddChildNode(RedPointSystemConst.家园_孵化槽_配对.ToString());
                }
            }

            //宠物
            RedPointNode chongwuNode = mainNode.AddChildNode(RedPointSystemConst.宠物.ToString());
            {
                //宠物卡片
                RedPointNode kapianNode = chongwuNode.AddChildNode(RedPointSystemConst.宠物_卡片.ToString());
                {
                    RedPointNode kapianshuxingNode = kapianNode.AddChildNode(RedPointSystemConst.宠物_卡片_详情属性.ToString());
                    {
                        kapianshuxingNode.AddChildNode(RedPointSystemConst.宠物_卡片_详情属性_升级.ToString());
                    }
                    kapianNode.AddChildNode(RedPointSystemConst.宠物_卡片_详情进化.ToString());
                    kapianNode.AddChildNode(RedPointSystemConst.宠物_卡片_详情觉醒.ToString());
                }
            }
        }
        
        rps = new RedPointSystems();
        rps.Init(mainNode);
```

注册红点数量变化事件
```

        RedPointSystemManager.Ins.rps.Register(RedPointSystemConst.家园.ToString(), (node) =>
        {
                ShowRedPoint(node.pointCount > 0);
        });
        
        ...
        
        RedPointSystemManager.Ins.rps.Register(RedPointSystemConst.家园_仓库.ToString(), (node) =>
        {
                ShowRedPoint(node.pointCount > 0);
        });
        
        ...
        
        RedPointSystemManager.Ins.rps.Register(RedPointSystemConst.家园_仓库_装备.ToString(), (node) =>
        {
                ShowRedPoint(node.pointCount > 0);
        });
```
派发事件
```

        RedPointSystemManager.Ins.rps.Dispatch(
            RedPointSystemConst.家园_仓库_装备.ToString(),
            PlayerData.GetNewItemData());

```

# MetaJUI优势
- 使用简单，自动生成UI预制体，自动生成UI代码
- 可以自定义UI模板
- UI代码完全脱离Monobehavior，可以轻松的完成热更

# 更新日志
## v0.4（2022.4.5）
- 新增了虚拟列表
- 新增了公共UI
- 新增了红点系统


# TodoList
- 常用的UI集合
- UI确认框
- UI消息Tip

# 支持我
- 通过支付宝请我喝一杯奶茶（支付宝账号：463056265@qq.com）
- 点击Star，你们的Star是我更新的动力

# 友情链接
[JEngine](https://github.com/JasonXuDeveloper/JEngine) 小白也能快速上手，轻松制作可以热更新的游戏
