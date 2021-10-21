using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField]private PlayerMove player;

    List<Image> energyImage = new List<Image>();
	private void Awake()
	{
		
	}
	// Start is called before the first frame update
	void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
			for (int i = 0; i < player.energy; i++)
			{
                energyImage[i].gameObject.SetActive(true);
			}
        }
    }
}
