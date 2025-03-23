using MarkusSecundus.TinyDialog.Expressions;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using ExpressionEvaluator = MarkusSecundus.TinyDialog.Expressions.ExpressionEvaluator;

public class DamageableConditionalAction : MonoBehaviour
{
    [SerializeField] string Condition;
    [SerializeField] UnityEvent ToInvoke;
    public void InvokeConditionally(Damageable.HealthChangeInfo info)
    {
        var expression = ExpressionParser.Instance.Parse(Condition);
        var ctx = new ExpressionContext();
        ctx.TrySetVariable(nameof(info.Damageable.HP), info.Damageable.HP);
        ctx.TrySetVariable(nameof(info.OriginalHP), info.OriginalHP);

        if ( ExpressionEvaluator.Instance.Evaluate(expression, ctx).BoolValue)
        {
            ToInvoke?.Invoke();
        }
    }
}
