using UnityEngine;
using System.Collections;

public class EnemyTag : MonoBehaviour
{

    /// <summary>
    /// 角色控制器 
    /// </summary>
    public CharacterController character;

    public DamageManager dm;
    /// <summary>
    /// 士兵的类型图标
    /// </summary>
    [Tooltip("士兵的类型图标")]
    public Texture2D textureIcon;
    [Tooltip("背景")]
    public Texture2D textureBg;

    [Tooltip("士兵的类型图标")]
    public Texture2D textureHP;

    public TPSInput tpsInput = null;
    /// <summary>
    /// 角色的高度
    /// </summary>
    float chartHight;
    // Use this for initialization
    void Start()
    {
        if (character == null)
        {
            character = GetComponent<CharacterController>();
        }
        if (!dm)
        {
            dm = GetComponent<DamageManager>();
        }

        chartHight = character.height * transform.localScale.y;
        if(!tpsInput)
        {
            tpsInput = GameObject.FindGameObjectWithTag("Player").GetComponent<TPSInput>();
        }
    }

    public void OnGUI()
    {
        //此处需要添加是否在游戏中的判断,
        //如果游戏结束或者未开始,则不显示
        if(GameValue.staus != GameStatu.InGame)
        {
            return;
        }

        //世界坐标
        Vector3 worldPosition = transform.position + new Vector3(0f, chartHight, 0f);
        //换算成2d屏幕坐标
        Vector3 position = Camera.main.WorldToScreenPoint(worldPosition);
        //真实坐标
        position = new Vector2(position.x, Screen.height - position.y);
        
        //没瞄准的状态下
        if (!tpsInput.IsAim)
        {
            Vector2 bgSize = new Vector2(textureBg.width, textureBg.height);
            Vector2 iconSize = new Vector2(textureIcon.width, textureIcon.height);
            Rect bgRect = new Rect(position.x - (iconSize.x + 10)/ 2, position.y - bgSize.y, (iconSize.x + 10), bgSize.y);
            GUI.DrawTexture(bgRect, textureBg);

            Rect iconRect = new Rect(bgRect.x + 5, bgRect.y + (bgSize.y - iconSize.y) / 2, iconSize.x, iconSize.y);
            GUI.DrawTexture(iconRect, textureIcon);
        }
        else
        {
           
            //绘制背景
            if (textureBg)
            {
                Vector2 bgSize = new Vector2(textureBg.width, textureBg.height);
                // Vector2 bgSize = GUI.skin.label.CalcSize(new GUIContent(textureBg));
                Rect bgRect = new Rect(position.x - bgSize.x / 2, position.y - bgSize.y, bgSize.x, bgSize.y);
                GUI.DrawTexture(bgRect, textureBg);

                //绘制ICON
                // Vector2 iconSize = GUI.skin.label.CalcSize(new GUIContent(textureIcon));
                Vector2 iconSize = new Vector2(textureIcon.width, textureIcon.height);
                Rect iconRect = new Rect(bgRect.x + 5, bgRect.y + (bgSize.y - iconSize.y) / 2, iconSize.x, iconSize.y);
                GUI.DrawTexture(iconRect, textureIcon);

                //绘制血条
                Vector2 hpSize = new Vector2(textureHP.width, textureHP.height);
                if (dm)
                {
                    Rect hpRect = new Rect(iconRect.x + iconRect.size.x + 3, bgRect.y + (bgSize.y - hpSize.y) / 2, hpSize.x * dm.GetHPPrectage(), hpSize.y);
                    GUI.DrawTexture(hpRect, textureHP);
                }
            }
        }
    }
}
