using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private Camera mainCam;
    public Map map;
    public List<WorldBlock> path = null;
    public bool clickEnable = true;
    public bool moving = false;
    public Vector3 destination;
    [SerializeField] float speed = 10f;

    private void Start() {
        mainCam = FindObjectOfType<Camera>();
        map = FindObjectOfType<Map>();
        path = new List<WorldBlock>();
    }
    private void Update() {
        if(Input.GetMouseButtonDown(0) && clickEnable)
        {
            clickEnable = false;
            Debug.Log("MouseClicked");
            RaycastHit hit;
            Ray ray = mainCam.ScreenPointToRay(Input.mousePosition);
            
            if(Physics.Raycast(ray, out hit, Mathf.Infinity))
            {
                Debug.Log("It hit a Block");
                WorldBlock hitBlock = hit.transform.GetComponent<WorldBlock>();
                if(hitBlock == null || !hitBlock.canYouPass) return;
                
                path = map.PathFinding(map.GetBlock(transform.position), hitBlock);
                if(path == null) return;
            }
        }
        if(path?.Count > 0 || (map.pathFindSucceed && map.visualEffect))
        {
            if(moving)
                KeepMove();
            else
                GetNextDest();
        }
        else{
            if(!clickEnable) clickEnable = true;
        }
    }

    private void GetNextDest()
    {
        destination = path[0].transform.position;
        moving = true;
    }

    private void KeepMove()
    {
        if(Vector3.Distance(destination, transform.position) <= Mathf.Epsilon) { moving = false; path.RemoveAt(0); return;}

        Vector3 dir = destination - transform.position;
        //dir.Normalize();
        Vector3 rawDir = dir * Time.deltaTime * speed;
        transform.Translate(dir, Space.World);
    }

    public void GetPathFromMap()
    {
        path = map.completedPath;
    }
}
