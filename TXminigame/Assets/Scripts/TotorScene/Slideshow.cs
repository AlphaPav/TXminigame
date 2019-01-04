using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UnityEngine.UI
{
    [AddComponentMenu("UI/Slidershow", 39)]          //添加菜单
    [ExecuteInEditMode]                             //编辑模式下可执行
    [DisallowMultipleComponent]                     //不可重复
    [RequireComponent(typeof(RectTransform))]       //依赖于RectTransform组件
    public class Slideshow : UIBehaviour,IPointerDownHandler,IPointerUpHandler
    {
        public enum MovementType
        {
            /// <summary>
            /// 循环
            /// </summary>
            Circulation,        //循环，轮播到最后一页之后，直接回到第一页

            /// <summary>
            /// 来回往复
            /// </summary>
            PingPong,           //来回往复，轮播到最后一页之后，倒序轮播，到第一页之后，同理
        }

        public enum MoveDir
        {
            Left,
            Right,
        }

        [SerializeField]
        private MovementType m_movement = MovementType.Circulation;
        public MovementType Movement { get { return m_movement; } set { m_movement = value; } }

        [SerializeField]
        private RectTransform m_content;
        public RectTransform Content { get { return m_content; } set { m_content = value; } }

        [SerializeField]
        private Button m_lastPageButton;
        public Button LastPageButton { get { return m_lastPageButton; } set { m_lastPageButton = value; } }

        [SerializeField]
        private Button m_nextPageButton;
        public Button NextPageButton { get { return m_nextPageButton; } set { m_nextPageButton = value; } }

        /// <summary>
        /// 自动轮播时长
        /// </summary>
        [SerializeField]
        private float m_showTime = 2.0f;
        public float ShowTime { get { return m_showTime; } set { m_showTime = value; } }
        
        /// <summary>
        /// 是否自动轮播
        /// </summary>
        [SerializeField]
        private bool m_autoSlide = false;
        public bool AutoSlide { get { return m_autoSlide; }set { m_autoSlide = value; } }

        /// <summary>
        /// 自动轮播方向，-1表示向左，1表示向右
        /// </summary>
        private MoveDir m_autoSlideDir = MoveDir.Right;

        /// <summary>
        /// 是否允许拖动切页
        /// </summary>
        [SerializeField]
        private bool m_allowDrag = true;
        public bool AllowDrag { get { return m_allowDrag; }set { m_allowDrag = value; } }

        /// <summary>
        /// 当前显示页的页码，下标从0开始
        /// </summary>
        private int m_curPageIndex = 0;
        public int CurPageIndex { get { return m_curPageIndex; } }

        /// <summary>
        /// 最大页码
        /// </summary>
        private int m_maxPageIndex = 0;
        public int MaxPageIndex { get { return m_maxPageIndex; } }

        /// <summary>
        /// 圆圈页码ToggleGroup
        /// </summary>
        [SerializeField]
        private ToggleGroup m_pageToggleGroup;
        public ToggleGroup PageToggleGroup { get { return m_pageToggleGroup; } set { m_pageToggleGroup = value; } }
        
        /// <summary>
        /// 圆圈页码Toggle List
        /// </summary>
        private List<Toggle> m_pageToggleList;
        public List<Toggle> PageToggleLise { get { return m_pageToggleList; }}

        //item数目
        private int m_itemNum = 0;
        public int ItemNum { get { return m_itemNum; } }

        //以Toggle为Key，返回页码
        private Dictionary<Toggle, int> m_togglePageNumDic = null;
        
        private float m_time = 0f;

        private List<float> m_childItemPos = new List<float>();

        private GridLayoutGroup m_grid = null;
        
        protected override void Awake()
        {
            base.Awake();
            if (null == m_content)
            {
                throw new Exception("Slideshow content is null");
            }
            else
            {
                m_grid = m_content.GetComponent<GridLayoutGroup>();
                if (m_grid == null)
                {
                    throw new Exception("Slideshow content is miss GridLayoutGroup Component");
                }
                InitChildItemPos();
            }


            if (null != m_lastPageButton)
            {
                m_lastPageButton.onClick.AddListener(OnLastPageButtonClick);
            }
            if (null != m_nextPageButton)
            {
                m_nextPageButton.onClick.AddListener(OnNextPageButtonClick);
            }
            if (null != m_pageToggleGroup)
            {
                int toggleNum = m_pageToggleGroup.transform.childCount;
                if (toggleNum > 0)
                {
                    m_pageToggleList = new List<Toggle>();
                    m_togglePageNumDic = new Dictionary<Toggle, int>();
                    for (int i = 0; i < toggleNum; i++)
                    {
                        Toggle childToggle = m_pageToggleGroup.transform.GetChild(i).GetComponent<Toggle>();
                        if (null != childToggle)
                        {
                            m_pageToggleList.Add(childToggle);
                            m_togglePageNumDic.Add(childToggle, i);
                            childToggle.onValueChanged.AddListener(OnPageToggleValueChanged);
                        }
                    }
                    m_itemNum = m_pageToggleList.Count;
                    m_maxPageIndex = m_pageToggleList.Count - 1;
                }
            }
            UpdateCutPageButtonActive(m_curPageIndex);
        }
        
        private void InitChildItemPos()
        {
            int childCount = m_content.transform.childCount;
            float cellSizeX = m_grid.cellSize.x;
            float spacingX = m_grid.spacing.x;
            float posX = -cellSizeX * 0.5f;
            m_childItemPos.Add(posX);
            for (int i = 1; i < childCount; i++)
            {
                posX -= cellSizeX + spacingX;
                m_childItemPos.Add(posX);
            }
        }

        private void OnPageToggleValueChanged(bool ison)
        {
            if (ison)
            {
                Toggle activeToggle = GetActivePageToggle();
                if (m_togglePageNumDic.ContainsKey(activeToggle))
                {
                    int page = m_togglePageNumDic[activeToggle];
                    SwitchToPageNum(page);
                }
            }
        }

        private Toggle GetActivePageToggle()
        {
            if (m_pageToggleGroup == null || m_pageToggleList == null || m_pageToggleList.Count <= 0)
            {
                return null;
            }
            for (int i = 0; i < m_pageToggleList.Count; i++)
            {
                if (m_pageToggleList[i].isOn)
                {
                    return m_pageToggleList[i];
                }
            }
            return null;
        }

        /// <summary>
        /// 切换至某页
        /// </summary>
        /// <param name="pageNum">页码</param>
        private void SwitchToPageNum(int pageNum)
        {
            if (pageNum < 0 || pageNum > m_maxPageIndex)
            {
                throw new Exception("page num is error");
            }
            if (pageNum == m_curPageIndex)
            {
                //目标页与当前页是同一页
                return;
            }
            m_curPageIndex = pageNum;
            if (m_movement == MovementType.PingPong)
            {
                UpdateCutPageButtonActive(m_curPageIndex);
            }
            Vector3 pos = m_content.localPosition;
            m_content.localPosition = new Vector3(m_childItemPos[m_curPageIndex], pos.y, pos.z);
            m_pageToggleList[m_curPageIndex].isOn = true;

            if (m_onValueChanged != null)
            {
                m_onValueChanged.Invoke(m_pageToggleList[m_curPageIndex].gameObject);
            }
        }
        
        /// <summary>
        /// 根据页码更新切页按钮active
        /// </summary>
        /// <param name="pageNum"></param>
        private void UpdateCutPageButtonActive(int pageNum)
        {
            if (pageNum == 0)
            {
                UpdateLastButtonActive(false);
                UpdateNextButtonActive(true);
            }
            else if (pageNum == m_maxPageIndex)
            {
                UpdateLastButtonActive(true);
                UpdateNextButtonActive(false);
            }
            else
            {
                UpdateLastButtonActive(true);
                UpdateNextButtonActive(true);
            }
        }

        private void OnNextPageButtonClick()
        {
            m_time = Time.time;     //重新计时
            switch (m_movement)
            {
                case MovementType.Circulation:
                    SwitchToPageNum((m_curPageIndex + 1) % m_itemNum);
                    break;
                case MovementType.PingPong:
                    //该模式下，会自动隐藏切页按钮
                    SwitchToPageNum(m_curPageIndex + 1);
                    break;
                default:
                    break;
            }
            Debug.Log(m_content.localPosition);
        }

        private void OnLastPageButtonClick()
        {
            m_time = Time.time; //重新计时
            switch (m_movement)
            {
                case MovementType.Circulation:
                    SwitchToPageNum((m_curPageIndex + m_itemNum - 1) % m_itemNum);
                    break;
                case MovementType.PingPong:
                    //该模式下，会自动隐藏切页按钮
                    SwitchToPageNum(m_curPageIndex - 1);
                    break;
                default:
                    break;
            }
        }

        private void UpdateLastButtonActive(bool activeSelf)
        {
            if (null == m_lastPageButton)
            {
                throw new Exception("Last Page Button is null");
            }
            bool curActive = m_lastPageButton.gameObject.activeSelf;
            if (curActive != activeSelf)
            {
                m_lastPageButton.gameObject.SetActive(activeSelf);
            }
        }

        private void UpdateNextButtonActive(bool activeSelf)
        {
            if (null == m_nextPageButton)
            {
                throw new Exception("Next Page Button is null");
            }
            bool curActive = m_nextPageButton.gameObject.activeSelf;
            if (curActive != activeSelf)
            {
                m_nextPageButton.gameObject.SetActive(activeSelf);
            }
        }

        private Vector3 m_originDragPos = Vector3.zero;
        private Vector3 m_desDragPos = Vector3.zero;
        private bool m_isDrag = false;
        
        public void OnPointerDown(PointerEventData eventData)
        {
            if (!m_allowDrag)
            {
                return;
            }
            if (eventData.button != PointerEventData.InputButton.Left)
            {
                return;
            }
            if (!IsActive())
            {
                return;
            }

            m_isDrag = true;
            m_originDragPos = eventData.position;
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            m_desDragPos = eventData.position;
            MoveDir dir = MoveDir.Right;
            if (m_desDragPos.x < m_originDragPos.x)
            {
                dir = MoveDir.Left;
            }
            switch (dir)
            {
                case MoveDir.Left:
                    if (m_movement == MovementType.Circulation || (m_movement == MovementType.PingPong && m_curPageIndex != 0))
                    {
                        OnLastPageButtonClick();
                    }
                    
                    break;
                case MoveDir.Right:
                    if (m_movement == MovementType.Circulation || (m_movement == MovementType.PingPong && m_curPageIndex != m_maxPageIndex))
                    {
                        OnNextPageButtonClick();
                    }
                    break;
            }
            m_isDrag = false;
        }
        
        /// <summary>
        /// 切页后回调函数
        /// </summary>
        [Serializable]
        public class SlideshowEvent : UnityEvent<GameObject> { }

        [SerializeField]
        private SlideshowEvent m_onValueChanged = new SlideshowEvent();
        public SlideshowEvent OnValueChanged { get { return m_onValueChanged; } set { m_onValueChanged = value; } }
        
        public override bool IsActive()
        {
            return base.IsActive() && m_content != null;
        }

        private void Update()
        {
            if (m_autoSlide && !m_isDrag)
            {
                if (Time.time > m_time + m_showTime)
                {
                    m_time = Time.time;
                    switch (m_movement)
                    {
                        case MovementType.Circulation:
                            m_autoSlideDir = MoveDir.Right;
                            break;
                        case MovementType.PingPong:
                            if (m_curPageIndex == 0)
                            {
                                m_autoSlideDir = MoveDir.Right;
                            }
                            else if (m_curPageIndex == m_maxPageIndex)
                            {
                                m_autoSlideDir = MoveDir.Left;
                            }
                            break;
                    }
                    switch (m_autoSlideDir)
                    {
                        case MoveDir.Left:
                            OnLastPageButtonClick();
                            break;
                        case MoveDir.Right:
                            OnNextPageButtonClick();
                            break;
                    }
                }
            }
        }
    }
}