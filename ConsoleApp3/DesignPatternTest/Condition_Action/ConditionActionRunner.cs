using System;
using System.Collections.Generic;
using System.Text;

class ConditionActionRunner
{
    #region ====:: 가장 최상위 추상화 Level 인 조건 & 행동 Interface 정의 ::====
    interface ICondition
    {
        /// <summary> 조건 만족하는가? </summary>
        bool Meet();
    }

    interface IBehaviour
    {
        /// <summary> 행동  </summary>
        void Do();
    }
    #endregion

    #region ====:: Condition Concret Implementation ::====

    /// <summary>
    /// 수 (Number) 2 개를 가지고 비교하여 조건을 체크하는 클래스
    /// 필요한건 
    ///     - 기준 숫자 
    ///     - 비교 방식  
    ///         - 같은지 (Equal)
    ///         - 다른지 (Different)
    ///         - 작은지 (Less)
    ///         - 더 큰지 (Greater)
    /// </summary>
    class NumberCondition : ICondition
    {
        public delegate bool ConditionChecker(int src, int dst);

        public int SrcNumber { get; set; }
        public int DstNumber { get; set; }

        ConditionChecker Checker;

        public NumberCondition(ConditionChecker checker)
        {
            this.Checker = checker;
        }

        public NumberCondition(int src, int dst, ConditionChecker checker)
        {
            this.Checker = checker;
            this.SrcNumber = src;
            this.DstNumber = dst;
        }

        public bool Meet()
        {
            if (Checker == null)
            {
                return false;
            }

            return Checker.Invoke(SrcNumber, DstNumber);
        }
    }
    #endregion

    #region ====:: Behaviour 의 세부 구현 ::====

    class PrintNumberBehaviour : IBehaviour
    {
        public int Number { get; set; }

        public PrintNumberBehaviour(int number) => this.Number = number;

        public void Do()
        {
            Console.WriteLine(this.Number);
        }
    }

    #endregion

    #region ====:: Condition 의 세부 비교 관련 Factory (주로 매번 할당을 막기 위함) ::====

    class ConditionCompareFactory
    {
        #region ==:: Int (정수) 비교자 ::==

        /// <summary> <see cref="NumberCondition.ConditionChecker"/> delegate 랑 Format 을 매칭시킴 (Return,Parameter) </summary>
        public class NumberComparer
        {
            public static bool EqualChecker(int src, int dst) => src == dst;
            public static bool DifferentChecker(int src, int dst) => src != dst;
            public static bool LessChecker(int src, int dst) => src < dst;
            public static bool GreaterChecker(int src, int dst) => src > dst;
        }
        #endregion
    }

    #endregion

    public void RunTest()
    {
        Console.WriteLine("---- 조건 / 행동 패턴 Test (이거 내가 맘대로 만든거임. 정식명칭X) ----");

        List<ICondition> conditionChain = new List<ICondition>();
        List<IBehaviour> behaviourChain = new List<IBehaviour>();

        #region ====:: 숫자 비교 및 출력 ::====
        {
            /// Condition 세팅 
            int srcNumber = 15;

            #region ====:: 조건 (Condition) 세팅 ::====

            /// 같고
            conditionChain.Add(ConditionBuilder.NumberConditionBuilder.Build(srcNumber, 15, ConditionBuilder.NumberConditionBuilder.CompareType.Equal));

            /// 더 클때 
            conditionChain.Add(ConditionBuilder.NumberConditionBuilder.Build(srcNumber, 5, ConditionBuilder.NumberConditionBuilder.CompareType.Greater));

            #endregion

            #region ====:: 행동 (Behaviour) 세팅 ::====

            /// Behaviour 세팅
            behaviourChain.Add(BehaviourBuilder.BuildNumberPrinter(12345));
            behaviourChain.Add(BehaviourBuilder.BuildNumberPrinter(54321));

            #endregion

            RunPair(conditionChain, behaviourChain);
        }
        #endregion
    }

    /// <summary> Condition | Behaviour 을 작동 ㄱㄱ Run! </summary>
    void RunPair(List<ICondition> conditionChain, List<IBehaviour> behaviourChain)
    {
        if (EvaluateCondition(conditionChain))
        {
            RunBehaviours(behaviourChain);
        }
    }

    /// <summary> 조건 체인 체크 </summary>
    bool EvaluateCondition(IEnumerable<ICondition> chain)
    {
        foreach (var condition in chain)
        {
            if (condition.Meet() == false)
            {
                return false;
            }
        }

        return true;
    }

    /// <summary> 행동 체인 실행 </summary> 
    void RunBehaviours(IEnumerable<IBehaviour> behaviours)
    {
        foreach (var beh in behaviours)
        {
            beh.Do();
        }
    }

    void DoBehaviour(IEnumerable<IBehaviour> chain)
    {
        foreach (var behaviour in chain)
        {
            behaviour.Do();
        }
    }

    /// <summary> 조건 빌더 </summary>
    class ConditionBuilder
    {
        /// <summary> 숫자 비교를 Condition Builder </summary>
        public class NumberConditionBuilder
        {
            public enum CompareType
            {
                Equal,
                Different,
                Less,
                Greater,
            }

            static NumberCondition.ConditionChecker ToComparer(CompareType type)
            {
                switch (type)
                {
                    case CompareType.Equal:
                        return ConditionCompareFactory.NumberComparer.EqualChecker;
                    case CompareType.Different:
                        return ConditionCompareFactory.NumberComparer.DifferentChecker;
                    case CompareType.Less:
                        return ConditionCompareFactory.NumberComparer.LessChecker;
                    case CompareType.Greater:
                        return ConditionCompareFactory.NumberComparer.GreaterChecker;
                    default:
                        return ConditionCompareFactory.NumberComparer.EqualChecker;
                }
            }

            public static ICondition Build(int src, int dst, CompareType type)
            {
                var comparer = ToComparer(type);
                return new NumberCondition(src, dst, comparer);
            }
        }
    }

    class BehaviourBuilder
    {
        public static IBehaviour BuildNumberPrinter(int number)
        {
            return new PrintNumberBehaviour(number);
        }
    }
}