using System.Collections.Generic;
using QFramework;
using UnityEngine;

namespace CodeFighter.UI
{
    /// <summary>
    /// UIKit 扩展：栈式面板管理 + 自动输入切换
    /// </summary>
    public static class UIKitEx
    {
        private static readonly Stack<UIPanel> PanelStack = new();
        private static readonly HashSet<UIPanel> InputRegistry = new();

        /// <summary>
        /// 打开面板并自动管理输入（禁用前一个面板输入，启用新面板输入）
        /// </summary>
        public static T OpenPanelWithInput<T>(
            UILevel canvasLevel = UILevel.Common,
            IUIData uiData = null,
            string assetBundleName = null,
            string prefabName = null) where T : UIPanel, IPanelInput
        {
            if (PanelStack.TryPeek(out var current))
            {
                if (current is T)
                {
                    return current as T;
                }

                if (current is IPanelInput currentInput)
                {
                    currentInput.UnbindInput();
                }

                current.Hide();
            }

            var panel = UIKit.OpenPanel<T>(canvasLevel, uiData, assetBundleName, prefabName);
            PanelStack.Push(panel);

            if (panel is IPanelInput newInput)
            {
                newInput.BindInput();
                InputRegistry.Add(panel);
            }

            return panel;
        }

        /// <summary>
        /// 打开面板（PanelOpenType 重载）
        /// </summary>
        public static T OpenPanelWithInput<T>(
            PanelOpenType panelOpenType,
            UILevel canvasLevel = UILevel.Common,
            IUIData uiData = null,
            string assetBundleName = null,
            string prefabName = null) where T : UIPanel, IPanelInput
        {
            if (PanelStack.TryPeek(out var current) && current is IPanelInput currentInput)
            {
                currentInput.UnbindInput();
            }

            var panel = UIKit.OpenPanel<T>(panelOpenType, canvasLevel, uiData, assetBundleName, prefabName);
            PanelStack.Push(panel);

            if (panel is IPanelInput newInput)
            {
                newInput.BindInput();
                InputRegistry.Add(panel);
            }

            return panel;
        }

        /// <summary>
        /// 关闭当前面板，恢复上一个面板的输入
        /// </summary>
        public static void CloseCurrentPanel()
        {
            if (!PanelStack.TryPop(out var current)) return;

            if (current is IPanelInput currentInput)
            {
                currentInput.UnbindInput();
                InputRegistry.Remove(current);
            }

            UIKit.ClosePanel(current);

            if (PanelStack.TryPeek(out var previous))
            {
                previous.Show();

                if (previous is IPanelInput previousInput)
                {
                    previousInput.BindInput();
                }
            }
        }

        /// <summary>
        /// 获取当前栈顶面板
        /// </summary>
        public static UIPanel GetCurrentPanel()
        {
            return PanelStack.TryPeek(out var panel) ? panel : null;
        }

        /// <summary>
        /// 清空所有面板和输入
        /// </summary>
        public static void ClearAll()
        {
            foreach (var panel in InputRegistry)
            {
                if (panel is IPanelInput input)
                {
                    input.UnbindInput();
                }
            }

            UIKit.CloseAllPanel();
            InputRegistry.Clear();
            PanelStack.Clear();
        }

#if UNITY_EDITOR
        /// <summary>
        /// 调试用：打印面板栈信息
        /// </summary>
        public static string GetStackInfo()
        {
            var sb = new System.Text.StringBuilder("UI Stack (Top → Bottom):\n");
            int i = 0;
            foreach (var panel in PanelStack)
            {
                sb.AppendLine($"  [{i++}] {panel.name}  Input: {InputRegistry.Contains(panel)}");
            }

            return sb.ToString();
        }
#endif
    }
}
