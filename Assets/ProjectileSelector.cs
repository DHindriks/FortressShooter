using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProjectileSelector : MonoBehaviour
{
    [SerializeField]
    List<GameObject> ProjectileList;

    [SerializeField]
    GameObject Button;

    Animator animator;

    void Start()
    {
        GenerateList();
        animator = GetComponentInParent<Animator>();
    }


    public void ToggleWindow()
    {
        animator.SetBool("Open", !animator.GetBool("Open"));
    }

    void GenerateList()
    {
        foreach(GameObject Projectile in ProjectileList)
        {
            GameObject Btn = Instantiate(Button, transform);
            Btn.GetComponent<Image>().sprite = Projectile.GetComponent<ProjectileData>().Icon;
            Btn.GetComponent<Button>().onClick.AddListener(delegate { ToggleWindow(); });
            Btn.GetComponent<Button>().onClick.AddListener(delegate { GameManager.Instance.MainSlingshot.SpawnProjectile(Projectile); });
        }
    }
}
