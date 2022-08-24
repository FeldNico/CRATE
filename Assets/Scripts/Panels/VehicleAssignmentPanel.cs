using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class VehicleAssignmentPanel: MonoBehaviour
{
    public VehicleType Type { private set; get; }

    [SerializeField] private TMP_Text _name;
    
    [SerializeField] private Image _image;

    [SerializeField] private TMP_Text _countLabel;

    public void Initialize(VehicleType type, int count)
    {
        name = type.VehicleName;
        Type = type;
        _name.text = type.VehicleName;
        _image.sprite = type.VehicleImage;
        _countLabel.text = count.ToString();
    }
    
}