using System.Collections;
using TMPro;
using UnityEngine;

public class start_squid_game : MonoBehaviour
{
    public TextMeshProUGUI countdownText;
    public float countdown = 3.0f;

    private void OnEnable()
    {
        Time.timeScale = 0;
    }
    public void start_bt()
    {
        Time.timeScale = 1;
        StartCoroutine(CountdownCoroutine());
    }
    IEnumerator CountdownCoroutine()
    {
        while (countdown > 0)
        {
            countdownText.text = Mathf.Ceil(countdown).ToString();

            yield return new WaitForSeconds(1.0f);

            countdown -= 1.0f;
        }

        countdownText.text = "";
        gameObject.SetActive(false);
    }
}
