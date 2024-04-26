using System;
using UI;
using UnityEngine;
namespace Asteroid.UI.GameplayHud
{
    public class GameplayHud : MonoBehaviour, IGameplayHud
    {
        private IGameplayHudView view;

        public void Init(IViewFactory viewFactory)
        {
            view = viewFactory.CreateGameplayHudView();
        }
        
        public void SetCoordinates(Vector2 coordinates)
        {
            view.SetCoordinates($"({FloatToString(coordinates.x, 1)}, {FloatToString(coordinates.y, 1)})");
        }
        
        public void SetRotation(float rotationAngle)
        {
            view.SetRotation($"{FloatToString(rotationAngle, 0)}");
        }
        
        public void SetSpeed(float speed)
        {
            // multiplied cause it looks better than 0.03
            view.SetSpeed($"{FloatToString(speed * 10000, 0)}");
        }
        
        public void SetLaserChargesCount(int count)
        {
            view.SetLaserChargesCount(count.ToString());
        }
        
        public void SetLaserReloadTime(float time)
        {
            view.SetLaserReloadTime($"{FloatToString(time)}");
        }

        private static string FloatToString(float value, int digits = 2)
        {
            return $"{(float)Math.Round(value, digits)}";
        }
        
        public void Show() => view.Show();

        public void Hide() => view.Hide();
    }
}
