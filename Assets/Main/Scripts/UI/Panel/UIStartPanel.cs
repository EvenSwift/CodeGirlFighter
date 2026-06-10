using Main.Scripts.UI.Base;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using QFramework;

namespace Main.Scripts.UI.Panel
{
    public class UIStartPanelData : UIPanelData
    {
    }

    public partial class UIStartPanel : UIEnhancedInputPanel
    {
        private GameInputSystem _input;

        #region InputLifecycle

        protected override void OnBindInput()
        {
            _input = new GameInputSystem();
            _input.Enable();
            _input.UI.Submit.performed += OnSubmit;
        }

        protected override void OnUnbindInput()
        {
            if (_input == null) return;
            _input.UI.Submit.performed -= OnSubmit;
            _input.Disable();
            _input.Dispose();
            _input = null;
        }

        #endregion

        protected override void OnInit(IUIData uiData = null)
        {
            mData = uiData as UIStartPanelData ?? new UIStartPanelData();
        }

        protected override void OnOpen(IUIData uiData = null)
        {
            BtnStart.onClick.AddListener(OnBtnStartClick);
            BtnSaves.onClick.AddListener(OnBtnSavesClick);
            BtnSettings.onClick.AddListener(OnBtnSettingsClick);
        }

        protected override void OnShow()
        {
        }

        protected override void OnHide()
        {
        }

        protected override void OnClose()
        {
            BtnStart.onClick.RemoveListener(OnBtnStartClick);
            BtnSaves.onClick.RemoveListener(OnBtnSavesClick);
            BtnSettings.onClick.RemoveListener(OnBtnSettingsClick);
        }

        #region UIEvent

        private void OnBtnStartClick()
        {
            UIKitEx.CloseCurrentPanel();
            UIKitEx.OpenPanelWithInput<UIMainPanel>();
        }

        private void OnBtnSavesClick()
        {
        }

        private void OnBtnSettingsClick()
        {
        }

        #endregion

        #region UIInput

        private void OnSubmit(InputAction.CallbackContext context)
        {
            OnBtnStartClick();
        }

        #endregion
    }
}