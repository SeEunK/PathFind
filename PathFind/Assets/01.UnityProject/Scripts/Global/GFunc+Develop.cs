using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static partial class GFunc
{
    #region Print log func
    [System.Diagnostics.Conditional("DEBUG_MODE")]
    public static void Log(object message)
    {
#if DEBUG_MODE
        Debug.Log(message);
#endif      // DEBUG_MODE
    }       // Log()

    [System.Diagnostics.Conditional("DEBUG_MODE")]
    public static void Log(object message, UnityEngine.Object context)
    {
#if DEBUG_MODE
        Debug.Log(message, context);
#endif      // DEBUG_MODE
    }

    [System.Diagnostics.Conditional("DEBUG_MODE")]
    public static void LogWarning(object message)
    {
#if DEBUG_MODE
        Debug.LogWarning(message);
#endif      // DEBUG_MODE
    }       // Log()
    #endregion      // Print log func

    #region Assert for debug
    [System.Diagnostics.Conditional("DEBUG_MODE")]
    public static void Assert(bool condition)
    {
#if DEBUG_MODE
        Debug.Assert(condition);
#endif      // DEBUG_MODE
    }       // Assert()

    [System.Diagnostics.Conditional("DEBUG_MODE")]
    public static void Assert(bool condition, object message)
    {
#if DEBUG_MODE
        Debug.Assert(condition, message);
#endif      // DEBUG_MODE
    }       // Assert()
    #endregion      // Assert for debug

    #region Vaild Func

    // 오브젝트의 유효성을 검사하는 함수
    public static bool IsValid(this GameObject obj_)
    {
        bool isInValid = (obj_ == null || obj_ == default);
        return !isInValid;
    }


    //리스트의 유효성을 검사.
    public static bool IsValid<T>(this List<T> list_)
    {
        bool isInValid = (list_ == null || list_ == default || list_.Count <1);
        return !isInValid;
    }

    //리스트의 유효성을 검사.
    public static bool IsValid<T>(this List<T> list_, int index_)
    {
        bool isInValid = (list_.IsValid() == false || index_ < 0 || list_.Count <= index_);
        return !isInValid;
    }



    // 컴포넌트의 유효성을 검사하는 함수
    public static bool IsValid<T>(this T component_) where T : Component
    {
        Component convert_ = (Component)(component_ as Component);
        bool isInvalid = convert_ == null || convert_ == default;
        return !isInvalid;
    }
    #endregion      // Vaild Func



    // 리스트를 생성해서 리턴하는 함수
    /**
 * @param int listLength : 생성할 리스트의 길디
 * @param int startIndex : 리스트에 연속으로 할당할 인덱스의 시작 숫자
 * @return List<T> list_ : 연속된 숫자로 생성한 리스트
 * 
 */
    public static List<int> CreateList(int listLength, int startIndex = 0)
    {
        List<int> list = new List<int>();
        for (int i = 0; i < listLength; i++)
        {
            list.Add(startIndex + i);
        }
        return list;
    }

    //두 변수의 값을 Swap 하는 함수
    public static void Swap<T>(ref T sourValue , ref T destValue)
    {
        T tempValue = sourValue;
        sourValue = destValue;
        destValue = tempValue;
    }

    //두 변수의 값을 Swap 하는 함수 (튜플? 스타일...)
    public static void Swap<T>((T sourValue, T destValue) swapvalue)
    {
        (T sourValue, T destValue) = (swapvalue.destValue, swapvalue.sourValue);
    }
}
