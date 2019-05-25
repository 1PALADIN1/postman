using Core.Enum;
using UnityEngine;

namespace Core.Model
{
    public class BaseModel : MonoBehaviour
    {
        [SerializeField]
        protected EntityType _serializeType = EntityType.None;

        public EntityType SerializeType
        {
            get => _serializeType;
        }
    }
}