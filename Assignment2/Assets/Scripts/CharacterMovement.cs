using UnityEngine;
using System.Collections;

public class CharacterMovement : MonoBehaviour {

    public float speed = 6f;
    public float turnSpeed = 60f;
    public float turnSmoothing = 15f;
    public Material MoneyObjMaterial;

    private Vector3 movement;
    private Vector3 turning;
    private Animator anim;
    private Rigidbody playerRigidbody;
    private int _goldCount = 0;

    void Awake()
    {
        anim = GetComponent<Animator>();
        playerRigidbody = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        float lh = Input.GetAxisRaw("Horizontal");
        float lv = Input.GetAxisRaw("Vertical");

        Move(lh, lv);
        Animating(lh, lv);
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Money")
        {
            MakeMoney();
            Destroy(other.gameObject);
        }
    }

    void MakeMoney()
    {
        _goldCount++;
        GameObject.Find("GoldCountText").GetComponent<TextMesh>().text = string.Format("Gold: {0}", _goldCount);
    }

    void Move(float lh, float lv)
    {
        movement.Set(lh, 0f, lv);
        movement = movement.normalized * speed * Time.deltaTime;
        playerRigidbody.MovePosition(transform.position + movement);

        if (lh != 0f || lv != 0f)
        {
            Rotating(lh, lv);
        }
    }

    void Rotating(float lh, float lv)
    {
        Vector3 targetDirection = new Vector3(lh, 0f, lv);
        Quaternion targetRotation = Quaternion.LookRotation(targetDirection, Vector3.up);
        Quaternion newRotation = Quaternion.Lerp(GetComponent<Rigidbody>().rotation, targetRotation, turnSmoothing * Time.deltaTime);
        GetComponent<Rigidbody>().MoveRotation(newRotation);
    }

    void Animating(float lh, float lv)
    {
        bool running = lh != 0f || lv != 0f;
        anim.SetBool("IsRunning", running);
    }

    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        SpendMoney();
    }

    void SpendMoney()
    {
        var AttachedGameObjPos = gameObject.transform.position;

        if (Input.GetKeyDown("space") && _goldCount > 0){
            _goldCount--;
            GameObject.Find("GoldCountText").GetComponent<TextMesh>().text = string.Format("Gold: {0}", _goldCount);
            var moneyObj = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
            moneyObj.tag = "Money";
            moneyObj.transform.position = new Vector3(AttachedGameObjPos.x + 3, 2.5f, AttachedGameObjPos.z);
            moneyObj.transform.localScale = new Vector3(2, 0.2f, 2);
            moneyObj.transform.Rotate(270, 0, 0);
            moneyObj.GetComponent<MeshRenderer>().material = MoneyObjMaterial;
            var collider = moneyObj.AddComponent<CapsuleCollider>();
            collider.radius = 0.5f;
            collider.height = 2;
            collider.isTrigger = true;
        }
    }
}
