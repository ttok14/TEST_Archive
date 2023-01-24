using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp3.ClassUnitTest.FSM
{
    #region ====:: 시스템적인 부분 정의 ::====

    public enum Tag
    {
        Me,
        Enemy
    }

    public enum CharacterBehaviour
    {
        None = 0,
        Idle,
        Attack,
        Hit,
        Die
    }

    public enum CharacterAnimation
    {
        None = 0,
        Idle01,
        Idle02,
        Attack,
        Hit,
        PreDie,
        Die
    }

    public class GameSystem
    {
        public Character myCharacter;
        public Character enemyCharacter;
    }

    public class CharacterFactory
    {
        public static Character Create(Tag tag, GameSystem system)
        {
            var newCharacter = new Character();
            newCharacter.Tag = tag;

            newCharacter.FSM.AddState(CharacterBehaviour.Idle, new CharacterFSM_Idle());
            newCharacter.FSM.AddState(CharacterBehaviour.Attack, new CharacterFSM_Attack());
            newCharacter.FSM.AddState(CharacterBehaviour.Hit, new CharacterFSM_Hit());
            newCharacter.FSM.AddState(CharacterBehaviour.Die, new CharacterFSM_Die());

            newCharacter.FSM.Initialize(newCharacter);

            return newCharacter;
        }
    }

    #endregion
}
