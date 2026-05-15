using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OrdersUI : MonoBehaviour
{
    [SerializeField] private OrderService orderService;
    [SerializeField] private OrderContentUI orderContentPrefab;
    [SerializeField] private Transform container;

    private Dictionary<Order, OrderContentUI> _orderUIMap = new Dictionary<Order, OrderContentUI>();

    private void OnEnable()
    {
        orderService.OnNewOrderCreated += HandleNewOrder;
        orderService.OnOrderComplete += HandleOrderComplete;
        orderService.OnOrderRemoved += HandleOrderComplete;
    }

    private void OnDisable()
    {
        orderService.OnNewOrderCreated -= HandleNewOrder;
        orderService.OnOrderRemoved -= HandleOrderComplete;
        orderService.OnOrderComplete -= HandleOrderComplete;
    }

    private void HandleNewOrder(Order order)
    {
        var orderUI = Instantiate(orderContentPrefab, container);
        orderUI.UpdateOrderContent(order.recipe);
        _orderUIMap[order] = orderUI;

        
        LayoutRebuilder.ForceRebuildLayoutImmediate(container.GetComponent<RectTransform>());
    }

    private void HandleOrderComplete(Order order)
    {
        if (_orderUIMap.TryGetValue(order, out var orderUI))
        {
            _orderUIMap.Remove(order);
            Destroy(orderUI.gameObject);
        }
    }
}
