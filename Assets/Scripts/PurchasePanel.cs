using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;

public class PurchasePanel : MonoBehaviour
{
    [SerializeField] private RectTransform panel;
    [SerializeField] private Toggle buyForSelfToggle;
    [SerializeField] private Toggle buyForFriendToggle;
    [SerializeField] private GameObject friendsListContainer;
    [SerializeField] private Transform friendsListParent;
    [SerializeField] private Button friendItemButton;
    [SerializeField] private  Sprite defaultButtonSprite;
    [SerializeField] private  Sprite selectedButtonSprite;
    [SerializeField] private Button buyButton;
    [SerializeField] private TMP_InputField searchInput;
    [SerializeField] private Button searchBtn;
    [SerializeField] private TMP_Text searchErrorTxt;
    [SerializeField] private TMP_Text errorTxt;
    
    private string _selectedFriend = "";
    private List<string> _friends = new List<string> { "Ali", "Reza", "Sara", "Nima", "Mina", "Omid", "Elham", "Farid", "Nasim", "Kian" };
    private Dictionary<string, Button> _friendsList;

    private void Start()
    {
        buyForSelfToggle.onValueChanged.AddListener(OnBuyForSelfToggled);
        buyForFriendToggle.onValueChanged.AddListener(OnBuyForFriendToggled);
        buyButton.onClick.AddListener(OnBuyButtonClicked);
        searchBtn.onClick.AddListener(OnSearchChanged);
        errorTxt.text = "";
        
        PopulateFriendsList();
    }

    private void OnBuyForSelfToggled(bool isOn)
    {
        if (isOn)
        {
            friendsListContainer.SetActive(false);
            buyForFriendToggle.isOn = false;
            ResetFriendSelection();
            _selectedFriend = "";
        }
        else if (!buyForFriendToggle.isOn)
        {
            buyForSelfToggle.isOn = true; 
        }

        errorTxt.text = "";
    }

    private void OnBuyForFriendToggled(bool isOn)
    {
        if (isOn)
        {
            friendsListContainer.SetActive(true);
            buyForSelfToggle.isOn = false;
        }
        else if (!buyForSelfToggle.isOn)
        {
            buyForFriendToggle.isOn = true; 
        }
        
        errorTxt.text = "";
    }
    

    private void PopulateFriendsList()
    {
        _friendsList = new();
        foreach (string friend in _friends)
        {
            Button friendItem = Instantiate(friendItemButton, friendsListParent);
            TMP_Text friendText = friendItem.transform.Find("FriendName").GetComponent<TMP_Text>();
            TMP_Text buttonText = friendItem.transform.Find("Note").GetComponent<TMP_Text>();
            Image buttonImage = friendItem.GetComponent<Image>();
            
            friendText.text = friend;
            buttonText.text = "ﺏﺎﺨﺘﻧﺍ ";
            buttonImage.sprite = defaultButtonSprite;

            friendItem.onClick.AddListener(() => ToggleFriendSelection(friend, buttonText, buttonImage));
            _friendsList.Add(friend, friendItem);
        }
    }

    void ToggleFriendSelection(string friendName, TMP_Text buttonText, Image buttonImage)
    {
        if (_selectedFriend == friendName)
        {
            _selectedFriend = "";
            buttonText.text = "ﺏﺎﺨﺘﻧﺍ ";
            buttonImage.sprite = defaultButtonSprite;
        }
        else
        {
            ResetFriendSelection();
            _selectedFriend = friendName;
            buttonText.text = "ﻑﺍﺮﺼﻧﺍ ";
            buttonImage.sprite = selectedButtonSprite;
        }
        errorTxt.text = "";
    }

    void ResetFriendSelection()
    {
        if (_friendsList.TryGetValue(_selectedFriend, out friendItemButton))
        {
            TMP_Text buttonText = friendItemButton.transform.Find("Note").GetComponent<TMP_Text>();
            Image buttonImage = friendItemButton.GetComponent<Image>();
            buttonText.text = "ﺏﺎﺨﺘﻧﺍ ";
            buttonImage.sprite = defaultButtonSprite;
        }
    }

    private void OnBuyButtonClicked()
    {
        if (buyForSelfToggle.isOn)
        {
            Debug.Log("Buy for myself");
        }
        else if (buyForFriendToggle.isOn)
        {
            if (!string.IsNullOrEmpty(_selectedFriend))
            {
                Debug.Log("Buy for " + _selectedFriend);
            }
            else
            {
                errorTxt.text = "ﺪﯿﻨﻛ  ﺏﺎﺨﺘﻧﺍ ﺖﺳﻭﺩ کﯾ ";
            }
        }
    }
    
    private void OnSearchChanged()
    {
        string query = searchInput.text;
        int resultNo = 0;
        foreach (var item in _friendsList)
        {
            bool check = item.Key.ToLower().Contains(query.ToLower());
            item.Value.gameObject.SetActive(check);
            resultNo += check ? 1 : 0;
        }

        searchErrorTxt.gameObject.SetActive(resultNo == 0 ? true : false);
        //resultTxt.text = resultNo == 0 ? "No results found!" : "";
        
        ResetFriendSelection();
        _selectedFriend = "";
    }
}
