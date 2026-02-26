#nullable disable

using System.Reflection;

namespace ItemScanner
{
    public enum UILanguage
    {
        English,
        中文
    }

    public enum MarkerColor
    {
        Red, Yellow, Blue, Green, Orange, Black, White
    }

    public enum MarkerShape
    {
        Circle, Square, Triangle
    }

    public enum HideItemsFilter
    {
        None, Stone, Stick, StoneAndStick
    }

    public enum ScanActivationMode
    {
        HoldKey,
        ToggleKey
    }

    public class ItemScannerSettings : JsonModSettings
    {
        // ==================== Language ====================
        [Section("Language / 语言")]
        [Name("Language / 语言")]
        [Description("Select display language / 选择显示语言\nChanges take effect immediately / 更改立即生效")]
        public UILanguage language = UILanguage.English;

        // ==================================================
        // ==================== ENGLISH =====================
        // ==================================================

        [Section("Basic Settings")]
        [Name("Activation Mode")]
        [Description("HoldKey: Hold the key to scan\nToggleKey: Press once to enable, press again to disable")]
        public ScanActivationMode en_scanActivationMode = ScanActivationMode.HoldKey;

        [Name("Scan Key")]
        [Description("Hold/press this key to show item markers")]
        public KeyCode en_scanKey = KeyCode.LeftAlt;

        [Name("Scan Interval")]
        [Description("Scan interval in seconds\nLower value = more frequent scans, Higher value = better performance")]
        [Slider(0.1f, 2.0f, 20)]
        public float en_scanInterval = 0.5f;

        [Name("Scan Radius")]
        [Description("Item detection radius in meters")]
        [Slider(1, 200)]
        public int en_scanRadius = 25;

        [Section("Gear Items")]
        [Name("Enable Gear Scan")]
        [Description("Enable scanning for gear items on the ground")]
        public bool en_scanGear = false;

        [Name("Gear Marker Color")]
        [Description("Color of the gear item markers")]
        public MarkerColor en_gearColor = MarkerColor.Red;

        [Name("Gear Marker Shape")]
        [Description("Shape of the gear item markers")]
        public MarkerShape en_gearShape = MarkerShape.Circle;

        [Name("Gear Marker Size")]
        [Description("Size of the gear item markers")]
        [Slider(10, 50)]
        public int en_gearMarkerSize = 20;

        [Name("Show Previously Picked Items")]
        [Description("Show items that have been picked up before")]
        public bool en_showInventoryItems = false;

        [Name("Hide Items Filter")]
        [Description("Select which items to hide from the scan")]
        public HideItemsFilter en_hideItemsFilter = HideItemsFilter.None;

        [Section("Containers")]
        [Name("Enable Container Scan")]
        [Description("Enable scanning for containers")]
        public bool en_scanContainers = false;

        [Name("Container Marker Color")]
        [Description("Color of the container markers")]
        public MarkerColor en_containerColor = MarkerColor.Blue;

        [Name("Container Marker Shape")]
        [Description("Shape of the container markers")]
        public MarkerShape en_containerShape = MarkerShape.Square;

        [Name("Container Marker Size")]
        [Description("Size of the container markers")]
        [Slider(10, 50)]
        public int en_containerMarkerSize = 20;

        [Name("Hide Opened Containers")]
        [Description("Hide containers that have already been opened")]
        public bool en_hideOpenedContainers = true;

        [Section("Plants")]
        [Name("Enable Plant Scan")]
        [Description("Enable scanning for harvestable plants")]
        public bool en_scanPlants = false;

        [Name("Plant Marker Color")]
        [Description("Color of the plant markers")]
        public MarkerColor en_plantColor = MarkerColor.Green;

        [Name("Plant Marker Shape")]
        [Description("Shape of the plant markers")]
        public MarkerShape en_plantShape = MarkerShape.Triangle;

        [Name("Plant Marker Size")]
        [Description("Size of the plant markers")]
        [Slider(10, 50)]
        public int en_plantMarkerSize = 20;

        // ==================================================
        // ==================== 中文 ========================
        // ==================================================

        [Section("基础设置")]
        [Name("激活模式")]
        [Description("HoldKey：长按按键时扫描\nToggleKey：按一次开启，再按一次关闭")]
        public ScanActivationMode cn_scanActivationMode = ScanActivationMode.HoldKey;

        [Name("扫描按键")]
        [Description("按住/点击此键显示物品标记")]
        public KeyCode cn_scanKey = KeyCode.LeftAlt;

        [Name("扫描间隔")]
        [Description("扫描间隔秒数\n数值越低扫描越频繁，数值越高性能越好")]
        [Slider(0.1f, 2.0f, 20)]
        public float cn_scanInterval = 0.5f;

        [Name("扫描半径")]
        [Description("物品检测半径（米）")]
        [Slider(1, 200)]
        public int cn_scanRadius = 25;

        [Section("物品扫描")]
        [Name("启用物品扫描")]
        [Description("启用物品扫描功能")]
        public bool cn_scanGear = false;

        [Name("物品标记颜色")]
        [Description("物品标记的颜色")]
        public MarkerColor cn_gearColor = MarkerColor.Red;

        [Name("物品标记形状")]
        [Description("物品标记的形状")]
        public MarkerShape cn_gearShape = MarkerShape.Circle;

        [Name("物品标记大小")]
        [Description("物品标记的大小")]
        [Slider(10, 50)]
        public int cn_gearMarkerSize = 20;

        [Name("已拾取过物品")]
        [Description("是否显示已拾取过的物品")]
        public bool cn_showInventoryItems = false;

        [Name("隐藏石头树枝")]
        [Description("隐藏选择的物品")]
        public HideItemsFilter cn_hideItemsFilter = HideItemsFilter.None;

        [Section("容器扫描")]
        [Name("启用容器扫描")]
        [Description("启用容器扫描功能")]
        public bool cn_scanContainers = false;

        [Name("容器标记颜色")]
        [Description("容器标记的颜色")]
        public MarkerColor cn_containerColor = MarkerColor.Blue;

        [Name("容器标记形状")]
        [Description("容器标记的形状")]
        public MarkerShape cn_containerShape = MarkerShape.Square;

        [Name("容器标记大小")]
        [Description("容器标记的大小")]
        [Slider(10, 50)]
        public int cn_containerMarkerSize = 20;

        [Name("隐藏已开启的容器")]
        [Description("隐藏已经开启过的容器")]
        public bool cn_hideOpenedContainers = true;

        [Section("植物扫描")]
        [Name("启用植物扫描")]
        [Description("启用可采集植物扫描功能")]
        public bool cn_scanPlants = false;

        [Name("植物标记颜色")]
        [Description("植物标记的颜色")]
        public MarkerColor cn_plantColor = MarkerColor.Green;

        [Name("植物标记形状")]
        [Description("植物标记的形状")]
        public MarkerShape cn_plantShape = MarkerShape.Triangle;

        [Name("植物标记大小")]
        [Description("植物标记的大小")]
        [Slider(10, 50)]
        public int cn_plantMarkerSize = 20;

        // ==================================================
        // ========== Unified Accessors (IS.cs 使用) ========
        // ==================================================
        public ScanActivationMode scanActivationMode => language == UILanguage.English ? en_scanActivationMode : cn_scanActivationMode;
        public KeyCode            scanKey            => language == UILanguage.English ? en_scanKey            : cn_scanKey;
        public float              scanInterval       => language == UILanguage.English ? en_scanInterval       : cn_scanInterval;
        public int                scanRadius         => language == UILanguage.English ? en_scanRadius         : cn_scanRadius;

        public bool            scanGear          => language == UILanguage.English ? en_scanGear          : cn_scanGear;
        public MarkerColor     gearColor         => language == UILanguage.English ? en_gearColor         : cn_gearColor;
        public MarkerShape     gearShape         => language == UILanguage.English ? en_gearShape         : cn_gearShape;
        public int             gearMarkerSize    => language == UILanguage.English ? en_gearMarkerSize    : cn_gearMarkerSize;
        public bool            showInventoryItems=> language == UILanguage.English ? en_showInventoryItems : cn_showInventoryItems;
        public HideItemsFilter hideItemsFilter   => language == UILanguage.English ? en_hideItemsFilter   : cn_hideItemsFilter;

        public bool        scanContainers      => language == UILanguage.English ? en_scanContainers      : cn_scanContainers;
        public MarkerColor containerColor      => language == UILanguage.English ? en_containerColor      : cn_containerColor;
        public MarkerShape containerShape      => language == UILanguage.English ? en_containerShape      : cn_containerShape;
        public int         containerMarkerSize => language == UILanguage.English ? en_containerMarkerSize : cn_containerMarkerSize;
        public bool        hideOpenedContainers=> language == UILanguage.English ? en_hideOpenedContainers: cn_hideOpenedContainers;

        public bool        scanPlants      => language == UILanguage.English ? en_scanPlants      : cn_scanPlants;
        public MarkerColor plantColor      => language == UILanguage.English ? en_plantColor      : cn_plantColor;
        public MarkerShape plantShape      => language == UILanguage.English ? en_plantShape      : cn_plantShape;
        public int         plantMarkerSize => language == UILanguage.English ? en_plantMarkerSize : cn_plantMarkerSize;

        // ==================== Color Methods ====================
        public Color GetColorFromEnum(MarkerColor colorEnum)
        {
            switch (colorEnum)
            {
                case MarkerColor.Red:    return new Color(1f, 0f, 0f);
                case MarkerColor.Yellow: return new Color(1f, 1f, 0f);
                case MarkerColor.Blue:   return new Color(0f, 0.6f, 1f);
                case MarkerColor.Green:  return new Color(0f, 1f, 0f);
                case MarkerColor.Orange: return new Color(1f, 0.65f, 0f);
                case MarkerColor.Black:  return new Color(0f, 0f, 0f);
                case MarkerColor.White:  return new Color(1f, 1f, 1f);
                default:                 return Color.white;
            }
        }

        public Color GetGearMarkerColor()      => GetColorFromEnum(gearColor);
        public Color GetContainerMarkerColor() => GetColorFromEnum(containerColor);
        public Color GetPlantMarkerColor()     => GetColorFromEnum(plantColor);

        // ==================== OnChange ====================
        protected override void OnChange(FieldInfo field, object oldValue, object newValue)
        {
            // 语言切换：显示/隐藏对应语言的字段组
            if (field.Name == nameof(language))
            {
                UILanguage lang = (UILanguage)newValue;
                bool isEN = lang == UILanguage.English;
                bool isCN = lang == UILanguage.中文;

                SetEnglishVisible(isEN);
                SetChineseVisible(isCN);

                // 切换语言时把另一套的值同步过来，保持设置不丢失
                if (isEN)   SyncCNtoEN();
                else        SyncENtoCN();

                // 切换后重新应用折叠状态
                if (isEN)
                {
                    ApplyENGearCollapse(en_scanGear);
                    ApplyENContainerCollapse(en_scanContainers);
                    ApplyENPlantCollapse(en_scanPlants);
                }
                else
                {
                    ApplyCNGearCollapse(cn_scanGear);
                    ApplyCNContainerCollapse(cn_scanContainers);
                    ApplyCNPlantCollapse(cn_scanPlants);
                }
                return;
            }

            // ---- English collapse ----
            if (field.Name == nameof(en_scanGear))       ApplyENGearCollapse((bool)newValue);
            if (field.Name == nameof(en_scanContainers)) ApplyENContainerCollapse((bool)newValue);
            if (field.Name == nameof(en_scanPlants))     ApplyENPlantCollapse((bool)newValue);

            // ---- Chinese collapse ----
            if (field.Name == nameof(cn_scanGear))       ApplyCNGearCollapse((bool)newValue);
            if (field.Name == nameof(cn_scanContainers)) ApplyCNContainerCollapse((bool)newValue);
            if (field.Name == nameof(cn_scanPlants))     ApplyCNPlantCollapse((bool)newValue);
        }

        protected override void OnConfirm()
        {
            base.OnConfirm();
        }

        // ==================== Collapse Helpers ====================
        private void ApplyENGearCollapse(bool visible)
        {
            SetFieldVisible(nameof(en_gearColor),          visible);
            SetFieldVisible(nameof(en_gearShape),          visible);
            SetFieldVisible(nameof(en_gearMarkerSize),     visible);
            SetFieldVisible(nameof(en_showInventoryItems), visible);
            SetFieldVisible(nameof(en_hideItemsFilter),    visible);
        }

        private void ApplyENContainerCollapse(bool visible)
        {
            SetFieldVisible(nameof(en_containerColor),        visible);
            SetFieldVisible(nameof(en_containerShape),        visible);
            SetFieldVisible(nameof(en_containerMarkerSize),   visible);
            SetFieldVisible(nameof(en_hideOpenedContainers),  visible);
        }

        private void ApplyENPlantCollapse(bool visible)
        {
            SetFieldVisible(nameof(en_plantColor),      visible);
            SetFieldVisible(nameof(en_plantShape),      visible);
            SetFieldVisible(nameof(en_plantMarkerSize), visible);
        }

        private void ApplyCNGearCollapse(bool visible)
        {
            SetFieldVisible(nameof(cn_gearColor),          visible);
            SetFieldVisible(nameof(cn_gearShape),          visible);
            SetFieldVisible(nameof(cn_gearMarkerSize),     visible);
            SetFieldVisible(nameof(cn_showInventoryItems), visible);
            SetFieldVisible(nameof(cn_hideItemsFilter),    visible);
        }

        private void ApplyCNContainerCollapse(bool visible)
        {
            SetFieldVisible(nameof(cn_containerColor),       visible);
            SetFieldVisible(nameof(cn_containerShape),       visible);
            SetFieldVisible(nameof(cn_containerMarkerSize),  visible);
            SetFieldVisible(nameof(cn_hideOpenedContainers), visible);
        }

        private void ApplyCNPlantCollapse(bool visible)
        {
            SetFieldVisible(nameof(cn_plantColor),      visible);
            SetFieldVisible(nameof(cn_plantShape),      visible);
            SetFieldVisible(nameof(cn_plantMarkerSize), visible);
        }

        // ==================== Language Block Visibility ====================
        private void SetEnglishVisible(bool visible)
        {
            SetFieldVisible(nameof(en_scanActivationMode), visible);
            SetFieldVisible(nameof(en_scanKey),            visible);
            SetFieldVisible(nameof(en_scanInterval),       visible);
            SetFieldVisible(nameof(en_scanRadius),         visible);

            SetFieldVisible(nameof(en_scanGear),           visible);
            SetFieldVisible(nameof(en_scanContainers),     visible);
            SetFieldVisible(nameof(en_scanPlants),         visible);

            if (visible)
            {
                ApplyENGearCollapse(en_scanGear);
                ApplyENContainerCollapse(en_scanContainers);
                ApplyENPlantCollapse(en_scanPlants);
            }
            else
            {
                ApplyENGearCollapse(false);
                ApplyENContainerCollapse(false);
                ApplyENPlantCollapse(false);
            }
        }

        private void SetChineseVisible(bool visible)
        {
            SetFieldVisible(nameof(cn_scanActivationMode), visible);
            SetFieldVisible(nameof(cn_scanKey),            visible);
            SetFieldVisible(nameof(cn_scanInterval),       visible);
            SetFieldVisible(nameof(cn_scanRadius),         visible);

            SetFieldVisible(nameof(cn_scanGear),           visible);
            SetFieldVisible(nameof(cn_scanContainers),     visible);
            SetFieldVisible(nameof(cn_scanPlants),         visible);

            if (visible)
            {
                ApplyCNGearCollapse(cn_scanGear);
                ApplyCNContainerCollapse(cn_scanContainers);
                ApplyCNPlantCollapse(cn_scanPlants);
            }
            else
            {
                ApplyCNGearCollapse(false);
                ApplyCNContainerCollapse(false);
                ApplyCNPlantCollapse(false);
            }
        }

        // ==================== Value Sync ====================
        private void SyncENtoCN()
        {
            cn_scanActivationMode  = en_scanActivationMode;
            cn_scanKey             = en_scanKey;
            cn_scanInterval        = en_scanInterval;
            cn_scanRadius          = en_scanRadius;
            cn_scanGear            = en_scanGear;
            cn_gearColor           = en_gearColor;
            cn_gearShape           = en_gearShape;
            cn_gearMarkerSize      = en_gearMarkerSize;
            cn_showInventoryItems  = en_showInventoryItems;
            cn_hideItemsFilter     = en_hideItemsFilter;
            cn_scanContainers      = en_scanContainers;
            cn_containerColor      = en_containerColor;
            cn_containerShape      = en_containerShape;
            cn_containerMarkerSize = en_containerMarkerSize;
            cn_hideOpenedContainers= en_hideOpenedContainers;
            cn_scanPlants          = en_scanPlants;
            cn_plantColor          = en_plantColor;
            cn_plantShape          = en_plantShape;
            cn_plantMarkerSize     = en_plantMarkerSize;
        }

        private void SyncCNtoEN()
        {
            en_scanActivationMode  = cn_scanActivationMode;
            en_scanKey             = cn_scanKey;
            en_scanInterval        = cn_scanInterval;
            en_scanRadius          = cn_scanRadius;
            en_scanGear            = cn_scanGear;
            en_gearColor           = cn_gearColor;
            en_gearShape           = cn_gearShape;
            en_gearMarkerSize      = cn_gearMarkerSize;
            en_showInventoryItems  = cn_showInventoryItems;
            en_hideItemsFilter     = cn_hideItemsFilter;
            en_scanContainers      = cn_scanContainers;
            en_containerColor      = cn_containerColor;
            en_containerShape      = cn_containerShape;
            en_containerMarkerSize = cn_containerMarkerSize;
            en_hideOpenedContainers= cn_hideOpenedContainers;
            en_scanPlants          = cn_scanPlants;
            en_plantColor          = cn_plantColor;
            en_plantShape          = cn_plantShape;
            en_plantMarkerSize     = cn_plantMarkerSize;
        }
    }

    public static class Settings
    {
        public static ItemScannerSettings options = null;

        public static void OnLoad()
        {
            options = new ItemScannerSettings();
            options.AddToModSettings("Item Scanner");

            // 初始化：根据默认语言显示对应字段
            bool isEN = options.language == UILanguage.English;
            options.SetFieldVisible(nameof(options.en_scanActivationMode), isEN);
            options.SetFieldVisible(nameof(options.en_scanKey),            isEN);
            options.SetFieldVisible(nameof(options.en_scanInterval),       isEN);
            options.SetFieldVisible(nameof(options.en_scanRadius),         isEN);
            options.SetFieldVisible(nameof(options.en_scanGear),           isEN);
            options.SetFieldVisible(nameof(options.en_scanContainers),     isEN);
            options.SetFieldVisible(nameof(options.en_scanPlants),         isEN);

            bool isCN = options.language == UILanguage.中文;
            options.SetFieldVisible(nameof(options.cn_scanActivationMode), isCN);
            options.SetFieldVisible(nameof(options.cn_scanKey),            isCN);
            options.SetFieldVisible(nameof(options.cn_scanInterval),       isCN);
            options.SetFieldVisible(nameof(options.cn_scanRadius),         isCN);
            options.SetFieldVisible(nameof(options.cn_scanGear),           isCN);
            options.SetFieldVisible(nameof(options.cn_scanContainers),     isCN);
            options.SetFieldVisible(nameof(options.cn_scanPlants),         isCN);

            // 初始折叠状态
            if (isEN)
            {
                options.SetFieldVisible(nameof(options.en_gearColor),          options.en_scanGear);
                options.SetFieldVisible(nameof(options.en_gearShape),          options.en_scanGear);
                options.SetFieldVisible(nameof(options.en_gearMarkerSize),     options.en_scanGear);
                options.SetFieldVisible(nameof(options.en_showInventoryItems), options.en_scanGear);
                options.SetFieldVisible(nameof(options.en_hideItemsFilter),    options.en_scanGear);
                options.SetFieldVisible(nameof(options.en_containerColor),       options.en_scanContainers);
                options.SetFieldVisible(nameof(options.en_containerShape),       options.en_scanContainers);
                options.SetFieldVisible(nameof(options.en_containerMarkerSize),  options.en_scanContainers);
                options.SetFieldVisible(nameof(options.en_hideOpenedContainers), options.en_scanContainers);
                options.SetFieldVisible(nameof(options.en_plantColor),      options.en_scanPlants);
                options.SetFieldVisible(nameof(options.en_plantShape),      options.en_scanPlants);
                options.SetFieldVisible(nameof(options.en_plantMarkerSize), options.en_scanPlants);
            }
            else
            {
                options.SetFieldVisible(nameof(options.cn_gearColor),          options.cn_scanGear);
                options.SetFieldVisible(nameof(options.cn_gearShape),          options.cn_scanGear);
                options.SetFieldVisible(nameof(options.cn_gearMarkerSize),     options.cn_scanGear);
                options.SetFieldVisible(nameof(options.cn_showInventoryItems), options.cn_scanGear);
                options.SetFieldVisible(nameof(options.cn_hideItemsFilter),    options.cn_scanGear);
                options.SetFieldVisible(nameof(options.cn_containerColor),       options.cn_scanContainers);
                options.SetFieldVisible(nameof(options.cn_containerShape),       options.cn_scanContainers);
                options.SetFieldVisible(nameof(options.cn_containerMarkerSize),  options.cn_scanContainers);
                options.SetFieldVisible(nameof(options.cn_hideOpenedContainers), options.cn_scanContainers);
                options.SetFieldVisible(nameof(options.cn_plantColor),      options.cn_scanPlants);
                options.SetFieldVisible(nameof(options.cn_plantShape),      options.cn_scanPlants);
                options.SetFieldVisible(nameof(options.cn_plantMarkerSize), options.cn_scanPlants);
            }
        }
    }
}
