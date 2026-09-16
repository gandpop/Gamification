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

    public void ShowNextArticle()
    {
        activeArticle = listOfPickedArticles[0];
        GameObject articleObj = GameObject.FindWithTag("Article");
        articleObj.GetComponent<SpriteRenderer>().sprite = activeArticle.ArticleTexture;
        listOfPickedArticles.RemoveAt(0);
    }

    public void CheckAnswers(List<int> answers)
    {
        List<int> wronganswers = new List<int>();

        if (answers[0] != (int)activeArticle.Tendens) wronganswers.Add(answers[0]);
        if (answers[1] != (int)activeArticle.Aegthed) wronganswers.Add(answers[1]);
        if (answers[2] != (int)activeArticle.Afhaengighed) wronganswers.Add(answers[2]);
        if (answers[3] != (int)activeArticle.Afsender) wronganswers.Add(answers[3]);
        if (answers[4] != (int)activeArticle.Vidensniveau) wronganswers.Add(answers[4]);
        if (answers[5] != (int)activeArticle.Tid) wronganswers.Add(answers[5]);

        if (wronganswers.Count != 0)
        {
            GameManager.Instance.RemoveHealthPoint(1);
            if (GameManager.Instance.PlayerHealth == 0)
            {
                GameManager.Instance.GameLost();
                return;
            }
            // Highlight wrong answar 
        }
        else // If all answers are correct
        {
            if (listOfPickedArticles.Count == 0)
            {
                //SourceCalculator.Instance.CalculateTrustRating();
                Debug.Log("You win the game");
                UIManager.Instance.ShowUI(UIManager.Instance.GameWonUI);
            }
            else
            {
                Debug.Log("Correct!");
                //SourceCalculator.Instance.CalculateTrustRating();
            }
        }
    }
}