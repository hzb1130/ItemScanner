#nullable disable

using System.Reflection;

namespace ItemScanner
{
    // ==================== Enum Definitions ====================
    public enum MarkerColor
    {
        Red,
        Yellow,
        Blue,
        Green,
        Orange,
        Black,
        White
    }

    public enum MarkerShape
    {
        Circle,
        Square,
        Triangle
    }

    public enum HideItemsFilter
    {
        None,
        Stone,
        Stick,
        StoneAndStick
    }

    public class ItemScannerSettings : JsonModSettings
    {
        // ==================== Basic Settings ====================
        [Section("Basic Settings")]
        
        [Name("Scan Key")]
        [Description("Hold this key to display item markers")]
        public KeyCode scanKey = KeyCode.LeftAlt;

        [Name("Scan Interval")]
        [Description("Scan interval in seconds\nLower = more frequent scans, Higher = better performance")]
        [Slider(0.1f, 2.0f, 20)]
        public float scanInterval = 0.5f;

        [Name("Scan Radius")]
        [Description("Detection radius for items (meters)")]
        [Slider(1, 50)]
        public int scanRadius = 25;

        // ==================== Gear Scan Settings ====================
        [Section("Gear Items")]
        
        [Name("Enable Gear Scan")]
        [Description("Enable scanning for gear items")]
        public bool scanGear = false;

        [Name("Gear Marker Color")]
        [Description("Color of gear item markers")]
        public MarkerColor gearColor = MarkerColor.Red;

        [Name("Gear Marker Shape")]
        [Description("Shape of gear item markers")]
        public MarkerShape gearShape = MarkerShape.Circle;

        [Name("Gear Marker Size")]
        [Description("Size of gear item markers")]
        [Slider(10, 50)]
        public int gearMarkerSize = 20;

        [Name("Show Picked Items")]
        [Description("Display items that have been picked up before")]
        public bool showInventoryItems = false;

        [Name("Hide Items Filter")]
        [Description("Filter to hide specific items")]
        public HideItemsFilter hideItemsFilter = HideItemsFilter.None;

        // ==================== Container Scan Settings ====================
        [Section("Containers")]
        
        [Name("Enable Container Scan")]
        [Description("Enable scanning for containers")]
        public bool scanContainers = false;

        [Name("Container Marker Color")]
        [Description("Color of container markers")]
        public MarkerColor containerColor = MarkerColor.Blue;

        [Name("Container Marker Shape")]
        [Description("Shape of container markers")]
        public MarkerShape containerShape = MarkerShape.Square;

        [Name("Container Marker Size")]
        [Description("Size of container markers")]
        [Slider(10, 50)]
        public int containerMarkerSize = 20;

        [Name("Hide Opened Containers")]
        [Description("Hide containers that have been opened")]
        public bool hideOpenedContainers = true;

        // ==================== Plant Scan Settings ====================
        [Section("Plants")]
        
        [Name("Enable Plant Scan")]
        [Description("Enable scanning for harvestable plants")]
        public bool scanPlants = false;

        [Name("Plant Marker Color")]
        [Description("Color of plant markers")]
        public MarkerColor plantColor = MarkerColor.Green;

        [Name("Plant Marker Shape")]
        [Description("Shape of plant markers")]
        public MarkerShape plantShape = MarkerShape.Triangle;

        [Name("Plant Marker Size")]
        [Description("Size of plant markers")]
        [Slider(10, 50)]
        public int plantMarkerSize = 20;

        // ==================== Color Conversion Methods ====================
        public Color GetColorFromEnum(MarkerColor colorEnum)
        {
            switch (colorEnum)
            {
                case MarkerColor.Red:
                    return new Color(1f, 0f, 0f);
                case MarkerColor.Yellow:
                    return new Color(1f, 1f, 0f);
                case MarkerColor.Blue:
                    return new Color(0f, 0.6f, 1f);
                case MarkerColor.Green:
                    return new Color(0f, 1f, 0f);
                case MarkerColor.Orange:
                    return new Color(1f, 0.65f, 0f);
                case MarkerColor.Black:
                    return new Color(0f, 0f, 0f);
                case MarkerColor.White:
                    return new Color(1f, 1f, 1f);
                default:
                    return Color.white;
            }
        }

        public Color GetGearMarkerColor()
        {
            return GetColorFromEnum(gearColor);
        }

        public Color GetContainerMarkerColor()
        {
            return GetColorFromEnum(containerColor);
        }

        public Color GetPlantMarkerColor()
        {
            return GetColorFromEnum(plantColor);
        }

        protected override void OnChange(FieldInfo field, object oldValue, object newValue)
        {
            // Gear scan collapse control
            if (field.Name == nameof(scanGear))
            {
                bool visible = (bool)newValue;
                SetFieldVisible(nameof(gearColor), visible);
                SetFieldVisible(nameof(gearShape), visible);
                SetFieldVisible(nameof(gearMarkerSize), visible);
                SetFieldVisible(nameof(showInventoryItems), visible);
                SetFieldVisible(nameof(hideItemsFilter), visible);
            }
            
            // Container scan collapse control
            if (field.Name == nameof(scanContainers))
            {
                bool visible = (bool)newValue;
                SetFieldVisible(nameof(containerColor), visible);
                SetFieldVisible(nameof(containerShape), visible);
                SetFieldVisible(nameof(containerMarkerSize), visible);
                SetFieldVisible(nameof(hideOpenedContainers), visible);
            }
            
            // Plant scan collapse control
            if (field.Name == nameof(scanPlants))
            {
                bool visible = (bool)newValue;
                SetFieldVisible(nameof(plantColor), visible);
                SetFieldVisible(nameof(plantShape), visible);
                SetFieldVisible(nameof(plantMarkerSize), visible);
            }
        }

        protected override void OnConfirm()
        {
            base.OnConfirm();
        }
    }

    public static class Settings
    {
        public static ItemScannerSettings options = null;

        public static void OnLoad()
        {
            options = new ItemScannerSettings();
            options.AddToModSettings("Item Scanner");
            
            // Initialize collapse states
            UpdateGearVisibility(options.scanGear);
            UpdateContainerVisibility(options.scanContainers);
            UpdatePlantVisibility(options.scanPlants);
        }

        internal static void UpdateGearVisibility(bool visible)
        {
            options.SetFieldVisible(nameof(options.gearColor), visible);
            options.SetFieldVisible(nameof(options.gearShape), visible);
            options.SetFieldVisible(nameof(options.gearMarkerSize), visible);
            options.SetFieldVisible(nameof(options.showInventoryItems), visible);
            options.SetFieldVisible(nameof(options.hideItemsFilter), visible);
        }

        internal static void UpdateContainerVisibility(bool visible)
        {
            options.SetFieldVisible(nameof(options.containerColor), visible);
            options.SetFieldVisible(nameof(options.containerShape), visible);
            options.SetFieldVisible(nameof(options.containerMarkerSize), visible);
            options.SetFieldVisible(nameof(options.hideOpenedContainers), visible);
        }

        internal static void UpdatePlantVisibility(bool visible)
        {
            options.SetFieldVisible(nameof(options.plantColor), visible);
            options.SetFieldVisible(nameof(options.plantShape), visible);
            options.SetFieldVisible(nameof(options.plantMarkerSize), visible);
        }
    }
}
