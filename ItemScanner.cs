#nullable disable

using System.Collections.Generic;
using Il2Cpp;

namespace ItemScanner
{
    public class ItemScanner : MelonMod
    {
        private static int gearLayerMask;

        private class ItemInfo
        {
            public Vector3 worldPosition;
            public float distance;
            public ItemType type;
        }

        private enum ItemType
        {
            Gear,
            Container,
            Plant
        }

        private List<ItemInfo> detectedItems = new List<ItemInfo>();
        private bool isDisplaying = false;

        private Dictionary<string, Texture2D> textureCache = new Dictionary<string, Texture2D>();
        private int lastGearMarkerSize = -1;
        private int lastContainerMarkerSize = -1;
        private int lastPlantMarkerSize = -1;
        private MarkerColor lastGearColor;
        private MarkerColor lastContainerColor;
        private MarkerColor lastPlantColor;
        private MarkerShape lastGearShape;
        private MarkerShape lastContainerShape;
        private MarkerShape lastPlantShape;

        private float lastScanTime = 0f;

        public override void OnInitializeMelon()
        {
            MelonLogger.Msg("ItemScanner Mod Initialized");
            gearLayerMask = 1 << 17;
            Settings.OnLoad();
        }

        public override void OnUpdate()
        {
            if (Settings.options == null)
                return;

            if (Input.GetKey(Settings.options.scanKey))
            {
                isDisplaying = true;

                float interval = Settings.options.scanInterval;
                if (Time.time - lastScanTime >= interval)
                {
                    DetectItems();
                    lastScanTime = Time.time;
                }
            }
            else
            {
                if (isDisplaying)
                {
                    isDisplaying = false;
                    detectedItems.Clear();
                }
            }
        }

        public override void OnGUI()
        {
            if (!isDisplaying || detectedItems.Count == 0 || Settings.options == null)
                return;

            if (GUI.skin == null)
                return;

            Camera activeCamera = GetGameCamera();
            if (activeCamera == null)
                return;

            bool needRegenerate = false;
            
            if (textureCache.Count == 0 || 
                Settings.options.gearMarkerSize != lastGearMarkerSize ||
                Settings.options.containerMarkerSize != lastContainerMarkerSize ||
                Settings.options.plantMarkerSize != lastPlantMarkerSize ||
                Settings.options.gearColor != lastGearColor ||
                Settings.options.containerColor != lastContainerColor ||
                Settings.options.plantColor != lastPlantColor ||
                Settings.options.gearShape != lastGearShape ||
                Settings.options.containerShape != lastContainerShape ||
                Settings.options.plantShape != lastPlantShape)
            {
                needRegenerate = true;
            }
            
            if (!needRegenerate && textureCache.Count > 0)
            {
                foreach (var kvp in textureCache)
                {
                    if (kvp.Value == null || !kvp.Value)
                    {
                        needRegenerate = true;
                        break;
                    }
                }
            }
            
            if (needRegenerate)
            {
                RegenerateTextures();
                lastGearMarkerSize = Settings.options.gearMarkerSize;
                lastContainerMarkerSize = Settings.options.containerMarkerSize;
                lastPlantMarkerSize = Settings.options.plantMarkerSize;
                lastGearColor = Settings.options.gearColor;
                lastContainerColor = Settings.options.containerColor;
                lastPlantColor = Settings.options.plantColor;
                lastGearShape = Settings.options.gearShape;
                lastContainerShape = Settings.options.containerShape;
                lastPlantShape = Settings.options.plantShape;
            }

            foreach (var item in detectedItems)
            {
                Vector3 screenPos = activeCamera.WorldToScreenPoint(item.worldPosition);

                if (screenPos.z > 0)
                {
                    screenPos.y = Screen.height - screenPos.y;

                    Color markerColor = Color.white;
                    string textureKey = "";
                    int markerSize = 20;

                    switch (item.type)
                    {
                        case ItemType.Gear:
                            markerColor = Settings.options.GetGearMarkerColor();
                            markerSize = Settings.options.gearMarkerSize;
                            textureKey = $"Gear_{Settings.options.gearShape}_{Settings.options.gearColor}_{markerSize}";
                            break;
                        case ItemType.Container:
                            markerColor = Settings.options.GetContainerMarkerColor();
                            markerSize = Settings.options.containerMarkerSize;
                            textureKey = $"Container_{Settings.options.containerShape}_{Settings.options.containerColor}_{markerSize}";
                            break;
                        case ItemType.Plant:
                            markerColor = Settings.options.GetPlantMarkerColor();
                            markerSize = Settings.options.plantMarkerSize;
                            textureKey = $"Plant_{Settings.options.plantShape}_{Settings.options.plantColor}_{markerSize}";
                            break;
                    }

                    if (textureCache.ContainsKey(textureKey))
                    {
                        Texture2D texture = textureCache[textureKey];
                        
                        if (texture != null && texture)
                        {
                            GUI.DrawTexture(
                                new Rect(screenPos.x - markerSize, screenPos.y - markerSize, markerSize * 2, markerSize * 2),
                                texture
                            );

                            GUIStyle distStyle = new GUIStyle(GUI.skin.label);
                            distStyle.normal.textColor = markerColor;
                            distStyle.fontSize = 14;
                            distStyle.fontStyle = FontStyle.Bold;
                            distStyle.alignment = TextAnchor.MiddleCenter;

                            GUI.color = Color.white;
                            string distText = item.distance.ToString("F1") + "m";
                            GUI.Label(
                                new Rect(screenPos.x - 30, screenPos.y + markerSize + 5, 60, 20),
                                distText,
                                distStyle
                            );
                        }
                    }
                }
            }

            GUI.color = Color.white;
        }

        private void RegenerateTextures()
        {
            textureCache.Clear();

            float thickness = 3f;

            if (Settings.options.scanGear)
            {
                int size = Settings.options.gearMarkerSize * 2;
                string key = $"Gear_{Settings.options.gearShape}_{Settings.options.gearColor}_{Settings.options.gearMarkerSize}";
                Color color = Settings.options.GetGearMarkerColor();
                textureCache[key] = CreateShapeTexture(size, thickness, color, Settings.options.gearShape);
            }

            if (Settings.options.scanContainers)
            {
                int size = Settings.options.containerMarkerSize * 2;
                string key = $"Container_{Settings.options.containerShape}_{Settings.options.containerColor}_{Settings.options.containerMarkerSize}";
                Color color = Settings.options.GetContainerMarkerColor();
                textureCache[key] = CreateShapeTexture(size, thickness, color, Settings.options.containerShape);
            }

            if (Settings.options.scanPlants)
            {
                int size = Settings.options.plantMarkerSize * 2;
                string key = $"Plant_{Settings.options.plantShape}_{Settings.options.plantColor}_{Settings.options.plantMarkerSize}";
                Color color = Settings.options.GetPlantMarkerColor();
                textureCache[key] = CreateShapeTexture(size, thickness, color, Settings.options.plantShape);
            }
        }

        private Texture2D CreateShapeTexture(int size, float thickness, Color color, MarkerShape shape)
        {
            switch (shape)
            {
                case MarkerShape.Circle:
                    return CreateCircleTexture(size, thickness, color);
                case MarkerShape.Square:
                    return CreateSquareTexture(size, thickness, color);
                case MarkerShape.Triangle:
                    return CreateTriangleTexture(size, thickness, color);
                default:
                    return CreateCircleTexture(size, thickness, color);
            }
        }

        private void DetectItems()
        {
            detectedItems.Clear();

            if (GameManager.GetPlayerManagerComponent() == null)
                return;

            Vector3 playerPos = GameManager.GetPlayerTransform().position;
            float radius = Settings.options.scanRadius;

            // 分类检测
            if (Settings.options.scanGear) 
                ScanGear(playerPos, radius);
            
            if (Settings.options.scanContainers) 
                ScanContainers(playerPos, radius);
            
            if (Settings.options.scanPlants) 
                ScanPlants(playerPos, radius);

            // 统一排序
            detectedItems.Sort((a, b) => a.distance.CompareTo(b.distance));
        }

        private void ScanGear(Vector3 playerPos, float radius)
        {
            // 物理检测：只寻找半径内 Layer 17 的碰撞体
            // 使用 QueryTriggerInteraction.Collide 确保即使没有 Rigidbody 的触发器也能被检测到
            Collider[] gearColliders = Physics.OverlapSphere(playerPos, radius, gearLayerMask, QueryTriggerInteraction.Collide);
            
            // 用于防止重复添加（一个 GearItem 可能有多个 Collider）
            HashSet<int> processedInstanceIDs = new HashSet<int>();

            foreach (var collider in gearColliders)
            {
                GearItem gearItem = collider.GetComponentInParent<GearItem>();
                if (gearItem == null || gearItem.gameObject == null)
                    continue;

                // 检查实例 ID，避免同一个物品因为有多个碰撞体而被添加多次
                int instanceID = gearItem.gameObject.GetInstanceID();
                if (processedInstanceIDs.Contains(instanceID))
                    continue;

                // 基础过滤：必须激活且不在背包内
                if (!gearItem.gameObject.activeInHierarchy || gearItem.m_InPlayerInventory)
                    continue;

                // 过滤曾在背包的物品
                if (!Settings.options.showInventoryItems && gearItem.m_BeenInPlayerInventory)
                    continue;

                // 物品名称过滤 (石头/树枝)
                bool shouldHide = false;
                switch (Settings.options.hideItemsFilter)
                {
                    case HideItemsFilter.Stone:
                        shouldHide = gearItem.name.Contains("GEAR_Stone");
                        break;
                    case HideItemsFilter.Stick:
                        shouldHide = gearItem.name.Contains("GEAR_Stick");
                        break;
                    case HideItemsFilter.StoneAndStick:
                        shouldHide = gearItem.name.Contains("GEAR_Stone") || gearItem.name.Contains("GEAR_Stick");
                        break;
                }

                if (shouldHide)
                    continue;

                processedInstanceIDs.Add(instanceID);
                detectedItems.Add(new ItemInfo
                {
                    worldPosition = gearItem.transform.position,
                    distance = Vector3.Distance(playerPos, gearItem.transform.position),
                    type = ItemType.Gear
                });
            }
        }


        private void ScanContainers(Vector3 playerPos, float radius)
        {
            Container[] allContainers = UnityEngine.Object.FindObjectsOfType<Container>();
            
            foreach (Container container in allContainers)
            {
                if (container == null || container.gameObject == null)
                    continue;

                if (Settings.options.hideOpenedContainers && container.IsInspected())
                    continue;

                float distance = Vector3.Distance(playerPos, container.transform.position);
                
                if (distance <= radius)
                {
                    detectedItems.Add(new ItemInfo
                    {
                        worldPosition = container.transform.position,
                        distance = distance,
                        type = ItemType.Container
                    });
                }
            }
        }

        private void ScanPlants(Vector3 playerPos, float radius)
        {
            Harvestable[] allHarvestables = UnityEngine.Object.FindObjectsOfType<Harvestable>();
            
            foreach (Harvestable harvestable in allHarvestables)
            {
                if (harvestable == null || harvestable.gameObject == null)
                    continue;

                if (harvestable.m_Harvested)
                    continue;

                if (!harvestable.RegisterAsPlantsHaversted)
                    continue;

                float distance = Vector3.Distance(playerPos, harvestable.transform.position);
                
                if (distance <= radius)
                {
                    detectedItems.Add(new ItemInfo
                    {
                        worldPosition = harvestable.transform.position,
                        distance = distance,
                        type = ItemType.Plant
                    });
                }
            }
        }

        private Texture2D CreateCircleTexture(int size, float thickness, Color color)
        {
            Texture2D tex = new Texture2D(size, size, TextureFormat.ARGB32, false);
            tex.filterMode = FilterMode.Bilinear;

            float center = size / 2f;
            float outerRadius = size / 2f - 1f;
            float innerRadius = outerRadius - thickness;

            for (int x = 0; x < size; x++)
            {
                for (int y = 0; y < size; y++)
                {
                    float dist = Mathf.Sqrt((x - center) * (x - center) + (y - center) * (y - center));

                    if (dist >= innerRadius && dist <= outerRadius)
                        tex.SetPixel(x, y, color);
                    else
                        tex.SetPixel(x, y, Color.clear);
                }
            }

            tex.Apply();
            return tex;
        }

        private Texture2D CreateSquareTexture(int size, float thickness, Color color)
        {
            Texture2D tex = new Texture2D(size, size, TextureFormat.ARGB32, false);
            tex.filterMode = FilterMode.Bilinear;

            int border = (int)thickness;

            for (int x = 0; x < size; x++)
            {
                for (int y = 0; y < size; y++)
                {
                    bool isEdge = (x < border || x >= size - border || y < border || y >= size - border);
                    bool isInner = (x >= border && x < size - border && y >= border && y < size - border);

                    if (isEdge && !isInner)
                        tex.SetPixel(x, y, color);
                    else
                        tex.SetPixel(x, y, Color.clear);
                }
            }

            tex.Apply();
            return tex;
        }

        private Texture2D CreateTriangleTexture(int size, float thickness, Color color)
        {
            Texture2D tex = new Texture2D(size, size, TextureFormat.ARGB32, false);
            tex.filterMode = FilterMode.Bilinear;

            float centerX = size / 2f;
            float topY = size * 0.1f;
            float bottomY = size * 0.9f;
            float leftX = size * 0.2f;
            float rightX = size * 0.8f;

            for (int x = 0; x < size; x++)
            {
                for (int y = 0; y < size; y++)
                {
                    float d1 = DistanceToLine(x, y, centerX, topY, leftX, bottomY);
                    float d2 = DistanceToLine(x, y, leftX, bottomY, rightX, bottomY);
                    float d3 = DistanceToLine(x, y, rightX, bottomY, centerX, topY);

                    float minDist = Mathf.Min(d1, Mathf.Min(d2, d3));

                    if (minDist <= thickness)
                        tex.SetPixel(x, y, color);
                    else
                        tex.SetPixel(x, y, Color.clear);
                }
            }

            tex.Apply();
            return tex;
        }

        private float DistanceToLine(float px, float py, float x1, float y1, float x2, float y2)
        {
            float A = px - x1;
            float B = py - y1;
            float C = x2 - x1;
            float D = y2 - y1;

            float dot = A * C + B * D;
            float lenSq = C * C + D * D;
            float param = -1;

            if (lenSq != 0)
                param = dot / lenSq;

            float xx, yy;

            if (param < 0)
            {
                xx = x1;
                yy = y1;
            }
            else if (param > 1)
            {
                xx = x2;
                yy = y2;
            }
            else
            {
                xx = x1 + param * C;
                yy = y1 + param * D;
            }

            float dx = px - xx;
            float dy = py - yy;
            return Mathf.Sqrt(dx * dx + dy * dy);
        }

        private Camera GetGameCamera()
        {
            if (Camera.main != null)
                return Camera.main;

            try
            {
                Camera cam = GameManager.GetMainCamera();
                if (cam != null)
                    return cam;
            }
            catch { }

            try
            {
                PlayerManager pm = GameManager.GetPlayerManagerComponent();
                if (pm != null)
                {
                    Camera cam = pm.GetComponentInChildren<Camera>();
                    if (cam != null && cam.enabled)
                        return cam;
                }
            }
            catch { }

            Camera[] cameras = Camera.allCameras;
            foreach (Camera cam in cameras)
            {
                if (cam != null && cam.enabled && cam.gameObject.activeInHierarchy)
                {
                    if (cam.depth >= 0)
                        return cam;
                }
            }

            if (cameras.Length > 0)
                return cameras[0];

            return null;
        }
    }
}
