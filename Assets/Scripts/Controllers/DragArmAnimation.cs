using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.XR;

public class DragArmAnimation : MonoBehaviour
{   
    static readonly Dictionary<string, InputFeatureUsage<bool>> availableBoolButtons = new Dictionary<string, InputFeatureUsage<bool>>
    {
        {"triggerButton", CommonUsages.triggerButton },
        {"thumbrest", CommonUsages.thumbrest },
        {"primary2DAxisClick", CommonUsages.primary2DAxisClick },
        {"primary2DAxisTouch", CommonUsages.primary2DAxisTouch },
        {"menuButton", CommonUsages.menuButton },
        {"gripButton", CommonUsages.gripButton },
        {"secondaryButton", CommonUsages.secondaryButton },
        {"secondaryTouch", CommonUsages.secondaryTouch },
        {"primaryButton", CommonUsages.primaryButton },
        {"primaryTouch", CommonUsages.primaryTouch },
    };
    
    static readonly Dictionary<string, InputFeatureUsage<float>> availableFloatButtons = new Dictionary<string, InputFeatureUsage<float>>
    {
        {"trigger", CommonUsages.trigger },
        {"grip", CommonUsages.grip}, 
    };

    public enum BoolButtonOption
    {
        triggerButton,
        thumbrest,
        primary2DAxisClick,
        primary2DAxisTouch,
        menuButton,
        gripButton,
        secondaryButton,
        secondaryTouch,
        primaryButton,
        primaryTouch
    };

    public enum FloatButtonOption
    {
        trigger,
        grip
    };
    
    [Serializable]
    public class BoolButton {
        public BoolButtonOption[] button;
        public float weight = 1f;
    }

    [Serializable]
    public class FloatButton {
        public FloatButtonOption button;
        public float weight = 1f;
    }

    [Serializable]
    public class BoolAnimateBone { //Класс для характеристики анимированной кости
        public Transform boneObject; //Transform кости
        public Vector3 zeroRotation; //Нулевое положение кости
        public Vector3 endRotation; //Положение при 100%
        public BoolButton[] buttons; //На что оно будет реагировать 
        [HideInInspector] public bool enabled = true;
    }

    [Serializable]
    public class FloatAnimateBone { //Класс для характеристики анимированной кости
        public Transform boneObject; //Transform кости
        public Vector3 zeroRotation; //Нулевое положение кости
        public Vector3 endRotation; //Положение при 100%
        public FloatButton[] buttons; //На что оно будет реагировать 
        [HideInInspector] public bool enabled = true;
    }

    public class MixAnimateBone { //Класс для характеристики анимированной кости
        public Transform boneObject; //Transform кости
        public Vector3 zeroRotation; //Нулевое положение кости
        public Vector3 endRotation; //Положение при 100%
        public BoolButton[] boolButtons;
        public FloatButton[] floatButtons; //На что оно будет реагировать 
    }

    public XRNode node; //Нода для снятии входа
    [HideInInspector] public List<MixAnimateBone> animatedMixBones = new List<MixAnimateBone>(); 
    public List<BoolAnimateBone> animatedBoolBones; //Массив с костями которые надо анимировать
    public List<FloatAnimateBone> animatedFloatBones;

    private void Start() {
        for (int i = 0; i < animatedBoolBones.Count; i++)
        {
            for (int j = 0; j < animatedFloatBones.Count; j++)
            {
                if (animatedBoolBones[i].boneObject == animatedFloatBones[j].boneObject)
                {
                    MixAnimateBone bone = new MixAnimateBone();
                    bone.boneObject = animatedBoolBones[i].boneObject;
                    bone.boolButtons = animatedBoolBones[i].buttons;
                    bone.floatButtons = animatedFloatBones[j].buttons;
                    bone.zeroRotation = animatedBoolBones[i].zeroRotation;
                    bone.endRotation = animatedBoolBones[i].endRotation;

                    animatedMixBones.Add(bone);

                    animatedBoolBones[i].enabled = false;
                    animatedFloatBones[j].enabled = false;
                }
            }
        }
    }

    private void Update() {
        foreach (var item in animatedMixBones) {
            float val = 0;

            foreach (var button in item.floatButtons)
            {
                availableFloatButtons.TryGetValue(Enum.GetName(typeof(FloatButtonOption), button.button), out InputFeatureUsage<float> inputFeature);
                var output = InputManager.GetFloat(node, inputFeature);

                val += output*button.weight;
            }

            foreach (var button in item.boolButtons)
            {
                bool reg = false;

                foreach (var bl in button.button)
                {
                    availableBoolButtons.TryGetValue(Enum.GetName(typeof(BoolButtonOption), bl), out InputFeatureUsage<bool> inputFeature);
                    var output = InputManager.GetBool(node, inputFeature); //Получение значения

                    if (output) { reg = true; break; }
                }

                
                val += toInt(reg)*button.weight;
            }

            item.boneObject.localRotation = Quaternion.Lerp(Quaternion.Euler(item.zeroRotation), 
                                                        Quaternion.Euler(item.endRotation), 
                                                        (val == null ? 0 : val)); 
        }
        
        foreach (var item in animatedFloatBones)
        {   
            if (!item.enabled) continue;

            float val = 0;

            foreach (var button in item.buttons)
            {
                availableFloatButtons.TryGetValue(Enum.GetName(typeof(FloatButtonOption), button.button), out InputFeatureUsage<float> inputFeature);
                var output = InputManager.GetFloat(node, inputFeature);

                val += output*button.weight;
            }

            item.boneObject.localRotation = Quaternion.Lerp(Quaternion.Euler(item.zeroRotation), 
                                                        Quaternion.Euler(item.endRotation), 
                                                        (val == null ? 0 : val)); 
        }

        foreach (var item in animatedBoolBones)
        {
            if (!item.enabled) continue;

            float val = 0;

            foreach (var button in item.buttons)
            {
                bool reg = false;

                foreach (var bl in button.button)
                {
                    availableBoolButtons.TryGetValue(Enum.GetName(typeof(BoolButtonOption), bl), out InputFeatureUsage<bool> inputFeature);
                    var output = InputManager.GetBool(node, inputFeature); //Получение значения

                    if (output) { reg = true; break; }
                }

                
                val += toInt(reg)*button.weight;
            }
            
            item.boneObject.localRotation = Quaternion.Lerp(Quaternion.Euler(item.zeroRotation), 
                                                        Quaternion.Euler(item.endRotation), 
                                                        val); 
        }
    }

    private int toInt(bool a) { //Функция для перевода из bool в int
        if (a) { return 1; }
        return 0;
    }
}
