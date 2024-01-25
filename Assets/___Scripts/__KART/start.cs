using System.Collections;
using TMPro;
using UnityEngine;

public class start : MonoBehaviour
{
    public TextMeshProUGUI countdownText;
    private float countdown = 3.0f;

    private void OnEnable()
    {
        Time.timeScale = 0;
    }
    public void start_bt()
    {
        StartCoroutine(CountdownCoroutine());
    }
    IEnumerator CountdownCoroutine()
    {
        while (countdown > 0)
        {
            countdownText.text = Mathf.Ceil(countdown).ToString();

            yield return new WaitForSecondsRealtime(1.0f);

            countdown -= 1.0f;
        }

        countdownText.text = "";
        Time.timeScale = 1;
        gameObject.SetActive(false);
    }
}
