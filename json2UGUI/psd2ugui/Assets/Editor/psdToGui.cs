using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using System.IO;
using System.Collections;
using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Text.RegularExpressions;


public class YsToUGUI : EditorWindow
{
    static string folderName;
    static YsToUGUI window;

    static string path;

    const int width = 800;
    const int height = 480;

    [MenuItem("ysTool/getUI")]


    static void Init()
    {
        window = (YsToUGUI)EditorWindow.GetWindowWithRect(typeof(YsToUGUI), new Rect(300f, 300f, 300f, 300f), true);



    }
    void OnGUI()
    {

        string jsonFile = "";
        try
        {
            UnityEngine.Object[] selectedAsset = Selection.GetFiltered(typeof(UnityEngine.Object), SelectionMode.Assets);
            folderName = selectedAsset[0].name;
            path = AssetDatabase.GetAssetPath(selectedAsset[0]);
            //Debug.Log(path);
            //object[] ss = AssetDatabase.LoadAllAssetsAtPath(path);

            string[] files = System.IO.Directory.GetFiles(path);
            //Debug.Log(folderName);
            for (int i = 0; i < files.Length; i++)
            {
                string fileName = files[i].Split('\\')[1];
                if (fileName.Split('.')[1] == "json")
                {
                    jsonFile = files[i];
                    break;
                }

            }
        }
        catch (Exception ex)
        {

        }



        GUILayout.Label("选中的文件夹：", EditorStyles.boldLabel);
        GUILayout.Label(folderName, EditorStyles.boldLabel);
        GUILayout.Label("JSON文件：", EditorStyles.boldLabel);
        GUILayout.Label(jsonFile, EditorStyles.boldLabel);
        if (GUILayout.Button("生成", GUILayout.Width(50)))
        {
            if (jsonFile == "")
            {
                this.ShowNotification(new GUIContent("没找到json文件"));
            }
            else
            {
                this.ShowNotification(new GUIContent("开始生成"));
                StreamReader sr = null;
                sr = File.OpenText(jsonFile);
                string jsonText;
                jsonText = sr.ReadToEnd();
                //Debug.Log(jsonText);

                sr.Close();
                sr.Dispose();

                JObject json = (JObject)JsonConvert.DeserializeObject(jsonText);
                GameObject canvas = new GameObject();
                canvas.name = "Canvas";
                canvas.AddComponent<Canvas>();
                canvas.GetComponent<Canvas>().renderMode = RenderMode.ScreenSpaceCamera;
                canvas.AddComponent<CanvasScaler>();
                canvas.AddComponent<GraphicRaycaster>();


                GameObject root = createGo(json, canvas);
                //root.GetComponent<RectTransform>().localPosition = new Vector3(-400, -240, 0);
                root.GetComponent<RectTransform>().localPosition = Vector3.zero;
                
                root.GetComponent<RectTransform>().sizeDelta = Vector3.zero;
                setAnchors(json, root);

                foreach (JToken k in json["child"].Children())
                {
                    //Debug.Log(k);
                    creatrUI(k, root);
                }


            }
            //Debug.Log(jsonFile);

        }

    }

    void creatrUI(JToken uiObject, GameObject parent)
    {
        GameObject thisObject;
        //Debug.Log(uiObject["type"].ToString());


        switch (uiObject["type"].ToString())
        {
            case "p":
                thisObject = createP(uiObject, parent);
                foreach (JToken k in uiObject["child"].Children())
                {
                    creatrUI(k, thisObject);
                }
                break;
            case "bg":
                thisObject = createBg(uiObject, parent);
                break;
            case "btn":
                thisObject = createBtn(uiObject, parent);
                break;
            case "idle":
                thisObject = createBgIdle(uiObject, parent);
                break;
            case "pressed":
                thisObject = createBgPressed(uiObject, parent);
                break;
            case "9":
                thisObject = create9(uiObject, parent);
                break;
            case "txt":
                thisObject = createTxt(uiObject, parent);
                break;
            case "prgh":
                thisObject = createPrgh(uiObject, parent);
                break;
            case "prgv":
                thisObject = createPrgv(uiObject, parent);
                break;
            case "prgr":
                thisObject = createPrgr(uiObject, parent);
                break;
            case "list":
                thisObject = createList(uiObject, parent);
                break;
        }

        //Debug.Log(uiObject["name"]);






    }
    void addImage(JToken uiObject,GameObject go)
    {
        go.AddComponent<Image>();
        //图片资源
        string imgPath = Regex.Split(path, "Resources/")[1];
        Sprite sprite = (Sprite)Resources.Load(imgPath + "/" + uiObject["resource"], typeof(Sprite));
        go.GetComponent<Image>().sprite = sprite;
        //图片坐标,大小
        int[] po = new int[4];
        int i = 0;
        foreach (var k in uiObject["bounds"].Children())
        {
            po[i++] = (int)k;
        }
        go.GetComponent<RectTransform>().sizeDelta = new Vector2(po[2] - po[0], po[3] - po[1]);
        //go.GetComponent<RectTransform>().pivot = Vector2.zero;
        go.GetComponent<RectTransform>().localPosition = new Vector3((po[0] + po[2])/2 - width/2, (po[1]+po[3])/2 - height/2, 0);

        

    }
    void rotateGo(GameObject go,int angle)
    {
        //图片旋转
        //Debug.Log(uiObject["name"].ToString());
        
        Vector3 ro = Vector3.zero;
        if (angle < 0)
        {
            ro.y += 180;
        }
        ro.z += angle;
        go.GetComponent<RectTransform>().localRotation = Quaternion.Euler(ro);
    }
    void setAnchors(JToken uiObject,GameObject go)
    {
        //图片坐标,大小
        int[] po = new int[4];
        int i = 0;
        foreach (var k in uiObject["bounds"].Children())
        {
            po[i++] = (int)k;
        }

        string scale = uiObject["scale"].ToString();

        switch (scale)
        {
            case "dy_a":
                go.GetComponent<RectTransform>().anchorMin = Vector2.zero;
                go.GetComponent<RectTransform>().anchorMax = Vector2.one;
                break;
            case "dy_lr_t":
                break;
            case "dy_lr_c":
                break;
            case "dy_lr_b":
                break;
            case "dy_tb_l":
                break;
            case "dy_tb_c":
                break;
            case "dy_tb_r":
                break;

            case "st_c":
                break;
            case "st_t":
                break;
            case "st_b":
                break;
            case "st_l":
                break;
            case "st_r":
                break;
            case "st_lt":
                break;
            case "st_rt":
                break;
            case "st_lb":
                break;
            case "st_rb":
                break;

        }
    }



    GameObject createGo(JToken uiObject, GameObject parent)
    {
        GameObject go = new GameObject();
        go.name = uiObject["name"].ToString();
        go.transform.parent = parent.transform;
        go.AddComponent<RectTransform>();

        
        return go;
    }
    GameObject createP(JToken uiObject, GameObject parent)
    {

        GameObject go = createGo(uiObject, parent);
        go.GetComponent<RectTransform>().localPosition = Vector3.zero;
        go.GetComponent<RectTransform>().sizeDelta = Vector3.zero;

        setAnchors(uiObject, go);
        //go.GetComponent<RectTransform>(). = Vector3.zero;
        return go;
    }

    GameObject createBg(JToken uiObject, GameObject parent)
    {
        GameObject go = createGo(uiObject, parent);
        addImage(uiObject, go);
        rotateGo(go, uiObject["angle"].ToObject<int>());

        setAnchors(uiObject, go);
        
        //图片坐标,大小
        int[] po = new int[4];
        int i = 0;
        foreach (var k in uiObject["bounds"].Children())
        {
            po[i++] = (int)k;
        }
        //go.GetComponent<RectTransform>().sizeDelta = new Vector2(po[2] - po[0], po[3] - po[1]);
        //go.GetComponent<RectTransform>().pivot = Vector2.zero;
        //go.GetComponent<RectTransform>().localPosition = new Vector3((po[0] + po[2]) / 2 - width / 2, (po[1] + po[3]) / 2 - height / 2, 0);
        Debug.Log(new Vector2(800 - po[2], po[1]));
        Debug.Log(new Vector2(po[0], 480 - po[3]));
        go.GetComponent<RectTransform>().sizeDelta = new Vector2(-620,-192);
        go.GetComponent<RectTransform>().localPosition =new  Vector2(100,0);
        Debug.Log("~~");
        Debug.Log(go.GetComponent<RectTransform>().rect);
        return go;
    }
    GameObject createBtn(JToken uiObject, GameObject parent)
    {
        GameObject go = createGo(uiObject, parent);
        addImage(uiObject, go);




        go.AddComponent<Button>();
        go.GetComponent<Button>().transition = Selectable.Transition.SpriteSwap;
        string imgPath = Regex.Split(path, "Resources/")[1];
        foreach (JToken k in uiObject["child"].Children())
        {
            if((k["type"].ToString()=="idle"))
            {
                Sprite sprite = (Sprite)Resources.Load(imgPath + "/" + k["resource"], typeof(Sprite));
                go.GetComponent<Image>().sprite = sprite;
                rotateGo(go, k["angle"].ToObject<int>());
            }
            else if(k["type"].ToString()=="pressed")
            {
                Sprite sprite = (Sprite)Resources.Load(imgPath + "/" + k["resource"], typeof(Sprite));
                SpriteState sss = new SpriteState();
                //sss.disabledSprite = sprite;
                //sss.highlightedSprite = sprite;
                sss.pressedSprite = sprite;
                go.GetComponent<Button>().spriteState = sss;
            }
            
        }
        

        return go;
    }
    GameObject createBgIdle(JToken uiObject, GameObject parent)
    {
        GameObject go = createGo(uiObject, parent);
        return go;
    }
    GameObject createBgPressed(JToken uiObject, GameObject parent)
    {
        GameObject go = createGo(uiObject, parent);
        return go;
    }
    GameObject create9(JToken uiObject, GameObject parent)
    {
        GameObject go = createGo(uiObject, parent);
        return go;
    }
    GameObject createTxt(JToken uiObject, GameObject parent)
    {
        GameObject go = createGo(uiObject, parent);
        //坐标,大小
        int[] po = new int[4];
        int i = 0;
        foreach (var k in uiObject["bounds"].Children())
        {
            po[i++] = (int)k;
        }
        go.GetComponent<RectTransform>().localPosition = new Vector3(po[0] - width/2, po[1] - int.Parse(uiObject["size"].ToString()) * 0.3f - height/2, 0);
        go.GetComponent<RectTransform>().pivot = Vector2.zero;

        go.AddComponent<Text>();
        go.GetComponent<Text>().text = uiObject["text"].ToString().Substring(1);//内容
        //字体资源
        string textPath = "Font/" + uiObject["font"];
        Font font = (Font)Resources.Load(textPath, typeof(Font));
        go.GetComponent<Text>().font = font;                          //字体


        go.GetComponent<Text>().color = new Color(float.Parse(uiObject["color"]["red"].ToString()) / 255, float.Parse(uiObject["color"]["green"].ToString()) / 255, float.Parse(uiObject["color"]["blue"].ToString()) / 255);
        go.GetComponent<Text>().fontSize = int.Parse(uiObject["size"].ToString());
        go.GetComponent<Text>().alignment = TextAnchor.LowerLeft;
        return go;
    }
    GameObject createPrgh(JToken uiObject, GameObject parent)
    {
        GameObject go = createGo(uiObject, parent);
        addImage(uiObject, go);
        
        go.GetComponent<Image>().type = Image.Type.Filled;
        go.GetComponent<Image>().fillMethod = Image.FillMethod.Horizontal;
        


        return go;
    }
    GameObject createPrgv(JToken uiObject, GameObject parent)
    {
        GameObject go = createGo(uiObject, parent);
        return go;
    }
    GameObject createPrgr(JToken uiObject, GameObject parent)
    {
        GameObject go = createGo(uiObject, parent);
        return go;
    }
    GameObject createList(JToken uiObject, GameObject parent)
    {
        GameObject go = createGo(uiObject, parent);
        return go;
    }
}