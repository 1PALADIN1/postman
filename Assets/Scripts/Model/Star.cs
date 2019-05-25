using UnityEngine;

namespace Core.Model
{
    public class Star : BaseModel, ICollectable
    {
        public delegate void OnCollect(MailBox box, Star star);

        private OnCollect OnCollectMethod;

        private void OnTriggerEnter2D(Collider2D collision)
        {
            MailBox box = collision.gameObject.GetComponent<MailBox>();
            if (box != null)
            {
                OnCollectMethod?.Invoke(box, this);
                Destroy(gameObject);
            }
        }

        public void SetCollect(OnCollect collectMethod)
        {
            OnCollectMethod = collectMethod;
        }
    }
}
