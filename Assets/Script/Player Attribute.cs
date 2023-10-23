using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerAttribute : MonoBehaviour
{
    // Start is called before the first frame update
    public static float HP;
    public static float nowHP;
    public Slider HPbar;
    public static float EP;
    public static float nowEP;
    public Slider EPbar;
    public static float SP;
    public static float nowSP;
    public Slider SPbar;
    public Image HPbuffer;

    public float RunEPPerFrame = 0.08f;
    public float EPRestorePerFrame = 0.16f;
    public float EPRestoreDelay = 0.5f;
    public float AttackEP = 25f;
    public float SPPerTime = 1f;

    public bool iftest;

    private float timer;
    private bool restore;

    public GameObject HPlinesAbove;
    public GameObject HPlinesRight;
    public GameObject HPlinesBottom;

    public GameObject EPlinesAbove;
    public GameObject EPlinesRight;
    public GameObject EPlinesBottom;

    public GameObject SPlinesAbove;
    public GameObject SPlinesRight;
    public GameObject SPlinesBottom;
    void Awake()
    {
        HP = 300;
        EP = 200;
        SP = 250;
    }
    void Start()
    {
        HPBarFresh();
        EPBarFresh();
        SPBarFresh();
        timer = 0;
        restore = false;
    }

    // Update is called once per frame
    void Update()
    {
        HPbar.value = nowHP;
        EPbar.value = nowEP;
        SPbar.value = nowSP;

        //test
        if (iftest)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                GetHurt(20);
            }
            if (Input.GetKeyDown(KeyCode.X))
            {
                HP += 50;
                HPBarFresh();
            }
            if (Input.GetKeyDown(KeyCode.C))
            {
                EP += 50;
                EPBarFresh();
            }
            if (Input.GetKeyDown(KeyCode.Z))
            {
                Eat(20f);
            }
        }
        
        //Attack
        if (Input.GetMouseButtonDown(0))
        {
            Attack();
        }

        //FastRun
        if (Input.GetKey(KeyCode.LeftShift)) {
            Run();
        }

        //Record time for EP retore delay
        if (Input.GetKeyUp(KeyCode.LeftShift)||Input.GetMouseButtonUp(0))
        {
            timer = Time.time;
            restore = true;
        }

        //Restore EP
        if( restore && Time.time > timer + EPRestoreDelay)
        {
            Restore();
        }
    }

    void FixedUpdate()
    {
        if (HPbuffer.fillAmount > nowHP / HP)
        {
            HPbuffer.fillAmount -= 0.002f * HPbuffer.fillAmount * HP * HPbuffer.fillAmount * HP / (nowHP*nowHP);
        }
        SPConsumption();
    }

    public static void GetHurt(int damage)
    {
        if (nowHP - damage > 0)
        {
            nowHP -= damage;
        }
        else
        {
            nowHP = 0;
            Debug.Log("¼Ä");
        }
    }

    public static void EnergyConsumption()
    {

    }

    private void HPBarFresh()
    {
        HPbar.maxValue = HP;
        nowHP = HP;
        HPbuffer.fillAmount = 1;
        RectTransform HPbarTrans = HPbar.GetComponent<RectTransform>();
        HPbarTrans.sizeDelta = new Vector2(HP,10);
        HPbarTrans.anchoredPosition3D = new Vector3(HP / 2 + 10, -15, 0);
        HPlinesAbove.GetComponent<RectTransform>().sizeDelta = new Vector2(HP + 2, 1);
        HPlinesAbove.GetComponent<RectTransform>().anchoredPosition3D = new Vector3(HP / 2 + 10, -9.5f, 0);

        HPlinesBottom.GetComponent<RectTransform>().sizeDelta = new Vector2(HP + 2, 1);
        HPlinesBottom.GetComponent<RectTransform>().anchoredPosition3D = new Vector3(HP / 2 + 10, -20.5f, 0);

        HPlinesRight.GetComponent<RectTransform>().anchoredPosition3D = new Vector3(HP + 10.5f, -15f, 0);
    }

    private void EPBarFresh()
    {
        EPbar.maxValue = EP;
        nowEP = EP;
        RectTransform EPbarTrans = EPbar.GetComponent<RectTransform>();
        EPbarTrans.sizeDelta = new Vector2(EP, 10);
        EPbarTrans.anchoredPosition3D = new Vector3(EP / 2 + 10, -26.5f, 0);

        EPlinesAbove.GetComponent<RectTransform>().sizeDelta = new Vector2(EP + 2, 1);
        EPlinesAbove.GetComponent<RectTransform>().anchoredPosition3D = new Vector3(EP / 2 + 10, -21f, 0);

        EPlinesBottom.GetComponent<RectTransform>().sizeDelta = new Vector2(EP + 2, 1);
        EPlinesBottom.GetComponent<RectTransform>().anchoredPosition3D = new Vector3(EP / 2 + 10, -32f, 0);

        EPlinesRight.GetComponent<RectTransform>().anchoredPosition3D = new Vector3(EP + 10.5f, -26.5f, 0);
    }

    private void SPBarFresh()
    {
        SPbar.maxValue = SP;
        nowSP = SP;
        RectTransform SPbarTrans = SPbar.GetComponent<RectTransform>();
        SPbarTrans.sizeDelta = new Vector2(SP, 10);
        SPbarTrans.anchoredPosition3D = new Vector3(SP / 2 + 10, -38, 0);

        SPlinesAbove.GetComponent<RectTransform>().sizeDelta = new Vector2(SP + 2, 1);
        SPlinesAbove.GetComponent<RectTransform>().anchoredPosition3D = new Vector3(SP / 2 + 10, -32.5f, 0);

        SPlinesBottom.GetComponent<RectTransform>().sizeDelta = new Vector2(SP + 2, 1);
        SPlinesBottom.GetComponent<RectTransform>().anchoredPosition3D = new Vector3(SP / 2 + 10, -43.5f, 0);

        SPlinesRight.GetComponent<RectTransform>().anchoredPosition3D = new Vector3(SP + 10.5f, -38f, 0);
    }
    
    private void Run()
    {
        restore = false;
        if (nowEP > RunEPPerFrame)
        {
            nowEP -= RunEPPerFrame;
        }
        else
        {
            nowEP = 0;
        }
    }

    private void Restore()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift) || Input.GetMouseButtonDown(0))
        {
            restore = false;
        }
        else
        {
            if (nowEP <= EP - EPRestorePerFrame)
            {
                nowEP += EPRestorePerFrame;
            }
            else
            {
                nowEP = EP;
                restore = false;
            }
        }
    }

    private void Attack()
    {
        restore = false;
        if (nowEP > AttackEP)
        {
            Debug.Log("Attack");
            nowEP -= AttackEP;

        }
        else
        {
            nowEP = 0;
        }
    }

    private void SPConsumption()
    {
        if (nowSP > SPPerTime)
        {
            nowSP -= SPPerTime;
        }
        else
        {
            nowSP = 0;
            Debug.Log("¶öËÀÁË£¡");
        }
    }

    public void Eat(float food)
    {
        if(nowSP < SP - food)
        {
            nowSP += food;
        }
        else
        {
            nowSP = SP;
        }
    }
}


