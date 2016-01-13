using UnityEngine;
//using UnityEngine.UI;
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

    [MenuItem("ysTool/getUI")]


    static void Init()
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


        if (jsonFile == "")
        {
            Debug.Log("没找到json文件");
        }
        else
        {
            Debug.Log("开始生成");

            StreamReader sr = null;
            sr = File.OpenText(jsonFile);
            string jsonText;
            jsonText = sr.ReadToEnd();
            //Debug.Log(jsonText);
            
            sr.Close();
            sr.Dispose();
            
            JObject json = (JObject)JsonConvert.DeserializeObject(jsonText);

            GameObject uiRoot = new GameObject();
            uiRoot.name = "UI Root";
            uiRoot.AddComponent<UIRoot>();

            GameObject camera = new GameObject();
            camera.name = "Camera";
            camera.AddComponent<Camera>();
            camera.AddComponent<UICamera>();
            camera.transform.parent = uiRoot.transform;

            GameObject anchor = new GameObject();
            anchor.name = "Anchor";
            anchor.AddComponent<UIAnchor>();
            anchor.transform.parent = camera.transform;




            //canvas.name = "Canvas";
            //canvas.AddComponent<Canvas>();
            //canvas.GetComponent<Canvas>().renderMode = RenderMode.ScreenSpaceCamera;
            //canvas.AddComponent<CanvasScaler>();
            //canvas.AddComponent<GraphicRaycaster>();
            
            
            //GameObject root = createGo(json, canvas);
            //
            foreach (JToken k in json["child"].Children())
            {
                //Debug.Log(k);
                creatrUI(k, anchor);
            }


        }

    }

   

    static void creatrUI(JToken uiObject, GameObject parent)
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
    static GameObject createGo(JToken uiObject, GameObject parent)
    {
        GameObject go = new GameObject();
        go.name = uiObject["name"].ToString();
        go.transform.parent = parent.transform;
        //go.AddComponent<RectTransform>();
        return go;
    }
    static GameObject createP(JToken uiObject, GameObject parent)
    {

        GameObject go = createGo(uiObject, parent);

        return go;
    }

    static GameObject createBg(JToken uiObject, GameObject parent)
    {
        GameObject go = createGo(uiObject, parent);
        go.AddComponent<UISprite>();

        //图片资源
        string imgPath = Regex.Split(path, "Resources/")[1];
        //Sprite sprite = (Sprite)Resources.Load(imgPath + "/" + uiObject["name"], typeof(Sprite));
        //go.GetComponent<UISprite>().atlas = 
        ////图片坐标,大小
        //int[] po = new int[4];
        //int i = 0;
        //foreach (var k in uiObject["bounds"].Children())
        //{
        //    po[i++] = (int)k;
        //}
        //go.GetComponent<RectTransform>().sizeDelta = new Vector2(po[2] - po[0], po[3] - po[1]);
        //go.GetComponent<RectTransform>().pivot = Vector2.zero;
        //go.GetComponent<RectTransform>().position = new Vector3(po[0], po[1], 0);
        return go;
    }

    //public static void SetImage(GameObject item, string altasPath, string spriteName, string imageName, int depth)
    //{
    //    UISprite uiSprite = ComponentHelper.Get<UISprite>(item);
    //    uiSprite.atlas = (AssetDatabase.LoadAssetAtPath(altasPath, typeof(GameObject)) as GameObject).GetComponent<UIAtlas>();
    //    uiSprite.spriteName = spriteName;
    //    uiSprite.depth = depth;
    //    uiSprite.autoResizeBoxCollider = true;
    //    try
    //    {
    //        if (!string.IsNullOrEmpty(imageName))
    //        {
    //            if (imageName.StartsWith("9_"))
    //            {
    //                uiSprite.type = UISprite.Type.Sliced;
    //                return;
    //            }
    //            UISpriteData usd = uiSprite.atlas.GetSprite(spriteName);
    //            if (usd.borderLeft > (int)(usd.width * 0.2))
    //            {
    //                uiSprite.type = UISprite.Type.Sliced;
    //                return;
    //            }
    //        }
    //    }
    //    catch (Exception e)
    //    {
    //
    //    }
    //}
    static GameObject createBtn(JToken uiObject, GameObject parent)
    {
        GameObject go = createGo(uiObject, parent);
        //go.AddComponent<Image>();
        //go.AddComponent<Button>();

        return go;
    }
    static GameObject createBgIdle(JToken uiObject, GameObject parent)
    {
        GameObject go = createGo(uiObject, parent);
        return go;
    }
    static GameObject createBgPressed(JToken uiObject, GameObject parent)
    {
        GameObject go = createGo(uiObject, parent);
        return go;
    }
    static GameObject create9(JToken uiObject, GameObject parent)
    {
        GameObject go = createGo(uiObject, parent);
        return go;
    }
    static GameObject createTxt(JToken uiObject, GameObject parent)
    {
        GameObject go = createGo(uiObject, parent);
        //坐标,大小
        int[] po = new int[4];
        int i = 0;
        foreach (var k in uiObject["bounds"].Children())
        {
            po[i++] = (int)k;
        }
        //go.GetComponent<RectTransform>().position = new Vector3(po[0], po[1], 0);
        //
        //go.AddComponent<Text>();
        //go.GetComponent<Text>().text = uiObject["text"].ToString();//内容
        ////go.GetComponent<Text>().font =                            //字体
        //go.GetComponent<Text>().color = new Color(float.Parse(uiObject["color"]["red"].ToString()) / 255, float.Parse(uiObject["color"]["green"].ToString()) / 255, float.Parse(uiObject["color"]["blue"].ToString()) / 255);
        //go.GetComponent<Text>().fontSize = int.Parse(uiObject["size"].ToString());
        return go;
    }
    static GameObject createPrgh(JToken uiObject, GameObject parent)
    {
        GameObject go = createGo(uiObject, parent);
        return go;
    }
    static GameObject createPrgv(JToken uiObject, GameObject parent)
    {
        GameObject go = createGo(uiObject, parent);
        return go;
    }
    static GameObject createPrgr(JToken uiObject, GameObject parent)
    {
        GameObject go = createGo(uiObject, parent);
        return go;
    }
    static GameObject createList(JToken uiObject, GameObject parent)
    {
        GameObject go = createGo(uiObject, parent);
        return go;
    }

}