//-----------------------------------------------------------------------
// <copyright file="HelloARController.cs" company="Google">
//
// Copyright 2017 Google Inc. All Rights Reserved.
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
// http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
//
// </copyright>
//-----------------------------------------------------------------------

namespace GoogleARCore.Examples.Common
{
    using System.Collections.Generic;
    using System.Collections;
    using GoogleARCore;
    using GoogleARCore.Examples.Common;
    using UnityEngine;
    using UnityEngine.UI;
    using UnityEngine.EventSystems;

#if UNITY_EDITOR
    // Set up touch input propagation while using Instant Preview in the editor.
    using Input = InstantPreviewInput;
#endif

    /// <summary>
    /// Controls the HelloAR example.
    /// </summary>
    public class HelloARController : MonoBehaviour
    {
        /// <summary>
        /// The first-person camera being used to render the passthrough camera image (i.e. AR
        /// background).
        /// </summary>
        public Camera FirstPersonCamera;


        /// <summary>
        /// A prefab for tracking and visualizing detected planes.
        /// </summary>
        public GameObject DetectedPlanePrefab;

        /// <summary>
        /// A model to place when a raycast from a user touch hits a plane.
        /// </summary>
        public GameObject AndyPlanePrefab;


        public Button button;

        GameObject[] objs = new GameObject[1];

        Anchor[] an = new Anchor[1];
        /// <summary>
        /// The rotation in degrees need to apply to model when the Andy model is placed.
        /// </summary>
        private const float k_ModelRotation = 180.0f;

        /// <summary>
        /// True if the app is in the process of quitting due to an ARCore connection error,
        /// otherwise false.
        /// </summary>
        private bool m_IsQuitting = false;

        public static bool recalib = true;

        public static int score = 0;

        private bool posession = false;
        private bool active = false;


        public GameObject ball;
        public GameObject player;

        float touchTimeStart, touchTimeFinish, timeInterval;

        Vector2 startPos, endPos, direction;

        float throwForceInXandY = 50f;

        float throwForceInZ = 50f;

        /// <summary>
        /// The Unity Update() method.
        /// </summary>
        /// 

        public void Start()
        {
            button.gameObject.SetActive(false);
            Physics.gravity = Physics.gravity * .1f;
        }

        public void Update()
        {
            if (recalib)
            {
                button.gameObject.SetActive(false);
                Shot.pause();
                Destroy(objs[0]);
                Destroy(an[0]);
                ScoringScript.msg.gameObject.SetActive(false);
                DirectionsScript.msg.gameObject.SetActive(false);
                DetectedPlaneVisualizer.switchTrue();
                setSpawn();
            } else
            {
                Shot.setPos(objs[0].transform.position);
                DetectedPlaneVisualizer.switchFalse();
                ScoringScript.msg.gameObject.SetActive(true);
                Shot.play();
            }
            
        }

        private void setSpawn()
        {
            // If the player has not touched the screen, we are done with this update.
            Touch touch;
            if (Input.touchCount < 1 || (touch = Input.GetTouch(0)).phase != TouchPhase.Began)
            {
                return;
            }

            // Should not handle input if the player is pointing on UI.
            if (EventSystem.current.IsPointerOverGameObject(touch.fingerId))
            {
                return;
            }

            // Raycast against the location the player touched to search for planes.
            TrackableHit hit;
            TrackableHitFlags raycastFilter = TrackableHitFlags.PlaneWithinPolygon |
                TrackableHitFlags.FeaturePointWithSurfaceNormal;

            if (Frame.Raycast(touch.position.x, touch.position.y, raycastFilter, out hit))
            {
                // Use hit pose and camera pose to check if hittest is from the
                // back of the plane, if it is, no need to create the anchor.
                if ((hit.Trackable is DetectedPlane) &&
                    Vector3.Dot(FirstPersonCamera.transform.position - hit.Pose.position,
                        hit.Pose.rotation * Vector3.up) < 0)
                {
                    Debug.Log("Hit at back of the current DetectedPlane");
                }
                else
                {
                    // Choose the Andy model for the Trackable that got hit.
                    GameObject prefab;

                    prefab = AndyPlanePrefab;

                    // Instantiate Andy model at the hit pose.
                    objs[0] = ((GameObject)Instantiate(prefab, hit.Pose.position, hit.Pose.rotation));
                    GameObject obj = objs[0];


                    // Compensate for the hitPose rotation facing away from the raycast (i.e.
                    // camera).
                    obj.transform.Rotate(0, k_ModelRotation, 0, Space.Self);

                    obj.transform.Rotate(0, -90, 0);
                    obj.transform.position += new Vector3(.5f, -.5f, .5f);

                    // Create an anchor to allow ARCore to track the hitpoint as understanding of
                    // the physical world evolves.
                    an[0] = hit.Trackable.CreateAnchor(hit.Pose);

                    // Make Andy model a child of the anchor.
                    obj.transform.parent = an[0].transform;
                    recalib = false;
                    DetectedPlaneVisualizer.switchFalse();
                    DirectionsScript.msg.gameObject.SetActive(true);
                    button.gameObject.SetActive(true);
                }
            }

        }

        public void setRecalib()
        {
            Debug.Log("BUTTON CLICKED");
            recalib = !recalib;
        }
    }
}