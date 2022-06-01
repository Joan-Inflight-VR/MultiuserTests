using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Networking;
using UnityEngine.UI;
using VRToolkit.Utils;

namespace VRToolkit.Surveys
{
    public class AnswerItem : MonoBehaviour
    {
        public Image image;
        public TextMeshProUGUI text;
        public InputManager.UI.Toggle toggle;
        public FillerInteractionHandler fillerHandler;

        public void SetUp(string spritePath, string answerText, Action callback)
        {
            Sprite sprite = ImageStorage.GetSprite(spritePath);

            if (sprite != null)
            {
                image.sprite = sprite;
                if (!string.IsNullOrEmpty(answerText))
                {
                    text.gameObject.SetActive(true);
                    text.text = answerText; // this should be localized
                }

                image.color = Color.white;
            }
            else
            {
                StartCoroutine(GetTexture(spritePath));
            }

            toggle.SetUp(fillerHandler);

            UnityAction action = () =>
            {
                toggle.group.currentSelected = toggle;
                fillerHandler.active = false;
                fillerHandler.ForceFill();

                toggle.group.UpdateToggles();

                callback();
            };

            fillerHandler.filler.OnFillCompleted.AddListener(action);

        }

        IEnumerator GetTexture(string spritePath)
        {
            using (UnityWebRequest uwr = UnityWebRequestTexture.GetTexture($"file://{Statics.inflightPath + spritePath}"))
            {
                yield return uwr.SendWebRequest();

                if (uwr.responseCode != 200)
                {
                    Debug.Log(uwr.error);
                }
                else
                {
                    Texture2D texture = DownloadHandlerTexture.GetContent(uwr);
                    Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));

                    image.sprite = sprite;
                    image.color = Color.white;

                    ImageStorage.StoreSprite(spritePath, sprite);
                }
            }
        }
    }
}