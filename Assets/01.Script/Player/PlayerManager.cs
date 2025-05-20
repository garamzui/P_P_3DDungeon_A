
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    //런타임 싱글톤 생성 및 방어 코드
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
