using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp3.ClassUnitTest.FSM
{
    #region ====:: FSM 기본 구조 ::=====

    public class FSMBase<OwnerType, TState>
        where OwnerType : class
        where TState : struct, Enum
    {
        protected OwnerType Owner;

        protected Dictionary<TState, FSMStateBase<OwnerType, TState>> States = new Dictionary<TState, FSMStateBase<OwnerType, TState>>();
        public TState CurrentState { get; protected set; } = default;

        public event Action<TState, TState> OnStateChangeEvent;

        public virtual void Initialize(OwnerType owner)
        {
            this.Owner = owner;
        }

        public virtual bool AddState(TState type, FSMStateBase<OwnerType, TState> stateBase)
        {
            if (States.ContainsKey(type))
            {
                return false;
            }

            States.Add(type, stateBase);

            stateBase.Initialize(Owner);

            return true;
        }

        public bool CanSwitch(TState switchTo)
        {
            if (States.ContainsKey(switchTo) == false)
            {
                return false;
            }

            if (States[switchTo].CanEnter() == false)
            {
                return false;
            }

            return true;
        }

        /// <summary> State 전환 </summary>
        /// <param name="newState"></param>
        public bool SwitchState(TState newState)
        {
            if (CurrentState.Equals(newState))
            {
                return false;
            }

            FSMStateBase<OwnerType, TState> target;

            if (States.TryGetValue(newState, out target))
            {
                if (target.CanEnter() == false)
                {
                    return false;
                }
            }

            var prevState = CurrentState;
            if (States.TryGetValue(prevState, out target))
            {
                target.OnExit();
            }

            CurrentState = newState;

            if (States.TryGetValue(newState, out target))
            {
                target.OnEnter();
            }

            OnStateChangeEvent?.Invoke(prevState, newState);

            return true;
        }
    }

    public abstract class FSMStateBase<Owner, TState>
        where Owner : class
        where TState : struct, Enum
    {
        protected Owner owner;

        public virtual void Initialize(Owner owner)
        {
            this.owner = owner;
        }

        public virtual void OnEnter()
        {
            Console.WriteLine($"FSM State OnEnter : {this.GetType()}");
        }

        public virtual bool CanEnter()
        {
            return true;
        }

        public virtual void OnExit()
        {
            Console.WriteLine($"FSM State OnExit : {this.GetType()}");
        }
    }
    #endregion
}
