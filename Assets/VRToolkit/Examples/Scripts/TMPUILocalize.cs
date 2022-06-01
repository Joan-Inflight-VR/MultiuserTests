using VRToolkit.Localization;

public class TMPUILocalize : UILocalizeBase
{
    private TMPro.TextMeshProUGUI textComponent;

    private void Start()
    {
        SetUp();

        textComponent = GetComponent<TMPro.TextMeshProUGUI>();
    }

    public override void ChangeLocalization(string localizedText)
    {
        if (!textComponent)
        {
            textComponent = GetComponent<TMPro.TextMeshProUGUI>();
        }

        textComponent.text = localizedText;
    }
}
