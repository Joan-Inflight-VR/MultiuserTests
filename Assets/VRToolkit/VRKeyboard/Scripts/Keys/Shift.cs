using TMPro;

namespace VRToolkit.VRKeyboard.Utils
{
    public class Shift : Key
    {
        TextMeshProUGUI subscript;

        public override void Awake()
        {
            base.Awake();
            subscript = transform.Find("Subscript (TMP)").GetComponent<TextMeshProUGUI>();
        }

        public override void ShiftKey()
        {
            var tmp = key.text;
            key.text = subscript.text;
            subscript.text = tmp;
        }
    }
}