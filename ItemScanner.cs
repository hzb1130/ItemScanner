#nullable disable

using System.Collections.Generic;
using Il2Cpp;
using UnityEngine;
namespace ItemScanner
{
    public class ItemScanner : MelonMod
    {
        private static int gearLayerMask;
        // private static int allLayerMask = ~0;

        private class ItemInfo
        {
            public Vector3 worldPosition;
            public float distance;
            public ItemType type;
            public string name;
        }

        private class TypeRenderConfig
        {
            public MarkerShape shape;
            public bool showName;
            public bool showDistance;
            public Color color;
            public int markerSize;
            public int fontSize;
        }

        private enum ItemType
        {
            Gear,
            Container,
            Plant
        }

        private List<ItemInfo> detectedItems = new List<ItemInfo>();

        private TypeRenderConfig gearConfig = new TypeRenderConfig();
        private TypeRenderConfig containerConfig = new TypeRenderConfig();
        private TypeRenderConfig plantConfig = new TypeRenderConfig();

        private GUIStyle gearStyle;
        private GUIStyle containerStyle;
        private GUIStyle plantStyle;
        private Texture2D gearTexture;
        private Texture2D containerTexture;
        private Texture2D plantTexture;
        private float lastScanTime = 0f;
        private bool toggleState = false;
        private bool lastKeyState = false;
        private bool isDisplaying = false;
        private bool showHint = false;
        private bool _needsRegenerateStyles = false;
        private int _lastResourceHash = -1;

        public override void OnInitializeMelon()
        {
            gearLayerMask = 1 << 17;
            Settings.OnLoad();
        }

        public override void OnUpdate()
        {
            if (Settings.options == null || !Settings.options.enableScanner)
            {
                isDisplaying = false;
                detectedItems.Clear();
                showHint = false;
                return;
            }
            bool key = Input.GetKey(Settings.options.scanKey);
            bool active = false;

            if (Settings.options.scanActivationMode == ScanActivationMode.HoldKey)
                active = key;
            else
            {
                if (key && !lastKeyState)
                    toggleState = !toggleState;

                active = toggleState;
            }

            lastKeyState = key;

            if (active)
            {
                isDisplaying = true;

                if (Time.time - lastScanTime >= Settings.options.scanInterval)
                {
                    DetectItems();
                    lastScanTime = Time.time;
                }

                showHint = Settings.options.showActivationHint;
            }
            else
            {
                isDisplaying = false;
                detectedItems.Clear();
                showHint = false;
            }
        }
        public override void OnGUI()
        {
            if (Settings.options == null)
                return;

            if (showHint && isDisplaying)
                DrawHint();

            if (!isDisplaying || detectedItems.Count == 0)
                return;

            if (_needsRegenerateStyles)
            {
                RegenerateStyles();
                _needsRegenerateStyles = false;
            }

            Camera cam = GetCamera();
            if (cam == null)
                return;

            foreach (var item in detectedItems)
                DrawItem(item, cam);
                
        }

        // ================= 渲染 =================

        private void DrawItem(ItemInfo item, Camera cam)
        {
            var config = GetConfig(item.type);
            if (config == null)
                return;

            Vector3 pos = cam.WorldToScreenPoint(item.worldPosition);
            if (pos.z <= 0)
                return;

            pos.y = Screen.height - pos.y;

            float markerSize = config.markerSize * 2f;

            // ==================== 1. 绘制图形 ====================
            if (config.shape != MarkerShape.None)
            {
                Texture2D tex = GetTexture(item.type);

                if (tex != null)
                {
                    GUI.DrawTexture(new Rect(
                        pos.x - markerSize / 2,
                        pos.y - markerSize / 2,
                        markerSize,
                        markerSize
                    ), tex);
                }
            }

            // ==================== 2. 组装文本（同一行） ====================
            if (!config.showName && !config.showDistance)
                return;

            string text = "";

            if (config.showName)
                text += item.name;

            if (config.showDistance)
            {
                if (text.Length > 0)
                    text += " ";

                text += $"[{(int)item.distance}m]";
            }

            // ==================== 3. 获取样式 ====================
            GUIStyle style = GetStyle(item.type);
            if (style == null)
                return;

            // ==================== 4. 避免遮挡（关键） ====================
            float textOffsetY = 0f;

            if (config.shape != MarkerShape.None)
                textOffsetY = markerSize / 2 + 5f;  // 在图形下方
            else
                textOffsetY = 0f; // 没图形直接居中

            // ==================== 5. 绘制文本 ====================
            Vector2 textSize = style.CalcSize(new GUIContent(text));

            GUI.Label(new Rect(
                pos.x - textSize.x / 2,
                pos.y + textOffsetY,
                textSize.x,
                textSize.y
            ), text, style);
        }

        // ================= 扫描 =================

        private void DetectItems()
        {
            detectedItems.Clear();

            var pm = GameManager.GetPlayerManagerComponent();
            if (pm == null)
                return;

            Vector3 p = GameManager.GetPlayerTransform().position;
            float r = Settings.options.scanRadius;

            if (Settings.options.scanGear)
                ScanGear(p, r);

            if (Settings.options.scanContainers)
                ScanContainers(p, r);

            if (Settings.options.scanPlants)
                ScanPlants(p, r);

            detectedItems.Sort((a, b) => a.distance.CompareTo(b.distance));
            UpdateTexturesIfNeeded();
            
        }

        private void ScanGear(Vector3 playerPos, float radius)
        {
            Collider[] colliders = Physics.OverlapSphere(
                playerPos,
                radius,
                gearLayerMask,
                QueryTriggerInteraction.Collide
            );

            HashSet<int> processed = new HashSet<int>();
            
            foreach (var collider in colliders)
            {
                GearItem gearItem = collider.GetComponentInParent<GearItem>();
                if (gearItem == null || gearItem.gameObject == null)
                    continue;

                int id = gearItem.gameObject.GetInstanceID();
                if (processed.Contains(id))
                    continue;

                if (!gearItem.gameObject.activeInHierarchy)
                    continue;

                // ==================== AlwaysShowArrow ====================
                bool isArrow = gearItem.name.Contains("GEAR_Arrow");
                bool alwaysShowArrow =
                    Settings.options.alwaysShowItem == AlwaysShowItem.Arrow && isArrow;

                // ==================== 已拾取过滤（只被 AlwaysShow 覆盖） ====================
                if (!alwaysShowArrow)
                {
                    if (gearItem.m_InPlayerInventory)
                        continue;

                    if (!Settings.options.showInventoryItems && gearItem.m_BeenInPlayerInventory)
                        continue;
                }

                // ==================== 其他过滤（不会被 AlwaysShow 覆盖） ====================
                switch (Settings.options.hideItemsFilter)
                {
                    case HideItemsFilter.Stone:
                        if (gearItem.name.Contains("GEAR_Stone"))
                            continue;
                        break;

                    case HideItemsFilter.Stick:
                        if (gearItem.name.Contains("GEAR_Stick"))
                            continue;
                        break;

                    case HideItemsFilter.StoneAndStick:
                        if (gearItem.name.Contains("GEAR_Stone") ||
                            gearItem.name.Contains("GEAR_Stick"))
                            continue;
                        break;
                }

                // ==================== 添加 ====================
                Vector3 pos = gearItem.transform.position;

                string displayName = gearItem.name;
                try
                {
                    displayName = gearItem.DisplayName;
                }
                catch { }

                detectedItems.Add(new ItemInfo
                {
                    worldPosition = pos,
                    distance = Vector3.Distance(playerPos, pos),
                    type = ItemType.Gear,
                    name = displayName
                });

                processed.Add(id);
            }
        }


        private void ScanContainers(Vector3 p, float r)
        {
            var cols = Physics.OverlapSphere(p, r);
            HashSet<int> ids = new HashSet<int>();

            foreach (var c in cols)
            {
                var obj = c.GetComponentInParent<Container>();
                if (obj == null || !obj.gameObject.activeInHierarchy || !obj.enabled)
                    continue;

                int id = obj.gameObject.GetInstanceID();
                if (ids.Contains(id))
                    continue;

                if (Settings.options.hideOpenedContainers && obj.IsInspected())
                    continue;

                Vector3 pos = obj.transform.position;

                detectedItems.Add(new ItemInfo
                {
                    worldPosition = pos,
                    distance = Vector3.Distance(p, pos),
                    type = ItemType.Container,
                    name = obj.LocalizedDisplayName.Text()
                });

                ids.Add(id);
            }
        }

        private void ScanPlants(Vector3 p, float r)
        {
            var cols = Physics.OverlapSphere(p, r);
            HashSet<int> ids = new HashSet<int>();

            foreach (var c in cols)
            {
                var obj = c.GetComponentInParent<Harvestable>();
                if (obj == null || !obj.gameObject.activeInHierarchy)
                    continue;

                int id = obj.gameObject.GetInstanceID();
                if (ids.Contains(id))
                    continue;

                // 过滤：已采集 or 非植物
                if (obj.m_Harvested || !obj.RegisterAsPlantsHaversted)
                    continue;

                Vector3 pos = obj.transform.position;

                // ==================== 获取显示名称（核心修改） ====================
                string displayName = obj.name;

                try
                {
                    if (obj.m_GearPrefab != null)
                    {
                        GearItem gear = obj.m_GearPrefab.GetComponent<GearItem>();
                        if (gear != null && !string.IsNullOrEmpty(gear.DisplayName))
                        {
                            displayName = gear.DisplayName;
                        }
                    }
                }
                catch { }

                // ==================== 添加 ====================
                detectedItems.Add(new ItemInfo
                {
                    worldPosition = pos,
                    distance = Vector3.Distance(p, pos),
                    type = ItemType.Plant,
                    name = displayName
                });

                ids.Add(id);
            }
        }

        // ================= 配置 =================

        private TypeRenderConfig GetConfig(ItemType type)
        {
            return type == ItemType.Gear ? gearConfig :
                   type == ItemType.Container ? containerConfig :
                   plantConfig;
        }

        private Texture2D GetTexture(ItemType type)
        {
            return type == ItemType.Gear ? gearTexture :
                   type == ItemType.Container ? containerTexture :
                   plantTexture;
        }

        private GUIStyle GetStyle(ItemType type)
        {
            return type == ItemType.Gear ? gearStyle :
                   type == ItemType.Container ? containerStyle :
                   plantStyle;
        }
        private void UpdateTexturesIfNeeded()
        {
            // 计算 hash（不变则不重建）
            int hash =
                Settings.options.gearMarkerSize ^
                Settings.options.containerMarkerSize ^
                Settings.options.plantMarkerSize ^
                (int)Settings.options.gearColor ^
                (int)Settings.options.containerColor ^
                (int)Settings.options.plantColor ^
                (int)Settings.options.gearShape ^
                (int)Settings.options.containerShape ^
                (int)Settings.options.plantShape ^
                Settings.options.gearFontSize ^
                Settings.options.containerFontSize ^
                Settings.options.plantFontSize ^
                (Settings.options.showGearName ? 1 : 0) ^
                (Settings.options.showGearDistance ? 2 : 0) ^
                (Settings.options.showContainerName ? 4 : 0) ^
                (Settings.options.showContainerDistance ? 8 : 0) ^
                (Settings.options.showPlantName ? 16 : 0) ^
                (Settings.options.showPlantDistance ? 32 : 0);

            bool texturesMissing =
                (gearConfig.shape != MarkerShape.None && gearTexture == null) ||
                (containerConfig.shape != MarkerShape.None && containerTexture == null) ||
                (plantConfig.shape != MarkerShape.None && plantTexture == null);

            if (hash == _lastResourceHash && !texturesMissing)
                return;

            // 需要重建
            _lastResourceHash = hash;
            _needsRegenerateStyles = true;

            // 更新配置
            BuildConfig(gearConfig, Settings.options.gearShape,
                Settings.options.showGearName, Settings.options.showGearDistance,
                Settings.options.gearColor, Settings.options.gearMarkerSize,
                Settings.options.gearFontSize);

            BuildConfig(containerConfig, Settings.options.containerShape,
                Settings.options.showContainerName, Settings.options.showContainerDistance,
                Settings.options.containerColor, Settings.options.containerMarkerSize,
                Settings.options.containerFontSize);

            BuildConfig(plantConfig, Settings.options.plantShape,
                Settings.options.showPlantName, Settings.options.showPlantDistance,
                Settings.options.plantColor, Settings.options.plantMarkerSize,
                Settings.options.plantFontSize);

            // 安全重建纹理
            RegenerateTextures();
        }
        

        private void BuildConfig(TypeRenderConfig cfg, MarkerShape shape, bool name, bool dist,
            MarkerColor color, int size, int font)
        {
            cfg.shape = shape;
            cfg.showName = name;
            cfg.showDistance = dist;
            cfg.color = Settings.options.GetColorFromEnum(color);
            cfg.markerSize = size;
            cfg.fontSize = font;
        }

        private void RegenerateTextures()
        {
            gearTexture = CreateShapeTexture(gearConfig);
            containerTexture = CreateShapeTexture(containerConfig);
            plantTexture = CreateShapeTexture(plantConfig);
        }
        private Texture2D CreateShapeTexture(TypeRenderConfig cfg)
        {
            if (cfg.shape == MarkerShape.None)
                return null;

            int size = cfg.markerSize * 2;
            float thickness = 3f;

            switch (cfg.shape)
            {
                case MarkerShape.Circle:
                    return CreateCircleTexture(size, thickness, cfg.color);

                case MarkerShape.Square:
                    return CreateSquareTexture(size, thickness, cfg.color);

                case MarkerShape.Triangle:
                    return CreateTriangleTexture(size, thickness, cfg.color);

                default:
                    return null;
            }
        }
        private Texture2D CreateCircleTexture(int size, float thickness, Color color)
        {
            Texture2D tex = new Texture2D(size, size);

            float center = size / 2f;
            float outer = center - 1f;
            float inner = outer - thickness;

            for (int x = 0; x < size; x++)
            {
                for (int y = 0; y < size; y++)
                {
                    float dist = Mathf.Sqrt((x - center) * (x - center) + (y - center) * (y - center));

                    tex.SetPixel(x, y,
                        (dist >= inner && dist <= outer) ? color : Color.clear);
                }
            }

            tex.Apply();
            return tex;
        }
        private Texture2D CreateSquareTexture(int size, float thickness, Color color)
        {
            Texture2D tex = new Texture2D(size, size);
            int border = (int)thickness;

            for (int x = 0; x < size; x++)
            {
                for (int y = 0; y < size; y++)
                {
                    bool edge =
                        x < border || x >= size - border ||
                        y < border || y >= size - border;

                    tex.SetPixel(x, y, edge ? color : Color.clear);
                }
            }

            tex.Apply();
            return tex;
        }
        private Texture2D CreateTriangleTexture(int size, float thickness, Color color)
        {
            Texture2D tex = new Texture2D(size, size);
            tex.filterMode = FilterMode.Bilinear;

            float margin = size * 0.1f;

            // 倒置三角形：底边在上方，顶点在底部
            Vector2 bottomLeft  = new Vector2(margin, size - margin);
            Vector2 bottomRight = new Vector2(size - margin, size - margin);

            float side = bottomRight.x - bottomLeft.x;
            float height = side * 0.866f;  

            Vector2 top = new Vector2(size / 2f, size - margin - height);

            for (int x = 0; x < size; x++)
            {
                for (int y = 0; y < size; y++)
                {
                    Vector2 p = new Vector2(x, y);

                    float d1 = DistanceToSegment(p, bottomLeft, bottomRight);
                    float d2 = DistanceToSegment(p, bottomRight, top); 
                    float d3 = DistanceToSegment(p, top, bottomLeft);

                    float minDist = Mathf.Min(d1, Mathf.Min(d2, d3));

                    tex.SetPixel(x, y,
                        (minDist <= thickness) ? color : Color.clear);
                }
            }

            tex.Apply();
            return tex;
        }

        private float DistanceToSegment(Vector2 p, Vector2 a, Vector2 b)
        {
            Vector2 ab = b - a;
            float t = Vector2.Dot(p - a, ab) / ab.sqrMagnitude;
            t = Mathf.Clamp01(t);
            Vector2 closest = a + t * ab;
            return Vector2.Distance(p, closest);
        }

        private void RegenerateStyles()
        {
            gearStyle = CreateStyle(gearConfig);
            containerStyle = CreateStyle(containerConfig);
            plantStyle = CreateStyle(plantConfig);
        }

        private GUIStyle CreateStyle(TypeRenderConfig cfg)
        {
            var s = new GUIStyle(GUI.skin.label);
            s.wordWrap = false;   // 必须
            s.clipping = TextClipping.Overflow; // 防止裁剪
            s.normal.textColor = cfg.color;
            s.fontSize = cfg.fontSize;
            s.alignment = TextAnchor.MiddleCenter;
            return s;
        }

        private void DrawHint()
        {
            float width = 140f;
            float height = 24f;
            float margin = 10f;

            Rect rect = new Rect(
                Screen.width - width - margin,
                Screen.height - height - margin,
                width,
                height
            );

            GUI.Box(rect, "Scanner Active");
            // GUI.Box(rect, "扫描中……");

        }

        private Camera GetCamera()
        {
            Camera[] allCameras = Camera.allCameras;
            foreach (Camera cam in allCameras)
            {
                if (cam != null && cam.enabled && cam.gameObject.activeInHierarchy)
                {
                    if (cam.name == "CameraGlobalRT")
                        return cam;
                }
            }
            return null;
        }
    }
}