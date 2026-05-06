namespace Main.Scripts.UI.Base
{
    /// <summary>
    /// 实现此接口的面板由 UIKitEx 自动管理输入绑定
    /// </summary>
    public interface IPanelInput
    {
        /// <summary>
        /// 面板激活时调用，在此方法中订阅 InputSystem 事件
        /// </summary>
        void BindInput();

        /// <summary>
        /// 面板失活时调用，在此方法中取消订阅 InputSystem 事件
        /// </summary>
        void UnbindInput();
    }
}
