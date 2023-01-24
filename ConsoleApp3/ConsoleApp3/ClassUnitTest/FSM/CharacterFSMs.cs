using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ConsoleApp3.ClassUnitTest.FSM;

namespace ConsoleApp3.ClassUnitTest.FSM
{
    class CharacterFSM_Idle : FSMStateBase<Character, CharacterBehaviour>
    {
        public override void OnEnter()
        {
            base.OnEnter();

            owner.AniController.Play(CharacterAnimation.Idle01);
        }

        public override void OnExit()
        {
            base.OnExit();
        }
    }

    class CharacterFSM_Attack : FSMStateBase<Character, CharacterBehaviour>
    {
        public override void OnEnter()
        {
            base.OnEnter();
        }

        public override void OnExit()
        {
            base.OnExit();
        }
    }

    class CharacterFSM_Hit : FSMStateBase<Character, CharacterBehaviour>
    {
        public override void OnEnter()
        {
            base.OnEnter();
        }

        public override void OnExit()
        {
            base.OnExit();
        }
    }

    class CharacterFSM_Die : FSMStateBase<Character, CharacterBehaviour>
    {
        public override void OnEnter()
        {
            base.OnEnter();
        }

        public override void OnExit()
        {
            base.OnExit();
        }
    }
}
