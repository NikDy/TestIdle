using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;

public class GUIUpdate : MonoBehaviour
{
    [SerializeField]
    private GameObject coin;
    [SerializeField]
    private RectTransform coinTarget;
    [SerializeField]
    private float coinMoveSpeed;
    [SerializeField]
    private TextMeshProUGUI textMeshPro;

    [SerializeField]
    private TextMeshProUGUI stacksText;

    private int total = 0;
    private int current = 0;
    private float lastTextUpdate = 0;
    [SerializeField]
    private float textUpdatePeriod;
    [SerializeField]
    private float textShakeDuration;
    private bool isShaking = false;

    private int stacks = 0;

    private List<RectTransform> coins = new List<RectTransform>();

    // Start is called before the first frame update
    void Start()
    {
        PlayerUpdate.SaleEvent += CreateAndMoveCoin;
        StackUpdate.HitEvent += CollectStack;
        TraidpointUpdate.HitEvent += SellStacks;
    }

    public void CollectStack(GameObject stack)
    {
        if(stacks < 40) stacks++;
        stacksText.text = stacks.ToString() + "/40";
        stacksText.ForceMeshUpdate();
    }

    public void SellStacks()
    {
        
    }

    public void CreateAndMoveCoin(Vector3 beginPosition)
    {
        if(stacks > 0) stacks--;
        stacksText.text = stacks.ToString() + "/40";
        stacksText.ForceMeshUpdate();
        var start = Camera.main.WorldToScreenPoint(beginPosition);
        coins.Add(Instantiate(coin, start, Quaternion.identity, gameObject.transform).GetComponent<RectTransform>());        
    }


    // Update is called once per fram
    void Update()
    {
        for (int i = 0; i < coins.Count; i++)
        {
            var coin = coins[i];
            var xDiriection = (coin.transform.position.x - coinTarget.transform.position.x) / Vector2.Distance(coin.transform.position, coinTarget.transform.position);
            var yDiriection = (coin.transform.position.y - coinTarget.transform.position.y) / Vector2.Distance(coin.transform.position, coinTarget.transform.position);            
            coin.transform.Translate(new Vector3(coinMoveSpeed * -xDiriection * Time.deltaTime,
                                                 coinMoveSpeed * -yDiriection * Time.deltaTime)); 
            if (Vector2.Distance(coin.transform.position, coinTarget.transform.position) < 130f)
            {                
                total += EntityHandler.Instance.coinValue;                                
                Destroy(coin.gameObject);
                coins.RemoveAt(i);
                i--;
            }
            coin.ForceUpdateRectTransforms();
        }
        
        if (Time.time - lastTextUpdate > textUpdatePeriod && current < total)
        {
            current++;
            textMeshPro.text = current.ToString();
            textMeshPro.ForceMeshUpdate();
            if (!isShaking)
            {
                isShaking = true;
                textMeshPro.transform.DOShakePosition(textShakeDuration, 3, 5).OnComplete(delegate () { isShaking = false; });
            }
        }
    }

}
