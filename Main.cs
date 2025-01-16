using System;
using System.Collections.Generic;
using BepInEx;
using UnityEngine;
using UnityEngine.UI;
using Utilla;

namespace MonkeModMenuReborn
{
    [BepInPlugin(PluginInfo.GUID, PluginInfo.Name, PluginInfo.Version)]
    public class Main : BaseUnityPlugin
    {
        public static Main Instance;

        public void Awake()
        {
            Instance = this;
        }
        public bool InModded()
        {
            if (NetworkSystem.Instance.GameModeString.Contains("MODDED") && NetworkSystem.Instance.InRoom)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool SuperMonke;
        public bool Longarms;
        public bool Speed;
        public bool Beacons;
        public bool Checkpoint;
        public bool Plats;
        public bool Handballs;
        public bool TPGun;
        private bool noGrav;
        private bool gravDebounce;


        private GameObject checkObj;
        private GameObject l;
        private GameObject r;
        public GameObject pointer;
        public void Mods()
        {
            if (TPGun)
            {
                if (ControllerInputPoller.instance.rightGrab)
                {

                    Physics.Raycast(GorillaLocomotion.Player.Instance.rightControllerTransform.position, -GorillaLocomotion.Player.Instance.rightControllerTransform.up, out var hitInfo);
                    pointer = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                    pointer.transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);
                    pointer.GetComponent<Renderer>().material.color = new Color32(255, 255, 255, 0);
                    pointer.transform.position = hitInfo.point;
                    GameObject.Destroy(pointer.GetComponent<BoxCollider>());
                    GameObject.Destroy(pointer.GetComponent<Rigidbody>());
                    GameObject.Destroy(pointer.GetComponent<Collider>());
                    if (ControllerInputPoller.instance.rightControllerIndexFloat >= 0.1)
                    {
                        GameObject.Destroy(pointer, Time.deltaTime);
                        GorillaLocomotion.Player.Instance.transform.position = pointer.transform.position;
                        GorillaLocomotion.Player.Instance.GetComponent<Rigidbody>().velocity = pointer.transform.position;
                    }

                    if (pointer != null)
                    {
                        GameObject.Destroy(pointer, Time.deltaTime);
                    }

                }
            }

            if (Handballs)
            {
                
                GameObject LeftBall = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                LeftBall.name = "LeftBall";
                LeftBall.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
                LeftBall.GetComponent<Renderer>().material.color = GorillaTagger.Instance.offlineVRRig.playerColor;
                LeftBall.GetComponent<Renderer>().material.shader = Shader.Find("GUI/Text Shader");
                LeftBall.GetComponent<Collider>().enabled = false;
                LeftBall.transform.position = GorillaLocomotion.Player.Instance.leftControllerTransform.position;
                GameObject RightBall = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                RightBall.name = "RightBall";
                RightBall.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
                RightBall.GetComponent<Renderer>().material.color = GorillaTagger.Instance.offlineVRRig.playerColor;
                RightBall.GetComponent<Renderer>().material.shader = Shader.Find("GUI/Text Shader");
                RightBall.GetComponent<Collider>().enabled = false;
                RightBall.transform.position = GorillaLocomotion.Player.Instance.rightControllerTransform.position;

                GameObject.Destroy(LeftBall, Time.deltaTime);
                GameObject.Destroy(RightBall, Time.deltaTime);
            }
            
            if (Plats)
            {
                if (ControllerInputPoller.instance.leftControllerIndexFloat > 0.5f)
                {
                    if (l == null)
                    {
                        l = GameObject.CreatePrimitive(PrimitiveType.Cube);
                        l.transform.localScale = new Vector3(0.025f, 0.3f, 0.4f);
                        l.transform.position = GorillaTagger.Instance.leftHandTransform.position;
                        l.transform.rotation = GorillaTagger.Instance.leftHandTransform.rotation;
                        l.GetComponent<Renderer>().material.shader = Shader.Find("GorillaTag/UberShader");
                        l.GetComponent<Renderer>().material.color = Color.black;
                    }
                }
                else
                {
                    Destroy(l);
                }
                
                
                if (ControllerInputPoller.instance.rightControllerIndexFloat > 0.5f)
                {
                    if (r == null)
                    {
                        r = GameObject.CreatePrimitive(PrimitiveType.Cube);                        
                        r.transform.localScale = new Vector3(0.025f, 0.3f, 0.4f);
                        r.transform.position = GorillaTagger.Instance.rightHandTransform.position;
                        r.transform.rotation = GorillaTagger.Instance.rightHandTransform.rotation;
                        
                        r.GetComponent<Renderer>().material.shader = Shader.Find("GorillaTag/UberShader");
                        r.GetComponent<Renderer>().material.color = Color.black;
                    }
                }
                else
                {
                    Destroy(r);
                }
            }
            else
            {
                Destroy(l);
                Destroy(r);
            }
            
            if (SuperMonke)
            {
                if (ControllerInputPoller.instance.rightControllerPrimaryButton)
                {
                    GorillaLocomotion.Player.Instance.transform.position += (GorillaLocomotion.Player.Instance.headCollider.transform.forward * Time.deltaTime) * 12f;
                    GorillaLocomotion.Player.Instance.GetComponent<Rigidbody>().velocity = Vector3.zero;
                }

                if (ControllerInputPoller.instance.rightControllerSecondaryButton)
                {
                    if (gravDebounce)
                    {
                        return;
                    }
                    else
                    {
                        gravDebounce = true;
                        noGrav = !noGrav;
                    }
                }
                else
                {
                    gravDebounce = false;
                }

                if (noGrav)
                {
                    Physics.gravity = new Vector3(0, 0, 0);
                }
                else
                {
                    Physics.gravity = new Vector3(0, -9.8f, 0);
                }
            }
            else
            {
                Physics.gravity = new Vector3(0, -9.8f, 0);
            }

            if (Longarms)
            {
                GorillaLocomotion.Player.Instance.transform.localScale = new Vector3(1.25f, 1.25f, 1.25f);
            }
            else
            {
                GorillaLocomotion.Player.Instance.transform.localScale = new Vector3(1f, 1f, 1f);
            }
            
            
            if (Speed)
            {
                GorillaLocomotion.Player.Instance.jumpMultiplier = 1.3f;
                GorillaLocomotion.Player.Instance.maxJumpSpeed = 8.5f;
            }
            else
            {
                GorillaLocomotion.Player.Instance.jumpMultiplier = 1.1f;
                GorillaLocomotion.Player.Instance.maxJumpSpeed = 6.5f;
            }

            if (Beacons)
            {
                foreach (VRRig rig in GorillaParent.instance.vrrigs)
                {
                    if (!rig.isOfflineVRRig && !rig.isMyPlayer && !rig.isLocal)
                    {
                        GameObject beacon = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
                        GameObject.Destroy(beacon.GetComponent<BoxCollider>());
                        GameObject.Destroy(beacon.GetComponent<Rigidbody>());
                        GameObject.Destroy(beacon.GetComponent<Collider>());
                        beacon.transform.rotation = Quaternion.identity;
                        beacon.transform.localScale = new Vector3(0.04f, 200f, 0.04f);
                        beacon.transform.position = rig.transform.position;
                        beacon.GetComponent<MeshRenderer>().material = rig.mainSkin.material;
                        beacon.GetComponent<Renderer>().material.shader = Shader.Find("GorillaTag/UberShader");
                        GameObject.Destroy(beacon, Time.deltaTime);
                    }
                }
            }

            if (Checkpoint)
            {
                if (checkObj == null)
                {
                    checkObj = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                    GameObject.Destroy(checkObj.GetComponent<SphereCollider>());
                    GameObject.Destroy(checkObj.GetComponent<Rigidbody>());
                    checkObj.transform.localScale = new Vector3(0.4f, 0.4f, 0.4f);
                    checkObj.GetComponent<Renderer>().material.shader = Shader.Find("GorillaTag/UberShader");
                    checkObj.GetComponent<Renderer>().material.color = Color.yellow;
                }

                if (ControllerInputPoller.instance.rightGrab)
                {
                    checkObj.transform.position = GorillaLocomotion.Player.Instance.rightControllerTransform.position;
                }

                if (ControllerInputPoller.instance.rightControllerIndexFloat > 0.5f)
                {
                    GorillaLocomotion.Player.Instance.transform.position = checkObj.transform.position;
                }
            }
            else
            {
                Destroy(checkObj);
            }
        }

        public void Update()
        {
            if (ControllerInputPoller.instance.leftGrab && InModded())
            {
                if (menu == null)
                {
                    Draw();
                }

                if (reference == null)
                {
                    CreateReference();
                }
                
                menu.transform.position = GorillaTagger.Instance.leftHandTransform.position;
                menu.transform.rotation = GorillaTagger.Instance.leftHandTransform.rotation;
            }
            else
            {
                Destroy(reference);
                Destroy(menu);
            }

            cooldown -= 1;
            
            Mods();
        }

        private GameObject menu;
        private GameObject menuBackground;
        private GameObject canvasObject;

        public GameObject reference;
        
        

        public void CreateReference()
        {
            reference = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            reference.transform.localScale = new Vector3(0.01f, 0.01f, 0.01f);
            reference.name = "Reference";
            reference.transform.parent = GorillaTagger.Instance.rightHandTransform;
            reference.transform.localPosition = new Vector3(0f, -0.15f, 0f);
            reference.GetComponent<Collider>().isTrigger = true;
            reference.GetComponent<Renderer>().material.shader = Shader.Find("GorillaTag/UberShader");
            reference.GetComponent<Renderer>().material.color = Color.grey;
        }

        public void CreateButton(float offset, string buttonText)
        {
            GameObject button = GameObject.CreatePrimitive(PrimitiveType.Cube);
            button.layer = 2;
            Destroy(button.GetComponent<Rigidbody>());
            button.GetComponent<BoxCollider>().isTrigger = true;
            button.transform.parent = menu.transform;
            button.transform.rotation = Quaternion.identity;
            button.transform.localScale = new Vector3(0.09f, 0.9f, 0.08f);
            button.transform.localPosition = new Vector3(0.56f, 0f, 0.28f - offset);
            button.GetComponent<Renderer>().material.shader = Shader.Find("GorillaTag/UberShader");

            BtnCollider btnCollider = button.AddComponent<BtnCollider>();
            btnCollider.relatedText = buttonText;

            Text text = new GameObject
            {
                transform =
                {
                    parent = canvasObject.transform
                }
            }.AddComponent<Text>();
            text.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
            text.text = buttonText;
            text.supportRichText = true;
            text.fontSize = 1;
            text.alignment = TextAnchor.MiddleCenter;
            text.fontStyle = FontStyle.Italic;
            text.resizeTextForBestFit = true;
            text.resizeTextMinSize = 0;
            RectTransform component = text.GetComponent<RectTransform>();
            component.localPosition = Vector3.zero;
            component.sizeDelta = new Vector2(.2f, .03f);
            component.localPosition = new Vector3(.064f, 0, .111f - offset / 2.6f);
            component.rotation = Quaternion.Euler(new Vector3(180f, 90f, 90f));
        }


        public void Draw()
        {
                        // Menu Holder
                menu = GameObject.CreatePrimitive(PrimitiveType.Cube);
                UnityEngine.Object.Destroy(menu.GetComponent<Rigidbody>());
                UnityEngine.Object.Destroy(menu.GetComponent<BoxCollider>());
                UnityEngine.Object.Destroy(menu.GetComponent<Renderer>());
                menu.transform.localScale = new Vector3(0.1f, 0.3f, 0.3825f);
                menu.GetComponent<Renderer>().material.shader = Shader.Find("GorillaTag/UberShader");

            // Menu Background
                menuBackground = GameObject.CreatePrimitive(PrimitiveType.Cube);
                UnityEngine.Object.Destroy(menuBackground.GetComponent<Rigidbody>());
                UnityEngine.Object.Destroy(menuBackground.GetComponent<BoxCollider>());
                menuBackground.transform.parent = menu.transform;
                menuBackground.transform.rotation = Quaternion.identity;
                menuBackground.transform.localScale = new Vector3(0.03f, 1f, 1f);
                menuBackground.GetComponent<Renderer>().material.shader = Shader.Find("GorillaTag/UberShader");
                menuBackground.GetComponent<Renderer>().material.color = Color.black;
                menuBackground.transform.position = new Vector3(0.05f, 0f, 0f);

                canvasObject = new GameObject();
                canvasObject.transform.parent = menu.transform;
                Canvas canvas = canvasObject.AddComponent<Canvas>();
                CanvasScaler canvasScaler = canvasObject.AddComponent<CanvasScaler>();
                canvasObject.AddComponent<GraphicRaycaster>();
                canvas.renderMode = RenderMode.WorldSpace;
                canvasScaler.dynamicPixelsPerUnit = 1000f;

                Text text = new GameObject
                {
                    transform =
                    {
                        parent = canvasObject.transform
                    }
                }.AddComponent<Text>();
                text.font = Resources.GetBuiltinResource<Font>("Arial.ttf") as Font;
                text.text = "Monke Mod Menu Reborn";
                text.fontSize = 1;
                text.color = Color.white;
                text.supportRichText = true;
                text.fontStyle = FontStyle.Italic;
                text.alignment = TextAnchor.MiddleCenter;
                text.resizeTextForBestFit = true;
                text.resizeTextMinSize = 0;
                RectTransform component = text.GetComponent<RectTransform>();
                component.localPosition = Vector3.zero;
                component.sizeDelta = new Vector2(0.28f, 0.05f);
                component.position = new Vector3(0.06f, 0f, 0.165f);
                component.rotation = Quaternion.Euler(new Vector3(180f, 90f, 90f));

                CreateButton(0f, "Toggle Super Monke");
                CreateButton(0.1f, "Toggle Longarm Monke");
                CreateButton(0.2f, "Toggle Speed Boost");
                CreateButton(0.3f, "Toggle Platform Monke");
                CreateButton(0.4f, "Toggle Beacons");
                CreateButton(0.5f, "Toggle Checkpoint Monke");
                CreateButton(0.6f, "Toggle HandBalls");
                CreateButton(0.7f, "Toggle TP Gun");
        }

        public void Toggle(string relatedText)
        {
            Debug.Log(relatedText + "Toggled");

            switch (relatedText)
            {
                case "Toggle Super Monke":
                    SuperMonke = !SuperMonke;
                    break;
                case "Toggle Longarm Monke":
                    Longarms = !Longarms;
                    break;
                case "Toggle Speed Boost":
                    Speed = !Speed;
                    break;
                case "Toggle Beacons":
                    Beacons = !Beacons;
                    break;
                case "Toggle Checkpoint Monke":
                    Checkpoint = !Checkpoint;
                    break;
                case "Toggle Platform Monke":
                    Plats = !Plats;
                    break;
                case "Toggle HandBalls":
                    Handballs = !Handballs;
                    break;
                case "Toggle TP Gun":
                    TPGun = !TPGun;
                    break;
            }
        }

        public int cooldown;
    }

    public class BtnCollider : MonoBehaviour
    {
        public string relatedText;
        public bool isToggled;
        public void OnTriggerEnter(Collider other)
        {
            if (other.gameObject == Main.Instance.reference)
            {
                if (Main.Instance.cooldown < 0)
                {
                    isToggled = !isToggled;     
                    Main.Instance.Toggle(relatedText);
                    
                    Main.Instance.cooldown = 10;
                }
            }
        }

        public void Update()
        {
            if (isToggled)
            {
                gameObject.GetComponent<Renderer>().material.color = Color.green;
            }
            else
            {
                gameObject.GetComponent<Renderer>().material.color = Color.red;
            }
            
            // terrible code incoming

            switch (relatedText)
            {
                case "Toggle Super Monke":
                    gameObject.GetComponent<Renderer>().material.color = Main.Instance.SuperMonke ? Color.green : Color.red;
                    isToggled = Main.Instance.SuperMonke; 
                    break;
                case "Toggle Longarm Monke":
                    gameObject.GetComponent<Renderer>().material.color = Main.Instance.Longarms ? Color.green : Color.red;
                    isToggled = Main.Instance.Longarms; 
                    break;
                case "Toggle Speed Boost":
                    gameObject.GetComponent<Renderer>().material.color = Main.Instance.Speed ? Color.green : Color.red;
                    isToggled = Main.Instance.Speed; 
                    break;
                case "Toggle Beacons":
                    gameObject.GetComponent<Renderer>().material.color = Main.Instance.Beacons ? Color.green : Color.red;
                    isToggled = Main.Instance.Beacons; 
                    break;
                case "Toggle Checkpoint Monke":
                    gameObject.GetComponent<Renderer>().material.color = Main.Instance.Checkpoint ? Color.green : Color.red;
                    isToggled = Main.Instance.Checkpoint; 
                    break;
                case "Toggle Platform Monke":
                    gameObject.GetComponent<Renderer>().material.color = Main.Instance.Plats ? Color.green : Color.red;
                    isToggled = Main.Instance.Plats; 
                    break;
                case "Toggle HandBalls":
                    gameObject.GetComponent<Renderer>().material.color = Main.Instance.Handballs ? Color.green : Color.red;
                    isToggled = Main.Instance.Handballs; 
                    break;
                case "Toggle TP Gun":
                    gameObject.GetComponent<Renderer>().material.color = Main.Instance.TPGun ? Color.green : Color.red;
                    isToggled = Main.Instance.TPGun; 
                    break;
            }
        }
    }
}