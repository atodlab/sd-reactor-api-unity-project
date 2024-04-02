using System;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

using Best.HTTP;
using Best.HTTP.Request.Upload;

public class GameController : MonoBehaviour
{
    // UI Variable
    public GameObject S1;
    public GameObject S2;
    public GameObject S2_P1_Selected;
    public GameObject S2_P2_Selected;
    public GameObject S3_1;
    public GameObject S3_W_Selected;
    public GameObject S3_M_Selected;
    public GameObject S3_2;
    public GameObject S3_L_W_Selected;
    public GameObject S3_L_M_Selected;
    public GameObject S3_R_W_Selected;
    public GameObject S3_R_M_Selected;
    public GameObject S4;
    public GameObject S5;
    public RawImage S5_Bar;
    public GameObject S6;
    public GameObject S7;
    public RawImage[] Rs = new RawImage[4];

    // User Info
    public int numberOfpeople;
    public int gender1; // 0: female, 1: male
    public int gender2; // 0: female, 1: male

    // Stage Variable
    public int stage;
    public TMP_Text count;
    private float timer;

    private float initTimer;

    private const int SHOT_TIME = 10;

    // Api Variable
    private string source;
    private string target;

    private int resultCount;
    private bool isApiUsed;

    private string apiSDUrl = "http://127.0.0.1:7860/reactor/image"; // Stable Diffusion WebUI ReActor API call URL

    public Camera inputCamera; // Webcam for Reactor Input

    private const int ORIGIN_1_PEOPLE_0_NUMBER = 1; // the number of 1 female image
    private const int ORIGIN_1_PEOPLE_1_NUMBER = 1; // the number of 1 male image
    private const int ORIGIN_2_PEOPLE_00_NUMBER = 1; // the number of 2 female image
    private const int ORIGIN_2_PEOPLE_01_NUMBER = 1; // the number of 1 female(left) & 1 male(right) image
    private const int ORIGIN_2_PEOPLE_10_NUMBER = 1; // the number of 1 female(right) & 1 male(left) image
    private const int ORIGIN_2_PEOPLE_11_NUMBER = 1; // the number of 2 male image

    void Start()
    {
        InitValue();
    }

    void Update()
    {
        initTimer += Time.deltaTime;

        if (initTimer > 60)
        {
            Init();
        }

        if (stage == 4)
        {
            timer += Time.deltaTime;

            if (timer >= 3)
            {
                GoS5();
                timer = 0;
            }
        }

        if (stage == 5)
        {
            timer += Time.deltaTime;
            count.text = (SHOT_TIME - (int)timer).ToString();

            if (timer >= SHOT_TIME)
            {
                resultCount = 1;

                CreateImageByCamera(inputCamera, Application.dataPath + "/Input/", "input.png", 900, 900);

                GoS6();
                timer = 0;
            }
        }

        if (stage == 6)
        {
            if (!isApiUsed)
            {
                if (resultCount == 1)
                {
                    GenerateImage();
                    isApiUsed = true;
                }
                else if (resultCount == 2)
                {
                    GenerateImage();
                    isApiUsed = true;
                }
                else if (resultCount == 3)
                {
                    GenerateImage();
                    isApiUsed = true;
                }
                else if (resultCount == 4)
                {
                    GenerateImage();
                    isApiUsed = true;
                }
                else if (resultCount > 4)
                {
                    GoS7();
                    isApiUsed = false;
                }
            }
        }
    }

    public void GoS1()
    {
        S1.SetActive(true);
        S2.SetActive(false);
        S2_P1_Selected.SetActive(false);
        S2_P2_Selected.SetActive(false);
        S3_1.SetActive(false);
        S3_W_Selected.SetActive(false);
        S3_M_Selected.SetActive(false);
        S3_2.SetActive(false);
        S3_L_W_Selected.SetActive(false);
        S3_L_M_Selected.SetActive(false);
        S3_R_W_Selected.SetActive(false);
        S3_R_M_Selected.SetActive(false);
        S4.SetActive(false);
        S5.SetActive(false);
        S5_Bar.gameObject.SetActive(false);
        S6.SetActive(false);
        S7.SetActive(false);

        stage = 1;
        initTimer = 0;
    }

    public void GoS2()
    {
        S1.SetActive(false);
        S2.SetActive(true);
        S3_1.SetActive(false);
        S3_2.SetActive(false);
        S5.SetActive(false);
        S5_Bar.gameObject.SetActive(false);
        S6.SetActive(false);
        S7.SetActive(false);

        stage = 2;
        initTimer = 0;
    }

    public void GoS3()
    {
        if (numberOfpeople != -1)
        {
            if (numberOfpeople == 1)
            {
                GoS3_1();
            }
            else
            {
                GoS3_2();
            }
        }
    }

    public void GoS3_1()
    {
        S1.SetActive(false);
        S2.SetActive(false);
        S3_1.SetActive(true);
        S3_2.SetActive(false);
        S4.SetActive(false);
        S5.SetActive(false);
        S5_Bar.gameObject.SetActive(false);
        S6.SetActive(false);
        S7.SetActive(false);

        stage = 3;
        initTimer = 0;
    }

    public void GoS3_2()
    {
        S1.SetActive(false);
        S2.SetActive(false);
        S3_1.SetActive(false);
        S3_2.SetActive(true);
        S4.SetActive(false);
        S5.SetActive(false);
        S5_Bar.gameObject.SetActive(false);
        S6.SetActive(false);
        S7.SetActive(false);

        stage = 3;
        initTimer = 0;
    }

    public void GoS4()
    {
        if (numberOfpeople == 1)
        {
            if (gender1 != -1)
            {
                S1.SetActive(false);
                S2.SetActive(false);
                S3_1.SetActive(false);
                S3_2.SetActive(false);
                S4.SetActive(true);
                S5.SetActive(false);
                S5_Bar.gameObject.SetActive(false);
                S6.SetActive(false);
                S7.SetActive(false);

                stage = 4;
            }
        }
        else
        {
            if (gender1 != -1 && gender2 != -1)
            {
                S1.SetActive(false);
                S2.SetActive(false);
                S3_1.SetActive(false);
                S3_2.SetActive(false);
                S4.SetActive(true);
                S5.SetActive(false);
                S5_Bar.gameObject.SetActive(false);
                S6.SetActive(false);
                S7.SetActive(false);

                stage = 4;
            }
        }

        initTimer = 0;
    }

    public void GoS5()
    {
        S1.SetActive(false);
        S2.SetActive(false);
        S3_1.SetActive(false);
        S3_2.SetActive(false);
        S4.SetActive(false);
        S5.SetActive(true);

        if (gender2 == -1)
        {
            S5_Bar.gameObject.SetActive(false);
        }
        else
        {
            S5_Bar.gameObject.SetActive(true);
        }

        S6.SetActive(false);
        S7.SetActive(false);

        stage = 5;
        initTimer = 0;
    }

    public void GoS6()
    {
        S1.SetActive(false);
        S2.SetActive(false);
        S3_1.SetActive(false);
        S3_2.SetActive(false);
        S4.SetActive(false);
        S5.SetActive(false);
        S5_Bar.gameObject.SetActive(false);
        S6.SetActive(true);
        S7.SetActive(false);
        
        stage = 6;
        initTimer = 0;
    }

    public void GoS7()
    {
        S1.SetActive(false);
        S2.SetActive(false);
        S3_1.SetActive(false);
        S3_2.SetActive(false);
        S4.SetActive(false);
        S5.SetActive(false);
        S5_Bar.gameObject.SetActive(false);
        S6.SetActive(false);
        S7.SetActive(true);
        
        stage = 7;
        initTimer = 0;
    }

    public void Init()
    {
        GoS1();
        InitValue();
    }

    public void inputNumberOfPeople(int num)
    {
        numberOfpeople = num;

        if (num == 1)
        {
            S2_P1_Selected.SetActive(true);
            S2_P2_Selected.SetActive(false);
        }
        if (num == 2)
        {
            S2_P1_Selected.SetActive(false);
            S2_P2_Selected.SetActive(true);
        }
    }

    public void inputGender1(int gender)
    {
        gender1 = gender;

        if (gender == 0)
        {
            S3_W_Selected.SetActive(true);
            S3_M_Selected.SetActive(false);
            S3_L_W_Selected.SetActive(true);
            S3_L_M_Selected.SetActive(false);
        }

        if (gender == 1)
        {
            S3_W_Selected.SetActive(false);
            S3_M_Selected.SetActive(true);
            S3_L_W_Selected.SetActive(false);
            S3_L_M_Selected.SetActive(true);
        }
    }

    public void inputGender2(int gender)
    {
        gender2 = gender;

        if (gender == 0)
        {
            S3_R_W_Selected.SetActive(true);
            S3_R_M_Selected.SetActive(false);
        }

        if (gender == 1)
        {
            S3_R_W_Selected.SetActive(false);
            S3_R_M_Selected.SetActive(true);
        }
    }

    public void GenerateImage()
    {
        SDData body = new SDData();

        body.source_image = source;

        byte[] target;

        if (gender2 == -1)
        {
            if (gender1 == 0)
            {
                int selectedNum = UnityEngine.Random.Range(1, ORIGIN_1_PEOPLE_0_NUMBER + 1);

                target = Resources.Load<Texture2D>("Profiles/Origin1People/0/" + selectedNum).EncodeToPNG();
            }
            else
            {
                int selectedNum = UnityEngine.Random.Range(1, ORIGIN_1_PEOPLE_1_NUMBER + 1);

                target = Resources.Load<Texture2D>("Profiles/Origin1People/1/" + selectedNum).EncodeToPNG();
            }

            body.source_faces_index = new int[] { 0 };
            body.face_index = new int[] { 0 };
        }
        else
        {
            if (gender1 == 0 && gender2 == 0)
            {
                int selectedNum = UnityEngine.Random.Range(1, ORIGIN_2_PEOPLE_00_NUMBER + 1);

                target = Resources.Load<Texture2D>("Profiles/Origin2People/00/" + selectedNum).EncodeToPNG();
            }
            else if (gender1 == 0 && gender2 == 1)
            {
                int selectedNum = UnityEngine.Random.Range(1, ORIGIN_2_PEOPLE_01_NUMBER + 1);

                target = Resources.Load<Texture2D>("Profiles/Origin2People/01/" + selectedNum).EncodeToPNG();
            }
            else if (gender1 == 1 && gender2 == 0)
            {
                int selectedNum = UnityEngine.Random.Range(1, ORIGIN_2_PEOPLE_10_NUMBER + 1);

                target = Resources.Load<Texture2D>("Profiles/Origin2People/10/" + selectedNum).EncodeToPNG();
            }
            else
            {
                int selectedNum = UnityEngine.Random.Range(1, ORIGIN_2_PEOPLE_11_NUMBER + 1);

                target = Resources.Load<Texture2D>("Profiles/Origin2People/11/" + selectedNum).EncodeToPNG();
            }

            body.source_faces_index = new int[] { 0, 1 };
            body.face_index = new int[] { 0, 1 };
        }

        body.target_image = "data:image/png;base64," + Convert.ToBase64String(target);
        body.upscale_visibility = 1;
        body.face_restorer = "CodeFormer";
        body.codeformer_weight = 0.5f;
        body.restorer_visibility = 1;
        body.restore_first = 1;
        body.model = "inswapper_128.onnx";
        body.gender_source = 0;
        body.gender_target = 0;
        body.save_to_file = 0;

        var request = HTTPRequest.CreatePost(apiSDUrl, SDRequestCallback);

        request.SetHeader("accept", "application/json");
        request.SetHeader("Content-Type", "application/json");

        request.UploadSettings.UploadStream = new JSonDataStream<SDData>(body);

        request.Send();
    }


    private void SDRequestCallback(HTTPRequest req, HTTPResponse resp)
    {
        switch (req.State)
        {
            case HTTPRequestStates.Finished:
                if (resp.IsSuccess)
                {
                    SDImageResult result = JsonUtility.FromJson<SDImageResult>(resp.DataAsText);

                    Debug.Log("Image translated successfully: " + resultCount);

                    byte[] imageBytes = Convert.FromBase64String(result.image);

                    Texture2D texture = new Texture2D(2, 2);

                    if (texture.LoadImage(imageBytes))
                    {
                        Rs[resultCount - 1].texture = texture;
                    }

                    resultCount += 1;
                    isApiUsed = false;
                }
                else
                {
                    Debug.Log($"Server sent an error: {resp.StatusCode}-{resp.Message}");

                    SceneManager.LoadScene(0);
                }
                break;

            default:
                Debug.LogError($"Request finished with error! Request state: {req.State}");
                SceneManager.LoadScene(0);
                break;
        }
    }

    public void CreateImageByCamera(Camera camera, string path, string name, int resWidth, int resHeight)
    {
        DirectoryInfo dir = new DirectoryInfo(path);
        if (!dir.Exists)
        {
            System.IO.Directory.CreateDirectory(path);
        }

        RenderTexture rt = new RenderTexture(resWidth, resHeight, 24);
        camera.targetTexture = rt;
        Texture2D screenShot = new Texture2D(resWidth, resHeight, TextureFormat.RGB24, false);
        Rect rec = new Rect(0, 0, screenShot.width, screenShot.height);
        camera.Render();
        RenderTexture.active = rt;
        screenShot.ReadPixels(new Rect(0, 0, resWidth, resHeight), 0, 0);
        screenShot.Apply();

        byte[] bytes = screenShot.EncodeToPNG();

        System.IO.File.WriteAllBytes(path + name, bytes);

        source = "data:image/png;base64," + Convert.ToBase64String(bytes);
    }

    private void InitValue()
    {
        numberOfpeople = -1;
        gender1 = -1;
        gender2 = -1;
        stage = 1;
        timer = 0;
        initTimer = 0;

        resultCount = 0;
        isApiUsed = false;
    }
}

// Reactor Input Class
public class SDData
{
    public string source_image;
    public string target_image;
    public int[] source_faces_index;
    public int[] face_index;
    public int upscale_visibility;
    public string face_restorer;
    public float codeformer_weight;
    public int restorer_visibility;
    public int restore_first;
    public string model;
    public int gender_source;
    public int gender_target;
    public int save_to_file;
    public float scale;
}

// Reactor Result Class
public class SDImageResult
{
    public string image;
}
