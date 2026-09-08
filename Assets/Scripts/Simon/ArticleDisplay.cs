using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ArticleDisplay : MonoBehaviour
{
    public ArticleSO articleObject;

    public TextMeshProUGUI articleName; // The header of the article
    public TextMeshProUGUI authorAndDate; // Author and date (top left)
    public TextMeshProUGUI rightSideText; // The text on the right side of the top image
    public TextMeshProUGUI articleText; // The text below the top image

    public Image topImage; // The image at the top
    public Image bottomImage; // The image at the top

    void Awake()
    {
        articleName.text = articleObject.articleName;
        authorAndDate.text = articleObject.authorAndDate;
        rightSideText.text = articleObject.rightSideText;
        articleText.text = articleObject.articleText;

        topImage.sprite = articleObject.topImage;
        bottomImage.sprite = articleObject.topImage;
    }
}