using System;
using System.Collections.Generic;
using System.Text;

namespace Crash.Core.Attributes
{
    /// <summary>
    /// マーカー属性の基底クラス。
    /// </summary>
    public abstract class MarkerAttribute : Attribute
    {

    }

    /// <summary>
    /// 破壊的変更を行うメソッド引数に指定する属性。
    /// </summary>
    [AttributeUsage(AttributeTargets.Parameter)]
    public sealed class ModifyAttribute : MarkerAttribute
    {

    }

    /// <summary>
    /// 試験的導入クラス、メソッド
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public sealed class ExperimentalAttribute : MarkerAttribute
    {
    }
}
