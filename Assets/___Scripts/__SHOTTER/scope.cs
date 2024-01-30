using Cinemachine;
using DG.Tweening;
using System.Collections;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.UI;

public class Scope : MonoBehaviour
{
    public static int HP_Sub;
    public static float start_time_shot;
    public static bool is_hold = false;

    public Image scope;
    public GameObject bullet;
    public CinemachineBrain cinemachineBrain;
    public GameObject virtual_camera;
    public PostProcessVolume volume;
    public GameObject sniper;
    private Animator anim;

    private Vignette vignetteEffect;
    private Camera mainCamera;
    private float normal_camera;
    private float zoom_camera = 30f;
    private float sensitivity_camera;
    private Rigidbody bullet_rb;
    private Color scope_Active_Color = new Color(1f, 0f, 0f, 1f);
    private Color scope_Not_Active_Color = new Color(0f, 0f, 0f, 0f);

    bool check; // chạy aim enemy 1 lần
    Vector2 start_position;
    float speed_bullet = 300;
    public static bool aim_enemy;

    private float xRotation = 0f;
    private float yRotation = 0f;

    public float reload = 1;
    public GameObject smoke_bullet;

    private Vector3 originalPosition;
    public GameObject shell;

    public static bool play_shot_anim;
    private void Awake()
    {
        play_shot_anim = false;
        originalPosition = sniper.transform.localPosition;
        Sound_Manager.Instance.Play_Music("BGM-Action", 0);
        Sound_Manager.Instance.Play_Music("BGM-Action", 1);
        canvas_controller.active_all_minigame = true;

        mainCamera = GetComponent<Camera>();
        bullet_rb = bullet.GetComponent<Rigidbody>();
        anim = sniper.GetComponent<Animator>();

        sensitivity_camera = 0.15f * zoom_camera / 60;
        normal_camera = mainCamera.fieldOfView;
        scope.color = scope_Not_Active_Color;
        virtual_camera.SetActive(false);

        if (volume.profile.TryGetSettings(out Vignette vignette))
        {
            vignetteEffect = vignette;
        }
    }
    private void OnEnable()
    {
        start_time_shot = -3;
    }

    private void Update()
    {
        Debug.Log(anim.GetInteger("state"));
        if (Bullet.shake_camera)
        {
            StartCoroutine(Shake(1f, 2f));
            Bullet.shake_camera = false;
        }

        if (scope.color == scope_Active_Color || virtual_camera.activeSelf)
        {
            if (mainCamera.fieldOfView < zoom_camera + 5)
            {
                sniper.SetActive(false);
                shell.SetActive(false);
            }
        }
        else
        {
            sniper.SetActive(true);
            if (play_shot_anim)
            {
                anim.SetInteger("state", 1);
                play_shot_anim = false;
            }
            //shell.SetActive(true);
        }
        if (Input.GetMouseButton(0) && !is_hold && Time.time - start_time_shot > reload && !virtual_camera.activeSelf)
        {
            anim.SetInteger("state", 0);
            is_hold = true;
            start_position = Input.mousePosition;
        }

        if (Input.GetMouseButtonUp(0) && is_hold)
        {
            //nếu còn 1 enemy cuối
            if (Bullet.rocket != 2 && count_obj_active_true() == 1)
            {
                Ray ray = new Ray(mainCamera.transform.position, mainCamera.transform.forward);
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit))
                {
                    //Debug.Log(hit.collider.tag);
                    //tính toán xem có sau khi bắn enemy có chết không
                    if (hit.collider.CompareTag("enemy"))
                    {
                        if (hit.transform.GetComponent<Animator>().GetInteger("state") != 16)
                        {
                            int crit = Random.Range(0, 100);
                            if (crit < 25 || spawn.cur_HP[spawn.pooledObjects.IndexOf(hit.transform.gameObject)] <= 1)
                            {
                                aim_enemy = true;
                                speed_bullet = 50;
                                hit.transform.GetComponent<Rigidbody>().velocity = Vector3.zero;
                                HP_Sub = 3;
                            }
                            else
                            {
                                HP_Sub = 1;
                            }
                        }
                    }
                    else
                    {
                        aim_enemy = false;
                        speed_bullet = 300;
                    }
                }
            }

            Deactivate_Scope();
            // nếu zoom đến 1 mức độ mới được bắn
            if (mainCamera.fieldOfView < zoom_camera + 5)
            {
                Shoot();
                start_time_shot = Time.time;
            }
            is_hold = false;
        }
        slow_motion();
        if (mainCamera.fieldOfView == 60 && is_hold) Activate_Scope();

        //tính toán góc xoay camera khi kéo
        if (is_hold && Time.timeScale == 1)
        {
            Vector2 deltaPosition = (Vector2)Input.mousePosition - start_position;

            float deltaX = deltaPosition.x * sensitivity_camera * Time.deltaTime * 50;
            float deltaY = -deltaPosition.y * sensitivity_camera * Time.deltaTime * 50;

            // Tính toán và giới hạn góc quay.
            xRotation += deltaY;
            xRotation = Mathf.Clamp(xRotation, -30, 30);
            yRotation += deltaX;
            yRotation = Mathf.Clamp(yRotation, -45, 45);

            //mainCamera.transform.localRotation = Quaternion.Euler(xRotation, yRotation, 0f);
            mainCamera.transform.DOLocalRotate(new Vector3(xRotation, yRotation, 0f), 0.2f);

            start_position = Input.mousePosition;
        }
    }





    void Activate_Scope()
    {
        scope.color = scope_Active_Color; // Hiện ống ngắm
        DOTween.To(() => vignetteEffect.intensity.value,
           x => vignetteEffect.intensity.value = x,
           0.4f,
           0.5f);

        mainCamera.DOFieldOfView(zoom_camera, 0.5f);
    }

    void Deactivate_Scope()
    {
        scope.color = scope_Not_Active_Color; // Ẩn ống ngắm
        DOTween.To(() => vignetteEffect.intensity.value,
            x => vignetteEffect.intensity.value = x,
            0f,
            0.5f);

        mainCamera.DOFieldOfView(normal_camera, 0.5f);
    }

    void Shoot()
    {
        play_shot_anim = true;
        Sound_Manager.Instance.Play_Sound("GunShot");

        //Invoke("Recoil", 0.1f);
        Invoke("play_reload", 0.5f);

        bullet.SetActive(true);
        bullet.transform.position = transform.position;
        bullet.transform.forward = transform.forward;
        bullet_rb.velocity = mainCamera.transform.forward * speed_bullet;
    }
    void slow_motion()
    {
        //bật virtural để tạo video slow motion
        if (bullet.activeSelf && aim_enemy)
        {
            play_shot_anim = false;

            virtual_camera.SetActive(true);
            smoke_bullet.SetActive(true);

            GetComponent<BoxCollider>().enabled = false;
            cinemachineBrain.enabled = true;
            check = true;
        }
        else
        {
            transform.position = new Vector3(0, 15, 0);

            if (check)
            {
                play_shot_anim = false;
                virtual_camera.SetActive(false);
                smoke_bullet.SetActive(false);

                GetComponent<BoxCollider>().enabled = true;
                cinemachineBrain.enabled = false;
                transform.rotation = Quaternion.Euler(xRotation, yRotation, 0f);
            }
            check = false;
            aim_enemy = false;
            speed_bullet = 300;
        }
    }
    public int count_obj_active_true()
    {
        int count_obj_active_true = 0;
        foreach (GameObject obj in spawn.pooledObjects)
        {
            if (obj.activeSelf && obj.GetComponent<Animator>().GetInteger("state") != 16)
            {
                count_obj_active_true++;
            }
        }
        return count_obj_active_true;
    }
    public IEnumerator Shake(float duration, float startMagnitude)
    {
        Sound_Manager.Instance.Play_Sound("Explosion");

        Vector3 originalPos = transform.localPosition;
        float elapsed = 0.0f;

        while (elapsed < duration && Time.timeScale == 1)
        {
            float remainingTime = duration - elapsed;
            float magnitude = Mathf.Lerp(0f, startMagnitude, remainingTime / duration);

            Vector3 targetPos = originalPos + new Vector3(Random.Range(-1f, 1f) * magnitude, Random.Range(-1f, 1f) * magnitude, 0);
            transform.localPosition = Vector3.Lerp(transform.localPosition, targetPos, 0.5f);

            elapsed += Time.deltaTime;

            yield return null;
        }

        transform.localPosition = originalPos;
    }
    void play_reload()
    {
        Sound_Manager.Instance.Play_Sound("Reload");
    }
    public void Recoil()
    {
        sniper.transform.DOLocalMove(sniper.transform.localPosition - sniper.transform.forward * 0.05f, 0.1f).SetEase(Ease.OutExpo)
          .OnComplete(() =>
          {
              sniper.transform.DOLocalMove(originalPosition, 0.5f).SetEase(Ease.OutCubic);
              Invoke("TriggerReload", 0.3f);
          });
    }
    private void TriggerReload()
    {
        //Vector3 reloadPosition = Vector3.left * 0.05f + Vector3.up * 0.02f;
        //shell.transform.DOLocalMove(shell.transform.localPosition + reloadPosition, 0.1f).SetEase(Ease.InOutQuad)
        //    .OnComplete(() =>
        //    {
        //        // Hiệu ứng vỏ đạn rơi xuống
        //        Vector3 fallDirection = Vector3.left * 0.02f + Vector3.down * 0.1f; // Điều chỉnh hướng và cường độ

        //        shell.transform.DOLocalMove(shell.transform.localPosition + fallDirection, 0.3f)
        //            .SetEase(Ease.InQuad)
        //            .OnComplete(() => shell.transform.DOLocalMove(originalPosition, 0.3f));

        //    });
        sniper.transform.DOLocalRotate(new Vector3(0, 0, 35f), 0.3f)
            .OnComplete(() =>
            {
                sniper.transform.DOLocalRotate(Vector3.zero, 0.3f);

                ////Hiệu ứng vỏ đạn rơi xuống
                //Vector3 fallDirection = Vector3.left * 0.02f + Vector3.down * 0.1f; // Điều chỉnh hướng và cường độ

                //shell.transform.DOLocalMove(shell.transform.localPosition + fallDirection, 0.3f)
                //    .SetEase(Ease.InQuad)
                //    .OnComplete(() => shell.transform.localPosition = originalPosition);
            });
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("enemy"))
        {
            Deactivate_Scope();
        }
    }
}
