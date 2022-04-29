using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class Fuelstation : MonoBehaviour
{

   [SerializeField]
   private Button _fuelButton;

   public Button FuelButton => _fuelButton;

   [SerializeField]
   private Slider _fuelSlider;

   public Slider FuelSlider => _fuelSlider;

   [SerializeField,Range(0,1f)]
   private float _fuel;

   public float Fuel
   {
      set
      {
         value = Math.Clamp(value, 0, 1f);
         _fuel = value;
         _fuelSlider.value = value;
      }
      get => _fuel;
   }

   
   
   private void Awake()
   {
      _fuelSlider.value = Fuel;
      FuelButton.onClick.AddListener(() =>
      {
         Fuel += 0.2f;
         FuelButton.interactable = false;
         Task.Run(async () =>
         {
            await Task.Delay(1000 * 5);
            FuelButton.interactable = true;
         });
      });
   }
}
