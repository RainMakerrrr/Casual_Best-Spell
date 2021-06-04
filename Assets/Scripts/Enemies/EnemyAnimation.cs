using UnityEngine;

namespace Enemies
{
    public class EnemyAnimation : MonoBehaviour
    {
        private static readonly int IsGetHit = Animator.StringToHash("isGetHit");
        private static readonly int IsAttack = Animator.StringToHash("isAttack");

        private Animator _animator;

        private void OnEnable()
        {
            _animator = GetComponent<Animator>();
        }

        public void PlayGetHitAnimation() => _animator.SetTrigger(IsGetHit);
        public void PlayAttackAnimation() => _animator.SetTrigger(IsAttack);
    }
}