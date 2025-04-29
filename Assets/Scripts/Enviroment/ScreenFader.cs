using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Enviroment
{
    public class ScreenFader : MonoBehaviour
    {
        [SerializeField] private Image fadeImage;
        [SerializeField] private float fadeDuration = 0.5f;
        
        public IEnumerator FadeIn()
        {
            float timer = 0f;
            Color color = fadeImage.color;

            while (timer < fadeDuration)
            {
                timer += Time.deltaTime;
                color.a = Mathf.Lerp(0f, 1f, timer / fadeDuration);
                fadeImage.color = color;
                yield return null;
            }
            color.a = 1f;
            fadeImage.color = color;
        }

        public IEnumerator FadeOut()
        {
            float timer = 0f;
            Color color = fadeImage.color;

            while (timer < fadeDuration)
            {
                timer += Time.deltaTime;
                color.a = Mathf.Lerp(1f, 0f, timer / fadeDuration);
                fadeImage.color = color;
                yield return null;
            }
            color.a = 0f;
            fadeImage.color = color;
        }
    }
}
