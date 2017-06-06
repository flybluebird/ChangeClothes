using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Net;
using UnityEngine;

public class ChangeClothesSystem : MonoBehaviour
{

    private Transform source;
    private Transform target;

    private GameObject tempSource;//用来实例化原来的模型
    private GameObject tempTarget;//用来实例话模型骨骼，其中包含了各个节点的位置

    private Dictionary<string,Dictionary<string,Transform>> data=new Dictionary<string, Dictionary<string, Transform>>();//存储原模型中各个蒙皮的具体信息，包含名字和位置
    private Transform[] hips;//存储骨架的位置信息

    private Dictionary<string,SkinnedMeshRenderer> targetSmr=new Dictionary<string, SkinnedMeshRenderer>();//存储已经存在的蒙皮及其名字

    private const float fadeLength = .8f;

    private Animation mAnim;
    private AnimationClip clip;

    private int count = 0;

	// Use this for initialization
	void Start ()
    {
		InstantiateAvatar();
        LoadAvatarData(source);
        LoadSkeleton();

        hips = target.GetComponentsInChildren<Transform>();
        mAnim = target.GetComponentInChildren<Animation>();

        InitAvatar();
    }
	
    //load source model
    void InstantiateAvatar()
    {
        tempSource = Instantiate(Resources.Load("FemaleAvatar")) as GameObject;
        source = tempSource.transform;
        tempSource.SetActive(false);
    }

    //load skinnedmeshrenderer form source
    void LoadAvatarData(Transform source)
    {
        if (source == null)
            return;

        SkinnedMeshRenderer[] parts = source.GetComponentsInChildren<SkinnedMeshRenderer>(true);

        foreach (SkinnedMeshRenderer part in parts)
        {
            string[] partName = part.name.Split('-');
            if (!data.ContainsKey(partName[0]))
            {
                data.Add(partName[0],new Dictionary<string, Transform>());
                GameObject partObj=new GameObject();
                partObj.name = partName[0];
                partObj.transform.parent = target;

                targetSmr.Add(partName[0],partObj.AddComponent<SkinnedMeshRenderer>());
            }

            data[partName[0]].Add(partName[1],part.transform);
        }
    }

    //load Skeleton
    void LoadSkeleton()
    {
        tempTarget = Instantiate(Resources.Load("targetmodel")) as GameObject;
        target = tempTarget.transform;
    }

    //avatar change mesh
    void ChangeMesh(string part,string item)
    {
        SkinnedMeshRenderer smr = data[part][item].GetComponent<SkinnedMeshRenderer>();//得到指定的蒙皮
        List<Transform> bones=new List<Transform>();
        foreach (Transform bone in smr.bones)
        {
            foreach (Transform hip in hips)
            {
                if (hip.name != bone.name)
                    continue;
                bones.Add(hip);
                break;
            }
        }

        targetSmr[part].sharedMesh = smr.sharedMesh;
        targetSmr[part].bones = bones.ToArray();
        targetSmr[part].materials = smr.materials;
    }

    //init avatar resource
    void InitAvatar()
    {
        ChangeMesh("coat","003");
        ChangeMesh("head","003");
        ChangeMesh("foot","003");
        ChangeMesh("hand","003");
        ChangeMesh("pant","003");
    }

    void OnGUI()
    {
        if (GUILayout.Button("Head"))
        {
            if (count == 1)
            {
                ChangeMesh("head", "003");
                count = 0;
            }
            else
            {
                ChangeMesh("head","001");
                count = 1;
            }
        }

        if (GUILayout.Button("Hand"))
        {
            if (count == 1)
            {
                ChangeMesh("hand", "003");
                count = 0;
            }
            else
            {
                ChangeMesh("hand","001");
                count = 1;
            }
        }

        if (GUILayout.Button("Pant"))
        {
            if (count == 1)
            {
                ChangeMesh("pant", "003");
                count = 0;
            }
            else
            {
                ChangeMesh("pant", "001");
                count = 1;
            }
        }

        if (GUILayout.Button("Foot"))
        {
            if (count == 1)
            {
                ChangeMesh("foot", "003");
                count = 0;
            }
            else
            {
                ChangeMesh("foot", "001");
                count = 1;
            }
        }

        if (GUILayout.Button("Hair"))
        {
            if (count == 1)
            {
                ChangeMesh("hair", "003");
                count = 0;
            }
            else
            {
                ChangeMesh("hair", "001");
                count = 1;
            }
        }
    }

    // Update is called once per frame
	void Update () {
		
	}
}
