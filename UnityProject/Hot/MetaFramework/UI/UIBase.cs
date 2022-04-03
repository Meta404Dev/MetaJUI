using UnityEngine;

namespace MetaFramework.UI
{
    /// <summary>
    /// 所有UI应该继承UIBase，并规定Model和View的泛型
    /// </summary>
    /// <typeparam name="M">指定Model的类</typeparam>
    /// <typeparam name="V">指定View的类</typeparam>
    public abstract class UIBase<M, V> : IUIBase
        where M : class, IUIModel
        where V : class, IUIView
    {
        /// <summary>
        /// ui name = 类名 = 预制体名
        /// </summary>
        public string uiName { get; set; }

        /// <summary>
        /// UI根节点
        /// </summary>
        public GameObject uiGo { get; set; }

        /// <summary>
        /// 数据层
        /// </summary>
        public IUIModel uiModel { get; set; }

        /// <summary>
        /// 显示层
        /// </summary>
        public IUIView uiView { get; set; }

        public M GetModel()
        {
            return uiModel as M;
        }

        public V GetView()
        {
            return uiView as V;
        }

        public abstract UILayer GetLayer();


        #region 生命周期
        /// <summary>
        /// 界面第一次打开
        /// </summary>
        public virtual void OnEnter(params object[] args) { }
        /// <summary>
        /// 界面暂停（Stack）
        /// </summary>
        public virtual void OnPause() { }
        /// <summary>
        /// 界面恢复（Stack）
        /// </summary>
        public virtual void OnResume() { }
        /// <summary>
        /// 界面关闭
        /// </summary>
        public virtual void OnExit() { }
        /// <summary>
        /// 界面刷新
        /// </summary>
        public virtual void OnUpdate() { }

        #endregion
    }
}