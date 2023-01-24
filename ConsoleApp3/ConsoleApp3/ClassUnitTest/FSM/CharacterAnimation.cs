using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp3.ClassUnitTest.FSM
{
    public class CharacterAnimationController
    {
        public Character Owner;

        public void Init(Character owner, FSMBase<Character, CharacterBehaviour> fsm)
        {
            Owner = owner;
            fsm.OnStateChangeEvent += OnStateChange;
        }

        private void OnStateChange(CharacterBehaviour prevBeh, CharacterBehaviour newBeh)
        {

        }

        public void Play(CharacterAnimation ani)
        {

        }
    }
}
