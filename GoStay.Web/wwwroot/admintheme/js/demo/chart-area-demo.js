// Set new default font family and font color to mimic Bootstrap's default styling
// Area Chart Example
var ctx = document.getElementById("myAreaChart");
var _labels = [];
var _data = [];
_labels = ctx.dataset.key.split(',');
_data = ctx.dataset.value.split(',');

var myLineChart = new Chart(ctx, {
  type: 'line',
  data: {
      labels: _labels,
    datasets: [{
        label: "Rooms",
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

var RoomByDay = document.getElementById("myAreaChartRoomByDay");

_labels = RoomByDay.dataset.key.split(',');
_data = RoomByDay.dataset.value.split(',');

var myLineChart = new Chart(RoomByDay, {
    type: 'line',
    data: {
        labels: _labels,
        datasets: [{
            label: "Rooms",
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
