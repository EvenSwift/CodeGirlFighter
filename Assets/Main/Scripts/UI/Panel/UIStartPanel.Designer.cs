using System;
using UnityEngine;
using UnityEngine.UI;
using QFramework;

namespace Main.Scripts.UI.Panel
{
	// Generate Id:e015aa7b-a858-4ec6-b0e2-8271246ee9be
	public partial class UIStartPanel
	{
		public const string Name = "UIStartPanel";
		
		[SerializeField]
		public Main.Scripts.UI.Base.UIEnhancedButton BtnStart;
		[SerializeField]
		public Main.Scripts.UI.Base.UIEnhancedButton BtnSaves;
		[SerializeField]
		public Main.Scripts.UI.Base.UIEnhancedButton BtnSettings;
		
		private UIStartPanelData mPrivateData = null;
		
		protected override void ClearUIComponents()
		{
			BtnStart = null;
			BtnSaves = null;
			BtnSettings = null;
			
			mData = null;
		}
		
		public UIStartPanelData Data
		{
			get
			{
				return mData;
			}
		}
		
		UIStartPanelData mData
		{
			get
			{
				return mPrivateData ?? (mPrivateData = new UIStartPanelData());
			}
			set
			{
				mUIData = value;
				mPrivateData = value;
			}
		}
	}
}
