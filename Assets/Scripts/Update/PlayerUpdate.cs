using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PlayerUpdate : MonoBehaviour
{
    public GameObject scythe;
    [SerializeField]
    private BoxCollider lookingCollider;
    [SerializeField]
    private Transform storagePivot;
    [Header("Stacking")]    
    [SerializeField]
    private float stackMovementSpeed = 10f;
    [SerializeField]
    private float stackAccuracyPlacing = 0.01f;
    [SerializeField]
    private float stackRotationDuration = 0.1f;
    [Header("Traid Spot Pivot")]
    [SerializeField]
    private Transform traidPivot;


    public delegate void OnPlayerHit(Collider collider);
    static public event OnPlayerHit HitEvent;

    public delegate void OnStackSale(Vector3 salepoint);
    static public event OnStackSale SaleEvent;

    private Animator animator;

    private bool isSplashing = false;
    private bool stackShaking = false;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {

    }


    void FixedUpdate()
    {
        if(animator.GetBool("Run") && !stackShaking)
        {
            stackShaking = true;
            StartCoroutine("ShakeStacks");
        }
    }


    public void SetPosition(Vector3 position)
    {
        transform.position = position;
    }

    private IEnumerator ShakeStacks()
    {
        var sequence = DOTween.Sequence();
        sequence.Append(storagePivot.transform.DOLocalRotate(new Vector3(0f, 0f, 3f),0.1f));
        sequence.Append(storagePivot.transform.DOLocalRotate(new Vector3(0f, 0f, -3f), 0.1f));
        sequence.Append(storagePivot.transform.DOLocalRotate(new Vector3(0f, 0, 0), 0.1f));
        yield return sequence.Play().WaitForCompletion();
        stackShaking = false;
    }

    public void CollectStack(GameObject stack, int shift)
    {         
        float shiftStep = 0.11f; //based on y size of the stack cube
        stack.GetComponent<Rigidbody>().isKinematic = true;
        stack.GetComponent<SphereCollider>().enabled = false;
        stack.GetComponentInChildren<BoxCollider>().enabled = false;
        var tweener = stack.transform.DOMove(storagePivot.position + new Vector3(0, shift * shiftStep, 0), stackMovementSpeed)
                                     .SetEase(Ease.OutExpo)
                                     .SetSpeedBased(true);      
        tweener.OnUpdate(delegate ()
        {
            if (Vector3.Distance(stack.transform.position, storagePivot.position + new Vector3(0, shift * shiftStep, 0)) > stackAccuracyPlacing)
            {                
                tweener.ChangeEndValue(storagePivot.position + new Vector3(0, shift * shiftStep, 0), true);
            }
            else 
            { 
                stack.transform.SetParent(storagePivot, true);
                tweener.Complete();
                tweener.OnComplete(delegate () { stack.transform.localPosition = new Vector3(0, shift * shiftStep, 0); });
                stack.transform.DOLocalRotate(storagePivot.localRotation.eulerAngles, 0.1f)
                               .OnComplete(delegate () { stack.transform.rotation = storagePivot.rotation; });                
            }
        });        
    }


    public void SellStack(GameObject stack)
    {
        stack.transform.DOMove(traidPivot.position, stackMovementSpeed)
                       .SetSpeedBased(true)
                       .SetEase(Ease.InExpo)
                       .OnComplete(delegate () 
                       {                                                      
                           SaleEvent(new Vector3(stack.transform.position.x,
                                                 stack.transform.position.y,
                                                 stack.transform.position.z));
                           Destroy(stack);
                       });
    }



    void OnTriggerEnter(Collider collider)
    {        
        if(isSplashing) HitEvent(collider);
        if (collider.gameObject.name == "Field" && !isSplashing)
        {
            isSplashing = true;
            lookingCollider.enabled = false;
            animator.Play("Slash", 1);
            StartCoroutine(ScythSplash());
        }
    }

    
    private IEnumerator ScythSplash()
    {
        var scyth = Instantiate(this.scythe, transform.GetChild(6).position,
                                             transform.rotation * (new Quaternion(0.5f, 0.5f, -0.5f, 0.5f)),
                                             transform);
        
        yield return scyth.transform.DOLocalRotate(new Vector3(90, -90, 0), 0.3f).
                                     SetEase(Ease.Linear).
                                     WaitForElapsedLoops(1); //fix duration        
        Destroy(scyth);
        isSplashing = false;
        lookingCollider.enabled = true;
    }

    public void StartRun()
    {
        animator.SetBool("Run", true);
    }

    public void StopRun()
    {
        animator.SetBool("Run", false);        
    }

    public void Move(Vector2 direction, float duration)
    {
        Vector3 target = new Vector3(transform.position.x + direction.x,
                                     transform.position.y,
                                     transform.position.z + direction.y);
        transform.LookAt(target, Vector3.up);
        transform.DOMove(target, duration).SetEase(Ease.Linear);
    }
}
