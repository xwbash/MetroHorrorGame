using System.Collections;
using System.Collections.Generic;
using Manager;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace EvolveGames
{
    public class ItemChange : MonoBehaviour
    {
        [Header("Item Change")]
        [SerializeField] public Animator ani;
        [SerializeField] Image ItemCanvasLogo;
        [SerializeField] bool LoopItems = true;
        [SerializeField, Tooltip("You can add your new item here.")] GameObject[] Items;
        [SerializeField, Tooltip("These logos must have the same order as the items.")] Sprite[] ItemLogos;
        [SerializeField] int ItemIdInt;
        int MaxItems;
        int ChangeItemInt;
        [HideInInspector] public bool DefiniteHide;
        bool ItemChangeLogo;
        private ItemChangeUI _itemChangeUI;

        [Inject]
        private void Construct(ItemChangeUI itemChangeUI)
        {
            _itemChangeUI = itemChangeUI;
            
            ChangeItemInt = ItemIdInt;
            _itemChangeUI.ChangeItem(ItemLogos[ItemIdInt]);
            MaxItems = Items.Length - 1;
            StartCoroutine(ItemChangeObject());
        }
        
        private void Start()
        {
            if (ani == null && GetComponent<Animator>()) ani = GetComponent<Animator>();
            Color OpacityColor = _itemChangeUI.GetColor();
            OpacityColor.a = 0;
            _itemChangeUI.SetColor(OpacityColor);
            ItemChangeLogo = false;
            DefiniteHide = false;
        }
        private void Update()
        {
            if (Input.GetAxis("Mouse ScrollWheel") > 0f)
            {
                ItemIdInt++;
            }

            if (Input.GetAxis("Mouse ScrollWheel") < 0f)
            {
                ItemIdInt--;
            }

            if(Input.GetKeyDown(KeyCode.H))
            {
                if (ani.GetBool("Hide")) Hide(false);
                else Hide(true);
            }

            if (ItemIdInt < 0) ItemIdInt = LoopItems ? MaxItems : 0;
            if (ItemIdInt > MaxItems) ItemIdInt = LoopItems ? 0 : MaxItems;


            if (ItemIdInt != ChangeItemInt)
            {
                ChangeItemInt = ItemIdInt;
                StartCoroutine(ItemChangeObject());
            }
        }

        public void Hide(bool Hide)
        {
            DefiniteHide = Hide;
            ani.SetBool("Hide", Hide);
        }

        IEnumerator ItemChangeObject()
        {
            if(!DefiniteHide) ani.SetBool("Hide", true);
            yield return new WaitForSeconds(0.3f);
            for (int i = 0; i < (MaxItems + 1); i++)
            {
                Items[i].SetActive(false);
            }
            Items[ItemIdInt].SetActive(true);
            if (!ItemChangeLogo) StartCoroutine(ItemLogoChange());

            if (!DefiniteHide) ani.SetBool("Hide", false);
        }

        IEnumerator ItemLogoChange()
        {
            ItemChangeLogo = true;
            yield return new WaitForSeconds(0.5f);
            _itemChangeUI.ChangeItem(ItemLogos[ItemIdInt]);
            yield return new WaitForSeconds(0.1f);
            ItemChangeLogo = false;
        }

        private void FixedUpdate()
        {
            
            if (ItemChangeLogo)
            {
                Color OpacityColor = _itemChangeUI.GetColor();
                OpacityColor.a = Mathf.Lerp(OpacityColor.a, 0, 20 * Time.deltaTime);
                _itemChangeUI.SetColor(OpacityColor);
            }
            else
            {
                Color OpacityColor = _itemChangeUI.GetColor();
                OpacityColor.a = Mathf.Lerp(OpacityColor.a, 1, 6 * Time.deltaTime);
                _itemChangeUI.SetColor(OpacityColor);
            }
        }
    }

}
