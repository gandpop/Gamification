using UnityEngine;
using System.Collections.Generic;

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

    public List<ArticleData> PickArticles(int articlesToPick)
    {
        List<ArticleData> pickedArticles = new List<ArticleData>();

        for (int i = 0; i < articlesToPick; i++)
        {
            int value = Random.Range(0, articles.Length);
            pickedArticles.Add(articles[value]);
        }
        return pickedArticles;
    }
}