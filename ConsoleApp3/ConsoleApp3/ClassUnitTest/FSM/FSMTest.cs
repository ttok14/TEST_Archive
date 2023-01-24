using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp3.ClassUnitTest.FSM
{
    class FSMTest
    {
        public void RunTest()
        {
            #region ====:: FSM 구성 ::======

            GameSystem system = new GameSystem();

            // 캐릭터 생성 
            system.myCharacter = CharacterFactory.Create(Tag.Me, system);
            system.enemyCharacter = CharacterFactory.Create(Tag.Enemy, system);

            system.myCharacter.FSM.SwitchState(CharacterBehaviour.Idle);
            system.enemyCharacter.FSM.SwitchState(CharacterBehaviour.Idle);

            system.myCharacter.FSM.SwitchState(CharacterBehaviour.Attack);
            system.enemyCharacter.FSM.SwitchState(CharacterBehaviour.Attack);

            #endregion
        }
    }
}
