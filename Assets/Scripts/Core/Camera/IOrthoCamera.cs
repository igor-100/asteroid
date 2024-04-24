namespace Core.Camera
{
    public interface IOrthoCamera
    {
        UnityEngine.Camera MainCamera { get; }
        void SetSize(float size);
    }
}
