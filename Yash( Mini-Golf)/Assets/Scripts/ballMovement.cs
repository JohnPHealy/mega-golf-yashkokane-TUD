﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ballMovement : MonoBehaviour
{
    [SerializeField] private float shotPower, maxForce, minSpeed;
    [SerializeField] private LineRenderer LR;
    [SerializeField] private UnityEvent<string> shotTaken;

    private Rigidbody RB;
    private float shotForce;
    private Vector3 startPos, endPos, direction;
    private bool canShoot, shotStarted;
    private int strokes;


    private void Start()
    {
        RB = GetComponent<Rigidbody>();
        canShoot = true;
        RB.sleepThreshold = minSpeed;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && canShoot)
        {
            startPos = MousePositionInWorld();
            shotStarted = true;
            LR.gameObject.SetActive(true);
            LR.SetPosition(0, LR.transform.localPosition);
        }

        if (Input.GetMouseButton(0) && shotStarted)
        {
            endPos = MousePositionInWorld();
            shotForce = Mathf.Clamp(Vector3.Distance(endPos, startPos), 0, maxForce);
            LR.SetPosition(1, transform.InverseTransformPoint(endPos));
        }

        if (Input.GetMouseButtonUp(0) && shotStarted)
        {
            canShoot = false;
            shotStarted = false;
            LR.gameObject.SetActive(false);
            strokes++;
            shotTaken.Invoke(strokes.ToString());
        }
    }

    private void FixedUpdate()
    {
        if (!canShoot)
        {
            direction = startPos - endPos;
            RB.AddForce(Vector3.Normalize(direction) * shotForce * shotPower, ForceMode.Impulse);
            startPos = endPos = Vector3.zero;
        }

        if (RB.velocity.magnitude <= minSpeed)
        {
            canShoot = true;
        }
    }

    private Vector3 MousePositionInWorld()
    {
        Vector3 position = Vector3.zero;
        var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit = new RaycastHit();

        if (Physics.Raycast(ray, out hit))
        {
            position = hit.point;
        }

        return position;
    }
}
