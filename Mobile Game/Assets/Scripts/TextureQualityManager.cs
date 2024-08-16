using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TextureQualityManager : MonoBehaviour
{
    [SerializeField] private TMP_Dropdown textureQualityDropdown;

    void Start()
    {
        
        textureQualityDropdown.options.Clear();
        textureQualityDropdown.options.Add(new TMP_Dropdown.OptionData("Low"));
        textureQualityDropdown.options.Add(new TMP_Dropdown.OptionData("Medium"));
        textureQualityDropdown.options.Add(new TMP_Dropdown.OptionData("High"));

        
        textureQualityDropdown.value = QualitySettings.GetQualityLevel();
        
        textureQualityDropdown.onValueChanged.AddListener(delegate {
            SetTextureQuality(textureQualityDropdown.value);
        });
    }

    public void SetTextureQuality(int qualityIndex)
    {
        
        QualitySettings.SetQualityLevel(qualityIndex);
        Debug.Log("Texture Quality set to: " + textureQualityDropdown.options[qualityIndex].text);
    }
}