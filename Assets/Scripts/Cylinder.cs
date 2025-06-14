using System.Collections;
using UnityEngine;

// 실린더 Rod를 minRange만큼 후진, maxRange만큼 전진
// 속성: 실린더 Rod의 transform, minRange Pos, maxRange Pos, 속도
public class Cylinder : MonoBehaviour
{
    public enum SolenoidType
    {
        단방향솔레노이드,
        양방향솔레노이드
    }
    SolenoidType type = SolenoidType.양방향솔레노이드;

    public Transform cylinderRod;
    public Renderer frontLimitSW;
    public Renderer backLimitSW;
    public Color originSWColor;
    public float speed; // 공압밸브 조절
    public float minPosY;
    public float maxPosY;
    public bool isForward; // 전방 솔레노이드 신호
    public bool isBackward; // 후방 솔레노이드 신호

    private void Start()
    {
        originSWColor = frontLimitSW.material.color;
        backLimitSW.material.SetColor("_BaseColor", Color.green);
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.RightArrow))
        {
            Vector3 back = new Vector3(0, minPosY, 0);
            Vector3 front = new Vector3(0, maxPosY, 0);
            StartCoroutine(MoveCylinder(back, front));

            ChangeSWColor(backLimitSW, originSWColor);
        }
        else if(Input.GetKeyDown(KeyCode.LeftArrow))
        {
            Vector3 back = new Vector3(0, minPosY, 0);
            Vector3 front = new Vector3(0, maxPosY, 0);
            StartCoroutine(MoveCylinder(front, back));

            ChangeSWColor(frontLimitSW, originSWColor);
        }
    }

    IEnumerator MoveCylinder(Vector3 from, Vector3 to)
    {
        Vector3 direction = Vector3.one;

        while (true)
        {
            direction = to - cylinderRod.localPosition;

            Vector3 normalizedDir = direction.normalized;
            float distance = direction.magnitude;

            if(distance < 0.1f)
            {
                if (isForward) 
                    ChangeSWColor(frontLimitSW, Color.green);
                else if(isBackward)
                    ChangeSWColor(backLimitSW, Color.green);

                break;
            }

            cylinderRod.localPosition += normalizedDir * speed * Time.deltaTime;

            yield return new WaitForEndOfFrame();
        }   
    }

    public void ChangeSWColor(Renderer sw, Color color)
    {
        sw.material.SetColor("_BaseColor", color);
    }
}
