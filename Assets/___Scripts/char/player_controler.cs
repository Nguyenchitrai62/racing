using DG.Tweening;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class player_controler : MonoBehaviour
{
    public GameObject circle;
    public FloatingJoystick joystick;
    public Transform hand;
    public Camera mainCamera;
    public RectTransform objectToPool;
    public GameObject canvas;
    public GameObject ball;

    public RectTransform recttranform;

    public List<GameObject> skin;
    public static int skin_value = 0;
    public static int unlock_skin = 0;

    private Animator anim;
    private Rigidbody rb;
    private List<RectTransform> objectPool;

    public GameObject light_ctrl;
    private Vector2 startTouchPosition;
    private bool isDragging = false;

    public TextMeshProUGUI equip_text;
    int temp_skin_value = 0;

    public RectTransform rect;
    public GameObject longsat;
    public GameObject smoke;

    public static bool cout_down = false;
    private float clock;
    public TextMeshProUGUI text_clock;

    private float slow_speed;
    //private void Reset()
    //{
    //    #if UNITY_EDITOR
    //        Load_Component();
    //    #endif
    //}
    //void Load_Component()
    //{
    //    circle = transform.Find("circle").gameObject;
    //    joystick = GameObject.Find("Canvas").transform.Find("play/BOTTOM/Floating Joystick").GetComponent<FloatingJoystick>();
    //    hand = GameObject.Find("player").transform.Find("Armature/mixamorig:Hips/mixamorig:Spine/mixamorig:Spine1/mixamorig:Spine2/mixamorig:RightShoulder/mixamorig:RightArm/mixamorig:RightForeArm/mixamorig:RightHand");
    //    mainCamera = GameObject.Find("Main Camera").GetComponent<Camera>();
    //    objectToPool = AssetDatabase.LoadAssetAtPath<RectTransform>("Assets/___Prefab/Other/money_ani.prefab");
    //    canvas = GameObject.Find("Canvas").transform.Find("money/TOP/money").gameObject;
    //    ball = GameObject.Find("player").transform.Find("ball").gameObject;
    //    recttranform = GameObject.Find("Canvas").transform.Find("money/TOP/money/money_image").GetComponent<RectTransform>();
    //}

    private void Awake()
    {
        transform.Find("SmokeWhiteSoftTrail").gameObject.SetActive(true);

        slow_speed = canvas_controller.speed * 0.6f;
        cout_down = false;
        Sound_Manager.Instance.stop_all_sound_effect();
        Sound_Manager.Instance.Play_Music("BGM-Action", 0);
        Sound_Manager.Instance.Play_Music("BGM-Action", 1);
        circle.gameObject.SetActive(false);
        rb = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
        ball.SetActive(false);

        foreach (GameObject obj in skin)
        {
            obj.SetActive(false);
        }
        skin_value = PlayerPrefs.GetInt("skin_value", 0);
        anim.avatar = skin[skin_value].GetComponent<Animator>().avatar;
        skin[skin_value].SetActive(true);
    }

    void Start()
    {
        rect.sizeDelta = new Vector2(rect.sizeDelta.x, rect.sizeDelta.y / 16 * 9 * Screen.height / Screen.width);

        objectPool = new List<RectTransform>();
        for (int i = 0; i < 24; i++)
        {
            RectTransform obj = Instantiate(objectToPool);
            obj.transform.SetParent(canvas.transform, false);
            obj.gameObject.SetActive(false);
            objectPool.Add(obj);
        }
    }
    private void Update()
    {
        rotate_model_in_shop();
    }
    private void FixedUpdate()
    {
        rb.AddForce(Vector3.down * 10);
        if (cout_down)
        {
            cout_down = false;
            clock = 30;
            clock_down();
        }
        if (Input.GetKeyDown(KeyCode.Q))
        {
            skin[skin_value].SetActive(false);
            skin_value++;
            if (skin_value == skin.Count) skin_value = 0;
            anim.avatar = skin[skin_value].GetComponent<Animator>().avatar;
            skin[skin_value].SetActive(true);
        }
        if (canvas_controller.Instance.play && canvas_controller.cur_score < canvas_controller.max_score)
        {
            if (Input.GetMouseButton(0) && (joystick.Horizontal != 0 || joystick.Vertical != 0))
            {
                anim.SetInteger("state", 1);
                rb.velocity = new Vector3(joystick.Horizontal * canvas_controller.speed * 0.6f, rb.velocity.y, joystick.Vertical * canvas_controller.speed * 0.6f);
                transform.rotation = Quaternion.LookRotation(new Vector3(rb.velocity.x, 0, rb.velocity.z));
            }
            else
            {
                anim.SetInteger("state", 0);
                rb.velocity = Vector3.zero;
            }
        }

        if (canvas_controller.cur_score == canvas_controller.max_score && !rb.isKinematic)
        {
            Sound_Manager.Instance.Play_Music("BGM-Action", 0);
            Sound_Manager.Instance.stop_all_sound_effect();
            Sound_Manager.Instance.Play_Sound("GameWin");
            rb.velocity = Vector3.zero;
            rb.isKinematic = true;

            Invoke("win_state", 2f);
            anim.SetInteger("state", 4);
            transform.eulerAngles = new Vector3(0, 180, 0);
        }

        if (Circle.check)
        {
            anim.SetInteger("state", 2);
        }
        circle.transform.position = transform.position + new Vector3(0, 0.005f, 0);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("buff") && !ball.activeSelf)
        {
            Sound_Manager.Instance.Play_Sound("CollectStone");
            other.gameObject.SetActive(false);
            ball.gameObject.SetActive(true);
            ball.transform.parent = gameObject.transform;
            ball.transform.position = hand.gameObject.transform.position + new Vector3(0,1,0);
            circle.gameObject.SetActive(true);
        }
        if (other.CompareTag("enemy") && !canvas_controller.Instance.play && clock > 0)
        {
            Sound_Manager.Instance.Play_Music("BGM-Action", 0);
            Sound_Manager.Instance.stop_all_sound_effect();
            Sound_Manager.Instance.Play_Sound("GameLost");

            longsat.SetActive(true);
            smoke.SetActive(true);
            CancelInvoke("clock_down");

            rb.velocity = Vector3.zero;
            rb.isKinematic = true;
            anim.SetInteger("state", 3);
            //Invoke("hanging", 1.3f);
        }
        if (other.CompareTag("destroy") /*&& Vector3.Distance(transform.position, other.transform.position) < 1*/)
        {
            other.gameObject.SetActive(false);
            canvas_controller.cur_score += 1;

            for (int i = 0; i < 8; i++)
            {
                RectTransform obj = Get_Object_From_Pool();
                obj.gameObject.SetActive(true);
                obj.anchoredPosition = obj.transform.InverseTransformPoint(mainCamera.WorldToScreenPoint(other.transform.position));
                Vector3 temp = obj.anchoredPosition + new Vector2(Random.Range(-200, 200), Random.Range(-200, 200));

                obj.DOAnchorPos(temp, 0.5f);
                obj.DOAnchorPos(recttranform.anchoredPosition, 0.7f).SetDelay(0.5f);
            }
            Invoke("plus_10", 1.2f);
        }
        if (other.CompareTag("chest"))
        {
            other.gameObject.SetActive(false);

            for (int i = 0; i < 8; i++)
            {
                RectTransform obj = Get_Object_From_Pool();
                obj.gameObject.SetActive(true);
                obj.anchoredPosition = obj.transform.InverseTransformPoint(mainCamera.WorldToScreenPoint(other.transform.position));
                Vector3 temp = obj.anchoredPosition + new Vector2(Random.Range(-200, 200), Random.Range(-200, 200));

                obj.DOAnchorPos(temp, 0.5f);
                obj.DOAnchorPos(recttranform.anchoredPosition, 0.7f).SetDelay(0.5f);
            }
            Invoke("plus_10", 1.2f);
        }
        if (other.CompareTag("coin") && Vector3.Distance(transform.position, other.transform.position) < 1.6)
        {
            if (other.name.StartsWith("Gold")) canvas_controller.money += 50;
            else canvas_controller.money += 5;
            PlayerPrefs.SetFloat("money", canvas_controller.money);

            other.gameObject.SetActive(false);
            Sound_Manager.Instance.Play_Sound("PointUp");
        }
        if (other.CompareTag("slow"))
        {
            canvas_controller.speed = slow_speed;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("slow"))
        {
            canvas_controller.speed = slow_speed / 0.6f;
        }
    }
    void hanging()
    {
        anim.SetInteger("state", 3);
        rb.isKinematic = true;
        if (transform.rotation.eulerAngles.x == 0)
            transform.rotation = Quaternion.Euler(new Vector3(180, transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z));
        transform.DOMoveY(0.6f, 1).SetEase(Ease.Linear);
    }
    public RectTransform Get_Object_From_Pool()
    {
        foreach (RectTransform obj in objectPool)
        {
            if (!obj.gameObject.activeInHierarchy)
            {
                return obj;
            }
        }

        RectTransform newObj = Instantiate(objectToPool);
        newObj.transform.SetParent(transform.parent, false);
        newObj.gameObject.SetActive(false);
        objectPool.Add(newObj);
        return newObj;
    }

    private void win_state()
    {
        canvas_controller.Instance.win = true;
    }
    public void swap_skin(int i)
    {
        skin_value = i;
        //PlayerPrefs.SetInt("skin_value", skin_value);
        unlock_skin = 0;
    }
    public void EQUIP()
    {
        equip_text.text = "EQUIPED";
        PlayerPrefs.SetInt("skin_value", skin_value);
    }
    public void EQUIP_text_change()
    {
        equip_text.text = "EQUIPED";
    }
    public void show_skin(int i)
    {
        Sound_Manager.Instance.Play_Sound("Tap");
        skin[skin_value].SetActive(false);

        skin_value = i;
        unlock_skin = i;

        anim.avatar = skin[skin_value].GetComponent<Animator>().avatar;
        skin[skin_value].SetActive(true);

        temp_skin_value = PlayerPrefs.GetInt("skin_value", 0);
        if (temp_skin_value != skin_value) equip_text.text = "EQUIP";
        else equip_text.text = "EQUIPED";
    }
    public void plus_10()
    {
        foreach (RectTransform obj in objectPool)
        {
            if (obj.anchoredPosition == recttranform.anchoredPosition) obj.gameObject.SetActive(false);
        }
        canvas_controller.money += 10f;
        PlayerPrefs.SetFloat("money", canvas_controller.money);
    }
    public void setup_light()
    {
        light_ctrl.transform.eulerAngles = new Vector3(180f, 0, 0);
        transform.eulerAngles = new Vector3(0, 180, 0);
    }
    void rotate_model_in_shop()
    {
        if (canvas_controller.Instance.canvas_skin.activeSelf)
        {
            if (Input.GetMouseButtonDown(0) && Input.mousePosition.y > Screen.height / 2)
            {
                isDragging = true;
                startTouchPosition = Input.mousePosition;
            }
            else if (Input.GetMouseButtonUp(0))
            {
                isDragging = false;
            }

            if (isDragging)
            {
                Vector2 currentSwipe = (Vector2)Input.mousePosition - startTouchPosition;
                startTouchPosition = Input.mousePosition;
                float rotationFactor = currentSwipe.x * Time.deltaTime * -100;
                transform.Rotate(Vector3.up, rotationFactor, Space.World);

                //transform.DORotate(transform.rotation.eulerAngles + new Vector3(0, rotationFactor, 0), 0.5f);
            }
            else
            {
                transform.Rotate(Vector3.up, -100 * Time.deltaTime, Space.World);
            }
        }
    }
    void clock_down()
    {
        clock -= 0.05f;
        if (clock >= 0)
        {
            text_clock.text = clock.ToString("F2");
            Invoke("clock_down", 0.05f);
        }
        else
        {
            canvas_controller.cur_score = canvas_controller.max_score;
        }

    }
}
