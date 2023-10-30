// Set new default font family and font color to mimic Bootstrap's default styling
// Area Chart Example
var Order_GetOrderByMonth = document.getElementById("myOrderChar_GetOrderByMonth");

_labels = Order_GetOrderByMonth.dataset.key.split(',');
_data = Order_GetOrderByMonth.dataset.value.split(',');

var OrderChart = new Chart(Order_GetOrderByMonth, {
    type: 'line',
    data: {
        labels: _labels,
        datasets: [{
            label: "Get Orders by Month",
            lineTension: 0.3,
            backgroundColor: "rgba(78, 115, 223, 0.05)",
            borderColor: "rgba(78, 115, 223, 1)",
            pointRadius: 3,
            pointBackgroundColor: "rgba(78, 115, 223, 1)",
            pointBorderColor: "rgba(78, 115, 223, 1)",
            pointHoverRadius: 3,
            pointHoverBackgroundColor: "rgba(78, 115, 223, 1)",
            pointHoverBorderColor: "rgba(78, 115, 223, 1)",
            pointHitRadius: 10,
            pointBorderWidth: 2,
            data: _data,
        }],
    },
    options: _options
});


var Order_GetOrderTotalMoneyByMonth = document.getElementById("myOrderChar_GetOrderTotalMoneyByMonth");

_labels = Order_GetOrderTotalMoneyByMonth.dataset.key.split(',');
_data = Order_GetOrderTotalMoneyByMonth.dataset.value.split(',');

var OrderChart = new Chart(Order_GetOrderTotalMoneyByMonth, {
    type: 'line',
    data: {
        labels: _labels,
        datasets: [{
            label: "Get Order Total Money By Month",
            lineTension: 0.3,
            backgroundColor: "rgba(78, 115, 223, 0.05)",
            borderColor: "rgba(78, 115, 223, 1)",
            pointRadius: 3,
            pointBackgroundColor: "rgba(78, 115, 223, 1)",
            pointBorderColor: "rgba(78, 115, 223, 1)",
            pointHoverRadius: 3,
            pointHoverBackgroundColor: "rgba(78, 115, 223, 1)",
            pointHoverBorderColor: "rgba(78, 115, 223, 1)",
            pointHitRadius: 10,
            pointBorderWidth: 2,
            data: _data,
        }],
    },
    options: _options
});

var Order_GetOrderRoomByMonth = document.getElementById("myOrderChar_GetOrderRoomByMonth");

_labels = Order_GetOrderRoomByMonth.dataset.key.split(',');
_data = Order_GetOrderRoomByMonth.dataset.value.split(',');

var OrderChart = new Chart(Order_GetOrderRoomByMonth, {
    type: 'line',
    data: {
        labels: _labels,
        datasets: [{
            label: "Get OrderRoom By Month",
            lineTension: 0.3,
            backgroundColor: "rgba(78, 115, 223, 0.05)",
            borderColor: "rgba(78, 115, 223, 1)",
            pointRadius: 3,
            pointBackgroundColor: "rgba(78, 115, 223, 1)",
            pointBorderColor: "rgba(78, 115, 223, 1)",
            pointHoverRadius: 3,
            pointHoverBackgroundColor: "rgba(78, 115, 223, 1)",
            pointHoverBorderColor: "rgba(78, 115, 223, 1)",
            pointHitRadius: 10,
            pointBorderWidth: 2,
            data: _data,
        }],
    },
    options: _options
});
