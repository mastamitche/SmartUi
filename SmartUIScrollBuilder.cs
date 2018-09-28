using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
[ExecuteInEditMode]
public class SmartUIScrollBuilder : MonoBehaviour {
    public ScrollType scrollType;
    public float itemSpacing = 0.0f;
    public float scrollToAnimTime = 0.1f;
    public BuildDirection direction = BuildDirection.Down;

    private GameObject ViewportContent;
    private SmartUI ViewSUI;
    private int OldChildCount = 0;
    private float lastScreenHeight = 0;
    private float lastScreenWidth = 0;
    private float lastItemSpacing = 0;

    private float contentOffset = 0;

	// Use this for initialization
	void Start () {
        ViewportContent = transform.Find("Viewport").Find("Content").gameObject;
        ViewSUI = ViewportContent.GetComponent<SmartUI>();
        if (direction == BuildDirection.Up || direction == BuildDirection.Down)
        {
            GetComponent<ScrollRect>().onValueChanged.AddListener(onScrollVertical);
        }else if(direction == BuildDirection.Left || direction == BuildDirection.Right)
        {
            GetComponent<ScrollRect>().onValueChanged.AddListener(onScrollHorizontal);
        }
	}
	
	// Update is called once per frame
	void Update () {
        if (OldChildCount != getChildCount() || lastItemSpacing != itemSpacing || lastScreenHeight != Screen.height || lastScreenWidth != Screen.width)
        {
            rebuildContent();
            OldChildCount = getChildCount();
            lastItemSpacing = itemSpacing;
            lastScreenHeight = Screen.height;
            lastScreenWidth = Screen.width;
        }
    }

    public void forceRebuild()
    {
        rebuildContent();
    }

    private void onScrollVertical(Vector2 val)
    {
        float change = val.y;
        // 1 is top
        // 0 bottom
        contentOffset = ViewSUI.position.height * (1 - change);
        ViewSUI.position.y =- contentOffset;
    }
    private void onScrollHorizontal(Vector2 val)
    {
        float change = val.x;
        // 1 is top
        // 0 bottom
        contentOffset = ViewSUI.position.width * (1 - change);
        ViewSUI.position.x = -contentOffset;
    }

    private int getChildCount()
    {
        int childCount = 0;
        foreach (Transform child in ViewportContent.transform)
            if (child.gameObject.activeSelf)
                childCount++;
        return childCount;
    }

    private void rebuildContent()
    {
        if (direction == BuildDirection.Down || direction == BuildDirection.Up)
        {
            float finalHeight = 0;
            foreach (Transform child in ViewportContent.transform)
            {
                if (!child.gameObject.activeSelf) continue;
                SmartUI sui = child.GetComponent<SmartUI>();
                Vector2 sizePixels = sui.getSize();
                sui.position.y = sui.convertHeightPixelToPercent(finalHeight);
                sui.setElementToRect();
                finalHeight += (sizePixels.y + itemSpacing * Screen.height);
            }
            float heightPercent = ViewSUI.convertHeightPixelToPercent(finalHeight);
            if (direction == BuildDirection.Up)
            {
                ViewSUI.VAlign = SmartUI.VAligns.bottom;
            }
            ViewSUI.position.height = heightPercent;
            ViewSUI.setElementToRect();
        }else if (direction == BuildDirection.Right || direction == BuildDirection.Left)
        {
            float finalWidth = itemSpacing * Screen.width;
            foreach (Transform child in ViewportContent.transform)
            {
                if (!child.gameObject.activeSelf) continue;
                SmartUI sui = child.GetComponent<SmartUI>();
                Vector2 sizePixels = sui.getSize();
                sui.position.x = sui.convertWidthPixelToPercent(finalWidth);
                sui.setElementToRect();
                finalWidth += (sizePixels.x + itemSpacing * Screen.width);
            }
            float widthPercent = ViewSUI.convertWidthPixelToPercent(finalWidth);
            if (direction == BuildDirection.Right)
            {
                ViewSUI.HAlign = SmartUI.HAligns.left;
            }
            if (direction == BuildDirection.Left)
            {
                ViewSUI.HAlign = SmartUI.HAligns.right;
            }
            ViewSUI.position.width = widthPercent;
            ViewSUI.setElementToRect();
        }
    }
    public void scrollToHorizontal(float value, ScrollToType type)
    {
        switch (type)
        {
            case ScrollToType.PIXEL:
                scrollToHorizontal(value);
                break;

            case ScrollToType.PERCENT:
                scrollToHorizontal(ViewSUI.position.width * value);
                break;
        }
    }
    public void scrollToVertical(float value, ScrollToType type)
    {
        switch (type)
        {
            case ScrollToType.PIXEL:
                scrollToVeritcal(value);
                break;

            case ScrollToType.PERCENT:
                scrollToVeritcal(ViewSUI.position.height * value);
                break;
        }
    }
    public void scrollToHorizontal(GameObject go)
    {
        for (int i = 0; i < ViewportContent.transform.childCount; i++)
        {
            GameObject child = ViewportContent.transform.GetChild(i).gameObject;
            if (go == child)
            {
                scrollToHorizontal(ViewportContent.transform.GetChild(i).gameObject.GetComponent<SmartUI>().position.x);
                return;
            }
        }
    }
    private void scrollToHorizontal(float xPos)
    {
        ViewSUI.addAnimation(new slideX(ViewSUI.position.x, -xPos, scrollToAnimTime, SmartUI.TweenTypes.elasticBoth), null);
    }

    public void scrollToVeritcal(GameObject go)
    {
        for(int i=0;i< ViewportContent.transform.childCount; i++)
        {
            GameObject child = ViewportContent.transform.GetChild(i).gameObject;
            if (go == child)
            {
                scrollToVeritcal(ViewportContent.transform.GetChild(i).gameObject.GetComponent<SmartUI>().position.y);
                return;
            }
        }
    }
    private void scrollToVeritcal(float yPos)
    {
        ViewSUI.addAnimation(new slideY(ViewSUI.position.y, -yPos, scrollToAnimTime, SmartUI.TweenTypes.elasticBoth),null);
    }

    public enum ScrollToType { PIXEL, PERCENT };
    public enum ScrollType {Vertical, Horizontal };
    public enum BuildDirection { Up, Down, Left, Right };
}
