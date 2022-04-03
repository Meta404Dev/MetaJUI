using System.Collections;
using UnityEngine;

namespace MetaFramework.UI
{
    public interface IUIBase
    {
        string uiName { get; set; }
        GameObject uiGo { get; set; }
        IUIModel uiModel { get; set; }
        IUIView uiView { get; set; }

        UILayer GetLayer();

        void OnEnter(params object[] args);

        void OnPause();

        void OnResume();

        void OnExit();

        void OnUpdate();

    }
}
