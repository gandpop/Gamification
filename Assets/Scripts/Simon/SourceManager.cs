using UnityEngine;
using System.Collections.Generic;

public class SourceManager : MonoBehaviour
{
    public static SourceManager Instance { get; private set; }

    [Header("All Articles (set manually)")]
    [SerializeField] ArticleData[] articles;

    [Header("Active article and list (serialized only for debugging purposes)")]
    [SerializeField] private ArticleData activeArticle; 
    public ArticleData ActiveArticle => activeArticle; //Property to acess currentArticle without being able to set anything
    [SerializeField] private List<ArticleData> listOfPickedArticles;
    public List<ArticleData> ListOfPickedArticles => listOfPickedArticles;
    
    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    /// <summary>
    /// Randomly selects specified amount of articles, and saves them to listOfPickedArticles.
    /// </summary>
    public void PickArticles(int articlesToPick)
    {
        List<ArticleData> pool = new List<ArticleData>(articles);
        List<ArticleData> pickedArticles = new List<ArticleData>();

        for (int i = 0; i < articlesToPick; i++)
        {
            int value = Random.Range(0, pool.Count);
            pickedArticles.Add(pool[value]);
            pool.RemoveAt(value);
        }
        listOfPickedArticles = pickedArticles;

    }

    public void ShowFirstArticle()
    {
        activeArticle = listOfPickedArticles[0];
        GameObject articleObj = GameObject.FindWithTag("Article");
        articleObj.GetComponent<SpriteRenderer>().sprite = activeArticle.ArticleTexture;
        listOfPickedArticles.RemoveAt(0);
    }

    public void ShowNextArticle()
    {
        
    }
}