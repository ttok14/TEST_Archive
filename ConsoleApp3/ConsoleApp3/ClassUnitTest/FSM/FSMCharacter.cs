using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp3.ClassUnitTest.FSM
{
    public class Character
    {
        public Tag Tag;
        public CharacterBehaviour CurrentBehaviour;
        public FSMBase<Character, CharacterBehaviour> FSM = new FSMBase<Character, CharacterBehaviour>();
        public CharacterAnimation CurrentAni;
        public CharacterAnimationController AniController = new CharacterAnimationController();

        public void Initialize()
        {
            AniController.Init(this, FSM);
        }

        public bool ChangeBehaviour(CharacterBehaviour newBeh)
        {
            return FSM.SwitchState(newBeh);
        }

        public void ChangeAnimation(CharacterAnimation newAni)
        {

        }
    }
}
