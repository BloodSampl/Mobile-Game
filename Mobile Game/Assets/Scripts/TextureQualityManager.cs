using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TextureQualityManager : MonoBehaviour
{
    [SerializeField] private TMP_Dropdown textureQualityDropdown;

    void Start()
    {
        // Populate the dropdown with the quality levels
        textureQualityDropdown.options.Clear();
        textureQualityDropdown.options.Add(new TMP_Dropdown.OptionData("Low"));
        textureQualityDropdown.options.Add(new TMP_Dropdown.OptionData("Medium"));
        textureQualityDropdown.options.Add(new TMP_Dropdown.OptionData("High"));

        // Set the initial value based on the current quality setting
        textureQualityDropdown.value = QualitySettings.globalTextureMipmapLimit;
        
        textureQualityDropdown.onValueChanged.AddListener(delegate {
            SetTextureQuality(textureQualityDropdown.value);
        });
    }

    public void SetTextureQuality(int qualityIndex)
    {
        // Inverse the quality index because Unity's masterTextureLimit works inversely
        QualitySettings.globalTextureMipmapLimit = qualityIndex;
        Debug.Log("Texture Quality set to: " + textureQualityDropdown.options[qualityIndex].text);
    }
}