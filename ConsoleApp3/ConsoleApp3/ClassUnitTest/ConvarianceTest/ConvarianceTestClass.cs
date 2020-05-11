using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/*
 * c# 에서 공변성/반공변성은 제너릭 타입의 형변환을 할수있게 해주는데 
 * 그 용도에 따라 in, out 으로 나눠놨고 , 오류 발생여지에 있는 부분을 막기위해 제너릭 타입의 
 * 부모 -> 자식 or 자식 -> 부모 형변환에 대해서  가능/불가능 기준을 나누어놓았음 . 

 * in, out 키워드 
 * 
 * in : 입력 목적으로 쓸 타입앞에 붙힘 
 *      입력이라 한다면 , = 로 값을 넣어주거나 하는 것들임. 즉 , 함수의 인자로 받거나 Set 접근자 또는 델리게이트 일부위치 (Action,Func 를보면알수있지) 에 쓸수있음 . 
 *      반환형으로는 못씀 . in 은 자식 = 부모( 즉 부모 -> 자식, 부모를 자식으로 형변환 ) 로 형변환 가능한데, 
 *      왜그럴까 ? 생각해보면알수잇슴 
 *      
 *      일케 생각해보자 . 
 *      
 * out : 출력 목적으로 쓸 타입앞에 붙힘
 *      출력은 , 값을 넣는게 아니라 이미 있는 값을 사용하기위함임. 
 * 
 * 
 * */
namespace ConsoleApp3.ClassUnitTest.ConvarianceTest
{
    public delegate T MyAction<T>();

    public class BaseClass { }
    public class DerClass01 : BaseClass { }
    public class DerClass02 : BaseClass { }

    public interface Interface_Normal<T> { }
    public interface Interface_Out<out T> { }
    public interface Interface_In<in T> { } // in 이기땜에 반환형으로는 못씀 . 

    public class InterfaceNormalClass<T> : Interface_Normal<T> { }
    public class InterfaceOutClass<T> : Interface_Out<T> { }
    public class InterfaceInClass<T> : Interface_In<T> { }

    public class GenericClass<T> { }

    public class ConvarianceTestClass01
    {
        public void TestNormal()
        {
            Interface_Normal<BaseClass> nb = null;
            Interface_Normal<DerClass01> nd = null;

            // in,out 키워드없이 제너릭 타입 캐스팅 불가능 
            // nb = nd;
            // nd = nb;
            
            // IEnumerable,Func 랑 같은 out T 
            Interface_Out<BaseClass> ob = null;
            Interface_Out<DerClass01> od = null;

            ob = od; // out 일때 base 는 derive 를 가리킬수있음
            // od = ob;

            // Action 이랑 같은 in T 
            Interface_In<BaseClass> ib = null;
            Interface_In<DerClass01> id = null;

            id = ib;
            // ib = id;
        }

        public void TestNormalCasting()
        {
            // 당연히 성립 
            BaseClass bc = new BaseClass();
            // 캐스팅 성공 
            bc = new DerClass01();
            // 캐스팅 성공 
            bc = new DerClass02();

            // base 배열 선언
            BaseClass[] bca = null;
            // baseClass 의 실제 인스턴스 배열 할당 
            bca = new BaseClass[5];
            // 자식들을 넣어줌 . 문제 X 
            bca[0] = new DerClass01();
            bca[0] = new DerClass02();

            // Der01 배열 할당. 당연됨 . 자식이니까 
            bca = new DerClass01[5];
            // 당연히됨 
            bca[0] = new DerClass01();
            try
            {
                // Exception . Der01 를 닭, Der02 를 오리라칠때 , '오리는 닭짓을 할수없기땜에' 안됨. 
                bca[0] = new DerClass02();
            }
            catch (Exception exp)
            {
                Console.WriteLine("Exception ! 오리는 닭이 될수 없다");
            }

            try
            {
                bca[0] = new BaseClass();
            }
            catch (Exception exp)
            {
                Console.WriteLine("Exception ! 선언은 Base 로 됐지만 , 실제 인스턴스는 자식의 배열임 , 그리고 그 자식의 배열 요소에 Base 인스턴스 할당 실패 !");
            }
        }

        // IEnumerable 은 out , IComparable 은 in 임 . 
        public void TestConvariance()
        {
            BaseClass bc = new BaseClass();
            DerClass01 der = new DerClass01();
            bc = der;
            der = bc as DerClass01;

            // 클래스는 공변성 못씀 public class TClass<out T> { }

            // ********* out *********
            // 캐스팅 잘됨 . 
            // 부모에 자식을 할당 ( 부모 = 자식 성공) 
            Interface_Out<BaseClass> sout = new InterfaceOutClass<DerClass01>();
            // 캐스팅 안됨 . 
            // 자식에 부모를 할당 ( 자식 = 부모 실패 ) 
            // Interface_Out<DerClass01> sout02 = new InterfaceOutClass<BaseClass>();

            // ********** in ***********
            // 캐스팅잘됨.
            // 자식에 부모를 할당 ( 자식 = 부모 성공 ) 
            Interface_In<DerClass01> sin = new InterfaceInClass<BaseClass>();
            // 캐스팅 안됨. 
            // 부모에 자식을 할당 ( 부모 = 자식 실패 ) 
            // Interface_In<BaseClass> sin02 = new InterfaceInClass<DerClass01>();
        }

        public void TestDelegate()
        {
            MyAction<BaseClass> b = DelegateFunc01;
        }

        public DerClass01 DelegateFunc01()
        {
            return null;
        }

        public interface inter<T>
        {
            void Put(T t);
        }

        public class PC : IComparable<PC>
        {
            public int CompareTo(PC other)
            {
                throw new NotImplementedException();
            }
        }

        public class PC2 : IComparable<PC2>
        {
            public int CompareTo(PC2 other)
            {
                throw new NotImplementedException();
            }
        }
        
    }
}
