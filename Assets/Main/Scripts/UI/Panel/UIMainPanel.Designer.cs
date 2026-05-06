namespace Main.Scripts.UI.Panel
{
	// Generate Id:ca063afd-e72f-4401-88f7-1d64fb1f3642
	public partial class UIMainPanel
	{
		public const string Name = "UIMainPanel";
		
		
		private UIMainPanelData mPrivateData = null;
		
		protected override void ClearUIComponents()
		{
			
			mData = null;
		}
		
		public UIMainPanelData Data
		{
			get
			{
				return mData;
			}
		}
		
		UIMainPanelData mData
		{
			get
			{
				return mPrivateData ?? (mPrivateData = new UIMainPanelData());
			}
			set
			{
				mUIData = value;
				mPrivateData = value;
			}
		}
	}
}
