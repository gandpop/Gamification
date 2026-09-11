using UnityEngine;
using System.Collections.Generic;

public class SourceManager : MonoBehaviour
{
    public static SourceManager Instance { get; private set; }

    [Header("All Articles")]
    [SerializeField] ArticleData[] articles;

    [Header("Current article and list")]
    [SerializeField] private ArticleData activeArticle; //Serialized for debugging purposes
    public ArticleData ActiveArticle => activeArticle; //Property to acess currentArticle without being able to set anything

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    /// <summary>
    /// Returns a list containing the specified number of articles.
    /// </summary>
    public List<ArticleData> PickArticles(int articlesToPick)
    {
        List<ArticleData> pool = new List<ArticleData>(articles);
        List<ArticleData> pickedArticles = new List<ArticleData>();

        for (int i = 0; i < articlesToPick; i++)
        {
            int value = Random.Range(0, articles.Length);
            pickedArticles.Add(pool[value]);
            pool.RemoveAt(value);
        }
        return pickedArticles;
    }

    public void ShowArticle(ArticleData data)
    {
        activeArticle = data;
    }

    public void ShowNextArticle()
    {
        
    }
}