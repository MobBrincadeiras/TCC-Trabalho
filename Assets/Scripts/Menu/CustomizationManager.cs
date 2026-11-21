
using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CustomizationManager : MonoBehaviour
{
    [System.Serializable]
    public class HatOption
    {
        public string hatName;
        public Sprite sprite;
        public Vector2 position;
        public Vector2 size;
        [Min(0)] public int price = 100;
    }

    [System.Serializable]
    public class FaceAccessoryOption
    {
        public string accessoryName;
        public Sprite sprite;
        public Vector2 position;
        public Vector2 size;
        [Min(0)] public int price = 100;
    }

    [System.Serializable]
    public class ShirtOption
    {
        public string shirtName;
        public Sprite torsoSprite;
        public Sprite leftArmSprite;
        public Sprite rightArmSprite;
        [Min(0)] public int price = 100;
    }

    [System.Serializable]
    public class PantsOption
    {
        public string pantsName;
        public Sprite leftLegSprite;
        public Vector2 leftLegPosition;
        public Vector2 leftLegSize;
        public Sprite rightLegSprite;
        public Vector2 rightLegPosition;
        public Vector2 rightLegSize;
        [Min(0)] public int price = 100;
    }

    [System.Serializable]
    public class ShoeOption
    {
        public string shoeName;
        public Sprite leftShoeSprite;
        public Vector2 leftShoePosition;
        public Vector2 leftShoeSize;
        public Sprite rightShoeSprite;
        public Vector2 rightShoePosition;
        public Vector2 rightShoeSize;
        [Min(0)] public int price = 100;
    }

    public GameObject customizationPanel;
    [SerializeField] private Sprite lockSprite;
    [SerializeField] private Vector2 lockSize = new Vector2(30, 30);
    [SerializeField] private Vector2 lockPosition = new Vector2(30, 30);
    [SerializeField] private Vector2 priceSize = new Vector2(100, 30);
    [SerializeField] private Vector2 pricePosition = new Vector2(0, -45);
    [SerializeField] private int priceFontSize = 18;
    public GameObject MainMenuPanel;
    [SerializeField] private MenuManager menuManager;
    public GameObject[] categoryPanels;
    public GameObject headAccessoriesPanel;
    public GameObject faceAccessoriesPanel;

    public Image accessoryImage;
    public HatOption[] hats;
    public Transform hatGrid;
    public GameObject hatButtonPrefab;

    public Image faceAccessoryImage;
    public FaceAccessoryOption[] faceAccessories;
    public Transform faceAccessoryGrid;
    public GameObject faceAccessoryButtonPrefab;

    public Image torsoImage;
    public Image leftArmImage;
    public Image rightArmImage;
    public ShirtOption[] shirts;
    public Transform shirtGrid;
    public GameObject shirtButtonPrefab;

    public Image leftLegImage;
    public Image rightLegImage;
    public PantsOption[] pants;
    public Transform pantsGrid;
    public GameObject pantsButtonPrefab;

    public Image leftShoeImage;
    public Image rightShoeImage;
    public ShoeOption[] shoes;
    public Transform shoeGrid;
    public GameObject shoeButtonPrefab;

    private int currentCategory = 0;

    private void Start()
    {
        SelectCategory(0);
        SelectAccessoryCategory(0);

        if (hatGrid != null && hatButtonPrefab != null)
            GenerateHatButtons();

        if (faceAccessoryGrid != null && faceAccessoryButtonPrefab != null)
            GenerateFaceAccessoryButtons();

        if (shirtGrid != null && shirtButtonPrefab != null)
            GenerateShirtButtons();

        if (pantsGrid != null && pantsButtonPrefab != null)
            GeneratePantsButtons();

        if (shoeGrid != null && shoeButtonPrefab != null)
            GenerateShoeButtons();

        LoadEquippedItems();
    }

    private bool TryUnlockItem(string category, int index, int price)
    {
        if (PlayerData.IsClothingUnlocked(category, index))
            return true;

        price = Mathf.Max(0, price);

        if (!PlayerData.TrySpendMoney(price))
        {
            Debug.Log("Dinheiro insuficiente para comprar este item!");
            return false;
        }

        PlayerData.UnlockClothingItem(category, index);

        if (menuManager != null)
        {
            menuManager.UpdatePlayerData();
        }

        Debug.Log("Item comprado: " + category);

        return true;
    }

    private void UpdateButtonAppearance(
        GameObject button,
        string category,
        int index)
    {
        bool unlocked = PlayerData.IsClothingUnlocked(category, index);

        Image image = button.GetComponent<Image>();

        if (image != null)
        {
            image.color = unlocked
                ? Color.white
                : new Color(0.55f, 0.55f, 0.55f, 1f);
        }

        Transform lockTransform = button.transform.Find("LockIcon");

        if (lockTransform != null)
        {
            lockTransform.gameObject.SetActive(!unlocked);
        }

        Transform priceTransform = button.transform.Find("PriceText");

        if (priceTransform != null)
        {
            priceTransform.gameObject.SetActive(!unlocked);
        }
    }

    private void PrepareGrid(Transform grid, GameObject prefab)
    {
        foreach (Transform child in grid)
        {
            if (child.gameObject == prefab)
            {
                child.gameObject.SetActive(false);
                continue;
            }

            Destroy(child.gameObject);
        }
    }

    private GameObject CreateItemButton(
        Transform grid,
        GameObject prefab,
        Sprite sprite,
        string category,
        int index,
        int price,
        Action<int> selectAction)
    {
        GameObject button = Instantiate(prefab, grid);

        button.SetActive(true);

        Image buttonImage = button.GetComponent<Image>();

        if (buttonImage != null)
        {
            buttonImage.sprite = sprite;
            buttonImage.preserveAspect = true;
        }

        GameObject lockObject = new GameObject(
        "LockIcon",
        typeof(RectTransform),
        typeof(CanvasRenderer),
        typeof(Image)
        );

        lockObject.transform.SetParent(button.transform, false);

        RectTransform lockRect = lockObject.GetComponent<RectTransform>();

        lockRect.anchorMin = new Vector2(1, 1);
        lockRect.anchorMax = new Vector2(1, 1);
        lockRect.pivot = new Vector2(1, 1);

        lockRect.anchoredPosition = lockPosition;
        lockRect.sizeDelta = lockSize;

        Image lockImage = lockObject.GetComponent<Image>();
        lockImage.sprite = lockSprite;
        lockImage.preserveAspect = true;
        lockImage.raycastTarget = false;

        GameObject priceObject = new GameObject(
        "PriceText",
        typeof(RectTransform),
        typeof(CanvasRenderer),
        typeof(TextMeshProUGUI)
        );

        priceObject.transform.SetParent(button.transform, false);

        RectTransform priceRect = priceObject.GetComponent<RectTransform>();

        priceRect.anchorMin = new Vector2(0.5f, 0);
        priceRect.anchorMax = new Vector2(0.5f, 0);
        priceRect.pivot = new Vector2(0.5f, 0);

        priceRect.anchoredPosition = pricePosition;
        priceRect.sizeDelta = priceSize;

        TextMeshProUGUI priceText = priceObject.GetComponent<TextMeshProUGUI>();

        priceText.text = price.ToString() + " moedas";
        priceText.fontSize = priceFontSize;
        priceText.alignment = TextAlignmentOptions.Center;
        priceText.color = Color.white;
        priceText.raycastTarget = false;

        priceObject.SetActive(!PlayerData.IsClothingUnlocked(category, index));

        UpdateButtonAppearance(button, category, index);

        Button buttonComponent = button.GetComponent<Button>();

        if (buttonComponent != null)
        {
            buttonComponent.onClick.RemoveAllListeners();
            buttonComponent.onClick.AddListener(
                () => selectAction(index)
            );
        }

        return button;
    }

    public void OpenCustomization()
    {
        customizationPanel.SetActive(true);
        MainMenuPanel.SetActive(false);
    }

    public void CloseCustomization()
    {
        customizationPanel.SetActive(false);
        MainMenuPanel.SetActive(true);
    }

    public void SelectCategory(int categoryIndex)
    {
        if (categoryPanels == null ||
            categoryIndex < 0 ||
            categoryIndex >= categoryPanels.Length)
            return;

        currentCategory = categoryIndex;

        for (int i = 0; i < categoryPanels.Length; i++)
        {
            categoryPanels[i].SetActive(i == currentCategory);
        }
    }

    public void SelectAccessoryCategory(int categoryIndex)
    {
        headAccessoriesPanel.SetActive(categoryIndex == 0);
        faceAccessoriesPanel.SetActive(categoryIndex == 1);
    }

    private void GenerateHatButtons()
    {
        PrepareGrid(hatGrid, hatButtonPrefab);

        for (int i = 0; i < hats.Length; i++)
        {
            int index = i;

            CreateItemButton(
                hatGrid,
                hatButtonPrefab,
                hats[i].sprite,
                "Hat",
                index,
                hats[i].price,
                SelectHat
            );
        }
    }

    public void SelectHat(int index)
    {
        if (index < 0 || index >= hats.Length)
            return;

        if (!TryUnlockItem("Hat", index, hats[index].price))
            return;

        ApplyHat(index);

        PlayerData.SetEquippedClothing("Hat", index);
        RefreshGrid(hatGrid, "Hat");
    }

    private void ApplyHat(int index)
    {
        if (accessoryImage == null)
            return;

        HatOption selectedHat = hats[index];

        accessoryImage.sprite = selectedHat.sprite;
        accessoryImage.enabled = true;

        accessoryImage.rectTransform.anchoredPosition =
            selectedHat.position;

        accessoryImage.rectTransform.sizeDelta =
            selectedHat.size;
    }

    private void GenerateFaceAccessoryButtons()
    {
        PrepareGrid(faceAccessoryGrid, faceAccessoryButtonPrefab);

        for (int i = 0; i < faceAccessories.Length; i++)
        {
            int index = i;

            CreateItemButton(
                faceAccessoryGrid,
                faceAccessoryButtonPrefab,
                faceAccessories[i].sprite,
                "FaceAccessory",
                index,
                faceAccessories[i].price,
                SelectFaceAccessory
            );
        }
    }

    public void SelectFaceAccessory(int index)
    {
        if (index < 0 || index >= faceAccessories.Length)
            return;

        if (!TryUnlockItem(
            "FaceAccessory",
            index,
            faceAccessories[index].price))
            return;

        ApplyFaceAccessory(index);

        PlayerData.SetEquippedClothing("FaceAccessory", index);
        RefreshGrid(faceAccessoryGrid, "FaceAccessory");
    }

    private void ApplyFaceAccessory(int index)
    {
        if (faceAccessoryImage == null)
            return;

        FaceAccessoryOption selectedAccessory = faceAccessories[index];

        faceAccessoryImage.sprite = selectedAccessory.sprite;
        faceAccessoryImage.enabled = true;

        faceAccessoryImage.rectTransform.anchoredPosition =
            selectedAccessory.position;

        faceAccessoryImage.rectTransform.sizeDelta =
            selectedAccessory.size;
    }

    private void GenerateShirtButtons()
    {
        PrepareGrid(shirtGrid, shirtButtonPrefab);

        for (int i = 0; i < shirts.Length; i++)
        {
            int index = i;

            CreateItemButton(
                shirtGrid,
                shirtButtonPrefab,
                shirts[i].torsoSprite,
                "Shirt",
                index,
                shirts[i].price,
                SelectShirt
            );
        }
    }

    public void SelectShirt(int index)
    {
        if (index < 0 || index >= shirts.Length)
            return;

        if (!TryUnlockItem("Shirt", index, shirts[index].price))
            return;

        ApplyShirt(index);

        PlayerData.SetEquippedClothing("Shirt", index);
        RefreshGrid(shirtGrid, "Shirt");
    }

    private void ApplyShirt(int index)
    {
        ShirtOption selectedShirt = shirts[index];

        if (torsoImage != null)
        {
            torsoImage.sprite = selectedShirt.torsoSprite;
            torsoImage.enabled = true;
        }

        if (leftArmImage != null)
        {
            leftArmImage.sprite = selectedShirt.leftArmSprite;
            leftArmImage.enabled = true;
        }

        if (rightArmImage != null)
        {
            rightArmImage.sprite = selectedShirt.rightArmSprite;
            rightArmImage.enabled = true;
        }
    }

    private void GeneratePantsButtons()
    {
        PrepareGrid(pantsGrid, pantsButtonPrefab);

        for (int i = 0; i < pants.Length; i++)
        {
            int index = i;

            CreateItemButton(
                pantsGrid,
                pantsButtonPrefab,
                pants[i].leftLegSprite,
                "Pants",
                index,
                pants[i].price,
                SelectPants
            );
        }
    }

    public void SelectPants(int index)
    {
        if (index < 0 || index >= pants.Length)
            return;

        if (!TryUnlockItem("Pants", index, pants[index].price))
            return;

        ApplyPants(index);

        PlayerData.SetEquippedClothing("Pants", index);
        RefreshGrid(pantsGrid, "Pants");
    }

    private void ApplyPants(int index)
    {
        PantsOption selectedPants = pants[index];

        if (leftLegImage != null)
        {
            leftLegImage.sprite = selectedPants.leftLegSprite;
            leftLegImage.enabled = true;

            leftLegImage.rectTransform.anchoredPosition =
                selectedPants.leftLegPosition;

            leftLegImage.rectTransform.sizeDelta =
                selectedPants.leftLegSize;
        }

        if (rightLegImage != null)
        {
            rightLegImage.sprite = selectedPants.rightLegSprite;
            rightLegImage.enabled = true;

            rightLegImage.rectTransform.anchoredPosition =
                selectedPants.rightLegPosition;

            rightLegImage.rectTransform.sizeDelta =
                selectedPants.rightLegSize;
        }
    }

    private void GenerateShoeButtons()
    {
        PrepareGrid(shoeGrid, shoeButtonPrefab);

        for (int i = 0; i < shoes.Length; i++)
        {
            int index = i;

            CreateItemButton(
                shoeGrid,
                shoeButtonPrefab,
                shoes[i].leftShoeSprite,
                "Shoe",
                index,
                shoes[i].price,
                SelectShoe
            );
        }
    }

    public void SelectShoe(int index)
    {
        if (index < 0 || index >= shoes.Length)
            return;

        if (leftShoeImage == null || rightShoeImage == null)
            return;

        if (!TryUnlockItem("Shoe", index, shoes[index].price))
            return;

        ApplyShoe(index);

        PlayerData.SetEquippedClothing("Shoe", index);
        RefreshGrid(shoeGrid, "Shoe");
    }

    private void ApplyShoe(int index)
    {
        ShoeOption selectedShoe = shoes[index];

        leftShoeImage.sprite = selectedShoe.leftShoeSprite;
        rightShoeImage.sprite = selectedShoe.rightShoeSprite;

        leftShoeImage.enabled = true;
        rightShoeImage.enabled = true;

        leftShoeImage.rectTransform.anchoredPosition =
            selectedShoe.leftShoePosition;

        leftShoeImage.rectTransform.sizeDelta =
            selectedShoe.leftShoeSize;

        rightShoeImage.rectTransform.anchoredPosition =
            selectedShoe.rightShoePosition;

        rightShoeImage.rectTransform.sizeDelta =
            selectedShoe.rightShoeSize;
    }

    private void RefreshGrid(Transform grid, string category)
    {
        if (grid == null)
            return;

        int itemIndex = 0;

        for (int i = 0; i < grid.childCount; i++)
        {
            Transform child = grid.GetChild(i);

            if (!child.gameObject.activeSelf)
                continue;

            UpdateButtonAppearance(child.gameObject, category, itemIndex);

            itemIndex++;
        }
    }

    private int GetValidEquippedIndex(string category, int arrayLength)
    {
        if (arrayLength <= 0)
            return -1;

        int index = PlayerData.GetEquippedClothing(category);

        if (index < 0 || index >= arrayLength)
            return 0;

        if (!PlayerData.IsClothingUnlocked(category, index))
            return 0;

        return index;
    }

    private void LoadEquippedItems()
    {
        int hatIndex = GetValidEquippedIndex("Hat", hats.Length);
        if (hatIndex >= 0)
            ApplyHat(hatIndex);

        int faceIndex = GetValidEquippedIndex(
            "FaceAccessory",
            faceAccessories.Length
        );
        if (faceIndex >= 0)
            ApplyFaceAccessory(faceIndex);

        int shirtIndex = GetValidEquippedIndex("Shirt", shirts.Length);
        if (shirtIndex >= 0)
            ApplyShirt(shirtIndex);

        int pantsIndex = GetValidEquippedIndex("Pants", pants.Length);
        if (pantsIndex >= 0)
            ApplyPants(pantsIndex);

        int shoeIndex = GetValidEquippedIndex("Shoe", shoes.Length);
        if (shoeIndex >= 0 &&
            leftShoeImage != null &&
            rightShoeImage != null)
        {
            ApplyShoe(shoeIndex);
        }
    }
}
