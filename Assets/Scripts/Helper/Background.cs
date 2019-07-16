using UnityEngine;

namespace Core.Helper
{
    public class Background : MonoBehaviour
    {
        private Camera _mainCamera;
        private SpriteRenderer _spriteRenderer;
        private Sprite _backgroundImage;

        private void Start()
        {
            _mainCamera = Camera.main;
            _spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
            _backgroundImage = _spriteRenderer.sprite;

            //подгоняем фон под размер камеры
            float cameraHeight = _mainCamera.orthographicSize * 2;
            Vector2 cameraSize = new Vector2(Camera.main.aspect * cameraHeight, cameraHeight);
            Vector2 spriteSize = _backgroundImage.bounds.size;

            transform.localScale = new Vector3(cameraSize.x / spriteSize.x,
                cameraSize.y / spriteSize.y, transform.localScale.z);
        }
    }
}