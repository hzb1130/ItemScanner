#nullable disable

using System.Reflection;

namespace ItemScanner
{
    public enum MarkerColor
    {
        Red, Yellow, Blue, Green, Orange, Black, White
    }

    public enum MarkerShape
    {
        Circle,
        Square,
        Triangle,
        None
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

    public enum AlwaysShowItem
    {
        None,
        Arrow
    }

    public class ItemScannerSettings : JsonModSettings
    {
        // ==================== Basic ====================
        [Section("Basic Settings / 基础设置")]

        [Name("Enable Scanner / 启用扫描系统")]
        [Description("Enable or disable the entire scanner system. / 启用或关闭整个扫描系统。")]
        public bool enableScanner = true;

        [Name("Activation Mode / 激活模式")]
        [Description("Choose how the scanner is activated: hold key or toggle. / 选择扫描激活方式：按住或切换。")]
        public ScanActivationMode scanActivationMode = ScanActivationMode.HoldKey;

        [Name("Scan Key / 扫描按键")]
        [Description("Key used to activate the scanner. / 用于激活扫描的按键。")]
        public KeyCode scanKey = KeyCode.LeftAlt;

        [Name("Show Activation Hint / 显示启用提示框")]
        [Description("Show a small hint in the bottom right corner of the screen when the scanner is active. / 扫描启用时在屏幕右下角显示提示。")]
        public bool showActivationHint = true;

        [Name("Scan Interval / 扫描间隔")]
        [Description("Time between scans in seconds. Lower = more responsive, higher = better performance. / 扫描间隔（秒），越低越灵敏，越高性能越好。")]
        [Slider(0.1f, 2.0f, 20)]
        public float scanInterval = 0.5f;

        [Name("Scan Radius / 扫描半径")]
        [Description("Maximum distance to detect items around the player. / 扫描玩家周围物体的最大距离。")]
        [Slider(1, 300)]
        public int scanRadius = 25;


        // ==================== Gear ====================
        [Section("Gear Scan / 物品扫描")]

        [Name("Enable Gear Scan / 启用物品扫描")]
        [Description("Enable scanning for loose gear items. / 启用地面物品扫描。")]
        public bool scanGear = false;

        [Name("Marker Shape / 标记形状")]
        [Description("Shape used to mark detected gear items. / 标记物品的形状。")]
        public MarkerShape gearShape = MarkerShape.Circle;

        [Name("Show Name / 显示名称")]
        [Description("Display item name on screen. / 显示物品名称。")]
        public bool showGearName = false;

        [Name("Show Distance / 显示距离")]
        [Description("Display distance to item. / 显示物品距离。")]
        public bool showGearDistance = true;

        [Name("Marker Color / 标记颜色")]
        [Description("Color of gear markers. / 物品标记颜色。")]
        public MarkerColor gearColor = MarkerColor.Red;

        [Name("Marker Size / 标记大小")]
        [Description("Size of the gear marker. / 物品标记大小。")]
        [Slider(10, 50)]
        public int gearMarkerSize = 20;

        [Name("Font Size / 字号大小")]
        [Description("Font size for item labels. / 物品文字大小。")]
        [Slider(10, 40)]
        public int gearFontSize = 18;

        [Name("Show Picked Items / 显示已拾取物品")]
        [Description("Show items that have been picked up before. / 显示已拾取过的物品。")]
        public bool showInventoryItems = false;

       [Name("Hide Common Items / 过滤常见物品")]
        [Description("Hide common items like stones or sticks. / 过滤常见物品，例如石头或树枝。")]
        public HideItemsFilter hideItemsFilter = HideItemsFilter.None;

        [Name("Always Show Arrows / 始终显示箭")]
        [Description("Always show arrows regardless of filters or pickup state. / 无论过滤或拾取状态，始终显示箭。")]
        public AlwaysShowItem alwaysShowItem = AlwaysShowItem.None;


        // ==================== Containers ====================
        [Section("Container Scan / 容器扫描")]

        [Name("Enable Container Scan / 启用容器扫描")]
        [Description("Enable scanning for containers like lockers or cabinets. / 启用容器扫描（柜子、箱子等）。")]
        public bool scanContainers = false;

        [Name("Marker Shape / 标记形状")]
        [Description("Shape used to mark containers. / 容器标记形状。")]
        public MarkerShape containerShape = MarkerShape.Square;

        [Name("Show Name / 显示名称")]
        [Description("Display container name. / 显示容器名称。")]
        public bool showContainerName = false;

        [Name("Show Distance / 显示距离")]
        [Description("Display distance to container. / 显示容器距离。")]
        public bool showContainerDistance = true;

        [Name("Marker Color / 标记颜色")]
        [Description("Color of container markers. / 容器标记颜色。")]
        public MarkerColor containerColor = MarkerColor.Blue;

        [Name("Marker Size / 标记大小")]
        [Description("Size of the container marker. / 容器标记大小。")]
        [Slider(10, 50)]
        public int containerMarkerSize = 20;

        [Name("Font Size / 字号大小")]
        [Description("Font size for container labels. / 容器文字大小。")]
        [Slider(10, 40)]
        public int containerFontSize = 18;

        [Name("Hide Opened Containers / 隐藏已开启容器")]
        [Description("Hide containers that have already been searched. / 隐藏已搜索的容器。")]
        public bool hideOpenedContainers = true;


        // ==================== Plants ====================
        [Section("Plant Scan / 植物扫描")]

        [Name("Enable Plant Scan / 启用植物扫描")]
        [Description("Enable scanning for harvestable plants. / 启用可采集植物扫描。")]
        public bool scanPlants = false;

        [Name("Marker Shape / 标记形状")]
        [Description("Shape used to mark plants. / 植物标记形状。")]
        public MarkerShape plantShape = MarkerShape.Triangle;

        [Name("Show Name / 显示名称")]
        [Description("Display plant name. / 显示植物名称。")]
        public bool showPlantName = false;

        [Name("Show Distance / 显示距离")]
        [Description("Display distance to plant. / 显示植物距离。")]
        public bool showPlantDistance = true;

        [Name("Marker Color / 标记颜色")]
        [Description("Color of plant markers. / 植物标记颜色。")]
        public MarkerColor plantColor = MarkerColor.Green;

        [Name("Marker Size / 标记大小")]
        [Description("Size of the plant marker. / 植物标记大小。")]
        [Slider(10, 50)]
        public int plantMarkerSize = 20;

        [Name("Font Size / 字号大小")]
        [Description("Font size for plant labels. / 植物文字大小。")]
        [Slider(10, 40)]
        public int plantFontSize = 18;

        // ==================== Color ====================
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


        // ==================== UI Logic ====================
        protected override void OnChange(FieldInfo field, object oldValue, object newValue)
        {
            base.OnChange(field, oldValue, newValue);
            RefreshFields(); 
        }
        public void RefreshFields()
        {
            SetFieldVisible(nameof(scanActivationMode), enableScanner);
            SetFieldVisible(nameof(scanKey), enableScanner);
            SetFieldVisible(nameof(showActivationHint), enableScanner);

            SetFieldVisible(nameof(gearShape), scanGear);
            SetFieldVisible(nameof(showGearName), scanGear);
            SetFieldVisible(nameof(showGearDistance), scanGear);
            SetFieldVisible(nameof(gearColor), scanGear);
            SetFieldVisible(nameof(gearMarkerSize), scanGear);
            SetFieldVisible(nameof(gearFontSize), scanGear);
            SetFieldVisible(nameof(hideItemsFilter), scanGear);
            SetFieldVisible(nameof(alwaysShowItem), scanGear);
            SetFieldVisible(nameof(showInventoryItems), scanGear);

            SetFieldVisible(nameof(containerShape), scanContainers);
            SetFieldVisible(nameof(showContainerName), scanContainers);
            SetFieldVisible(nameof(showContainerDistance), scanContainers);
            SetFieldVisible(nameof(containerColor), scanContainers);
            SetFieldVisible(nameof(containerMarkerSize), scanContainers);
            SetFieldVisible(nameof(containerFontSize), scanContainers);
            SetFieldVisible(nameof(hideOpenedContainers), scanContainers);

            SetFieldVisible(nameof(plantShape), scanPlants);
            SetFieldVisible(nameof(showPlantName), scanPlants);
            SetFieldVisible(nameof(showPlantDistance), scanPlants);
            SetFieldVisible(nameof(plantColor), scanPlants);
            SetFieldVisible(nameof(plantMarkerSize), scanPlants);
            SetFieldVisible(nameof(plantFontSize), scanPlants);
        }
    }


    public static class Settings
    {
        public static ItemScannerSettings options = null;
        
        public static void OnLoad()
        {
            options = new ItemScannerSettings();
            options.AddToModSettings("Item Scanner v1.0.4");
            // options.AddToModSettings("物品扫描器v1.0.4");
            options.RefreshFields();
        }
    }
}