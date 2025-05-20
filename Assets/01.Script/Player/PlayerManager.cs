
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    //��Ÿ�� �̱��� ���� �� ��� �ڵ�
    private static PlayerManager instance;

    public static PlayerManager Instance
    {
        get
        {

            if (instance == null)
            {
                instance = new GameObject("PlayerManager").AddComponent<PlayerManager>();
            }
            return instance;
        }


    }

    public Player player;

    public Player Player
    {
        get { return player; }
        set { player = value; }
    }
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }

        else
        {
            if (instance == this)
                Destroy(gameObject);
        }
    }
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
