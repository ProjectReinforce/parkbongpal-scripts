
using UnityEngine;

public class DecompositionUI:MonoBehaviour 
{
    [SerializeField] GameObject breakUI;
    
    [SerializeField] UnityEngine.UI. Text text;
    [SerializeField] UnityEngine.UI.Button baseButton; 
    [SerializeField] UnityEngine.UI.Image baseImage;
    static Color red =  new Color(255/255f, 70/255f, 70/255f), purple =new Color(235/255f, 125/255f, 255/255f);
    public void ViewUpdate(bool isDecompositing)
    {
        text.text = isDecompositing?"분해중" :"무기 분해";
        baseButton.enabled = !isDecompositing;
        baseImage.color = isDecompositing ? red : purple;
        
        breakUI.SetActive(isDecompositing);
    }
    
}
