using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public GameObject player;
    public PlayerMove playerScript;
    Queue<Coin> coinQueue = new Queue<Coin>();
    public GameObject coinPool;
    public GameObject coinPrefab;
    private void Awake()
	{
        if (instance != null)
        {
            Debug.Log("���� �Ŵ��� �ߺ�");
            Destroy(this);
        }
        instance = this;

    }
	void Start()
    {
        for (int i = 0; i < 3; i++)
		{
            NewCoin();
        }
    }

    public void CoinPool(Vector3 pos,int coinNum)
    {
		for (int i = 0; i < coinNum; i++)
		{
            if(coinQueue.Count == 0)
            {
                NewCoin();
            }
            Coin retrunCoin = coinQueue.Dequeue();
            retrunCoin.gameObject.transform.position = pos;
            retrunCoin.gameObject.SetActive(true);
            retrunCoin.dropCoin();
        }
    }
    private void NewCoin()
    {
        GameObject coin = Instantiate(coinPrefab, coinPool.transform);
        Coin coinCs = coin.GetComponent<Coin>();
        coinCs.getCoin += () => { coinQueue.Enqueue(coinCs); };
        coin.SetActive(false);
        coinQueue.Enqueue(coinCs);
    }
}