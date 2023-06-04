using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.XR;
public class RaySelect : MonoBehaviour
{
    [HideInInspector] public Movement mov; //Скрипт передвижения (для отслеживания луча)
    private RayButton lastButton; //Последняя выбраная кнопка
    private GameObject lastObject; //Последний выбранный объект

    private void Start() {
        mov = GetComponent<Movement>();
    }

    private void Update() {
        if (mov.collided) { //Если луч с чем-то столкнулся
            mov.ray.collider.gameObject.TryGetComponent<RayButton>(out RayButton button); //Пробуем получить у объекта скрипт RayButton
            lastObject = mov.ray.collider.gameObject;
            
            if (button) { //Если получили скрипт
                if (button != lastButton && lastButton) {
                    lastButton.ExitHover.Invoke(); //Вызываем у предыдущей кнопки выход из наведения
                    lastButton = button;
                    button.EnterHover.Invoke(); //Вызываем у текущей кнопки вход в наведение
                } else if (!lastButton) {
                    lastButton = button;
                    button.EnterHover.Invoke();
                }
            } else {
                if (lastButton) {
                    lastButton.ExitHover.Invoke();
                    lastButton = null;
                }
            }
        } else if (lastObject && lastObject == lastButton.gameObject && !mov.enable) { //Если последний объект равен последней кнопке и луч не включен
            lastButton.Selected.Invoke(); //Отправляем выбор этой кнопки
            lastButton.ExitHover.Invoke(); 
            lastButton = null;
        } else if (mov.enable) { //Если он ни с чем не столкнулся, но при этом включен
            if (lastButton) {
                lastButton.ExitHover.Invoke();
                lastButton = null;
            }
        }
    }
}
