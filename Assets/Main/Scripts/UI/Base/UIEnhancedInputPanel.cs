namespace Main.Scripts.UI.Base
{
    /// <summary>
    /// 集成输入绑定的增强面板基类，由 UIKitEx 自动管理输入生命周期
    /// </summary>
    public abstract class UIEnhancedInputPanel : UIEnhancedPanel, IPanelInput
    {
        private bool _inputBound;

        public void BindInput()
        {
            if (_inputBound) return;
            _inputBound = true;
            OnBindInput();
        }

        public void UnbindInput()
        {
            if (!_inputBound) return;
            _inputBound = false;
            OnUnbindInput();
        }

        /// <summary>
        /// 在此订阅 InputSystem 事件
        /// </summary>
        protected abstract void OnBindInput();

        /// <summary>
        /// 在此取消订阅 InputSystem 事件
        /// </summary>
        protected abstract void OnUnbindInput();

        protected override void OnClose()
        {
            UnbindInput();
            base.OnClose();
        }
    }
}
