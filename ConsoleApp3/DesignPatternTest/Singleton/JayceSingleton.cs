using System;
using System.Collections.Generic;
using System.Text;

namespace DesignPatternTest.Singleton
{
    /// <summary> Singleton 구현하기 </summary>
    ///     => 핵심
    ///             - 생성자는 외부에서 new() 로 할당하는 걸 막기 위해 private 로 설정 
    ///             - Singleton 은 내부에 Data 를 많이 들고 있는걸 지양해야함 . 
    ///                     - Global 한 접근 수준을 고려했을때 Race condition 의 여지가 많고 
    ///                         의도하지 않은 동작이 일어났을때 어디서 바꿧는지를 트래킹하기가 쉽지가 않음.
    ///             - Dependency 관리 유의 . 이것도 역시 global 한 접근 수준때문에 발생하기 쉬움 
    ///             - Singleton 은 어느 상황에서나 단 하나의 인스턴스만 있다라고 '가정' 하고 있기에 그 규칙이 깨져서는 안됨
    ///                     즉 유의해야함 . 
    ///             - 
    public class JayceSingleton
    {
        private static JayceSingleton instance;

        /// <summary> 생성자는 외부에서 생성하는 걸 막기 위해 private 레벨 설정 </summary>
        private JayceSingleton() { }

        /// <summary> 외부에서 인스턴스를 얻을수 있게끔 Get 함수 구현 </summary>
        ///     => public 
        public static JayceSingleton GetInstance()
        {
            if (instance == null)
            {
                instance = new JayceSingleton();
            }

            return instance;
        }
    }
}
