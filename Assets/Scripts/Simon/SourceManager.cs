using UnityEngine;

public class SourceManager : MonoBehaviour
{
    public static SourceManager Instance { get; private set; }

    [Header("All Articles")]
    [SerializeField] ArticleData[] articles;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public ArticleData[] ShuffleSources()
    {
        foreach (ArticleData article in articles)
        {
            
        }
    }
}
