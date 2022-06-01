using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[ExecuteInEditMode]
public class TMPMaterialConfig : MonoBehaviour
{

    [System.Serializable]
    public struct OutlineSettings
    {
        public bool outlineActive;
        public Color color;

        [Range(0, 1)]
        public float thickness;
        public OutlineSettings(bool active, Color newColor, float newThickness)
        {
            outlineActive = active;
            color = newColor;
            thickness = newThickness;
        }
    }

    [System.Serializable]
    public struct UnderlaySettings
    {
        public bool underlayActive;
        public Color color;
        [Range(-1, 1)]
        public float offsetX;
        [Range(-1, 1)]
        public float offsetY;
        [Range(-1, 1)]
        public float dilate;
        [Range(0, 1)]
        public float softness;

        public UnderlaySettings(bool active, Color newColor, float newOffsetX, float newOffsetY, float newDilate, float newSoftness)
        {
            underlayActive = active;
            color = newColor;
            offsetX = newOffsetX;
            offsetY = newOffsetY;
            dilate = newDilate;
            softness = newSoftness;
        }
    }

    public bool bold = false;
    public bool italic = false;
    public OutlineSettings outlineSettings = new OutlineSettings(false, Color.black, 1);
    public UnderlaySettings underlaySettings = new UnderlaySettings(false, Color.black, 1, -1, 1, 0);

    private bool makeUpdate = false;
    Material materialInstance;
    TextMeshProUGUI textInstance;

    void Start()
    {
        textInstance = GetComponent<TextMeshProUGUI>();
        materialInstance = textInstance.fontMaterial;
        ApplyChanges();
    }

    private void Update()
    {
        if (makeUpdate)
        {
            ExecuteChanges();
            makeUpdate = false;
        }
    }

    private void OnValidate()
    {
        ApplyChanges();
    }

    public void ApplyChanges(bool _bold, bool _italics, OutlineSettings _outline, UnderlaySettings _underlay)
    {
        outlineSettings = _outline;
        underlaySettings = _underlay;
        bold = _bold;
        italic = _italics;

        makeUpdate = true;
    }

    public void ApplyChanges()
    {
        makeUpdate = true;
    }

    private void ExecuteChanges()
    {
        materialInstance = textInstance.fontMaterial;
        //Outline 
        if (outlineSettings.outlineActive)
        {
            materialInstance.EnableKeyword(ShaderUtilities.Keyword_Outline);

            materialInstance.SetColor("_OutlineColor", outlineSettings.color);
            materialInstance.SetFloat("_OutlineWidth", outlineSettings.thickness);
        }
        else
        {
            materialInstance.DisableKeyword(ShaderUtilities.Keyword_Outline);

            materialInstance.SetColor("_OutlineColor", outlineSettings.color);
            materialInstance.SetFloat("_OutlineWidth", 0);
        }

        //Underlay 
        if (underlaySettings.underlayActive)
        {
            materialInstance.EnableKeyword(ShaderUtilities.Keyword_Underlay);

            materialInstance.SetColor("_UnderlayColor", underlaySettings.color);
            materialInstance.SetFloat("_UnderlayOffsetX", underlaySettings.offsetX);
            materialInstance.SetFloat("_UnderlayOffsetY", underlaySettings.offsetY);
            materialInstance.SetFloat("_UnderlayDilate", underlaySettings.dilate);
            materialInstance.SetFloat("_UnderlaySoftness", underlaySettings.softness);
        }
        else
            materialInstance.DisableKeyword(ShaderUtilities.Keyword_Underlay);

        textInstance.fontStyle = FontStyles.Normal;

        if (bold)
            textInstance.fontStyle |= FontStyles.Bold;
        if (italic)
            textInstance.fontStyle |= FontStyles.Italic;

        textInstance.UpdateMeshPadding();
    }
}
