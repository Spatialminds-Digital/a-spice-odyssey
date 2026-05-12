
using System;
using System.Collections.Generic;
using UnityEngine;

public class OrderService : MonoBehaviour
{
    [SerializeField] private Recipe[] availableRecipes;

    private List<Order> _availableOrders = new List<Order>();

    public Action<Order> OnNewOrderCreated;
    public Action<Order> OnOrderComplete;
    public Action<Order> OnOrderRemoved;
    public Action<Order> OnWrongOrder;

    private Order _tempOrder;

    public void CreateOrder(int complexity)
    {
        _tempOrder = new Order();
        _tempOrder.recipe = availableRecipes[UnityEngine.Random.Range(0, Mathf.Min(complexity, availableRecipes.Length))];
        _availableOrders.Add(_tempOrder);
        OnNewOrderCreated?.Invoke(_tempOrder);
    }

    public void CheckAndRemoveOrder(Recipe recipe)
    {
        for (int i = 0; i < _availableOrders.Count; i++)
        {
            if (_availableOrders[i].recipe == recipe)
            {
                var completedOrder = _availableOrders[i];
                _availableOrders.RemoveAt(i);
                OnOrderComplete?.Invoke(completedOrder);
                return;
            }
        }

        var wrongOrder = new Order { recipe = recipe };
        OnWrongOrder?.Invoke(wrongOrder);
    }

    public void RemoveOrder(Recipe recipe)
    {
        for (int i = 0; i < _availableOrders.Count; i++)
        {
            if (_availableOrders[i].recipe == recipe)
            {
                var removedOrder = _availableOrders[i];
                _availableOrders.RemoveAt(i);
                OnOrderRemoved?.Invoke(removedOrder);
                return;
            }
        }
    }

    public void ClearAllOrders()
    {
        _availableOrders.Clear();
    }
}

public class Order
{
    public Recipe recipe;
}
