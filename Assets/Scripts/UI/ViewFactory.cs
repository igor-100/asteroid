using Core;
using Core.ResourceEnums;
using UI.Pause;
using UnityEngine;
namespace UI
{
    public class ViewFactory : IViewFactory
    {
        private IUIRoot uiRoot;
        private IResourceManager resourceManager;

        public ViewFactory(IUIRoot uiRoot, IResourceManager resourceManager)
        {
            this.uiRoot = uiRoot;
            this.resourceManager = resourceManager;
        }
        
        public IPauseScreenView CreatePauseScreen() =>
            CreateView<IPauseScreenView>(EViews.PauseScreenView, uiRoot.Overlay3Canvas);

        private T CreateView<T>(EViews eView, Transform viewParent) where T : IView
        {
            var view = resourceManager.CreatePrefabInstance<T, EViews>(eView);
            view.SetParent(viewParent);

            return view;
        }
    }
}
