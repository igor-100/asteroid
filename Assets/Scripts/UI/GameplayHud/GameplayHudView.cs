using TMPro;
using UI;
using UnityEngine;
using UnityEngine.Serialization;
namespace Asteroid.UI.GameplayHud
{
    public class GameplayHudView : BaseView, IGameplayHudView
    {
        [SerializeField] private TextMeshProUGUI coordinates;
        [SerializeField] private TextMeshProUGUI rotation;
        [SerializeField] private TextMeshProUGUI speed;
        [SerializeField] private TextMeshProUGUI laserChargesCount;
        [FormerlySerializedAs("laserReloatTime")]
        [SerializeField] private TextMeshProUGUI laserReloadTime;
        
        public void SetCoordinates(string text) => coordinates.text = text;
        public void SetRotation(string text) => rotation.text = text;
        public void SetSpeed(string text) => speed.text = text;
        public void SetLaserChargesCount(string text) => laserChargesCount.text = text;
        public void SetLaserReloadTime(string text) => laserReloadTime.text = text;
    }
}
