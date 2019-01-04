using System;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.Networking;

public class MapStick : NetworkBehaviour, IPointerUpHandler, IPointerDownHandler
{
    private Image mapImg;
    private Camera mapCamera;
    private bool zoomInFlag = false;    //地图是否已放大
    private float shakeTime = 0;

    GameObject bossMapPos, hero1MapPos, hero2MapPos, hero3MapPos;

    GameObject heromap;

    Vector2 UIVect2;


    // Use this for initialization
    void Start()
    {
        mapImg = this.GetComponent<Image>();
        mapCamera = GameObject.FindGameObjectWithTag("MapCamera").GetComponent<Camera>();
        heromap = GameObject.Find("MapStick");
        if (NetChoose.chosenID == 0)
        {
            bossMapPos = GameObject.Find("VirtualJoystick/PosIcon/BossIcon");
            heromap.SetActive(false);
            //PagesUpdate();
        }
        else
        {
            hero1MapPos = GameObject.Find("VirtualJoystick/PosIcon/Hero1Icon");
            hero2MapPos = GameObject.Find("VirtualJoystick/PosIcon/Hero2Icon");
            hero3MapPos = GameObject.Find("VirtualJoystick/PosIcon/Hero3Icon");
        }

        UIVect2 = GameObject.Find("VirtualJoystick").GetComponent<RectTransform>().sizeDelta;
    }

    void Update()
    {
        shakeTime += Time.deltaTime;
        //chosenID为0 选择Boss
        if (NetChoose.chosenID == 0)
        {
            BossPosUpdate();
            //PagesUpdate();
        }
        else
        {
            HerosPosUpdate();
        }
        //HerosPosUpdate();
        //BossPosUpdate();
    }

    /// <summary>
    /// Boss 小地图位置更新
    /// </summary>
    private void BossPosUpdate()
    {
        Vector3 tempPos;
        //获取boss位置
        if (GameObject.FindGameObjectWithTag("Boss") != null)
        {
            tempPos = GameObject.FindGameObjectWithTag("Boss").transform.position;
            //小地图放大
            if (zoomInFlag)
            {

            }
            else
            {
                bossMapPos.GetComponent<RectTransform>().anchoredPosition = new Vector3(-UIVect2.x * 0.26f * (19 - tempPos.x) / 37, -UIVect2.y * 0.35f * (7.5f - tempPos.z) / 25, 0);
            }
        }
    }

    /// <summary>
    /// Pages位置显示
    /// </summary>
    //private void PagesUpdate()
    //{
    //    Vector3 tempPos;
    //    if (GameObject.FindGameObjectsWithTag("GoldenPage") != null)
    //    {
    //        GameObject[] GoldenPages = GameObject.FindGameObjectsWithTag("GoldenPage");
    //        for (int i = 0; i < GoldenPages.Length; ++i)
    //        {
    //            tempPos = GoldenPages[i].transform.position;
    //            //小地图放大
    //            if (zoomInFlag)
    //            {

    //            }
    //            else
    //            {
    //                GoldenPagesMapPos[i].GetComponent<RectTransform>().anchoredPosition = new Vector3(-UIVect2.x * 0.26f * (19 - tempPos.x) / 37, -UIVect2.y * 0.35f * (7.5f - tempPos.z) / 25, 0);
    //            }
    //            /*
    //             * 解锁到50% 小地图上的图标闪烁
    //             */
    //            if (GoldenPages[i].GetComponent<PageInfo>().unlock_time_left < GoldenPages[i].GetComponent<PageInfo>().unlock_time_total * 0.5f)
    //            {
    //                shakePagePosInMap(GoldenPagesMapPos[i]);
    //            }

    //            if (GoldenPages[i].GetComponent<PageInfo>().unlock_time_left <= 0)
    //            {
    //                GoldenPages[i].GetComponent<RectTransform>().anchoredPosition = new Vector3(20, 20, 0);
    //            }
    //        }
    //    }
    //    if (GameObject.FindGameObjectsWithTag("BlackPage") != null)
    //    {
    //        GameObject[] BlackPages = GameObject.FindGameObjectsWithTag("BlackPage");
    //        for (int i = 0; i < BlackPages.Length; ++i)
    //        {
    //            tempPos = BlackPages[i].transform.position;
    //            //小地图放大
    //            if (zoomInFlag)
    //            {

    //            }
    //            else
    //            {
    //                BlackPagesMapPos[i].GetComponent<RectTransform>().anchoredPosition = new Vector3(-UIVect2.x * 0.26f * (19 - tempPos.x) / 37, -UIVect2.y * 0.35f * (7.5f - tempPos.z) / 25, 0);
    //            }
    //            /*
    //             * 解锁到50% 小地图上的图标闪烁
    //             */
    //            if (BlackPages[i].GetComponent<PageInfo>().unlock_time_left < BlackPages[i].GetComponent<PageInfo>().unlock_time_total * 0.5f)
    //            {
    //                shakePagePosInMap(BlackPagesMapPos[i]);
    //            }

    //            if (BlackPages[i].GetComponent<PageInfo>().unlock_time_left <= 0)
    //            {
    //                BlackPages[i].GetComponent<RectTransform>().anchoredPosition = new Vector3(20, 20, 0);
    //            }
    //        }
    //    }
    //    if (GameObject.FindGameObjectsWithTag("WhitePage") != null)
    //    {
    //        GameObject[] WhitePages = GameObject.FindGameObjectsWithTag("WhitePage");
    //        for (int i = 0; i < WhitePages.Length; ++i)
    //        {
    //            tempPos = WhitePages[i].transform.position;
    //            //小地图放大
    //            if (zoomInFlag)
    //            {

    //            }
    //            else
    //            {
    //                WhitePagesMapPos[i].GetComponent<RectTransform>().anchoredPosition = new Vector3(-UIVect2.x * 0.26f * (19 - tempPos.x) / 37, -UIVect2.y * 0.35f * (7.5f - tempPos.z) / 25, 0);
    //            }
    //            /*
    //             * 解锁到50% 小地图上的图标闪烁
    //             */
    //            if (WhitePages[i].GetComponent<PageInfo>().unlock_time_left < WhitePages[i].GetComponent<PageInfo>().unlock_time_total * 0.5f)
    //            {
    //                shakePagePosInMap(WhitePagesMapPos[i]);
    //            }

    //            if (WhitePages[i].GetComponent<PageInfo>().unlock_time_left <= 0)
    //            {
    //                WhitePagesMapPos[i].GetComponent<RectTransform>().anchoredPosition = new Vector3(20, 20, 0);
    //            }
    //        }
    //    }
    //}

    /// <summary>
    /// 闪烁脚本
    /// </summary>
    /// <param name="obj">闪烁物体</param>
    //private void shakePagePosInMap(GameObject obj)
    //{
    //    Image tx = obj.GetComponent<Image>();

    //    if (shakeTime % 4 > 2f)
    //        tx.enabled = true;
    //    else
    //        tx.enabled = false;
    //}

    /// <summary>
    /// Heros 小地图位置更新
    /// </summary>
    private void HerosPosUpdate()
    {
        Vector3 tempPos;

        if (GameObject.FindGameObjectWithTag("Hero1") != null)
        {
            tempPos = GameObject.FindGameObjectWithTag("Hero1").transform.position;
            //小地图放大
            if (zoomInFlag)
            {

            }
            else
            {
                hero1MapPos.GetComponent<RectTransform>().anchoredPosition = new Vector3(-UIVect2.x * 0.26f * (19 - tempPos.x) / 37, -UIVect2.y * 0.35f * (7.5f - tempPos.z) / 25, 0);
            }
        }
        if (GameObject.FindGameObjectWithTag("Hero2") != null)
        {
            tempPos = GameObject.FindGameObjectWithTag("Hero2").transform.position;
            //小地图放大
            if (zoomInFlag)
            {
            }
            else
            {
                hero2MapPos.GetComponent<RectTransform>().anchoredPosition = new Vector3(-UIVect2.x * 0.26f * (19 - tempPos.x) / 37, -UIVect2.y * 0.35f * (7.5f - tempPos.z) / 25, 0);
            }
        }
        if (GameObject.FindGameObjectWithTag("Hero3") != null)
        {
            tempPos = GameObject.FindGameObjectWithTag("Hero3").transform.position;
            //小地图放大
            if (zoomInFlag)
            {

            }
            else
            {
                hero3MapPos.GetComponent<RectTransform>().anchoredPosition = new Vector3(-UIVect2.x * 0.26f * (19 - tempPos.x) / 37, -UIVect2.y * 0.35f * (7.5f - tempPos.z) / 25, 0);
            }
        }
    }

    public virtual void OnPointerDown(PointerEventData ped)
    {
        //Vector2 pos = new Vector2();
        //if (RectTransformUtility.ScreenPointToLocalPointInRectangle(mapImg.rectTransform, ped.position, ped.pressEventCamera, out pos))
        //{
        //    ZoomMap(ped);
        //}
    }

    public void OnPointerUp(PointerEventData eventData)
    {

    }

    /// <summary>
    /// 放大小地图
    /// </summary>
    /// <param name="ped"></param>
    //private void ZoomMap(PointerEventData ped)
    //{
    //    Vector3 temp;
    //    //地图已放大
    //    if (zoomInFlag)
    //    {
    //        mapCamera.rect = new Rect(0.65f, 0.65f, 1, 1);
    //        //mapCamera.fieldOfView = 40;
    //        zoomInFlag = false;
    //    }
    //    else
    //    {
    //        mapCamera.rect = new Rect(0.3f, 0.3f, 1, 1);
    //        //mapCamera.fieldOfView = 100;
    //        zoomInFlag = true;
    //    }
    //}
}
