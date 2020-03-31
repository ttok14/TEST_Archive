using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp3.ClassUnitTest
{
    public class SortTestClass : IComparable<SortTestClass>
    {
        public int score;

        // IComparable 인터페이스 함수 구현 
        public int CompareTo(SortTestClass dest)
        {
            return CompareByComparison_Static(this, dest);
        }

        public int Compare(SortTestClass t1, SortTestClass t2)
        {
            return CompareByComparison_Static(t1, t2);
        }

        // 이 함수로하면 인스턴스를 만들어서 함수를 넘겨줘야함 . 
        // 참고로 해당 인스턴스의 함수를 넘기는것이기 때문에 
        // 이 함수안에서 해당 인스턴스의 변수같은것들을 그대로 접근가능 . 
        // 응용 가능한 부분임. 
        public int CompareByComparison_NonStatic(SortTestClass t1, SortTestClass t2)
        {
            return CompareByComparison_Static(t1, t2);
        }

        static public int CompareByComparison_Static(SortTestClass t1, SortTestClass t2)
        {
            // 스코어가 큰 순서대로 앞으로 가게끔 정렬 .
            if (t1.score < t2.score)
            {
                return 1;
            }
            else if (t1.score > t2.score)
            {
                return -1;
            }
            else
            {
                return 0;
            }
        }
    }

    // 전용 Comparer 클래스 .
    // Compare 의 스케일이 커지면 이런식으로 전용 클래스로 빼서 
    // 디자인하는거도 나쁘지않은 응용 방법
    public class SortTestClass_Comparer : IComparer<SortTestClass>
    {
        public int Compare(SortTestClass t1, SortTestClass t2)
        {
            return SortTestClass.CompareByComparison_Static(t1, t2);
        }
    }
}
