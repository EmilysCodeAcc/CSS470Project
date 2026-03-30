using UnityEngine;
using TMPro;


public class CheckIfUserExists : MonoBehaviour
{
    public GameObject Panel;
    public TMP_InputField NameInputField;
    public DataBaseManager dataBaseManager;

    //hide username prompt if playerprefs has usercreated set to 1
    void Start()
    {
        if(PlayerPrefs.GetInt("UserCreated", 0) == 1)
        {
            Panel.SetActive(false);
        }else
        {
            Panel.SetActive(true);
        }
        
        
    }
}
