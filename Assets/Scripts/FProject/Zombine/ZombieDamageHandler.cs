using UnityEngine;
using System.Collections;
namespace FProject
{
    public class ZombieDamageHandler : vp_DamageHandler
    {

        static Vector3 ZOMBIE_HP_SLIDER_POSITION = new Vector3(10000, 10000, 0);

        public Transform headT;
        protected Transform hpTranform;
        /// <summary>
        /// 血条离摄像机的距离
        /// </summary>
        protected float fomat;///
        protected float hpShowTime = 0.0f;
        ZombieEventHandler _eventHandler;
        ZombieEventHandler eventHandler
        {
            get
            {
                if (_eventHandler == null)
                    _eventHandler = GetComponent<ZombieEventHandler>();
                return _eventHandler;
            }
        }
        public override void Die()
        {
            vp_Utility.Activate(hpTranform.gameObject, false);
            hpTranform.position = new Vector3(10000, 1000, 0);
            DestroyImmediate(hpTranform.gameObject);
            if (!eventHandler.Die.Active)
                eventHandler.Die.TryStart();
            GamePlay.Instance.ZombieDie(this.GetComponent<ZombineBase>());
            vp_Utility.Destroy(this.gameObject, 2f);

        }

        protected override void CacheColider()
        {
            var coliders = transform.GetComponentsInChildren<Collider>();
            for (int i = 0; i < coliders.Length; i++)
            {
                Instances.Add(coliders[i], this);
            }
        }

        public override void Damage(vp_DamageInfo damageInfo)
        {
            base.Damage(damageInfo);
            hpShowTime = 2.0f;
        }

        public void SetHP(int hp)
        {
            CurrentHealth = MaxHealth = hp;
        }

        public void SetStartPos(Vector3 v3)
        {
            fomat = Vector3.Distance(v3, Camera.main.transform.position);
        }


        protected Vector3 originScale;
        public void LateUpdate()
        {
            if (eventHandler.Start.Active && !eventHandler.Die.Active)
            {
                if (hpShowTime > 0)
                {
                    hpShowTime -= Time.deltaTime;
                    if (!hpTranform)
                    {
                        hpTranform = UIManager.Instance.CreateZombieHealthBar(this);
                        originScale = hpTranform.localScale;

                    }
                    CommonUtils.SetChildSpriteSliderValue(hpTranform, "Foreground", (float)CurrentHealth / MaxHealth);
                    hpTranform.position = WorldToUI(headT.position);
                    float newFomat = fomat / Vector3.Distance(headT.position, Camera.main.transform.position);
                    hpTranform.localScale = originScale * (newFomat > 3 ? 3 : newFomat);
                }
                else
                {
                    if (hpTranform)
                    {
                        hpTranform.position = ZOMBIE_HP_SLIDER_POSITION;
                    }
                }

            }
        }
        //核心代码在这里把3D点换算成NGUI屏幕上的2D点。
        public static Vector3 WorldToUI(Vector3 point)
        {
            Vector3 pt = Camera.main.WorldToScreenPoint(point);
            //我发现有时候UICamera.currentCamera 有时候currentCamera会取错，取的时候注意一下啊。
            Vector3 ff = UICamera.currentCamera.ScreenToWorldPoint(pt);
            //UI的话Z轴 等于0 
            ff.z = 0;
            return ff;
        }
    }
}